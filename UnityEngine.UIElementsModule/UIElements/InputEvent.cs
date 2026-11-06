using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001D2 RID: 466
	public class InputEvent : EventBase<InputEvent>
	{
		// Token: 0x06000E1C RID: 3612 RVA: 0x000369E5 File Offset: 0x00034BE5
		static InputEvent()
		{
			EventBase<InputEvent>.SetCreateFunction(() => new InputEvent());
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x000369FE File Offset: 0x00034BFE
		// (set) Token: 0x06000E1E RID: 3614 RVA: 0x00036A06 File Offset: 0x00034C06
		public string previousData { get; protected set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00036A0F File Offset: 0x00034C0F
		// (set) Token: 0x06000E20 RID: 3616 RVA: 0x00036A17 File Offset: 0x00034C17
		public string newData { get; protected set; }

		// Token: 0x06000E21 RID: 3617 RVA: 0x00036A20 File Offset: 0x00034C20
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00036A31 File Offset: 0x00034C31
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.previousData = null;
			this.newData = null;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00036A4C File Offset: 0x00034C4C
		public static InputEvent GetPooled(string previousData, string newData)
		{
			InputEvent pooled = EventBase<InputEvent>.GetPooled();
			pooled.previousData = previousData;
			pooled.newData = newData;
			return pooled;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00036A75 File Offset: 0x00034C75
		public InputEvent()
		{
			this.LocalInit();
		}
	}
}
