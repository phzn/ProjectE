using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200001A RID: 26
	internal class MyIntersectNodeSort : IComparer<IntersectNode>
	{
		// Token: 0x06000074 RID: 116 RVA: 0x000035AC File Offset: 0x000017AC
		public int Compare(IntersectNode node1, IntersectNode node2)
		{
			long num = node2.Pt.Y - node1.Pt.Y;
			if (num > 0L)
			{
				return 1;
			}
			if (num < 0L)
			{
				return -1;
			}
			return 0;
		}
	}
}
