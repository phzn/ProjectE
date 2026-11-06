using System;
using System.IO;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000136 RID: 310
	internal class XmlUtf8RawTextWriterIndent : XmlUtf8RawTextWriter
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x0004BA2E File Offset: 0x00049C2E
		public XmlUtf8RawTextWriterIndent(Stream stream, XmlWriterSettings settings)
			: base(stream, settings)
		{
			this.Init(settings);
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0004BA3F File Offset: 0x00049C3F
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

		// Token: 0x06000B11 RID: 2833 RVA: 0x0004BA74 File Offset: 0x00049C74
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0004BAA0 File Offset: 0x00049CA0
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

		// Token: 0x06000B13 RID: 2835 RVA: 0x0004BAF1 File Offset: 0x00049CF1
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

		// Token: 0x06000B14 RID: 2836 RVA: 0x0004BB25 File Offset: 0x00049D25
		internal override void OnRootElement(ConformanceLevel currentConformanceLevel)
		{
			this.conformanceLevel = currentConformanceLevel;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0004BB30 File Offset: 0x00049D30
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

		// Token: 0x06000B16 RID: 2838 RVA: 0x0004BB90 File Offset: 0x00049D90
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

		// Token: 0x06000B17 RID: 2839 RVA: 0x0004BBEF File Offset: 0x00049DEF
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.newLineOnAttributes)
			{
				this.WriteIndent();
			}
			base.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0004BC08 File Offset: 0x00049E08
		public override void WriteCData(string text)
		{
			this.mixedContent = true;
			base.WriteCData(text);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0004BC18 File Offset: 0x00049E18
		public override void WriteComment(string text)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteComment(text);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0004BC3D File Offset: 0x00049E3D
		public override void WriteProcessingInstruction(string target, string text)
		{
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteProcessingInstruction(target, text);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0004BC63 File Offset: 0x00049E63
		public override void WriteEntityRef(string name)
		{
			this.mixedContent = true;
			base.WriteEntityRef(name);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0004BC73 File Offset: 0x00049E73
		public override void WriteCharEntity(char ch)
		{
			this.mixedContent = true;
			base.WriteCharEntity(ch);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0004BC83 File Offset: 0x00049E83
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.mixedContent = true;
			base.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0004BC94 File Offset: 0x00049E94
		public override void WriteWhitespace(string ws)
		{
			this.mixedContent = true;
			base.WriteWhitespace(ws);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0004BCA4 File Offset: 0x00049EA4
		public override void WriteString(string text)
		{
			this.mixedContent = true;
			base.WriteString(text);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0004BCB4 File Offset: 0x00049EB4
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.mixedContent = true;
			base.WriteChars(buffer, index, count);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0004BCC6 File Offset: 0x00049EC6
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.mixedContent = true;
			base.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0004BCD8 File Offset: 0x00049ED8
		public override void WriteRaw(string data)
		{
			this.mixedContent = true;
			base.WriteRaw(data);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0004BCE8 File Offset: 0x00049EE8
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.mixedContent = true;
			base.WriteBase64(buffer, index, count);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0004BCFC File Offset: 0x00049EFC
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

		// Token: 0x06000B25 RID: 2853 RVA: 0x0004BD94 File Offset: 0x00049F94
		private void WriteIndent()
		{
			base.RawText(this.newLineChars);
			for (int i = this.indentLevel; i > 0; i--)
			{
				base.RawText(this.indentChars);
			}
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0004BDCC File Offset: 0x00049FCC
		public override async Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteDocTypeAsync(name, pubid, sysid, subset).ConfigureAwait(false);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0004BE30 File Offset: 0x0004A030
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

		// Token: 0x06000B28 RID: 2856 RVA: 0x0004BE8C File Offset: 0x0004A08C
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

		// Token: 0x06000B29 RID: 2857 RVA: 0x0004BEE8 File Offset: 0x0004A0E8
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

		// Token: 0x06000B2A RID: 2858 RVA: 0x0004BF44 File Offset: 0x0004A144
		protected internal override async Task WriteStartAttributeAsync(string prefix, string localName, string ns)
		{
			base.CheckAsyncCall();
			if (this.newLineOnAttributes)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteStartAttributeAsync(prefix, localName, ns).ConfigureAwait(false);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0004BF9F File Offset: 0x0004A19F
		public override Task WriteCDataAsync(string text)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteCDataAsync(text);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0004BFB8 File Offset: 0x0004A1B8
		public override async Task WriteCommentAsync(string text)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteCommentAsync(text).ConfigureAwait(false);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0004C004 File Offset: 0x0004A204
		public override async Task WriteProcessingInstructionAsync(string target, string text)
		{
			base.CheckAsyncCall();
			if (!this.mixedContent && this.textPos != this.bufPos)
			{
				await this.WriteIndentAsync().ConfigureAwait(false);
			}
			await base.WriteProcessingInstructionAsync(target, text).ConfigureAwait(false);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0004C057 File Offset: 0x0004A257
		public override Task WriteEntityRefAsync(string name)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteEntityRefAsync(name);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0004C06D File Offset: 0x0004A26D
		public override Task WriteCharEntityAsync(char ch)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteCharEntityAsync(ch);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0004C083 File Offset: 0x0004A283
		public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteSurrogateCharEntityAsync(lowChar, highChar);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0004C09A File Offset: 0x0004A29A
		public override Task WriteWhitespaceAsync(string ws)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteWhitespaceAsync(ws);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0004C0B0 File Offset: 0x0004A2B0
		public override Task WriteStringAsync(string text)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteStringAsync(text);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0004C0C6 File Offset: 0x0004A2C6
		public override Task WriteCharsAsync(char[] buffer, int index, int count)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteCharsAsync(buffer, index, count);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0004C0DE File Offset: 0x0004A2DE
		public override Task WriteRawAsync(char[] buffer, int index, int count)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteRawAsync(buffer, index, count);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0004C0F6 File Offset: 0x0004A2F6
		public override Task WriteRawAsync(string data)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteRawAsync(data);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0004C10C File Offset: 0x0004A30C
		public override Task WriteBase64Async(byte[] buffer, int index, int count)
		{
			base.CheckAsyncCall();
			this.mixedContent = true;
			return base.WriteBase64Async(buffer, index, count);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0004C124 File Offset: 0x0004A324
		private async Task WriteIndentAsync()
		{
			base.CheckAsyncCall();
			await base.RawTextAsync(this.newLineChars).ConfigureAwait(false);
			for (int i = this.indentLevel; i > 0; i--)
			{
				await base.RawTextAsync(this.indentChars).ConfigureAwait(false);
			}
		}

		// Token: 0x04000CE9 RID: 3305
		protected int indentLevel;

		// Token: 0x04000CEA RID: 3306
		protected bool newLineOnAttributes;

		// Token: 0x04000CEB RID: 3307
		protected string indentChars;

		// Token: 0x04000CEC RID: 3308
		protected bool mixedContent;

		// Token: 0x04000CED RID: 3309
		private BitStack mixedContentStack;

		// Token: 0x04000CEE RID: 3310
		protected ConformanceLevel conformanceLevel;
	}
}
