using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200003A RID: 58
	public interface ITwistCorrectionData
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000111 RID: 273
		Transform source { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000112 RID: 274
		WeightedTransformArray twistNodes { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000113 RID: 275
		Vector3 twistAxis { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000114 RID: 276
		string twistNodesProperty { get; }
	}
}
