using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.UIElements
{
	// Token: 0x02000287 RID: 647
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/UIElementsRuntimeUtilityNative.h")]
	[VisibleToOtherModules(new string[] { "Unity.UIElements" })]
	internal static class UIElementsRuntimeUtilityNative
	{
		// Token: 0x06001234 RID: 4660 RVA: 0x00041509 File Offset: 0x0003F709
		[RequiredByNativeCode]
		public static void RepaintOverlayPanels()
		{
			Action repaintOverlayPanelsCallback = UIElementsRuntimeUtilityNative.RepaintOverlayPanelsCallback;
			if (repaintOverlayPanelsCallback != null)
			{
				repaintOverlayPanelsCallback();
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0004151D File Offset: 0x0003F71D
		[RequiredByNativeCode]
		public static void UpdateRuntimePanels()
		{
			Action updateRuntimePanelsCallback = UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback;
			if (updateRuntimePanelsCallback != null)
			{
				updateRuntimePanelsCallback();
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00041531 File Offset: 0x0003F731
		[RequiredByNativeCode]
		public static void RepaintOffscreenPanels()
		{
			Action repaintOffscreenPanelsCallback = UIElementsRuntimeUtilityNative.RepaintOffscreenPanelsCallback;
			if (repaintOffscreenPanelsCallback != null)
			{
				repaintOffscreenPanelsCallback();
			}
		}

		// Token: 0x06001237 RID: 4663
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RegisterPlayerloopCallback();

		// Token: 0x06001238 RID: 4664
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnregisterPlayerloopCallback();

		// Token: 0x06001239 RID: 4665
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VisualElementCreation();

		// Token: 0x04000838 RID: 2104
		internal static Action RepaintOverlayPanelsCallback;

		// Token: 0x04000839 RID: 2105
		internal static Action UpdateRuntimePanelsCallback;

		// Token: 0x0400083A RID: 2106
		internal static Action RepaintOffscreenPanelsCallback;
	}
}
