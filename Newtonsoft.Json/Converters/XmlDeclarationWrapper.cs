using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EE RID: 238
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x000321DE File Offset: 0x000303DE
		[NullableContext(1)]
		public XmlDeclarationWrapper(XmlDeclaration declaration)
			: base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x000321EE File Offset: 0x000303EE
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x000321FB File Offset: 0x000303FB
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00032208 File Offset: 0x00030408
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00032216 File Offset: 0x00030416
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x00032223 File Offset: 0x00030423
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x04000401 RID: 1025
		[Nullable(1)]
		private readonly XmlDeclaration _declaration;
	}
}
