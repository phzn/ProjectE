using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AD RID: 173
	[Flags]
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum JsonSchemaType
	{
		// Token: 0x0400035B RID: 859
		None = 0,
		// Token: 0x0400035C RID: 860
		String = 1,
		// Token: 0x0400035D RID: 861
		Float = 2,
		// Token: 0x0400035E RID: 862
		Integer = 4,
		// Token: 0x0400035F RID: 863
		Boolean = 8,
		// Token: 0x04000360 RID: 864
		Object = 16,
		// Token: 0x04000361 RID: 865
		Array = 32,
		// Token: 0x04000362 RID: 866
		Null = 64,
		// Token: 0x04000363 RID: 867
		Any = 127
	}
}
