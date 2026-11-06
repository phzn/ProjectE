using System;

namespace DA_Assets.Extensions
{
	// Token: 0x0200002D RID: 45
	public static class Levenshtein
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00004324 File Offset: 0x00002524
		public static float CalculateSimilarity(this string source, string target)
		{
			if (source == null || target == null)
			{
				return 0f;
			}
			if (source.Length == 0 || target.Length == 0)
			{
				return 0f;
			}
			if (source == target)
			{
				return 1f;
			}
			int num = Levenshtein.ComputeLevenshteinDistance(source, target);
			return 1f - (float)num / (float)Math.Max(source.Length, target.Length);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004388 File Offset: 0x00002588
		private static int ComputeLevenshteinDistance(string source, string target)
		{
			if (source == null || target == null)
			{
				return 0;
			}
			if (source.Length == 0 || target.Length == 0)
			{
				return 0;
			}
			if (source == target)
			{
				return source.Length;
			}
			int length = source.Length;
			int length2 = target.Length;
			if (length == 0)
			{
				return length2;
			}
			if (length2 == 0)
			{
				return length;
			}
			int[,] array = new int[length + 1, length2 + 1];
			int i = 0;
			while (i <= length)
			{
				array[i, 0] = i++;
			}
			int j = 0;
			while (j <= length2)
			{
				array[0, j] = j++;
			}
			for (int k = 1; k <= length; k++)
			{
				for (int l = 1; l <= length2; l++)
				{
					int num = ((target[l - 1] == source[k - 1]) ? 0 : 1);
					array[k, l] = Math.Min(Math.Min(array[k - 1, l] + 1, array[k, l - 1] + 1), array[k - 1, l - 1] + num);
				}
			}
			return array[length, length2];
		}
	}
}
