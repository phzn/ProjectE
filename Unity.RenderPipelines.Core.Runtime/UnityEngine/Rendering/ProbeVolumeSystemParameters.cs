using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000086 RID: 134
	public struct ProbeVolumeSystemParameters
	{
		// Token: 0x0400027F RID: 639
		public ProbeVolumeTextureMemoryBudget memoryBudget;

		// Token: 0x04000280 RID: 640
		public ProbeVolumeBlendingTextureMemoryBudget blendingMemoryBudget;

		// Token: 0x04000281 RID: 641
		public Mesh probeDebugMesh;

		// Token: 0x04000282 RID: 642
		public Shader probeDebugShader;

		// Token: 0x04000283 RID: 643
		public Mesh offsetDebugMesh;

		// Token: 0x04000284 RID: 644
		public Shader offsetDebugShader;

		// Token: 0x04000285 RID: 645
		public ComputeShader scenarioBlendingShader;

		// Token: 0x04000286 RID: 646
		public ProbeVolumeSceneData sceneData;

		// Token: 0x04000287 RID: 647
		public ProbeVolumeSHBands shBands;

		// Token: 0x04000288 RID: 648
		public bool supportsRuntimeDebug;

		// Token: 0x04000289 RID: 649
		public bool supportStreaming;
	}
}
