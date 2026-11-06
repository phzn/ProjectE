using System;
using System.Linq;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000031 RID: 49
	public static class OtherExtensions
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00005318 File Offset: 0x00003518
		public static bool IsDefault<T>(this T obj)
		{
			return obj == null || obj.Equals(default(T));
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000534C File Offset: 0x0000354C
		public static bool IsFlagSet<T>(this T value, T flag) where T : struct
		{
			long num = Convert.ToInt64(value);
			long num2 = Convert.ToInt64(flag);
			return (num & num2) != 0L;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005376 File Offset: 0x00003576
		public static T CopyClass<T>(this T source)
		{
			return JsonUtility.FromJson<T>(JsonUtility.ToJson(source));
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005388 File Offset: 0x00003588
		public static string GetNumbers(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return string.Empty;
			}
			return new string(text.Where<char>((char p) => char.IsDigit(p)).ToArray<char>());
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000053C7 File Offset: 0x000035C7
		public static bool ToBoolNullTrue(this bool? value)
		{
			return value == null || value.Value;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000053DB File Offset: 0x000035DB
		public static bool ToBoolNullFalse(this bool? value)
		{
			return value != null && value.Value;
		}
	}
}
