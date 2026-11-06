using System;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000071 RID: 113
	internal class XmlCharCheckingWriter : XmlWrappingWriter
	{
		// Token: 0x06000427 RID: 1063 RVA: 0x00013D35 File Offset: 0x00011F35
		internal XmlCharCheckingWriter(XmlWriter baseWriter, bool checkValues, bool checkNames, bool replaceNewLines, string newLineChars)
			: base(baseWriter)
		{
			this.checkValues = checkValues;
			this.checkNames = checkNames;
			this.replaceNewLines = replaceNewLines;
			this.newLineChars = newLineChars;
			if (checkValues)
			{
				this.xmlCharType = XmlCharType.Instance;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00013D6C File Offset: 0x00011F6C
		public override XmlWriterSettings Settings
		{
			get
			{
				XmlWriterSettings xmlWriterSettings = this.writer.Settings;
				xmlWriterSettings = ((xmlWriterSettings != null) ? xmlWriterSettings.Clone() : new XmlWriterSettings());
				if (this.checkValues)
				{
					xmlWriterSettings.CheckCharacters = true;
				}
				if (this.replaceNewLines)
				{
					xmlWriterSettings.NewLineHandling = NewLineHandling.Replace;
					xmlWriterSettings.NewLineChars = this.newLineChars;
				}
				xmlWriterSettings.ReadOnly = true;
				return xmlWriterSettings;
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00013DC8 File Offset: 0x00011FC8
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (this.checkNames)
			{
				this.ValidateQName(name);
			}
			if (this.checkValues)
			{
				int num;
				if (pubid != null && (num = this.xmlCharType.IsPublicId(pubid)) >= 0)
				{
					throw XmlConvert.CreateInvalidCharException(pubid, num);
				}
				if (sysid != null)
				{
					this.CheckCharacters(sysid);
				}
				if (subset != null)
				{
					this.CheckCharacters(subset);
				}
			}
			if (this.replaceNewLines)
			{
				sysid = this.ReplaceNewLines(sysid);
				pubid = this.ReplaceNewLines(pubid);
				subset = this.ReplaceNewLines(subset);
			}
			this.writer.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00013E54 File Offset: 0x00012054
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (this.checkNames)
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
				}
				this.ValidateNCName(localName);
				if (prefix != null && prefix.Length > 0)
				{
					this.ValidateNCName(prefix);
				}
			}
			this.writer.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00013EAC File Offset: 0x000120AC
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.checkNames)
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
				}
				this.ValidateNCName(localName);
				if (prefix != null && prefix.Length > 0)
				{
					this.ValidateNCName(prefix);
				}
			}
			this.writer.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00013F04 File Offset: 0x00012104
		public override void WriteCData(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
				int num;
				while ((num = text.IndexOf("]]>", StringComparison.Ordinal)) >= 0)
				{
					this.writer.WriteCData(text.Substring(0, num + 2));
					text = text.Substring(num + 2);
				}
			}
			this.writer.WriteCData(text);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00013F73 File Offset: 0x00012173
		public override void WriteComment(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
					text = this.InterleaveInvalidChars(text, '-', '-');
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			this.writer.WriteComment(text);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00013FB4 File Offset: 0x000121B4
		public override void WriteProcessingInstruction(string name, string text)
		{
			if (this.checkNames)
			{
				this.ValidateNCName(name);
			}
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
					text = this.InterleaveInvalidChars(text, '?', '>');
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			this.writer.WriteProcessingInstruction(name, text);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001400D File Offset: 0x0001220D
		public override void WriteEntityRef(string name)
		{
			if (this.checkNames)
			{
				this.ValidateQName(name);
			}
			this.writer.WriteEntityRef(name);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001402C File Offset: 0x0001222C
		public override void WriteWhitespace(string ws)
		{
			if (ws == null)
			{
				ws = string.Empty;
			}
			int num;
			if (this.checkNames && (num = this.xmlCharType.IsOnlyWhitespaceWithPos(ws)) != -1)
			{
				string text = "The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.";
				object[] array = XmlException.BuildCharExceptionArgs(ws, num);
				throw new ArgumentException(Res.GetString(text, array));
			}
			if (this.replaceNewLines)
			{
				ws = this.ReplaceNewLines(ws);
			}
			this.writer.WriteWhitespace(ws);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00014092 File Offset: 0x00012292
		public override void WriteString(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
				}
				if (this.replaceNewLines && this.WriteState != WriteState.Attribute)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			this.writer.WriteString(text);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000140CC File Offset: 0x000122CC
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.writer.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000140DC File Offset: 0x000122DC
		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.checkValues)
			{
				this.CheckCharacters(buffer, index, count);
			}
			if (this.replaceNewLines && this.WriteState != WriteState.Attribute)
			{
				string text = this.ReplaceNewLines(buffer, index, count);
				if (text != null)
				{
					this.WriteString(text);
					return;
				}
			}
			this.writer.WriteChars(buffer, index, count);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001416D File Offset: 0x0001236D
		public override void WriteNmToken(string name)
		{
			if (this.checkNames)
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
				}
				XmlConvert.VerifyNMTOKEN(name);
			}
			this.writer.WriteNmToken(name);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000141A5 File Offset: 0x000123A5
		public override void WriteName(string name)
		{
			if (this.checkNames)
			{
				XmlConvert.VerifyQName(name, ExceptionType.XmlException);
			}
			this.writer.WriteName(name);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000141C3 File Offset: 0x000123C3
		public override void WriteQualifiedName(string localName, string ns)
		{
			if (this.checkNames)
			{
				this.ValidateNCName(localName);
			}
			this.writer.WriteQualifiedName(localName, ns);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000141E1 File Offset: 0x000123E1
		private void CheckCharacters(string str)
		{
			XmlConvert.VerifyCharData(str, ExceptionType.ArgumentException);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000141EA File Offset: 0x000123EA
		private void CheckCharacters(char[] data, int offset, int len)
		{
			XmlConvert.VerifyCharData(data, offset, len, ExceptionType.ArgumentException);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000141F8 File Offset: 0x000123F8
		private void ValidateNCName(string ncname)
		{
			if (ncname.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			int num = ValidateNames.ParseNCName(ncname, 0);
			if (num != ncname.Length)
			{
				string text = ((num == 0) ? "Name cannot begin with the '{0}' character, hexadecimal value {1}." : "The '{0}' character, hexadecimal value {1}, cannot be included in a name.");
				object[] array = XmlException.BuildCharExceptionArgs(ncname, num);
				throw new ArgumentException(Res.GetString(text, array));
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00014254 File Offset: 0x00012454
		private void ValidateQName(string name)
		{
			if (name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			int num2;
			int num = ValidateNames.ParseQName(name, 0, out num2);
			if (num != name.Length)
			{
				string text = ((num == 0 || (num2 > -1 && num == num2 + 1)) ? "Name cannot begin with the '{0}' character, hexadecimal value {1}." : "The '{0}' character, hexadecimal value {1}, cannot be included in a name.");
				object[] array = XmlException.BuildCharExceptionArgs(name, num);
				throw new ArgumentException(Res.GetString(text, array));
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000142BC File Offset: 0x000124BC
		private string ReplaceNewLines(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num = 0;
			int i;
			for (i = 0; i < str.Length; i++)
			{
				char c;
				if ((c = str[i]) < ' ')
				{
					if (c == '\n')
					{
						if (this.newLineChars == "\n")
						{
							goto IL_00F7;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(str.Length + 5);
						}
						stringBuilder.Append(str, num, i - num);
					}
					else
					{
						if (c != '\r')
						{
							goto IL_00F7;
						}
						if (i + 1 < str.Length && str[i + 1] == '\n')
						{
							if (this.newLineChars == "\r\n")
							{
								i++;
								goto IL_00F7;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(str.Length + 5);
							}
							stringBuilder.Append(str, num, i - num);
							i++;
						}
						else
						{
							if (this.newLineChars == "\r")
							{
								goto IL_00F7;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(str.Length + 5);
							}
							stringBuilder.Append(str, num, i - num);
						}
					}
					stringBuilder.Append(this.newLineChars);
					num = i + 1;
				}
				IL_00F7:;
			}
			if (stringBuilder == null)
			{
				return str;
			}
			stringBuilder.Append(str, num, i - num);
			return stringBuilder.ToString();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000143E8 File Offset: 0x000125E8
		private string ReplaceNewLines(char[] data, int offset, int len)
		{
			if (data == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num = offset;
			int num2 = offset + len;
			int i;
			for (i = offset; i < num2; i++)
			{
				char c;
				if ((c = data[i]) < ' ')
				{
					if (c == '\n')
					{
						if (this.newLineChars == "\n")
						{
							goto IL_00DF;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(len + 5);
						}
						stringBuilder.Append(data, num, i - num);
					}
					else
					{
						if (c != '\r')
						{
							goto IL_00DF;
						}
						if (i + 1 < num2 && data[i + 1] == '\n')
						{
							if (this.newLineChars == "\r\n")
							{
								i++;
								goto IL_00DF;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(len + 5);
							}
							stringBuilder.Append(data, num, i - num);
							i++;
						}
						else
						{
							if (this.newLineChars == "\r")
							{
								goto IL_00DF;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(len + 5);
							}
							stringBuilder.Append(data, num, i - num);
						}
					}
					stringBuilder.Append(this.newLineChars);
					num = i + 1;
				}
				IL_00DF:;
			}
			if (stringBuilder == null)
			{
				return null;
			}
			stringBuilder.Append(data, num, i - num);
			return stringBuilder.ToString();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000144F8 File Offset: 0x000126F8
		private string InterleaveInvalidChars(string text, char invChar1, char invChar2)
		{
			StringBuilder stringBuilder = null;
			int num = 0;
			int i;
			for (i = 0; i < text.Length; i++)
			{
				if (text[i] == invChar2 && i > 0 && text[i - 1] == invChar1)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(text.Length + 5);
					}
					stringBuilder.Append(text, num, i - num);
					stringBuilder.Append(' ');
					num = i;
				}
			}
			if (stringBuilder != null)
			{
				stringBuilder.Append(text, num, i - num);
				if (i > 0 && text[i - 1] == invChar1)
				{
					stringBuilder.Append(' ');
				}
				return stringBuilder.ToString();
			}
			if (i != 0 && text[i - 1] == invChar1)
			{
				return text + " ";
			}
			return text;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000145A8 File Offset: 0x000127A8
		public override Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
		{
			if (this.checkNames)
			{
				this.ValidateQName(name);
			}
			if (this.checkValues)
			{
				int num;
				if (pubid != null && (num = this.xmlCharType.IsPublicId(pubid)) >= 0)
				{
					throw XmlConvert.CreateInvalidCharException(pubid, num);
				}
				if (sysid != null)
				{
					this.CheckCharacters(sysid);
				}
				if (subset != null)
				{
					this.CheckCharacters(subset);
				}
			}
			if (this.replaceNewLines)
			{
				sysid = this.ReplaceNewLines(sysid);
				pubid = this.ReplaceNewLines(pubid);
				subset = this.ReplaceNewLines(subset);
			}
			return this.writer.WriteDocTypeAsync(name, pubid, sysid, subset);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00014634 File Offset: 0x00012834
		public override Task WriteStartElementAsync(string prefix, string localName, string ns)
		{
			if (this.checkNames)
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
				}
				this.ValidateNCName(localName);
				if (prefix != null && prefix.Length > 0)
				{
					this.ValidateNCName(prefix);
				}
			}
			return this.writer.WriteStartElementAsync(prefix, localName, ns);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001468C File Offset: 0x0001288C
		protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string ns)
		{
			if (this.checkNames)
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
				}
				this.ValidateNCName(localName);
				if (prefix != null && prefix.Length > 0)
				{
					this.ValidateNCName(prefix);
				}
			}
			return this.writer.WriteStartAttributeAsync(prefix, localName, ns);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000146E4 File Offset: 0x000128E4
		public override async Task WriteCDataAsync(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
				int i;
				while ((i = text.IndexOf("]]>", StringComparison.Ordinal)) >= 0)
				{
					await this.writer.WriteCDataAsync(text.Substring(0, i + 2)).ConfigureAwait(false);
					text = text.Substring(i + 2);
				}
			}
			await this.writer.WriteCDataAsync(text).ConfigureAwait(false);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001472F File Offset: 0x0001292F
		public override Task WriteCommentAsync(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
					text = this.InterleaveInvalidChars(text, '-', '-');
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			return this.writer.WriteCommentAsync(text);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00014770 File Offset: 0x00012970
		public override Task WriteProcessingInstructionAsync(string name, string text)
		{
			if (this.checkNames)
			{
				this.ValidateNCName(name);
			}
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
					text = this.InterleaveInvalidChars(text, '?', '>');
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			return this.writer.WriteProcessingInstructionAsync(name, text);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x000147C9 File Offset: 0x000129C9
		public override Task WriteEntityRefAsync(string name)
		{
			if (this.checkNames)
			{
				this.ValidateQName(name);
			}
			return this.writer.WriteEntityRefAsync(name);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000147E8 File Offset: 0x000129E8
		public override Task WriteWhitespaceAsync(string ws)
		{
			if (ws == null)
			{
				ws = string.Empty;
			}
			int num;
			if (this.checkNames && (num = this.xmlCharType.IsOnlyWhitespaceWithPos(ws)) != -1)
			{
				string text = "The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.";
				object[] array = XmlException.BuildCharExceptionArgs(ws, num);
				throw new ArgumentException(Res.GetString(text, array));
			}
			if (this.replaceNewLines)
			{
				ws = this.ReplaceNewLines(ws);
			}
			return this.writer.WriteWhitespaceAsync(ws);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001484E File Offset: 0x00012A4E
		public override Task WriteStringAsync(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
				}
				if (this.replaceNewLines && this.WriteState != WriteState.Attribute)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			return this.writer.WriteStringAsync(text);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00014888 File Offset: 0x00012A88
		public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
		{
			return this.writer.WriteSurrogateCharEntityAsync(lowChar, highChar);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00014898 File Offset: 0x00012A98
		public override Task WriteCharsAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.checkValues)
			{
				this.CheckCharacters(buffer, index, count);
			}
			if (this.replaceNewLines && this.WriteState != WriteState.Attribute)
			{
				string text = this.ReplaceNewLines(buffer, index, count);
				if (text != null)
				{
					return this.WriteStringAsync(text);
				}
			}
			return this.writer.WriteCharsAsync(buffer, index, count);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00014929 File Offset: 0x00012B29
		public override Task WriteNmTokenAsync(string name)
		{
			if (this.checkNames)
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
				}
				XmlConvert.VerifyNMTOKEN(name);
			}
			return this.writer.WriteNmTokenAsync(name);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00014961 File Offset: 0x00012B61
		public override Task WriteNameAsync(string name)
		{
			if (this.checkNames)
			{
				XmlConvert.VerifyQName(name, ExceptionType.XmlException);
			}
			return this.writer.WriteNameAsync(name);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001497F File Offset: 0x00012B7F
		public override Task WriteQualifiedNameAsync(string localName, string ns)
		{
			if (this.checkNames)
			{
				this.ValidateNCName(localName);
			}
			return this.writer.WriteQualifiedNameAsync(localName, ns);
		}

		// Token: 0x040006F1 RID: 1777
		private bool checkValues;

		// Token: 0x040006F2 RID: 1778
		private bool checkNames;

		// Token: 0x040006F3 RID: 1779
		private bool replaceNewLines;

		// Token: 0x040006F4 RID: 1780
		private string newLineChars;

		// Token: 0x040006F5 RID: 1781
		private XmlCharType xmlCharType;
	}
}
