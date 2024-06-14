using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpMotionType : IHavokObject
    {
        public virtual uint Signature { get => 926589731; }
        
        public enum Enum
        {
            STATIC = 0,     ///< Body that will not change position or orientation unless manually set
            KEYFRAMED = 1,      ///< Body with infinite mass and inertia
            DYNAMIC = 2,        ///< Fully dynamic body
            NUM_TYPES = 3,
		}
        
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            br.ReadByte();
        }
        
        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteByte(0);
        }
    }
}
