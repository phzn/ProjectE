using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AA RID: 170
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaNode
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x000263D2 File Offset: 0x000245D2
		public string Id { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x000263DA File Offset: 0x000245DA
		public ReadOnlyCollection<JsonSchema> Schemas { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x000263E2 File Offset: 0x000245E2
		public Dictionary<string, JsonSchemaNode> Properties { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x000263EA File Offset: 0x000245EA
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x000263F2 File Offset: 0x000245F2
		public List<JsonSchemaNode> Items { get; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x000263FA File Offset: 0x000245FA
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x00026402 File Offset: 0x00024602
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0002640B File Offset: 0x0002460B
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x00026413 File Offset: 0x00024613
		public JsonSchemaNode AdditionalItems { get; set; }

		// Token: 0x0600093E RID: 2366 RVA: 0x0002641C File Offset: 0x0002461C
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[] { schema });
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00026478 File Offset: 0x00024678
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union<JsonSchema>(new JsonSchema[] { schema }).ToList<JsonSchema>());
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.AdditionalItems = source.AdditionalItems;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002650C File Offset: 0x0002470C
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00026518 File Offset: 0x00024718
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", schemata.Select<JsonSchema, string>((JsonSchema s) => s.InternalId).OrderBy<string, string>((string id) => id, StringComparer.Ordinal));
		}
	}
}
