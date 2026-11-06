using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FC RID: 252
	[NullableContext(1)]
	[Nullable(0)]
	internal class XContainerWrapper : XObjectWrapper
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00032882 File Offset: 0x00030A82
		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0003288F File Offset: 0x00030A8F
		public XContainerWrapper(XContainer container)
			: base(container)
		{
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00032898 File Offset: 0x00030A98
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					if (!this.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>();
						foreach (XNode xnode in this.Container.Nodes())
						{
							this._childNodes.Add(XContainerWrapper.WrapNode(xnode));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00032924 File Offset: 0x00030B24
		protected virtual bool HasChildNodes
		{
			get
			{
				return this.Container.LastNode != null;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00032934 File Offset: 0x00030B34
		[Nullable(2)]
		public override IXmlNode ParentNode
		{
			[NullableContext(2)]
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00032958 File Offset: 0x00030B58
		internal static IXmlNode WrapNode(XObject node)
		{
			XDocument xdocument = node as XDocument;
			if (xdocument != null)
			{
				return new XDocumentWrapper(xdocument);
			}
			XElement xelement = node as XElement;
			if (xelement != null)
			{
				return new XElementWrapper(xelement);
			}
			XContainer xcontainer = node as XContainer;
			if (xcontainer != null)
			{
				return new XContainerWrapper(xcontainer);
			}
			XProcessingInstruction xprocessingInstruction = node as XProcessingInstruction;
			if (xprocessingInstruction != null)
			{
				return new XProcessingInstructionWrapper(xprocessingInstruction);
			}
			XText xtext = node as XText;
			if (xtext != null)
			{
				return new XTextWrapper(xtext);
			}
			XComment xcomment = node as XComment;
			if (xcomment != null)
			{
				return new XCommentWrapper(xcomment);
			}
			XAttribute xattribute = node as XAttribute;
			if (xattribute != null)
			{
				return new XAttributeWrapper(xattribute);
			}
			XDocumentType xdocumentType = node as XDocumentType;
			if (xdocumentType != null)
			{
				return new XDocumentTypeWrapper(xdocumentType);
			}
			return new XObjectWrapper(node);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000329FF File Offset: 0x00030BFF
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			this._childNodes = null;
			return newChild;
		}

		// Token: 0x04000408 RID: 1032
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _childNodes;
	}
}
