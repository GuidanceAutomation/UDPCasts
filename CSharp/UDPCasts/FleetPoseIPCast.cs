using System;
using System.Collections.Generic;
using System.Linq;

namespace UDPCasts
{
    /// <summary>
    /// A cast the represents a collection of objects uniquely identified by an IP v4 address
    /// with a pose (position and orientation).
    /// </summary>
    [Serializable]
    public class FleetPoseIPCast : ByteArrayCast
    {
        private List<PoseIPStruct> dataSet = new List<PoseIPStruct>();

        public FleetPoseIPCast(byte tick, byte[][] byteArray)
            : base(tick, byteArray)
        {
            foreach (byte[] bytes in byteArray)
            {
                PoseIPStruct data = new PoseIPStruct(bytes);
                dataSet.Add(data);
            }
        }

        public IEnumerable<PoseIPStruct> DataSet { get { return dataSet.ToList(); } }
    }
}