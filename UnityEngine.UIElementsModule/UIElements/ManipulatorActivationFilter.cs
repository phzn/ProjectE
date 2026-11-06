using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000273 RID: 627
	public struct ManipulatorActivationFilter : IEquatable<ManipulatorActivationFilter>
	{
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00040B2A File Offset: 0x0003ED2A
		// (set) Token: 0x060011C9 RID: 4553 RVA: 0x00040B32 File Offset: 0x0003ED32
		public MouseButton button { readonly get; set; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x00040B3B File Offset: 0x0003ED3B
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x00040B43 File Offset: 0x0003ED43
		public EventModifiers modifiers { readonly get; set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00040B4C File Offset: 0x0003ED4C
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x00040B54 File Offset: 0x0003ED54
		public int clickCount { readonly get; set; }

		// Token: 0x060011CE RID: 4558 RVA: 0x00040B60 File Offset: 0x0003ED60
		public override bool Equals(object obj)
		{
			return obj is ManipulatorActivationFilter && this.Equals((ManipulatorActivationFilter)obj);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00040B8C File Offset: 0x0003ED8C
		public bool Equals(ManipulatorActivationFilter other)
		{
			return this.button == other.button && this.modifiers == other.modifiers && this.clickCount == other.clickCount;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00040BD0 File Offset: 0x0003EDD0
		public override int GetHashCode()
		{
			int num = 390957112;
			num = num * -1521134295 + this.button.GetHashCode();
			num = num * -1521134295 + this.modifiers.GetHashCode();
			return num * -1521134295 + this.clickCount.GetHashCode();
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00040C3C File Offset: 0x0003EE3C
		public bool Matches(IMouseEvent e)
		{
			bool flag = e == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.clickCount == 0 || e.clickCount >= this.clickCount;
				flag2 = this.button == (MouseButton)e.button && this.HasModifiers(e) && flag3;
			}
			return flag2;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00040C94 File Offset: 0x0003EE94
		private bool HasModifiers(IMouseEvent e)
		{
			bool flag = e == null;
			return !flag && this.MatchModifiers(e.altKey, e.ctrlKey, e.shiftKey, e.commandKey);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00040CD0 File Offset: 0x0003EED0
		public bool Matches(IPointerEvent e)
		{
			bool flag = e == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.clickCount == 0 || e.clickCount >= this.clickCount;
				flag2 = this.button == (MouseButton)e.button && this.HasModifiers(e) && flag3;
			}
			return flag2;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00040D28 File Offset: 0x0003EF28
		private bool HasModifiers(IPointerEvent e)
		{
			bool flag = e == null;
			return !flag && this.MatchModifiers(e.altKey, e.ctrlKey, e.shiftKey, e.commandKey);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00040D64 File Offset: 0x0003EF64
		private bool MatchModifiers(bool alt, bool ctrl, bool shift, bool command)
		{
			bool flag = ((this.modifiers & EventModifiers.Alt) != EventModifiers.None && !alt) || ((this.modifiers & EventModifiers.Alt) == EventModifiers.None && alt);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = ((this.modifiers & EventModifiers.Control) != EventModifiers.None && !ctrl) || ((this.modifiers & EventModifiers.Control) == EventModifiers.None && ctrl);
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					bool flag4 = ((this.modifiers & EventModifiers.Shift) != EventModifiers.None && !shift) || ((this.modifiers & EventModifiers.Shift) == EventModifiers.None && shift);
					flag2 = !flag4 && ((this.modifiers & EventModifiers.Command) == EventModifiers.None || command) && ((this.modifiers & EventModifiers.Command) != EventModifiers.None || !command);
				}
			}
			return flag2;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00040E10 File Offset: 0x0003F010
		public static bool operator ==(ManipulatorActivationFilter filter1, ManipulatorActivationFilter filter2)
		{
			return filter1.Equals(filter2);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00040E2C File Offset: 0x0003F02C
		public static bool operator !=(ManipulatorActivationFilter filter1, ManipulatorActivationFilter filter2)
		{
			return !(filter1 == filter2);
		}
	}
}
