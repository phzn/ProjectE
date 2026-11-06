using System;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000757 RID: 1879
	internal sealed class DefaultProxySectionInternal
	{
		// Token: 0x06003B4C RID: 15180 RVA: 0x000CC17C File Offset: 0x000CA37C
		private static IWebProxy GetDefaultProxy_UsingOldMonoCode()
		{
			DefaultProxySection defaultProxySection = ConfigurationManager.GetSection("system.net/defaultProxy") as DefaultProxySection;
			if (defaultProxySection == null)
			{
				return DefaultProxySectionInternal.GetSystemWebProxy();
			}
			ProxyElement proxy = defaultProxySection.Proxy;
			WebProxy webProxy;
			if (proxy.UseSystemDefault != ProxyElement.UseSystemDefaultValues.False && proxy.ProxyAddress == null)
			{
				IWebProxy systemWebProxy = DefaultProxySectionInternal.GetSystemWebProxy();
				if (!(systemWebProxy is WebProxy))
				{
					return systemWebProxy;
				}
				webProxy = (WebProxy)systemWebProxy;
			}
			else
			{
				webProxy = new WebProxy();
			}
			if (proxy.ProxyAddress != null)
			{
				webProxy.Address = proxy.ProxyAddress;
			}
			if (proxy.BypassOnLocal != ProxyElement.BypassOnLocalValues.Unspecified)
			{
				webProxy.BypassProxyOnLocal = proxy.BypassOnLocal == ProxyElement.BypassOnLocalValues.True;
			}
			foreach (object obj in defaultProxySection.BypassList)
			{
				BypassElement bypassElement = (BypassElement)obj;
				webProxy.BypassArrayList.Add(bypassElement.Address);
			}
			return webProxy;
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x000AB962 File Offset: 0x000A9B62
		private static IWebProxy GetSystemWebProxy()
		{
			return global::System.Net.WebProxy.CreateDefaultProxy();
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x000CC274 File Offset: 0x000CA474
		internal static object ClassSyncObject
		{
			get
			{
				if (DefaultProxySectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref DefaultProxySectionInternal.classSyncObject, obj, null);
				}
				return DefaultProxySectionInternal.classSyncObject;
			}
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x000CC2A0 File Offset: 0x000CA4A0
		internal static DefaultProxySectionInternal GetSection()
		{
			object obj = DefaultProxySectionInternal.ClassSyncObject;
			DefaultProxySectionInternal defaultProxySectionInternal;
			lock (obj)
			{
				defaultProxySectionInternal = new DefaultProxySectionInternal
				{
					webProxy = DefaultProxySectionInternal.GetDefaultProxy_UsingOldMonoCode()
				};
			}
			return defaultProxySectionInternal;
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x000CC2EC File Offset: 0x000CA4EC
		internal IWebProxy WebProxy
		{
			get
			{
				return this.webProxy;
			}
		}

		// Token: 0x04002363 RID: 9059
		private IWebProxy webProxy;

		// Token: 0x04002364 RID: 9060
		private static object classSyncObject;
	}
}
