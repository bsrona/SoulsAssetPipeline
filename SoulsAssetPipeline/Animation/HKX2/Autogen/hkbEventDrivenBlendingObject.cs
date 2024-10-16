using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkbEventDrivenBlendingObject : IHavokObject
    {
        public virtual uint Signature { get => 3765471744; }
        
        public float m_weight;
        public float m_fadeInDuration;
        public float m_fadeOutDuration;
        public int m_onEventId;
        public int m_offEventId;
        public bool m_onByDefault;
        public bool m_forceFullFadeDurations;
        public BlendCurve m_fadeInOutCurve;
        public hkbEventDrivenBlendingObjectInternalState m_internalState;
        
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
