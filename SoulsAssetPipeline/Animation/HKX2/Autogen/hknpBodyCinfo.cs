using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpBodyCinfo : IHavokObject
    {
        public virtual uint Signature { get => 1754724297; }
        
        public hknpShape m_shape;
        public int m_flags;
        public short m_collisionCntrl;
        public uint m_collisionFilterInfo;
		public ushort m_materialId;
		public byte m_qualityId;
		public string m_name;
		public ulong m_userData;
		public hknpMotionType.Enum m_motionType;
		public Vector4 m_position;
		public Quaternion m_orientation;
		public Vector4 m_linearVelocity;
		public Vector4 m_angularVelocity;
		public float m_mass;
		public hknpRefMassDistribution m_massDistribution;
		public ushort m_motionPropertiesId;
		public hknpBodyId m_desiredBodyId;
		public uint m_motionId;
		public float m_collisionLookAheadDistance;
		public hkLocalFrame m_localFrame;
		//public uint m_reservedBodyId;
        //public byte m_spuFlags;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_shape = des.ReadClassPointer<hknpShape>(br);
            m_flags = br.ReadInt32();
            m_collisionCntrl = br.ReadInt16();
			m_collisionFilterInfo = br.ReadUInt32();
			m_materialId = br.ReadUInt16();
			m_qualityId = br.ReadByte();
			m_name = des.ReadStringPointer(br);
			m_userData = br.ReadUInt64();
			m_motionType = (hknpMotionType.Enum)br.ReadByte();
			m_position = des.ReadVector4(br);
			m_orientation = des.ReadQuaternion(br);
			m_linearVelocity = des.ReadVector4(br);
			m_angularVelocity = des.ReadVector4(br);
			m_mass = br.ReadSingle();
			m_massDistribution = des.ReadClassPointer<hknpRefMassDistribution>(br);
			m_motionPropertiesId = br.ReadUInt16();
			m_desiredBodyId = new hknpBodyId();
			m_desiredBodyId.Read(des, br);
			m_motionId = br.ReadUInt32();
			m_collisionLookAheadDistance = br.ReadSingle();
			m_localFrame = des.ReadClassPointer<hkLocalFrame>(br);
			//m_reservedBodyId = br.ReadUInt32();
            br.ReadByte();
            //m_spuFlags = br.ReadByte();
            br.ReadUInt32();
            br.ReadUInt16();
            br.ReadByte();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer<hknpShape>(bw, m_shape);
            bw.WriteInt32(m_flags);
            bw.WriteInt16(m_collisionCntrl);
			bw.WriteUInt32(m_collisionFilterInfo);
			bw.WriteUInt16(m_materialId);
			bw.WriteByte(m_qualityId);
			s.WriteStringPointer(bw, m_name);
			bw.WriteUInt64(m_userData);
			bw.WriteByte((byte)m_motionType);
			s.WriteVector4(bw, m_position);
			s.WriteQuaternion(bw, m_orientation);
			s.WriteVector4(bw, m_linearVelocity);
			s.WriteVector4(bw, m_angularVelocity);
			bw.WriteSingle(m_mass);
			s.WriteClassPointer<hknpRefMassDistribution>(bw, m_massDistribution);
			bw.WriteUInt16(m_motionPropertiesId);
			m_desiredBodyId.Write(s, bw);
			bw.WriteUInt32(m_motionId);
            bw.WriteByte(0);
            bw.WriteSingle(m_collisionLookAheadDistance);
			s.WriteClassPointer<hkLocalFrame>(bw, m_localFrame);
			//bw.WriteUInt32(m_reservedBodyId);
			//bw.WriteByte(m_spuFlags);
			bw.WriteUInt32(0);
            bw.WriteUInt16(0);
            bw.WriteByte(0);
        }
    }
}
