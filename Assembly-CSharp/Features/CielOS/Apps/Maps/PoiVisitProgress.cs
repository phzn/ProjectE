using System;
using System.Collections.Generic;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x02001482 RID: 5250
	[Serializable]
	public sealed class PoiVisitProgress
	{
		// Token: 0x0400A579 RID: 42361
		public string sceneName;

		// Token: 0x0400A57A RID: 42362
		public int visitCount;

		// Token: 0x0400A57B RID: 42363
		public List<string> discoveredInstanceIds = new List<string>();
	}
}
