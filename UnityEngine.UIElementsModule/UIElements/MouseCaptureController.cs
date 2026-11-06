using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000277 RID: 631
	public static class MouseCaptureController
	{
		// Token: 0x060011DF RID: 4575 RVA: 0x00040EA4 File Offset: 0x0003F0A4
		public static bool IsMouseCaptured()
		{
			bool flag = !MouseCaptureController.m_IsMouseCapturedWarningEmitted;
			if (flag)
			{
				Debug.LogError("MouseCaptureController.IsMouseCaptured() can not be used in playmode. Please use PointerCaptureHelper.GetCapturingElement() instead.");
				MouseCaptureController.m_IsMouseCapturedWarningEmitted = true;
			}
			return false;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00040ED8 File Offset: 0x0003F0D8
		public static bool HasMouseCapture(this IEventHandler handler)
		{
			VisualElement visualElement = handler as VisualElement;
			return visualElement.HasPointerCapture(PointerId.mousePointerId);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00040EFC File Offset: 0x0003F0FC
		public static void CaptureMouse(this IEventHandler handler)
		{
			VisualElement visualElement = handler as VisualElement;
			bool flag = visualElement != null;
			if (flag)
			{
				visualElement.CapturePointer(PointerId.mousePointerId);
				visualElement.panel.ProcessPointerCapture(PointerId.mousePointerId);
			}
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00040F38 File Offset: 0x0003F138
		public static void ReleaseMouse(this IEventHandler handler)
		{
			VisualElement visualElement = handler as VisualElement;
			bool flag = visualElement != null;
			if (flag)
			{
				visualElement.ReleasePointer(PointerId.mousePointerId);
				visualElement.panel.ProcessPointerCapture(PointerId.mousePointerId);
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00040F74 File Offset: 0x0003F174
		public static void ReleaseMouse()
		{
			bool flag = !MouseCaptureController.m_ReleaseMouseWarningEmitted;
			if (flag)
			{
				Debug.LogError("MouseCaptureController.ReleaseMouse() can not be used in playmode. Please use PointerCaptureHelper.GetCapturingElement() instead.");
				MouseCaptureController.m_ReleaseMouseWarningEmitted = true;
			}
		}

		// Token: 0x040007ED RID: 2029
		private static bool m_IsMouseCapturedWarningEmitted;

		// Token: 0x040007EE RID: 2030
		private static bool m_ReleaseMouseWarningEmitted;
	}
}
