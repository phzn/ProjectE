using System;

namespace System.Reflection
{
	// Token: 0x020008D4 RID: 2260
	public static class TypeExtensions
	{
		// Token: 0x06004B4C RID: 19276 RVA: 0x000F0483 File Offset: 0x000EE683
		public static ConstructorInfo GetConstructor(Type type, Type[] types)
		{
			Requires.NotNull(type, "type");
			return type.GetConstructor(types);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x000F0497 File Offset: 0x000EE697
		public static ConstructorInfo[] GetConstructors(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetConstructors();
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x000F04AA File Offset: 0x000EE6AA
		public static ConstructorInfo[] GetConstructors(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetConstructors(bindingAttr);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x000F04BE File Offset: 0x000EE6BE
		public static MemberInfo[] GetDefaultMembers(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetDefaultMembers();
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x000F04D1 File Offset: 0x000EE6D1
		public static EventInfo GetEvent(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetEvent(name);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x000F04E5 File Offset: 0x000EE6E5
		public static EventInfo GetEvent(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x000F04FA File Offset: 0x000EE6FA
		public static EventInfo[] GetEvents(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetEvents();
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x000F050D File Offset: 0x000EE70D
		public static EventInfo[] GetEvents(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetEvents(bindingAttr);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x000F0521 File Offset: 0x000EE721
		public static FieldInfo GetField(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetField(name);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x000F0535 File Offset: 0x000EE735
		public static FieldInfo GetField(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetField(name, bindingAttr);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x000F054A File Offset: 0x000EE74A
		public static FieldInfo[] GetFields(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetFields();
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x000F055D File Offset: 0x000EE75D
		public static FieldInfo[] GetFields(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetFields(bindingAttr);
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x000F0571 File Offset: 0x000EE771
		public static Type[] GetGenericArguments(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetGenericArguments();
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x000F0584 File Offset: 0x000EE784
		public static Type[] GetInterfaces(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetInterfaces();
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x000F0597 File Offset: 0x000EE797
		public static MemberInfo[] GetMember(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetMember(name);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x000F05AB File Offset: 0x000EE7AB
		public static MemberInfo[] GetMember(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMember(name, bindingAttr);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x000F05C0 File Offset: 0x000EE7C0
		public static MemberInfo[] GetMembers(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetMembers();
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x000F05D3 File Offset: 0x000EE7D3
		public static MemberInfo[] GetMembers(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMembers(bindingAttr);
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x000F05E7 File Offset: 0x000EE7E7
		public static MethodInfo GetMethod(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetMethod(name);
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x000F05FB File Offset: 0x000EE7FB
		public static MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMethod(name, bindingAttr);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x000F0610 File Offset: 0x000EE810
		public static MethodInfo GetMethod(Type type, string name, Type[] types)
		{
			Requires.NotNull(type, "type");
			return type.GetMethod(name, types);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x000F0625 File Offset: 0x000EE825
		public static MethodInfo[] GetMethods(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetMethods();
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x000F0638 File Offset: 0x000EE838
		public static MethodInfo[] GetMethods(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMethods(bindingAttr);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x000F064C File Offset: 0x000EE84C
		public static Type GetNestedType(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x000F0661 File Offset: 0x000EE861
		public static Type[] GetNestedTypes(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x000F0675 File Offset: 0x000EE875
		public static PropertyInfo[] GetProperties(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetProperties();
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x000F0688 File Offset: 0x000EE888
		public static PropertyInfo[] GetProperties(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetProperties(bindingAttr);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x000F069C File Offset: 0x000EE89C
		public static PropertyInfo GetProperty(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x000F06B0 File Offset: 0x000EE8B0
		public static PropertyInfo GetProperty(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name, bindingAttr);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x000F06C5 File Offset: 0x000EE8C5
		public static PropertyInfo GetProperty(Type type, string name, Type returnType)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name, returnType);
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x000F06DA File Offset: 0x000EE8DA
		public static PropertyInfo GetProperty(Type type, string name, Type returnType, Type[] types)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name, returnType, types);
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x000F06F0 File Offset: 0x000EE8F0
		public static bool IsAssignableFrom(Type type, Type c)
		{
			Requires.NotNull(type, "type");
			return type.IsAssignableFrom(c);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x000F0704 File Offset: 0x000EE904
		public static bool IsInstanceOfType(Type type, object o)
		{
			Requires.NotNull(type, "type");
			return type.IsInstanceOfType(o);
		}
	}
}
