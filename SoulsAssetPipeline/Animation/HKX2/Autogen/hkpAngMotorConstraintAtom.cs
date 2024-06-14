using Havoc.Objects;
using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkpAngMotorConstraintAtom : hkpConstraintAtom
    {
        public override uint Signature { get => 1112114262; }
        
        public bool m_isEnabled;
        public byte m_motorAxis;
        public short m_initializedOffset;
		public short m_previousTargetAngleOffset;
        public hkpConstraintMotor m_motor;
		public float m_targetAngle;
		public short m_correspondingAngLimitSolverResultOffset;

		public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_isEnabled = br.ReadBoolean();
            m_motorAxis = br.ReadByte();
            m_initializedOffset = br.ReadInt16();
			m_previousTargetAngleOffset = br.ReadInt16();
			br.ReadUInt64();
            m_motor = des.ReadClassPointer<hkpConstraintMotor>(br);
			m_targetAngle = br.ReadSingle();
			m_correspondingAngLimitSolverResultOffset = br.ReadInt16();
			br.ReadUInt64();
            br.ReadUInt64();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(m_isEnabled);
            bw.WriteByte(m_motorAxis);
            bw.WriteInt16(m_initializedOffset);
			bw.WriteInt16(m_previousTargetAngleOffset);
			bw.WriteUInt64(0);
            s.WriteClassPointer<hkpConstraintMotor>(bw, m_motor);
			bw.WriteSingle(m_targetAngle);
			bw.WriteInt16(m_correspondingAngLimitSolverResultOffset);
			bw.WriteUInt64(0);
            bw.WriteUInt64(0);
        }
    }
}
