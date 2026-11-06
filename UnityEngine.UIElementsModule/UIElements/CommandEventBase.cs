using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200019A RID: 410
	[EventCategory(EventCategory.Command)]
	public abstract class CommandEventBase<T> : EventBase<T>, ICommandEvent where T : CommandEventBase<T>, new()
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0003204C File Offset: 0x0003024C
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x0003208B File Offset: 0x0003028B
		public string commandName
		{
			get
			{
				bool flag = this.m_CommandName == null && base.imguiEvent != null;
				string text;
				if (flag)
				{
					text = base.imguiEvent.commandName;
				}
				else
				{
					text = this.m_CommandName;
				}
				return text;
			}
			protected set
			{
				this.m_CommandName = value;
			}
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00032095 File Offset: 0x00030295
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x000320A6 File Offset: 0x000302A6
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			this.commandName = null;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x000320BC File Offset: 0x000302BC
		public static T GetPooled(Event systemEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.imguiEvent = systemEvent;
			return pooled;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000320E4 File Offset: 0x000302E4
		public static T GetPooled(string commandName)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.commandName = commandName;
			return pooled;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0003210A File Offset: 0x0003030A
		protected CommandEventBase()
		{
			this.LocalInit();
		}

		// Token: 0x040005FB RID: 1531
		private string m_CommandName;
	}
}
