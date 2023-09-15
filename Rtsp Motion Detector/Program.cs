using Rtsp_Motion_Detector;
using System.Diagnostics;
using System.Text.Json;

internal class Program
{
    public static void Main()
    {
        var streamLinks = new List<string>() {
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c2/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c1/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c5/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c7/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c8/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c9/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c11/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c6/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c4/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c12/s1/live",
            "rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c3/s1/live",
        };

        List<MotionDetector> detectors = new();

        foreach (string link in streamLinks)
        {
            var detector = new MotionDetector(link, senitivity: 1, showCamera: false);
            detector.MotionDetected += MotionDetected;
            detector.Start();
            detectors.Add(detector);
        }

        var countThread = new Thread(() => UpdateDetectorCount(detectors)) { IsBackground = true };
        countThread.Start();

        Console.WriteLine($"Motion detection started.\nPress any key to stop...");
        Console.ReadKey();
    }

    //static readonly List<string> WorkingCameras = new();
    //static readonly object Flag = new();

    private static void MotionDetected(object? sender, MotionDetector e)
    {
        e.MotionDetected -= MotionDetected;
        //lock (Flag)
        //{
        //    if (WorkingCameras.Contains(e.StreamUrl))
        //        return;
        //    Debug.WriteLine(e.StreamUrl);
        //    WorkingCameras.Add(e.StreamUrl);
        //    File.WriteAllText("Working.txt", JsonSerializer.Serialize(WorkingCameras));
        //}
    }

    private static void UpdateDetectorCount(List<MotionDetector> detectors)
    {
        while (true)
        {
            // Calculate the count of detectors with IsDetecting == true
            int count = detectors.Count(detector => detector.IsDetecting == true);

            // Display the count
            Console.Clear();
            Console.WriteLine($"Running now: {count} cameras");

            // Sleep for 1000 milliseconds (1 second) before the next update
            Thread.Sleep(5000);
        }
    }
}