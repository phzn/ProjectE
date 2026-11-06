using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F2 RID: 242
	[NullableContext(2)]
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CB7 RID: 3255
		string Version { get; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CB8 RID: 3256
		// (set) Token: 0x06000CB9 RID: 3257
		string Encoding { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CBA RID: 3258
		// (set) Token: 0x06000CBB RID: 3259
		string Standalone { get; set; }
	}
}
