using System;
using UnityEngine;

namespace DA_Assets.DAG
{
	// Token: 0x02000005 RID: 5
	public class DAColorBlender
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002825 File Offset: 0x00000A25
		public static Color Blend(Color c1, Color c2, DAColorBlendMode mode, float intensity)
		{
			if (mode == DAColorBlendMode.Overlay)
			{
				return c1.Overlay(c2, intensity);
			}
			if (mode == DAColorBlendMode.Difference)
			{
				return c1.Difference(c2, intensity);
			}
			return c1.Multiply(c2, intensity);
		}
	}
}
