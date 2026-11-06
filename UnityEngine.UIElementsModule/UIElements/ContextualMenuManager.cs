using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000052 RID: 82
	public abstract class ContextualMenuManager
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000D609 File Offset: 0x0000B809
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000D611 File Offset: 0x0000B811
		internal bool displayMenuHandledOSX { get; set; }

		// Token: 0x06000382 RID: 898
		public abstract void DisplayMenuIfEventMatches(EventBase evt, IEventHandler eventHandler);

		// Token: 0x06000383 RID: 899 RVA: 0x0000D61C File Offset: 0x0000B81C
		public void DisplayMenu(EventBase triggerEvent, IEventHandler target)
		{
			DropdownMenu dropdownMenu = new DropdownMenu();
			int num;
			int button;
			using (ContextualMenuPopulateEvent pooled = ContextualMenuPopulateEvent.GetPooled(triggerEvent, dropdownMenu, target, this))
			{
				IPointerEvent pointerEvent = triggerEvent as IPointerEvent;
				num = ((pointerEvent != null) ? pointerEvent.pointerId : PointerId.mousePointerId);
				button = pooled.button;
				if (target != null)
				{
					target.SendEvent(pooled);
				}
			}
			bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
			if (flag)
			{
				this.displayMenuHandledOSX = true;
				bool flag2 = button >= 0;
				if (flag2)
				{
					PointerDeviceState.ReleaseButton(num, button);
				}
			}
		}

		// Token: 0x06000384 RID: 900
		protected internal abstract void DoDisplayMenu(DropdownMenu menu, EventBase triggerEvent);
	}
}
