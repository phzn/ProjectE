using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000229 RID: 553
	internal class XmlDownloadManager
	{
		// Token: 0x060014FA RID: 5370 RVA: 0x00082A11 File Offset: 0x00080C11
		internal Stream GetStream(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
		{
			if (uri.Scheme == "file")
			{
				return new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1);
			}
			return this.GetNonFileStream(uri, credentials, proxy, cachePolicy);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00082A40 File Offset: 0x00080C40
		private Stream GetNonFileStream(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
		{
			WebRequest webRequest = WebRequest.Create(uri);
			if (credentials != null)
			{
				webRequest.Credentials = credentials;
			}
			if (proxy != null)
			{
				webRequest.Proxy = proxy;
			}
			if (cachePolicy != null)
			{
				webRequest.CachePolicy = cachePolicy;
			}
			WebResponse response = webRequest.GetResponse();
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				lock (this)
				{
					if (this.connections == null)
					{
						this.connections = new Hashtable();
					}
					OpenedHost openedHost = (OpenedHost)this.connections[httpWebRequest.Address.Host];
					if (openedHost == null)
					{
						openedHost = new OpenedHost();
					}
					if (openedHost.nonCachedConnectionsCount < httpWebRequest.ServicePoint.ConnectionLimit - 1)
					{
						if (openedHost.nonCachedConnectionsCount == 0)
						{
							this.connections.Add(httpWebRequest.Address.Host, openedHost);
						}
						openedHost.nonCachedConnectionsCount++;
						return new XmlRegisteredNonCachedStream(response.GetResponseStream(), this, httpWebRequest.Address.Host);
					}
					return new XmlCachedStream(response.ResponseUri, response.GetResponseStream());
				}
			}
			return response.GetResponseStream();
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00082B6C File Offset: 0x00080D6C
		internal void Remove(string host)
		{
			lock (this)
			{
				OpenedHost openedHost = (OpenedHost)this.connections[host];
				if (openedHost != null)
				{
					OpenedHost openedHost2 = openedHost;
					int num = openedHost2.nonCachedConnectionsCount - 1;
					openedHost2.nonCachedConnectionsCount = num;
					if (num == 0)
					{
						this.connections.Remove(host);
					}
				}
			}
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00082BD8 File Offset: 0x00080DD8
		internal Task<Stream> GetStreamAsync(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
		{
			if (uri.Scheme == "file")
			{
				return Task.Run<Stream>(() => new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1, true));
			}
			return this.GetNonFileStreamAsync(uri, credentials, proxy, cachePolicy);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00082C2C File Offset: 0x00080E2C
		private async Task<Stream> GetNonFileStreamAsync(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
		{
			WebRequest req = WebRequest.Create(uri);
			if (credentials != null)
			{
				req.Credentials = credentials;
			}
			if (proxy != null)
			{
				req.Proxy = proxy;
			}
			if (cachePolicy != null)
			{
				req.CachePolicy = cachePolicy;
			}
			WebResponse webResponse = await Task<WebResponse>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(req.BeginGetResponse), new Func<IAsyncResult, WebResponse>(req.EndGetResponse), null).ConfigureAwait(false);
			HttpWebRequest httpWebRequest = req as HttpWebRequest;
			if (httpWebRequest != null)
			{
				lock (this)
				{
					if (this.connections == null)
					{
						this.connections = new Hashtable();
					}
					OpenedHost openedHost = (OpenedHost)this.connections[httpWebRequest.Address.Host];
					if (openedHost == null)
					{
						openedHost = new OpenedHost();
					}
					if (openedHost.nonCachedConnectionsCount < httpWebRequest.ServicePoint.ConnectionLimit - 1)
					{
						if (openedHost.nonCachedConnectionsCount == 0)
						{
							this.connections.Add(httpWebRequest.Address.Host, openedHost);
						}
						openedHost.nonCachedConnectionsCount++;
						return new XmlRegisteredNonCachedStream(webResponse.GetResponseStream(), this, httpWebRequest.Address.Host);
					}
					return new XmlCachedStream(webResponse.ResponseUri, webResponse.GetResponseStream());
				}
			}
			return webResponse.GetResponseStream();
		}

		// Token: 0x040012BE RID: 4798
		private Hashtable connections;
	}
}
