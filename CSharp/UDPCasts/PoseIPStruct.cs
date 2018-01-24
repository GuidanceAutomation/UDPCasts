using System;
using System.Linq;
using System.Net;

namespace UDPCasts
{
    [Serializable]
    /// <summary>
    /// Represents the a pose (position and orientation) of an object uniquely identified
    /// by an IP v4 address.
    /// </summary>
    public struct PoseIPStruct
    {
        public static readonly int NUMBYTES = 14;

        private readonly byte[] bytes;

        public PoseIPStruct(byte[] byteData)
        {
            if (byteData.Count() != NUMBYTES)
            {
                throw new ArgumentOutOfRangeException("byteData");
            }

            bytes = new byte[NUMBYTES];
            byteData.CopyTo(bytes, 0);
        }

        public PoseIPStruct(IPAddress ipAddress, double x, double y, double headingRad)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }

            bytes = new byte[NUMBYTES];

            ipAddress.GetAddressBytes().CopyTo(bytes, 0); // 0-4 are ipAddress

            // Store positions in mm
            BitConverter.GetBytes((Int32)(x * 100)).CopyTo(bytes, 4);
            BitConverter.GetBytes((Int32)(y * 100)).CopyTo(bytes, 8);

            // Store heading in deg
            double wrapped = headingRad.PiWrap();
            double headingDeg = wrapped.RadToDeg();

            short shortValue = (short)(headingDeg * 100);
            BitConverter.GetBytes(shortValue).CopyTo(bytes, 12);
        }

        public byte[] Bytes { get { return bytes; } }

        /// <summary>
        /// Heading in radians.
        /// </summary>
        public double Heading
        {
            get
            {
                int headingDeg = BitConverter.ToInt16(bytes, 12);
                if (headingDeg == short.MinValue)
                {
                    return double.NaN;
                }

                double scaledBack = ((double)headingDeg) / 100.0;
                return scaledBack.DegToRad();
            }
        }

        /// <summary>
        /// IPv4 address.
        /// </summary>
        public IPAddress IPAddress
        {
            get { return new IPAddress(bytes.Take(4).ToArray()); }
        }

        /// <summary>
        /// X position in meters.
        /// </summary>
        public double X
        {
            get
            {
                int xmm = BitConverter.ToInt32(bytes, 4);
                return (xmm == int.MinValue) ? double.NaN : ((double)xmm) / 100.0;
            }
        }

        /// <summary>
        /// Y position in meters.
        /// </summary>
        public double Y
        {
            get
            {
                int ymm = BitConverter.ToInt32(bytes, 8);
                return (ymm == int.MinValue) ? double.NaN : ((double)ymm) / 100.0;
            }
        }
    }
}