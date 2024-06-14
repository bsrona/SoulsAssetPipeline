using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hkPackedVector3 : IHavokObject
    {
        public virtual uint Signature { get => 2191969155; }
        
        public List<short> m_values;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadInt16Array(br);
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteInt16Array(bw, m_values);
        }
    }
}
