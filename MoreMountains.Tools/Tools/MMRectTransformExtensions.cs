using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000EC RID: 236
	public static class MMRectTransformExtensions
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x00016F2A File Offset: 0x0001512A
		public static void MMSetLeft(this RectTransform rt, float left)
		{
			rt.offsetMin = new Vector2(left, rt.offsetMin.y);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00016F43 File Offset: 0x00015143
		public static void MMSetRight(this RectTransform rt, float right)
		{
			rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00016F5D File Offset: 0x0001515D
		public static void MMSetTop(this RectTransform rt, float top)
		{
			rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00016F77 File Offset: 0x00015177
		public static void MMSetBottom(this RectTransform rt, float bottom)
		{
			rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
		}
	}
}
