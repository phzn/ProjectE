using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A0 RID: 672
	internal static class ProjectionUtils
	{
		// Token: 0x06001347 RID: 4935 RVA: 0x000435DC File Offset: 0x000417DC
		public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float near, float far)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			float num = right - left;
			float num2 = top - bottom;
			float num3 = far - near;
			matrix4x.m00 = 2f / num;
			matrix4x.m11 = 2f / num2;
			matrix4x.m22 = 2f / num3;
			matrix4x.m03 = -(right + left) / num;
			matrix4x.m13 = -(top + bottom) / num2;
			matrix4x.m23 = -(far + near) / num3;
			matrix4x.m33 = 1f;
			return matrix4x;
		}
	}
}
