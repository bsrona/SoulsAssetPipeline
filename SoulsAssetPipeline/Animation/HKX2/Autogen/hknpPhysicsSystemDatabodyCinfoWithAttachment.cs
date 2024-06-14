using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpPhysicsSystemDatabodyCinfoWithAttachment : hknpBodyCinfo
    {
        public override uint Signature { get => 3092738443; }
        
        public int m_attachedBody;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_attachedBody = br.ReadInt32();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(m_attachedBody);
        }
    }
}
