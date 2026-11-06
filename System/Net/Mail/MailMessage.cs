using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents an email message that can be sent using the <see cref="T:System.Net.Mail.SmtpClient" /> class.</summary>
	// Token: 0x02000800 RID: 2048
	public class MailMessage : IDisposable
	{
		/// <summary>Initializes an empty instance of the <see cref="T:System.Net.Mail.MailMessage" /> class.</summary>
		// Token: 0x06004179 RID: 16761 RVA: 0x000E22D4 File Offset: 0x000E04D4
		public MailMessage()
		{
			this.to = new MailAddressCollection();
			this.alternateViews = new AlternateViewCollection();
			this.attachments = new AttachmentCollection();
			this.bcc = new MailAddressCollection();
			this.cc = new MailAddressCollection();
			this.replyTo = new MailAddressCollection();
			this.headers = new NameValueCollection();
			this.headers.Add("MIME-Version", "1.0");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailMessage" /> class by using the specified <see cref="T:System.Net.Mail.MailAddress" /> class objects.</summary>
		/// <param name="from">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the email message.</param>
		/// <param name="to">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the recipient of the email message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="to" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="from" /> or <paramref name="to" /> is malformed.</exception>
		// Token: 0x0600417A RID: 16762 RVA: 0x000E2354 File Offset: 0x000E0554
		public MailMessage(MailAddress from, MailAddress to)
			: this()
		{
			if (from == null || to == null)
			{
				throw new ArgumentNullException();
			}
			this.From = from;
			this.to.Add(to);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailMessage" /> class by using the specified <see cref="T:System.String" /> class objects.</summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the email message.</param>
		/// <param name="to">A <see cref="T:System.String" /> that contains the addresses of the recipients of the email message. Multiple email addresses must be separated with a comma character (",").</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="to" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" /> ("").  
		/// -or-  
		/// <paramref name="to" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="from" /> or <paramref name="to" /> is malformed.</exception>
		// Token: 0x0600417B RID: 16763 RVA: 0x000E237C File Offset: 0x000E057C
		public MailMessage(string from, string to)
			: this()
		{
			if (from == null || from == string.Empty)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null || to == string.Empty)
			{
				throw new ArgumentNullException("to");
			}
			this.from = new MailAddress(from);
			foreach (string text in to.Split(new char[] { ',' }))
			{
				this.to.Add(new MailAddress(text.Trim()));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailMessage" /> class.</summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the email message.</param>
		/// <param name="to">A <see cref="T:System.String" /> that contains the addresses of the recipients of the email message. Multiple email addresses must be separated with a comma character (",").</param>
		/// <param name="subject">A <see cref="T:System.String" /> that contains the subject text.</param>
		/// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="to" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" /> ("").  
		/// -or-  
		/// <paramref name="to" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="from" /> or <paramref name="to" /> is malformed.</exception>
		// Token: 0x0600417C RID: 16764 RVA: 0x000E240C File Offset: 0x000E060C
		public MailMessage(string from, string to, string subject, string body)
			: this()
		{
			if (from == null || from == string.Empty)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null || to == string.Empty)
			{
				throw new ArgumentNullException("to");
			}
			this.from = new MailAddress(from);
			foreach (string text in to.Split(new char[] { ',' }))
			{
				this.to.Add(new MailAddress(text.Trim()));
			}
			this.Body = body;
			this.Subject = subject;
		}

		/// <summary>Gets the attachment collection used to store alternate forms of the message body.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.AlternateViewCollection" />.</returns>
		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x000E24AA File Offset: 0x000E06AA
		public AlternateViewCollection AlternateViews
		{
			get
			{
				return this.alternateViews;
			}
		}

		/// <summary>Gets the attachment collection used to store data attached to this email message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.AttachmentCollection" />.</returns>
		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x000E24B2 File Offset: 0x000E06B2
		public AttachmentCollection Attachments
		{
			get
			{
				return this.attachments;
			}
		}

		/// <summary>Gets the address collection that contains the blind carbon copy (BCC) recipients for this email message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</returns>
		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x000E24BA File Offset: 0x000E06BA
		public MailAddressCollection Bcc
		{
			get
			{
				return this.bcc;
			}
		}

		/// <summary>Gets or sets the message body.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the body text.</returns>
		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06004180 RID: 16768 RVA: 0x000E24C2 File Offset: 0x000E06C2
		// (set) Token: 0x06004181 RID: 16769 RVA: 0x000E24CA File Offset: 0x000E06CA
		public string Body
		{
			get
			{
				return this.body;
			}
			set
			{
				if (value != null && this.bodyEncoding == null)
				{
					this.bodyEncoding = this.GuessEncoding(value) ?? Encoding.ASCII;
				}
				this.body = value;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06004182 RID: 16770 RVA: 0x000E24F4 File Offset: 0x000E06F4
		internal ContentType BodyContentType
		{
			get
			{
				return new ContentType(this.isHtml ? "text/html" : "text/plain")
				{
					CharSet = (this.BodyEncoding ?? Encoding.ASCII).HeaderName
				};
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06004183 RID: 16771 RVA: 0x000E2529 File Offset: 0x000E0729
		internal TransferEncoding ContentTransferEncoding
		{
			get
			{
				return MailMessage.GuessTransferEncoding(this.BodyEncoding);
			}
		}

		/// <summary>Gets or sets the encoding used to encode the message body.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> applied to the contents of the <see cref="P:System.Net.Mail.MailMessage.Body" />.</returns>
		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06004184 RID: 16772 RVA: 0x000E2536 File Offset: 0x000E0736
		// (set) Token: 0x06004185 RID: 16773 RVA: 0x000E253E File Offset: 0x000E073E
		public Encoding BodyEncoding
		{
			get
			{
				return this.bodyEncoding;
			}
			set
			{
				this.bodyEncoding = value;
			}
		}

		/// <summary>Gets or sets the transfer encoding used to encode the message body.</summary>
		/// <returns>A <see cref="T:System.Net.Mime.TransferEncoding" /> applied to the contents of the <see cref="P:System.Net.Mail.MailMessage.Body" />.</returns>
		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06004186 RID: 16774 RVA: 0x000E2529 File Offset: 0x000E0729
		// (set) Token: 0x06004187 RID: 16775 RVA: 0x0000829A File Offset: 0x0000649A
		public TransferEncoding BodyTransferEncoding
		{
			get
			{
				return MailMessage.GuessTransferEncoding(this.BodyEncoding);
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the address collection that contains the carbon copy (CC) recipients for this email message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</returns>
		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06004188 RID: 16776 RVA: 0x000E2547 File Offset: 0x000E0747
		public MailAddressCollection CC
		{
			get
			{
				return this.cc;
			}
		}

		/// <summary>Gets or sets the delivery notifications for this email message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.DeliveryNotificationOptions" /> value that contains the delivery notifications for this message.</returns>
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x000E254F File Offset: 0x000E074F
		// (set) Token: 0x0600418A RID: 16778 RVA: 0x000E2557 File Offset: 0x000E0757
		public DeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return this.deliveryNotificationOptions;
			}
			set
			{
				this.deliveryNotificationOptions = value;
			}
		}

		/// <summary>Gets or sets the from address for this email message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.MailAddress" /> that contains the from address information.</returns>
		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x000E2560 File Offset: 0x000E0760
		// (set) Token: 0x0600418C RID: 16780 RVA: 0x000E2568 File Offset: 0x000E0768
		public MailAddress From
		{
			get
			{
				return this.from;
			}
			set
			{
				this.from = value;
			}
		}

		/// <summary>Gets the email headers that are transmitted with this email message.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> that contains the email headers.</returns>
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x000E2571 File Offset: 0x000E0771
		public NameValueCollection Headers
		{
			get
			{
				return this.headers;
			}
		}

		/// <summary>Gets or sets a value indicating whether the mail message body is in HTML.</summary>
		/// <returns>
		///   <see langword="true" /> if the message body is in HTML; else <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x000E2579 File Offset: 0x000E0779
		// (set) Token: 0x0600418F RID: 16783 RVA: 0x000E2581 File Offset: 0x000E0781
		public bool IsBodyHtml
		{
			get
			{
				return this.isHtml;
			}
			set
			{
				this.isHtml = value;
			}
		}

		/// <summary>Gets or sets the priority of this email message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.MailPriority" /> that contains the priority of this message.</returns>
		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x000E258A File Offset: 0x000E078A
		// (set) Token: 0x06004191 RID: 16785 RVA: 0x000E2592 File Offset: 0x000E0792
		public MailPriority Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
			}
		}

		/// <summary>Gets or sets the encoding used for the user-defined custom headers for this email message.</summary>
		/// <returns>The encoding used for user-defined custom headers for this email message.</returns>
		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x000E259B File Offset: 0x000E079B
		// (set) Token: 0x06004193 RID: 16787 RVA: 0x000E25A3 File Offset: 0x000E07A3
		public Encoding HeadersEncoding
		{
			get
			{
				return this.headersEncoding;
			}
			set
			{
				this.headersEncoding = value;
			}
		}

		/// <summary>Gets the list of addresses to reply to for the mail message.</summary>
		/// <returns>The list of the addresses to reply to for the mail message.</returns>
		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06004194 RID: 16788 RVA: 0x000E25AC File Offset: 0x000E07AC
		public MailAddressCollection ReplyToList
		{
			get
			{
				return this.replyTo;
			}
		}

		/// <summary>Gets or sets the ReplyTo address for the mail message.</summary>
		/// <returns>A MailAddress that indicates the value of the <see cref="P:System.Net.Mail.MailMessage.ReplyTo" /> field.</returns>
		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x000E25B4 File Offset: 0x000E07B4
		// (set) Token: 0x06004196 RID: 16790 RVA: 0x000E25D1 File Offset: 0x000E07D1
		[Obsolete("Use ReplyToList instead")]
		public MailAddress ReplyTo
		{
			get
			{
				if (this.replyTo.Count == 0)
				{
					return null;
				}
				return this.replyTo[0];
			}
			set
			{
				this.replyTo.Clear();
				this.replyTo.Add(value);
			}
		}

		/// <summary>Gets or sets the sender's address for this email message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.MailAddress" /> that contains the sender's address information.</returns>
		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x000E25EA File Offset: 0x000E07EA
		// (set) Token: 0x06004198 RID: 16792 RVA: 0x000E25F2 File Offset: 0x000E07F2
		public MailAddress Sender
		{
			get
			{
				return this.sender;
			}
			set
			{
				this.sender = value;
			}
		}

		/// <summary>Gets or sets the subject line for this email message.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the subject content.</returns>
		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x000E25FB File Offset: 0x000E07FB
		// (set) Token: 0x0600419A RID: 16794 RVA: 0x000E2603 File Offset: 0x000E0803
		public string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				if (value != null && this.subjectEncoding == null)
				{
					this.subjectEncoding = this.GuessEncoding(value);
				}
				this.subject = value;
			}
		}

		/// <summary>Gets or sets the encoding used for the subject content for this email message.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> that was used to encode the <see cref="P:System.Net.Mail.MailMessage.Subject" /> property.</returns>
		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x000E2624 File Offset: 0x000E0824
		// (set) Token: 0x0600419C RID: 16796 RVA: 0x000E262C File Offset: 0x000E082C
		public Encoding SubjectEncoding
		{
			get
			{
				return this.subjectEncoding;
			}
			set
			{
				this.subjectEncoding = value;
			}
		}

		/// <summary>Gets the address collection that contains the recipients of this email message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</returns>
		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x000E2635 File Offset: 0x000E0835
		public MailAddressCollection To
		{
			get
			{
				return this.to;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.MailMessage" />.</summary>
		// Token: 0x0600419E RID: 16798 RVA: 0x000E263D File Offset: 0x000E083D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Mail.MailMessage" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x0600419F RID: 16799 RVA: 0x00003917 File Offset: 0x00001B17
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x000E264C File Offset: 0x000E084C
		private Encoding GuessEncoding(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] >= '\u0080')
				{
					return MailMessage.UTF8Unmarked;
				}
			}
			return null;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x000E2680 File Offset: 0x000E0880
		internal static TransferEncoding GuessTransferEncoding(Encoding enc)
		{
			if (Encoding.ASCII.Equals(enc))
			{
				return TransferEncoding.SevenBit;
			}
			if (Encoding.UTF8.CodePage == enc.CodePage || Encoding.Unicode.CodePage == enc.CodePage || Encoding.UTF32.CodePage == enc.CodePage)
			{
				return TransferEncoding.Base64;
			}
			return TransferEncoding.QuotedPrintable;
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x000E26D8 File Offset: 0x000E08D8
		internal static string To2047(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				if (b < 33 || b > 126 || b == 63 || b == 61 || b == 95)
				{
					stringBuilder.Append('=');
					stringBuilder.Append(MailMessage.hex[(b >> 4) & 15]);
					stringBuilder.Append(MailMessage.hex[(int)(b & 15)]);
				}
				else
				{
					stringBuilder.Append((char)b);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x000E2758 File Offset: 0x000E0958
		internal static string EncodeSubjectRFC2047(string s, Encoding enc)
		{
			if (s == null || Encoding.ASCII.Equals(enc))
			{
				return s;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] >= '\u0080')
				{
					string text = MailMessage.To2047(enc.GetBytes(s));
					return string.Concat(new string[] { "=?", enc.HeaderName, "?Q?", text, "?=" });
				}
			}
			return s;
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x000E27D5 File Offset: 0x000E09D5
		private static Encoding UTF8Unmarked
		{
			get
			{
				if (MailMessage.utf8unmarked == null)
				{
					MailMessage.utf8unmarked = new UTF8Encoding(false);
				}
				return MailMessage.utf8unmarked;
			}
		}

		// Token: 0x0400272C RID: 10028
		private AlternateViewCollection alternateViews;

		// Token: 0x0400272D RID: 10029
		private AttachmentCollection attachments;

		// Token: 0x0400272E RID: 10030
		private MailAddressCollection bcc;

		// Token: 0x0400272F RID: 10031
		private MailAddressCollection replyTo;

		// Token: 0x04002730 RID: 10032
		private string body;

		// Token: 0x04002731 RID: 10033
		private MailPriority priority;

		// Token: 0x04002732 RID: 10034
		private MailAddress sender;

		// Token: 0x04002733 RID: 10035
		private DeliveryNotificationOptions deliveryNotificationOptions;

		// Token: 0x04002734 RID: 10036
		private MailAddressCollection cc;

		// Token: 0x04002735 RID: 10037
		private MailAddress from;

		// Token: 0x04002736 RID: 10038
		private NameValueCollection headers;

		// Token: 0x04002737 RID: 10039
		private MailAddressCollection to;

		// Token: 0x04002738 RID: 10040
		private string subject;

		// Token: 0x04002739 RID: 10041
		private Encoding subjectEncoding;

		// Token: 0x0400273A RID: 10042
		private Encoding bodyEncoding;

		// Token: 0x0400273B RID: 10043
		private Encoding headersEncoding = Encoding.UTF8;

		// Token: 0x0400273C RID: 10044
		private bool isHtml;

		// Token: 0x0400273D RID: 10045
		private static char[] hex = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		// Token: 0x0400273E RID: 10046
		private static Encoding utf8unmarked;
	}
}
