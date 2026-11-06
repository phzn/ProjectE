using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000036 RID: 54
	[Flags]
	[Serializable]
	public enum NotificationFlags : short
	{
		// Token: 0x040000D8 RID: 216
		TriggerInEditMode = 1,
		// Token: 0x040000D9 RID: 217
		Retroactive = 2,
		// Token: 0x040000DA RID: 218
		TriggerOnce = 4
	}
}
