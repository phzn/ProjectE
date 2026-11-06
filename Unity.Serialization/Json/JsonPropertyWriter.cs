using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Unity.Serialization.Json
{
	// Token: 0x02000063 RID: 99
	internal class JsonPropertyWriter : JsonPropertyVisitor, IPropertyBagVisitor, ICollectionPropertyBagVisitor, IListPropertyBagVisitor, IDictionaryPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000A493 File Offset: 0x00008693
		public JsonWriter Writer
		{
			get
			{
				return this.m_Writer;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000A49B File Offset: 0x0000869B
		public void SetWriter(JsonWriter writer)
		{
			this.m_Writer = writer;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000A4A4 File Offset: 0x000086A4
		public void SetSerializedType(Type type)
		{
			this.m_SerializedType = type;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000A4AD File Offset: 0x000086AD
		public void SetDisableRootAdapters(bool disableRootAdapters)
		{
			this.m_DisableRootAdapters = disableRootAdapters;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000A4B6 File Offset: 0x000086B6
		public void SetGlobalAdapters(List<IJsonAdapter> adapters)
		{
			this.m_Adapters.Global = adapters;
			this.m_HasPrimitiveOrStringGlobalAdapters = JsonAdapterCollection.ContainsPrimitiveOrStringAdapter(adapters);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000A4D0 File Offset: 0x000086D0
		public void SetUserDefinedAdapters(List<IJsonAdapter> adapters)
		{
			this.m_Adapters.UserDefined = adapters;
			this.m_HasPrimitiveOrStringUserDefinedAdapters = JsonAdapterCollection.ContainsPrimitiveOrStringAdapter(adapters);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000A4EA File Offset: 0x000086EA
		public void SetGlobalMigrations(List<IJsonMigration> migrations)
		{
			this.m_Migrations.Global = migrations;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000A4F8 File Offset: 0x000086F8
		public void SetUserDefinedMigration(List<IJsonMigration> migrations)
		{
			this.m_Migrations.UserDefined = migrations;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000A506 File Offset: 0x00008706
		public void SetSerializedReferences(SerializedReferences serializedReferences)
		{
			this.m_SerializedReferences = serializedReferences;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000A50F File Offset: 0x0000870F
		public JsonPropertyWriter()
		{
			this.m_Adapters.InternalAdapter = new JsonAdapter();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000A528 File Offset: 0x00008728
		private JsonPropertyWriter.SerializedContainerMetadata GetSerializedContainerMetadata<TContainer>(ref TContainer container)
		{
			Type typeFromHandle = typeof(TContainer);
			if (typeFromHandle == typeof(JsonObject) || typeFromHandle == typeof(JsonArray))
			{
				return default(JsonPropertyWriter.SerializedContainerMetadata);
			}
			JsonPropertyWriter.SerializedContainerMetadata serializedContainerMetadata = default(JsonPropertyWriter.SerializedContainerMetadata);
			if (!TypeTraits<TContainer>.IsValueType && !container.GetType().IsValueType)
			{
				object obj = container;
				int num;
				if (this.m_SerializedReferences != null && this.m_SerializedReferences.TryGetSerializedReference(obj, out num))
				{
					if (!this.m_SerializedReferences.SetSerialized(obj))
					{
						return new JsonPropertyWriter.SerializedContainerMetadata
						{
							IsSerializedReference = true,
							SerializedId = num
						};
					}
					serializedContainerMetadata.HasSerializedId = true;
					serializedContainerMetadata.SerializedId = num;
				}
			}
			else
			{
				serializedContainerMetadata.SerializedId = -1;
			}
			bool flag = base.Property is IPropertyWrapper && null != this.m_SerializedType;
			Type type = base.Property.DeclaredValueType();
			serializedContainerMetadata.HasSerializedType = typeFromHandle != type && !flag && Nullable.GetUnderlyingType(type) != typeFromHandle;
			int num2;
			serializedContainerMetadata.HasSerializedVersion = this.m_Migrations.TryGetSerializedVersion<TContainer>(out num2);
			serializedContainerMetadata.SerializedVersion = num2;
			return serializedContainerMetadata;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000A668 File Offset: 0x00008868
		private void WriteSerializedContainerMetadata<TContainer>(ref TContainer container, JsonPropertyWriter.SerializedContainerMetadata metadata, ref int count)
		{
			if (metadata.HasSerializedId)
			{
				using (base.CreatePropertyScope(JsonPropertyWriter.s_SerializedIdProperty))
				{
					JsonPropertyWriter.SerializedId serializedId = new JsonPropertyWriter.SerializedId
					{
						Id = metadata.SerializedId
					};
					((IPropertyAccept<JsonPropertyWriter.SerializedId>)JsonPropertyWriter.s_SerializedIdProperty).Accept(this, ref serializedId);
				}
			}
			if (metadata.HasSerializedType)
			{
				using (base.CreatePropertyScope(JsonPropertyWriter.s_SerializedTypeProperty))
				{
					JsonPropertyWriter.SerializedType serializedType = new JsonPropertyWriter.SerializedType
					{
						Type = container.GetType()
					};
					((IPropertyAccept<JsonPropertyWriter.SerializedType>)JsonPropertyWriter.s_SerializedTypeProperty).Accept(this, ref serializedType);
				}
			}
			if (metadata.HasSerializedVersion)
			{
				using (base.CreatePropertyScope(JsonPropertyWriter.s_SerializedVersionProperty))
				{
					JsonPropertyWriter.SerializedVersion serializedVersion = new JsonPropertyWriter.SerializedVersion
					{
						Version = metadata.SerializedVersion
					};
					((IPropertyAccept<JsonPropertyWriter.SerializedVersion>)JsonPropertyWriter.s_SerializedVersionProperty).Accept(this, ref serializedVersion);
				}
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A780 File Offset: 0x00008980
		private void WriteSerializedReference(int id)
		{
			using (this.m_Writer.WriteObjectScope(null))
			{
				this.m_Writer.WriteKey("$ref");
				this.m_Writer.WriteValue(id);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A7D8 File Offset: 0x000089D8
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			bool flag = properties is IPropertyWrapper;
			int num = 0;
			if (!flag)
			{
				JsonPropertyWriter.SerializedContainerMetadata serializedContainerMetadata = this.GetSerializedContainerMetadata<TContainer>(ref container);
				if (serializedContainerMetadata.IsSerializedReference)
				{
					this.WriteSerializedReference(serializedContainerMetadata.SerializedId);
					return;
				}
				this.m_Writer.WriteBeginObject(null);
				this.WriteSerializedContainerMetadata<TContainer>(ref container, serializedContainerMetadata, ref num);
			}
			foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
			{
				if (!PropertyChecks.IsPropertyExcludedFromSerialization(property))
				{
					using (base.CreatePropertyScope(property))
					{
						property.Accept(this, ref container);
					}
				}
			}
			if (!flag)
			{
				this.m_Writer.WriteEndObject();
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		void ICollectionPropertyBagVisitor.Visit<TCollection, TElement>(ICollectionPropertyBag<TCollection, TElement> properties, ref TCollection container)
		{
			JsonPropertyWriter.SerializedContainerMetadata serializedContainerMetadata = this.GetSerializedContainerMetadata<TCollection>(ref container);
			if (serializedContainerMetadata.IsSerializedReference)
			{
				this.WriteSerializedReference(serializedContainerMetadata.SerializedId);
				return;
			}
			int num = 0;
			if (serializedContainerMetadata.Exists)
			{
				this.m_Writer.WriteBeginObject(null);
				this.WriteSerializedContainerMetadata<TCollection>(ref container, serializedContainerMetadata, ref num);
				this.m_Writer.WriteKey("$elements");
			}
			using (this.m_Writer.WriteArrayScope(null))
			{
				foreach (IProperty<TCollection> property in properties.GetProperties(ref container))
				{
					using (base.CreatePropertyScope(property))
					{
						property.Accept(this, ref container);
					}
				}
			}
			if (serializedContainerMetadata.Exists)
			{
				this.m_Writer.WriteEndObject();
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A9C0 File Offset: 0x00008BC0
		void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList container)
		{
			JsonPropertyWriter.SerializedContainerMetadata serializedContainerMetadata = this.GetSerializedContainerMetadata<TList>(ref container);
			if (serializedContainerMetadata.IsSerializedReference)
			{
				this.WriteSerializedReference(serializedContainerMetadata.SerializedId);
				return;
			}
			int num = 0;
			if (serializedContainerMetadata.Exists)
			{
				this.m_Writer.WriteBeginObject(null);
				this.WriteSerializedContainerMetadata<TList>(ref container, serializedContainerMetadata, ref num);
				this.m_Writer.WriteKey("$elements");
			}
			using (this.m_Writer.WriteArrayScope(null))
			{
				foreach (IProperty<TList> property in properties.GetProperties(ref container))
				{
					using (base.CreatePropertyScope(property))
					{
						property.Accept(this, ref container);
					}
				}
			}
			if (serializedContainerMetadata.Exists)
			{
				this.m_Writer.WriteEndObject();
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000AACC File Offset: 0x00008CCC
		void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary container)
		{
			if (typeof(TKey) != typeof(string))
			{
				((ICollectionPropertyBagVisitor)this).Visit<TDictionary, KeyValuePair<TKey, TValue>>(properties, ref container);
				return;
			}
			JsonPropertyWriter.SerializedContainerMetadata serializedContainerMetadata = this.GetSerializedContainerMetadata<TDictionary>(ref container);
			if (serializedContainerMetadata.IsSerializedReference)
			{
				this.WriteSerializedReference(serializedContainerMetadata.SerializedId);
				return;
			}
			int num = 0;
			if (serializedContainerMetadata.Exists)
			{
				this.m_Writer.WriteBeginObject(null);
				this.WriteSerializedContainerMetadata<TDictionary>(ref container, serializedContainerMetadata, ref num);
				this.m_Writer.WriteKey("$elements");
			}
			using (this.m_Writer.WriteObjectScope(null))
			{
				DictionaryElementProperty<TDictionary, TKey, TValue> dictionaryElementProperty = new DictionaryElementProperty<TDictionary, TKey, TValue>();
				foreach (KeyValuePair<TKey, TValue> keyValuePair in container)
				{
					dictionaryElementProperty.Key = keyValuePair.Key;
					((IPropertyAccept<TDictionary>)dictionaryElementProperty).Accept(this, ref container);
				}
			}
			if (serializedContainerMetadata.Exists)
			{
				this.m_Writer.WriteEndObject();
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			bool flag = property is IPropertyWrapper;
			if (!flag && !(property is ICollectionElementProperty))
			{
				this.m_Writer.WriteKey(property.Name);
			}
			TValue value = property.GetValue(ref container);
			this.WriteValue<TValue>(ref value, flag);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000AC28 File Offset: 0x00008E28
		internal void WriteValue<TValue>(ref TValue value, bool isRoot = false)
		{
			JsonAdapterFilter jsonAdapterFilter = JsonAdapterFilter.All;
			if (isRoot && this.m_DisableRootAdapters)
			{
				jsonAdapterFilter = JsonAdapterFilter.None;
			}
			else if (TypeTraits<TValue>.IsPrimitiveOrString)
			{
				if (!this.m_HasPrimitiveOrStringGlobalAdapters)
				{
					jsonAdapterFilter &= ~JsonAdapterFilter.Global;
				}
				if (!this.m_HasPrimitiveOrStringUserDefinedAdapters)
				{
					jsonAdapterFilter &= ~JsonAdapterFilter.UserDefined;
				}
			}
			this.WriteValueWithAdapters<TValue>(value, this.m_Adapters.GetEnumerator(jsonAdapterFilter), isRoot);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000AC80 File Offset: 0x00008E80
		internal void WriteValueWithAdapters<TValue>(TValue value, JsonAdapterCollection.Enumerator adapters, bool isRoot)
		{
			while (adapters.MoveNext())
			{
				IJsonAdapter jsonAdapter = adapters.Current;
				IJsonAdapter<TValue> jsonAdapter2 = jsonAdapter as IJsonAdapter<TValue>;
				if (jsonAdapter2 != null)
				{
					IJsonAdapter<TValue> jsonAdapter3 = jsonAdapter2;
					JsonSerializationContext<TValue> jsonSerializationContext = new JsonSerializationContext<TValue>(this, adapters, value, isRoot);
					jsonAdapter3.Serialize(in jsonSerializationContext, value);
					return;
				}
				IContravariantJsonAdapter<TValue> contravariantJsonAdapter = jsonAdapter as IContravariantJsonAdapter<TValue>;
				if (contravariantJsonAdapter != null)
				{
					contravariantJsonAdapter.Serialize(new JsonSerializationContext<TValue>(this, adapters, value, isRoot), value);
					return;
				}
			}
			this.WriteValueWithoutAdapters<TValue>(value, isRoot);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		internal void WriteValueWithoutAdapters<TValue>(TValue value, bool isRoot)
		{
			if (!TypeTraits<TValue>.IsValueType && (!isRoot || !this.m_DisableRootAdapters) && value is Object)
			{
				throw new NotSupportedException("JsonSerialization does not support polymorphic unity object references.");
			}
			if (TypeTraits<TValue>.IsEnum)
			{
				JsonPropertyWriter.WritePrimitiveBoxed(this.m_Writer, value, Enum.GetUnderlyingType(typeof(TValue)));
				return;
			}
			if (TypeTraits<TValue>.CanBeNull && EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
			{
				this.m_Writer.WriteNull();
				return;
			}
			if (TypeTraits<TValue>.IsMultidimensionalArray)
			{
				this.m_Writer.WriteNull();
				return;
			}
			if (TypeTraits<TValue>.IsNullable)
			{
				Type underlyingType = Nullable.GetUnderlyingType(typeof(TValue));
				if (TypeTraits.IsContainer(underlyingType))
				{
					object obj = Convert.ChangeType(value, underlyingType);
					VisitReturnCode visitReturnCode;
					if (!PropertyContainer.TryAccept<object>(this, ref obj, out visitReturnCode, default(VisitParameters)))
					{
						switch (visitReturnCode)
						{
						case VisitReturnCode.NullContainer:
							throw new ArgumentNullException("value");
						case VisitReturnCode.InvalidContainerType:
							throw new InvalidContainerTypeException(value.GetType());
						case VisitReturnCode.MissingPropertyBag:
							throw new MissingPropertyBagException(value.GetType());
						default:
							throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
						}
					}
				}
				else
				{
					JsonPropertyWriter.WritePrimitiveBoxed(this.m_Writer, value, underlyingType);
				}
				return;
			}
			if (TypeTraits<TValue>.IsObject && !TypeTraits.IsContainer(value.GetType()))
			{
				JsonPropertyWriter.WritePrimitiveBoxed(this.m_Writer, value, value.GetType());
				return;
			}
			if (!TypeTraits<TValue>.IsContainer)
			{
				throw new Exception(string.Format("Unsupported Type {0}.", value.GetType()));
			}
			VisitReturnCode visitReturnCode2;
			if (PropertyContainer.TryAccept<TValue>(this, ref value, out visitReturnCode2, default(VisitParameters)))
			{
				return;
			}
			switch (visitReturnCode2)
			{
			case VisitReturnCode.NullContainer:
				throw new ArgumentNullException("value");
			case VisitReturnCode.InvalidContainerType:
				throw new InvalidContainerTypeException(value.GetType());
			case VisitReturnCode.MissingPropertyBag:
				throw new MissingPropertyBagException(value.GetType());
			default:
				throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode2));
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000AF18 File Offset: 0x00009118
		internal static void WritePrimitiveBoxed(JsonWriter writer, object value, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				writer.WriteValueLiteral(((bool)value) ? "true" : "false");
				return;
			case TypeCode.Char:
				writer.WriteValue((char)value);
				return;
			case TypeCode.SByte:
				writer.WriteValue((int)((sbyte)value));
				return;
			case TypeCode.Byte:
				writer.WriteValue((int)((byte)value));
				return;
			case TypeCode.Int16:
				writer.WriteValue((int)((short)value));
				return;
			case TypeCode.UInt16:
				writer.WriteValue((int)((ushort)value));
				return;
			case TypeCode.Int32:
				writer.WriteValue((int)value);
				return;
			case TypeCode.UInt32:
				writer.WriteValue((uint)value);
				return;
			case TypeCode.Int64:
				writer.WriteValue((long)value);
				return;
			case TypeCode.UInt64:
				writer.WriteValue((ulong)value);
				return;
			case TypeCode.Single:
				writer.WriteValue((float)value);
				return;
			case TypeCode.Double:
				writer.WriteValue((double)value);
				return;
			case TypeCode.String:
				writer.WriteValue(value as string);
				return;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x04000135 RID: 309
		private static readonly JsonPropertyWriter.SerializedIdProperty s_SerializedIdProperty = new JsonPropertyWriter.SerializedIdProperty();

		// Token: 0x04000136 RID: 310
		private static readonly JsonPropertyWriter.SerializedTypeProperty s_SerializedTypeProperty = new JsonPropertyWriter.SerializedTypeProperty();

		// Token: 0x04000137 RID: 311
		private static readonly JsonPropertyWriter.SerializedVersionProperty s_SerializedVersionProperty = new JsonPropertyWriter.SerializedVersionProperty();

		// Token: 0x04000138 RID: 312
		private JsonWriter m_Writer;

		// Token: 0x04000139 RID: 313
		private Type m_SerializedType;

		// Token: 0x0400013A RID: 314
		private bool m_DisableRootAdapters;

		// Token: 0x0400013B RID: 315
		private JsonAdapterCollection m_Adapters;

		// Token: 0x0400013C RID: 316
		private JsonMigrationCollection m_Migrations;

		// Token: 0x0400013D RID: 317
		private SerializedReferences m_SerializedReferences;

		// Token: 0x0400013E RID: 318
		private bool m_HasPrimitiveOrStringGlobalAdapters;

		// Token: 0x0400013F RID: 319
		private bool m_HasPrimitiveOrStringUserDefinedAdapters;

		// Token: 0x02000064 RID: 100
		private struct SerializedId
		{
			// Token: 0x04000140 RID: 320
			public int Id;
		}

		// Token: 0x02000065 RID: 101
		private struct SerializedType
		{
			// Token: 0x04000141 RID: 321
			public Type Type;
		}

		// Token: 0x02000066 RID: 102
		private struct SerializedVersion
		{
			// Token: 0x04000142 RID: 322
			public int Version;
		}

		// Token: 0x02000067 RID: 103
		private class SerializedIdProperty : Property<JsonPropertyWriter.SerializedId, int>
		{
			// Token: 0x1700003F RID: 63
			// (get) Token: 0x06000273 RID: 627 RVA: 0x0000B062 File Offset: 0x00009262
			public override string Name
			{
				get
				{
					return "$id";
				}
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x06000274 RID: 628 RVA: 0x00003204 File Offset: 0x00001404
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000275 RID: 629 RVA: 0x0000B069 File Offset: 0x00009269
			public override int GetValue(ref JsonPropertyWriter.SerializedId container)
			{
				return container.Id;
			}

			// Token: 0x06000276 RID: 630 RVA: 0x0000B071 File Offset: 0x00009271
			public override void SetValue(ref JsonPropertyWriter.SerializedId container, int value)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}
		}

		// Token: 0x02000068 RID: 104
		private class SerializedTypeProperty : Property<JsonPropertyWriter.SerializedType, string>
		{
			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000278 RID: 632 RVA: 0x0000B085 File Offset: 0x00009285
			public override string Name
			{
				get
				{
					return "$type";
				}
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x06000279 RID: 633 RVA: 0x00003204 File Offset: 0x00001404
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600027A RID: 634 RVA: 0x0000B08C File Offset: 0x0000928C
			public override string GetValue(ref JsonPropertyWriter.SerializedType container)
			{
				return string.Format("{0}, {1}", container.Type, container.Type.Assembly.GetName().Name);
			}

			// Token: 0x0600027B RID: 635 RVA: 0x0000B071 File Offset: 0x00009271
			public override void SetValue(ref JsonPropertyWriter.SerializedType container, string value)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}
		}

		// Token: 0x02000069 RID: 105
		private class SerializedVersionProperty : Property<JsonPropertyWriter.SerializedVersion, int>
		{
			// Token: 0x17000043 RID: 67
			// (get) Token: 0x0600027D RID: 637 RVA: 0x0000B0BB File Offset: 0x000092BB
			public override string Name
			{
				get
				{
					return "$version";
				}
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x0600027E RID: 638 RVA: 0x00003204 File Offset: 0x00001404
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600027F RID: 639 RVA: 0x0000B0C2 File Offset: 0x000092C2
			public override int GetValue(ref JsonPropertyWriter.SerializedVersion container)
			{
				return container.Version;
			}

			// Token: 0x06000280 RID: 640 RVA: 0x0000B071 File Offset: 0x00009271
			public override void SetValue(ref JsonPropertyWriter.SerializedVersion container, int value)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}
		}

		// Token: 0x0200006A RID: 106
		private struct SerializedContainerMetadata
		{
			// Token: 0x17000045 RID: 69
			// (get) Token: 0x06000282 RID: 642 RVA: 0x0000B0D2 File Offset: 0x000092D2
			public bool Exists
			{
				get
				{
					return this.HasSerializedId || this.HasSerializedType || this.HasSerializedVersion;
				}
			}

			// Token: 0x04000143 RID: 323
			public bool IsSerializedReference;

			// Token: 0x04000144 RID: 324
			public bool HasSerializedId;

			// Token: 0x04000145 RID: 325
			public bool HasSerializedType;

			// Token: 0x04000146 RID: 326
			public bool HasSerializedVersion;

			// Token: 0x04000147 RID: 327
			public int SerializedId;

			// Token: 0x04000148 RID: 328
			public int SerializedVersion;
		}
	}
}
