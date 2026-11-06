using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200010A RID: 266
	[Serializable]
	public class MMMultipleObjectPoolerObject
	{
		// Token: 0x040006FA RID: 1786
		public GameObject GameObjectToPool;

		// Token: 0x040006FB RID: 1787
		public int PoolSize;

		// Token: 0x040006FC RID: 1788
		public bool PoolCanExpand = true;

		// Token: 0x040006FD RID: 1789
		public bool Enabled = true;
	}
}
