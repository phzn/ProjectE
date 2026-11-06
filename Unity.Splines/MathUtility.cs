using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000016 RID: 22
	internal static class MathUtility
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00003E4C File Offset: 0x0000204C
		public static float3 MultiplyVector(float4x4 matrix, float3 vector)
		{
			float3 @float;
			@float.x = matrix.c0.x * vector.x + matrix.c1.x * vector.y + matrix.c2.x * vector.z;
			@float.y = matrix.c0.y * vector.x + matrix.c1.y * vector.y + matrix.c2.y * vector.z;
			@float.z = matrix.c0.z * vector.x + matrix.c1.z * vector.y + matrix.c2.z * vector.z;
			return @float;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003F18 File Offset: 0x00002118
		public static bool All(float4x4 matrixA, float4x4 matrixB)
		{
			bool4x4 bool4x = matrixA == matrixB;
			return math.all(bool4x.c0) && math.all(bool4x.c1) && math.all(bool4x.c2) && math.all(bool4x.c3);
		}
	}
}
