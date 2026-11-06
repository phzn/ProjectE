using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
	// Token: 0x02000025 RID: 37
	public class ResourceLocationMap : IResourceLocator
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x00007E19 File Offset: 0x00006019
		public ResourceLocationMap(string id, int capacity = 0)
		{
			this.LocatorId = id;
			this.locations = new Dictionary<object, IList<IResourceLocation>>((capacity == 0) ? 100 : capacity);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00007E3B File Offset: 0x0000603B
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00007E43 File Offset: 0x00006043
		public string LocatorId { get; private set; }

		// Token: 0x060001C8 RID: 456 RVA: 0x00007E4C File Offset: 0x0000604C
		public ResourceLocationMap(string id, IList<ResourceLocationData> locations)
		{
			this.LocatorId = id;
			if (locations == null)
			{
				return;
			}
			this.locations = new Dictionary<object, IList<IResourceLocation>>(locations.Count * 2);
			Dictionary<string, ResourceLocationBase> dictionary = new Dictionary<string, ResourceLocationBase>();
			Dictionary<string, ResourceLocationData> dictionary2 = new Dictionary<string, ResourceLocationData>();
			for (int i = 0; i < locations.Count; i++)
			{
				ResourceLocationData resourceLocationData = locations[i];
				if (resourceLocationData.Keys == null || resourceLocationData.Keys.Length < 1)
				{
					Addressables.LogErrorFormat("Address with id '{0}' does not have any valid keys, skipping...", new object[] { resourceLocationData.InternalId });
				}
				else if (dictionary.ContainsKey(resourceLocationData.Keys[0]))
				{
					Addressables.LogErrorFormat("Duplicate address '{0}' with id '{1}' found, skipping...", new object[]
					{
						resourceLocationData.Keys[0],
						resourceLocationData.InternalId
					});
				}
				else
				{
					ResourceLocationBase resourceLocationBase = new ResourceLocationBase(resourceLocationData.Keys[0], Addressables.ResolveInternalId(resourceLocationData.InternalId), resourceLocationData.Provider, resourceLocationData.ResourceType, Array.Empty<IResourceLocation>());
					resourceLocationBase.Data = resourceLocationData.Data;
					dictionary.Add(resourceLocationData.Keys[0], resourceLocationBase);
					dictionary2.Add(resourceLocationData.Keys[0], resourceLocationData);
				}
			}
			foreach (KeyValuePair<string, ResourceLocationBase> keyValuePair in dictionary)
			{
				ResourceLocationData resourceLocationData2 = dictionary2[keyValuePair.Key];
				if (resourceLocationData2.Dependencies != null)
				{
					foreach (string text in resourceLocationData2.Dependencies)
					{
						keyValuePair.Value.Dependencies.Add(dictionary[text]);
					}
					keyValuePair.Value.ComputeDependencyHash();
				}
			}
			foreach (KeyValuePair<string, ResourceLocationBase> keyValuePair2 in dictionary)
			{
				foreach (string text2 in dictionary2[keyValuePair2.Key].Keys)
				{
					this.Add(text2, keyValuePair2.Value);
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00008078 File Offset: 0x00006278
		public Dictionary<object, IList<IResourceLocation>> Locations
		{
			get
			{
				return this.locations;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00008080 File Offset: 0x00006280
		public IEnumerable<object> Keys
		{
			get
			{
				return this.locations.Keys;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008090 File Offset: 0x00006290
		public bool Locate(object key, Type type, out IList<IResourceLocation> locations)
		{
			IList<IResourceLocation> list = null;
			if (!this.locations.TryGetValue(key, out list))
			{
				locations = null;
				return false;
			}
			if (type == null)
			{
				locations = list;
				return true;
			}
			int num = 0;
			foreach (IResourceLocation resourceLocation in list)
			{
				if (type.IsAssignableFrom(resourceLocation.ResourceType))
				{
					num++;
				}
			}
			if (num == 0)
			{
				locations = null;
				return false;
			}
			if (num == list.Count)
			{
				locations = list;
				return true;
			}
			locations = new List<IResourceLocation>();
			foreach (IResourceLocation resourceLocation2 in list)
			{
				if (type.IsAssignableFrom(resourceLocation2.ResourceType))
				{
					locations.Add(resourceLocation2);
				}
			}
			return true;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008174 File Offset: 0x00006374
		public void Add(object key, IResourceLocation location)
		{
			IList<IResourceLocation> list;
			if (!this.locations.TryGetValue(key, out list))
			{
				this.locations.Add(key, list = new List<IResourceLocation>());
			}
			list.Add(location);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000081AB File Offset: 0x000063AB
		public void Add(object key, IList<IResourceLocation> locations)
		{
			this.locations.Add(key, locations);
		}

		// Token: 0x04000076 RID: 118
		private Dictionary<object, IList<IResourceLocation>> locations;
	}
}
