using System;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000012 RID: 18
	public static class ParallelGaussianBlurExtensions
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002A65 File Offset: 0x00000C65
		public static void Blur(this Texture2D texture2D, float step = 0.5f)
		{
			new ParallelGaussianBlur(texture2D, step).GaussianBlur();
		}
	}
}
