using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A8 RID: 168
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00025B6C File Offset: 0x00023D6C
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x00025B74 File Offset: 0x00023D74
		public bool Required { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00025B7D File Offset: 0x00023D7D
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x00025B85 File Offset: 0x00023D85
		public JsonSchemaType Type { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00025B8E File Offset: 0x00023D8E
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00025B96 File Offset: 0x00023D96
		public int? MinimumLength { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00025B9F File Offset: 0x00023D9F
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x00025BA7 File Offset: 0x00023DA7
		public int? MaximumLength { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00025BB0 File Offset: 0x00023DB0
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x00025BB8 File Offset: 0x00023DB8
		public double? DivisibleBy { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00025BC1 File Offset: 0x00023DC1
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x00025BC9 File Offset: 0x00023DC9
		public double? Minimum { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00025BD2 File Offset: 0x00023DD2
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x00025BDA File Offset: 0x00023DDA
		public double? Maximum { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00025BE3 File Offset: 0x00023DE3
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x00025BEB File Offset: 0x00023DEB
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00025BF4 File Offset: 0x00023DF4
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x00025BFC File Offset: 0x00023DFC
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00025C05 File Offset: 0x00023E05
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00025C0D File Offset: 0x00023E0D
		public int? MinimumItems { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x00025C16 File Offset: 0x00023E16
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x00025C1E File Offset: 0x00023E1E
		public int? MaximumItems { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00025C27 File Offset: 0x00023E27
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x00025C2F File Offset: 0x00023E2F
		public IList<string> Patterns { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00025C38 File Offset: 0x00023E38
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00025C40 File Offset: 0x00023E40
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00025C49 File Offset: 0x00023E49
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00025C51 File Offset: 0x00023E51
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00025C5A File Offset: 0x00023E5A
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00025C62 File Offset: 0x00023E62
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x00025C6B File Offset: 0x00023E6B
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x00025C73 File Offset: 0x00023E73
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00025C7C File Offset: 0x00023E7C
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x00025C84 File Offset: 0x00023E84
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x00025C8D File Offset: 0x00023E8D
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x00025C95 File Offset: 0x00023E95
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x00025C9E File Offset: 0x00023E9E
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x00025CA6 File Offset: 0x00023EA6
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00025CAF File Offset: 0x00023EAF
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x00025CB7 File Offset: 0x00023EB7
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x00025CC0 File Offset: 0x00023EC0
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x00025CC8 File Offset: 0x00023EC8
		public bool UniqueItems { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00025CD1 File Offset: 0x00023ED1
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00025CD9 File Offset: 0x00023ED9
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00025CE2 File Offset: 0x00023EE2
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x00025CEA File Offset: 0x00023EEA
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x06000929 RID: 2345 RVA: 0x00025CF3 File Offset: 0x00023EF3
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00025D18 File Offset: 0x00023F18
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema jsonSchema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, jsonSchema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00025D68 File Offset: 0x00023F68
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = model.Required || schema.Required.GetValueOrDefault();
			model.Type &= schema.Type ?? JsonSchemaType.Any;
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = model.ExclusiveMinimum || schema.ExclusiveMinimum.GetValueOrDefault();
			model.ExclusiveMaximum = model.ExclusiveMaximum || schema.ExclusiveMaximum.GetValueOrDefault();
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = model.PositionalItemsValidation || schema.PositionalItemsValidation;
			model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
			model.AllowAdditionalItems = model.AllowAdditionalItems && schema.AllowAdditionalItems;
			model.UniqueItems = model.UniqueItems || schema.UniqueItems;
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct<JToken>(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= schema.Disallow.GetValueOrDefault();
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
