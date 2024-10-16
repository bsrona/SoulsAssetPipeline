using SoulsFormats;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2
{
    public enum VariableMode
    {
        VARIABLE_MODE_DISCARD_WHEN_INACTIVE = 0,
        VARIABLE_MODE_MAINTAIN_VALUES_WHEN_INACTIVE = 1,
    }
    
    public partial class hkbBehaviorGraph : hkbGenerator
    {
        public override uint Signature { get => 4260214843; }
        
        public VariableMode m_variableMode;
        public hkbGenerator m_rootGenerator;
        public hkbBehaviorGraphData m_data;
        public List<hkReflectDetailOpaque> m_uniqueIdPool;
        public hkReflectDetailOpaque m_idToStateMachineTemplateMap;
        public hkReflectDetailOpaque m_mirroredExternalIdMap;
        public hkReflectDetailOpaque m_pseudoRandomGenerator;
        public hkAssetRefPtr m_template;
        public hkReflectDetailOpaque m_activeNodes;
        public hkReflectDetailOpaque m_globalTransitionData;
        public hkReflectDetailOpaque m_eventIdMap;
        public hkReflectDetailOpaque m_attributeIdMap;
        public hkReflectDetailOpaque m_variableIdMap;
        public hkReflectDetailOpaque m_characterPropertyIdMap;
        public hkReflectDetailOpaque m_animationIdMap;
        public hkReflectDetailOpaque m_variableValueSet;
        public hkReflectDetailOpaque m_nodeTemplateToCloneMap;
        public hkReflectDetailOpaque m_stateListenerTemplateToCloneMap;
        public List<hkReflectDetailOpaque> m_recentlyCreatedClones;
        public hkReflectDetailOpaque m_nodePartitionInfo;
        public int m_numIntermediateOutputs;
        public List<hkReflectDetailOpaque> m_intermediateOutputSizes;
        public List<hkReflectDetailOpaque> m_jobs;
        public List<hkReflectDetailOpaque> m_allPartitionMemory;
        public List<hkReflectDetailOpaque> m_internalToRootVariableIdMap;
        public List<hkReflectDetailOpaque> m_internalToCharacterCharacterPropertyIdMap;
        public List<hkReflectDetailOpaque> m_internalToRootAttributeIdMap;
        public ushort m_nextUniqueId;
        public bool m_isActive;
        public bool m_isLinked;
        public bool m_updateActiveNodes;
        public bool m_updateActiveNodesForEnable;
        public bool m_checkNodeValidity;
        public bool m_stateOrTransitionChanged;
        
        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            m_variableMode = (VariableMode)br.ReadSByte();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt32();
            br.ReadUInt16();
            br.ReadByte();
            m_rootGenerator = des.ReadClassPointer<hkbGenerator>(br);
            m_data = des.ReadClassPointer<hkbBehaviorGraphData>(br);
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
            br.ReadUInt64();
        }
        
        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte((sbyte)m_variableMode);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt32(0);
            bw.WriteUInt16(0);
            bw.WriteByte(0);
            s.WriteClassPointer<hkbGenerator>(bw, m_rootGenerator);
            s.WriteClassPointer<hkbBehaviorGraphData>(bw, m_data);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
            bw.WriteUInt64(0);
        }
    }
}
