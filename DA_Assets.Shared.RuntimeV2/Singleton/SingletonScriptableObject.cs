using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace DA_Assets.Singleton
{
	// Token: 0x02000007 RID: 7
	public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		// Token: 0x06000012 RID: 18 RVA: 0x0000239D File Offset: 0x0000059D
		protected virtual void OnCreateInstance()
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023A0 File Offset: 0x000005A0
		public virtual async Task OnCreateInstanceAsync()
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023DB File Offset: 0x000005DB
		protected virtual void OnExitPlayMode()
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000023DD File Offset: 0x000005DD
		private static SingletonScriptableObject<T> _genericInstance
		{
			get
			{
				return SingletonScriptableObject<T>._instance as SingletonScriptableObject<T>;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000023EE File Offset: 0x000005EE
		public static T Instance
		{
			get
			{
				SingletonScriptableObject<T>.CreateInstance();
				return SingletonScriptableObject<T>._instance;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023FC File Offset: 0x000005FC
		public static async Task<T> GetInstanceAsync()
		{
			await SingletonScriptableObject<T>.InitializeAsync();
			return SingletonScriptableObject<T>._instance;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002437 File Offset: 0x00000637
		public static void CreateInstance()
		{
			if (SingletonScriptableObject<T>._instance == null)
			{
				SingletonScriptableObject<T>._instance = SingletonScriptableObject<T>.LoadInstance();
				SingletonScriptableObject<T>.SetupPlayModeExitEvent();
				SingletonScriptableObject<T> genericInstance = SingletonScriptableObject<T>._genericInstance;
				if (genericInstance == null)
				{
					return;
				}
				genericInstance.OnCreateInstance();
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000246C File Offset: 0x0000066C
		public static async Task InitializeAsync()
		{
			if (SingletonScriptableObject<T>._instance == null)
			{
				TaskAwaiter<T> taskAwaiter = SingletonScriptableObject<T>.LoadInstanceAsync().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<T> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<T>);
				}
				SingletonScriptableObject<T>._instance = taskAwaiter.GetResult();
				SingletonScriptableObject<T>.SetupPlayModeExitEvent();
				SingletonScriptableObject<T> genericInstance = SingletonScriptableObject<T>._genericInstance;
				await ((genericInstance != null) ? genericInstance.OnCreateInstanceAsync() : null);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024A8 File Offset: 0x000006A8
		private static string GetAssetPath()
		{
			object[] customAttributes = typeof(T).GetCustomAttributes(typeof(ResourcePathAttribute), true);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				return Path.Combine(Path.Combine((customAttributes[0] as ResourcePathAttribute).Path), typeof(T).Name);
			}
			return typeof(T).Name;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000250D File Offset: 0x0000070D
		private static T LoadInstance()
		{
			T t = Resources.Load<T>(SingletonScriptableObject<T>.GetAssetPath());
			if (t == null)
			{
				throw new NullReferenceException(SingletonScriptableObject<T>._objectNotFoundErrorStr);
			}
			return t;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002534 File Offset: 0x00000734
		private static async Task<T> LoadInstanceAsync()
		{
			string assetPath = SingletonScriptableObject<T>.GetAssetPath();
			ResourceRequest request = Resources.LoadAsync<T>(assetPath);
			while (!request.isDone)
			{
				await Task.Yield();
			}
			T t = request.asset as T;
			if (t == null)
			{
				throw new NullReferenceException(SingletonScriptableObject<T>._objectNotFoundErrorStr);
			}
			return t;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000256F File Offset: 0x0000076F
		private static void SetupPlayModeExitEvent()
		{
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002571 File Offset: 0x00000771
		private static string _missingAttributeErrorStr
		{
			get
			{
				return "Missing ResourcePathAttribute in " + typeof(T).Name;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000258C File Offset: 0x0000078C
		private static string _objectNotFoundErrorStr
		{
			get
			{
				return "ScriptableObject '" + typeof(T).Name + "' not found in the project.";
			}
		}

		// Token: 0x04000008 RID: 8
		private static T _instance;
	}
}
