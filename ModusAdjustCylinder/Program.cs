using ModusAdjustCylinder_Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModusAdjustCylinder
{

    public enum Axis
    {
        NONE,
        X,
        Y,
        Z
    }

    public enum BasicPlane
    {
        NONE,
        XY,
        YZ,
        ZX
    }


    class Program
    {
        static void Main(string[] args)
        {

            if (!GetArguments(args, out string inputFile, out string outputFile, out Axis rotationAxis, out BasicPlane outputPlane)) return;
            if (!CheckArguments(inputFile, rotationAxis, outputPlane)) return;
            if (!ReadFile(inputFile, out List<Point> points)) return;

            if (!AdjustCylinder(points, rotationAxis, outputPlane, out List<Point> adjustedPoints)) return;

            WritePoints(outputFile, adjustedPoints);




        }

        private static void WritePoints(string outputFile, List<Point> adjustedPoints)
        {
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach(Point p in adjustedPoints)
                {
                    string format = "{0: 0.000000;-0.000000; 0.000000}";

                    string line = string.Join(",", p.xyz.ToArray().Select(item => string.Format(format, item)));
                    
                    /*if (p.HasIJK)
                        line += $",{string.Join(",", p.ijk.ToArray().Select(item => string.Format(format, item)))}";*/
                    if (p.HasRadius)
                        line += $",{p.Radius:F6}";

                    sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();
            }
        }

        private static bool ProjectPoint(Vector3d rotationVector, Point p, out Point projectedPoint)
        {
            try
            {
                double c = new Vector3d(0, 0, 0) * rotationVector;
                double a = (p.xyz * rotationVector) - c;
                projectedPoint = new Point()
                {
                    xyz = p.xyz - a * rotationVector,
                    ijk = p.ijk,
                    HasRadius = p.HasRadius,
                    HasIJK = p.HasIJK,
                    Radius = p.Radius
                };
            }
            catch(Exception ex)
            {
                projectedPoint = null;
                Console.WriteLine(ex.Message);
                return false;

            }
            return true;
        }

        private static bool AdjustCylinder(List<Point> points, Axis rotationAxis, BasicPlane outputPlane, out List<Point> adjustedPoints)
        {
            adjustedPoints = null;
            Vector3d rotationVector = new Vector3d(1, 0, 0);
            if (rotationAxis == Axis.Y) rotationVector = new Vector3d(0, 1, 0);
            else if (rotationAxis == Axis.Z) rotationVector = new Vector3d(0, 0, 1);

            Vector3d outputPlaneVector = new Vector3d(1, 0, 0);
            if (outputPlane == BasicPlane.XY) outputPlaneVector = new Vector3d(0, 0, 1);
            else if (outputPlane == BasicPlane.YZ) outputPlaneVector = new Vector3d(1, 0, 0);
            else if (outputPlane == BasicPlane.ZX) outputPlaneVector = new Vector3d(0, 1, 0);

            Vector3d resultDirection = (outputPlaneVector^rotationVector).Normalized();

            adjustedPoints = new List<Point>();
            foreach (Point p in points)
            {
                if(!ProjectPoint(rotationVector,p,out Point projectedPoint))
                    return false;

                double dot = projectedPoint.xyz.Normalized() * resultDirection;
                double angle = Math.Acos(dot);
                double angleDegrees = angle * (180.0 / Math.PI);

                if (angle > Math.PI / 2.0) angle = -((Math.PI) - angle);
                angleDegrees = angle * (180.0 / Math.PI);


                AngleAxisd aa = new AngleAxisd(-angle, rotationVector);

                Point rotatedPoint = new()
                {
                    xyz = aa * p.xyz,
                    ijk = aa * p.ijk,
                    HasIJK = p.HasIJK,
                    HasRadius = p.HasRadius,
                    Radius = p.Radius
                };
                adjustedPoints.Add(rotatedPoint);
            }
            return true;
        }

        private static bool ReadFile(string inputFile, out List<Point> points)
        {
            try
            {
                List<string> fileLines = new List<string>();
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    while (!sr.EndOfStream)
                        fileLines.Add(sr.ReadLine());
                    sr.Close();
                }

                points = new List<Point>();
                foreach (string line in fileLines)
                {
                    if (!Point.ParseFromLine(line, out Point p))
                    {
                        points = null;
                        return false;
                    }
                    points.Add(p);
                }
                return true;
            }
            catch(Exception ex)
            {
                points = null;
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        private static bool GetArguments(string[] args, out string inputFile, out string outputFile, out Axis rotationAxis, out BasicPlane outputPlane)
        {
            inputFile = null;
            outputFile = null;
            rotationAxis = Axis.NONE;
            outputPlane = BasicPlane.NONE;
            try
            {
                if (args.Length < 4)
                {
                    Console.WriteLine(@"Invalid number of arguments. Arguments are: 
    1. Input file name.
    2. Output file name.
    3. Axis of rotation (X,Y,Z).
    4. Output Plane (XY,YZ,ZX).");
                    return false;
                }

                inputFile = args[0];
                outputFile = args[1];
                if (!Enum.TryParse(typeof(Axis), args[2], true, out object tempAxis))
                {
                    Console.WriteLine("Rotation Axis is not valid.");
                    return false;
                }

                rotationAxis = (Axis)tempAxis;
                if (!Enum.TryParse(typeof(BasicPlane), args[3], true, out object tempPlane))
                {
                    Console.WriteLine("Output Plane is not valid.");
                    return false;
                }

                outputPlane = (BasicPlane)tempPlane;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static bool CheckArguments(string inputFile, Axis rotationAxis, BasicPlane outputPlane)
        {
            if (!System.IO.File.Exists(inputFile))
            {
                Console.WriteLine("Input file not found.");
                return false;
            }

            if (rotationAxis == Axis.X && outputPlane == BasicPlane.YZ ||
                rotationAxis == Axis.Y && outputPlane == BasicPlane.ZX ||
                rotationAxis == Axis.Z && outputPlane == BasicPlane.YZ)
            {
                Console.WriteLine("Invalid combination of rotation axis and output plane");
                return false;
            }

            return true;

        }
    }
}
