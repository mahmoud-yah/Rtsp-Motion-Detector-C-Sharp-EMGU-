using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Drawing;

namespace Rtsp_Motion_Detector;

internal class MotionDetector
{
    private static int counter = 0;

    private readonly Size GaussianKernalSize = new(21, 21);

    public readonly int cameraID;
    public readonly bool showCamera;
    public readonly string StreamUrl;
    public readonly int Sensitivity;

    public bool IsDetecting { get; private set; } = false;
    public static bool ResizeSource { get; set; } = true;
    public static bool ResizeFrames { get; set; } = true;
    public static int FrameLimit { get; set; } = 20;
    public static TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(10);
    public event EventHandler? MotionDetected;

    public MotionDetector(string streamUrl, int senitivity = 5, bool showCamera = true)
    {
        counter++;
        cameraID = counter;
        this.showCamera = showCamera;
        StreamUrl = streamUrl;
        Sensitivity = senitivity <= 0 || senitivity > 10 ? 5 : senitivity;
    }

    public void Start()
    {
        Task.Run(() =>
        {
            try
            {
                StartDetector();
            }
            catch (Exception ex)
            {
                Console.WriteLine("...Error... " + ex.Message);
                if (ex is TimeoutException)
                    return;
                Console.WriteLine("Restarting detector..");
                StartDetector();
            }
        });
    }

    private void StartDetector()
    {
        IsDetecting = true;
        int framesCount = 0;

        var frame = new Mat();
        var gray = new Mat();
        var staticFrame = new Mat();
        var frameDelta = new Mat();
        var thresh = new Mat();
        var vectorOfVectorOfPoint = new VectorOfVectorOfPoint();

        //passing 0 uses the local webcam instead of a network stream
        var capture = new VideoCapture(StreamUrl);

        // Wait for the new source to open (you can set a timeout if needed)
        if (!WaitSourceToOpen(capture))
            throw new TimeoutException($"Timed out waiting for the new source to open camera {cameraID}, {StreamUrl}.");

        Console.WriteLine($"starting camera #{cameraID}, {StreamUrl}");

        //set frame limit and video size to 512x288
        capture.Set(CapProp.Fps, FrameLimit);
        if (ResizeSource)
        {
            capture.Set(CapProp.FrameWidth, 512);
            capture.Set(CapProp.FrameHeight, 288);
        }

        //initializing the first frame
        capture.Read(frame);

        //resizing frames to optimize the performance
        CvInvoke.Resize(frame, frame, new Size(), 0.5, 0.5);


        //convert the frame to grayscale
        CvInvoke.CvtColor(frame, staticFrame, ColorConversion.Bgr2Gray);
        CvInvoke.GaussianBlur(staticFrame, staticFrame, GaussianKernalSize, 0);

        while (capture.Read(frame))
        {
            //checking if the detector is still running
            if (!IsDetecting)
                break;

            //resizing frames to lower the resources
            if (ResizeFrames)
                CvInvoke.Resize(frame, frame, new Size(), 0.5, 0.5);

            //convert the frame to grayscale
            CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);
            CvInvoke.GaussianBlur(gray, gray, GaussianKernalSize, 0);

            if (framesCount == 10)
            {
                //reinitializing the static frame every 10 frames
                CvInvoke.CvtColor(frame, staticFrame, ColorConversion.Bgr2Gray);
                CvInvoke.GaussianBlur(staticFrame, staticFrame, GaussianKernalSize, 0);
                framesCount = 0;
            }

            //compute the difference between the static frame and the current frame
            CvInvoke.AbsDiff(staticFrame, gray, frameDelta);
            CvInvoke.Threshold(frameDelta, thresh, 25, 255, ThresholdType.Binary);
            CvInvoke.FindContours(thresh, vectorOfVectorOfPoint, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++)
            {
                if (CvInvoke.ContourArea(vectorOfVectorOfPoint[i]) < (Sensitivity * 100))
                    continue;
                //motionDetected
                // Raise the motion detected event
                OnMotionDetected(EventArgs.Empty);
                break;
            }

            framesCount++;

            if (showCamera)
                CvInvoke.Imshow("Camera " + cameraID.ToString(), frame);

            if (CvInvoke.WaitKey(1) == 27)
                break;
        }
    }

    private static bool WaitSourceToOpen(VideoCapture capture)
    {
        var startTime = DateTime.Now;

        while (!capture.IsOpened)
        {
            if ((DateTime.Now - startTime) > MaxWaitTime)
                break;

            Task.Delay(500).Wait();
        }

        return capture.IsOpened;
    }

    // Helper method to raise the MotionDetected event
    private void OnMotionDetected(EventArgs e)
    {
        Debug.WriteLine("Motion Detected from camera" + cameraID.ToString());
        MotionDetected?.Invoke(this, e);
    }

    public void Stop() => IsDetecting = false;
}
