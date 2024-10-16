using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public enum TriggerType
    {
        TRIGGER_TYPE_NONE = 0,
        TRIGGER_TYPE_BROAD_PHASE = 1,
        TRIGGER_TYPE_NARROW_PHASE = 2,
        TRIGGER_TYPE_CONTACT_SOLVER = 3,
    }
    
    public enum CombinePolicy
    {
        COMBINE_GEOMETRIC_MEAN = 0,
        COMBINE_MIN = 1,
        COMBINE_MAX = 2,
        COMBINE_ARITHMETIC_MEAN = 3,
    }
    
    public enum MassChangerCategory
    {
        MASS_CHANGER_IGNORE = 0,
        MASS_CHANGER_DEBRIS = 1,
        MASS_CHANGER_HEAVY = 2,
    }
    
    public partial class hknpMaterial : hkReferencedObject
    {
        public override uint Signature { get => 3083203150; }
        
        public string m_name;
        public uint m_isExclusive;
        public int m_flags;
        public TriggerType m_triggerType;
        public hkUFloat8 m_triggerManifoldTolerance;
        public hknpHalf m_dynamicFriction;
        public hknpHalf m_staticFriction;
        public hknpHalf m_restitution;
        public CombinePolicy m_frictionCombinePolicy;
        public CombinePolicy m_restitutionCombinePolicy;
        public float m_weldingTolerance;
        public float m_maxContactImpulse;
        public float m_fractionOfClippedImpulseToApply;
        public MassChangerCategory m_massChangerCategory;
        public hknpHalf m_massChangerHeavyObjectFactor;
        public hknpHalf m_softContactForceFactor;
        public hknpHalf m_softContactDampFactor;
        public hkUFloat8 m_softContactSeparationVelocity;
        public hknpSurfaceVelocity m_surfaceVelocity;
        public hknpHalf m_disablingCollisionsBetweenCvxCvxDynamicObjectsDistance;
        public ulong m_userData;
        public bool m_isShared;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_name = des.ReadStringPointer(br);
            m_isExclusive = br.ReadUInt32();
            m_flags = br.ReadInt32();
            m_triggerType = (TriggerType)br.ReadByte();
            m_triggerManifoldTolerance = new hkUFloat8();
            m_triggerManifoldTolerance.Read(des, br);
            m_dynamicFriction = new hknpHalf();
            m_dynamicFriction.Read(des, br);
            m_staticFriction = new hknpHalf();
            m_staticFriction.Read(des, br);
            m_restitution = new hknpHalf();
            m_restitution.Read(des, br);
            m_frictionCombinePolicy = (CombinePolicy)br.ReadByte();
            m_restitutionCombinePolicy = (CombinePolicy)br.ReadByte();
            m_weldingTolerance = br.ReadSingle();
            m_maxContactImpulse = br.ReadSingle();
            m_fractionOfClippedImpulseToApply = br.ReadSingle();
            m_massChangerCategory = (MassChangerCategory)br.ReadByte();
            br.ReadByte();
            m_massChangerHeavyObjectFactor = new hknpHalf();
            m_massChangerHeavyObjectFactor.Read(des, br);
            m_softContactForceFactor = new hknpHalf();
            m_softContactForceFactor.Read(des, br);
            m_softContactDampFactor = new hknpHalf();
            m_softContactDampFactor.Read(des, br);
            m_softContactSeparationVelocity = new hkUFloat8();
            m_softContactSeparationVelocity.Read(des, br);
            br.ReadUInt16();
            br.ReadByte();
            m_surfaceVelocity = des.ReadClassPointer<hknpSurfaceVelocity>(br);
            m_disablingCollisionsBetweenCvxCvxDynamicObjectsDistance = new hknpHalf();
            m_disablingCollisionsBetweenCvxCvxDynamicObjectsDistance.Read(des, br);
            br.ReadUInt32();
            br.ReadUInt16();
            m_userData = br.ReadUInt64();
            m_isShared = br.ReadBoolean();
            br.ReadUInt32();
            br.ReadUInt16();
            br.ReadByte();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, m_name);
            bw.WriteUInt32(m_isExclusive);
            bw.WriteInt32(m_flags);
            bw.WriteByte((byte)m_triggerType);
            m_triggerManifoldTolerance.Write(s, bw);
            m_dynamicFriction.Write(s, bw);
            m_staticFriction.Write(s, bw);
            m_restitution.Write(s, bw);
            bw.WriteByte((byte)m_frictionCombinePolicy);
            bw.WriteByte((byte)m_restitutionCombinePolicy);
            bw.WriteSingle(m_weldingTolerance);
            bw.WriteSingle(m_maxContactImpulse);
            bw.WriteSingle(m_fractionOfClippedImpulseToApply);
            bw.WriteByte((byte)m_massChangerCategory);
            bw.WriteByte(0);
            m_massChangerHeavyObjectFactor.Write(s, bw);
            m_softContactForceFactor.Write(s, bw);
            m_softContactDampFactor.Write(s, bw);
            m_softContactSeparationVelocity.Write(s, bw);
            bw.WriteUInt16(0);
            bw.WriteByte(0);
            s.WriteClassPointer<hknpSurfaceVelocity>(bw, m_surfaceVelocity);
            m_disablingCollisionsBetweenCvxCvxDynamicObjectsDistance.Write(s, bw);
            bw.WriteUInt32(0);
            bw.WriteUInt16(0);
            bw.WriteUInt64(m_userData);
            bw.WriteBoolean(m_isShared);
            bw.WriteUInt32(0);
            bw.WriteUInt16(0);
            bw.WriteByte(0);
        }
    }
}
