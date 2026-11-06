using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200000F RID: 15
	public interface IDampedTransformData
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003B RID: 59
		Transform constrainedObject { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003C RID: 60
		Transform sourceObject { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003D RID: 61
		bool maintainAim { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003E RID: 62
		string dampPositionFloatProperty { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003F RID: 63
		string dampRotationFloatProperty { get; }
	}
}
