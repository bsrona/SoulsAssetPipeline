using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkbEventDrivenBlendingObjectInternalState : IHavokObject
    {
        public virtual uint Signature { get => 3765471744; }

        public enum FadingState
        {

        }
        
        public float m_weight;
        public float m_timeElapsed;
        public float m_onFraction;
        public float m_onFractionOffset;
        public FadingState m_fadingState;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            br.ReadUInt64();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt64(0);
        }
    }
}
