using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C6 RID: 454
	public class FocusOutEvent : FocusEventBase<FocusOutEvent>
	{
		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003630C File Offset: 0x0003450C
		static FocusOutEvent()
		{
			EventBase<FocusOutEvent>.SetCreateFunction(() => new FocusOutEvent());
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00036325 File Offset: 0x00034525
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00036341 File Offset: 0x00034541
		public FocusOutEvent()
		{
			this.LocalInit();
		}
	}
}
