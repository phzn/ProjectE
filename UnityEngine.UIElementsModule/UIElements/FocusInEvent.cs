using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001CA RID: 458
	public class FocusInEvent : FocusEventBase<FocusInEvent>
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x000363CE File Offset: 0x000345CE
		static FocusInEvent()
		{
			EventBase<FocusInEvent>.SetCreateFunction(() => new FocusInEvent());
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000363E7 File Offset: 0x000345E7
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000363F8 File Offset: 0x000345F8
		public FocusInEvent()
		{
			this.LocalInit();
		}
	}
}
