using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200019D RID: 413
	[CreateAssetMenu(fileName = "LootDefinition", menuName = "MoreMountains/Loot Definition")]
	public class MMLootTableGameObjectSO : ScriptableObject
	{
		// Token: 0x060009B4 RID: 2484 RVA: 0x0002CC38 File Offset: 0x0002AE38
		public virtual GameObject GetLoot()
		{
			MMLootGameObject loot = this.LootTable.GetLoot();
			if (loot == null)
			{
				return null;
			}
			return loot.Loot;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002CC50 File Offset: 0x0002AE50
		public virtual void ComputeWeights()
		{
			this.LootTable.ComputeWeights();
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002CC5D File Offset: 0x0002AE5D
		protected virtual void OnValidate()
		{
			this.ComputeWeights();
		}

		// Token: 0x04000B7C RID: 2940
		public MMLootTableGameObject LootTable;
	}
}
