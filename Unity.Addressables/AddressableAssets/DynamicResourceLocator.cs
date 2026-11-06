using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.U2D;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000018 RID: 24
	internal class DynamicResourceLocator : IResourceLocator
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000068E0 File Offset: 0x00004AE0
		public string LocatorId
		{
			get
			{
				return "DynamicResourceLocator";
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000068E7 File Offset: 0x00004AE7
		public virtual IEnumerable<object> Keys
		{
			get
			{
				return new object[0];
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000068F0 File Offset: 0x00004AF0
		private string AtlasSpriteProviderId
		{
			get
			{
				if (!string.IsNullOrEmpty(this.m_AtlasSpriteProviderId))
				{
					return this.m_AtlasSpriteProviderId;
				}
				foreach (IResourceProvider resourceProvider in this.m_Addressables.ResourceManager.ResourceProviders)
				{
					if (resourceProvider is AtlasSpriteProvider)
					{
						this.m_AtlasSpriteProviderId = resourceProvider.ProviderId;
						return this.m_AtlasSpriteProviderId;
					}
				}
				return typeof(AtlasSpriteProvider).FullName;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006984 File Offset: 0x00004B84
		public IEnumerable<IResourceLocation> AllLocations
		{
			get
			{
				return new IResourceLocation[0];
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000698C File Offset: 0x00004B8C
		public DynamicResourceLocator(AddressablesImpl addr)
		{
			this.m_Addressables = addr;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000699C File Offset: 0x00004B9C
		public bool Locate(object key, Type type, out IList<IResourceLocation> locations)
		{
			locations = null;
			string text;
			string text2;
			if (ResourceManagerConfig.ExtractKeyAndSubKey(key, out text, out text2))
			{
				IList<IResourceLocation> list;
				if (!this.m_Addressables.GetResourceLocations(text, type, out list) && type == typeof(Sprite))
				{
					this.m_Addressables.GetResourceLocations(text, typeof(SpriteAtlas), out list);
				}
				if (list != null && list.Count > 0)
				{
					locations = new List<IResourceLocation>(list.Count);
					foreach (IResourceLocation resourceLocation in list)
					{
						this.CreateDynamicLocations(type, locations, key as string, text2, resourceLocation);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006A5C File Offset: 0x00004C5C
		internal void CreateDynamicLocations(Type type, IList<IResourceLocation> locations, string locName, string subKey, IResourceLocation mainLoc)
		{
			if (type == typeof(Sprite) && mainLoc.ResourceType == typeof(SpriteAtlas))
			{
				locations.Add(new ResourceLocationBase(locName, mainLoc.InternalId + "[" + subKey + "]", this.AtlasSpriteProviderId, type, new IResourceLocation[] { mainLoc }));
				return;
			}
			if (mainLoc.HasDependencies)
			{
				locations.Add(new ResourceLocationBase(locName, mainLoc.InternalId + "[" + subKey + "]", mainLoc.ProviderId, mainLoc.ResourceType, mainLoc.Dependencies.ToArray<IResourceLocation>()));
				return;
			}
			locations.Add(new ResourceLocationBase(locName, mainLoc.InternalId + "[" + subKey + "]", mainLoc.ProviderId, mainLoc.ResourceType, Array.Empty<IResourceLocation>()));
		}

		// Token: 0x0400003B RID: 59
		private AddressablesImpl m_Addressables;

		// Token: 0x0400003C RID: 60
		private string m_AtlasSpriteProviderId;
	}
}
