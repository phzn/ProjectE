using System;

namespace System.Net.Mail
{
	// Token: 0x0200080B RID: 2059
	internal class CCredentialsByHost : ICredentialsByHost
	{
		// Token: 0x060041FF RID: 16895 RVA: 0x000E471D File Offset: 0x000E291D
		public CCredentialsByHost(string userName, string password)
		{
			this.userName = userName;
			this.password = password;
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x000E4733 File Offset: 0x000E2933
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			return new NetworkCredential(this.userName, this.password);
		}

		// Token: 0x04002775 RID: 10101
		private string userName;

		// Token: 0x04002776 RID: 10102
		private string password;
	}
}
