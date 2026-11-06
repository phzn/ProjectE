using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000025 RID: 37
	public interface IMultiPositionConstraintData
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A2 RID: 162
		Transform constrainedObject { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A3 RID: 163
		WeightedTransformArray sourceObjects { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A4 RID: 164
		bool maintainOffset { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A5 RID: 165
		string offsetVector3Property { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A6 RID: 166
		string sourceObjectsProperty { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A7 RID: 167
		bool constrainedXAxis { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A8 RID: 168
		bool constrainedYAxis { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A9 RID: 169
		bool constrainedZAxis { get; }
	}
}
