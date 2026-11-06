using System;

namespace UnityEngine.VFX
{
	// Token: 0x02000009 RID: 9
	[Flags]
	internal enum VFXUpdateMode
	{
		// Token: 0x040000E0 RID: 224
		FixedDeltaTime = 0,
		// Token: 0x040000E1 RID: 225
		DeltaTime = 1,
		// Token: 0x040000E2 RID: 226
		IgnoreTimeScale = 2,
		// Token: 0x040000E3 RID: 227
		ExactFixedTimeStep = 4,
		// Token: 0x040000E4 RID: 228
		DeltaTimeAndIgnoreTimeScale = 3,
		// Token: 0x040000E5 RID: 229
		FixedDeltaAndExactTime = 4,
		// Token: 0x040000E6 RID: 230
		FixedDeltaAndExactTimeAndIgnoreTimeScale = 6
	}
}
