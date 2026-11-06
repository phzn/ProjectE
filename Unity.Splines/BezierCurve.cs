using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000004 RID: 4
	public struct BezierCurve : IEquatable<BezierCurve>
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021C5 File Offset: 0x000003C5
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000021D8 File Offset: 0x000003D8
		public float3 Tangent0
		{
			get
			{
				return this.P1 - this.P0;
			}
			set
			{
				this.P1 = this.P0 + value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021EC File Offset: 0x000003EC
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000021FF File Offset: 0x000003FF
		public float3 Tangent1
		{
			get
			{
				return this.P2 - this.P3;
			}
			set
			{
				this.P2 = this.P3 + value;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002214 File Offset: 0x00000414
		public BezierCurve(float3 p0, float3 p1)
		{
			this.P2 = p0;
			this.P0 = p0;
			this.P3 = p1;
			this.P1 = p1;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002244 File Offset: 0x00000444
		public BezierCurve(float3 p0, float3 p1, float3 p2)
		{
			float3 @float = 0.6666667f * p1;
			this.P0 = p0;
			this.P1 = 0.33333334f * p0 + @float;
			this.P2 = 0.33333334f * p2 + @float;
			this.P3 = p2;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002299 File Offset: 0x00000499
		public BezierCurve(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			this.P0 = p0;
			this.P1 = p1;
			this.P2 = p2;
			this.P3 = p3;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022B8 File Offset: 0x000004B8
		public BezierCurve(BezierKnot a, BezierKnot b)
		{
			this = new BezierCurve(a.Position, a.Position + math.rotate(a.Rotation, a.TangentOut), b.Position + math.rotate(b.Rotation, b.TangentIn), b.Position);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000230F File Offset: 0x0000050F
		public BezierCurve Transform(float4x4 matrix)
		{
			return new BezierCurve(math.transform(matrix, this.P0), math.transform(matrix, this.P1), math.transform(matrix, this.P2), math.transform(matrix, this.P3));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002346 File Offset: 0x00000546
		public static BezierCurve FromTangent(float3 pointA, float3 tangentOutA, float3 pointB, float3 tangentInB)
		{
			return new BezierCurve(pointA, pointA + tangentOutA, pointB + tangentInB, pointB);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000235D File Offset: 0x0000055D
		public BezierCurve GetInvertedCurve()
		{
			return new BezierCurve(this.P3, this.P2, this.P1, this.P0);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000237C File Offset: 0x0000057C
		public bool Equals(BezierCurve other)
		{
			return this.P0.Equals(other.P0) && this.P1.Equals(other.P1) && this.P2.Equals(other.P2) && this.P3.Equals(other.P3);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023D8 File Offset: 0x000005D8
		public override bool Equals(object obj)
		{
			if (obj is BezierCurve)
			{
				BezierCurve bezierCurve = (BezierCurve)obj;
				return this.Equals(bezierCurve);
			}
			return false;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002400 File Offset: 0x00000600
		public override int GetHashCode()
		{
			return (((((this.P0.GetHashCode() * 397) ^ this.P1.GetHashCode()) * 397) ^ this.P2.GetHashCode()) * 397) ^ this.P3.GetHashCode();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002466 File Offset: 0x00000666
		public static bool operator ==(BezierCurve left, BezierCurve right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002470 File Offset: 0x00000670
		public static bool operator !=(BezierCurve left, BezierCurve right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000001 RID: 1
		public float3 P0;

		// Token: 0x04000002 RID: 2
		public float3 P1;

		// Token: 0x04000003 RID: 3
		public float3 P2;

		// Token: 0x04000004 RID: 4
		public float3 P3;
	}
}
