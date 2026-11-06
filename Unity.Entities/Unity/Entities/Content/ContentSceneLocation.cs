using System;
using Unity.Collections;
using Unity.Entities.Serialization;

namespace Unity.Entities.Content
{
	// Token: 0x02000483 RID: 1155
	[Serializable]
	internal struct ContentSceneLocation
	{
		// Token: 0x04000A0F RID: 2575
		public UnsafeUntypedWeakReferenceId SceneId;

		// Token: 0x04000A10 RID: 2576
		public int FileIndex;

		// Token: 0x04000A11 RID: 2577
		public FixedString128Bytes SceneName;
	}
}
