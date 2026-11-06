using System;

namespace System.Text
{
	// Token: 0x020003AC RID: 940
	internal static class StringBuilderCache
	{
		// Token: 0x060026E3 RID: 9955 RVA: 0x0008AA00 File Offset: 0x00088C00
		public static StringBuilder Acquire(int capacity = 16)
		{
			if (capacity <= 360)
			{
				StringBuilder stringBuilder = StringBuilderCache.t_cachedInstance;
				if (stringBuilder != null && capacity <= stringBuilder.Capacity)
				{
					StringBuilderCache.t_cachedInstance = null;
					stringBuilder.Clear();
					return stringBuilder;
				}
			}
			return new StringBuilder(capacity);
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0008AA3C File Offset: 0x00088C3C
		public static void Release(StringBuilder sb)
		{
			if (sb.Capacity <= 360)
			{
				StringBuilderCache.t_cachedInstance = sb;
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x0008AA51 File Offset: 0x00088C51
		public static string GetStringAndRelease(StringBuilder sb)
		{
			string text = sb.ToString();
			StringBuilderCache.Release(sb);
			return text;
		}

		// Token: 0x04001DDD RID: 7645
		private const int MaxBuilderSize = 360;

		// Token: 0x04001DDE RID: 7646
		private const int DefaultCapacity = 16;

		// Token: 0x04001DDF RID: 7647
		[ThreadStatic]
		private static StringBuilder t_cachedInstance;
	}
}
