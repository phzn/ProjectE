using System;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000FE RID: 254
	public static class MMHelpers
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x0001B1D4 File Offset: 0x000193D4
		public static T CopyComponent<T>(T original, GameObject destination) where T : Component
		{
			Type type = original.GetType();
			T t = destination.GetComponent(type) as T;
			if (!t)
			{
				t = destination.AddComponent(type) as T;
			}
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				if (!fieldInfo.IsStatic)
				{
					fieldInfo.SetValue(t, fieldInfo.GetValue(original));
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				if (propertyInfo.CanWrite && propertyInfo.CanWrite && !(propertyInfo.Name == "name"))
				{
					propertyInfo.SetValue(t, propertyInfo.GetValue(original, null), null);
				}
			}
			return t;
		}
	}
}
