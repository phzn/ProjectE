using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FB RID: 251
	[NullableContext(2)]
	[Nullable(0)]
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0003283B File Offset: 0x00030A3B
		[Nullable(1)]
		private XProcessingInstruction ProcessingInstruction
		{
			[NullableContext(1)]
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00032848 File Offset: 0x00030A48
		[NullableContext(1)]
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
			: base(processingInstruction)
		{
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00032851 File Offset: 0x00030A51
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0003285E File Offset: 0x00030A5E
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x0003286B File Offset: 0x00030A6B
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value ?? string.Empty;
			}
		}
	}
}
