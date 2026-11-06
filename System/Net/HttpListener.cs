using System;
using System.Collections;
using System.IO;
using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mono.Net.Security.Private;
using Mono.Security.Authenticode;
using Mono.Security.Interface;

namespace System.Net
{
	/// <summary>Provides a simple, programmatically controlled HTTP protocol listener. This class cannot be inherited.</summary>
	// Token: 0x02000688 RID: 1672
	public sealed class HttpListener : IDisposable
	{
		// Token: 0x060034AF RID: 13487 RVA: 0x000B8579 File Offset: 0x000B6779
		internal HttpListener(X509Certificate certificate, MonoTlsProvider tlsProvider, MonoTlsSettings tlsSettings)
			: this()
		{
			this.certificate = certificate;
			this.tlsProvider = tlsProvider;
			this.tlsSettings = tlsSettings;
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000B8598 File Offset: 0x000B6798
		internal X509Certificate LoadCertificateAndKey(IPAddress addr, int port)
		{
			object internalLock = this._internalLock;
			X509Certificate x509Certificate;
			lock (internalLock)
			{
				if (this.certificate != null)
				{
					x509Certificate = this.certificate;
				}
				else
				{
					try
					{
						string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono");
						text = Path.Combine(text, "httplistener");
						string text2 = Path.Combine(text, string.Format("{0}.cer", port));
						if (!File.Exists(text2))
						{
							x509Certificate = null;
						}
						else
						{
							string text3 = Path.Combine(text, string.Format("{0}.pvk", port));
							if (!File.Exists(text3))
							{
								x509Certificate = null;
							}
							else
							{
								X509Certificate2 x509Certificate2 = new X509Certificate2(text2);
								RSA rsa = PrivateKey.CreateFromFile(text3).RSA;
								this.certificate = new X509Certificate2((X509Certificate2Impl)x509Certificate2.Impl.CopyWithPrivateKey(rsa));
								x509Certificate = this.certificate;
							}
						}
					}
					catch
					{
						this.certificate = null;
						x509Certificate = null;
					}
				}
			}
			return x509Certificate;
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000B86A4 File Offset: 0x000B68A4
		internal SslStream CreateSslStream(Stream innerStream, bool ownsStream, RemoteCertificateValidationCallback callback)
		{
			object internalLock = this._internalLock;
			SslStream sslStream;
			lock (internalLock)
			{
				if (this.tlsProvider == null)
				{
					this.tlsProvider = MonoTlsProviderFactory.GetProvider();
				}
				MonoTlsSettings monoTlsSettings = (this.tlsSettings ?? MonoTlsSettings.DefaultSettings).Clone();
				monoTlsSettings.RemoteCertificateValidationCallback = CallbackHelpers.PublicToMono(callback);
				sslStream = new SslStream(innerStream, ownsStream, this.tlsProvider, monoTlsSettings);
			}
			return sslStream;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListener" /> class.</summary>
		/// <exception cref="T:System.PlatformNotSupportedException">This class cannot be used on the current operating system. Windows Server 2003 or Windows XP SP2 is required to use instances of this class.</exception>
		// Token: 0x060034B2 RID: 13490 RVA: 0x000B8724 File Offset: 0x000B6924
		public HttpListener()
		{
			this._internalLock = new object();
			this.prefixes = new HttpListenerPrefixCollection(this);
			this.registry = new Hashtable();
			this.connections = Hashtable.Synchronized(new Hashtable());
			this.ctx_queue = new ArrayList();
			this.wait_queue = new ArrayList();
			this.auth_schemes = AuthenticationSchemes.Anonymous;
			this.defaultServiceNames = new ServiceNameStore();
			this.extendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
		}

		/// <summary>Gets or sets the scheme used to authenticate clients.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Net.AuthenticationSchemes" /> enumeration values that indicates how clients are to be authenticated. The default value is <see cref="F:System.Net.AuthenticationSchemes.Anonymous" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060034B3 RID: 13491 RVA: 0x000B87A1 File Offset: 0x000B69A1
		// (set) Token: 0x060034B4 RID: 13492 RVA: 0x000B87A9 File Offset: 0x000B69A9
		public AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this.auth_schemes;
			}
			set
			{
				this.CheckDisposed();
				this.auth_schemes = value;
			}
		}

		/// <summary>Gets or sets the delegate called to determine the protocol used to authenticate clients.</summary>
		/// <returns>An <see cref="T:System.Net.AuthenticationSchemeSelector" /> delegate that invokes the method used to select an authentication protocol. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060034B5 RID: 13493 RVA: 0x000B87B8 File Offset: 0x000B69B8
		// (set) Token: 0x060034B6 RID: 13494 RVA: 0x000B87C0 File Offset: 0x000B69C0
		public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
		{
			get
			{
				return this.auth_selector;
			}
			set
			{
				this.CheckDisposed();
				this.auth_selector = value;
			}
		}

		/// <summary>Gets or sets the delegate called to determine the <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> to use for each request.</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> that specifies the policy to use for extended protection.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property, but the <see cref="P:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy.CustomChannelBinding" /> property must be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property to <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property after the <see cref="M:System.Net.HttpListener.Start" /> method was already called.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property on a platform that does not support extended protection.</exception>
		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x060034B7 RID: 13495 RVA: 0x000B87CF File Offset: 0x000B69CF
		// (set) Token: 0x060034B8 RID: 13496 RVA: 0x000B87D7 File Offset: 0x000B69D7
		public HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
		{
			get
			{
				return this.extendedProtectionSelectorDelegate;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!AuthenticationManager.OSSupportsExtendedProtection)
				{
					throw new PlatformNotSupportedException(SR.GetString("This operation requires OS support for extended protection."));
				}
				this.extendedProtectionSelectorDelegate = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether your application receives exceptions that occur when an <see cref="T:System.Net.HttpListener" /> sends the response to the client.</summary>
		/// <returns>
		///   <see langword="true" /> if this <see cref="T:System.Net.HttpListener" /> should not return exceptions that occur when sending the response to the client; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060034B9 RID: 13497 RVA: 0x000B8806 File Offset: 0x000B6A06
		// (set) Token: 0x060034BA RID: 13498 RVA: 0x000B880E File Offset: 0x000B6A0E
		public bool IgnoreWriteExceptions
		{
			get
			{
				return this.ignore_write_exceptions;
			}
			set
			{
				this.CheckDisposed();
				this.ignore_write_exceptions = value;
			}
		}

		/// <summary>Gets a value that indicates whether <see cref="T:System.Net.HttpListener" /> has been started.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.HttpListener" /> was started; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060034BB RID: 13499 RVA: 0x000B881D File Offset: 0x000B6A1D
		public bool IsListening
		{
			get
			{
				return this.listening;
			}
		}

		/// <summary>Gets a value that indicates whether <see cref="T:System.Net.HttpListener" /> can be used with the current operating system.</summary>
		/// <returns>
		///   <see langword="true" /> if <see cref="T:System.Net.HttpListener" /> is supported; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060034BC RID: 13500 RVA: 0x0000390E File Offset: 0x00001B0E
		public static bool IsSupported
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) prefixes handled by this <see cref="T:System.Net.HttpListener" /> object.</summary>
		/// <returns>An <see cref="T:System.Net.HttpListenerPrefixCollection" /> that contains the URI prefixes that this <see cref="T:System.Net.HttpListener" /> object is configured to handle.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060034BD RID: 13501 RVA: 0x000B8825 File Offset: 0x000B6A25
		public HttpListenerPrefixCollection Prefixes
		{
			get
			{
				this.CheckDisposed();
				return this.prefixes;
			}
		}

		/// <summary>The timeout manager for this <see cref="T:System.Net.HttpListener" /> instance.</summary>
		/// <returns>The timeout manager for this <see cref="T:System.Net.HttpListener" /> instance.</returns>
		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060034BE RID: 13502 RVA: 0x0000829A File Offset: 0x0000649A
		[MonoTODO]
		public HttpListenerTimeoutManager TimeoutManager
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> to use for extended protection for a session.</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> that specifies the policy to use for extended protection.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionPolicy" /> property, but the <see cref="P:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy.CustomChannelBinding" /> property was not <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionPolicy" /> property to <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionPolicy" /> property after the <see cref="M:System.Net.HttpListener.Start" /> method was already called.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="P:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy.PolicyEnforcement" /> property was set to <see cref="F:System.Security.Authentication.ExtendedProtection.PolicyEnforcement.Always" /> on a platform that does not support extended protection.</exception>
		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060034BF RID: 13503 RVA: 0x000B8833 File Offset: 0x000B6A33
		// (set) Token: 0x060034C0 RID: 13504 RVA: 0x000B883C File Offset: 0x000B6A3C
		[MonoTODO("not used anywhere in the implementation")]
		public ExtendedProtectionPolicy ExtendedProtectionPolicy
		{
			get
			{
				return this.extendedProtectionPolicy;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!AuthenticationManager.OSSupportsExtendedProtection && value.PolicyEnforcement == PolicyEnforcement.Always)
				{
					throw new PlatformNotSupportedException(SR.GetString("This operation requires OS support for extended protection."));
				}
				if (value.CustomChannelBinding != null)
				{
					throw new ArgumentException(SR.GetString("Custom channel bindings are not supported."), "CustomChannelBinding");
				}
				this.extendedProtectionPolicy = value;
			}
		}

		/// <summary>Gets a default list of Service Provider Names (SPNs) as determined by registered prefixes.</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.ExtendedProtection.ServiceNameCollection" /> that contains a list of SPNs.</returns>
		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060034C1 RID: 13505 RVA: 0x000B88A1 File Offset: 0x000B6AA1
		public ServiceNameCollection DefaultServiceNames
		{
			get
			{
				return this.defaultServiceNames.ServiceNames;
			}
		}

		/// <summary>Gets or sets the realm, or resource partition, associated with this <see cref="T:System.Net.HttpListener" /> object.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the name of the realm associated with the <see cref="T:System.Net.HttpListener" /> object.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060034C2 RID: 13506 RVA: 0x000B88AE File Offset: 0x000B6AAE
		// (set) Token: 0x060034C3 RID: 13507 RVA: 0x000B88B6 File Offset: 0x000B6AB6
		public string Realm
		{
			get
			{
				return this.realm;
			}
			set
			{
				this.CheckDisposed();
				this.realm = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether, when NTLM is used, additional requests using the same Transmission Control Protocol (TCP) connection are required to authenticate.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Security.Principal.IIdentity" /> of the first request will be used for subsequent requests on the same connection; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060034C4 RID: 13508 RVA: 0x000B88C5 File Offset: 0x000B6AC5
		// (set) Token: 0x060034C5 RID: 13509 RVA: 0x000B88CD File Offset: 0x000B6ACD
		[MonoTODO("Support for NTLM needs some loving.")]
		public bool UnsafeConnectionNtlmAuthentication
		{
			get
			{
				return this.unsafe_ntlm_auth;
			}
			set
			{
				this.CheckDisposed();
				this.unsafe_ntlm_auth = value;
			}
		}

		/// <summary>Shuts down the <see cref="T:System.Net.HttpListener" /> object immediately, discarding all currently queued requests.</summary>
		// Token: 0x060034C6 RID: 13510 RVA: 0x000B88DC File Offset: 0x000B6ADC
		public void Abort()
		{
			if (this.disposed)
			{
				return;
			}
			if (!this.listening)
			{
				return;
			}
			this.Close(true);
		}

		/// <summary>Shuts down the <see cref="T:System.Net.HttpListener" />.</summary>
		// Token: 0x060034C7 RID: 13511 RVA: 0x000B88F7 File Offset: 0x000B6AF7
		public void Close()
		{
			if (this.disposed)
			{
				return;
			}
			if (!this.listening)
			{
				this.disposed = true;
				return;
			}
			this.Close(true);
			this.disposed = true;
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000B8920 File Offset: 0x000B6B20
		private void Close(bool force)
		{
			this.CheckDisposed();
			EndPointManager.RemoveListener(this);
			this.Cleanup(force);
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000B8938 File Offset: 0x000B6B38
		private void Cleanup(bool close_existing)
		{
			object internalLock = this._internalLock;
			lock (internalLock)
			{
				if (close_existing)
				{
					ICollection keys = this.registry.Keys;
					HttpListenerContext[] array = new HttpListenerContext[keys.Count];
					keys.CopyTo(array, 0);
					this.registry.Clear();
					for (int i = array.Length - 1; i >= 0; i--)
					{
						array[i].Connection.Close(true);
					}
				}
				object syncRoot = this.connections.SyncRoot;
				lock (syncRoot)
				{
					ICollection keys2 = this.connections.Keys;
					HttpConnection[] array2 = new HttpConnection[keys2.Count];
					keys2.CopyTo(array2, 0);
					this.connections.Clear();
					for (int j = array2.Length - 1; j >= 0; j--)
					{
						array2[j].Close(true);
					}
				}
				ArrayList arrayList = this.ctx_queue;
				lock (arrayList)
				{
					HttpListenerContext[] array3 = (HttpListenerContext[])this.ctx_queue.ToArray(typeof(HttpListenerContext));
					this.ctx_queue.Clear();
					for (int k = array3.Length - 1; k >= 0; k--)
					{
						array3[k].Connection.Close(true);
					}
				}
				arrayList = this.wait_queue;
				lock (arrayList)
				{
					Exception ex = new ObjectDisposedException("listener");
					foreach (object obj in this.wait_queue)
					{
						((ListenerAsyncResult)obj).Complete(ex);
					}
					this.wait_queue.Clear();
				}
			}
		}

		/// <summary>Begins asynchronously retrieving an incoming request.</summary>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when a client request is available.</param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="callback" /> delegate when the operation completes.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation.</returns>
		/// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
		/// <exception cref="T:System.InvalidOperationException">This object has not been started or is currently stopped.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
		// Token: 0x060034CA RID: 13514 RVA: 0x000B8B84 File Offset: 0x000B6D84
		public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
		{
			this.CheckDisposed();
			if (!this.listening)
			{
				throw new InvalidOperationException("Please, call Start before using this method.");
			}
			ListenerAsyncResult listenerAsyncResult = new ListenerAsyncResult(callback, state);
			ArrayList arrayList = this.wait_queue;
			lock (arrayList)
			{
				ArrayList arrayList2 = this.ctx_queue;
				lock (arrayList2)
				{
					HttpListenerContext contextFromQueue = this.GetContextFromQueue();
					if (contextFromQueue != null)
					{
						listenerAsyncResult.Complete(contextFromQueue, true);
						return listenerAsyncResult;
					}
				}
				this.wait_queue.Add(listenerAsyncResult);
			}
			return listenerAsyncResult;
		}

		/// <summary>Completes an asynchronous operation to retrieve an incoming client request.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that was obtained when the asynchronous operation was started.</param>
		/// <returns>An <see cref="T:System.Net.HttpListenerContext" /> object that represents the client request.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not obtained by calling the <see cref="M:System.Net.HttpListener.BeginGetContext(System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.HttpListener.EndGetContext(System.IAsyncResult)" /> method was already called for the specified <paramref name="asyncResult" /> object.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
		// Token: 0x060034CB RID: 13515 RVA: 0x000B8C38 File Offset: 0x000B6E38
		public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
		{
			this.CheckDisposed();
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			ListenerAsyncResult listenerAsyncResult = asyncResult as ListenerAsyncResult;
			if (listenerAsyncResult == null)
			{
				throw new ArgumentException("Wrong IAsyncResult.", "asyncResult");
			}
			if (listenerAsyncResult.EndCalled)
			{
				throw new ArgumentException("Cannot reuse this IAsyncResult");
			}
			listenerAsyncResult.EndCalled = true;
			if (!listenerAsyncResult.IsCompleted)
			{
				listenerAsyncResult.AsyncWaitHandle.WaitOne();
			}
			ArrayList arrayList = this.wait_queue;
			lock (arrayList)
			{
				int num = this.wait_queue.IndexOf(listenerAsyncResult);
				if (num >= 0)
				{
					this.wait_queue.RemoveAt(num);
				}
			}
			HttpListenerContext context = listenerAsyncResult.GetContext();
			context.ParseAuthentication(this.SelectAuthenticationScheme(context));
			return context;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000B8D04 File Offset: 0x000B6F04
		internal AuthenticationSchemes SelectAuthenticationScheme(HttpListenerContext context)
		{
			if (this.AuthenticationSchemeSelectorDelegate != null)
			{
				return this.AuthenticationSchemeSelectorDelegate(context.Request);
			}
			return this.auth_schemes;
		}

		/// <summary>Waits for an incoming request and returns when one is received.</summary>
		/// <returns>An <see cref="T:System.Net.HttpListenerContext" /> object that represents a client request.</returns>
		/// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
		/// <exception cref="T:System.InvalidOperationException">This object has not been started or is currently stopped.  
		///  -or-  
		///  The <see cref="T:System.Net.HttpListener" /> does not have any Uniform Resource Identifier (URI) prefixes to respond to.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
		// Token: 0x060034CD RID: 13517 RVA: 0x000B8D28 File Offset: 0x000B6F28
		public HttpListenerContext GetContext()
		{
			if (this.prefixes.Count == 0)
			{
				throw new InvalidOperationException("Please, call AddPrefix before using this method.");
			}
			ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)this.BeginGetContext(null, null);
			listenerAsyncResult.InGet = true;
			return this.EndGetContext(listenerAsyncResult);
		}

		/// <summary>Allows this instance to receive incoming requests.</summary>
		/// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
		// Token: 0x060034CE RID: 13518 RVA: 0x000B8D69 File Offset: 0x000B6F69
		public void Start()
		{
			this.CheckDisposed();
			if (this.listening)
			{
				return;
			}
			EndPointManager.AddListener(this);
			this.listening = true;
		}

		/// <summary>Causes this instance to stop receiving incoming requests.</summary>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		// Token: 0x060034CF RID: 13519 RVA: 0x000B8D87 File Offset: 0x000B6F87
		public void Stop()
		{
			this.CheckDisposed();
			this.listening = false;
			this.Close(false);
		}

		/// <summary>Releases the resources held by this <see cref="T:System.Net.HttpListener" /> object.</summary>
		// Token: 0x060034D0 RID: 13520 RVA: 0x000B8D9D File Offset: 0x000B6F9D
		void IDisposable.Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.Close(true);
			this.disposed = true;
		}

		/// <summary>Waits for an incoming request as an asynchronous operation.</summary>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns an <see cref="T:System.Net.HttpListenerContext" /> object that represents a client request.</returns>
		// Token: 0x060034D1 RID: 13521 RVA: 0x000B8DB6 File Offset: 0x000B6FB6
		public Task<HttpListenerContext> GetContextAsync()
		{
			return Task<HttpListenerContext>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetContext), new Func<IAsyncResult, HttpListenerContext>(this.EndGetContext), null);
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000B8DDB File Offset: 0x000B6FDB
		internal void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000B8DF6 File Offset: 0x000B6FF6
		private HttpListenerContext GetContextFromQueue()
		{
			if (this.ctx_queue.Count == 0)
			{
				return null;
			}
			HttpListenerContext httpListenerContext = (HttpListenerContext)this.ctx_queue[0];
			this.ctx_queue.RemoveAt(0);
			return httpListenerContext;
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000B8E24 File Offset: 0x000B7024
		internal void RegisterContext(HttpListenerContext context)
		{
			object internalLock = this._internalLock;
			lock (internalLock)
			{
				this.registry[context] = context;
			}
			ListenerAsyncResult listenerAsyncResult = null;
			ArrayList arrayList = this.wait_queue;
			lock (arrayList)
			{
				if (this.wait_queue.Count == 0)
				{
					ArrayList arrayList2 = this.ctx_queue;
					lock (arrayList2)
					{
						this.ctx_queue.Add(context);
						goto IL_00A3;
					}
				}
				listenerAsyncResult = (ListenerAsyncResult)this.wait_queue[0];
				this.wait_queue.RemoveAt(0);
			}
			IL_00A3:
			if (listenerAsyncResult != null)
			{
				listenerAsyncResult.Complete(context);
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000B8F08 File Offset: 0x000B7108
		internal void UnregisterContext(HttpListenerContext context)
		{
			object internalLock = this._internalLock;
			lock (internalLock)
			{
				this.registry.Remove(context);
			}
			ArrayList arrayList = this.ctx_queue;
			lock (arrayList)
			{
				int num = this.ctx_queue.IndexOf(context);
				if (num >= 0)
				{
					this.ctx_queue.RemoveAt(num);
				}
			}
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000B8F94 File Offset: 0x000B7194
		internal void AddConnection(HttpConnection cnc)
		{
			this.connections[cnc] = cnc;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000B8FA3 File Offset: 0x000B71A3
		internal void RemoveConnection(HttpConnection cnc)
		{
			this.connections.Remove(cnc);
		}

		// Token: 0x04001ECC RID: 7884
		private MonoTlsProvider tlsProvider;

		// Token: 0x04001ECD RID: 7885
		private MonoTlsSettings tlsSettings;

		// Token: 0x04001ECE RID: 7886
		private X509Certificate certificate;

		// Token: 0x04001ECF RID: 7887
		private AuthenticationSchemes auth_schemes;

		// Token: 0x04001ED0 RID: 7888
		private HttpListenerPrefixCollection prefixes;

		// Token: 0x04001ED1 RID: 7889
		private AuthenticationSchemeSelector auth_selector;

		// Token: 0x04001ED2 RID: 7890
		private string realm;

		// Token: 0x04001ED3 RID: 7891
		private bool ignore_write_exceptions;

		// Token: 0x04001ED4 RID: 7892
		private bool unsafe_ntlm_auth;

		// Token: 0x04001ED5 RID: 7893
		private bool listening;

		// Token: 0x04001ED6 RID: 7894
		private bool disposed;

		// Token: 0x04001ED7 RID: 7895
		private readonly object _internalLock;

		// Token: 0x04001ED8 RID: 7896
		private Hashtable registry;

		// Token: 0x04001ED9 RID: 7897
		private ArrayList ctx_queue;

		// Token: 0x04001EDA RID: 7898
		private ArrayList wait_queue;

		// Token: 0x04001EDB RID: 7899
		private Hashtable connections;

		// Token: 0x04001EDC RID: 7900
		private ServiceNameStore defaultServiceNames;

		// Token: 0x04001EDD RID: 7901
		private ExtendedProtectionPolicy extendedProtectionPolicy;

		// Token: 0x04001EDE RID: 7902
		private HttpListener.ExtendedProtectionSelector extendedProtectionSelectorDelegate;

		/// <summary>A delegate called to determine the <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> to use for each <see cref="T:System.Net.HttpListener" /> request.</summary>
		/// <param name="request">The <see cref="T:System.Net.HttpListenerRequest" /> to determine the extended protection policy that the <see cref="T:System.Net.HttpListener" /> instance will use to provide extended protection.</param>
		/// <returns>An <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> object that specifies the extended protection policy to use for this request.</returns>
		// Token: 0x02000689 RID: 1673
		// (Invoke) Token: 0x060034D9 RID: 13529
		public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);
	}
}
