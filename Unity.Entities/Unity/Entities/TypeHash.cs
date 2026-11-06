using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x0200031E RID: 798
	public class TypeHash
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x00050A34 File Offset: 0x0004EC34
		public static ulong FNV1A64(string text)
		{
			ulong num = 14695981039346656037UL;
			foreach (char c in text)
			{
				num = 1099511628211UL * (num ^ (ulong)((byte)(c & 'ÿ')));
				num = 1099511628211UL * (num ^ (ulong)((byte)(c >> 8)));
			}
			return num;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00050A90 File Offset: 0x0004EC90
		public static ulong FNV1A64<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T text) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			ulong num = 14695981039346656037UL;
			for (int i = 0; i < text.Length; i++)
			{
				byte b = text[i];
				num = 1099511628211UL * (num ^ (ulong)(b & byte.MaxValue));
				num = 1099511628211UL * (num ^ (ulong)((byte)(b >> 8)));
			}
			return num;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00050AF8 File Offset: 0x0004ECF8
		public static ulong FNV1A64(int val)
		{
			ulong num = 14695981039346656037UL;
			num = (ulong)(((long)(val & 255) ^ (long)num) * 1099511628211L);
			num = (((ulong)((long)(val & 65280)) >> 8) ^ num) * 1099511628211UL;
			num = (((ulong)((long)(val & 16711680)) >> 16) ^ num) * 1099511628211UL;
			return (((ulong)((long)val & (long)((ulong)(-16777216))) >> 24) ^ num) * 1099511628211UL;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00050B6D File Offset: 0x0004ED6D
		public static ulong CombineFNV1A64(ulong hash, ulong value)
		{
			hash ^= value;
			hash *= 1099511628211UL;
			return hash;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00050B84 File Offset: 0x0004ED84
		private static ulong HashType(Type type, Dictionary<Type, ulong> cache)
		{
			ulong num;
			if (cache.TryGetValue(type, out num))
			{
				return num;
			}
			num = TypeHash.HashTypeName(type);
			if (!type.IsArray && !type.IsPointer && !type.IsPrimitive && !type.IsEnum)
			{
				Type unityEngineObjectType = TypeManager.UnityEngineObjectType;
				if (unityEngineObjectType == null || !unityEngineObjectType.IsAssignableFrom(type))
				{
					if (type.ContainsGenericParameters)
					{
						throw new ArgumentException(string.Format("'{0}' contains open generic parameters. Generic types must have all generic parameters specified to closed types when calculating stable type hashes", type));
					}
					num = TypeHash.CombineFNV1A64(num, TypeHash.HashVersionAttribute(type));
					FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					for (ulong num2 = 0UL; num2 < (ulong)((long)fields.Length); num2 += 1UL)
					{
						FieldInfo fieldInfo = fields[(int)(checked((IntPtr)num2))];
						if (!fieldInfo.IsStatic)
						{
							Type fieldType = fieldInfo.FieldType;
							ulong num3 = 0UL;
							if (fieldType.IsClass)
							{
								num3 = TypeHash.HashTypeName(fieldType);
							}
							else if (!cache.TryGetValue(fieldType, out num3))
							{
								num3 = TypeHash.HashType(fieldType, cache);
							}
							num3 = TypeHash.CombineFNV1A64(num3, num2);
							FieldOffsetAttribute customAttribute = fieldInfo.GetCustomAttribute<FieldOffsetAttribute>();
							if (customAttribute != null)
							{
								num3 = TypeHash.CombineFNV1A64(num3, (ulong)((long)customAttribute.Value));
							}
							num = TypeHash.CombineFNV1A64(num, num3);
						}
					}
					cache.Add(type, num);
					return num;
				}
			}
			cache.Add(type, num);
			return num;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00050CA0 File Offset: 0x0004EEA0
		private static ulong HashVersionAttribute(Type type)
		{
			int num = 0;
			TypeManager.TypeVersionAttribute customAttribute = type.GetCustomAttribute<TypeManager.TypeVersionAttribute>(true);
			if (customAttribute != null)
			{
				num = customAttribute.TypeVersion;
			}
			return TypeHash.FNV1A64(num);
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00050CC8 File Offset: 0x0004EEC8
		private static ulong HashNamespace(Type type)
		{
			ulong num = 14695981039346656037UL;
			if (type.IsNested)
			{
				num = TypeHash.CombineFNV1A64(num, TypeHash.HashNamespace(type.DeclaringType));
				num = TypeHash.CombineFNV1A64(num, TypeHash.FNV1A64(type.DeclaringType.Name));
			}
			else if (!string.IsNullOrEmpty(type.Namespace))
			{
				num = TypeHash.CombineFNV1A64(num, TypeHash.FNV1A64(type.Namespace));
			}
			return num;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00050D34 File Offset: 0x0004EF34
		private static ulong HashTypeName(Type type)
		{
			ulong num = TypeHash.HashNamespace(type);
			num = TypeHash.CombineFNV1A64(num, TypeHash.FNV1A64(type.Name));
			num = TypeHash.CombineFNV1A64(num, TypeHash.FNV1A64(type.Assembly.GetName().Name));
			foreach (Type type2 in type.GenericTypeArguments)
			{
				num = TypeHash.CombineFNV1A64(num, TypeHash.HashTypeName(type2));
			}
			return num;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00050D9D File Offset: 0x0004EF9D
		public static ulong CalculateStableTypeHash(Type type, IEnumerable<CustomAttributeData> customAttributes = null, Dictionary<Type, ulong> hashCache = null)
		{
			if (hashCache == null)
			{
				hashCache = new Dictionary<Type, ulong>();
			}
			return TypeHash.CalculateStableTypeHash(type, hashCache);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00050DB0 File Offset: 0x0004EFB0
		public static ulong CalculateStableTypeHash(Type type, Dictionary<Type, ulong> hashCache)
		{
			return TypeHash.HashType(type, hashCache);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00050DBC File Offset: 0x0004EFBC
		public static ulong CalculateMemoryOrdering(Type type, out bool hasCustomMemoryOrder, Dictionary<Type, ulong> hashCache = null)
		{
			hasCustomMemoryOrder = false;
			if (type == null || type.FullName == "Unity.Entities.Entity")
			{
				return 0UL;
			}
			IEnumerable<CustomAttributeData> customAttributes = type.CustomAttributes;
			if (customAttributes.Any<CustomAttributeData>())
			{
				CustomAttributeData customAttributeData = customAttributes.FirstOrDefault<CustomAttributeData>((CustomAttributeData ca) => ca.Constructor.DeclaringType == typeof(TypeManager.ForcedMemoryOrderingAttribute));
				if (customAttributeData != null)
				{
					hasCustomMemoryOrder = true;
					return (ulong)customAttributeData.ConstructorArguments.First<CustomAttributeTypedArgument>((CustomAttributeTypedArgument arg) => arg.ArgumentType.Name == "UInt64" || arg.ArgumentType.Name == "ulong").Value;
				}
			}
			return TypeHash.CalculateStableTypeHash(type, customAttributes, hashCache);
		}

		// Token: 0x040007C6 RID: 1990
		private const ulong kFNV1A64OffsetBasis = 14695981039346656037UL;

		// Token: 0x040007C7 RID: 1991
		private const ulong kFNV1A64Prime = 1099511628211UL;
	}
}
