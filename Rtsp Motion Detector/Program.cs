using Rtsp_Motion_Detector;

internal class Program
{
    public static void Main()
    {
        // Replace "YOUR_RTSP_STREAM_URL" with the actual RTSP stream URL you want to monitor
        //string rtspStreamUrl = "http://158.58.130.148/mjpg/video.mjpg";
        List<String> streamLinks = new List<String>() {
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c1/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c2/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c3/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c4/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c5/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c6/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c7/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c8/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c9/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c10/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c11/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c12/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c13/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c14/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c15/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c16/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c17/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c18/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c19/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c20/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c21/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c22/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c23/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c24/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c25/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c26/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c27/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c28/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c1/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c2/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c3/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c4/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c5/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c6/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c7/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c8/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c9/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c10/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c11/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c12/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c13/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c14/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c15/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c16/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c17/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c18/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c19/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c20/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c21/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c22/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c23/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c24/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c25/s1/live",
            "rtsp://admin:isourse@12345@103.84.203.174:554/unicast/c26/s1/live"
        };
        List<MotionDetector> detectors = new();

        // Create a new instance of MotionDetector
        //MotionDetector motionDetector = new(streamLinks[0],senitivity: 1);

        for (int i = 0; i < streamLinks.Count; i++)
        {
            MotionDetector tempReference = new MotionDetector(streamLinks[i], senitivity: 1);
            tempReference.Start(showCamera:true);
            detectors.Add(tempReference);
        }

        // Subscribe to the MotionDetected event
        //motionDetector.MotionDetected += MotionDetector_MotionDetected;

        // Start motion detection
        //motionDetector.Start(showCamera: true);

        //Console.WriteLine($"Motion detection started.\nListening on: {rtspStreamUrl} \nPress any key to stop...");
        Console.WriteLine($"Motion detection started.\nPress any key to stop...");
        Console.ReadKey();

        // Stop motion detection when a key is pressed
        //motionDetector.Stop();
    }

    // Event handler for MotionDetected event
    private static void MotionDetector_MotionDetected(object? sender, EventArgs e)
    {
        Console.WriteLine("Motion detected!");
        // You can add any custom action you want to perform when motion is detected.
        // For example, you can send a notification, save the video clip, etc.
    }
}