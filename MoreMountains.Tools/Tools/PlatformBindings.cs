using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class PlatformBindings
	{
		// Token: 0x04000015 RID: 21
		public RuntimePlatform Platform = RuntimePlatform.WindowsPlayer;

		// Token: 0x04000016 RID: 22
		public PlatformBindings.PlatformActions PlatformAction;

		// Token: 0x02000398 RID: 920
		public enum PlatformActions
		{
			// Token: 0x04001D35 RID: 7477
			DoNothing,
			// Token: 0x04001D36 RID: 7478
			Disable
		}
	}
}
