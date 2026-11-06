using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FD RID: 253
	[NullableContext(2)]
	[Nullable(0)]
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x06000D03 RID: 3331 RVA: 0x00032A1A File Offset: 0x00030C1A
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00032A29 File Offset: 0x00030C29
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00032A31 File Offset: 0x00030C31
		public virtual XmlNodeType NodeType
		{
			get
			{
				XObject xmlObject = this._xmlObject;
				if (xmlObject == null)
				{
					return XmlNodeType.None;
				}
				return xmlObject.NodeType;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00032A44 File Offset: 0x00030C44
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00032A47 File Offset: 0x00030C47
		[Nullable(1)]
		public virtual List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00032A4E File Offset: 0x00030C4E
		[Nullable(1)]
		public virtual List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00032A55 File Offset: 0x00030C55
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00032A58 File Offset: 0x00030C58
		// (set) Token: 0x06000D0B RID: 3339 RVA: 0x00032A5B File Offset: 0x00030C5B
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00032A62 File Offset: 0x00030C62
		[NullableContext(1)]
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00032A69 File Offset: 0x00030C69
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000409 RID: 1033
		private readonly XObject _xmlObject;
	}
}
