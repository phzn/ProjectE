using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A6 RID: 166
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00025167 File Offset: 0x00023367
		public int LineNumber { get; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0002516F File Offset: 0x0002336F
		public int LinePosition { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x00025177 File Offset: 0x00023377
		public string Path { get; }

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002517F File Offset: 0x0002337F
		public JsonSchemaException()
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00025187 File Offset: 0x00023387
		public JsonSchemaException(string message)
			: base(message)
		{
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00025190 File Offset: 0x00023390
		public JsonSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002519A File Offset: 0x0002339A
		public JsonSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000251A4 File Offset: 0x000233A4
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
