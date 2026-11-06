using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200009B RID: 155
	[Serializable]
	public struct SphericalHarmonicsL1
	{
		// Token: 0x060004FA RID: 1274 RVA: 0x00018C50 File Offset: 0x00016E50
		public static SphericalHarmonicsL1 operator +(SphericalHarmonicsL1 lhs, SphericalHarmonicsL1 rhs)
		{
			return new SphericalHarmonicsL1
			{
				shAr = lhs.shAr + rhs.shAr,
				shAg = lhs.shAg + rhs.shAg,
				shAb = lhs.shAb + rhs.shAb
			};
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00018CB0 File Offset: 0x00016EB0
		public static SphericalHarmonicsL1 operator -(SphericalHarmonicsL1 lhs, SphericalHarmonicsL1 rhs)
		{
			return new SphericalHarmonicsL1
			{
				shAr = lhs.shAr - rhs.shAr,
				shAg = lhs.shAg - rhs.shAg,
				shAb = lhs.shAb - rhs.shAb
			};
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00018D10 File Offset: 0x00016F10
		public static SphericalHarmonicsL1 operator *(SphericalHarmonicsL1 lhs, float rhs)
		{
			return new SphericalHarmonicsL1
			{
				shAr = lhs.shAr * rhs,
				shAg = lhs.shAg * rhs,
				shAb = lhs.shAb * rhs
			};
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00018D60 File Offset: 0x00016F60
		public static SphericalHarmonicsL1 operator /(SphericalHarmonicsL1 lhs, float rhs)
		{
			return new SphericalHarmonicsL1
			{
				shAr = lhs.shAr / rhs,
				shAg = lhs.shAg / rhs,
				shAb = lhs.shAb / rhs
			};
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00018DAF File Offset: 0x00016FAF
		public static bool operator ==(SphericalHarmonicsL1 lhs, SphericalHarmonicsL1 rhs)
		{
			return lhs.shAr == rhs.shAr && lhs.shAg == rhs.shAg && lhs.shAb == rhs.shAb;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00018DEA File Offset: 0x00016FEA
		public static bool operator !=(SphericalHarmonicsL1 lhs, SphericalHarmonicsL1 rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00018DF6 File Offset: 0x00016FF6
		public override bool Equals(object other)
		{
			return other is SphericalHarmonicsL1 && this == (SphericalHarmonicsL1)other;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00018E14 File Offset: 0x00017014
		public override int GetHashCode()
		{
			return ((391 + this.shAr.GetHashCode()) * 23 + this.shAg.GetHashCode()) * 23 + this.shAb.GetHashCode();
		}

		// Token: 0x0400035B RID: 859
		public Vector4 shAr;

		// Token: 0x0400035C RID: 860
		public Vector4 shAg;

		// Token: 0x0400035D RID: 861
		public Vector4 shAb;

		// Token: 0x0400035E RID: 862
		public static readonly SphericalHarmonicsL1 zero = new SphericalHarmonicsL1
		{
			shAr = Vector4.zero,
			shAg = Vector4.zero,
			shAb = Vector4.zero
		};
	}
}
