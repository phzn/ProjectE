using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200029D RID: 669
	public static class PointerCaptureHelper
	{
		// Token: 0x0600132E RID: 4910 RVA: 0x00043058 File Offset: 0x00041258
		private static PointerDispatchState GetStateFor(IEventHandler handler)
		{
			VisualElement visualElement = handler as VisualElement;
			PointerDispatchState pointerDispatchState;
			if (visualElement == null)
			{
				pointerDispatchState = null;
			}
			else
			{
				IPanel panel = visualElement.panel;
				if (panel == null)
				{
					pointerDispatchState = null;
				}
				else
				{
					EventDispatcher dispatcher = panel.dispatcher;
					pointerDispatchState = ((dispatcher != null) ? dispatcher.pointerState : null);
				}
			}
			return pointerDispatchState;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00043098 File Offset: 0x00041298
		public static bool HasPointerCapture(this IEventHandler handler, int pointerId)
		{
			PointerDispatchState stateFor = PointerCaptureHelper.GetStateFor(handler);
			return stateFor != null && stateFor.HasPointerCapture(handler, pointerId);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000430BE File Offset: 0x000412BE
		public static void CapturePointer(this IEventHandler handler, int pointerId)
		{
			PointerDispatchState stateFor = PointerCaptureHelper.GetStateFor(handler);
			if (stateFor != null)
			{
				stateFor.CapturePointer(handler, pointerId);
			}
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000430D5 File Offset: 0x000412D5
		public static void ReleasePointer(this IEventHandler handler, int pointerId)
		{
			PointerDispatchState stateFor = PointerCaptureHelper.GetStateFor(handler);
			if (stateFor != null)
			{
				stateFor.ReleasePointer(handler, pointerId);
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x000430EC File Offset: 0x000412EC
		public static IEventHandler GetCapturingElement(this IPanel panel, int pointerId)
		{
			IEventHandler eventHandler;
			if (panel == null)
			{
				eventHandler = null;
			}
			else
			{
				EventDispatcher dispatcher = panel.dispatcher;
				eventHandler = ((dispatcher != null) ? dispatcher.pointerState.GetCapturingElement(pointerId) : null);
			}
			return eventHandler;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0004311C File Offset: 0x0004131C
		public static void ReleasePointer(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.ReleasePointer(pointerId);
				}
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0004313C File Offset: 0x0004133C
		internal static void ActivateCompatibilityMouseEvents(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.ActivateCompatibilityMouseEvents(pointerId);
				}
			}
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0004315C File Offset: 0x0004135C
		internal static void PreventCompatibilityMouseEvents(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.PreventCompatibilityMouseEvents(pointerId);
				}
			}
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0004317C File Offset: 0x0004137C
		internal static bool ShouldSendCompatibilityMouseEvents(this IPanel panel, IPointerEvent evt)
		{
			bool? flag;
			if (panel == null)
			{
				flag = null;
			}
			else
			{
				EventDispatcher dispatcher = panel.dispatcher;
				flag = ((dispatcher != null) ? new bool?(dispatcher.pointerState.ShouldSendCompatibilityMouseEvents(evt)) : null);
			}
			return flag ?? true;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000431D5 File Offset: 0x000413D5
		internal static void ProcessPointerCapture(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.ProcessPointerCapture(pointerId);
				}
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000431F5 File Offset: 0x000413F5
		internal static void ResetPointerDispatchState(this IPanel panel)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.Reset();
				}
			}
		}
	}
}
