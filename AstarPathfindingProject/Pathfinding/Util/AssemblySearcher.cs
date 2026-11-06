using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pathfinding.Util
{
	// Token: 0x0200028F RID: 655
	internal static class AssemblySearcher
	{
		// Token: 0x06001053 RID: 4179 RVA: 0x00062F84 File Offset: 0x00061184
		public static List<Type> FindTypesInheritingFrom<T>()
		{
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				string name = assembly.GetName().Name;
				if (!name.StartsWith("Unity.") && !name.StartsWith("UnityEngine.") && !(name == "UnityEngine") && !name.StartsWith("UnityEditor.") && !(name == "UnityEditor") && !name.StartsWith("Mono.") && !name.StartsWith("System.") && !(name == "System") && !name.StartsWith("mscorlib") && !name.StartsWith("I18N") && !(name == "netstandard") && !(name == "nunit.framework"))
				{
					Type[] array = null;
					try
					{
						array = assembly.GetTypes();
					}
					catch
					{
						goto IL_0158;
					}
					foreach (Type type in array)
					{
						Type type2 = type.BaseType;
						while (type2 != null)
						{
							if (object.Equals(type2, typeof(T)))
							{
								list.Add(type);
								break;
							}
							type2 = type2.BaseType;
						}
					}
				}
				IL_0158:;
			}
			return list;
		}
	}
}
