using System;
using UnityEngine;

namespace DA_Assets.Singleton
{
	// Token: 0x02000006 RID: 6
	public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002321 File Offset: 0x00000521
		public static T Instance
		{
			get
			{
				if (SingletonBehaviour<T>._instance == null)
				{
					SingletonBehaviour<T>._instance = Object.FindObjectOfType<T>(true);
				}
				return SingletonBehaviour<T>._instance;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002348 File Offset: 0x00000548
		protected virtual void Awake()
		{
			if (SingletonBehaviour<T>._instance == null)
			{
				SingletonBehaviour<T>._instance = this as T;
				return;
			}
			if (SingletonBehaviour<T>._instance != this)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04000007 RID: 7
		private static T _instance;
	}
}
