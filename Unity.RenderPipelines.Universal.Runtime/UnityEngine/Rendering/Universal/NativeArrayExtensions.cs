using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000F5 RID: 245
	internal static class NativeArrayExtensions
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x0003382B File Offset: 0x00031A2B
		public static ref T UnsafeElementAt<T>(this NativeArray<T> array, int index) where T : struct
		{
			return UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafeReadOnlyPtr<T>(), index);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00033839 File Offset: 0x00031A39
		public static ref T UnsafeElementAtMutable<T>(this NativeArray<T> array, int index) where T : struct
		{
			return UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr<T>(), index);
		}
	}
}
