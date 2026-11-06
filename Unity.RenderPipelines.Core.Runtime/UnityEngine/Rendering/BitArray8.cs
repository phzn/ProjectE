using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C8 RID: 200
	[DebuggerDisplay("{this.GetType().Name} {humanizedData}")]
	[Serializable]
	public struct BitArray8 : IBitArray
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001F844 File Offset: 0x0001DA44
		public uint capacity
		{
			get
			{
				return 8U;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001F847 File Offset: 0x0001DA47
		public bool allFalse
		{
			get
			{
				return this.data == 0;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001F852 File Offset: 0x0001DA52
		public bool allTrue
		{
			get
			{
				return this.data == byte.MaxValue;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001F864 File Offset: 0x0001DA64
		public string humanizedData
		{
			get
			{
				return string.Format("{0, " + this.capacity.ToString() + "}", Convert.ToString(this.data, 2)).Replace(' ', '0');
			}
		}

		// Token: 0x170000F7 RID: 247
		public bool this[uint index]
		{
			get
			{
				return BitArrayUtilities.Get8(index, this.data);
			}
			set
			{
				BitArrayUtilities.Set8(index, ref this.data, value);
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001F8C5 File Offset: 0x0001DAC5
		public BitArray8(byte initValue)
		{
			this.data = initValue;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		public BitArray8(IEnumerable<uint> bitIndexTrue)
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
					this.data |= (byte)(1 << (int)num);
				}
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001F921 File Offset: 0x0001DB21
		public static BitArray8 operator ~(BitArray8 a)
		{
			return new BitArray8(~a.data);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001F930 File Offset: 0x0001DB30
		public static BitArray8 operator |(BitArray8 a, BitArray8 b)
		{
			return new BitArray8(a.data | b.data);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001F945 File Offset: 0x0001DB45
		public static BitArray8 operator &(BitArray8 a, BitArray8 b)
		{
			return new BitArray8(a.data & b.data);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001F95A File Offset: 0x0001DB5A
		public IBitArray BitAnd(IBitArray other)
		{
			return this & (BitArray8)other;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001F972 File Offset: 0x0001DB72
		public IBitArray BitOr(IBitArray other)
		{
			return this | (BitArray8)other;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001F98A File Offset: 0x0001DB8A
		public IBitArray BitNot()
		{
			return ~this;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001F99C File Offset: 0x0001DB9C
		public static bool operator ==(BitArray8 a, BitArray8 b)
		{
			return a.data == b.data;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001F9AC File Offset: 0x0001DBAC
		public static bool operator !=(BitArray8 a, BitArray8 b)
		{
			return a.data != b.data;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001F9BF File Offset: 0x0001DBBF
		public override bool Equals(object obj)
		{
			return obj is BitArray8 && ((BitArray8)obj).data == this.data;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001F9DE File Offset: 0x0001DBDE
		public override int GetHashCode()
		{
			return 1768953197 + this.data.GetHashCode();
		}

		// Token: 0x04000465 RID: 1125
		[SerializeField]
		private byte data;
	}
}
