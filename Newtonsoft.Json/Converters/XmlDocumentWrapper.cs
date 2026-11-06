using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EC RID: 236
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x06000C7E RID: 3198 RVA: 0x0003205A File Offset: 0x0003025A
		public XmlDocumentWrapper(XmlDocument document)
			: base(document)
		{
			this._document = document;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0003206A File Offset: 0x0003026A
		public IXmlNode CreateComment([Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateComment(data));
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0003207D File Offset: 0x0003027D
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateTextNode(text));
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00032090 File Offset: 0x00030290
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateCDataSection(data));
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000320A3 File Offset: 0x000302A3
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateWhitespace(text));
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000320B6 File Offset: 0x000302B6
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateSignificantWhitespace(text));
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000320C9 File Offset: 0x000302C9
		public IXmlNode CreateXmlDeclaration(string version, [Nullable(2)] string encoding, [Nullable(2)] string standalone)
		{
			return new XmlDeclarationWrapper(this._document.CreateXmlDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x000320DE File Offset: 0x000302DE
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType([Nullable(1)] string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(this._document.CreateDocumentType(name, publicId, systemId, null));
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000320F4 File Offset: 0x000302F4
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XmlNodeWrapper(this._document.CreateProcessingInstruction(target, data));
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00032108 File Offset: 0x00030308
		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(this._document.CreateElement(elementName));
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0003211B File Offset: 0x0003031B
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0003212F File Offset: 0x0003032F
		public IXmlNode CreateAttribute(string name, [Nullable(2)] string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(name))
			{
				Value = value
			};
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00032149 File Offset: 0x00030349
		public IXmlNode CreateAttribute(string qualifiedName, [Nullable(2)] string namespaceUri, [Nullable(2)] string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(qualifiedName, namespaceUri))
			{
				Value = value
			};
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00032164 File Offset: 0x00030364
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this._document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(this._document.DocumentElement);
			}
		}

		// Token: 0x040003FF RID: 1023
		private readonly XmlDocument _document;
	}
}
