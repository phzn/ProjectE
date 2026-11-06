using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections.LowLevel.Unsafe.NotBurstCompatible;
using Unity.Properties;
using UnityEngine;

namespace Unity.Serialization.Binary
{
	// Token: 0x020000A5 RID: 165
	internal class BinaryPropertyReader : BinaryPropertyVisitor, ISerializedTypeProvider, IPropertyBagVisitor, IListPropertyBagVisitor, ISetPropertyBagVisitor, IDictionaryPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000109F9 File Offset: 0x0000EBF9
		internal unsafe UnsafeAppendBuffer.Reader* Reader
		{
			get
			{
				return this.m_Stream;
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00010A01 File Offset: 0x0000EC01
		public unsafe void SetStream(UnsafeAppendBuffer.Reader* stream)
		{
			this.m_Stream = stream;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00010A0A File Offset: 0x0000EC0A
		public void SetSerializedType(Type type)
		{
			this.m_SerializedType = type;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00010A13 File Offset: 0x0000EC13
		public void SetDisableRootAdapters(bool disableRootAdapters)
		{
			this.m_DisableRootAdapters = disableRootAdapters;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00010A1C File Offset: 0x0000EC1C
		public void SetGlobalAdapters(List<IBinaryAdapter> adapters)
		{
			this.m_Adapters.Global = adapters;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00010A2A File Offset: 0x0000EC2A
		public void SetUserDefinedAdapters(List<IBinaryAdapter> adapters)
		{
			this.m_Adapters.UserDefined = adapters;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00010A38 File Offset: 0x0000EC38
		public void SetSerializedReferences(SerializedReferences serializedReferences)
		{
			this.m_SerializedReferences = serializedReferences;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010A41 File Offset: 0x0000EC41
		public BinaryPropertyReader()
		{
			this.m_Adapters.Internal = this;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00010A58 File Offset: 0x0000EC58
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			SerializedReferences serializedReferences = this.m_SerializedReferences;
			if (serializedReferences != null)
			{
				serializedReferences.AddDeserializedReference<TContainer>(container);
			}
			foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
			{
				if (!PropertyChecks.IsPropertyExcludedFromSerialization(property))
				{
					property.Accept(this, ref container);
				}
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		unsafe void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList container)
		{
			SerializedReferences serializedReferences = this.m_SerializedReferences;
			if (serializedReferences != null)
			{
				serializedReferences.AddDeserializedReference<TList>(container);
			}
			int num = this.m_Stream->ReadNext<int>();
			if (typeof(TList).IsArray)
			{
				for (int i = 0; i < num; i++)
				{
					container[i] = this.ReadValue<TElement>();
				}
				return;
			}
			container.Clear();
			for (int j = 0; j < num; j++)
			{
				container.Add(this.ReadValue<TElement>());
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00010B5C File Offset: 0x0000ED5C
		unsafe void ISetPropertyBagVisitor.Visit<TSet, TValue>(ISetPropertyBag<TSet, TValue> properties, ref TSet container)
		{
			SerializedReferences serializedReferences = this.m_SerializedReferences;
			if (serializedReferences != null)
			{
				serializedReferences.AddDeserializedReference<TSet>(container);
			}
			container.Clear();
			int num = this.m_Stream->ReadNext<int>();
			for (int i = 0; i < num; i++)
			{
				container.Add(this.ReadValue<TValue>());
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00010BB8 File Offset: 0x0000EDB8
		unsafe void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary container)
		{
			SerializedReferences serializedReferences = this.m_SerializedReferences;
			if (serializedReferences != null)
			{
				serializedReferences.AddDeserializedReference<TDictionary>(container);
			}
			container.Clear();
			int num = this.m_Stream->ReadNext<int>();
			for (int i = 0; i < num; i++)
			{
				container.Add(this.ReadValue<TKey>(), this.ReadValue<TValue>());
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010C18 File Offset: 0x0000EE18
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			TValue value = property.GetValue(ref container);
			bool flag = property is IPropertyWrapper;
			this.ReadValue<TValue>(ref value, flag);
			if (!property.IsReadOnly)
			{
				property.SetValue(ref container, value);
				return;
			}
			string text;
			if (PropertyChecks.CheckReadOnlyPropertyForDeserialization<TContainer, TValue>(property, ref container, ref value, out text))
			{
				throw new SerializationException(text);
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00010C68 File Offset: 0x0000EE68
		private TValue ReadValue<TValue>()
		{
			TValue tvalue = default(TValue);
			this.ReadValue<TValue>(ref tvalue, false);
			return tvalue;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010C87 File Offset: 0x0000EE87
		internal void ReadValue<TValue>(ref TValue value, bool isRoot = false)
		{
			if (!isRoot || !this.m_DisableRootAdapters)
			{
				this.ReadValueWithAdapters<TValue>(ref value, this.m_Adapters.GetEnumerator(), isRoot);
				return;
			}
			this.ReadValueWithoutAdapters<TValue>(ref value, true);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00010CB0 File Offset: 0x0000EEB0
		internal void ReadValueWithAdapters<TValue>(ref TValue value, BinaryAdapterCollection.Enumerator adapters, bool isRoot)
		{
			while (adapters.MoveNext())
			{
				IBinaryAdapter binaryAdapter = adapters.Current;
				IBinaryAdapter<TValue> binaryAdapter2 = binaryAdapter as IBinaryAdapter<TValue>;
				if (binaryAdapter2 != null)
				{
					IBinaryAdapter<TValue> binaryAdapter3 = binaryAdapter2;
					BinaryDeserializationContext<TValue> binaryDeserializationContext = new BinaryDeserializationContext<TValue>(this, adapters, isRoot);
					value = binaryAdapter3.Deserialize(in binaryDeserializationContext);
					return;
				}
				IContravariantBinaryAdapter<TValue> contravariantBinaryAdapter = binaryAdapter as IContravariantBinaryAdapter<TValue>;
				if (contravariantBinaryAdapter != null)
				{
					value = (TValue)((object)contravariantBinaryAdapter.Deserialize(new BinaryDeserializationContext<TValue>(this, adapters, isRoot)));
					return;
				}
			}
			this.ReadValueWithoutAdapters<TValue>(ref value, isRoot);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00010D28 File Offset: 0x0000EF28
		internal unsafe void ReadValueWithoutAdapters<TValue>(ref TValue value, bool isRoot)
		{
			if (TypeTraits<TValue>.IsEnum)
			{
				BinarySerialization.ReadPrimitiveUnsafe<TValue>(this.m_Stream, ref value, Enum.GetUnderlyingType(typeof(TValue)));
				return;
			}
			byte b = 0;
			if (TypeTraits<TValue>.CanBeNull)
			{
				b = this.m_Stream->ReadNext<byte>();
				if (b == 1)
				{
					value = default(TValue);
					return;
				}
				if (b == 4)
				{
					int num = this.m_Stream->ReadNext<int>();
					if (this.m_SerializedReferences == null)
					{
						throw new Exception("Deserialization encountered a serialized object reference while running with DisableSerializedReferences.");
					}
					value = (TValue)((object)this.m_SerializedReferences.GetDeserializedReference(num));
					return;
				}
			}
			if (TypeTraits<TValue>.IsNullable)
			{
				Type underlyingType = Nullable.GetUnderlyingType(typeof(TValue));
				if (!TypeTraits.IsContainer(underlyingType))
				{
					BinarySerialization.ReadPrimitiveBoxed<TValue>(this.m_Stream, ref value, Nullable.GetUnderlyingType(typeof(TValue)));
					return;
				}
				this.m_SerializedTypeProviderSerializedType = underlyingType;
				DefaultTypeConstruction.Construct<TValue>(ref value, this);
				object obj = Convert.ChangeType(value, underlyingType);
				VisitReturnCode visitReturnCode;
				if (PropertyContainer.TryAccept<object>(this, ref obj, out visitReturnCode, default(VisitParameters)))
				{
					value = (TValue)((object)obj);
					return;
				}
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
			else
			{
				if ((!isRoot || !this.m_DisableRootAdapters) && b == 3)
				{
					foreach (IBinaryAdapter binaryAdapter in this.m_Adapters)
					{
						IContravariantBinaryAdapter<Object> contravariantBinaryAdapter = binaryAdapter as IContravariantBinaryAdapter<Object>;
						if (contravariantBinaryAdapter != null)
						{
							value = (TValue)((object)contravariantBinaryAdapter.Deserialize(new BinaryDeserializationContext<TValue>(this, default(BinaryAdapterCollection.Enumerator), isRoot)));
							return;
						}
					}
					return;
				}
				if (b == 2)
				{
					string text;
					(ref *this.m_Stream).ReadNextNBC(out text);
					if (string.IsNullOrEmpty(text))
					{
						throw new ArgumentException();
					}
					Type type = Type.GetType(text);
					if (null == type)
					{
						string text2;
						if (FormerNameAttribute.TryGetCurrentTypeName(text, out text2))
						{
							type = Type.GetType(text2);
						}
						if (null == type)
						{
							throw new ArgumentException();
						}
					}
					this.m_SerializedTypeProviderSerializedType = type;
				}
				else
				{
					this.m_SerializedTypeProviderSerializedType = (isRoot ? this.m_SerializedType : null);
				}
				DefaultTypeConstruction.Construct<TValue>(ref value, this);
				if (TypeTraits<TValue>.IsObject && !TypeTraits.IsContainer(value.GetType()))
				{
					BinarySerialization.ReadPrimitiveBoxed<TValue>(this.m_Stream, ref value, value.GetType());
					return;
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
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00011023 File Offset: 0x0000F223
		Type ISerializedTypeProvider.GetSerializedType()
		{
			return this.m_SerializedTypeProviderSerializedType;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001102C File Offset: 0x0000F22C
		unsafe int ISerializedTypeProvider.GetArrayLength()
		{
			int offset = this.m_Stream->Offset;
			int num = this.m_Stream->ReadNext<int>();
			this.m_Stream->Offset = offset;
			return num;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00002B85 File Offset: 0x00000D85
		object ISerializedTypeProvider.GetDefaultObject()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x040001FA RID: 506
		private unsafe UnsafeAppendBuffer.Reader* m_Stream;

		// Token: 0x040001FB RID: 507
		private Type m_SerializedType;

		// Token: 0x040001FC RID: 508
		private bool m_DisableRootAdapters;

		// Token: 0x040001FD RID: 509
		private BinaryAdapterCollection m_Adapters;

		// Token: 0x040001FE RID: 510
		private SerializedReferences m_SerializedReferences;

		// Token: 0x040001FF RID: 511
		private Type m_SerializedTypeProviderSerializedType;
	}
}
