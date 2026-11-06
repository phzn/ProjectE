using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200000C RID: 12
	public abstract class GPUIProceduralDetailObject : ScriptableObject
	{
		// Token: 0x060000BF RID: 191
		public abstract void Execute(GPUITerrain gpuiTerrain, int detailLayer);

		// Token: 0x04000077 RID: 119
		[SerializeField]
		public bool isReadTerrainDetails = true;
	}
}
