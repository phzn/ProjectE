using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021A RID: 538
	public sealed class PointerDownEvent : PointerEventBase<PointerDownEvent>
	{
		// Token: 0x06000FD9 RID: 4057 RVA: 0x0003ACB4 File Offset: 0x00038EB4
		static PointerDownEvent()
		{
			EventBase<PointerDownEvent>.SetCreateFunction(() => new PointerDownEvent());
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0003ACCD File Offset: 0x00038ECD
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0003ACDE File Offset: 0x00038EDE
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0003ACFA File Offset: 0x00038EFA
		public PointerDownEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0003AD0C File Offset: 0x00038F0C
		protected internal override void PostDispatch(IPanel panel)
		{
			bool flag = !base.isDefaultPrevented;
			if (flag)
			{
				bool flag2 = panel.ShouldSendCompatibilityMouseEvents(this);
				if (flag2)
				{
					using (MouseDownEvent pooled = MouseDownEvent.GetPooled(this))
					{
						pooled.target = base.target;
						pooled.target.SendEvent(pooled);
					}
				}
			}
			else
			{
				panel.PreventCompatibilityMouseEvents(base.pointerId);
			}
			base.PostDispatch(panel);
		}
	}
}
