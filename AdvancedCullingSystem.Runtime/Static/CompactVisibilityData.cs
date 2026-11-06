using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200002C RID: 44
	public class CompactVisibilityData : IVisibilityData
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00006477 File Offset: 0x00004677
		public CompactVisibilityData(ICollection<int> indexes)
		{
			this._indexes = indexes.Select<int, ushort>((int i) => (ushort)i).ToArray<ushort>();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000064B0 File Offset: 0x000046B0
		public void SetVisible(CullingTarget[] allTargets)
		{
			for (int i = 0; i < this._indexes.Length; i++)
			{
				allTargets[(int)this._indexes[i]].SetVisible();
			}
		}

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private ushort[] _indexes;
	}
}
