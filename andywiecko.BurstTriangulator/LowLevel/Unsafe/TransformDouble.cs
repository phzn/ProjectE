using System;
using Unity.Collections;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000028 RID: 40
	internal readonly struct TransformDouble : ITransform<TransformDouble, double, double2>
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000084DA File Offset: 0x000066DA
		public TransformDouble Identity
		{
			get
			{
				return new TransformDouble(double2x2.identity, double2.zero);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000084EB File Offset: 0x000066EB
		public double AreaScalingFactor
		{
			get
			{
				return math.abs(math.determinant(this.rotScale));
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00008500 File Offset: 0x00006700
		public TransformDouble(double2x2 rotScale, double2 translation)
		{
			this.rotScale = rotScale;
			this.translation = translation;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000851F File Offset: 0x0000671F
		private static TransformDouble Translate(double2 offset)
		{
			return new TransformDouble(double2x2.identity, offset);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000852C File Offset: 0x0000672C
		private static TransformDouble Scale(double2 scale)
		{
			return new TransformDouble(new double2x2(scale.x, 0.0, 0.0, scale.y), double2.zero);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000855B File Offset: 0x0000675B
		private static TransformDouble Rotate(double2x2 rotation)
		{
			return new TransformDouble(rotation, double2.zero);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00008568 File Offset: 0x00006768
		public static TransformDouble operator *(TransformDouble lhs, TransformDouble rhs)
		{
			return new TransformDouble(math.mul(lhs.rotScale, rhs.rotScale), math.mul(math.inverse(rhs.rotScale), lhs.translation) + rhs.translation);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000085A1 File Offset: 0x000067A1
		public TransformDouble Inverse()
		{
			return new TransformDouble(math.inverse(this.rotScale), math.mul(this.rotScale, -this.translation));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000085C9 File Offset: 0x000067C9
		public double2 Transform(double2 point)
		{
			return math.mul(this.rotScale, point + this.translation);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000085E4 File Offset: 0x000067E4
		public TransformDouble CalculatePCATransformation(NativeArray<double2> positions)
		{
			double2 @double = 0;
			foreach (double2 double2 in positions)
			{
				@double += double2;
			}
			@double /= (double)positions.Length;
			double2x2 double2x = double2x2.zero;
			for (int i = 0; i < positions.Length; i++)
			{
				double2 double3 = positions[i] - @double;
				double2x += TransformDouble.Kron(double3, double3);
			}
			double2x /= (double)positions.Length;
			double2 double4;
			double2x2 double2x2;
			TransformDouble.Eigen(double2x, out double4, out double2x2);
			TransformDouble transformDouble = TransformDouble.Rotate(math.transpose(double2x2)) * TransformDouble.Translate(-@double);
			double2 double5 = double.MaxValue;
			double2 double6 = double.MinValue;
			for (int j = 0; j < positions.Length; j++)
			{
				double2 double7 = transformDouble.Transform(positions[j]);
				double5 = math.min(double7, double5);
				double6 = math.max(double7, double6);
			}
			double2 double8 = 0.5 * (double5 + double6);
			return TransformDouble.Scale(2.0 / (double6 - double5)) * TransformDouble.Translate(-double8) * transformDouble;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00008764 File Offset: 0x00006964
		public TransformDouble CalculateLocalTransformation(NativeArray<double2> positions)
		{
			double2 @double = double.MaxValue;
			double2 double2 = double.MinValue;
			double2 double3 = 0;
			foreach (double2 double4 in positions)
			{
				@double = math.min(double4, @double);
				double2 = math.max(double4, double2);
				double3 += double4;
			}
			double3 /= (double)positions.Length;
			return TransformDouble.Scale(1.0 / math.cmax(math.max(math.abs(double2 - double3), math.abs(@double - double3)))) * TransformDouble.Translate(-double3);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00008844 File Offset: 0x00006A44
		private static void Eigen(double2x2 matrix, out double2 eigval, out double2x2 eigvec)
		{
			double num = matrix[0][0];
			double num2 = matrix[1][1];
			double num3 = matrix[0][1];
			double num4 = num - num2;
			double num5 = num + num2;
			double num6 = (double)((num4 >= 0.0) ? 1 : (-1)) * math.sqrt(num4 * num4 + 4.0 * num3 * num3);
			double num7 = num5 + num6;
			double num8 = num5 - num6;
			eigval = 0.5 * math.double2(num7, num8);
			double num9 = 0.5 * math.atan2(2.0 * num3, num4);
			eigvec = math.double2x2(math.cos(num9), -math.sin(num9), math.sin(num9), math.cos(num9));
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008917 File Offset: 0x00006B17
		private static double2x2 Kron(double2 a, double2 b)
		{
			return math.double2x2(a * b[0], a * b[1]);
		}

		// Token: 0x040000AC RID: 172
		private readonly double2x2 rotScale;

		// Token: 0x040000AD RID: 173
		private readonly double2 translation;
	}
}
