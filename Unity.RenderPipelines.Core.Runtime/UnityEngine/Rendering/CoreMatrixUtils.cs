using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000D3 RID: 211
	public static class CoreMatrixUtils
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x000220E8 File Offset: 0x000202E8
		public static void MatrixTimesTranslation(ref Matrix4x4 inOutMatrix, Vector3 translation)
		{
			inOutMatrix.m03 += inOutMatrix.m00 * translation.x + inOutMatrix.m01 * translation.y + inOutMatrix.m02 * translation.z;
			inOutMatrix.m13 += inOutMatrix.m10 * translation.x + inOutMatrix.m11 * translation.y + inOutMatrix.m12 * translation.z;
			inOutMatrix.m23 += inOutMatrix.m20 * translation.x + inOutMatrix.m21 * translation.y + inOutMatrix.m22 * translation.z;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00022190 File Offset: 0x00020390
		public static void TranslationTimesMatrix(ref Matrix4x4 inOutMatrix, Vector3 translation)
		{
			inOutMatrix.m00 += translation.x * inOutMatrix.m30;
			inOutMatrix.m01 += translation.x * inOutMatrix.m31;
			inOutMatrix.m02 += translation.x * inOutMatrix.m32;
			inOutMatrix.m03 += translation.x * inOutMatrix.m33;
			inOutMatrix.m10 += translation.y * inOutMatrix.m30;
			inOutMatrix.m11 += translation.y * inOutMatrix.m31;
			inOutMatrix.m12 += translation.y * inOutMatrix.m32;
			inOutMatrix.m13 += translation.y * inOutMatrix.m33;
			inOutMatrix.m20 += translation.z * inOutMatrix.m30;
			inOutMatrix.m21 += translation.z * inOutMatrix.m31;
			inOutMatrix.m22 += translation.z * inOutMatrix.m32;
			inOutMatrix.m23 += translation.z * inOutMatrix.m33;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000222B4 File Offset: 0x000204B4
		public static Matrix4x4 MultiplyPerspectiveMatrix(Matrix4x4 perspective, Matrix4x4 rhs)
		{
			Matrix4x4 matrix4x;
			matrix4x.m00 = perspective.m00 * rhs.m00;
			matrix4x.m01 = perspective.m00 * rhs.m01;
			matrix4x.m02 = perspective.m00 * rhs.m02;
			matrix4x.m03 = perspective.m00 * rhs.m03;
			matrix4x.m10 = perspective.m11 * rhs.m10;
			matrix4x.m11 = perspective.m11 * rhs.m11;
			matrix4x.m12 = perspective.m11 * rhs.m12;
			matrix4x.m13 = perspective.m11 * rhs.m13;
			matrix4x.m20 = perspective.m22 * rhs.m20 + perspective.m23 * rhs.m30;
			matrix4x.m21 = perspective.m22 * rhs.m21 + perspective.m23 * rhs.m31;
			matrix4x.m22 = perspective.m22 * rhs.m22 + perspective.m23 * rhs.m32;
			matrix4x.m23 = perspective.m22 * rhs.m23 + perspective.m23 * rhs.m33;
			matrix4x.m30 = -rhs.m20;
			matrix4x.m31 = -rhs.m21;
			matrix4x.m32 = -rhs.m22;
			matrix4x.m33 = -rhs.m23;
			return matrix4x;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00022424 File Offset: 0x00020624
		private static Matrix4x4 MultiplyOrthoMatrixCentered(Matrix4x4 ortho, Matrix4x4 rhs)
		{
			Matrix4x4 matrix4x;
			matrix4x.m00 = ortho.m00 * rhs.m00;
			matrix4x.m01 = ortho.m00 * rhs.m01;
			matrix4x.m02 = ortho.m00 * rhs.m02;
			matrix4x.m03 = ortho.m00 * rhs.m03;
			matrix4x.m10 = ortho.m11 * rhs.m10;
			matrix4x.m11 = ortho.m11 * rhs.m11;
			matrix4x.m12 = ortho.m11 * rhs.m12;
			matrix4x.m13 = ortho.m11 * rhs.m13;
			matrix4x.m20 = ortho.m22 * rhs.m20 + ortho.m23 * rhs.m30;
			matrix4x.m21 = ortho.m22 * rhs.m21 + ortho.m23 * rhs.m31;
			matrix4x.m22 = ortho.m22 * rhs.m22 + ortho.m23 * rhs.m32;
			matrix4x.m23 = ortho.m22 * rhs.m23 + ortho.m23 * rhs.m33;
			matrix4x.m30 = rhs.m20;
			matrix4x.m31 = rhs.m21;
			matrix4x.m32 = rhs.m22;
			matrix4x.m33 = rhs.m23;
			return matrix4x;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00022590 File Offset: 0x00020790
		private static Matrix4x4 MultiplyGenericOrthoMatrix(Matrix4x4 ortho, Matrix4x4 rhs)
		{
			Matrix4x4 matrix4x;
			matrix4x.m00 = ortho.m00 * rhs.m00 + ortho.m03 * rhs.m30;
			matrix4x.m01 = ortho.m00 * rhs.m01 + ortho.m03 * rhs.m31;
			matrix4x.m02 = ortho.m00 * rhs.m02 + ortho.m03 * rhs.m32;
			matrix4x.m03 = ortho.m00 * rhs.m03 + ortho.m03 * rhs.m33;
			matrix4x.m10 = ortho.m11 * rhs.m10 + ortho.m13 * rhs.m30;
			matrix4x.m11 = ortho.m11 * rhs.m11 + ortho.m13 * rhs.m31;
			matrix4x.m12 = ortho.m11 * rhs.m12 + ortho.m13 * rhs.m32;
			matrix4x.m13 = ortho.m11 * rhs.m13 + ortho.m13 * rhs.m33;
			matrix4x.m20 = ortho.m22 * rhs.m20 + ortho.m23 * rhs.m30;
			matrix4x.m21 = ortho.m22 * rhs.m21 + ortho.m23 * rhs.m31;
			matrix4x.m22 = ortho.m22 * rhs.m22 + ortho.m23 * rhs.m32;
			matrix4x.m23 = ortho.m22 * rhs.m23 + ortho.m23 * rhs.m33;
			matrix4x.m30 = rhs.m20;
			matrix4x.m31 = rhs.m21;
			matrix4x.m32 = rhs.m22;
			matrix4x.m33 = rhs.m23;
			return matrix4x;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002276A File Offset: 0x0002096A
		public static Matrix4x4 MultiplyOrthoMatrix(Matrix4x4 ortho, Matrix4x4 rhs, bool centered)
		{
			if (!centered)
			{
				return CoreMatrixUtils.MultiplyOrthoMatrixCentered(ortho, rhs);
			}
			return CoreMatrixUtils.MultiplyGenericOrthoMatrix(ortho, rhs);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002277E File Offset: 0x0002097E
		public static Matrix4x4 MultiplyProjectionMatrix(Matrix4x4 projMatrix, Matrix4x4 rhs, bool orthoCentered)
		{
			if (!orthoCentered)
			{
				return CoreMatrixUtils.MultiplyPerspectiveMatrix(projMatrix, rhs);
			}
			return CoreMatrixUtils.MultiplyOrthoMatrixCentered(projMatrix, rhs);
		}
	}
}
