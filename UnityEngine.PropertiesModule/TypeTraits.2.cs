using System;
using System.Reflection;
using UnityEngine;

namespace Unity.Properties
{
	// Token: 0x0200008B RID: 139
	public static class TypeTraits<T>
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000ADC6 File Offset: 0x00008FC6
		public static bool IsValueType { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000ADCD File Offset: 0x00008FCD
		public static bool IsPrimitive { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000ADD4 File Offset: 0x00008FD4
		public static bool IsInterface { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000ADDB File Offset: 0x00008FDB
		public static bool IsAbstract { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000ADE2 File Offset: 0x00008FE2
		public static bool IsArray { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000ADE9 File Offset: 0x00008FE9
		public static bool IsMultidimensionalArray { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public static bool IsEnum { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000ADF7 File Offset: 0x00008FF7
		public static bool IsEnumFlags { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000ADFE File Offset: 0x00008FFE
		public static bool IsNullable { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000AE05 File Offset: 0x00009005
		public static bool IsObject { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000AE0C File Offset: 0x0000900C
		public static bool IsString { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000AE13 File Offset: 0x00009013
		public static bool IsContainer { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000AE1A File Offset: 0x0000901A
		public static bool CanBeNull { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000AE21 File Offset: 0x00009021
		public static bool IsPrimitiveOrString { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000AE28 File Offset: 0x00009028
		public static bool IsAbstractOrInterface { get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000AE2F File Offset: 0x0000902F
		public static bool IsUnityObject { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000AE36 File Offset: 0x00009036
		public static bool IsLazyLoadReference { get; }

		// Token: 0x06000306 RID: 774 RVA: 0x0000AE40 File Offset: 0x00009040
		static TypeTraits()
		{
			Type typeFromHandle = typeof(T);
			TypeTraits<T>.IsValueType = typeFromHandle.IsValueType;
			TypeTraits<T>.IsPrimitive = typeFromHandle.IsPrimitive;
			TypeTraits<T>.IsInterface = typeFromHandle.IsInterface;
			TypeTraits<T>.IsAbstract = typeFromHandle.IsAbstract;
			TypeTraits<T>.IsArray = typeFromHandle.IsArray;
			TypeTraits<T>.IsEnum = typeFromHandle.IsEnum;
			TypeTraits<T>.IsEnumFlags = TypeTraits<T>.IsEnum && typeFromHandle.GetCustomAttribute<FlagsAttribute>() != null;
			TypeTraits<T>.IsNullable = Nullable.GetUnderlyingType(typeof(T)) != null;
			TypeTraits<T>.IsMultidimensionalArray = TypeTraits<T>.IsArray && typeof(T).GetArrayRank() != 1;
			TypeTraits<T>.IsObject = typeFromHandle == typeof(object);
			TypeTraits<T>.IsString = typeFromHandle == typeof(string);
			TypeTraits<T>.IsContainer = TypeTraits.IsContainer(typeFromHandle);
			TypeTraits<T>.CanBeNull = !TypeTraits<T>.IsValueType;
			TypeTraits<T>.IsPrimitiveOrString = TypeTraits<T>.IsPrimitive || TypeTraits<T>.IsString;
			TypeTraits<T>.IsAbstractOrInterface = TypeTraits<T>.IsAbstract || TypeTraits<T>.IsInterface;
			TypeTraits<T>.CanBeNull |= TypeTraits<T>.IsNullable;
			TypeTraits<T>.IsLazyLoadReference = typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(LazyLoadReference<>);
			TypeTraits<T>.IsUnityObject = typeof(Object).IsAssignableFrom(typeFromHandle);
		}
	}
}
