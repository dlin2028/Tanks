using CSCore;
using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace CvProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpCamera skyCam = new HttpCamera("HTTPCam", "http://192.168.1.121:1181/stream.mjpg");

            MjpegServer mjpeg = new MjpegServer("MjpegServer", 9000);
            mjpeg.Source = skyCam;

            CvSink skySink = new CvSink("Sky");
            skySink.Source = mjpeg.Source;
            skySink.Enabled = true;

            CvSource cvSource1 = new CvSource("Source 1", PixelFormat.Mjpeg, 640, 360, 30);
            MjpegServer source1Server = new MjpegServer("Source 1 server", 9001);
            source1Server.Source = cvSource1;

            CvSource cvSource2 = new CvSource("Source 2", PixelFormat.Mjpeg, 640, 360, 30);
            MjpegServer inRangeServer = new MjpegServer("Source 2 server", 9002);
            inRangeServer.Source = cvSource2;


            cvSource2.CreateProperty("HLow", PropertyKind.Integer, 0, 180, 1, 75, 75);
            cvSource2.CreateProperty("HHigh", PropertyKind.Integer, 0, 180, 1, 80, 80);

            cvSource2.CreateProperty("SLow", PropertyKind.Integer, 0, 255, 1, 100, 100);
            cvSource2.CreateProperty("SHigh", PropertyKind.Integer, 0, 255, 1, 255, 255);

            cvSource2.CreateProperty("VLow", PropertyKind.Integer, 0, 255, 1, 155, 155);
            cvSource2.CreateProperty("VHigh", PropertyKind.Integer, 0, 255, 1, 255, 255);



            Mat input = new Mat();
            Mat hsv = new Mat();
            Mat inRange = new Mat();
            Mat inRangeCopy = new Mat();

            while (true)
            {
                if (skySink.GrabFrame(input) == 0)
                {
                    continue;
                }

                Cv2.Resize(input, input, new Size(640, 360));

                Cv2.CvtColor(input, hsv, ColorConversionCodes.BGR2HSV);

                int hlow = cvSource2.GetProperty("HLow").Get();
                int hhigh = cvSource2.GetProperty("HHigh").Get();
                int slow = cvSource2.GetProperty("SLow").Get();
                int shigh = cvSource2.GetProperty("SHigh").Get();
                int vlow = cvSource2.GetProperty("VLow").Get();
                int vhigh = cvSource2.GetProperty("VHigh").Get();

                Cv2.InRange(hsv, new Scalar(hlow, slow, vlow), new Scalar(hhigh, shigh, vhigh), inRange);

                inRange.CopyTo(inRangeCopy);
                Point[][] contours = Cv2.FindContoursAsArray(inRangeCopy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

                List<Point[]> GreenDraws = new List<Point[]>();
                List<Point[]> BlueDraws = new List<Point[]>();
                List<int> averages = new List<int>();
                foreach (var contour in contours)
                {
                    Point[] hull = Cv2.ConvexHull(contour);
                    if(Cv2.ContourArea(hull) < 400)
                    {
                        continue;
                    }

                    Point[] poly = Cv2.ApproxPolyDP(contour, 4, true);

                    if(poly.Length != 3)
                    {
                        continue;
                    }

                    GreenDraws.Add(hull);
                }

                input.DrawContours(GreenDraws, -1, Scalar.DarkGreen, -1);
                input.DrawContours(BlueDraws, -1, Scalar.Blue, -1);
                cvSource1.PutFrame(input);
            }
        }
    }
}