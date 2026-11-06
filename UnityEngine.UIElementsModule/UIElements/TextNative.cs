using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.UIElements
{
	// Token: 0x02000286 RID: 646
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/TextNative.bindings.h")]
	internal static class TextNative
	{
		// Token: 0x06001224 RID: 4644 RVA: 0x000412A8 File Offset: 0x0003F4A8
		public static Vector2 GetCursorPosition(TextNativeSettings settings, Rect rect, int cursorIndex)
		{
			bool flag = settings.font == null;
			Vector2 vector;
			if (flag)
			{
				Debug.LogError("Cannot process a null font.");
				vector = Vector2.zero;
			}
			else
			{
				vector = TextNative.DoGetCursorPosition(settings, rect, cursorIndex);
			}
			return vector;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x000412E8 File Offset: 0x0003F4E8
		public static float ComputeTextWidth(TextNativeSettings settings)
		{
			bool flag = settings.font == null;
			float num;
			if (flag)
			{
				Debug.LogError("Cannot process a null font.");
				num = 0f;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(settings.text);
				if (flag2)
				{
					num = 0f;
				}
				else
				{
					num = TextNative.DoComputeTextWidth(settings);
				}
			}
			return num;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0004133C File Offset: 0x0003F53C
		public static float ComputeTextHeight(TextNativeSettings settings)
		{
			bool flag = settings.font == null;
			float num;
			if (flag)
			{
				Debug.LogError("Cannot process a null font.");
				num = 0f;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(settings.text);
				if (flag2)
				{
					num = 0f;
				}
				else
				{
					num = TextNative.DoComputeTextHeight(settings);
				}
			}
			return num;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00041390 File Offset: 0x0003F590
		public static NativeArray<TextVertex> GetVertices(TextNativeSettings settings)
		{
			int num = 0;
			TextNative.GetVertices(settings, IntPtr.Zero, UnsafeUtility.SizeOf<TextVertex>(), ref num);
			NativeArray<TextVertex> nativeArray = new NativeArray<TextVertex>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			bool flag = num > 0;
			if (flag)
			{
				TextNative.GetVertices(settings, (IntPtr)nativeArray.GetUnsafePtr<TextVertex>(), UnsafeUtility.SizeOf<TextVertex>(), ref num);
				Debug.Assert(num == nativeArray.Length);
			}
			return nativeArray;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000413F8 File Offset: 0x0003F5F8
		public static Vector2 GetOffset(TextNativeSettings settings, Rect screenRect)
		{
			bool flag = settings.font == null;
			Vector2 vector;
			if (flag)
			{
				Debug.LogError("Cannot process a null font.");
				vector = new Vector2(0f, 0f);
			}
			else
			{
				settings.text = settings.text ?? "";
				vector = TextNative.DoGetOffset(settings, screenRect);
			}
			return vector;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00041458 File Offset: 0x0003F658
		public static float ComputeTextScaling(Matrix4x4 worldMatrix, float pixelsPerPoint)
		{
			Vector3 vector = new Vector3(worldMatrix.m00, worldMatrix.m10, worldMatrix.m20);
			Vector3 vector2 = new Vector3(worldMatrix.m01, worldMatrix.m11, worldMatrix.m21);
			float num = (vector.magnitude + vector2.magnitude) / 2f;
			return num * pixelsPerPoint;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x000414B5 File Offset: 0x0003F6B5
		[FreeFunction(Name = "TextNative::ComputeTextWidth")]
		private static float DoComputeTextWidth(TextNativeSettings settings)
		{
			return TextNative.DoComputeTextWidth_Injected(ref settings);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000414BE File Offset: 0x0003F6BE
		[FreeFunction(Name = "TextNative::ComputeTextHeight")]
		private static float DoComputeTextHeight(TextNativeSettings settings)
		{
			return TextNative.DoComputeTextHeight_Injected(ref settings);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000414C8 File Offset: 0x0003F6C8
		[FreeFunction(Name = "TextNative::GetCursorPosition")]
		private static Vector2 DoGetCursorPosition(TextNativeSettings settings, Rect rect, int cursorPosition)
		{
			Vector2 vector;
			TextNative.DoGetCursorPosition_Injected(ref settings, ref rect, cursorPosition, out vector);
			return vector;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000414E2 File Offset: 0x0003F6E2
		[FreeFunction(Name = "TextNative::GetVertices")]
		private static void GetVertices(TextNativeSettings settings, IntPtr buffer, int vertexSize, ref int vertexCount)
		{
			TextNative.GetVertices_Injected(ref settings, buffer, vertexSize, ref vertexCount);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000414F0 File Offset: 0x0003F6F0
		[FreeFunction(Name = "TextNative::GetOffset")]
		private static Vector2 DoGetOffset(TextNativeSettings settings, Rect rect)
		{
			Vector2 vector;
			TextNative.DoGetOffset_Injected(ref settings, ref rect, out vector);
			return vector;
		}

		// Token: 0x0600122F RID: 4655
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float DoComputeTextWidth_Injected(ref TextNativeSettings settings);

		// Token: 0x06001230 RID: 4656
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float DoComputeTextHeight_Injected(ref TextNativeSettings settings);

		// Token: 0x06001231 RID: 4657
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DoGetCursorPosition_Injected(ref TextNativeSettings settings, ref Rect rect, int cursorPosition, out Vector2 ret);

		// Token: 0x06001232 RID: 4658
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetVertices_Injected(ref TextNativeSettings settings, IntPtr buffer, int vertexSize, ref int vertexCount);

		// Token: 0x06001233 RID: 4659
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DoGetOffset_Injected(ref TextNativeSettings settings, ref Rect rect, out Vector2 ret);
	}
}
