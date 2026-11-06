using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonWriterException : JsonException
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00010243 File Offset: 0x0000E443
		[Nullable(2)]
		public string Path
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001024B File Offset: 0x0000E44B
		public JsonWriterException()
		{
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00010253 File Offset: 0x0000E453
		public JsonWriterException(string message)
			: base(message)
		{
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001025C File Offset: 0x0000E45C
		public JsonWriterException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00010266 File Offset: 0x0000E466
		public JsonWriterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00010270 File Offset: 0x0000E470
		public JsonWriterException(string message, string path, [Nullable(2)] Exception innerException)
			: base(message, innerException)
		{
			this.Path = path;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00010281 File Offset: 0x0000E481
		internal static JsonWriterException Create(JsonWriter writer, string message, [Nullable(2)] Exception ex)
		{
			return JsonWriterException.Create(writer.ContainerPath, message, ex);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00010290 File Offset: 0x0000E490
		internal static JsonWriterException Create(string path, string message, [Nullable(2)] Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, path, ex);
		}
	}
}
