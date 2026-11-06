using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000292 RID: 658
	public class PathInterpolator
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0006318F File Offset: 0x0006138F
		public bool valid
		{
			get
			{
				return this.path != null;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0006319A File Offset: 0x0006139A
		public PathInterpolator.Cursor start
		{
			get
			{
				return PathInterpolator.Cursor.StartOfPath(this);
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000631A4 File Offset: 0x000613A4
		public PathInterpolator.Cursor AtDistanceFromStart(float distance)
		{
			PathInterpolator.Cursor start = this.start;
			start.distance = distance;
			return start;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x000631C4 File Offset: 0x000613C4
		public void SetPath(List<Vector3> path)
		{
			this.version++;
			if (this.path == null)
			{
				this.path = new List<Vector3>();
			}
			this.path.Clear();
			if (path == null)
			{
				this.totalDistance = float.PositiveInfinity;
				return;
			}
			if (path.Count < 2)
			{
				throw new ArgumentException("Path must have a length of at least 2");
			}
			Vector3 vector = path[0];
			this.totalDistance = 0f;
			this.path.Capacity = Mathf.Max(this.path.Capacity, path.Count);
			this.path.Add(path[0]);
			for (int i = 1; i < path.Count; i++)
			{
				Vector3 vector2 = path[i];
				if (vector2 != vector)
				{
					this.totalDistance += (vector2 - vector).magnitude;
					this.path.Add(vector2);
					vector = vector2;
				}
			}
			if (this.path.Count < 2)
			{
				this.path.Add(path[0]);
			}
			if (float.IsNaN(this.totalDistance))
			{
				throw new ArgumentException("Path contains NaN values");
			}
		}

		// Token: 0x04000BEA RID: 3050
		private List<Vector3> path;

		// Token: 0x04000BEB RID: 3051
		private int version = 1;

		// Token: 0x04000BEC RID: 3052
		private float totalDistance;

		// Token: 0x02000293 RID: 659
		public struct Cursor
		{
			// Token: 0x17000263 RID: 611
			// (get) Token: 0x0600105D RID: 4189 RVA: 0x000632F9 File Offset: 0x000614F9
			// (set) Token: 0x0600105E RID: 4190 RVA: 0x00063301 File Offset: 0x00061501
			private int segmentIndex { readonly get; set; }

			// Token: 0x17000264 RID: 612
			// (get) Token: 0x0600105F RID: 4191 RVA: 0x0006330A File Offset: 0x0006150A
			public int segmentCount
			{
				get
				{
					this.AssertValid();
					return this.interpolator.path.Count - 1;
				}
			}

			// Token: 0x17000265 RID: 613
			// (get) Token: 0x06001060 RID: 4192 RVA: 0x00063324 File Offset: 0x00061524
			public Vector3 endPoint
			{
				get
				{
					this.AssertValid();
					return this.interpolator.path[this.interpolator.path.Count - 1];
				}
			}

			// Token: 0x17000266 RID: 614
			// (get) Token: 0x06001061 RID: 4193 RVA: 0x0006334E File Offset: 0x0006154E
			// (set) Token: 0x06001062 RID: 4194 RVA: 0x00063377 File Offset: 0x00061577
			public float fractionAlongCurrentSegment
			{
				get
				{
					if (this.currentSegmentLength <= 0f)
					{
						return 1f;
					}
					return (this.currentDistance - this.distanceToSegmentStart) / this.currentSegmentLength;
				}
				set
				{
					this.currentDistance = this.distanceToSegmentStart + Mathf.Clamp01(value) * this.currentSegmentLength;
				}
			}

			// Token: 0x06001063 RID: 4195 RVA: 0x00063394 File Offset: 0x00061594
			public static PathInterpolator.Cursor StartOfPath(PathInterpolator interpolator)
			{
				if (!interpolator.valid)
				{
					throw new InvalidOperationException("PathInterpolator has no path set");
				}
				return new PathInterpolator.Cursor
				{
					interpolator = interpolator,
					version = interpolator.version,
					segmentIndex = 0,
					currentDistance = 0f,
					distanceToSegmentStart = 0f,
					currentSegmentLength = (interpolator.path[1] - interpolator.path[0]).magnitude
				};
			}

			// Token: 0x17000267 RID: 615
			// (get) Token: 0x06001064 RID: 4196 RVA: 0x0006341E File Offset: 0x0006161E
			public bool valid
			{
				get
				{
					return this.interpolator != null && this.interpolator.version == this.version;
				}
			}

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06001065 RID: 4197 RVA: 0x0006343D File Offset: 0x0006163D
			public Vector3 tangent
			{
				get
				{
					this.AssertValid();
					return this.interpolator.path[this.segmentIndex + 1] - this.interpolator.path[this.segmentIndex];
				}
			}

			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06001066 RID: 4198 RVA: 0x00063478 File Offset: 0x00061678
			// (set) Token: 0x06001067 RID: 4199 RVA: 0x00063492 File Offset: 0x00061692
			public float remainingDistance
			{
				get
				{
					this.AssertValid();
					return this.interpolator.totalDistance - this.distance;
				}
				set
				{
					this.AssertValid();
					this.distance = this.interpolator.totalDistance - value;
				}
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06001068 RID: 4200 RVA: 0x000634AD File Offset: 0x000616AD
			// (set) Token: 0x06001069 RID: 4201 RVA: 0x000634B8 File Offset: 0x000616B8
			public float distance
			{
				get
				{
					return this.currentDistance;
				}
				set
				{
					this.AssertValid();
					this.currentDistance = value;
					while (this.currentDistance < this.distanceToSegmentStart)
					{
						if (this.segmentIndex <= 0)
						{
							break;
						}
						this.PrevSegment();
					}
					while (this.currentDistance > this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.interpolator.path.Count - 2)
					{
						this.NextSegment();
					}
				}
			}

			// Token: 0x1700026B RID: 619
			// (get) Token: 0x0600106A RID: 4202 RVA: 0x00063528 File Offset: 0x00061728
			public Vector3 position
			{
				get
				{
					this.AssertValid();
					float num = ((this.currentSegmentLength > 0.0001f) ? ((this.currentDistance - this.distanceToSegmentStart) / this.currentSegmentLength) : 0f);
					return Vector3.Lerp(this.interpolator.path[this.segmentIndex], this.interpolator.path[this.segmentIndex + 1], num);
				}
			}

			// Token: 0x0600106B RID: 4203 RVA: 0x00063598 File Offset: 0x00061798
			public void GetRemainingPath(List<Vector3> buffer)
			{
				this.AssertValid();
				buffer.Add(this.position);
				for (int i = this.segmentIndex + 1; i < this.interpolator.path.Count; i++)
				{
					buffer.Add(this.interpolator.path[i]);
				}
			}

			// Token: 0x0600106C RID: 4204 RVA: 0x000635F0 File Offset: 0x000617F0
			private void AssertValid()
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The cursor has been invalidated because SetPath has been called on the interpolator. Please create a new cursor.");
				}
			}

			// Token: 0x0600106D RID: 4205 RVA: 0x00063608 File Offset: 0x00061808
			public void GetTangents(out Vector3 t1, out Vector3 t2)
			{
				this.AssertValid();
				bool flag = this.currentDistance <= this.distanceToSegmentStart + 0.001f;
				bool flag2 = this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength - 0.001f;
				if (flag || flag2)
				{
					int num;
					int num2;
					if (flag)
					{
						num = ((this.segmentIndex > 0) ? (this.segmentIndex - 1) : this.segmentIndex);
						num2 = this.segmentIndex;
					}
					else
					{
						num = this.segmentIndex;
						num2 = ((this.segmentIndex < this.interpolator.path.Count - 2) ? (this.segmentIndex + 1) : this.segmentIndex);
					}
					t1 = this.interpolator.path[num + 1] - this.interpolator.path[num];
					t2 = this.interpolator.path[num2 + 1] - this.interpolator.path[num2];
					return;
				}
				t1 = this.tangent;
				t2 = t1;
			}

			// Token: 0x1700026C RID: 620
			// (get) Token: 0x0600106E RID: 4206 RVA: 0x0006372C File Offset: 0x0006192C
			public Vector3 curvatureDirection
			{
				get
				{
					Vector3 vector;
					Vector3 vector2;
					this.GetTangents(out vector, out vector2);
					Vector3 vector3 = Vector3.Cross(vector, vector2);
					if (vector3.sqrMagnitude > 1E-06f)
					{
						return vector3;
					}
					return Vector3.zero;
				}
			}

			// Token: 0x0600106F RID: 4207 RVA: 0x00063760 File Offset: 0x00061960
			public void MoveToNextCorner()
			{
				this.AssertValid();
				List<Vector3> path = this.interpolator.path;
				while (this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength)
				{
					if (this.segmentIndex >= path.Count - 2)
					{
						break;
					}
					this.NextSegment();
				}
				while (this.segmentIndex < path.Count - 2 && VectorMath.IsColinear(path[this.segmentIndex], path[this.segmentIndex + 1], path[this.segmentIndex + 2]))
				{
					this.NextSegment();
				}
				this.currentDistance = this.distanceToSegmentStart + this.currentSegmentLength;
			}

			// Token: 0x06001070 RID: 4208 RVA: 0x00063808 File Offset: 0x00061A08
			public bool MoveToClosestIntersectionWithLineSegment(Vector3 origin, Vector3 direction, Vector2 range)
			{
				this.AssertValid();
				float num = float.PositiveInfinity;
				float num2 = float.PositiveInfinity;
				float num3 = 0f;
				for (int i = 0; i < this.interpolator.path.Count - 1; i++)
				{
					Vector3 vector = this.interpolator.path[i];
					Vector3 vector2 = this.interpolator.path[i + 1];
					float magnitude = (vector2 - vector).magnitude;
					float num4;
					float num5;
					if (VectorMath.LineLineIntersectionFactors(vector.xz, (vector2 - vector).xz, origin.xz, direction.xz, out num4, out num5) && num4 >= 0f && num4 <= 1f && num5 >= range.x && num5 <= range.y)
					{
						float num6 = num3 + num4 * magnitude;
						float num7 = Mathf.Abs(num6 - this.currentDistance);
						if (num7 < num2)
						{
							num = num6;
							num2 = num7;
						}
					}
					num3 += magnitude;
				}
				if (num2 != float.PositiveInfinity)
				{
					this.distance = num;
					return true;
				}
				return false;
			}

			// Token: 0x06001071 RID: 4209 RVA: 0x00063940 File Offset: 0x00061B40
			private void MoveToSegment(int index, float fractionAlongSegment)
			{
				this.AssertValid();
				if (index < 0 || index >= this.interpolator.path.Count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				while (this.segmentIndex > index)
				{
					this.PrevSegment();
				}
				while (this.segmentIndex < index)
				{
					this.NextSegment();
				}
				this.currentDistance = this.distanceToSegmentStart + Mathf.Clamp01(fractionAlongSegment) * this.currentSegmentLength;
			}

			// Token: 0x06001072 RID: 4210 RVA: 0x000639B4 File Offset: 0x00061BB4
			public void MoveToClosestPoint(Vector3 point)
			{
				this.AssertValid();
				float num = float.PositiveInfinity;
				float num2 = 0f;
				int num3 = 0;
				List<Vector3> path = this.interpolator.path;
				for (int i = 0; i < path.Count - 1; i++)
				{
					float num4 = VectorMath.ClosestPointOnLineFactor(path[i], path[i + 1], point);
					Vector3 vector = Vector3.Lerp(path[i], path[i + 1], num4);
					float sqrMagnitude = (point - vector).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						num2 = num4;
						num3 = i;
					}
				}
				this.MoveToSegment(num3, num2);
			}

			// Token: 0x06001073 RID: 4211 RVA: 0x00063A58 File Offset: 0x00061C58
			public void MoveToLocallyClosestPoint(Vector3 point, bool allowForwards = true, bool allowBackwards = true)
			{
				this.AssertValid();
				List<Vector3> path = this.interpolator.path;
				this.segmentIndex = Mathf.Min(this.segmentIndex, path.Count - 2);
				float num;
				for (;;)
				{
					int segmentIndex = this.segmentIndex;
					num = VectorMath.ClosestPointOnLineFactor(path[segmentIndex], path[segmentIndex + 1], point);
					if (num > 1f && allowForwards && this.segmentIndex < path.Count - 2)
					{
						this.NextSegment();
						allowBackwards = false;
					}
					else
					{
						if (num >= 0f || !allowBackwards || this.segmentIndex <= 0)
						{
							break;
						}
						this.PrevSegment();
						allowForwards = false;
					}
				}
				if (num > 0.5f && this.segmentIndex < path.Count - 2)
				{
					this.NextSegment();
				}
				float num2 = 0f;
				float num3 = float.PositiveInfinity;
				if (this.segmentIndex > 0)
				{
					int num4 = this.segmentIndex - 1;
					num2 = VectorMath.ClosestPointOnLineFactor(path[num4], path[num4 + 1], point);
					num3 = (Vector3.Lerp(path[num4], path[num4 + 1], num2) - point).sqrMagnitude;
				}
				float num5 = VectorMath.ClosestPointOnLineFactor(path[this.segmentIndex], path[this.segmentIndex + 1], point);
				float sqrMagnitude = (Vector3.Lerp(path[this.segmentIndex], path[this.segmentIndex + 1], num5) - point).sqrMagnitude;
				if (num3 < sqrMagnitude)
				{
					this.MoveToSegment(this.segmentIndex - 1, num2);
					return;
				}
				this.MoveToSegment(this.segmentIndex, num5);
			}

			// Token: 0x06001074 RID: 4212 RVA: 0x00063BF0 File Offset: 0x00061DF0
			public void MoveToCircleIntersection2D<T>(Vector3 circleCenter3D, float radius, T transform) where T : IMovementPlane
			{
				this.AssertValid();
				List<Vector3> path = this.interpolator.path;
				while (this.segmentIndex < path.Count - 2 && VectorMath.ClosestPointOnLineFactor(path[this.segmentIndex], path[this.segmentIndex + 1], circleCenter3D) > 1f)
				{
					this.NextSegment();
				}
				Vector2 vector = transform.ToPlane(circleCenter3D);
				while (this.segmentIndex < path.Count - 2 && (transform.ToPlane(path[this.segmentIndex + 1]) - vector).sqrMagnitude <= radius * radius)
				{
					this.NextSegment();
				}
				float num = VectorMath.LineCircleIntersectionFactor(vector, transform.ToPlane(path[this.segmentIndex]), transform.ToPlane(path[this.segmentIndex + 1]), radius);
				this.MoveToSegment(this.segmentIndex, num);
			}

			// Token: 0x06001075 RID: 4213 RVA: 0x00063CFC File Offset: 0x00061EFC
			private static float IntegrateSmoothingKernel(float a, float b, float smoothingDistance)
			{
				if (smoothingDistance <= 0f)
				{
					return (float)((a <= 0f && b > 0f) ? 1 : 0);
				}
				float num = ((a < 0f) ? Mathf.Exp(a / smoothingDistance) : (2f - Mathf.Exp(-a / smoothingDistance)));
				float num2 = ((b < 0f) ? Mathf.Exp(b / smoothingDistance) : (2f - Mathf.Exp(-b / smoothingDistance)));
				return 0.5f * (num2 - num);
			}

			// Token: 0x06001076 RID: 4214 RVA: 0x00063D74 File Offset: 0x00061F74
			private static float IntegrateSmoothingKernel2(float a, float b, float smoothingDistance)
			{
				if (smoothingDistance <= 0f)
				{
					return 0f;
				}
				float num = -Mathf.Exp(-a / smoothingDistance) * smoothingDistance;
				float num2 = -Mathf.Exp(-b / smoothingDistance) * (smoothingDistance + b - a);
				return 0.5f * (num2 - num);
			}

			// Token: 0x06001077 RID: 4215 RVA: 0x00063DB8 File Offset: 0x00061FB8
			private static Vector3 IntegrateSmoothTangent(Vector3 p1, Vector3 p2, ref Vector3 tangent, ref float distance, float expectedRadius, float smoothingDistance)
			{
				Vector3 vector = p2 - p1;
				float magnitude = vector.magnitude;
				if (magnitude <= 1E-05f)
				{
					return Vector3.zero;
				}
				Vector3 vector2 = vector * (1f / magnitude);
				float num = Vector3.Angle(tangent, vector2) * 0.017453292f;
				float num2 = expectedRadius * Mathf.Abs(num);
				Vector3 vector3 = Vector3.zero;
				if (num2 > 1E-45f)
				{
					Vector3 vector4 = tangent * PathInterpolator.Cursor.IntegrateSmoothingKernel(distance, distance + num2, smoothingDistance) + (vector2 - tangent) * PathInterpolator.Cursor.IntegrateSmoothingKernel2(distance, distance + num2, smoothingDistance) / num2;
					vector3 += vector4;
					distance += num2;
				}
				vector3 += vector2 * PathInterpolator.Cursor.IntegrateSmoothingKernel(distance, distance + magnitude, smoothingDistance);
				tangent = vector2;
				distance += magnitude;
				return vector3;
			}

			// Token: 0x06001078 RID: 4216 RVA: 0x00063EA4 File Offset: 0x000620A4
			public Vector3 EstimateSmoothTangent(Vector3 normalizedTangent, float smoothingDistance, float expectedRadius, Vector3 beforePathStartContribution, bool forward = true, bool backward = true)
			{
				this.AssertValid();
				if (expectedRadius <= 1E-45f || smoothingDistance <= 0f)
				{
					return normalizedTangent;
				}
				List<Vector3> path = this.interpolator.path;
				Vector3 vector = Vector3.zero;
				while (this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.interpolator.path.Count - 2)
				{
					this.NextSegment();
				}
				if (forward)
				{
					float num = 0f;
					Vector3 vector2 = this.position;
					Vector3 vector3 = normalizedTangent;
					for (int i = this.segmentIndex + 1; i < path.Count; i++)
					{
						vector += PathInterpolator.Cursor.IntegrateSmoothTangent(vector2, path[i], ref vector3, ref num, expectedRadius, smoothingDistance);
						vector2 = path[i];
					}
				}
				if (backward)
				{
					float num2 = 0f;
					Vector3 vector4 = -normalizedTangent;
					Vector3 vector5 = this.position;
					for (int j = this.segmentIndex; j >= 0; j--)
					{
						vector -= PathInterpolator.Cursor.IntegrateSmoothTangent(vector5, path[j], ref vector4, ref num2, expectedRadius, smoothingDistance);
						vector5 = path[j];
					}
					vector += beforePathStartContribution * PathInterpolator.Cursor.IntegrateSmoothingKernel(float.NegativeInfinity, -this.currentDistance, smoothingDistance);
				}
				return vector;
			}

			// Token: 0x06001079 RID: 4217 RVA: 0x00063FE0 File Offset: 0x000621E0
			public Vector3 EstimateSmoothCurvature(Vector3 tangent, float smoothingDistance, float expectedRadius)
			{
				this.AssertValid();
				if (expectedRadius <= 1E-45f)
				{
					return Vector3.zero;
				}
				List<Vector3> path = this.interpolator.path;
				tangent = tangent.normalized;
				Vector3 vector = Vector3.zero;
				while (this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.interpolator.path.Count - 2)
				{
					this.NextSegment();
				}
				float num = 0f;
				Vector3 vector2 = this.position;
				Vector3 vector3 = tangent.normalized;
				for (int i = this.segmentIndex + 1; i < path.Count; i++)
				{
					Vector3 vector4 = path[i] - vector2;
					Vector3 normalized = vector4.normalized;
					float num2 = Vector3.Angle(vector3, normalized) * 0.017453292f;
					Vector3 normalized2 = Vector3.Cross(vector3, normalized).normalized;
					float num3 = 1f / expectedRadius;
					float num4 = expectedRadius * Mathf.Abs(num2);
					float num5 = num3 * PathInterpolator.Cursor.IntegrateSmoothingKernel(num, num + num4, smoothingDistance);
					vector -= num5 * normalized2;
					vector3 = normalized;
					num += num4;
					num += vector4.magnitude;
					vector2 = path[i];
				}
				num = float.Epsilon;
				vector3 = -tangent.normalized;
				vector2 = this.position;
				for (int j = this.segmentIndex; j >= 0; j--)
				{
					Vector3 vector5 = path[j] - vector2;
					if (!(vector5 == Vector3.zero))
					{
						Vector3 normalized3 = vector5.normalized;
						float num6 = Vector3.Angle(vector3, normalized3) * 0.017453292f;
						Vector3 normalized4 = Vector3.Cross(vector3, normalized3).normalized;
						float num7 = 1f / expectedRadius;
						float num8 = expectedRadius * Mathf.Abs(num6);
						float num9 = num7 * PathInterpolator.Cursor.IntegrateSmoothingKernel(num, num + num8, smoothingDistance);
						vector += num9 * normalized4;
						vector3 = normalized3;
						num += num8;
						num += vector5.magnitude;
						vector2 = path[j];
					}
				}
				return vector;
			}

			// Token: 0x0600107A RID: 4218 RVA: 0x000641E4 File Offset: 0x000623E4
			public void MoveWithTurningSpeed(float time, float speed, float turningSpeed, ref Vector3 tangent)
			{
				if (turningSpeed <= 0f)
				{
					throw new ArgumentException("turningSpeed must be greater than zero");
				}
				if (speed <= 0f)
				{
					throw new ArgumentException("speed must be greater than zero");
				}
				this.AssertValid();
				float num = speed / turningSpeed;
				float num2 = time * speed;
				int num3 = 0;
				while (num2 > 0f && this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength)
				{
					if (this.segmentIndex >= this.interpolator.path.Count - 2)
					{
						break;
					}
					this.NextSegment();
				}
				while (num2 < 0f && this.currentDistance <= this.distanceToSegmentStart)
				{
					if (this.segmentIndex <= 0)
					{
						break;
					}
					this.PrevSegment();
				}
				while (num2 != 0f)
				{
					num3++;
					if (num3 > 100)
					{
						throw new Exception("Infinite Loop " + num2.ToString() + " " + time.ToString());
					}
					Vector3 tangent2 = this.tangent;
					if (tangent != tangent2 && this.currentSegmentLength > 0f)
					{
						float num4 = Vector3.Angle(tangent, tangent2) * 0.017453292f * num;
						if (Mathf.Abs(num2) <= num4)
						{
							tangent = Vector3.Slerp(tangent, tangent2, Mathf.Abs(num2) / num4);
							return;
						}
						num2 -= num4 * Mathf.Sign(num2);
						tangent = tangent2;
					}
					if (num2 > 0f)
					{
						float num5 = this.currentSegmentLength - (this.currentDistance - this.distanceToSegmentStart);
						if (num2 < num5)
						{
							this.currentDistance += num2;
							return;
						}
						num2 -= num5;
						if (this.segmentIndex + 1 >= this.interpolator.path.Count - 1)
						{
							this.MoveToSegment(this.segmentIndex, 1f);
							return;
						}
						this.MoveToSegment(this.segmentIndex + 1, 0f);
					}
					else
					{
						float num6 = this.currentDistance - this.distanceToSegmentStart;
						if (-num2 <= num6)
						{
							this.currentDistance += num2;
							return;
						}
						num2 += num6;
						if (this.segmentIndex - 1 < 0)
						{
							this.MoveToSegment(this.segmentIndex, 0f);
							return;
						}
						this.MoveToSegment(this.segmentIndex - 1, 1f);
					}
				}
			}

			// Token: 0x0600107B RID: 4219 RVA: 0x0006441C File Offset: 0x0006261C
			private void PrevSegment()
			{
				int segmentIndex = this.segmentIndex;
				this.segmentIndex = segmentIndex - 1;
				this.currentSegmentLength = (this.interpolator.path[this.segmentIndex + 1] - this.interpolator.path[this.segmentIndex]).magnitude;
				this.distanceToSegmentStart -= this.currentSegmentLength;
			}

			// Token: 0x0600107C RID: 4220 RVA: 0x00064490 File Offset: 0x00062690
			private void NextSegment()
			{
				int segmentIndex = this.segmentIndex;
				this.segmentIndex = segmentIndex + 1;
				this.distanceToSegmentStart += this.currentSegmentLength;
				this.currentSegmentLength = (this.interpolator.path[this.segmentIndex + 1] - this.interpolator.path[this.segmentIndex]).magnitude;
			}

			// Token: 0x04000BED RID: 3053
			private PathInterpolator interpolator;

			// Token: 0x04000BEE RID: 3054
			private int version;

			// Token: 0x04000BEF RID: 3055
			private float currentDistance;

			// Token: 0x04000BF0 RID: 3056
			private float distanceToSegmentStart;

			// Token: 0x04000BF1 RID: 3057
			private float currentSegmentLength;
		}
	}
}
