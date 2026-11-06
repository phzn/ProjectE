using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x0200006A RID: 106
	public static class UnityRectExtensions
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x00018730 File Offset: 0x00016930
		public static Rect Inflated(this Rect r, Vector2 delta)
		{
			return new Rect(r.xMin - delta.x, r.yMin - delta.y, r.width + delta.x * 2f, r.height + delta.y * 2f);
		}
	}
}
