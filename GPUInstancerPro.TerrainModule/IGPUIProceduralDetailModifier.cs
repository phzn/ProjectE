using System;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000007 RID: 7
	public interface IGPUIProceduralDetailModifier
	{
		// Token: 0x06000076 RID: 118
		bool IsReadTerrainDetails(int terrainDetailPrototypeIndex);

		// Token: 0x06000077 RID: 119
		void Execute(GPUITerrain gpuiTerrain);
	}
}
