using System;

namespace UnityEngine.AI
{
	// Token: 0x02000016 RID: 22
	public struct NavMeshQueryFilter
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00002EDB File Offset: 0x000010DB
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00002EE3 File Offset: 0x000010E3
		internal float[] costs { readonly get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00002EEC File Offset: 0x000010EC
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00002EF4 File Offset: 0x000010F4
		public int areaMask { readonly get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00002EFD File Offset: 0x000010FD
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00002F05 File Offset: 0x00001105
		public int agentTypeID { readonly get; set; }

		// Token: 0x06000129 RID: 297 RVA: 0x00002F10 File Offset: 0x00001110
		public float GetAreaCost(int areaIndex)
		{
			bool flag = this.costs == null;
			float num;
			if (flag)
			{
				bool flag2 = areaIndex < 0 || areaIndex >= 32;
				if (flag2)
				{
					string text = string.Format("The valid range is [0:{0}]", 31);
					throw new IndexOutOfRangeException(text);
				}
				num = 1f;
			}
			else
			{
				num = this.costs[areaIndex];
			}
			return num;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00002F70 File Offset: 0x00001170
		public void SetAreaCost(int areaIndex, float cost)
		{
			bool flag = this.costs == null;
			if (flag)
			{
				this.costs = new float[32];
				for (int i = 0; i < 32; i++)
				{
					this.costs[i] = 1f;
				}
			}
			this.costs[areaIndex] = cost;
		}

		// Token: 0x04000039 RID: 57
		private const int k_AreaCostElementCount = 32;
	}
}
