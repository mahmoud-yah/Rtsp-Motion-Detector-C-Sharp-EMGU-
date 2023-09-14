using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace Rtsp_Motion_Detector
{
    internal class MotionDetector
    {

        static int counter = 0;
        int cameraID;
        public readonly string StreamUrl;
        public readonly int Sensitivity;
        public bool IsDetecting { get; private set; } = false;
        public event EventHandler? MotionDetected;

        private Thread? detectorThread;

        public MotionDetector(string streamUrl, int senitivity = 5)
        {
            counter++;
            cameraID = counter;
            StreamUrl = streamUrl;
            if (senitivity <= 0 || senitivity > 10)
            {
                Sensitivity = 5;
            }
            else
            {
                Sensitivity = senitivity;
            }
        }

        public void Start(bool showCamera = false)
        {
            detectorThread = new Thread(() => StartDetector(showCamera: showCamera)) { IsBackground = true };
            detectorThread.Start();
        }

        private void StartDetector(bool showCamera = false)
        {
            IsDetecting = true;

            try
            {
                Mat frame = new(),
                    gray = new(),
                    staticFrame = new(),
                    frameDelta = new(),
                    thresh = new();
                VectorOfVectorOfPoint cnts = new();
                Size kSize = new(21, 21);
                int framesCount = 0;
                bool motionDetected = false;

                //passing 0 uses the local webcam instead of a network stream
                VideoCapture capture = new(StreamUrl);

                if (capture.IsOpened)
                {

                    //set video size to 512x288 to process faster
                    capture.Set(CapProp.FrameWidth, 512);
                    capture.Set(CapProp.FrameHeight, 288);

                    //initializing the first frame
                    capture.Read(frame);

                    //resizing frames to optimize the performance
                    CvInvoke.Resize(frame, frame, new Size(), 0.5, 0.5);


                    //convert the frame to grayscale
                    CvInvoke.CvtColor(frame, staticFrame, ColorConversion.Bgr2Gray);
                    CvInvoke.GaussianBlur(staticFrame, staticFrame, kSize, 0);

                    while (capture.Read(frame))
                    {
                        //resizing frames to lower the resources
                        CvInvoke.Resize(frame, frame, new Size(), 0.5, 0.5);

                        //checking if the detector is still running
                        if (!IsDetecting)
                            break;

                        //convert the frame to grayscale
                        CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);
                        CvInvoke.GaussianBlur(gray, gray, kSize, 0);


                        if (framesCount == 10)
                        {
                            //reinitializing the static frame every 10 frames
                            CvInvoke.CvtColor(frame, staticFrame, ColorConversion.Bgr2Gray);
                            CvInvoke.GaussianBlur(staticFrame, staticFrame, kSize, 0);
                            framesCount = 0;
                        }

                        //compute the difference between the static frame and the current frame
                        CvInvoke.AbsDiff(staticFrame, gray, frameDelta);
                        CvInvoke.Threshold(frameDelta, thresh, 25, 255, ThresholdType.Binary);

                        //CvInvoke.Dilate(thresh, thresh, new Mat(), new Point(-1, -1), 2, BorderType.Constant, new MCvScalar(255, 255, 255));

                        CvInvoke.FindContours(thresh, cnts, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                        for (int i = 0; i < cnts.Size; i++)
                        {
                            if (CvInvoke.ContourArea(cnts[i]) < (Sensitivity * 100))
                                continue;
                            motionDetected = true;
                            break;
                        }

                        if (motionDetected)
                        {
                            // Raise the motion detected event
                            //OnMotionDetected(EventArgs.Empty);
                            Console.WriteLine("Motion Detected from camera" + cameraID.ToString());
                            motionDetected = false;
                        }
                        if (showCamera)
                        {
                            //CvInvoke.NamedWindow("Camera"+counter.ToString(), WindowFlags.KeepRatio);
                            CvInvoke.Imshow("Camera " + cameraID.ToString(), frame);
                        }

                        framesCount++;

                        if (CvInvoke.WaitKey(1) == 27)
                            break;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("...Error... "+ex.Message); 
                Console.WriteLine("Restarting detector.."); 
                Start(); 
            }
        }

        // Helper method to raise the MotionDetected event
        private void OnMotionDetected(EventArgs e)
        {
            MotionDetected?.Invoke(this, e);
        }

        public void Stop() => IsDetecting = false;
    }
}
