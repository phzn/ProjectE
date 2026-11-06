using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000325E8 File Offset: 0x000307E8
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x000325F5 File Offset: 0x000307F5
		public XDocumentWrapper(XDocument document)
			: base(document)
		{
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00032600 File Offset: 0x00030800
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				List<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && (childNodes.Count == 0 || childNodes[0].NodeType != XmlNodeType.XmlDeclaration))
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00032651 File Offset: 0x00030851
		protected override bool HasChildNodes
		{
			get
			{
				return base.HasChildNodes || this.Document.Declaration != null;
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0003266B File Offset: 0x0003086B
		public IXmlNode CreateComment([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00032678 File Offset: 0x00030878
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00032685 File Offset: 0x00030885
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00032692 File Offset: 0x00030892
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0003269F File Offset: 0x0003089F
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000326AC File Offset: 0x000308AC
		public IXmlNode CreateXmlDeclaration(string version, [Nullable(2)] string encoding, [Nullable(2)] string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x000326BB File Offset: 0x000308BB
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType([Nullable(1)] string name, string publicId, string systemId, string internalSubset)
		{
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000326CC File Offset: 0x000308CC
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000326DA File Offset: 0x000308DA
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000326EC File Offset: 0x000308EC
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00032704 File Offset: 0x00030904
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00032717 File Offset: 0x00030917
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			return new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), value));
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x00032730 File Offset: 0x00030930
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00032754 File Offset: 0x00030954
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper = newChild as XDeclarationWrapper;
			if (xdeclarationWrapper != null)
			{
				this.Document.Declaration = xdeclarationWrapper.Declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
