using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x0200010E RID: 270
	public struct ExCostSortedList4
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x0002DA15 File Offset: 0x0002BC15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ExCostSortedList4(float invalidCost)
		{
			this.costs = invalidCost;
			this.data = 0;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0002DA30 File Offset: 0x0002BC30
		public int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				for (int i = 3; i >= 0; i--)
				{
					if (this.costs[i] >= 0f)
					{
						return i + 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0002DA61 File Offset: 0x0002BC61
		public bool IsValid
		{
			get
			{
				return this.costs[0] >= 0f;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0002DA7C File Offset: 0x0002BC7C
		public bool Add(float cost, int item)
		{
			if (this.costs[3] >= 0f && cost > this.costs[3])
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				float num = this.costs[i];
				if (num < 0f)
				{
					this.costs[i] = cost;
					this.data[i] = item;
					return true;
				}
				if (cost < num)
				{
					for (int j = 2; j >= i; j--)
					{
						this.costs[j + 1] = this.costs[j];
						this.data[j + 1] = this.data[j];
					}
					this.costs[i] = cost;
					this.data[i] = item;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0002DB54 File Offset: 0x0002BD54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(int item)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.costs[i] >= 0f && this.data[i] == item)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0002DB94 File Offset: 0x0002BD94
		public int indexOf(int item)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.costs[i] >= 0f && this.data[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0002DBD4 File Offset: 0x0002BDD4
		public void RemoveItem(int item)
		{
			int num = -1;
			for (int i = 0; i < 4; i++)
			{
				if (this.costs[i] >= 0f && this.data[i] == item)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				return;
			}
			for (int j = num; j < 3; j++)
			{
				this.costs[j] = this.costs[j + 1];
				this.data[j] = this.data[j + 1];
			}
			this.costs[3] = -1f;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0002DC6B File Offset: 0x0002BE6B
		public float MinCost
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.costs[0];
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0002DC7C File Offset: 0x0002BE7C
		public float MaxCost
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				for (int i = 3; i >= 0; i--)
				{
					if (this.costs[i] >= 0f)
					{
						return this.costs[i];
					}
				}
				return 0f;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0002DCBC File Offset: 0x0002BEBC
		public override string ToString()
		{
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			for (int i = 0; i < this.Count; i++)
			{
				(ref fixedString512Bytes).Append<FixedString512Bytes>(string.Format("({0} : {1}) ", this.costs[i], this.data[i]));
			}
			return fixedString512Bytes.ToString();
		}

		// Token: 0x040008A1 RID: 2209
		internal float4 costs;

		// Token: 0x040008A2 RID: 2210
		internal int4 data;
	}
}
