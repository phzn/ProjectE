using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E1 RID: 481
	public interface IMouseEvent
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E6D RID: 3693
		EventModifiers modifiers { get; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E6E RID: 3694
		Vector2 mousePosition { get; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E6F RID: 3695
		Vector2 localMousePosition { get; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E70 RID: 3696
		Vector2 mouseDelta { get; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E71 RID: 3697
		int clickCount { get; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E72 RID: 3698
		int button { get; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E73 RID: 3699
		int pressedButtons { get; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E74 RID: 3700
		bool shiftKey { get; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E75 RID: 3701
		bool ctrlKey { get; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E76 RID: 3702
		bool commandKey { get; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E77 RID: 3703
		bool altKey { get; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E78 RID: 3704
		bool actionKey { get; }
	}
}
