using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityEngine.Rendering
{
	// Token: 0x020000CB RID: 203
	[DebuggerDisplay("{this.GetType().Name} {humanizedData}")]
	[Serializable]
	public struct BitArray64 : IBitArray
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0001FD7C File Offset: 0x0001DF7C
		public uint capacity
		{
			get
			{
				return 64U;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001FD80 File Offset: 0x0001DF80
		public bool allFalse
		{
			get
			{
				return this.data == 0UL;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001FD8C File Offset: 0x0001DF8C
		public bool allTrue
		{
			get
			{
				return this.data == ulong.MaxValue;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001FD98 File Offset: 0x0001DF98
		public string humanizedData
		{
			get
			{
				return Regex.Replace(string.Format("{0, " + this.capacity.ToString() + "}", Convert.ToString((long)this.data, 2)).Replace(' ', '0'), ".{8}", "$0.").TrimEnd('.');
			}
		}

		// Token: 0x17000107 RID: 263
		public bool this[uint index]
		{
			get
			{
				return BitArrayUtilities.Get64(index, this.data);
			}
			set
			{
				BitArrayUtilities.Set64(index, ref this.data, value);
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001FE0F File Offset: 0x0001E00F
		public BitArray64(ulong initValue)
		{
			this.data = initValue;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001FE18 File Offset: 0x0001E018
		public BitArray64(IEnumerable<uint> bitIndexTrue)
		{
			this.data = 0UL;
			if (bitIndexTrue == null)
			{
				return;
			}
			for (int i = bitIndexTrue.Count<uint>() - 1; i >= 0; i--)
			{
				uint num = bitIndexTrue.ElementAt<uint>(i);
				if (num < this.capacity)
				{
					this.data |= 1UL << (int)num;
				}
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001FE69 File Offset: 0x0001E069
		public static BitArray64 operator ~(BitArray64 a)
		{
			return new BitArray64(~a.data);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001FE77 File Offset: 0x0001E077
		public static BitArray64 operator |(BitArray64 a, BitArray64 b)
		{
			return new BitArray64(a.data | b.data);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001FE8B File Offset: 0x0001E08B
		public static BitArray64 operator &(BitArray64 a, BitArray64 b)
		{
			return new BitArray64(a.data & b.data);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001FE9F File Offset: 0x0001E09F
		public IBitArray BitAnd(IBitArray other)
		{
			return this & (BitArray64)other;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001FEB7 File Offset: 0x0001E0B7
		public IBitArray BitOr(IBitArray other)
		{
			return this | (BitArray64)other;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001FECF File Offset: 0x0001E0CF
		public IBitArray BitNot()
		{
			return ~this;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001FEE1 File Offset: 0x0001E0E1
		public static bool operator ==(BitArray64 a, BitArray64 b)
		{
			return a.data == b.data;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001FEF1 File Offset: 0x0001E0F1
		public static bool operator !=(BitArray64 a, BitArray64 b)
		{
			return a.data != b.data;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001FF04 File Offset: 0x0001E104
		public override bool Equals(object obj)
		{
			return obj is BitArray64 && ((BitArray64)obj).data == this.data;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001FF23 File Offset: 0x0001E123
		public override int GetHashCode()
		{
			return 1768953197 + this.data.GetHashCode();
		}

		// Token: 0x04000468 RID: 1128
		[SerializeField]
		private ulong data;
	}
}
