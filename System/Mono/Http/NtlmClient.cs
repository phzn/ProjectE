using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace Mono.Http
{
	// Token: 0x020000CB RID: 203
	internal class NtlmClient : IAuthenticationModule
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0000C99C File Offset: 0x0000AB9C
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || challenge == null)
			{
				return null;
			}
			string text = challenge.Trim();
			int num = text.ToLower().IndexOf("ntlm");
			if (num == -1)
			{
				return null;
			}
			num = text.IndexOfAny(new char[] { ' ', '\t' });
			if (num != -1)
			{
				text = text.Substring(num).Trim();
			}
			else
			{
				text = null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			ConditionalWeakTable<HttpWebRequest, NtlmSession> conditionalWeakTable = NtlmClient.cache;
			Authorization authorization;
			lock (conditionalWeakTable)
			{
				authorization = NtlmClient.cache.GetValue(httpWebRequest, (HttpWebRequest x) => new NtlmSession()).Authenticate(text, webRequest, credentials);
			}
			return authorization;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00002F6A File Offset: 0x0000116A
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return null;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0000CA6C File Offset: 0x0000AC6C
		public string AuthenticationType
		{
			get
			{
				return "NTLM";
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00003062 File Offset: 0x00001262
		public bool CanPreAuthenticate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000391 RID: 913
		private static readonly ConditionalWeakTable<HttpWebRequest, NtlmSession> cache = new ConditionalWeakTable<HttpWebRequest, NtlmSession>();
	}
}
