using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x0200004E RID: 78
	internal sealed class EntityArchetypeDebugView
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000ACE4 File Offset: 0x00008EE4
		public EntityArchetypeDebugView(EntityArchetype entityArchetype)
		{
			this.m_EntityArchetype = entityArchetype;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public unsafe ComponentType[] ComponentTypes
		{
			get
			{
				Archetype* archetype = this.m_EntityArchetype.Archetype;
				if (archetype == null)
				{
					return null;
				}
				int typesCount = archetype->TypesCount;
				ComponentType[] array = new ComponentType[typesCount];
				for (int i = 0; i < typesCount; i++)
				{
					array[i] = archetype->Types[i].ToComponentType();
				}
				return array;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000AD4C File Offset: 0x00008F4C
		public unsafe EntityArchetypeDebugView.ChunkReport ChunkUtilization
		{
			get
			{
				Archetype* archetype = this.m_EntityArchetype.Archetype;
				if (archetype == null)
				{
					return default(EntityArchetypeDebugView.ChunkReport);
				}
				int chunkCount = this.m_EntityArchetype.ChunkCount;
				if (chunkCount <= 0)
				{
					return default(EntityArchetypeDebugView.ChunkReport);
				}
				float num = (float)archetype->ChunkCapacity;
				EntityArchetypeDebugView.ChunkReport chunkReport = new EntityArchetypeDebugView.ChunkReport
				{
					PerChunk = new List<float>(chunkCount),
					WorstUtilization = (float)archetype->Chunks[0].Count / num
				};
				for (int i = 0; i < chunkCount; i++)
				{
					float num2 = (float)archetype->Chunks[i].Count / num;
					chunkReport.PerChunk.Add(num2);
					chunkReport.AvgUtilization += num2;
					if (num2 < chunkReport.WorstUtilization)
					{
						chunkReport.WorstUtilization = num2;
					}
				}
				chunkReport.AvgUtilization /= (float)chunkCount;
				return chunkReport;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000AE35 File Offset: 0x00009035
		public List<ArchetypeChunk> ArchetypeChunks
		{
			get
			{
				return new List<ArchetypeChunk>();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000AE3C File Offset: 0x0000903C
		public unsafe int[] Offsets
		{
			get
			{
				Archetype* archetype = this.m_EntityArchetype.Archetype;
				if (archetype == null)
				{
					return null;
				}
				int[] array = new int[archetype->TypesCount];
				Marshal.Copy((IntPtr)((void*)archetype->Offsets), array, 0, archetype->TypesCount);
				return array;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000AE84 File Offset: 0x00009084
		public unsafe int[] SizeOfs
		{
			get
			{
				Archetype* archetype = this.m_EntityArchetype.Archetype;
				if (archetype == null)
				{
					return null;
				}
				int[] array = new int[archetype->TypesCount];
				Marshal.Copy((IntPtr)((void*)archetype->SizeOfs), array, 0, archetype->TypesCount);
				return array;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000AECC File Offset: 0x000090CC
		public unsafe ComponentType[] TypesInMemoryOrder
		{
			get
			{
				Archetype* archetype = this.m_EntityArchetype.Archetype;
				if (archetype == null)
				{
					return new ComponentType[0];
				}
				ComponentType[] array = new ComponentType[archetype->TypesCount];
				for (int i = 0; i < archetype->TypesCount; i++)
				{
					int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
					array[i] = archetype->Types[num].ToComponentType();
				}
				return array;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000AF38 File Offset: 0x00009138
		public unsafe Dictionary<TypeIndex, int> TypeMemoryOrderIndex
		{
			get
			{
				Archetype* archetype = this.m_EntityArchetype.Archetype;
				if (archetype == null)
				{
					return new Dictionary<TypeIndex, int>(0);
				}
				Dictionary<TypeIndex, int> dictionary = new Dictionary<TypeIndex, int>(archetype->TypesCount);
				for (int i = 0; i < archetype->TypesCount; i++)
				{
					dictionary[archetype->Types[i].TypeIndex] = archetype->TypeIndexInArchetypeToMemoryOrderIndex[i];
				}
				return dictionary;
			}
		}

		// Token: 0x04000093 RID: 147
		private EntityArchetype m_EntityArchetype;

		// Token: 0x0200004F RID: 79
		public struct ChunkReport
		{
			// Token: 0x0600022C RID: 556 RVA: 0x0000AFA2 File Offset: 0x000091A2
			public override string ToString()
			{
				return string.Format("{0}%", this.AvgUtilization * 100f);
			}

			// Token: 0x04000094 RID: 148
			public float AvgUtilization;

			// Token: 0x04000095 RID: 149
			public float WorstUtilization;

			// Token: 0x04000096 RID: 150
			public List<float> PerChunk;
		}
	}
}
