using System;

namespace UnityEngine
{
	// Token: 0x02000007 RID: 7
	internal struct EventInterests
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003A2E File Offset: 0x00001C2E
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003A36 File Offset: 0x00001C36
		public bool wantsMouseMove { readonly get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003A3F File Offset: 0x00001C3F
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003A47 File Offset: 0x00001C47
		public bool wantsMouseEnterLeaveWindow { readonly get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003A50 File Offset: 0x00001C50
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003A58 File Offset: 0x00001C58
		public bool wantsLessLayoutEvents { readonly get; set; }

		// Token: 0x06000059 RID: 89 RVA: 0x00003A64 File Offset: 0x00001C64
		public bool WantsEvent(EventType type)
		{
			bool flag;
			if (type != EventType.MouseMove)
			{
				flag = type - EventType.MouseEnterWindow > 1 || this.wantsMouseEnterLeaveWindow;
			}
			else
			{
				flag = this.wantsMouseMove;
			}
			return flag;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003A9C File Offset: 0x00001C9C
		public bool WantsLayoutPass(EventType type)
		{
			bool flag = !this.wantsLessLayoutEvents;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				switch (type)
				{
				case EventType.MouseDown:
				case EventType.MouseUp:
					return this.wantsMouseMove;
				case EventType.MouseMove:
				case EventType.MouseDrag:
				case EventType.ScrollWheel:
					goto IL_006C;
				case EventType.KeyDown:
				case EventType.KeyUp:
					return GUIUtility.textFieldInput;
				case EventType.Repaint:
					break;
				default:
					if (type != EventType.ExecuteCommand)
					{
						if (type - EventType.MouseEnterWindow > 1)
						{
							goto IL_006C;
						}
						return this.wantsMouseEnterLeaveWindow;
					}
					break;
				}
				return true;
				IL_006C:
				flag2 = false;
			}
			return flag2;
		}
	}
}
