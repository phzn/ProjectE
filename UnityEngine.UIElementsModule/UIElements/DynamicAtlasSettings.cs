using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200024A RID: 586
	[Serializable]
	public class DynamicAtlasSettings
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x0003CA11 File Offset: 0x0003AC11
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x0003CA19 File Offset: 0x0003AC19
		public int minAtlasSize
		{
			get
			{
				return this.m_MinAtlasSize;
			}
			set
			{
				this.m_MinAtlasSize = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0003CA22 File Offset: 0x0003AC22
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x0003CA2A File Offset: 0x0003AC2A
		public int maxAtlasSize
		{
			get
			{
				return this.m_MaxAtlasSize;
			}
			set
			{
				this.m_MaxAtlasSize = value;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0003CA33 File Offset: 0x0003AC33
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x0003CA3B File Offset: 0x0003AC3B
		public int maxSubTextureSize
		{
			get
			{
				return this.m_MaxSubTextureSize;
			}
			set
			{
				this.m_MaxSubTextureSize = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0003CA44 File Offset: 0x0003AC44
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x0003CA4C File Offset: 0x0003AC4C
		public DynamicAtlasFilters activeFilters
		{
			get
			{
				return this.m_ActiveFilters;
			}
			set
			{
				this.m_ActiveFilters = value;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0003CA55 File Offset: 0x0003AC55
		public static DynamicAtlasFilters defaultFilters
		{
			get
			{
				return DynamicAtlas.defaultFilters;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0003CA5C File Offset: 0x0003AC5C
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x0003CA64 File Offset: 0x0003AC64
		public DynamicAtlasCustomFilter customFilter
		{
			get
			{
				return this.m_CustomFilter;
			}
			set
			{
				this.m_CustomFilter = value;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0003CA6D File Offset: 0x0003AC6D
		public static DynamicAtlasSettings defaults
		{
			get
			{
				return new DynamicAtlasSettings
				{
					minAtlasSize = 64,
					maxAtlasSize = 4096,
					maxSubTextureSize = 64,
					activeFilters = DynamicAtlasSettings.defaultFilters,
					customFilter = null
				};
			}
		}

		// Token: 0x04000755 RID: 1877
		[HideInInspector]
		[SerializeField]
		private int m_MinAtlasSize;

		// Token: 0x04000756 RID: 1878
		[SerializeField]
		[HideInInspector]
		private int m_MaxAtlasSize;

		// Token: 0x04000757 RID: 1879
		[SerializeField]
		[HideInInspector]
		private int m_MaxSubTextureSize;

		// Token: 0x04000758 RID: 1880
		[HideInInspector]
		[SerializeField]
		private DynamicAtlasFilters m_ActiveFilters;

		// Token: 0x04000759 RID: 1881
		private DynamicAtlasCustomFilter m_CustomFilter;
	}
}
