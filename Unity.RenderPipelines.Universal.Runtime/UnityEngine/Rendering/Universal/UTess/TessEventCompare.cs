using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200013A RID: 314
	internal struct TessEventCompare : IComparer<UEvent>
	{
		// Token: 0x06000A17 RID: 2583 RVA: 0x000456E4 File Offset: 0x000438E4
		public int Compare(UEvent a, UEvent b)
		{
			float num = a.a.x - b.a.x;
			if (0f != num)
			{
				if (num <= 0f)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				num = a.a.y - b.a.y;
				if (0f != num)
				{
					if (num <= 0f)
					{
						return -1;
					}
					return 1;
				}
				else
				{
					int num2 = a.type - b.type;
					if (num2 != 0)
					{
						return num2;
					}
					if (a.type != 0)
					{
						float num3 = ModuleHandle.OrientFast(a.a, a.b, b.b);
						if (0f != num3)
						{
							if (num3 <= 0f)
							{
								return -1;
							}
							return 1;
						}
					}
					return a.idx - b.idx;
				}
			}
		}
	}
}
