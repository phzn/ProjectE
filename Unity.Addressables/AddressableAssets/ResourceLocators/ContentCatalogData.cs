using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.AddressableAssets.Utility;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.Serialization;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public class ContentCatalogData
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000076DD File Offset: 0x000058DD
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000076E5 File Offset: 0x000058E5
		public string BuildResultHash
		{
			get
			{
				return this.m_BuildResultHash;
			}
			set
			{
				this.m_BuildResultHash = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000076EE File Offset: 0x000058EE
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x000076F6 File Offset: 0x000058F6
		public string ProviderId
		{
			get
			{
				return this.m_LocatorId;
			}
			internal set
			{
				this.m_LocatorId = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x000076FF File Offset: 0x000058FF
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00007707 File Offset: 0x00005907
		public ObjectInitializationData InstanceProviderData
		{
			get
			{
				return this.m_InstanceProviderData;
			}
			set
			{
				this.m_InstanceProviderData = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007710 File Offset: 0x00005910
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007718 File Offset: 0x00005918
		public ObjectInitializationData SceneProviderData
		{
			get
			{
				return this.m_SceneProviderData;
			}
			set
			{
				this.m_SceneProviderData = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007721 File Offset: 0x00005921
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00007729 File Offset: 0x00005929
		public List<ObjectInitializationData> ResourceProviderData
		{
			get
			{
				return this.m_ResourceProviderData;
			}
			set
			{
				this.m_ResourceProviderData = value;
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007732 File Offset: 0x00005932
		public ContentCatalogData(string id)
		{
			this.m_LocatorId = id;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000774C File Offset: 0x0000594C
		public ContentCatalogData()
		{
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000775F File Offset: 0x0000595F
		public string[] ProviderIds
		{
			get
			{
				return this.m_ProviderIds;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00007767 File Offset: 0x00005967
		public string[] InternalIds
		{
			get
			{
				return this.m_InternalIds;
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000776F File Offset: 0x0000596F
		internal static ContentCatalogData LoadFromFile(string path, int cacheSize = 1024)
		{
			return JsonUtility.FromJson<ContentCatalogData>(File.ReadAllText(path));
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000777C File Offset: 0x0000597C
		internal void SaveToFile(string path)
		{
			File.WriteAllText(path, JsonUtility.ToJson(this));
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000778C File Offset: 0x0000598C
		internal void CleanData()
		{
			this.m_KeyDataString = "";
			this.m_BucketDataString = "";
			this.m_EntryDataString = "";
			this.m_ExtraDataString = "";
			this.m_InternalIds = null;
			this.m_LocatorId = "";
			this.m_ProviderIds = null;
			this.m_ResourceProviderData = null;
			this.m_resourceTypes = null;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000077EC File Offset: 0x000059EC
		internal ResourceLocationMap CreateCustomLocator(string overrideId = "", string providerSuffix = null)
		{
			this.m_LocatorId = overrideId;
			return this.CreateLocator(providerSuffix);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000077FC File Offset: 0x000059FC
		public ResourceLocationMap CreateLocator(string providerSuffix = null)
		{
			byte[] array = Convert.FromBase64String(this.m_BucketDataString);
			int num = BitConverter.ToInt32(array, 0);
			ContentCatalogData.Bucket[] array2 = new ContentCatalogData.Bucket[num];
			int num2 = 4;
			for (int i = 0; i < num; i++)
			{
				int num3 = SerializationUtilities.ReadInt32FromByteArray(array, num2);
				num2 += 4;
				int num4 = SerializationUtilities.ReadInt32FromByteArray(array, num2);
				num2 += 4;
				int[] array3 = new int[num4];
				for (int j = 0; j < num4; j++)
				{
					array3[j] = SerializationUtilities.ReadInt32FromByteArray(array, num2);
					num2 += 4;
				}
				array2[i] = new ContentCatalogData.Bucket
				{
					entries = array3,
					dataOffset = num3
				};
			}
			if (!string.IsNullOrEmpty(providerSuffix))
			{
				for (int k = 0; k < this.m_ProviderIds.Length; k++)
				{
					if (!this.m_ProviderIds[k].EndsWith(providerSuffix, StringComparison.Ordinal))
					{
						this.m_ProviderIds[k] = this.m_ProviderIds[k] + providerSuffix;
					}
				}
			}
			byte[] array4 = Convert.FromBase64String(this.m_ExtraDataString);
			byte[] array5 = Convert.FromBase64String(this.m_KeyDataString);
			object[] array6 = new object[BitConverter.ToInt32(array5, 0)];
			for (int l = 0; l < array2.Length; l++)
			{
				array6[l] = SerializationUtilities.ReadObjectFromByteArray(array5, array2[l].dataOffset);
			}
			ResourceLocationMap resourceLocationMap = new ResourceLocationMap(this.m_LocatorId, array2.Length);
			byte[] array7 = Convert.FromBase64String(this.m_EntryDataString);
			int num5 = SerializationUtilities.ReadInt32FromByteArray(array7, 0);
			IResourceLocation[] array8 = new IResourceLocation[num5];
			for (int m = 0; m < num5; m++)
			{
				int num6 = 4 + m * 28;
				int num7 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				num6 += 4;
				int num8 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				num6 += 4;
				int num9 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				num6 += 4;
				int num10 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				num6 += 4;
				int num11 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				num6 += 4;
				int num12 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				num6 += 4;
				int num13 = SerializationUtilities.ReadInt32FromByteArray(array7, num6);
				object obj = ((num11 < 0) ? null : SerializationUtilities.ReadObjectFromByteArray(array4, num11));
				array8[m] = new ContentCatalogData.CompactLocation(resourceLocationMap, Addressables.ResolveInternalId(ContentCatalogData.ExpandInternalId(this.m_InternalIdPrefixes, this.m_InternalIds[num7])), this.m_ProviderIds[num8], (num9 < 0) ? null : array6[num9], obj, num10, array6[num12].ToString(), this.m_resourceTypes[num13].Value);
			}
			for (int n = 0; n < array2.Length; n++)
			{
				ContentCatalogData.Bucket bucket = array2[n];
				object obj2 = array6[n];
				IResourceLocation[] array9 = new IResourceLocation[bucket.entries.Length];
				for (int num14 = 0; num14 < bucket.entries.Length; num14++)
				{
					array9[num14] = array8[bucket.entries[num14]];
				}
				resourceLocationMap.Add(obj2, array9);
			}
			return resourceLocationMap;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007AD4 File Offset: 0x00005CD4
		internal IList<ContentCatalogDataEntry> GetData()
		{
			ResourceLocationMap resourceLocationMap = this.CreateLocator(null);
			List<ContentCatalogDataEntry> list = new List<ContentCatalogDataEntry>();
			Dictionary<IResourceLocation, List<object>> dictionary = new Dictionary<IResourceLocation, List<object>>();
			foreach (object obj in resourceLocationMap.Keys)
			{
				IList<IResourceLocation> list2;
				resourceLocationMap.Locate(obj, null, out list2);
				foreach (IResourceLocation resourceLocation in list2)
				{
					List<object> list3;
					if (!dictionary.TryGetValue(resourceLocation, out list3))
					{
						dictionary.Add(resourceLocation, list3 = new List<object>());
					}
					list3.Add(obj.ToString());
				}
			}
			foreach (KeyValuePair<IResourceLocation, List<object>> keyValuePair in dictionary)
			{
				List<ContentCatalogDataEntry> list4 = list;
				Type resourceType = keyValuePair.Key.ResourceType;
				string internalId = keyValuePair.Key.InternalId;
				string providerId = keyValuePair.Key.ProviderId;
				IEnumerable<object> value = keyValuePair.Value;
				IEnumerable<object> enumerable;
				if (keyValuePair.Key.Dependencies != null)
				{
					enumerable = keyValuePair.Key.Dependencies.Select<IResourceLocation, string>((IResourceLocation d) => d.PrimaryKey).ToList<string>();
				}
				else
				{
					enumerable = null;
				}
				list4.Add(new ContentCatalogDataEntry(resourceType, internalId, providerId, value, enumerable, keyValuePair.Key.Data));
			}
			return list;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007C64 File Offset: 0x00005E64
		internal static string ExpandInternalId(string[] internalIdPrefixes, string v)
		{
			if (internalIdPrefixes == null || internalIdPrefixes.Length == 0)
			{
				return v;
			}
			int num = v.LastIndexOf('#');
			if (num < 0)
			{
				return v;
			}
			int num2 = 0;
			if (!int.TryParse(v.Substring(0, num), out num2))
			{
				return v;
			}
			return internalIdPrefixes[num2] + v.Substring(num + 1);
		}

		// Token: 0x0400005A RID: 90
		private static int kMagic = "ContentCatalogData".GetHashCode();

		// Token: 0x0400005B RID: 91
		private const int kVersion = 1;

		// Token: 0x0400005C RID: 92
		[NonSerialized]
		public string LocalHash;

		// Token: 0x0400005D RID: 93
		[NonSerialized]
		internal IResourceLocation location;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		internal string m_LocatorId;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		internal string m_BuildResultHash;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private ObjectInitializationData m_InstanceProviderData;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private ObjectInitializationData m_SceneProviderData;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		internal List<ObjectInitializationData> m_ResourceProviderData = new List<ObjectInitializationData>();

		// Token: 0x04000063 RID: 99
		private IList<ContentCatalogDataEntry> m_Entries;

		// Token: 0x04000064 RID: 100
		[FormerlySerializedAs("m_providerIds")]
		[SerializeField]
		internal string[] m_ProviderIds;

		// Token: 0x04000065 RID: 101
		[FormerlySerializedAs("m_internalIds")]
		[SerializeField]
		internal string[] m_InternalIds;

		// Token: 0x04000066 RID: 102
		[FormerlySerializedAs("m_keyDataString")]
		[SerializeField]
		internal string m_KeyDataString;

		// Token: 0x04000067 RID: 103
		[FormerlySerializedAs("m_bucketDataString")]
		[SerializeField]
		internal string m_BucketDataString;

		// Token: 0x04000068 RID: 104
		[FormerlySerializedAs("m_entryDataString")]
		[SerializeField]
		internal string m_EntryDataString;

		// Token: 0x04000069 RID: 105
		private const int kBytesPerInt32 = 4;

		// Token: 0x0400006A RID: 106
		private const int k_EntryDataItemPerEntry = 7;

		// Token: 0x0400006B RID: 107
		[FormerlySerializedAs("m_extraDataString")]
		[SerializeField]
		internal string m_ExtraDataString;

		// Token: 0x0400006C RID: 108
		[SerializeField]
		internal SerializedType[] m_resourceTypes;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private string[] m_InternalIdPrefixes;

		// Token: 0x02000055 RID: 85
		private struct Bucket
		{
			// Token: 0x04000123 RID: 291
			public int dataOffset;

			// Token: 0x04000124 RID: 292
			public int[] entries;
		}

		// Token: 0x02000056 RID: 86
		private class CompactLocation : IResourceLocation
		{
			// Token: 0x17000067 RID: 103
			// (get) Token: 0x06000267 RID: 615 RVA: 0x00009E98 File Offset: 0x00008098
			public string InternalId
			{
				get
				{
					return this.m_InternalId;
				}
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x06000268 RID: 616 RVA: 0x00009EA0 File Offset: 0x000080A0
			public string ProviderId
			{
				get
				{
					return this.m_ProviderId;
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x06000269 RID: 617 RVA: 0x00009EA8 File Offset: 0x000080A8
			public IList<IResourceLocation> Dependencies
			{
				get
				{
					if (this.m_Dependency == null)
					{
						return null;
					}
					IList<IResourceLocation> list;
					this.m_Locator.Locate(this.m_Dependency, typeof(object), out list);
					return list;
				}
			}

			// Token: 0x1700006A RID: 106
			// (get) Token: 0x0600026A RID: 618 RVA: 0x00009EDE File Offset: 0x000080DE
			public bool HasDependencies
			{
				get
				{
					return this.m_Dependency != null;
				}
			}

			// Token: 0x1700006B RID: 107
			// (get) Token: 0x0600026B RID: 619 RVA: 0x00009EE9 File Offset: 0x000080E9
			public int DependencyHashCode
			{
				get
				{
					return this.m_DependencyHashCode;
				}
			}

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x0600026C RID: 620 RVA: 0x00009EF1 File Offset: 0x000080F1
			public object Data
			{
				get
				{
					return this.m_Data;
				}
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x0600026D RID: 621 RVA: 0x00009EF9 File Offset: 0x000080F9
			// (set) Token: 0x0600026E RID: 622 RVA: 0x00009F01 File Offset: 0x00008101
			public string PrimaryKey
			{
				get
				{
					return this.m_PrimaryKey;
				}
				set
				{
					this.m_PrimaryKey = value;
				}
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x0600026F RID: 623 RVA: 0x00009F0A File Offset: 0x0000810A
			public Type ResourceType
			{
				get
				{
					return this.m_Type;
				}
			}

			// Token: 0x06000270 RID: 624 RVA: 0x00009F12 File Offset: 0x00008112
			public override string ToString()
			{
				return this.m_InternalId;
			}

			// Token: 0x06000271 RID: 625 RVA: 0x00009F1A File Offset: 0x0000811A
			public int Hash(Type t)
			{
				return (this.m_HashCode * 31 + t.GetHashCode()) * 31 + this.DependencyHashCode;
			}

			// Token: 0x06000272 RID: 626 RVA: 0x00009F38 File Offset: 0x00008138
			public CompactLocation(ResourceLocationMap locator, string internalId, string providerId, object dependencyKey, object data, int depHash, string primaryKey, Type type)
			{
				this.m_Locator = locator;
				this.m_InternalId = internalId;
				this.m_ProviderId = providerId;
				this.m_Dependency = dependencyKey;
				this.m_Data = data;
				this.m_HashCode = internalId.GetHashCode() * 31 + providerId.GetHashCode();
				this.m_DependencyHashCode = depHash;
				this.m_PrimaryKey = primaryKey;
				this.m_Type = ((type == null) ? typeof(object) : type);
			}

			// Token: 0x04000125 RID: 293
			private ResourceLocationMap m_Locator;

			// Token: 0x04000126 RID: 294
			private string m_InternalId;

			// Token: 0x04000127 RID: 295
			private string m_ProviderId;

			// Token: 0x04000128 RID: 296
			private object m_Dependency;

			// Token: 0x04000129 RID: 297
			private object m_Data;

			// Token: 0x0400012A RID: 298
			private int m_HashCode;

			// Token: 0x0400012B RID: 299
			private int m_DependencyHashCode;

			// Token: 0x0400012C RID: 300
			private string m_PrimaryKey;

			// Token: 0x0400012D RID: 301
			private Type m_Type;
		}
	}
}
