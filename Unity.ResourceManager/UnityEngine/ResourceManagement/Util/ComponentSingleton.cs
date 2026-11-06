using System;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000015 RID: 21
	[ExecuteInEditMode]
	public abstract class ComponentSingleton<T> : MonoBehaviour where T : ComponentSingleton<T>
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004492 File Offset: 0x00002692
		public static bool Exists
		{
			get
			{
				return ComponentSingleton<T>.s_Instance != null;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000044A4 File Offset: 0x000026A4
		public static T Instance
		{
			get
			{
				if (ComponentSingleton<T>.s_Instance == null)
				{
					T t;
					if ((t = ComponentSingleton<T>.FindInstance()) == null)
					{
						t = ComponentSingleton<T>.CreateNewSingleton();
					}
					ComponentSingleton<T>.s_Instance = t;
				}
				return ComponentSingleton<T>.s_Instance;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000044D5 File Offset: 0x000026D5
		private static T FindInstance()
		{
			return Object.FindObjectOfType<T>();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000044DC File Offset: 0x000026DC
		protected virtual string GetGameObjectName()
		{
			return typeof(T).Name;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000044F0 File Offset: 0x000026F0
		private static T CreateNewSingleton()
		{
			GameObject gameObject = new GameObject();
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
				gameObject.hideFlags = HideFlags.DontSave;
			}
			else
			{
				gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			T t = gameObject.AddComponent<T>();
			gameObject.name = t.GetGameObjectName();
			return t;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000453C File Offset: 0x0000273C
		private void Awake()
		{
			if (ComponentSingleton<T>.s_Instance != null && ComponentSingleton<T>.s_Instance != this)
			{
				Object.DestroyImmediate(base.gameObject);
				return;
			}
			ComponentSingleton<T>.s_Instance = this as T;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004589 File Offset: 0x00002789
		public static void DestroySingleton()
		{
			if (ComponentSingleton<T>.Exists)
			{
				Object.DestroyImmediate(ComponentSingleton<T>.Instance.gameObject);
				ComponentSingleton<T>.s_Instance = default(T);
			}
		}

		// Token: 0x04000044 RID: 68
		private static T s_Instance;
	}
}
