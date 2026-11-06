using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E8 RID: 232
	public static class GameObjectExtensions
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x00016CF3 File Offset: 0x00014EF3
		public static Component MMGetComponentNoAlloc(this GameObject @this, Type componentType)
		{
			@this.GetComponents(componentType, GameObjectExtensions.m_ComponentCache);
			Component component = ((GameObjectExtensions.m_ComponentCache.Count > 0) ? GameObjectExtensions.m_ComponentCache[0] : null);
			GameObjectExtensions.m_ComponentCache.Clear();
			return component;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00016D28 File Offset: 0x00014F28
		public static T MMGetComponentNoAlloc<T>(this GameObject @this) where T : Component
		{
			@this.GetComponents(typeof(T), GameObjectExtensions.m_ComponentCache);
			Component component = ((GameObjectExtensions.m_ComponentCache.Count > 0) ? GameObjectExtensions.m_ComponentCache[0] : null);
			GameObjectExtensions.m_ComponentCache.Clear();
			return component as T;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00016D7C File Offset: 0x00014F7C
		public static T MMGetComponentAroundOrAdd<T>(this GameObject @this) where T : Component
		{
			T t = @this.GetComponentInChildren<T>(true);
			if (t == null)
			{
				t = @this.GetComponentInParent<T>();
			}
			if (t == null)
			{
				t = @this.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00016DBC File Offset: 0x00014FBC
		public static T MMGetOrAddComponent<T>(this GameObject @this) where T : Component
		{
			T t = @this.GetComponent<T>();
			if (t == null)
			{
				t = @this.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00016DE8 File Offset: 0x00014FE8
		[return: TupleElementNames(new string[] { "newComponent", "createdNew" })]
		public static ValueTuple<T, bool> MMFindOrCreateObjectOfType<T>(this GameObject @this, string newObjectName, Transform parent, bool forceNewCreation = false) where T : Component
		{
			T t = (T)((object)Object.FindObjectOfType(typeof(T)));
			if (t == null || forceNewCreation)
			{
				GameObject gameObject = new GameObject(newObjectName);
				gameObject.transform.SetParent(parent);
				return new ValueTuple<T, bool>(gameObject.AddComponent<T>(), true);
			}
			return new ValueTuple<T, bool>(t, false);
		}

		// Token: 0x0400063A RID: 1594
		private static List<Component> m_ComponentCache = new List<Component>();
	}
}
