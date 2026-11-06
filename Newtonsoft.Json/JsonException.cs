using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonException : Exception
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00002FB5 File Offset: 0x000011B5
		public JsonException()
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002FBD File Offset: 0x000011BD
		public JsonException(string message)
			: base(message)
		{
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002FC6 File Offset: 0x000011C6
		public JsonException(string message, [Nullable(2)] Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002FD0 File Offset: 0x000011D0
		public JsonException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002FDA File Offset: 0x000011DA
		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
}
