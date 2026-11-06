using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E4 RID: 228
	[NullableContext(1)]
	[Nullable(0)]
	public class ExpandoObjectConverter : JsonConverter
	{
		// Token: 0x06000C40 RID: 3136 RVA: 0x00030FA9 File Offset: 0x0002F1A9
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00030FAB File Offset: 0x0002F1AB
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			return this.ReadValue(reader);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00030FB4 File Offset: 0x0002F1B4
		[return: Nullable(2)]
		private object ReadValue(JsonReader reader)
		{
			if (!reader.MoveToContent())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
			}
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadObject(reader);
			}
			if (tokenType == JsonToken.StartArray)
			{
				return this.ReadList(reader);
			}
			if (JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
			{
				return reader.Value;
			}
			throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0003102C File Offset: 0x0002F22C
		private object ReadList(JsonReader reader)
		{
			IList<object> list = new List<object>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						return list;
					}
					object obj = this.ReadValue(reader);
					list.Add(obj);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00031078 File Offset: 0x0002F278
		private object ReadObject(JsonReader reader)
		{
			IDictionary<string, object> dictionary = new ExpandoObject();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType == JsonToken.EndObject)
						{
							return dictionary;
						}
					}
				}
				else
				{
					string text = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
					}
					object obj = this.ReadValue(reader);
					dictionary[text] = obj;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000310EA File Offset: 0x0002F2EA
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ExpandoObject);
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x000310FC File Offset: 0x0002F2FC
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
