using System;
using Unity.Properties;

namespace Unity.Serialization
{
	// Token: 0x02000010 RID: 16
	internal static class PropertyChecks
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002494 File Offset: 0x00000694
		internal static string GetReadOnlyValueTypeErrorMessage(Type type, string propertyName)
		{
			return string.Concat(new string[] { "Failed to deserialize Property=[", propertyName, "] for Type=[", type.FullName, "]. The property is readonly value type and can not be assigned. The property needs to be writable or ignored by serialization using [DontSerializeAttribute]." });
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024C6 File Offset: 0x000006C6
		internal static string GetReadOnlyReferenceTypeErrorMessage(Type type, string propertyName)
		{
			return string.Concat(new string[] { "Failed to deserialize Property=[", propertyName, "] for Type=[", type.FullName, "]. The property is readonly and the reference can not be assigned. The property needs to be writable, default constructed, or ignored by serialization using [DontSerializeAttribute]." });
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000024F8 File Offset: 0x000006F8
		internal static string GetReadOnlyReferenceTypeWithInvalidTypeErrorMessage(Type type, string propertyName)
		{
			return string.Concat(new string[] { "Failed to deserialize Property=[", propertyName, "] for Type=[", type.FullName, "]. The default constructed instance does not match the deserialized type. The property needs to be writable, default constructed with the correct type, or ignored by serialization using [DontSerializeAttribute]" });
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000252C File Offset: 0x0000072C
		public static bool CheckReadOnlyPropertyForDeserialization<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value, out string error)
		{
			if (!property.IsReadOnly)
			{
				error = string.Empty;
				return false;
			}
			if (typeof(TValue).IsValueType)
			{
				error = PropertyChecks.GetReadOnlyValueTypeErrorMessage(typeof(TContainer), property.Name);
				return true;
			}
			TValue value2 = property.GetValue(ref container);
			if (value2 != value)
			{
				error = ((value2 == null) ? PropertyChecks.GetReadOnlyReferenceTypeErrorMessage(typeof(TContainer), property.Name) : PropertyChecks.GetReadOnlyReferenceTypeWithInvalidTypeErrorMessage(typeof(TContainer), property.Name));
				return true;
			}
			error = string.Empty;
			return false;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000025CF File Offset: 0x000007CF
		internal static bool IsPropertyExcludedFromSerialization(IProperty property)
		{
			return property.HasAttribute<NonSerializedAttribute>() || property.HasAttribute<DontSerializeAttribute>();
		}
	}
}
