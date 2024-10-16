using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkReflectDetailOpaque : IHavokObject
    {
        public virtual uint Signature { get => 2493362767; }
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
        }
    }
}
