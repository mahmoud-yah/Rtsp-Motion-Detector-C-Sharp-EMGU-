using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
    public readonly DetectionCoordinates Coordinates;

    public bool KeepDetecting { get; private set; } = true;
    public bool IsDetecting { get; private set; } = false;
    public static bool ResizeSource { get; set; } = true;
    public static bool ResizeFrames { get; set; } = true;
    public static int FrameLimit { get; set; } = 20;
    public static TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(10);
    public event EventHandler<MotionDetector>? MotionDetected;

    public MotionDetector(string streamUrl, DetectionCoordinates coordinates, int senitivity = 5, bool showCamera = true)
    {
        counter++;
        cameraID = counter;
        this.showCamera = showCamera;
        StreamUrl = streamUrl;
        Sensitivity = senitivity <= 0 || senitivity > 10 ? 5 : senitivity;
        Coordinates = coordinates;
    }

    public void Start()
    {
        Task.Run(() =>
        {
            int failTimeOut = 1;
            try
            {
                StartDetector();
            }
            catch (Exception ex)
            {
                IsDetecting = false;
                Console.WriteLine("...Error... " + ex.Message);
                if (ex is TimeoutException || --failTimeOut == 0)
                    return;
                Console.WriteLine("Restarting detector..");
                StartDetector();
            }
        });
    }

    private void StartDetector()
    {
        int framesCount = 0;

        var frame = new Mat();
        var gray = new Mat();
        var staticFrame = new Mat();
        var frameDelta = new Mat();
        var thresh = new Mat();
        var croppedFrame=new Mat();
        var vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
        var rect = new Rectangle((int)Coordinates.X,(int)Coordinates.Y,(int)Coordinates.Width,(int)Coordinates.Height);
        var rectangleColor = new MCvScalar(0, 0, 255);
        

        //passing 0 uses the local webcam instead of a network stream
        using var capture = new VideoCapture(StreamUrl);

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

        croppedFrame = new Mat(frame, rect);

        //convert the frame to grayscale
        CvInvoke.CvtColor(croppedFrame, staticFrame, ColorConversion.Bgr2Gray);
        CvInvoke.GaussianBlur(staticFrame, staticFrame, GaussianKernalSize, 0);

        while (capture.Read(frame))
        {
            //checking if the detector is still running
            if (!KeepDetecting)
                break;

            IsDetecting = true;

            //resizing frames to lower the resources
            if (ResizeFrames)
                CvInvoke.Resize(frame, frame, new Size(), 0.5, 0.5);

            //croping the area of interest from the original frame
            croppedFrame = new Mat(frame, rect);

            //drawing the red rectangle on the original frame around the area of interest  
            CvInvoke.Rectangle(frame, rect, rectangleColor,2);
            

            //convert the frame to grayscale
            CvInvoke.CvtColor(croppedFrame, gray, ColorConversion.Bgr2Gray);
            CvInvoke.GaussianBlur(gray, gray, GaussianKernalSize, 0);

            if (framesCount == 10)
            {
                //reinitializing the static frame every 10 frames
                CvInvoke.CvtColor(croppedFrame, staticFrame, ColorConversion.Bgr2Gray);
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
                OnMotionDetected(this);
                break;
            }

            framesCount++;

            if (showCamera)
                CvInvoke.Imshow($"Camera {cameraID}", frame);

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
    private void OnMotionDetected(MotionDetector e)
    {
        Console.WriteLine($"Motion Detected from camera #{cameraID}");
        MotionDetected?.Invoke(this, e);
    }

    public void Stop() => KeepDetecting = false;
}
