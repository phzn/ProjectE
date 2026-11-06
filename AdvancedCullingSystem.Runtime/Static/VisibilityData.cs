using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200002B RID: 43
	public class VisibilityData : IVisibilityData
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00006432 File Offset: 0x00004632
		public VisibilityData(ICollection<int> indexes)
		{
			this._indexes = indexes.ToArray<int>();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006448 File Offset: 0x00004648
		public void SetVisible(CullingTarget[] allTargets)
		{
			for (int i = 0; i < this._indexes.Length; i++)
			{
				allTargets[this._indexes[i]].SetVisible();
			}
		}

		// Token: 0x040000CC RID: 204
		[SerializeField]
		private int[] _indexes;
	}
}
