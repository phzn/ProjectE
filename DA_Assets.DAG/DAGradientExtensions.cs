using System;

namespace DA_Assets.DAG
{
	// Token: 0x02000004 RID: 4
	internal static class DAGradientExtensions
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000280E File Offset: 0x00000A0E
		internal static bool IsBetween(this float intersectionFactor)
		{
			return intersectionFactor <= 1f && intersectionFactor >= 0f;
		}
	}
}
