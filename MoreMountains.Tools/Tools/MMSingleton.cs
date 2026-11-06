using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000154 RID: 340
	public class MMSingleton<T> : MonoBehaviour where T : Component
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00024279 File Offset: 0x00022479
		public static bool HasInstance
		{
			get
			{
				return MMSingleton<T>._instance != null;
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002428C File Offset: 0x0002248C
		public static T TryGetInstance()
		{
			if (!MMSingleton<T>.HasInstance)
			{
				return default(T);
			}
			return MMSingleton<T>._instance;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x000242AF File Offset: 0x000224AF
		public static T Current
		{
			get
			{
				return MMSingleton<T>._instance;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000242B8 File Offset: 0x000224B8
		public static T Instance
		{
			get
			{
				if (MMSingleton<T>._instance == null)
				{
					MMSingleton<T>._instance = Object.FindObjectOfType<T>();
					if (MMSingleton<T>._instance == null)
					{
						MMSingleton<T>._instance = new GameObject
						{
							name = typeof(T).Name + "_AutoCreated"
						}.AddComponent<T>();
					}
				}
				return MMSingleton<T>._instance;
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00024326 File Offset: 0x00022526
		protected virtual void Awake()
		{
			this.InitializeSingleton();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002432E File Offset: 0x0002252E
		protected virtual void InitializeSingleton()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			MMSingleton<T>._instance = this as T;
		}

		// Token: 0x0400092B RID: 2347
		protected static T _instance;
	}
}
