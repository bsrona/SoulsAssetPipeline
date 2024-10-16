using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public partial class hknpShapeType : IHavokObject
    {
        public virtual uint Signature { get => 926589731; }
        
        public enum Enum
        {
            CONVEX = 0,
            CONVEX_POLYTOPE = 1,
            SPHERE = 2,
            CAPSULE = 3,
            CYLINDER = 4,
            TRIANGLE = 5,
            BOX = 6,
            DEBRIS = 7,

            COMPOSITE = 8,
            COMPRESSED_MESH = 9,
            EXTERN_MESH = 10,
            FX_PIPELINE = 11,
            COMPOUND = 12,
            DISTANCE_FIELD_BASE = 13,
            HEIGHT_FIELD = 14,
            DISTANCE_FIELD = 15,

            PARTICLE_SYSTEM = 16,

            SCALED_CONVEX = 17,
            MASKED = 18,
            MASKED_COMPOUND = 19,
            LOD = 20,

            DUMMY = 21,

            USER_0 = 22,
            USER_1 = 23,
            USER_2 = 24,
            USER_3 = 25,
			USER_4 = 26,
			USER_5 = 27,
			USER_6 = 28,
			USER_7 = 29,
            
            NUM_SHAPE_TYPES = 30,

            INVALID = 31,
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
