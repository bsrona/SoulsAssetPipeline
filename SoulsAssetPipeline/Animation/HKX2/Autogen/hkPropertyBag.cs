using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkPropertyBag : IHavokObject
    {
        public virtual uint Signature { get => 3071048009; }

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
        }
    }
}
