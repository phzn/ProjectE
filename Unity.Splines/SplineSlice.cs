using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200003A RID: 58
	public struct SplineSlice<T> : ISpline, IReadOnlyList<BezierKnot>, IEnumerable<BezierKnot>, IEnumerable, IReadOnlyCollection<BezierKnot> where T : ISpline
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000CB70 File Offset: 0x0000AD70
		public int Count
		{
			get
			{
				if (this.Spline.Closed)
				{
					return math.clamp(this.Range.Count, 0, this.Spline.Count + 1);
				}
				if (this.Range.Direction == SliceDirection.Backward)
				{
					return math.clamp(this.Range.Count, 0, this.Range.Start + 1);
				}
				return math.clamp(this.Range.Count, 0, this.Spline.Count - this.Range.Start);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000CC10 File Offset: 0x0000AE10
		public bool Closed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000CC13 File Offset: 0x0000AE13
		private static BezierKnot FlipTangents(BezierKnot knot)
		{
			return new BezierKnot(knot.Position, knot.TangentOut, knot.TangentIn, knot.Rotation);
		}

		// Token: 0x170000AC RID: 172
		public BezierKnot this[int index]
		{
			get
			{
				int num = this.Range[index];
				num = (num + this.Spline.Count) % this.Spline.Count;
				if (this.Range.Direction != SliceDirection.Backward)
				{
					return this.Spline[num].Transform(this.Transform);
				}
				return SplineSlice<T>.FlipTangents(this.Spline[num]).Transform(this.Transform);
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
		public IEnumerator<BezierKnot> GetEnumerator()
		{
			int i = 0;
			int c = this.Range.Count;
			while (i < c)
			{
				yield return this[i];
				int num = i + 1;
				i = num;
			}
			yield break;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		public SplineSlice(T spline, SplineRange range)
		{
			this = new SplineSlice<T>(spline, range, float4x4.identity);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000CCF3 File Offset: 0x0000AEF3
		public SplineSlice(T spline, SplineRange range, float4x4 transform)
		{
			this.Spline = spline;
			this.Range = range;
			this.Transform = transform;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000CD0C File Offset: 0x0000AF0C
		public float GetLength()
		{
			float num = 0f;
			int i = 0;
			int count = this.Count;
			while (i < count)
			{
				num += this.GetCurveLength(i);
				i++;
			}
			return num;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000CD40 File Offset: 0x0000AF40
		public BezierCurve GetCurve(int index)
		{
			int num = math.min(math.max(index + 1, 0), this.Range.Count - 1);
			BezierKnot bezierKnot = this[index];
			BezierKnot bezierKnot2 = this[num];
			if (index == num)
			{
				return new BezierCurve(bezierKnot.Position, bezierKnot2.Position);
			}
			return new BezierCurve(bezierKnot, bezierKnot2);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000CD96 File Offset: 0x0000AF96
		public float GetCurveLength(int index)
		{
			return CurveUtility.CalculateLength(this.GetCurve(index), 30);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000CDA6 File Offset: 0x0000AFA6
		public float3 GetCurveUpVector(int index, float t)
		{
			return this.CalculateUpVector(index, t);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000CDB5 File Offset: 0x0000AFB5
		public float GetCurveInterpolation(int curveIndex, float curveDistance)
		{
			return CurveUtility.GetDistanceToInterpolation(this.GetCurve(curveIndex), curveDistance);
		}

		// Token: 0x040000E9 RID: 233
		public T Spline;

		// Token: 0x040000EA RID: 234
		public SplineRange Range;

		// Token: 0x040000EB RID: 235
		public float4x4 Transform;
	}
}
