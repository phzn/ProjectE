using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200013B RID: 315
	internal struct TessEdgeCompare : IComparer<int2>
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x000457A0 File Offset: 0x000439A0
		public int Compare(int2 a, int2 b)
		{
			int num = a.x - b.x;
			if (num != 0)
			{
				return num;
			}
			return a.y - b.y;
		}
	}
}
