using System;
using Pathfinding.RVO;
using Unity.Collections;
using Unity.Entities;

namespace Pathfinding.ECS.RVO
{
	// Token: 0x020003B2 RID: 946
	[WriteGroup(typeof(ResolvedMovement))]
	public readonly struct AgentIndex : ICleanupComponentData, IComponentData, IQueryTypeParameter
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x0007FDB9 File Offset: 0x0007DFB9
		internal int Index
		{
			get
			{
				return this.packedAgentIndex & 16777215;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0007FDC7 File Offset: 0x0007DFC7
		private int Version
		{
			get
			{
				return this.packedAgentIndex & 2130706432;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x0007FDD5 File Offset: 0x0007DFD5
		internal bool Valid
		{
			get
			{
				return (this.packedAgentIndex & int.MinValue) == 0;
			}
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0007FDE6 File Offset: 0x0007DFE6
		internal AgentIndex(int packedAgentIndex)
		{
			this.packedAgentIndex = packedAgentIndex;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0007FDEF File Offset: 0x0007DFEF
		internal AgentIndex(int version, int index)
		{
			version <<= 24;
			this.packedAgentIndex = (version & 2130706432) | (index & 16777215);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0007FE0C File Offset: 0x0007E00C
		internal AgentIndex WithIncrementedVersion()
		{
			return new AgentIndex((((this.packedAgentIndex & 2130706432) + 16777216) & 2130706432) | this.Index);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0007FE32 File Offset: 0x0007E032
		internal AgentIndex WithDeleted()
		{
			return new AgentIndex(this.packedAgentIndex | int.MinValue);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0007FE48 File Offset: 0x0007E048
		public bool Exists(ref SimulatorBurst.AgentData agentData)
		{
			int num;
			return this.TryGetIndex(ref agentData, out num);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0007FE5E File Offset: 0x0007E05E
		public bool TryGetIndex(ref SimulatorBurst.AgentData agentData, out int index)
		{
			return this.TryGetIndex(ref agentData.version, out index);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0007FE70 File Offset: 0x0007E070
		public bool TryGetIndex(ref NativeArray<AgentIndex> agentDataVersions, out int index)
		{
			int index2 = this.Index;
			index = -1;
			if (!agentDataVersions.IsCreated)
			{
				return false;
			}
			if (index2 >= agentDataVersions.Length)
			{
				return false;
			}
			if (agentDataVersions[index2].Version != this.Version)
			{
				return false;
			}
			index = index2;
			return true;
		}

		// Token: 0x04000FC4 RID: 4036
		private const int DeletedBit = -2147483648;

		// Token: 0x04000FC5 RID: 4037
		private const int IndexMask = 16777215;

		// Token: 0x04000FC6 RID: 4038
		private const int VersionOffset = 24;

		// Token: 0x04000FC7 RID: 4039
		private const int VersionMask = 2130706432;

		// Token: 0x04000FC8 RID: 4040
		private readonly int packedAgentIndex;
	}
}
