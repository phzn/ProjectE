using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200026F RID: 623
	public interface ITransform
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060011B8 RID: 4536
		// (set) Token: 0x060011B9 RID: 4537
		Vector3 position { get; set; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060011BA RID: 4538
		// (set) Token: 0x060011BB RID: 4539
		Quaternion rotation { get; set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060011BC RID: 4540
		// (set) Token: 0x060011BD RID: 4541
		Vector3 scale { get; set; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060011BE RID: 4542
		Matrix4x4 matrix { get; }
	}
}
