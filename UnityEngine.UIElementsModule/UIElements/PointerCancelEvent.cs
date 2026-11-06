using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000222 RID: 546
	public sealed class PointerCancelEvent : PointerEventBase<PointerCancelEvent>
	{
		// Token: 0x06000FFA RID: 4090 RVA: 0x0003B0BB File Offset: 0x000392BB
		static PointerCancelEvent()
		{
			EventBase<PointerCancelEvent>.SetCreateFunction(() => new PointerCancelEvent());
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0003B0D4 File Offset: 0x000392D4
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0003B0E5 File Offset: 0x000392E5
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.SkipDisabledElements;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0003B101 File Offset: 0x00039301
		public PointerCancelEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0003B114 File Offset: 0x00039314
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
					base.target.SendEvent(pooled);
				}
			}
			base.PostDispatch(panel);
			panel.ActivateCompatibilityMouseEvents(base.pointerId);
		}
	}
}
