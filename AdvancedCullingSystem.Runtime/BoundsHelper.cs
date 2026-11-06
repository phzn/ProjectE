using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem
{
	// Token: 0x02000007 RID: 7
	public static class BoundsHelper
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000029E7 File Offset: 0x00000BE7
		public static bool Contains(this Bounds bounds, Bounds target)
		{
			return bounds.Contains(target.min) && bounds.Contains(target.max);
		}
	}
}
