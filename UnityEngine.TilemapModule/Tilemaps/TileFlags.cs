using System;

namespace UnityEngine.Tilemaps
{
	// Token: 0x0200000F RID: 15
	[Flags]
	public enum TileFlags
	{
		// Token: 0x04000035 RID: 53
		None = 0,
		// Token: 0x04000036 RID: 54
		LockColor = 1,
		// Token: 0x04000037 RID: 55
		LockTransform = 2,
		// Token: 0x04000038 RID: 56
		InstantiateGameObjectRuntimeOnly = 4,
		// Token: 0x04000039 RID: 57
		KeepGameObjectRuntimeOnly = 8,
		// Token: 0x0400003A RID: 58
		LockAll = 3
	}
}
