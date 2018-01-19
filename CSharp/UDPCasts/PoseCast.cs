using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace UDPCasts
{
    /// <summary>
    /// Represents the pose (x,y position and heading) of an object identified by an ip address.
    /// </summary>
    [Serializable]
    public class PoseCast : AbstractCast
    {
        private const int NUMBYTES = 14;

        private readonly byte[] bytes = new byte[NUMBYTES];

        public PoseCast(byte tick, byte[] bytes) 
            : base(tick)
        {
            bytes.CopyTo(bytes, 0);
        }

        public PoseCast(byte tick, IPAddress ipAddress, double x, double y, double headingRad)
            : base(tick)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }

            byte[] stateBytes = new byte[NUMBYTES];

            ipAddress.GetAddressBytes().CopyTo(stateBytes, 0); // 0-4 are ipAddress

            // Store positions in mm
            BitConverter.GetBytes((Int32)(x * 100)).CopyTo(stateBytes, 4);
            BitConverter.GetBytes((Int32)(y * 100)).CopyTo(stateBytes, 8);

            // Store heading
            double wrapped = headingRad.PiWrap();
            double headingDeg = wrapped.RadToDeg();

            short shortValue = (short)(headingDeg * 100);
            BitConverter.GetBytes(shortValue).CopyTo(stateBytes, 12);

            stateBytes.CopyTo(bytes, 0);
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
    }
}
