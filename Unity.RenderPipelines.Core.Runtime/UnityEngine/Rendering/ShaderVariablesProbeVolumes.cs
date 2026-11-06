using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200009A RID: 154
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Library\\PackageCache\\com.unity.render-pipelines.core@14.0.12\\Runtime\\Lighting\\ProbeVolume\\ShaderVariablesProbeVolumes.cs", needAccessors = false, generateCBuffer = true, constantRegister = 5)]
	internal struct ShaderVariablesProbeVolumes
	{
		// Token: 0x04000353 RID: 851
		public Vector4 _PoolDim_CellInMeters;

		// Token: 0x04000354 RID: 852
		public Vector4 _MinCellPos_Noise;

		// Token: 0x04000355 RID: 853
		public Vector4 _IndicesDim_IndexChunkSize;

		// Token: 0x04000356 RID: 854
		public Vector4 _Biases_CellInMinBrick_MinBrickSize;

		// Token: 0x04000357 RID: 855
		public Vector4 _LeakReductionParams;

		// Token: 0x04000358 RID: 856
		public Vector4 _Weight_MinLoadedCell;

		// Token: 0x04000359 RID: 857
		public Vector4 _MaxLoadedCell_FrameIndex;

		// Token: 0x0400035A RID: 858
		public Vector4 _NormalizationClamp_Padding12;
	}
}
