using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200003D RID: 61
	public interface ITwoBoneIKConstraintData
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600011C RID: 284
		Transform root { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600011D RID: 285
		Transform mid { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600011E RID: 286
		Transform tip { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600011F RID: 287
		Transform target { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000120 RID: 288
		Transform hint { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000121 RID: 289
		bool maintainTargetPositionOffset { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000122 RID: 290
		bool maintainTargetRotationOffset { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000123 RID: 291
		string targetPositionWeightFloatProperty { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000124 RID: 292
		string targetRotationWeightFloatProperty { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000125 RID: 293
		string hintWeightFloatProperty { get; }
	}
}
