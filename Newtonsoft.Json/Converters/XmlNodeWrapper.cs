using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F0 RID: 240
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlNodeWrapper : IXmlNode
	{
		// Token: 0x06000C9C RID: 3228 RVA: 0x0003227C File Offset: 0x0003047C
		[NullableContext(1)]
		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0003228B File Offset: 0x0003048B
		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00032293 File Offset: 0x00030493
		public XmlNodeType NodeType
		{
			get
			{
				return this._node.NodeType;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x000322A0 File Offset: 0x000304A0
		public virtual string LocalName
		{
			get
			{
				return this._node.LocalName;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x000322B0 File Offset: 0x000304B0
		[Nullable(1)]
		public List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				if (this._childNodes == null)
				{
					if (!this._node.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
						foreach (object obj in this._node.ChildNodes)
						{
							XmlNode xmlNode = (XmlNode)obj;
							this._childNodes.Add(XmlNodeWrapper.WrapNode(xmlNode));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00032360 File Offset: 0x00030560
		protected virtual bool HasChildNodes
		{
			get
			{
				return this._node.HasChildNodes;
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00032370 File Offset: 0x00030570
		[NullableContext(1)]
		internal static IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType == XmlNodeType.DocumentType)
			{
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			}
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x000323C0 File Offset: 0x000305C0
		[Nullable(1)]
		public List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				if (this._attributes == null)
				{
					if (!this.HasAttributes)
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
						foreach (object obj in this._node.Attributes)
						{
							XmlAttribute xmlAttribute = (XmlAttribute)obj;
							this._attributes.Add(XmlNodeWrapper.WrapNode(xmlAttribute));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00032468 File Offset: 0x00030668
		private bool HasAttributes
		{
			get
			{
				XmlElement xmlElement = this._node as XmlElement;
				if (xmlElement != null)
				{
					return xmlElement.HasAttributes;
				}
				XmlAttributeCollection attributes = this._node.Attributes;
				return attributes != null && attributes.Count > 0;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000324A4 File Offset: 0x000306A4
		public IXmlNode ParentNode
		{
			get
			{
				XmlAttribute xmlAttribute = this._node as XmlAttribute;
				XmlNode xmlNode = ((xmlAttribute != null) ? xmlAttribute.OwnerElement : this._node.ParentNode);
				if (xmlNode == null)
				{
					return null;
				}
				return XmlNodeWrapper.WrapNode(xmlNode);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x000324DF File Offset: 0x000306DF
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x000324EC File Offset: 0x000306EC
		public string Value
		{
			get
			{
				return this._node.Value;
			}
			set
			{
				this._node.Value = value;
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000324FC File Offset: 0x000306FC
		[NullableContext(1)]
		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			this._childNodes = null;
			this._attributes = null;
			return newChild;
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00032531 File Offset: 0x00030731
		public string NamespaceUri
		{
			get
			{
				return this._node.NamespaceURI;
			}
		}

		// Token: 0x04000403 RID: 1027
		[Nullable(1)]
		private readonly XmlNode _node;

		// Token: 0x04000404 RID: 1028
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _childNodes;

		// Token: 0x04000405 RID: 1029
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _attributes;
	}
}
