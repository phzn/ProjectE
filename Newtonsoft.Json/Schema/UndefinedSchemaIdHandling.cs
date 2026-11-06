using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AF RID: 175
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum UndefinedSchemaIdHandling
	{
		// Token: 0x04000367 RID: 871
		None,
		// Token: 0x04000368 RID: 872
		UseTypeName,
		// Token: 0x04000369 RID: 873
		UseAssemblyQualifiedName
	}
}
