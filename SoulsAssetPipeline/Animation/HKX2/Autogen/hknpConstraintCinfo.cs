using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpConstraintCinfo : IHavokObject
    {
        public virtual uint Signature { get => 1743427693; }
        
        public hkpConstraintData m_constraintData;
        public hknpBodyId m_bodyA;
        public hknpBodyId m_bodyB;
        public ushort m_flags;
        public string m_name;
        public hknpConstraintId m_desiredConstraintId;
        public hknpConstraintGroupId m_constraintGroupId;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_constraintData = des.ReadClassPointer<hkpConstraintData>(br);
            m_bodyA = new hknpBodyId();
            m_bodyA.Read(des, br);
            m_bodyB = new hknpBodyId();
            m_bodyB.Read(des, br);
            m_flags = br.ReadUInt16();
            m_name = des.ReadStringPointer(br);
            m_desiredConstraintId = new hknpConstraintId();
            m_desiredConstraintId.Read(des, br);
			m_constraintGroupId = new hknpConstraintGroupId();
			m_constraintGroupId.Read(des, br);
			br.ReadUInt32();
            br.ReadUInt16();
            br.ReadByte();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer<hkpConstraintData>(bw, m_constraintData);
            m_bodyA.Write(s, bw);
            m_bodyA.Write(s, bw);
            bw.WriteUInt16(m_flags);
            s.WriteStringPointer(bw, m_name);
            m_desiredConstraintId.Write(s, bw);
			m_constraintGroupId.Write(s, bw);
			bw.WriteUInt32(0);
            bw.WriteUInt16(0);
            bw.WriteByte(0);
        }
    }
}
