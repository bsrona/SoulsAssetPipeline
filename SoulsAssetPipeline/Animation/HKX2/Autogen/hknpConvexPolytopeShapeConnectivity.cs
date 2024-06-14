using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpConvexPolytopeShapeConnectivity : hkReferencedObject
    {
        public override uint Signature { get => 4089468224; }
        
        public List<hknpConvexPolytopeShapeConnectivityEdge> m_vertexEdges;
        public List<hknpConvexPolytopeShapeConnectivityEdge> m_faceLinks;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_vertexEdges = des.ReadClassArray<hknpConvexPolytopeShapeConnectivityEdge>(br);
            m_faceLinks = des.ReadClassArray<hknpConvexPolytopeShapeConnectivityEdge>(br);
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, m_vertexEdges);
			s.WriteClassArray(bw, m_faceLinks);
        }
    }
}
