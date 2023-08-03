using System;
using System.Drawing;
using System.Numerics;
using System.Threading;
using Emgu;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

internal class Program
{
    private static void Main(string[] args)
    {
        Mat frame = new(), gray = new(), staticFrame = new(),
            frameDelta = new(), inputFrame = new(),
            thresh = new(), tempFrame = new(), hierarchy = new();
        VectorOfVectorOfPoint cnts=new();
        Size kSize = new(21,21);
        Point kPoint = new(-1,-1);

        VideoCapture capture = new(0);
        capture.Set(CapProp.Fps, 0);
        
        capture.Start();

        int framesCount = 0;
        //string defaultRtspUrl = "rtsp://admin:admin123@user10.ddns.net/user=admin&password=admin123&channel=16&stream=0.sdp";
        //string defaultMjpgUrl = "http://38.81.159.248:80/mjpg/video.mjpg";
        bool motionDetected = false;

        while (capture.Retrieve(frame, 0))
        {
            
            framesCount++;
            
            CvInvoke.NamedWindow("Camera", WindowFlags.KeepRatio);
            
            CvInvoke.Imshow("Camera",frame);

            //CvInvoke.CvtColor(frame, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            //CvInvoke.GaussianBlur(gray, gray, kSize, 0);
            //if(framesCount==10)
            //{
            //    CvInvoke.CvtColor(frame, staticFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            //    CvInvoke.GaussianBlur(staticFrame, staticFrame, kSize, 0);
            //    framesCount = 0;
            //}

            //CvInvoke.AbsDiff(staticFrame, gray, frameDelta);
            //CvInvoke.Threshold(frameDelta, thresh, 25, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            //CvInvoke.Dilate(thresh, thresh, Mat, kPoint, 2,Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(255, 255, 255));
            //CvInvoke.FindContours(thresh, cnts,hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);
        }

        CvInvoke.WaitKey();
        Console.WriteLine("Hello, World!");
    }
}