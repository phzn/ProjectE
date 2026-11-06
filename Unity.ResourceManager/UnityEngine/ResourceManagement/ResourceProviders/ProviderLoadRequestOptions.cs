using System;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class ProviderLoadRequestOptions
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00006E10 File Offset: 0x00005010
		public ProviderLoadRequestOptions Copy()
		{
			return (ProviderLoadRequestOptions)base.MemberwiseClone();
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006E1D File Offset: 0x0000501D
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00006E25 File Offset: 0x00005025
		public bool IgnoreFailures
		{
			get
			{
				return this.m_IgnoreFailures;
			}
			set
			{
				this.m_IgnoreFailures = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00006E2E File Offset: 0x0000502E
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00006E36 File Offset: 0x00005036
		public int WebRequestTimeout
		{
			get
			{
				return this.m_WebRequestTimeout;
			}
			set
			{
				this.m_WebRequestTimeout = value;
			}
		}

		// Token: 0x040000A1 RID: 161
		[SerializeField]
		private bool m_IgnoreFailures;

		// Token: 0x040000A2 RID: 162
		private int m_WebRequestTimeout;
	}
}
