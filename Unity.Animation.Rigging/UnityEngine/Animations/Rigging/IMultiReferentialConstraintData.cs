using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000028 RID: 40
	public interface IMultiReferentialConstraintData
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B2 RID: 178
		int driverValue { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B3 RID: 179
		string driverIntProperty { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B4 RID: 180
		Transform[] sourceObjects { get; }
	}
}
