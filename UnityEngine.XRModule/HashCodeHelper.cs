using System;

namespace UnityEngine.XR
{
	// Token: 0x02000029 RID: 41
	internal static class HashCodeHelper
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00004778 File Offset: 0x00002978
		public static int Combine(int hash1, int hash2)
		{
			return hash1 * 486187739 + hash2;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004794 File Offset: 0x00002994
		public static int Combine(int hash1, int hash2, int hash3)
		{
			return HashCodeHelper.Combine(HashCodeHelper.Combine(hash1, hash2), hash3);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000047A3 File Offset: 0x000029A3
		public static int Combine(int hash1, int hash2, int hash3, int hash4)
		{
			return HashCodeHelper.Combine(HashCodeHelper.Combine(hash1, hash2, hash3), hash4);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000047B3 File Offset: 0x000029B3
		public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5)
		{
			return HashCodeHelper.Combine(HashCodeHelper.Combine(hash1, hash2, hash3, hash4), hash5);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000047C5 File Offset: 0x000029C5
		public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6)
		{
			return HashCodeHelper.Combine(HashCodeHelper.Combine(hash1, hash2, hash3, hash4, hash5), hash6);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000047D9 File Offset: 0x000029D9
		public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6, int hash7)
		{
			return HashCodeHelper.Combine(HashCodeHelper.Combine(hash1, hash2, hash3, hash4, hash5, hash6), hash7);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000047EF File Offset: 0x000029EF
		public static int Combine(int hash1, int hash2, int hash3, int hash4, int hash5, int hash6, int hash7, int hash8)
		{
			return HashCodeHelper.Combine(HashCodeHelper.Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7), hash8);
		}

		// Token: 0x040000FA RID: 250
		private const int k_HashCodeMultiplier = 486187739;
	}
}
