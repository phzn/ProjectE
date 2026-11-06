using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200002E RID: 46
	public interface IOverrideTransformData
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CC RID: 204
		Transform constrainedObject { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000CD RID: 205
		Transform sourceObject { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000CE RID: 206
		int space { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000CF RID: 207
		string positionWeightFloatProperty { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D0 RID: 208
		string rotationWeightFloatProperty { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D1 RID: 209
		string positionVector3Property { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D2 RID: 210
		string rotationVector3Property { get; }
	}
}
