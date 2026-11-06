using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	/// <summary>Represents a MIME protocol Content-Disposition header.</summary>
	// Token: 0x020007CD RID: 1997
	public class ContentDisposition
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mime.ContentDisposition" /> class with a <see cref="P:System.Net.Mime.ContentDisposition.DispositionType" /> of <see cref="F:System.Net.Mime.DispositionTypeNames.Attachment" />.</summary>
		// Token: 0x06003FFE RID: 16382 RVA: 0x000DA9D4 File Offset: 0x000D8BD4
		public ContentDisposition()
		{
			this._isChanged = true;
			this._disposition = (this._dispositionType = "attachment");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mime.ContentDisposition" /> class with the specified disposition information.</summary>
		/// <param name="disposition">A <see cref="T:System.Net.Mime.DispositionTypeNames" /> value that contains the disposition.</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="disposition" /> is <see langword="null" /> or equal to <see cref="F:System.String.Empty" /> ("").</exception>
		// Token: 0x06003FFF RID: 16383 RVA: 0x000DAA02 File Offset: 0x000D8C02
		public ContentDisposition(string disposition)
		{
			if (disposition == null)
			{
				throw new ArgumentNullException("disposition");
			}
			this._isChanged = true;
			this._disposition = disposition;
			this.ParseValue();
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x000DAA2C File Offset: 0x000D8C2C
		internal DateTime GetDateParameter(string parameterName)
		{
			SmtpDateTime smtpDateTime = ((TrackingValidationObjectDictionary)this.Parameters).InternalGet(parameterName) as SmtpDateTime;
			if (smtpDateTime != null)
			{
				return smtpDateTime.Date;
			}
			return DateTime.MinValue;
		}

		/// <summary>Gets or sets the disposition type for an email attachment.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the disposition type. The value is not restricted but is typically one of the <see cref="P:System.Net.Mime.ContentDisposition.DispositionType" /> values.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The value specified for a set operation is equal to <see cref="F:System.String.Empty" /> ("").</exception>
		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06004001 RID: 16385 RVA: 0x000DAA5F File Offset: 0x000D8C5F
		// (set) Token: 0x06004002 RID: 16386 RVA: 0x000DAA67 File Offset: 0x000D8C67
		public string DispositionType
		{
			get
			{
				return this._dispositionType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException("This property cannot be set to an empty string.", "value");
				}
				this._isChanged = true;
				this._dispositionType = value;
			}
		}

		/// <summary>Gets the parameters included in the Content-Disposition header represented by this instance.</summary>
		/// <returns>A writable <see cref="T:System.Collections.Specialized.StringDictionary" /> that contains parameter name/value pairs.</returns>
		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06004003 RID: 16387 RVA: 0x000DAAA4 File Offset: 0x000D8CA4
		public StringDictionary Parameters
		{
			get
			{
				TrackingValidationObjectDictionary trackingValidationObjectDictionary;
				if ((trackingValidationObjectDictionary = this._parameters) == null)
				{
					trackingValidationObjectDictionary = (this._parameters = new TrackingValidationObjectDictionary(ContentDisposition.s_validators));
				}
				return trackingValidationObjectDictionary;
			}
		}

		/// <summary>Gets or sets the suggested file name for an email attachment.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the file name.</returns>
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x000DAACE File Offset: 0x000D8CCE
		// (set) Token: 0x06004005 RID: 16389 RVA: 0x000DAAE0 File Offset: 0x000D8CE0
		public string FileName
		{
			get
			{
				return this.Parameters["filename"];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Parameters.Remove("filename");
					return;
				}
				this.Parameters["filename"] = value;
			}
		}

		/// <summary>Gets or sets the creation date for a file attachment.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that indicates the file creation date; otherwise, <see cref="F:System.DateTime.MinValue" /> if no date was specified.</returns>
		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x000DAB0C File Offset: 0x000D8D0C
		// (set) Token: 0x06004007 RID: 16391 RVA: 0x000DAB1C File Offset: 0x000D8D1C
		public DateTime CreationDate
		{
			get
			{
				return this.GetDateParameter("creation-date");
			}
			set
			{
				SmtpDateTime smtpDateTime = new SmtpDateTime(value);
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("creation-date", smtpDateTime);
			}
		}

		/// <summary>Gets or sets the modification date for a file attachment.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that indicates the file modification date; otherwise, <see cref="F:System.DateTime.MinValue" /> if no date was specified.</returns>
		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06004008 RID: 16392 RVA: 0x000DAB46 File Offset: 0x000D8D46
		// (set) Token: 0x06004009 RID: 16393 RVA: 0x000DAB54 File Offset: 0x000D8D54
		public DateTime ModificationDate
		{
			get
			{
				return this.GetDateParameter("modification-date");
			}
			set
			{
				SmtpDateTime smtpDateTime = new SmtpDateTime(value);
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("modification-date", smtpDateTime);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines the disposition type (Inline or Attachment) for an email attachment.</summary>
		/// <returns>
		///   <see langword="true" /> if content in the attachment is presented inline as part of the email body; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x000DAB7E File Offset: 0x000D8D7E
		// (set) Token: 0x0600400B RID: 16395 RVA: 0x000DAB90 File Offset: 0x000D8D90
		public bool Inline
		{
			get
			{
				return this._dispositionType == "inline";
			}
			set
			{
				this._isChanged = true;
				this._dispositionType = (value ? "inline" : "attachment");
			}
		}

		/// <summary>Gets or sets the read date for a file attachment.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that indicates the file read date; otherwise, <see cref="F:System.DateTime.MinValue" /> if no date was specified.</returns>
		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x000DABAE File Offset: 0x000D8DAE
		// (set) Token: 0x0600400D RID: 16397 RVA: 0x000DABBC File Offset: 0x000D8DBC
		public DateTime ReadDate
		{
			get
			{
				return this.GetDateParameter("read-date");
			}
			set
			{
				SmtpDateTime smtpDateTime = new SmtpDateTime(value);
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("read-date", smtpDateTime);
			}
		}

		/// <summary>Gets or sets the size of a file attachment.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the number of bytes in the file attachment. The default value is -1, which indicates that the file size is unknown.</returns>
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x000DABE8 File Offset: 0x000D8DE8
		// (set) Token: 0x0600400F RID: 16399 RVA: 0x000DAC17 File Offset: 0x000D8E17
		public long Size
		{
			get
			{
				object obj = ((TrackingValidationObjectDictionary)this.Parameters).InternalGet("size");
				if (obj != null)
				{
					return (long)obj;
				}
				return -1L;
			}
			set
			{
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("size", value);
			}
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x000DAC34 File Offset: 0x000D8E34
		internal void Set(string contentDisposition, HeaderCollection headers)
		{
			this._disposition = contentDisposition;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
			this._isPersisted = true;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x000DAC5C File Offset: 0x000D8E5C
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this._isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
				this._isPersisted = true;
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06004012 RID: 16402 RVA: 0x000DAC8F File Offset: 0x000D8E8F
		internal bool IsChanged
		{
			get
			{
				return this._isChanged || (this._parameters != null && this._parameters.IsChanged);
			}
		}

		/// <summary>Returns a <see cref="T:System.String" /> representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the property values for this instance.</returns>
		// Token: 0x06004013 RID: 16403 RVA: 0x000DACB0 File Offset: 0x000D8EB0
		public override string ToString()
		{
			if (this._disposition == null || this._isChanged || (this._parameters != null && this._parameters.IsChanged))
			{
				this._disposition = this.Encode(false);
				this._isChanged = false;
				this._parameters.IsChanged = false;
				this._isPersisted = false;
			}
			return this._disposition;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x000DAD10 File Offset: 0x000D8F10
		internal string Encode(bool allowUnicode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this._dispositionType);
			foreach (object obj in this.Parameters.Keys)
			{
				string text = (string)obj;
				stringBuilder.Append("; ");
				ContentDisposition.EncodeToBuffer(text, stringBuilder, allowUnicode);
				stringBuilder.Append('=');
				ContentDisposition.EncodeToBuffer(this._parameters[text], stringBuilder, allowUnicode);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x000DADB0 File Offset: 0x000D8FB0
		private static void EncodeToBuffer(string value, StringBuilder builder, bool allowUnicode)
		{
			Encoding encoding = MimeBasePart.DecodeEncoding(value);
			if (encoding != null)
			{
				builder.Append('"').Append(value).Append('"');
				return;
			}
			if ((allowUnicode && !MailBnfHelper.HasCROrLF(value)) || MimeBasePart.IsAscii(value, false))
			{
				MailBnfHelper.GetTokenOrQuotedString(value, builder, allowUnicode);
				return;
			}
			encoding = Encoding.GetEncoding("utf-8");
			builder.Append('"').Append(MimeBasePart.EncodeHeaderValue(value, encoding, MimeBasePart.ShouldUseBase64Encoding(encoding))).Append('"');
		}

		/// <summary>Determines whether the content-disposition header of the specified <see cref="T:System.Net.Mime.ContentDisposition" /> object is equal to the content-disposition header of this object.</summary>
		/// <param name="rparam">The <see cref="T:System.Net.Mime.ContentDisposition" /> object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the content-disposition headers are the same; otherwise <see langword="false" />.</returns>
		// Token: 0x06004016 RID: 16406 RVA: 0x000DAE28 File Offset: 0x000D9028
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Equals(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>Determines the hash code of the specified <see cref="T:System.Net.Mime.ContentDisposition" /> object</summary>
		/// <returns>An integer hash value.</returns>
		// Token: 0x06004017 RID: 16407 RVA: 0x000DAE41 File Offset: 0x000D9041
		public override int GetHashCode()
		{
			return this.ToString().ToLowerInvariant().GetHashCode();
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x000DAE54 File Offset: 0x000D9054
		private void ParseValue()
		{
			int num = 0;
			try
			{
				this._dispositionType = MailBnfHelper.ReadToken(this._disposition, ref num, null);
				if (string.IsNullOrEmpty(this._dispositionType))
				{
					throw new FormatException("The mail header is malformed.");
				}
				if (this._parameters == null)
				{
					this._parameters = new TrackingValidationObjectDictionary(ContentDisposition.s_validators);
				}
				else
				{
					this._parameters.Clear();
				}
				while (MailBnfHelper.SkipCFWS(this._disposition, ref num))
				{
					if (this._disposition[num++] != ';')
					{
						throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", this._disposition[num - 1]));
					}
					if (!MailBnfHelper.SkipCFWS(this._disposition, ref num))
					{
						break;
					}
					string text = MailBnfHelper.ReadParameterAttribute(this._disposition, ref num, null);
					if (this._disposition[num++] != '=')
					{
						throw new FormatException("The mail header is malformed.");
					}
					if (!MailBnfHelper.SkipCFWS(this._disposition, ref num))
					{
						throw new FormatException("The specified content disposition is invalid.");
					}
					string text2 = ((this._disposition[num] == '"') ? MailBnfHelper.ReadQuotedString(this._disposition, ref num, null) : MailBnfHelper.ReadToken(this._disposition, ref num, null));
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						throw new FormatException("The specified content disposition is invalid.");
					}
					this.Parameters.Add(text, text2);
				}
			}
			catch (FormatException ex)
			{
				throw new FormatException("The specified content disposition is invalid.", ex);
			}
			this._parameters.IsChanged = false;
		}

		// Token: 0x0400265B RID: 9819
		private const string CreationDateKey = "creation-date";

		// Token: 0x0400265C RID: 9820
		private const string ModificationDateKey = "modification-date";

		// Token: 0x0400265D RID: 9821
		private const string ReadDateKey = "read-date";

		// Token: 0x0400265E RID: 9822
		private const string FileNameKey = "filename";

		// Token: 0x0400265F RID: 9823
		private const string SizeKey = "size";

		// Token: 0x04002660 RID: 9824
		private TrackingValidationObjectDictionary _parameters;

		// Token: 0x04002661 RID: 9825
		private string _disposition;

		// Token: 0x04002662 RID: 9826
		private string _dispositionType;

		// Token: 0x04002663 RID: 9827
		private bool _isChanged;

		// Token: 0x04002664 RID: 9828
		private bool _isPersisted;

		// Token: 0x04002665 RID: 9829
		private static readonly TrackingValidationObjectDictionary.ValidateAndParseValue s_dateParser = (object v) => new SmtpDateTime(v.ToString());

		// Token: 0x04002666 RID: 9830
		private static readonly TrackingValidationObjectDictionary.ValidateAndParseValue s_longParser = delegate(object value)
		{
			long num;
			if (!long.TryParse(value.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out num))
			{
				throw new FormatException("The specified content disposition is invalid.");
			}
			return num;
		};

		// Token: 0x04002667 RID: 9831
		private static readonly Dictionary<string, TrackingValidationObjectDictionary.ValidateAndParseValue> s_validators = new Dictionary<string, TrackingValidationObjectDictionary.ValidateAndParseValue>
		{
			{
				"creation-date",
				ContentDisposition.s_dateParser
			},
			{
				"modification-date",
				ContentDisposition.s_dateParser
			},
			{
				"read-date",
				ContentDisposition.s_dateParser
			},
			{
				"size",
				ContentDisposition.s_longParser
			}
		};
	}
}
