using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
	public enum _DockingType
	{
	}

	public partial class CustomDockingGenerator : hkbDockingGenerator
    {
        public override uint Signature { get => 2140111424; }

        public _DockingType m_titleDokingType;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
        }
    }
}
