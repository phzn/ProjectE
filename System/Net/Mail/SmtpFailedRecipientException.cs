using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	/// <summary>Represents the exception that is thrown when the <see cref="T:System.Net.Mail.SmtpClient" /> is not able to complete a <see cref="Overload:System.Net.Mail.SmtpClient.Send" /> or <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> operation to a particular recipient.</summary>
	// Token: 0x0200080F RID: 2063
	[Serializable]
	public class SmtpFailedRecipientException : SmtpException, ISerializable
	{
		/// <summary>Initializes an empty instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class.</summary>
		// Token: 0x0600420B RID: 16907 RVA: 0x000E483E File Offset: 0x000E2A3E
		public SmtpFailedRecipientException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class with the specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that contains the error message.</param>
		// Token: 0x0600420C RID: 16908 RVA: 0x000E4846 File Offset: 0x000E2A46
		public SmtpFailedRecipientException(string message)
			: base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information required to serialize the new <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source and destination of the serialized stream that is associated with the new instance.</param>
		// Token: 0x0600420D RID: 16909 RVA: 0x000E484F File Offset: 0x000E2A4F
		protected SmtpFailedRecipientException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.failedRecipient = info.GetString("failedRecipient");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class with the specified status code and email address.</summary>
		/// <param name="statusCode">An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value.</param>
		/// <param name="failedRecipient">A <see cref="T:System.String" /> that contains the email address.</param>
		// Token: 0x0600420E RID: 16910 RVA: 0x000E4878 File Offset: 0x000E2A78
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient)
			: base(statusCode)
		{
			this.failedRecipient = failedRecipient;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified error message and inner exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		// Token: 0x0600420F RID: 16911 RVA: 0x000E4888 File Offset: 0x000E2A88
		public SmtpFailedRecipientException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified error message, email address, and inner exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		/// <param name="failedRecipient">A <see cref="T:System.String" /> that contains the email address.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		// Token: 0x06004210 RID: 16912 RVA: 0x000E4892 File Offset: 0x000E2A92
		public SmtpFailedRecipientException(string message, string failedRecipient, Exception innerException)
			: base(message, innerException)
		{
			this.failedRecipient = failedRecipient;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class with the specified status code, email address, and server response.</summary>
		/// <param name="statusCode">An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value.</param>
		/// <param name="failedRecipient">A <see cref="T:System.String" /> that contains the email address.</param>
		/// <param name="serverResponse">A <see cref="T:System.String" /> that contains the server response.</param>
		// Token: 0x06004211 RID: 16913 RVA: 0x000E48A3 File Offset: 0x000E2AA3
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient, string serverResponse)
			: base(statusCode, serverResponse)
		{
			this.failedRecipient = failedRecipient;
		}

		/// <summary>Indicates the email address with delivery difficulties.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the email address.</returns>
		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x000E48B4 File Offset: 0x000E2AB4
		public string FailedRecipient
		{
			get
			{
				return this.failedRecipient;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data that is needed to serialize the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		// Token: 0x06004213 RID: 16915 RVA: 0x000E48BC File Offset: 0x000E2ABC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("failedRecipient", this.failedRecipient);
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data that is needed to serialize the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance, which holds the serialized data for the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> instance that contains the destination of the serialized stream that is associated with the new <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</param>
		// Token: 0x06004214 RID: 16916 RVA: 0x000A7812 File Offset: 0x000A5A12
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0400277F RID: 10111
		private string failedRecipient;
	}
}
