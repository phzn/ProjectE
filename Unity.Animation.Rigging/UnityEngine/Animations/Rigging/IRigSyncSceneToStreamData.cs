using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000032 RID: 50
	internal interface IRigSyncSceneToStreamData
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D9 RID: 217
		Transform[] syncableTransforms { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000DA RID: 218
		SyncableProperties[] syncableProperties { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DB RID: 219
		bool[] rigStates { get; }
	}
}
