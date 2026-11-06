using System;

namespace System.Net.Mail
{
	/// <summary>Specifies the outcome of sending email by using the <see cref="T:System.Net.Mail.SmtpClient" /> class.</summary>
	// Token: 0x02000813 RID: 2067
	public enum SmtpStatusCode
	{
		/// <summary>The commands were sent in the incorrect sequence.</summary>
		// Token: 0x04002786 RID: 10118
		BadCommandSequence = 503,
		/// <summary>The specified user is not local, but the receiving SMTP service accepted the message and attempted to deliver it. This status code is defined in RFC 1123, which is available at https://www.ietf.org.</summary>
		// Token: 0x04002787 RID: 10119
		CannotVerifyUserWillAttemptDelivery = 252,
		/// <summary>The client was not authenticated or is not allowed to send mail using the specified SMTP host.</summary>
		// Token: 0x04002788 RID: 10120
		ClientNotPermitted = 454,
		/// <summary>The SMTP service does not implement the specified command.</summary>
		// Token: 0x04002789 RID: 10121
		CommandNotImplemented = 502,
		/// <summary>The SMTP service does not implement the specified command parameter.</summary>
		// Token: 0x0400278A RID: 10122
		CommandParameterNotImplemented = 504,
		/// <summary>The SMTP service does not recognize the specified command.</summary>
		// Token: 0x0400278B RID: 10123
		CommandUnrecognized = 500,
		/// <summary>The message is too large to be stored in the destination mailbox.</summary>
		// Token: 0x0400278C RID: 10124
		ExceededStorageAllocation = 552,
		/// <summary>The transaction could not occur. You receive this error when the specified SMTP host cannot be found.</summary>
		// Token: 0x0400278D RID: 10125
		GeneralFailure = -1,
		/// <summary>A Help message was returned by the service.</summary>
		// Token: 0x0400278E RID: 10126
		HelpMessage = 214,
		/// <summary>The SMTP service does not have sufficient storage to complete the request.</summary>
		// Token: 0x0400278F RID: 10127
		InsufficientStorage = 452,
		/// <summary>The SMTP service cannot complete the request. This error can occur if the client's IP address cannot be resolved (that is, a reverse lookup failed). You can also receive this error if the client domain has been identified as an open relay or source for unsolicited email (spam). For details, see RFC 2505, which is available at https://www.ietf.org.</summary>
		// Token: 0x04002790 RID: 10128
		LocalErrorInProcessing = 451,
		/// <summary>The destination mailbox is in use.</summary>
		// Token: 0x04002791 RID: 10129
		MailboxBusy = 450,
		/// <summary>The syntax used to specify the destination mailbox is incorrect.</summary>
		// Token: 0x04002792 RID: 10130
		MailboxNameNotAllowed = 553,
		/// <summary>The destination mailbox was not found or could not be accessed.</summary>
		// Token: 0x04002793 RID: 10131
		MailboxUnavailable = 550,
		/// <summary>The email was successfully sent to the SMTP service.</summary>
		// Token: 0x04002794 RID: 10132
		Ok = 250,
		/// <summary>The SMTP service is closing the transmission channel.</summary>
		// Token: 0x04002795 RID: 10133
		ServiceClosingTransmissionChannel = 221,
		/// <summary>The SMTP service is not available; the server is closing the transmission channel.</summary>
		// Token: 0x04002796 RID: 10134
		ServiceNotAvailable = 421,
		/// <summary>The SMTP service is ready.</summary>
		// Token: 0x04002797 RID: 10135
		ServiceReady = 220,
		/// <summary>The SMTP service is ready to receive the email content.</summary>
		// Token: 0x04002798 RID: 10136
		StartMailInput = 354,
		/// <summary>The syntax used to specify a command or parameter is incorrect.</summary>
		// Token: 0x04002799 RID: 10137
		SyntaxError = 501,
		/// <summary>A system status or system Help reply.</summary>
		// Token: 0x0400279A RID: 10138
		SystemStatus = 211,
		/// <summary>The transaction failed.</summary>
		// Token: 0x0400279B RID: 10139
		TransactionFailed = 554,
		/// <summary>The user mailbox is not located on the receiving server. You should resend using the supplied address information.</summary>
		// Token: 0x0400279C RID: 10140
		UserNotLocalTryAlternatePath = 551,
		/// <summary>The user mailbox is not located on the receiving server; the server forwards the email.</summary>
		// Token: 0x0400279D RID: 10141
		UserNotLocalWillForward = 251,
		/// <summary>The SMTP server is configured to accept only TLS connections, and the SMTP client is attempting to connect by using a non-TLS connection. The solution is for the user to set EnableSsl=true on the SMTP Client.</summary>
		// Token: 0x0400279E RID: 10142
		MustIssueStartTlsFirst = 530
	}
}
