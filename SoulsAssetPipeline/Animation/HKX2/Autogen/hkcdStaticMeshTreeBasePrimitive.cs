using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkcdStaticMeshTreeBasePrimitive : IHavokObject
    {
        public virtual uint Signature { get => 1457139580; }
        
        public enum Type
        {
            INVALID = 0,
            TRIANGLE = 1,
            QUAD = 2,
            CUSTOM = 3,
            NUM_TYPES = 4,
        }
        
        public List<byte> m_indices;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_indices = des.ReadByteArray(br);
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteByteArray(bw, m_indices);
        }
    }
}
