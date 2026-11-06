using System;
using System.IO;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x0200008C RID: 140
	internal class XmlEncodedRawTextWriterIndent : XmlEncodedRawTextWriter
	{
		// Token: 0x060004ED RID: 1261 RVA: 0x0001C196 File Offset: 0x0001A396
		public XmlEncodedRawTextWriterIndent(TextWriter writer, XmlWriterSettings settings)
			: base(writer, settings)
		{
			this.Init(settings);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001C1A7 File Offset: 0x0001A3A7
		public XmlEncodedRawTextWriterIndent(Stream stream, XmlWriterSettings settings)
			: base(stream, settings)
		{
			this.Init(settings);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0001C1B8 File Offset: 0x0001A3B8
		public override XmlWriterSettings Settings
		{
			get
			{
				XmlWriterSettings settings = base.Settings;
				settings.ReadOnly = false;
				settings.Indent = true;
				settings.IndentChars = this.indentChars;
				settings.NewLineOnAttributes = this.newLineOnAttributes;
				settings.ReadOnly = true;
				return settings;
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001C1ED File Offset: 0x0001A3ED
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001C218 File Offset: 0x0001A418
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			this.indentLevel++;
			this.mixedContentStack.PushBit(this.mixedContent);
			base.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001C269 File Offset: 0x0001A469
		internal override void StartElementContent()
		{
			if (this.indentLevel == 1 && this.conformanceLevel == ConformanceLevel.Document)
			{
				this.mixedContent = false;
			}
			else
			{
				this.mixedContent = this.mixedContentStack.PeekBit();
			}
			base.StartElementContent();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001C29D File Offset: 0x0001A49D
		internal override void OnRootElement(ConformanceLevel currentConformanceLevel)
		{
			this.conformanceLevel = currentConformanceLevel;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001C2A8 File Offset: 0x0001A4A8
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.indentLevel--;
			if (!this.mixedContent && this.contentPos != this.bufPos && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			this.mixedContent = this.mixedContentStack.PopBit();
			base.WriteEndElement(prefix, localName, ns);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001C308 File Offset: 0x0001A508
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.indentLevel--;
			if (!this.mixedContent && this.contentPos != this.bufPos && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			this.mixedContent = this.mixedContentStack.PopBit();
			base.WriteFullEndElement(prefix, localName, ns);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001C367 File Offset: 0x0001A567
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.newLineOnAttributes)
			{
				this.WriteIndent();
			}
			base.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001C380 File Offset: 0x0001A580
		public override void WriteCData(string text)
		{
			this.mixedContent = true;
			base.WriteCData(text);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001C390 File Offset: 0x0001A590
		public override void WriteComment(string text)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteComment(text);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001C3B5 File Offset: 0x0001A5B5
		public override void WriteProcessingInstruction(string target, string text)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteProcessingInstruction(target, text);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001C3DB File Offset: 0x0001A5DB
		public override void WriteEntityRef(string name)
		{
			this.mixedContent = true;
			base.WriteEntityRef(name);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001C3EB File Offset: 0x0001A5EB
		public override void WriteCharEntity(char ch)
		{
			this.mixedContent = true;
			base.WriteCharEntity(ch);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001C3FB File Offset: 0x0001A5FB
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.mixedContent = true;
			base.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001C40C File Offset: 0x0001A60C
		public override void WriteWhitespace(string ws)
		{
			this.mixedContent = true;
			base.WriteWhitespace(ws);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001C41C File Offset: 0x0001A61C
		public override void WriteString(string text)
		{
			this.mixedContent = true;
			base.WriteString(text);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001C42C File Offset: 0x0001A62C
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.mixedContent = true;
			base.WriteChars(buffer, index, count);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001C43E File Offset: 0x0001A63E
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.mixedContent = true;
			base.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001C450 File Offset: 0x0001A650
		public override void WriteRaw(string data)
		{
			this.mixedContent = true;
			base.WriteRaw(data);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001C460 File Offset: 0x0001A660
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.mixedContent = true;
			base.WriteBase64(buffer, index, count);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001C474 File Offset: 0x0001A674
		private void Init(XmlWriterSettings settings)
		{
			this.indentLevel = 0;
			this.indentChars = settings.IndentChars;
			this.newLineOnAttributes = settings.NewLineOnAttributes;
			this.mixedContentStack = new BitStack();
			if (this.checkCharacters)
			{
				if (this.newLineOnAttributes)
				{
					base.ValidateContentChars(this.indentChars, "IndentChars", true);
					base.ValidateContentChars(this.newLineChars, "NewLineChars", true);
					return;
				}
				base.ValidateContentChars(this.indentChars, "IndentChars", false);
				if (this.newLineHandling != NewLineHandling.Replace)
				{
					base.ValidateContentChars(this.newLineChars, "NewLineChars", false);
				}
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001C50C File Offset: 0x0001A70C
		private void WriteIndent()
		{
			base.RawText(this.newLineChars);
			for (int i = this.indentLevel; i > 0; i--)
			{
				base.RawText(this.indentChars);
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001C544 File Offset: 0x0001A744
		public override async Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteDocTypeAsync(name, pubid, sysid, subset).ConfigureAwait(false);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001C5A8 File Offset: 0x0001A7A8
		public override async Task WriteStartElementAsync(string prefix, string localName, string ns)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			this.indentLevel++;
			this.mixedContentStack.PushBit(this.mixedContent);
			await base.WriteStartElementAsync(prefix, localName, ns).ConfigureAwait(false);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001C604 File Offset: 0x0001A804
		internal override async Task WriteEndElementAsync(string prefix, string localName, string ns)
		{
			base.CheckAsyncCall();
			this.indentLevel--;
			if (!this.mixedContent && this.contentPos != this.bufPos && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			this.mixedContent = this.mixedContentStack.PopBit();
			await base.WriteEndElementAsync(prefix, localName, ns).ConfigureAwait(false);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001C660 File Offset: 0x0001A860
		internal override async Task WriteFullEndElementAsync(string prefix, string localName, string ns)
		{
			base.CheckAsyncCall();
			this.indentLevel--;
			if (!this.mixedContent && this.contentPos != this.bufPos && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			this.mixedContent = this.mixedContentStack.PopBit();
			await base.WriteFullEndElementAsync(prefix, localName, ns).ConfigureAwait(false);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001C6BC File Offset: 0x0001A8BC
		protected internal override async Task WriteStartAttributeAsync(string prefix, string localName, string ns)
		{
			base.CheckAsyncCall();
			if (this.newLineOnAttributes)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteStartAttributeAsync(prefix, localName, ns).ConfigureAwait(false);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001C717 File Offset: 0x0001A917
		public override Task WriteCDataAsync(string text)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteCDataAsync(text);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001C730 File Offset: 0x0001A930
		public override async Task WriteCommentAsync(string text)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteCommentAsync(text).ConfigureAwait(false);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001C77C File Offset: 0x0001A97C
		public override async Task WriteProcessingInstructionAsync(string target, string text)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteProcessingInstructionAsync(target, text).ConfigureAwait(false);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001C7CF File Offset: 0x0001A9CF
		public override Task WriteEntityRefAsync(string name)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteEntityRefAsync(name);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001C7E5 File Offset: 0x0001A9E5
		public override Task WriteCharEntityAsync(char ch)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteCharEntityAsync(ch);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001C7FB File Offset: 0x0001A9FB
		public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteSurrogateCharEntityAsync(lowChar, highChar);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001C812 File Offset: 0x0001AA12
		public override Task WriteWhitespaceAsync(string ws)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteWhitespaceAsync(ws);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001C828 File Offset: 0x0001AA28
		public override Task WriteStringAsync(string text)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteStringAsync(text);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001C83E File Offset: 0x0001AA3E
		public override Task WriteCharsAsync(char[] buffer, int index, int count)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteCharsAsync(buffer, index, count);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001C856 File Offset: 0x0001AA56
		public override Task WriteRawAsync(char[] buffer, int index, int count)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteRawAsync(buffer, index, count);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001C86E File Offset: 0x0001AA6E
		public override Task WriteRawAsync(string data)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteRawAsync(data);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001C884 File Offset: 0x0001AA84
		public override Task WriteBase64Async(byte[] buffer, int index, int count)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteBase64Async(buffer, index, count);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001C89C File Offset: 0x0001AA9C
		private async Task WriteIndentAsync()
		{
			base.CheckAsyncCall();
			await base.RawTextAsync(this.newLineChars).ConfigureAwait(false);
			for (int i = this.indentLevel; i > 0; i--)
			{
				await base.RawTextAsync(this.indentChars).ConfigureAwait(false);
			}
		}

		// Token: 0x040007C9 RID: 1993
		protected int indentLevel;

		// Token: 0x040007CA RID: 1994
		protected bool newLineOnAttributes;

		// Token: 0x040007CB RID: 1995
		protected string indentChars;

		// Token: 0x040007CC RID: 1996
		protected bool mixedContent;

		// Token: 0x040007CD RID: 1997
		private BitStack mixedContentStack;

		// Token: 0x040007CE RID: 1998
		protected ConformanceLevel conformanceLevel;
	}
}
