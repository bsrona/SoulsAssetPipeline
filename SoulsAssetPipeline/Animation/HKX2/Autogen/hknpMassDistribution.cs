using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpMassDistribution : IHavokObject
    {
        public virtual uint Signature { get => 4089468224; }
        
        public Vector4 m_centerOfMassAndVolume;
        public Quaternion m_majorAxisSpace;
        public Vector4 m_inertiaTensor;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_centerOfMassAndVolume = des.ReadVector4(br);
            m_majorAxisSpace = des.ReadQuaternion(br);
            m_inertiaTensor = des.ReadVector4(br);
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVector4(bw, m_centerOfMassAndVolume);
            s.WriteQuaternion(bw, m_majorAxisSpace);
            s.WriteVector4(bw, m_inertiaTensor);
        }
    }
}
