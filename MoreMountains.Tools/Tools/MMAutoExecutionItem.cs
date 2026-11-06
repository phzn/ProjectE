using System;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class MMAutoExecutionItem
	{
		// Token: 0x0400001A RID: 26
		public bool AutoExecuteOnAwake;

		// Token: 0x0400001B RID: 27
		public bool AutoExecuteOnEnable;

		// Token: 0x0400001C RID: 28
		public bool AutoExecuteOnDisable;

		// Token: 0x0400001D RID: 29
		public bool AutoExecuteOnStart;

		// Token: 0x0400001E RID: 30
		public bool AutoExecuteOnInstantiate;

		// Token: 0x0400001F RID: 31
		public UnityEvent Event;
	}
}
