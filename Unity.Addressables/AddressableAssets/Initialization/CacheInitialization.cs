using System;
using System.IO;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.AddressableAssets.Initialization
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class CacheInitialization : IInitializableObject
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x00008538 File Offset: 0x00006738
		public bool Initialize(string id, string dataStr)
		{
			CacheInitializationData cacheInitializationData = JsonUtility.FromJson<CacheInitializationData>(dataStr);
			if (cacheInitializationData != null)
			{
				Caching.compressionEnabled = cacheInitializationData.CompressionEnabled;
				Cache cache = Caching.currentCacheForWriting;
				if (!string.IsNullOrEmpty(cacheInitializationData.CacheDirectoryOverride))
				{
					string text = Addressables.ResolveInternalId(cacheInitializationData.CacheDirectoryOverride);
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					cache = Caching.GetCacheByPath(text);
					if (!cache.valid)
					{
						cache = Caching.AddCache(text);
					}
					Caching.currentCacheForWriting = cache;
				}
				if (cacheInitializationData.LimitCacheSize)
				{
					cache.maximumAvailableStorageSpace = cacheInitializationData.MaximumCacheSize;
				}
				else
				{
					cache.maximumAvailableStorageSpace = long.MaxValue;
				}
				cache.expirationDelay = cacheInitializationData.ExpirationDelay;
			}
			return true;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000085E0 File Offset: 0x000067E0
		public virtual AsyncOperationHandle<bool> InitializeAsync(ResourceManager rm, string id, string data)
		{
			CacheInitialization.CacheInitOp cacheInitOp = new CacheInitialization.CacheInitOp();
			cacheInitOp.Init(() => this.Initialize(id, data));
			return rm.StartOperation<bool>(cacheInitOp, default(AsyncOperationHandle));
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00008630 File Offset: 0x00006830
		public static string RootPath
		{
			get
			{
				return Path.GetDirectoryName(Caching.defaultCache.path);
			}
		}

		// Token: 0x02000058 RID: 88
		private class CacheInitOp : AsyncOperationBase<bool>, IUpdateReceiver
		{
			// Token: 0x06000276 RID: 630 RVA: 0x00009FD0 File Offset: 0x000081D0
			public void Init(Func<bool> callback)
			{
				this.m_Callback = callback;
			}

			// Token: 0x06000277 RID: 631 RVA: 0x00009FD9 File Offset: 0x000081D9
			protected override bool InvokeWaitForCompletion()
			{
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (!base.IsDone)
				{
					base.InvokeExecute();
				}
				return base.IsDone;
			}

			// Token: 0x06000278 RID: 632 RVA: 0x0000A008 File Offset: 0x00008208
			public void Update(float unscaledDeltaTime)
			{
				if (Caching.ready && this.m_UpdateRequired)
				{
					this.m_UpdateRequired = false;
					if (this.m_Callback != null)
					{
						base.Complete(this.m_Callback(), true, "");
						return;
					}
					base.Complete(true, true, "");
				}
			}

			// Token: 0x06000279 RID: 633 RVA: 0x0000A058 File Offset: 0x00008258
			protected override void Execute()
			{
				((IUpdateReceiver)this).Update(0f);
			}

			// Token: 0x04000130 RID: 304
			private Func<bool> m_Callback;

			// Token: 0x04000131 RID: 305
			private bool m_UpdateRequired = true;
		}
	}
}
