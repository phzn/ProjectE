using System;

namespace System.Net
{
	// Token: 0x02000636 RID: 1590
	internal class DirectProxy : ProxyChain
	{
		// Token: 0x06003235 RID: 12853 RVA: 0x000ADB6E File Offset: 0x000ABD6E
		internal DirectProxy(Uri destination)
			: base(destination)
		{
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000ADB77 File Offset: 0x000ABD77
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = null;
			if (this.m_ProxyRetrieved)
			{
				return false;
			}
			this.m_ProxyRetrieved = true;
			return true;
		}

		// Token: 0x04001D59 RID: 7513
		private bool m_ProxyRetrieved;
	}
}
