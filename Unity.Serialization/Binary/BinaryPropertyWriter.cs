using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections.LowLevel.Unsafe.NotBurstCompatible;
using Unity.Properties;
using UnityEngine;

namespace Unity.Serialization.Binary
{
	// Token: 0x020000A8 RID: 168
	internal class BinaryPropertyWriter : BinaryPropertyVisitor, IPropertyBagVisitor, IListPropertyBagVisitor, ISetPropertyBagVisitor, IDictionaryPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0001109B File Offset: 0x0000F29B
		internal unsafe UnsafeAppendBuffer* Writer
		{
			get
			{
				return this.m_Stream;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000110A3 File Offset: 0x0000F2A3
		public unsafe void SetStream(UnsafeAppendBuffer* stream)
		{
			this.m_Stream = stream;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000110AC File Offset: 0x0000F2AC
		public void SetSerializedType(Type type)
		{
			this.m_SerializedType = type;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000110B5 File Offset: 0x0000F2B5
		public void SetDisableRootAdapters(bool disableRootAdapters)
		{
			this.m_DisableRootAdapters = disableRootAdapters;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000110BE File Offset: 0x0000F2BE
		public void SetGlobalAdapters(List<IBinaryAdapter> adapters)
		{
			this.m_Adapters.Global = adapters;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000110CC File Offset: 0x0000F2CC
		public void SetUserDefinedAdapters(List<IBinaryAdapter> adapters)
		{
			this.m_Adapters.UserDefined = adapters;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000110DA File Offset: 0x0000F2DA
		public void SetSerializedReferences(SerializedReferences serializedReferences)
		{
			this.m_SerializedReferences = serializedReferences;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000110E3 File Offset: 0x0000F2E3
		public BinaryPropertyWriter()
		{
			this.m_Adapters.Internal = this;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000110F8 File Offset: 0x0000F2F8
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
			{
				if (!PropertyChecks.IsPropertyExcludedFromSerialization(property))
				{
					property.Accept(this, ref container);
				}
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00011158 File Offset: 0x0000F358
		unsafe void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList container)
		{
			this.m_Stream->Add<int>(container.Count);
			for (int i = 0; i < container.Count; i++)
			{
				this.WriteValue<TElement>(container[i], false);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000111A8 File Offset: 0x0000F3A8
		unsafe void ISetPropertyBagVisitor.Visit<TSet, TValue>(ISetPropertyBag<TSet, TValue> properties, ref TSet container)
		{
			this.m_Stream->Add<int>(container.Count);
			foreach (TValue tvalue in container)
			{
				this.WriteValue<TValue>(tvalue, false);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00011210 File Offset: 0x0000F410
		unsafe void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary container)
		{
			this.m_Stream->Add<int>(container.Count);
			foreach (KeyValuePair<TKey, TValue> keyValuePair in container)
			{
				this.WriteValue<TKey>(keyValuePair.Key, false);
				this.WriteValue<TValue>(keyValuePair.Value, false);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001128C File Offset: 0x0000F48C
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			bool flag = property is IPropertyWrapper;
			this.WriteValue<TValue>(property.GetValue(ref container), flag);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000112B1 File Offset: 0x0000F4B1
		internal void WriteValue<TValue>(TValue value, bool isRoot = false)
		{
			if (!isRoot || !this.m_DisableRootAdapters)
			{
				this.WriteValueWithAdapters<TValue>(value, this.m_Adapters.GetEnumerator(), isRoot);
				return;
			}
			this.WriteValueWithoutAdapters<TValue>(value, true);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000112DC File Offset: 0x0000F4DC
		internal void WriteValueWithAdapters<TValue>(TValue value, BinaryAdapterCollection.Enumerator adapters, bool isRoot)
		{
			while (adapters.MoveNext())
			{
				IBinaryAdapter binaryAdapter = adapters.Current;
				IBinaryAdapter<TValue> binaryAdapter2 = binaryAdapter as IBinaryAdapter<TValue>;
				if (binaryAdapter2 != null)
				{
					IBinaryAdapter<TValue> binaryAdapter3 = binaryAdapter2;
					BinarySerializationContext<TValue> binarySerializationContext = new BinarySerializationContext<TValue>(this, adapters, value, isRoot);
					binaryAdapter3.Serialize(in binarySerializationContext, value);
					return;
				}
				IContravariantBinaryAdapter<TValue> contravariantBinaryAdapter = binaryAdapter as IContravariantBinaryAdapter<TValue>;
				if (contravariantBinaryAdapter != null)
				{
					contravariantBinaryAdapter.Serialize(new BinarySerializationContext<TValue>(this, adapters, value, isRoot), value);
					return;
				}
			}
			this.WriteValueWithoutAdapters<TValue>(value, isRoot);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00011344 File Offset: 0x0000F544
		internal unsafe void WriteValueWithoutAdapters<TValue>(TValue value, bool isRoot)
		{
			if (TypeTraits<TValue>.IsEnum)
			{
				BinarySerialization.WritePrimitiveUnsafe<TValue>(this.m_Stream, ref value, Enum.GetUnderlyingType(typeof(TValue)));
				return;
			}
			if (TypeTraits<TValue>.CanBeNull && value == null)
			{
				this.m_Stream->Add<byte>(1);
				return;
			}
			if (TypeTraits<TValue>.IsMultidimensionalArray)
			{
				this.m_Stream->Add<byte>(1);
				return;
			}
			if (TypeTraits<TValue>.IsNullable)
			{
				this.m_Stream->Add<byte>(0);
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
					BinarySerialization.WritePrimitiveBoxed(this.m_Stream, value, underlyingType);
				}
				return;
			}
			if (!TypeTraits<TValue>.IsValueType)
			{
				if (!isRoot || !this.m_DisableRootAdapters)
				{
					Object @object = value as Object;
					if (@object != null)
					{
						this.m_Stream->Add<byte>(3);
						foreach (IBinaryAdapter binaryAdapter in this.m_Adapters)
						{
							IContravariantBinaryAdapter<Object> contravariantBinaryAdapter = binaryAdapter as IContravariantBinaryAdapter<Object>;
							if (contravariantBinaryAdapter != null)
							{
								contravariantBinaryAdapter.Serialize(new BinarySerializationContext<TValue>(this, default(BinaryAdapterCollection.Enumerator), value, isRoot), @object);
								return;
							}
						}
						return;
					}
				}
				if (this.m_SerializedReferences != null && !value.GetType().IsValueType)
				{
					int num = this.m_SerializedReferences.AddSerializedReference(value);
					if (!this.m_SerializedReferences.SetSerialized(value))
					{
						this.m_Stream->Add<byte>(4);
						this.m_Stream->Add<int>(num);
						return;
					}
				}
				bool flag = isRoot && null != this.m_SerializedType;
				if (typeof(TValue) != value.GetType() && !flag)
				{
					this.m_Stream->Add<byte>(2);
					(ref *this.m_Stream).AddNBC(value.GetType().AssemblyQualifiedName);
				}
				else
				{
					this.m_Stream->Add<byte>(0);
				}
			}
			if (TypeTraits<TValue>.IsObject && !TypeTraits.IsContainer(value.GetType()))
			{
				BinarySerialization.WritePrimitiveBoxed(this.m_Stream, value, value.GetType());
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

		// Token: 0x04000207 RID: 519
		private unsafe UnsafeAppendBuffer* m_Stream;

		// Token: 0x04000208 RID: 520
		private Type m_SerializedType;

		// Token: 0x04000209 RID: 521
		private bool m_DisableRootAdapters;

		// Token: 0x0400020A RID: 522
		private BinaryAdapterCollection m_Adapters;

		// Token: 0x0400020B RID: 523
		private SerializedReferences m_SerializedReferences;
	}
}
