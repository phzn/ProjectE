using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000116 RID: 278
	[Obsolete("This modifier is deprecated")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/advancedsmooth.html")]
	[Serializable]
	public class AdvancedSmooth : MonoModifier
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00030F05 File Offset: 0x0002F105
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00030F0C File Offset: 0x0002F10C
		public override void Apply(Path p)
		{
			Vector3[] array = p.vectorPath.ToArray();
			if (array.Length <= 2)
			{
				return;
			}
			List<Vector3> list = new List<Vector3>();
			list.Add(array[0]);
			AdvancedSmooth.TurnConstructor.turningRadius = this.turningRadius;
			for (int i = 1; i < array.Length - 1; i++)
			{
				List<AdvancedSmooth.Turn> list2 = new List<AdvancedSmooth.Turn>();
				AdvancedSmooth.TurnConstructor.Setup(i, array);
				this.turnConstruct1.Prepare(i, array);
				this.turnConstruct2.Prepare(i, array);
				AdvancedSmooth.TurnConstructor.PostPrepare();
				if (i == 1)
				{
					this.turnConstruct1.PointToTangent(list2);
					this.turnConstruct2.PointToTangent(list2);
				}
				else
				{
					this.turnConstruct1.TangentToTangent(list2);
					this.turnConstruct2.TangentToTangent(list2);
				}
				this.EvaluatePaths(list2, list);
				if (i == array.Length - 2)
				{
					this.turnConstruct1.TangentToPoint(list2);
					this.turnConstruct2.TangentToPoint(list2);
				}
				this.EvaluatePaths(list2, list);
			}
			list.Add(array[array.Length - 1]);
			p.vectorPath = list;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0003100C File Offset: 0x0002F20C
		private void EvaluatePaths(List<AdvancedSmooth.Turn> turnList, List<Vector3> output)
		{
			turnList.Sort();
			for (int i = 0; i < turnList.Count; i++)
			{
				if (i == 0)
				{
					turnList[i].GetPath(output);
				}
			}
			turnList.Clear();
			if (AdvancedSmooth.TurnConstructor.changedPreviousTangent)
			{
				this.turnConstruct1.OnTangentUpdate();
				this.turnConstruct2.OnTangentUpdate();
			}
		}

		// Token: 0x040005FC RID: 1532
		public float turningRadius = 1f;

		// Token: 0x040005FD RID: 1533
		public AdvancedSmooth.MaxTurn turnConstruct1 = new AdvancedSmooth.MaxTurn();

		// Token: 0x040005FE RID: 1534
		public AdvancedSmooth.ConstantTurn turnConstruct2 = new AdvancedSmooth.ConstantTurn();

		// Token: 0x02000117 RID: 279
		[Serializable]
		public class MaxTurn : AdvancedSmooth.TurnConstructor
		{
			// Token: 0x0600098D RID: 2445 RVA: 0x00031090 File Offset: 0x0002F290
			public override void OnTangentUpdate()
			{
				this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.141592653589793;
			}

			// Token: 0x0600098E RID: 2446 RVA: 0x00031110 File Offset: 0x0002F310
			public override void Prepare(int i, Vector3[] vectorPath)
			{
				this.preRightCircleCenter = this.rightCircleCenter;
				this.preLeftCircleCenter = this.leftCircleCenter;
				this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.preVaRight = this.vaRight;
				this.preVaLeft = this.vaLeft;
				this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.141592653589793;
			}

			// Token: 0x0600098F RID: 2447 RVA: 0x000311C0 File Offset: 0x0002F3C0
			public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				this.alfaRightRight = base.Atan2(this.rightCircleCenter - this.preRightCircleCenter);
				this.alfaLeftLeft = base.Atan2(this.leftCircleCenter - this.preLeftCircleCenter);
				this.alfaRightLeft = base.Atan2(this.leftCircleCenter - this.preRightCircleCenter);
				this.alfaLeftRight = base.Atan2(this.rightCircleCenter - this.preLeftCircleCenter);
				double num = (double)(this.leftCircleCenter - this.preRightCircleCenter).magnitude;
				double num2 = (double)(this.rightCircleCenter - this.preLeftCircleCenter).magnitude;
				bool flag = false;
				bool flag2 = false;
				if (num < (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num = (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag = true;
				}
				if (num2 < (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num2 = (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag2 = true;
				}
				this.deltaRightLeft = (flag ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num)));
				this.deltaLeftRight = (flag2 ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num2)));
				this.betaRightRight = base.ClockwiseAngle(this.preVaRight, this.alfaRightRight - 1.5707963267948966);
				this.betaRightLeft = base.ClockwiseAngle(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft);
				this.betaLeftRight = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight);
				this.betaLeftLeft = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966);
				this.betaRightRight += base.ClockwiseAngle(this.alfaRightRight - 1.5707963267948966, this.vaRight);
				this.betaRightLeft += base.CounterClockwiseAngle(this.alfaRightLeft + this.deltaRightLeft, this.vaLeft);
				this.betaLeftRight += base.ClockwiseAngle(this.alfaLeftRight - this.deltaLeftRight, this.vaRight);
				this.betaLeftLeft += base.CounterClockwiseAngle(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft);
				this.betaRightRight = base.GetLengthFromAngle(this.betaRightRight, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaRightLeft = base.GetLengthFromAngle(this.betaRightLeft, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftRight = base.GetLengthFromAngle(this.betaLeftRight, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftLeft = base.GetLengthFromAngle(this.betaLeftLeft, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				Vector3 vector = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				Vector3 vector2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				Vector3 vector3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				Vector3 vector4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				Vector3 vector5 = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				Vector3 vector6 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft + 3.141592653589793) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				Vector3 vector7 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight + 3.141592653589793) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				Vector3 vector8 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				this.betaRightRight += (double)(vector - vector5).magnitude;
				this.betaRightLeft += (double)(vector2 - vector6).magnitude;
				this.betaLeftRight += (double)(vector3 - vector7).magnitude;
				this.betaLeftLeft += (double)(vector4 - vector8).magnitude;
				if (flag)
				{
					this.betaRightLeft += 10000000.0;
				}
				if (flag2)
				{
					this.betaLeftRight += 10000000.0;
				}
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaRightRight, this, 2));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaRightLeft, this, 3));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaLeftRight, this, 4));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaLeftLeft, this, 5));
			}

			// Token: 0x06000990 RID: 2448 RVA: 0x00031710 File Offset: 0x0002F910
			public override void PointToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude;
				float magnitude2 = (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude;
				if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				double num = (flag ? 0.0 : base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter));
				double num2 = (flag ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude))));
				this.gammaRight = num + num2;
				double num3 = (flag ? 0.0 : base.ClockwiseAngle(this.gammaRight, this.vaRight));
				double num4 = (flag2 ? 0.0 : base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter));
				double num5 = (flag2 ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude))));
				this.gammaLeft = num4 - num5;
				double num6 = (flag2 ? 0.0 : base.CounterClockwiseAngle(this.gammaLeft, this.vaLeft));
				if (!flag)
				{
					turnList.Add(new AdvancedSmooth.Turn((float)num3, this, 0));
				}
				if (!flag2)
				{
					turnList.Add(new AdvancedSmooth.Turn((float)num6, this, 1));
				}
			}

			// Token: 0x06000991 RID: 2449 RVA: 0x000318B0 File Offset: 0x0002FAB0
			public override void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter).magnitude;
				float magnitude2 = (AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter).magnitude;
				if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				if (!flag)
				{
					double num = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter);
					double num2 = 1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / magnitude));
					this.gammaRight = num - num2;
					double num3 = base.ClockwiseAngle(this.vaRight, this.gammaRight);
					turnList.Add(new AdvancedSmooth.Turn((float)num3, this, 6));
				}
				if (!flag2)
				{
					double num4 = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter);
					double num5 = 1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / magnitude2));
					this.gammaLeft = num4 + num5;
					double num6 = base.CounterClockwiseAngle(this.vaLeft, this.gammaLeft);
					turnList.Add(new AdvancedSmooth.Turn((float)num6, this, 7));
				}
			}

			// Token: 0x06000992 RID: 2450 RVA: 0x000319D0 File Offset: 0x0002FBD0
			public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
			{
				switch (turn.id)
				{
				case 0:
					base.AddCircleSegment(this.gammaRight, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 1:
					base.AddCircleSegment(this.gammaLeft, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 2:
					base.AddCircleSegment(this.preVaRight, this.alfaRightRight - 1.5707963267948966, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightRight - 1.5707963267948966, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 3:
					base.AddCircleSegment(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightLeft - this.deltaRightLeft + 3.141592653589793, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 4:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftRight + this.deltaLeftRight + 3.141592653589793, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 5:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 6:
					base.AddCircleSegment(this.vaRight, this.gammaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 7:
					base.AddCircleSegment(this.vaLeft, this.gammaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				default:
					return;
				}
			}

			// Token: 0x040005FF RID: 1535
			private Vector3 preRightCircleCenter = Vector3.zero;

			// Token: 0x04000600 RID: 1536
			private Vector3 preLeftCircleCenter = Vector3.zero;

			// Token: 0x04000601 RID: 1537
			private Vector3 rightCircleCenter;

			// Token: 0x04000602 RID: 1538
			private Vector3 leftCircleCenter;

			// Token: 0x04000603 RID: 1539
			private double vaRight;

			// Token: 0x04000604 RID: 1540
			private double vaLeft;

			// Token: 0x04000605 RID: 1541
			private double preVaLeft;

			// Token: 0x04000606 RID: 1542
			private double preVaRight;

			// Token: 0x04000607 RID: 1543
			private double gammaLeft;

			// Token: 0x04000608 RID: 1544
			private double gammaRight;

			// Token: 0x04000609 RID: 1545
			private double betaRightRight;

			// Token: 0x0400060A RID: 1546
			private double betaRightLeft;

			// Token: 0x0400060B RID: 1547
			private double betaLeftRight;

			// Token: 0x0400060C RID: 1548
			private double betaLeftLeft;

			// Token: 0x0400060D RID: 1549
			private double deltaRightLeft;

			// Token: 0x0400060E RID: 1550
			private double deltaLeftRight;

			// Token: 0x0400060F RID: 1551
			private double alfaRightRight;

			// Token: 0x04000610 RID: 1552
			private double alfaLeftLeft;

			// Token: 0x04000611 RID: 1553
			private double alfaRightLeft;

			// Token: 0x04000612 RID: 1554
			private double alfaLeftRight;
		}

		// Token: 0x02000118 RID: 280
		[Serializable]
		public class ConstantTurn : AdvancedSmooth.TurnConstructor
		{
			// Token: 0x06000994 RID: 2452 RVA: 0x00003788 File Offset: 0x00001988
			public override void Prepare(int i, Vector3[] vectorPath)
			{
			}

			// Token: 0x06000995 RID: 2453 RVA: 0x00031BFC File Offset: 0x0002FDFC
			public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				Vector3 vector = Vector3.Cross(AdvancedSmooth.TurnConstructor.t1, Vector3.up);
				Vector3 vector2 = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.prev;
				Vector3 vector3 = vector2 * 0.5f + AdvancedSmooth.TurnConstructor.prev;
				vector2 = Vector3.Cross(vector2, Vector3.up);
				bool flag;
				this.circleCenter = VectorMath.LineDirIntersectionPointXZ(AdvancedSmooth.TurnConstructor.prev, vector, vector3, vector2, out flag);
				if (!flag)
				{
					return;
				}
				this.gamma1 = base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.circleCenter);
				this.gamma2 = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.circleCenter);
				this.clockwise = !VectorMath.RightOrColinearXZ(this.circleCenter, AdvancedSmooth.TurnConstructor.prev, AdvancedSmooth.TurnConstructor.prev + AdvancedSmooth.TurnConstructor.t1);
				double num = (this.clockwise ? base.ClockwiseAngle(this.gamma1, this.gamma2) : base.CounterClockwiseAngle(this.gamma1, this.gamma2));
				num = base.GetLengthFromAngle(num, (double)(this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
				turnList.Add(new AdvancedSmooth.Turn((float)num, this, 0));
			}

			// Token: 0x06000996 RID: 2454 RVA: 0x00031D28 File Offset: 0x0002FF28
			public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
			{
				base.AddCircleSegment(this.gamma1, this.gamma2, this.clockwise, this.circleCenter, output, (this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
				AdvancedSmooth.TurnConstructor.normal = (AdvancedSmooth.TurnConstructor.current - this.circleCenter).normalized;
				AdvancedSmooth.TurnConstructor.t2 = Vector3.Cross(AdvancedSmooth.TurnConstructor.normal, Vector3.up).normalized;
				AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
				if (!this.clockwise)
				{
					AdvancedSmooth.TurnConstructor.t2 = -AdvancedSmooth.TurnConstructor.t2;
					AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
				}
				AdvancedSmooth.TurnConstructor.changedPreviousTangent = true;
			}

			// Token: 0x04000613 RID: 1555
			private Vector3 circleCenter;

			// Token: 0x04000614 RID: 1556
			private double gamma1;

			// Token: 0x04000615 RID: 1557
			private double gamma2;

			// Token: 0x04000616 RID: 1558
			private bool clockwise;
		}

		// Token: 0x02000119 RID: 281
		public abstract class TurnConstructor
		{
			// Token: 0x06000998 RID: 2456
			public abstract void Prepare(int i, Vector3[] vectorPath);

			// Token: 0x06000999 RID: 2457 RVA: 0x00003788 File Offset: 0x00001988
			public virtual void OnTangentUpdate()
			{
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x00003788 File Offset: 0x00001988
			public virtual void PointToTangent(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x0600099B RID: 2459 RVA: 0x00003788 File Offset: 0x00001988
			public virtual void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x0600099C RID: 2460 RVA: 0x00003788 File Offset: 0x00001988
			public virtual void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x0600099D RID: 2461
			public abstract void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output);

			// Token: 0x0600099E RID: 2462 RVA: 0x00031DE8 File Offset: 0x0002FFE8
			public static void Setup(int i, Vector3[] vectorPath)
			{
				AdvancedSmooth.TurnConstructor.current = vectorPath[i];
				AdvancedSmooth.TurnConstructor.prev = vectorPath[i - 1];
				AdvancedSmooth.TurnConstructor.next = vectorPath[i + 1];
				AdvancedSmooth.TurnConstructor.prev.y = AdvancedSmooth.TurnConstructor.current.y;
				AdvancedSmooth.TurnConstructor.next.y = AdvancedSmooth.TurnConstructor.current.y;
				AdvancedSmooth.TurnConstructor.t1 = AdvancedSmooth.TurnConstructor.t2;
				AdvancedSmooth.TurnConstructor.t2 = (AdvancedSmooth.TurnConstructor.next - AdvancedSmooth.TurnConstructor.current).normalized - (AdvancedSmooth.TurnConstructor.prev - AdvancedSmooth.TurnConstructor.current).normalized;
				AdvancedSmooth.TurnConstructor.t2 = AdvancedSmooth.TurnConstructor.t2.normalized;
				AdvancedSmooth.TurnConstructor.prevNormal = AdvancedSmooth.TurnConstructor.normal;
				AdvancedSmooth.TurnConstructor.normal = Vector3.Cross(AdvancedSmooth.TurnConstructor.t2, Vector3.up);
				AdvancedSmooth.TurnConstructor.normal = AdvancedSmooth.TurnConstructor.normal.normalized;
			}

			// Token: 0x0600099F RID: 2463 RVA: 0x00031EC3 File Offset: 0x000300C3
			public static void PostPrepare()
			{
				AdvancedSmooth.TurnConstructor.changedPreviousTangent = false;
			}

			// Token: 0x060009A0 RID: 2464 RVA: 0x00031ECC File Offset: 0x000300CC
			public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, Vector3 center, List<Vector3> output, float radius)
			{
				double num = 0.06283185307179587;
				if (clockwise)
				{
					while (endAngle > startAngle + 6.283185307179586)
					{
						endAngle -= 6.283185307179586;
					}
					while (endAngle < startAngle)
					{
						endAngle += 6.283185307179586;
					}
				}
				else
				{
					while (endAngle < startAngle - 6.283185307179586)
					{
						endAngle += 6.283185307179586;
					}
					while (endAngle > startAngle)
					{
						endAngle -= 6.283185307179586;
					}
				}
				if (clockwise)
				{
					for (double num2 = startAngle; num2 < endAngle; num2 += num)
					{
						output.Add(this.AngleToVector(num2) * radius + center);
					}
				}
				else
				{
					for (double num3 = startAngle; num3 > endAngle; num3 -= num)
					{
						output.Add(this.AngleToVector(num3) * radius + center);
					}
				}
				output.Add(this.AngleToVector(endAngle) * radius + center);
			}

			// Token: 0x060009A1 RID: 2465 RVA: 0x00031FB8 File Offset: 0x000301B8
			public void DebugCircleSegment(Vector3 center, double startAngle, double endAngle, double radius, Color color)
			{
				double num = 0.06283185307179587;
				while (endAngle < startAngle)
				{
					endAngle += 6.283185307179586;
				}
				Vector3 vector = this.AngleToVector(startAngle) * (float)radius + center;
				for (double num2 = startAngle + num; num2 < endAngle; num2 += num)
				{
					Debug.DrawLine(vector, this.AngleToVector(num2) * (float)radius + center);
				}
				Debug.DrawLine(vector, this.AngleToVector(endAngle) * (float)radius + center);
			}

			// Token: 0x060009A2 RID: 2466 RVA: 0x0003203C File Offset: 0x0003023C
			public void DebugCircle(Vector3 center, double radius, Color color)
			{
				double num = 0.06283185307179587;
				Vector3 vector = this.AngleToVector(-num) * (float)radius + center;
				for (double num2 = 0.0; num2 < 6.283185307179586; num2 += num)
				{
					Vector3 vector2 = this.AngleToVector(num2) * (float)radius + center;
					Debug.DrawLine(vector, vector2, color);
					vector = vector2;
				}
			}

			// Token: 0x060009A3 RID: 2467 RVA: 0x000320A4 File Offset: 0x000302A4
			public double GetLengthFromAngle(double angle, double radius)
			{
				return radius * angle;
			}

			// Token: 0x060009A4 RID: 2468 RVA: 0x000320A9 File Offset: 0x000302A9
			public double ClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(to - from);
			}

			// Token: 0x060009A5 RID: 2469 RVA: 0x000320B4 File Offset: 0x000302B4
			public double CounterClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(from - to);
			}

			// Token: 0x060009A6 RID: 2470 RVA: 0x000320BF File Offset: 0x000302BF
			public Vector3 AngleToVector(double a)
			{
				return new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x000320D9 File Offset: 0x000302D9
			public double ToDegrees(double rad)
			{
				return rad * 57.295780181884766;
			}

			// Token: 0x060009A8 RID: 2472 RVA: 0x000320E6 File Offset: 0x000302E6
			public double ClampAngle(double a)
			{
				while (a < 0.0)
				{
					a += 6.283185307179586;
				}
				while (a > 6.283185307179586)
				{
					a -= 6.283185307179586;
				}
				return a;
			}

			// Token: 0x060009A9 RID: 2473 RVA: 0x0003211F File Offset: 0x0003031F
			public double Atan2(Vector3 v)
			{
				return Math.Atan2((double)v.z, (double)v.x);
			}

			// Token: 0x04000617 RID: 1559
			public float constantBias;

			// Token: 0x04000618 RID: 1560
			public float factorBias = 1f;

			// Token: 0x04000619 RID: 1561
			public static float turningRadius = 1f;

			// Token: 0x0400061A RID: 1562
			public const double ThreeSixtyRadians = 6.283185307179586;

			// Token: 0x0400061B RID: 1563
			public static Vector3 prev;

			// Token: 0x0400061C RID: 1564
			public static Vector3 current;

			// Token: 0x0400061D RID: 1565
			public static Vector3 next;

			// Token: 0x0400061E RID: 1566
			public static Vector3 t1;

			// Token: 0x0400061F RID: 1567
			public static Vector3 t2;

			// Token: 0x04000620 RID: 1568
			public static Vector3 normal;

			// Token: 0x04000621 RID: 1569
			public static Vector3 prevNormal;

			// Token: 0x04000622 RID: 1570
			public static bool changedPreviousTangent = false;
		}

		// Token: 0x0200011A RID: 282
		public struct Turn : IComparable<AdvancedSmooth.Turn>
		{
			// Token: 0x170001BE RID: 446
			// (get) Token: 0x060009AC RID: 2476 RVA: 0x00032159 File Offset: 0x00030359
			public float score
			{
				get
				{
					return this.length * this.constructor.factorBias + this.constructor.constantBias;
				}
			}

			// Token: 0x060009AD RID: 2477 RVA: 0x00032179 File Offset: 0x00030379
			public Turn(float length, AdvancedSmooth.TurnConstructor constructor, int id = 0)
			{
				this.length = length;
				this.id = id;
				this.constructor = constructor;
			}

			// Token: 0x060009AE RID: 2478 RVA: 0x00032190 File Offset: 0x00030390
			public void GetPath(List<Vector3> output)
			{
				this.constructor.GetPath(this, output);
			}

			// Token: 0x060009AF RID: 2479 RVA: 0x000321A4 File Offset: 0x000303A4
			public int CompareTo(AdvancedSmooth.Turn t)
			{
				if (t.score > this.score)
				{
					return -1;
				}
				if (t.score >= this.score)
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x060009B0 RID: 2480 RVA: 0x000321C9 File Offset: 0x000303C9
			public static bool operator <(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
			{
				return lhs.score < rhs.score;
			}

			// Token: 0x060009B1 RID: 2481 RVA: 0x000321DB File Offset: 0x000303DB
			public static bool operator >(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
			{
				return lhs.score > rhs.score;
			}

			// Token: 0x04000623 RID: 1571
			public float length;

			// Token: 0x04000624 RID: 1572
			public int id;

			// Token: 0x04000625 RID: 1573
			public AdvancedSmooth.TurnConstructor constructor;
		}
	}
}
