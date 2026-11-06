using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class AssetReference : IKeyEvaluator
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00005B5E File Offset: 0x00003D5E
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00005B66 File Offset: 0x00003D66
		public AsyncOperationHandle OperationHandle
		{
			get
			{
				return this.m_Operation;
			}
			internal set
			{
				this.m_Operation = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00005B6F File Offset: 0x00003D6F
		public virtual object RuntimeKey
		{
			get
			{
				if (this.m_AssetGUID == null)
				{
					this.m_AssetGUID = string.Empty;
				}
				if (!string.IsNullOrEmpty(this.m_SubObjectName))
				{
					return string.Format("{0}[{1}]", this.m_AssetGUID, this.m_SubObjectName);
				}
				return this.m_AssetGUID;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00005BAE File Offset: 0x00003DAE
		public virtual string AssetGUID
		{
			get
			{
				return this.m_AssetGUID;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00005BB6 File Offset: 0x00003DB6
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00005BBE File Offset: 0x00003DBE
		public virtual string SubObjectName
		{
			get
			{
				return this.m_SubObjectName;
			}
			set
			{
				this.m_SubObjectName = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00005BC7 File Offset: 0x00003DC7
		internal virtual Type SubObjectType
		{
			get
			{
				if (!string.IsNullOrEmpty(this.m_SubObjectName) && this.m_SubObjectType != null)
				{
					return Type.GetType(this.m_SubObjectType);
				}
				return null;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005BEB File Offset: 0x00003DEB
		public bool IsValid()
		{
			return this.m_Operation.IsValid();
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00005BF8 File Offset: 0x00003DF8
		public bool IsDone
		{
			get
			{
				return this.m_Operation.IsDone;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005C05 File Offset: 0x00003E05
		public AssetReference()
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005C18 File Offset: 0x00003E18
		public AssetReference(string guid)
		{
			this.m_AssetGUID = guid;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00005C32 File Offset: 0x00003E32
		public virtual Object Asset
		{
			get
			{
				if (!this.m_Operation.IsValid())
				{
					return null;
				}
				return this.m_Operation.Result as Object;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005C53 File Offset: 0x00003E53
		public override string ToString()
		{
			return "[" + this.m_AssetGUID + "]";
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005C6C File Offset: 0x00003E6C
		private static AsyncOperationHandle<T> CreateFailedOperation<T>()
		{
			Addressables.InitializeAsync();
			return Addressables.ResourceManager.CreateCompletedOperation<T>(default(T), new Exception("Attempting to load an asset reference that has no asset assigned to it.").Message);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005CA1 File Offset: 0x00003EA1
		[Obsolete]
		public AsyncOperationHandle<TObject> LoadAsset<TObject>()
		{
			return this.LoadAssetAsync<TObject>();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005CA9 File Offset: 0x00003EA9
		[Obsolete]
		public AsyncOperationHandle<SceneInstance> LoadScene()
		{
			return this.LoadSceneAsync(LoadSceneMode.Single, true, 100);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005CB5 File Offset: 0x00003EB5
		[Obsolete]
		public AsyncOperationHandle<GameObject> Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return this.InstantiateAsync(position, rotation, parent);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005CC0 File Offset: 0x00003EC0
		[Obsolete]
		public AsyncOperationHandle<GameObject> Instantiate(Transform parent = null, bool instantiateInWorldSpace = false)
		{
			return this.InstantiateAsync(parent, instantiateInWorldSpace);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005CCC File Offset: 0x00003ECC
		public virtual AsyncOperationHandle<TObject> LoadAssetAsync<TObject>()
		{
			AsyncOperationHandle<TObject> asyncOperationHandle = default(AsyncOperationHandle<TObject>);
			if (this.m_Operation.IsValid())
			{
				Debug.LogError("Attempting to load AssetReference that has already been loaded. Handle is exposed through getter OperationHandle");
			}
			else
			{
				asyncOperationHandle = Addressables.LoadAssetAsync<TObject>(this.RuntimeKey);
				this.OperationHandle = asyncOperationHandle;
			}
			return asyncOperationHandle;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005D14 File Offset: 0x00003F14
		public virtual AsyncOperationHandle<SceneInstance> LoadSceneAsync(LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
		{
			AsyncOperationHandle<SceneInstance> asyncOperationHandle = default(AsyncOperationHandle<SceneInstance>);
			if (this.m_Operation.IsValid())
			{
				Debug.LogError("Attempting to load AssetReference Scene that has already been loaded. Handle is exposed through getter OperationHandle");
			}
			else
			{
				asyncOperationHandle = Addressables.LoadSceneAsync(this.RuntimeKey, loadMode, activateOnLoad, priority);
				this.OperationHandle = asyncOperationHandle;
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005D5E File Offset: 0x00003F5E
		public virtual AsyncOperationHandle<SceneInstance> UnLoadScene()
		{
			return Addressables.UnloadSceneAsync(this.m_Operation, true);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005D6C File Offset: 0x00003F6C
		public virtual AsyncOperationHandle<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return Addressables.InstantiateAsync(this.RuntimeKey, position, rotation, parent, true);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005D7D File Offset: 0x00003F7D
		public virtual AsyncOperationHandle<GameObject> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
		{
			return Addressables.InstantiateAsync(this.RuntimeKey, parent, instantiateInWorldSpace, true);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005D90 File Offset: 0x00003F90
		public virtual bool RuntimeKeyIsValid()
		{
			string text = this.RuntimeKey.ToString();
			int num = text.IndexOf('[');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			Guid guid;
			return Guid.TryParse(text, out guid);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005DC7 File Offset: 0x00003FC7
		public virtual void ReleaseAsset()
		{
			if (!this.m_Operation.IsValid())
			{
				Debug.LogWarning("Cannot release a null or unloaded asset.");
				return;
			}
			this.m_Operation.Release();
			this.m_Operation = default(AsyncOperationHandle);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005DF8 File Offset: 0x00003FF8
		public virtual void ReleaseInstance(GameObject obj)
		{
			Addressables.ReleaseInstance(obj);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005E01 File Offset: 0x00004001
		public virtual bool ValidateAsset(Object obj)
		{
			return true;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005E04 File Offset: 0x00004004
		public virtual bool ValidateAsset(string path)
		{
			return true;
		}

		// Token: 0x04000028 RID: 40
		[FormerlySerializedAs("m_assetGUID")]
		[SerializeField]
		protected internal string m_AssetGUID = "";

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private string m_SubObjectName;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private string m_SubObjectType;

		// Token: 0x0400002B RID: 43
		private AsyncOperationHandle m_Operation;
	}
}
