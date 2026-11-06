using System;

namespace System.Net
{
	// Token: 0x02000652 RID: 1618
	internal struct HeaderVariantInfo
	{
		// Token: 0x060032EE RID: 13038 RVA: 0x000B0FDD File Offset: 0x000AF1DD
		internal HeaderVariantInfo(string name, CookieVariant variant)
		{
			this.m_name = name;
			this.m_variant = variant;
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x000B0FED File Offset: 0x000AF1ED
		internal string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060032F0 RID: 13040 RVA: 0x000B0FF5 File Offset: 0x000AF1F5
		internal CookieVariant Variant
		{
			get
			{
				return this.m_variant;
			}
		}

		// Token: 0x04001DED RID: 7661
		private string m_name;

		// Token: 0x04001DEE RID: 7662
		private CookieVariant m_variant;
	}
}
