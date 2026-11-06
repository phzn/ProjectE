using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E7 RID: 231
	[NullableContext(1)]
	[Nullable(0)]
	public class KeyValuePairConverter : JsonConverter
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x00031524 File Offset: 0x0002F724
		private static ReflectionObject InitializeReflectionObject(Type t)
		{
			Type[] genericArguments = t.GetGenericArguments();
			Type type = ((IList<Type>)genericArguments)[0];
			Type type2 = ((IList<Type>)genericArguments)[1];
			return ReflectionObject.Create(t, t.GetConstructor(new Type[] { type, type2 }), new string[] { "Key", "Value" });
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00031578 File Offset: 0x0002F778
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Key"), reflectionObject.GetType("Key"));
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Value"), reflectionObject.GetType("Value"));
			writer.WriteEndObject();
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003162C File Offset: 0x0002F82C
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Null)
			{
				object obj = null;
				object obj2 = null;
				reader.ReadAndAssert();
				Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
				ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(type);
				JsonContract jsonContract = serializer.ContractResolver.ResolveContract(reflectionObject.GetType("Key"));
				JsonContract jsonContract2 = serializer.ContractResolver.ResolveContract(reflectionObject.GetType("Value"));
				while (reader.TokenType == JsonToken.PropertyName)
				{
					string text = reader.Value.ToString();
					if (string.Equals(text, "Key", StringComparison.OrdinalIgnoreCase))
					{
						reader.ReadForTypeAndAssert(jsonContract, false);
						obj = serializer.Deserialize(reader, jsonContract.UnderlyingType);
					}
					else if (string.Equals(text, "Value", StringComparison.OrdinalIgnoreCase))
					{
						reader.ReadForTypeAndAssert(jsonContract2, false);
						obj2 = serializer.Deserialize(reader, jsonContract2.UnderlyingType);
					}
					else
					{
						reader.Skip();
					}
					reader.ReadAndAssert();
				}
				return reflectionObject.Creator(new object[] { obj, obj2 });
			}
			if (!ReflectionUtils.IsNullableType(objectType))
			{
				throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
			}
			return null;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00031744 File Offset: 0x0002F944
		public override bool CanConvert(Type objectType)
		{
			Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
			return type.IsValueType() && type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >);
		}

		// Token: 0x040003F6 RID: 1014
		private const string KeyName = "Key";

		// Token: 0x040003F7 RID: 1015
		private const string ValueName = "Value";

		// Token: 0x040003F8 RID: 1016
		private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(new Func<Type, ReflectionObject>(KeyValuePairConverter.InitializeReflectionObject));
	}
}
