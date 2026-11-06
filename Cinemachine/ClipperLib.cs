using System;
using System.Collections.Generic;

namespace Cinemachine
{
	// Token: 0x0200005C RID: 92
	internal static class ClipperLib
	{
		// Token: 0x020000CC RID: 204
		public struct DoublePoint
		{
			// Token: 0x0600049A RID: 1178 RVA: 0x0001AB78 File Offset: 0x00018D78
			public DoublePoint(double x = 0.0, double y = 0.0)
			{
				this.X = x;
				this.Y = y;
			}

			// Token: 0x0600049B RID: 1179 RVA: 0x0001AB88 File Offset: 0x00018D88
			public DoublePoint(ClipperLib.DoublePoint dp)
			{
				this.X = dp.X;
				this.Y = dp.Y;
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x0001ABA2 File Offset: 0x00018DA2
			public DoublePoint(ClipperLib.IntPoint ip)
			{
				this.X = (double)ip.X;
				this.Y = (double)ip.Y;
			}

			// Token: 0x0400040C RID: 1036
			public double X;

			// Token: 0x0400040D RID: 1037
			public double Y;
		}

		// Token: 0x020000CD RID: 205
		public class PolyTree : ClipperLib.PolyNode
		{
			// Token: 0x0600049D RID: 1181 RVA: 0x0001ABC0 File Offset: 0x00018DC0
			public void Clear()
			{
				for (int i = 0; i < this.m_AllPolys.Count; i++)
				{
					this.m_AllPolys[i] = null;
				}
				this.m_AllPolys.Clear();
				this.m_Childs.Clear();
			}

			// Token: 0x0600049E RID: 1182 RVA: 0x0001AC06 File Offset: 0x00018E06
			public ClipperLib.PolyNode GetFirst()
			{
				if (this.m_Childs.Count > 0)
				{
					return this.m_Childs[0];
				}
				return null;
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001AC24 File Offset: 0x00018E24
			public int Total
			{
				get
				{
					int num = this.m_AllPolys.Count;
					if (num > 0 && this.m_Childs[0] != this.m_AllPolys[0])
					{
						num--;
					}
					return num;
				}
			}

			// Token: 0x0400040E RID: 1038
			internal List<ClipperLib.PolyNode> m_AllPolys = new List<ClipperLib.PolyNode>();
		}

		// Token: 0x020000CE RID: 206
		public class PolyNode
		{
			// Token: 0x060004A1 RID: 1185 RVA: 0x0001AC74 File Offset: 0x00018E74
			private bool IsHoleNode()
			{
				bool flag = true;
				for (ClipperLib.PolyNode polyNode = this.m_Parent; polyNode != null; polyNode = polyNode.m_Parent)
				{
					flag = !flag;
				}
				return flag;
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0001AC9C File Offset: 0x00018E9C
			public int ChildCount
			{
				get
				{
					return this.m_Childs.Count;
				}
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001ACA9 File Offset: 0x00018EA9
			public List<ClipperLib.IntPoint> Contour
			{
				get
				{
					return this.m_polygon;
				}
			}

			// Token: 0x060004A4 RID: 1188 RVA: 0x0001ACB4 File Offset: 0x00018EB4
			internal void AddChild(ClipperLib.PolyNode Child)
			{
				int count = this.m_Childs.Count;
				this.m_Childs.Add(Child);
				Child.m_Parent = this;
				Child.m_Index = count;
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x0001ACE7 File Offset: 0x00018EE7
			public ClipperLib.PolyNode GetNext()
			{
				if (this.m_Childs.Count > 0)
				{
					return this.m_Childs[0];
				}
				return this.GetNextSiblingUp();
			}

			// Token: 0x060004A6 RID: 1190 RVA: 0x0001AD0C File Offset: 0x00018F0C
			internal ClipperLib.PolyNode GetNextSiblingUp()
			{
				if (this.m_Parent == null)
				{
					return null;
				}
				if (this.m_Index == this.m_Parent.m_Childs.Count - 1)
				{
					return this.m_Parent.GetNextSiblingUp();
				}
				return this.m_Parent.m_Childs[this.m_Index + 1];
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001AD61 File Offset: 0x00018F61
			public List<ClipperLib.PolyNode> Childs
			{
				get
				{
					return this.m_Childs;
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0001AD69 File Offset: 0x00018F69
			public ClipperLib.PolyNode Parent
			{
				get
				{
					return this.m_Parent;
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001AD71 File Offset: 0x00018F71
			public bool IsHole
			{
				get
				{
					return this.IsHoleNode();
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001AD79 File Offset: 0x00018F79
			// (set) Token: 0x060004AB RID: 1195 RVA: 0x0001AD81 File Offset: 0x00018F81
			public bool IsOpen { get; set; }

			// Token: 0x0400040F RID: 1039
			internal ClipperLib.PolyNode m_Parent;

			// Token: 0x04000410 RID: 1040
			internal List<ClipperLib.IntPoint> m_polygon = new List<ClipperLib.IntPoint>();

			// Token: 0x04000411 RID: 1041
			internal int m_Index;

			// Token: 0x04000412 RID: 1042
			internal ClipperLib.JoinType m_jointype;

			// Token: 0x04000413 RID: 1043
			internal ClipperLib.EndType m_endtype;

			// Token: 0x04000414 RID: 1044
			internal List<ClipperLib.PolyNode> m_Childs = new List<ClipperLib.PolyNode>();
		}

		// Token: 0x020000CF RID: 207
		internal struct Int128
		{
			// Token: 0x060004AD RID: 1197 RVA: 0x0001ADA8 File Offset: 0x00018FA8
			public Int128(long _lo)
			{
				this.lo = (ulong)_lo;
				if (_lo < 0L)
				{
					this.hi = -1L;
					return;
				}
				this.hi = 0L;
			}

			// Token: 0x060004AE RID: 1198 RVA: 0x0001ADC7 File Offset: 0x00018FC7
			public Int128(long _hi, ulong _lo)
			{
				this.lo = _lo;
				this.hi = _hi;
			}

			// Token: 0x060004AF RID: 1199 RVA: 0x0001ADD7 File Offset: 0x00018FD7
			public Int128(ClipperLib.Int128 val)
			{
				this.hi = val.hi;
				this.lo = val.lo;
			}

			// Token: 0x060004B0 RID: 1200 RVA: 0x0001ADF1 File Offset: 0x00018FF1
			public bool IsNegative()
			{
				return this.hi < 0L;
			}

			// Token: 0x060004B1 RID: 1201 RVA: 0x0001AE00 File Offset: 0x00019000
			public static bool operator ==(ClipperLib.Int128 val1, ClipperLib.Int128 val2)
			{
				return val1 == val2 || (val1 != null && val2 != null && val1.hi == val2.hi && val1.lo == val2.lo);
			}

			// Token: 0x060004B2 RID: 1202 RVA: 0x0001AE4D File Offset: 0x0001904D
			public static bool operator !=(ClipperLib.Int128 val1, ClipperLib.Int128 val2)
			{
				return !(val1 == val2);
			}

			// Token: 0x060004B3 RID: 1203 RVA: 0x0001AE5C File Offset: 0x0001905C
			public override bool Equals(object obj)
			{
				if (obj == null || !(obj is ClipperLib.Int128))
				{
					return false;
				}
				ClipperLib.Int128 @int = (ClipperLib.Int128)obj;
				return @int.hi == this.hi && @int.lo == this.lo;
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x0001AE9B File Offset: 0x0001909B
			public override int GetHashCode()
			{
				return this.hi.GetHashCode() ^ this.lo.GetHashCode();
			}

			// Token: 0x060004B5 RID: 1205 RVA: 0x0001AEB4 File Offset: 0x000190B4
			public static bool operator >(ClipperLib.Int128 val1, ClipperLib.Int128 val2)
			{
				if (val1.hi != val2.hi)
				{
					return val1.hi > val2.hi;
				}
				return val1.lo > val2.lo;
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x0001AEE1 File Offset: 0x000190E1
			public static bool operator <(ClipperLib.Int128 val1, ClipperLib.Int128 val2)
			{
				if (val1.hi != val2.hi)
				{
					return val1.hi < val2.hi;
				}
				return val1.lo < val2.lo;
			}

			// Token: 0x060004B7 RID: 1207 RVA: 0x0001AF0E File Offset: 0x0001910E
			public static ClipperLib.Int128 operator +(ClipperLib.Int128 lhs, ClipperLib.Int128 rhs)
			{
				lhs.hi += rhs.hi;
				lhs.lo += rhs.lo;
				if (lhs.lo < rhs.lo)
				{
					lhs.hi += 1L;
				}
				return lhs;
			}

			// Token: 0x060004B8 RID: 1208 RVA: 0x0001AF4E File Offset: 0x0001914E
			public static ClipperLib.Int128 operator -(ClipperLib.Int128 lhs, ClipperLib.Int128 rhs)
			{
				return lhs + -rhs;
			}

			// Token: 0x060004B9 RID: 1209 RVA: 0x0001AF5C File Offset: 0x0001915C
			public static ClipperLib.Int128 operator -(ClipperLib.Int128 val)
			{
				if (val.lo == 0UL)
				{
					return new ClipperLib.Int128(-val.hi, 0UL);
				}
				return new ClipperLib.Int128(~val.hi, ~val.lo + 1UL);
			}

			// Token: 0x060004BA RID: 1210 RVA: 0x0001AF8C File Offset: 0x0001918C
			public static explicit operator double(ClipperLib.Int128 val)
			{
				if (val.hi >= 0L)
				{
					return val.lo + (double)val.hi * 1.8446744073709552E+19;
				}
				if (val.lo == 0UL)
				{
					return (double)val.hi * 1.8446744073709552E+19;
				}
				return -(~val.lo + (double)(~(double)val.hi) * 1.8446744073709552E+19);
			}

			// Token: 0x060004BB RID: 1211 RVA: 0x0001AFF8 File Offset: 0x000191F8
			public static ClipperLib.Int128 Int128Mul(long lhs, long rhs)
			{
				bool flag = lhs < 0L != rhs < 0L;
				if (lhs < 0L)
				{
					lhs = -lhs;
				}
				if (rhs < 0L)
				{
					rhs = -rhs;
				}
				ulong num = (ulong)lhs >> 32;
				ulong num2 = (ulong)(lhs & (long)((ulong)(-1)));
				ulong num3 = (ulong)rhs >> 32;
				ulong num4 = (ulong)(rhs & (long)((ulong)(-1)));
				ulong num5 = num * num3;
				ulong num6 = num2 * num4;
				ulong num7 = num * num4 + num2 * num3;
				long num8 = (long)(num5 + (num7 >> 32));
				ulong num9 = (num7 << 32) + num6;
				if (num9 < num6)
				{
					num8 += 1L;
				}
				ClipperLib.Int128 @int = new ClipperLib.Int128(num8, num9);
				if (!flag)
				{
					return @int;
				}
				return -@int;
			}

			// Token: 0x04000416 RID: 1046
			private long hi;

			// Token: 0x04000417 RID: 1047
			private ulong lo;
		}

		// Token: 0x020000D0 RID: 208
		public struct IntPoint
		{
			// Token: 0x060004BC RID: 1212 RVA: 0x0001B081 File Offset: 0x00019281
			public IntPoint(long X, long Y)
			{
				this.X = X;
				this.Y = Y;
			}

			// Token: 0x060004BD RID: 1213 RVA: 0x0001B091 File Offset: 0x00019291
			public IntPoint(double x, double y)
			{
				this.X = (long)x;
				this.Y = (long)y;
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x0001B0A3 File Offset: 0x000192A3
			public IntPoint(ClipperLib.IntPoint pt)
			{
				this.X = pt.X;
				this.Y = pt.Y;
			}

			// Token: 0x060004BF RID: 1215 RVA: 0x0001B0BD File Offset: 0x000192BD
			public static bool operator ==(ClipperLib.IntPoint a, ClipperLib.IntPoint b)
			{
				return a.X == b.X && a.Y == b.Y;
			}

			// Token: 0x060004C0 RID: 1216 RVA: 0x0001B0DD File Offset: 0x000192DD
			public static bool operator !=(ClipperLib.IntPoint a, ClipperLib.IntPoint b)
			{
				return a.X != b.X || a.Y != b.Y;
			}

			// Token: 0x060004C1 RID: 1217 RVA: 0x0001B100 File Offset: 0x00019300
			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj is ClipperLib.IntPoint)
				{
					ClipperLib.IntPoint intPoint = (ClipperLib.IntPoint)obj;
					return this.X == intPoint.X && this.Y == intPoint.Y;
				}
				return false;
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x0001B141 File Offset: 0x00019341
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x04000418 RID: 1048
			public long X;

			// Token: 0x04000419 RID: 1049
			public long Y;
		}

		// Token: 0x020000D1 RID: 209
		public struct IntRect
		{
			// Token: 0x060004C3 RID: 1219 RVA: 0x0001B153 File Offset: 0x00019353
			public IntRect(long l, long t, long r, long b)
			{
				this.left = l;
				this.top = t;
				this.right = r;
				this.bottom = b;
			}

			// Token: 0x060004C4 RID: 1220 RVA: 0x0001B172 File Offset: 0x00019372
			public IntRect(ClipperLib.IntRect ir)
			{
				this.left = ir.left;
				this.top = ir.top;
				this.right = ir.right;
				this.bottom = ir.bottom;
			}

			// Token: 0x0400041A RID: 1050
			public long left;

			// Token: 0x0400041B RID: 1051
			public long top;

			// Token: 0x0400041C RID: 1052
			public long right;

			// Token: 0x0400041D RID: 1053
			public long bottom;
		}

		// Token: 0x020000D2 RID: 210
		public enum ClipType
		{
			// Token: 0x0400041F RID: 1055
			ctIntersection,
			// Token: 0x04000420 RID: 1056
			ctUnion,
			// Token: 0x04000421 RID: 1057
			ctDifference,
			// Token: 0x04000422 RID: 1058
			ctXor
		}

		// Token: 0x020000D3 RID: 211
		public enum PolyType
		{
			// Token: 0x04000424 RID: 1060
			ptSubject,
			// Token: 0x04000425 RID: 1061
			ptClip
		}

		// Token: 0x020000D4 RID: 212
		public enum PolyFillType
		{
			// Token: 0x04000427 RID: 1063
			pftEvenOdd,
			// Token: 0x04000428 RID: 1064
			pftNonZero,
			// Token: 0x04000429 RID: 1065
			pftPositive,
			// Token: 0x0400042A RID: 1066
			pftNegative
		}

		// Token: 0x020000D5 RID: 213
		public enum JoinType
		{
			// Token: 0x0400042C RID: 1068
			jtSquare,
			// Token: 0x0400042D RID: 1069
			jtRound,
			// Token: 0x0400042E RID: 1070
			jtMiter
		}

		// Token: 0x020000D6 RID: 214
		public enum EndType
		{
			// Token: 0x04000430 RID: 1072
			etClosedPolygon,
			// Token: 0x04000431 RID: 1073
			etClosedLine,
			// Token: 0x04000432 RID: 1074
			etOpenButt,
			// Token: 0x04000433 RID: 1075
			etOpenSquare,
			// Token: 0x04000434 RID: 1076
			etOpenRound
		}

		// Token: 0x020000D7 RID: 215
		internal enum EdgeSide
		{
			// Token: 0x04000436 RID: 1078
			esLeft,
			// Token: 0x04000437 RID: 1079
			esRight
		}

		// Token: 0x020000D8 RID: 216
		internal enum Direction
		{
			// Token: 0x04000439 RID: 1081
			dRightToLeft,
			// Token: 0x0400043A RID: 1082
			dLeftToRight
		}

		// Token: 0x020000D9 RID: 217
		internal class TEdge
		{
			// Token: 0x0400043B RID: 1083
			internal ClipperLib.IntPoint Bot;

			// Token: 0x0400043C RID: 1084
			internal ClipperLib.IntPoint Curr;

			// Token: 0x0400043D RID: 1085
			internal ClipperLib.IntPoint Top;

			// Token: 0x0400043E RID: 1086
			internal ClipperLib.IntPoint Delta;

			// Token: 0x0400043F RID: 1087
			internal double Dx;

			// Token: 0x04000440 RID: 1088
			internal ClipperLib.PolyType PolyTyp;

			// Token: 0x04000441 RID: 1089
			internal ClipperLib.EdgeSide Side;

			// Token: 0x04000442 RID: 1090
			internal int WindDelta;

			// Token: 0x04000443 RID: 1091
			internal int WindCnt;

			// Token: 0x04000444 RID: 1092
			internal int WindCnt2;

			// Token: 0x04000445 RID: 1093
			internal int OutIdx;

			// Token: 0x04000446 RID: 1094
			internal ClipperLib.TEdge Next;

			// Token: 0x04000447 RID: 1095
			internal ClipperLib.TEdge Prev;

			// Token: 0x04000448 RID: 1096
			internal ClipperLib.TEdge NextInLML;

			// Token: 0x04000449 RID: 1097
			internal ClipperLib.TEdge NextInAEL;

			// Token: 0x0400044A RID: 1098
			internal ClipperLib.TEdge PrevInAEL;

			// Token: 0x0400044B RID: 1099
			internal ClipperLib.TEdge NextInSEL;

			// Token: 0x0400044C RID: 1100
			internal ClipperLib.TEdge PrevInSEL;
		}

		// Token: 0x020000DA RID: 218
		public class IntersectNode
		{
			// Token: 0x0400044D RID: 1101
			internal ClipperLib.TEdge Edge1;

			// Token: 0x0400044E RID: 1102
			internal ClipperLib.TEdge Edge2;

			// Token: 0x0400044F RID: 1103
			internal ClipperLib.IntPoint Pt;
		}

		// Token: 0x020000DB RID: 219
		public class MyIntersectNodeSort : IComparer<ClipperLib.IntersectNode>
		{
			// Token: 0x060004C7 RID: 1223 RVA: 0x0001B1B4 File Offset: 0x000193B4
			public int Compare(ClipperLib.IntersectNode node1, ClipperLib.IntersectNode node2)
			{
				long num = node2.Pt.Y - node1.Pt.Y;
				if (num > 0L)
				{
					return 1;
				}
				if (num < 0L)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x020000DC RID: 220
		internal class LocalMinima
		{
			// Token: 0x04000450 RID: 1104
			internal long Y;

			// Token: 0x04000451 RID: 1105
			internal ClipperLib.TEdge LeftBound;

			// Token: 0x04000452 RID: 1106
			internal ClipperLib.TEdge RightBound;

			// Token: 0x04000453 RID: 1107
			internal ClipperLib.LocalMinima Next;
		}

		// Token: 0x020000DD RID: 221
		internal class Scanbeam
		{
			// Token: 0x04000454 RID: 1108
			internal long Y;

			// Token: 0x04000455 RID: 1109
			internal ClipperLib.Scanbeam Next;
		}

		// Token: 0x020000DE RID: 222
		internal class Maxima
		{
			// Token: 0x04000456 RID: 1110
			internal long X;

			// Token: 0x04000457 RID: 1111
			internal ClipperLib.Maxima Next;

			// Token: 0x04000458 RID: 1112
			internal ClipperLib.Maxima Prev;
		}

		// Token: 0x020000DF RID: 223
		internal class OutRec
		{
			// Token: 0x04000459 RID: 1113
			internal int Idx;

			// Token: 0x0400045A RID: 1114
			internal bool IsHole;

			// Token: 0x0400045B RID: 1115
			internal bool IsOpen;

			// Token: 0x0400045C RID: 1116
			internal ClipperLib.OutRec FirstLeft;

			// Token: 0x0400045D RID: 1117
			internal ClipperLib.OutPt Pts;

			// Token: 0x0400045E RID: 1118
			internal ClipperLib.OutPt BottomPt;

			// Token: 0x0400045F RID: 1119
			internal ClipperLib.PolyNode PolyNode;
		}

		// Token: 0x020000E0 RID: 224
		internal class OutPt
		{
			// Token: 0x04000460 RID: 1120
			internal int Idx;

			// Token: 0x04000461 RID: 1121
			internal ClipperLib.IntPoint Pt;

			// Token: 0x04000462 RID: 1122
			internal ClipperLib.OutPt Next;

			// Token: 0x04000463 RID: 1123
			internal ClipperLib.OutPt Prev;
		}

		// Token: 0x020000E1 RID: 225
		internal class Join
		{
			// Token: 0x04000464 RID: 1124
			internal ClipperLib.OutPt OutPt1;

			// Token: 0x04000465 RID: 1125
			internal ClipperLib.OutPt OutPt2;

			// Token: 0x04000466 RID: 1126
			internal ClipperLib.IntPoint OffPt;
		}

		// Token: 0x020000E2 RID: 226
		public class ClipperBase
		{
			// Token: 0x060004CF RID: 1231 RVA: 0x0001B220 File Offset: 0x00019420
			internal static bool near_zero(double val)
			{
				return val > -1E-20 && val < 1E-20;
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0001B23C File Offset: 0x0001943C
			// (set) Token: 0x060004D1 RID: 1233 RVA: 0x0001B244 File Offset: 0x00019444
			public bool PreserveCollinear { get; set; }

			// Token: 0x060004D2 RID: 1234 RVA: 0x0001B250 File Offset: 0x00019450
			public void Swap(ref long val1, ref long val2)
			{
				long num = val1;
				val1 = val2;
				val2 = num;
			}

			// Token: 0x060004D3 RID: 1235 RVA: 0x0001B267 File Offset: 0x00019467
			internal static bool IsHorizontal(ClipperLib.TEdge e)
			{
				return e.Delta.Y == 0L;
			}

			// Token: 0x060004D4 RID: 1236 RVA: 0x0001B278 File Offset: 0x00019478
			internal bool PointIsVertex(ClipperLib.IntPoint pt, ClipperLib.OutPt pp)
			{
				ClipperLib.OutPt outPt = pp;
				while (!(outPt.Pt == pt))
				{
					outPt = outPt.Next;
					if (outPt == pp)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060004D5 RID: 1237 RVA: 0x0001B2A4 File Offset: 0x000194A4
			internal bool PointOnLineSegment(ClipperLib.IntPoint pt, ClipperLib.IntPoint linePt1, ClipperLib.IntPoint linePt2, bool UseFullRange)
			{
				if (UseFullRange)
				{
					return (pt.X == linePt1.X && pt.Y == linePt1.Y) || (pt.X == linePt2.X && pt.Y == linePt2.Y) || (pt.X > linePt1.X == pt.X < linePt2.X && pt.Y > linePt1.Y == pt.Y < linePt2.Y && ClipperLib.Int128.Int128Mul(pt.X - linePt1.X, linePt2.Y - linePt1.Y) == ClipperLib.Int128.Int128Mul(linePt2.X - linePt1.X, pt.Y - linePt1.Y));
				}
				return (pt.X == linePt1.X && pt.Y == linePt1.Y) || (pt.X == linePt2.X && pt.Y == linePt2.Y) || (pt.X > linePt1.X == pt.X < linePt2.X && pt.Y > linePt1.Y == pt.Y < linePt2.Y && (pt.X - linePt1.X) * (linePt2.Y - linePt1.Y) == (linePt2.X - linePt1.X) * (pt.Y - linePt1.Y));
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x0001B430 File Offset: 0x00019630
			internal bool PointOnPolygon(ClipperLib.IntPoint pt, ClipperLib.OutPt pp, bool UseFullRange)
			{
				ClipperLib.OutPt outPt = pp;
				while (!this.PointOnLineSegment(pt, outPt.Pt, outPt.Next.Pt, UseFullRange))
				{
					outPt = outPt.Next;
					if (outPt == pp)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060004D7 RID: 1239 RVA: 0x0001B468 File Offset: 0x00019668
			internal static bool SlopesEqual(ClipperLib.TEdge e1, ClipperLib.TEdge e2, bool UseFullRange)
			{
				if (UseFullRange)
				{
					return ClipperLib.Int128.Int128Mul(e1.Delta.Y, e2.Delta.X) == ClipperLib.Int128.Int128Mul(e1.Delta.X, e2.Delta.Y);
				}
				return e1.Delta.Y * e2.Delta.X == e1.Delta.X * e2.Delta.Y;
			}

			// Token: 0x060004D8 RID: 1240 RVA: 0x0001B4E4 File Offset: 0x000196E4
			internal static bool SlopesEqual(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2, ClipperLib.IntPoint pt3, bool UseFullRange)
			{
				if (UseFullRange)
				{
					return ClipperLib.Int128.Int128Mul(pt1.Y - pt2.Y, pt2.X - pt3.X) == ClipperLib.Int128.Int128Mul(pt1.X - pt2.X, pt2.Y - pt3.Y);
				}
				return (pt1.Y - pt2.Y) * (pt2.X - pt3.X) - (pt1.X - pt2.X) * (pt2.Y - pt3.Y) == 0L;
			}

			// Token: 0x060004D9 RID: 1241 RVA: 0x0001B574 File Offset: 0x00019774
			internal static bool SlopesEqual(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2, ClipperLib.IntPoint pt3, ClipperLib.IntPoint pt4, bool UseFullRange)
			{
				if (UseFullRange)
				{
					return ClipperLib.Int128.Int128Mul(pt1.Y - pt2.Y, pt3.X - pt4.X) == ClipperLib.Int128.Int128Mul(pt1.X - pt2.X, pt3.Y - pt4.Y);
				}
				return (pt1.Y - pt2.Y) * (pt3.X - pt4.X) - (pt1.X - pt2.X) * (pt3.Y - pt4.Y) == 0L;
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x0001B604 File Offset: 0x00019804
			internal ClipperBase()
			{
				this.m_MinimaList = null;
				this.m_CurrentLM = null;
				this.m_UseFullRange = false;
				this.m_HasOpenPaths = false;
			}

			// Token: 0x060004DB RID: 1243 RVA: 0x0001B634 File Offset: 0x00019834
			public virtual void Clear()
			{
				this.DisposeLocalMinimaList();
				for (int i = 0; i < this.m_edges.Count; i++)
				{
					for (int j = 0; j < this.m_edges[i].Count; j++)
					{
						this.m_edges[i][j] = null;
					}
					this.m_edges[i].Clear();
				}
				this.m_edges.Clear();
				this.m_UseFullRange = false;
				this.m_HasOpenPaths = false;
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x0001B6B8 File Offset: 0x000198B8
			private void DisposeLocalMinimaList()
			{
				while (this.m_MinimaList != null)
				{
					ClipperLib.LocalMinima next = this.m_MinimaList.Next;
					this.m_MinimaList = null;
					this.m_MinimaList = next;
				}
				this.m_CurrentLM = null;
			}

			// Token: 0x060004DD RID: 1245 RVA: 0x0001B6F0 File Offset: 0x000198F0
			private void RangeTest(ClipperLib.IntPoint Pt, ref bool useFullRange)
			{
				if (useFullRange)
				{
					if (Pt.X > 4611686018427387903L || Pt.Y > 4611686018427387903L || -Pt.X > 4611686018427387903L || -Pt.Y > 4611686018427387903L)
					{
						throw new ClipperLib.ClipperException("Coordinate outside allowed range");
					}
				}
				else if (Pt.X > 1073741823L || Pt.Y > 1073741823L || -Pt.X > 1073741823L || -Pt.Y > 1073741823L)
				{
					useFullRange = true;
					this.RangeTest(Pt, ref useFullRange);
				}
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x0001B797 File Offset: 0x00019997
			private void InitEdge(ClipperLib.TEdge e, ClipperLib.TEdge eNext, ClipperLib.TEdge ePrev, ClipperLib.IntPoint pt)
			{
				e.Next = eNext;
				e.Prev = ePrev;
				e.Curr = pt;
				e.OutIdx = -1;
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0001B7B8 File Offset: 0x000199B8
			private void InitEdge2(ClipperLib.TEdge e, ClipperLib.PolyType polyType)
			{
				if (e.Curr.Y >= e.Next.Curr.Y)
				{
					e.Bot = e.Curr;
					e.Top = e.Next.Curr;
				}
				else
				{
					e.Top = e.Curr;
					e.Bot = e.Next.Curr;
				}
				this.SetDx(e);
				e.PolyTyp = polyType;
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x0001B82C File Offset: 0x00019A2C
			private ClipperLib.TEdge FindNextLocMin(ClipperLib.TEdge E)
			{
				ClipperLib.TEdge tedge;
				for (;;)
				{
					if (!(E.Bot != E.Prev.Bot) && !(E.Curr == E.Top))
					{
						if (E.Dx != -3.4E+38 && E.Prev.Dx != -3.4E+38)
						{
							break;
						}
						while (E.Prev.Dx == -3.4E+38)
						{
							E = E.Prev;
						}
						tedge = E;
						while (E.Dx == -3.4E+38)
						{
							E = E.Next;
						}
						if (E.Top.Y != E.Prev.Bot.Y)
						{
							goto Block_7;
						}
					}
					else
					{
						E = E.Next;
					}
				}
				return E;
				Block_7:
				if (tedge.Prev.Bot.X < E.Bot.X)
				{
					E = tedge;
				}
				return E;
			}

			// Token: 0x060004E1 RID: 1249 RVA: 0x0001B914 File Offset: 0x00019B14
			private ClipperLib.TEdge ProcessBound(ClipperLib.TEdge E, bool LeftBoundIsForward)
			{
				ClipperLib.TEdge tedge = E;
				if (tedge.OutIdx == -2)
				{
					E = tedge;
					if (LeftBoundIsForward)
					{
						while (E.Top.Y == E.Next.Bot.Y)
						{
							E = E.Next;
						}
						while (E != tedge)
						{
							if (E.Dx != -3.4E+38)
							{
								break;
							}
							E = E.Prev;
						}
					}
					else
					{
						while (E.Top.Y == E.Prev.Bot.Y)
						{
							E = E.Prev;
						}
						while (E != tedge && E.Dx == -3.4E+38)
						{
							E = E.Next;
						}
					}
					if (E == tedge)
					{
						if (LeftBoundIsForward)
						{
							tedge = E.Next;
						}
						else
						{
							tedge = E.Prev;
						}
					}
					else
					{
						if (LeftBoundIsForward)
						{
							E = tedge.Next;
						}
						else
						{
							E = tedge.Prev;
						}
						ClipperLib.LocalMinima localMinima = new ClipperLib.LocalMinima();
						localMinima.Next = null;
						localMinima.Y = E.Bot.Y;
						localMinima.LeftBound = null;
						localMinima.RightBound = E;
						E.WindDelta = 0;
						tedge = this.ProcessBound(E, LeftBoundIsForward);
						this.InsertLocalMinima(localMinima);
					}
					return tedge;
				}
				ClipperLib.TEdge tedge2;
				if (E.Dx == -3.4E+38)
				{
					if (LeftBoundIsForward)
					{
						tedge2 = E.Prev;
					}
					else
					{
						tedge2 = E.Next;
					}
					if (tedge2.Dx == -3.4E+38)
					{
						if (tedge2.Bot.X != E.Bot.X && tedge2.Top.X != E.Bot.X)
						{
							this.ReverseHorizontal(E);
						}
					}
					else if (tedge2.Bot.X != E.Bot.X)
					{
						this.ReverseHorizontal(E);
					}
				}
				tedge2 = E;
				if (LeftBoundIsForward)
				{
					while (tedge.Top.Y == tedge.Next.Bot.Y && tedge.Next.OutIdx != -2)
					{
						tedge = tedge.Next;
					}
					if (tedge.Dx == -3.4E+38 && tedge.Next.OutIdx != -2)
					{
						ClipperLib.TEdge tedge3 = tedge;
						while (tedge3.Prev.Dx == -3.4E+38)
						{
							tedge3 = tedge3.Prev;
						}
						if (tedge3.Prev.Top.X > tedge.Next.Top.X)
						{
							tedge = tedge3.Prev;
						}
					}
					while (E != tedge)
					{
						E.NextInLML = E.Next;
						if (E.Dx == -3.4E+38 && E != tedge2 && E.Bot.X != E.Prev.Top.X)
						{
							this.ReverseHorizontal(E);
						}
						E = E.Next;
					}
					if (E.Dx == -3.4E+38 && E != tedge2 && E.Bot.X != E.Prev.Top.X)
					{
						this.ReverseHorizontal(E);
					}
					tedge = tedge.Next;
				}
				else
				{
					while (tedge.Top.Y == tedge.Prev.Bot.Y && tedge.Prev.OutIdx != -2)
					{
						tedge = tedge.Prev;
					}
					if (tedge.Dx == -3.4E+38 && tedge.Prev.OutIdx != -2)
					{
						ClipperLib.TEdge tedge3 = tedge;
						while (tedge3.Next.Dx == -3.4E+38)
						{
							tedge3 = tedge3.Next;
						}
						if (tedge3.Next.Top.X == tedge.Prev.Top.X || tedge3.Next.Top.X > tedge.Prev.Top.X)
						{
							tedge = tedge3.Next;
						}
					}
					while (E != tedge)
					{
						E.NextInLML = E.Prev;
						if (E.Dx == -3.4E+38 && E != tedge2 && E.Bot.X != E.Next.Top.X)
						{
							this.ReverseHorizontal(E);
						}
						E = E.Prev;
					}
					if (E.Dx == -3.4E+38 && E != tedge2 && E.Bot.X != E.Next.Top.X)
					{
						this.ReverseHorizontal(E);
					}
					tedge = tedge.Prev;
				}
				return tedge;
			}

			// Token: 0x060004E2 RID: 1250 RVA: 0x0001BD60 File Offset: 0x00019F60
			public bool AddPath(List<ClipperLib.IntPoint> pg, ClipperLib.PolyType polyType, bool Closed)
			{
				if (!Closed && polyType == ClipperLib.PolyType.ptClip)
				{
					throw new ClipperLib.ClipperException("AddPath: Open paths must be subject.");
				}
				int i = pg.Count - 1;
				if (Closed)
				{
					while (i > 0)
					{
						if (!(pg[i] == pg[0]))
						{
							break;
						}
						i--;
					}
				}
				while (i > 0 && pg[i] == pg[i - 1])
				{
					i--;
				}
				if ((Closed && i < 2) || (!Closed && i < 1))
				{
					return false;
				}
				List<ClipperLib.TEdge> list = new List<ClipperLib.TEdge>(i + 1);
				for (int j = 0; j <= i; j++)
				{
					list.Add(new ClipperLib.TEdge());
				}
				bool flag = true;
				list[1].Curr = pg[1];
				this.RangeTest(pg[0], ref this.m_UseFullRange);
				this.RangeTest(pg[i], ref this.m_UseFullRange);
				this.InitEdge(list[0], list[1], list[i], pg[0]);
				this.InitEdge(list[i], list[0], list[i - 1], pg[i]);
				for (int k = i - 1; k >= 1; k--)
				{
					this.RangeTest(pg[k], ref this.m_UseFullRange);
					this.InitEdge(list[k], list[k + 1], list[k - 1], pg[k]);
				}
				ClipperLib.TEdge tedge = list[0];
				ClipperLib.TEdge tedge2 = tedge;
				ClipperLib.TEdge tedge3 = tedge;
				for (;;)
				{
					if (tedge2.Curr == tedge2.Next.Curr && (Closed || tedge2.Next != tedge))
					{
						if (tedge2 == tedge2.Next)
						{
							break;
						}
						if (tedge2 == tedge)
						{
							tedge = tedge2.Next;
						}
						tedge2 = this.RemoveEdge(tedge2);
						tedge3 = tedge2;
					}
					else
					{
						if (tedge2.Prev == tedge2.Next)
						{
							break;
						}
						if (Closed && ClipperLib.ClipperBase.SlopesEqual(tedge2.Prev.Curr, tedge2.Curr, tedge2.Next.Curr, this.m_UseFullRange) && (!this.PreserveCollinear || !this.Pt2IsBetweenPt1AndPt3(tedge2.Prev.Curr, tedge2.Curr, tedge2.Next.Curr)))
						{
							if (tedge2 == tedge)
							{
								tedge = tedge2.Next;
							}
							tedge2 = this.RemoveEdge(tedge2);
							tedge2 = tedge2.Prev;
							tedge3 = tedge2;
						}
						else
						{
							tedge2 = tedge2.Next;
							if (tedge2 == tedge3 || (!Closed && tedge2.Next == tedge))
							{
								break;
							}
						}
					}
				}
				if ((!Closed && tedge2 == tedge2.Next) || (Closed && tedge2.Prev == tedge2.Next))
				{
					return false;
				}
				if (!Closed)
				{
					this.m_HasOpenPaths = true;
					tedge.Prev.OutIdx = -2;
				}
				tedge2 = tedge;
				do
				{
					this.InitEdge2(tedge2, polyType);
					tedge2 = tedge2.Next;
					if (flag && tedge2.Curr.Y != tedge.Curr.Y)
					{
						flag = false;
					}
				}
				while (tedge2 != tedge);
				if (!flag)
				{
					this.m_edges.Add(list);
					ClipperLib.TEdge tedge4 = null;
					if (tedge2.Prev.Bot == tedge2.Prev.Top)
					{
						tedge2 = tedge2.Next;
					}
					for (;;)
					{
						tedge2 = this.FindNextLocMin(tedge2);
						if (tedge2 == tedge4)
						{
							break;
						}
						if (tedge4 == null)
						{
							tedge4 = tedge2;
						}
						ClipperLib.LocalMinima localMinima = new ClipperLib.LocalMinima();
						localMinima.Next = null;
						localMinima.Y = tedge2.Bot.Y;
						bool flag2;
						if (tedge2.Dx < tedge2.Prev.Dx)
						{
							localMinima.LeftBound = tedge2.Prev;
							localMinima.RightBound = tedge2;
							flag2 = false;
						}
						else
						{
							localMinima.LeftBound = tedge2;
							localMinima.RightBound = tedge2.Prev;
							flag2 = true;
						}
						localMinima.LeftBound.Side = ClipperLib.EdgeSide.esLeft;
						localMinima.RightBound.Side = ClipperLib.EdgeSide.esRight;
						if (!Closed)
						{
							localMinima.LeftBound.WindDelta = 0;
						}
						else if (localMinima.LeftBound.Next == localMinima.RightBound)
						{
							localMinima.LeftBound.WindDelta = -1;
						}
						else
						{
							localMinima.LeftBound.WindDelta = 1;
						}
						localMinima.RightBound.WindDelta = -localMinima.LeftBound.WindDelta;
						tedge2 = this.ProcessBound(localMinima.LeftBound, flag2);
						if (tedge2.OutIdx == -2)
						{
							tedge2 = this.ProcessBound(tedge2, flag2);
						}
						ClipperLib.TEdge tedge5 = this.ProcessBound(localMinima.RightBound, !flag2);
						if (tedge5.OutIdx == -2)
						{
							tedge5 = this.ProcessBound(tedge5, !flag2);
						}
						if (localMinima.LeftBound.OutIdx == -2)
						{
							localMinima.LeftBound = null;
						}
						else if (localMinima.RightBound.OutIdx == -2)
						{
							localMinima.RightBound = null;
						}
						this.InsertLocalMinima(localMinima);
						if (!flag2)
						{
							tedge2 = tedge5;
						}
					}
					return true;
				}
				if (Closed)
				{
					return false;
				}
				tedge2.Prev.OutIdx = -2;
				ClipperLib.LocalMinima localMinima2 = new ClipperLib.LocalMinima();
				localMinima2.Next = null;
				localMinima2.Y = tedge2.Bot.Y;
				localMinima2.LeftBound = null;
				localMinima2.RightBound = tedge2;
				localMinima2.RightBound.Side = ClipperLib.EdgeSide.esRight;
				localMinima2.RightBound.WindDelta = 0;
				for (;;)
				{
					if (tedge2.Bot.X != tedge2.Prev.Top.X)
					{
						this.ReverseHorizontal(tedge2);
					}
					if (tedge2.Next.OutIdx == -2)
					{
						break;
					}
					tedge2.NextInLML = tedge2.Next;
					tedge2 = tedge2.Next;
				}
				this.InsertLocalMinima(localMinima2);
				this.m_edges.Add(list);
				return true;
			}

			// Token: 0x060004E3 RID: 1251 RVA: 0x0001C304 File Offset: 0x0001A504
			public bool AddPaths(List<List<ClipperLib.IntPoint>> ppg, ClipperLib.PolyType polyType, bool closed)
			{
				bool flag = false;
				for (int i = 0; i < ppg.Count; i++)
				{
					if (this.AddPath(ppg[i], polyType, closed))
					{
						flag = true;
					}
				}
				return flag;
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x0001C338 File Offset: 0x0001A538
			internal bool Pt2IsBetweenPt1AndPt3(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2, ClipperLib.IntPoint pt3)
			{
				if (pt1 == pt3 || pt1 == pt2 || pt3 == pt2)
				{
					return false;
				}
				if (pt1.X != pt3.X)
				{
					return pt2.X > pt1.X == pt2.X < pt3.X;
				}
				return pt2.Y > pt1.Y == pt2.Y < pt3.Y;
			}

			// Token: 0x060004E5 RID: 1253 RVA: 0x0001C3AD File Offset: 0x0001A5AD
			private ClipperLib.TEdge RemoveEdge(ClipperLib.TEdge e)
			{
				e.Prev.Next = e.Next;
				e.Next.Prev = e.Prev;
				ClipperLib.TEdge next = e.Next;
				e.Prev = null;
				return next;
			}

			// Token: 0x060004E6 RID: 1254 RVA: 0x0001C3E0 File Offset: 0x0001A5E0
			private void SetDx(ClipperLib.TEdge e)
			{
				e.Delta.X = e.Top.X - e.Bot.X;
				e.Delta.Y = e.Top.Y - e.Bot.Y;
				if (e.Delta.Y == 0L)
				{
					e.Dx = -3.4E+38;
					return;
				}
				e.Dx = (double)e.Delta.X / (double)e.Delta.Y;
			}

			// Token: 0x060004E7 RID: 1255 RVA: 0x0001C470 File Offset: 0x0001A670
			private void InsertLocalMinima(ClipperLib.LocalMinima newLm)
			{
				if (this.m_MinimaList == null)
				{
					this.m_MinimaList = newLm;
					return;
				}
				if (newLm.Y >= this.m_MinimaList.Y)
				{
					newLm.Next = this.m_MinimaList;
					this.m_MinimaList = newLm;
					return;
				}
				ClipperLib.LocalMinima localMinima = this.m_MinimaList;
				while (localMinima.Next != null && newLm.Y < localMinima.Next.Y)
				{
					localMinima = localMinima.Next;
				}
				newLm.Next = localMinima.Next;
				localMinima.Next = newLm;
			}

			// Token: 0x060004E8 RID: 1256 RVA: 0x0001C4F2 File Offset: 0x0001A6F2
			internal bool PopLocalMinima(long Y, out ClipperLib.LocalMinima current)
			{
				current = this.m_CurrentLM;
				if (this.m_CurrentLM != null && this.m_CurrentLM.Y == Y)
				{
					this.m_CurrentLM = this.m_CurrentLM.Next;
					return true;
				}
				return false;
			}

			// Token: 0x060004E9 RID: 1257 RVA: 0x0001C526 File Offset: 0x0001A726
			private void ReverseHorizontal(ClipperLib.TEdge e)
			{
				this.Swap(ref e.Top.X, ref e.Bot.X);
			}

			// Token: 0x060004EA RID: 1258 RVA: 0x0001C544 File Offset: 0x0001A744
			internal virtual void Reset()
			{
				this.m_CurrentLM = this.m_MinimaList;
				if (this.m_CurrentLM == null)
				{
					return;
				}
				this.m_Scanbeam = null;
				for (ClipperLib.LocalMinima localMinima = this.m_MinimaList; localMinima != null; localMinima = localMinima.Next)
				{
					this.InsertScanbeam(localMinima.Y);
					ClipperLib.TEdge tedge = localMinima.LeftBound;
					if (tedge != null)
					{
						tedge.Curr = tedge.Bot;
						tedge.OutIdx = -1;
					}
					tedge = localMinima.RightBound;
					if (tedge != null)
					{
						tedge.Curr = tedge.Bot;
						tedge.OutIdx = -1;
					}
				}
				this.m_ActiveEdges = null;
			}

			// Token: 0x060004EB RID: 1259 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
			public static ClipperLib.IntRect GetBounds(List<List<ClipperLib.IntPoint>> paths)
			{
				int i = 0;
				int count = paths.Count;
				while (i < count && paths[i].Count == 0)
				{
					i++;
				}
				if (i == count)
				{
					return new ClipperLib.IntRect(0L, 0L, 0L, 0L);
				}
				ClipperLib.IntRect intRect = default(ClipperLib.IntRect);
				intRect.left = paths[i][0].X;
				intRect.right = intRect.left;
				intRect.top = paths[i][0].Y;
				intRect.bottom = intRect.top;
				while (i < count)
				{
					for (int j = 0; j < paths[i].Count; j++)
					{
						if (paths[i][j].X < intRect.left)
						{
							intRect.left = paths[i][j].X;
						}
						else if (paths[i][j].X > intRect.right)
						{
							intRect.right = paths[i][j].X;
						}
						if (paths[i][j].Y < intRect.top)
						{
							intRect.top = paths[i][j].Y;
						}
						else if (paths[i][j].Y > intRect.bottom)
						{
							intRect.bottom = paths[i][j].Y;
						}
					}
					i++;
				}
				return intRect;
			}

			// Token: 0x060004EC RID: 1260 RVA: 0x0001C764 File Offset: 0x0001A964
			internal void InsertScanbeam(long Y)
			{
				if (this.m_Scanbeam == null)
				{
					this.m_Scanbeam = new ClipperLib.Scanbeam();
					this.m_Scanbeam.Next = null;
					this.m_Scanbeam.Y = Y;
					return;
				}
				if (Y > this.m_Scanbeam.Y)
				{
					this.m_Scanbeam = new ClipperLib.Scanbeam
					{
						Y = Y,
						Next = this.m_Scanbeam
					};
					return;
				}
				ClipperLib.Scanbeam scanbeam = this.m_Scanbeam;
				while (scanbeam.Next != null && Y <= scanbeam.Next.Y)
				{
					scanbeam = scanbeam.Next;
				}
				if (Y == scanbeam.Y)
				{
					return;
				}
				scanbeam.Next = new ClipperLib.Scanbeam
				{
					Y = Y,
					Next = scanbeam.Next
				};
			}

			// Token: 0x060004ED RID: 1261 RVA: 0x0001C81C File Offset: 0x0001AA1C
			internal bool PopScanbeam(out long Y)
			{
				if (this.m_Scanbeam == null)
				{
					Y = 0L;
					return false;
				}
				Y = this.m_Scanbeam.Y;
				this.m_Scanbeam = this.m_Scanbeam.Next;
				return true;
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x0001C84B File Offset: 0x0001AA4B
			internal bool LocalMinimaPending()
			{
				return this.m_CurrentLM != null;
			}

			// Token: 0x060004EF RID: 1263 RVA: 0x0001C858 File Offset: 0x0001AA58
			internal ClipperLib.OutRec CreateOutRec()
			{
				ClipperLib.OutRec outRec = new ClipperLib.OutRec();
				outRec.Idx = -1;
				outRec.IsHole = false;
				outRec.IsOpen = false;
				outRec.FirstLeft = null;
				outRec.Pts = null;
				outRec.BottomPt = null;
				outRec.PolyNode = null;
				this.m_PolyOuts.Add(outRec);
				outRec.Idx = this.m_PolyOuts.Count - 1;
				return outRec;
			}

			// Token: 0x060004F0 RID: 1264 RVA: 0x0001C8BC File Offset: 0x0001AABC
			internal void DisposeOutRec(int index)
			{
				this.m_PolyOuts[index].Pts = null;
				this.m_PolyOuts[index] = null;
			}

			// Token: 0x060004F1 RID: 1265 RVA: 0x0001C8E0 File Offset: 0x0001AAE0
			internal void UpdateEdgeIntoAEL(ref ClipperLib.TEdge e)
			{
				if (e.NextInLML == null)
				{
					throw new ClipperLib.ClipperException("UpdateEdgeIntoAEL: invalid call");
				}
				ClipperLib.TEdge prevInAEL = e.PrevInAEL;
				ClipperLib.TEdge nextInAEL = e.NextInAEL;
				e.NextInLML.OutIdx = e.OutIdx;
				if (prevInAEL != null)
				{
					prevInAEL.NextInAEL = e.NextInLML;
				}
				else
				{
					this.m_ActiveEdges = e.NextInLML;
				}
				if (nextInAEL != null)
				{
					nextInAEL.PrevInAEL = e.NextInLML;
				}
				e.NextInLML.Side = e.Side;
				e.NextInLML.WindDelta = e.WindDelta;
				e.NextInLML.WindCnt = e.WindCnt;
				e.NextInLML.WindCnt2 = e.WindCnt2;
				e = e.NextInLML;
				e.Curr = e.Bot;
				e.PrevInAEL = prevInAEL;
				e.NextInAEL = nextInAEL;
				if (!ClipperLib.ClipperBase.IsHorizontal(e))
				{
					this.InsertScanbeam(e.Top.Y);
				}
			}

			// Token: 0x060004F2 RID: 1266 RVA: 0x0001C9E4 File Offset: 0x0001ABE4
			internal void SwapPositionsInAEL(ClipperLib.TEdge edge1, ClipperLib.TEdge edge2)
			{
				if (edge1.NextInAEL == edge1.PrevInAEL || edge2.NextInAEL == edge2.PrevInAEL)
				{
					return;
				}
				if (edge1.NextInAEL == edge2)
				{
					ClipperLib.TEdge nextInAEL = edge2.NextInAEL;
					if (nextInAEL != null)
					{
						nextInAEL.PrevInAEL = edge1;
					}
					ClipperLib.TEdge prevInAEL = edge1.PrevInAEL;
					if (prevInAEL != null)
					{
						prevInAEL.NextInAEL = edge2;
					}
					edge2.PrevInAEL = prevInAEL;
					edge2.NextInAEL = edge1;
					edge1.PrevInAEL = edge2;
					edge1.NextInAEL = nextInAEL;
				}
				else if (edge2.NextInAEL == edge1)
				{
					ClipperLib.TEdge nextInAEL2 = edge1.NextInAEL;
					if (nextInAEL2 != null)
					{
						nextInAEL2.PrevInAEL = edge2;
					}
					ClipperLib.TEdge prevInAEL2 = edge2.PrevInAEL;
					if (prevInAEL2 != null)
					{
						prevInAEL2.NextInAEL = edge1;
					}
					edge1.PrevInAEL = prevInAEL2;
					edge1.NextInAEL = edge2;
					edge2.PrevInAEL = edge1;
					edge2.NextInAEL = nextInAEL2;
				}
				else
				{
					ClipperLib.TEdge nextInAEL3 = edge1.NextInAEL;
					ClipperLib.TEdge prevInAEL3 = edge1.PrevInAEL;
					edge1.NextInAEL = edge2.NextInAEL;
					if (edge1.NextInAEL != null)
					{
						edge1.NextInAEL.PrevInAEL = edge1;
					}
					edge1.PrevInAEL = edge2.PrevInAEL;
					if (edge1.PrevInAEL != null)
					{
						edge1.PrevInAEL.NextInAEL = edge1;
					}
					edge2.NextInAEL = nextInAEL3;
					if (edge2.NextInAEL != null)
					{
						edge2.NextInAEL.PrevInAEL = edge2;
					}
					edge2.PrevInAEL = prevInAEL3;
					if (edge2.PrevInAEL != null)
					{
						edge2.PrevInAEL.NextInAEL = edge2;
					}
				}
				if (edge1.PrevInAEL == null)
				{
					this.m_ActiveEdges = edge1;
					return;
				}
				if (edge2.PrevInAEL == null)
				{
					this.m_ActiveEdges = edge2;
				}
			}

			// Token: 0x060004F3 RID: 1267 RVA: 0x0001CB50 File Offset: 0x0001AD50
			internal void DeleteFromAEL(ClipperLib.TEdge e)
			{
				ClipperLib.TEdge prevInAEL = e.PrevInAEL;
				ClipperLib.TEdge nextInAEL = e.NextInAEL;
				if (prevInAEL == null && nextInAEL == null && e != this.m_ActiveEdges)
				{
					return;
				}
				if (prevInAEL != null)
				{
					prevInAEL.NextInAEL = nextInAEL;
				}
				else
				{
					this.m_ActiveEdges = nextInAEL;
				}
				if (nextInAEL != null)
				{
					nextInAEL.PrevInAEL = prevInAEL;
				}
				e.NextInAEL = null;
				e.PrevInAEL = null;
			}

			// Token: 0x04000467 RID: 1127
			internal const double horizontal = -3.4E+38;

			// Token: 0x04000468 RID: 1128
			internal const int Skip = -2;

			// Token: 0x04000469 RID: 1129
			internal const int Unassigned = -1;

			// Token: 0x0400046A RID: 1130
			internal const double tolerance = 1E-20;

			// Token: 0x0400046B RID: 1131
			public const long loRange = 1073741823L;

			// Token: 0x0400046C RID: 1132
			public const long hiRange = 4611686018427387903L;

			// Token: 0x0400046D RID: 1133
			internal ClipperLib.LocalMinima m_MinimaList;

			// Token: 0x0400046E RID: 1134
			internal ClipperLib.LocalMinima m_CurrentLM;

			// Token: 0x0400046F RID: 1135
			internal List<List<ClipperLib.TEdge>> m_edges = new List<List<ClipperLib.TEdge>>();

			// Token: 0x04000470 RID: 1136
			internal ClipperLib.Scanbeam m_Scanbeam;

			// Token: 0x04000471 RID: 1137
			internal List<ClipperLib.OutRec> m_PolyOuts;

			// Token: 0x04000472 RID: 1138
			internal ClipperLib.TEdge m_ActiveEdges;

			// Token: 0x04000473 RID: 1139
			internal bool m_UseFullRange;

			// Token: 0x04000474 RID: 1140
			internal bool m_HasOpenPaths;
		}

		// Token: 0x020000E3 RID: 227
		public class Clipper : ClipperLib.ClipperBase
		{
			// Token: 0x060004F4 RID: 1268 RVA: 0x0001CBA8 File Offset: 0x0001ADA8
			public Clipper(int InitOptions = 0)
			{
				this.m_Scanbeam = null;
				this.m_Maxima = null;
				this.m_ActiveEdges = null;
				this.m_SortedEdges = null;
				this.m_IntersectList = new List<ClipperLib.IntersectNode>();
				this.m_IntersectNodeComparer = new ClipperLib.MyIntersectNodeSort();
				this.m_ExecuteLocked = false;
				this.m_UsingPolyTree = false;
				this.m_PolyOuts = new List<ClipperLib.OutRec>();
				this.m_Joins = new List<ClipperLib.Join>();
				this.m_GhostJoins = new List<ClipperLib.Join>();
				this.ReverseSolution = (1 & InitOptions) != 0;
				this.StrictlySimple = (2 & InitOptions) != 0;
				base.PreserveCollinear = (4 & InitOptions) != 0;
			}

			// Token: 0x060004F5 RID: 1269 RVA: 0x0001CC40 File Offset: 0x0001AE40
			private void InsertMaxima(long X)
			{
				ClipperLib.Maxima maxima = new ClipperLib.Maxima();
				maxima.X = X;
				if (this.m_Maxima == null)
				{
					this.m_Maxima = maxima;
					this.m_Maxima.Next = null;
					this.m_Maxima.Prev = null;
					return;
				}
				if (X < this.m_Maxima.X)
				{
					maxima.Next = this.m_Maxima;
					maxima.Prev = null;
					this.m_Maxima = maxima;
					return;
				}
				ClipperLib.Maxima maxima2 = this.m_Maxima;
				while (maxima2.Next != null && X >= maxima2.Next.X)
				{
					maxima2 = maxima2.Next;
				}
				if (X == maxima2.X)
				{
					return;
				}
				maxima.Next = maxima2.Next;
				maxima.Prev = maxima2;
				if (maxima2.Next != null)
				{
					maxima2.Next.Prev = maxima;
				}
				maxima2.Next = maxima;
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0001CD09 File Offset: 0x0001AF09
			// (set) Token: 0x060004F7 RID: 1271 RVA: 0x0001CD11 File Offset: 0x0001AF11
			public bool ReverseSolution { get; set; }

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0001CD1A File Offset: 0x0001AF1A
			// (set) Token: 0x060004F9 RID: 1273 RVA: 0x0001CD22 File Offset: 0x0001AF22
			public bool StrictlySimple { get; set; }

			// Token: 0x060004FA RID: 1274 RVA: 0x0001CD2B File Offset: 0x0001AF2B
			public bool Execute(ClipperLib.ClipType clipType, List<List<ClipperLib.IntPoint>> solution, ClipperLib.PolyFillType FillType = ClipperLib.PolyFillType.pftEvenOdd)
			{
				return this.Execute(clipType, solution, FillType, FillType);
			}

			// Token: 0x060004FB RID: 1275 RVA: 0x0001CD37 File Offset: 0x0001AF37
			public bool Execute(ClipperLib.ClipType clipType, ClipperLib.PolyTree polytree, ClipperLib.PolyFillType FillType = ClipperLib.PolyFillType.pftEvenOdd)
			{
				return this.Execute(clipType, polytree, FillType, FillType);
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x0001CD44 File Offset: 0x0001AF44
			public bool Execute(ClipperLib.ClipType clipType, List<List<ClipperLib.IntPoint>> solution, ClipperLib.PolyFillType subjFillType, ClipperLib.PolyFillType clipFillType)
			{
				if (this.m_ExecuteLocked)
				{
					return false;
				}
				if (this.m_HasOpenPaths)
				{
					throw new ClipperLib.ClipperException("Error: PolyTree struct is needed for open path clipping.");
				}
				this.m_ExecuteLocked = true;
				solution.Clear();
				this.m_SubjFillType = subjFillType;
				this.m_ClipFillType = clipFillType;
				this.m_ClipType = clipType;
				this.m_UsingPolyTree = false;
				bool flag;
				try
				{
					flag = this.ExecuteInternal();
					if (flag)
					{
						this.BuildResult(solution);
					}
				}
				finally
				{
					this.DisposeAllPolyPts();
					this.m_ExecuteLocked = false;
				}
				return flag;
			}

			// Token: 0x060004FD RID: 1277 RVA: 0x0001CDCC File Offset: 0x0001AFCC
			public bool Execute(ClipperLib.ClipType clipType, ClipperLib.PolyTree polytree, ClipperLib.PolyFillType subjFillType, ClipperLib.PolyFillType clipFillType)
			{
				if (this.m_ExecuteLocked)
				{
					return false;
				}
				this.m_ExecuteLocked = true;
				this.m_SubjFillType = subjFillType;
				this.m_ClipFillType = clipFillType;
				this.m_ClipType = clipType;
				this.m_UsingPolyTree = true;
				bool flag;
				try
				{
					flag = this.ExecuteInternal();
					if (flag)
					{
						this.BuildResult2(polytree);
					}
				}
				finally
				{
					this.DisposeAllPolyPts();
					this.m_ExecuteLocked = false;
				}
				return flag;
			}

			// Token: 0x060004FE RID: 1278 RVA: 0x0001CE3C File Offset: 0x0001B03C
			internal void FixHoleLinkage(ClipperLib.OutRec outRec)
			{
				if (outRec.FirstLeft == null || (outRec.IsHole != outRec.FirstLeft.IsHole && outRec.FirstLeft.Pts != null))
				{
					return;
				}
				ClipperLib.OutRec outRec2 = outRec.FirstLeft;
				while (outRec2 != null && (outRec2.IsHole == outRec.IsHole || outRec2.Pts == null))
				{
					outRec2 = outRec2.FirstLeft;
				}
				outRec.FirstLeft = outRec2;
			}

			// Token: 0x060004FF RID: 1279 RVA: 0x0001CEA4 File Offset: 0x0001B0A4
			private bool ExecuteInternal()
			{
				bool flag;
				try
				{
					this.Reset();
					this.m_SortedEdges = null;
					this.m_Maxima = null;
					long num;
					if (!base.PopScanbeam(out num))
					{
						flag = false;
					}
					else
					{
						this.InsertLocalMinimaIntoAEL(num);
						long num2;
						while (base.PopScanbeam(out num2) || base.LocalMinimaPending())
						{
							this.ProcessHorizontals();
							this.m_GhostJoins.Clear();
							if (!this.ProcessIntersections(num2))
							{
								return false;
							}
							this.ProcessEdgesAtTopOfScanbeam(num2);
							num = num2;
							this.InsertLocalMinimaIntoAEL(num);
						}
						foreach (ClipperLib.OutRec outRec in this.m_PolyOuts)
						{
							if (outRec.Pts != null && !outRec.IsOpen && (outRec.IsHole ^ this.ReverseSolution) == this.Area(outRec) > 0.0)
							{
								this.ReversePolyPtLinks(outRec.Pts);
							}
						}
						this.JoinCommonEdges();
						foreach (ClipperLib.OutRec outRec2 in this.m_PolyOuts)
						{
							if (outRec2.Pts != null)
							{
								if (outRec2.IsOpen)
								{
									this.FixupOutPolyline(outRec2);
								}
								else
								{
									this.FixupOutPolygon(outRec2);
								}
							}
						}
						if (this.StrictlySimple)
						{
							this.DoSimplePolygons();
						}
						flag = true;
					}
				}
				finally
				{
					this.m_Joins.Clear();
					this.m_GhostJoins.Clear();
				}
				return flag;
			}

			// Token: 0x06000500 RID: 1280 RVA: 0x0001D064 File Offset: 0x0001B264
			private void DisposeAllPolyPts()
			{
				for (int i = 0; i < this.m_PolyOuts.Count; i++)
				{
					base.DisposeOutRec(i);
				}
				this.m_PolyOuts.Clear();
			}

			// Token: 0x06000501 RID: 1281 RVA: 0x0001D09C File Offset: 0x0001B29C
			private void AddJoin(ClipperLib.OutPt Op1, ClipperLib.OutPt Op2, ClipperLib.IntPoint OffPt)
			{
				ClipperLib.Join join = new ClipperLib.Join();
				join.OutPt1 = Op1;
				join.OutPt2 = Op2;
				join.OffPt = OffPt;
				this.m_Joins.Add(join);
			}

			// Token: 0x06000502 RID: 1282 RVA: 0x0001D0D0 File Offset: 0x0001B2D0
			private void AddGhostJoin(ClipperLib.OutPt Op, ClipperLib.IntPoint OffPt)
			{
				ClipperLib.Join join = new ClipperLib.Join();
				join.OutPt1 = Op;
				join.OffPt = OffPt;
				this.m_GhostJoins.Add(join);
			}

			// Token: 0x06000503 RID: 1283 RVA: 0x0001D100 File Offset: 0x0001B300
			private void InsertLocalMinimaIntoAEL(long botY)
			{
				ClipperLib.LocalMinima localMinima;
				while (base.PopLocalMinima(botY, out localMinima))
				{
					ClipperLib.TEdge leftBound = localMinima.LeftBound;
					ClipperLib.TEdge rightBound = localMinima.RightBound;
					ClipperLib.OutPt outPt = null;
					if (leftBound == null)
					{
						this.InsertEdgeIntoAEL(rightBound, null);
						this.SetWindingCount(rightBound);
						if (this.IsContributing(rightBound))
						{
							outPt = this.AddOutPt(rightBound, rightBound.Bot);
						}
					}
					else if (rightBound == null)
					{
						this.InsertEdgeIntoAEL(leftBound, null);
						this.SetWindingCount(leftBound);
						if (this.IsContributing(leftBound))
						{
							outPt = this.AddOutPt(leftBound, leftBound.Bot);
						}
						base.InsertScanbeam(leftBound.Top.Y);
					}
					else
					{
						this.InsertEdgeIntoAEL(leftBound, null);
						this.InsertEdgeIntoAEL(rightBound, leftBound);
						this.SetWindingCount(leftBound);
						rightBound.WindCnt = leftBound.WindCnt;
						rightBound.WindCnt2 = leftBound.WindCnt2;
						if (this.IsContributing(leftBound))
						{
							outPt = this.AddLocalMinPoly(leftBound, rightBound, leftBound.Bot);
						}
						base.InsertScanbeam(leftBound.Top.Y);
					}
					if (rightBound != null)
					{
						if (ClipperLib.ClipperBase.IsHorizontal(rightBound))
						{
							if (rightBound.NextInLML != null)
							{
								base.InsertScanbeam(rightBound.NextInLML.Top.Y);
							}
							this.AddEdgeToSEL(rightBound);
						}
						else
						{
							base.InsertScanbeam(rightBound.Top.Y);
						}
					}
					if (leftBound != null && rightBound != null)
					{
						if (outPt != null && ClipperLib.ClipperBase.IsHorizontal(rightBound) && this.m_GhostJoins.Count > 0 && rightBound.WindDelta != 0)
						{
							for (int i = 0; i < this.m_GhostJoins.Count; i++)
							{
								ClipperLib.Join join = this.m_GhostJoins[i];
								if (this.HorzSegmentsOverlap(join.OutPt1.Pt.X, join.OffPt.X, rightBound.Bot.X, rightBound.Top.X))
								{
									this.AddJoin(join.OutPt1, outPt, join.OffPt);
								}
							}
						}
						if (leftBound.OutIdx >= 0 && leftBound.PrevInAEL != null && leftBound.PrevInAEL.Curr.X == leftBound.Bot.X && leftBound.PrevInAEL.OutIdx >= 0 && ClipperLib.ClipperBase.SlopesEqual(leftBound.PrevInAEL.Curr, leftBound.PrevInAEL.Top, leftBound.Curr, leftBound.Top, this.m_UseFullRange) && leftBound.WindDelta != 0 && leftBound.PrevInAEL.WindDelta != 0)
						{
							ClipperLib.OutPt outPt2 = this.AddOutPt(leftBound.PrevInAEL, leftBound.Bot);
							this.AddJoin(outPt, outPt2, leftBound.Top);
						}
						if (leftBound.NextInAEL != rightBound)
						{
							if (rightBound.OutIdx >= 0 && rightBound.PrevInAEL.OutIdx >= 0 && ClipperLib.ClipperBase.SlopesEqual(rightBound.PrevInAEL.Curr, rightBound.PrevInAEL.Top, rightBound.Curr, rightBound.Top, this.m_UseFullRange) && rightBound.WindDelta != 0 && rightBound.PrevInAEL.WindDelta != 0)
							{
								ClipperLib.OutPt outPt3 = this.AddOutPt(rightBound.PrevInAEL, rightBound.Bot);
								this.AddJoin(outPt, outPt3, rightBound.Top);
							}
							ClipperLib.TEdge tedge = leftBound.NextInAEL;
							if (tedge != null)
							{
								while (tedge != rightBound)
								{
									this.IntersectEdges(rightBound, tedge, leftBound.Curr);
									tedge = tedge.NextInAEL;
								}
							}
						}
					}
				}
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x0001D448 File Offset: 0x0001B648
			private void InsertEdgeIntoAEL(ClipperLib.TEdge edge, ClipperLib.TEdge startEdge)
			{
				if (this.m_ActiveEdges == null)
				{
					edge.PrevInAEL = null;
					edge.NextInAEL = null;
					this.m_ActiveEdges = edge;
					return;
				}
				if (startEdge == null && this.E2InsertsBeforeE1(this.m_ActiveEdges, edge))
				{
					edge.PrevInAEL = null;
					edge.NextInAEL = this.m_ActiveEdges;
					this.m_ActiveEdges.PrevInAEL = edge;
					this.m_ActiveEdges = edge;
					return;
				}
				if (startEdge == null)
				{
					startEdge = this.m_ActiveEdges;
				}
				while (startEdge.NextInAEL != null && !this.E2InsertsBeforeE1(startEdge.NextInAEL, edge))
				{
					startEdge = startEdge.NextInAEL;
				}
				edge.NextInAEL = startEdge.NextInAEL;
				if (startEdge.NextInAEL != null)
				{
					startEdge.NextInAEL.PrevInAEL = edge;
				}
				edge.PrevInAEL = startEdge;
				startEdge.NextInAEL = edge;
			}

			// Token: 0x06000505 RID: 1285 RVA: 0x0001D508 File Offset: 0x0001B708
			private bool E2InsertsBeforeE1(ClipperLib.TEdge e1, ClipperLib.TEdge e2)
			{
				if (e2.Curr.X != e1.Curr.X)
				{
					return e2.Curr.X < e1.Curr.X;
				}
				if (e2.Top.Y > e1.Top.Y)
				{
					return e2.Top.X < ClipperLib.Clipper.TopX(e1, e2.Top.Y);
				}
				return e1.Top.X > ClipperLib.Clipper.TopX(e2, e1.Top.Y);
			}

			// Token: 0x06000506 RID: 1286 RVA: 0x0001D59B File Offset: 0x0001B79B
			private bool IsEvenOddFillType(ClipperLib.TEdge edge)
			{
				if (edge.PolyTyp == ClipperLib.PolyType.ptSubject)
				{
					return this.m_SubjFillType == ClipperLib.PolyFillType.pftEvenOdd;
				}
				return this.m_ClipFillType == ClipperLib.PolyFillType.pftEvenOdd;
			}

			// Token: 0x06000507 RID: 1287 RVA: 0x0001D5B8 File Offset: 0x0001B7B8
			private bool IsEvenOddAltFillType(ClipperLib.TEdge edge)
			{
				if (edge.PolyTyp == ClipperLib.PolyType.ptSubject)
				{
					return this.m_ClipFillType == ClipperLib.PolyFillType.pftEvenOdd;
				}
				return this.m_SubjFillType == ClipperLib.PolyFillType.pftEvenOdd;
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x0001D5D8 File Offset: 0x0001B7D8
			private bool IsContributing(ClipperLib.TEdge edge)
			{
				ClipperLib.PolyFillType polyFillType;
				ClipperLib.PolyFillType polyFillType2;
				if (edge.PolyTyp == ClipperLib.PolyType.ptSubject)
				{
					polyFillType = this.m_SubjFillType;
					polyFillType2 = this.m_ClipFillType;
				}
				else
				{
					polyFillType = this.m_ClipFillType;
					polyFillType2 = this.m_SubjFillType;
				}
				switch (polyFillType)
				{
				case ClipperLib.PolyFillType.pftEvenOdd:
					if (edge.WindDelta == 0 && edge.WindCnt != 1)
					{
						return false;
					}
					break;
				case ClipperLib.PolyFillType.pftNonZero:
					if (Math.Abs(edge.WindCnt) != 1)
					{
						return false;
					}
					break;
				case ClipperLib.PolyFillType.pftPositive:
					if (edge.WindCnt != 1)
					{
						return false;
					}
					break;
				default:
					if (edge.WindCnt != -1)
					{
						return false;
					}
					break;
				}
				switch (this.m_ClipType)
				{
				case ClipperLib.ClipType.ctIntersection:
					if (polyFillType2 <= ClipperLib.PolyFillType.pftNonZero)
					{
						return edge.WindCnt2 != 0;
					}
					if (polyFillType2 != ClipperLib.PolyFillType.pftPositive)
					{
						return edge.WindCnt2 < 0;
					}
					return edge.WindCnt2 > 0;
				case ClipperLib.ClipType.ctUnion:
					if (polyFillType2 <= ClipperLib.PolyFillType.pftNonZero)
					{
						return edge.WindCnt2 == 0;
					}
					if (polyFillType2 != ClipperLib.PolyFillType.pftPositive)
					{
						return edge.WindCnt2 >= 0;
					}
					return edge.WindCnt2 <= 0;
				case ClipperLib.ClipType.ctDifference:
					if (edge.PolyTyp == ClipperLib.PolyType.ptSubject)
					{
						if (polyFillType2 <= ClipperLib.PolyFillType.pftNonZero)
						{
							return edge.WindCnt2 == 0;
						}
						if (polyFillType2 != ClipperLib.PolyFillType.pftPositive)
						{
							return edge.WindCnt2 >= 0;
						}
						return edge.WindCnt2 <= 0;
					}
					else
					{
						if (polyFillType2 <= ClipperLib.PolyFillType.pftNonZero)
						{
							return edge.WindCnt2 != 0;
						}
						if (polyFillType2 != ClipperLib.PolyFillType.pftPositive)
						{
							return edge.WindCnt2 < 0;
						}
						return edge.WindCnt2 > 0;
					}
					break;
				case ClipperLib.ClipType.ctXor:
					if (edge.WindDelta != 0)
					{
						return true;
					}
					if (polyFillType2 <= ClipperLib.PolyFillType.pftNonZero)
					{
						return edge.WindCnt2 == 0;
					}
					if (polyFillType2 != ClipperLib.PolyFillType.pftPositive)
					{
						return edge.WindCnt2 >= 0;
					}
					return edge.WindCnt2 <= 0;
				default:
					return true;
				}
			}

			// Token: 0x06000509 RID: 1289 RVA: 0x0001D768 File Offset: 0x0001B968
			private void SetWindingCount(ClipperLib.TEdge edge)
			{
				ClipperLib.TEdge tedge = edge.PrevInAEL;
				while (tedge != null && (tedge.PolyTyp != edge.PolyTyp || tedge.WindDelta == 0))
				{
					tedge = tedge.PrevInAEL;
				}
				if (tedge == null)
				{
					ClipperLib.PolyFillType polyFillType = ((edge.PolyTyp == ClipperLib.PolyType.ptSubject) ? this.m_SubjFillType : this.m_ClipFillType);
					if (edge.WindDelta == 0)
					{
						edge.WindCnt = ((polyFillType == ClipperLib.PolyFillType.pftNegative) ? (-1) : 1);
					}
					else
					{
						edge.WindCnt = edge.WindDelta;
					}
					edge.WindCnt2 = 0;
					tedge = this.m_ActiveEdges;
				}
				else if (edge.WindDelta == 0 && this.m_ClipType != ClipperLib.ClipType.ctUnion)
				{
					edge.WindCnt = 1;
					edge.WindCnt2 = tedge.WindCnt2;
					tedge = tedge.NextInAEL;
				}
				else if (this.IsEvenOddFillType(edge))
				{
					if (edge.WindDelta == 0)
					{
						bool flag = true;
						for (ClipperLib.TEdge tedge2 = tedge.PrevInAEL; tedge2 != null; tedge2 = tedge2.PrevInAEL)
						{
							if (tedge2.PolyTyp == tedge.PolyTyp && tedge2.WindDelta != 0)
							{
								flag = !flag;
							}
						}
						edge.WindCnt = (flag ? 0 : 1);
					}
					else
					{
						edge.WindCnt = edge.WindDelta;
					}
					edge.WindCnt2 = tedge.WindCnt2;
					tedge = tedge.NextInAEL;
				}
				else
				{
					if (tedge.WindCnt * tedge.WindDelta < 0)
					{
						if (Math.Abs(tedge.WindCnt) > 1)
						{
							if (tedge.WindDelta * edge.WindDelta < 0)
							{
								edge.WindCnt = tedge.WindCnt;
							}
							else
							{
								edge.WindCnt = tedge.WindCnt + edge.WindDelta;
							}
						}
						else
						{
							edge.WindCnt = ((edge.WindDelta == 0) ? 1 : edge.WindDelta);
						}
					}
					else if (edge.WindDelta == 0)
					{
						edge.WindCnt = ((tedge.WindCnt < 0) ? (tedge.WindCnt - 1) : (tedge.WindCnt + 1));
					}
					else if (tedge.WindDelta * edge.WindDelta < 0)
					{
						edge.WindCnt = tedge.WindCnt;
					}
					else
					{
						edge.WindCnt = tedge.WindCnt + edge.WindDelta;
					}
					edge.WindCnt2 = tedge.WindCnt2;
					tedge = tedge.NextInAEL;
				}
				if (this.IsEvenOddAltFillType(edge))
				{
					while (tedge != edge)
					{
						if (tedge.WindDelta != 0)
						{
							edge.WindCnt2 = ((edge.WindCnt2 == 0) ? 1 : 0);
						}
						tedge = tedge.NextInAEL;
					}
					return;
				}
				while (tedge != edge)
				{
					edge.WindCnt2 += tedge.WindDelta;
					tedge = tedge.NextInAEL;
				}
			}

			// Token: 0x0600050A RID: 1290 RVA: 0x0001D9C0 File Offset: 0x0001BBC0
			private void AddEdgeToSEL(ClipperLib.TEdge edge)
			{
				if (this.m_SortedEdges == null)
				{
					this.m_SortedEdges = edge;
					edge.PrevInSEL = null;
					edge.NextInSEL = null;
					return;
				}
				edge.NextInSEL = this.m_SortedEdges;
				edge.PrevInSEL = null;
				this.m_SortedEdges.PrevInSEL = edge;
				this.m_SortedEdges = edge;
			}

			// Token: 0x0600050B RID: 1291 RVA: 0x0001DA14 File Offset: 0x0001BC14
			internal bool PopEdgeFromSEL(out ClipperLib.TEdge e)
			{
				e = this.m_SortedEdges;
				if (e == null)
				{
					return false;
				}
				ClipperLib.TEdge tedge = e;
				this.m_SortedEdges = e.NextInSEL;
				if (this.m_SortedEdges != null)
				{
					this.m_SortedEdges.PrevInSEL = null;
				}
				tedge.NextInSEL = null;
				tedge.PrevInSEL = null;
				return true;
			}

			// Token: 0x0600050C RID: 1292 RVA: 0x0001DA60 File Offset: 0x0001BC60
			private void CopyAELToSEL()
			{
				ClipperLib.TEdge tedge = this.m_ActiveEdges;
				this.m_SortedEdges = tedge;
				while (tedge != null)
				{
					tedge.PrevInSEL = tedge.PrevInAEL;
					tedge.NextInSEL = tedge.NextInAEL;
					tedge = tedge.NextInAEL;
				}
			}

			// Token: 0x0600050D RID: 1293 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
			private void SwapPositionsInSEL(ClipperLib.TEdge edge1, ClipperLib.TEdge edge2)
			{
				if (edge1.NextInSEL == null && edge1.PrevInSEL == null)
				{
					return;
				}
				if (edge2.NextInSEL == null && edge2.PrevInSEL == null)
				{
					return;
				}
				if (edge1.NextInSEL == edge2)
				{
					ClipperLib.TEdge nextInSEL = edge2.NextInSEL;
					if (nextInSEL != null)
					{
						nextInSEL.PrevInSEL = edge1;
					}
					ClipperLib.TEdge prevInSEL = edge1.PrevInSEL;
					if (prevInSEL != null)
					{
						prevInSEL.NextInSEL = edge2;
					}
					edge2.PrevInSEL = prevInSEL;
					edge2.NextInSEL = edge1;
					edge1.PrevInSEL = edge2;
					edge1.NextInSEL = nextInSEL;
				}
				else if (edge2.NextInSEL == edge1)
				{
					ClipperLib.TEdge nextInSEL2 = edge1.NextInSEL;
					if (nextInSEL2 != null)
					{
						nextInSEL2.PrevInSEL = edge2;
					}
					ClipperLib.TEdge prevInSEL2 = edge2.PrevInSEL;
					if (prevInSEL2 != null)
					{
						prevInSEL2.NextInSEL = edge1;
					}
					edge1.PrevInSEL = prevInSEL2;
					edge1.NextInSEL = edge2;
					edge2.PrevInSEL = edge1;
					edge2.NextInSEL = nextInSEL2;
				}
				else
				{
					ClipperLib.TEdge nextInSEL3 = edge1.NextInSEL;
					ClipperLib.TEdge prevInSEL3 = edge1.PrevInSEL;
					edge1.NextInSEL = edge2.NextInSEL;
					if (edge1.NextInSEL != null)
					{
						edge1.NextInSEL.PrevInSEL = edge1;
					}
					edge1.PrevInSEL = edge2.PrevInSEL;
					if (edge1.PrevInSEL != null)
					{
						edge1.PrevInSEL.NextInSEL = edge1;
					}
					edge2.NextInSEL = nextInSEL3;
					if (edge2.NextInSEL != null)
					{
						edge2.NextInSEL.PrevInSEL = edge2;
					}
					edge2.PrevInSEL = prevInSEL3;
					if (edge2.PrevInSEL != null)
					{
						edge2.PrevInSEL.NextInSEL = edge2;
					}
				}
				if (edge1.PrevInSEL == null)
				{
					this.m_SortedEdges = edge1;
					return;
				}
				if (edge2.PrevInSEL == null)
				{
					this.m_SortedEdges = edge2;
				}
			}

			// Token: 0x0600050E RID: 1294 RVA: 0x0001DC10 File Offset: 0x0001BE10
			private void AddLocalMaxPoly(ClipperLib.TEdge e1, ClipperLib.TEdge e2, ClipperLib.IntPoint pt)
			{
				this.AddOutPt(e1, pt);
				if (e2.WindDelta == 0)
				{
					this.AddOutPt(e2, pt);
				}
				if (e1.OutIdx == e2.OutIdx)
				{
					e1.OutIdx = -1;
					e2.OutIdx = -1;
					return;
				}
				if (e1.OutIdx < e2.OutIdx)
				{
					this.AppendPolygon(e1, e2);
					return;
				}
				this.AppendPolygon(e2, e1);
			}

			// Token: 0x0600050F RID: 1295 RVA: 0x0001DC74 File Offset: 0x0001BE74
			private ClipperLib.OutPt AddLocalMinPoly(ClipperLib.TEdge e1, ClipperLib.TEdge e2, ClipperLib.IntPoint pt)
			{
				ClipperLib.OutPt outPt;
				ClipperLib.TEdge tedge;
				ClipperLib.TEdge tedge2;
				if (ClipperLib.ClipperBase.IsHorizontal(e2) || e1.Dx > e2.Dx)
				{
					outPt = this.AddOutPt(e1, pt);
					e2.OutIdx = e1.OutIdx;
					e1.Side = ClipperLib.EdgeSide.esLeft;
					e2.Side = ClipperLib.EdgeSide.esRight;
					tedge = e1;
					if (tedge.PrevInAEL == e2)
					{
						tedge2 = e2.PrevInAEL;
					}
					else
					{
						tedge2 = tedge.PrevInAEL;
					}
				}
				else
				{
					outPt = this.AddOutPt(e2, pt);
					e1.OutIdx = e2.OutIdx;
					e1.Side = ClipperLib.EdgeSide.esRight;
					e2.Side = ClipperLib.EdgeSide.esLeft;
					tedge = e2;
					if (tedge.PrevInAEL == e1)
					{
						tedge2 = e1.PrevInAEL;
					}
					else
					{
						tedge2 = tedge.PrevInAEL;
					}
				}
				if (tedge2 != null && tedge2.OutIdx >= 0 && tedge2.Top.Y < pt.Y && tedge.Top.Y < pt.Y)
				{
					long num = ClipperLib.Clipper.TopX(tedge2, pt.Y);
					long num2 = ClipperLib.Clipper.TopX(tedge, pt.Y);
					if (num == num2 && tedge.WindDelta != 0 && tedge2.WindDelta != 0 && ClipperLib.ClipperBase.SlopesEqual(new ClipperLib.IntPoint(num, pt.Y), tedge2.Top, new ClipperLib.IntPoint(num2, pt.Y), tedge.Top, this.m_UseFullRange))
					{
						ClipperLib.OutPt outPt2 = this.AddOutPt(tedge2, pt);
						this.AddJoin(outPt, outPt2, tedge.Top);
					}
				}
				return outPt;
			}

			// Token: 0x06000510 RID: 1296 RVA: 0x0001DDCC File Offset: 0x0001BFCC
			private ClipperLib.OutPt AddOutPt(ClipperLib.TEdge e, ClipperLib.IntPoint pt)
			{
				if (e.OutIdx < 0)
				{
					ClipperLib.OutRec outRec = base.CreateOutRec();
					outRec.IsOpen = e.WindDelta == 0;
					ClipperLib.OutPt outPt = new ClipperLib.OutPt();
					outRec.Pts = outPt;
					outPt.Idx = outRec.Idx;
					outPt.Pt = pt;
					outPt.Next = outPt;
					outPt.Prev = outPt;
					if (!outRec.IsOpen)
					{
						this.SetHoleState(e, outRec);
					}
					e.OutIdx = outRec.Idx;
					return outPt;
				}
				ClipperLib.OutRec outRec2 = this.m_PolyOuts[e.OutIdx];
				ClipperLib.OutPt pts = outRec2.Pts;
				bool flag = e.Side == ClipperLib.EdgeSide.esLeft;
				if (flag && pt == pts.Pt)
				{
					return pts;
				}
				if (!flag && pt == pts.Prev.Pt)
				{
					return pts.Prev;
				}
				ClipperLib.OutPt outPt2 = new ClipperLib.OutPt();
				outPt2.Idx = outRec2.Idx;
				outPt2.Pt = pt;
				outPt2.Next = pts;
				outPt2.Prev = pts.Prev;
				outPt2.Prev.Next = outPt2;
				pts.Prev = outPt2;
				if (flag)
				{
					outRec2.Pts = outPt2;
				}
				return outPt2;
			}

			// Token: 0x06000511 RID: 1297 RVA: 0x0001DEF0 File Offset: 0x0001C0F0
			private ClipperLib.OutPt GetLastOutPt(ClipperLib.TEdge e)
			{
				ClipperLib.OutRec outRec = this.m_PolyOuts[e.OutIdx];
				if (e.Side == ClipperLib.EdgeSide.esLeft)
				{
					return outRec.Pts;
				}
				return outRec.Pts.Prev;
			}

			// Token: 0x06000512 RID: 1298 RVA: 0x0001DF2C File Offset: 0x0001C12C
			internal void SwapPoints(ref ClipperLib.IntPoint pt1, ref ClipperLib.IntPoint pt2)
			{
				ClipperLib.IntPoint intPoint = new ClipperLib.IntPoint(pt1);
				pt1 = pt2;
				pt2 = intPoint;
			}

			// Token: 0x06000513 RID: 1299 RVA: 0x0001DF59 File Offset: 0x0001C159
			private bool HorzSegmentsOverlap(long seg1a, long seg1b, long seg2a, long seg2b)
			{
				if (seg1a > seg1b)
				{
					base.Swap(ref seg1a, ref seg1b);
				}
				if (seg2a > seg2b)
				{
					base.Swap(ref seg2a, ref seg2b);
				}
				return seg1a < seg2b && seg2a < seg1b;
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x0001DF84 File Offset: 0x0001C184
			private void SetHoleState(ClipperLib.TEdge e, ClipperLib.OutRec outRec)
			{
				ClipperLib.TEdge tedge = e.PrevInAEL;
				ClipperLib.TEdge tedge2 = null;
				while (tedge != null)
				{
					if (tedge.OutIdx >= 0 && tedge.WindDelta != 0)
					{
						if (tedge2 == null)
						{
							tedge2 = tedge;
						}
						else if (tedge2.OutIdx == tedge.OutIdx)
						{
							tedge2 = null;
						}
					}
					tedge = tedge.PrevInAEL;
				}
				if (tedge2 == null)
				{
					outRec.FirstLeft = null;
					outRec.IsHole = false;
					return;
				}
				outRec.FirstLeft = this.m_PolyOuts[tedge2.OutIdx];
				outRec.IsHole = !outRec.FirstLeft.IsHole;
			}

			// Token: 0x06000515 RID: 1301 RVA: 0x0001E00B File Offset: 0x0001C20B
			private double GetDx(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2)
			{
				if (pt1.Y == pt2.Y)
				{
					return -3.4E+38;
				}
				return (double)(pt2.X - pt1.X) / (double)(pt2.Y - pt1.Y);
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x0001E044 File Offset: 0x0001C244
			private bool FirstIsBottomPt(ClipperLib.OutPt btmPt1, ClipperLib.OutPt btmPt2)
			{
				ClipperLib.OutPt outPt = btmPt1.Prev;
				while (outPt.Pt == btmPt1.Pt && outPt != btmPt1)
				{
					outPt = outPt.Prev;
				}
				double num = Math.Abs(this.GetDx(btmPt1.Pt, outPt.Pt));
				outPt = btmPt1.Next;
				while (outPt.Pt == btmPt1.Pt && outPt != btmPt1)
				{
					outPt = outPt.Next;
				}
				double num2 = Math.Abs(this.GetDx(btmPt1.Pt, outPt.Pt));
				outPt = btmPt2.Prev;
				while (outPt.Pt == btmPt2.Pt && outPt != btmPt2)
				{
					outPt = outPt.Prev;
				}
				double num3 = Math.Abs(this.GetDx(btmPt2.Pt, outPt.Pt));
				outPt = btmPt2.Next;
				while (outPt.Pt == btmPt2.Pt && outPt != btmPt2)
				{
					outPt = outPt.Next;
				}
				double num4 = Math.Abs(this.GetDx(btmPt2.Pt, outPt.Pt));
				if (Math.Max(num, num2) == Math.Max(num3, num4) && Math.Min(num, num2) == Math.Min(num3, num4))
				{
					return this.Area(btmPt1) > 0.0;
				}
				return (num >= num3 && num >= num4) || (num2 >= num3 && num2 >= num4);
			}

			// Token: 0x06000517 RID: 1303 RVA: 0x0001E19C File Offset: 0x0001C39C
			private ClipperLib.OutPt GetBottomPt(ClipperLib.OutPt pp)
			{
				ClipperLib.OutPt outPt = null;
				ClipperLib.OutPt outPt2;
				for (outPt2 = pp.Next; outPt2 != pp; outPt2 = outPt2.Next)
				{
					if (outPt2.Pt.Y > pp.Pt.Y)
					{
						pp = outPt2;
						outPt = null;
					}
					else if (outPt2.Pt.Y == pp.Pt.Y && outPt2.Pt.X <= pp.Pt.X)
					{
						if (outPt2.Pt.X < pp.Pt.X)
						{
							outPt = null;
							pp = outPt2;
						}
						else if (outPt2.Next != pp && outPt2.Prev != pp)
						{
							outPt = outPt2;
						}
					}
				}
				if (outPt != null)
				{
					while (outPt != outPt2)
					{
						if (!this.FirstIsBottomPt(outPt2, outPt))
						{
							pp = outPt;
						}
						outPt = outPt.Next;
						while (outPt.Pt != pp.Pt)
						{
							outPt = outPt.Next;
						}
					}
				}
				return pp;
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x0001E284 File Offset: 0x0001C484
			private ClipperLib.OutRec GetLowermostRec(ClipperLib.OutRec outRec1, ClipperLib.OutRec outRec2)
			{
				if (outRec1.BottomPt == null)
				{
					outRec1.BottomPt = this.GetBottomPt(outRec1.Pts);
				}
				if (outRec2.BottomPt == null)
				{
					outRec2.BottomPt = this.GetBottomPt(outRec2.Pts);
				}
				ClipperLib.OutPt bottomPt = outRec1.BottomPt;
				ClipperLib.OutPt bottomPt2 = outRec2.BottomPt;
				if (bottomPt.Pt.Y > bottomPt2.Pt.Y)
				{
					return outRec1;
				}
				if (bottomPt.Pt.Y < bottomPt2.Pt.Y)
				{
					return outRec2;
				}
				if (bottomPt.Pt.X < bottomPt2.Pt.X)
				{
					return outRec1;
				}
				if (bottomPt.Pt.X > bottomPt2.Pt.X)
				{
					return outRec2;
				}
				if (bottomPt.Next == bottomPt)
				{
					return outRec2;
				}
				if (bottomPt2.Next == bottomPt2)
				{
					return outRec1;
				}
				if (this.FirstIsBottomPt(bottomPt, bottomPt2))
				{
					return outRec1;
				}
				return outRec2;
			}

			// Token: 0x06000519 RID: 1305 RVA: 0x0001E35E File Offset: 0x0001C55E
			private bool OutRec1RightOfOutRec2(ClipperLib.OutRec outRec1, ClipperLib.OutRec outRec2)
			{
				for (;;)
				{
					outRec1 = outRec1.FirstLeft;
					if (outRec1 == outRec2)
					{
						break;
					}
					if (outRec1 == null)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0600051A RID: 1306 RVA: 0x0001E374 File Offset: 0x0001C574
			private ClipperLib.OutRec GetOutRec(int idx)
			{
				ClipperLib.OutRec outRec;
				for (outRec = this.m_PolyOuts[idx]; outRec != this.m_PolyOuts[outRec.Idx]; outRec = this.m_PolyOuts[outRec.Idx])
				{
				}
				return outRec;
			}

			// Token: 0x0600051B RID: 1307 RVA: 0x0001E3B8 File Offset: 0x0001C5B8
			private void AppendPolygon(ClipperLib.TEdge e1, ClipperLib.TEdge e2)
			{
				ClipperLib.OutRec outRec = this.m_PolyOuts[e1.OutIdx];
				ClipperLib.OutRec outRec2 = this.m_PolyOuts[e2.OutIdx];
				ClipperLib.OutRec outRec3;
				if (this.OutRec1RightOfOutRec2(outRec, outRec2))
				{
					outRec3 = outRec2;
				}
				else if (this.OutRec1RightOfOutRec2(outRec2, outRec))
				{
					outRec3 = outRec;
				}
				else
				{
					outRec3 = this.GetLowermostRec(outRec, outRec2);
				}
				ClipperLib.OutPt pts = outRec.Pts;
				ClipperLib.OutPt prev = pts.Prev;
				ClipperLib.OutPt pts2 = outRec2.Pts;
				ClipperLib.OutPt prev2 = pts2.Prev;
				if (e1.Side == ClipperLib.EdgeSide.esLeft)
				{
					if (e2.Side == ClipperLib.EdgeSide.esLeft)
					{
						this.ReversePolyPtLinks(pts2);
						pts2.Next = pts;
						pts.Prev = pts2;
						prev.Next = prev2;
						prev2.Prev = prev;
						outRec.Pts = prev2;
					}
					else
					{
						prev2.Next = pts;
						pts.Prev = prev2;
						pts2.Prev = prev;
						prev.Next = pts2;
						outRec.Pts = pts2;
					}
				}
				else if (e2.Side == ClipperLib.EdgeSide.esRight)
				{
					this.ReversePolyPtLinks(pts2);
					prev.Next = prev2;
					prev2.Prev = prev;
					pts2.Next = pts;
					pts.Prev = pts2;
				}
				else
				{
					prev.Next = pts2;
					pts2.Prev = prev;
					pts.Prev = prev2;
					prev2.Next = pts;
				}
				outRec.BottomPt = null;
				if (outRec3 == outRec2)
				{
					if (outRec2.FirstLeft != outRec)
					{
						outRec.FirstLeft = outRec2.FirstLeft;
					}
					outRec.IsHole = outRec2.IsHole;
				}
				outRec2.Pts = null;
				outRec2.BottomPt = null;
				outRec2.FirstLeft = outRec;
				int outIdx = e1.OutIdx;
				int outIdx2 = e2.OutIdx;
				e1.OutIdx = -1;
				e2.OutIdx = -1;
				for (ClipperLib.TEdge tedge = this.m_ActiveEdges; tedge != null; tedge = tedge.NextInAEL)
				{
					if (tedge.OutIdx == outIdx2)
					{
						tedge.OutIdx = outIdx;
						tedge.Side = e1.Side;
						break;
					}
				}
				outRec2.Idx = outRec.Idx;
			}

			// Token: 0x0600051C RID: 1308 RVA: 0x0001E5A0 File Offset: 0x0001C7A0
			private void ReversePolyPtLinks(ClipperLib.OutPt pp)
			{
				if (pp == null)
				{
					return;
				}
				ClipperLib.OutPt outPt = pp;
				do
				{
					ClipperLib.OutPt next = outPt.Next;
					outPt.Next = outPt.Prev;
					outPt.Prev = next;
					outPt = next;
				}
				while (outPt != pp);
			}

			// Token: 0x0600051D RID: 1309 RVA: 0x0001E5D4 File Offset: 0x0001C7D4
			private static void SwapSides(ClipperLib.TEdge edge1, ClipperLib.TEdge edge2)
			{
				ClipperLib.EdgeSide side = edge1.Side;
				edge1.Side = edge2.Side;
				edge2.Side = side;
			}

			// Token: 0x0600051E RID: 1310 RVA: 0x0001E5FC File Offset: 0x0001C7FC
			private static void SwapPolyIndexes(ClipperLib.TEdge edge1, ClipperLib.TEdge edge2)
			{
				int outIdx = edge1.OutIdx;
				edge1.OutIdx = edge2.OutIdx;
				edge2.OutIdx = outIdx;
			}

			// Token: 0x0600051F RID: 1311 RVA: 0x0001E624 File Offset: 0x0001C824
			private void IntersectEdges(ClipperLib.TEdge e1, ClipperLib.TEdge e2, ClipperLib.IntPoint pt)
			{
				bool flag = e1.OutIdx >= 0;
				bool flag2 = e2.OutIdx >= 0;
				if (e1.WindDelta == 0 || e2.WindDelta == 0)
				{
					if (e1.WindDelta == 0 && e2.WindDelta == 0)
					{
						return;
					}
					if (e1.PolyTyp == e2.PolyTyp && e1.WindDelta != e2.WindDelta && this.m_ClipType == ClipperLib.ClipType.ctUnion)
					{
						if (e1.WindDelta == 0)
						{
							if (flag2)
							{
								this.AddOutPt(e1, pt);
								if (flag)
								{
									e1.OutIdx = -1;
									return;
								}
							}
						}
						else if (flag)
						{
							this.AddOutPt(e2, pt);
							if (flag2)
							{
								e2.OutIdx = -1;
								return;
							}
						}
					}
					else if (e1.PolyTyp != e2.PolyTyp)
					{
						if (e1.WindDelta == 0 && Math.Abs(e2.WindCnt) == 1 && (this.m_ClipType != ClipperLib.ClipType.ctUnion || e2.WindCnt2 == 0))
						{
							this.AddOutPt(e1, pt);
							if (flag)
							{
								e1.OutIdx = -1;
								return;
							}
						}
						else if (e2.WindDelta == 0 && Math.Abs(e1.WindCnt) == 1 && (this.m_ClipType != ClipperLib.ClipType.ctUnion || e1.WindCnt2 == 0))
						{
							this.AddOutPt(e2, pt);
							if (flag2)
							{
								e2.OutIdx = -1;
							}
						}
					}
					return;
				}
				else
				{
					if (e1.PolyTyp == e2.PolyTyp)
					{
						if (this.IsEvenOddFillType(e1))
						{
							int windCnt = e1.WindCnt;
							e1.WindCnt = e2.WindCnt;
							e2.WindCnt = windCnt;
						}
						else
						{
							if (e1.WindCnt + e2.WindDelta == 0)
							{
								e1.WindCnt = -e1.WindCnt;
							}
							else
							{
								e1.WindCnt += e2.WindDelta;
							}
							if (e2.WindCnt - e1.WindDelta == 0)
							{
								e2.WindCnt = -e2.WindCnt;
							}
							else
							{
								e2.WindCnt -= e1.WindDelta;
							}
						}
					}
					else
					{
						if (!this.IsEvenOddFillType(e2))
						{
							e1.WindCnt2 += e2.WindDelta;
						}
						else
						{
							e1.WindCnt2 = ((e1.WindCnt2 == 0) ? 1 : 0);
						}
						if (!this.IsEvenOddFillType(e1))
						{
							e2.WindCnt2 -= e1.WindDelta;
						}
						else
						{
							e2.WindCnt2 = ((e2.WindCnt2 == 0) ? 1 : 0);
						}
					}
					ClipperLib.PolyFillType polyFillType;
					ClipperLib.PolyFillType polyFillType2;
					if (e1.PolyTyp == ClipperLib.PolyType.ptSubject)
					{
						polyFillType = this.m_SubjFillType;
						polyFillType2 = this.m_ClipFillType;
					}
					else
					{
						polyFillType = this.m_ClipFillType;
						polyFillType2 = this.m_SubjFillType;
					}
					ClipperLib.PolyFillType polyFillType3;
					ClipperLib.PolyFillType polyFillType4;
					if (e2.PolyTyp == ClipperLib.PolyType.ptSubject)
					{
						polyFillType3 = this.m_SubjFillType;
						polyFillType4 = this.m_ClipFillType;
					}
					else
					{
						polyFillType3 = this.m_ClipFillType;
						polyFillType4 = this.m_SubjFillType;
					}
					int num;
					if (polyFillType != ClipperLib.PolyFillType.pftPositive)
					{
						if (polyFillType != ClipperLib.PolyFillType.pftNegative)
						{
							num = Math.Abs(e1.WindCnt);
						}
						else
						{
							num = -e1.WindCnt;
						}
					}
					else
					{
						num = e1.WindCnt;
					}
					int num2;
					if (polyFillType3 != ClipperLib.PolyFillType.pftPositive)
					{
						if (polyFillType3 != ClipperLib.PolyFillType.pftNegative)
						{
							num2 = Math.Abs(e2.WindCnt);
						}
						else
						{
							num2 = -e2.WindCnt;
						}
					}
					else
					{
						num2 = e2.WindCnt;
					}
					if (!flag || !flag2)
					{
						if (flag)
						{
							if (num2 == 0 || num2 == 1)
							{
								this.AddOutPt(e1, pt);
								ClipperLib.Clipper.SwapSides(e1, e2);
								ClipperLib.Clipper.SwapPolyIndexes(e1, e2);
								return;
							}
						}
						else if (flag2)
						{
							if (num == 0 || num == 1)
							{
								this.AddOutPt(e2, pt);
								ClipperLib.Clipper.SwapSides(e1, e2);
								ClipperLib.Clipper.SwapPolyIndexes(e1, e2);
								return;
							}
						}
						else if ((num == 0 || num == 1) && (num2 == 0 || num2 == 1))
						{
							long num3;
							if (polyFillType2 != ClipperLib.PolyFillType.pftPositive)
							{
								if (polyFillType2 != ClipperLib.PolyFillType.pftNegative)
								{
									num3 = (long)Math.Abs(e1.WindCnt2);
								}
								else
								{
									num3 = (long)(-(long)e1.WindCnt2);
								}
							}
							else
							{
								num3 = (long)e1.WindCnt2;
							}
							long num4;
							if (polyFillType4 != ClipperLib.PolyFillType.pftPositive)
							{
								if (polyFillType4 != ClipperLib.PolyFillType.pftNegative)
								{
									num4 = (long)Math.Abs(e2.WindCnt2);
								}
								else
								{
									num4 = (long)(-(long)e2.WindCnt2);
								}
							}
							else
							{
								num4 = (long)e2.WindCnt2;
							}
							if (e1.PolyTyp != e2.PolyTyp)
							{
								this.AddLocalMinPoly(e1, e2, pt);
								return;
							}
							if (num == 1 && num2 == 1)
							{
								switch (this.m_ClipType)
								{
								case ClipperLib.ClipType.ctIntersection:
									if (num3 > 0L && num4 > 0L)
									{
										this.AddLocalMinPoly(e1, e2, pt);
										return;
									}
									break;
								case ClipperLib.ClipType.ctUnion:
									if (num3 <= 0L && num4 <= 0L)
									{
										this.AddLocalMinPoly(e1, e2, pt);
										return;
									}
									break;
								case ClipperLib.ClipType.ctDifference:
									if ((e1.PolyTyp == ClipperLib.PolyType.ptClip && num3 > 0L && num4 > 0L) || (e1.PolyTyp == ClipperLib.PolyType.ptSubject && num3 <= 0L && num4 <= 0L))
									{
										this.AddLocalMinPoly(e1, e2, pt);
										return;
									}
									break;
								case ClipperLib.ClipType.ctXor:
									this.AddLocalMinPoly(e1, e2, pt);
									return;
								default:
									return;
								}
							}
							else
							{
								ClipperLib.Clipper.SwapSides(e1, e2);
							}
						}
						return;
					}
					if ((num != 0 && num != 1) || (num2 != 0 && num2 != 1) || (e1.PolyTyp != e2.PolyTyp && this.m_ClipType != ClipperLib.ClipType.ctXor))
					{
						this.AddLocalMaxPoly(e1, e2, pt);
						return;
					}
					this.AddOutPt(e1, pt);
					this.AddOutPt(e2, pt);
					ClipperLib.Clipper.SwapSides(e1, e2);
					ClipperLib.Clipper.SwapPolyIndexes(e1, e2);
					return;
				}
			}

			// Token: 0x06000520 RID: 1312 RVA: 0x0001EAE0 File Offset: 0x0001CCE0
			private void DeleteFromSEL(ClipperLib.TEdge e)
			{
				ClipperLib.TEdge prevInSEL = e.PrevInSEL;
				ClipperLib.TEdge nextInSEL = e.NextInSEL;
				if (prevInSEL == null && nextInSEL == null && e != this.m_SortedEdges)
				{
					return;
				}
				if (prevInSEL != null)
				{
					prevInSEL.NextInSEL = nextInSEL;
				}
				else
				{
					this.m_SortedEdges = nextInSEL;
				}
				if (nextInSEL != null)
				{
					nextInSEL.PrevInSEL = prevInSEL;
				}
				e.NextInSEL = null;
				e.PrevInSEL = null;
			}

			// Token: 0x06000521 RID: 1313 RVA: 0x0001EB38 File Offset: 0x0001CD38
			private void ProcessHorizontals()
			{
				ClipperLib.TEdge tedge;
				while (this.PopEdgeFromSEL(out tedge))
				{
					this.ProcessHorizontal(tedge);
				}
			}

			// Token: 0x06000522 RID: 1314 RVA: 0x0001EB58 File Offset: 0x0001CD58
			private void GetHorzDirection(ClipperLib.TEdge HorzEdge, out ClipperLib.Direction Dir, out long Left, out long Right)
			{
				if (HorzEdge.Bot.X < HorzEdge.Top.X)
				{
					Left = HorzEdge.Bot.X;
					Right = HorzEdge.Top.X;
					Dir = ClipperLib.Direction.dLeftToRight;
					return;
				}
				Left = HorzEdge.Top.X;
				Right = HorzEdge.Bot.X;
				Dir = ClipperLib.Direction.dRightToLeft;
			}

			// Token: 0x06000523 RID: 1315 RVA: 0x0001EBBC File Offset: 0x0001CDBC
			private void ProcessHorizontal(ClipperLib.TEdge horzEdge)
			{
				bool flag = horzEdge.WindDelta == 0;
				ClipperLib.Direction direction;
				long num;
				long num2;
				this.GetHorzDirection(horzEdge, out direction, out num, out num2);
				ClipperLib.TEdge tedge = horzEdge;
				ClipperLib.TEdge tedge2 = null;
				while (tedge.NextInLML != null && ClipperLib.ClipperBase.IsHorizontal(tedge.NextInLML))
				{
					tedge = tedge.NextInLML;
				}
				if (tedge.NextInLML == null)
				{
					tedge2 = this.GetMaximaPair(tedge);
				}
				ClipperLib.Maxima maxima = this.m_Maxima;
				if (maxima != null)
				{
					if (direction == ClipperLib.Direction.dLeftToRight)
					{
						while (maxima != null && maxima.X <= horzEdge.Bot.X)
						{
							maxima = maxima.Next;
						}
						if (maxima != null && maxima.X >= tedge.Top.X)
						{
							maxima = null;
						}
					}
					else
					{
						while (maxima.Next != null && maxima.Next.X < horzEdge.Bot.X)
						{
							maxima = maxima.Next;
						}
						if (maxima.X <= tedge.Top.X)
						{
							maxima = null;
						}
					}
				}
				ClipperLib.OutPt outPt = null;
				for (;;)
				{
					bool flag2 = horzEdge == tedge;
					ClipperLib.TEdge nextInAEL;
					for (ClipperLib.TEdge tedge3 = this.GetNextInAEL(horzEdge, direction); tedge3 != null; tedge3 = nextInAEL)
					{
						if (maxima != null)
						{
							if (direction == ClipperLib.Direction.dLeftToRight)
							{
								while (maxima != null)
								{
									if (maxima.X >= tedge3.Curr.X)
									{
										break;
									}
									if (horzEdge.OutIdx >= 0 && !flag)
									{
										this.AddOutPt(horzEdge, new ClipperLib.IntPoint(maxima.X, horzEdge.Bot.Y));
									}
									maxima = maxima.Next;
								}
							}
							else
							{
								while (maxima != null && maxima.X > tedge3.Curr.X)
								{
									if (horzEdge.OutIdx >= 0 && !flag)
									{
										this.AddOutPt(horzEdge, new ClipperLib.IntPoint(maxima.X, horzEdge.Bot.Y));
									}
									maxima = maxima.Prev;
								}
							}
						}
						if ((direction == ClipperLib.Direction.dLeftToRight && tedge3.Curr.X > num2) || (direction == ClipperLib.Direction.dRightToLeft && tedge3.Curr.X < num) || (tedge3.Curr.X == horzEdge.Top.X && horzEdge.NextInLML != null && tedge3.Dx < horzEdge.NextInLML.Dx))
						{
							break;
						}
						if (horzEdge.OutIdx >= 0 && !flag)
						{
							outPt = this.AddOutPt(horzEdge, tedge3.Curr);
							for (ClipperLib.TEdge tedge4 = this.m_SortedEdges; tedge4 != null; tedge4 = tedge4.NextInSEL)
							{
								if (tedge4.OutIdx >= 0 && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, tedge4.Bot.X, tedge4.Top.X))
								{
									ClipperLib.OutPt lastOutPt = this.GetLastOutPt(tedge4);
									this.AddJoin(lastOutPt, outPt, tedge4.Top);
								}
							}
							this.AddGhostJoin(outPt, horzEdge.Bot);
						}
						if (tedge3 == tedge2 && flag2)
						{
							goto Block_28;
						}
						if (direction == ClipperLib.Direction.dLeftToRight)
						{
							ClipperLib.IntPoint intPoint = new ClipperLib.IntPoint(tedge3.Curr.X, horzEdge.Curr.Y);
							this.IntersectEdges(horzEdge, tedge3, intPoint);
						}
						else
						{
							ClipperLib.IntPoint intPoint2 = new ClipperLib.IntPoint(tedge3.Curr.X, horzEdge.Curr.Y);
							this.IntersectEdges(tedge3, horzEdge, intPoint2);
						}
						nextInAEL = this.GetNextInAEL(tedge3, direction);
						base.SwapPositionsInAEL(horzEdge, tedge3);
					}
					if (horzEdge.NextInLML == null || !ClipperLib.ClipperBase.IsHorizontal(horzEdge.NextInLML))
					{
						goto IL_039F;
					}
					base.UpdateEdgeIntoAEL(ref horzEdge);
					if (horzEdge.OutIdx >= 0)
					{
						this.AddOutPt(horzEdge, horzEdge.Bot);
					}
					this.GetHorzDirection(horzEdge, out direction, out num, out num2);
				}
				Block_28:
				if (horzEdge.OutIdx >= 0)
				{
					this.AddLocalMaxPoly(horzEdge, tedge2, horzEdge.Top);
				}
				base.DeleteFromAEL(horzEdge);
				base.DeleteFromAEL(tedge2);
				return;
				IL_039F:
				if (horzEdge.OutIdx >= 0 && outPt == null)
				{
					outPt = this.GetLastOutPt(horzEdge);
					for (ClipperLib.TEdge tedge5 = this.m_SortedEdges; tedge5 != null; tedge5 = tedge5.NextInSEL)
					{
						if (tedge5.OutIdx >= 0 && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, tedge5.Bot.X, tedge5.Top.X))
						{
							ClipperLib.OutPt lastOutPt2 = this.GetLastOutPt(tedge5);
							this.AddJoin(lastOutPt2, outPt, tedge5.Top);
						}
					}
					this.AddGhostJoin(outPt, horzEdge.Top);
				}
				if (horzEdge.NextInLML != null)
				{
					if (horzEdge.OutIdx < 0)
					{
						base.UpdateEdgeIntoAEL(ref horzEdge);
						return;
					}
					outPt = this.AddOutPt(horzEdge, horzEdge.Top);
					base.UpdateEdgeIntoAEL(ref horzEdge);
					if (horzEdge.WindDelta == 0)
					{
						return;
					}
					ClipperLib.TEdge prevInAEL = horzEdge.PrevInAEL;
					ClipperLib.TEdge nextInAEL2 = horzEdge.NextInAEL;
					if (prevInAEL != null && prevInAEL.Curr.X == horzEdge.Bot.X && prevInAEL.Curr.Y == horzEdge.Bot.Y && prevInAEL.WindDelta != 0 && prevInAEL.OutIdx >= 0 && prevInAEL.Curr.Y > prevInAEL.Top.Y && ClipperLib.ClipperBase.SlopesEqual(horzEdge, prevInAEL, this.m_UseFullRange))
					{
						ClipperLib.OutPt outPt2 = this.AddOutPt(prevInAEL, horzEdge.Bot);
						this.AddJoin(outPt, outPt2, horzEdge.Top);
						return;
					}
					if (nextInAEL2 != null && nextInAEL2.Curr.X == horzEdge.Bot.X && nextInAEL2.Curr.Y == horzEdge.Bot.Y && nextInAEL2.WindDelta != 0 && nextInAEL2.OutIdx >= 0 && nextInAEL2.Curr.Y > nextInAEL2.Top.Y && ClipperLib.ClipperBase.SlopesEqual(horzEdge, nextInAEL2, this.m_UseFullRange))
					{
						ClipperLib.OutPt outPt3 = this.AddOutPt(nextInAEL2, horzEdge.Bot);
						this.AddJoin(outPt, outPt3, horzEdge.Top);
						return;
					}
				}
				else
				{
					if (horzEdge.OutIdx >= 0)
					{
						this.AddOutPt(horzEdge, horzEdge.Top);
					}
					base.DeleteFromAEL(horzEdge);
				}
			}

			// Token: 0x06000524 RID: 1316 RVA: 0x0001F1A6 File Offset: 0x0001D3A6
			private ClipperLib.TEdge GetNextInAEL(ClipperLib.TEdge e, ClipperLib.Direction Direction)
			{
				if (Direction != ClipperLib.Direction.dLeftToRight)
				{
					return e.PrevInAEL;
				}
				return e.NextInAEL;
			}

			// Token: 0x06000525 RID: 1317 RVA: 0x0001F1B9 File Offset: 0x0001D3B9
			private bool IsMinima(ClipperLib.TEdge e)
			{
				return e != null && e.Prev.NextInLML != e && e.Next.NextInLML != e;
			}

			// Token: 0x06000526 RID: 1318 RVA: 0x0001F1DF File Offset: 0x0001D3DF
			private bool IsMaxima(ClipperLib.TEdge e, double Y)
			{
				return e != null && (double)e.Top.Y == Y && e.NextInLML == null;
			}

			// Token: 0x06000527 RID: 1319 RVA: 0x0001F1FE File Offset: 0x0001D3FE
			private bool IsIntermediate(ClipperLib.TEdge e, double Y)
			{
				return (double)e.Top.Y == Y && e.NextInLML != null;
			}

			// Token: 0x06000528 RID: 1320 RVA: 0x0001F21C File Offset: 0x0001D41C
			internal ClipperLib.TEdge GetMaximaPair(ClipperLib.TEdge e)
			{
				if (e.Next.Top == e.Top && e.Next.NextInLML == null)
				{
					return e.Next;
				}
				if (e.Prev.Top == e.Top && e.Prev.NextInLML == null)
				{
					return e.Prev;
				}
				return null;
			}

			// Token: 0x06000529 RID: 1321 RVA: 0x0001F284 File Offset: 0x0001D484
			internal ClipperLib.TEdge GetMaximaPairEx(ClipperLib.TEdge e)
			{
				ClipperLib.TEdge maximaPair = this.GetMaximaPair(e);
				if (maximaPair == null || maximaPair.OutIdx == -2 || (maximaPair.NextInAEL == maximaPair.PrevInAEL && !ClipperLib.ClipperBase.IsHorizontal(maximaPair)))
				{
					return null;
				}
				return maximaPair;
			}

			// Token: 0x0600052A RID: 1322 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
			private bool ProcessIntersections(long topY)
			{
				if (this.m_ActiveEdges == null)
				{
					return true;
				}
				try
				{
					this.BuildIntersectList(topY);
					if (this.m_IntersectList.Count == 0)
					{
						return true;
					}
					if (this.m_IntersectList.Count != 1 && !this.FixupIntersectionOrder())
					{
						return false;
					}
					this.ProcessIntersectList();
				}
				catch
				{
					this.m_SortedEdges = null;
					this.m_IntersectList.Clear();
					throw new ClipperLib.ClipperException("ProcessIntersections error");
				}
				this.m_SortedEdges = null;
				return true;
			}

			// Token: 0x0600052B RID: 1323 RVA: 0x0001F34C File Offset: 0x0001D54C
			private void BuildIntersectList(long topY)
			{
				if (this.m_ActiveEdges == null)
				{
					return;
				}
				ClipperLib.TEdge tedge = this.m_ActiveEdges;
				this.m_SortedEdges = tedge;
				while (tedge != null)
				{
					tedge.PrevInSEL = tedge.PrevInAEL;
					tedge.NextInSEL = tedge.NextInAEL;
					tedge.Curr.X = ClipperLib.Clipper.TopX(tedge, topY);
					tedge = tedge.NextInAEL;
				}
				bool flag = true;
				while (flag && this.m_SortedEdges != null)
				{
					flag = false;
					tedge = this.m_SortedEdges;
					while (tedge.NextInSEL != null)
					{
						ClipperLib.TEdge nextInSEL = tedge.NextInSEL;
						if (tedge.Curr.X > nextInSEL.Curr.X)
						{
							ClipperLib.IntPoint intPoint;
							this.IntersectPoint(tedge, nextInSEL, out intPoint);
							if (intPoint.Y < topY)
							{
								intPoint = new ClipperLib.IntPoint(ClipperLib.Clipper.TopX(tedge, topY), topY);
							}
							ClipperLib.IntersectNode intersectNode = new ClipperLib.IntersectNode();
							intersectNode.Edge1 = tedge;
							intersectNode.Edge2 = nextInSEL;
							intersectNode.Pt = intPoint;
							this.m_IntersectList.Add(intersectNode);
							this.SwapPositionsInSEL(tedge, nextInSEL);
							flag = true;
						}
						else
						{
							tedge = nextInSEL;
						}
					}
					if (tedge.PrevInSEL == null)
					{
						break;
					}
					tedge.PrevInSEL.NextInSEL = null;
				}
				this.m_SortedEdges = null;
			}

			// Token: 0x0600052C RID: 1324 RVA: 0x0001F467 File Offset: 0x0001D667
			private bool EdgesAdjacent(ClipperLib.IntersectNode inode)
			{
				return inode.Edge1.NextInSEL == inode.Edge2 || inode.Edge1.PrevInSEL == inode.Edge2;
			}

			// Token: 0x0600052D RID: 1325 RVA: 0x0001F491 File Offset: 0x0001D691
			private static int IntersectNodeSort(ClipperLib.IntersectNode node1, ClipperLib.IntersectNode node2)
			{
				return (int)(node2.Pt.Y - node1.Pt.Y);
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x0001F4AC File Offset: 0x0001D6AC
			private bool FixupIntersectionOrder()
			{
				this.m_IntersectList.Sort(this.m_IntersectNodeComparer);
				this.CopyAELToSEL();
				int count = this.m_IntersectList.Count;
				for (int i = 0; i < count; i++)
				{
					if (!this.EdgesAdjacent(this.m_IntersectList[i]))
					{
						int num = i + 1;
						while (num < count && !this.EdgesAdjacent(this.m_IntersectList[num]))
						{
							num++;
						}
						if (num == count)
						{
							return false;
						}
						ClipperLib.IntersectNode intersectNode = this.m_IntersectList[i];
						this.m_IntersectList[i] = this.m_IntersectList[num];
						this.m_IntersectList[num] = intersectNode;
					}
					this.SwapPositionsInSEL(this.m_IntersectList[i].Edge1, this.m_IntersectList[i].Edge2);
				}
				return true;
			}

			// Token: 0x0600052F RID: 1327 RVA: 0x0001F588 File Offset: 0x0001D788
			private void ProcessIntersectList()
			{
				for (int i = 0; i < this.m_IntersectList.Count; i++)
				{
					ClipperLib.IntersectNode intersectNode = this.m_IntersectList[i];
					this.IntersectEdges(intersectNode.Edge1, intersectNode.Edge2, intersectNode.Pt);
					base.SwapPositionsInAEL(intersectNode.Edge1, intersectNode.Edge2);
				}
				this.m_IntersectList.Clear();
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x0001F5ED File Offset: 0x0001D7ED
			internal static long Round(double value)
			{
				if (value >= 0.0)
				{
					return (long)(value + 0.5);
				}
				return (long)(value - 0.5);
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x0001F614 File Offset: 0x0001D814
			private static long TopX(ClipperLib.TEdge edge, long currentY)
			{
				if (currentY == edge.Top.Y)
				{
					return edge.Top.X;
				}
				return edge.Bot.X + ClipperLib.Clipper.Round(edge.Dx * (double)(currentY - edge.Bot.Y));
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x0001F664 File Offset: 0x0001D864
			private void IntersectPoint(ClipperLib.TEdge edge1, ClipperLib.TEdge edge2, out ClipperLib.IntPoint ip)
			{
				ip = default(ClipperLib.IntPoint);
				if (edge1.Dx == edge2.Dx)
				{
					ip.Y = edge1.Curr.Y;
					ip.X = ClipperLib.Clipper.TopX(edge1, ip.Y);
					return;
				}
				if (edge1.Delta.X == 0L)
				{
					ip.X = edge1.Bot.X;
					if (ClipperLib.ClipperBase.IsHorizontal(edge2))
					{
						ip.Y = edge2.Bot.Y;
					}
					else
					{
						double num = (double)edge2.Bot.Y - (double)edge2.Bot.X / edge2.Dx;
						ip.Y = ClipperLib.Clipper.Round((double)ip.X / edge2.Dx + num);
					}
				}
				else if (edge2.Delta.X == 0L)
				{
					ip.X = edge2.Bot.X;
					if (ClipperLib.ClipperBase.IsHorizontal(edge1))
					{
						ip.Y = edge1.Bot.Y;
					}
					else
					{
						double num2 = (double)edge1.Bot.Y - (double)edge1.Bot.X / edge1.Dx;
						ip.Y = ClipperLib.Clipper.Round((double)ip.X / edge1.Dx + num2);
					}
				}
				else
				{
					double num2 = (double)edge1.Bot.X - (double)edge1.Bot.Y * edge1.Dx;
					double num = (double)edge2.Bot.X - (double)edge2.Bot.Y * edge2.Dx;
					double num3 = (num - num2) / (edge1.Dx - edge2.Dx);
					ip.Y = ClipperLib.Clipper.Round(num3);
					if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
					{
						ip.X = ClipperLib.Clipper.Round(edge1.Dx * num3 + num2);
					}
					else
					{
						ip.X = ClipperLib.Clipper.Round(edge2.Dx * num3 + num);
					}
				}
				if (ip.Y < edge1.Top.Y || ip.Y < edge2.Top.Y)
				{
					if (edge1.Top.Y > edge2.Top.Y)
					{
						ip.Y = edge1.Top.Y;
					}
					else
					{
						ip.Y = edge2.Top.Y;
					}
					if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
					{
						ip.X = ClipperLib.Clipper.TopX(edge1, ip.Y);
					}
					else
					{
						ip.X = ClipperLib.Clipper.TopX(edge2, ip.Y);
					}
				}
				if (ip.Y > edge1.Curr.Y)
				{
					ip.Y = edge1.Curr.Y;
					if (Math.Abs(edge1.Dx) > Math.Abs(edge2.Dx))
					{
						ip.X = ClipperLib.Clipper.TopX(edge2, ip.Y);
						return;
					}
					ip.X = ClipperLib.Clipper.TopX(edge1, ip.Y);
				}
			}

			// Token: 0x06000533 RID: 1331 RVA: 0x0001F94C File Offset: 0x0001DB4C
			private void ProcessEdgesAtTopOfScanbeam(long topY)
			{
				ClipperLib.TEdge tedge = this.m_ActiveEdges;
				while (tedge != null)
				{
					bool flag = this.IsMaxima(tedge, (double)topY);
					if (flag)
					{
						ClipperLib.TEdge maximaPairEx = this.GetMaximaPairEx(tedge);
						flag = maximaPairEx == null || !ClipperLib.ClipperBase.IsHorizontal(maximaPairEx);
					}
					if (flag)
					{
						if (this.StrictlySimple)
						{
							this.InsertMaxima(tedge.Top.X);
						}
						ClipperLib.TEdge prevInAEL = tedge.PrevInAEL;
						this.DoMaxima(tedge);
						if (prevInAEL == null)
						{
							tedge = this.m_ActiveEdges;
						}
						else
						{
							tedge = prevInAEL.NextInAEL;
						}
					}
					else
					{
						if (this.IsIntermediate(tedge, (double)topY) && ClipperLib.ClipperBase.IsHorizontal(tedge.NextInLML))
						{
							base.UpdateEdgeIntoAEL(ref tedge);
							if (tedge.OutIdx >= 0)
							{
								this.AddOutPt(tedge, tedge.Bot);
							}
							this.AddEdgeToSEL(tedge);
						}
						else
						{
							tedge.Curr.X = ClipperLib.Clipper.TopX(tedge, topY);
							tedge.Curr.Y = topY;
						}
						if (this.StrictlySimple)
						{
							ClipperLib.TEdge prevInAEL2 = tedge.PrevInAEL;
							if (tedge.OutIdx >= 0 && tedge.WindDelta != 0 && prevInAEL2 != null && prevInAEL2.OutIdx >= 0 && prevInAEL2.Curr.X == tedge.Curr.X && prevInAEL2.WindDelta != 0)
							{
								ClipperLib.IntPoint intPoint = new ClipperLib.IntPoint(tedge.Curr);
								ClipperLib.OutPt outPt = this.AddOutPt(prevInAEL2, intPoint);
								ClipperLib.OutPt outPt2 = this.AddOutPt(tedge, intPoint);
								this.AddJoin(outPt, outPt2, intPoint);
							}
						}
						tedge = tedge.NextInAEL;
					}
				}
				this.ProcessHorizontals();
				this.m_Maxima = null;
				for (tedge = this.m_ActiveEdges; tedge != null; tedge = tedge.NextInAEL)
				{
					if (this.IsIntermediate(tedge, (double)topY))
					{
						ClipperLib.OutPt outPt3 = null;
						if (tedge.OutIdx >= 0)
						{
							outPt3 = this.AddOutPt(tedge, tedge.Top);
						}
						base.UpdateEdgeIntoAEL(ref tedge);
						ClipperLib.TEdge prevInAEL3 = tedge.PrevInAEL;
						ClipperLib.TEdge nextInAEL = tedge.NextInAEL;
						if (prevInAEL3 != null && prevInAEL3.Curr.X == tedge.Bot.X && prevInAEL3.Curr.Y == tedge.Bot.Y && outPt3 != null && prevInAEL3.OutIdx >= 0 && prevInAEL3.Curr.Y > prevInAEL3.Top.Y && ClipperLib.ClipperBase.SlopesEqual(tedge.Curr, tedge.Top, prevInAEL3.Curr, prevInAEL3.Top, this.m_UseFullRange) && tedge.WindDelta != 0 && prevInAEL3.WindDelta != 0)
						{
							ClipperLib.OutPt outPt4 = this.AddOutPt(prevInAEL3, tedge.Bot);
							this.AddJoin(outPt3, outPt4, tedge.Top);
						}
						else if (nextInAEL != null && nextInAEL.Curr.X == tedge.Bot.X && nextInAEL.Curr.Y == tedge.Bot.Y && outPt3 != null && nextInAEL.OutIdx >= 0 && nextInAEL.Curr.Y > nextInAEL.Top.Y && ClipperLib.ClipperBase.SlopesEqual(tedge.Curr, tedge.Top, nextInAEL.Curr, nextInAEL.Top, this.m_UseFullRange) && tedge.WindDelta != 0 && nextInAEL.WindDelta != 0)
						{
							ClipperLib.OutPt outPt5 = this.AddOutPt(nextInAEL, tedge.Bot);
							this.AddJoin(outPt3, outPt5, tedge.Top);
						}
					}
				}
			}

			// Token: 0x06000534 RID: 1332 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
			private void DoMaxima(ClipperLib.TEdge e)
			{
				ClipperLib.TEdge maximaPairEx = this.GetMaximaPairEx(e);
				if (maximaPairEx == null)
				{
					if (e.OutIdx >= 0)
					{
						this.AddOutPt(e, e.Top);
					}
					base.DeleteFromAEL(e);
					return;
				}
				ClipperLib.TEdge tedge = e.NextInAEL;
				while (tedge != null && tedge != maximaPairEx)
				{
					this.IntersectEdges(e, tedge, e.Top);
					base.SwapPositionsInAEL(e, tedge);
					tedge = e.NextInAEL;
				}
				if (e.OutIdx == -1 && maximaPairEx.OutIdx == -1)
				{
					base.DeleteFromAEL(e);
					base.DeleteFromAEL(maximaPairEx);
					return;
				}
				if (e.OutIdx >= 0 && maximaPairEx.OutIdx >= 0)
				{
					if (e.OutIdx >= 0)
					{
						this.AddLocalMaxPoly(e, maximaPairEx, e.Top);
					}
					base.DeleteFromAEL(e);
					base.DeleteFromAEL(maximaPairEx);
					return;
				}
				if (e.WindDelta == 0)
				{
					if (e.OutIdx >= 0)
					{
						this.AddOutPt(e, e.Top);
						e.OutIdx = -1;
					}
					base.DeleteFromAEL(e);
					if (maximaPairEx.OutIdx >= 0)
					{
						this.AddOutPt(maximaPairEx, e.Top);
						maximaPairEx.OutIdx = -1;
					}
					base.DeleteFromAEL(maximaPairEx);
					return;
				}
				throw new ClipperLib.ClipperException("DoMaxima error");
			}

			// Token: 0x06000535 RID: 1333 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
			public static void ReversePaths(List<List<ClipperLib.IntPoint>> polys)
			{
				foreach (List<ClipperLib.IntPoint> list in polys)
				{
					list.Reverse();
				}
			}

			// Token: 0x06000536 RID: 1334 RVA: 0x0001FE10 File Offset: 0x0001E010
			public static bool Orientation(List<ClipperLib.IntPoint> poly)
			{
				return ClipperLib.Clipper.Area(poly) >= 0.0;
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x0001FE28 File Offset: 0x0001E028
			private int PointCount(ClipperLib.OutPt pts)
			{
				if (pts == null)
				{
					return 0;
				}
				int num = 0;
				ClipperLib.OutPt outPt = pts;
				do
				{
					num++;
					outPt = outPt.Next;
				}
				while (outPt != pts);
				return num;
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x0001FE50 File Offset: 0x0001E050
			private void BuildResult(List<List<ClipperLib.IntPoint>> polyg)
			{
				polyg.Clear();
				polyg.Capacity = this.m_PolyOuts.Count;
				for (int i = 0; i < this.m_PolyOuts.Count; i++)
				{
					ClipperLib.OutRec outRec = this.m_PolyOuts[i];
					if (outRec.Pts != null)
					{
						ClipperLib.OutPt outPt = outRec.Pts.Prev;
						int num = this.PointCount(outPt);
						if (num >= 2)
						{
							List<ClipperLib.IntPoint> list = new List<ClipperLib.IntPoint>(num);
							for (int j = 0; j < num; j++)
							{
								list.Add(outPt.Pt);
								outPt = outPt.Prev;
							}
							polyg.Add(list);
						}
					}
				}
			}

			// Token: 0x06000539 RID: 1337 RVA: 0x0001FEEC File Offset: 0x0001E0EC
			private void BuildResult2(ClipperLib.PolyTree polytree)
			{
				polytree.Clear();
				polytree.m_AllPolys.Capacity = this.m_PolyOuts.Count;
				for (int i = 0; i < this.m_PolyOuts.Count; i++)
				{
					ClipperLib.OutRec outRec = this.m_PolyOuts[i];
					int num = this.PointCount(outRec.Pts);
					if ((!outRec.IsOpen || num >= 2) && (outRec.IsOpen || num >= 3))
					{
						this.FixHoleLinkage(outRec);
						ClipperLib.PolyNode polyNode = new ClipperLib.PolyNode();
						polytree.m_AllPolys.Add(polyNode);
						outRec.PolyNode = polyNode;
						polyNode.m_polygon.Capacity = num;
						ClipperLib.OutPt outPt = outRec.Pts.Prev;
						for (int j = 0; j < num; j++)
						{
							polyNode.m_polygon.Add(outPt.Pt);
							outPt = outPt.Prev;
						}
					}
				}
				polytree.m_Childs.Capacity = this.m_PolyOuts.Count;
				for (int k = 0; k < this.m_PolyOuts.Count; k++)
				{
					ClipperLib.OutRec outRec2 = this.m_PolyOuts[k];
					if (outRec2.PolyNode != null)
					{
						if (outRec2.IsOpen)
						{
							outRec2.PolyNode.IsOpen = true;
							polytree.AddChild(outRec2.PolyNode);
						}
						else if (outRec2.FirstLeft != null && outRec2.FirstLeft.PolyNode != null)
						{
							outRec2.FirstLeft.PolyNode.AddChild(outRec2.PolyNode);
						}
						else
						{
							polytree.AddChild(outRec2.PolyNode);
						}
					}
				}
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x00020078 File Offset: 0x0001E278
			private void FixupOutPolyline(ClipperLib.OutRec outrec)
			{
				ClipperLib.OutPt outPt = outrec.Pts;
				ClipperLib.OutPt outPt2 = outPt.Prev;
				while (outPt != outPt2)
				{
					outPt = outPt.Next;
					if (outPt.Pt == outPt.Prev.Pt)
					{
						if (outPt == outPt2)
						{
							outPt2 = outPt.Prev;
						}
						ClipperLib.OutPt prev = outPt.Prev;
						prev.Next = outPt.Next;
						outPt.Next.Prev = prev;
						outPt = prev;
					}
				}
				if (outPt == outPt.Prev)
				{
					outrec.Pts = null;
				}
			}

			// Token: 0x0600053B RID: 1339 RVA: 0x000200F4 File Offset: 0x0001E2F4
			private void FixupOutPolygon(ClipperLib.OutRec outRec)
			{
				ClipperLib.OutPt outPt = null;
				outRec.BottomPt = null;
				ClipperLib.OutPt outPt2 = outRec.Pts;
				bool flag = base.PreserveCollinear || this.StrictlySimple;
				while (outPt2.Prev != outPt2 && outPt2.Prev != outPt2.Next)
				{
					if (outPt2.Pt == outPt2.Next.Pt || outPt2.Pt == outPt2.Prev.Pt || (ClipperLib.ClipperBase.SlopesEqual(outPt2.Prev.Pt, outPt2.Pt, outPt2.Next.Pt, this.m_UseFullRange) && (!flag || !base.Pt2IsBetweenPt1AndPt3(outPt2.Prev.Pt, outPt2.Pt, outPt2.Next.Pt))))
					{
						outPt = null;
						outPt2.Prev.Next = outPt2.Next;
						outPt2.Next.Prev = outPt2.Prev;
						outPt2 = outPt2.Prev;
					}
					else
					{
						if (outPt2 == outPt)
						{
							outRec.Pts = outPt2;
							return;
						}
						if (outPt == null)
						{
							outPt = outPt2;
						}
						outPt2 = outPt2.Next;
					}
				}
				outRec.Pts = null;
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x00020210 File Offset: 0x0001E410
			private ClipperLib.OutPt DupOutPt(ClipperLib.OutPt outPt, bool InsertAfter)
			{
				ClipperLib.OutPt outPt2 = new ClipperLib.OutPt();
				outPt2.Pt = outPt.Pt;
				outPt2.Idx = outPt.Idx;
				if (InsertAfter)
				{
					outPt2.Next = outPt.Next;
					outPt2.Prev = outPt;
					outPt.Next.Prev = outPt2;
					outPt.Next = outPt2;
				}
				else
				{
					outPt2.Prev = outPt.Prev;
					outPt2.Next = outPt;
					outPt.Prev.Next = outPt2;
					outPt.Prev = outPt2;
				}
				return outPt2;
			}

			// Token: 0x0600053D RID: 1341 RVA: 0x00020290 File Offset: 0x0001E490
			private bool GetOverlap(long a1, long a2, long b1, long b2, out long Left, out long Right)
			{
				if (a1 < a2)
				{
					if (b1 < b2)
					{
						Left = Math.Max(a1, b1);
						Right = Math.Min(a2, b2);
					}
					else
					{
						Left = Math.Max(a1, b2);
						Right = Math.Min(a2, b1);
					}
				}
				else if (b1 < b2)
				{
					Left = Math.Max(a2, b1);
					Right = Math.Min(a1, b2);
				}
				else
				{
					Left = Math.Max(a2, b2);
					Right = Math.Min(a1, b1);
				}
				return Left < Right;
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x00020310 File Offset: 0x0001E510
			private bool JoinHorz(ClipperLib.OutPt op1, ClipperLib.OutPt op1b, ClipperLib.OutPt op2, ClipperLib.OutPt op2b, ClipperLib.IntPoint Pt, bool DiscardLeft)
			{
				ClipperLib.Direction direction = ((op1.Pt.X > op1b.Pt.X) ? ClipperLib.Direction.dRightToLeft : ClipperLib.Direction.dLeftToRight);
				ClipperLib.Direction direction2 = ((op2.Pt.X > op2b.Pt.X) ? ClipperLib.Direction.dRightToLeft : ClipperLib.Direction.dLeftToRight);
				if (direction == direction2)
				{
					return false;
				}
				if (direction == ClipperLib.Direction.dLeftToRight)
				{
					while (op1.Next.Pt.X <= Pt.X && op1.Next.Pt.X >= op1.Pt.X && op1.Next.Pt.Y == Pt.Y)
					{
						op1 = op1.Next;
					}
					if (DiscardLeft && op1.Pt.X != Pt.X)
					{
						op1 = op1.Next;
					}
					op1b = this.DupOutPt(op1, !DiscardLeft);
					if (op1b.Pt != Pt)
					{
						op1 = op1b;
						op1.Pt = Pt;
						op1b = this.DupOutPt(op1, !DiscardLeft);
					}
				}
				else
				{
					while (op1.Next.Pt.X >= Pt.X && op1.Next.Pt.X <= op1.Pt.X && op1.Next.Pt.Y == Pt.Y)
					{
						op1 = op1.Next;
					}
					if (!DiscardLeft && op1.Pt.X != Pt.X)
					{
						op1 = op1.Next;
					}
					op1b = this.DupOutPt(op1, DiscardLeft);
					if (op1b.Pt != Pt)
					{
						op1 = op1b;
						op1.Pt = Pt;
						op1b = this.DupOutPt(op1, DiscardLeft);
					}
				}
				if (direction2 == ClipperLib.Direction.dLeftToRight)
				{
					while (op2.Next.Pt.X <= Pt.X && op2.Next.Pt.X >= op2.Pt.X && op2.Next.Pt.Y == Pt.Y)
					{
						op2 = op2.Next;
					}
					if (DiscardLeft && op2.Pt.X != Pt.X)
					{
						op2 = op2.Next;
					}
					op2b = this.DupOutPt(op2, !DiscardLeft);
					if (op2b.Pt != Pt)
					{
						op2 = op2b;
						op2.Pt = Pt;
						op2b = this.DupOutPt(op2, !DiscardLeft);
					}
				}
				else
				{
					while (op2.Next.Pt.X >= Pt.X && op2.Next.Pt.X <= op2.Pt.X && op2.Next.Pt.Y == Pt.Y)
					{
						op2 = op2.Next;
					}
					if (!DiscardLeft && op2.Pt.X != Pt.X)
					{
						op2 = op2.Next;
					}
					op2b = this.DupOutPt(op2, DiscardLeft);
					if (op2b.Pt != Pt)
					{
						op2 = op2b;
						op2.Pt = Pt;
						op2b = this.DupOutPt(op2, DiscardLeft);
					}
				}
				if (direction == ClipperLib.Direction.dLeftToRight == DiscardLeft)
				{
					op1.Prev = op2;
					op2.Next = op1;
					op1b.Next = op2b;
					op2b.Prev = op1b;
				}
				else
				{
					op1.Next = op2;
					op2.Prev = op1;
					op1b.Prev = op2b;
					op2b.Next = op1b;
				}
				return true;
			}

			// Token: 0x0600053F RID: 1343 RVA: 0x00020674 File Offset: 0x0001E874
			private bool JoinPoints(ClipperLib.Join j, ClipperLib.OutRec outRec1, ClipperLib.OutRec outRec2)
			{
				ClipperLib.OutPt outPt = j.OutPt1;
				ClipperLib.OutPt outPt2 = j.OutPt2;
				bool flag = j.OutPt1.Pt.Y == j.OffPt.Y;
				if (flag && j.OffPt == j.OutPt1.Pt && j.OffPt == j.OutPt2.Pt)
				{
					if (outRec1 != outRec2)
					{
						return false;
					}
					ClipperLib.OutPt outPt3 = j.OutPt1.Next;
					while (outPt3 != outPt && outPt3.Pt == j.OffPt)
					{
						outPt3 = outPt3.Next;
					}
					bool flag2 = outPt3.Pt.Y > j.OffPt.Y;
					ClipperLib.OutPt outPt4 = j.OutPt2.Next;
					while (outPt4 != outPt2 && outPt4.Pt == j.OffPt)
					{
						outPt4 = outPt4.Next;
					}
					bool flag3 = outPt4.Pt.Y > j.OffPt.Y;
					if (flag2 == flag3)
					{
						return false;
					}
					if (flag2)
					{
						outPt3 = this.DupOutPt(outPt, false);
						outPt4 = this.DupOutPt(outPt2, true);
						outPt.Prev = outPt2;
						outPt2.Next = outPt;
						outPt3.Next = outPt4;
						outPt4.Prev = outPt3;
						j.OutPt1 = outPt;
						j.OutPt2 = outPt3;
						return true;
					}
					outPt3 = this.DupOutPt(outPt, true);
					outPt4 = this.DupOutPt(outPt2, false);
					outPt.Next = outPt2;
					outPt2.Prev = outPt;
					outPt3.Prev = outPt4;
					outPt4.Next = outPt3;
					j.OutPt1 = outPt;
					j.OutPt2 = outPt3;
					return true;
				}
				else if (flag)
				{
					ClipperLib.OutPt outPt3 = outPt;
					while (outPt.Prev.Pt.Y == outPt.Pt.Y && outPt.Prev != outPt3)
					{
						if (outPt.Prev == outPt2)
						{
							break;
						}
						outPt = outPt.Prev;
					}
					while (outPt3.Next.Pt.Y == outPt3.Pt.Y && outPt3.Next != outPt && outPt3.Next != outPt2)
					{
						outPt3 = outPt3.Next;
					}
					if (outPt3.Next == outPt || outPt3.Next == outPt2)
					{
						return false;
					}
					ClipperLib.OutPt outPt4 = outPt2;
					while (outPt2.Prev.Pt.Y == outPt2.Pt.Y && outPt2.Prev != outPt4)
					{
						if (outPt2.Prev == outPt3)
						{
							break;
						}
						outPt2 = outPt2.Prev;
					}
					while (outPt4.Next.Pt.Y == outPt4.Pt.Y && outPt4.Next != outPt2 && outPt4.Next != outPt)
					{
						outPt4 = outPt4.Next;
					}
					if (outPt4.Next == outPt2 || outPt4.Next == outPt)
					{
						return false;
					}
					long num;
					long num2;
					if (!this.GetOverlap(outPt.Pt.X, outPt3.Pt.X, outPt2.Pt.X, outPt4.Pt.X, out num, out num2))
					{
						return false;
					}
					ClipperLib.IntPoint intPoint;
					bool flag4;
					if (outPt.Pt.X >= num && outPt.Pt.X <= num2)
					{
						intPoint = outPt.Pt;
						flag4 = outPt.Pt.X > outPt3.Pt.X;
					}
					else if (outPt2.Pt.X >= num && outPt2.Pt.X <= num2)
					{
						intPoint = outPt2.Pt;
						flag4 = outPt2.Pt.X > outPt4.Pt.X;
					}
					else if (outPt3.Pt.X >= num && outPt3.Pt.X <= num2)
					{
						intPoint = outPt3.Pt;
						flag4 = outPt3.Pt.X > outPt.Pt.X;
					}
					else
					{
						intPoint = outPt4.Pt;
						flag4 = outPt4.Pt.X > outPt2.Pt.X;
					}
					j.OutPt1 = outPt;
					j.OutPt2 = outPt2;
					return this.JoinHorz(outPt, outPt3, outPt2, outPt4, intPoint, flag4);
				}
				else
				{
					ClipperLib.OutPt outPt3 = outPt.Next;
					while (outPt3.Pt == outPt.Pt && outPt3 != outPt)
					{
						outPt3 = outPt3.Next;
					}
					bool flag5 = outPt3.Pt.Y > outPt.Pt.Y || !ClipperLib.ClipperBase.SlopesEqual(outPt.Pt, outPt3.Pt, j.OffPt, this.m_UseFullRange);
					if (flag5)
					{
						outPt3 = outPt.Prev;
						while (outPt3.Pt == outPt.Pt && outPt3 != outPt)
						{
							outPt3 = outPt3.Prev;
						}
						if (outPt3.Pt.Y > outPt.Pt.Y || !ClipperLib.ClipperBase.SlopesEqual(outPt.Pt, outPt3.Pt, j.OffPt, this.m_UseFullRange))
						{
							return false;
						}
					}
					ClipperLib.OutPt outPt4 = outPt2.Next;
					while (outPt4.Pt == outPt2.Pt && outPt4 != outPt2)
					{
						outPt4 = outPt4.Next;
					}
					bool flag6 = outPt4.Pt.Y > outPt2.Pt.Y || !ClipperLib.ClipperBase.SlopesEqual(outPt2.Pt, outPt4.Pt, j.OffPt, this.m_UseFullRange);
					if (flag6)
					{
						outPt4 = outPt2.Prev;
						while (outPt4.Pt == outPt2.Pt && outPt4 != outPt2)
						{
							outPt4 = outPt4.Prev;
						}
						if (outPt4.Pt.Y > outPt2.Pt.Y || !ClipperLib.ClipperBase.SlopesEqual(outPt2.Pt, outPt4.Pt, j.OffPt, this.m_UseFullRange))
						{
							return false;
						}
					}
					if (outPt3 == outPt || outPt4 == outPt2 || outPt3 == outPt4 || (outRec1 == outRec2 && flag5 == flag6))
					{
						return false;
					}
					if (flag5)
					{
						outPt3 = this.DupOutPt(outPt, false);
						outPt4 = this.DupOutPt(outPt2, true);
						outPt.Prev = outPt2;
						outPt2.Next = outPt;
						outPt3.Next = outPt4;
						outPt4.Prev = outPt3;
						j.OutPt1 = outPt;
						j.OutPt2 = outPt3;
						return true;
					}
					outPt3 = this.DupOutPt(outPt, true);
					outPt4 = this.DupOutPt(outPt2, false);
					outPt.Next = outPt2;
					outPt2.Prev = outPt;
					outPt3.Prev = outPt4;
					outPt4.Next = outPt3;
					j.OutPt1 = outPt;
					j.OutPt2 = outPt3;
					return true;
				}
			}

			// Token: 0x06000540 RID: 1344 RVA: 0x00020C84 File Offset: 0x0001EE84
			public static int PointInPolygon(ClipperLib.IntPoint pt, List<ClipperLib.IntPoint> path)
			{
				int num = 0;
				int count = path.Count;
				if (count < 3)
				{
					return 0;
				}
				ClipperLib.IntPoint intPoint = path[0];
				for (int i = 1; i <= count; i++)
				{
					ClipperLib.IntPoint intPoint2 = ((i == count) ? path[0] : path[i]);
					if (intPoint2.Y == pt.Y && (intPoint2.X == pt.X || (intPoint.Y == pt.Y && intPoint2.X > pt.X == intPoint.X < pt.X)))
					{
						return -1;
					}
					if (intPoint.Y < pt.Y != intPoint2.Y < pt.Y)
					{
						if (intPoint.X >= pt.X)
						{
							if (intPoint2.X > pt.X)
							{
								num = 1 - num;
							}
							else
							{
								double num2 = (double)(intPoint.X - pt.X) * (double)(intPoint2.Y - pt.Y) - (double)(intPoint2.X - pt.X) * (double)(intPoint.Y - pt.Y);
								if (num2 == 0.0)
								{
									return -1;
								}
								if (num2 > 0.0 == intPoint2.Y > intPoint.Y)
								{
									num = 1 - num;
								}
							}
						}
						else if (intPoint2.X > pt.X)
						{
							double num3 = (double)(intPoint.X - pt.X) * (double)(intPoint2.Y - pt.Y) - (double)(intPoint2.X - pt.X) * (double)(intPoint.Y - pt.Y);
							if (num3 == 0.0)
							{
								return -1;
							}
							if (num3 > 0.0 == intPoint2.Y > intPoint.Y)
							{
								num = 1 - num;
							}
						}
					}
					intPoint = intPoint2;
				}
				return num;
			}

			// Token: 0x06000541 RID: 1345 RVA: 0x00020E60 File Offset: 0x0001F060
			private static int PointInPolygon(ClipperLib.IntPoint pt, ClipperLib.OutPt op)
			{
				int num = 0;
				ClipperLib.OutPt outPt = op;
				long x = pt.X;
				long y = pt.Y;
				long num2 = op.Pt.X;
				long num3 = op.Pt.Y;
				for (;;)
				{
					op = op.Next;
					long x2 = op.Pt.X;
					long y2 = op.Pt.Y;
					if (y2 == y && (x2 == x || (num3 == y && x2 > x == num2 < x)))
					{
						break;
					}
					if (num3 < y != y2 < y)
					{
						if (num2 >= x)
						{
							if (x2 > x)
							{
								num = 1 - num;
							}
							else
							{
								double num4 = (double)(num2 - x) * (double)(y2 - y) - (double)(x2 - x) * (double)(num3 - y);
								if (num4 == 0.0)
								{
									return -1;
								}
								if (num4 > 0.0 == y2 > num3)
								{
									num = 1 - num;
								}
							}
						}
						else if (x2 > x)
						{
							double num5 = (double)(num2 - x) * (double)(y2 - y) - (double)(x2 - x) * (double)(num3 - y);
							if (num5 == 0.0)
							{
								return -1;
							}
							if (num5 > 0.0 == y2 > num3)
							{
								num = 1 - num;
							}
						}
					}
					num2 = x2;
					num3 = y2;
					if (outPt == op)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x06000542 RID: 1346 RVA: 0x00020F94 File Offset: 0x0001F194
			private static bool Poly2ContainsPoly1(ClipperLib.OutPt outPt1, ClipperLib.OutPt outPt2)
			{
				ClipperLib.OutPt outPt3 = outPt1;
				int num;
				for (;;)
				{
					num = ClipperLib.Clipper.PointInPolygon(outPt3.Pt, outPt2);
					if (num >= 0)
					{
						break;
					}
					outPt3 = outPt3.Next;
					if (outPt3 == outPt1)
					{
						return true;
					}
				}
				return num > 0;
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x00020FC8 File Offset: 0x0001F1C8
			private void FixupFirstLefts1(ClipperLib.OutRec OldOutRec, ClipperLib.OutRec NewOutRec)
			{
				foreach (ClipperLib.OutRec outRec in this.m_PolyOuts)
				{
					ClipperLib.OutRec outRec2 = ClipperLib.Clipper.ParseFirstLeft(outRec.FirstLeft);
					if (outRec.Pts != null && outRec2 == OldOutRec && ClipperLib.Clipper.Poly2ContainsPoly1(outRec.Pts, NewOutRec.Pts))
					{
						outRec.FirstLeft = NewOutRec;
					}
				}
			}

			// Token: 0x06000544 RID: 1348 RVA: 0x00021048 File Offset: 0x0001F248
			private void FixupFirstLefts2(ClipperLib.OutRec innerOutRec, ClipperLib.OutRec outerOutRec)
			{
				ClipperLib.OutRec firstLeft = outerOutRec.FirstLeft;
				foreach (ClipperLib.OutRec outRec in this.m_PolyOuts)
				{
					if (outRec.Pts != null && outRec != outerOutRec && outRec != innerOutRec)
					{
						ClipperLib.OutRec outRec2 = ClipperLib.Clipper.ParseFirstLeft(outRec.FirstLeft);
						if (outRec2 == firstLeft || outRec2 == innerOutRec || outRec2 == outerOutRec)
						{
							if (ClipperLib.Clipper.Poly2ContainsPoly1(outRec.Pts, innerOutRec.Pts))
							{
								outRec.FirstLeft = innerOutRec;
							}
							else if (ClipperLib.Clipper.Poly2ContainsPoly1(outRec.Pts, outerOutRec.Pts))
							{
								outRec.FirstLeft = outerOutRec;
							}
							else if (outRec.FirstLeft == innerOutRec || outRec.FirstLeft == outerOutRec)
							{
								outRec.FirstLeft = firstLeft;
							}
						}
					}
				}
			}

			// Token: 0x06000545 RID: 1349 RVA: 0x0002111C File Offset: 0x0001F31C
			private void FixupFirstLefts3(ClipperLib.OutRec OldOutRec, ClipperLib.OutRec NewOutRec)
			{
				foreach (ClipperLib.OutRec outRec in this.m_PolyOuts)
				{
					ClipperLib.OutRec outRec2 = ClipperLib.Clipper.ParseFirstLeft(outRec.FirstLeft);
					if (outRec.Pts != null && outRec2 == OldOutRec)
					{
						outRec.FirstLeft = NewOutRec;
					}
				}
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x00021188 File Offset: 0x0001F388
			private static ClipperLib.OutRec ParseFirstLeft(ClipperLib.OutRec FirstLeft)
			{
				while (FirstLeft != null && FirstLeft.Pts == null)
				{
					FirstLeft = FirstLeft.FirstLeft;
				}
				return FirstLeft;
			}

			// Token: 0x06000547 RID: 1351 RVA: 0x000211A0 File Offset: 0x0001F3A0
			private void JoinCommonEdges()
			{
				for (int i = 0; i < this.m_Joins.Count; i++)
				{
					ClipperLib.Join join = this.m_Joins[i];
					ClipperLib.OutRec outRec = this.GetOutRec(join.OutPt1.Idx);
					ClipperLib.OutRec outRec2 = this.GetOutRec(join.OutPt2.Idx);
					if (outRec.Pts != null && outRec2.Pts != null && !outRec.IsOpen && !outRec2.IsOpen)
					{
						ClipperLib.OutRec outRec3;
						if (outRec == outRec2)
						{
							outRec3 = outRec;
						}
						else if (this.OutRec1RightOfOutRec2(outRec, outRec2))
						{
							outRec3 = outRec2;
						}
						else if (this.OutRec1RightOfOutRec2(outRec2, outRec))
						{
							outRec3 = outRec;
						}
						else
						{
							outRec3 = this.GetLowermostRec(outRec, outRec2);
						}
						if (this.JoinPoints(join, outRec, outRec2))
						{
							if (outRec == outRec2)
							{
								outRec.Pts = join.OutPt1;
								outRec.BottomPt = null;
								outRec2 = base.CreateOutRec();
								outRec2.Pts = join.OutPt2;
								this.UpdateOutPtIdxs(outRec2);
								if (ClipperLib.Clipper.Poly2ContainsPoly1(outRec2.Pts, outRec.Pts))
								{
									outRec2.IsHole = !outRec.IsHole;
									outRec2.FirstLeft = outRec;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts2(outRec2, outRec);
									}
									if ((outRec2.IsHole ^ this.ReverseSolution) == this.Area(outRec2) > 0.0)
									{
										this.ReversePolyPtLinks(outRec2.Pts);
									}
								}
								else if (ClipperLib.Clipper.Poly2ContainsPoly1(outRec.Pts, outRec2.Pts))
								{
									outRec2.IsHole = outRec.IsHole;
									outRec.IsHole = !outRec2.IsHole;
									outRec2.FirstLeft = outRec.FirstLeft;
									outRec.FirstLeft = outRec2;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts2(outRec, outRec2);
									}
									if ((outRec.IsHole ^ this.ReverseSolution) == this.Area(outRec) > 0.0)
									{
										this.ReversePolyPtLinks(outRec.Pts);
									}
								}
								else
								{
									outRec2.IsHole = outRec.IsHole;
									outRec2.FirstLeft = outRec.FirstLeft;
									if (this.m_UsingPolyTree)
									{
										this.FixupFirstLefts1(outRec, outRec2);
									}
								}
							}
							else
							{
								outRec2.Pts = null;
								outRec2.BottomPt = null;
								outRec2.Idx = outRec.Idx;
								outRec.IsHole = outRec3.IsHole;
								if (outRec3 == outRec2)
								{
									outRec.FirstLeft = outRec2.FirstLeft;
								}
								outRec2.FirstLeft = outRec;
								if (this.m_UsingPolyTree)
								{
									this.FixupFirstLefts3(outRec2, outRec);
								}
							}
						}
					}
				}
			}

			// Token: 0x06000548 RID: 1352 RVA: 0x00021404 File Offset: 0x0001F604
			private void UpdateOutPtIdxs(ClipperLib.OutRec outrec)
			{
				ClipperLib.OutPt outPt = outrec.Pts;
				do
				{
					outPt.Idx = outrec.Idx;
					outPt = outPt.Prev;
				}
				while (outPt != outrec.Pts);
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x00021434 File Offset: 0x0001F634
			private void DoSimplePolygons()
			{
				int i = 0;
				while (i < this.m_PolyOuts.Count)
				{
					ClipperLib.OutRec outRec = this.m_PolyOuts[i++];
					ClipperLib.OutPt outPt = outRec.Pts;
					if (outPt != null && !outRec.IsOpen)
					{
						do
						{
							for (ClipperLib.OutPt outPt2 = outPt.Next; outPt2 != outRec.Pts; outPt2 = outPt2.Next)
							{
								if (outPt.Pt == outPt2.Pt && outPt2.Next != outPt && outPt2.Prev != outPt)
								{
									ClipperLib.OutPt prev = outPt.Prev;
									ClipperLib.OutPt prev2 = outPt2.Prev;
									outPt.Prev = prev2;
									prev2.Next = outPt;
									outPt2.Prev = prev;
									prev.Next = outPt2;
									outRec.Pts = outPt;
									ClipperLib.OutRec outRec2 = base.CreateOutRec();
									outRec2.Pts = outPt2;
									this.UpdateOutPtIdxs(outRec2);
									if (ClipperLib.Clipper.Poly2ContainsPoly1(outRec2.Pts, outRec.Pts))
									{
										outRec2.IsHole = !outRec.IsHole;
										outRec2.FirstLeft = outRec;
										if (this.m_UsingPolyTree)
										{
											this.FixupFirstLefts2(outRec2, outRec);
										}
									}
									else if (ClipperLib.Clipper.Poly2ContainsPoly1(outRec.Pts, outRec2.Pts))
									{
										outRec2.IsHole = outRec.IsHole;
										outRec.IsHole = !outRec2.IsHole;
										outRec2.FirstLeft = outRec.FirstLeft;
										outRec.FirstLeft = outRec2;
										if (this.m_UsingPolyTree)
										{
											this.FixupFirstLefts2(outRec, outRec2);
										}
									}
									else
									{
										outRec2.IsHole = outRec.IsHole;
										outRec2.FirstLeft = outRec.FirstLeft;
										if (this.m_UsingPolyTree)
										{
											this.FixupFirstLefts1(outRec, outRec2);
										}
									}
									outPt2 = outPt;
								}
							}
							outPt = outPt.Next;
						}
						while (outPt != outRec.Pts);
					}
				}
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x000215FC File Offset: 0x0001F7FC
			public static double Area(List<ClipperLib.IntPoint> poly)
			{
				int count = poly.Count;
				if (count < 3)
				{
					return 0.0;
				}
				double num = 0.0;
				int i = 0;
				int num2 = count - 1;
				while (i < count)
				{
					num += ((double)poly[num2].X + (double)poly[i].X) * ((double)poly[num2].Y - (double)poly[i].Y);
					num2 = i;
					i++;
				}
				return -num * 0.5;
			}

			// Token: 0x0600054B RID: 1355 RVA: 0x00021680 File Offset: 0x0001F880
			internal double Area(ClipperLib.OutRec outRec)
			{
				return this.Area(outRec.Pts);
			}

			// Token: 0x0600054C RID: 1356 RVA: 0x00021690 File Offset: 0x0001F890
			internal double Area(ClipperLib.OutPt op)
			{
				ClipperLib.OutPt outPt = op;
				if (op == null)
				{
					return 0.0;
				}
				double num = 0.0;
				do
				{
					num += (double)(op.Prev.Pt.X + op.Pt.X) * (double)(op.Prev.Pt.Y - op.Pt.Y);
					op = op.Next;
				}
				while (op != outPt);
				return num * 0.5;
			}

			// Token: 0x0600054D RID: 1357 RVA: 0x0002170C File Offset: 0x0001F90C
			public static List<List<ClipperLib.IntPoint>> SimplifyPolygon(List<ClipperLib.IntPoint> poly, ClipperLib.PolyFillType fillType = ClipperLib.PolyFillType.pftEvenOdd)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.StrictlySimple = true;
				clipper.AddPath(poly, ClipperLib.PolyType.ptSubject, true);
				clipper.Execute(ClipperLib.ClipType.ctUnion, list, fillType, fillType);
				return list;
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x00021744 File Offset: 0x0001F944
			public static List<List<ClipperLib.IntPoint>> SimplifyPolygons(List<List<ClipperLib.IntPoint>> polys, ClipperLib.PolyFillType fillType = ClipperLib.PolyFillType.pftEvenOdd)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.StrictlySimple = true;
				clipper.AddPaths(polys, ClipperLib.PolyType.ptSubject, true);
				clipper.Execute(ClipperLib.ClipType.ctUnion, list, fillType, fillType);
				return list;
			}

			// Token: 0x0600054F RID: 1359 RVA: 0x0002177C File Offset: 0x0001F97C
			private static double DistanceSqrd(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2)
			{
				double num = (double)pt1.X - (double)pt2.X;
				double num2 = (double)pt1.Y - (double)pt2.Y;
				return num * num + num2 * num2;
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x000217B0 File Offset: 0x0001F9B0
			private static double DistanceFromLineSqrd(ClipperLib.IntPoint pt, ClipperLib.IntPoint ln1, ClipperLib.IntPoint ln2)
			{
				double num = (double)(ln1.Y - ln2.Y);
				double num2 = (double)(ln2.X - ln1.X);
				double num3 = num * (double)ln1.X + num2 * (double)ln1.Y;
				num3 = num * (double)pt.X + num2 * (double)pt.Y - num3;
				return num3 * num3 / (num * num + num2 * num2);
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x00021810 File Offset: 0x0001FA10
			private static bool SlopesNearCollinear(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2, ClipperLib.IntPoint pt3, double distSqrd)
			{
				if (Math.Abs(pt1.X - pt2.X) > Math.Abs(pt1.Y - pt2.Y))
				{
					if (pt1.X > pt2.X == pt1.X < pt3.X)
					{
						return ClipperLib.Clipper.DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd;
					}
					if (pt2.X > pt1.X == pt2.X < pt3.X)
					{
						return ClipperLib.Clipper.DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd;
					}
					return ClipperLib.Clipper.DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd;
				}
				else
				{
					if (pt1.Y > pt2.Y == pt1.Y < pt3.Y)
					{
						return ClipperLib.Clipper.DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd;
					}
					if (pt2.Y > pt1.Y == pt2.Y < pt3.Y)
					{
						return ClipperLib.Clipper.DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd;
					}
					return ClipperLib.Clipper.DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd;
				}
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x00021904 File Offset: 0x0001FB04
			private static bool PointsAreClose(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2, double distSqrd)
			{
				double num = (double)pt1.X - (double)pt2.X;
				double num2 = (double)pt1.Y - (double)pt2.Y;
				return num * num + num2 * num2 <= distSqrd;
			}

			// Token: 0x06000553 RID: 1363 RVA: 0x0002193C File Offset: 0x0001FB3C
			private static ClipperLib.OutPt ExcludeOp(ClipperLib.OutPt op)
			{
				ClipperLib.OutPt prev = op.Prev;
				prev.Next = op.Next;
				op.Next.Prev = prev;
				prev.Idx = 0;
				return prev;
			}

			// Token: 0x06000554 RID: 1364 RVA: 0x00021970 File Offset: 0x0001FB70
			public static List<ClipperLib.IntPoint> CleanPolygon(List<ClipperLib.IntPoint> path, double distance = 1.415)
			{
				int num = path.Count;
				if (num == 0)
				{
					return new List<ClipperLib.IntPoint>();
				}
				ClipperLib.OutPt[] array = new ClipperLib.OutPt[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = new ClipperLib.OutPt();
				}
				for (int j = 0; j < num; j++)
				{
					array[j].Pt = path[j];
					array[j].Next = array[(j + 1) % num];
					array[j].Next.Prev = array[j];
					array[j].Idx = 0;
				}
				double num2 = distance * distance;
				ClipperLib.OutPt outPt = array[0];
				while (outPt.Idx == 0 && outPt.Next != outPt.Prev)
				{
					if (ClipperLib.Clipper.PointsAreClose(outPt.Pt, outPt.Prev.Pt, num2))
					{
						outPt = ClipperLib.Clipper.ExcludeOp(outPt);
						num--;
					}
					else if (ClipperLib.Clipper.PointsAreClose(outPt.Prev.Pt, outPt.Next.Pt, num2))
					{
						ClipperLib.Clipper.ExcludeOp(outPt.Next);
						outPt = ClipperLib.Clipper.ExcludeOp(outPt);
						num -= 2;
					}
					else if (ClipperLib.Clipper.SlopesNearCollinear(outPt.Prev.Pt, outPt.Pt, outPt.Next.Pt, num2))
					{
						outPt = ClipperLib.Clipper.ExcludeOp(outPt);
						num--;
					}
					else
					{
						outPt.Idx = 1;
						outPt = outPt.Next;
					}
				}
				if (num < 3)
				{
					num = 0;
				}
				List<ClipperLib.IntPoint> list = new List<ClipperLib.IntPoint>(num);
				for (int k = 0; k < num; k++)
				{
					list.Add(outPt.Pt);
					outPt = outPt.Next;
				}
				return list;
			}

			// Token: 0x06000555 RID: 1365 RVA: 0x00021AF4 File Offset: 0x0001FCF4
			public static List<List<ClipperLib.IntPoint>> CleanPolygons(List<List<ClipperLib.IntPoint>> polys, double distance = 1.415)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>(polys.Count);
				for (int i = 0; i < polys.Count; i++)
				{
					list.Add(ClipperLib.Clipper.CleanPolygon(polys[i], distance));
				}
				return list;
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x00021B34 File Offset: 0x0001FD34
			internal static List<List<ClipperLib.IntPoint>> Minkowski(List<ClipperLib.IntPoint> pattern, List<ClipperLib.IntPoint> path, bool IsSum, bool IsClosed)
			{
				int num = (IsClosed ? 1 : 0);
				int count = pattern.Count;
				int count2 = path.Count;
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>(count2);
				if (IsSum)
				{
					for (int i = 0; i < count2; i++)
					{
						List<ClipperLib.IntPoint> list2 = new List<ClipperLib.IntPoint>(count);
						foreach (ClipperLib.IntPoint intPoint in pattern)
						{
							list2.Add(new ClipperLib.IntPoint(path[i].X + intPoint.X, path[i].Y + intPoint.Y));
						}
						list.Add(list2);
					}
				}
				else
				{
					for (int j = 0; j < count2; j++)
					{
						List<ClipperLib.IntPoint> list3 = new List<ClipperLib.IntPoint>(count);
						foreach (ClipperLib.IntPoint intPoint2 in pattern)
						{
							list3.Add(new ClipperLib.IntPoint(path[j].X - intPoint2.X, path[j].Y - intPoint2.Y));
						}
						list.Add(list3);
					}
				}
				List<List<ClipperLib.IntPoint>> list4 = new List<List<ClipperLib.IntPoint>>((count2 + num) * (count + 1));
				for (int k = 0; k < count2 - 1 + num; k++)
				{
					for (int l = 0; l < count; l++)
					{
						List<ClipperLib.IntPoint> list5 = new List<ClipperLib.IntPoint>(4);
						list5.Add(list[k % count2][l % count]);
						list5.Add(list[(k + 1) % count2][l % count]);
						list5.Add(list[(k + 1) % count2][(l + 1) % count]);
						list5.Add(list[k % count2][(l + 1) % count]);
						if (!ClipperLib.Clipper.Orientation(list5))
						{
							list5.Reverse();
						}
						list4.Add(list5);
					}
				}
				return list4;
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x00021D5C File Offset: 0x0001FF5C
			public static List<List<ClipperLib.IntPoint>> MinkowskiSum(List<ClipperLib.IntPoint> pattern, List<ClipperLib.IntPoint> path, bool pathIsClosed)
			{
				List<List<ClipperLib.IntPoint>> list = ClipperLib.Clipper.Minkowski(pattern, path, true, pathIsClosed);
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.AddPaths(list, ClipperLib.PolyType.ptSubject, true);
				clipper.Execute(ClipperLib.ClipType.ctUnion, list, ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);
				return list;
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x00021D90 File Offset: 0x0001FF90
			private static List<ClipperLib.IntPoint> TranslatePath(List<ClipperLib.IntPoint> path, ClipperLib.IntPoint delta)
			{
				List<ClipperLib.IntPoint> list = new List<ClipperLib.IntPoint>(path.Count);
				for (int i = 0; i < path.Count; i++)
				{
					list.Add(new ClipperLib.IntPoint(path[i].X + delta.X, path[i].Y + delta.Y));
				}
				return list;
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x00021DEC File Offset: 0x0001FFEC
			public static List<List<ClipperLib.IntPoint>> MinkowskiSum(List<ClipperLib.IntPoint> pattern, List<List<ClipperLib.IntPoint>> paths, bool pathIsClosed)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				for (int i = 0; i < paths.Count; i++)
				{
					List<List<ClipperLib.IntPoint>> list2 = ClipperLib.Clipper.Minkowski(pattern, paths[i], true, pathIsClosed);
					clipper.AddPaths(list2, ClipperLib.PolyType.ptSubject, true);
					if (pathIsClosed)
					{
						List<ClipperLib.IntPoint> list3 = ClipperLib.Clipper.TranslatePath(paths[i], pattern[0]);
						clipper.AddPath(list3, ClipperLib.PolyType.ptClip, true);
					}
				}
				clipper.Execute(ClipperLib.ClipType.ctUnion, list, ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);
				return list;
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x00021E60 File Offset: 0x00020060
			public static List<List<ClipperLib.IntPoint>> MinkowskiDiff(List<ClipperLib.IntPoint> poly1, List<ClipperLib.IntPoint> poly2)
			{
				List<List<ClipperLib.IntPoint>> list = ClipperLib.Clipper.Minkowski(poly1, poly2, false, true);
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.AddPaths(list, ClipperLib.PolyType.ptSubject, true);
				clipper.Execute(ClipperLib.ClipType.ctUnion, list, ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);
				return list;
			}

			// Token: 0x0600055B RID: 1371 RVA: 0x00021E94 File Offset: 0x00020094
			public static List<List<ClipperLib.IntPoint>> PolyTreeToPaths(ClipperLib.PolyTree polytree)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				list.Capacity = polytree.Total;
				ClipperLib.Clipper.AddPolyNodeToPaths(polytree, ClipperLib.Clipper.NodeType.ntAny, list);
				return list;
			}

			// Token: 0x0600055C RID: 1372 RVA: 0x00021EBC File Offset: 0x000200BC
			internal static void AddPolyNodeToPaths(ClipperLib.PolyNode polynode, ClipperLib.Clipper.NodeType nt, List<List<ClipperLib.IntPoint>> paths)
			{
				bool flag = true;
				if (nt != ClipperLib.Clipper.NodeType.ntOpen)
				{
					if (nt == ClipperLib.Clipper.NodeType.ntClosed)
					{
						flag = !polynode.IsOpen;
					}
					if (polynode.m_polygon.Count > 0 && flag)
					{
						paths.Add(polynode.m_polygon);
					}
					foreach (ClipperLib.PolyNode polyNode in polynode.Childs)
					{
						ClipperLib.Clipper.AddPolyNodeToPaths(polyNode, nt, paths);
					}
					return;
				}
			}

			// Token: 0x0600055D RID: 1373 RVA: 0x00021F44 File Offset: 0x00020144
			public static List<List<ClipperLib.IntPoint>> OpenPathsFromPolyTree(ClipperLib.PolyTree polytree)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				list.Capacity = polytree.ChildCount;
				for (int i = 0; i < polytree.ChildCount; i++)
				{
					if (polytree.Childs[i].IsOpen)
					{
						list.Add(polytree.Childs[i].m_polygon);
					}
				}
				return list;
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x00021FA0 File Offset: 0x000201A0
			public static List<List<ClipperLib.IntPoint>> ClosedPathsFromPolyTree(ClipperLib.PolyTree polytree)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				list.Capacity = polytree.Total;
				ClipperLib.Clipper.AddPolyNodeToPaths(polytree, ClipperLib.Clipper.NodeType.ntClosed, list);
				return list;
			}

			// Token: 0x04000476 RID: 1142
			public const int ioReverseSolution = 1;

			// Token: 0x04000477 RID: 1143
			public const int ioStrictlySimple = 2;

			// Token: 0x04000478 RID: 1144
			public const int ioPreserveCollinear = 4;

			// Token: 0x04000479 RID: 1145
			private ClipperLib.ClipType m_ClipType;

			// Token: 0x0400047A RID: 1146
			private ClipperLib.Maxima m_Maxima;

			// Token: 0x0400047B RID: 1147
			private ClipperLib.TEdge m_SortedEdges;

			// Token: 0x0400047C RID: 1148
			private List<ClipperLib.IntersectNode> m_IntersectList;

			// Token: 0x0400047D RID: 1149
			private IComparer<ClipperLib.IntersectNode> m_IntersectNodeComparer;

			// Token: 0x0400047E RID: 1150
			private bool m_ExecuteLocked;

			// Token: 0x0400047F RID: 1151
			private ClipperLib.PolyFillType m_ClipFillType;

			// Token: 0x04000480 RID: 1152
			private ClipperLib.PolyFillType m_SubjFillType;

			// Token: 0x04000481 RID: 1153
			private List<ClipperLib.Join> m_Joins;

			// Token: 0x04000482 RID: 1154
			private List<ClipperLib.Join> m_GhostJoins;

			// Token: 0x04000483 RID: 1155
			private bool m_UsingPolyTree;

			// Token: 0x020000F6 RID: 246
			internal enum NodeType
			{
				// Token: 0x040004C1 RID: 1217
				ntAny,
				// Token: 0x040004C2 RID: 1218
				ntOpen,
				// Token: 0x040004C3 RID: 1219
				ntClosed
			}
		}

		// Token: 0x020000E4 RID: 228
		public class ClipperOffset
		{
			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x0600055F RID: 1375 RVA: 0x00021FC8 File Offset: 0x000201C8
			// (set) Token: 0x06000560 RID: 1376 RVA: 0x00021FD0 File Offset: 0x000201D0
			public double ArcTolerance { get; set; }

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x06000561 RID: 1377 RVA: 0x00021FD9 File Offset: 0x000201D9
			// (set) Token: 0x06000562 RID: 1378 RVA: 0x00021FE1 File Offset: 0x000201E1
			public double MiterLimit { get; set; }

			// Token: 0x06000563 RID: 1379 RVA: 0x00021FEA File Offset: 0x000201EA
			public ClipperOffset(double miterLimit = 2.0, double arcTolerance = 0.25)
			{
				this.MiterLimit = miterLimit;
				this.ArcTolerance = arcTolerance;
				this.m_lowest.X = -1L;
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x00022023 File Offset: 0x00020223
			public void Clear()
			{
				this.m_polyNodes.Childs.Clear();
				this.m_lowest.X = -1L;
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x00022042 File Offset: 0x00020242
			internal static long Round(double value)
			{
				if (value >= 0.0)
				{
					return (long)(value + 0.5);
				}
				return (long)(value - 0.5);
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x0002206C File Offset: 0x0002026C
			public void AddPath(List<ClipperLib.IntPoint> path, ClipperLib.JoinType joinType, ClipperLib.EndType endType)
			{
				int num = path.Count - 1;
				if (num < 0)
				{
					return;
				}
				ClipperLib.PolyNode polyNode = new ClipperLib.PolyNode();
				polyNode.m_jointype = joinType;
				polyNode.m_endtype = endType;
				if (endType != ClipperLib.EndType.etClosedLine)
				{
					if (endType != ClipperLib.EndType.etClosedPolygon)
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
				if (endType == ClipperLib.EndType.etClosedPolygon && num2 < 2)
				{
					return;
				}
				this.m_polyNodes.AddChild(polyNode);
				if (endType != ClipperLib.EndType.etClosedPolygon)
				{
					return;
				}
				if (this.m_lowest.X < 0L)
				{
					this.m_lowest = new ClipperLib.IntPoint((long)(this.m_polyNodes.ChildCount - 1), (long)num3);
					return;
				}
				ClipperLib.IntPoint intPoint = this.m_polyNodes.Childs[(int)this.m_lowest.X].m_polygon[(int)this.m_lowest.Y];
				if (polyNode.m_polygon[num3].Y > intPoint.Y || (polyNode.m_polygon[num3].Y == intPoint.Y && polyNode.m_polygon[num3].X < intPoint.X))
				{
					this.m_lowest = new ClipperLib.IntPoint((long)(this.m_polyNodes.ChildCount - 1), (long)num3);
				}
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x00022270 File Offset: 0x00020470
			public void AddPaths(List<List<ClipperLib.IntPoint>> paths, ClipperLib.JoinType joinType, ClipperLib.EndType endType)
			{
				foreach (List<ClipperLib.IntPoint> list in paths)
				{
					this.AddPath(list, joinType, endType);
				}
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x000222C0 File Offset: 0x000204C0
			private void FixOrientations()
			{
				if (this.m_lowest.X >= 0L && !ClipperLib.Clipper.Orientation(this.m_polyNodes.Childs[(int)this.m_lowest.X].m_polygon))
				{
					for (int i = 0; i < this.m_polyNodes.ChildCount; i++)
					{
						ClipperLib.PolyNode polyNode = this.m_polyNodes.Childs[i];
						if (polyNode.m_endtype == ClipperLib.EndType.etClosedPolygon || (polyNode.m_endtype == ClipperLib.EndType.etClosedLine && ClipperLib.Clipper.Orientation(polyNode.m_polygon)))
						{
							polyNode.m_polygon.Reverse();
						}
					}
					return;
				}
				for (int j = 0; j < this.m_polyNodes.ChildCount; j++)
				{
					ClipperLib.PolyNode polyNode2 = this.m_polyNodes.Childs[j];
					if (polyNode2.m_endtype == ClipperLib.EndType.etClosedLine && !ClipperLib.Clipper.Orientation(polyNode2.m_polygon))
					{
						polyNode2.m_polygon.Reverse();
					}
				}
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x000223A0 File Offset: 0x000205A0
			internal static ClipperLib.DoublePoint GetUnitNormal(ClipperLib.IntPoint pt1, ClipperLib.IntPoint pt2)
			{
				double num = (double)(pt2.X - pt1.X);
				double num2 = (double)(pt2.Y - pt1.Y);
				if (num == 0.0 && num2 == 0.0)
				{
					return default(ClipperLib.DoublePoint);
				}
				double num3 = 1.0 / Math.Sqrt(num * num + num2 * num2);
				num *= num3;
				num2 *= num3;
				return new ClipperLib.DoublePoint(num2, -num);
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x00022414 File Offset: 0x00020614
			private void DoOffset(double delta)
			{
				this.m_destPolys = new List<List<ClipperLib.IntPoint>>();
				this.m_delta = delta;
				if (ClipperLib.ClipperBase.near_zero(delta))
				{
					this.m_destPolys.Capacity = this.m_polyNodes.ChildCount;
					for (int i = 0; i < this.m_polyNodes.ChildCount; i++)
					{
						ClipperLib.PolyNode polyNode = this.m_polyNodes.Childs[i];
						if (polyNode.m_endtype == ClipperLib.EndType.etClosedPolygon)
						{
							this.m_destPolys.Add(polyNode.m_polygon);
						}
					}
					return;
				}
				if (this.MiterLimit > 2.0)
				{
					this.m_miterLim = 2.0 / (this.MiterLimit * this.MiterLimit);
				}
				else
				{
					this.m_miterLim = 0.5;
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
					ClipperLib.PolyNode polyNode2 = this.m_polyNodes.Childs[j];
					this.m_srcPoly = polyNode2.m_polygon;
					int count = this.m_srcPoly.Count;
					if (count != 0 && (delta > 0.0 || (count >= 3 && polyNode2.m_endtype == ClipperLib.EndType.etClosedPolygon)))
					{
						this.m_destPoly = new List<ClipperLib.IntPoint>();
						if (count == 1)
						{
							if (polyNode2.m_jointype == ClipperLib.JoinType.jtRound)
							{
								double num3 = 1.0;
								double num4 = 0.0;
								int num5 = 1;
								while ((double)num5 <= num2)
								{
									this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].X + num3 * delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].Y + num4 * delta)));
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
									this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].X + num7 * delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].Y + num8 * delta)));
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
								this.m_normals.Add(ClipperLib.ClipperOffset.GetUnitNormal(this.m_srcPoly[l], this.m_srcPoly[l + 1]));
							}
							if (polyNode2.m_endtype == ClipperLib.EndType.etClosedLine || polyNode2.m_endtype == ClipperLib.EndType.etClosedPolygon)
							{
								this.m_normals.Add(ClipperLib.ClipperOffset.GetUnitNormal(this.m_srcPoly[count - 1], this.m_srcPoly[0]));
							}
							else
							{
								this.m_normals.Add(new ClipperLib.DoublePoint(this.m_normals[count - 2]));
							}
							if (polyNode2.m_endtype == ClipperLib.EndType.etClosedPolygon)
							{
								int num9 = count - 1;
								for (int m = 0; m < count; m++)
								{
									this.OffsetPoint(m, ref num9, polyNode2.m_jointype);
								}
								this.m_destPolys.Add(this.m_destPoly);
							}
							else if (polyNode2.m_endtype == ClipperLib.EndType.etClosedLine)
							{
								int num10 = count - 1;
								for (int n = 0; n < count; n++)
								{
									this.OffsetPoint(n, ref num10, polyNode2.m_jointype);
								}
								this.m_destPolys.Add(this.m_destPoly);
								this.m_destPoly = new List<ClipperLib.IntPoint>();
								ClipperLib.DoublePoint doublePoint = this.m_normals[count - 1];
								for (int num11 = count - 1; num11 > 0; num11--)
								{
									this.m_normals[num11] = new ClipperLib.DoublePoint(-this.m_normals[num11 - 1].X, -this.m_normals[num11 - 1].Y);
								}
								this.m_normals[0] = new ClipperLib.DoublePoint(-doublePoint.X, -doublePoint.Y);
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
								if (polyNode2.m_endtype == ClipperLib.EndType.etOpenButt)
								{
									int num15 = count - 1;
									ClipperLib.IntPoint intPoint = new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[num15].X + this.m_normals[num15].X * delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[num15].Y + this.m_normals[num15].Y * delta));
									this.m_destPoly.Add(intPoint);
									intPoint = new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[num15].X - this.m_normals[num15].X * delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[num15].Y - this.m_normals[num15].Y * delta));
									this.m_destPoly.Add(intPoint);
								}
								else
								{
									int num16 = count - 1;
									num13 = count - 2;
									this.m_sinA = 0.0;
									this.m_normals[num16] = new ClipperLib.DoublePoint(-this.m_normals[num16].X, -this.m_normals[num16].Y);
									if (polyNode2.m_endtype == ClipperLib.EndType.etOpenSquare)
									{
										this.DoSquare(num16, num13);
									}
									else
									{
										this.DoRound(num16, num13);
									}
								}
								for (int num17 = count - 1; num17 > 0; num17--)
								{
									this.m_normals[num17] = new ClipperLib.DoublePoint(-this.m_normals[num17 - 1].X, -this.m_normals[num17 - 1].Y);
								}
								this.m_normals[0] = new ClipperLib.DoublePoint(-this.m_normals[1].X, -this.m_normals[1].Y);
								num13 = count - 1;
								for (int num18 = num13 - 1; num18 > 0; num18--)
								{
									this.OffsetPoint(num18, ref num13, polyNode2.m_jointype);
								}
								if (polyNode2.m_endtype == ClipperLib.EndType.etOpenButt)
								{
									ClipperLib.IntPoint intPoint = new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].X - this.m_normals[0].X * delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].Y - this.m_normals[0].Y * delta));
									this.m_destPoly.Add(intPoint);
									intPoint = new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].X + this.m_normals[0].X * delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[0].Y + this.m_normals[0].Y * delta));
									this.m_destPoly.Add(intPoint);
								}
								else
								{
									num13 = 1;
									this.m_sinA = 0.0;
									if (polyNode2.m_endtype == ClipperLib.EndType.etOpenSquare)
									{
										this.DoSquare(0, 1);
									}
									else
									{
										this.DoRound(0, 1);
									}
								}
								this.m_destPolys.Add(this.m_destPoly);
							}
						}
					}
				}
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x00022D14 File Offset: 0x00020F14
			public void Execute(ref List<List<ClipperLib.IntPoint>> solution, double delta)
			{
				solution.Clear();
				this.FixOrientations();
				this.DoOffset(delta);
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.AddPaths(this.m_destPolys, ClipperLib.PolyType.ptSubject, true);
				if (delta > 0.0)
				{
					clipper.Execute(ClipperLib.ClipType.ctUnion, solution, ClipperLib.PolyFillType.pftPositive, ClipperLib.PolyFillType.pftPositive);
					return;
				}
				ClipperLib.IntRect bounds = ClipperLib.ClipperBase.GetBounds(this.m_destPolys);
				clipper.AddPath(new List<ClipperLib.IntPoint>(4)
				{
					new ClipperLib.IntPoint(bounds.left - 10L, bounds.bottom + 10L),
					new ClipperLib.IntPoint(bounds.right + 10L, bounds.bottom + 10L),
					new ClipperLib.IntPoint(bounds.right + 10L, bounds.top - 10L),
					new ClipperLib.IntPoint(bounds.left - 10L, bounds.top - 10L)
				}, ClipperLib.PolyType.ptSubject, true);
				clipper.ReverseSolution = true;
				clipper.Execute(ClipperLib.ClipType.ctUnion, solution, ClipperLib.PolyFillType.pftNegative, ClipperLib.PolyFillType.pftNegative);
				if (solution.Count > 0)
				{
					solution.RemoveAt(0);
				}
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x00022E24 File Offset: 0x00021024
			public void Execute(ref ClipperLib.PolyTree solution, double delta)
			{
				solution.Clear();
				this.FixOrientations();
				this.DoOffset(delta);
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.AddPaths(this.m_destPolys, ClipperLib.PolyType.ptSubject, true);
				if (delta > 0.0)
				{
					clipper.Execute(ClipperLib.ClipType.ctUnion, solution, ClipperLib.PolyFillType.pftPositive, ClipperLib.PolyFillType.pftPositive);
					return;
				}
				ClipperLib.IntRect bounds = ClipperLib.ClipperBase.GetBounds(this.m_destPolys);
				clipper.AddPath(new List<ClipperLib.IntPoint>(4)
				{
					new ClipperLib.IntPoint(bounds.left - 10L, bounds.bottom + 10L),
					new ClipperLib.IntPoint(bounds.right + 10L, bounds.bottom + 10L),
					new ClipperLib.IntPoint(bounds.right + 10L, bounds.top - 10L),
					new ClipperLib.IntPoint(bounds.left - 10L, bounds.top - 10L)
				}, ClipperLib.PolyType.ptSubject, true);
				clipper.ReverseSolution = true;
				clipper.Execute(ClipperLib.ClipType.ctUnion, solution, ClipperLib.PolyFillType.pftNegative, ClipperLib.PolyFillType.pftNegative);
				if (solution.ChildCount == 1 && solution.Childs[0].ChildCount > 0)
				{
					ClipperLib.PolyNode polyNode = solution.Childs[0];
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

			// Token: 0x0600056D RID: 1389 RVA: 0x00022FC0 File Offset: 0x000211C0
			private void OffsetPoint(int j, ref int k, ClipperLib.JoinType jointype)
			{
				this.m_sinA = this.m_normals[k].X * this.m_normals[j].Y - this.m_normals[j].X * this.m_normals[k].Y;
				if (Math.Abs(this.m_sinA * this.m_delta) < 1.0)
				{
					if (this.m_normals[k].X * this.m_normals[j].X + this.m_normals[j].Y * this.m_normals[k].Y > 0.0)
					{
						this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[k].X * this.m_delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[k].Y * this.m_delta)));
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
					this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[k].X * this.m_delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[k].Y * this.m_delta)));
					this.m_destPoly.Add(this.m_srcPoly[j]);
					this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[j].X * this.m_delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[j].Y * this.m_delta)));
				}
				else
				{
					switch (jointype)
					{
					case ClipperLib.JoinType.jtSquare:
						this.DoSquare(j, k);
						break;
					case ClipperLib.JoinType.jtRound:
						this.DoRound(j, k);
						break;
					case ClipperLib.JoinType.jtMiter:
					{
						double num = 1.0 + (this.m_normals[j].X * this.m_normals[k].X + this.m_normals[j].Y * this.m_normals[k].Y);
						if (num >= this.m_miterLim)
						{
							this.DoMiter(j, k, num);
						}
						else
						{
							this.DoSquare(j, k);
						}
						break;
					}
					}
				}
				k = j;
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x00023304 File Offset: 0x00021504
			internal void DoSquare(int j, int k)
			{
				double num = Math.Tan(Math.Atan2(this.m_sinA, this.m_normals[k].X * this.m_normals[j].X + this.m_normals[k].Y * this.m_normals[j].Y) / 4.0);
				this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_delta * (this.m_normals[k].X - this.m_normals[k].Y * num)), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_delta * (this.m_normals[k].Y + this.m_normals[k].X * num))));
				this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_delta * (this.m_normals[j].X + this.m_normals[j].Y * num)), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_delta * (this.m_normals[j].Y - this.m_normals[j].X * num))));
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x000234A4 File Offset: 0x000216A4
			internal void DoMiter(int j, int k, double r)
			{
				double num = this.m_delta / r;
				this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + (this.m_normals[k].X + this.m_normals[j].X) * num), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + (this.m_normals[k].Y + this.m_normals[j].Y) * num)));
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x00023544 File Offset: 0x00021744
			internal void DoRound(int j, int k)
			{
				double num = Math.Atan2(this.m_sinA, this.m_normals[k].X * this.m_normals[j].X + this.m_normals[k].Y * this.m_normals[j].Y);
				int num2 = Math.Max((int)ClipperLib.ClipperOffset.Round(this.m_StepsPerRad * Math.Abs(num)), 1);
				double num3 = this.m_normals[k].X;
				double num4 = this.m_normals[k].Y;
				for (int i = 0; i < num2; i++)
				{
					this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + num3 * this.m_delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + num4 * this.m_delta)));
					double num5 = num3;
					num3 = num3 * this.m_cos - this.m_sin * num4;
					num4 = num5 * this.m_sin + num4 * this.m_cos;
				}
				this.m_destPoly.Add(new ClipperLib.IntPoint(ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].X + this.m_normals[j].X * this.m_delta), ClipperLib.ClipperOffset.Round((double)this.m_srcPoly[j].Y + this.m_normals[j].Y * this.m_delta)));
			}

			// Token: 0x04000486 RID: 1158
			private List<List<ClipperLib.IntPoint>> m_destPolys;

			// Token: 0x04000487 RID: 1159
			private List<ClipperLib.IntPoint> m_srcPoly;

			// Token: 0x04000488 RID: 1160
			private List<ClipperLib.IntPoint> m_destPoly;

			// Token: 0x04000489 RID: 1161
			private List<ClipperLib.DoublePoint> m_normals = new List<ClipperLib.DoublePoint>();

			// Token: 0x0400048A RID: 1162
			private double m_delta;

			// Token: 0x0400048B RID: 1163
			private double m_sinA;

			// Token: 0x0400048C RID: 1164
			private double m_sin;

			// Token: 0x0400048D RID: 1165
			private double m_cos;

			// Token: 0x0400048E RID: 1166
			private double m_miterLim;

			// Token: 0x0400048F RID: 1167
			private double m_StepsPerRad;

			// Token: 0x04000490 RID: 1168
			private ClipperLib.IntPoint m_lowest;

			// Token: 0x04000491 RID: 1169
			private ClipperLib.PolyNode m_polyNodes = new ClipperLib.PolyNode();

			// Token: 0x04000494 RID: 1172
			private const double two_pi = 6.283185307179586;

			// Token: 0x04000495 RID: 1173
			private const double def_arc_tolerance = 0.25;
		}

		// Token: 0x020000E5 RID: 229
		private class ClipperException : Exception
		{
			// Token: 0x06000571 RID: 1393 RVA: 0x000236D6 File Offset: 0x000218D6
			public ClipperException(string description)
				: base(description)
			{
			}
		}
	}
}
