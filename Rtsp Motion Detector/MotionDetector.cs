using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;

namespace Rtsp_Motion_Detector
{
    internal class MotionDetector
    {
        public readonly string StreamUrl;
        public readonly int Sensitivity;
        public bool IsDetecting { get; private set; } = false;
        public event EventHandler? MotionDetected;

        private Task? detectorTask;

        public MotionDetector(string streamUrl, int sensitivity = 5)
        {
            StreamUrl = streamUrl;
            Sensitivity = Math.Max(5, Math.Min(10, sensitivity));
        }

        public void Start(bool showCamera = false)
        {
            detectorTask = Task.Run(() => StartDetector(showCamera: showCamera));
        }

        private async Task StartDetector(bool showCamera = false)
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

                //initializing the first frame
                capture.Read(frame);

                //convert the frame to grayscale
                CvInvoke.CvtColor(frame, staticFrame, ColorConversion.Bgr2Gray);
                CvInvoke.GaussianBlur(staticFrame, staticFrame, kSize, 0);

                while (capture.Read(frame))
                {
                    //checking if the detector is still running
                    if (!IsDetecting)
                        break;

                    framesCount++;

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
                        OnMotionDetected(EventArgs.Empty);
                        motionDetected = false;
                    }
                    if (showCamera)
                    {
                        CvInvoke.NamedWindow("Camera", WindowFlags.KeepRatio);
                        CvInvoke.Imshow("Camera", frame);
                    }

                    if (CvInvoke.WaitKey(1) == 27)
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        // Helper method to raise the MotionDetected event
        private void OnMotionDetected(EventArgs e)
        {
            MotionDetected?.Invoke(this, e);
        }

        public async Task StopAsync()
        {
            IsDetecting = false;
            if (detectorTask != null)
                await detectorTask;
        }
    }
}
