using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000050 RID: 80
	public enum UpscalingFilterSelection
	{
		// Token: 0x040001FA RID: 506
		[InspectorName("Automatic")]
		[Tooltip("Unity selects a filtering option automatically based on the Render Scale value and the current screen resolution.")]
		Auto,
		// Token: 0x040001FB RID: 507
		[InspectorName("Bilinear")]
		Linear,
		// Token: 0x040001FC RID: 508
		[InspectorName("Nearest-Neighbor")]
		Point,
		// Token: 0x040001FD RID: 509
		[InspectorName("FidelityFX Super Resolution 1.0")]
		[Tooltip("If the target device does not support Unity shader model 4.5, Unity falls back to the Automatic option.")]
		FSR
	}
}
