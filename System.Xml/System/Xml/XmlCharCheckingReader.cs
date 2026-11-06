using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000068 RID: 104
	internal class XmlCharCheckingReader : XmlWrappingReader
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00011CAC File Offset: 0x0000FEAC
		internal XmlCharCheckingReader(XmlReader reader, bool checkCharacters, bool ignoreWhitespace, bool ignoreComments, bool ignorePis, DtdProcessing dtdProcessing)
			: base(reader)
		{
			this.state = XmlCharCheckingReader.State.Initial;
			this.checkCharacters = checkCharacters;
			this.ignoreWhitespace = ignoreWhitespace;
			this.ignoreComments = ignoreComments;
			this.ignorePis = ignorePis;
			this.dtdProcessing = dtdProcessing;
			this.lastNodeType = XmlNodeType.None;
			if (checkCharacters)
			{
				this.xmlCharType = XmlCharType.Instance;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00011D04 File Offset: 0x0000FF04
		public override XmlReaderSettings Settings
		{
			get
			{
				XmlReaderSettings xmlReaderSettings = this.reader.Settings;
				if (xmlReaderSettings == null)
				{
					xmlReaderSettings = new XmlReaderSettings();
				}
				else
				{
					xmlReaderSettings = xmlReaderSettings.Clone();
				}
				if (this.checkCharacters)
				{
					xmlReaderSettings.CheckCharacters = true;
				}
				if (this.ignoreWhitespace)
				{
					xmlReaderSettings.IgnoreWhitespace = true;
				}
				if (this.ignoreComments)
				{
					xmlReaderSettings.IgnoreComments = true;
				}
				if (this.ignorePis)
				{
					xmlReaderSettings.IgnoreProcessingInstructions = true;
				}
				if (this.dtdProcessing != (DtdProcessing)(-1))
				{
					xmlReaderSettings.DtdProcessing = this.dtdProcessing;
				}
				xmlReaderSettings.ReadOnly = true;
				return xmlReaderSettings;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00011D88 File Offset: 0x0000FF88
		public override bool MoveToAttribute(string name)
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			return this.reader.MoveToAttribute(name);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00011DA5 File Offset: 0x0000FFA5
		public override bool MoveToAttribute(string name, string ns)
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			return this.reader.MoveToAttribute(name, ns);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00011DC3 File Offset: 0x0000FFC3
		public override void MoveToAttribute(int i)
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			this.reader.MoveToAttribute(i);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00011DE0 File Offset: 0x0000FFE0
		public override bool MoveToFirstAttribute()
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			return this.reader.MoveToFirstAttribute();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00011DFC File Offset: 0x0000FFFC
		public override bool MoveToNextAttribute()
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			return this.reader.MoveToNextAttribute();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00011E18 File Offset: 0x00010018
		public override bool MoveToElement()
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			return this.reader.MoveToElement();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00011E34 File Offset: 0x00010034
		public override bool Read()
		{
			switch (this.state)
			{
			case XmlCharCheckingReader.State.Initial:
				this.state = XmlCharCheckingReader.State.Interactive;
				if (this.reader.ReadState != ReadState.Initial)
				{
					goto IL_0055;
				}
				break;
			case XmlCharCheckingReader.State.InReadBinary:
				this.FinishReadBinary();
				this.state = XmlCharCheckingReader.State.Interactive;
				break;
			case XmlCharCheckingReader.State.Error:
				return false;
			case XmlCharCheckingReader.State.Interactive:
				break;
			default:
				return false;
			}
			if (!this.reader.Read())
			{
				return false;
			}
			IL_0055:
			XmlNodeType nodeType = this.reader.NodeType;
			if (!this.checkCharacters)
			{
				switch (nodeType)
				{
				case XmlNodeType.ProcessingInstruction:
					if (this.ignorePis)
					{
						return this.Read();
					}
					break;
				case XmlNodeType.Comment:
					if (this.ignoreComments)
					{
						return this.Read();
					}
					break;
				case XmlNodeType.DocumentType:
					if (this.dtdProcessing == DtdProcessing.Prohibit)
					{
						this.Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
					}
					else if (this.dtdProcessing == DtdProcessing.Ignore)
					{
						return this.Read();
					}
					break;
				case XmlNodeType.Whitespace:
					if (this.ignoreWhitespace)
					{
						return this.Read();
					}
					break;
				}
				return true;
			}
			switch (nodeType)
			{
			case XmlNodeType.Element:
				if (this.checkCharacters)
				{
					this.ValidateQName(this.reader.Prefix, this.reader.LocalName);
					if (this.reader.MoveToFirstAttribute())
					{
						do
						{
							this.ValidateQName(this.reader.Prefix, this.reader.LocalName);
							this.CheckCharacters(this.reader.Value);
						}
						while (this.reader.MoveToNextAttribute());
						this.reader.MoveToElement();
					}
				}
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				if (this.checkCharacters)
				{
					this.CheckCharacters(this.reader.Value);
				}
				break;
			case XmlNodeType.EntityReference:
				if (this.checkCharacters)
				{
					this.ValidateQName(this.reader.Name);
				}
				break;
			case XmlNodeType.ProcessingInstruction:
				if (this.ignorePis)
				{
					return this.Read();
				}
				if (this.checkCharacters)
				{
					this.ValidateQName(this.reader.Name);
					this.CheckCharacters(this.reader.Value);
				}
				break;
			case XmlNodeType.Comment:
				if (this.ignoreComments)
				{
					return this.Read();
				}
				if (this.checkCharacters)
				{
					this.CheckCharacters(this.reader.Value);
				}
				break;
			case XmlNodeType.DocumentType:
				if (this.dtdProcessing == DtdProcessing.Prohibit)
				{
					this.Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
				}
				else if (this.dtdProcessing == DtdProcessing.Ignore)
				{
					return this.Read();
				}
				if (this.checkCharacters)
				{
					this.ValidateQName(this.reader.Name);
					this.CheckCharacters(this.reader.Value);
					string text = this.reader.GetAttribute("SYSTEM");
					if (text != null)
					{
						this.CheckCharacters(text);
					}
					text = this.reader.GetAttribute("PUBLIC");
					int num;
					if (text != null && (num = this.xmlCharType.IsPublicId(text)) >= 0)
					{
						this.Throw("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(text, num));
					}
				}
				break;
			case XmlNodeType.Whitespace:
				if (this.ignoreWhitespace)
				{
					return this.Read();
				}
				if (this.checkCharacters)
				{
					this.CheckWhitespace(this.reader.Value);
				}
				break;
			case XmlNodeType.SignificantWhitespace:
				if (this.checkCharacters)
				{
					this.CheckWhitespace(this.reader.Value);
				}
				break;
			case XmlNodeType.EndElement:
				if (this.checkCharacters)
				{
					this.ValidateQName(this.reader.Prefix, this.reader.LocalName);
				}
				break;
			}
			this.lastNodeType = nodeType;
			return true;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000121C8 File Offset: 0x000103C8
		public override ReadState ReadState
		{
			get
			{
				switch (this.state)
				{
				case XmlCharCheckingReader.State.Initial:
					if (this.reader.ReadState != ReadState.Closed)
					{
						return ReadState.Initial;
					}
					return ReadState.Closed;
				case XmlCharCheckingReader.State.Error:
					return ReadState.Error;
				}
				return this.reader.ReadState;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00012213 File Offset: 0x00010413
		public override bool ReadAttributeValue()
		{
			if (this.state == XmlCharCheckingReader.State.InReadBinary)
			{
				this.FinishReadBinary();
			}
			return this.reader.ReadAttributeValue();
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0001222F File Offset: 0x0001042F
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00012234 File Offset: 0x00010434
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.state != XmlCharCheckingReader.State.InReadBinary)
			{
				if (base.CanReadBinaryContent && !this.checkCharacters)
				{
					this.readBinaryHelper = null;
					this.state = XmlCharCheckingReader.State.InReadBinary;
					return base.ReadContentAsBase64(buffer, index, count);
				}
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			else if (this.readBinaryHelper == null)
			{
				return base.ReadContentAsBase64(buffer, index, count);
			}
			this.state = XmlCharCheckingReader.State.Interactive;
			int num = this.readBinaryHelper.ReadContentAsBase64(buffer, index, count);
			this.state = XmlCharCheckingReader.State.InReadBinary;
			return num;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x000122C0 File Offset: 0x000104C0
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.state != XmlCharCheckingReader.State.InReadBinary)
			{
				if (base.CanReadBinaryContent && !this.checkCharacters)
				{
					this.readBinaryHelper = null;
					this.state = XmlCharCheckingReader.State.InReadBinary;
					return base.ReadContentAsBinHex(buffer, index, count);
				}
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			else if (this.readBinaryHelper == null)
			{
				return base.ReadContentAsBinHex(buffer, index, count);
			}
			this.state = XmlCharCheckingReader.State.Interactive;
			int num = this.readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
			this.state = XmlCharCheckingReader.State.InReadBinary;
			return num;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001234C File Offset: 0x0001054C
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.state != XmlCharCheckingReader.State.InReadBinary)
			{
				if (base.CanReadBinaryContent && !this.checkCharacters)
				{
					this.readBinaryHelper = null;
					this.state = XmlCharCheckingReader.State.InReadBinary;
					return base.ReadElementContentAsBase64(buffer, index, count);
				}
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			else if (this.readBinaryHelper == null)
			{
				return base.ReadElementContentAsBase64(buffer, index, count);
			}
			this.state = XmlCharCheckingReader.State.Interactive;
			int num = this.readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
			this.state = XmlCharCheckingReader.State.InReadBinary;
			return num;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00012418 File Offset: 0x00010618
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			if (this.state != XmlCharCheckingReader.State.InReadBinary)
			{
				if (base.CanReadBinaryContent && !this.checkCharacters)
				{
					this.readBinaryHelper = null;
					this.state = XmlCharCheckingReader.State.InReadBinary;
					return base.ReadElementContentAsBinHex(buffer, index, count);
				}
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			else if (this.readBinaryHelper == null)
			{
				return base.ReadElementContentAsBinHex(buffer, index, count);
			}
			this.state = XmlCharCheckingReader.State.Interactive;
			int num = this.readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
			this.state = XmlCharCheckingReader.State.InReadBinary;
			return num;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000124E2 File Offset: 0x000106E2
		private void Throw(string res, string arg)
		{
			this.state = XmlCharCheckingReader.State.Error;
			throw new XmlException(res, arg, null);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000124F3 File Offset: 0x000106F3
		private void Throw(string res, string[] args)
		{
			this.state = XmlCharCheckingReader.State.Error;
			throw new XmlException(res, args, null);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00012504 File Offset: 0x00010704
		private void CheckWhitespace(string value)
		{
			int num;
			if ((num = this.xmlCharType.IsOnlyWhitespaceWithPos(value)) != -1)
			{
				this.Throw("The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.", XmlException.BuildCharExceptionArgs(value, num));
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00012534 File Offset: 0x00010734
		private void ValidateQName(string name)
		{
			string text;
			string text2;
			ValidateNames.ParseQNameThrow(name, out text, out text2);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001254C File Offset: 0x0001074C
		private void ValidateQName(string prefix, string localName)
		{
			try
			{
				if (prefix.Length > 0)
				{
					ValidateNames.ParseNCNameThrow(prefix);
				}
				ValidateNames.ParseNCNameThrow(localName);
			}
			catch
			{
				this.state = XmlCharCheckingReader.State.Error;
				throw;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001258C File Offset: 0x0001078C
		private void CheckCharacters(string value)
		{
			XmlConvert.VerifyCharData(value, ExceptionType.ArgumentException, ExceptionType.XmlException);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00012596 File Offset: 0x00010796
		private void FinishReadBinary()
		{
			this.state = XmlCharCheckingReader.State.Interactive;
			if (this.readBinaryHelper != null)
			{
				this.readBinaryHelper.Finish();
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000125B4 File Offset: 0x000107B4
		public override async Task<bool> ReadAsync()
		{
			switch (this.state)
			{
			case XmlCharCheckingReader.State.Initial:
				this.state = XmlCharCheckingReader.State.Interactive;
				if (this.reader.ReadState != ReadState.Initial)
				{
					goto IL_0176;
				}
				break;
			case XmlCharCheckingReader.State.InReadBinary:
				await this.FinishReadBinaryAsync().ConfigureAwait(false);
				this.state = XmlCharCheckingReader.State.Interactive;
				break;
			case XmlCharCheckingReader.State.Error:
				return false;
			case XmlCharCheckingReader.State.Interactive:
				break;
			default:
				return false;
			}
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = this.reader.ReadAsync().ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			if (!configuredTaskAwaiter.GetResult())
			{
				return false;
			}
			IL_0176:
			XmlNodeType nodeType = this.reader.NodeType;
			bool flag;
			if (!this.checkCharacters)
			{
				switch (nodeType)
				{
				case XmlNodeType.ProcessingInstruction:
					if (this.ignorePis)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					break;
				case XmlNodeType.Comment:
					if (this.ignoreComments)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					break;
				case XmlNodeType.DocumentType:
					if (this.dtdProcessing == DtdProcessing.Prohibit)
					{
						this.Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
					}
					else if (this.dtdProcessing == DtdProcessing.Ignore)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					break;
				case XmlNodeType.Whitespace:
					if (this.ignoreWhitespace)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					break;
				}
				flag = true;
			}
			else
			{
				switch (nodeType)
				{
				case XmlNodeType.Element:
					if (this.checkCharacters)
					{
						this.ValidateQName(this.reader.Prefix, this.reader.LocalName);
						if (this.reader.MoveToFirstAttribute())
						{
							do
							{
								this.ValidateQName(this.reader.Prefix, this.reader.LocalName);
								this.CheckCharacters(this.reader.Value);
							}
							while (this.reader.MoveToNextAttribute());
							this.reader.MoveToElement();
						}
					}
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					if (this.checkCharacters)
					{
						this.CheckCharacters(await this.reader.GetValueAsync().ConfigureAwait(false));
					}
					break;
				case XmlNodeType.EntityReference:
					if (this.checkCharacters)
					{
						this.ValidateQName(this.reader.Name);
					}
					break;
				case XmlNodeType.ProcessingInstruction:
					if (this.ignorePis)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					if (this.checkCharacters)
					{
						this.ValidateQName(this.reader.Name);
						this.CheckCharacters(this.reader.Value);
					}
					break;
				case XmlNodeType.Comment:
					if (this.ignoreComments)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					if (this.checkCharacters)
					{
						this.CheckCharacters(this.reader.Value);
					}
					break;
				case XmlNodeType.DocumentType:
					if (this.dtdProcessing == DtdProcessing.Prohibit)
					{
						this.Throw("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.", string.Empty);
					}
					else if (this.dtdProcessing == DtdProcessing.Ignore)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					if (this.checkCharacters)
					{
						this.ValidateQName(this.reader.Name);
						this.CheckCharacters(this.reader.Value);
						string text = this.reader.GetAttribute("SYSTEM");
						if (text != null)
						{
							this.CheckCharacters(text);
						}
						text = this.reader.GetAttribute("PUBLIC");
						if (text != null)
						{
							int num = this.xmlCharType.IsPublicId(text);
							if (num >= 0)
							{
								this.Throw("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(text, num));
							}
						}
					}
					break;
				case XmlNodeType.Whitespace:
					if (this.ignoreWhitespace)
					{
						return await this.ReadAsync().ConfigureAwait(false);
					}
					if (this.checkCharacters)
					{
						this.CheckWhitespace(await this.reader.GetValueAsync().ConfigureAwait(false));
					}
					break;
				case XmlNodeType.SignificantWhitespace:
					if (this.checkCharacters)
					{
						this.CheckWhitespace(await this.reader.GetValueAsync().ConfigureAwait(false));
					}
					break;
				case XmlNodeType.EndElement:
					if (this.checkCharacters)
					{
						this.ValidateQName(this.reader.Prefix, this.reader.LocalName);
					}
					break;
				}
				this.lastNodeType = nodeType;
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000125F8 File Offset: 0x000107F8
		public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
		{
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.state != XmlCharCheckingReader.State.InReadBinary)
				{
					if (base.CanReadBinaryContent && !this.checkCharacters)
					{
						this.readBinaryHelper = null;
						this.state = XmlCharCheckingReader.State.InReadBinary;
						return await base.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
					}
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				}
				else if (this.readBinaryHelper == null)
				{
					return await base.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
				}
				this.state = XmlCharCheckingReader.State.Interactive;
				int num2 = await this.readBinaryHelper.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
				this.state = XmlCharCheckingReader.State.InReadBinary;
				num = num2;
			}
			return num;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00012654 File Offset: 0x00010854
		public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.state != XmlCharCheckingReader.State.InReadBinary)
				{
					if (base.CanReadBinaryContent && !this.checkCharacters)
					{
						this.readBinaryHelper = null;
						this.state = XmlCharCheckingReader.State.InReadBinary;
						return await base.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
					}
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				}
				else if (this.readBinaryHelper == null)
				{
					return await base.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
				}
				this.state = XmlCharCheckingReader.State.Interactive;
				int num2 = await this.readBinaryHelper.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
				this.state = XmlCharCheckingReader.State.InReadBinary;
				num = num2;
			}
			return num;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000126B0 File Offset: 0x000108B0
		public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.state != XmlCharCheckingReader.State.InReadBinary)
				{
					if (base.CanReadBinaryContent && !this.checkCharacters)
					{
						this.readBinaryHelper = null;
						this.state = XmlCharCheckingReader.State.InReadBinary;
						return await base.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
					}
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				}
				else if (this.readBinaryHelper == null)
				{
					return await base.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
				}
				this.state = XmlCharCheckingReader.State.Interactive;
				int num2 = await this.readBinaryHelper.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(false);
				this.state = XmlCharCheckingReader.State.InReadBinary;
				num = num2;
			}
			return num;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001270C File Offset: 0x0001090C
		public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num;
			if (this.ReadState != ReadState.Interactive)
			{
				num = 0;
			}
			else
			{
				if (this.state != XmlCharCheckingReader.State.InReadBinary)
				{
					if (base.CanReadBinaryContent && !this.checkCharacters)
					{
						this.readBinaryHelper = null;
						this.state = XmlCharCheckingReader.State.InReadBinary;
						return await base.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
					}
					this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
				}
				else if (this.readBinaryHelper == null)
				{
					return await base.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
				}
				this.state = XmlCharCheckingReader.State.Interactive;
				int num2 = await this.readBinaryHelper.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(false);
				this.state = XmlCharCheckingReader.State.InReadBinary;
				num = num2;
			}
			return num;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00012768 File Offset: 0x00010968
		private async Task FinishReadBinaryAsync()
		{
			this.state = XmlCharCheckingReader.State.Interactive;
			if (this.readBinaryHelper != null)
			{
				await this.readBinaryHelper.FinishAsync().ConfigureAwait(false);
			}
		}

		// Token: 0x040006BB RID: 1723
		private XmlCharCheckingReader.State state;

		// Token: 0x040006BC RID: 1724
		private bool checkCharacters;

		// Token: 0x040006BD RID: 1725
		private bool ignoreWhitespace;

		// Token: 0x040006BE RID: 1726
		private bool ignoreComments;

		// Token: 0x040006BF RID: 1727
		private bool ignorePis;

		// Token: 0x040006C0 RID: 1728
		private DtdProcessing dtdProcessing;

		// Token: 0x040006C1 RID: 1729
		private XmlNodeType lastNodeType;

		// Token: 0x040006C2 RID: 1730
		private XmlCharType xmlCharType;

		// Token: 0x040006C3 RID: 1731
		private ReadContentAsBinaryHelper readBinaryHelper;

		// Token: 0x02000069 RID: 105
		private enum State
		{
			// Token: 0x040006C5 RID: 1733
			Initial,
			// Token: 0x040006C6 RID: 1734
			InReadBinary,
			// Token: 0x040006C7 RID: 1735
			Error,
			// Token: 0x040006C8 RID: 1736
			Interactive
		}
	}
}
