using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000DE RID: 222
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CustomCreationConverter<[Nullable(2)] T> : JsonConverter
	{
		// Token: 0x06000C21 RID: 3105 RVA: 0x000301A5 File Offset: 0x0002E3A5
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000301B4 File Offset: 0x0002E3B4
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000C23 RID: 3107
		public abstract T Create(Type objectType);

		// Token: 0x06000C24 RID: 3108 RVA: 0x000301FC File Offset: 0x0002E3FC
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0003020E File Offset: 0x0002E40E
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
