using System;
using ModusAdjustCylinder_Math;
namespace ModusAdjustCylinder
{
    internal class Point
    {

        Vector3d m_xyz = new Vector3d(),
                 m_ijk = new Vector3d();

        double m_radius = 0;


        bool m_hasRadius = false,
             m_hasIJK = false;



        #region Public Properties

        public bool HasRadius { get => m_hasRadius; set { m_hasRadius = value; } }
        public bool HasIJK { get => m_hasIJK; set { m_hasIJK = value; } }
        public Vector3d xyz { get => m_xyz; set { m_xyz = value; } }
        public Vector3d ijk { get => m_ijk; set { m_ijk = value; } }

        public double Radius { get => m_radius; set { m_radius = value; } }
        #endregion


        public static bool ParseFromLine(string line, out Point result)
        {
            result = null;
            try
            {
                result = new Point();
                string[] splitLine = line.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < 3; i++)
                    result.xyz[i] = Convert.ToDouble(splitLine[i]);


                if (splitLine.Length == 4)
                {
                    result.HasRadius = true;
                    result.HasIJK = false;
                    result.Radius = Convert.ToDouble(splitLine[3]);
                }

                if (splitLine.Length >= 6)
                {
                    result.HasRadius = false;
                    result.HasIJK = true;
                    for (int i = 0; i < 3; i++)
                        result.ijk[i] = Convert.ToDouble(splitLine[i + 3]);
                }
                if (splitLine.Length == 7)
                {
                    result.HasRadius = true;
                    result.Radius = Convert.ToDouble(splitLine[6]);
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: {line}{Environment.NewLine}{ex.Message}");
                return false;
            }
        }


    }
}