using System;
using System.Reflection;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000029 RID: 41
	public static class ComponentExtensions
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00003E7C File Offset: 0x0000207C
		public static void CopySerializedFields(this Component source, Component destination)
		{
			foreach (FieldInfo fieldInfo in source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<SerializeField>() != null)
				{
					fieldInfo.SetValue(destination, fieldInfo.GetValue(source));
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003EC7 File Offset: 0x000020C7
		public static bool TryGetComponentSafe<T>(this Component gameObject, out T component)
		{
			component = default(T);
			return !(gameObject == null) && gameObject.TryGetComponent<T>(out component);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003EE4 File Offset: 0x000020E4
		public static bool Destroy(this Component unityComponent)
		{
			bool flag;
			try
			{
				if (unityComponent.IsRequiredByAnotherComponents())
				{
					flag = false;
				}
				else
				{
					Object.Destroy(unityComponent);
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003F20 File Offset: 0x00002120
		public static bool RemoveComponentsDependingOn(this Component component)
		{
			bool flag = false;
			Type type = component.GetType();
			foreach (Component component2 in component.gameObject.GetComponents<Component>())
			{
				foreach (RequireComponent requireComponent in component2.GetType().GetCustomAttributes(typeof(RequireComponent), true))
				{
					if (requireComponent.m_Type0 == type || requireComponent.m_Type1 == type || requireComponent.m_Type2 == type)
					{
						component2.Destroy();
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003FC8 File Offset: 0x000021C8
		public static bool IsRequiredByAnotherComponents(this Component component)
		{
			Type type = component.GetType();
			Component[] components = component.gameObject.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				foreach (RequireComponent requireComponent in components[i].GetType().GetCustomAttributes(typeof(RequireComponent), true))
				{
					if (requireComponent.m_Type0 == type || requireComponent.m_Type1 == type || requireComponent.m_Type2 == type)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
