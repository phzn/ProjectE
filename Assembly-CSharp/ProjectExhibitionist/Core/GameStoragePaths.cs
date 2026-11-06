using System;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A5D RID: 2653
	public static class GameStoragePaths
	{
		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004B8A RID: 19338 RVA: 0x001D5AB5 File Offset: 0x001D3CB5
		public static string PersistentRoot
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		// Token: 0x04004D44 RID: 19780
		public const string CompanyName = "JoystickyLabs";
	}
}
