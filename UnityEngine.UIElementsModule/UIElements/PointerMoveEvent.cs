using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021C RID: 540
	[EventCategory(EventCategory.PointerMove)]
	public sealed class PointerMoveEvent : PointerEventBase<PointerMoveEvent>
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x0003ADA3 File Offset: 0x00038FA3
		static PointerMoveEvent()
		{
			EventBase<PointerMoveEvent>.SetCreateFunction(() => new PointerMoveEvent());
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0003ADBC File Offset: 0x00038FBC
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0003ADC4 File Offset: 0x00038FC4
		internal bool isHandledByDraggable { get; set; }

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0003ADCD File Offset: 0x00038FCD
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0003ADDE File Offset: 0x00038FDE
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
			this.isHandledByDraggable = false;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0003AE01 File Offset: 0x00039001
		public PointerMoveEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0003AE14 File Offset: 0x00039014
		protected internal override void PostDispatch(IPanel panel)
		{
			bool flag = panel.ShouldSendCompatibilityMouseEvents(this);
			if (flag)
			{
				bool flag2 = base.imguiEvent != null && base.imguiEvent.rawType == EventType.MouseDown;
				if (flag2)
				{
					using (MouseDownEvent pooled = MouseDownEvent.GetPooled(this))
					{
						pooled.target = base.target;
						pooled.target.SendEvent(pooled);
					}
				}
				else
				{
					bool flag3 = base.imguiEvent != null && base.imguiEvent.rawType == EventType.MouseUp;
					if (flag3)
					{
						using (MouseUpEvent pooled2 = MouseUpEvent.GetPooled(this))
						{
							pooled2.target = base.target;
							pooled2.target.SendEvent(pooled2);
						}
					}
					else
					{
						using (MouseMoveEvent pooled3 = MouseMoveEvent.GetPooled(this))
						{
							pooled3.target = base.target;
							pooled3.target.SendEvent(pooled3);
						}
					}
				}
			}
			base.PostDispatch(panel);
		}
	}
}
