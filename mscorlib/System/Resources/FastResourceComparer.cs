using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace System.Resources
{
	// Token: 0x02000864 RID: 2148
	internal sealed class FastResourceComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
	{
		// Token: 0x06004760 RID: 18272 RVA: 0x000E87D5 File Offset: 0x000E69D5
		public int GetHashCode(object key)
		{
			return FastResourceComparer.HashFunction((string)key);
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x000E87E2 File Offset: 0x000E69E2
		public int GetHashCode(string key)
		{
			return FastResourceComparer.HashFunction(key);
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x000E87EC File Offset: 0x000E69EC
		internal static int HashFunction(string key)
		{
			uint num = 5381U;
			for (int i = 0; i < key.Length; i++)
			{
				num = ((num << 5) + num) ^ (uint)key[i];
			}
			return (int)num;
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x000E8820 File Offset: 0x000E6A20
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			string text = (string)a;
			string text2 = (string)b;
			return string.CompareOrdinal(text, text2);
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x00041971 File Offset: 0x0003FB71
		public int Compare(string a, string b)
		{
			return string.CompareOrdinal(a, b);
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0004197A File Offset: 0x0003FB7A
		public bool Equals(string a, string b)
		{
			return string.Equals(a, b);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x000E8848 File Offset: 0x000E6A48
		public bool Equals(object a, object b)
		{
			if (a == b)
			{
				return true;
			}
			string text = (string)a;
			string text2 = (string)b;
			return string.Equals(text, text2);
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x000E8870 File Offset: 0x000E6A70
		[SecurityCritical]
		public unsafe static int CompareOrdinal(string a, byte[] bytes, int bCharLength)
		{
			int num = 0;
			int num2 = 0;
			int num3 = a.Length;
			if (num3 > bCharLength)
			{
				num3 = bCharLength;
			}
			if (bCharLength == 0)
			{
				if (a.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				fixed (byte[] array = bytes)
				{
					byte* ptr;
					if (bytes == null || array.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array[0];
					}
					byte* ptr2 = ptr;
					while (num < num3 && num2 == 0)
					{
						int num4 = (int)(*ptr2) | ((int)ptr2[1] << 8);
						num2 = (int)a[num++] - num4;
						ptr2 += 2;
					}
				}
				if (num2 != 0)
				{
					return num2;
				}
				return a.Length - bCharLength;
			}
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x000E88F6 File Offset: 0x000E6AF6
		[SecurityCritical]
		public static int CompareOrdinal(byte[] bytes, int aCharLength, string b)
		{
			return -FastResourceComparer.CompareOrdinal(b, bytes, aCharLength);
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x000E8904 File Offset: 0x000E6B04
		[SecurityCritical]
		internal unsafe static int CompareOrdinal(byte* a, int byteLen, string b)
		{
			int num = 0;
			int num2 = 0;
			int num3 = byteLen >> 1;
			if (num3 > b.Length)
			{
				num3 = b.Length;
			}
			while (num2 < num3 && num == 0)
			{
				num = (int)((char)((int)(*(a++)) | ((int)(*(a++)) << 8)) - b[num2++]);
			}
			if (num != 0)
			{
				return num;
			}
			return byteLen - b.Length * 2;
		}

		// Token: 0x04002DCB RID: 11723
		internal static readonly FastResourceComparer Default = new FastResourceComparer();
	}
}
