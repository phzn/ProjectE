using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unity.Serialization
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class FormerNameAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002191 File Offset: 0x00000391
		public string OldName { get; }

		// Token: 0x0600000C RID: 12 RVA: 0x00002199 File Offset: 0x00000399
		public FormerNameAttribute(string oldName)
		{
			this.OldName = oldName;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021A8 File Offset: 0x000003A8
		private static void RegisterFormerlySerializedAsTypes()
		{
			if (FormerNameAttribute.m_Registered)
			{
				return;
			}
			FormerNameAttribute.m_Registered = true;
			foreach (Type type in from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany<Assembly, Type>((Assembly a) => a.GetTypes())
				where !t.IsAbstract && !t.IsGenericType
				select t)
			{
				foreach (FormerNameAttribute formerNameAttribute in type.GetCustomAttributes<FormerNameAttribute>())
				{
					FormerNameAttribute.s_FormerlySerializedAsToCurrentName.Add(formerNameAttribute.OldName, string.Format("{0}, {1}", type, type.Assembly.GetName().Name));
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022A8 File Offset: 0x000004A8
		public static bool TryGetCurrentTypeName(string oldName, out string currentName)
		{
			FormerNameAttribute.RegisterFormerlySerializedAsTypes();
			return FormerNameAttribute.s_FormerlySerializedAsToCurrentName.TryGetValue(oldName, out currentName);
		}

		// Token: 0x04000009 RID: 9
		private static readonly Dictionary<string, string> s_FormerlySerializedAsToCurrentName = new Dictionary<string, string>();

		// Token: 0x0400000A RID: 10
		private static bool m_Registered;
	}
}
