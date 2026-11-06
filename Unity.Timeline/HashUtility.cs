using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200004C RID: 76
	internal static class HashUtility
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x0000A2B0 File Offset: 0x000084B0
		public static int CombineHash(this int h1, int h2)
		{
			return h1 ^ (int)((long)h2 + (long)((ulong)(-1640531527)) + (long)((long)h1 << 6) + (long)(h1 >> 2));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000A2C8 File Offset: 0x000084C8
		public static int CombineHash(int h1, int h2, int h3)
		{
			return h1.CombineHash(h2).CombineHash(h3);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000A2D7 File Offset: 0x000084D7
		public static int CombineHash(int h1, int h2, int h3, int h4)
		{
			return HashUtility.CombineHash(h1, h2, h3).CombineHash(h4);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000A2E7 File Offset: 0x000084E7
		public static int CombineHash(int h1, int h2, int h3, int h4, int h5)
		{
			return HashUtility.CombineHash(h1, h2, h3, h4).CombineHash(h5);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A2F9 File Offset: 0x000084F9
		public static int CombineHash(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return HashUtility.CombineHash(h1, h2, h3, h4, h5).CombineHash(h6);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A30D File Offset: 0x0000850D
		public static int CombineHash(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return HashUtility.CombineHash(h1, h2, h3, h4, h5, h6).CombineHash(h7);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A324 File Offset: 0x00008524
		public static int CombineHash(int[] hashes)
		{
			if (hashes == null || hashes.Length == 0)
			{
				return 0;
			}
			int num = hashes[0];
			for (int i = 1; i < hashes.Length; i++)
			{
				num = num.CombineHash(hashes[i]);
			}
			return num;
		}
	}
}
