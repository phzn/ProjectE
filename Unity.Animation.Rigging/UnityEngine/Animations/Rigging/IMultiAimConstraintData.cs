using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200001F RID: 31
	public interface IMultiAimConstraintData
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007A RID: 122
		Transform constrainedObject { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007B RID: 123
		WeightedTransformArray sourceObjects { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007C RID: 124
		bool maintainOffset { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007D RID: 125
		Vector3 aimAxis { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007E RID: 126
		Vector3 upAxis { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007F RID: 127
		int worldUpType { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000080 RID: 128
		Vector3 worldUpAxis { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000081 RID: 129
		Transform worldUpObject { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000082 RID: 130
		bool constrainedXAxis { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000083 RID: 131
		bool constrainedYAxis { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000084 RID: 132
		bool constrainedZAxis { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000085 RID: 133
		string offsetVector3Property { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000086 RID: 134
		string minLimitFloatProperty { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000087 RID: 135
		string maxLimitFloatProperty { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000088 RID: 136
		string sourceObjectsProperty { get; }
	}
}
