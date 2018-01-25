using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace UDPCasts
{
    public static class ExtensionMethods
    {
        public static byte [] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static byte[][] ToByteArray(this IEnumerable<PoseIPStruct> dataSet)
        {
            int numRows = dataSet.Count();
            byte[][] bytes = new byte[numRows][];

            int nextIndex = 0;

            foreach(PoseIPStruct poseIP in dataSet)
            {
                bytes[nextIndex] = poseIP.Bytes;
                nextIndex++;
            }

            return bytes;
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        public static double DegToRad(this double value)
        {
            return (value * Math.PI) / 180.0d;
        }

        /// <summary>
        /// Wraps into range [-PI, PI]
        /// </summary>
        public static double PiWrap(this double angleRad)
        {
            int numRevs = (int)(angleRad / (2.0d * Math.PI));
            angleRad -= (double)numRevs * 2.0d * Math.PI;
            if (angleRad > Math.PI)
            {
                angleRad -= 2.0d * Math.PI;
            }
            if (angleRad <= -Math.PI)
            {
                angleRad += 2.0d * Math.PI;
            }
            return angleRad;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        public static double RadToDeg(this double value)
        {
            return (value * 180.0d) / Math.PI;
        }
    }
}