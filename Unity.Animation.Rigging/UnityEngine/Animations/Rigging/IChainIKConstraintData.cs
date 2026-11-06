using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200000C RID: 12
	public interface IChainIKConstraintData
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002A RID: 42
		Transform root { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43
		Transform tip { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002C RID: 44
		Transform target { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002D RID: 45
		int maxIterations { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46
		float tolerance { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002F RID: 47
		bool maintainTargetPositionOffset { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000030 RID: 48
		bool maintainTargetRotationOffset { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49
		string chainRotationWeightFloatProperty { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000032 RID: 50
		string tipRotationWeightFloatProperty { get; }
	}
}
