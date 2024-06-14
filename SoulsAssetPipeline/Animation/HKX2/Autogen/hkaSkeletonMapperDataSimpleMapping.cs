using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkaSkeletonMapperDataSimpleMapping : IHavokObject
    {
        public virtual uint Signature { get => 872799946; }
        
        public short m_boneA;
        public short m_boneB;
        public hkQsTransform m_aFromBTransform;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_boneA = br.ReadInt16();
            m_boneB = br.ReadInt16();
            br.ReadUInt64();
            br.ReadUInt32();
            m_aFromBTransform = new hkQsTransform();
            m_aFromBTransform.Read(des, br);
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(m_boneA);
            bw.WriteInt16(m_boneB);
            bw.WriteUInt64(0);
            bw.WriteUInt32(0);
            m_aFromBTransform.Write(s, bw);
        }
    }
}
