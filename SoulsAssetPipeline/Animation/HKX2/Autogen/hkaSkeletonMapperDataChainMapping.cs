using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkaSkeletonMapperDataChainMapping : IHavokObject
    {
        public virtual uint Signature { get => 2770925519; }
        
        public short m_startBoneA;
        public short m_endBoneA;
        public short m_startBoneB;
        public short m_endBoneB;
        public hkQsTransform m_startAFromBTransform;
        public hkQsTransform m_endAFromBTransform;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_startBoneA = br.ReadInt16();
            m_endBoneA = br.ReadInt16();
            m_startBoneB = br.ReadInt16();
            m_endBoneB = br.ReadInt16();
            br.ReadUInt64();
            m_startAFromBTransform = new hkQsTransform();
            m_startAFromBTransform.Read(des, br);
            m_endAFromBTransform = new hkQsTransform();
			m_endAFromBTransform.Read(des, br);
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(m_startBoneA);
            bw.WriteInt16(m_endBoneA);
            bw.WriteInt16(m_startBoneB);
            bw.WriteInt16(m_endBoneB);
            bw.WriteUInt64(0);
            m_startAFromBTransform.Write(s, bw);
            m_endAFromBTransform.Write(s, bw);
        }
    }
}
