using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpConvexPolytopeShapeConnectivityEdge : IHavokObject
    {
        public virtual uint Signature { get => 4089468224; }
        
        public ushort m_faceIndex;
        public byte m_edgeIndex;
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            m_faceIndex = br.ReadUInt16();
            m_edgeIndex = br.ReadByte();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt16(m_faceIndex);
            bw.WriteByte(m_edgeIndex);
        }
    }
}
