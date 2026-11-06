using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x02000005 RID: 5
	public class BezierToVertex
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00003340 File Offset: 0x00001540
		public void GetContours(SVGData svg, MeshData data)
		{
			this.WorkBufferPool.Get<Vector2>(ref this.WorkVertices);
			Vector2 vector = Vector2.zero;
			List<Curve> curves = svg.Curves;
			int count = curves.Count;
			for (int i = 0; i < count; i++)
			{
				Curve curve = curves[i];
				if (curve.IsMove)
				{
					this.EmitWorkVerticesIfNeeded(data);
				}
				else
				{
					this.FillBezier(vector, curve.InControl, curve.OutControl, curve.Position);
				}
				vector = curve.Position;
			}
			this.EmitWorkVerticesIfNeeded(data);
			this.WorkBufferPool.Release<Vector2>(ref this.WorkVertices);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000033D5 File Offset: 0x000015D5
		private void EmitWorkVerticesIfNeeded(MeshData data)
		{
			if (this.WorkVertices.UsedSize == 0)
			{
				return;
			}
			data.AddVertices(this.WorkVertices);
			this.WorkVertices.Clear();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000033FC File Offset: 0x000015FC
		private void FillBezier(Vector2 start, Vector2 c1, Vector2 c2, Vector2 end)
		{
			float num = this.PathDistanceEpsilon / this.Scale;
			num *= num;
			this.BeginFillBezier(start, c1, c2, end, num);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003427 File Offset: 0x00001627
		private void BeginFillBezier(Vector2 start, Vector2 c1, Vector2 c2, Vector2 end, float distanceTolerance)
		{
			this.WorkVertices.Push(ref start);
			this.RecursiveFillBezier(start, c1, c2, end, distanceTolerance, 0);
			this.WorkVertices.Push(ref end);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003454 File Offset: 0x00001654
		private void RecursiveFillBezier(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float distanceTolerance, int level)
		{
			if (level > this.RecursionLimit)
			{
				return;
			}
			float num = 3.1415927f;
			Vector2 vector = (v1 + v2) / 2f;
			Vector2 vector2 = (v2 + v3) / 2f;
			Vector2 vector3 = (v3 + v4) / 2f;
			Vector2 vector4 = (vector + vector2) / 2f;
			Vector2 vector5 = (vector2 + vector3) / 2f;
			Vector2 vector6 = (vector4 + vector5) / 2f;
			if (level > 0)
			{
				Vector2 vector7 = v4 - v1;
				float num2 = Mathf.Abs((v2.x - v4.x) * vector7.y - (v2.y - v4.y) * vector7.x);
				float num3 = Mathf.Abs((v3.x - v4.x) * vector7.y - (v3.y - v4.y) * vector7.x);
				if (num2 > this.FLTEpsilon && num3 > this.FLTEpsilon)
				{
					if ((num2 + num3) * (num2 + num3) <= distanceTolerance * (vector7.x * vector7.x + vector7.y * vector7.y))
					{
						if (this.AngleTolerance < this.AngleEpsilon)
						{
							this.WorkVertices.Push(ref vector6);
							return;
						}
						float num4 = Mathf.Atan2(v3.y - v2.y, v3.x - v2.x);
						float num5 = Mathf.Abs(num4 - Mathf.Atan2(v2.y - v1.y, v2.x - v1.x));
						float num6 = Mathf.Abs(Mathf.Atan2(v4.y - v3.y, v4.x - v3.x) - num4);
						if (num5 >= num)
						{
							num5 = 2f * num - num5;
						}
						if (num6 >= num)
						{
							num6 = 2f * num - num6;
						}
						if (num5 + num6 < this.AngleTolerance)
						{
							this.WorkVertices.Push(ref vector6);
							return;
						}
						if (this.CuspLimit > 0f)
						{
							if (num5 > this.CuspLimit)
							{
								this.WorkVertices.Push(ref v2);
								return;
							}
							if (num6 > this.CuspLimit)
							{
								this.WorkVertices.Push(ref v3);
								return;
							}
						}
					}
				}
				else if (num2 > this.FLTEpsilon)
				{
					if (num2 * num2 <= distanceTolerance * (vector7.x * vector7.x + vector7.y * vector7.y))
					{
						if (this.AngleTolerance < this.AngleEpsilon)
						{
							this.WorkVertices.Push(ref vector6);
							return;
						}
						float num7 = Mathf.Abs(Mathf.Atan2(v3.y - v2.y, v3.x - v2.x) - Mathf.Atan2(v2.y - v1.y, v2.x - v1.x));
						if (num7 >= num)
						{
							num7 = 2f * num - num7;
						}
						if (num7 < this.AngleTolerance)
						{
							this.WorkVertices.Push(ref v2);
							this.WorkVertices.Push(ref v3);
							return;
						}
						if (this.CuspLimit > 0f && num7 > this.CuspLimit)
						{
							this.WorkVertices.Push(ref v2);
							return;
						}
					}
				}
				else if (num3 > this.FLTEpsilon)
				{
					if (num3 * num3 <= distanceTolerance * (vector7.x * vector7.x + vector7.y * vector7.y))
					{
						if (this.AngleTolerance < this.AngleEpsilon)
						{
							this.WorkVertices.Push(ref vector6);
							return;
						}
						float num8 = Mathf.Abs(Mathf.Atan2(v4.y - v3.y, v4.x - v3.x) - Mathf.Atan2(v3.y - v2.y, v3.x - v2.x));
						if (num8 >= num)
						{
							num8 = 2f * num - num8;
						}
						if (num8 < this.AngleTolerance)
						{
							this.WorkVertices.Push(ref v2);
							this.WorkVertices.Push(ref v3);
							return;
						}
						if (this.CuspLimit > 0f && num8 > this.CuspLimit)
						{
							this.WorkVertices.Push(ref v3);
							return;
						}
					}
				}
				else
				{
					float num9 = vector6.x - (v1.x + v4.x) / 2f;
					float num10 = vector6.y - (v1.y + v4.y) / 2f;
					if (num9 * num9 + num10 * num10 <= distanceTolerance)
					{
						this.WorkVertices.Push(ref vector6);
						return;
					}
				}
			}
			this.RecursiveFillBezier(v1, vector, vector4, vector6, distanceTolerance, level + 1);
			this.RecursiveFillBezier(vector6, vector5, vector3, v4, distanceTolerance, level + 1);
		}

		// Token: 0x04000012 RID: 18
		public float Scale = 1f;

		// Token: 0x04000013 RID: 19
		public float PathDistanceEpsilon = 1f;

		// Token: 0x04000014 RID: 20
		public int RecursionLimit = 8;

		// Token: 0x04000015 RID: 21
		public float FLTEpsilon = 1.1920929E-07f;

		// Token: 0x04000016 RID: 22
		public float AngleEpsilon = 0.01f;

		// Token: 0x04000017 RID: 23
		public float AngleTolerance;

		// Token: 0x04000018 RID: 24
		public float CuspLimit;

		// Token: 0x04000019 RID: 25
		public WorkBufferPool WorkBufferPool;

		// Token: 0x0400001A RID: 26
		private WorkBuffer<Vector2> WorkVertices;
	}
}
