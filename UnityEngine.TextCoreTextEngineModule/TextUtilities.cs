using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000050 RID: 80
	internal static class TextUtilities
	{
		// Token: 0x0600025A RID: 602 RVA: 0x00024A9C File Offset: 0x00022C9C
		internal static void ResizeArray<T>(ref T[] array)
		{
			int num = TextUtilities.NextPowerOfTwo(array.Length);
			Array.Resize<T>(ref array, num);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00024ABC File Offset: 0x00022CBC
		internal static void ResizeArray<T>(ref T[] array, int size)
		{
			size = TextUtilities.NextPowerOfTwo(size);
			Array.Resize<T>(ref array, size);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00024AD0 File Offset: 0x00022CD0
		internal static int NextPowerOfTwo(int v)
		{
			v |= v >> 16;
			v |= v >> 8;
			v |= v >> 4;
			v |= v >> 2;
			v |= v >> 1;
			return v + 1;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00024B0C File Offset: 0x00022D0C
		internal static char ToLowerFast(char c)
		{
			bool flag = (int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1;
			char c2;
			if (flag)
			{
				c2 = c;
			}
			else
			{
				c2 = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[(int)c];
			}
			return c2;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00024B40 File Offset: 0x00022D40
		internal static char ToUpperFast(char c)
		{
			bool flag = (int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1;
			char c2;
			if (flag)
			{
				c2 = c;
			}
			else
			{
				c2 = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
			}
			return c2;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00024B74 File Offset: 0x00022D74
		internal static uint ToUpperASCIIFast(uint c)
		{
			bool flag = (ulong)c > (ulong)((long)("-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1));
			uint num;
			if (flag)
			{
				num = c;
			}
			else
			{
				num = (uint)"-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
			}
			return num;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00024BAC File Offset: 0x00022DAC
		internal static uint ToLowerASCIIFast(uint c)
		{
			bool flag = (ulong)c > (ulong)((long)("-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1));
			uint num;
			if (flag)
			{
				num = c;
			}
			else
			{
				num = (uint)"-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[(int)c];
			}
			return num;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00024BE4 File Offset: 0x00022DE4
		public static int GetHashCodeCaseSensitive(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ (int)s[i];
			}
			return num;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00024C1C File Offset: 0x00022E1C
		public static int GetHashCodeCaseInSensitive(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ (int)TextUtilities.ToUpperFast(s[i]);
			}
			return num;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00024C5C File Offset: 0x00022E5C
		public static uint GetSimpleHashCodeLowercase(string s)
		{
			uint num = 0U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ (uint)TextUtilities.ToLowerFast(s[i]);
			}
			return num;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00024C9C File Offset: 0x00022E9C
		internal static uint ConvertToUTF32(uint highSurrogate, uint lowSurrogate)
		{
			return (highSurrogate - 55296U) * 1024U + (lowSurrogate - 56320U + 65536U);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00024CCC File Offset: 0x00022ECC
		internal static uint ReadUTF16(uint[] text, int index)
		{
			uint num = 0U;
			num += TextUtilities.HexToInt((char)text[index]) << 12;
			num += TextUtilities.HexToInt((char)text[index + 1]) << 8;
			num += TextUtilities.HexToInt((char)text[index + 2]) << 4;
			return num + TextUtilities.HexToInt((char)text[index + 3]);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00024D20 File Offset: 0x00022F20
		internal static uint ReadUTF32(uint[] text, int index)
		{
			uint num = 0U;
			num += TextUtilities.HexToInt((char)text[index]) << 30;
			num += TextUtilities.HexToInt((char)text[index + 1]) << 24;
			num += TextUtilities.HexToInt((char)text[index + 2]) << 20;
			num += TextUtilities.HexToInt((char)text[index + 3]) << 16;
			num += TextUtilities.HexToInt((char)text[index + 4]) << 12;
			num += TextUtilities.HexToInt((char)text[index + 5]) << 8;
			num += TextUtilities.HexToInt((char)text[index + 6]) << 4;
			return num + TextUtilities.HexToInt((char)text[index + 7]);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00024DB8 File Offset: 0x00022FB8
		private static uint HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0U;
			case '1':
				return 1U;
			case '2':
				return 2U;
			case '3':
				return 3U;
			case '4':
				return 4U;
			case '5':
				return 5U;
			case '6':
				return 6U;
			case '7':
				return 7U;
			case '8':
				return 8U;
			case '9':
				return 9U;
			case ':':
			case ';':
			case '<':
			case '=':
			case '>':
			case '?':
			case '@':
				break;
			case 'A':
				return 10U;
			case 'B':
				return 11U;
			case 'C':
				return 12U;
			case 'D':
				return 13U;
			case 'E':
				return 14U;
			case 'F':
				return 15U;
			default:
				switch (hex)
				{
				case 'a':
					return 10U;
				case 'b':
					return 11U;
				case 'c':
					return 12U;
				case 'd':
					return 13U;
				case 'e':
					return 14U;
				case 'f':
					return 15U;
				}
				break;
			}
			return 15U;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00024EC0 File Offset: 0x000230C0
		public static uint StringHexToInt(string s)
		{
			uint num = 0U;
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				num += TextUtilities.HexToInt(s[i]) * (uint)Mathf.Pow(16f, (float)(length - 1 - i));
			}
			return num;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00024F10 File Offset: 0x00023110
		internal static string UintToString(this List<uint> unicodes)
		{
			char[] array = new char[unicodes.Count];
			for (int i = 0; i < unicodes.Count; i++)
			{
				array[i] = (char)unicodes[i];
			}
			return new string(array);
		}

		// Token: 0x0400040F RID: 1039
		private const string k_LookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

		// Token: 0x04000410 RID: 1040
		private const string k_LookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";
	}
}
