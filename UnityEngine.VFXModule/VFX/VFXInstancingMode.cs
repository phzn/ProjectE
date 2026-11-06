using System;

namespace UnityEngine.VFX
{
	// Token: 0x0200000B RID: 11
	internal enum VFXInstancingMode
	{
		// Token: 0x040000ED RID: 237
		Disabled = -1,
		// Token: 0x040000EE RID: 238
		[InspectorName("Automatic batch capacity")]
		Auto,
		// Token: 0x040000EF RID: 239
		[InspectorName("Custom batch capacity")]
		Custom
	}
}
