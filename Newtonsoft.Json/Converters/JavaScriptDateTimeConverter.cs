using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E6 RID: 230
	[NullableContext(1)]
	[Nullable(0)]
	public class JavaScriptDateTimeConverter : DateTimeConverterBase
	{
		// Token: 0x06000C51 RID: 3153 RVA: 0x000313D8 File Offset: 0x0002F5D8
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			long num;
			if (value is DateTime)
			{
				num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(((DateTime)value).ToUniversalTime());
			}
			else
			{
				if (!(value is DateTimeOffset))
				{
					throw new JsonSerializationException("Expected date object value.");
				}
				num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(((DateTimeOffset)value).ToUniversalTime().UtcDateTime);
			}
			writer.WriteStartConstructor("Date");
			writer.WriteValue(num);
			writer.WriteEndConstructor();
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0003144C File Offset: 0x0002F64C
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Null)
			{
				if (reader.TokenType == JsonToken.StartConstructor)
				{
					object value = reader.Value;
					if (string.Equals((value != null) ? value.ToString() : null, "Date", StringComparison.Ordinal))
					{
						DateTime dateTime;
						string text;
						if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out dateTime, out text))
						{
							throw JsonSerializationException.Create(reader, text);
						}
						if ((ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType) == typeof(DateTimeOffset))
						{
							return new DateTimeOffset(dateTime);
						}
						return dateTime;
					}
				}
				throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
			}
			if (!ReflectionUtils.IsNullable(objectType))
			{
				throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			return null;
		}
	}
}
