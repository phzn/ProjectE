using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000022 RID: 34
	public interface IMultiParentConstraintData
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000090 RID: 144
		Transform constrainedObject { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000091 RID: 145
		WeightedTransformArray sourceObjects { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000092 RID: 146
		bool maintainPositionOffset { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000093 RID: 147
		bool maintainRotationOffset { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000094 RID: 148
		bool constrainedPositionXAxis { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000095 RID: 149
		bool constrainedPositionYAxis { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000096 RID: 150
		bool constrainedPositionZAxis { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000097 RID: 151
		bool constrainedRotationXAxis { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000098 RID: 152
		bool constrainedRotationYAxis { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000099 RID: 153
		bool constrainedRotationZAxis { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600009A RID: 154
		string sourceObjectsProperty { get; }
	}
}
