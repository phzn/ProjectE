using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	public class SplineData<T> : IEnumerable<DataPoint<T>>, IEnumerable, ISplineModificationHandler
	{
		// Token: 0x1700004B RID: 75
		public DataPoint<T> this[int index]
		{
			get
			{
				return this.m_DataPoints[index];
			}
			set
			{
				this.SetDataPoint(index, value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000740B File Offset: 0x0000560B
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00007413 File Offset: 0x00005613
		public PathIndexUnit PathIndexUnit
		{
			get
			{
				return this.m_IndexUnit;
			}
			set
			{
				this.m_IndexUnit = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000741C File Offset: 0x0000561C
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00007424 File Offset: 0x00005624
		public T DefaultValue
		{
			get
			{
				return this.m_DefaultValue;
			}
			set
			{
				this.m_DefaultValue = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000742D File Offset: 0x0000562D
		public int Count
		{
			get
			{
				return this.m_DataPoints.Count;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000743A File Offset: 0x0000563A
		public IEnumerable<float> Indexes
		{
			get
			{
				return this.m_DataPoints.Select<DataPoint<T>, float>((DataPoint<T> dp) => dp.Index);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600018C RID: 396 RVA: 0x00007468 File Offset: 0x00005668
		// (remove) Token: 0x0600018D RID: 397 RVA: 0x000074A0 File Offset: 0x000056A0
		[Obsolete("Use Changed instead.", false)]
		public event Action changed;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600018E RID: 398 RVA: 0x000074D8 File Offset: 0x000056D8
		// (remove) Token: 0x0600018F RID: 399 RVA: 0x00007510 File Offset: 0x00005710
		public event Action Changed;

		// Token: 0x06000190 RID: 400 RVA: 0x00007545 File Offset: 0x00005745
		public SplineData()
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000755F File Offset: 0x0000575F
		public SplineData(T init)
		{
			this.Add(0f, init);
			this.SetDirty();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000758C File Offset: 0x0000578C
		public SplineData(IEnumerable<DataPoint<T>> dataPoints)
		{
			foreach (DataPoint<T> dataPoint in dataPoints)
			{
				this.Add(dataPoint);
			}
			this.SetDirty();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000075F4 File Offset: 0x000057F4
		private void SetDirty()
		{
			Action action = this.changed;
			if (action != null)
			{
				action();
			}
			Action action2 = this.Changed;
			if (action2 == null)
			{
				return;
			}
			action2();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007617 File Offset: 0x00005817
		public void Add(float t, T data)
		{
			this.Add(new DataPoint<T>(t, data));
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007628 File Offset: 0x00005828
		public int Add(DataPoint<T> dataPoint)
		{
			int num = this.m_DataPoints.BinarySearch(0, this.Count, dataPoint, SplineData<T>.k_DataPointComparer);
			num = ((num < 0) ? (~num) : num);
			this.m_DataPoints.Insert(num, dataPoint);
			this.SetDirty();
			return num;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000766C File Offset: 0x0000586C
		public int AddDataPointWithDefaultValue(float t, bool useDefaultValue = false)
		{
			DataPoint<T> dataPoint = new DataPoint<T>(t, this.m_DefaultValue);
			if (this.Count == 0 || useDefaultValue)
			{
				return this.Add(dataPoint);
			}
			if (this.Count == 1)
			{
				dataPoint.Value = this.m_DataPoints[0].Value;
				return this.Add(dataPoint);
			}
			int num = this.m_DataPoints.BinarySearch(0, this.Count, dataPoint, SplineData<T>.k_DataPointComparer);
			num = ((num < 0) ? (~num) : num);
			dataPoint.Value = ((num == 0) ? this.m_DataPoints[0].Value : this.m_DataPoints[num - 1].Value);
			this.m_DataPoints.Insert(num, dataPoint);
			this.SetDirty();
			return num;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007735 File Offset: 0x00005935
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.m_DataPoints.RemoveAt(index);
			this.SetDirty();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007764 File Offset: 0x00005964
		public bool RemoveDataPoint(float t)
		{
			bool flag = this.m_DataPoints.Remove(this.m_DataPoints.FirstOrDefault<DataPoint<T>>((DataPoint<T> point) => Mathf.Approximately(point.Index, t)));
			if (flag)
			{
				this.SetDirty();
			}
			return flag;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000077AC File Offset: 0x000059AC
		public int MoveDataPoint(int index, float newIndex)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			DataPoint<T> dataPoint = this.m_DataPoints[index];
			if (Mathf.Approximately(newIndex, dataPoint.Index))
			{
				return index;
			}
			this.RemoveAt(index);
			dataPoint.Index = newIndex;
			return this.Add(dataPoint);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007805 File Offset: 0x00005A05
		public void Clear()
		{
			this.m_DataPoints.Clear();
			this.SetDirty();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007818 File Offset: 0x00005A18
		private static int Wrap(int value, int lowerBound, int upperBound)
		{
			int num = upperBound - lowerBound + 1;
			if (value < lowerBound)
			{
				value += num * ((lowerBound - value) / num + 1);
			}
			return lowerBound + (value - lowerBound) % num;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007843 File Offset: 0x00005A43
		private int ResolveBinaryIndex(int index, bool wrap)
		{
			index = ((index < 0) ? (~index) : index) - 1;
			if (wrap)
			{
				index = SplineData<T>.Wrap(index, 0, this.Count - 1);
			}
			return math.clamp(index, 0, this.Count - 1);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007878 File Offset: 0x00005A78
		private ValueTuple<int, int, float> GetIndex(float t, float splineLength, int knotCount, bool closed)
		{
			if (this.Count < 1)
			{
				return default(ValueTuple<int, int, float>);
			}
			this.SortIfNecessary();
			float num = splineLength;
			if (this.m_IndexUnit == PathIndexUnit.Normalized)
			{
				num = 1f;
			}
			else if (this.m_IndexUnit == PathIndexUnit.Knot)
			{
				num = (float)(closed ? knotCount : (knotCount - 1));
			}
			float num2 = math.ceil(this.m_DataPoints[this.m_DataPoints.Count - 1].Index / num) * num;
			float num3 = (closed ? math.max(num2, num) : num);
			if (closed)
			{
				if (t < 0f)
				{
					t = num3 + t % num3;
				}
				else
				{
					t %= num3;
				}
			}
			else
			{
				t = math.clamp(t, 0f, num3);
			}
			int num4 = this.m_DataPoints.BinarySearch(0, this.Count, new DataPoint<T>(t, default(T)), SplineData<T>.k_DataPointComparer);
			int num5 = this.ResolveBinaryIndex(num4, closed);
			int num6 = (closed ? ((num5 + 1) % this.Count) : math.clamp(num5 + 1, 0, this.Count - 1));
			float index = this.m_DataPoints[num5].Index;
			float num7 = this.m_DataPoints[num6].Index;
			if (num5 > num6)
			{
				num7 += num3;
			}
			if (t < index && closed)
			{
				t += num3;
			}
			if (index == num7)
			{
				return new ValueTuple<int, int, float>(num5, num6, index);
			}
			return new ValueTuple<int, int, float>(num5, num6, math.abs(math.max(0f, t - index) / (num7 - index)));
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007A02 File Offset: 0x00005C02
		public T Evaluate<TSpline, TInterpolator>(TSpline spline, float t, PathIndexUnit indexUnit, TInterpolator interpolator) where TSpline : ISpline where TInterpolator : IInterpolator<T>
		{
			if (indexUnit == this.m_IndexUnit)
			{
				return this.Evaluate<TSpline, TInterpolator>(spline, t, interpolator);
			}
			return this.Evaluate<TSpline, TInterpolator>(spline, spline.ConvertIndexUnit(t, indexUnit, this.m_IndexUnit), interpolator);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007A30 File Offset: 0x00005C30
		public T Evaluate<TSpline, TInterpolator>(TSpline spline, float t, TInterpolator interpolator) where TSpline : ISpline where TInterpolator : IInterpolator<T>
		{
			int count = spline.Count;
			if (count < 1 || this.m_DataPoints.Count == 0)
			{
				return default(T);
			}
			ValueTuple<int, int, float> index = this.GetIndex(t, spline.GetLength(), count, spline.Closed);
			DataPoint<T> dataPoint = this.m_DataPoints[index.Item1];
			DataPoint<T> dataPoint2 = this.m_DataPoints[index.Item2];
			return interpolator.Interpolate(dataPoint.Value, dataPoint2.Value, index.Item3);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007ACF File Offset: 0x00005CCF
		public void SetDataPoint(int index, DataPoint<T> value)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.RemoveAt(index);
			this.Add(value);
			this.SetDirty();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007AFE File Offset: 0x00005CFE
		public void SetDataPointNoSort(int index, DataPoint<T> value)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.m_NeedsSort = true;
			this.m_DataPoints[index] = value;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007B2C File Offset: 0x00005D2C
		public void SortIfNecessary()
		{
			if (!this.m_NeedsSort)
			{
				return;
			}
			this.m_NeedsSort = false;
			this.m_DataPoints.Sort();
			this.SetDirty();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007B4F File Offset: 0x00005D4F
		internal void ForceSort()
		{
			this.m_NeedsSort = true;
			this.SortIfNecessary();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007B60 File Offset: 0x00005D60
		public void ConvertPathUnit<TSplineType>(TSplineType spline, PathIndexUnit toUnit) where TSplineType : ISpline
		{
			if (toUnit == this.m_IndexUnit)
			{
				return;
			}
			for (int i = 0; i < this.m_DataPoints.Count; i++)
			{
				DataPoint<T> dataPoint = this.m_DataPoints[i];
				float num = spline.ConvertIndexUnit(dataPoint.Index, this.m_IndexUnit, toUnit);
				this.m_DataPoints[i] = new DataPoint<T>(num, dataPoint.Value);
			}
			this.m_IndexUnit = toUnit;
			this.SetDirty();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007BD5 File Offset: 0x00005DD5
		public float GetNormalizedInterpolation<TSplineType>(TSplineType spline, float t) where TSplineType : ISpline
		{
			return SplineUtility.GetNormalizedInterpolation<TSplineType>(spline, t, this.m_IndexUnit);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public IEnumerator<DataPoint<T>> GetEnumerator()
		{
			int i = 0;
			int c = this.Count;
			while (i < c)
			{
				yield return this.m_DataPoints[i];
				int num = i + 1;
				i = num;
			}
			yield break;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007BF3 File Offset: 0x00005DF3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007BFB File Offset: 0x00005DFB
		private static float WrapInt(float index, int lowerBound, int upperBound)
		{
			return (float)SplineData<T>.Wrap((int)math.floor(index), lowerBound, upperBound) + math.frac(index);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007C13 File Offset: 0x00005E13
		private static float ClampInt(float index, int lowerBound, int upperBound)
		{
			return (float)math.clamp((int)math.floor(index), lowerBound, upperBound) + math.frac(index);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007C2C File Offset: 0x00005E2C
		void ISplineModificationHandler.OnSplineModified(SplineModificationData data)
		{
			if (this.m_IndexUnit != PathIndexUnit.Knot)
			{
				return;
			}
			if (data.Modification == SplineModification.KnotModified || data.Modification == SplineModification.KnotReordered || data.Modification == SplineModification.Default)
			{
				return;
			}
			int knotIndex = data.KnotIndex;
			float prevCurveLength = data.PrevCurveLength;
			float nextCurveLength = data.NextCurveLength;
			List<int> list = new List<int>();
			int i = 0;
			int count = this.Count;
			while (i < count)
			{
				DataPoint<T> dataPoint = this.m_DataPoints[i];
				int num = (int)math.floor(dataPoint.Index);
				float num2 = dataPoint.Index - (float)num;
				if (data.Modification == SplineModification.KnotInserted)
				{
					float curveLength = data.Spline.GetCurveLength(data.Spline.PreviousIndex(knotIndex));
					if (num == knotIndex - 1)
					{
						if (num2 < curveLength / prevCurveLength)
						{
							dataPoint.Index = (float)num + num2 * (prevCurveLength / curveLength);
							goto IL_02A9;
						}
						dataPoint.Index = (float)(num + 1) + (num2 * prevCurveLength - curveLength) / (prevCurveLength - curveLength);
						goto IL_02A9;
					}
					else if (data.Spline.Closed && num == data.Spline.Count - 2 && knotIndex == 0)
					{
						if (num2 < curveLength / prevCurveLength)
						{
							dataPoint.Index = (float)(num + 1) + num2 * (prevCurveLength / curveLength);
							goto IL_02A9;
						}
						dataPoint.Index = (num2 * prevCurveLength - curveLength) / (prevCurveLength - curveLength);
						goto IL_02A9;
					}
					else
					{
						if (num >= knotIndex)
						{
							dataPoint.Index += 1f;
							goto IL_02A9;
						}
						goto IL_02A9;
					}
				}
				else if (data.Modification == SplineModification.KnotRemoved)
				{
					if (knotIndex == -1)
					{
						list.Add(i);
					}
					else
					{
						bool flag = num2 == 0f && num == knotIndex;
						bool flag2 = !data.Spline.Closed && ((num <= 0 && knotIndex == 0) || (knotIndex == data.Spline.Count && math.ceil(dataPoint.Index) >= (float)knotIndex));
						if (flag || flag2 || data.Spline.Count == 1)
						{
							list.Add(i);
							goto IL_02A9;
						}
						if (num == knotIndex - 1)
						{
							dataPoint.Index = (float)num + num2 * prevCurveLength / (prevCurveLength + nextCurveLength);
							goto IL_02A9;
						}
						if (num == knotIndex)
						{
							dataPoint.Index = (float)(num - 1) + (prevCurveLength + num2 * nextCurveLength) / (prevCurveLength + nextCurveLength);
							goto IL_02A9;
						}
						if (data.Spline.Closed && knotIndex == 0 && num == data.Spline.Count)
						{
							dataPoint.Index = (float)(num - 1) + num2 * prevCurveLength / (prevCurveLength + nextCurveLength);
							goto IL_02A9;
						}
						if (num >= knotIndex)
						{
							dataPoint.Index -= 1f;
							goto IL_02A9;
						}
						goto IL_02A9;
					}
				}
				else
				{
					if (!data.Spline.Closed && math.ceil(dataPoint.Index) >= (float)data.Spline.Count)
					{
						list.Add(i);
						goto IL_02A9;
					}
					goto IL_02A9;
				}
				IL_02B8:
				i++;
				continue;
				IL_02A9:
				this.m_DataPoints[i] = dataPoint;
				goto IL_02B8;
			}
			for (int j = list.Count - 1; j > -1; j--)
			{
				this.m_DataPoints.RemoveAt(list[j]);
			}
		}

		// Token: 0x0400007D RID: 125
		private static readonly DataPointComparer<DataPoint<T>> k_DataPointComparer = new DataPointComparer<DataPoint<T>>();

		// Token: 0x0400007E RID: 126
		[SerializeField]
		private PathIndexUnit m_IndexUnit = PathIndexUnit.Knot;

		// Token: 0x0400007F RID: 127
		[SerializeField]
		private T m_DefaultValue;

		// Token: 0x04000080 RID: 128
		[SerializeField]
		private List<DataPoint<T>> m_DataPoints = new List<DataPoint<T>>();

		// Token: 0x04000081 RID: 129
		[NonSerialized]
		private bool m_NeedsSort;
	}
}
