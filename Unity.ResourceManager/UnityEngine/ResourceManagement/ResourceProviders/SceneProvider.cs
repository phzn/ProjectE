using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Profiling;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000044 RID: 68
	public class SceneProvider : ISceneProvider2, ISceneProvider
	{
		// Token: 0x060001AF RID: 431 RVA: 0x00006E47 File Offset: 0x00005047
		public AsyncOperationHandle<SceneInstance> ProvideScene(ResourceManager resourceManager, IResourceLocation location, LoadSceneMode loadSceneMode, bool activateOnLoad, int priority)
		{
			return this.ProvideScene(resourceManager, location, new LoadSceneParameters(loadSceneMode), activateOnLoad, priority);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006E5C File Offset: 0x0000505C
		public AsyncOperationHandle<SceneInstance> ProvideScene(ResourceManager resourceManager, IResourceLocation location, LoadSceneParameters loadSceneParameters, bool activateOnLoad, int priority)
		{
			AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle = default(AsyncOperationHandle<IList<AsyncOperationHandle>>);
			if (location.HasDependencies)
			{
				asyncOperationHandle = resourceManager.ProvideResourceGroupCached(location.Dependencies, location.DependencyHashCode, typeof(IAssetBundleResource), null, true);
			}
			SceneProvider.SceneOp sceneOp = new SceneProvider.SceneOp(resourceManager, this);
			sceneOp.Init(location, loadSceneParameters, activateOnLoad, priority, asyncOperationHandle);
			AsyncOperationHandle<SceneInstance> asyncOperationHandle2 = resourceManager.StartOperation<SceneInstance>(sceneOp, asyncOperationHandle);
			if (asyncOperationHandle.IsValid())
			{
				asyncOperationHandle.Release();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006ECA File Offset: 0x000050CA
		public AsyncOperationHandle<SceneInstance> ReleaseScene(ResourceManager resourceManager, AsyncOperationHandle<SceneInstance> sceneLoadHandle)
		{
			return ((ISceneProvider2)this).ReleaseScene(resourceManager, sceneLoadHandle, UnloadSceneOptions.None);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00006ED8 File Offset: 0x000050D8
		AsyncOperationHandle<SceneInstance> ISceneProvider2.ReleaseScene(ResourceManager resourceManager, AsyncOperationHandle<SceneInstance> sceneLoadHandle, UnloadSceneOptions unloadOptions)
		{
			SceneProvider.UnloadSceneOp unloadSceneOp = new SceneProvider.UnloadSceneOp();
			unloadSceneOp.Init(sceneLoadHandle, unloadOptions);
			return resourceManager.StartOperation<SceneInstance>(unloadSceneOp, sceneLoadHandle);
		}

		// Token: 0x0200007A RID: 122
		private class SceneOp : AsyncOperationBase<SceneInstance>, IUpdateReceiver
		{
			// Token: 0x06000374 RID: 884 RVA: 0x0000C5BF File Offset: 0x0000A7BF
			public SceneOp(ResourceManager rm, ISceneProvider2 provider)
			{
				this.m_ResourceManager = rm;
				this.m_provider = provider;
			}

			// Token: 0x06000375 RID: 885 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
			internal override DownloadStatus GetDownloadStatus(HashSet<object> visited)
			{
				if (!this.m_DepOp.IsValid())
				{
					return new DownloadStatus
					{
						IsDone = base.IsDone
					};
				}
				return this.m_DepOp.InternalGetDownloadStatus(visited);
			}

			// Token: 0x06000376 RID: 886 RVA: 0x0000C615 File Offset: 0x0000A815
			public void Init(IResourceLocation location, LoadSceneMode loadSceneMode, bool activateOnLoad, int priority, AsyncOperationHandle<IList<AsyncOperationHandle>> depOp)
			{
				this.Init(location, new LoadSceneParameters(loadSceneMode), activateOnLoad, priority, depOp);
			}

			// Token: 0x06000377 RID: 887 RVA: 0x0000C629 File Offset: 0x0000A829
			public void Init(IResourceLocation location, LoadSceneParameters loadSceneParameters, bool activateOnLoad, int priority, AsyncOperationHandle<IList<AsyncOperationHandle>> depOp)
			{
				this.m_DepOp = (depOp.IsValid() ? depOp.Acquire() : depOp);
				this.m_Location = location;
				this.m_LoadSceneParameters = loadSceneParameters;
				this.m_ActivateOnLoad = activateOnLoad;
				this.m_Priority = priority;
			}

			// Token: 0x06000378 RID: 888 RVA: 0x0000C664 File Offset: 0x0000A864
			protected override bool InvokeWaitForCompletion()
			{
				if (this.m_DepOp.IsValid() && !this.m_DepOp.IsDone)
				{
					this.m_DepOp.WaitForCompletion();
				}
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (!this.HasExecuted)
				{
					base.InvokeExecute();
				}
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				while (!base.IsDone)
				{
					((IUpdateReceiver)this).Update(Time.unscaledDeltaTime);
					if (this.m_Inst.m_Operation.progress == 0f && stopwatch.ElapsedMilliseconds > 5000L)
					{
						throw new Exception("Infinite loop detected within LoadSceneAsync.WaitForCompletion. For more information see the notes under the Scenes section of the \"Synchronous Addressables\" page of the Addressables documentation, or consider using asynchronous scene loading code.");
					}
					if (this.m_Inst.m_Operation.allowSceneActivation && Mathf.Approximately(this.m_Inst.m_Operation.progress, 0.9f))
					{
						base.Result = this.m_Inst;
						return true;
					}
				}
				return base.IsDone;
			}

			// Token: 0x06000379 RID: 889 RVA: 0x0000C74E File Offset: 0x0000A94E
			public override void GetDependencies(List<AsyncOperationHandle> deps)
			{
				if (this.m_DepOp.IsValid())
				{
					deps.Add(this.m_DepOp);
				}
			}

			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x0600037A RID: 890 RVA: 0x0000C76E File Offset: 0x0000A96E
			protected override string DebugName
			{
				get
				{
					return string.Format("Scene({0})", (this.m_Location == null) ? "Invalid" : AsyncOperationBase<SceneInstance>.ShortenPath(this.m_ResourceManager.TransformInternalId(this.m_Location), false));
				}
			}

			// Token: 0x0600037B RID: 891 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
			protected override void Execute()
			{
				bool flag = false;
				if (this.m_DepOp.IsValid())
				{
					foreach (AsyncOperationHandle asyncOperationHandle in this.m_DepOp.Result)
					{
						IAssetBundleResource assetBundleResource = asyncOperationHandle.Result as IAssetBundleResource;
						if (assetBundleResource != null && assetBundleResource.GetAssetBundle() != null)
						{
							flag = true;
						}
					}
				}
				if (!this.m_DepOp.IsValid() || this.m_DepOp.OperationException == null)
				{
					this.m_Inst = this.InternalLoadScene(this.m_Location, flag, this.m_LoadSceneParameters, this.m_ActivateOnLoad, this.m_Priority);
					((IUpdateReceiver)this).Update(0f);
				}
				else
				{
					base.Complete(this.m_Inst, false, this.m_DepOp.OperationException, true);
				}
				this.HasExecuted = true;
			}

			// Token: 0x0600037C RID: 892 RVA: 0x0000C888 File Offset: 0x0000AA88
			internal SceneInstance InternalLoadScene(IResourceLocation location, bool loadingFromBundle, LoadSceneParameters loadSceneParameters, bool activateOnLoad, int priority)
			{
				string text = this.m_ResourceManager.TransformInternalId(location);
				AsyncOperation asyncOperation = this.InternalLoad(text, loadingFromBundle, loadSceneParameters);
				asyncOperation.allowSceneActivation = activateOnLoad;
				asyncOperation.priority = priority;
				return new SceneInstance
				{
					m_Operation = asyncOperation,
					Scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1)
				};
			}

			// Token: 0x0600037D RID: 893 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
			private AsyncOperation InternalLoad(string path, bool loadingFromBundle, LoadSceneParameters loadSceneParameters)
			{
				ProfilerRuntime.AddSceneOperation(base.Handle, this.m_Location, ContentStatus.Loading);
				return SceneManager.LoadSceneAsync(path, loadSceneParameters);
			}

			// Token: 0x0600037E RID: 894 RVA: 0x0000C8FC File Offset: 0x0000AAFC
			protected override void Destroy()
			{
				if (this.m_Inst.Scene.IsValid())
				{
					this.m_provider.ReleaseScene(this.m_ResourceManager, base.Handle, UnloadSceneOptions.None).ReleaseHandleOnCompletion();
				}
				if (this.m_DepOp.IsValid())
				{
					this.m_DepOp.Release();
				}
				base.Destroy();
			}

			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x0600037F RID: 895 RVA: 0x0000C95C File Offset: 0x0000AB5C
			protected override float Progress
			{
				get
				{
					float num = 0.9f;
					float num2 = 0.1f;
					float num3 = 0f;
					if (this.m_Inst.m_Operation != null)
					{
						num3 += this.m_Inst.m_Operation.progress * num2;
					}
					if (!this.m_DepOp.IsDone)
					{
						num3 += this.m_DepOp.PercentComplete * num;
					}
					else
					{
						num3 += num;
					}
					return num3;
				}
			}

			// Token: 0x06000380 RID: 896 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
			void IUpdateReceiver.Update(float unscaledDeltaTime)
			{
				if (this.m_Inst.m_Operation != null && (this.m_Inst.m_Operation.isDone || (!this.m_Inst.m_Operation.allowSceneActivation && Mathf.Approximately(this.m_Inst.m_Operation.progress, 0.9f))))
				{
					this.m_ResourceManager.RemoveUpdateReciever(this);
					ProfilerRuntime.AddSceneOperation(base.Handle, this.m_Location, ContentStatus.Active);
					base.Complete(this.m_Inst, true, null);
				}
			}

			// Token: 0x0400016E RID: 366
			private bool m_ActivateOnLoad;

			// Token: 0x0400016F RID: 367
			private SceneInstance m_Inst;

			// Token: 0x04000170 RID: 368
			private IResourceLocation m_Location;

			// Token: 0x04000171 RID: 369
			private LoadSceneParameters m_LoadSceneParameters;

			// Token: 0x04000172 RID: 370
			private int m_Priority;

			// Token: 0x04000173 RID: 371
			private AsyncOperationHandle<IList<AsyncOperationHandle>> m_DepOp;

			// Token: 0x04000174 RID: 372
			private ResourceManager m_ResourceManager;

			// Token: 0x04000175 RID: 373
			private ISceneProvider2 m_provider;
		}

		// Token: 0x0200007B RID: 123
		private class UnloadSceneOp : AsyncOperationBase<SceneInstance>
		{
			// Token: 0x06000381 RID: 897 RVA: 0x0000CA4E File Offset: 0x0000AC4E
			public void Init(AsyncOperationHandle<SceneInstance> sceneLoadHandle, UnloadSceneOptions options)
			{
				if (sceneLoadHandle.IsValid())
				{
					this.m_sceneLoadHandle = sceneLoadHandle;
					this.m_Instance = this.m_sceneLoadHandle.Result;
				}
				this.m_UnloadOptions = options;
			}

			// Token: 0x06000382 RID: 898 RVA: 0x0000CA78 File Offset: 0x0000AC78
			protected override void Execute()
			{
				if (this.m_sceneLoadHandle.IsValid() && this.m_Instance.Scene.isLoaded)
				{
					AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(this.m_Instance.Scene, this.m_UnloadOptions);
					if (asyncOperation == null)
					{
						this.UnloadSceneCompleted(null);
					}
					else
					{
						asyncOperation.completed += this.UnloadSceneCompleted;
					}
				}
				else
				{
					this.UnloadSceneCompleted(null);
				}
				this.HasExecuted = true;
			}

			// Token: 0x06000383 RID: 899 RVA: 0x0000CAEC File Offset: 0x0000ACEC
			protected override bool InvokeWaitForCompletion()
			{
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (!this.HasExecuted)
				{
					base.InvokeExecute();
				}
				Debug.LogWarning("Cannot unload a Scene with WaitForCompletion. Scenes must be unloaded asynchronously.");
				return true;
			}

			// Token: 0x06000384 RID: 900 RVA: 0x0000CB20 File Offset: 0x0000AD20
			private void UnloadSceneCompleted(AsyncOperation obj)
			{
				base.Complete(this.m_Instance, true, "");
				if (this.m_sceneLoadHandle.IsValid() && this.m_sceneLoadHandle.ReferenceCount > 0)
				{
					if (this.m_sceneLoadHandle.ReferenceCount == 1)
					{
						ProfilerRuntime.SceneReleased(this.m_sceneLoadHandle);
					}
					this.m_sceneLoadHandle.Release();
				}
			}

			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x06000385 RID: 901 RVA: 0x0000CB7E File Offset: 0x0000AD7E
			protected override float Progress
			{
				get
				{
					return this.m_sceneLoadHandle.PercentComplete;
				}
			}

			// Token: 0x04000176 RID: 374
			private SceneInstance m_Instance;

			// Token: 0x04000177 RID: 375
			private AsyncOperationHandle<SceneInstance> m_sceneLoadHandle;

			// Token: 0x04000178 RID: 376
			private UnloadSceneOptions m_UnloadOptions;
		}
	}
}
