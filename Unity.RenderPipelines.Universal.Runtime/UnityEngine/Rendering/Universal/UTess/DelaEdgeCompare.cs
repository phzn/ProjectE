using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200013E RID: 318
	internal struct DelaEdgeCompare : IComparer<int4>
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x00045844 File Offset: 0x00043A44
		public int Compare(int4 a, int4 b)
		{
			int num = a.x - b.x;
			if (num != 0)
			{
				return num;
			}
			num = a.y - b.y;
			if (num != 0)
			{
				return num;
			}
			num = a.z - b.z;
			if (num != 0)
			{
				return num;
			}
			return a.w - b.w;
		}
	}
}
