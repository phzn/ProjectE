using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F6 RID: 246
	[NullableContext(2)]
	[Nullable(0)]
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0003253E File Offset: 0x0003073E
		[Nullable(1)]
		internal XDeclaration Declaration
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00032546 File Offset: 0x00030746
		[NullableContext(1)]
		public XDeclarationWrapper(XDeclaration declaration)
			: base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00032556 File Offset: 0x00030756
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0003255A File Offset: 0x0003075A
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00032567 File Offset: 0x00030767
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00032574 File Offset: 0x00030774
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00032582 File Offset: 0x00030782
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x0003258F File Offset: 0x0003078F
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
