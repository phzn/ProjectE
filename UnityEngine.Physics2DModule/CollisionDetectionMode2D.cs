using System;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x0200000B RID: 11
	public enum CollisionDetectionMode2D
	{
		// Token: 0x04000029 RID: 41
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Enum member CollisionDetectionMode2D.None has been deprecated. Use CollisionDetectionMode2D.Discrete instead (UnityUpgradable) -> Discrete", true)]
		None,
		// Token: 0x0400002A RID: 42
		Discrete = 0,
		// Token: 0x0400002B RID: 43
		Continuous
	}
}
