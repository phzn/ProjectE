using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace Microsoft.Internal
{
	// Token: 0x0200000D RID: 13
	internal static class ReflectionServices
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002904 File Offset: 0x00000B04
		public static Assembly Assembly(this MemberInfo member)
		{
			Type type = member as Type;
			if (type != null)
			{
				return type.Assembly;
			}
			return member.DeclaringType.Assembly;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002933 File Offset: 0x00000B33
		public static bool IsVisible(this ConstructorInfo constructor)
		{
			return constructor.DeclaringType.IsVisible && constructor.IsPublic;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000294A File Offset: 0x00000B4A
		public static bool IsVisible(this FieldInfo field)
		{
			return field.DeclaringType.IsVisible && field.IsPublic;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002964 File Offset: 0x00000B64
		public static bool IsVisible(this MethodInfo method)
		{
			if (!method.DeclaringType.IsVisible)
			{
				return false;
			}
			if (!method.IsPublic)
			{
				return false;
			}
			if (method.IsGenericMethod)
			{
				Type[] genericArguments = method.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!genericArguments[i].IsVisible)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000029B4 File Offset: 0x00000BB4
		public static string GetDisplayName(Type declaringType, string name)
		{
			Assumes.NotNull<Type>(declaringType);
			return declaringType.GetDisplayName() + "." + name;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000029D0 File Offset: 0x00000BD0
		public static string GetDisplayName(this MemberInfo member)
		{
			Assumes.NotNull<MemberInfo>(member);
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
			{
				return AttributedModelServices.GetTypeIdentity((Type)member);
			}
			return ReflectionServices.GetDisplayName(member.DeclaringType, member.Name);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A14 File Offset: 0x00000C14
		internal static bool TryGetGenericInterfaceType(Type instanceType, Type targetOpenInterfaceType, out Type targetClosedInterfaceType)
		{
			Assumes.IsTrue(targetOpenInterfaceType.IsInterface);
			Assumes.IsTrue(targetOpenInterfaceType.IsGenericTypeDefinition);
			Assumes.IsTrue(!instanceType.IsGenericTypeDefinition);
			if (instanceType.IsInterface && instanceType.IsGenericType && instanceType.UnderlyingSystemType.GetGenericTypeDefinition() == targetOpenInterfaceType.UnderlyingSystemType)
			{
				targetClosedInterfaceType = instanceType;
				return true;
			}
			try
			{
				Type @interface = instanceType.GetInterface(targetOpenInterfaceType.Name, false);
				if (@interface != null && @interface.UnderlyingSystemType.GetGenericTypeDefinition() == targetOpenInterfaceType.UnderlyingSystemType)
				{
					targetClosedInterfaceType = @interface;
					return true;
				}
			}
			catch (AmbiguousMatchException)
			{
			}
			targetClosedInterfaceType = null;
			return false;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002AC4 File Offset: 0x00000CC4
		internal static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
		{
			return type.GetInterfaces().Concat<Type>(new Type[] { type }).SelectMany<Type, PropertyInfo>((Type itf) => itf.GetProperties());
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B00 File Offset: 0x00000D00
		internal static IEnumerable<MethodInfo> GetAllMethods(this Type type)
		{
			IEnumerable<MethodInfo> declaredMethods = type.GetDeclaredMethods();
			Type baseType = type.BaseType;
			if (baseType.UnderlyingSystemType != typeof(object))
			{
				return declaredMethods.Concat<MethodInfo>(baseType.GetAllMethods());
			}
			return declaredMethods;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B40 File Offset: 0x00000D40
		private static IEnumerable<MethodInfo> GetDeclaredMethods(this Type type)
		{
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				yield return methodInfo;
			}
			MethodInfo[] array = null;
			yield break;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B50 File Offset: 0x00000D50
		public static IEnumerable<FieldInfo> GetAllFields(this Type type)
		{
			IEnumerable<FieldInfo> declaredFields = type.GetDeclaredFields();
			Type baseType = type.BaseType;
			if (baseType.UnderlyingSystemType != typeof(object))
			{
				return declaredFields.Concat<FieldInfo>(baseType.GetAllFields());
			}
			return declaredFields;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002B90 File Offset: 0x00000D90
		private static IEnumerable<FieldInfo> GetDeclaredFields(this Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				yield return fieldInfo;
			}
			FieldInfo[] array = null;
			yield break;
		}
	}
}
