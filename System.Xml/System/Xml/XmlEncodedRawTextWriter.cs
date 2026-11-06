using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000073 RID: 115
	internal class XmlEncodedRawTextWriter : XmlRawWriter
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x00014B88 File Offset: 0x00012D88
		protected XmlEncodedRawTextWriter(XmlWriterSettings settings)
		{
			this.useAsync = settings.Async;
			this.newLineHandling = settings.NewLineHandling;
			this.omitXmlDeclaration = settings.OmitXmlDeclaration;
			this.newLineChars = settings.NewLineChars;
			this.checkCharacters = settings.CheckCharacters;
			this.closeOutput = settings.CloseOutput;
			this.standalone = settings.Standalone;
			this.outputMethod = settings.OutputMethod;
			this.mergeCDataSections = settings.MergeCDataSections;
			if (this.checkCharacters && this.newLineHandling == NewLineHandling.Replace)
			{
				this.ValidateContentChars(this.newLineChars, "NewLineChars", false);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00014C50 File Offset: 0x00012E50
		public XmlEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings)
			: this(settings)
		{
			this.writer = writer;
			this.encoding = writer.Encoding;
			if (settings.Async)
			{
				this.bufLen = 65536;
			}
			this.bufChars = new char[this.bufLen + 32];
			if (settings.AutoXmlDeclaration)
			{
				this.WriteXmlDeclaration(this.standalone);
				this.autoXmlDeclaration = true;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00014CBC File Offset: 0x00012EBC
		public XmlEncodedRawTextWriter(Stream stream, XmlWriterSettings settings)
			: this(settings)
		{
			this.stream = stream;
			this.encoding = settings.Encoding;
			if (settings.Async)
			{
				this.bufLen = 65536;
			}
			this.bufChars = new char[this.bufLen + 32];
			this.bufBytes = new byte[this.bufChars.Length];
			this.bufBytesUsed = 0;
			this.trackTextContent = true;
			this.inTextContent = false;
			this.lastMarkPos = 0;
			this.textContentMarks = new int[64];
			this.textContentMarks[0] = 1;
			this.charEntityFallback = new CharEntityEncoderFallback();
			this.encoding = (Encoding)settings.Encoding.Clone();
			this.encoding.EncoderFallback = this.charEntityFallback;
			this.encoder = this.encoding.GetEncoder();
			if (!stream.CanSeek || stream.Position == 0L)
			{
				byte[] preamble = this.encoding.GetPreamble();
				if (preamble.Length != 0)
				{
					this.stream.Write(preamble, 0, preamble.Length);
				}
			}
			if (settings.AutoXmlDeclaration)
			{
				this.WriteXmlDeclaration(this.standalone);
				this.autoXmlDeclaration = true;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00014DE0 File Offset: 0x00012FE0
		public override XmlWriterSettings Settings
		{
			get
			{
				return new XmlWriterSettings
				{
					Encoding = this.encoding,
					OmitXmlDeclaration = this.omitXmlDeclaration,
					NewLineHandling = this.newLineHandling,
					NewLineChars = this.newLineChars,
					CloseOutput = this.closeOutput,
					ConformanceLevel = ConformanceLevel.Auto,
					CheckCharacters = this.checkCharacters,
					AutoXmlDeclaration = this.autoXmlDeclaration,
					Standalone = this.standalone,
					OutputMethod = this.outputMethod,
					ReadOnly = true
				};
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00014E6C File Offset: 0x0001306C
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
			if (!this.omitXmlDeclaration && !this.autoXmlDeclaration)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					this.ChangeTextContentMark(false);
				}
				this.RawText("<?xml version=\"");
				this.RawText("1.0");
				if (this.encoding != null)
				{
					this.RawText("\" encoding=\"");
					this.RawText(this.encoding.WebName);
				}
				if (standalone != XmlStandalone.Omit)
				{
					this.RawText("\" standalone=\"");
					this.RawText((standalone == XmlStandalone.Yes) ? "yes" : "no");
				}
				this.RawText("\"?>");
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00014F0F File Offset: 0x0001310F
		internal override void WriteXmlDeclaration(string xmldecl)
		{
			if (!this.omitXmlDeclaration && !this.autoXmlDeclaration)
			{
				this.WriteProcessingInstruction("xml", xmldecl);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00014F30 File Offset: 0x00013130
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			this.RawText("<!DOCTYPE ");
			this.RawText(name);
			int num;
			if (pubid != null)
			{
				this.RawText(" PUBLIC \"");
				this.RawText(pubid);
				this.RawText("\" \"");
				if (sysid != null)
				{
					this.RawText(sysid);
				}
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 34;
			}
			else if (sysid != null)
			{
				this.RawText(" SYSTEM \"");
				this.RawText(sysid);
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 34;
			}
			else
			{
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 32;
			}
			if (subset != null)
			{
				char[] array4 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array4[num] = 91;
				this.RawText(subset);
				char[] array5 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array5[num] = 93;
			}
			char[] array6 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array6[num] = 62;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00015054 File Offset: 0x00013254
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			if (prefix != null && prefix.Length != 0)
			{
				this.RawText(prefix);
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 58;
			}
			this.RawText(localName);
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000150D4 File Offset: 0x000132D4
		internal override void StartElementContent()
		{
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 62;
			this.contentPos = this.bufPos;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00015108 File Offset: 0x00013308
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num;
			if (this.contentPos != this.bufPos)
			{
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 60;
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 47;
				if (prefix != null && prefix.Length != 0)
				{
					this.RawText(prefix);
					char[] array3 = this.bufChars;
					num = this.bufPos;
					this.bufPos = num + 1;
					array3[num] = 58;
				}
				this.RawText(localName);
				char[] array4 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array4[num] = 62;
				return;
			}
			this.bufPos--;
			char[] array5 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array5[num] = 32;
			char[] array6 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array6[num] = 47;
			char[] array7 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array7[num] = 62;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001521C File Offset: 0x0001341C
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 47;
			if (prefix != null && prefix.Length != 0)
			{
				this.RawText(prefix);
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 58;
			}
			this.RawText(localName);
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 62;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000152C4 File Offset: 0x000134C4
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num;
			if (this.attrEndPos == this.bufPos)
			{
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 32;
			}
			if (prefix != null && prefix.Length > 0)
			{
				this.RawText(prefix);
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 58;
			}
			this.RawText(localName);
			char[] array3 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array3[num] = 61;
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 34;
			this.inAttributeValue = true;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00015380 File Offset: 0x00013580
		public override void WriteEndAttribute()
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 34;
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000153D1 File Offset: 0x000135D1
		internal override void WriteNamespaceDeclaration(string prefix, string namespaceName)
		{
			this.WriteStartNamespaceDeclaration(prefix);
			this.WriteString(namespaceName);
			this.WriteEndNamespaceDeclaration();
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0001222F File Offset: 0x0001042F
		internal override bool SupportsNamespaceDeclarationInChunks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000153E8 File Offset: 0x000135E8
		internal override void WriteStartNamespaceDeclaration(string prefix)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			if (prefix.Length == 0)
			{
				this.RawText(" xmlns=\"");
			}
			else
			{
				this.RawText(" xmlns:");
				this.RawText(prefix);
				char[] array = this.bufChars;
				int num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 61;
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 34;
			}
			this.inAttributeValue = true;
			if (this.trackTextContent && !this.inTextContent)
			{
				this.ChangeTextContentMark(true);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00015488 File Offset: 0x00013688
		internal override void WriteEndNamespaceDeclaration()
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			this.inAttributeValue = false;
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 34;
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000154DC File Offset: 0x000136DC
		public override void WriteCData(string text)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num;
			if (this.mergeCDataSections && this.bufPos == this.cdataPos)
			{
				this.bufPos -= 3;
			}
			else
			{
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 60;
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 33;
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 91;
				char[] array4 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array4[num] = 67;
				char[] array5 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array5[num] = 68;
				char[] array6 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array6[num] = 65;
				char[] array7 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array7[num] = 84;
				char[] array8 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array8[num] = 65;
				char[] array9 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array9[num] = 91;
			}
			this.WriteCDataSection(text);
			char[] array10 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array10[num] = 93;
			char[] array11 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array11[num] = 93;
			char[] array12 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array12[num] = 62;
			this.textPos = this.bufPos;
			this.cdataPos = this.bufPos;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00015680 File Offset: 0x00013880
		public override void WriteComment(string text)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 33;
			char[] array3 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array3[num] = 45;
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 45;
			this.WriteCommentOrPi(text, 45);
			char[] array5 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array5[num] = 45;
			char[] array6 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array6[num] = 45;
			char[] array7 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array7[num] = 62;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00015764 File Offset: 0x00013964
		public override void WriteProcessingInstruction(string name, string text)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 63;
			this.RawText(name);
			if (text.Length > 0)
			{
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 32;
				this.WriteCommentOrPi(text, 63);
			}
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 63;
			char[] array5 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array5[num] = 62;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00015824 File Offset: 0x00013A24
		public override void WriteEntityRef(string name)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 38;
			this.RawText(name);
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 59;
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000158A4 File Offset: 0x00013AA4
		public override void WriteCharEntity(char ch)
		{
			int num = (int)ch;
			string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
			if (this.checkCharacters && !this.xmlCharType.IsCharData(ch))
			{
				throw XmlConvert.CreateInvalidCharException(ch, '\0');
			}
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 38;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 35;
			char[] array3 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array3[num] = 120;
			this.RawText(text);
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 59;
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001598C File Offset: 0x00013B8C
		public unsafe override void WriteWhitespace(string ws)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			fixed (string text = ws)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char* ptr2 = ptr + ws.Length;
				if (this.inAttributeValue)
				{
					this.WriteAttributeTextBlock(ptr, ptr2);
				}
				else
				{
					this.WriteElementTextBlock(ptr, ptr2);
				}
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000159E8 File Offset: 0x00013BE8
		public unsafe override void WriteString(string text)
		{
			if (this.trackTextContent && !this.inTextContent)
			{
				this.ChangeTextContentMark(true);
			}
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char* ptr2 = ptr + text.Length;
				if (this.inAttributeValue)
				{
					this.WriteAttributeTextBlock(ptr, ptr2);
				}
				else
				{
					this.WriteElementTextBlock(ptr, ptr2);
				}
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00015A44 File Offset: 0x00013C44
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num = XmlCharType.CombineSurrogateChar((int)lowChar, (int)highChar);
			char[] array = this.bufChars;
			int num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array[num2] = 38;
			char[] array2 = this.bufChars;
			num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array2[num2] = 35;
			char[] array3 = this.bufChars;
			num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array3[num2] = 120;
			this.RawText(num.ToString("X", NumberFormatInfo.InvariantInfo));
			char[] array4 = this.bufChars;
			num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array4[num2] = 59;
			this.textPos = this.bufPos;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00015AFC File Offset: 0x00013CFC
		public unsafe override void WriteChars(char[] buffer, int index, int count)
		{
			if (this.trackTextContent && !this.inTextContent)
			{
				this.ChangeTextContentMark(true);
			}
			fixed (char* ptr = &buffer[index])
			{
				char* ptr2 = ptr;
				if (this.inAttributeValue)
				{
					this.WriteAttributeTextBlock(ptr2, ptr2 + count);
				}
				else
				{
					this.WriteElementTextBlock(ptr2, ptr2 + count);
				}
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00015B54 File Offset: 0x00013D54
		public unsafe override void WriteRaw(char[] buffer, int index, int count)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			fixed (char* ptr = &buffer[index])
			{
				char* ptr2 = ptr;
				this.WriteRawWithCharChecking(ptr2, ptr2 + count);
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00015BA0 File Offset: 0x00013DA0
		public unsafe override void WriteRaw(string data)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			fixed (string text = data)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.WriteRawWithCharChecking(ptr, ptr + data.Length);
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00015BF4 File Offset: 0x00013DF4
		public override void Close()
		{
			try
			{
				this.FlushBuffer();
				this.FlushEncoder();
			}
			finally
			{
				this.writeToNull = true;
				if (this.stream != null)
				{
					try
					{
						this.stream.Flush();
						goto IL_007D;
					}
					finally
					{
						try
						{
							if (this.closeOutput)
							{
								this.stream.Close();
							}
						}
						finally
						{
							this.stream = null;
						}
					}
				}
				if (this.writer != null)
				{
					try
					{
						this.writer.Flush();
					}
					finally
					{
						try
						{
							if (this.closeOutput)
							{
								this.writer.Close();
							}
						}
						finally
						{
							this.writer = null;
						}
					}
				}
				IL_007D:;
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00015CC0 File Offset: 0x00013EC0
		public override void Flush()
		{
			this.FlushBuffer();
			this.FlushEncoder();
			if (this.stream != null)
			{
				this.stream.Flush();
				return;
			}
			if (this.writer != null)
			{
				this.writer.Flush();
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00015CF8 File Offset: 0x00013EF8
		protected virtual void FlushBuffer()
		{
			try
			{
				if (!this.writeToNull)
				{
					if (this.stream != null)
					{
						if (this.trackTextContent)
						{
							this.charEntityFallback.Reset(this.textContentMarks, this.lastMarkPos);
							if ((this.lastMarkPos & 1) != 0)
							{
								this.textContentMarks[1] = 1;
								this.lastMarkPos = 1;
							}
							else
							{
								this.lastMarkPos = 0;
							}
						}
						this.EncodeChars(1, this.bufPos, true);
					}
					else
					{
						this.writer.Write(this.bufChars, 1, this.bufPos - 1);
					}
				}
			}
			catch
			{
				this.writeToNull = true;
				throw;
			}
			finally
			{
				this.bufChars[0] = this.bufChars[this.bufPos - 1];
				this.textPos = ((this.textPos == this.bufPos) ? 1 : 0);
				this.attrEndPos = ((this.attrEndPos == this.bufPos) ? 1 : 0);
				this.contentPos = 0;
				this.cdataPos = 0;
				this.bufPos = 1;
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00015E08 File Offset: 0x00014008
		private void EncodeChars(int startOffset, int endOffset, bool writeAllToStream)
		{
			while (startOffset < endOffset)
			{
				if (this.charEntityFallback != null)
				{
					this.charEntityFallback.StartOffset = startOffset;
				}
				int num;
				int num2;
				bool flag;
				this.encoder.Convert(this.bufChars, startOffset, endOffset - startOffset, this.bufBytes, this.bufBytesUsed, this.bufBytes.Length - this.bufBytesUsed, false, out num, out num2, out flag);
				startOffset += num;
				this.bufBytesUsed += num2;
				if (this.bufBytesUsed >= this.bufBytes.Length - 16)
				{
					this.stream.Write(this.bufBytes, 0, this.bufBytesUsed);
					this.bufBytesUsed = 0;
				}
			}
			if (writeAllToStream && this.bufBytesUsed > 0)
			{
				this.stream.Write(this.bufBytes, 0, this.bufBytesUsed);
				this.bufBytesUsed = 0;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00015EDC File Offset: 0x000140DC
		private void FlushEncoder()
		{
			if (this.stream != null)
			{
				int num;
				int num2;
				bool flag;
				this.encoder.Convert(this.bufChars, 1, 0, this.bufBytes, 0, this.bufBytes.Length, true, out num, out num2, out flag);
				if (num2 != 0)
				{
					this.stream.Write(this.bufBytes, 0, num2);
				}
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00015F30 File Offset: 0x00014130
		protected unsafe void WriteAttributeTextBlock(char* pSrc, char* pSrcEnd)
		{
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			char* ptr2 = ptr + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr3 = ptr2 + (long)(pSrcEnd - pSrc) * 2L / 2L;
				if (ptr3 != ptr + this.bufLen)
				{
					ptr3 = ptr + this.bufLen;
				}
				while (ptr2 < ptr3 && (this.xmlCharType.charProperties[num = (int)(*pSrc)] & 128) != 0)
				{
					*ptr2 = (char)num;
					ptr2++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (ptr2 >= ptr3)
				{
					this.bufPos = (int)((long)(ptr2 - ptr));
					this.FlushBuffer();
					ptr2 = ptr + 1;
				}
				else
				{
					if (num <= 38)
					{
						switch (num)
						{
						case 9:
							if (this.newLineHandling == NewLineHandling.None)
							{
								*ptr2 = (char)num;
								ptr2++;
								goto IL_01D3;
							}
							ptr2 = XmlEncodedRawTextWriter.TabEntity(ptr2);
							goto IL_01D3;
						case 10:
							if (this.newLineHandling == NewLineHandling.None)
							{
								*ptr2 = (char)num;
								ptr2++;
								goto IL_01D3;
							}
							ptr2 = XmlEncodedRawTextWriter.LineFeedEntity(ptr2);
							goto IL_01D3;
						case 11:
						case 12:
							break;
						case 13:
							if (this.newLineHandling == NewLineHandling.None)
							{
								*ptr2 = (char)num;
								ptr2++;
								goto IL_01D3;
							}
							ptr2 = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr2);
							goto IL_01D3;
						default:
							if (num == 34)
							{
								ptr2 = XmlEncodedRawTextWriter.QuoteEntity(ptr2);
								goto IL_01D3;
							}
							if (num == 38)
							{
								ptr2 = XmlEncodedRawTextWriter.AmpEntity(ptr2);
								goto IL_01D3;
							}
							break;
						}
					}
					else
					{
						if (num == 39)
						{
							*ptr2 = (char)num;
							ptr2++;
							goto IL_01D3;
						}
						if (num == 60)
						{
							ptr2 = XmlEncodedRawTextWriter.LtEntity(ptr2);
							goto IL_01D3;
						}
						if (num == 62)
						{
							ptr2 = XmlEncodedRawTextWriter.GtEntity(ptr2);
							goto IL_01D3;
						}
					}
					if (XmlCharType.IsSurrogate(num))
					{
						ptr2 = XmlEncodedRawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, ptr2);
						pSrc += 2;
						continue;
					}
					if (num <= 127 || num >= 65534)
					{
						ptr2 = this.InvalidXmlChar(num, ptr2, true);
						pSrc++;
						continue;
					}
					*ptr2 = (char)num;
					ptr2++;
					pSrc++;
					continue;
					IL_01D3:
					pSrc++;
				}
			}
			this.bufPos = (int)((long)(ptr2 - ptr));
			array = null;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001612C File Offset: 0x0001432C
		protected unsafe void WriteElementTextBlock(char* pSrc, char* pSrcEnd)
		{
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			char* ptr2 = ptr + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr3 = ptr2 + (long)(pSrcEnd - pSrc) * 2L / 2L;
				if (ptr3 != ptr + this.bufLen)
				{
					ptr3 = ptr + this.bufLen;
				}
				while (ptr2 < ptr3 && (this.xmlCharType.charProperties[num = (int)(*pSrc)] & 128) != 0)
				{
					*ptr2 = (char)num;
					ptr2++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (ptr2 < ptr3)
				{
					if (num <= 38)
					{
						switch (num)
						{
						case 9:
							goto IL_010F;
						case 10:
							if (this.newLineHandling == NewLineHandling.Replace)
							{
								ptr2 = this.WriteNewLine(ptr2);
								goto IL_01D6;
							}
							*ptr2 = (char)num;
							ptr2++;
							goto IL_01D6;
						case 11:
						case 12:
							break;
						case 13:
							switch (this.newLineHandling)
							{
							case NewLineHandling.Replace:
								if (pSrc[1] == '\n')
								{
									pSrc++;
								}
								ptr2 = this.WriteNewLine(ptr2);
								goto IL_01D6;
							case NewLineHandling.Entitize:
								ptr2 = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr2);
								goto IL_01D6;
							case NewLineHandling.None:
								*ptr2 = (char)num;
								ptr2++;
								goto IL_01D6;
							default:
								goto IL_01D6;
							}
							break;
						default:
							if (num == 34)
							{
								goto IL_010F;
							}
							if (num == 38)
							{
								ptr2 = XmlEncodedRawTextWriter.AmpEntity(ptr2);
								goto IL_01D6;
							}
							break;
						}
					}
					else
					{
						if (num == 39)
						{
							goto IL_010F;
						}
						if (num == 60)
						{
							ptr2 = XmlEncodedRawTextWriter.LtEntity(ptr2);
							goto IL_01D6;
						}
						if (num == 62)
						{
							ptr2 = XmlEncodedRawTextWriter.GtEntity(ptr2);
							goto IL_01D6;
						}
					}
					if (XmlCharType.IsSurrogate(num))
					{
						ptr2 = XmlEncodedRawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, ptr2);
						pSrc += 2;
						continue;
					}
					if (num <= 127 || num >= 65534)
					{
						ptr2 = this.InvalidXmlChar(num, ptr2, true);
						pSrc++;
						continue;
					}
					*ptr2 = (char)num;
					ptr2++;
					pSrc++;
					continue;
					IL_01D6:
					pSrc++;
					continue;
					IL_010F:
					*ptr2 = (char)num;
					ptr2++;
					goto IL_01D6;
				}
				this.bufPos = (int)((long)(ptr2 - ptr));
				this.FlushBuffer();
				ptr2 = ptr + 1;
			}
			this.bufPos = (int)((long)(ptr2 - ptr));
			this.textPos = this.bufPos;
			this.contentPos = 0;
			array = null;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001633C File Offset: 0x0001453C
		protected unsafe void RawText(string s)
		{
			fixed (string text = s)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.RawText(ptr, ptr + s.Length);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00016370 File Offset: 0x00014570
		protected unsafe void RawText(char* pSrcBegin, char* pSrcEnd)
		{
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			char* ptr2 = ptr + this.bufPos;
			char* ptr3 = pSrcBegin;
			int num = 0;
			for (;;)
			{
				char* ptr4 = ptr2 + (long)(pSrcEnd - ptr3) * 2L / 2L;
				if (ptr4 != ptr + this.bufLen)
				{
					ptr4 = ptr + this.bufLen;
				}
				while (ptr2 < ptr4 && (num = (int)(*ptr3)) < 55296)
				{
					ptr3++;
					*ptr2 = (char)num;
					ptr2++;
				}
				if (ptr3 >= pSrcEnd)
				{
					break;
				}
				if (ptr2 >= ptr4)
				{
					this.bufPos = (int)((long)(ptr2 - ptr));
					this.FlushBuffer();
					ptr2 = ptr + 1;
				}
				else if (XmlCharType.IsSurrogate(num))
				{
					ptr2 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr3, pSrcEnd, ptr2);
					ptr3 += 2;
				}
				else if (num <= 127 || num >= 65534)
				{
					ptr2 = this.InvalidXmlChar(num, ptr2, false);
					ptr3++;
				}
				else
				{
					*ptr2 = (char)num;
					ptr2++;
					ptr3++;
				}
			}
			this.bufPos = (int)((long)(ptr2 - ptr));
			array = null;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001647C File Offset: 0x0001467C
		protected unsafe void WriteRawWithCharChecking(char* pSrcBegin, char* pSrcEnd)
		{
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			char* ptr2 = pSrcBegin;
			char* ptr3 = ptr + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr4 = ptr3 + (long)(pSrcEnd - ptr2) * 2L / 2L;
				if (ptr4 != ptr + this.bufLen)
				{
					ptr4 = ptr + this.bufLen;
				}
				while (ptr3 < ptr4 && (this.xmlCharType.charProperties[num = (int)(*ptr2)] & 64) != 0)
				{
					*ptr3 = (char)num;
					ptr3++;
					ptr2++;
				}
				if (ptr2 >= pSrcEnd)
				{
					break;
				}
				if (ptr3 < ptr4)
				{
					if (num <= 38)
					{
						switch (num)
						{
						case 9:
							goto IL_00DF;
						case 10:
							if (this.newLineHandling == NewLineHandling.Replace)
							{
								ptr3 = this.WriteNewLine(ptr3);
								goto IL_0186;
							}
							*ptr3 = (char)num;
							ptr3++;
							goto IL_0186;
						case 11:
						case 12:
							break;
						case 13:
							if (this.newLineHandling == NewLineHandling.Replace)
							{
								if (ptr2[1] == '\n')
								{
									ptr2++;
								}
								ptr3 = this.WriteNewLine(ptr3);
								goto IL_0186;
							}
							*ptr3 = (char)num;
							ptr3++;
							goto IL_0186;
						default:
							if (num == 38)
							{
								goto IL_00DF;
							}
							break;
						}
					}
					else if (num == 60 || num == 93)
					{
						goto IL_00DF;
					}
					if (XmlCharType.IsSurrogate(num))
					{
						ptr3 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr2, pSrcEnd, ptr3);
						ptr2 += 2;
						continue;
					}
					if (num <= 127 || num >= 65534)
					{
						ptr3 = this.InvalidXmlChar(num, ptr3, false);
						ptr2++;
						continue;
					}
					*ptr3 = (char)num;
					ptr3++;
					ptr2++;
					continue;
					IL_0186:
					ptr2++;
					continue;
					IL_00DF:
					*ptr3 = (char)num;
					ptr3++;
					goto IL_0186;
				}
				this.bufPos = (int)((long)(ptr3 - ptr));
				this.FlushBuffer();
				ptr3 = ptr + 1;
			}
			this.bufPos = (int)((long)(ptr3 - ptr));
			array = null;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00016628 File Offset: 0x00014828
		protected unsafe void WriteCommentOrPi(string text, int stopChar)
		{
			if (text.Length == 0)
			{
				if (this.bufPos >= this.bufLen)
				{
					this.FlushBuffer();
				}
				return;
			}
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char[] array;
				char* ptr2;
				if ((array = this.bufChars) == null || array.Length == 0)
				{
					ptr2 = null;
				}
				else
				{
					ptr2 = &array[0];
				}
				char* ptr3 = ptr;
				char* ptr4 = ptr + text.Length;
				char* ptr5 = ptr2 + this.bufPos;
				int num = 0;
				for (;;)
				{
					char* ptr6 = ptr5 + (long)(ptr4 - ptr3) * 2L / 2L;
					if (ptr6 != ptr2 + this.bufLen)
					{
						ptr6 = ptr2 + this.bufLen;
					}
					while (ptr5 < ptr6 && (this.xmlCharType.charProperties[num = (int)(*ptr3)] & 64) != 0 && num != stopChar)
					{
						*ptr5 = (char)num;
						ptr5++;
						ptr3++;
					}
					if (ptr3 >= ptr4)
					{
						break;
					}
					if (ptr5 < ptr6)
					{
						if (num <= 45)
						{
							switch (num)
							{
							case 9:
								goto IL_0226;
							case 10:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									ptr5 = this.WriteNewLine(ptr5);
									goto IL_0296;
								}
								*ptr5 = (char)num;
								ptr5++;
								goto IL_0296;
							case 11:
							case 12:
								break;
							case 13:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									if (ptr3[1] == '\n')
									{
										ptr3++;
									}
									ptr5 = this.WriteNewLine(ptr5);
									goto IL_0296;
								}
								*ptr5 = (char)num;
								ptr5++;
								goto IL_0296;
							default:
								if (num == 38)
								{
									goto IL_0226;
								}
								if (num == 45)
								{
									*ptr5 = '-';
									ptr5++;
									if (num == stopChar && (ptr3 + 1 == ptr4 || ptr3[1] == '-'))
									{
										*ptr5 = ' ';
										ptr5++;
										goto IL_0296;
									}
									goto IL_0296;
								}
								break;
							}
						}
						else
						{
							if (num == 60)
							{
								goto IL_0226;
							}
							if (num != 63)
							{
								if (num == 93)
								{
									*ptr5 = ']';
									ptr5++;
									goto IL_0296;
								}
							}
							else
							{
								*ptr5 = '?';
								ptr5++;
								if (num == stopChar && ptr3 + 1 < ptr4 && ptr3[1] == '>')
								{
									*ptr5 = ' ';
									ptr5++;
									goto IL_0296;
								}
								goto IL_0296;
							}
						}
						if (XmlCharType.IsSurrogate(num))
						{
							ptr5 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr3, ptr4, ptr5);
							ptr3 += 2;
							continue;
						}
						if (num <= 127 || num >= 65534)
						{
							ptr5 = this.InvalidXmlChar(num, ptr5, false);
							ptr3++;
							continue;
						}
						*ptr5 = (char)num;
						ptr5++;
						ptr3++;
						continue;
						IL_0296:
						ptr3++;
						continue;
						IL_0226:
						*ptr5 = (char)num;
						ptr5++;
						goto IL_0296;
					}
					this.bufPos = (int)((long)(ptr5 - ptr2));
					this.FlushBuffer();
					ptr5 = ptr2 + 1;
				}
				this.bufPos = (int)((long)(ptr5 - ptr2));
				array = null;
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000168E8 File Offset: 0x00014AE8
		protected unsafe void WriteCDataSection(string text)
		{
			if (text.Length == 0)
			{
				if (this.bufPos >= this.bufLen)
				{
					this.FlushBuffer();
				}
				return;
			}
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char[] array;
				char* ptr2;
				if ((array = this.bufChars) == null || array.Length == 0)
				{
					ptr2 = null;
				}
				else
				{
					ptr2 = &array[0];
				}
				char* ptr3 = ptr;
				char* ptr4 = ptr + text.Length;
				char* ptr5 = ptr2 + this.bufPos;
				int num = 0;
				for (;;)
				{
					char* ptr6 = ptr5 + (long)(ptr4 - ptr3) * 2L / 2L;
					if (ptr6 != ptr2 + this.bufLen)
					{
						ptr6 = ptr2 + this.bufLen;
					}
					while (ptr5 < ptr6 && (this.xmlCharType.charProperties[num = (int)(*ptr3)] & 128) != 0 && num != 93)
					{
						*ptr5 = (char)num;
						ptr5++;
						ptr3++;
					}
					if (ptr3 >= ptr4)
					{
						break;
					}
					if (ptr5 < ptr6)
					{
						if (num <= 39)
						{
							switch (num)
							{
							case 9:
								goto IL_0210;
							case 10:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									ptr5 = this.WriteNewLine(ptr5);
									goto IL_0280;
								}
								*ptr5 = (char)num;
								ptr5++;
								goto IL_0280;
							case 11:
							case 12:
								break;
							case 13:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									if (ptr3[1] == '\n')
									{
										ptr3++;
									}
									ptr5 = this.WriteNewLine(ptr5);
									goto IL_0280;
								}
								*ptr5 = (char)num;
								ptr5++;
								goto IL_0280;
							default:
								if (num == 34 || num - 38 <= 1)
								{
									goto IL_0210;
								}
								break;
							}
						}
						else
						{
							if (num == 60)
							{
								goto IL_0210;
							}
							if (num == 62)
							{
								if (this.hadDoubleBracket && ptr5[-1] == ']')
								{
									ptr5 = XmlEncodedRawTextWriter.RawEndCData(ptr5);
									ptr5 = XmlEncodedRawTextWriter.RawStartCData(ptr5);
								}
								*ptr5 = '>';
								ptr5++;
								goto IL_0280;
							}
							if (num == 93)
							{
								if (ptr5[-1] == ']')
								{
									this.hadDoubleBracket = true;
								}
								else
								{
									this.hadDoubleBracket = false;
								}
								*ptr5 = ']';
								ptr5++;
								goto IL_0280;
							}
						}
						if (XmlCharType.IsSurrogate(num))
						{
							ptr5 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr3, ptr4, ptr5);
							ptr3 += 2;
							continue;
						}
						if (num <= 127 || num >= 65534)
						{
							ptr5 = this.InvalidXmlChar(num, ptr5, false);
							ptr3++;
							continue;
						}
						*ptr5 = (char)num;
						ptr5++;
						ptr3++;
						continue;
						IL_0280:
						ptr3++;
						continue;
						IL_0210:
						*ptr5 = (char)num;
						ptr5++;
						goto IL_0280;
					}
					this.bufPos = (int)((long)(ptr5 - ptr2));
					this.FlushBuffer();
					ptr5 = ptr2 + 1;
				}
				this.bufPos = (int)((long)(ptr5 - ptr2));
				array = null;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00016B94 File Offset: 0x00014D94
		private unsafe static char* EncodeSurrogate(char* pSrc, char* pSrcEnd, char* pDst)
		{
			int num = (int)(*pSrc);
			if (num > 56319)
			{
				throw XmlConvert.CreateInvalidHighSurrogateCharException((char)num);
			}
			if (pSrc + 1 >= pSrcEnd)
			{
				throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
			}
			int num2 = (int)pSrc[1];
			if (num2 >= 56320 && (LocalAppContextSwitches.DontThrowOnInvalidSurrogatePairs || num2 <= 57343))
			{
				*pDst = (char)num;
				pDst[1] = (char)num2;
				pDst += 2;
				return pDst;
			}
			throw XmlConvert.CreateInvalidSurrogatePairException((char)num2, (char)num);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00016C03 File Offset: 0x00014E03
		private unsafe char* InvalidXmlChar(int ch, char* pDst, bool entitize)
		{
			if (this.checkCharacters)
			{
				throw XmlConvert.CreateInvalidCharException((char)ch, '\0');
			}
			if (entitize)
			{
				return XmlEncodedRawTextWriter.CharEntity(pDst, (char)ch);
			}
			*pDst = (char)ch;
			pDst++;
			return pDst;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00016C2C File Offset: 0x00014E2C
		internal unsafe void EncodeChar(ref char* pSrc, char* pSrcEnd, ref char* pDst)
		{
			int num = (int)(*pSrc);
			if (XmlCharType.IsSurrogate(num))
			{
				pDst = XmlEncodedRawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, pDst);
				pSrc += (IntPtr)2 * 2;
				return;
			}
			if (num <= 127 || num >= 65534)
			{
				pDst = this.InvalidXmlChar(num, pDst, false);
				pSrc += 2;
				return;
			}
			*pDst = (short)((ushort)num);
			pDst += 2;
			pSrc += 2;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00016C8C File Offset: 0x00014E8C
		protected void ChangeTextContentMark(bool value)
		{
			this.inTextContent = value;
			if (this.lastMarkPos + 1 == this.textContentMarks.Length)
			{
				this.GrowTextContentMarks();
			}
			int[] array = this.textContentMarks;
			int num = this.lastMarkPos + 1;
			this.lastMarkPos = num;
			array[num] = this.bufPos;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00016CD8 File Offset: 0x00014ED8
		private void GrowTextContentMarks()
		{
			int[] array = new int[this.textContentMarks.Length * 2];
			Array.Copy(this.textContentMarks, array, this.textContentMarks.Length);
			this.textContentMarks = array;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00016D10 File Offset: 0x00014F10
		protected unsafe char* WriteNewLine(char* pDst)
		{
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			this.bufPos = (int)((long)(pDst - ptr));
			this.RawText(this.newLineChars);
			return ptr + this.bufPos;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00016D5E File Offset: 0x00014F5E
		protected unsafe static char* LtEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = 'l';
			pDst[2] = 't';
			pDst[3] = ';';
			return pDst + 4;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00016D82 File Offset: 0x00014F82
		protected unsafe static char* GtEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = 'g';
			pDst[2] = 't';
			pDst[3] = ';';
			return pDst + 4;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00016DA6 File Offset: 0x00014FA6
		protected unsafe static char* AmpEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = 'a';
			pDst[2] = 'm';
			pDst[3] = 'p';
			pDst[4] = ';';
			return pDst + 5;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00016DD3 File Offset: 0x00014FD3
		protected unsafe static char* QuoteEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = 'q';
			pDst[2] = 'u';
			pDst[3] = 'o';
			pDst[4] = 't';
			pDst[5] = ';';
			return pDst + 6;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00016E09 File Offset: 0x00015009
		protected unsafe static char* TabEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = '#';
			pDst[2] = 'x';
			pDst[3] = '9';
			pDst[4] = ';';
			return pDst + 5;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00016E36 File Offset: 0x00015036
		protected unsafe static char* LineFeedEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = '#';
			pDst[2] = 'x';
			pDst[3] = 'A';
			pDst[4] = ';';
			return pDst + 5;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00016E63 File Offset: 0x00015063
		protected unsafe static char* CarriageReturnEntity(char* pDst)
		{
			*pDst = '&';
			pDst[1] = '#';
			pDst[2] = 'x';
			pDst[3] = 'D';
			pDst[4] = ';';
			return pDst + 5;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00016E90 File Offset: 0x00015090
		private unsafe static char* CharEntity(char* pDst, char ch)
		{
			int num = (int)ch;
			string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
			*pDst = '&';
			pDst[1] = '#';
			pDst[2] = 'x';
			pDst += 3;
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				char* ptr2 = ptr;
				while ((*(pDst++) = *(ptr2++)) != '\0')
				{
				}
			}
			pDst[-1] = ';';
			return pDst;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00016EFC File Offset: 0x000150FC
		protected unsafe static char* RawStartCData(char* pDst)
		{
			*pDst = '<';
			pDst[1] = '!';
			pDst[2] = '[';
			pDst[3] = 'C';
			pDst[4] = 'D';
			pDst[5] = 'A';
			pDst[6] = 'T';
			pDst[7] = 'A';
			pDst[8] = '[';
			return pDst + 9;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00016F59 File Offset: 0x00015159
		protected unsafe static char* RawEndCData(char* pDst)
		{
			*pDst = ']';
			pDst[1] = ']';
			pDst[2] = '>';
			return pDst + 3;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00016F74 File Offset: 0x00015174
		protected void ValidateContentChars(string chars, string propertyName, bool allowOnlyWhitespace)
		{
			if (!allowOnlyWhitespace)
			{
				for (int i = 0; i < chars.Length; i++)
				{
					if (!this.xmlCharType.IsTextChar(chars[i]))
					{
						char c = chars[i];
						if (c <= '&')
						{
							switch (c)
							{
							case '\t':
							case '\n':
							case '\r':
								goto IL_011C;
							case '\v':
							case '\f':
								goto IL_00A2;
							default:
								if (c != '&')
								{
									goto IL_00A2;
								}
								break;
							}
						}
						else if (c != '<' && c != ']')
						{
							goto IL_00A2;
						}
						string text = "'{0}', hexadecimal value {1}, is an invalid character.";
						object[] array = XmlException.BuildCharExceptionArgs(chars, i);
						string text2 = Res.GetString(text, array);
						goto IL_012D;
						IL_00A2:
						if (XmlCharType.IsHighSurrogate((int)chars[i]))
						{
							if (i + 1 < chars.Length && XmlCharType.IsLowSurrogate((int)chars[i + 1]))
							{
								i++;
								goto IL_011C;
							}
							text2 = Res.GetString("The surrogate pair is invalid. Missing a low surrogate character.");
						}
						else
						{
							if (!XmlCharType.IsLowSurrogate((int)chars[i]))
							{
								goto IL_011C;
							}
							text2 = Res.GetString("Invalid high surrogate character (0x{0}). A high surrogate character must have a value from range (0xD800 - 0xDBFF).", new object[] { ((uint)chars[i]).ToString("X", CultureInfo.InvariantCulture) });
						}
						IL_012D:
						string text3 = "XmlWriterSettings.{0} can contain only valid XML text content characters when XmlWriterSettings.CheckCharacters is true. {1}";
						array = new string[] { propertyName, text2 };
						throw new ArgumentException(Res.GetString(text3, array));
					}
					IL_011C:;
				}
				return;
			}
			if (!this.xmlCharType.IsOnlyWhitespace(chars))
			{
				throw new ArgumentException(Res.GetString("XmlWriterSettings.{0} can contain only valid XML white space characters when XmlWriterSettings.CheckCharacters and XmlWriterSettings.NewLineOnAttributes are true.", new object[] { propertyName }));
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000170CE File Offset: 0x000152CE
		protected void CheckAsyncCall()
		{
			if (!this.useAsync)
			{
				throw new InvalidOperationException(Res.GetString("Set XmlWriterSettings.Async to true if you want to use Async Methods."));
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000170E8 File Offset: 0x000152E8
		internal override async Task WriteXmlDeclarationAsync(XmlStandalone standalone)
		{
			this.CheckAsyncCall();
			if (!this.omitXmlDeclaration && !this.autoXmlDeclaration)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					this.ChangeTextContentMark(false);
				}
				await this.RawTextAsync("<?xml version=\"").ConfigureAwait(false);
				await this.RawTextAsync("1.0").ConfigureAwait(false);
				if (this.encoding != null)
				{
					await this.RawTextAsync("\" encoding=\"").ConfigureAwait(false);
					await this.RawTextAsync(this.encoding.WebName).ConfigureAwait(false);
				}
				if (standalone != XmlStandalone.Omit)
				{
					await this.RawTextAsync("\" standalone=\"").ConfigureAwait(false);
					await this.RawTextAsync((standalone == XmlStandalone.Yes) ? "yes" : "no").ConfigureAwait(false);
				}
				await this.RawTextAsync("\"?>").ConfigureAwait(false);
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00017133 File Offset: 0x00015333
		internal override Task WriteXmlDeclarationAsync(string xmldecl)
		{
			this.CheckAsyncCall();
			if (!this.omitXmlDeclaration && !this.autoXmlDeclaration)
			{
				return this.WriteProcessingInstructionAsync("xml", xmldecl);
			}
			return AsyncHelper.DoneTask;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00017160 File Offset: 0x00015360
		public override async Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			await this.RawTextAsync("<!DOCTYPE ").ConfigureAwait(false);
			await this.RawTextAsync(name).ConfigureAwait(false);
			int num;
			if (pubid != null)
			{
				await this.RawTextAsync(" PUBLIC \"").ConfigureAwait(false);
				await this.RawTextAsync(pubid).ConfigureAwait(false);
				await this.RawTextAsync("\" \"").ConfigureAwait(false);
				if (sysid != null)
				{
					await this.RawTextAsync(sysid).ConfigureAwait(false);
				}
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 34;
			}
			else if (sysid != null)
			{
				await this.RawTextAsync(" SYSTEM \"").ConfigureAwait(false);
				await this.RawTextAsync(sysid).ConfigureAwait(false);
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 34;
			}
			else
			{
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 32;
			}
			if (subset != null)
			{
				char[] array4 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array4[num] = 91;
				await this.RawTextAsync(subset).ConfigureAwait(false);
				char[] array5 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array5[num] = 93;
			}
			char[] array6 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array6[num] = 62;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000171C4 File Offset: 0x000153C4
		public override Task WriteStartElementAsync(string prefix, string localName, string ns)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			Task task;
			if (prefix != null && prefix.Length != 0)
			{
				task = this.RawTextAsync(prefix + ":" + localName);
			}
			else
			{
				task = this.RawTextAsync(localName);
			}
			return task.CallVoidFuncWhenFinish(new Action(this.WriteStartElementAsync_SetAttEndPos));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00017242 File Offset: 0x00015442
		private void WriteStartElementAsync_SetAttEndPos()
		{
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00017250 File Offset: 0x00015450
		internal override Task WriteEndElementAsync(string prefix, string localName, string ns)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num;
			if (this.contentPos == this.bufPos)
			{
				this.bufPos--;
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 32;
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 47;
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 62;
				return AsyncHelper.DoneTask;
			}
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 60;
			char[] array5 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array5[num] = 47;
			if (prefix != null && prefix.Length != 0)
			{
				return this.RawTextAsync(prefix + ":" + localName + ">");
			}
			return this.RawTextAsync(localName + ">");
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00017354 File Offset: 0x00015554
		internal override Task WriteFullEndElementAsync(string prefix, string localName, string ns)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 47;
			if (prefix != null && prefix.Length != 0)
			{
				return this.RawTextAsync(prefix + ":" + localName + ">");
			}
			return this.RawTextAsync(localName + ">");
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000173E8 File Offset: 0x000155E8
		protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string ns)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			if (this.attrEndPos == this.bufPos)
			{
				char[] array = this.bufChars;
				int num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 32;
			}
			Task task;
			if (prefix != null && prefix.Length > 0)
			{
				task = this.RawTextAsync(prefix + ":" + localName);
			}
			else
			{
				task = this.RawTextAsync(localName);
			}
			return task.CallVoidFuncWhenFinish(new Action(this.WriteStartAttribute_SetInAttribute));
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00017478 File Offset: 0x00015678
		private void WriteStartAttribute_SetInAttribute()
		{
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 61;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 34;
			this.inAttributeValue = true;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000174C0 File Offset: 0x000156C0
		protected internal override Task WriteEndAttributeAsync()
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 34;
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
			return AsyncHelper.DoneTask;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001751C File Offset: 0x0001571C
		internal override async Task WriteNamespaceDeclarationAsync(string prefix, string namespaceName)
		{
			this.CheckAsyncCall();
			await this.WriteStartNamespaceDeclarationAsync(prefix).ConfigureAwait(false);
			await this.WriteStringAsync(namespaceName).ConfigureAwait(false);
			await this.WriteEndNamespaceDeclarationAsync().ConfigureAwait(false);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00017570 File Offset: 0x00015770
		internal override async Task WriteStartNamespaceDeclarationAsync(string prefix)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			if (prefix.Length == 0)
			{
				await this.RawTextAsync(" xmlns=\"").ConfigureAwait(false);
			}
			else
			{
				await this.RawTextAsync(" xmlns:").ConfigureAwait(false);
				await this.RawTextAsync(prefix).ConfigureAwait(false);
				char[] array = this.bufChars;
				int num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 61;
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 34;
			}
			this.inAttributeValue = true;
			if (this.trackTextContent && !this.inTextContent)
			{
				this.ChangeTextContentMark(true);
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000175BC File Offset: 0x000157BC
		internal override Task WriteEndNamespaceDeclarationAsync()
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			this.inAttributeValue = false;
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 34;
			this.attrEndPos = this.bufPos;
			return AsyncHelper.DoneTask;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00017618 File Offset: 0x00015818
		public override async Task WriteCDataAsync(string text)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num;
			if (this.mergeCDataSections && this.bufPos == this.cdataPos)
			{
				this.bufPos -= 3;
			}
			else
			{
				char[] array = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array[num] = 60;
				char[] array2 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array2[num] = 33;
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 91;
				char[] array4 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array4[num] = 67;
				char[] array5 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array5[num] = 68;
				char[] array6 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array6[num] = 65;
				char[] array7 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array7[num] = 84;
				char[] array8 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array8[num] = 65;
				char[] array9 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array9[num] = 91;
			}
			await this.WriteCDataSectionAsync(text).ConfigureAwait(false);
			char[] array10 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array10[num] = 93;
			char[] array11 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array11[num] = 93;
			char[] array12 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array12[num] = 62;
			this.textPos = this.bufPos;
			this.cdataPos = this.bufPos;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00017664 File Offset: 0x00015864
		public override async Task WriteCommentAsync(string text)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 33;
			char[] array3 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array3[num] = 45;
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 45;
			await this.WriteCommentOrPiAsync(text, 45).ConfigureAwait(false);
			char[] array5 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array5[num] = 45;
			char[] array6 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array6[num] = 45;
			char[] array7 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array7[num] = 62;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000176B0 File Offset: 0x000158B0
		public override async Task WriteProcessingInstructionAsync(string name, string text)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 60;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 63;
			await this.RawTextAsync(name).ConfigureAwait(false);
			if (text.Length > 0)
			{
				char[] array3 = this.bufChars;
				num = this.bufPos;
				this.bufPos = num + 1;
				array3[num] = 32;
				await this.WriteCommentOrPiAsync(text, 63).ConfigureAwait(false);
			}
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 63;
			char[] array5 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array5[num] = 62;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00017704 File Offset: 0x00015904
		public override async Task WriteEntityRefAsync(string name)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			int num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 38;
			await this.RawTextAsync(name).ConfigureAwait(false);
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 59;
			if (this.bufPos > this.bufLen)
			{
				await this.FlushBufferAsync().ConfigureAwait(false);
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00017750 File Offset: 0x00015950
		public override async Task WriteCharEntityAsync(char ch)
		{
			this.CheckAsyncCall();
			int num = (int)ch;
			string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
			if (this.checkCharacters && !this.xmlCharType.IsCharData(ch))
			{
				throw XmlConvert.CreateInvalidCharException(ch, '\0');
			}
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			char[] array = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array[num] = 38;
			char[] array2 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array2[num] = 35;
			char[] array3 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array3[num] = 120;
			await this.RawTextAsync(text).ConfigureAwait(false);
			char[] array4 = this.bufChars;
			num = this.bufPos;
			this.bufPos = num + 1;
			array4[num] = 59;
			if (this.bufPos > this.bufLen)
			{
				await this.FlushBufferAsync().ConfigureAwait(false);
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001779B File Offset: 0x0001599B
		public override Task WriteWhitespaceAsync(string ws)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			if (this.inAttributeValue)
			{
				return this.WriteAttributeTextBlockAsync(ws);
			}
			return this.WriteElementTextBlockAsync(ws);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000177D1 File Offset: 0x000159D1
		public override Task WriteStringAsync(string text)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && !this.inTextContent)
			{
				this.ChangeTextContentMark(true);
			}
			if (this.inAttributeValue)
			{
				return this.WriteAttributeTextBlockAsync(text);
			}
			return this.WriteElementTextBlockAsync(text);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00017808 File Offset: 0x00015A08
		public override async Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			int num = XmlCharType.CombineSurrogateChar((int)lowChar, (int)highChar);
			char[] array = this.bufChars;
			int num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array[num2] = 38;
			char[] array2 = this.bufChars;
			num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array2[num2] = 35;
			char[] array3 = this.bufChars;
			num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array3[num2] = 120;
			await this.RawTextAsync(num.ToString("X", NumberFormatInfo.InvariantInfo)).ConfigureAwait(false);
			char[] array4 = this.bufChars;
			num2 = this.bufPos;
			this.bufPos = num2 + 1;
			array4[num2] = 59;
			this.textPos = this.bufPos;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001785B File Offset: 0x00015A5B
		public override Task WriteCharsAsync(char[] buffer, int index, int count)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && !this.inTextContent)
			{
				this.ChangeTextContentMark(true);
			}
			if (this.inAttributeValue)
			{
				return this.WriteAttributeTextBlockAsync(buffer, index, count);
			}
			return this.WriteElementTextBlockAsync(buffer, index, count);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00017898 File Offset: 0x00015A98
		public override async Task WriteRawAsync(char[] buffer, int index, int count)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			await this.WriteRawWithCharCheckingAsync(buffer, index, count).ConfigureAwait(false);
			this.textPos = this.bufPos;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000178F4 File Offset: 0x00015AF4
		public override async Task WriteRawAsync(string data)
		{
			this.CheckAsyncCall();
			if (this.trackTextContent && this.inTextContent)
			{
				this.ChangeTextContentMark(false);
			}
			await this.WriteRawWithCharCheckingAsync(data).ConfigureAwait(false);
			this.textPos = this.bufPos;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00017940 File Offset: 0x00015B40
		public override async Task FlushAsync()
		{
			this.CheckAsyncCall();
			await this.FlushBufferAsync().ConfigureAwait(false);
			await this.FlushEncoderAsync().ConfigureAwait(false);
			if (this.stream != null)
			{
				await this.stream.FlushAsync().ConfigureAwait(false);
			}
			else if (this.writer != null)
			{
				await this.writer.FlushAsync().ConfigureAwait(false);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00017984 File Offset: 0x00015B84
		protected virtual async Task FlushBufferAsync()
		{
			try
			{
				if (!this.writeToNull)
				{
					if (this.stream != null)
					{
						if (this.trackTextContent)
						{
							this.charEntityFallback.Reset(this.textContentMarks, this.lastMarkPos);
							if ((this.lastMarkPos & 1) != 0)
							{
								this.textContentMarks[1] = 1;
								this.lastMarkPos = 1;
							}
							else
							{
								this.lastMarkPos = 0;
							}
						}
						await this.EncodeCharsAsync(1, this.bufPos, true).ConfigureAwait(false);
					}
					else
					{
						await this.writer.WriteAsync(this.bufChars, 1, this.bufPos - 1).ConfigureAwait(false);
					}
				}
			}
			catch
			{
				this.writeToNull = true;
				throw;
			}
			finally
			{
				this.bufChars[0] = this.bufChars[this.bufPos - 1];
				this.textPos = ((this.textPos == this.bufPos) ? 1 : 0);
				this.attrEndPos = ((this.attrEndPos == this.bufPos) ? 1 : 0);
				this.contentPos = 0;
				this.cdataPos = 0;
				this.bufPos = 1;
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000179C8 File Offset: 0x00015BC8
		private async Task EncodeCharsAsync(int startOffset, int endOffset, bool writeAllToStream)
		{
			while (startOffset < endOffset)
			{
				if (this.charEntityFallback != null)
				{
					this.charEntityFallback.StartOffset = startOffset;
				}
				int num;
				int num2;
				bool flag;
				this.encoder.Convert(this.bufChars, startOffset, endOffset - startOffset, this.bufBytes, this.bufBytesUsed, this.bufBytes.Length - this.bufBytesUsed, false, out num, out num2, out flag);
				startOffset += num;
				this.bufBytesUsed += num2;
				if (this.bufBytesUsed >= this.bufBytes.Length - 16)
				{
					await this.stream.WriteAsync(this.bufBytes, 0, this.bufBytesUsed).ConfigureAwait(false);
					this.bufBytesUsed = 0;
				}
			}
			if (writeAllToStream && this.bufBytesUsed > 0)
			{
				await this.stream.WriteAsync(this.bufBytes, 0, this.bufBytesUsed).ConfigureAwait(false);
				this.bufBytesUsed = 0;
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00017A24 File Offset: 0x00015C24
		private Task FlushEncoderAsync()
		{
			if (this.stream != null)
			{
				int num;
				int num2;
				bool flag;
				this.encoder.Convert(this.bufChars, 1, 0, this.bufBytes, 0, this.bufBytes.Length, true, out num, out num2, out flag);
				if (num2 != 0)
				{
					return this.stream.WriteAsync(this.bufBytes, 0, num2);
				}
			}
			return AsyncHelper.DoneTask;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00017A80 File Offset: 0x00015C80
		[SecuritySafeCritical]
		protected unsafe int WriteAttributeTextBlockNoFlush(char* pSrc, char* pSrcEnd)
		{
			char* ptr = pSrc;
			char[] array;
			char* ptr2;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &array[0];
			}
			char* ptr3 = ptr2 + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr4 = ptr3 + (long)(pSrcEnd - pSrc) * 2L / 2L;
				if (ptr4 != ptr2 + this.bufLen)
				{
					ptr4 = ptr2 + this.bufLen;
				}
				while (ptr3 < ptr4 && (this.xmlCharType.charProperties[num = (int)(*pSrc)] & 128) != 0)
				{
					*ptr3 = (char)num;
					ptr3++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					goto IL_01EE;
				}
				if (ptr3 >= ptr4)
				{
					break;
				}
				if (num <= 38)
				{
					switch (num)
					{
					case 9:
						if (this.newLineHandling == NewLineHandling.None)
						{
							*ptr3 = (char)num;
							ptr3++;
							goto IL_01E4;
						}
						ptr3 = XmlEncodedRawTextWriter.TabEntity(ptr3);
						goto IL_01E4;
					case 10:
						if (this.newLineHandling == NewLineHandling.None)
						{
							*ptr3 = (char)num;
							ptr3++;
							goto IL_01E4;
						}
						ptr3 = XmlEncodedRawTextWriter.LineFeedEntity(ptr3);
						goto IL_01E4;
					case 11:
					case 12:
						break;
					case 13:
						if (this.newLineHandling == NewLineHandling.None)
						{
							*ptr3 = (char)num;
							ptr3++;
							goto IL_01E4;
						}
						ptr3 = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr3);
						goto IL_01E4;
					default:
						if (num == 34)
						{
							ptr3 = XmlEncodedRawTextWriter.QuoteEntity(ptr3);
							goto IL_01E4;
						}
						if (num == 38)
						{
							ptr3 = XmlEncodedRawTextWriter.AmpEntity(ptr3);
							goto IL_01E4;
						}
						break;
					}
				}
				else
				{
					if (num == 39)
					{
						*ptr3 = (char)num;
						ptr3++;
						goto IL_01E4;
					}
					if (num == 60)
					{
						ptr3 = XmlEncodedRawTextWriter.LtEntity(ptr3);
						goto IL_01E4;
					}
					if (num == 62)
					{
						ptr3 = XmlEncodedRawTextWriter.GtEntity(ptr3);
						goto IL_01E4;
					}
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr3 = XmlEncodedRawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, ptr3);
					pSrc += 2;
					continue;
				}
				if (num <= 127 || num >= 65534)
				{
					ptr3 = this.InvalidXmlChar(num, ptr3, true);
					pSrc++;
					continue;
				}
				*ptr3 = (char)num;
				ptr3++;
				pSrc++;
				continue;
				IL_01E4:
				pSrc++;
			}
			this.bufPos = (int)((long)(ptr3 - ptr2));
			return (int)((long)(pSrc - ptr));
			IL_01EE:
			this.bufPos = (int)((long)(ptr3 - ptr2));
			array = null;
			return -1;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00017C8C File Offset: 0x00015E8C
		[SecuritySafeCritical]
		protected unsafe int WriteAttributeTextBlockNoFlush(char[] chars, int index, int count)
		{
			if (count == 0)
			{
				return -1;
			}
			fixed (char* ptr = &chars[index])
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + count;
				return this.WriteAttributeTextBlockNoFlush(ptr2, ptr3);
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00017CB8 File Offset: 0x00015EB8
		[SecuritySafeCritical]
		protected unsafe int WriteAttributeTextBlockNoFlush(string text, int index, int count)
		{
			if (count == 0)
			{
				return -1;
			}
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = ptr + index;
			char* ptr3 = ptr2 + count;
			return this.WriteAttributeTextBlockNoFlush(ptr2, ptr3);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00017CF0 File Offset: 0x00015EF0
		protected async Task WriteAttributeTextBlockAsync(char[] chars, int index, int count)
		{
			int writeLen = 0;
			int curIndex = index;
			int leftCount = count;
			do
			{
				writeLen = this.WriteAttributeTextBlockNoFlush(chars, curIndex, leftCount);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00017D4C File Offset: 0x00015F4C
		protected Task WriteAttributeTextBlockAsync(string text)
		{
			int num = 0;
			int num2 = text.Length;
			int num3 = this.WriteAttributeTextBlockNoFlush(text, num, num2);
			num += num3;
			num2 -= num3;
			if (num3 >= 0)
			{
				return this._WriteAttributeTextBlockAsync(text, num, num2);
			}
			return AsyncHelper.DoneTask;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00017D8C File Offset: 0x00015F8C
		private async Task _WriteAttributeTextBlockAsync(string text, int curIndex, int leftCount)
		{
			await this.FlushBufferAsync().ConfigureAwait(false);
			int writeLen;
			do
			{
				writeLen = this.WriteAttributeTextBlockNoFlush(text, curIndex, leftCount);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00017DE8 File Offset: 0x00015FE8
		[SecuritySafeCritical]
		protected unsafe int WriteElementTextBlockNoFlush(char* pSrc, char* pSrcEnd, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			char* ptr = pSrc;
			char[] array;
			char* ptr2;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &array[0];
			}
			char* ptr3 = ptr2 + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr4 = ptr3 + (long)(pSrcEnd - pSrc) * 2L / 2L;
				if (ptr4 != ptr2 + this.bufLen)
				{
					ptr4 = ptr2 + this.bufLen;
				}
				while (ptr3 < ptr4 && (this.xmlCharType.charProperties[num = (int)(*pSrc)] & 128) != 0)
				{
					*ptr3 = (char)num;
					ptr3++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					goto IL_020F;
				}
				if (ptr3 >= ptr4)
				{
					break;
				}
				if (num <= 38)
				{
					switch (num)
					{
					case 9:
						goto IL_011A;
					case 10:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_13;
						}
						*ptr3 = (char)num;
						ptr3++;
						goto IL_0205;
					case 11:
					case 12:
						break;
					case 13:
						switch (this.newLineHandling)
						{
						case NewLineHandling.Replace:
							goto IL_0176;
						case NewLineHandling.Entitize:
							ptr3 = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr3);
							goto IL_0205;
						case NewLineHandling.None:
							*ptr3 = (char)num;
							ptr3++;
							goto IL_0205;
						default:
							goto IL_0205;
						}
						break;
					default:
						if (num == 34)
						{
							goto IL_011A;
						}
						if (num == 38)
						{
							ptr3 = XmlEncodedRawTextWriter.AmpEntity(ptr3);
							goto IL_0205;
						}
						break;
					}
				}
				else
				{
					if (num == 39)
					{
						goto IL_011A;
					}
					if (num == 60)
					{
						ptr3 = XmlEncodedRawTextWriter.LtEntity(ptr3);
						goto IL_0205;
					}
					if (num == 62)
					{
						ptr3 = XmlEncodedRawTextWriter.GtEntity(ptr3);
						goto IL_0205;
					}
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr3 = XmlEncodedRawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, ptr3);
					pSrc += 2;
					continue;
				}
				if (num <= 127 || num >= 65534)
				{
					ptr3 = this.InvalidXmlChar(num, ptr3, true);
					pSrc++;
					continue;
				}
				*ptr3 = (char)num;
				ptr3++;
				pSrc++;
				continue;
				IL_0205:
				pSrc++;
				continue;
				IL_011A:
				*ptr3 = (char)num;
				ptr3++;
				goto IL_0205;
			}
			this.bufPos = (int)((long)(ptr3 - ptr2));
			return (int)((long)(pSrc - ptr));
			Block_13:
			this.bufPos = (int)((long)(ptr3 - ptr2));
			needWriteNewLine = true;
			return (int)((long)(pSrc - ptr));
			IL_0176:
			if (pSrc[1] == '\n')
			{
				pSrc++;
			}
			this.bufPos = (int)((long)(ptr3 - ptr2));
			needWriteNewLine = true;
			return (int)((long)(pSrc - ptr));
			IL_020F:
			this.bufPos = (int)((long)(ptr3 - ptr2));
			this.textPos = this.bufPos;
			this.contentPos = 0;
			array = null;
			return -1;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00018028 File Offset: 0x00016228
		[SecuritySafeCritical]
		protected unsafe int WriteElementTextBlockNoFlush(char[] chars, int index, int count, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			if (count == 0)
			{
				this.contentPos = 0;
				return -1;
			}
			fixed (char* ptr = &chars[index])
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + count;
				return this.WriteElementTextBlockNoFlush(ptr2, ptr3, out needWriteNewLine);
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00018064 File Offset: 0x00016264
		[SecuritySafeCritical]
		protected unsafe int WriteElementTextBlockNoFlush(string text, int index, int count, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			if (count == 0)
			{
				this.contentPos = 0;
				return -1;
			}
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = ptr + index;
			char* ptr3 = ptr2 + count;
			return this.WriteElementTextBlockNoFlush(ptr2, ptr3, out needWriteNewLine);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000180AC File Offset: 0x000162AC
		protected async Task WriteElementTextBlockAsync(char[] chars, int index, int count)
		{
			int writeLen = 0;
			int curIndex = index;
			int leftCount = count;
			bool needWriteNewLine = false;
			do
			{
				writeLen = this.WriteElementTextBlockNoFlush(chars, curIndex, leftCount, out needWriteNewLine);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (needWriteNewLine)
				{
					await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
					curIndex++;
					leftCount--;
				}
				else if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0 || needWriteNewLine);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00018108 File Offset: 0x00016308
		protected Task WriteElementTextBlockAsync(string text)
		{
			int num = 0;
			int num2 = text.Length;
			bool flag = false;
			int num3 = this.WriteElementTextBlockNoFlush(text, num, num2, out flag);
			num += num3;
			num2 -= num3;
			if (flag)
			{
				return this._WriteElementTextBlockAsync(true, text, num, num2);
			}
			if (num3 >= 0)
			{
				return this._WriteElementTextBlockAsync(false, text, num, num2);
			}
			return AsyncHelper.DoneTask;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00018158 File Offset: 0x00016358
		private async Task _WriteElementTextBlockAsync(bool newLine, string text, int curIndex, int leftCount)
		{
			int writeLen = 0;
			bool needWriteNewLine = false;
			if (newLine)
			{
				await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
				curIndex++;
				leftCount--;
			}
			else
			{
				await this.FlushBufferAsync().ConfigureAwait(false);
			}
			do
			{
				writeLen = this.WriteElementTextBlockNoFlush(text, curIndex, leftCount, out needWriteNewLine);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (needWriteNewLine)
				{
					await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
					curIndex++;
					leftCount--;
				}
				else if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0 || needWriteNewLine);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000181BC File Offset: 0x000163BC
		[SecuritySafeCritical]
		protected unsafe int RawTextNoFlush(char* pSrcBegin, char* pSrcEnd)
		{
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			char* ptr2 = ptr + this.bufPos;
			char* ptr3 = pSrcBegin;
			int num = 0;
			for (;;)
			{
				char* ptr4 = ptr2 + (long)(pSrcEnd - ptr3) * 2L / 2L;
				if (ptr4 != ptr + this.bufLen)
				{
					ptr4 = ptr + this.bufLen;
				}
				while (ptr2 < ptr4 && (num = (int)(*ptr3)) < 55296)
				{
					ptr3++;
					*ptr2 = (char)num;
					ptr2++;
				}
				if (ptr3 >= pSrcEnd)
				{
					goto IL_00F9;
				}
				if (ptr2 >= ptr4)
				{
					break;
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr2 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr3, pSrcEnd, ptr2);
					ptr3 += 2;
				}
				else if (num <= 127 || num >= 65534)
				{
					ptr2 = this.InvalidXmlChar(num, ptr2, false);
					ptr3++;
				}
				else
				{
					*ptr2 = (char)num;
					ptr2++;
					ptr3++;
				}
			}
			this.bufPos = (int)((long)(ptr2 - ptr));
			return (int)((long)(ptr3 - pSrcBegin));
			IL_00F9:
			this.bufPos = (int)((long)(ptr2 - ptr));
			array = null;
			return -1;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000182D4 File Offset: 0x000164D4
		[SecuritySafeCritical]
		protected unsafe int RawTextNoFlush(string text, int index, int count)
		{
			if (count == 0)
			{
				return -1;
			}
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = ptr + index;
			char* ptr3 = ptr2 + count;
			return this.RawTextNoFlush(ptr2, ptr3);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001830C File Offset: 0x0001650C
		protected Task RawTextAsync(string text)
		{
			int num = 0;
			int num2 = text.Length;
			int num3 = this.RawTextNoFlush(text, num, num2);
			num += num3;
			num2 -= num3;
			if (num3 >= 0)
			{
				return this._RawTextAsync(text, num, num2);
			}
			return AsyncHelper.DoneTask;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001834C File Offset: 0x0001654C
		private async Task _RawTextAsync(string text, int curIndex, int leftCount)
		{
			await this.FlushBufferAsync().ConfigureAwait(false);
			int writeLen = 0;
			do
			{
				writeLen = this.RawTextNoFlush(text, curIndex, leftCount);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000183A8 File Offset: 0x000165A8
		[SecuritySafeCritical]
		protected unsafe int WriteRawWithCharCheckingNoFlush(char* pSrcBegin, char* pSrcEnd, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			char[] array;
			char* ptr;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			char* ptr2 = pSrcBegin;
			char* ptr3 = ptr + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr4 = ptr3 + (long)(pSrcEnd - ptr2) * 2L / 2L;
				if (ptr4 != ptr + this.bufLen)
				{
					ptr4 = ptr + this.bufLen;
				}
				while (ptr3 < ptr4 && (this.xmlCharType.charProperties[num = (int)(*ptr2)] & 64) != 0)
				{
					*ptr3 = (char)num;
					ptr3++;
					ptr2++;
				}
				if (ptr2 >= pSrcEnd)
				{
					goto IL_01CC;
				}
				if (ptr3 >= ptr4)
				{
					break;
				}
				if (num <= 38)
				{
					switch (num)
					{
					case 9:
						goto IL_00EB;
					case 10:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_12;
						}
						*ptr3 = (char)num;
						ptr3++;
						goto IL_01C3;
					case 11:
					case 12:
						break;
					case 13:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_10;
						}
						*ptr3 = (char)num;
						ptr3++;
						goto IL_01C3;
					default:
						if (num == 38)
						{
							goto IL_00EB;
						}
						break;
					}
				}
				else if (num == 60 || num == 93)
				{
					goto IL_00EB;
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr3 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr2, pSrcEnd, ptr3);
					ptr2 += 2;
					continue;
				}
				if (num <= 127 || num >= 65534)
				{
					ptr3 = this.InvalidXmlChar(num, ptr3, false);
					ptr2++;
					continue;
				}
				*ptr3 = (char)num;
				ptr3++;
				ptr2++;
				continue;
				IL_01C3:
				ptr2++;
				continue;
				IL_00EB:
				*ptr3 = (char)num;
				ptr3++;
				goto IL_01C3;
			}
			this.bufPos = (int)((long)(ptr3 - ptr));
			return (int)((long)(ptr2 - pSrcBegin));
			Block_10:
			if (ptr2[1] == '\n')
			{
				ptr2++;
			}
			this.bufPos = (int)((long)(ptr3 - ptr));
			needWriteNewLine = true;
			return (int)((long)(ptr2 - pSrcBegin));
			Block_12:
			this.bufPos = (int)((long)(ptr3 - ptr));
			needWriteNewLine = true;
			return (int)((long)(ptr2 - pSrcBegin));
			IL_01CC:
			this.bufPos = (int)((long)(ptr3 - ptr));
			array = null;
			return -1;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00018594 File Offset: 0x00016794
		[SecuritySafeCritical]
		protected unsafe int WriteRawWithCharCheckingNoFlush(char[] chars, int index, int count, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			if (count == 0)
			{
				return -1;
			}
			fixed (char* ptr = &chars[index])
			{
				char* ptr2 = ptr;
				char* ptr3 = ptr2 + count;
				return this.WriteRawWithCharCheckingNoFlush(ptr2, ptr3, out needWriteNewLine);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000185C8 File Offset: 0x000167C8
		[SecuritySafeCritical]
		protected unsafe int WriteRawWithCharCheckingNoFlush(string text, int index, int count, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			if (count == 0)
			{
				return -1;
			}
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = ptr + index;
			char* ptr3 = ptr2 + count;
			return this.WriteRawWithCharCheckingNoFlush(ptr2, ptr3, out needWriteNewLine);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00018608 File Offset: 0x00016808
		protected async Task WriteRawWithCharCheckingAsync(char[] chars, int index, int count)
		{
			int writeLen = 0;
			int curIndex = index;
			int leftCount = count;
			bool needWriteNewLine = false;
			do
			{
				writeLen = this.WriteRawWithCharCheckingNoFlush(chars, curIndex, leftCount, out needWriteNewLine);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (needWriteNewLine)
				{
					await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
					curIndex++;
					leftCount--;
				}
				else if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0 || needWriteNewLine);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00018664 File Offset: 0x00016864
		protected async Task WriteRawWithCharCheckingAsync(string text)
		{
			int writeLen = 0;
			int curIndex = 0;
			int leftCount = text.Length;
			bool needWriteNewLine = false;
			do
			{
				writeLen = this.WriteRawWithCharCheckingNoFlush(text, curIndex, leftCount, out needWriteNewLine);
				curIndex += writeLen;
				leftCount -= writeLen;
				if (needWriteNewLine)
				{
					await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
					curIndex++;
					leftCount--;
				}
				else if (writeLen >= 0)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			while (writeLen >= 0 || needWriteNewLine);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000186B0 File Offset: 0x000168B0
		[SecuritySafeCritical]
		protected unsafe int WriteCommentOrPiNoFlush(string text, int index, int count, int stopChar, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			if (count == 0)
			{
				return -1;
			}
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = ptr + index;
			char[] array;
			char* ptr3;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr3 = null;
			}
			else
			{
				ptr3 = &array[0];
			}
			char* ptr4 = ptr2;
			char* ptr5 = ptr4;
			char* ptr6 = ptr2 + count;
			char* ptr7 = ptr3 + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr8 = ptr7 + (long)(ptr6 - ptr4) * 2L / 2L;
				if (ptr8 != ptr3 + this.bufLen)
				{
					ptr8 = ptr3 + this.bufLen;
				}
				while (ptr7 < ptr8 && (this.xmlCharType.charProperties[num = (int)(*ptr4)] & 64) != 0 && num != stopChar)
				{
					*ptr7 = (char)num;
					ptr7++;
					ptr4++;
				}
				if (ptr4 >= ptr6)
				{
					goto IL_02AB;
				}
				if (ptr7 >= ptr8)
				{
					break;
				}
				if (num <= 45)
				{
					switch (num)
					{
					case 9:
						goto IL_0230;
					case 10:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_23;
						}
						*ptr7 = (char)num;
						ptr7++;
						goto IL_02A0;
					case 11:
					case 12:
						break;
					case 13:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_21;
						}
						*ptr7 = (char)num;
						ptr7++;
						goto IL_02A0;
					default:
						if (num == 38)
						{
							goto IL_0230;
						}
						if (num == 45)
						{
							*ptr7 = '-';
							ptr7++;
							if (num == stopChar && (ptr4 + 1 == ptr6 || ptr4[1] == '-'))
							{
								*ptr7 = ' ';
								ptr7++;
								goto IL_02A0;
							}
							goto IL_02A0;
						}
						break;
					}
				}
				else
				{
					if (num == 60)
					{
						goto IL_0230;
					}
					if (num != 63)
					{
						if (num == 93)
						{
							*ptr7 = ']';
							ptr7++;
							goto IL_02A0;
						}
					}
					else
					{
						*ptr7 = '?';
						ptr7++;
						if (num == stopChar && ptr4 + 1 < ptr6 && ptr4[1] == '>')
						{
							*ptr7 = ' ';
							ptr7++;
							goto IL_02A0;
						}
						goto IL_02A0;
					}
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr7 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr4, ptr6, ptr7);
					ptr4 += 2;
					continue;
				}
				if (num <= 127 || num >= 65534)
				{
					ptr7 = this.InvalidXmlChar(num, ptr7, false);
					ptr4++;
					continue;
				}
				*ptr7 = (char)num;
				ptr7++;
				ptr4++;
				continue;
				IL_02A0:
				ptr4++;
				continue;
				IL_0230:
				*ptr7 = (char)num;
				ptr7++;
				goto IL_02A0;
			}
			this.bufPos = (int)((long)(ptr7 - ptr3));
			return (int)((long)(ptr4 - ptr5));
			Block_21:
			if (ptr4[1] == '\n')
			{
				ptr4++;
			}
			this.bufPos = (int)((long)(ptr7 - ptr3));
			needWriteNewLine = true;
			return (int)((long)(ptr4 - ptr5));
			Block_23:
			this.bufPos = (int)((long)(ptr7 - ptr3));
			needWriteNewLine = true;
			return (int)((long)(ptr4 - ptr5));
			IL_02AB:
			this.bufPos = (int)((long)(ptr7 - ptr3));
			array = null;
			return -1;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001897C File Offset: 0x00016B7C
		protected async Task WriteCommentOrPiAsync(string text, int stopChar)
		{
			if (text.Length == 0)
			{
				if (this.bufPos >= this.bufLen)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			else
			{
				int writeLen = 0;
				int curIndex = 0;
				int leftCount = text.Length;
				bool needWriteNewLine = false;
				do
				{
					writeLen = this.WriteCommentOrPiNoFlush(text, curIndex, leftCount, stopChar, out needWriteNewLine);
					curIndex += writeLen;
					leftCount -= writeLen;
					if (needWriteNewLine)
					{
						await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
						curIndex++;
						leftCount--;
					}
					else if (writeLen >= 0)
					{
						await this.FlushBufferAsync().ConfigureAwait(false);
					}
				}
				while (writeLen >= 0 || needWriteNewLine);
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000189D0 File Offset: 0x00016BD0
		[SecuritySafeCritical]
		protected unsafe int WriteCDataSectionNoFlush(string text, int index, int count, out bool needWriteNewLine)
		{
			needWriteNewLine = false;
			if (count == 0)
			{
				return -1;
			}
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = ptr + index;
			char[] array;
			char* ptr3;
			if ((array = this.bufChars) == null || array.Length == 0)
			{
				ptr3 = null;
			}
			else
			{
				ptr3 = &array[0];
			}
			char* ptr4 = ptr2;
			char* ptr5 = ptr2 + count;
			char* ptr6 = ptr4;
			char* ptr7 = ptr3 + this.bufPos;
			int num = 0;
			for (;;)
			{
				char* ptr8 = ptr7 + (long)(ptr5 - ptr4) * 2L / 2L;
				if (ptr8 != ptr3 + this.bufLen)
				{
					ptr8 = ptr3 + this.bufLen;
				}
				while (ptr7 < ptr8 && (this.xmlCharType.charProperties[num = (int)(*ptr4)] & 128) != 0 && num != 93)
				{
					*ptr7 = (char)num;
					ptr7++;
					ptr4++;
				}
				if (ptr4 >= ptr5)
				{
					goto IL_0292;
				}
				if (ptr7 >= ptr8)
				{
					break;
				}
				if (num <= 39)
				{
					switch (num)
					{
					case 9:
						goto IL_0217;
					case 10:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_21;
						}
						*ptr7 = (char)num;
						ptr7++;
						goto IL_0287;
					case 11:
					case 12:
						break;
					case 13:
						if (this.newLineHandling == NewLineHandling.Replace)
						{
							goto Block_19;
						}
						*ptr7 = (char)num;
						ptr7++;
						goto IL_0287;
					default:
						if (num == 34 || num - 38 <= 1)
						{
							goto IL_0217;
						}
						break;
					}
				}
				else
				{
					if (num == 60)
					{
						goto IL_0217;
					}
					if (num == 62)
					{
						if (this.hadDoubleBracket && ptr7[-1] == ']')
						{
							ptr7 = XmlEncodedRawTextWriter.RawEndCData(ptr7);
							ptr7 = XmlEncodedRawTextWriter.RawStartCData(ptr7);
						}
						*ptr7 = '>';
						ptr7++;
						goto IL_0287;
					}
					if (num == 93)
					{
						if (ptr7[-1] == ']')
						{
							this.hadDoubleBracket = true;
						}
						else
						{
							this.hadDoubleBracket = false;
						}
						*ptr7 = ']';
						ptr7++;
						goto IL_0287;
					}
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr7 = XmlEncodedRawTextWriter.EncodeSurrogate(ptr4, ptr5, ptr7);
					ptr4 += 2;
					continue;
				}
				if (num <= 127 || num >= 65534)
				{
					ptr7 = this.InvalidXmlChar(num, ptr7, false);
					ptr4++;
					continue;
				}
				*ptr7 = (char)num;
				ptr7++;
				ptr4++;
				continue;
				IL_0287:
				ptr4++;
				continue;
				IL_0217:
				*ptr7 = (char)num;
				ptr7++;
				goto IL_0287;
			}
			this.bufPos = (int)((long)(ptr7 - ptr3));
			return (int)((long)(ptr4 - ptr6));
			Block_19:
			if (ptr4[1] == '\n')
			{
				ptr4++;
			}
			this.bufPos = (int)((long)(ptr7 - ptr3));
			needWriteNewLine = true;
			return (int)((long)(ptr4 - ptr6));
			Block_21:
			this.bufPos = (int)((long)(ptr7 - ptr3));
			needWriteNewLine = true;
			return (int)((long)(ptr4 - ptr6));
			IL_0292:
			this.bufPos = (int)((long)(ptr7 - ptr3));
			array = null;
			return -1;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00018C80 File Offset: 0x00016E80
		protected async Task WriteCDataSectionAsync(string text)
		{
			if (text.Length == 0)
			{
				if (this.bufPos >= this.bufLen)
				{
					await this.FlushBufferAsync().ConfigureAwait(false);
				}
			}
			else
			{
				int writeLen = 0;
				int curIndex = 0;
				int leftCount = text.Length;
				bool needWriteNewLine = false;
				do
				{
					writeLen = this.WriteCDataSectionNoFlush(text, curIndex, leftCount, out needWriteNewLine);
					curIndex += writeLen;
					leftCount -= writeLen;
					if (needWriteNewLine)
					{
						await this.RawTextAsync(this.newLineChars).ConfigureAwait(false);
						curIndex++;
						leftCount--;
					}
					else if (writeLen >= 0)
					{
						await this.FlushBufferAsync().ConfigureAwait(false);
					}
				}
				while (writeLen >= 0 || needWriteNewLine);
			}
		}

		// Token: 0x040006FC RID: 1788
		private readonly bool useAsync;

		// Token: 0x040006FD RID: 1789
		protected byte[] bufBytes;

		// Token: 0x040006FE RID: 1790
		protected Stream stream;

		// Token: 0x040006FF RID: 1791
		protected Encoding encoding;

		// Token: 0x04000700 RID: 1792
		protected XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x04000701 RID: 1793
		protected int bufPos = 1;

		// Token: 0x04000702 RID: 1794
		protected int textPos = 1;

		// Token: 0x04000703 RID: 1795
		protected int contentPos;

		// Token: 0x04000704 RID: 1796
		protected int cdataPos;

		// Token: 0x04000705 RID: 1797
		protected int attrEndPos;

		// Token: 0x04000706 RID: 1798
		protected int bufLen = 6144;

		// Token: 0x04000707 RID: 1799
		protected bool writeToNull;

		// Token: 0x04000708 RID: 1800
		protected bool hadDoubleBracket;

		// Token: 0x04000709 RID: 1801
		protected bool inAttributeValue;

		// Token: 0x0400070A RID: 1802
		protected int bufBytesUsed;

		// Token: 0x0400070B RID: 1803
		protected char[] bufChars;

		// Token: 0x0400070C RID: 1804
		protected Encoder encoder;

		// Token: 0x0400070D RID: 1805
		protected TextWriter writer;

		// Token: 0x0400070E RID: 1806
		protected bool trackTextContent;

		// Token: 0x0400070F RID: 1807
		protected bool inTextContent;

		// Token: 0x04000710 RID: 1808
		private int lastMarkPos;

		// Token: 0x04000711 RID: 1809
		private int[] textContentMarks;

		// Token: 0x04000712 RID: 1810
		private CharEntityEncoderFallback charEntityFallback;

		// Token: 0x04000713 RID: 1811
		protected NewLineHandling newLineHandling;

		// Token: 0x04000714 RID: 1812
		protected bool closeOutput;

		// Token: 0x04000715 RID: 1813
		protected bool omitXmlDeclaration;

		// Token: 0x04000716 RID: 1814
		protected string newLineChars;

		// Token: 0x04000717 RID: 1815
		protected bool checkCharacters;

		// Token: 0x04000718 RID: 1816
		protected XmlStandalone standalone;

		// Token: 0x04000719 RID: 1817
		protected XmlOutputMethod outputMethod;

		// Token: 0x0400071A RID: 1818
		protected bool autoXmlDeclaration;

		// Token: 0x0400071B RID: 1819
		protected bool mergeCDataSections;

		// Token: 0x0400071C RID: 1820
		private const int BUFSIZE = 6144;

		// Token: 0x0400071D RID: 1821
		private const int ASYNCBUFSIZE = 65536;

		// Token: 0x0400071E RID: 1822
		private const int OVERFLOW = 32;

		// Token: 0x0400071F RID: 1823
		private const int INIT_MARKS_COUNT = 64;
	}
}
