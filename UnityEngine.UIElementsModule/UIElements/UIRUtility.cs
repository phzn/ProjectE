using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B8 RID: 696
	internal static class UIRUtility
	{
		// Token: 0x06001425 RID: 5157 RVA: 0x00047E0C File Offset: 0x0004600C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ShapeWindingIsClockwise(int maskDepth, int stencilRef)
		{
			Debug.Assert(maskDepth == stencilRef || maskDepth == stencilRef + 1);
			return maskDepth == stencilRef;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00047E38 File Offset: 0x00046038
		public static Vector4 ToVector4(Rect rc)
		{
			return new Vector4(rc.xMin, rc.yMin, rc.xMax, rc.yMax);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00047E6C File Offset: 0x0004606C
		public static bool IsRoundRect(VisualElement ve)
		{
			IResolvedStyle resolvedStyle = ve.resolvedStyle;
			return resolvedStyle.borderTopLeftRadius >= 1E-30f || resolvedStyle.borderTopRightRadius >= 1E-30f || resolvedStyle.borderBottomLeftRadius >= 1E-30f || resolvedStyle.borderBottomRightRadius >= 1E-30f;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00047EC0 File Offset: 0x000460C0
		public static void Multiply2D(this Quaternion rotation, ref Vector2 point)
		{
			float num = rotation.z * 2f;
			float num2 = 1f - rotation.z * num;
			float num3 = rotation.w * num;
			point = new Vector2(num2 * point.x - num3 * point.y, num3 * point.x + num2 * point.y);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00047F20 File Offset: 0x00046120
		public static bool IsVectorImageBackground(VisualElement ve)
		{
			return ve.computedStyle.backgroundImage.vectorImage != null;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00047F4C File Offset: 0x0004614C
		public static bool IsElementSelfHidden(VisualElement ve)
		{
			return ve.resolvedStyle.visibility == Visibility.Hidden;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00047F6C File Offset: 0x0004616C
		public static void Destroy(Object obj)
		{
			bool flag = obj == null;
			if (!flag)
			{
				bool isPlaying = Application.isPlaying;
				if (isPlaying)
				{
					Object.Destroy(obj);
				}
				else
				{
					Object.DestroyImmediate(obj);
				}
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00047FA0 File Offset: 0x000461A0
		public static int GetPrevPow2(int n)
		{
			int num = 0;
			while (n > 1)
			{
				n >>= 1;
				num++;
			}
			return 1 << num;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x00047FD0 File Offset: 0x000461D0
		public static int GetNextPow2(int n)
		{
			int i;
			for (i = 1; i < n; i <<= 1)
			{
			}
			return i;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00047FF4 File Offset: 0x000461F4
		public static int GetNextPow2Exp(int n)
		{
			int i = 1;
			int num = 0;
			while (i < n)
			{
				i <<= 1;
				num++;
			}
			return num;
		}

		// Token: 0x04000962 RID: 2402
		public static readonly string k_DefaultShaderName = Shaders.k_Runtime;

		// Token: 0x04000963 RID: 2403
		public static readonly string k_DefaultWorldSpaceShaderName = Shaders.k_RuntimeWorld;

		// Token: 0x04000964 RID: 2404
		public const float k_Epsilon = 1E-30f;

		// Token: 0x04000965 RID: 2405
		public const float k_ClearZ = 0.99f;

		// Token: 0x04000966 RID: 2406
		public const float k_MeshPosZ = 0f;

		// Token: 0x04000967 RID: 2407
		public const float k_MaskPosZ = 1f;

		// Token: 0x04000968 RID: 2408
		public const int k_MaxMaskDepth = 7;
	}
}
