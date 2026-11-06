using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200013D RID: 317
	internal struct TessJunctionCompare : IComparer<int2>
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x00045814 File Offset: 0x00043A14
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
