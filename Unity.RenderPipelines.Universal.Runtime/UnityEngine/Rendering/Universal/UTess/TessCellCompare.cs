using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200013C RID: 316
	internal struct TessCellCompare : IComparer<int3>
	{
		// Token: 0x06000A19 RID: 2585 RVA: 0x000457D0 File Offset: 0x000439D0
		public int Compare(int3 a, int3 b)
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
			return a.z - b.z;
		}
	}
}
