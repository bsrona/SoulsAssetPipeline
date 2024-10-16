using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpCompositeShape : hknpShape
    {
        public override uint Signature { get => 314260463; }
        
        public hknpSparseCompactMapunsignedshort m_edgeWeldingMap;
        public uint m_shapeTagCodecInfo;
        public hkReferencedObject m_materialTable;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_edgeWeldingMap = new hknpSparseCompactMapunsignedshort();
            m_edgeWeldingMap.Read(des, br);
            m_shapeTagCodecInfo = br.ReadUInt32();
            des.ReadClassPointer<hkReferencedObject>(br);
            br.ReadUInt32();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            m_edgeWeldingMap.Write(s, bw);
            bw.WriteUInt32(m_shapeTagCodecInfo);
            s.WriteClassPointer<hkReferencedObject>(bw, m_materialTable);
            bw.WriteUInt32(0);
        }
    }
}
