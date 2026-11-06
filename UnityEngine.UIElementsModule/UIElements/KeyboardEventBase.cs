using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001D6 RID: 470
	[EventCategory(EventCategory.Keyboard)]
	public abstract class KeyboardEventBase<T> : EventBase<T>, IKeyboardEvent where T : KeyboardEventBase<T>, new()
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x00036B81 File Offset: 0x00034D81
		// (set) Token: 0x06000E34 RID: 3636 RVA: 0x00036B89 File Offset: 0x00034D89
		public EventModifiers modifiers { get; protected set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00036B92 File Offset: 0x00034D92
		// (set) Token: 0x06000E36 RID: 3638 RVA: 0x00036B9A File Offset: 0x00034D9A
		public char character { get; protected set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00036BA3 File Offset: 0x00034DA3
		// (set) Token: 0x06000E38 RID: 3640 RVA: 0x00036BAB File Offset: 0x00034DAB
		public KeyCode keyCode { get; protected set; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00036BB4 File Offset: 0x00034DB4
		public bool shiftKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Shift) > EventModifiers.None;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00036BD4 File Offset: 0x00034DD4
		public bool ctrlKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Control) > EventModifiers.None;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00036BF4 File Offset: 0x00034DF4
		public bool commandKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Command) > EventModifiers.None;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00036C14 File Offset: 0x00034E14
		public bool altKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Alt) > EventModifiers.None;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00036C34 File Offset: 0x00034E34
		internal bool functionKey
		{
			get
			{
				return (this.modifiers & EventModifiers.FunctionKey) > EventModifiers.None;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00036C54 File Offset: 0x00034E54
		public bool actionKey
		{
			get
			{
				bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
				bool flag2;
				if (flag)
				{
					flag2 = this.commandKey;
				}
				else
				{
					flag2 = this.ctrlKey;
				}
				return flag2;
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00036C8D File Offset: 0x00034E8D
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00036C9E File Offset: 0x00034E9E
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			this.modifiers = EventModifiers.None;
			this.character = '\0';
			this.keyCode = KeyCode.None;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00036CC4 File Offset: 0x00034EC4
		public static T GetPooled(char c, KeyCode keyCode, EventModifiers modifiers)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.modifiers = modifiers;
			pooled.character = c;
			pooled.keyCode = keyCode;
			return pooled;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00036D04 File Offset: 0x00034F04
		public static T GetPooled(Event systemEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.imguiEvent = systemEvent;
			bool flag = systemEvent != null;
			if (flag)
			{
				pooled.modifiers = systemEvent.modifiers;
				pooled.character = systemEvent.character;
				pooled.keyCode = systemEvent.keyCode;
			}
			return pooled;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00036D6A File Offset: 0x00034F6A
		protected KeyboardEventBase()
		{
			this.LocalInit();
		}
	}
}
