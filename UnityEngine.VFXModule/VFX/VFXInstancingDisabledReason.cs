using System;
using System.ComponentModel;

namespace UnityEngine.VFX
{
	// Token: 0x0200000C RID: 12
	[Flags]
	internal enum VFXInstancingDisabledReason
	{
		// Token: 0x040000F1 RID: 241
		None = 0,
		// Token: 0x040000F2 RID: 242
		[Description("A system is using indirect draw.")]
		IndirectDraw = 1,
		// Token: 0x040000F3 RID: 243
		[Description("The effect is using output events.")]
		OutputEvent = 2,
		// Token: 0x040000F4 RID: 244
		[Description("The effect is using GPU events.")]
		GPUEvent = 4,
		// Token: 0x040000F5 RID: 245
		[Description("An Initialize node has Bounds Mode set to 'Automatic'.")]
		AutomaticBounds = 8,
		// Token: 0x040000F6 RID: 246
		[Description("The effect contains a mesh output.")]
		MeshOutput = 16,
		// Token: 0x040000F7 RID: 247
		[Description("The effect has exposed texture, mesh or graphics buffer properties.")]
		ExposedObject = 32,
		// Token: 0x040000F8 RID: 248
		[Description("Unknown reason.")]
		Unknown = -1
	}
}
