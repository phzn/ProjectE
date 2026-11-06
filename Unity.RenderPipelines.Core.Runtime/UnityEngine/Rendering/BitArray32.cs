using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityEngine.Rendering
{
	// Token: 0x020000CA RID: 202
	[DebuggerDisplay("{this.GetType().Name} {humanizedData}")]
	[Serializable]
	public struct BitArray32 : IBitArray
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001FBB1 File Offset: 0x0001DDB1
		public uint capacity
		{
			get
			{
				return 32U;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001FBB5 File Offset: 0x0001DDB5
		public bool allFalse
		{
			get
			{
				return this.data == 0U;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		public bool allTrue
		{
			get
			{
				return this.data == uint.MaxValue;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001FBCB File Offset: 0x0001DDCB
		private string humanizedVersion
		{
			get
			{
				return Convert.ToString((long)((ulong)this.data), 2);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001FBDC File Offset: 0x0001DDDC
		public string humanizedData
		{
			get
			{
				return Regex.Replace(string.Format("{0, " + this.capacity.ToString() + "}", Convert.ToString((long)((ulong)this.data), 2)).Replace(' ', '0'), ".{8}", "$0.").TrimEnd('.');
			}
		}

		// Token: 0x17000102 RID: 258
		public bool this[uint index]
		{
			get
			{
				return BitArrayUtilities.Get32(index, this.data);
			}
			set
			{
				BitArrayUtilities.Set32(index, ref this.data, value);
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001FC54 File Offset: 0x0001DE54
		public BitArray32(uint initValue)
		{
			this.data = initValue;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001FC60 File Offset: 0x0001DE60
		public BitArray32(IEnumerable<uint> bitIndexTrue)
		{
			this.data = 0U;
			if (bitIndexTrue == null)
			{
				return;
			}
			for (int i = bitIndexTrue.Count<uint>() - 1; i >= 0; i--)
			{
				uint num = bitIndexTrue.ElementAt<uint>(i);
				if (num < this.capacity)
				{
					this.data |= 1U << (int)num;
				}
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001FCAF File Offset: 0x0001DEAF
		public IBitArray BitAnd(IBitArray other)
		{
			return this & (BitArray32)other;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001FCC7 File Offset: 0x0001DEC7
		public IBitArray BitOr(IBitArray other)
		{
			return this | (BitArray32)other;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001FCDF File Offset: 0x0001DEDF
		public IBitArray BitNot()
		{
			return ~this;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001FCF1 File Offset: 0x0001DEF1
		public static BitArray32 operator ~(BitArray32 a)
		{
			return new BitArray32(~a.data);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001FCFF File Offset: 0x0001DEFF
		public static BitArray32 operator |(BitArray32 a, BitArray32 b)
		{
			return new BitArray32(a.data | b.data);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001FD13 File Offset: 0x0001DF13
		public static BitArray32 operator &(BitArray32 a, BitArray32 b)
		{
			return new BitArray32(a.data & b.data);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001FD27 File Offset: 0x0001DF27
		public static bool operator ==(BitArray32 a, BitArray32 b)
		{
			return a.data == b.data;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001FD37 File Offset: 0x0001DF37
		public static bool operator !=(BitArray32 a, BitArray32 b)
		{
			return a.data != b.data;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001FD4A File Offset: 0x0001DF4A
		public override bool Equals(object obj)
		{
			return obj is BitArray32 && ((BitArray32)obj).data == this.data;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001FD69 File Offset: 0x0001DF69
		public override int GetHashCode()
		{
			return 1768953197 + this.data.GetHashCode();
		}

		// Token: 0x04000467 RID: 1127
		[SerializeField]
		private uint data;
	}
}
