using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	/// <summary>Represents a MIME protocol Content-Type header.</summary>
	// Token: 0x020007CF RID: 1999
	public class ContentType
	{
		/// <summary>Initializes a new default instance of the <see cref="T:System.Net.Mime.ContentType" /> class.</summary>
		// Token: 0x0600401E RID: 16414 RVA: 0x000DB0BF File Offset: 0x000D92BF
		public ContentType()
			: this("application/octet-stream")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mime.ContentType" /> class using the specified string.</summary>
		/// <param name="contentType">A <see cref="T:System.String" />, for example, <c>"text/plain; charset=us-ascii"</c>, that contains the MIME media type, subtype, and optional parameters.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="contentType" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is in a form that cannot be parsed.</exception>
		// Token: 0x0600401F RID: 16415 RVA: 0x000DB0CC File Offset: 0x000D92CC
		public ContentType(string contentType)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			if (contentType == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "contentType"), "contentType");
			}
			this._isChanged = true;
			this._type = contentType;
			this.ParseValue();
		}

		/// <summary>Gets or sets the value of the boundary parameter included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value associated with the boundary parameter.</returns>
		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x000DB133 File Offset: 0x000D9333
		// (set) Token: 0x06004021 RID: 16417 RVA: 0x000DB145 File Offset: 0x000D9345
		public string Boundary
		{
			get
			{
				return this.Parameters["boundary"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("boundary");
					return;
				}
				this.Parameters["boundary"] = value;
			}
		}

		/// <summary>Gets or sets the value of the charset parameter included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value associated with the charset parameter.</returns>
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x000DB179 File Offset: 0x000D9379
		// (set) Token: 0x06004023 RID: 16419 RVA: 0x000DB18B File Offset: 0x000D938B
		public string CharSet
		{
			get
			{
				return this.Parameters["charset"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("charset");
					return;
				}
				this.Parameters["charset"] = value;
			}
		}

		/// <summary>Gets or sets the media type value included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the media type and subtype value. This value does not include the semicolon (;) separator that follows the subtype.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The value specified for a set operation is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">The value specified for a set operation is in a form that cannot be parsed.</exception>
		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06004024 RID: 16420 RVA: 0x000DB1BF File Offset: 0x000D93BF
		// (set) Token: 0x06004025 RID: 16421 RVA: 0x000DB1D8 File Offset: 0x000D93D8
		public string MediaType
		{
			get
			{
				return this._mediaType + "/" + this._subType;
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
				int num = 0;
				this._mediaType = MailBnfHelper.ReadToken(value, ref num, null);
				if (this._mediaType.Length == 0 || num >= value.Length || value[num++] != '/')
				{
					throw new FormatException("The specified media type is invalid.");
				}
				this._subType = MailBnfHelper.ReadToken(value, ref num, null);
				if (this._subType.Length == 0 || num < value.Length)
				{
					throw new FormatException("The specified media type is invalid.");
				}
				this._isChanged = true;
				this._isPersisted = false;
			}
		}

		/// <summary>Gets or sets the value of the name parameter included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value associated with the name parameter.</returns>
		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06004026 RID: 16422 RVA: 0x000DB290 File Offset: 0x000D9490
		// (set) Token: 0x06004027 RID: 16423 RVA: 0x000DB2BE File Offset: 0x000D94BE
		public string Name
		{
			get
			{
				string text = this.Parameters["name"];
				if (MimeBasePart.DecodeEncoding(text) != null)
				{
					text = MimeBasePart.DecodeHeaderValue(text);
				}
				return text;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("name");
					return;
				}
				this.Parameters["name"] = value;
			}
		}

		/// <summary>Gets the dictionary that contains the parameters included in the Content-Type header represented by this instance.</summary>
		/// <returns>A writable <see cref="T:System.Collections.Specialized.StringDictionary" /> that contains name and value pairs.</returns>
		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06004028 RID: 16424 RVA: 0x000DB2F2 File Offset: 0x000D94F2
		public StringDictionary Parameters
		{
			get
			{
				return this._parameters;
			}
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x000DB2FA File Offset: 0x000D94FA
		internal void Set(string contentType, HeaderCollection headers)
		{
			this._type = contentType;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.ToString());
			this._isPersisted = true;
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x000DB322 File Offset: 0x000D9522
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this._isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.ToString());
				this._isPersisted = true;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x000DB355 File Offset: 0x000D9555
		internal bool IsChanged
		{
			get
			{
				return this._isChanged || (this._parameters != null && this._parameters.IsChanged);
			}
		}

		/// <summary>Returns a string representation of this <see cref="T:System.Net.Mime.ContentType" /> object.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the current settings for this <see cref="T:System.Net.Mime.ContentType" />.</returns>
		// Token: 0x0600402C RID: 16428 RVA: 0x000DB376 File Offset: 0x000D9576
		public override string ToString()
		{
			if (this._type == null || this.IsChanged)
			{
				this._type = this.Encode(false);
				this._isChanged = false;
				this._parameters.IsChanged = false;
				this._isPersisted = false;
			}
			return this._type;
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x000DB3B8 File Offset: 0x000D95B8
		internal string Encode(bool allowUnicode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this._mediaType);
			stringBuilder.Append('/');
			stringBuilder.Append(this._subType);
			foreach (object obj in this.Parameters.Keys)
			{
				string text = (string)obj;
				stringBuilder.Append("; ");
				ContentType.EncodeToBuffer(text, stringBuilder, allowUnicode);
				stringBuilder.Append('=');
				ContentType.EncodeToBuffer(this._parameters[text], stringBuilder, allowUnicode);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x000DB470 File Offset: 0x000D9670
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

		/// <summary>Determines whether the content-type header of the specified <see cref="T:System.Net.Mime.ContentType" /> object is equal to the content-type header of this object.</summary>
		/// <param name="rparam">The <see cref="T:System.Net.Mime.ContentType" /> object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the content-type headers are the same; otherwise <see langword="false" />.</returns>
		// Token: 0x0600402F RID: 16431 RVA: 0x000DAE28 File Offset: 0x000D9028
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Equals(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>Determines the hash code of the specified <see cref="T:System.Net.Mime.ContentType" /> object</summary>
		/// <returns>An integer hash value.</returns>
		// Token: 0x06004030 RID: 16432 RVA: 0x000DAE41 File Offset: 0x000D9041
		public override int GetHashCode()
		{
			return this.ToString().ToLowerInvariant().GetHashCode();
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x000DB4E8 File Offset: 0x000D96E8
		private void ParseValue()
		{
			int num = 0;
			Exception ex = null;
			try
			{
				this._mediaType = MailBnfHelper.ReadToken(this._type, ref num, null);
				if (this._mediaType == null || this._mediaType.Length == 0 || num >= this._type.Length || this._type[num++] != '/')
				{
					ex = new FormatException("The specified content type is invalid.");
				}
				if (ex == null)
				{
					this._subType = MailBnfHelper.ReadToken(this._type, ref num, null);
					if (this._subType == null || this._subType.Length == 0)
					{
						ex = new FormatException("The specified content type is invalid.");
					}
				}
				if (ex == null)
				{
					while (MailBnfHelper.SkipCFWS(this._type, ref num))
					{
						if (this._type[num++] != ';')
						{
							ex = new FormatException("The specified content type is invalid.");
							break;
						}
						if (!MailBnfHelper.SkipCFWS(this._type, ref num))
						{
							break;
						}
						string text = MailBnfHelper.ReadParameterAttribute(this._type, ref num, null);
						if (text == null || text.Length == 0)
						{
							ex = new FormatException("The specified content type is invalid.");
							break;
						}
						if (num >= this._type.Length || this._type[num++] != '=')
						{
							ex = new FormatException("The specified content type is invalid.");
							break;
						}
						if (!MailBnfHelper.SkipCFWS(this._type, ref num))
						{
							ex = new FormatException("The specified content type is invalid.");
							break;
						}
						string text2 = ((this._type[num] == '"') ? MailBnfHelper.ReadQuotedString(this._type, ref num, null) : MailBnfHelper.ReadToken(this._type, ref num, null));
						if (text2 == null)
						{
							ex = new FormatException("The specified content type is invalid.");
							break;
						}
						this._parameters.Add(text, text2);
					}
				}
				this._parameters.IsChanged = false;
			}
			catch (FormatException)
			{
				throw new FormatException("The specified content type is invalid.");
			}
			if (ex != null)
			{
				throw new FormatException("The specified content type is invalid.");
			}
		}

		// Token: 0x04002669 RID: 9833
		private readonly TrackingStringDictionary _parameters = new TrackingStringDictionary();

		// Token: 0x0400266A RID: 9834
		private string _mediaType;

		// Token: 0x0400266B RID: 9835
		private string _subType;

		// Token: 0x0400266C RID: 9836
		private bool _isChanged;

		// Token: 0x0400266D RID: 9837
		private string _type;

		// Token: 0x0400266E RID: 9838
		private bool _isPersisted;

		// Token: 0x0400266F RID: 9839
		internal const string Default = "application/octet-stream";
	}
}
