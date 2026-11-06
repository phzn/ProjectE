using System;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000007 RID: 7
	public class ResourceLocatorInfo
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000245D File Offset: 0x0000065D
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002465 File Offset: 0x00000665
		public IResourceLocator Locator { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000246E File Offset: 0x0000066E
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002476 File Offset: 0x00000676
		public string LocalHash { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000247F File Offset: 0x0000067F
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002487 File Offset: 0x00000687
		public IResourceLocation CatalogLocation { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002490 File Offset: 0x00000690
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002498 File Offset: 0x00000698
		internal bool ContentUpdateAvailable { get; set; }

		// Token: 0x0600001C RID: 28 RVA: 0x000024A1 File Offset: 0x000006A1
		public ResourceLocatorInfo(IResourceLocator loc, string localHash, IResourceLocation remoteCatalogLocation)
		{
			this.Locator = loc;
			this.LocalHash = localHash;
			this.CatalogLocation = remoteCatalogLocation;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024BE File Offset: 0x000006BE
		public IResourceLocation HashLocation
		{
			get
			{
				return this.CatalogLocation.Dependencies[0];
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000024D1 File Offset: 0x000006D1
		public bool CanUpdateContent
		{
			get
			{
				return !string.IsNullOrEmpty(this.LocalHash) && this.CatalogLocation != null && this.CatalogLocation.HasDependencies && this.CatalogLocation.Dependencies.Count == 2;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000250A File Offset: 0x0000070A
		internal void UpdateContent(IResourceLocator locator, string hash, IResourceLocation loc)
		{
			this.LocalHash = hash;
			this.CatalogLocation = loc;
			this.Locator = locator;
		}
	}
}
