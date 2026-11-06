using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200006E RID: 110
	internal class DecalEntityIndexer
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x00017439 File Offset: 0x00015639
		public bool IsValid(DecalEntity decalEntity)
		{
			return this.m_Entities.Count > decalEntity.index && this.m_Entities[decalEntity.index].version == decalEntity.version;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00017470 File Offset: 0x00015670
		public DecalEntity CreateDecalEntity(int arrayIndex, int chunkIndex)
		{
			if (this.m_FreeIndices.Count != 0)
			{
				int num = this.m_FreeIndices.Dequeue();
				int num2 = this.m_Entities[num].version + 1;
				this.m_Entities[num] = new DecalEntityIndexer.DecalEntityItem
				{
					arrayIndex = arrayIndex,
					chunkIndex = chunkIndex,
					version = num2
				};
				return new DecalEntity
				{
					index = num,
					version = num2
				};
			}
			int count = this.m_Entities.Count;
			int num3 = 1;
			this.m_Entities.Add(new DecalEntityIndexer.DecalEntityItem
			{
				arrayIndex = arrayIndex,
				chunkIndex = chunkIndex,
				version = num3
			});
			return new DecalEntity
			{
				index = count,
				version = num3
			};
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001754C File Offset: 0x0001574C
		public void DestroyDecalEntity(DecalEntity decalEntity)
		{
			this.m_FreeIndices.Enqueue(decalEntity.index);
			DecalEntityIndexer.DecalEntityItem decalEntityItem = this.m_Entities[decalEntity.index];
			decalEntityItem.version++;
			this.m_Entities[decalEntity.index] = decalEntityItem;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001759A File Offset: 0x0001579A
		public DecalEntityIndexer.DecalEntityItem GetItem(DecalEntity decalEntity)
		{
			return this.m_Entities[decalEntity.index];
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000175B0 File Offset: 0x000157B0
		public void UpdateIndex(DecalEntity decalEntity, int newArrayIndex)
		{
			DecalEntityIndexer.DecalEntityItem decalEntityItem = this.m_Entities[decalEntity.index];
			decalEntityItem.arrayIndex = newArrayIndex;
			decalEntityItem.version = decalEntity.version;
			this.m_Entities[decalEntity.index] = decalEntityItem;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000175F8 File Offset: 0x000157F8
		public void RemapChunkIndices(List<int> remaper)
		{
			for (int i = 0; i < this.m_Entities.Count; i++)
			{
				int num = remaper[this.m_Entities[i].chunkIndex];
				DecalEntityIndexer.DecalEntityItem decalEntityItem = this.m_Entities[i];
				decalEntityItem.chunkIndex = num;
				this.m_Entities[i] = decalEntityItem;
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00017655 File Offset: 0x00015855
		public void Clear()
		{
			this.m_Entities.Clear();
			this.m_FreeIndices.Clear();
		}

		// Token: 0x04000300 RID: 768
		private List<DecalEntityIndexer.DecalEntityItem> m_Entities = new List<DecalEntityIndexer.DecalEntityItem>();

		// Token: 0x04000301 RID: 769
		private Queue<int> m_FreeIndices = new Queue<int>();

		// Token: 0x0200017E RID: 382
		public struct DecalEntityItem
		{
			// Token: 0x04000A25 RID: 2597
			public int chunkIndex;

			// Token: 0x04000A26 RID: 2598
			public int arrayIndex;

			// Token: 0x04000A27 RID: 2599
			public int version;
		}
	}
}
