using System;
using System.Collections.Generic;
using System.IO;

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
                if (!Enum.TryParse(typeof(BasicPlane), args[2], true, out object tempPlane))
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
