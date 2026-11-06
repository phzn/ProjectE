using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
	// Token: 0x02000022 RID: 34
	public interface IResourceLocator
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B6 RID: 438
		string LocatorId { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B7 RID: 439
		IEnumerable<object> Keys { get; }

		// Token: 0x060001B8 RID: 440
		bool Locate(object key, Type type, out IList<IResourceLocation> locations);
	}
}
