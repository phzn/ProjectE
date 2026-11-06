using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C8 RID: 456
	public class BlurEvent : FocusEventBase<BlurEvent>
	{
		// Token: 0x06000E00 RID: 3584 RVA: 0x00036365 File Offset: 0x00034565
		static BlurEvent()
		{
			EventBase<BlurEvent>.SetCreateFunction(() => new BlurEvent());
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00036380 File Offset: 0x00034580
		protected internal override void PreDispatch(IPanel panel)
		{
			base.PreDispatch(panel);
			bool flag = base.relatedTarget == null;
			if (flag)
			{
				base.focusController.ProcessPendingFocusChange(null);
			}
		}
	}
}
