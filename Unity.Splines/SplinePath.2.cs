using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000036 RID: 54
	public class SplinePath<T> : ISpline, IReadOnlyList<BezierKnot>, IEnumerable<BezierKnot>, IEnumerable, IReadOnlyCollection<BezierKnot>, IHasEmptyCurves where T : ISpline
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000C342 File Offset: 0x0000A542
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000C34A File Offset: 0x0000A54A
		public IReadOnlyList<T> Slices
		{
			get
			{
				return this.m_Splines;
			}
			set
			{
				this.m_Splines = value.ToArray<T>();
				this.BuildSplitData();
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C35E File Offset: 0x0000A55E
		public SplinePath(IEnumerable<T> slices)
		{
			this.m_Splines = slices.ToArray<T>();
			this.BuildSplitData();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000C378 File Offset: 0x0000A578
		private void BuildSplitData()
		{
			this.m_Splits = new int[this.m_Splines.Length];
			int i = 0;
			int num = this.m_Splits.Length;
			int num2 = 0;
			while (i < num)
			{
				this.m_Splits[i] = (num2 += this.m_Splines[i].Count + (this.m_Splines[i].Closed ? 1 : 0)) - 1;
				i++;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000C3F6 File Offset: 0x0000A5F6
		public IEnumerator<BezierKnot> GetEnumerator()
		{
			foreach (T t in this.m_Splines)
			{
				foreach (BezierKnot bezierKnot in t)
				{
					yield return bezierKnot;
				}
				IEnumerator<BezierKnot> enumerator = null;
			}
			T[] array = null;
			yield break;
			yield break;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000C405 File Offset: 0x0000A605
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000C410 File Offset: 0x0000A610
		public int Count
		{
			get
			{
				int num = 0;
				foreach (T t in this.m_Splines)
				{
					num += t.Count + (t.Closed ? 1 : 0);
				}
				return num;
			}
		}

		// Token: 0x1700009E RID: 158
		public BezierKnot this[int index]
		{
			get
			{
				return this[this.GetBranchKnotIndex(index)];
			}
		}

		// Token: 0x1700009F RID: 159
		public BezierKnot this[SplineKnotIndex index]
		{
			get
			{
				T t = this.m_Splines[index.Spline];
				int num = (t.Closed ? (index.Knot % t.Count) : index.Knot);
				return t[num];
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000C4CC File Offset: 0x0000A6CC
		internal SplineKnotIndex GetBranchKnotIndex(int knot)
		{
			knot = (this.Closed ? (knot % this.Count) : math.clamp(knot, 0, this.Count));
			int i = 0;
			int num = 0;
			while (i < this.m_Splines.Length)
			{
				T t = this.m_Splines[i];
				int num2 = t.Count + (t.Closed ? 1 : 0);
				if (knot < num + num2)
				{
					return new SplineKnotIndex(i, math.max(0, knot - num));
				}
				num += num2;
				i++;
			}
			int num3 = this.m_Splines.Length - 1;
			T[] splines = this.m_Splines;
			return new SplineKnotIndex(num3, splines[splines.Length - 1].Count - 1);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000C586 File Offset: 0x0000A786
		public bool Closed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000C58C File Offset: 0x0000A78C
		public float GetLength()
		{
			float num = 0f;
			int i = 0;
			int num2 = (this.Closed ? this.Count : (this.Count - 1));
			while (i < num2)
			{
				num += this.GetCurveLength(i);
				i++;
			}
			return num;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000C5CF File Offset: 0x0000A7CF
		public IReadOnlyList<int> EmptyCurves
		{
			get
			{
				return this.m_Splits;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000C5D7 File Offset: 0x0000A7D7
		private bool IsDegenerate(int index)
		{
			return Array.BinarySearch<int>(this.m_Splits, index) >= 0;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		public BezierCurve GetCurve(int knot)
		{
			SplineKnotIndex branchKnotIndex = this.GetBranchKnotIndex(knot);
			if (this.IsDegenerate(knot))
			{
				BezierKnot bezierKnot = new BezierKnot(this[branchKnotIndex].Position);
				return new BezierCurve(bezierKnot, bezierKnot);
			}
			BezierKnot bezierKnot2 = this[branchKnotIndex];
			BezierKnot bezierKnot3 = this.Next(knot);
			return new BezierCurve(bezierKnot2, bezierKnot3);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000C638 File Offset: 0x0000A838
		public float GetCurveLength(int index)
		{
			if (this.IsDegenerate(index))
			{
				return 0f;
			}
			SplineKnotIndex branchKnotIndex = this.GetBranchKnotIndex(index);
			T t = this.m_Splines[branchKnotIndex.Spline];
			if (branchKnotIndex.Spline >= this.m_Splines.Length - 1 && branchKnotIndex.Knot >= t.Count - 1)
			{
				return CurveUtility.CalculateLength(this.GetCurve(index), 30);
			}
			return t.GetCurveLength(branchKnotIndex.Knot);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		public float3 GetCurveUpVector(int index, float t)
		{
			if (this.IsDegenerate(index))
			{
				return 0f;
			}
			SplineKnotIndex branchKnotIndex = this.GetBranchKnotIndex(index);
			T t2 = this.m_Splines[branchKnotIndex.Spline];
			if (branchKnotIndex.Spline >= this.m_Splines.Length - 1 && branchKnotIndex.Knot >= t2.Count - 1)
			{
				BezierKnot bezierKnot = this[branchKnotIndex];
				BezierKnot bezierKnot2 = this.Next(index);
				BezierCurve bezierCurve = new BezierCurve(bezierKnot, bezierKnot2);
				float3 @float = math.rotate(bezierKnot.Rotation, math.up());
				float3 float2 = math.rotate(bezierKnot2.Rotation, math.up());
				return CurveUtility.EvaluateUpVector(bezierCurve, t, @float, float2, true);
			}
			return t2.GetCurveUpVector(branchKnotIndex.Knot, t);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000C77C File Offset: 0x0000A97C
		public float GetCurveInterpolation(int curveIndex, float curveDistance)
		{
			SplineKnotIndex branchKnotIndex = this.GetBranchKnotIndex(curveIndex);
			T t = this.m_Splines[branchKnotIndex.Spline];
			return t.GetCurveInterpolation(branchKnotIndex.Knot, curveDistance);
		}

		// Token: 0x040000D8 RID: 216
		private T[] m_Splines;

		// Token: 0x040000D9 RID: 217
		private int[] m_Splits;
	}
}
