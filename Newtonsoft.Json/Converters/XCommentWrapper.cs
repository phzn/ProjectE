using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FA RID: 250
	[NullableContext(2)]
	[Nullable(0)]
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x000327E0 File Offset: 0x000309E0
		[Nullable(1)]
		private XComment Text
		{
			[NullableContext(1)]
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000327ED File Offset: 0x000309ED
		[NullableContext(1)]
		public XCommentWrapper(XComment text)
			: base(text)
		{
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x000327F6 File Offset: 0x000309F6
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x00032803 File Offset: 0x00030A03
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

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x0003281A File Offset: 0x00030A1A
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
