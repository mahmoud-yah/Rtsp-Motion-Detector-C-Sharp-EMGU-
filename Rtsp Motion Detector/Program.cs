using Rtsp_Motion_Detector;

internal class Program
{
    public static void Main()
    {
        // Replace "YOUR_RTSP_STREAM_URL" with the actual RTSP stream URL you want to monitor
        string rtspStreamUrl = "http://158.58.130.148/mjpg/video.mjpg";

        // Create a new instance of MotionDetector
        MotionDetector motionDetector = new(rtspStreamUrl,senitivity: 1);

        // Subscribe to the MotionDetected event
        motionDetector.MotionDetected += MotionDetector_MotionDetected;

        // Start motion detection
        motionDetector.Start(showCamera: false);

        Console.WriteLine($"Motion detection started.\nListening on: {rtspStreamUrl} \nPress any key to stop...");
        Console.ReadKey();

        // Stop motion detection when a key is pressed
        motionDetector.Stop();
    }

    // Event handler for MotionDetected event
    private static void MotionDetector_MotionDetected(object? sender, EventArgs e)
    {
        Console.WriteLine("Motion detected!");
        // You can add any custom action you want to perform when motion is detected.
        // For example, you can send a notification, save the video clip, etc.
    }
}