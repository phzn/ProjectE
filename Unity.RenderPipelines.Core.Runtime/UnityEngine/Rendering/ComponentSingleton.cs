using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000036 RID: 54
	public static class ComponentSingleton<TType> where TType : Component
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A564 File Offset: 0x00008764
		public static TType instance
		{
			get
			{
				if (ComponentSingleton<TType>.s_Instance == null)
				{
					GameObject gameObject = new GameObject("Default " + typeof(TType).Name);
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					Object.DontDestroyOnLoad(gameObject);
					gameObject.SetActive(false);
					ComponentSingleton<TType>.s_Instance = gameObject.AddComponent<TType>();
				}
				return ComponentSingleton<TType>.s_Instance;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A5C5 File Offset: 0x000087C5
		public static void Release()
		{
			if (ComponentSingleton<TType>.s_Instance != null)
			{
				CoreUtils.Destroy(ComponentSingleton<TType>.s_Instance.gameObject);
				ComponentSingleton<TType>.s_Instance = default(TType);
			}
		}

		// Token: 0x0400013C RID: 316
		private static TType s_Instance;
	}
}
