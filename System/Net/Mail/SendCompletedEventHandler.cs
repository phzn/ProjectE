using System;
using System.ComponentModel;

namespace System.Net.Mail
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Net.Mail.SmtpClient.SendCompleted" /> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> containing event data.</param>
	// Token: 0x02000802 RID: 2050
	// (Invoke) Token: 0x060041A7 RID: 16807
	public delegate void SendCompletedEventHandler(object sender, AsyncCompletedEventArgs e);
}
