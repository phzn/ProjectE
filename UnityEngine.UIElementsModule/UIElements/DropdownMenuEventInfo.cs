using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000179 RID: 377
	public class DropdownMenuEventInfo
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00030D92 File Offset: 0x0002EF92
		public EventModifiers modifiers { get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00030D9A File Offset: 0x0002EF9A
		public Vector2 mousePosition { get; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00030DA2 File Offset: 0x0002EFA2
		public Vector2 localMousePosition { get; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00030DAA File Offset: 0x0002EFAA
		private char character { get; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00030DB2 File Offset: 0x0002EFB2
		private KeyCode keyCode { get; }

		// Token: 0x06000C24 RID: 3108 RVA: 0x00030DBC File Offset: 0x0002EFBC
		public DropdownMenuEventInfo(EventBase e)
		{
			IMouseEvent mouseEvent = e as IMouseEvent;
			bool flag = mouseEvent != null;
			if (flag)
			{
				this.mousePosition = mouseEvent.mousePosition;
				this.localMousePosition = mouseEvent.localMousePosition;
				this.modifiers = mouseEvent.modifiers;
				this.character = '\0';
				this.keyCode = KeyCode.None;
			}
			else
			{
				IKeyboardEvent keyboardEvent = e as IKeyboardEvent;
				bool flag2 = keyboardEvent != null;
				if (flag2)
				{
					this.character = keyboardEvent.character;
					this.keyCode = keyboardEvent.keyCode;
					this.modifiers = keyboardEvent.modifiers;
					this.mousePosition = Vector2.zero;
					this.localMousePosition = Vector2.zero;
				}
			}
		}
	}
}
