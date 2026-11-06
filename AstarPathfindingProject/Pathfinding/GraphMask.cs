using System;
using Pathfinding.Collections;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public struct GraphMask
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000BC12 File Offset: 0x00009E12
		public static GraphMask everything
		{
			get
			{
				return new GraphMask(uint.MaxValue);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000BC1A File Offset: 0x00009E1A
		public bool containsAllGraphs
		{
			get
			{
				return this.value == -1;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000BC25 File Offset: 0x00009E25
		public bool isPureBitmask
		{
			get
			{
				return this.value == -1 || ((long)this.value & (long)((ulong)int.MinValue)) == 0L;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000BC44 File Offset: 0x00009E44
		public GraphMask(uint value)
		{
			this.value = (int)value;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000BC50 File Offset: 0x00009E50
		public unsafe static GraphMask operator |(GraphMask lhs, GraphMask rhs)
		{
			if (lhs.value == -1 || rhs.value == -1)
			{
				return GraphMask.everything;
			}
			if (lhs.value == rhs.value)
			{
				return lhs;
			}
			uint num = (uint)(lhs.value | rhs.value);
			if ((num & 2147483648U) != 0U)
			{
				uint* ptr = stackalloc uint[(UIntPtr)12];
				*ptr = 255U;
				ptr[1] = 255U;
				ptr[2] = 255U;
				int num2 = 0;
				GraphMask.InsertMask(ptr, ref num2, (uint)lhs.value);
				GraphMask.InsertMask(ptr, ref num2, (uint)rhs.value);
				return new GraphMask(2147483648U | *ptr | (ptr[1] << 8) | (ptr[2] << 16));
			}
			if (num == 2147483647U)
			{
				return GraphMask.everything;
			}
			return new GraphMask(num);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000BD10 File Offset: 0x00009F10
		private unsafe static void InsertMask(uint* indices, ref int cnt, uint mask)
		{
			if ((mask & 2147483648U) == 0U)
			{
				if ((mask & 1073741824U) != 0U)
				{
					throw new ArgumentException("Cannot combine more than 3 large graph indices into a single mask. The GraphMask can only represent arbitrary combinations of graph indices smaller than 30. Larger graph indices have some limitations.");
				}
				int num = 0;
				while ((long)num < 30L)
				{
					if ((mask & (1U << num)) != 0U)
					{
						GraphMask.InsertSorted(indices, ref cnt, (uint)num);
					}
					num++;
				}
				return;
			}
			else
			{
				if ((mask & 1073741824U) != 0U)
				{
					throw new ArgumentException("Cannot combine an inverted mask with large graph indices. The GraphMask can only represent arbitrary combinations of graph indices smaller than 30. Larger graph indices have some limitations.");
				}
				uint num2 = mask & 255U;
				uint num3 = (mask >> 8) & 255U;
				uint num4 = (mask >> 16) & 255U;
				GraphMask.InsertSorted(indices, ref cnt, num2);
				GraphMask.InsertSorted(indices, ref cnt, num3);
				GraphMask.InsertSorted(indices, ref cnt, num4);
				return;
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		private unsafe static void InsertSorted(uint* indices, ref int cnt, uint value)
		{
			if (value == 255U)
			{
				return;
			}
			for (int i = 0; i < cnt; i++)
			{
				if (indices[i] == value)
				{
					return;
				}
			}
			if (cnt >= 3)
			{
				throw new ArgumentException("Cannot combine more than 3 large graph indices into a single mask. The GraphMask can only represent arbitrary combinations of graph indices smaller than 30. Larger graph indices have some limitations.");
			}
			cnt++;
			int num = cnt;
			while (num > 0 && value < indices[num - 1])
			{
				num--;
			}
			new UnsafeSpan<int>((void*)indices, cnt, Allocator.None).Move(num, num + 1, cnt - num - 1);
			indices[num] = value;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000BE28 File Offset: 0x0000A028
		public static GraphMask operator ~(GraphMask lhs)
		{
			if (lhs.value == -1)
			{
				return default(GraphMask);
			}
			if (((long)lhs.value & (long)((ulong)(-2147483648))) != 0L)
			{
				return new GraphMask((uint)(lhs.value ^ 1073741824));
			}
			return new GraphMask((uint)(~lhs.value & int.MaxValue));
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000BE7C File Offset: 0x0000A07C
		public static bool operator ==(GraphMask lhs, GraphMask rhs)
		{
			return lhs.value == rhs.value;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000BE8C File Offset: 0x0000A08C
		public static bool operator !=(GraphMask lhs, GraphMask rhs)
		{
			return lhs.value != rhs.value;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000BEA0 File Offset: 0x0000A0A0
		public override readonly bool Equals(object obj)
		{
			if (obj is GraphMask)
			{
				GraphMask graphMask = (GraphMask)obj;
				return this.value == graphMask.value;
			}
			return false;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public override readonly int GetHashCode()
		{
			return this.value;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public readonly bool Contains(NavGraph graph)
		{
			return this.Contains(graph.graphIndex);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000BEE4 File Offset: 0x0000A0E4
		public readonly bool Contains(uint graphIndex)
		{
			uint num = (uint)this.value;
			if (num == 4294967295U)
			{
				return true;
			}
			if ((num & 2147483648U) != 0U)
			{
				uint num2 = num & 255U;
				uint num3 = (num >> 8) & 255U;
				uint num4 = (num >> 16) & 255U;
				return (num2 == graphIndex || num3 == graphIndex || num4 == graphIndex) == ((num & 1073741824U) == 0U);
			}
			return ((num >> (int)Mathf.Min(graphIndex, 30f)) & 1U) > 0U;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000BF56 File Offset: 0x0000A156
		public static GraphMask FromGraph(NavGraph graph)
		{
			return GraphMask.FromGraphIndex(graph.graphIndex);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000BF63 File Offset: 0x0000A163
		public override string ToString()
		{
			return Convert.ToString(this.value, 2);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000BF71 File Offset: 0x0000A171
		public static GraphMask FromGraphIndex(uint graphIndex)
		{
			if (graphIndex < 30U)
			{
				return new GraphMask(1U << (int)graphIndex);
			}
			return new GraphMask(2147483648U | graphIndex | 65280U | 16711680U);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000BF9C File Offset: 0x0000A19C
		public static GraphMask FromGraphName(string graphName)
		{
			NavGraph navGraph = AstarPath.active.data.FindGraph((NavGraph g) => g.name == graphName);
			if (navGraph == null)
			{
				throw new ArgumentException("Could not find any graph with the name '" + graphName + "'");
			}
			return GraphMask.FromGraph(navGraph);
		}

		// Token: 0x04000189 RID: 393
		[SerializeField]
		private int value;

		// Token: 0x0400018A RID: 394
		private const uint LargeGraphIndicesBit = 2147483648U;

		// Token: 0x0400018B RID: 395
		private const uint InvertedBit = 1073741824U;

		// Token: 0x0400018C RID: 396
		private const uint SmallestLargeGraphIndex = 30U;

		// Token: 0x0400018D RID: 397
		private const uint RemainingGraphIndicesBit = 1073741824U;

		// Token: 0x0400018E RID: 398
		private const uint Everything = 4294967295U;
	}
}
