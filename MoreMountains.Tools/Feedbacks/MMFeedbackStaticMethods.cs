using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001BB RID: 443
	public static class MMFeedbackStaticMethods
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x0002F983 File Offset: 0x0002DB83
		public static Component GetComponentNoAlloc(this GameObject @this, Type componentType)
		{
			@this.GetComponents(componentType, MMFeedbackStaticMethods.m_ComponentCache);
			Component component = ((MMFeedbackStaticMethods.m_ComponentCache.Count > 0) ? MMFeedbackStaticMethods.m_ComponentCache[0] : null);
			MMFeedbackStaticMethods.m_ComponentCache.Clear();
			return component;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002F9B8 File Offset: 0x0002DBB8
		public static Type MMFGetTypeByName(string name)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				foreach (Type type in assemblies[i].GetTypes())
				{
					if (type.Name == name)
					{
						return type;
					}
				}
			}
			return null;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002FA10 File Offset: 0x0002DC10
		public static T MMFGetComponentNoAlloc<T>(this GameObject @this) where T : Component
		{
			@this.GetComponents(typeof(T), MMFeedbackStaticMethods.m_ComponentCache);
			Component component = ((MMFeedbackStaticMethods.m_ComponentCache.Count > 0) ? MMFeedbackStaticMethods.m_ComponentCache[0] : null);
			MMFeedbackStaticMethods.m_ComponentCache.Clear();
			return component as T;
		}

		// Token: 0x04000C95 RID: 3221
		private static List<Component> m_ComponentCache = new List<Component>();
	}
}
