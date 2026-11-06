using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.Profiling;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000035 RID: 53
	[DisplayName("Assets from Bundles Provider")]
	public class BundledAssetProvider : ResourceProviderBase
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000684F File Offset: 0x00004A4F
		public override void Provide(ProvideHandle provideHandle)
		{
			new BundledAssetProvider.InternalOp().Start(provideHandle);
		}

		// Token: 0x02000076 RID: 118
		internal class InternalOp
		{
			// Token: 0x0600035C RID: 860 RVA: 0x0000BDE4 File Offset: 0x00009FE4
			internal static T LoadBundleFromDependecies<T>(IList<object> results) where T : class, IAssetBundleResource
			{
				if (results == null || results.Count == 0)
				{
					return default(T);
				}
				IAssetBundleResource assetBundleResource = null;
				bool flag = true;
				for (int i = 0; i < results.Count; i++)
				{
					IAssetBundleResource assetBundleResource2 = results[i] as IAssetBundleResource;
					if (assetBundleResource2 != null)
					{
						assetBundleResource2.GetAssetBundle();
						if (flag)
						{
							assetBundleResource = assetBundleResource2;
						}
						flag = false;
					}
				}
				return assetBundleResource as T;
			}

			// Token: 0x0600035D RID: 861 RVA: 0x0000BE48 File Offset: 0x0000A048
			public void Start(ProvideHandle provideHandle)
			{
				provideHandle.SetProgressCallback(new Func<float>(this.ProgressCallback));
				provideHandle.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionHandler));
				this.subObjectName = null;
				this.m_ProvideHandle = provideHandle;
				this.m_RequestOperation = null;
				List<object> list = new List<object>();
				this.m_ProvideHandle.GetDependencies(list);
				IAssetBundleResource assetBundleResource = BundledAssetProvider.InternalOp.LoadBundleFromDependecies<IAssetBundleResource>(list);
				if (assetBundleResource == null)
				{
					AssetBundle assetBundle = null;
					bool flag = false;
					string text = "Unable to load dependent bundle from location ";
					IResourceLocation location = this.m_ProvideHandle.Location;
					this.m_ProvideHandle.Complete<AssetBundle>(assetBundle, flag, new Exception(text + ((location != null) ? location.ToString() : null)));
					return;
				}
				this.m_AssetBundle = assetBundleResource.GetAssetBundle();
				if (this.m_AssetBundle == null)
				{
					AssetBundle assetBundle2 = null;
					bool flag2 = false;
					string text2 = "Unable to load dependent bundle from location ";
					IResourceLocation location2 = this.m_ProvideHandle.Location;
					this.m_ProvideHandle.Complete<AssetBundle>(assetBundle2, flag2, new Exception(text2 + ((location2 != null) ? location2.ToString() : null)));
					return;
				}
				AssetBundleResource assetBundleResource2 = assetBundleResource as AssetBundleResource;
				if (assetBundleResource2 != null)
				{
					this.m_PreloadRequest = assetBundleResource2.GetAssetPreloadRequest();
				}
				if (this.m_PreloadRequest == null || this.m_PreloadRequest.isDone)
				{
					this.BeginAssetLoad();
					return;
				}
				this.m_PreloadRequest.completed += delegate(AsyncOperation operation)
				{
					this.BeginAssetLoad();
				};
			}

			// Token: 0x0600035E RID: 862 RVA: 0x0000BF78 File Offset: 0x0000A178
			private void BeginAssetLoad()
			{
				if (this.m_AssetBundle == null)
				{
					AssetBundle assetBundle = null;
					bool flag = false;
					string text = "Unable to load dependent bundle from location ";
					IResourceLocation location = this.m_ProvideHandle.Location;
					this.m_ProvideHandle.Complete<AssetBundle>(assetBundle, flag, new Exception(text + ((location != null) ? location.ToString() : null)));
					return;
				}
				string text2 = this.m_ProvideHandle.ResourceManager.TransformInternalId(this.m_ProvideHandle.Location);
				string text3;
				string text4;
				if (this.m_ProvideHandle.Type.IsArray)
				{
					this.m_RequestOperation = this.m_AssetBundle.LoadAssetWithSubAssetsAsync(text2, this.m_ProvideHandle.Type.GetElementType());
				}
				else if (this.m_ProvideHandle.Type.IsGenericType && typeof(IList<>) == this.m_ProvideHandle.Type.GetGenericTypeDefinition())
				{
					this.m_RequestOperation = this.m_AssetBundle.LoadAssetWithSubAssetsAsync(text2, this.m_ProvideHandle.Type.GetGenericArguments()[0]);
				}
				else if (ResourceManagerConfig.ExtractKeyAndSubKey(text2, out text3, out text4))
				{
					this.subObjectName = text4;
					this.m_RequestOperation = this.m_AssetBundle.LoadAssetWithSubAssetsAsync(text3, this.m_ProvideHandle.Type);
				}
				else
				{
					this.m_RequestOperation = this.m_AssetBundle.LoadAssetAsync(text2, this.m_ProvideHandle.Type);
				}
				if (this.m_RequestOperation != null)
				{
					if (this.m_RequestOperation.isDone)
					{
						this.ActionComplete(this.m_RequestOperation);
						return;
					}
					if (Profiler.enabled && this.m_ProvideHandle.IsValid)
					{
						ProfilerRuntime.AddAssetOperation(this.m_ProvideHandle, ContentStatus.Loading);
					}
					this.m_RequestOperation.completed += this.ActionComplete;
				}
			}

			// Token: 0x0600035F RID: 863 RVA: 0x0000C120 File Offset: 0x0000A320
			private bool WaitForCompletionHandler()
			{
				if (this.m_PreloadRequest != null && !this.m_PreloadRequest.isDone)
				{
					return this.m_PreloadRequest.asset == null;
				}
				return this.m_Result != null || (this.m_RequestOperation != null && (this.m_RequestOperation.isDone || this.m_RequestOperation.asset != null));
			}

			// Token: 0x06000360 RID: 864 RVA: 0x0000C188 File Offset: 0x0000A388
			private void ActionComplete(AsyncOperation obj)
			{
				if (this.m_RequestOperation != null)
				{
					if (this.m_ProvideHandle.Type.IsArray)
					{
						this.GetArrayResult(this.m_RequestOperation.allAssets);
					}
					else if (this.m_ProvideHandle.Type.IsGenericType && typeof(IList<>) == this.m_ProvideHandle.Type.GetGenericTypeDefinition())
					{
						this.GetListResult(this.m_RequestOperation.allAssets);
					}
					else if (string.IsNullOrEmpty(this.subObjectName))
					{
						this.GetAssetResult(this.m_RequestOperation.asset);
					}
					else
					{
						this.GetAssetSubObjectResult(this.m_RequestOperation.allAssets);
					}
				}
				this.CompleteOperation();
			}

			// Token: 0x06000361 RID: 865 RVA: 0x0000C242 File Offset: 0x0000A442
			private void GetArrayResult(Object[] allAssets)
			{
				this.m_Result = ResourceManagerConfig.CreateArrayResult(this.m_ProvideHandle.Type, allAssets);
			}

			// Token: 0x06000362 RID: 866 RVA: 0x0000C25B File Offset: 0x0000A45B
			private void GetListResult(Object[] allAssets)
			{
				this.m_Result = ResourceManagerConfig.CreateListResult(this.m_ProvideHandle.Type, allAssets);
			}

			// Token: 0x06000363 RID: 867 RVA: 0x0000C274 File Offset: 0x0000A474
			private void GetAssetResult(Object asset)
			{
				this.m_Result = ((asset != null && this.m_ProvideHandle.Type.IsAssignableFrom(asset.GetType())) ? asset : null);
			}

			// Token: 0x06000364 RID: 868 RVA: 0x0000C2A4 File Offset: 0x0000A4A4
			private void GetAssetSubObjectResult(Object[] allAssets)
			{
				foreach (Object @object in allAssets)
				{
					if (@object.name == this.subObjectName && this.m_ProvideHandle.Type.IsAssignableFrom(@object.GetType()))
					{
						this.m_Result = @object;
						return;
					}
				}
			}

			// Token: 0x06000365 RID: 869 RVA: 0x0000C2F8 File Offset: 0x0000A4F8
			private void CompleteOperation()
			{
				if (Profiler.enabled && this.m_Result != null && this.m_ProvideHandle.IsValid)
				{
					ProfilerRuntime.AddAssetOperation(this.m_ProvideHandle, ContentStatus.Active);
				}
				Exception ex = ((this.m_Result == null) ? new Exception(string.Format("Unable to load asset of type {0} from location {1}.", this.m_ProvideHandle.Type, this.m_ProvideHandle.Location)) : null);
				this.m_ProvideHandle.Complete<object>(this.m_Result, this.m_Result != null, ex);
			}

			// Token: 0x06000366 RID: 870 RVA: 0x0000C37D File Offset: 0x0000A57D
			public float ProgressCallback()
			{
				if (this.m_RequestOperation == null)
				{
					return 0f;
				}
				return this.m_RequestOperation.progress;
			}

			// Token: 0x04000162 RID: 354
			private AssetBundle m_AssetBundle;

			// Token: 0x04000163 RID: 355
			private AssetBundleRequest m_PreloadRequest;

			// Token: 0x04000164 RID: 356
			private AssetBundleRequest m_RequestOperation;

			// Token: 0x04000165 RID: 357
			private object m_Result;

			// Token: 0x04000166 RID: 358
			private ProvideHandle m_ProvideHandle;

			// Token: 0x04000167 RID: 359
			private string subObjectName;
		}
	}
}
