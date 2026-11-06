using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{
	// Token: 0x020000FC RID: 252
	public static class MMGeometry
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x0001AF3C File Offset: 0x0001913C
		public static List<MMGeometry.MMEdge> GetEdges(int[] indices)
		{
			List<MMGeometry.MMEdge> list = new List<MMGeometry.MMEdge>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				int num = indices[i];
				int num2 = indices[i + 1];
				int num3 = indices[i + 2];
				list.Add(new MMGeometry.MMEdge(num, num2, i));
				list.Add(new MMGeometry.MMEdge(num2, num3, i));
				list.Add(new MMGeometry.MMEdge(num3, num, i));
			}
			return list;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001AF9C File Offset: 0x0001919C
		public static List<MMGeometry.MMEdge> FindBoundary(this List<MMGeometry.MMEdge> edges)
		{
			List<MMGeometry.MMEdge> list = new List<MMGeometry.MMEdge>(edges);
			for (int i = list.Count - 1; i > 0; i--)
			{
				for (int j = i - 1; j >= 0; j--)
				{
					if (list[i].Vertice1 == list[j].Vertice2 && list[i].Vertice2 == list[j].Vertice1)
					{
						list.RemoveAt(i);
						list.RemoveAt(j);
						i--;
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001B01C File Offset: 0x0001921C
		public static List<MMGeometry.MMEdge> SortEdges(this List<MMGeometry.MMEdge> edges)
		{
			List<MMGeometry.MMEdge> list = new List<MMGeometry.MMEdge>(edges);
			for (int i = 0; i < list.Count - 2; i++)
			{
				MMGeometry.MMEdge mmedge = list[i];
				int j = i + 1;
				while (j < list.Count)
				{
					MMGeometry.MMEdge mmedge2 = list[j];
					if (mmedge.Vertice2 == mmedge2.Vertice1)
					{
						if (j != i + 1)
						{
							list[j] = list[i + 1];
							list[i + 1] = mmedge2;
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
			return list;
		}

		// Token: 0x02000429 RID: 1065
		public struct MMEdge
		{
			// Token: 0x060019FE RID: 6654 RVA: 0x0006C5D6 File Offset: 0x0006A7D6
			public MMEdge(int aV1, int aV2, int aIndex)
			{
				this.Vertice1 = aV1;
				this.Vertice2 = aV2;
				this.TriangleIndex = aIndex;
			}

			// Token: 0x04001F5D RID: 8029
			public int Vertice1;

			// Token: 0x04001F5E RID: 8030
			public int Vertice2;

			// Token: 0x04001F5F RID: 8031
			public int TriangleIndex;
		}
	}
}
