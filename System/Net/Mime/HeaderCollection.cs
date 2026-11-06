using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020007D3 RID: 2003
	internal class HeaderCollection : NameValueCollection
	{
		// Token: 0x06004042 RID: 16450 RVA: 0x000DB990 File Offset: 0x000D9B90
		internal HeaderCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x000DB9A0 File Offset: 0x000D9BA0
		public override void Remove(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this._part != null)
			{
				this._part.ContentType = null;
			}
			else if (id == MailHeaderID.ContentDisposition && this._part is MimePart)
			{
				((MimePart)this._part).ContentDisposition = null;
			}
			base.Remove(name);
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x000DBA2C File Offset: 0x000D9C2C
		public override string Get(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this._part != null)
			{
				this._part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this._part is MimePart)
			{
				((MimePart)this._part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.Get(name);
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x000DBAC4 File Offset: 0x000D9CC4
		public override string[] GetValues(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this._part != null)
			{
				this._part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this._part is MimePart)
			{
				((MimePart)this._part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.GetValues(name);
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x000DBB5C File Offset: 0x000D9D5C
		internal void InternalRemove(string name)
		{
			base.Remove(name);
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x000DBB65 File Offset: 0x000D9D65
		internal void InternalSet(string name, string value)
		{
			base.Set(name, value);
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x000DBB6F File Offset: 0x000D9D6F
		internal void InternalAdd(string name, string value)
		{
			if (MailHeaderInfo.IsSingleton(name))
			{
				base.Set(name, value);
				return;
			}
			base.Add(name, value);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x000DBB8C File Offset: 0x000D9D8C
		public override void Set(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "value"), "value");
			}
			if (!MimeBasePart.IsAscii(name, false))
			{
				throw new FormatException(SR.Format("An invalid character was found in header name.", Array.Empty<object>()));
			}
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			value = value.Normalize(NormalizationForm.FormC);
			if (id == MailHeaderID.ContentType && this._part != null)
			{
				this._part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this._part is MimePart)
			{
				((MimePart)this._part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			base.Set(name, value);
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x000DBCA0 File Offset: 0x000D9EA0
		public override void Add(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.Format("The parameter '{0}' cannot be an empty string.", "value"), "value");
			}
			MailBnfHelper.ValidateHeaderName(name);
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			value = value.Normalize(NormalizationForm.FormC);
			if (id == MailHeaderID.ContentType && this._part != null)
			{
				this._part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this._part is MimePart)
			{
				((MimePart)this._part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			this.InternalAdd(name, value);
		}

		// Token: 0x04002676 RID: 9846
		private MimeBasePart _part;
	}
}
