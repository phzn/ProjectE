using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DA_Assets.Extensions
{
	// Token: 0x02000034 RID: 52
	public static class ReflectionExtensions
	{
		// Token: 0x060000EA RID: 234 RVA: 0x0000553B File Offset: 0x0000373B
		public static IEnumerable<MemberInfo> GetPropertiesOrFields(this Type t, BindingFlags bf = BindingFlags.Instance | BindingFlags.Public)
		{
			return from mi in t.GetMembers(bf)
				where mi.MemberType == MemberTypes.Field || mi.MemberType == MemberTypes.Property
				select mi;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005568 File Offset: 0x00003768
		public static IEnumerable<string> GetPropertyOrFieldNames(this Type t)
		{
			return from mi in t.GetPropertiesOrFields(BindingFlags.Instance | BindingFlags.Public)
				select mi.Name;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005596 File Offset: 0x00003796
		public static MemberInfo GetPropertyOrField(this Type t, string memberName, BindingFlags bf = BindingFlags.Instance | BindingFlags.Public)
		{
			return (from mi in t.GetMember(memberName, bf)
				where mi.MemberType == MemberTypes.Field || mi.MemberType == MemberTypes.Property
				select mi).Single<MemberInfo>();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000055CC File Offset: 0x000037CC
		public static Type GetMemberType(this MemberInfo member)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.FieldType;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			EventInfo eventInfo = member as EventInfo;
			if (eventInfo == null)
			{
				throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", "member");
			}
			return eventInfo.EventHandlerType;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005620 File Offset: 0x00003820
		public static bool GetCanWrite(this MemberInfo member)
		{
			if (member is FieldInfo)
			{
				return true;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", "member");
			}
			return propertyInfo.CanWrite;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000565C File Offset: 0x0000385C
		public static bool GetCanRead(this MemberInfo member)
		{
			if (member is FieldInfo)
			{
				return true;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", "member");
			}
			return propertyInfo.CanRead;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005698 File Offset: 0x00003898
		public static object GetValue(this MemberInfo member, object srcObject)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(srcObject);
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(srcObject);
			}
			MethodInfo methodInfo = member as MethodInfo;
			if (methodInfo == null)
			{
				throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or MethodInfo", "member");
			}
			return methodInfo.Invoke(srcObject, null);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000056ED File Offset: 0x000038ED
		public static T GetValue<T>(this MemberInfo member, object srcObject)
		{
			return (T)((object)member.GetValue(srcObject));
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000056FC File Offset: 0x000038FC
		public static void SetValue(this MemberInfo member, object destObject, object value)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(destObject, value);
				return;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(destObject, value);
				return;
			}
			MethodInfo methodInfo = member as MethodInfo;
			if (methodInfo == null)
			{
				throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or MethodInfo", "member");
			}
			methodInfo.Invoke(destObject, new object[] { value });
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000575D File Offset: 0x0000395D
		public static void SetValue<T>(this MemberInfo member, object destObject, T value)
		{
			member.SetValue(destObject, value);
		}
	}
}
