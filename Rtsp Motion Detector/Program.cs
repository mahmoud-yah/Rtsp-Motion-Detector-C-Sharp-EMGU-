using System;
using System.Drawing;
using System.Numerics;
using System.Threading;
using Emgu;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Rtsp_Motion_Detector;

internal class Program
{
    private static void Main(string[] args)
    {
        MotionDetector motionDetector = new(streamUrl: "http://158.58.130.148/mjpg/video.mjpg", senitivity: 10);
        motionDetector.Start(showCamera:true);
    }
}