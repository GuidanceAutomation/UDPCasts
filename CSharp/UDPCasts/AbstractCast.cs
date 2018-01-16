using System;

namespace UDPCasts
{
    [Serializable]
    public abstract class AbstractCast
    {
        private readonly byte tick;

        public AbstractCast(byte tick)
        {
            this.tick = tick;
        }

        public byte Tick { get { return tick; } }

        public override int GetHashCode()
        {
            return tick.GetHashCode();
        }

        public override string ToString()
        {
            return ToTickString();
        }

        public string ToTickString()
        {
            return string.Format("Tick: {0}", tick);
        }
    }
}