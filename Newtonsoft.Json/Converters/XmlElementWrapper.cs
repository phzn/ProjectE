using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x06000C8C RID: 3212 RVA: 0x00032185 File Offset: 0x00030385
		public XmlElementWrapper(XmlElement element)
			: base(element)
		{
			this._element = element;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00032198 File Offset: 0x00030398
		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000321C3 File Offset: 0x000303C3
		[return: Nullable(2)]
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x000321D1 File Offset: 0x000303D1
		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		// Token: 0x04000400 RID: 1024
		private readonly XmlElement _element;
	}
}
