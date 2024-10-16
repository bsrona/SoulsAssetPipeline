using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkcdStaticTreeCodec3Axis : IHavokObject
    {
        public virtual uint Signature { get => 1255292721; }
        
        public List<byte> m_xyz;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_xyz = des.ReadByteArray(br);
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteByteArray(bw, m_xyz);
        }
    }
}
