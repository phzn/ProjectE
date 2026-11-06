using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A3 RID: 163
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00023C6C File Offset: 0x00021E6C
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00023C74 File Offset: 0x00021E74
		public string Id { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00023C7D File Offset: 0x00021E7D
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00023C85 File Offset: 0x00021E85
		public string Title { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00023C8E File Offset: 0x00021E8E
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00023C96 File Offset: 0x00021E96
		public bool? Required { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00023C9F File Offset: 0x00021E9F
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x00023CA7 File Offset: 0x00021EA7
		public bool? ReadOnly { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00023CB0 File Offset: 0x00021EB0
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x00023CB8 File Offset: 0x00021EB8
		public bool? Hidden { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00023CC1 File Offset: 0x00021EC1
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00023CC9 File Offset: 0x00021EC9
		public bool? Transient { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00023CD2 File Offset: 0x00021ED2
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x00023CDA File Offset: 0x00021EDA
		public string Description { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00023CE3 File Offset: 0x00021EE3
		// (set) Token: 0x06000888 RID: 2184 RVA: 0x00023CEB File Offset: 0x00021EEB
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00023CF4 File Offset: 0x00021EF4
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x00023CFC File Offset: 0x00021EFC
		public string Pattern { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00023D05 File Offset: 0x00021F05
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x00023D0D File Offset: 0x00021F0D
		public int? MinimumLength { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00023D16 File Offset: 0x00021F16
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x00023D1E File Offset: 0x00021F1E
		public int? MaximumLength { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x00023D27 File Offset: 0x00021F27
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x00023D2F File Offset: 0x00021F2F
		public double? DivisibleBy { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00023D38 File Offset: 0x00021F38
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x00023D40 File Offset: 0x00021F40
		public double? Minimum { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00023D49 File Offset: 0x00021F49
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x00023D51 File Offset: 0x00021F51
		public double? Maximum { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00023D5A File Offset: 0x00021F5A
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x00023D62 File Offset: 0x00021F62
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00023D6B File Offset: 0x00021F6B
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x00023D73 File Offset: 0x00021F73
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x00023D7C File Offset: 0x00021F7C
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x00023D84 File Offset: 0x00021F84
		public int? MinimumItems { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00023D8D File Offset: 0x00021F8D
		// (set) Token: 0x0600089C RID: 2204 RVA: 0x00023D95 File Offset: 0x00021F95
		public int? MaximumItems { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x00023D9E File Offset: 0x00021F9E
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x00023DA6 File Offset: 0x00021FA6
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00023DAF File Offset: 0x00021FAF
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00023DB7 File Offset: 0x00021FB7
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00023DC0 File Offset: 0x00021FC0
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00023DC8 File Offset: 0x00021FC8
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00023DD1 File Offset: 0x00021FD1
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x00023DD9 File Offset: 0x00021FD9
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00023DE2 File Offset: 0x00021FE2
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00023DEA File Offset: 0x00021FEA
		public bool UniqueItems { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00023DF3 File Offset: 0x00021FF3
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x00023DFB File Offset: 0x00021FFB
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00023E04 File Offset: 0x00022004
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x00023E0C File Offset: 0x0002200C
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x00023E15 File Offset: 0x00022015
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x00023E1D File Offset: 0x0002201D
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00023E26 File Offset: 0x00022026
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x00023E2E File Offset: 0x0002202E
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00023E37 File Offset: 0x00022037
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00023E3F File Offset: 0x0002203F
		public string Requires { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00023E48 File Offset: 0x00022048
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00023E50 File Offset: 0x00022050
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00023E59 File Offset: 0x00022059
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00023E61 File Offset: 0x00022061
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00023E6A File Offset: 0x0002206A
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x00023E72 File Offset: 0x00022072
		public JToken Default { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00023E7B File Offset: 0x0002207B
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x00023E83 File Offset: 0x00022083
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00023E8C File Offset: 0x0002208C
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x00023E94 File Offset: 0x00022094
		public string Format { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00023E9D File Offset: 0x0002209D
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x00023EA5 File Offset: 0x000220A5
		internal string Location { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00023EAE File Offset: 0x000220AE
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00023EB6 File Offset: 0x000220B6
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x00023EBE File Offset: 0x000220BE
		internal string DeferredReference { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00023EC7 File Offset: 0x000220C7
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x00023ECF File Offset: 0x000220CF
		internal bool ReferencesResolved { get; set; }

		// Token: 0x060008C2 RID: 2242 RVA: 0x00023ED8 File Offset: 0x000220D8
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00023F11 File Offset: 0x00022111
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00023F1E File Offset: 0x0002211E
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return new JsonSchemaBuilder(resolver).Read(reader);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00023F42 File Offset: 0x00022142
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00023F50 File Offset: 0x00022150
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema jsonSchema;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				jsonSchema = JsonSchema.Read(jsonReader, resolver);
			}
			return jsonSchema;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00023F9C File Offset: 0x0002219C
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00023FAA File Offset: 0x000221AA
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			new JsonSchemaWriter(writer, resolver).WriteSchema(this);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00023FD0 File Offset: 0x000221D0
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x04000307 RID: 775
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
