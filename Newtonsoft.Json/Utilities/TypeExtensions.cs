using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TypeExtensions
	{
		// Token: 0x060005E1 RID: 1505 RVA: 0x00018948 File Offset: 0x00016B48
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00018950 File Offset: 0x00016B50
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00018958 File Offset: 0x00016B58
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00018960 File Offset: 0x00016B60
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00018968 File Offset: 0x00016B68
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00018970 File Offset: 0x00016B70
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00018978 File Offset: 0x00016B78
		[return: Nullable(2)]
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00018980 File Offset: 0x00016B80
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00018988 File Offset: 0x00016B88
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00018990 File Offset: 0x00016B90
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00018998 File Offset: 0x00016B98
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000189A0 File Offset: 0x00016BA0
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000189A8 File Offset: 0x00016BA8
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000189B0 File Offset: 0x00016BB0
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000189B8 File Offset: 0x00016BB8
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000189C0 File Offset: 0x00016BC0
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces, [Nullable(2)] [NotNullWhen(true)] out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			if (searchInterfaces)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (string.Equals(interfaces[i].Name, fullTypeName, StringComparison.Ordinal))
					{
						match = type;
						return true;
					}
				}
			}
			match = null;
			return false;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00018A28 File Offset: 0x00016C28
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, searchInterfaces, out type2);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00018A40 File Offset: 0x00016C40
		public static bool ImplementInterface(this Type type, Type interfaceType)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (Type type3 in ((IEnumerable<Type>)type2.GetInterfaces()))
				{
					if (type3 == interfaceType || (type3 != null && type3.ImplementInterface(interfaceType)))
					{
						return true;
					}
				}
				type2 = type2.BaseType();
			}
			return false;
		}
	}
}
