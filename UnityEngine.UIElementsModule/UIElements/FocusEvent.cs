using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001CC RID: 460
	public class FocusEvent : FocusEventBase<FocusEvent>
	{
		// Token: 0x06000E0D RID: 3597 RVA: 0x0003641C File Offset: 0x0003461C
		static FocusEvent()
		{
			EventBase<FocusEvent>.SetCreateFunction(() => new FocusEvent());
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00036435 File Offset: 0x00034635
		protected internal override void PreDispatch(IPanel panel)
		{
			base.PreDispatch(panel);
			base.focusController.ProcessPendingFocusChange(base.target as Focusable);
		}
	}
}
