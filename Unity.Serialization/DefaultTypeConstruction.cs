using System;
using Unity.Properties;

namespace Unity.Serialization
{
	// Token: 0x0200000F RID: 15
	internal static class DefaultTypeConstruction
	{
		// Token: 0x0600001B RID: 27 RVA: 0x0000230C File Offset: 0x0000050C
		internal static void Construct<TValue>(ref TValue value, ISerializedTypeProvider provider)
		{
			if (TypeTraits<TValue>.IsValueType && (!TypeTraits<TValue>.IsNullable || !TypeTraits.IsContainer(Nullable.GetUnderlyingType(typeof(TValue)))))
			{
				return;
			}
			Type serializedType = provider.GetSerializedType();
			if (null != serializedType)
			{
				if (!typeof(TValue).IsAssignableFrom(serializedType))
				{
					throw new ArgumentException(string.Format("Type mismatch. DeclaredType=[{0}] SerializedType=[{1}]", typeof(TValue), serializedType));
				}
				DefaultTypeConstruction.ConstructFromSerializedType<TValue>(ref value, serializedType, provider);
				return;
			}
			else
			{
				if (TypeTraits<TValue>.IsObject && value == null)
				{
					value = (TValue)((object)provider.GetDefaultObject());
					return;
				}
				if (TypeTraits<TValue>.IsAbstractOrInterface)
				{
					throw new ArgumentException();
				}
				DefaultTypeConstruction.ConstructFromDeclaredType<TValue>(ref value, provider);
				return;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023C0 File Offset: 0x000005C0
		private static void ConstructFromSerializedType<TValue>(ref TValue value, Type type, ISerializedTypeProvider provider)
		{
			if (type.IsArray)
			{
				int arrayLength = provider.GetArrayLength();
				value = TypeUtility.InstantiateArray<TValue>(type, arrayLength);
				return;
			}
			if (value != null && value.GetType() == type)
			{
				return;
			}
			value = TypeUtility.Instantiate<TValue>(type);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002418 File Offset: 0x00000618
		private static void ConstructFromDeclaredType<TValue>(ref TValue value, ISerializedTypeProvider provider)
		{
			if (typeof(TValue).IsArray)
			{
				int arrayLength = provider.GetArrayLength();
				if (value != null)
				{
					Array array = value as Array;
					if (array != null && array.Length == arrayLength)
					{
						return;
					}
				}
				value = TypeUtility.InstantiateArray<TValue>(arrayLength);
				return;
			}
			if (value == null)
			{
				value = TypeUtility.Instantiate<TValue>();
			}
		}
	}
}
