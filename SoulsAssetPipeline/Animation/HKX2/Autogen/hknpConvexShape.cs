using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpConvexShape : hknpShape
    {
        public override uint Signature { get => 3371680013; }
        
        public hknpHalf m_maxAllowedPenetration;
        public List<Vector4> m_vertices;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_maxAllowedPenetration = new hknpHalf();
            m_maxAllowedPenetration.Read(des, br);
            // Read TYPE_RELARRAY
            br.ReadUInt64();
            br.ReadUInt32();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            m_maxAllowedPenetration.Write(s, bw);
            // Read TYPE_RELARRAY
            bw.WriteUInt64(0);
            bw.WriteUInt32(0);
        }
    }
}
