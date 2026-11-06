using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityEngine.Rendering
{
	// Token: 0x020000CD RID: 205
	[DebuggerDisplay("{this.GetType().Name} {humanizedData}")]
	[Serializable]
	public struct BitArray256 : IBitArray
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x000201FD File Offset: 0x0001E3FD
		public uint capacity
		{
			get
			{
				return 256U;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00020204 File Offset: 0x0001E404
		public bool allFalse
		{
			get
			{
				return this.data1 == 0UL && this.data2 == 0UL && this.data3 == 0UL && this.data4 == 0UL;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0002022A File Offset: 0x0001E42A
		public bool allTrue
		{
			get
			{
				return this.data1 == ulong.MaxValue && this.data2 == ulong.MaxValue && this.data3 == ulong.MaxValue && this.data4 == ulong.MaxValue;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00020258 File Offset: 0x0001E458
		public string humanizedData
		{
			get
			{
				return Regex.Replace(string.Format("{0, " + 64U.ToString() + "}", Convert.ToString((long)this.data4, 2)).Replace(' ', '0'), ".{8}", "$0.") + Regex.Replace(string.Format("{0, " + 64U.ToString() + "}", Convert.ToString((long)this.data3, 2)).Replace(' ', '0'), ".{8}", "$0.") + Regex.Replace(string.Format("{0, " + 64U.ToString() + "}", Convert.ToString((long)this.data2, 2)).Replace(' ', '0'), ".{8}", "$0.") + Regex.Replace(string.Format("{0, " + 64U.ToString() + "}", Convert.ToString((long)this.data1, 2)).Replace(' ', '0'), ".{8}", "$0.").TrimEnd('.');
			}
		}

		// Token: 0x17000111 RID: 273
		public bool this[uint index]
		{
			get
			{
				return BitArrayUtilities.Get256(index, this.data1, this.data2, this.data3, this.data4);
			}
			set
			{
				BitArrayUtilities.Set256(index, ref this.data1, ref this.data2, ref this.data3, ref this.data4, value);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000203BA File Offset: 0x0001E5BA
		public BitArray256(ulong initValue1, ulong initValue2, ulong initValue3, ulong initValue4)
		{
			this.data1 = initValue1;
			this.data2 = initValue2;
			this.data3 = initValue3;
			this.data4 = initValue4;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000203DC File Offset: 0x0001E5DC
		public BitArray256(IEnumerable<uint> bitIndexTrue)
		{
			this.data1 = (this.data2 = (this.data3 = (this.data4 = 0UL)));
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
				else if (num < 128U)
				{
					this.data2 |= 1UL << (int)(num - 64U);
				}
				else if (num < 192U)
				{
					this.data3 |= 1UL << (int)(num - 128U);
				}
				else if (num < this.capacity)
				{
					this.data4 |= 1UL << (int)(num - 192U);
				}
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000204B4 File Offset: 0x0001E6B4
		public static BitArray256 operator ~(BitArray256 a)
		{
			return new BitArray256(~a.data1, ~a.data2, ~a.data3, ~a.data4);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000204D7 File Offset: 0x0001E6D7
		public static BitArray256 operator |(BitArray256 a, BitArray256 b)
		{
			return new BitArray256(a.data1 | b.data1, a.data2 | b.data2, a.data3 | b.data3, a.data4 | b.data4);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00020512 File Offset: 0x0001E712
		public static BitArray256 operator &(BitArray256 a, BitArray256 b)
		{
			return new BitArray256(a.data1 & b.data1, a.data2 & b.data2, a.data3 & b.data3, a.data4 & b.data4);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002054D File Offset: 0x0001E74D
		public IBitArray BitAnd(IBitArray other)
		{
			return this & (BitArray256)other;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00020565 File Offset: 0x0001E765
		public IBitArray BitOr(IBitArray other)
		{
			return this | (BitArray256)other;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002057D File Offset: 0x0001E77D
		public IBitArray BitNot()
		{
			return ~this;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002058F File Offset: 0x0001E78F
		public static bool operator ==(BitArray256 a, BitArray256 b)
		{
			return a.data1 == b.data1 && a.data2 == b.data2 && a.data3 == b.data3 && a.data4 == b.data4;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000205CB File Offset: 0x0001E7CB
		public static bool operator !=(BitArray256 a, BitArray256 b)
		{
			return a.data1 != b.data1 || a.data2 != b.data2 || a.data3 != b.data3 || a.data4 != b.data4;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002060C File Offset: 0x0001E80C
		public override bool Equals(object obj)
		{
			return obj is BitArray256 && this.data1.Equals(((BitArray256)obj).data1) && this.data2.Equals(((BitArray256)obj).data2) && this.data3.Equals(((BitArray256)obj).data3) && this.data4.Equals(((BitArray256)obj).data4);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00020684 File Offset: 0x0001E884
		public override int GetHashCode()
		{
			return (((1870826326 * -1521134295 + this.data1.GetHashCode()) * -1521134295 + this.data2.GetHashCode()) * -1521134295 + this.data3.GetHashCode()) * -1521134295 + this.data4.GetHashCode();
		}

		// Token: 0x0400046B RID: 1131
		[SerializeField]
		private ulong data1;

		// Token: 0x0400046C RID: 1132
		[SerializeField]
		private ulong data2;

		// Token: 0x0400046D RID: 1133
		[SerializeField]
		private ulong data3;

		// Token: 0x0400046E RID: 1134
		[SerializeField]
		private ulong data4;
	}
}
