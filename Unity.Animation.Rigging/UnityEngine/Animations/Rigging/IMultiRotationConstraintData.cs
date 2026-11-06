using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200002B RID: 43
	public interface IMultiRotationConstraintData
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000BC RID: 188
		Transform constrainedObject { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000BD RID: 189
		WeightedTransformArray sourceObjects { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000BE RID: 190
		bool maintainOffset { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000BF RID: 191
		string offsetVector3Property { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C0 RID: 192
		string sourceObjectsProperty { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C1 RID: 193
		bool constrainedXAxis { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C2 RID: 194
		bool constrainedYAxis { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000C3 RID: 195
		bool constrainedZAxis { get; }
	}
}
