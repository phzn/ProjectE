using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F3 RID: 243
	[NullableContext(2)]
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000CBC RID: 3260
		[Nullable(1)]
		string Name
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000CBD RID: 3261
		string System { get; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000CBE RID: 3262
		string Public { get; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000CBF RID: 3263
		string InternalSubset { get; }
	}
}
