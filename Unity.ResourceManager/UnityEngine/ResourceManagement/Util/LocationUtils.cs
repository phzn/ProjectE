using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200001C RID: 28
	internal static class LocationUtils
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public static bool LocationEquals(IResourceLocation loc1, IResourceLocation loc2)
		{
			return loc1 == loc2 || (loc1 != null && loc2 != null && (loc1.InternalId.Equals(loc2.InternalId) && loc1.ProviderId.Equals(loc2.ProviderId)) && loc1.ResourceType.Equals(loc2.ResourceType));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004B28 File Offset: 0x00002D28
		public static bool DependenciesEqual(IList<IResourceLocation> deps1, IList<IResourceLocation> deps2)
		{
			if (deps1 == deps2)
			{
				return true;
			}
			if (deps1 == null)
			{
				return false;
			}
			if (deps2 == null)
			{
				return false;
			}
			if (deps1.Count != deps2.Count)
			{
				return false;
			}
			for (int i = 0; i < deps1.Count; i++)
			{
				if (!LocationUtils.LocationEquals(deps1[i], deps2[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
