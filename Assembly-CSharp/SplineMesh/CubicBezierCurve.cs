using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SplineMesh
{
	// Token: 0x02000180 RID: 384
	[Serializable]
	public class CubicBezierCurve
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00052F04 File Offset: 0x00051104
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x00052F0C File Offset: 0x0005110C
		public float Length { get; private set; }

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00052F18 File Offset: 0x00051118
		public CubicBezierCurve(SplineNode n1, SplineNode n2)
		{
			this.n1 = n1;
			this.n2 = n2;
			n1.Changed += this.ComputeSamples;
			n2.Changed += this.ComputeSamples;
			this.ComputeSamples(null, null);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00052F7D File Offset: 0x0005117D
		public void ConnectStart(SplineNode n1)
		{
			this.n1.Changed -= this.ComputeSamples;
			this.n1 = n1;
			n1.Changed += this.ComputeSamples;
			this.ComputeSamples(null, null);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00052FB7 File Offset: 0x000511B7
		public void ConnectEnd(SplineNode n2)
		{
			this.n2.Changed -= this.ComputeSamples;
			this.n2 = n2;
			n2.Changed += this.ComputeSamples;
			this.ComputeSamples(null, null);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00052FF1 File Offset: 0x000511F1
		public Vector3 GetInverseDirection()
		{
			return 2f * this.n2.Position - this.n2.Direction;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00053018 File Offset: 0x00051218
		private Vector3 GetLocation(float t)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return this.n1.Position * (num2 * num) + this.n1.Direction * (3f * num2 * t) + this.GetInverseDirection() * (3f * num * num3) + this.n2.Position * (num3 * t);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00053098 File Offset: 0x00051298
		private Vector3 GetTangent(float t)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return (this.n1.Position * -num2 + this.n1.Direction * (3f * num2 - 2f * num) + this.GetInverseDirection() * (-3f * num3 + 2f * t) + this.n2.Position * num3).normalized;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00053128 File Offset: 0x00051328
		private Vector3 GetUp(float t)
		{
			return Vector3.Lerp(this.n1.Up, this.n2.Up, t);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00053146 File Offset: 0x00051346
		private Vector2 GetScale(float t)
		{
			return Vector2.Lerp(this.n1.Scale, this.n2.Scale, t);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00053164 File Offset: 0x00051364
		private float GetRoll(float t)
		{
			return Mathf.Lerp(this.n1.Roll, this.n2.Roll, t);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00053184 File Offset: 0x00051384
		private void ComputeSamples(object sender, EventArgs e)
		{
			this.samples.Clear();
			this.Length = 0f;
			Vector3 vector = this.GetLocation(0f);
			for (float num = 0f; num < 1f; num += 0.033333335f)
			{
				Vector3 location = this.GetLocation(num);
				this.Length += Vector3.Distance(vector, location);
				vector = location;
				this.samples.Add(this.CreateSample(this.Length, num));
			}
			this.Length += Vector3.Distance(vector, this.GetLocation(1f));
			this.samples.Add(this.CreateSample(this.Length, 1f));
			if (this.Changed != null)
			{
				this.Changed.Invoke();
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0005324E File Offset: 0x0005144E
		private CurveSample CreateSample(float distance, float time)
		{
			return new CurveSample(this.GetLocation(time), this.GetTangent(time), this.GetUp(time), this.GetScale(time), this.GetRoll(time), distance, time, this);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0005327C File Offset: 0x0005147C
		public CurveSample GetSample(float time)
		{
			CubicBezierCurve.AssertTimeInBounds(time);
			CurveSample curveSample = this.samples[0];
			CurveSample curveSample2 = default(CurveSample);
			bool flag = false;
			foreach (CurveSample curveSample3 in this.samples)
			{
				if (curveSample3.timeInCurve >= time)
				{
					curveSample2 = curveSample3;
					flag = true;
					break;
				}
				curveSample = curveSample3;
			}
			if (!flag)
			{
				throw new Exception("Can't find curve samples.");
			}
			float num = ((curveSample2 == curveSample) ? 0f : ((time - curveSample.timeInCurve) / (curveSample2.timeInCurve - curveSample.timeInCurve)));
			return CurveSample.Lerp(curveSample, curveSample2, num);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00053338 File Offset: 0x00051538
		public CurveSample GetSampleAtDistance(float d)
		{
			if (d < 0f || d > this.Length)
			{
				throw new ArgumentException("Distance must be positive and less than curve length. Length = " + this.Length.ToString() + ", given distance was " + d.ToString());
			}
			CurveSample curveSample = this.samples[0];
			CurveSample curveSample2 = default(CurveSample);
			bool flag = false;
			foreach (CurveSample curveSample3 in this.samples)
			{
				if (curveSample3.distanceInCurve >= d)
				{
					curveSample2 = curveSample3;
					flag = true;
					break;
				}
				curveSample = curveSample3;
			}
			if (!flag)
			{
				throw new Exception("Can't find curve samples.");
			}
			float num = ((curveSample2 == curveSample) ? 0f : ((d - curveSample.distanceInCurve) / (curveSample2.distanceInCurve - curveSample.distanceInCurve)));
			return CurveSample.Lerp(curveSample, curveSample2, num);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0005342C File Offset: 0x0005162C
		private static void AssertTimeInBounds(float time)
		{
			if (time < 0f || time > 1f)
			{
				throw new ArgumentException("Time must be between 0 and 1 (was " + time.ToString() + ").");
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0005345C File Offset: 0x0005165C
		public CurveSample GetProjectionSample(Vector3 pointToProject)
		{
			float num = float.PositiveInfinity;
			int num2 = -1;
			int num3 = 0;
			foreach (CurveSample curveSample in this.samples)
			{
				float sqrMagnitude = (curveSample.location - pointToProject).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					num2 = num3;
				}
				num3++;
			}
			CurveSample curveSample2;
			CurveSample curveSample3;
			if (num2 == 0)
			{
				curveSample2 = this.samples[num2];
				curveSample3 = this.samples[num2 + 1];
			}
			else if (num2 == this.samples.Count - 1)
			{
				curveSample2 = this.samples[num2 - 1];
				curveSample3 = this.samples[num2];
			}
			else
			{
				float sqrMagnitude2 = (pointToProject - this.samples[num2 - 1].location).sqrMagnitude;
				float sqrMagnitude3 = (pointToProject - this.samples[num2 + 1].location).sqrMagnitude;
				if (sqrMagnitude2 < sqrMagnitude3)
				{
					curveSample2 = this.samples[num2 - 1];
					curveSample3 = this.samples[num2];
				}
				else
				{
					curveSample2 = this.samples[num2];
					curveSample3 = this.samples[num2 + 1];
				}
			}
			float num4 = (Vector3.Project(pointToProject - curveSample2.location, curveSample3.location - curveSample2.location) + curveSample2.location - curveSample2.location).sqrMagnitude / (curveSample3.location - curveSample2.location).sqrMagnitude;
			num4 = Mathf.Clamp(num4, 0f, 1f);
			return CurveSample.Lerp(curveSample2, curveSample3, num4);
		}

		// Token: 0x04000BFF RID: 3071
		private const int STEP_COUNT = 30;

		// Token: 0x04000C00 RID: 3072
		private const float T_STEP = 0.033333335f;

		// Token: 0x04000C01 RID: 3073
		private readonly List<CurveSample> samples = new List<CurveSample>(30);

		// Token: 0x04000C02 RID: 3074
		public SplineNode n1;

		// Token: 0x04000C03 RID: 3075
		public SplineNode n2;

		// Token: 0x04000C05 RID: 3077
		public UnityEvent Changed = new UnityEvent();
	}
}
