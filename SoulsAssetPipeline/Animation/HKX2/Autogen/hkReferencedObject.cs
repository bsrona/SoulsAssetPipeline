using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkReferencedObject : hkBaseObject
    {
        public override uint Signature { get => 3071048009; }

        public hkPropertyBag m_propertyBag;        
        public ushort m_memSizeAndFlags;
        public ushort m_refCount;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_propertyBag = new hkPropertyBag();
            m_propertyBag.Read(des, br);
            m_memSizeAndFlags = br.ReadUInt16();
            m_refCount = br.ReadUInt16();
            br.ReadUInt64();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            m_propertyBag.Write(s, bw);
            bw.WriteUInt16(m_memSizeAndFlags);
            bw.WriteUInt16(m_refCount);
            bw.WriteUInt64(0);
        }
    }
}
