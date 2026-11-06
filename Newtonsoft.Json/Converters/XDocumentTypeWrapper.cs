using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(2)]
	[Nullable(0)]
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x0003259D File Offset: 0x0003079D
		[NullableContext(1)]
		public XDocumentTypeWrapper(XDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x000325AD File Offset: 0x000307AD
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000325BA File Offset: 0x000307BA
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000325C7 File Offset: 0x000307C7
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x000325D4 File Offset: 0x000307D4
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000325E1 File Offset: 0x000307E1
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x04000407 RID: 1031
		[Nullable(1)]
		private readonly XDocumentType _documentType;
	}
}
