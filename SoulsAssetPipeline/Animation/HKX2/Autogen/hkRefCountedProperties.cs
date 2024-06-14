using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public enum ReferenceCountHandling
    {
        REFERENCE_COUNT_INCREMENT = 0,
        REFERENCE_COUNT_IGNORE = 1,
    }
    
    public partial class hkRefCountedProperties : hkReferencedObject
    {
        public override uint Signature { get => 2086094951; }
        
        public List<hkRefCountedPropertiesEntry> m_entries;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_entries = des.ReadClassArray<hkRefCountedPropertiesEntry>(br);
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray<hkRefCountedPropertiesEntry>(bw, m_entries);
        }
    }
}
