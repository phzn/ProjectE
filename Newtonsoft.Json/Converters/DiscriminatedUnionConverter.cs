using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E2 RID: 226
	[NullableContext(1)]
	[Nullable(0)]
	public class DiscriminatedUnionConverter : JsonConverter
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x00030820 File Offset: 0x0002EA20
		private static Type CreateUnionTypeLookup(Type t)
		{
			MethodCall<object, object> getUnionCases = FSharpUtils.Instance.GetUnionCases;
			object obj = null;
			object[] array = new object[2];
			array[0] = t;
			object obj2 = ((object[])getUnionCases(obj, array)).First<object>();
			return (Type)FSharpUtils.Instance.GetUnionCaseInfoDeclaringType(obj2);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00030868 File Offset: 0x0002EA68
		private static DiscriminatedUnionConverter.Union CreateUnion(Type t)
		{
			MethodCall<object, object> preComputeUnionTagReader = FSharpUtils.Instance.PreComputeUnionTagReader;
			object obj = null;
			object[] array = new object[2];
			array[0] = t;
			DiscriminatedUnionConverter.Union union = new DiscriminatedUnionConverter.Union((FSharpFunction)preComputeUnionTagReader(obj, array), new List<DiscriminatedUnionConverter.UnionCase>());
			MethodCall<object, object> getUnionCases = FSharpUtils.Instance.GetUnionCases;
			object obj2 = null;
			object[] array2 = new object[2];
			array2[0] = t;
			foreach (object obj3 in (object[])getUnionCases(obj2, array2))
			{
				int num = (int)FSharpUtils.Instance.GetUnionCaseInfoTag(obj3);
				string text = (string)FSharpUtils.Instance.GetUnionCaseInfoName(obj3);
				PropertyInfo[] array4 = (PropertyInfo[])FSharpUtils.Instance.GetUnionCaseInfoFields(obj3, Array.Empty<object>());
				MethodCall<object, object> preComputeUnionReader = FSharpUtils.Instance.PreComputeUnionReader;
				object obj4 = null;
				object[] array5 = new object[2];
				array5[0] = obj3;
				FSharpFunction fsharpFunction = (FSharpFunction)preComputeUnionReader(obj4, array5);
				MethodCall<object, object> preComputeUnionConstructor = FSharpUtils.Instance.PreComputeUnionConstructor;
				object obj5 = null;
				object[] array6 = new object[2];
				array6[0] = obj3;
				DiscriminatedUnionConverter.UnionCase unionCase = new DiscriminatedUnionConverter.UnionCase(num, text, array4, fsharpFunction, (FSharpFunction)preComputeUnionConstructor(obj5, array6));
				union.Cases.Add(unionCase);
			}
			return union;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00030970 File Offset: 0x0002EB70
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type type = DiscriminatedUnionConverter.UnionTypeLookupCache.Get(value.GetType());
			DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(type);
			int tag = (int)union.TagReader.Invoke(new object[] { value });
			DiscriminatedUnionConverter.UnionCase unionCase = union.Cases.Single<DiscriminatedUnionConverter.UnionCase>((DiscriminatedUnionConverter.UnionCase c) => c.Tag == tag);
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue(unionCase.Name);
			if (unionCase.Fields != null && unionCase.Fields.Length != 0)
			{
				object[] array = (object[])unionCase.FieldReader.Invoke(new object[] { value });
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				writer.WriteStartArray();
				foreach (object obj in array)
				{
					serializer.Serialize(writer, obj);
				}
				writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00030AA0 File Offset: 0x0002ECA0
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DiscriminatedUnionConverter.UnionCase unionCase = null;
			string caseName = null;
			JArray jarray = null;
			reader.ReadAndAssert();
			Func<DiscriminatedUnionConverter.UnionCase, bool> <>9__0;
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = reader.Value.ToString();
				if (string.Equals(text, "Case", StringComparison.OrdinalIgnoreCase))
				{
					reader.ReadAndAssert();
					DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(objectType);
					caseName = reader.Value.ToString();
					IEnumerable<DiscriminatedUnionConverter.UnionCase> cases = union.Cases;
					Func<DiscriminatedUnionConverter.UnionCase, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (DiscriminatedUnionConverter.UnionCase c) => c.Name == caseName);
					}
					unionCase = cases.SingleOrDefault<DiscriminatedUnionConverter.UnionCase>(func);
					if (unionCase == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
					}
				}
				else
				{
					if (!string.Equals(text, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text));
					}
					reader.ReadAndAssert();
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jarray = (JArray)JToken.ReadFrom(reader);
				}
				reader.ReadAndAssert();
			}
			if (unionCase == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			object[] array = new object[unionCase.Fields.Length];
			if (unionCase.Fields.Length != 0 && jarray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jarray != null)
			{
				if (unionCase.Fields.Length != jarray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties defined by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
				}
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					PropertyInfo propertyInfo = unionCase.Fields[i];
					array[i] = jtoken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			object[] array2 = new object[] { array };
			return unionCase.Constructor.Invoke(array2);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00030C9C File Offset: 0x0002EE9C
		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(true);
			bool flag = false;
			object[] array = customAttributes;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i].GetType();
				if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			MethodCall<object, object> isUnion = FSharpUtils.Instance.IsUnion;
			object obj = null;
			object[] array2 = new object[2];
			array2[0] = objectType;
			return (bool)isUnion(obj, array2);
		}

		// Token: 0x040003E9 RID: 1001
		private const string CasePropertyName = "Case";

		// Token: 0x040003EA RID: 1002
		private const string FieldsPropertyName = "Fields";

		// Token: 0x040003EB RID: 1003
		private static readonly ThreadSafeStore<Type, DiscriminatedUnionConverter.Union> UnionCache = new ThreadSafeStore<Type, DiscriminatedUnionConverter.Union>(new Func<Type, DiscriminatedUnionConverter.Union>(DiscriminatedUnionConverter.CreateUnion));

		// Token: 0x040003EC RID: 1004
		private static readonly ThreadSafeStore<Type, Type> UnionTypeLookupCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(DiscriminatedUnionConverter.CreateUnionTypeLookup));

		// Token: 0x020001DD RID: 477
		[Nullable(0)]
		internal class Union
		{
			// Token: 0x06001038 RID: 4152 RVA: 0x00046FD7 File Offset: 0x000451D7
			public Union(FSharpFunction tagReader, List<DiscriminatedUnionConverter.UnionCase> cases)
			{
				this.TagReader = tagReader;
				this.Cases = cases;
			}

			// Token: 0x0400086E RID: 2158
			public readonly FSharpFunction TagReader;

			// Token: 0x0400086F RID: 2159
			public readonly List<DiscriminatedUnionConverter.UnionCase> Cases;
		}

		// Token: 0x020001DE RID: 478
		[Nullable(0)]
		internal class UnionCase
		{
			// Token: 0x06001039 RID: 4153 RVA: 0x00046FED File Offset: 0x000451ED
			public UnionCase(int tag, string name, PropertyInfo[] fields, FSharpFunction fieldReader, FSharpFunction constructor)
			{
				this.Tag = tag;
				this.Name = name;
				this.Fields = fields;
				this.FieldReader = fieldReader;
				this.Constructor = constructor;
			}

			// Token: 0x04000870 RID: 2160
			public readonly int Tag;

			// Token: 0x04000871 RID: 2161
			public readonly string Name;

			// Token: 0x04000872 RID: 2162
			public readonly PropertyInfo[] Fields;

			// Token: 0x04000873 RID: 2163
			public readonly FSharpFunction FieldReader;

			// Token: 0x04000874 RID: 2164
			public readonly FSharpFunction Constructor;
		}
	}
}
