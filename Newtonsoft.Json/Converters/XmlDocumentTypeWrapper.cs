using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EF RID: 239
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x00032231 File Offset: 0x00030431
		[NullableContext(1)]
		public XmlDocumentTypeWrapper(XmlDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x00032241 File Offset: 0x00030441
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0003224E File Offset: 0x0003044E
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0003225B File Offset: 0x0003045B
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00032268 File Offset: 0x00030468
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00032275 File Offset: 0x00030475
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x04000402 RID: 1026
		[Nullable(1)]
		private readonly XmlDocumentType _documentType;
	}
}
