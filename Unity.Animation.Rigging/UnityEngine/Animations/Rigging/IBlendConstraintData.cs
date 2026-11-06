using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000009 RID: 9
	public interface IBlendConstraintData
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26
		Transform constrainedObject { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001B RID: 27
		Transform sourceObjectA { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001C RID: 28
		Transform sourceObjectB { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29
		bool maintainPositionOffsets { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30
		bool maintainRotationOffsets { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31
		string blendPositionBoolProperty { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32
		string blendRotationBoolProperty { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000021 RID: 33
		string positionWeightFloatProperty { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34
		string rotationWeightFloatProperty { get; }
	}
}
