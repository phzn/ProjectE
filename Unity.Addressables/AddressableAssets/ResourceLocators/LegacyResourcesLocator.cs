using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
	// Token: 0x02000023 RID: 35
	public class LegacyResourcesLocator : IResourceLocator
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00007CC0 File Offset: 0x00005EC0
		public bool Locate(object key, Type type, out IList<IResourceLocation> locations)
		{
			locations = null;
			string text = key as string;
			if (text == null)
			{
				return false;
			}
			locations = new List<IResourceLocation>();
			locations.Add(new ResourceLocationBase("LegacyResourceLocation", text, typeof(LegacyResourcesProvider).FullName, typeof(Object), Array.Empty<IResourceLocation>()));
			return true;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00007D14 File Offset: 0x00005F14
		public IEnumerable<object> Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00007D17 File Offset: 0x00005F17
		public string LocatorId
		{
			get
			{
				return "LegacyResourcesLocator";
			}
		}
	}
}
