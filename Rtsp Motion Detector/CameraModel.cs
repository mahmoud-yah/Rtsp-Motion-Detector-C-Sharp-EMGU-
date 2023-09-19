using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtsp_Motion_Detector
{

    public class DetectionCoordinates
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Width;
        public readonly double Height;

        public DetectionCoordinates(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

    }
    internal class CameraModel
    {
        public readonly int CameraID;
        public readonly string RTSPlink;
        public readonly int Threshold;
        public readonly DetectionCoordinates Coords;

        public CameraModel(int cameraId, string rtspLink, int threshold, DetectionCoordinates coords)
        {
            CameraID = cameraId;
            RTSPlink = rtspLink;
            Threshold = threshold;
            Coords = coords;
        }

    }
}
