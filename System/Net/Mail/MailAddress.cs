using System;
using System.Globalization;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents the address of an electronic mail sender or recipient.</summary>
	// Token: 0x020007F2 RID: 2034
	public class MailAddress
	{
		// Token: 0x0600410C RID: 16652 RVA: 0x000DF028 File Offset: 0x000DD228
		internal MailAddress(string displayName, string userName, string domain)
		{
			this._host = domain;
			this._userName = userName;
			this._displayName = displayName;
			this._displayNameEncoding = Encoding.GetEncoding("utf-8");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailAddress" /> class using the specified address.</summary>
		/// <param name="address">A <see cref="T:System.String" /> that contains an email address.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not in a recognized format.</exception>
		// Token: 0x0600410D RID: 16653 RVA: 0x000DF055 File Offset: 0x000DD255
		public MailAddress(string address)
			: this(address, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailAddress" /> class using the specified address and display name.</summary>
		/// <param name="address">A <see cref="T:System.String" /> that contains an email address.</param>
		/// <param name="displayName">A <see cref="T:System.String" /> that contains the display name associated with <paramref name="address" />. This parameter can be <see langword="null" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not in a recognized format.  
		/// -or-  
		/// <paramref name="address" /> contains non-ASCII characters.</exception>
		// Token: 0x0600410E RID: 16654 RVA: 0x000DF060 File Offset: 0x000DD260
		public MailAddress(string address, string displayName)
			: this(address, displayName, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailAddress" /> class using the specified address, display name, and encoding.</summary>
		/// <param name="address">A <see cref="T:System.String" /> that contains an email address.</param>
		/// <param name="displayName">A <see cref="T:System.String" /> that contains the display name associated with <paramref name="address" />.</param>
		/// <param name="displayNameEncoding">The <see cref="T:System.Text.Encoding" /> that defines the character set used for <paramref name="displayName" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="displayName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").  
		/// -or-  
		/// <paramref name="displayName" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not in a recognized format.  
		/// -or-  
		/// <paramref name="address" /> contains non-ASCII characters.</exception>
		// Token: 0x0600410F RID: 16655 RVA: 0x000DF06C File Offset: 0x000DD26C
		public MailAddress(string address, string displayName, Encoding displayNameEncoding)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "address"), "address");
			}
			this._displayNameEncoding = displayNameEncoding ?? Encoding.GetEncoding("utf-8");
			this._displayName = displayName ?? string.Empty;
			if (!string.IsNullOrEmpty(this._displayName))
			{
				this._displayName = MailAddressParser.NormalizeOrThrow(this._displayName);
				if (this._displayName.Length >= 2 && this._displayName[0] == '"' && this._displayName[this._displayName.Length - 1] == '"')
				{
					this._displayName = this._displayName.Substring(1, this._displayName.Length - 2);
				}
			}
			MailAddress mailAddress = MailAddressParser.ParseAddress(address);
			this._host = mailAddress._host;
			this._userName = mailAddress._userName;
			if (string.IsNullOrEmpty(this._displayName))
			{
				this._displayName = mailAddress._displayName;
			}
		}

		/// <summary>Gets the display name composed from the display name and address information specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the display name; otherwise, <see cref="F:System.String.Empty" /> ("") if no display name information was specified when this instance was created.</returns>
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06004110 RID: 16656 RVA: 0x000DF188 File Offset: 0x000DD388
		public string DisplayName
		{
			get
			{
				return this._displayName;
			}
		}

		/// <summary>Gets the user information from the address specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the user name portion of the <see cref="P:System.Net.Mail.MailAddress.Address" />.</returns>
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x000DF190 File Offset: 0x000DD390
		public string User
		{
			get
			{
				return this._userName;
			}
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x000DF198 File Offset: 0x000DD398
		private string GetUser(bool allowUnicode)
		{
			if (!allowUnicode && !MimeBasePart.IsAscii(this._userName, true))
			{
				throw new SmtpException(SR.Format("The client or server is only configured for E-mail addresses with ASCII local-parts: {0}.", this.Address));
			}
			return this._userName;
		}

		/// <summary>Gets the host portion of the address specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the host computer that accepts email for the <see cref="P:System.Net.Mail.MailAddress.User" /> property.</returns>
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06004113 RID: 16659 RVA: 0x000DF1C7 File Offset: 0x000DD3C7
		public string Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x000DF1D0 File Offset: 0x000DD3D0
		private string GetHost(bool allowUnicode)
		{
			string text = this._host;
			if (!allowUnicode && !MimeBasePart.IsAscii(text, true))
			{
				IdnMapping idnMapping = new IdnMapping();
				try
				{
					text = idnMapping.GetAscii(text);
				}
				catch (ArgumentException ex)
				{
					throw new SmtpException(SR.Format("The address has an invalid host name: {0}.", this.Address), ex);
				}
			}
			return text;
		}

		/// <summary>Gets the email address specified when this instance was created.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the email address.</returns>
		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x000DF22C File Offset: 0x000DD42C
		public string Address
		{
			get
			{
				return this._userName + "@" + this._host;
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x000DF244 File Offset: 0x000DD444
		private string GetAddress(bool allowUnicode)
		{
			return this.GetUser(allowUnicode) + "@" + this.GetHost(allowUnicode);
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x000DF25E File Offset: 0x000DD45E
		private string SmtpAddress
		{
			get
			{
				return "<" + this.Address + ">";
			}
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x000DF275 File Offset: 0x000DD475
		internal string GetSmtpAddress(bool allowUnicode)
		{
			return "<" + this.GetAddress(allowUnicode) + ">";
		}

		/// <summary>Returns a string representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the contents of this <see cref="T:System.Net.Mail.MailAddress" />.</returns>
		// Token: 0x06004119 RID: 16665 RVA: 0x000DF28D File Offset: 0x000DD48D
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.DisplayName))
			{
				return this.Address;
			}
			return "\"" + this.DisplayName + "\" " + this.SmtpAddress;
		}

		/// <summary>Compares two mail addresses.</summary>
		/// <param name="value">A <see cref="T:System.Net.Mail.MailAddress" /> instance to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the two mail addresses are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600411A RID: 16666 RVA: 0x000DF2BE File Offset: 0x000DD4BE
		public override bool Equals(object value)
		{
			return value != null && this.ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>Returns a hash value for a mail address.</summary>
		/// <returns>An integer hash value.</returns>
		// Token: 0x0600411B RID: 16667 RVA: 0x000B7406 File Offset: 0x000B5606
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x000DF2D8 File Offset: 0x000DD4D8
		internal string Encode(int charsConsumed, bool allowUnicode)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this._displayName))
			{
				if (MimeBasePart.IsAscii(this._displayName, false) || allowUnicode)
				{
					text = "\"" + this._displayName + "\"";
				}
				else
				{
					IEncodableStream encoderForHeader = MailAddress.s_encoderFactory.GetEncoderForHeader(this._displayNameEncoding, false, charsConsumed);
					byte[] bytes = this._displayNameEncoding.GetBytes(this._displayName);
					encoderForHeader.EncodeBytes(bytes, 0, bytes.Length);
					text = encoderForHeader.GetEncodedString();
				}
				text = text + " " + this.GetSmtpAddress(allowUnicode);
			}
			else
			{
				text = this.GetAddress(allowUnicode);
			}
			return text;
		}

		// Token: 0x040026EC RID: 9964
		private readonly Encoding _displayNameEncoding;

		// Token: 0x040026ED RID: 9965
		private readonly string _displayName;

		// Token: 0x040026EE RID: 9966
		private readonly string _userName;

		// Token: 0x040026EF RID: 9967
		private readonly string _host;

		// Token: 0x040026F0 RID: 9968
		private static readonly EncodedStreamFactory s_encoderFactory = new EncodedStreamFactory();
	}
}
