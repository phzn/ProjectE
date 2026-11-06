using System;
using System.Collections.Generic;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;

namespace ProjectExhibitionist.Map
{
	// Token: 0x020009C8 RID: 2504
	[DisallowMultipleComponent]
	public sealed class MallRenderCombineBindings : MonoBehaviour
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004681 RID: 18049 RVA: 0x001AB003 File Offset: 0x001A9203
		public int RecipeVersion
		{
			get
			{
				return this.recipeVersion;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004682 RID: 18050 RVA: 0x001AB00B File Offset: 0x001A920B
		public IReadOnlyList<MallRenderCombineBindings.LodState> LodStates
		{
			get
			{
				return this.lodStates;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x001AB013 File Offset: 0x001A9213
		public IReadOnlyList<MallRenderCombineBindings.DetailState> DetailStates
		{
			get
			{
				return this.detailStates;
			}
		}

		// Token: 0x0400461F RID: 17951
		[SerializeField]
		private List<MallRenderCombineBindings.LodState> lodStates = new List<MallRenderCombineBindings.LodState>();

		// Token: 0x04004620 RID: 17952
		[SerializeField]
		private List<MallRenderCombineBindings.DetailState> detailStates = new List<MallRenderCombineBindings.DetailState>();

		// Token: 0x04004621 RID: 17953
		[SerializeField]
		private int recipeVersion;

		// Token: 0x020009C9 RID: 2505
		[Serializable]
		public sealed class LodLevel
		{
			// Token: 0x04004622 RID: 17954
			public float transition;

			// Token: 0x04004623 RID: 17955
			public float fadeWidth;

			// Token: 0x04004624 RID: 17956
			public Renderer[] renderers;
		}

		// Token: 0x020009CA RID: 2506
		[Serializable]
		public sealed class LodState
		{
			// Token: 0x04004625 RID: 17957
			public LODGroup group;

			// Token: 0x04004626 RID: 17958
			public Vector3 referencePoint;

			// Token: 0x04004627 RID: 17959
			public float size;

			// Token: 0x04004628 RID: 17960
			public MallRenderCombineBindings.LodLevel[] levels;
		}

		// Token: 0x020009CB RID: 2507
		[Serializable]
		public sealed class DetailState
		{
			// Token: 0x04004629 RID: 17961
			public PoiStaticDetailBudget budget;

			// Token: 0x0400462A RID: 17962
			public PoiStaticDetailBudget.Cell[] cells;
		}
	}
}
