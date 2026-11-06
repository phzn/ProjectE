using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Runtime
{
	// Token: 0x02000036 RID: 54
	internal static class TypeHelper
	{
		// Token: 0x06000193 RID: 403 RVA: 0x000069E5 File Offset: 0x00004BE5
		public static bool AreTypesCompatible(object source, Type destinationType)
		{
			if (source == null)
			{
				return !destinationType.IsValueType || TypeHelper.IsNullableType(destinationType);
			}
			return TypeHelper.AreTypesCompatible(source.GetType(), destinationType);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006A07 File Offset: 0x00004C07
		public static bool AreTypesCompatible(Type sourceType, Type destinationType)
		{
			return sourceType == destinationType || TypeHelper.IsImplicitNumericConversion(sourceType, destinationType) || TypeHelper.IsImplicitReferenceConversion(sourceType, destinationType) || TypeHelper.IsImplicitBoxingConversion(sourceType, destinationType) || TypeHelper.IsImplicitNullableConversion(sourceType, destinationType);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006A33 File Offset: 0x00004C33
		public static bool AreReferenceTypesCompatible(Type sourceType, Type destinationType)
		{
			return sourceType == destinationType || TypeHelper.IsImplicitReferenceConversion(sourceType, destinationType);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006A42 File Offset: 0x00004C42
		public static IEnumerable<Type> GetCompatibleTypes(IEnumerable<Type> enumerable, Type targetType)
		{
			foreach (Type type in enumerable)
			{
				if (TypeHelper.AreTypesCompatible(type, targetType))
				{
					yield return type;
				}
			}
			IEnumerator<Type> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006A5C File Offset: 0x00004C5C
		public static bool ContainsCompatibleType(IEnumerable<Type> enumerable, Type targetType)
		{
			using (IEnumerator<Type> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TypeHelper.AreTypesCompatible(enumerator.Current, targetType))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006AAC File Offset: 0x00004CAC
		public static T Convert<T>(object source)
		{
			if (source is T)
			{
				return (T)((object)source);
			}
			if (source == null)
			{
				if (typeof(T).IsValueType && !TypeHelper.IsNullableType(typeof(T)))
				{
					throw Fx.Exception.AsError(new InvalidCastException(InternalSR.CannotConvertObject(source, typeof(T))));
				}
				return default(T);
			}
			else
			{
				T t;
				if (TypeHelper.TryNumericConversion<T>(source, out t))
				{
					return t;
				}
				throw Fx.Exception.AsError(new InvalidCastException(InternalSR.CannotConvertObject(source, typeof(T))));
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006B44 File Offset: 0x00004D44
		public static IEnumerable<Type> GetImplementedTypes(Type type)
		{
			Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
			TypeHelper.GetImplementedTypesHelper(type, dictionary);
			return dictionary.Keys;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006B64 File Offset: 0x00004D64
		private static void GetImplementedTypesHelper(Type type, Dictionary<Type, object> typesEncountered)
		{
			if (typesEncountered.ContainsKey(type))
			{
				return;
			}
			typesEncountered.Add(type, type);
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				TypeHelper.GetImplementedTypesHelper(interfaces[i], typesEncountered);
			}
			Type type2 = type.BaseType;
			while (type2 != null && type2 != TypeHelper.ObjectType)
			{
				TypeHelper.GetImplementedTypesHelper(type2, typesEncountered);
				type2 = type2.BaseType;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006BD0 File Offset: 0x00004DD0
		private static bool IsImplicitNumericConversion(Type source, Type destination)
		{
			TypeCode typeCode = Type.GetTypeCode(source);
			TypeCode typeCode2 = Type.GetTypeCode(destination);
			switch (typeCode)
			{
			case TypeCode.Char:
				return typeCode2 - TypeCode.UInt16 <= 7;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.Byte:
				return typeCode2 - TypeCode.Int16 <= 8;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.UInt16:
				return typeCode2 - TypeCode.Int32 <= 6;
			case TypeCode.Int32:
				return typeCode2 == TypeCode.Int64 || typeCode2 - TypeCode.Single <= 2;
			case TypeCode.UInt32:
				return typeCode2 - TypeCode.UInt32 <= 5;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return typeCode2 - TypeCode.Single <= 2;
			case TypeCode.Single:
				return typeCode2 == TypeCode.Double;
			default:
				return false;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006CC9 File Offset: 0x00004EC9
		private static bool IsImplicitReferenceConversion(Type sourceType, Type destinationType)
		{
			return destinationType.IsAssignableFrom(sourceType);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006CD4 File Offset: 0x00004ED4
		private static bool IsImplicitBoxingConversion(Type sourceType, Type destinationType)
		{
			return (sourceType.IsValueType && (destinationType == TypeHelper.ObjectType || destinationType == typeof(ValueType))) || (sourceType.IsEnum && destinationType == typeof(Enum));
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006D27 File Offset: 0x00004F27
		private static bool IsImplicitNullableConversion(Type sourceType, Type destinationType)
		{
			if (!TypeHelper.IsNullableType(destinationType))
			{
				return false;
			}
			destinationType = destinationType.GetGenericArguments()[0];
			if (TypeHelper.IsNullableType(sourceType))
			{
				sourceType = sourceType.GetGenericArguments()[0];
			}
			return TypeHelper.AreTypesCompatible(sourceType, destinationType);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006D56 File Offset: 0x00004F56
		private static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == TypeHelper.NullableType;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006D74 File Offset: 0x00004F74
		private static bool TryNumericConversion<T>(object source, out T result)
		{
			TypeCode typeCode = Type.GetTypeCode(source.GetType());
			TypeCode typeCode2 = Type.GetTypeCode(typeof(T));
			switch (typeCode)
			{
			case TypeCode.Char:
			{
				char c = (char)source;
				switch (typeCode2)
				{
				case TypeCode.UInt16:
					result = (T)((object)((ushort)c));
					return true;
				case TypeCode.Int32:
					result = (T)((object)((int)c));
					return true;
				case TypeCode.UInt32:
					result = (T)((object)((uint)c));
					return true;
				case TypeCode.Int64:
					result = (T)((object)((long)((ulong)c)));
					return true;
				case TypeCode.UInt64:
					result = (T)((object)((ulong)c));
					return true;
				case TypeCode.Single:
					result = (T)((object)((float)c));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)c));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)c);
					return true;
				}
				break;
			}
			case TypeCode.SByte:
			{
				sbyte b = (sbyte)source;
				switch (typeCode2)
				{
				case TypeCode.Int16:
					result = (T)((object)((short)b));
					return true;
				case TypeCode.Int32:
					result = (T)((object)((int)b));
					return true;
				case TypeCode.Int64:
					result = (T)((object)((long)b));
					return true;
				case TypeCode.Single:
					result = (T)((object)((float)b));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)b));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)b);
					return true;
				}
				break;
			}
			case TypeCode.Byte:
			{
				byte b2 = (byte)source;
				switch (typeCode2)
				{
				case TypeCode.Int16:
					result = (T)((object)((short)b2));
					return true;
				case TypeCode.UInt16:
					result = (T)((object)((ushort)b2));
					return true;
				case TypeCode.Int32:
					result = (T)((object)((int)b2));
					return true;
				case TypeCode.UInt32:
					result = (T)((object)((uint)b2));
					return true;
				case TypeCode.Int64:
					result = (T)((object)((long)((ulong)b2)));
					return true;
				case TypeCode.UInt64:
					result = (T)((object)((ulong)b2));
					return true;
				case TypeCode.Single:
					result = (T)((object)((float)b2));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)b2));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)b2);
					return true;
				}
				break;
			}
			case TypeCode.Int16:
			{
				short num = (short)source;
				switch (typeCode2)
				{
				case TypeCode.Int32:
					result = (T)((object)((int)num));
					return true;
				case TypeCode.Int64:
					result = (T)((object)((long)num));
					return true;
				case TypeCode.Single:
					result = (T)((object)((float)num));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)num));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)num);
					return true;
				}
				break;
			}
			case TypeCode.UInt16:
			{
				ushort num2 = (ushort)source;
				switch (typeCode2)
				{
				case TypeCode.Int32:
					result = (T)((object)((int)num2));
					return true;
				case TypeCode.UInt32:
					result = (T)((object)((uint)num2));
					return true;
				case TypeCode.Int64:
					result = (T)((object)((long)((ulong)num2)));
					return true;
				case TypeCode.UInt64:
					result = (T)((object)((ulong)num2));
					return true;
				case TypeCode.Single:
					result = (T)((object)((float)num2));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)num2));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)num2);
					return true;
				}
				break;
			}
			case TypeCode.Int32:
			{
				int num3 = (int)source;
				switch (typeCode2)
				{
				case TypeCode.Int64:
					result = (T)((object)((long)num3));
					return true;
				case TypeCode.Single:
					result = (T)((object)((float)num3));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)num3));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)num3);
					return true;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num4 = (uint)source;
				switch (typeCode2)
				{
				case TypeCode.UInt32:
					result = (T)((object)num4);
					return true;
				case TypeCode.Int64:
					result = (T)((object)((long)((ulong)num4)));
					return true;
				case TypeCode.UInt64:
					result = (T)((object)((ulong)num4));
					return true;
				case TypeCode.Single:
					result = (T)((object)num4);
					return true;
				case TypeCode.Double:
					result = (T)((object)num4);
					return true;
				case TypeCode.Decimal:
					result = (T)((object)num4);
					return true;
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num5 = (long)source;
				switch (typeCode2)
				{
				case TypeCode.Single:
					result = (T)((object)((float)num5));
					return true;
				case TypeCode.Double:
					result = (T)((object)((double)num5));
					return true;
				case TypeCode.Decimal:
					result = (T)((object)num5);
					return true;
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num6 = (ulong)source;
				switch (typeCode2)
				{
				case TypeCode.Single:
					result = (T)((object)num6);
					return true;
				case TypeCode.Double:
					result = (T)((object)num6);
					return true;
				case TypeCode.Decimal:
					result = (T)((object)num6);
					return true;
				}
				break;
			}
			case TypeCode.Single:
				if (typeCode2 == TypeCode.Double)
				{
					result = (T)((object)((double)((float)source)));
					return true;
				}
				break;
			}
			result = default(T);
			return false;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000073D8 File Offset: 0x000055D8
		public static object GetDefaultValueForType(Type type)
		{
			if (!type.IsValueType)
			{
				return null;
			}
			if (type.IsEnum)
			{
				Array values = Enum.GetValues(type);
				if (values.Length > 0)
				{
					return values.GetValue(0);
				}
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007415 File Offset: 0x00005615
		public static bool IsNullableValueType(Type type)
		{
			return type.IsValueType && TypeHelper.IsNullableType(type);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007427 File Offset: 0x00005627
		public static bool IsNonNullableValueType(Type type)
		{
			return type.IsValueType && !type.IsGenericType && type != TypeHelper.StringType;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007448 File Offset: 0x00005648
		public static bool ShouldFilterProperty(PropertyDescriptor property, Attribute[] attributes)
		{
			if (attributes == null || attributes.Length == 0)
			{
				return false;
			}
			foreach (Attribute attribute in attributes)
			{
				Attribute attribute2 = property.Attributes[attribute.GetType()];
				if (attribute2 == null)
				{
					if (!attribute.IsDefaultAttribute())
					{
						return true;
					}
				}
				else if (!attribute.Match(attribute2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000FD RID: 253
		public static readonly Type ArrayType = typeof(Array);

		// Token: 0x040000FE RID: 254
		public static readonly Type BoolType = typeof(bool);

		// Token: 0x040000FF RID: 255
		public static readonly Type GenericCollectionType = typeof(ICollection<>);

		// Token: 0x04000100 RID: 256
		public static readonly Type ByteType = typeof(byte);

		// Token: 0x04000101 RID: 257
		public static readonly Type SByteType = typeof(sbyte);

		// Token: 0x04000102 RID: 258
		public static readonly Type CharType = typeof(char);

		// Token: 0x04000103 RID: 259
		public static readonly Type ShortType = typeof(short);

		// Token: 0x04000104 RID: 260
		public static readonly Type UShortType = typeof(ushort);

		// Token: 0x04000105 RID: 261
		public static readonly Type IntType = typeof(int);

		// Token: 0x04000106 RID: 262
		public static readonly Type UIntType = typeof(uint);

		// Token: 0x04000107 RID: 263
		public static readonly Type LongType = typeof(long);

		// Token: 0x04000108 RID: 264
		public static readonly Type ULongType = typeof(ulong);

		// Token: 0x04000109 RID: 265
		public static readonly Type FloatType = typeof(float);

		// Token: 0x0400010A RID: 266
		public static readonly Type DoubleType = typeof(double);

		// Token: 0x0400010B RID: 267
		public static readonly Type DecimalType = typeof(decimal);

		// Token: 0x0400010C RID: 268
		public static readonly Type ExceptionType = typeof(Exception);

		// Token: 0x0400010D RID: 269
		public static readonly Type NullableType = typeof(Nullable<>);

		// Token: 0x0400010E RID: 270
		public static readonly Type ObjectType = typeof(object);

		// Token: 0x0400010F RID: 271
		public static readonly Type StringType = typeof(string);

		// Token: 0x04000110 RID: 272
		public static readonly Type TypeType = typeof(Type);

		// Token: 0x04000111 RID: 273
		public static readonly Type VoidType = typeof(void);
	}
}
