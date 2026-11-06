using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000150 RID: 336
	public class MMPersistentHumbleSingleton<T> : MonoBehaviour where T : Component
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00023E0D File Offset: 0x0002200D
		public static bool HasInstance
		{
			get
			{
				return MMPersistentHumbleSingleton<T>._instance != null;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00023E1F File Offset: 0x0002201F
		public static T Current
		{
			get
			{
				return MMPersistentHumbleSingleton<T>._instance;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00023E28 File Offset: 0x00022028
		public static T Instance
		{
			get
			{
				if (MMPersistentHumbleSingleton<T>._instance == null)
				{
					MMPersistentHumbleSingleton<T>._instance = Object.FindObjectOfType<T>();
					if (MMPersistentHumbleSingleton<T>._instance == null)
					{
						MMPersistentHumbleSingleton<T>._instance = new GameObject
						{
							hideFlags = HideFlags.HideAndDontSave,
							name = typeof(T).Name + "_AutoCreated"
						}.AddComponent<T>();
					}
				}
				return MMPersistentHumbleSingleton<T>._instance;
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00023E9E File Offset: 0x0002209E
		protected virtual void Awake()
		{
			this.InitializeSingleton();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00023EA8 File Offset: 0x000220A8
		protected virtual void InitializeSingleton()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.InitializationTime = Time.time;
			Object.DontDestroyOnLoad(base.gameObject);
			foreach (T t in Object.FindObjectsOfType<T>())
			{
				if (t != this && t.GetComponent<MMPersistentHumbleSingleton<T>>().InitializationTime < this.InitializationTime)
				{
					Object.Destroy(t.gameObject);
				}
			}
			if (MMPersistentHumbleSingleton<T>._instance == null)
			{
				MMPersistentHumbleSingleton<T>._instance = this as T;
			}
		}

		// Token: 0x04000922 RID: 2338
		protected static T _instance;

		// Token: 0x04000923 RID: 2339
		[MMReadOnly]
		public float InitializationTime;
	}
}
