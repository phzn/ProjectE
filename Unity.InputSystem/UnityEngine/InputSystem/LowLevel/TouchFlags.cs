using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000DB RID: 219
	[Flags]
	internal enum TouchFlags : byte
	{
		// Token: 0x0400057C RID: 1404
		IndirectTouch = 1,
		// Token: 0x0400057D RID: 1405
		PrimaryTouch = 8,
		// Token: 0x0400057E RID: 1406
		TapPress = 16,
		// Token: 0x0400057F RID: 1407
		TapRelease = 32,
		// Token: 0x04000580 RID: 1408
		OrphanedPrimaryTouch = 64,
		// Token: 0x04000581 RID: 1409
		BeganInSameFrame = 128
	}
}
