using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Linq
{
	// Token: 0x02000097 RID: 151
	internal static class TypeHelper
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		internal static Type FindGenericType(Type definition, Type type)
		{
			bool? flag = null;
			while (type != null && type != typeof(object))
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
				{
					return type;
				}
				if (flag == null)
				{
					flag = new bool?(definition.IsInterface);
				}
				if (flag.GetValueOrDefault())
				{
					foreach (Type type2 in type.GetInterfaces())
					{
						Type type3 = TypeHelper.FindGenericType(definition, type2);
						if (type3 != null)
						{
							return type3;
						}
					}
				}
				type = type.BaseType;
			}
			return null;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000F878 File Offset: 0x0000DA78
		internal static IEnumerable<MethodInfo> GetStaticMethods(this Type type)
		{
			return from m in type.GetRuntimeMethods()
				where m.IsStatic
				select m;
		}
	}
}
