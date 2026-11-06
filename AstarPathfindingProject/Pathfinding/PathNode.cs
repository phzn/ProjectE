using System;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x020000BC RID: 188
	public struct PathNode
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x0001DC23 File Offset: 0x0001BE23
		public static uint ReverseFractionAlongEdge(uint v)
		{
			return 15U - v;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001DC29 File Offset: 0x0001BE29
		public static uint QuantizeFractionAlongEdge(float v)
		{
			v *= 15f;
			v += 0.5f;
			return math.clamp((uint)v, 0U, 15U);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001DC47 File Offset: 0x0001BE47
		public static float UnQuantizeFractionAlongEdge(uint v)
		{
			return v * 0.06666667f;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001DC52 File Offset: 0x0001BE52
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001DC63 File Offset: 0x0001BE63
		public uint fractionAlongEdge
		{
			get
			{
				return (this.flags & 1006632960U) >> 26;
			}
			set
			{
				this.flags = (this.flags & 3288334335U) | ((value << 26) & 1006632960U);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0001DC82 File Offset: 0x0001BE82
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0001DC90 File Offset: 0x0001BE90
		public uint parentIndex
		{
			get
			{
				return this.flags & 67108863U;
			}
			set
			{
				this.flags = (this.flags & 4227858432U) | value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001DCA6 File Offset: 0x0001BEA6
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0001DCB7 File Offset: 0x0001BEB7
		public bool flag1
		{
			get
			{
				return (this.flags & 1073741824U) > 0U;
			}
			set
			{
				this.flags = (this.flags & 3221225471U) | (value ? 1073741824U : 0U);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001DCD7 File Offset: 0x0001BED7
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0001DCE8 File Offset: 0x0001BEE8
		public bool flag2
		{
			get
			{
				return (this.flags & 2147483648U) > 0U;
			}
			set
			{
				this.flags = (this.flags & 2147483647U) | (value ? 2147483648U : 0U);
			}
		}

		// Token: 0x040003F8 RID: 1016
		public ushort pathID;

		// Token: 0x040003F9 RID: 1017
		public ushort heapIndex;

		// Token: 0x040003FA RID: 1018
		private uint flags;

		// Token: 0x040003FB RID: 1019
		public static readonly PathNode Default = new PathNode
		{
			pathID = 0,
			heapIndex = ushort.MaxValue,
			flags = 0U
		};

		// Token: 0x040003FC RID: 1020
		private const uint ParentIndexMask = 67108863U;

		// Token: 0x040003FD RID: 1021
		private const int FractionAlongEdgeOffset = 26;

		// Token: 0x040003FE RID: 1022
		private const uint FractionAlongEdgeMask = 1006632960U;

		// Token: 0x040003FF RID: 1023
		public const int FractionAlongEdgeQuantization = 16;

		// Token: 0x04000400 RID: 1024
		private const int Flag1Offset = 30;

		// Token: 0x04000401 RID: 1025
		private const uint Flag1Mask = 1073741824U;

		// Token: 0x04000402 RID: 1026
		private const int Flag2Offset = 31;

		// Token: 0x04000403 RID: 1027
		private const uint Flag2Mask = 2147483648U;
	}
}
