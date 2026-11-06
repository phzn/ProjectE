using System;
using System.IO;
using Unity.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Unity.Entities.Content
{
	// Token: 0x0200045C RID: 1116
	public static class ContentDeliveryGlobalState
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x0005FD09 File Offset: 0x0005DF09
		public static ContentDeliveryGlobalState.ContentUpdateState CurrentContentUpdateState
		{
			get
			{
				return ContentDeliveryGlobalState.contentUpdateState;
			}
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0005FD10 File Offset: 0x0005DF10
		internal static void Cleanup()
		{
			ContentDeliveryService contentDeliveryService = ContentDeliveryGlobalState.contentDeliveryService;
			if (contentDeliveryService == null)
			{
				return;
			}
			contentDeliveryService.Dispose();
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0005FD21 File Offset: 0x0005DF21
		public static void RegisterForContentUpdateCompletion(Action<ContentDeliveryGlobalState.ContentUpdateState> updateStateFunc)
		{
			if (updateStateFunc != null)
			{
				if (ContentDeliveryGlobalState.contentUpdateState >= ContentDeliveryGlobalState.ContentUpdateState.ContentReady)
				{
					updateStateFunc(ContentDeliveryGlobalState.contentUpdateState);
					return;
				}
				ContentDeliveryGlobalState.OnContentReady = (Action<ContentDeliveryGlobalState.ContentUpdateState>)Delegate.Combine(ContentDeliveryGlobalState.OnContentReady, updateStateFunc);
			}
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0005FD50 File Offset: 0x0005DF50
		public static void Initialize(string remoteUrlRoot, string cachePath, string initialContentSet, Action<ContentDeliveryGlobalState.ContentUpdateState> updateStateFunc)
		{
			if (string.IsNullOrEmpty(remoteUrlRoot))
			{
				ContentDeliveryGlobalState.contentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.UsingContentFromStreamingAssets;
				ContentDeliveryGlobalState.PathRemapFunc = (string p) => Application.streamingAssetsPath + "/" + p;
			}
			else
			{
				ContentDeliveryGlobalState.contentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.None;
				ContentDeliveryGlobalState.contentUpdateContext = new ContentDeliveryGlobalState.ContentUpdateContext
				{
					remoteUrlRoot = remoteUrlRoot,
					cachePath = cachePath,
					initialContentSet = initialContentSet
				};
				ContentDeliveryGlobalState.contentDeliveryService = new ContentDeliveryService();
				ContentDeliveryGlobalState.PathRemapFunc = new Func<string, string>(ContentDeliveryGlobalState.contentDeliveryService.RemapContentPath);
				ContentDeliveryGlobalState.contentDeliveryService.AddDownloadService(new ContentDownloadService("default", cachePath, 1, 5, null));
			}
			ContentDeliveryGlobalState.RegisterForContentUpdateCompletion(updateStateFunc);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0005FDF4 File Offset: 0x0005DFF4
		internal static void Update()
		{
			if (ContentDeliveryGlobalState.contentDeliveryService == null)
			{
				return;
			}
			ContentDeliveryGlobalState.contentDeliveryService.Process();
			if (ContentDeliveryGlobalState.contentUpdateContext == null)
			{
				return;
			}
			ContentDeliveryGlobalState.ContentUpdateState contentUpdateState = ContentDeliveryGlobalState.contentUpdateState;
			if (ContentDeliveryGlobalState.contentUpdateContext.Update(ContentDeliveryGlobalState.contentDeliveryService, ref contentUpdateState))
			{
				ContentDeliveryGlobalState.contentUpdateContext = null;
				if (contentUpdateState == ContentDeliveryGlobalState.ContentUpdateState.UsingContentFromStreamingAssets)
				{
					ContentDeliveryGlobalState.contentDeliveryService.Dispose();
					ContentDeliveryGlobalState.contentDeliveryService = null;
					ContentDeliveryGlobalState.PathRemapFunc = (string p) => Application.streamingAssetsPath + "/" + p;
				}
			}
			if (contentUpdateState != ContentDeliveryGlobalState.contentUpdateState)
			{
				ContentDeliveryGlobalState.contentUpdateState = contentUpdateState;
				if (ContentDeliveryGlobalState.OnContentReady != null)
				{
					ContentDeliveryGlobalState.OnContentReady(ContentDeliveryGlobalState.contentUpdateState);
					if (ContentDeliveryGlobalState.contentUpdateState >= ContentDeliveryGlobalState.ContentUpdateState.ContentReady)
					{
						ContentDeliveryGlobalState.OnContentReady = null;
					}
				}
			}
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0005FEA4 File Offset: 0x0005E0A4
		internal static string CreateCatalogLocationPath(string streamingAssetsPath)
		{
			return Path.Combine(streamingAssetsPath, "catalogs.bin");
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0005FEB4 File Offset: 0x0005E0B4
		internal unsafe static bool FileExists(string path)
		{
			FileInfoResult fileInfoResult;
			AsyncReadManager.GetFileInfo(path, &fileInfoResult).JobHandle.Complete();
			return fileInfoResult.FileState == FileState.Exists;
		}

		// Token: 0x04000991 RID: 2449
		public static Action<string> LogFunc;

		// Token: 0x04000992 RID: 2450
		public const string kCatalogLocations = "catalogs";

		// Token: 0x04000993 RID: 2451
		public static Func<string, string> PathRemapFunc;

		// Token: 0x04000994 RID: 2452
		private static Action<ContentDeliveryGlobalState.ContentUpdateState> OnContentReady;

		// Token: 0x04000995 RID: 2453
		private static ContentDeliveryGlobalState.ContentUpdateState contentUpdateState;

		// Token: 0x04000996 RID: 2454
		private static ContentDeliveryGlobalState.ContentUpdateContext contentUpdateContext;

		// Token: 0x04000997 RID: 2455
		private static ContentDeliveryService contentDeliveryService;

		// Token: 0x0200045D RID: 1117
		public enum ContentUpdateState
		{
			// Token: 0x04000999 RID: 2457
			None,
			// Token: 0x0400099A RID: 2458
			DownloadingCatalogInfo,
			// Token: 0x0400099B RID: 2459
			DownloadingCatalogs,
			// Token: 0x0400099C RID: 2460
			DownloadingContentSet,
			// Token: 0x0400099D RID: 2461
			NoContentAvailable,
			// Token: 0x0400099E RID: 2462
			ContentReady,
			// Token: 0x0400099F RID: 2463
			UsingContentFromStreamingAssets,
			// Token: 0x040009A0 RID: 2464
			ContentUpdatedFromRemote,
			// Token: 0x040009A1 RID: 2465
			UsingContentFromCache
		}

		// Token: 0x0200045E RID: 1118
		internal class ContentUpdateContext
		{
			// Token: 0x06001BAC RID: 7084 RVA: 0x0005FEE4 File Offset: 0x0005E0E4
			public bool Update(ContentDeliveryService contentDeliveryService, ref ContentDeliveryGlobalState.ContentUpdateState currentUpdateState)
			{
				if (currentUpdateState == ContentDeliveryGlobalState.ContentUpdateState.None)
				{
					Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
					if (logFunc != null)
					{
						logFunc("Downloading remote catalog info from " + this.remoteUrlRoot + "catalogs.bin");
					}
					this.currentRemoteId = contentDeliveryService.DeliverContent(this.remoteUrlRoot + "catalogs.bin", default(Hash128), 0L, 0U);
					currentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.DownloadingCatalogInfo;
				}
				if (currentUpdateState == ContentDeliveryGlobalState.ContentUpdateState.DownloadingCatalogInfo)
				{
					ContentDeliveryService.DeliveryStatus deliveryStatus = contentDeliveryService.GetDeliveryStatus(in this.currentRemoteId);
					if (deliveryStatus.State == ContentDeliveryService.DeliveryState.ContentDownloaded)
					{
						Action<string> logFunc2 = ContentDeliveryGlobalState.LogFunc;
						if (logFunc2 != null)
						{
							logFunc2(string.Format("Remote catalog info loaded, creating location service from {0}", deliveryStatus.DownloadStatus.LocalPath));
						}
						string text = Path.Combine(this.cachePath, "catalogs.bin");
						File.Copy(deliveryStatus.DownloadStatus.LocalPath.ToString(), text, true);
						File.Delete(deliveryStatus.DownloadStatus.LocalPath.ToString());
						DefaultContentLocationService defaultContentLocationService = new DefaultContentLocationService("catalogs", 1, text, (string p) => this.remoteUrlRoot + p);
						contentDeliveryService.AddLocationService(defaultContentLocationService);
						Action<string> logFunc3 = ContentDeliveryGlobalState.LogFunc;
						if (logFunc3 != null)
						{
							logFunc3("Downloading content set 'catalogs'");
						}
						FixedString512Bytes fixedString512Bytes = "catalogs";
						this.currentRemoteId = contentDeliveryService.DeliverContent(in fixedString512Bytes);
						currentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.DownloadingCatalogs;
					}
					else if (deliveryStatus.State == ContentDeliveryService.DeliveryState.Failed)
					{
						Action<string> logFunc4 = ContentDeliveryGlobalState.LogFunc;
						if (logFunc4 != null)
						{
							logFunc4("Failed to load remote catalog info.");
						}
						string text2 = Path.Combine(this.cachePath, "catalogs.bin");
						if (File.Exists(text2))
						{
							Action<string> logFunc5 = ContentDeliveryGlobalState.LogFunc;
							if (logFunc5 != null)
							{
								logFunc5("Cached catalog info file found at '" + text2 + "', attempting to use cached content.");
							}
							DefaultContentLocationService defaultContentLocationService2 = new DefaultContentLocationService("catalogs", 1, text2, (string p) => this.remoteUrlRoot + p);
							contentDeliveryService.AddLocationService(defaultContentLocationService2);
							Action<string> logFunc6 = ContentDeliveryGlobalState.LogFunc;
							if (logFunc6 != null)
							{
								logFunc6("Downloading content set 'catalogs'");
							}
							FixedString512Bytes fixedString512Bytes = "catalogs";
							this.currentRemoteId = contentDeliveryService.DeliverContent(in fixedString512Bytes);
							currentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.DownloadingCatalogs;
						}
						else
						{
							currentUpdateState = this.AttemptToSetContentPathsToStreamingAssets();
						}
					}
				}
				if (currentUpdateState == ContentDeliveryGlobalState.ContentUpdateState.DownloadingCatalogs)
				{
					ContentDeliveryService.DeliveryStatus deliveryStatus2 = contentDeliveryService.GetDeliveryStatus(in this.currentRemoteId);
					if (deliveryStatus2.State == ContentDeliveryService.DeliveryState.ContentDownloaded)
					{
						NativeList<ContentDeliveryService.DeliveryStatus> nativeList = new NativeList<ContentDeliveryService.DeliveryStatus>(3, Allocator.Temp);
						contentDeliveryService.GetDeliveryStatus(in this.currentRemoteId, ref nativeList);
						Action<string> logFunc7 = ContentDeliveryGlobalState.LogFunc;
						if (logFunc7 != null)
						{
							logFunc7(string.Format("Content set '{0}' loaded, found {1} catalog locations.", "catalogs", nativeList.Length));
						}
						for (int i = 0; i < nativeList.Length; i++)
						{
							Action<string> logFunc8 = ContentDeliveryGlobalState.LogFunc;
							if (logFunc8 != null)
							{
								logFunc8(string.Format("Loading catalog '{0}' from path '{1}'", nativeList[i].ContentId.Name, nativeList[i].DownloadStatus.LocalPath));
							}
							ContentDeliveryService.DeliveryStatus deliveryStatus3 = nativeList[i];
							string text3 = deliveryStatus3.ContentId.Name.ToString();
							int num = 2 + (nativeList.Length - i);
							deliveryStatus3 = nativeList[i];
							DefaultContentLocationService defaultContentLocationService3 = new DefaultContentLocationService(text3, num, deliveryStatus3.DownloadStatus.LocalPath.ToString(), (string p) => this.remoteUrlRoot + p);
							contentDeliveryService.AddLocationService(defaultContentLocationService3);
						}
						nativeList.Dispose();
						if (!string.IsNullOrEmpty(this.initialContentSet))
						{
							Action<string> logFunc9 = ContentDeliveryGlobalState.LogFunc;
							if (logFunc9 != null)
							{
								logFunc9("Downloading content set '" + this.initialContentSet + "'");
							}
							FixedString512Bytes fixedString512Bytes = this.initialContentSet;
							this.currentRemoteId = contentDeliveryService.DeliverContent(in fixedString512Bytes);
							currentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.DownloadingContentSet;
						}
						else
						{
							Action<string> logFunc10 = ContentDeliveryGlobalState.LogFunc;
							if (logFunc10 != null)
							{
								logFunc10("No initial content set specified");
							}
							this.currentRemoteId = default(RemoteContentId);
							currentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.ContentUpdatedFromRemote;
						}
					}
					else if (deliveryStatus2.State == ContentDeliveryService.DeliveryState.Failed)
					{
						currentUpdateState = this.AttemptToSetContentPathsToStreamingAssets();
					}
				}
				if (currentUpdateState == ContentDeliveryGlobalState.ContentUpdateState.DownloadingContentSet)
				{
					ContentDeliveryService.DeliveryStatus deliveryStatus4 = contentDeliveryService.GetDeliveryStatus(in this.currentRemoteId);
					if (deliveryStatus4.State == ContentDeliveryService.DeliveryState.ContentDownloaded)
					{
						NativeList<ContentDeliveryService.DeliveryStatus> nativeList2 = new NativeList<ContentDeliveryService.DeliveryStatus>(128, Allocator.Temp);
						contentDeliveryService.GetDeliveryStatus(in this.currentRemoteId, ref nativeList2);
						Action<string> logFunc11 = ContentDeliveryGlobalState.LogFunc;
						if (logFunc11 != null)
						{
							logFunc11(string.Format("Content set '{0}' loaded, found {1} content locations.", this.initialContentSet, nativeList2.Length));
						}
						nativeList2.Dispose();
						this.currentRemoteId = default(RemoteContentId);
						currentUpdateState = ContentDeliveryGlobalState.ContentUpdateState.ContentUpdatedFromRemote;
					}
					else if (deliveryStatus4.State == ContentDeliveryService.DeliveryState.Failed)
					{
						currentUpdateState = this.AttemptToSetContentPathsToStreamingAssets();
					}
				}
				return currentUpdateState >= ContentDeliveryGlobalState.ContentUpdateState.NoContentAvailable;
			}

			// Token: 0x06001BAD RID: 7085 RVA: 0x0006035C File Offset: 0x0005E55C
			internal ContentDeliveryGlobalState.ContentUpdateState AttemptToSetContentPathsToStreamingAssets()
			{
				string text = Application.streamingAssetsPath + "/" + RuntimeContentManager.RelativeCatalogPath;
				if (ContentDeliveryGlobalState.FileExists(text))
				{
					Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
					if (logFunc != null)
					{
						logFunc("No connection, but catalog found at " + text + ".  Attempting to use streaming assets for content.");
					}
					return ContentDeliveryGlobalState.ContentUpdateState.UsingContentFromStreamingAssets;
				}
				Action<string> logFunc2 = ContentDeliveryGlobalState.LogFunc;
				if (logFunc2 != null)
				{
					logFunc2("No connection, no cached data, no catalog found at " + text + ".  Content is not available.");
				}
				return ContentDeliveryGlobalState.ContentUpdateState.NoContentAvailable;
			}

			// Token: 0x040009A2 RID: 2466
			public string remoteUrlRoot;

			// Token: 0x040009A3 RID: 2467
			public string initialContentSet;

			// Token: 0x040009A4 RID: 2468
			public string cachePath;

			// Token: 0x040009A5 RID: 2469
			private RemoteContentId currentRemoteId;
		}
	}
}
