using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkpAngLimitConstraintAtom : hkpConstraintAtom
    {
        public override uint Signature { get => 29728989; }
        
        public byte m_isEnabled;
        public byte m_limitAxis;
		public byte m_cosineAxis;
		public float m_minAngle;
        public float m_maxAngle;
        public float m_angularLimitsTauFactor;
		public float m_angularLimitsDampFactor;

		public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_isEnabled = br.ReadByte();
            m_limitAxis = br.ReadByte();
			m_cosineAxis = br.ReadByte();
			m_minAngle = br.ReadSingle();
            m_maxAngle = br.ReadSingle();
            m_angularLimitsTauFactor = br.ReadSingle();
			m_angularLimitsDampFactor = br.ReadSingle();
		}

		public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(m_isEnabled);
            bw.WriteByte(m_limitAxis);
			bw.WriteByte(m_cosineAxis);
			bw.WriteSingle(m_minAngle);
            bw.WriteSingle(m_maxAngle);
            bw.WriteSingle(m_angularLimitsTauFactor);
			bw.WriteSingle(m_angularLimitsDampFactor);
		}
	}
}
