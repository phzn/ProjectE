using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Configuration;

namespace System.Net
{
	/// <summary>Manages the authentication modules called during the client authentication process.</summary>
	// Token: 0x02000669 RID: 1641
	public class AuthenticationManager
	{
		// Token: 0x060033C3 RID: 13251 RVA: 0x0000219B File Offset: 0x0000039B
		private AuthenticationManager()
		{
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000B454C File Offset: 0x000B274C
		private static void EnsureModules()
		{
			object obj = AuthenticationManager.locker;
			lock (obj)
			{
				if (AuthenticationManager.modules == null)
				{
					AuthenticationManager.modules = new ArrayList();
					AuthenticationModulesSection authenticationModulesSection = ConfigurationManager.GetSection("system.net/authenticationModules") as AuthenticationModulesSection;
					if (authenticationModulesSection != null)
					{
						foreach (object obj2 in authenticationModulesSection.AuthenticationModules)
						{
							AuthenticationModuleElement authenticationModuleElement = (AuthenticationModuleElement)obj2;
							IAuthenticationModule authenticationModule = null;
							try
							{
								authenticationModule = (IAuthenticationModule)Activator.CreateInstance(Type.GetType(authenticationModuleElement.Type, true));
							}
							catch
							{
							}
							AuthenticationManager.modules.Add(authenticationModule);
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the credential policy to be used for resource requests made using the <see cref="T:System.Net.HttpWebRequest" /> class.</summary>
		/// <returns>An object that implements the <see cref="T:System.Net.ICredentialPolicy" /> interface that determines whether credentials are sent with requests. The default value is <see langword="null" />.</returns>
		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000B4630 File Offset: 0x000B2830
		// (set) Token: 0x060033C6 RID: 13254 RVA: 0x000B4637 File Offset: 0x000B2837
		public static ICredentialPolicy CredentialPolicy
		{
			get
			{
				return AuthenticationManager.credential_policy;
			}
			set
			{
				AuthenticationManager.credential_policy = value;
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x0001FD2F File Offset: 0x0001DF2F
		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets the dictionary that contains Service Principal Names (SPNs) that are used to identify hosts during Kerberos authentication for requests made using <see cref="T:System.Net.WebRequest" /> and its derived classes.</summary>
		/// <returns>A writable <see cref="T:System.Collections.Specialized.StringDictionary" /> that contains the SPN values for keys composed of host information.</returns>
		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x000B463F File Offset: 0x000B283F
		[MonoTODO]
		public static StringDictionary CustomTargetNameDictionary
		{
			get
			{
				throw AuthenticationManager.GetMustImplement();
			}
		}

		/// <summary>Gets a list of authentication modules that are registered with the authentication manager.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that enables the registered authentication modules to be read.</returns>
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x060033C9 RID: 13257 RVA: 0x000B4646 File Offset: 0x000B2846
		public static IEnumerator RegisteredModules
		{
			get
			{
				AuthenticationManager.EnsureModules();
				return AuthenticationManager.modules.GetEnumerator();
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x060033CA RID: 13258 RVA: 0x00003062 File Offset: 0x00001262
		[MonoTODO]
		internal static bool OSSupportsExtendedProtection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000B4658 File Offset: 0x000B2858
		internal static void Clear()
		{
			AuthenticationManager.EnsureModules();
			ArrayList arrayList = AuthenticationManager.modules;
			lock (arrayList)
			{
				AuthenticationManager.modules.Clear();
			}
		}

		/// <summary>Calls each registered authentication module to find the first module that can respond to the authentication request.</summary>
		/// <param name="challenge">The challenge returned by the Internet resource.</param>
		/// <param name="request">The <see cref="T:System.Net.WebRequest" /> that initiated the authentication challenge.</param>
		/// <param name="credentials">The <see cref="T:System.Net.ICredentials" /> associated with this request.</param>
		/// <returns>An instance of the <see cref="T:System.Net.Authorization" /> class containing the result of the authorization attempt. If there is no authentication module to respond to the challenge, this method returns <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="challenge" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="request" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="credentials" /> is <see langword="null" />.</exception>
		// Token: 0x060033CC RID: 13260 RVA: 0x000B46A0 File Offset: 0x000B28A0
		public static Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}
			if (challenge == null)
			{
				throw new ArgumentNullException("challenge");
			}
			return AuthenticationManager.DoAuthenticate(challenge, request, credentials);
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000B46D4 File Offset: 0x000B28D4
		private static Authorization DoAuthenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			AuthenticationManager.EnsureModules();
			ArrayList arrayList = AuthenticationManager.modules;
			lock (arrayList)
			{
				foreach (object obj in AuthenticationManager.modules)
				{
					IAuthenticationModule authenticationModule = (IAuthenticationModule)obj;
					Authorization authorization = authenticationModule.Authenticate(challenge, request, credentials);
					if (authorization != null)
					{
						authorization.ModuleAuthenticationType = authenticationModule.AuthenticationType;
						return authorization;
					}
				}
			}
			return null;
		}

		/// <summary>Preauthenticates a request.</summary>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> to an Internet resource.</param>
		/// <param name="credentials">The <see cref="T:System.Net.ICredentials" /> associated with the request.</param>
		/// <returns>An instance of the <see cref="T:System.Net.Authorization" /> class if the request can be preauthenticated; otherwise, <see langword="null" />. If <paramref name="credentials" /> is <see langword="null" />, this method returns <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="request" /> is <see langword="null" />.</exception>
		// Token: 0x060033CE RID: 13262 RVA: 0x000B477C File Offset: 0x000B297C
		public static Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				return null;
			}
			AuthenticationManager.EnsureModules();
			ArrayList arrayList = AuthenticationManager.modules;
			lock (arrayList)
			{
				foreach (object obj in AuthenticationManager.modules)
				{
					IAuthenticationModule authenticationModule = (IAuthenticationModule)obj;
					Authorization authorization = authenticationModule.PreAuthenticate(request, credentials);
					if (authorization != null)
					{
						authorization.ModuleAuthenticationType = authenticationModule.AuthenticationType;
						return authorization;
					}
				}
			}
			return null;
		}

		/// <summary>Registers an authentication module with the authentication manager.</summary>
		/// <param name="authenticationModule">The <see cref="T:System.Net.IAuthenticationModule" /> to register with the authentication manager.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="authenticationModule" /> is <see langword="null" />.</exception>
		// Token: 0x060033CF RID: 13263 RVA: 0x000B4834 File Offset: 0x000B2A34
		public static void Register(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			AuthenticationManager.DoUnregister(authenticationModule.AuthenticationType, false);
			ArrayList arrayList = AuthenticationManager.modules;
			lock (arrayList)
			{
				AuthenticationManager.modules.Add(authenticationModule);
			}
		}

		/// <summary>Removes the specified authentication module from the list of registered modules.</summary>
		/// <param name="authenticationModule">The <see cref="T:System.Net.IAuthenticationModule" /> to remove from the list of registered modules.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="authenticationModule" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The specified <see cref="T:System.Net.IAuthenticationModule" /> is not registered.</exception>
		// Token: 0x060033D0 RID: 13264 RVA: 0x000B4894 File Offset: 0x000B2A94
		public static void Unregister(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			AuthenticationManager.DoUnregister(authenticationModule.AuthenticationType, true);
		}

		/// <summary>Removes authentication modules with the specified authentication scheme from the list of registered modules.</summary>
		/// <param name="authenticationScheme">The authentication scheme of the module to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="authenticationScheme" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">A module for this authentication scheme is not registered.</exception>
		// Token: 0x060033D1 RID: 13265 RVA: 0x000B48B0 File Offset: 0x000B2AB0
		public static void Unregister(string authenticationScheme)
		{
			if (authenticationScheme == null)
			{
				throw new ArgumentNullException("authenticationScheme");
			}
			AuthenticationManager.DoUnregister(authenticationScheme, true);
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000B48C8 File Offset: 0x000B2AC8
		private static void DoUnregister(string authenticationScheme, bool throwEx)
		{
			AuthenticationManager.EnsureModules();
			ArrayList arrayList = AuthenticationManager.modules;
			lock (arrayList)
			{
				IAuthenticationModule authenticationModule = null;
				foreach (object obj in AuthenticationManager.modules)
				{
					IAuthenticationModule authenticationModule2 = (IAuthenticationModule)obj;
					if (string.Compare(authenticationModule2.AuthenticationType, authenticationScheme, true) == 0)
					{
						authenticationModule = authenticationModule2;
						break;
					}
				}
				if (authenticationModule == null)
				{
					if (throwEx)
					{
						throw new InvalidOperationException("Scheme not registered.");
					}
				}
				else
				{
					AuthenticationManager.modules.Remove(authenticationModule);
				}
			}
		}

		// Token: 0x04001E5A RID: 7770
		private static ArrayList modules;

		// Token: 0x04001E5B RID: 7771
		private static object locker = new object();

		// Token: 0x04001E5C RID: 7772
		private static ICredentialPolicy credential_policy = null;
	}
}
