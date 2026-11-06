using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000220 RID: 544
	public sealed class PointerUpEvent : PointerEventBase<PointerUpEvent>
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003AFC4 File Offset: 0x000391C4
		static PointerUpEvent()
		{
			EventBase<PointerUpEvent>.SetCreateFunction(() => new PointerUpEvent());
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0003AFDD File Offset: 0x000391DD
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003ACDE File Offset: 0x00038EDE
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003AFEE File Offset: 0x000391EE
		public PointerUpEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003B000 File Offset: 0x00039200
		protected internal override void PostDispatch(IPanel panel)
		{
			bool flag = PointerType.IsDirectManipulationDevice(base.pointerType);
			if (flag)
			{
				panel.ReleasePointer(base.pointerId);
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					baseVisualElementPanel.ClearCachedElementUnderPointer(base.pointerId, this);
				}
			}
			bool flag2 = panel.ShouldSendCompatibilityMouseEvents(this);
			if (flag2)
			{
				using (MouseUpEvent pooled = MouseUpEvent.GetPooled(this))
				{
					pooled.target = base.target;
					pooled.target.SendEvent(pooled);
				}
			}
			base.PostDispatch(panel);
			panel.ActivateCompatibilityMouseEvents(base.pointerId);
		}
	}
}
