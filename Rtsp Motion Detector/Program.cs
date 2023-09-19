using Rtsp_Motion_Detector;
using System.Diagnostics;
using System.Text.Json;

internal class Program
{
    public static void Main()
    {
        var streamCameras = new List<CameraModel>() {
            new CameraModel(1,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c1/s1/live",1,new DetectionCoordinates(0,0,50,50)),
            new CameraModel(2,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c2/s1/live",1,new DetectionCoordinates(25,25,75,50)),
            new CameraModel(3,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c3/s1/live",1,new DetectionCoordinates(50,50,100,100)),
            new CameraModel(4,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c4/s1/live",1,new DetectionCoordinates(75,75,100,70)),
            new CameraModel(5,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c5/s1/live",1,new DetectionCoordinates(100,100,100,120)),
            new CameraModel(6,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c6/s1/live",1,new DetectionCoordinates(125,125,100,100)),
            new CameraModel(7,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c7/s1/live",1,new DetectionCoordinates(150,150,150,30)),
            new CameraModel(8,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c8/s1/live",1,new DetectionCoordinates(175,175,100,100)),
            new CameraModel(9,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c9/s1/live",1,new DetectionCoordinates(200,200,75,50)),
            new CameraModel(10,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c10/s1/live",1,new DetectionCoordinates(0,200,200,50)),
            new CameraModel(11,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c11/s1/live",1,new DetectionCoordinates(0,150,170,50)),
            new CameraModel(12,"rtsp://admin:isourse@12345@115.241.133.98:554/unicast/c12/s1/live",1,new DetectionCoordinates(0,100,100,100)),
        };

        List<MotionDetector> detectors = new();

        foreach (var camera in streamCameras)
        {
            var detector = new MotionDetector(camera.RTSPlink,camera.Coords ,camera.Threshold, showCamera: true);
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