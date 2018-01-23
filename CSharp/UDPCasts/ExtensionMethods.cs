using System;

namespace UDPCasts
{
    public static class ExtensionMethods
    {
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