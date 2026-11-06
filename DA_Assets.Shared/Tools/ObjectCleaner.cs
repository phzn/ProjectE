using System;
using System.Reflection;

namespace DA_Assets.Tools
{
	// Token: 0x02000011 RID: 17
	public static class ObjectCleaner
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002928 File Offset: 0x00000B28
		public static void ClearByAttribute<T>(object obj) where T : Attribute
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type type = obj.GetType();
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (Attribute.IsDefined(fieldInfo, typeof(T)))
				{
					ObjectCleaner.SetMemberValue(fieldInfo, obj);
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (Attribute.IsDefined(propertyInfo, typeof(T)) && propertyInfo.CanWrite)
				{
					ObjectCleaner.SetMemberValue(propertyInfo, obj);
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000029C4 File Offset: 0x00000BC4
		private static void SetMemberValue(MemberInfo member, object obj)
		{
			FieldInfo field = member as FieldInfo;
			Type type;
			Action<object, object> action;
			if (field != null)
			{
				type = field.FieldType;
				action = delegate(object target, object value)
				{
					field.SetValue(target, value);
				};
			}
			else
			{
				PropertyInfo property = member as PropertyInfo;
				if (property == null)
				{
					throw new ArgumentException("Unsupported.");
				}
				type = property.PropertyType;
				action = delegate(object target, object value)
				{
					property.SetValue(target, value);
				};
			}
			object obj2 = (type.IsValueType ? Activator.CreateInstance(type) : null);
			action(obj, obj2);
		}
	}
}
