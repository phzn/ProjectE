using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000133 RID: 307
	public struct FourCC : IEquatable<FourCC>
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x00051AE1 File Offset: 0x0004FCE1
		public FourCC(int code)
		{
			this.m_Code = code;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00051AEA File Offset: 0x0004FCEA
		public FourCC(char a, char b = ' ', char c = ' ', char d = ' ')
		{
			this.m_Code = (int)(((int)a << 24) | ((int)b << 16) | ((int)c << 8) | d);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00051B04 File Offset: 0x0004FD04
		public FourCC(string str)
		{
			this = default(FourCC);
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			int length = str.Length;
			if (length < 1 || length > 4)
			{
				throw new ArgumentException("FourCC string must be one to four characters long!", "str");
			}
			char c = str[0];
			char c2 = ((length > 1) ? str[1] : ' ');
			char c3 = ((length > 2) ? str[2] : ' ');
			char c4 = ((length > 3) ? str[3] : ' ');
			this.m_Code = (int)(((int)c << 24) | ((int)c2 << 16) | ((int)c3 << 8) | c4);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00051B94 File Offset: 0x0004FD94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int(FourCC fourCC)
		{
			return fourCC.m_Code;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00051B9C File Offset: 0x0004FD9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator FourCC(int i)
		{
			return new FourCC(i);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00051BA4 File Offset: 0x0004FDA4
		public override string ToString()
		{
			return string.Format("{0}{1}{2}{3}", new object[]
			{
				(char)(this.m_Code >> 24),
				(char)((this.m_Code & 16711680) >> 16),
				(char)((this.m_Code & 65280) >> 8),
				(char)(this.m_Code & 255)
			});
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00051C17 File Offset: 0x0004FE17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(FourCC other)
		{
			return this.m_Code == other.m_Code;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00051C28 File Offset: 0x0004FE28
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is FourCC)
			{
				FourCC fourCC = (FourCC)obj;
				return this.Equals(fourCC);
			}
			return false;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00051C52 File Offset: 0x0004FE52
		public override int GetHashCode()
		{
			return this.m_Code;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00051C5A File Offset: 0x0004FE5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(FourCC left, FourCC right)
		{
			return left.m_Code == right.m_Code;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00051C6A File Offset: 0x0004FE6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(FourCC left, FourCC right)
		{
			return left.m_Code != right.m_Code;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00051C7D File Offset: 0x0004FE7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FourCC FromInt32(int i)
		{
			return i;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00051C85 File Offset: 0x0004FE85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32(FourCC fourCC)
		{
			return fourCC.m_Code;
		}

		// Token: 0x0400070A RID: 1802
		private int m_Code;
	}
}
