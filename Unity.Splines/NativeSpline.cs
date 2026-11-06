using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000017 RID: 23
	public struct NativeSpline : ISpline, IReadOnlyList<BezierKnot>, IEnumerable<BezierKnot>, IEnumerable, IReadOnlyCollection<BezierKnot>, IDisposable
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003F61 File Offset: 0x00002161
		public NativeArray<BezierKnot> Knots
		{
			get
			{
				return this.m_Knots;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003F69 File Offset: 0x00002169
		public NativeArray<BezierCurve> Curves
		{
			get
			{
				return this.m_Curves;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003F71 File Offset: 0x00002171
		public bool Closed
		{
			get
			{
				return this.m_Closed;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003F79 File Offset: 0x00002179
		public int Count
		{
			get
			{
				return this.m_Knots.Length;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003F86 File Offset: 0x00002186
		public float GetLength()
		{
			return this.m_Length;
		}

		// Token: 0x17000022 RID: 34
		public BezierKnot this[int index]
		{
			get
			{
				return this.m_Knots[index];
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003F9C File Offset: 0x0000219C
		public IEnumerator<BezierKnot> GetEnumerator()
		{
			return this.m_Knots.GetEnumerator();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003FAE File Offset: 0x000021AE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003FB6 File Offset: 0x000021B6
		public NativeSpline(ISpline spline, Allocator allocator = Allocator.Temp)
		{
			this = new NativeSpline(spline, float4x4.identity, false, allocator);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003FC6 File Offset: 0x000021C6
		public NativeSpline(ISpline spline, bool cacheUpVectors, Allocator allocator = Allocator.Temp)
		{
			this = new NativeSpline(spline, float4x4.identity, cacheUpVectors, allocator);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003FD8 File Offset: 0x000021D8
		public NativeSpline(ISpline spline, float4x4 transform, Allocator allocator = Allocator.Temp)
		{
			IHasEmptyCurves hasEmptyCurves = spline as IHasEmptyCurves;
			this = new NativeSpline(spline, (hasEmptyCurves != null) ? hasEmptyCurves.EmptyCurves : null, spline.Closed, transform, false, allocator);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004008 File Offset: 0x00002208
		public NativeSpline(ISpline spline, float4x4 transform, bool cacheUpVectors, Allocator allocator = Allocator.Temp)
		{
			IHasEmptyCurves hasEmptyCurves = spline as IHasEmptyCurves;
			this = new NativeSpline(spline, (hasEmptyCurves != null) ? hasEmptyCurves.EmptyCurves : null, spline.Closed, transform, cacheUpVectors, allocator);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004039 File Offset: 0x00002239
		public NativeSpline(IReadOnlyList<BezierKnot> knots, bool closed, float4x4 transform, Allocator allocator = Allocator.Temp)
		{
			this = new NativeSpline(knots, null, closed, transform, false, allocator);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004048 File Offset: 0x00002248
		public NativeSpline(IReadOnlyList<BezierKnot> knots, bool closed, float4x4 transform, bool cacheUpVectors, Allocator allocator = Allocator.Temp)
		{
			this = new NativeSpline(knots, null, closed, transform, cacheUpVectors, allocator);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004058 File Offset: 0x00002258
		public NativeSpline(IReadOnlyList<BezierKnot> knots, IReadOnlyList<int> splits, bool closed, float4x4 transform, Allocator allocator = Allocator.Temp)
		{
			this = new NativeSpline(knots, splits, closed, transform, false, allocator);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004068 File Offset: 0x00002268
		public NativeSpline(IReadOnlyList<BezierKnot> knots, IReadOnlyList<int> splits, bool closed, float4x4 transform, bool cacheUpVectors, Allocator allocator = Allocator.Temp)
		{
			int count = knots.Count;
			this.m_Knots = new NativeArray<BezierKnot>(count, allocator, NativeArrayOptions.ClearMemory);
			this.m_Curves = new NativeArray<BezierCurve>(count, allocator, NativeArrayOptions.ClearMemory);
			this.m_SegmentLengthsLookupTable = new NativeArray<DistanceToInterpolation>(count * 30, allocator, NativeArrayOptions.ClearMemory);
			this.m_Closed = closed;
			this.m_Length = 0f;
			this.m_UpVectorsLookupTable = new NativeArray<float3>(cacheUpVectors ? (count * 30) : 0, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<DistanceToInterpolation> nativeArray = new NativeArray<DistanceToInterpolation>(30, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<float3> nativeArray2 = (cacheUpVectors ? new NativeArray<float3>(30, Allocator.Temp, NativeArrayOptions.ClearMemory) : default(NativeArray<float3>));
			if (count > 0)
			{
				BezierKnot bezierKnot = knots[0].Transform(transform);
				for (int i = 0; i < count; i++)
				{
					BezierKnot bezierKnot2 = knots[(i + 1) % count].Transform(transform);
					this.m_Knots[i] = bezierKnot;
					if (splits != null && splits.Contains(i))
					{
						this.m_Curves[i] = new BezierCurve(new BezierKnot(bezierKnot.Position), new BezierKnot(bezierKnot.Position));
						float3 @float = (cacheUpVectors ? math.rotate(bezierKnot.Rotation, math.up()) : float3.zero);
						for (int j = 0; j < 30; j++)
						{
							nativeArray[j] = default(DistanceToInterpolation);
							if (cacheUpVectors)
							{
								nativeArray2[j] = @float;
							}
						}
					}
					else
					{
						this.m_Curves[i] = new BezierCurve(bezierKnot, bezierKnot2);
						CurveUtility.CalculateCurveLengths(this.m_Curves[i], nativeArray);
						if (cacheUpVectors)
						{
							float3 float2 = math.rotate(bezierKnot.Rotation, math.up());
							float3 float3 = math.rotate(bezierKnot2.Rotation, math.up());
							CurveUtility.EvaluateUpVectors(this.m_Curves[i], float2, float3, nativeArray2);
						}
					}
					if (this.m_Closed || i < count - 1)
					{
						this.m_Length += nativeArray[29].Distance;
					}
					for (int k = 0; k < 30; k++)
					{
						this.m_SegmentLengthsLookupTable[i * 30 + k] = nativeArray[k];
						if (cacheUpVectors)
						{
							this.m_UpVectorsLookupTable[i * 30 + k] = nativeArray2[k];
						}
					}
					bezierKnot = bezierKnot2;
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000042C6 File Offset: 0x000024C6
		public BezierCurve GetCurve(int index)
		{
			return this.m_Curves[index];
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000042D4 File Offset: 0x000024D4
		public float GetCurveLength(int curveIndex)
		{
			return this.m_SegmentLengthsLookupTable[curveIndex * 30 + 30 - 1].Distance;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000042F0 File Offset: 0x000024F0
		public float3 GetCurveUpVector(int index, float t)
		{
			if (this.m_UpVectorsLookupTable.Length == 0)
			{
				return this.CalculateUpVector(index, t);
			}
			int num = index * 30;
			float num2 = 0.03448276f;
			float num3 = 0f;
			for (int i = 0; i < 30; i++)
			{
				if (t <= num3 + num2)
				{
					return math.lerp(this.m_UpVectorsLookupTable[num + i], this.m_UpVectorsLookupTable[num + i + 1], (t - num3) / num2);
				}
				num3 += num2;
			}
			return this.m_UpVectorsLookupTable[num + 30 - 1];
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000437A File Offset: 0x0000257A
		public void Dispose()
		{
			this.m_Knots.Dispose();
			this.m_Curves.Dispose();
			this.m_SegmentLengthsLookupTable.Dispose();
			this.m_UpVectorsLookupTable.Dispose();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000043A8 File Offset: 0x000025A8
		public float GetCurveInterpolation(int curveIndex, float curveDistance)
		{
			if (curveIndex < 0 || curveIndex >= this.m_SegmentLengthsLookupTable.Length || curveDistance <= 0f)
			{
				return 0f;
			}
			float curveLength = this.GetCurveLength(curveIndex);
			if (curveDistance >= curveLength)
			{
				return 1f;
			}
			int num = curveIndex * 30;
			return CurveUtility.GetDistanceToInterpolation<NativeSpline.Slice<DistanceToInterpolation>>(new NativeSpline.Slice<DistanceToInterpolation>(this.m_SegmentLengthsLookupTable, num, 30), curveDistance);
		}

		// Token: 0x04000031 RID: 49
		[ReadOnly]
		private NativeArray<BezierKnot> m_Knots;

		// Token: 0x04000032 RID: 50
		[ReadOnly]
		private NativeArray<BezierCurve> m_Curves;

		// Token: 0x04000033 RID: 51
		[ReadOnly]
		private NativeArray<DistanceToInterpolation> m_SegmentLengthsLookupTable;

		// Token: 0x04000034 RID: 52
		[ReadOnly]
		private NativeArray<float3> m_UpVectorsLookupTable;

		// Token: 0x04000035 RID: 53
		private bool m_Closed;

		// Token: 0x04000036 RID: 54
		private float m_Length;

		// Token: 0x04000037 RID: 55
		private const int k_SegmentResolution = 30;

		// Token: 0x02000055 RID: 85
		private struct Slice<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : struct
		{
			// Token: 0x0600033A RID: 826 RVA: 0x000104D6 File Offset: 0x0000E6D6
			public Slice(NativeArray<T> array, int start, int count)
			{
				this.m_Slice = new NativeSlice<T>(array, start, count);
			}

			// Token: 0x0600033B RID: 827 RVA: 0x000104E6 File Offset: 0x0000E6E6
			public IEnumerator<T> GetEnumerator()
			{
				return this.m_Slice.GetEnumerator();
			}

			// Token: 0x0600033C RID: 828 RVA: 0x000104F8 File Offset: 0x0000E6F8
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x0600033D RID: 829 RVA: 0x00010500 File Offset: 0x0000E700
			public int Count
			{
				get
				{
					return this.m_Slice.Length;
				}
			}

			// Token: 0x170000B7 RID: 183
			public T this[int index]
			{
				get
				{
					return this.m_Slice[index];
				}
			}

			// Token: 0x04000111 RID: 273
			private NativeSlice<T> m_Slice;
		}
	}
}
