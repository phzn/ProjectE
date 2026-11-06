using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public struct SplineRange : IEnumerable<int>, IEnumerable
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000C7B7 File Offset: 0x0000A9B7
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000C7BF File Offset: 0x0000A9BF
		public int Start
		{
			get
			{
				return this.m_Start;
			}
			set
			{
				this.m_Start = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		public int End
		{
			get
			{
				return this[this.Count - 1];
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		public int Count
		{
			get
			{
				return this.m_Count;
			}
			set
			{
				this.m_Count = math.max(value, 0);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000C7EF File Offset: 0x0000A9EF
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000C7F7 File Offset: 0x0000A9F7
		public SliceDirection Direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				this.m_Direction = value;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000C800 File Offset: 0x0000AA00
		public SplineRange(int start, int count)
		{
			this = new SplineRange(start, count, (count < 0) ? SliceDirection.Backward : SliceDirection.Forward);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000C812 File Offset: 0x0000AA12
		public SplineRange(int start, int count, SliceDirection direction)
		{
			this.m_Start = start;
			this.m_Count = math.abs(count);
			this.m_Direction = direction;
		}

		// Token: 0x170000A6 RID: 166
		public int this[int index]
		{
			get
			{
				if (this.Direction != SliceDirection.Backward)
				{
					return this.m_Start + index;
				}
				return this.m_Start - index;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000C84A File Offset: 0x0000AA4A
		public IEnumerator<int> GetEnumerator()
		{
			return new SplineRange.SplineRangeEnumerator(this);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000C85C File Offset: 0x0000AA5C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000C864 File Offset: 0x0000AA64
		public override string ToString()
		{
			return string.Format("{{{0}..{1}}}", this.Start, this.End);
		}

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private int m_Start;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		private int m_Count;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		private SliceDirection m_Direction;

		// Token: 0x0200006F RID: 111
		public struct SplineRangeEnumerator : IEnumerator<int>, IEnumerator, IDisposable
		{
			// Token: 0x06000381 RID: 897 RVA: 0x00010EEC File Offset: 0x0000F0EC
			public bool MoveNext()
			{
				int num = this.m_Index + 1;
				this.m_Index = num;
				return num < this.m_Count;
			}

			// Token: 0x06000382 RID: 898 RVA: 0x00010F12 File Offset: 0x0000F112
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x06000383 RID: 899 RVA: 0x00010F1B File Offset: 0x0000F11B
			public int Current
			{
				get
				{
					if (!this.m_Reverse)
					{
						return this.m_Start + this.m_Index;
					}
					return this.m_End - this.m_Index;
				}
			}

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x06000384 RID: 900 RVA: 0x00010F40 File Offset: 0x0000F140
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000385 RID: 901 RVA: 0x00010F50 File Offset: 0x0000F150
			public SplineRangeEnumerator(SplineRange range)
			{
				this.m_Index = -1;
				this.m_Reverse = range.Direction == SliceDirection.Backward;
				int start = range.Start;
				int num = (this.m_Reverse ? (range.Start - range.Count) : (range.Start + range.Count));
				this.m_Start = math.min(start, num);
				this.m_End = math.max(start, num);
				this.m_Count = range.Count;
			}

			// Token: 0x06000386 RID: 902 RVA: 0x00010FCC File Offset: 0x0000F1CC
			public void Dispose()
			{
			}

			// Token: 0x04000170 RID: 368
			private int m_Index;

			// Token: 0x04000171 RID: 369
			private int m_Start;

			// Token: 0x04000172 RID: 370
			private int m_End;

			// Token: 0x04000173 RID: 371
			private int m_Count;

			// Token: 0x04000174 RID: 372
			private bool m_Reverse;
		}
	}
}
