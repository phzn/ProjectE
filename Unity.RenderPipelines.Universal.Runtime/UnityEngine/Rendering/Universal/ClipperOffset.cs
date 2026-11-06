using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000023 RID: 35
	internal class ClipperOffset
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000A4D8 File Offset: 0x000086D8
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000A4E0 File Offset: 0x000086E0
		public double ArcTolerance { get; set; }

		// Token: 0x06000110 RID: 272 RVA: 0x0000A4E9 File Offset: 0x000086E9
		public ClipperOffset(double arcTolerance = 0.25)
		{
			this.ArcTolerance = arcTolerance;
			this.m_lowest.X = -1L;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000A51B File Offset: 0x0000871B
		public void Clear()
		{
			this.m_polyNodes.Childs.Clear();
			this.m_lowest.X = -1L;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000A53A File Offset: 0x0000873A
		internal static long Round(double value)
		{
			if (value >= 0.0)
			{
				return (long)(value + 0.5);
			}
			return (long)(value - 0.5);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000A564 File Offset: 0x00008764
		public void AddPath(List<IntPoint> path, JoinType joinType, EndType endType)
		{
			int num = path.Count - 1;
			if (num < 0)
			{
				return;
			}
			PolyNode polyNode = new PolyNode();
			polyNode.m_jointype = joinType;
			polyNode.m_endtype = endType;
			if (endType != EndType.etClosedLine)
			{
				if (endType != EndType.etClosedPolygon)
				{
					goto IL_0048;
				}
			}
			while (num > 0 && path[0] == path[num])
			{
				num--;
			}
			IL_0048:
			polyNode.m_polygon.Capacity = num + 1;
			polyNode.m_polygon.Add(path[0]);
			int num2 = 0;
			int num3 = 0;
			for (int i = 1; i <= num; i++)
			{
				if (polyNode.m_polygon[num2] != path[i])
				{
					num2++;
					polyNode.m_polygon.Add(path[i]);
					if (path[i].Y > polyNode.m_polygon[num3].Y || (path[i].Y == polyNode.m_polygon[num3].Y && path[i].X < polyNode.m_polygon[num3].X))
					{
						num3 = num2;
					}
				}
			}
			if (endType == EndType.etClosedPolygon && num2 < 2)
			{
				return;
			}
			this.m_polyNodes.AddChild(polyNode);
			if (endType != EndType.etClosedPolygon)
			{
				return;
			}
			if (this.m_lowest.X < 0L)
			{
				this.m_lowest = new IntPoint((long)(this.m_polyNodes.ChildCount - 1), (long)num3);
				return;
			}
			IntPoint intPoint = this.m_polyNodes.Childs[(int)this.m_lowest.X].m_polygon[(int)this.m_lowest.Y];
			if (polyNode.m_polygon[num3].Y > intPoint.Y || (polyNode.m_polygon[num3].Y == intPoint.Y && polyNode.m_polygon[num3].X < intPoint.X))
			{
				this.m_lowest = new IntPoint((long)(this.m_polyNodes.ChildCount - 1), (long)num3);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000A768 File Offset: 0x00008968
		public void AddPaths(List<List<IntPoint>> paths, JoinType joinType, EndType endType)
		{
			foreach (List<IntPoint> list in paths)
			{
				this.AddPath(list, joinType, endType);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000A7B8 File Offset: 0x000089B8
		private void FixOrientations()
		{
			if (this.m_lowest.X >= 0L && !Clipper.Orientation(this.m_polyNodes.Childs[(int)this.m_lowest.X].m_polygon))
			{
				for (int i = 0; i < this.m_polyNodes.ChildCount; i++)
				{
					PolyNode polyNode = this.m_polyNodes.Childs[i];
					if (polyNode.m_endtype == EndType.etClosedPolygon || (polyNode.m_endtype == EndType.etClosedLine && Clipper.Orientation(polyNode.m_polygon)))
					{
						polyNode.m_polygon.Reverse();
					}
				}
				return;
			}
			for (int j = 0; j < this.m_polyNodes.ChildCount; j++)
			{
				PolyNode polyNode2 = this.m_polyNodes.Childs[j];
				if (polyNode2.m_endtype == EndType.etClosedLine && !Clipper.Orientation(polyNode2.m_polygon))
				{
					polyNode2.m_polygon.Reverse();
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000A898 File Offset: 0x00008A98
		internal static DoublePoint GetUnitNormal(IntPoint pt1, IntPoint pt2)
		{
			double num = (double)(pt2.X - pt1.X);
			double num2 = (double)(pt2.Y - pt1.Y);
			if (num == 0.0 && num2 == 0.0)
			{
				return default(DoublePoint);
			}
			double num3 = 1.0 / Math.Sqrt(num * num + num2 * num2);
			num *= num3;
			num2 *= num3;
			return new DoublePoint(num2, -num);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000A90C File Offset: 0x00008B0C
		private void DoOffset(double delta)
		{
			this.m_destPolys = new List<List<IntPoint>>();
			this.m_delta = delta;
			if (ClipperBase.near_zero(delta))
			{
				this.m_destPolys.Capacity = this.m_polyNodes.ChildCount;
				for (int i = 0; i < this.m_polyNodes.ChildCount; i++)
				{
					PolyNode polyNode = this.m_polyNodes.Childs[i];
					if (polyNode.m_endtype == EndType.etClosedPolygon)
					{
						this.m_destPolys.Add(polyNode.m_polygon);
					}
				}
				return;
			}
			double num;
			if (this.ArcTolerance <= 0.0)
			{
				num = 0.25;
			}
			else if (this.ArcTolerance > Math.Abs(delta) * 0.25)
			{
				num = Math.Abs(delta) * 0.25;
			}
			else
			{
				num = this.ArcTolerance;
			}
			double num2 = 3.141592653589793 / Math.Acos(1.0 - num / Math.Abs(delta));
			this.m_sin = Math.Sin(6.283185307179586 / num2);
			this.m_cos = Math.Cos(6.283185307179586 / num2);
			this.m_StepsPerRad = num2 / 6.283185307179586;
			if (delta < 0.0)
			{
				this.m_sin = -this.m_sin;
			}
			this.m_destPolys.Capacity = this.m_polyNodes.ChildCount * 2;
			for (int j = 0; j < this.m_polyNodes.ChildCount; j++)
			{
				PolyNode polyNode2 = this.m_polyNodes.Childs[j];
				this.m_srcPoly = polyNode2.m_polygon;
				int count = this.m_srcPoly.Count;
				if (count != 0 && (delta > 0.0 || (count >= 3 && polyNode2.m_endtype == EndType.etClosedPolygon)))
				{
					this.m_destPoly = new List<IntPoint>();
					if (count == 1)
					{
						if (polyNode2.m_jointype == JoinType.jtRound)
						{
							double num3 = 1.0;
							double num4 = 0.0;
							int num5 = 1;
							while ((double)num5 <= num2)
							{
								this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[0].X + num3 * delta), ClipperOffset.Round((double)this.m_srcPoly[0].Y + num4 * delta)));
								double num6 = num3;
								num3 = num3 * this.m_cos - this.m_sin * num4;
								num4 = num6 * this.m_sin + num4 * this.m_cos;
								num5++;
							}
						}
						else
						{
							double num7 = -1.0;
							double num8 = -1.0;
							for (int k = 0; k < 4; k++)
							{
								this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[0].X + num7 * delta), ClipperOffset.Round((double)this.m_srcPoly[0].Y + num8 * delta)));
								if (num7 < 0.0)
								{
									num7 = 1.0;
								}
								else if (num8 < 0.0)
								{
									num8 = 1.0;
								}
								else
								{
									num7 = -1.0;
								}
							}
						}
						this.m_destPolys.Add(this.m_destPoly);
					}
					else
					{
						this.m_normals.Clear();
						this.m_normals.Capacity = count;
						for (int l = 0; l < count - 1; l++)
						{
							this.m_normals.Add(ClipperOffset.GetUnitNormal(this.m_srcPoly[l], this.m_srcPoly[l + 1]));
						}
						if (polyNode2.m_endtype == EndType.etClosedLine || polyNode2.m_endtype == EndType.etClosedPolygon)
						{
							this.m_normals.Add(ClipperOffset.GetUnitNormal(this.m_srcPoly[count - 1], this.m_srcPoly[0]));
						}
						else
						{
							this.m_normals.Add(new DoublePoint(this.m_normals[count - 2]));
						}
						if (polyNode2.m_endtype == EndType.etClosedPolygon)
						{
							int num9 = count - 1;
							for (int m = 0; m < count; m++)
							{
								this.OffsetPoint(m, ref num9, polyNode2.m_jointype);
							}
							this.m_destPolys.Add(this.m_destPoly);
						}
						else if (polyNode2.m_endtype == EndType.etClosedLine)
						{
							int num10 = count - 1;
							for (int n = 0; n < count; n++)
							{
								this.OffsetPoint(n, ref num10, polyNode2.m_jointype);
							}
							this.m_destPolys.Add(this.m_destPoly);
							this.m_destPoly = new List<IntPoint>();
							DoublePoint doublePoint = this.m_normals[count - 1];
							for (int num11 = count - 1; num11 > 0; num11--)
							{
								this.m_normals[num11] = new DoublePoint(-this.m_normals[num11 - 1].X, -this.m_normals[num11 - 1].Y);
							}
							this.m_normals[0] = new DoublePoint(-doublePoint.X, -doublePoint.Y);
							num10 = 0;
							for (int num12 = count - 1; num12 >= 0; num12--)
							{
								this.OffsetPoint(num12, ref num10, polyNode2.m_jointype);
							}
							this.m_destPolys.Add(this.m_destPoly);
						}
						else
						{
							int num13 = 0;
							for (int num14 = 1; num14 < count - 1; num14++)
							{
								this.OffsetPoint(num14, ref num13, polyNode2.m_jointype);
							}
							int num15 = count - 1;
							num13 = count - 2;
							this.m_sinA = 0.0;
							this.m_normals[num15] = new DoublePoint(-this.m_normals[num15].X, -this.m_normals[num15].Y);
							this.DoRound(num15, num13);
							for (int num16 = count - 1; num16 > 0; num16--)
							{
								this.m_normals[num16] = new DoublePoint(-this.m_normals[num16 - 1].X, -this.m_normals[num16 - 1].Y);
							}
							this.m_normals[0] = new DoublePoint(-this.m_normals[1].X, -this.m_normals[1].Y);
							num13 = count - 1;
							for (int num17 = num13 - 1; num17 > 0; num17--)
							{
								this.OffsetPoint(num17, ref num13, polyNode2.m_jointype);
							}
							num13 = 1;
							this.m_sinA = 0.0;
							this.DoRound(0, 1);
							this.m_destPolys.Add(this.m_destPoly);
						}
					}
				}
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public void Execute(ref List<List<IntPoint>> solution, double delta, int inputSize)
		{
			solution.Clear();
			this.FixOrientations();
			this.DoOffset(delta);
			Clipper clipper = new Clipper(0);
			clipper.AddPaths(this.m_destPolys, PolyType.ptSubject, true);
			clipper.LastIndex = inputSize - 1;
			if (delta > 0.0)
			{
				clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
				return;
			}
			IntRect bounds = ClipperBase.GetBounds(this.m_destPolys);
			clipper.AddPath(new List<IntPoint>(4)
			{
				new IntPoint(bounds.left - 10L, bounds.bottom + 10L),
				new IntPoint(bounds.right + 10L, bounds.bottom + 10L),
				new IntPoint(bounds.right + 10L, bounds.top - 10L),
				new IntPoint(bounds.left - 10L, bounds.top - 10L)
			}, PolyType.ptSubject, true);
			clipper.ReverseSolution = true;
			clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNegative, PolyFillType.pftNegative);
			if (solution.Count > 0)
			{
				solution.RemoveAt(0);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000B0E8 File Offset: 0x000092E8
		public void Execute(ref PolyTree solution, double delta)
		{
			solution.Clear();
			this.FixOrientations();
			this.DoOffset(delta);
			Clipper clipper = new Clipper(0);
			clipper.AddPaths(this.m_destPolys, PolyType.ptSubject, true);
			if (delta > 0.0)
			{
				clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
				return;
			}
			IntRect bounds = ClipperBase.GetBounds(this.m_destPolys);
			clipper.AddPath(new List<IntPoint>(4)
			{
				new IntPoint(bounds.left - 10L, bounds.bottom + 10L),
				new IntPoint(bounds.right + 10L, bounds.bottom + 10L),
				new IntPoint(bounds.right + 10L, bounds.top - 10L),
				new IntPoint(bounds.left - 10L, bounds.top - 10L)
			}, PolyType.ptSubject, true);
			clipper.ReverseSolution = true;
			clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNegative, PolyFillType.pftNegative);
			if (solution.ChildCount == 1 && solution.Childs[0].ChildCount > 0)
			{
				PolyNode polyNode = solution.Childs[0];
				solution.Childs.Capacity = polyNode.ChildCount;
				solution.Childs[0] = polyNode.Childs[0];
				solution.Childs[0].m_Parent = solution;
				for (int i = 1; i < polyNode.ChildCount; i++)
				{
					solution.AddChild(polyNode.Childs[i]);
				}
				return;
			}
			solution.Clear();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000B284 File Offset: 0x00009484
		private void OffsetPoint(int j, ref int k, JoinType jointype)
		{
			this.m_sinA = this.m_normals[k].X * this.m_normals[j].Y - this.m_normals[j].X * this.m_normals[k].Y;
			if (Math.Abs(this.m_sinA * this.m_delta) < 1.0)
			{
				if (this.m_normals[k].X * this.m_normals[j].X + this.m_normals[j].Y * this.m_normals[k].Y > 0.0)
				{
					IntPoint intPoint = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[k].X * this.m_delta), ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[k].Y * this.m_delta));
					intPoint.NX = this.m_normals[k].X;
					intPoint.NY = this.m_normals[k].Y;
					intPoint.N = (long)j;
					intPoint.D = 1L;
					this.m_destPoly.Add(intPoint);
					return;
				}
			}
			else if (this.m_sinA > 1.0)
			{
				this.m_sinA = 1.0;
			}
			else if (this.m_sinA < -1.0)
			{
				this.m_sinA = -1.0;
			}
			if (this.m_sinA * this.m_delta < 0.0)
			{
				IntPoint intPoint2 = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[k].X * this.m_delta), ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[k].Y * this.m_delta));
				intPoint2.NX = this.m_normals[k].X;
				intPoint2.NY = this.m_normals[k].Y;
				this.m_destPoly.Add(intPoint2);
				intPoint2 = this.m_srcPoly[j];
				intPoint2.NX = this.m_normals[k].X;
				intPoint2.NY = this.m_normals[k].Y;
				intPoint2.N = (long)j;
				intPoint2.D = 1L;
				this.m_destPoly.Add(intPoint2);
				intPoint2 = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[j].X * this.m_delta), ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[j].Y * this.m_delta));
				intPoint2.NX = this.m_normals[j].X;
				intPoint2.NY = this.m_normals[j].Y;
				intPoint2.N = (long)j;
				intPoint2.D = 1L;
				this.m_destPoly.Add(intPoint2);
			}
			else if (jointype == JoinType.jtRound)
			{
				this.DoRound(j, k);
			}
			k = j;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000B638 File Offset: 0x00009838
		internal void DoSquare(int j, int k)
		{
			double num = Math.Tan(Math.Atan2(this.m_sinA, this.m_normals[k].X * this.m_normals[j].X + this.m_normals[k].Y * this.m_normals[j].Y) / 4.0);
			IntPoint intPoint = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_delta * (this.m_normals[k].X - this.m_normals[k].Y * num)), ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_delta * (this.m_normals[k].Y + this.m_normals[k].X * num)));
			intPoint.NX = this.m_normals[k].X - this.m_normals[k].Y * num;
			intPoint.NY = this.m_normals[k].Y + this.m_normals[k].X * num;
			this.m_destPoly.Add(intPoint);
			intPoint = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_delta * (this.m_normals[j].X + this.m_normals[j].Y * num)), ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_delta * (this.m_normals[j].Y - this.m_normals[j].X * num)));
			intPoint.NX = this.m_normals[k].X + this.m_normals[k].Y * num;
			intPoint.NY = this.m_normals[k].Y - this.m_normals[k].X * num;
			this.m_destPoly.Add(intPoint);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000B890 File Offset: 0x00009A90
		internal void DoMiter(int j, int k, double r)
		{
			double num = this.m_delta / r;
			IntPoint intPoint = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + (this.m_normals[k].X + this.m_normals[j].X) * num), ClipperOffset.Round((double)this.m_srcPoly[j].Y + (this.m_normals[k].Y + this.m_normals[j].Y) * num));
			intPoint.NX = (this.m_normals[k].X + this.m_normals[j].X) * num;
			intPoint.NY = (this.m_normals[k].Y + this.m_normals[j].Y) * num;
			this.m_destPoly.Add(intPoint);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000B98C File Offset: 0x00009B8C
		internal void DoRound(int j, int k)
		{
			double num = Math.Atan2(this.m_sinA, this.m_normals[k].X * this.m_normals[j].X + this.m_normals[k].Y * this.m_normals[j].Y);
			int num2 = Math.Max((int)ClipperOffset.Round(this.m_StepsPerRad * Math.Abs(num)), 1);
			double num3 = this.m_normals[k].X;
			double num4 = this.m_normals[k].Y;
			for (int i = 0; i < num2; i++)
			{
				IntPoint intPoint = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + num3 * this.m_delta), ClipperOffset.Round((double)this.m_srcPoly[j].Y + num4 * this.m_delta));
				intPoint.NX = num3;
				intPoint.NY = num4;
				intPoint.N = (long)j;
				intPoint.D = 1L;
				this.m_destPoly.Add(intPoint);
				double num5 = num3;
				num3 = num3 * this.m_cos - this.m_sin * num4;
				num4 = num5 * this.m_sin + num4 * this.m_cos;
			}
			IntPoint intPoint2 = new IntPoint(ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[j].X * this.m_delta), ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[j].Y * this.m_delta));
			intPoint2.NX = this.m_normals[j].X;
			intPoint2.NY = this.m_normals[j].Y;
			intPoint2.N = (long)j;
			intPoint2.D = 1L;
			this.m_destPoly.Add(intPoint2);
		}

		// Token: 0x0400009D RID: 157
		private List<List<IntPoint>> m_destPolys;

		// Token: 0x0400009E RID: 158
		private List<IntPoint> m_srcPoly;

		// Token: 0x0400009F RID: 159
		private List<IntPoint> m_destPoly;

		// Token: 0x040000A0 RID: 160
		private List<DoublePoint> m_normals = new List<DoublePoint>();

		// Token: 0x040000A1 RID: 161
		private double m_delta;

		// Token: 0x040000A2 RID: 162
		private double m_sinA;

		// Token: 0x040000A3 RID: 163
		private double m_sin;

		// Token: 0x040000A4 RID: 164
		private double m_cos;

		// Token: 0x040000A5 RID: 165
		private double m_StepsPerRad;

		// Token: 0x040000A6 RID: 166
		private IntPoint m_lowest;

		// Token: 0x040000A7 RID: 167
		private PolyNode m_polyNodes = new PolyNode();

		// Token: 0x040000A9 RID: 169
		private const double two_pi = 6.283185307179586;

		// Token: 0x040000AA RID: 170
		private const double def_arc_tolerance = 0.25;
	}
}
