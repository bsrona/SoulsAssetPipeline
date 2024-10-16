using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class CustomHandIkControlsModifier : hkbHandIkControlsModifier
    {
        public override uint Signature { get => 220736967; }
        
        public List<hkQsTransform> m_unmodifiedWristTransform;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_unmodifiedWristTransform = des.ReadClassArray<hkQsTransform>(br);
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray<hkQsTransform>(bw, m_unmodifiedWristTransform);
        }
    }
}
