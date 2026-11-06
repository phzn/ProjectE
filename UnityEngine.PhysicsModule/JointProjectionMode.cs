using System;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x02000004 RID: 4
	public enum JointProjectionMode
	{
		// Token: 0x04000012 RID: 18
		None,
		// Token: 0x04000013 RID: 19
		PositionAndRotation,
		// Token: 0x04000014 RID: 20
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("JointProjectionMode.PositionOnly is no longer supported", true)]
		PositionOnly
	}
}
