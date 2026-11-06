using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000311 RID: 785
	[Serializable]
	public struct Hash128 : IEquatable<Hash128>, IComparable<Hash128>
	{
		// Token: 0x06001527 RID: 5415 RVA: 0x000502E9 File Offset: 0x0004E4E9
		public Hash128(uint4 value)
		{
			this.Value = value;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x000502F2 File Offset: 0x0004E4F2
		public Hash128(uint x, uint y, uint z, uint w)
		{
			this.Value = new uint4(x, y, z, w);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00050304 File Offset: 0x0004E504
		public unsafe Hash128(string value)
		{
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.Value = Hash128.StringToHash(ptr, value.Length, true);
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x00050338 File Offset: 0x0004E538
		public unsafe Hash128(string value, bool guidFormatted)
		{
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.Value = Hash128.StringToHash(ptr, value.Length, guidFormatted);
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0005036C File Offset: 0x0004E56C
		public unsafe override string ToString()
		{
			char* ptr = stackalloc char[(UIntPtr)64];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 7; j >= 0; j--)
				{
					uint num = this.Value[i];
					num >>= j * 4;
					num &= 15U;
					ptr[i * 8 + j] = Hash128.k_HexToLiteral[(int)num];
				}
			}
			return new string(ptr, 0, 32);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x000503D0 File Offset: 0x0004E5D0
		private unsafe static uint4 StringToHash(char* guidString, int length, bool guidFormatted = true)
		{
			if (length != 32)
			{
				return default(uint4);
			}
			int* ptr = stackalloc int[(UIntPtr)128];
			for (int i = 0; i < 32; i++)
			{
				int num = (int)guidString[i];
				if (num < 0 || num > 255)
				{
					return default(uint4);
				}
				ptr[i] = (int)Hash128.k_LiteralToHex[num];
			}
			uint4 @uint = default(uint4);
			if (guidFormatted)
			{
				for (int j = 0; j < 4; j++)
				{
					uint num2 = 0U;
					for (int k = 7; k >= 0; k--)
					{
						int num3 = ptr[j * 8 + k];
						if (num3 == -1)
						{
							return default(uint4);
						}
						num2 |= (uint)((uint)num3 << k * 4);
					}
					@uint[j] = num2;
				}
			}
			else
			{
				int num4 = 0;
				for (int l = 0; l < 4; l++)
				{
					uint num5 = 0U;
					for (int m = 0; m < 4; m++)
					{
						num5 |= (uint)((uint)ptr[(IntPtr)(num4++) * 4] << m * 8 + 4);
						num5 |= (uint)((uint)ptr[(IntPtr)(num4++) * 4] << m * 8);
					}
					@uint[l] = num5;
				}
			}
			return @uint;
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00050502 File Offset: 0x0004E702
		public static bool operator ==(Hash128 obj1, Hash128 obj2)
		{
			return obj1.Value.Equals(obj2.Value);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00050516 File Offset: 0x0004E716
		public static bool operator !=(Hash128 obj1, Hash128 obj2)
		{
			return !obj1.Value.Equals(obj2.Value);
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0005052D File Offset: 0x0004E72D
		public bool Equals(Hash128 obj)
		{
			return this.Value.Equals(obj.Value);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00050540 File Offset: 0x0004E740
		public override bool Equals(object obj)
		{
			if (obj is Hash128)
			{
				Hash128 hash = (Hash128)obj;
				return this.Equals(hash);
			}
			return false;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x00050568 File Offset: 0x0004E768
		public static bool operator <(Hash128 a, Hash128 b)
		{
			if (a.Value.w != b.Value.w)
			{
				return a.Value.w < b.Value.w;
			}
			if (a.Value.z != b.Value.z)
			{
				return a.Value.z < b.Value.z;
			}
			if (a.Value.y != b.Value.y)
			{
				return a.Value.y < b.Value.y;
			}
			return a.Value.x < b.Value.x;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00050620 File Offset: 0x0004E820
		public static bool operator >(Hash128 a, Hash128 b)
		{
			if (a.Value.w != b.Value.w)
			{
				return a.Value.w > b.Value.w;
			}
			if (a.Value.z != b.Value.z)
			{
				return a.Value.z > b.Value.z;
			}
			if (a.Value.y != b.Value.y)
			{
				return a.Value.y > b.Value.y;
			}
			return a.Value.x > b.Value.x;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x000506D8 File Offset: 0x0004E8D8
		public int CompareTo(Hash128 other)
		{
			if (this.Value.w != other.Value.w)
			{
				if (this.Value.w >= other.Value.w)
				{
					return 1;
				}
				return -1;
			}
			else if (this.Value.z != other.Value.z)
			{
				if (this.Value.z >= other.Value.z)
				{
					return 1;
				}
				return -1;
			}
			else if (this.Value.y != other.Value.y)
			{
				if (this.Value.y >= other.Value.y)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (this.Value.x == other.Value.x)
				{
					return 0;
				}
				if (this.Value.x >= other.Value.x)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000507B6 File Offset: 0x0004E9B6
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x000507C9 File Offset: 0x0004E9C9
		public bool IsValid
		{
			get
			{
				return !this.Value.Equals(uint4.zero);
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000507DE File Offset: 0x0004E9DE
		public unsafe static implicit operator Hash128(Hash128 guid)
		{
			return *(Hash128*)(&guid);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x000507E8 File Offset: 0x0004E9E8
		public unsafe static implicit operator Hash128(Hash128 guid)
		{
			return *(Hash128*)(&guid);
		}

		// Token: 0x040007A6 RID: 1958
		public uint4 Value;

		// Token: 0x040007A7 RID: 1959
		private static readonly char[] k_HexToLiteral = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'a', 'b', 'c', 'd', 'e', 'f'
		};

		// Token: 0x040007A8 RID: 1960
		private static readonly sbyte[] k_LiteralToHex = new sbyte[]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, 0, 1,
			2, 3, 4, 5, 6, 7, 8, 9, -1, -1,
			-1, -1, -1, -1, -1, 10, 11, 12, 13, 14,
			15, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, 10, 11, 12,
			13, 14, 15, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1
		};

		// Token: 0x040007A9 RID: 1961
		private const int k_GUIDStringLength = 32;
	}
}
