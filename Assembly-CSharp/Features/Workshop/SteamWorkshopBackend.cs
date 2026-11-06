using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.Localization;
using Features.RunModes;
using Steamworks;
using UnityEngine;

namespace Features.Workshop
{
	// Token: 0x02000B34 RID: 2868
	[DisallowMultipleComponent]
	public sealed class SteamWorkshopBackend : MonoBehaviour, IWorkshopBackend
	{
		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x001FA65A File Offset: 0x001F885A
		public bool IsAvailable
		{
			get
			{
				return WorkshopService.IsAvailable;
			}
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001FA664 File Offset: 0x001F8864
		internal static SteamWorkshopBackend EnsureAvailable()
		{
			if (!Application.isPlaying || !RankedPlatformPolicy.CanInitializeSteam || !SteamworksNetLeaderboardBackend.EnsureInitialized() || !WorkshopService.IsAvailable)
			{
				return null;
			}
			if (SteamWorkshopBackend._instance == null)
			{
				GameObject gameObject = new GameObject("[Steam Workshop]");
				Object.DontDestroyOnLoad(gameObject);
				SteamWorkshopBackend._instance = gameObject.AddComponent<SteamWorkshopBackend>();
			}
			SteamWorkshopBackend._instance._platform = Object.FindObjectOfType<SteamworksNetLeaderboardBackend>();
			SteamWorkshopBackend._instance.EnsureCallbacks();
			return SteamWorkshopBackend._instance;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001FA6D4 File Offset: 0x001F88D4
		private void Awake()
		{
			if (SteamWorkshopBackend._instance != null && SteamWorkshopBackend._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			SteamWorkshopBackend._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			RankedLeaderboardService.AvailabilityChanged += this.OnAvailabilityChanged;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001FA72C File Offset: 0x001F892C
		private void EnsureCallbacks()
		{
			if (this._callbacksReady)
			{
				return;
			}
			this._callbacksReady = true;
			this._installed = Callback<ItemInstalled_t>.Create(delegate(ItemInstalled_t result)
			{
				if (result.m_unAppID.m_AppId == 4688700U)
				{
					this._failedDownloads.Remove(result.m_nPublishedFileId.m_PublishedFileId);
					WorkshopService.NotifyItemsChanged();
				}
			});
			this._downloaded = Callback<DownloadItemResult_t>.Create(delegate(DownloadItemResult_t result)
			{
				if (result.m_unAppID.m_AppId != 4688700U)
				{
					return;
				}
				ulong publishedFileId = result.m_nPublishedFileId.m_PublishedFileId;
				this._requestedDownloads.Remove(publishedFileId);
				if (result.m_eResult == EResult.k_EResultOK)
				{
					this._failedDownloads.Remove(publishedFileId);
				}
				else
				{
					this._failedDownloads.Add(publishedFileId);
				}
				WorkshopService.NotifyItemsChanged();
			});
			this._subscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(delegate(RemoteStoragePublishedFileSubscribed_t result)
			{
				if (result.m_nAppID.m_AppId == 4688700U)
				{
					WorkshopService.NotifyItemsChanged();
				}
			});
			this._unsubscribed = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(delegate(RemoteStoragePublishedFileUnsubscribed_t result)
			{
				if (result.m_nAppID.m_AppId == 4688700U)
				{
					this._failedDownloads.Remove(result.m_nPublishedFileId.m_PublishedFileId);
					WorkshopService.NotifyItemsChanged();
				}
			});
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001FA7B8 File Offset: 0x001F89B8
		public IDisposable RequestPage(WorkshopScope scope, int page, string search, Action<WorkshopPage> callback)
		{
			SteamWorkshopBackend.QueryRequest queryRequest = new SteamWorkshopBackend.QueryRequest(this, scope, page, callback);
			this._queries.Add(queryRequest);
			try
			{
				queryRequest.Start(search);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Workshop] Query failed: " + ex.GetType().Name);
				queryRequest.Fail(WorkshopErrorCode.QueryFailed);
			}
			return queryRequest;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001FA81C File Offset: 0x001F8A1C
		public IDisposable SetSubscribed(ulong id, bool subscribed, Action<WorkshopOperationResult> callback)
		{
			if (this._subscriptions.ContainsKey(id))
			{
				callback(new WorkshopOperationResult(WorkshopErrorCode.Busy));
				return new SteamWorkshopBackend.ListenerLease(null);
			}
			SteamWorkshopBackend.SubscriptionRequest subscriptionRequest = new SteamWorkshopBackend.SubscriptionRequest(this, id, subscribed, callback);
			this._subscriptions.Add(id, subscriptionRequest);
			try
			{
				subscriptionRequest.Start();
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Workshop] Subscription failed: " + ex.GetType().Name);
				subscriptionRequest.Complete(WorkshopErrorCode.SubscriptionFailed);
			}
			return new SteamWorkshopBackend.ListenerLease(new Action(subscriptionRequest.DetachListener));
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001FA8B0 File Offset: 0x001F8AB0
		public WorkshopInstallState GetInstallState(ulong id)
		{
			return SteamWorkshopBackend.ReadInstallState(id);
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001FA8B8 File Offset: 0x001F8AB8
		public bool Download(ulong id)
		{
			if (!SteamWorkshopBackend.ReadInstallState(id).IsSubscribed)
			{
				return false;
			}
			bool flag = SteamUGC.DownloadItem(new PublishedFileId_t(id), false);
			if (flag)
			{
				this._failedDownloads.Remove(id);
				this._requestedDownloads.Add(id);
			}
			else
			{
				this._failedDownloads.Add(id);
			}
			WorkshopService.NotifyItemsChanged();
			return flag;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001FA914 File Offset: 0x001F8B14
		internal static WorkshopInstallState ReadInstallState(ulong id)
		{
			SteamWorkshopBackend.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.state = (EItemState)SteamUGC.GetItemState(new PublishedFileId_t(id));
			bool flag = SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateDownloading, ref CS$<>8__locals1);
			bool flag2 = SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateDownloadPending, ref CS$<>8__locals1) || (SteamWorkshopBackend._instance != null && SteamWorkshopBackend._instance._requestedDownloads.Contains(id));
			ulong num = 0UL;
			ulong num2 = 0UL;
			if (flag || flag2 || SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateNeedsUpdate, ref CS$<>8__locals1))
			{
				SteamUGC.GetItemDownloadInfo(new PublishedFileId_t(id), out num, out num2);
			}
			return new WorkshopInstallState(SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateSubscribed, ref CS$<>8__locals1) && !SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateDisabledLocally, ref CS$<>8__locals1), SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateInstalled, ref CS$<>8__locals1), flag, flag2, SteamWorkshopBackend.<ReadInstallState>g__has|21_0(EItemState.k_EItemStateNeedsUpdate, ref CS$<>8__locals1), num, num2, (SteamWorkshopBackend._instance != null && SteamWorkshopBackend._instance._failedDownloads.Contains(id)) ? WorkshopErrorCode.DownloadFailed : WorkshopErrorCode.None);
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x001FA9E8 File Offset: 0x001F8BE8
		private void Update()
		{
			if (!WorkshopService.IsAvailable)
			{
				if (this._queries.Count > 0 || this._subscriptions.Count > 0)
				{
					this.FailOutstanding();
				}
				return;
			}
			double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
			foreach (SteamWorkshopBackend.QueryRequest queryRequest in this._queries.ToArray())
			{
				if (realtimeSinceStartupAsDouble >= queryRequest.Deadline)
				{
					queryRequest.Fail(WorkshopErrorCode.TimedOut);
				}
			}
			foreach (SteamWorkshopBackend.SubscriptionRequest subscriptionRequest in new List<SteamWorkshopBackend.SubscriptionRequest>(this._subscriptions.Values))
			{
				subscriptionRequest.CheckTimeout(realtimeSinceStartupAsDouble);
			}
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x001FAAA4 File Offset: 0x001F8CA4
		private void OnAvailabilityChanged()
		{
			if (!WorkshopService.IsAvailable)
			{
				this.FailOutstanding();
			}
			WorkshopService.NotifyItemsChanged();
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x001FAAB8 File Offset: 0x001F8CB8
		private void FailOutstanding()
		{
			SteamWorkshopBackend.QueryRequest[] array = this._queries.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Fail(WorkshopErrorCode.SteamUnavailable);
			}
			foreach (SteamWorkshopBackend.SubscriptionRequest subscriptionRequest in new List<SteamWorkshopBackend.SubscriptionRequest>(this._subscriptions.Values))
			{
				subscriptionRequest.Complete(WorkshopErrorCode.SteamUnavailable);
			}
			this._failedDownloads.Clear();
			this._requestedDownloads.Clear();
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001FAB4C File Offset: 0x001F8D4C
		private void OnDestroy()
		{
			if (SteamWorkshopBackend._instance != this)
			{
				return;
			}
			RankedLeaderboardService.AvailabilityChanged -= this.OnAvailabilityChanged;
			this.FailOutstanding();
			Callback<ItemInstalled_t> installed = this._installed;
			if (installed != null)
			{
				installed.Dispose();
			}
			Callback<DownloadItemResult_t> downloaded = this._downloaded;
			if (downloaded != null)
			{
				downloaded.Dispose();
			}
			Callback<RemoteStoragePublishedFileSubscribed_t> subscribed = this._subscribed;
			if (subscribed != null)
			{
				subscribed.Dispose();
			}
			Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribed = this._unsubscribed;
			if (unsubscribed != null)
			{
				unsubscribed.Dispose();
			}
			SteamWorkshopBackend._instance = null;
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x001FABC8 File Offset: 0x001F8DC8
		private bool CanReleaseNativeHandle
		{
			get
			{
				return this._platform != null && this._platform.WorkshopNativeSessionActive;
			}
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001FACDF File Offset: 0x001F8EDF
		[CompilerGenerated]
		internal static bool <ReadInstallState>g__has|21_0(EItemState flag, ref SteamWorkshopBackend.<>c__DisplayClass21_0 A_1)
		{
			return (A_1.state & flag) > EItemState.k_EItemStateNone;
		}

		// Token: 0x04005237 RID: 21047
		private const double TimeoutSeconds = 25.0;

		// Token: 0x04005238 RID: 21048
		private static SteamWorkshopBackend _instance;

		// Token: 0x04005239 RID: 21049
		private SteamworksNetLeaderboardBackend _platform;

		// Token: 0x0400523A RID: 21050
		private readonly List<SteamWorkshopBackend.QueryRequest> _queries = new List<SteamWorkshopBackend.QueryRequest>();

		// Token: 0x0400523B RID: 21051
		private readonly Dictionary<ulong, SteamWorkshopBackend.SubscriptionRequest> _subscriptions = new Dictionary<ulong, SteamWorkshopBackend.SubscriptionRequest>();

		// Token: 0x0400523C RID: 21052
		private readonly HashSet<ulong> _failedDownloads = new HashSet<ulong>();

		// Token: 0x0400523D RID: 21053
		private readonly HashSet<ulong> _requestedDownloads = new HashSet<ulong>();

		// Token: 0x0400523E RID: 21054
		private Callback<ItemInstalled_t> _installed;

		// Token: 0x0400523F RID: 21055
		private Callback<DownloadItemResult_t> _downloaded;

		// Token: 0x04005240 RID: 21056
		private Callback<RemoteStoragePublishedFileSubscribed_t> _subscribed;

		// Token: 0x04005241 RID: 21057
		private Callback<RemoteStoragePublishedFileUnsubscribed_t> _unsubscribed;

		// Token: 0x04005242 RID: 21058
		private bool _callbacksReady;

		// Token: 0x02000B35 RID: 2869
		private sealed class QueryRequest : IDisposable
		{
			// Token: 0x17000D9D RID: 3485
			// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001FACEC File Offset: 0x001F8EEC
			public double Deadline { get; }

			// Token: 0x060051AA RID: 20906 RVA: 0x001FACF4 File Offset: 0x001F8EF4
			public QueryRequest(SteamWorkshopBackend owner, WorkshopScope scope, int page, Action<WorkshopPage> callback)
			{
				this._owner = owner;
				this._scope = scope;
				this._page = page;
				this._callback = callback;
				this._account = RankedLeaderboardService.CurrentAccountId;
				this.Deadline = Time.realtimeSinceStartupAsDouble + 25.0;
			}

			// Token: 0x060051AB RID: 20907 RVA: 0x001FAD50 File Offset: 0x001F8F50
			public void Start(string search)
			{
				AppId_t appId_t = new AppId_t(4688700U);
				this._handle = ((this._scope == WorkshopScope.Subscribed) ? SteamUGC.CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Subscribed, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, EUserUGCListSortOrder.k_EUserUGCListSortOrder_SubscriptionDateDesc, AppId_t.Invalid, appId_t, (uint)(this._page + 1)) : SteamUGC.CreateQueryAllUGCRequest(string.IsNullOrEmpty(search) ? EUGCQuery.k_EUGCQuery_RankedByPublicationDate : EUGCQuery.k_EUGCQuery_RankedByTextSearch, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, AppId_t.Invalid, appId_t, (uint)(this._page + 1)));
				if (this._handle == UGCQueryHandle_t.Invalid)
				{
					this.Fail(WorkshopErrorCode.QueryFailed);
					return;
				}
				SteamUGC.SetLanguage(this._handle, WorkshopRules.SteamLanguage(GameLocalization.CurrentCode));
				SteamUGC.SetAllowCachedResponse(this._handle, 0U);
				SteamUGC.SetReturnLongDescription(this._handle, true);
				if (!string.IsNullOrEmpty(search) && !SteamUGC.SetSearchText(this._handle, search))
				{
					this.Fail(WorkshopErrorCode.QueryFailed);
					return;
				}
				this._result = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnCompleted));
				SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(this._handle);
				if (steamAPICall_t == SteamAPICall_t.Invalid)
				{
					this.Fail(WorkshopErrorCode.QueryFailed);
					return;
				}
				this._result.Set(steamAPICall_t, null);
			}

			// Token: 0x060051AC RID: 20908 RVA: 0x001FAE6C File Offset: 0x001F906C
			private void OnCompleted(SteamUGCQueryCompleted_t response, bool ioFailure)
			{
				if (this._callback == null)
				{
					return;
				}
				if (!WorkshopService.IsAvailable || this._account != RankedLeaderboardService.CurrentAccountId)
				{
					this.Fail(WorkshopErrorCode.SteamUnavailable);
					return;
				}
				if (ioFailure || response.m_eResult != EResult.k_EResultOK || response.m_handle != this._handle)
				{
					this.Fail(WorkshopErrorCode.QueryFailed);
					return;
				}
				List<WorkshopItem> list = new List<WorkshopItem>();
				for (uint num = 0U; num < Math.Min(response.m_unNumResultsReturned, 50U); num += 1U)
				{
					SteamUGCDetails_t steamUGCDetails_t;
					if (SteamUGC.GetQueryUGCResult(this._handle, num, out steamUGCDetails_t) && steamUGCDetails_t.m_eResult == EResult.k_EResultOK && !steamUGCDetails_t.m_bBanned && steamUGCDetails_t.m_nConsumerAppID.m_AppId == 4688700U)
					{
						string text;
						SteamUGC.GetQueryUGCPreviewURL(this._handle, num, out text, 4096U);
						list.Add(new WorkshopItem(steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId, steamUGCDetails_t.m_rgchTitle, steamUGCDetails_t.m_rgchDescription, text, steamUGCDetails_t.m_ulSteamIDOwner));
					}
				}
				WorkshopPage workshopPage = new WorkshopPage(this._scope, this._page, WorkshopPageState.Ready, list, (int)Math.Min(response.m_unTotalMatchingResults, 2147483647U), WorkshopErrorCode.None);
				Action<WorkshopPage> callback = this._callback;
				this.Dispose();
				if (callback == null)
				{
					return;
				}
				callback(workshopPage);
			}

			// Token: 0x060051AD RID: 20909 RVA: 0x001FAF9A File Offset: 0x001F919A
			public void Fail(WorkshopErrorCode error)
			{
				Action<WorkshopPage> callback = this._callback;
				this.Dispose();
				if (callback == null)
				{
					return;
				}
				callback(new WorkshopPage(this._scope, this._page, (error == WorkshopErrorCode.SteamUnavailable) ? WorkshopPageState.Unavailable : ((error == WorkshopErrorCode.TimedOut) ? WorkshopPageState.Timeout : WorkshopPageState.Error), null, 0, error));
			}

			// Token: 0x060051AE RID: 20910 RVA: 0x001FAFD8 File Offset: 0x001F91D8
			public void Dispose()
			{
				this._callback = null;
				CallResult<SteamUGCQueryCompleted_t> result = this._result;
				if (result != null)
				{
					result.Dispose();
				}
				this._result = null;
				if (this._handle != UGCQueryHandle_t.Invalid)
				{
					if (this._owner.CanReleaseNativeHandle)
					{
						SteamUGC.ReleaseQueryUGCRequest(this._handle);
					}
					this._handle = UGCQueryHandle_t.Invalid;
				}
				this._owner._queries.Remove(this);
			}

			// Token: 0x04005243 RID: 21059
			private readonly SteamWorkshopBackend _owner;

			// Token: 0x04005244 RID: 21060
			private readonly WorkshopScope _scope;

			// Token: 0x04005245 RID: 21061
			private readonly int _page;

			// Token: 0x04005246 RID: 21062
			private readonly string _account;

			// Token: 0x04005247 RID: 21063
			private Action<WorkshopPage> _callback;

			// Token: 0x04005248 RID: 21064
			private UGCQueryHandle_t _handle = UGCQueryHandle_t.Invalid;

			// Token: 0x04005249 RID: 21065
			private CallResult<SteamUGCQueryCompleted_t> _result;
		}

		// Token: 0x02000B36 RID: 2870
		private sealed class SubscriptionRequest
		{
			// Token: 0x060051AF RID: 20911 RVA: 0x001FB04C File Offset: 0x001F924C
			public SubscriptionRequest(SteamWorkshopBackend owner, ulong id, bool subscribe, Action<WorkshopOperationResult> callback)
			{
				this._owner = owner;
				this._id = id;
				this._subscribe = subscribe;
				this._callback = callback;
				this._account = RankedLeaderboardService.CurrentAccountId;
				this._deadline = Time.realtimeSinceStartupAsDouble + 25.0;
			}

			// Token: 0x060051B0 RID: 20912 RVA: 0x001FB09C File Offset: 0x001F929C
			public void Start()
			{
				PublishedFileId_t id = new PublishedFileId_t(this._id);
				if (this._subscribe)
				{
					this._subscribeResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(delegate(RemoteStorageSubscribePublishedFileResult_t r, bool failure)
					{
						this.Complete((!failure && r.m_eResult == EResult.k_EResultOK && r.m_nPublishedFileId == id) ? WorkshopErrorCode.None : WorkshopErrorCode.SubscriptionFailed);
					});
					SteamAPICall_t steamAPICall_t = SteamUGC.SubscribeItem(id);
					if (steamAPICall_t == SteamAPICall_t.Invalid)
					{
						this.Complete(WorkshopErrorCode.SubscriptionFailed);
						return;
					}
					this._subscribeResult.Set(steamAPICall_t, null);
					return;
				}
				else
				{
					this._unsubscribeResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(delegate(RemoteStorageUnsubscribePublishedFileResult_t r, bool failure)
					{
						this.Complete((!failure && r.m_eResult == EResult.k_EResultOK && r.m_nPublishedFileId == id) ? WorkshopErrorCode.None : WorkshopErrorCode.SubscriptionFailed);
					});
					SteamAPICall_t steamAPICall_t2 = SteamUGC.UnsubscribeItem(id);
					if (steamAPICall_t2 == SteamAPICall_t.Invalid)
					{
						this.Complete(WorkshopErrorCode.SubscriptionFailed);
						return;
					}
					this._unsubscribeResult.Set(steamAPICall_t2, null);
					return;
				}
			}

			// Token: 0x060051B1 RID: 20913 RVA: 0x001FB15C File Offset: 0x001F935C
			public void CheckTimeout(double now)
			{
				if (now < this._deadline || this._finished || now < this._nextReconcile)
				{
					return;
				}
				this._nextReconcile = now + 1.0;
				if (SteamWorkshopBackend.ReadInstallState(this._id).IsSubscribed == this._subscribe)
				{
					this.Complete(WorkshopErrorCode.None);
					return;
				}
				if (this._callback == null)
				{
					return;
				}
				Action<WorkshopOperationResult> callback = this._callback;
				this._callback = null;
				callback(new WorkshopOperationResult(WorkshopErrorCode.TimedOut));
			}

			// Token: 0x060051B2 RID: 20914 RVA: 0x001FB1DC File Offset: 0x001F93DC
			public void Complete(WorkshopErrorCode error)
			{
				if (this._finished)
				{
					return;
				}
				this._finished = true;
				if (!WorkshopService.IsAvailable || this._account != RankedLeaderboardService.CurrentAccountId)
				{
					error = WorkshopErrorCode.SteamUnavailable;
				}
				CallResult<RemoteStorageSubscribePublishedFileResult_t> subscribeResult = this._subscribeResult;
				if (subscribeResult != null)
				{
					subscribeResult.Dispose();
				}
				CallResult<RemoteStorageUnsubscribePublishedFileResult_t> unsubscribeResult = this._unsubscribeResult;
				if (unsubscribeResult != null)
				{
					unsubscribeResult.Dispose();
				}
				this._owner._subscriptions.Remove(this._id);
				Action<WorkshopOperationResult> callback = this._callback;
				this._callback = null;
				if (callback != null)
				{
					callback(new WorkshopOperationResult(error));
				}
				WorkshopService.NotifyItemsChanged();
			}

			// Token: 0x060051B3 RID: 20915 RVA: 0x001FB271 File Offset: 0x001F9471
			public void DetachListener()
			{
				this._callback = null;
			}

			// Token: 0x0400524B RID: 21067
			private readonly SteamWorkshopBackend _owner;

			// Token: 0x0400524C RID: 21068
			private readonly ulong _id;

			// Token: 0x0400524D RID: 21069
			private readonly bool _subscribe;

			// Token: 0x0400524E RID: 21070
			private readonly string _account;

			// Token: 0x0400524F RID: 21071
			private readonly double _deadline;

			// Token: 0x04005250 RID: 21072
			private double _nextReconcile;

			// Token: 0x04005251 RID: 21073
			private Action<WorkshopOperationResult> _callback;

			// Token: 0x04005252 RID: 21074
			private CallResult<RemoteStorageSubscribePublishedFileResult_t> _subscribeResult;

			// Token: 0x04005253 RID: 21075
			private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> _unsubscribeResult;

			// Token: 0x04005254 RID: 21076
			private bool _finished;
		}

		// Token: 0x02000B38 RID: 2872
		private sealed class ListenerLease : IDisposable
		{
			// Token: 0x060051B7 RID: 20919 RVA: 0x001FB2DA File Offset: 0x001F94DA
			public ListenerLease(Action dispose)
			{
				this._dispose = dispose;
			}

			// Token: 0x060051B8 RID: 20920 RVA: 0x001FB2E9 File Offset: 0x001F94E9
			public void Dispose()
			{
				Action dispose = this._dispose;
				this._dispose = null;
				if (dispose == null)
				{
					return;
				}
				dispose();
			}

			// Token: 0x04005257 RID: 21079
			private Action _dispose;
		}
	}
}
