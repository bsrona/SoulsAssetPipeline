using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpCompressedMeshShape : hknpCompositeShape
    {
        public override uint Signature { get => 1600181558; }
        
        public hknpCompressedMeshShapeData m_data;
        public hkBitField m_triangleIsInterior;
        public int m_numTriangles;
        public int m_numConvexShapes;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_data = des.ReadClassPointer<hknpCompressedMeshShapeData>(br);
            m_triangleIsInterior = new hkBitField();
            m_triangleIsInterior.Read(des, br);
            m_numTriangles = br.ReadInt32();
            m_numConvexShapes = br.ReadInt32();
            br.ReadUInt64();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer<hknpCompressedMeshShapeData>(bw, m_data);
            m_triangleIsInterior.Write(s, bw);
            bw.WriteInt32(m_numTriangles);
            bw.WriteInt32(m_numConvexShapes);
            bw.WriteUInt64(0);
        }
    }
}
