using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkpRagdollMotorConstraintAtom : hkpConstraintAtom
    {
        public override uint Signature { get => 2643776556; }
        
        public bool m_isEnabled;
        public short m_initializedOffset;
        public short m_previousTargetAnglesOffset;
        public Matrix4x4 m_target_bRca;
        public List<hkpConstraintMotor> m_motors;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_isEnabled = br.ReadBoolean();
            m_initializedOffset = br.ReadInt16();
			m_previousTargetAnglesOffset = br.ReadInt16();
			br.ReadUInt64();
            br.ReadUInt32();
            br.ReadByte();
            m_target_bRca = des.ReadMatrix3(br);
            m_motors = des.ReadClassPointerArray<hkpConstraintMotor>(br);
            br.ReadUInt64();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(m_isEnabled);
            bw.WriteInt16(m_initializedOffset);
			bw.WriteInt16(m_previousTargetAnglesOffset);
			bw.WriteUInt64(0);
            bw.WriteUInt32(0);
            bw.WriteByte(0);
            s.WriteMatrix3(bw, m_target_bRca);
            s.WriteClassPointerArray<hkpConstraintMotor>(bw, m_motors);
            bw.WriteUInt64(0);
        }
    }
}
