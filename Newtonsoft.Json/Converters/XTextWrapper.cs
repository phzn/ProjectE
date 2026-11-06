using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(2)]
	[Nullable(0)]
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00032785 File Offset: 0x00030985
		[Nullable(1)]
		private XText Text
		{
			[NullableContext(1)]
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00032792 File Offset: 0x00030992
		[NullableContext(1)]
		public XTextWrapper(XText text)
			: base(text)
		{
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0003279B File Offset: 0x0003099B
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x000327A8 File Offset: 0x000309A8
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value ?? string.Empty;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x000327BF File Offset: 0x000309BF
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
