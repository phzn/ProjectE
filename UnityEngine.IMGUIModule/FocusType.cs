using System;

namespace UnityEngine
{
	// Token: 0x02000018 RID: 24
	public enum FocusType
	{
		// Token: 0x0400007A RID: 122
		[Obsolete("FocusType.Native now behaves the same as FocusType.Passive in all OS cases. (UnityUpgradable) -> Passive", false)]
		Native,
		// Token: 0x0400007B RID: 123
		Keyboard,
		// Token: 0x0400007C RID: 124
		Passive
	}
}
