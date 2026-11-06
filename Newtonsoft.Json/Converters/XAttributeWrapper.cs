using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FE RID: 254
	[NullableContext(2)]
	[Nullable(0)]
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00032A6C File Offset: 0x00030C6C
		[Nullable(1)]
		private XAttribute Attribute
		{
			[NullableContext(1)]
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00032A79 File Offset: 0x00030C79
		[NullableContext(1)]
		public XAttributeWrapper(XAttribute attribute)
			: base(attribute)
		{
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00032A82 File Offset: 0x00030C82
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00032A8F File Offset: 0x00030C8F
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value ?? string.Empty;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00032AA6 File Offset: 0x00030CA6
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x00032AB8 File Offset: 0x00030CB8
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00032ACA File Offset: 0x00030CCA
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
