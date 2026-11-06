using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C9 RID: 201
	[DebuggerDisplay("{this.GetType().Name} {humanizedData}")]
	[Serializable]
	public struct BitArray16 : IBitArray
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0001F9F1 File Offset: 0x0001DBF1
		public uint capacity
		{
			get
			{
				return 16U;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0001F9F5 File Offset: 0x0001DBF5
		public bool allFalse
		{
			get
			{
				return this.data == 0;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001FA00 File Offset: 0x0001DC00
		public bool allTrue
		{
			get
			{
				return this.data == ushort.MaxValue;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001FA10 File Offset: 0x0001DC10
		public string humanizedData
		{
			get
			{
				return Regex.Replace(string.Format("{0, " + this.capacity.ToString() + "}", Convert.ToString((int)this.data, 2)).Replace(' ', '0'), ".{8}", "$0.").TrimEnd('.');
			}
		}

		// Token: 0x170000FC RID: 252
		public bool this[uint index]
		{
			get
			{
				return BitArrayUtilities.Get16(index, this.data);
			}
			set
			{
				BitArrayUtilities.Set16(index, ref this.data, value);
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001FA87 File Offset: 0x0001DC87
		public BitArray16(ushort initValue)
		{
			this.data = initValue;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001FA90 File Offset: 0x0001DC90
		public BitArray16(IEnumerable<uint> bitIndexTrue)
		{
			this.data = 0;
			if (bitIndexTrue == null)
			{
				return;
			}
			for (int i = bitIndexTrue.Count<uint>() - 1; i >= 0; i--)
			{
				uint num = bitIndexTrue.ElementAt<uint>(i);
				if (num < this.capacity)
				{
					this.data |= (ushort)(1 << (int)num);
				}
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001FAE1 File Offset: 0x0001DCE1
		public static BitArray16 operator ~(BitArray16 a)
		{
			return new BitArray16(~a.data);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001FAF0 File Offset: 0x0001DCF0
		public static BitArray16 operator |(BitArray16 a, BitArray16 b)
		{
			return new BitArray16(a.data | b.data);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001FB05 File Offset: 0x0001DD05
		public static BitArray16 operator &(BitArray16 a, BitArray16 b)
		{
			return new BitArray16(a.data & b.data);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001FB1A File Offset: 0x0001DD1A
		public IBitArray BitAnd(IBitArray other)
		{
			return this & (BitArray16)other;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001FB32 File Offset: 0x0001DD32
		public IBitArray BitOr(IBitArray other)
		{
			return this | (BitArray16)other;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001FB4A File Offset: 0x0001DD4A
		public IBitArray BitNot()
		{
			return ~this;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001FB5C File Offset: 0x0001DD5C
		public static bool operator ==(BitArray16 a, BitArray16 b)
		{
			return a.data == b.data;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001FB6C File Offset: 0x0001DD6C
		public static bool operator !=(BitArray16 a, BitArray16 b)
		{
			return a.data != b.data;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001FB7F File Offset: 0x0001DD7F
		public override bool Equals(object obj)
		{
			return obj is BitArray16 && ((BitArray16)obj).data == this.data;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001FB9E File Offset: 0x0001DD9E
		public override int GetHashCode()
		{
			return 1768953197 + this.data.GetHashCode();
		}

		// Token: 0x04000466 RID: 1126
		[SerializeField]
		private ushort data;
	}
}
