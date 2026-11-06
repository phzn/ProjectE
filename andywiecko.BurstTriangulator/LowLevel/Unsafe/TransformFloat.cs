using System;
using Unity.Collections;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000027 RID: 39
	internal readonly struct TransformFloat : ITransform<TransformFloat, float, float2>
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000080B5 File Offset: 0x000062B5
		public TransformFloat Identity
		{
			get
			{
				return new TransformFloat(float2x2.identity, float2.zero);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000080C6 File Offset: 0x000062C6
		public float AreaScalingFactor
		{
			get
			{
				return math.abs(math.determinant(this.rotScale));
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000080D8 File Offset: 0x000062D8
		public TransformFloat(float2x2 rotScale, float2 translation)
		{
			this.rotScale = rotScale;
			this.translation = translation;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000080F7 File Offset: 0x000062F7
		private static TransformFloat Translate(float2 offset)
		{
			return new TransformFloat(float2x2.identity, offset);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00008104 File Offset: 0x00006304
		private static TransformFloat Scale(float2 scale)
		{
			return new TransformFloat(new float2x2(scale.x, 0f, 0f, scale.y), float2.zero);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000812B File Offset: 0x0000632B
		private static TransformFloat Rotate(float2x2 rotation)
		{
			return new TransformFloat(rotation, float2.zero);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00008138 File Offset: 0x00006338
		public static TransformFloat operator *(TransformFloat lhs, TransformFloat rhs)
		{
			return new TransformFloat(math.mul(lhs.rotScale, rhs.rotScale), math.mul(math.inverse(rhs.rotScale), lhs.translation) + rhs.translation);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00008171 File Offset: 0x00006371
		public TransformFloat Inverse()
		{
			return new TransformFloat(math.inverse(this.rotScale), math.mul(this.rotScale, -this.translation));
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00008199 File Offset: 0x00006399
		public float2 Transform(float2 point)
		{
			return math.mul(this.rotScale, point + this.translation);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000081B4 File Offset: 0x000063B4
		public TransformFloat CalculatePCATransformation(NativeArray<float2> positions)
		{
			float2 @float = 0;
			foreach (float2 float2 in positions)
			{
				@float += float2;
			}
			@float /= (float)positions.Length;
			float2x2 float2x = float2x2.zero;
			for (int i = 0; i < positions.Length; i++)
			{
				float2 float3 = positions[i] - @float;
				float2x += TransformFloat.Kron(float3, float3);
			}
			float2x /= (float)positions.Length;
			float2 float4;
			float2x2 float2x2;
			TransformFloat.Eigen(float2x, out float4, out float2x2);
			TransformFloat transformFloat = TransformFloat.Rotate(math.transpose(float2x2)) * TransformFloat.Translate(-@float);
			float2 float5 = float.MaxValue;
			float2 float6 = float.MinValue;
			for (int j = 0; j < positions.Length; j++)
			{
				float2 float7 = transformFloat.Transform(positions[j]);
				float5 = math.min(float7, float5);
				float6 = math.max(float7, float6);
			}
			float2 float8 = 0.5f * (float5 + float6);
			return TransformFloat.Scale(2f / (float6 - float5)) * TransformFloat.Translate(-float8) * transformFloat;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00008324 File Offset: 0x00006524
		public TransformFloat CalculateLocalTransformation(NativeArray<float2> positions)
		{
			float2 @float = float.MaxValue;
			float2 float2 = float.MinValue;
			float2 float3 = 0;
			foreach (float2 float4 in positions)
			{
				@float = math.min(float4, @float);
				float2 = math.max(float4, float2);
				float3 += float4;
			}
			float3 /= (float)positions.Length;
			return TransformFloat.Scale(1f / math.cmax(math.max(math.abs(float2 - float3), math.abs(@float - float3)))) * TransformFloat.Translate(-float3);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000083F8 File Offset: 0x000065F8
		private static void Eigen(float2x2 matrix, out float2 eigval, out float2x2 eigvec)
		{
			float num = matrix[0][0];
			float num2 = matrix[1][1];
			float num3 = matrix[0][1];
			float num4 = num - num2;
			float num5 = num + num2;
			float num6 = (float)((num4 >= 0f) ? 1 : (-1)) * math.sqrt(num4 * num4 + 4f * num3 * num3);
			float num7 = num5 + num6;
			float num8 = num5 - num6;
			eigval = 0.5f * math.float2(num7, num8);
			float num9 = 0.5f * math.atan2(2f * num3, num4);
			eigvec = math.float2x2(math.cos(num9), -math.sin(num9), math.sin(num9), math.cos(num9));
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000084B7 File Offset: 0x000066B7
		private static float2x2 Kron(float2 a, float2 b)
		{
			return math.float2x2(a * b[0], a * b[1]);
		}

		// Token: 0x040000AA RID: 170
		private readonly float2x2 rotScale;

		// Token: 0x040000AB RID: 171
		private readonly float2 translation;
	}
}
