using System;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200013F RID: 319
	internal struct TessLink
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x0004589C File Offset: 0x00043A9C
		internal static TessLink CreateLink(int count, Allocator allocator)
		{
			TessLink tessLink = default(TessLink);
			tessLink.roots = new NativeArray<int>(count, allocator, NativeArrayOptions.ClearMemory);
			tessLink.ranks = new NativeArray<int>(count, allocator, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < count; i++)
			{
				tessLink.roots[i] = i;
				tessLink.ranks[i] = 0;
			}
			return tessLink;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x000458F8 File Offset: 0x00043AF8
		internal static void DestroyLink(TessLink link)
		{
			link.ranks.Dispose();
			link.roots.Dispose();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00045914 File Offset: 0x00043B14
		internal int Find(int x)
		{
			int num = x;
			while (this.roots[x] != x)
			{
				x = this.roots[x];
			}
			while (this.roots[num] != x)
			{
				int num2 = this.roots[num];
				this.roots[num] = x;
				num = num2;
			}
			return x;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00045970 File Offset: 0x00043B70
		internal void Link(int x, int y)
		{
			int num = this.Find(x);
			int num2 = this.Find(y);
			if (num == num2)
			{
				return;
			}
			int num3 = this.ranks[num];
			int num4 = this.ranks[num2];
			if (num3 < num4)
			{
				this.roots[num] = num2;
				return;
			}
			if (num4 < num3)
			{
				this.roots[num2] = num;
				return;
			}
			this.roots[num2] = num;
			int num5 = num;
			int num6 = this.ranks[num5] + 1;
			this.ranks[num5] = num6;
		}

		// Token: 0x04000911 RID: 2321
		internal NativeArray<int> roots;

		// Token: 0x04000912 RID: 2322
		internal NativeArray<int> ranks;
	}
}
