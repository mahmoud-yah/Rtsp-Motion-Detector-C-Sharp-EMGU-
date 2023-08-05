using Rtsp_Motion_Detector;

internal class Program
{
    public static void Main(string[] args)
    {
        MotionDetector motionDetector = new(streamUrl: "http://158.58.130.148/mjpg/video.mjpg", senitivity: 5);
        motionDetector.Start(showCamera:true);
    }
}