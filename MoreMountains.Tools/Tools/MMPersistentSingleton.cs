using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000151 RID: 337
	public class MMPersistentSingleton<T> : MonoBehaviour where T : Component
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00023F4F File Offset: 0x0002214F
		public static bool HasInstance
		{
			get
			{
				return MMPersistentSingleton<T>._instance != null;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00023F61 File Offset: 0x00022161
		public static T Current
		{
			get
			{
				return MMPersistentSingleton<T>._instance;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00023F68 File Offset: 0x00022168
		public static T Instance
		{
			get
			{
				if (MMPersistentSingleton<T>._instance == null)
				{
					MMPersistentSingleton<T>._instance = Object.FindObjectOfType<T>();
					if (MMPersistentSingleton<T>._instance == null)
					{
						MMPersistentSingleton<T>._instance = new GameObject
						{
							name = typeof(T).Name + "_AutoCreated"
						}.AddComponent<T>();
					}
				}
				return MMPersistentSingleton<T>._instance;
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00023FD6 File Offset: 0x000221D6
		protected virtual void Awake()
		{
			this.InitializeSingleton();
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00023FE0 File Offset: 0x000221E0
		protected virtual void InitializeSingleton()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.AutomaticallyUnparentOnAwake)
			{
				base.transform.SetParent(null);
			}
			if (MMPersistentSingleton<T>._instance == null)
			{
				MMPersistentSingleton<T>._instance = this as T;
				Object.DontDestroyOnLoad(base.transform.gameObject);
				this._enabled = true;
				return;
			}
			if (this != MMPersistentSingleton<T>._instance)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04000924 RID: 2340
		[Header("Persistent Singleton")]
		[Tooltip("if this is true, this singleton will auto detach if it finds itself parented on awake")]
		public bool AutomaticallyUnparentOnAwake = true;

		// Token: 0x04000925 RID: 2341
		protected static T _instance;

		// Token: 0x04000926 RID: 2342
		protected bool _enabled;
	}
}
