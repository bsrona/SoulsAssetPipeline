using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpBoxShape : hknpConvexPolytopeShape
    {
        public override uint Signature { get => 1621581644; }

        public Matrix4x4 m_obb;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);

            m_obb = des.ReadMatrix4(br);
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);

            s.WriteMatrix4(bw, m_obb);
        }
    }
}
