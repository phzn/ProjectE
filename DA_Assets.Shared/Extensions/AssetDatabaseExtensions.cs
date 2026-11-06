using System;
using System.Collections.Generic;

namespace DA_Assets.Extensions
{
	// Token: 0x02000026 RID: 38
	public static class AssetDatabaseExtensions
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00003CAA File Offset: 0x00001EAA
		public static List<string> FindAssetsPathes<T>(this string path, string customType = null)
		{
			List<string> list = new List<string>();
			if (customType == null)
			{
				customType = "t:" + typeof(T).Name;
			}
			return list;
		}
	}
}
