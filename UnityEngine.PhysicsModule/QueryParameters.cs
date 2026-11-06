using System;

namespace UnityEngine
{
	// Token: 0x02000041 RID: 65
	public struct QueryParameters
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x00006BC9 File Offset: 0x00004DC9
		public QueryParameters(int layerMask = -5, bool hitMultipleFaces = false, QueryTriggerInteraction hitTriggers = QueryTriggerInteraction.UseGlobal, bool hitBackfaces = false)
		{
			this.layerMask = layerMask;
			this.hitMultipleFaces = hitMultipleFaces;
			this.hitTriggers = hitTriggers;
			this.hitBackfaces = hitBackfaces;
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00006BE9 File Offset: 0x00004DE9
		public static QueryParameters Default
		{
			get
			{
				return new QueryParameters(-5, false, QueryTriggerInteraction.UseGlobal, false);
			}
		}

		// Token: 0x04000102 RID: 258
		public int layerMask;

		// Token: 0x04000103 RID: 259
		public bool hitMultipleFaces;

		// Token: 0x04000104 RID: 260
		public QueryTriggerInteraction hitTriggers;

		// Token: 0x04000105 RID: 261
		public bool hitBackfaces;
	}
}
