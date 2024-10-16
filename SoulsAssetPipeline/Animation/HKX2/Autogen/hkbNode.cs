using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
	/// An enumeration of some of the Behavior node types.
	/// Some deprecated nodes have type HKB_NODE_TYPE_OTHER_GENERATOR or HKB_NODE_TYPE_OTHER_MODIFIER.
	public enum hkbNodeType
	{
		HKB_NODE_TYPE_INVALID = 0,

		// Generators - must be contiguous and include Transition Effects, which must also be contiguous.
		HKB_NODE_TYPE_FIRST_GENERATOR,
		HKB_NODE_TYPE_BEHAVIOR_GRAPH = HKB_NODE_TYPE_FIRST_GENERATOR,
		HKB_NODE_TYPE_BEHAVIOR_REFERENCE_GENERATOR,
		HKB_NODE_TYPE_BLENDER_GENERATOR,
		HKB_NODE_TYPE_CLIP_GENERATOR,
		HKB_NODE_TYPE_MANUAL_SELECTOR_GENERATOR,
		HKB_NODE_TYPE_MODIFIER_GENERATOR,
		HKB_NODE_TYPE_REFERENCE_POSE_GENERATOR,
		HKB_NODE_TYPE_STATE_MACHINE,
		HKB_NODE_TYPE_SCRIPT_GENERATOR,
		HKB_NODE_TYPE_LAYER_GENERATOR,

		// You can add your own custom generator types here if you implement them on SPU.
		// You must also update hkbSpuBehaviorUtils::s_generatorSize.
		// HKB_NODE_TYPE_USER_SPU_GENERATOR_0,

		HKB_NODE_TYPE_END_OF_SPU_GENERATORS,

		// the rest of the generators don't run on SPU

		HKB_NODE_TYPE_DOCKING_GENERATOR = HKB_NODE_TYPE_END_OF_SPU_GENERATORS,
		HKB_NODE_TYPE_PARAMETRIC_MOTION_GENERATOR,
		HKB_NODE_TYPE_PIN_BONE_GENERATOR,
		HKB_NODE_TYPE_OTHER_GENERATOR,

		// This is where you can define your custom generator types if they don't run on SPU.
		// HKB_NODE_TYPE_USER_GENERATOR_0

		// Transition Effects (they are also generators) - Must be contiguous.
		HKB_NODE_TYPE_FIRST_TRANSITION_EFFECT = 48,
		HKB_NODE_TYPE_BLENDING_TRANSITION_EFFECT = HKB_NODE_TYPE_FIRST_TRANSITION_EFFECT,
		HKB_NODE_TYPE_GENERATOR_TRANSITION_EFFECT,

		// You can add your own custom transition effect types here if you implement them on SPU.
		// You must also update hkbSpuBehaviorUtils::s_transitionEffectSize.
		// HKB_NODE_TYPE_USER_SPU_TRANSITION_EFFECT_0,

		HKB_NODE_TYPE_END_OF_SPU_TRANSITION_EFFECTS,

		// The rest of the transition effects don't run on SPU.

		// This is where you can define your custom transition effect types if they don't run on SPU.
		// HKB_NODE_TYPE_USER_TRANSITION_EFFECT_0

		// Modifiers - Must be contiguous.
		HKB_NODE_TYPE_FIRST_MODIFIER = 64,
		HKB_NODE_TYPE_ATTACHMENT_MODIFIER = HKB_NODE_TYPE_FIRST_MODIFIER,
		HKB_NODE_TYPE_ATTRIBUTE_MODIFIER,
		HKB_NODE_TYPE_CHARACTER_CONTROLLER_MODIFIER,
		HKB_NODE_TYPE_COMBINE_TRANSFORMS_MODIFIER,
		HKB_NODE_TYPE_COMPUTE_DIRECTION_MODIFIER,
		HKB_NODE_TYPE_COMPUTE_ROTATION_FROM_AXIS_ANGLE_MODIFIER,
		HKB_NODE_TYPE_COMPUTE_ROTATION_TO_TARGET_MODIFIER,
		HKB_NODE_TYPE_DAMPING_MODIFIER,
		HKB_NODE_TYPE_DELAYED_MODIFIER,
		HKB_NODE_TYPE_EVALUATE_EXPRESSION_MODIFIER,
		HKB_NODE_TYPE_EVENTS_FROM_RANGE_MODIFIER,
		HKB_NODE_TYPE_EVENT_DRIVEN_MODIFIER,
		HKB_NODE_TYPE_FOOT_IK_CONTROLS_MODIFIER,
		HKB_NODE_TYPE_GET_WORLD_FROM_MODEL_MODIFIER,
		HKB_NODE_TYPE_HAND_IK_CONTROLS_MODIFIER,
		HKB_NODE_TYPE_KEYFRAME_BONES_MODIFIER,
		HKB_NODE_TYPE_LOOK_AT_MODIFIER,
		HKB_NODE_TYPE_MIRROR_MODIFIER,
		HKB_NODE_TYPE_MODIFIER_LIST,
		HKB_NODE_TYPE_MOVE_BONE_ATTACHMENT_MODIFIER,
		HKB_NODE_TYPE_MOVE_CHARACTER_MODIFIER,
		HKB_NODE_TYPE_POWERED_RAGDOLL_CONTROLS_MODIFIER,
		HKB_NODE_TYPE_RIGID_BODY_RAGDOLL_CONTROLS_MODIFIER,
		HKB_NODE_TYPE_ROTATE_CHARACTER_MODIFIER,
		HKB_NODE_TYPE_SET_WORLD_FROM_MODEL_MODIFIER,
		HKB_NODE_TYPE_TIMER_MODIFIER,
		HKB_NODE_TYPE_TRANSFORM_VECTOR_MODIFIER,
		HKB_NODE_TYPE_TWIST_MODIFIER,

		// You can add your own custom modifier types here if you implement them on SPU.
		// You must also update hkbSpuBehaviorUtils::s_modifierSize.
		// HKB_NODE_TYPE_USER_SPU_MODIFIER_0,

		HKB_NODE_TYPE_END_OF_SPU_MODIFIERS,

		// the rest of the modifiers don't run on SPU

		HKB_NODE_TYPE_DETECT_CLOSE_TO_GROUND_MODIFIER = HKB_NODE_TYPE_END_OF_SPU_MODIFIERS,
		HKB_NODE_TYPE_EVALUATE_HANDLE_MODIFIER,
		HKB_NODE_TYPE_GET_HANDLE_ON_BONE_MODIFIER,
		HKB_NODE_TYPE_GET_UP_MODIFIER,
		HKB_NODE_TYPE_JIGGLER_MODIFIER,
		HKB_NODE_TYPE_SENSE_HANDLE_MODIFIER,
		HKB_NODE_TYPE_SEQUENCE,
		HKB_NODE_TYPE_AI_STEERING_MODIFIER,
		HKB_NODE_TYPE_AI_CONTROL_MODIFIER,
		HKB_NODE_TYPE_OTHER_MODIFIER,

		// This is where you can define your custom modifier types if they don't run on SPU.
		// HKB_NODE_TYPE_USER_MODIFIER_0
	};

	public enum GetChildrenFlagBits
    {
        FLAG_ACTIVE_ONLY = 1,
        FLAG_GENERATORS_ONLY = 4,
        FLAG_IGNORE_REFERENCED_BEHAVIORS = 8,
    }
    
    public enum CloneState
    {
        CLONE_STATE_DEFAULT = 0,
        CLONE_STATE_TEMPLATE = 1,
        CLONE_STATE_CLONE = 2,
    }
    
    public enum TemplateOrClone
    {
        NODE_IS_TEMPLATE = 0,
        NODE_IS_CLONE = 1,
    }
    
    public partial class hkbNode : hkbBindable
    {
        public override uint Signature { get => 146023711; }
        
        public ulong m_userData;
        public string m_name;
        public ushort m_id;
        public CloneState m_cloneState;
		public hkbNodeType m_type;
		public hkReflectDetailOpaque m_nodeInfo;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_userData = br.ReadUInt64();
            m_name = des.ReadStringPointer(br);
            br.ReadUInt64();
            br.ReadUInt64();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(m_userData);
            s.WriteStringPointer(bw, m_name);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
        }
    }
}
