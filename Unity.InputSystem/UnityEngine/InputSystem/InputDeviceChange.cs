using System;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000039 RID: 57
	public enum InputDeviceChange
	{
		// Token: 0x0400017A RID: 378
		Added,
		// Token: 0x0400017B RID: 379
		Removed,
		// Token: 0x0400017C RID: 380
		Disconnected,
		// Token: 0x0400017D RID: 381
		Reconnected,
		// Token: 0x0400017E RID: 382
		Enabled,
		// Token: 0x0400017F RID: 383
		Disabled,
		// Token: 0x04000180 RID: 384
		UsageChanged,
		// Token: 0x04000181 RID: 385
		ConfigurationChanged,
		// Token: 0x04000182 RID: 386
		SoftReset,
		// Token: 0x04000183 RID: 387
		HardReset,
		// Token: 0x04000184 RID: 388
		[Obsolete("Destroyed enum has been deprecated.")]
		Destroyed
	}
}
