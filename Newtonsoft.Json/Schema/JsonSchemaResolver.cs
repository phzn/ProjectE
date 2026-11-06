using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AC RID: 172
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0002658D File Offset: 0x0002478D
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x00026595 File Offset: 0x00024795
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x06000946 RID: 2374 RVA: 0x0002659E File Offset: 0x0002479E
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000265B4 File Offset: 0x000247B4
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
