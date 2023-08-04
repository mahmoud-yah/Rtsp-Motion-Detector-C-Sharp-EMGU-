using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;

namespace Rtsp_Motion_Detector
{
    internal class MotionDetector
    {
        readonly String streamUrl;
        readonly int sensitivity;
        bool isDetecting = false;
        Thread? detectorThread;
        public MotionDetector(string streamUrl, int senitivity)
        {
            this.streamUrl = streamUrl;
            if (senitivity <= 0 || senitivity > 10)
            {
                this.sensitivity = 5;
            }
            else
            {
                this.sensitivity = senitivity;
            }
        }

        public void Start(bool showCamera = false)
        {
            detectorThread = new Thread(() => StartDetector(showCamera: showCamera));
            detectorThread.Start();
        }

        private void StartDetector(bool showCamera = false)
        {
            isDetecting = true;

            try
            {
                Mat frame = new(), gray = new(), staticFrame = new(),
                frameDelta = new(), thresh = new();
                VectorOfVectorOfPoint cnts = new();
                Size kSize = new(21, 21);
                //string defaultMjpgUrl = "http://158.58.130.148/mjpg/video.mjpg";
                //string defaultRtspUrl = "rtsp://admin:admin123@user10.ddns.net/user=admin&password=admin123&channel=16&stream=0.sdp";
                int framesCount = 0;
                bool motionDetected = false;

                //passing 0 uses the local webcam instead of a network stream
                VideoCapture capture = new(streamUrl);
                
                //initializing the first frame
                capture.Read(frame);

                //convert the frame to grayscale
                CvInvoke.CvtColor(frame, staticFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                CvInvoke.GaussianBlur(staticFrame, staticFrame, kSize, 0);

                while (capture.Read(frame))
                {
                    //checking if the detector is still running
                    if (isDetecting == false) { break; }

                    framesCount++;

                    //convert the frame to grayscale
                    CvInvoke.CvtColor(frame, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    CvInvoke.GaussianBlur(gray, gray, kSize, 0);

                    if (framesCount == 10)
                    {
                        //reinitializing the static frame every 10 frames
                        CvInvoke.CvtColor(frame, staticFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                        CvInvoke.GaussianBlur(staticFrame, staticFrame, kSize, 0);
                        framesCount = 0;
                    }

                    //compute the difference between the static frame and the current frame
                    CvInvoke.AbsDiff(staticFrame, gray, frameDelta);
                    CvInvoke.Threshold(frameDelta, thresh, 25, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                    //CvInvoke.Dilate(thresh, thresh, new Mat(), new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(255, 255, 255));

                    CvInvoke.FindContours(thresh, cnts, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                    for (int i = 0; i < cnts.Size; i++)
                    {
                        if (CvInvoke.ContourArea(cnts[i]) < sensitivity * 100)
                        {
                            continue;
                        }
                        motionDetected = true;
                    }

                    if (motionDetected)
                    {
                        Console.Write("MotionDetected");
                        motionDetected = false;
                    }
                    if (showCamera)
                    {
                        CvInvoke.NamedWindow("Camera", WindowFlags.KeepRatio);
                        CvInvoke.Imshow("Camera", frame);
                    }

                    if (CvInvoke.WaitKey(1) == 27) { break; }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        public void Stop()
        {
            isDetecting = false;

        }

       
    }
}
