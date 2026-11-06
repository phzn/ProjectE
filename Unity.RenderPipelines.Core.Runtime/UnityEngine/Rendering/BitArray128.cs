using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityEngine.Rendering
{
	// Token: 0x020000CC RID: 204
	[DebuggerDisplay("{this.GetType().Name} {humanizedData}")]
	[Serializable]
	public struct BitArray128 : IBitArray
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001FF36 File Offset: 0x0001E136
		public uint capacity
		{
			get
			{
				return 128U;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001FF3D File Offset: 0x0001E13D
		public bool allFalse
		{
			get
			{
				return this.data1 == 0UL && this.data2 == 0UL;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001FF53 File Offset: 0x0001E153
		public bool allTrue
		{
			get
			{
				return this.data1 == ulong.MaxValue && this.data2 == ulong.MaxValue;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001FF6C File Offset: 0x0001E16C
		public string humanizedData
		{
			get
			{
				return Regex.Replace(string.Format("{0, " + 64U.ToString() + "}", Convert.ToString((long)this.data2, 2)).Replace(' ', '0'), ".{8}", "$0.") + Regex.Replace(string.Format("{0, " + 64U.ToString() + "}", Convert.ToString((long)this.data1, 2)).Replace(' ', '0'), ".{8}", "$0.").TrimEnd('.');
			}
		}

		// Token: 0x1700010C RID: 268
		public bool this[uint index]
		{
			get
			{
				return BitArrayUtilities.Get128(index, this.data1, this.data2);
			}
			set
			{
				BitArrayUtilities.Set128(index, ref this.data1, ref this.data2, value);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00020032 File Offset: 0x0001E232
		public BitArray128(ulong initValue1, ulong initValue2)
		{
			this.data1 = initValue1;
			this.data2 = initValue2;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00020044 File Offset: 0x0001E244
		public BitArray128(IEnumerable<uint> bitIndexTrue)
		{
			this.data1 = (this.data2 = 0UL);
			if (bitIndexTrue == null)
			{
				return;
			}
			for (int i = bitIndexTrue.Count<uint>() - 1; i >= 0; i--)
			{
				uint num = bitIndexTrue.ElementAt<uint>(i);
				if (num < 64U)
				{
					this.data1 |= 1UL << (int)num;
				}
				else if (num < this.capacity)
				{
					this.data2 |= 1UL << (int)(num - 64U);
				}
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000200BC File Offset: 0x0001E2BC
		public static BitArray128 operator ~(BitArray128 a)
		{
			return new BitArray128(~a.data1, ~a.data2);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000200D1 File Offset: 0x0001E2D1
		public static BitArray128 operator |(BitArray128 a, BitArray128 b)
		{
			return new BitArray128(a.data1 | b.data1, a.data2 | b.data2);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000200F2 File Offset: 0x0001E2F2
		public static BitArray128 operator &(BitArray128 a, BitArray128 b)
		{
			return new BitArray128(a.data1 & b.data1, a.data2 & b.data2);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00020113 File Offset: 0x0001E313
		public IBitArray BitAnd(IBitArray other)
		{
			return this & (BitArray128)other;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0002012B File Offset: 0x0001E32B
		public IBitArray BitOr(IBitArray other)
		{
			return this | (BitArray128)other;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00020143 File Offset: 0x0001E343
		public IBitArray BitNot()
		{
			return ~this;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00020155 File Offset: 0x0001E355
		public static bool operator ==(BitArray128 a, BitArray128 b)
		{
			return a.data1 == b.data1 && a.data2 == b.data2;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00020175 File Offset: 0x0001E375
		public static bool operator !=(BitArray128 a, BitArray128 b)
		{
			return a.data1 != b.data1 || a.data2 != b.data2;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00020198 File Offset: 0x0001E398
		public override bool Equals(object obj)
		{
			return obj is BitArray128 && this.data1.Equals(((BitArray128)obj).data1) && this.data2.Equals(((BitArray128)obj).data2);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000201D2 File Offset: 0x0001E3D2
		public override int GetHashCode()
		{
			return (1755735569 * -1521134295 + this.data1.GetHashCode()) * -1521134295 + this.data2.GetHashCode();
		}

		// Token: 0x04000469 RID: 1129
		[SerializeField]
		private ulong data1;

		// Token: 0x0400046A RID: 1130
		[SerializeField]
		private ulong data2;
	}
}
