using System;

namespace UnityEngine.AI
{
	// Token: 0x0200001B RID: 27
	[Flags]
	public enum NavMeshBuildDebugFlags
	{
		// Token: 0x04000046 RID: 70
		None = 0,
		// Token: 0x04000047 RID: 71
		InputGeometry = 1,
		// Token: 0x04000048 RID: 72
		Voxels = 2,
		// Token: 0x04000049 RID: 73
		Regions = 4,
		// Token: 0x0400004A RID: 74
		RawContours = 8,
		// Token: 0x0400004B RID: 75
		SimplifiedContours = 16,
		// Token: 0x0400004C RID: 76
		PolygonMeshes = 32,
		// Token: 0x0400004D RID: 77
		PolygonMeshesDetail = 64,
		// Token: 0x0400004E RID: 78
		All = 127
	}
}
