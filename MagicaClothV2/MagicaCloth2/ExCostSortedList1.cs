using System;
using System.Runtime.CompilerServices;

namespace MagicaCloth2
{
	// Token: 0x0200010D RID: 269
	public struct ExCostSortedList1 : IComparable<ExCostSortedList1>
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x0002D960 File Offset: 0x0002BB60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ExCostSortedList1(float invalidCost)
		{
			this.cost = invalidCost;
			this.data = 0;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0002D970 File Offset: 0x0002BB70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ExCostSortedList1(float invalidCost, int initData)
		{
			this.cost = invalidCost;
			this.data = initData;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0002D980 File Offset: 0x0002BB80
		public bool IsValid
		{
			get
			{
				return this.cost >= 0f;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0002D992 File Offset: 0x0002BB92
		public int Count
		{
			get
			{
				if (!this.IsValid)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0002D99F File Offset: 0x0002BB9F
		public float Cost
		{
			get
			{
				return this.cost;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0002D9A7 File Offset: 0x0002BBA7
		public int Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0002D9AF File Offset: 0x0002BBAF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(float cost, int item)
		{
			if (!this.IsValid || cost < this.cost)
			{
				this.cost = cost;
				this.data = item;
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0002D9D0 File Offset: 0x0002BBD0
		public int CompareTo(ExCostSortedList1 other)
		{
			if (this.cost == other.cost)
			{
				return 0;
			}
			if (this.cost >= other.cost)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0002D9F3 File Offset: 0x0002BBF3
		public override string ToString()
		{
			return string.Format("({0} : {1})", this.cost, this.data);
		}

		// Token: 0x0400089F RID: 2207
		internal float cost;

		// Token: 0x040008A0 RID: 2208
		internal int data;
	}
}
