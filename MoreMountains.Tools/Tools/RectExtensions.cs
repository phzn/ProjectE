using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000EB RID: 235
	public static class RectExtensions
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x00016ED8 File Offset: 0x000150D8
		public static bool MMIntersects(this Rect thisRectangle, Rect otherRectangle)
		{
			return thisRectangle.x <= otherRectangle.xMax && thisRectangle.xMax >= otherRectangle.x && thisRectangle.y <= otherRectangle.yMax && thisRectangle.yMax >= otherRectangle.y;
		}
	}
}
