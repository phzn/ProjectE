using System;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.Serialization;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x0200002F RID: 47
	[Serializable]
	public class AssetBundleRequestOptions : ILocationSizeData
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005675 File Offset: 0x00003875
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000567D File Offset: 0x0000387D
		public string Hash
		{
			get
			{
				return this.m_Hash;
			}
			set
			{
				this.m_Hash = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005686 File Offset: 0x00003886
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000568E File Offset: 0x0000388E
		public uint Crc
		{
			get
			{
				return this.m_Crc;
			}
			set
			{
				this.m_Crc = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005697 File Offset: 0x00003897
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000569F File Offset: 0x0000389F
		public int Timeout
		{
			get
			{
				return this.m_Timeout;
			}
			set
			{
				this.m_Timeout = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000056A8 File Offset: 0x000038A8
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000056B0 File Offset: 0x000038B0
		public bool ChunkedTransfer
		{
			get
			{
				return this.m_ChunkedTransfer;
			}
			set
			{
				this.m_ChunkedTransfer = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000056B9 File Offset: 0x000038B9
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000056D4 File Offset: 0x000038D4
		public int RedirectLimit
		{
			get
			{
				if (this.m_RedirectLimit <= 128)
				{
					return this.m_RedirectLimit;
				}
				return 128;
			}
			set
			{
				this.m_RedirectLimit = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000056DD File Offset: 0x000038DD
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000056E5 File Offset: 0x000038E5
		public int RetryCount
		{
			get
			{
				return this.m_RetryCount;
			}
			set
			{
				this.m_RetryCount = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000056EE File Offset: 0x000038EE
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000056F6 File Offset: 0x000038F6
		public string BundleName
		{
			get
			{
				return this.m_BundleName;
			}
			set
			{
				this.m_BundleName = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600012E RID: 302 RVA: 0x000056FF File Offset: 0x000038FF
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00005707 File Offset: 0x00003907
		public AssetLoadMode AssetLoadMode
		{
			get
			{
				return this.m_AssetLoadMode;
			}
			set
			{
				this.m_AssetLoadMode = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005710 File Offset: 0x00003910
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00005718 File Offset: 0x00003918
		public long BundleSize
		{
			get
			{
				return this.m_BundleSize;
			}
			set
			{
				this.m_BundleSize = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005721 File Offset: 0x00003921
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00005729 File Offset: 0x00003929
		public bool UseCrcForCachedBundle
		{
			get
			{
				return this.m_UseCrcForCachedBundles;
			}
			set
			{
				this.m_UseCrcForCachedBundles = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005732 File Offset: 0x00003932
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000573A File Offset: 0x0000393A
		public bool UseUnityWebRequestForLocalBundles
		{
			get
			{
				return this.m_UseUWRForLocalBundles;
			}
			set
			{
				this.m_UseUWRForLocalBundles = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00005743 File Offset: 0x00003943
		// (set) Token: 0x06000137 RID: 311 RVA: 0x0000574B File Offset: 0x0000394B
		public bool ClearOtherCachedVersionsWhenLoaded
		{
			get
			{
				return this.m_ClearOtherCachedVersionsWhenLoaded;
			}
			set
			{
				this.m_ClearOtherCachedVersionsWhenLoaded = value;
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005754 File Offset: 0x00003954
		public virtual long ComputeSize(IResourceLocation location, ResourceManager resourceManager)
		{
			if (!ResourceManagerConfig.IsPathRemote((resourceManager == null) ? location.InternalId : resourceManager.TransformInternalId(location)))
			{
				return 0L;
			}
			Hash128 hash = Hash128.Parse(this.Hash);
			if (!hash.isValid)
			{
				return this.BundleSize;
			}
			if (Caching.IsVersionCached(new CachedAssetBundle(this.BundleName, hash)))
			{
				return 0L;
			}
			return this.BundleSize;
		}

		// Token: 0x0400006E RID: 110
		[FormerlySerializedAs("m_hash")]
		[SerializeField]
		private string m_Hash = "";

		// Token: 0x0400006F RID: 111
		[FormerlySerializedAs("m_crc")]
		[SerializeField]
		private uint m_Crc;

		// Token: 0x04000070 RID: 112
		[FormerlySerializedAs("m_timeout")]
		[SerializeField]
		private int m_Timeout;

		// Token: 0x04000071 RID: 113
		[FormerlySerializedAs("m_chunkedTransfer")]
		[SerializeField]
		private bool m_ChunkedTransfer;

		// Token: 0x04000072 RID: 114
		[FormerlySerializedAs("m_redirectLimit")]
		[SerializeField]
		private int m_RedirectLimit = -1;

		// Token: 0x04000073 RID: 115
		[FormerlySerializedAs("m_retryCount")]
		[SerializeField]
		private int m_RetryCount;

		// Token: 0x04000074 RID: 116
		[SerializeField]
		private string m_BundleName;

		// Token: 0x04000075 RID: 117
		[SerializeField]
		private AssetLoadMode m_AssetLoadMode;

		// Token: 0x04000076 RID: 118
		[SerializeField]
		private long m_BundleSize;

		// Token: 0x04000077 RID: 119
		[SerializeField]
		private bool m_UseCrcForCachedBundles;

		// Token: 0x04000078 RID: 120
		[SerializeField]
		private bool m_UseUWRForLocalBundles;

		// Token: 0x04000079 RID: 121
		[SerializeField]
		private bool m_ClearOtherCachedVersionsWhenLoaded;
	}
}
