using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpRefMassDistribution : hkReferencedObject
    {
        public override uint Signature { get => 4089468224; }
        
        public hknpMassDistribution m_massDistribution;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_massDistribution = new hknpMassDistribution();
            m_massDistribution.Read(des, br);
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            m_massDistribution.Write(s, bw);
        }
    }
}
