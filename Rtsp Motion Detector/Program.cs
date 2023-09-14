using Rtsp_Motion_Detector;

internal class Program
{
    public static void Main()
    {
        List<string> streamLinks = new() {
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

        for (int i = 0; i < streamLinks.Count; i++)
        {
            MotionDetector tempReference = new(streamLinks[i], senitivity: 1, showCamera: false);
            tempReference.Start();
            detectors.Add(tempReference);
        }

        while(true)
        {
            Task.Delay(1000).Wait();
            Console.WriteLine($"Running now : {detectors.Count(c => c.IsDetecting == true)} cameras");
        }

        Console.WriteLine($"Motion detection started.\nPress any key to stop...");
        Console.ReadKey();
    }
}