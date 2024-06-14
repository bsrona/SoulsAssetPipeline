using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
   public partial class hknpBodyId : IHavokObject
    {
        public virtual uint Signature { get => 42283992; }
        
        public uint m_serialAndIndex;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_serialAndIndex = br.ReadUInt32();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(m_serialAndIndex);
        }
    }
}
