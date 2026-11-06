using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000015 RID: 21
	[NativeHeader("Modules/IMGUI/GUIDebugger.bindings.h")]
	internal class GUIDebugger
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x000080EF File Offset: 0x000062EF
		[NativeConditional("UNITY_EDITOR")]
		public static void LogLayoutEntry(Rect rect, int left, int right, int top, int bottom, GUIStyle style)
		{
			GUIDebugger.LogLayoutEntry_Injected(ref rect, left, right, top, bottom, style);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000080FF File Offset: 0x000062FF
		[NativeConditional("UNITY_EDITOR")]
		public static void LogLayoutGroupEntry(Rect rect, int left, int right, int top, int bottom, GUIStyle style, bool isVertical)
		{
			GUIDebugger.LogLayoutGroupEntry_Injected(ref rect, left, right, top, bottom, style, isVertical);
		}

		// Token: 0x060001A9 RID: 425
		[NativeConditional("UNITY_EDITOR")]
		[StaticAccessor("GetGUIDebuggerManager()", StaticAccessorType.Dot)]
		[NativeMethod("LogEndGroup")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogLayoutEndGroup();

		// Token: 0x060001AA RID: 426 RVA: 0x00008111 File Offset: 0x00006311
		[NativeConditional("UNITY_EDITOR")]
		[StaticAccessor("GetGUIDebuggerManager()", StaticAccessorType.Dot)]
		public static void LogBeginProperty(string targetTypeAssemblyQualifiedName, string path, Rect position)
		{
			GUIDebugger.LogBeginProperty_Injected(targetTypeAssemblyQualifiedName, path, ref position);
		}

		// Token: 0x060001AB RID: 427
		[StaticAccessor("GetGUIDebuggerManager()", StaticAccessorType.Dot)]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogEndProperty();

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001AC RID: 428
		[NativeConditional("UNITY_EDITOR")]
		public static extern bool active
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060001AE RID: 430
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LogLayoutEntry_Injected(ref Rect rect, int left, int right, int top, int bottom, GUIStyle style);

		// Token: 0x060001AF RID: 431
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LogLayoutGroupEntry_Injected(ref Rect rect, int left, int right, int top, int bottom, GUIStyle style, bool isVertical);

		// Token: 0x060001B0 RID: 432
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LogBeginProperty_Injected(string targetTypeAssemblyQualifiedName, string path, ref Rect position);
	}
}
