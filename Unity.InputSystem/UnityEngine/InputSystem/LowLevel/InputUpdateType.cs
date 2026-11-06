using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F3 RID: 243
	[Flags]
	public enum InputUpdateType
	{
		// Token: 0x040005EE RID: 1518
		None = 0,
		// Token: 0x040005EF RID: 1519
		Dynamic = 1,
		// Token: 0x040005F0 RID: 1520
		Fixed = 2,
		// Token: 0x040005F1 RID: 1521
		BeforeRender = 4,
		// Token: 0x040005F2 RID: 1522
		Editor = 8,
		// Token: 0x040005F3 RID: 1523
		Manual = 16,
		// Token: 0x040005F4 RID: 1524
		Default = 11
	}
}
