using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringReferenceExtensions
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x00018310 File Offset: 0x00016510
		public static int IndexOf(this StringReference s, char c, int startIndex, int length)
		{
			int num = Array.IndexOf<char>(s.Chars, c, s.StartIndex + startIndex, length);
			if (num == -1)
			{
				return -1;
			}
			return num - s.StartIndex;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00018344 File Offset: 0x00016544
		public static bool StartsWith(this StringReference s, string text)
		{
			if (text.Length > s.Length)
			{
				return false;
			}
			char[] chars = s.Chars;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != chars[i + s.StartIndex])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00018394 File Offset: 0x00016594
		public static bool EndsWith(this StringReference s, string text)
		{
			if (text.Length > s.Length)
			{
				return false;
			}
			char[] chars = s.Chars;
			int num = s.StartIndex + s.Length - text.Length;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != chars[i + num])
				{
					return false;
				}
			}
			return true;
		}
	}
}
