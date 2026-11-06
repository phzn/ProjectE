using System;

namespace System.Net.Mail
{
	/// <summary>Specifies the level of access allowed to a Simple Mail Transport Protocol (SMTP) server.</summary>
	// Token: 0x02000803 RID: 2051
	public enum SmtpAccess
	{
		/// <summary>No access to an SMTP host.</summary>
		// Token: 0x04002744 RID: 10052
		None,
		/// <summary>Connection to an SMTP host on the default port (port 25).</summary>
		// Token: 0x04002745 RID: 10053
		Connect,
		/// <summary>Connection to an SMTP host on any port.</summary>
		// Token: 0x04002746 RID: 10054
		ConnectToUnrestrictedPort
	}
}
