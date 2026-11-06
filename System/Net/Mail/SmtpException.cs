using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	/// <summary>Represents the exception that is thrown when the <see cref="T:System.Net.Mail.SmtpClient" /> is not able to complete a <see cref="Overload:System.Net.Mail.SmtpClient.Send" /> or <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> operation.</summary>
	// Token: 0x0200080E RID: 2062
	[Serializable]
	public class SmtpException : Exception, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class.</summary>
		// Token: 0x06004201 RID: 16897 RVA: 0x000E4746 File Offset: 0x000E2946
		public SmtpException()
			: this(SmtpStatusCode.GeneralFailure)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified status code.</summary>
		/// <param name="statusCode">An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value.</param>
		// Token: 0x06004202 RID: 16898 RVA: 0x000E474F File Offset: 0x000E294F
		public SmtpException(SmtpStatusCode statusCode)
			: this(statusCode, "Syntax error, command unrecognized.")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		// Token: 0x06004203 RID: 16899 RVA: 0x000E475D File Offset: 0x000E295D
		public SmtpException(string message)
			: this(SmtpStatusCode.GeneralFailure, message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information required to serialize the new <see cref="T:System.Net.Mail.SmtpException" />.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source and destination of the serialized stream associated with the new instance.</param>
		// Token: 0x06004204 RID: 16900 RVA: 0x000E4768 File Offset: 0x000E2968
		protected SmtpException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			try
			{
				this.statusCode = (SmtpStatusCode)serializationInfo.GetValue("Status", typeof(int));
			}
			catch (SerializationException)
			{
				this.statusCode = (SmtpStatusCode)serializationInfo.GetValue("statusCode", typeof(SmtpStatusCode));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified status code and error message.</summary>
		/// <param name="statusCode">An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value.</param>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		// Token: 0x06004205 RID: 16901 RVA: 0x000E47D4 File Offset: 0x000E29D4
		public SmtpException(SmtpStatusCode statusCode, string message)
			: base(message)
		{
			this.statusCode = statusCode;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified error message and inner exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		// Token: 0x06004206 RID: 16902 RVA: 0x000E47E4 File Offset: 0x000E29E4
		public SmtpException(string message, Exception innerException)
			: base(message, innerException)
		{
			this.statusCode = SmtpStatusCode.GeneralFailure;
		}

		/// <summary>Gets the status code returned by an SMTP server when an email message is transmitted.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value that indicates the error that occurred.</returns>
		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x000E47F5 File Offset: 0x000E29F5
		// (set) Token: 0x06004208 RID: 16904 RVA: 0x000E47FD File Offset: 0x000E29FD
		public SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				this.statusCode = value;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data needed to serialize the <see cref="T:System.Net.Mail.SmtpException" />.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		// Token: 0x06004209 RID: 16905 RVA: 0x000E4806 File Offset: 0x000E2A06
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("Status", this.statusCode, typeof(int));
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data needed to serialize the <see cref="T:System.Net.Mail.SmtpException" />.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" />, which holds the serialized data for the <see cref="T:System.Net.Mail.SmtpException" />.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the destination of the serialized stream associated with the new <see cref="T:System.Net.Mail.SmtpException" />.</param>
		// Token: 0x0600420A RID: 16906 RVA: 0x000A7812 File Offset: 0x000A5A12
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		// Token: 0x0400277E RID: 10110
		private SmtpStatusCode statusCode;
	}
}
