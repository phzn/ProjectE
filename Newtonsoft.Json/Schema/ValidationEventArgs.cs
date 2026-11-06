using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000B0 RID: 176
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x00026D4F File Offset: 0x00024F4F
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00026D69 File Offset: 0x00024F69
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00026D71 File Offset: 0x00024F71
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00026D7E File Offset: 0x00024F7E
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x0400036A RID: 874
		private readonly JsonSchemaException _ex;
	}
}
