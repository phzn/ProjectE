using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200001B RID: 27
	internal class RamerDouglasPeucker<T> where T : IList<float3>
	{
		// Token: 0x0600009F RID: 159 RVA: 0x0000441E File Offset: 0x0000261E
		public RamerDouglasPeucker(T points)
		{
			this.m_Points = points;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004430 File Offset: 0x00002630
		public void Reduce(List<float3> results, float epsilon)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.m_Epsilon = math.max(float.Epsilon, epsilon);
			this.m_KeepCount = this.m_Points.Count;
			this.m_Keep = new bool[this.m_KeepCount];
			for (int i = 0; i < this.m_KeepCount; i++)
			{
				this.Keep(i);
			}
			this.Reduce(new RamerDouglasPeucker<T>.Range(0, this.m_KeepCount));
			results.Clear();
			if (results.Capacity < this.m_KeepCount)
			{
				results.Capacity = this.m_KeepCount;
			}
			for (int j = 0; j < this.m_Keep.Length; j++)
			{
				if (this.m_Keep[j])
				{
					results.Add(this.m_Points[j]);
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004503 File Offset: 0x00002703
		private void Keep(int index)
		{
			this.m_Keep[index] = true;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004510 File Offset: 0x00002710
		private void Discard(RamerDouglasPeucker<T>.Range range)
		{
			this.m_KeepCount -= range.Count;
			for (int i = range.Start; i <= range.End; i++)
			{
				this.m_Keep[i] = false;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004550 File Offset: 0x00002750
		private void Reduce(RamerDouglasPeucker<T>.Range range)
		{
			if (range.Count < 3)
			{
				return;
			}
			ValueTuple<int, float> valueTuple = this.FindFarthest(range);
			if (valueTuple.Item2 < this.m_Epsilon)
			{
				this.Discard(new RamerDouglasPeucker<T>.Range(range.Start + 1, range.Count - 2));
				return;
			}
			this.Reduce(new RamerDouglasPeucker<T>.Range(range.Start, valueTuple.Item1 - range.Start + 1));
			this.Reduce(new RamerDouglasPeucker<T>.Range(valueTuple.Item1, range.End - valueTuple.Item1 + 1));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000045DC File Offset: 0x000027DC
		[return: TupleElementNames(new string[] { "index", "distance" })]
		private ValueTuple<int, float> FindFarthest(RamerDouglasPeucker<T>.Range range)
		{
			float num = 0f;
			int num2 = -1;
			for (int i = range.Start + 1; i < range.End; i++)
			{
				float num3 = SplineMath.DistancePointLine(this.m_Points[i], this.m_Points[range.Start], this.m_Points[range.End]);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			return new ValueTuple<int, float>(num2, num);
		}

		// Token: 0x04000040 RID: 64
		private T m_Points;

		// Token: 0x04000041 RID: 65
		private bool[] m_Keep;

		// Token: 0x04000042 RID: 66
		private float m_Epsilon;

		// Token: 0x04000043 RID: 67
		private int m_KeepCount;

		// Token: 0x02000056 RID: 86
		private struct Range
		{
			// Token: 0x170000B8 RID: 184
			// (get) Token: 0x0600033F RID: 831 RVA: 0x0001051B File Offset: 0x0000E71B
			public int End
			{
				get
				{
					return this.Start + this.Count - 1;
				}
			}

			// Token: 0x06000340 RID: 832 RVA: 0x0001052C File Offset: 0x0000E72C
			public Range(int start, int count)
			{
				this.Start = start;
				this.Count = count;
			}

			// Token: 0x06000341 RID: 833 RVA: 0x0001053C File Offset: 0x0000E73C
			public override string ToString()
			{
				return string.Format("[{0}, {1}]", this.Start, this.End);
			}

			// Token: 0x04000112 RID: 274
			public int Start;

			// Token: 0x04000113 RID: 275
			public int Count;
		}
	}
}
