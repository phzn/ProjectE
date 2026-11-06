using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000196 RID: 406
	[EventCategory(EventCategory.ChangeValue)]
	public class ChangeEvent<T> : EventBase<ChangeEvent<T>>, IChangeEvent
	{
		// Token: 0x06000C82 RID: 3202 RVA: 0x00031E55 File Offset: 0x00030055
		static ChangeEvent()
		{
			EventBase<ChangeEvent<T>>.SetCreateFunction(() => new ChangeEvent<T>());
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00031E6E File Offset: 0x0003006E
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x00031E76 File Offset: 0x00030076
		public T previousValue { get; protected set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00031E7F File Offset: 0x0003007F
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x00031E87 File Offset: 0x00030087
		public T newValue { get; protected set; }

		// Token: 0x06000C87 RID: 3207 RVA: 0x00031E90 File Offset: 0x00030090
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00031EA4 File Offset: 0x000300A4
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.previousValue = default(T);
			this.newValue = default(T);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00031EDC File Offset: 0x000300DC
		public static ChangeEvent<T> GetPooled(T previousValue, T newValue)
		{
			ChangeEvent<T> pooled = EventBase<ChangeEvent<T>>.GetPooled();
			pooled.previousValue = previousValue;
			pooled.newValue = newValue;
			return pooled;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00031F05 File Offset: 0x00030105
		public ChangeEvent()
		{
			this.LocalInit();
		}
	}
}
