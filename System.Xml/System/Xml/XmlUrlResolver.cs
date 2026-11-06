using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml
{
	/// <summary>Resolves external XML resources named by a Uniform Resource Identifier (URI).</summary>
	// Token: 0x02000248 RID: 584
	public class XmlUrlResolver : XmlResolver
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x00084C14 File Offset: 0x00082E14
		private static XmlDownloadManager DownloadManager
		{
			get
			{
				if (XmlUrlResolver.s_DownloadManager == null)
				{
					object obj = new XmlDownloadManager();
					Interlocked.CompareExchange<object>(ref XmlUrlResolver.s_DownloadManager, obj, null);
				}
				return (XmlDownloadManager)XmlUrlResolver.s_DownloadManager;
			}
		}

		/// <summary>Sets credentials used to authenticate web requests.</summary>
		/// <returns>The credentials to be used to authenticate web requests. If this property is not set, the value defaults to <see langword="null" />; that is, the <see langword="XmlUrlResolver" /> has no user credentials.</returns>
		// Token: 0x170003D9 RID: 985
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00084C45 File Offset: 0x00082E45
		public override ICredentials Credentials
		{
			set
			{
				this._credentials = value;
			}
		}

		/// <summary>Gets or sets the network proxy for the underlying <see cref="T:System.Net.WebRequest" /> object.</summary>
		/// <returns>The <see cref="T:System.Net.IWebProxy" /> to use to access the Internet resource.</returns>
		// Token: 0x170003DA RID: 986
		// (set) Token: 0x060015B8 RID: 5560 RVA: 0x00084C4E File Offset: 0x00082E4E
		public IWebProxy Proxy
		{
			set
			{
				this._proxy = value;
			}
		}

		/// <summary>Gets or sets the cache policy for the underlying <see cref="T:System.Net.WebRequest" /> object.</summary>
		/// <returns>The cache policy for the underlying web request.</returns>
		// Token: 0x170003DB RID: 987
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x00084C57 File Offset: 0x00082E57
		public RequestCachePolicy CachePolicy
		{
			set
			{
				this._cachePolicy = value;
			}
		}

		/// <summary>Maps a URI to an object that contains the actual resource.</summary>
		/// <param name="absoluteUri">The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)" />.</param>
		/// <param name="role">Currently not used.</param>
		/// <param name="ofObjectToReturn">The type of object to return. The current implementation only returns <see cref="T:System.IO.Stream" /> objects.</param>
		/// <returns>A stream object or <see langword="null" /> if a type other than stream is specified.</returns>
		/// <exception cref="T:System.Xml.XmlException">
		///         <paramref name="ofObjectToReturn" /> is neither <see langword="null" /> nor a <see langword="Stream" /> type.</exception>
		/// <exception cref="T:System.UriFormatException">The specified URI is not an absolute URI.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="absoluteUri" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Exception">There is a runtime error (for example, an interrupted server connection).</exception>
		// Token: 0x060015BA RID: 5562 RVA: 0x00084C60 File Offset: 0x00082E60
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream) || ofObjectToReturn == typeof(object))
			{
				return XmlUrlResolver.DownloadManager.GetStream(absoluteUri, this._credentials, this._proxy, this._cachePolicy);
			}
			throw new XmlException("Object type is not supported.", string.Empty);
		}

		/// <summary>Resolves the absolute URI from the base and relative URIs.</summary>
		/// <param name="baseUri">The base URI used to resolve the relative URI.</param>
		/// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative. If absolute, this value effectively replaces the <paramref name="baseUri" /> value. If relative, it combines with the <paramref name="baseUri" /> to make an absolute URI.</param>
		/// <returns>The absolute URI, or <see langword="null" /> if the relative URI cannot be resolved.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="baseUri" /> is <see langword="null" /> or <paramref name="relativeUri" /> is <see langword="null" />.</exception>
		// Token: 0x060015BB RID: 5563 RVA: 0x00084CC7 File Offset: 0x00082EC7
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return base.ResolveUri(baseUri, relativeUri);
		}

		/// <summary>Asynchronously maps a URI to an object that contains the actual resource.</summary>
		/// <param name="absoluteUri">The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)" />.</param>
		/// <param name="role">Currently not used.</param>
		/// <param name="ofObjectToReturn">The type of object to return. The current implementation only returns <see cref="T:System.IO.Stream" /> objects.</param>
		/// <returns>A stream object or <see langword="null" /> if a type other than stream is specified.</returns>
		// Token: 0x060015BC RID: 5564 RVA: 0x00084CD4 File Offset: 0x00082ED4
		public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream) || ofObjectToReturn == typeof(object))
			{
				return await XmlUrlResolver.DownloadManager.GetStreamAsync(absoluteUri, this._credentials, this._proxy, this._cachePolicy).ConfigureAwait(false);
			}
			throw new XmlException("Object type is not supported.", string.Empty);
		}

		// Token: 0x04001325 RID: 4901
		private static object s_DownloadManager;

		// Token: 0x04001326 RID: 4902
		private ICredentials _credentials;

		// Token: 0x04001327 RID: 4903
		private IWebProxy _proxy;

		// Token: 0x04001328 RID: 4904
		private RequestCachePolicy _cachePolicy;
	}
}
