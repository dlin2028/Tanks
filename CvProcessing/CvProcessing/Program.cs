using CSCore;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Numerics;
using System.Threading.Tasks;

namespace CvProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();
            List<byte> data = new List<byte>();

            int x2 = 0;
            int y2 = 0;
            int a2 = 0;

            HttpCamera skyCam = new HttpCamera("HTTPCam", "http://192.168.1.121:1181/stream.mjpg");

            MjpegServer mjpeg = new MjpegServer("MjpegServer", 9000);
            mjpeg.Source = skyCam;

            CvSink skySink = new CvSink("Sky");
            skySink.Source = mjpeg.Source;
            skySink.Enabled = true;

            CvSource cvSource1 = new CvSource("Source 1", PixelFormat.Mjpeg, 1920, 1080, 30);
            MjpegServer source1Server = new MjpegServer("Source 1 server", 9001);
            source1Server.Source = cvSource1;

            CvSource cvSource2 = new CvSource("Source 2", PixelFormat.Mjpeg, 1920, 1080, 30);
            MjpegServer inRangeServer = new MjpegServer("Source 2 server", 9002);
            inRangeServer.Source = cvSource2;


            cvSource2.CreateProperty("HLow", PropertyKind.Integer, 0, 180, 1, 75, 75);
            cvSource2.CreateProperty("HHigh", PropertyKind.Integer, 0, 180, 1, 80, 80);

            cvSource2.CreateProperty("SLow", PropertyKind.Integer, 0, 255, 1, 21, 21);
            cvSource2.CreateProperty("SHigh", PropertyKind.Integer, 0, 255, 1, 255, 255);

            cvSource2.CreateProperty("VLow", PropertyKind.Integer, 0, 255, 1, 131, 131);
            cvSource2.CreateProperty("VHigh", PropertyKind.Integer, 0, 255, 1, 255, 255);

            //IPEndPoint EP = new IPEndPoint(IPAddress.Any, 0);
            //UdpClient reciever = new UdpClient(9003, AddressFamily.InterNetwork);

            //while (true)
            //{
            //    byte[] received = reciever.Receive(ref EP);
            //    Console.Write("{ ");
            //    for (int i = 0; i < received.Length; i++)                    
            //    {
            //        Console.Write($"{received[i]}{(i < received.Length - 1 ? "," : "")}");
            //    }
            //    Console.WriteLine($" }} (from: {EP})");
            //}

            Mat input = new Mat();
            Mat hsv = new Mat();
            Mat inRange = new Mat();
            Mat inRangeCopy = new Mat();

            double angle1 = 0;
            double angle2 = 0;

            while (true)
            {
                if (skySink.GrabFrame(input) == 0)
                {
                    continue;
                }

                //Cv2.Resize(input, input, new Size(640, 360));

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
                List<int> averages = new List<int>();
                List<Point[]> polyPoints = new List<Point[]>();


                foreach (var contour in contours)
                {
                    Point[] hull = Cv2.ConvexHull(contour);

                    Point[] poly = Cv2.ApproxPolyDP(hull, 5, true);
                    if (poly.Length == 3)
                    {
                        if (Cv2.ContourArea(hull) < 560)
                        {
                            #region OtherTriangle
                            if(Cv2.ContourArea(hull) < 500)
                            {
                                continue;
                            }
                            double yAv = 0;
                            double xAv = 0;
                            int ct = 0;
                            for (int i = 0; i < poly.Length; i++)
                            {
                                ct++;
                                xAv += poly[i].X;
                                yAv += poly[i].Y;
                            }

                            double zO = poly[0].DistanceTo(poly[1]);
                            double oT = poly[1].DistanceTo(poly[2]);
                            double tZ = poly[2].DistanceTo(poly[0]);

                            double op = 0;
                            double adj = 0;
                            bool rQ = false;

                            if (zO < oT && zO < tZ)
                            {
                                Point p = new Point((poly[0].X + poly[1].X) / 2, (poly[0].Y + poly[1].Y) / 2);
                                input.Line(p, poly[2], Scalar.White, 2);
                                op = poly[2].Y - p.Y;
                                adj = poly[2].X - p.X;
                                if (adj >= 0) rQ = true;
                            }
                            else if (oT < zO && oT < tZ)
                            {
                                Point p = new Point((poly[1].X + poly[2].X) / 2, (poly[1].Y + poly[2].Y) / 2);
                                input.Line(p, poly[0], Scalar.White, 2);
                                op = poly[0].Y - p.Y;
                                adj = poly[0].X - p.X;
                                if (adj >= 0) rQ = true;
                            }
                            else
                            {
                                Point p = new Point((poly[2].X + poly[0].X) / 2, (poly[2].Y + poly[0].Y) / 2);
                                input.Line(p, poly[1], Scalar.White, 2);
                                op = poly[1].Y - p.Y;
                                adj = poly[1].X - p.X;
                                if (adj >= 0) rQ = true;
                            }

                            xAv /= ct;
                            yAv /= ct;

                            x2 = (int)(xAv * 100);
                            y2 = (int)(yAv * 100);
                            a2 = (int)((Math.Atan(op / adj) * 180 / Math.PI));
                            if (rQ)
                            {
                                a2 = a2 + 270;
                                rQ = false;
                            }
                            else
                            {
                                a2 += 90;
                            }
                            #endregion

                            continue;
                        }
                        double yAverage = 0;
                        double xAverage = 0;
                        int count = 0;
                        for (int i = 0; i < poly.Length; i++)
                        {
                            count++;
                            xAverage += poly[i].X;
                            yAverage += poly[i].Y;
                        }

                        double zeroOne = poly[0].DistanceTo(poly[1]);
                        double oneTwo = poly[1].DistanceTo(poly[2]);
                        double twoZero = poly[2].DistanceTo(poly[0]);

                        double opposite = 0;
                        double adjacent = 0;
                        bool rightQuadrants = false;

                        if (zeroOne < oneTwo && zeroOne < twoZero)
                        {
                            Point p = new Point((poly[0].X + poly[1].X) / 2, (poly[0].Y + poly[1].Y) / 2);
                            input.Line(p, poly[2], Scalar.White, 2);
                            opposite = poly[2].Y - p.Y;
                            adjacent = poly[2].X - p.X;
                            if (adjacent >= 0) rightQuadrants = true;
                        }
                        else if (oneTwo < zeroOne && oneTwo < twoZero)
                        {
                            Point p = new Point((poly[1].X + poly[2].X) / 2, (poly[1].Y + poly[2].Y) / 2);
                            input.Line(p, poly[0], Scalar.White, 2);
                            opposite = poly[0].Y - p.Y;
                            adjacent = poly[0].X - p.X;
                            if (adjacent >= 0) rightQuadrants = true;
                        }
                        else
                        {
                            Point p = new Point((poly[2].X + poly[0].X) / 2, (poly[2].Y + poly[0].Y) / 2);
                            input.Line(p, poly[1], Scalar.White, 2);
                            opposite = poly[1].Y - p.Y;
                            adjacent = poly[1].X - p.X;
                            if (adjacent >= 0) rightQuadrants = true;
                        }

                        xAverage /= count;
                        yAverage /= count;

                        int x = (int)(xAverage * 100);
                        int y = (int)(yAverage * 100);
                        int angle = (int)((Math.Atan(opposite / adjacent) * 180 / Math.PI));
                        if (rightQuadrants)
                        {
                            angle = angle + 270;
                            rightQuadrants = false;
                        }
                        else
                        {
                            angle += 90;
                        }
                        //Console.WriteLine(angle);
                        AddData(x, data);
                        AddData(y, data);
                        AddData(angle, data);
                    }
                    else if(poly.Length == 4)
                    {
                        if (Cv2.ContourArea(hull) < 300)
                        {
                            continue;
                        }
                        polyPoints.Add(poly);
                        Rect boundingRect = Cv2.BoundingRect(poly);
                        Point centerPoint = new Point(boundingRect.X + boundingRect.Width / 2, boundingRect.Y + boundingRect.Height / 2);

                        if (boundingRect.X < 540)
                        {
                            Cv2.Circle(input, centerPoint, 10, Scalar.AliceBlue);
                            Point difference = centerPoint - new Point(421, 372);

                            angle1 = Math.Atan2(difference.X, difference.Y) * 180 / Math.PI;
                        }
                        else
                        {
                            Cv2.Circle(input, centerPoint, 10, Scalar.HotPink);
                            Point difference = centerPoint - new Point(876, 369);

                            angle2 = Math.Atan2(difference.X, difference.Y) * 180 / Math.PI;
                        }

                    }
                }
                input.PutText("Angle1: " + angle1.ToString(), new Point(421, 400), HersheyFonts.HersheyComplex, 0.5f, Scalar.AliceBlue);


                input.PutText("Angle2: " + angle2.ToString(), new Point(876, 400), HersheyFonts.HersheyComplex, 0.5f, Scalar.HotPink);
                Cv2.Circle(input, new Point(421, 372), 1, Scalar.AliceBlue);
                Cv2.Circle(input, new Point(876, 369), 1, Scalar.AliceBlue);

                AddData((int)Math.Round(angle1, 0), data);
                AddData((int)Math.Round(angle2, 0), data);
                AddData(x2, data);
                AddData(y2, data);
                AddData(a2, data);

                input.DrawContours(polyPoints, - 1, Scalar.Blue, -1);
                cvSource1.PutFrame(input);
                cvSource2.PutFrame(inRange);
                client.Send(data.ToArray(), data.Count, new IPEndPoint(IPAddress.Loopback, 9003));
                data.Clear();
                Thread.Sleep(50);
            }
        }

        static void AddData(int x, List<byte> data)
        {
            data.Add((byte) (x & 0xff));
            data.Add((byte)((x >> 8) & 0xff));
            data.Add((byte)((x >> 16) & 0xff));
            data.Add((byte)((x >> 24) & 0xff));
        }
    }
}