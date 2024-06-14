using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkCompressedMassProperties : IHavokObject
    {
        public virtual uint Signature { get => 2596654817; }
        
        public hkPackedVector3 m_centerOfMass;
        public hkPackedVector3 m_inertia;
        public List<short> m_majorAxisSpace;
        public float m_mass;
        public float m_volume;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_centerOfMass = new hkPackedVector3();
            m_centerOfMass.Read(des, br);
            m_inertia = new hkPackedVector3();
            m_inertia.Read(des, br);
            m_majorAxisSpace = des.ReadInt16Array(br);
            m_mass = br.ReadSingle();
            m_volume = br.ReadSingle();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            m_centerOfMass.Write(s, bw);
            m_inertia.Write(s, bw);
            s.WriteInt16Array(bw, m_majorAxisSpace);
            bw.WriteSingle(m_mass);
            bw.WriteSingle(m_volume);
        }
    }
}
