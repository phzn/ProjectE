using System;
using UnityEngine.Serialization;

namespace UnityEngine.AddressableAssets.Initialization
{
	// Token: 0x02000028 RID: 40
	[Serializable]
	public class CacheInitializationData
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00008657 File Offset: 0x00006857
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000865F File Offset: 0x0000685F
		public bool CompressionEnabled
		{
			get
			{
				return this.m_CompressionEnabled;
			}
			set
			{
				this.m_CompressionEnabled = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00008668 File Offset: 0x00006868
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00008670 File Offset: 0x00006870
		public string CacheDirectoryOverride
		{
			get
			{
				return this.m_CacheDirectoryOverride;
			}
			set
			{
				this.m_CacheDirectoryOverride = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00008679 File Offset: 0x00006879
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00008681 File Offset: 0x00006881
		[Obsolete("Functionality remains unchanged.  However, due to issues with Caching this property is being marked obsolete.  See Caching API documentation for more details.")]
		public int ExpirationDelay
		{
			get
			{
				return this.m_ExpirationDelay;
			}
			set
			{
				this.m_ExpirationDelay = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000868A File Offset: 0x0000688A
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00008692 File Offset: 0x00006892
		public bool LimitCacheSize
		{
			get
			{
				return this.m_LimitCacheSize;
			}
			set
			{
				this.m_LimitCacheSize = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000869B File Offset: 0x0000689B
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x000086A3 File Offset: 0x000068A3
		public long MaximumCacheSize
		{
			get
			{
				return this.m_MaximumCacheSize;
			}
			set
			{
				this.m_MaximumCacheSize = value;
			}
		}

		// Token: 0x0400007B RID: 123
		[FormerlySerializedAs("m_compressionEnabled")]
		[SerializeField]
		private bool m_CompressionEnabled = true;

		// Token: 0x0400007C RID: 124
		[FormerlySerializedAs("m_cacheDirectoryOverride")]
		[SerializeField]
		private string m_CacheDirectoryOverride = "";

		// Token: 0x0400007D RID: 125
		[FormerlySerializedAs("m_expirationDelay")]
		[SerializeField]
		private int m_ExpirationDelay = 12960000;

		// Token: 0x0400007E RID: 126
		[FormerlySerializedAs("m_limitCacheSize")]
		[SerializeField]
		private bool m_LimitCacheSize;

		// Token: 0x0400007F RID: 127
		[FormerlySerializedAs("m_maximumCacheSize")]
		[SerializeField]
		private long m_MaximumCacheSize = long.MaxValue;
	}
}
