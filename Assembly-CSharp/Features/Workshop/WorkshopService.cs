using System;
using System.Collections.Generic;
using System.Globalization;
using Features.RunModes;
using Steamworks;
using UnityEngine;

namespace Features.Workshop
{
	// Token: 0x02000B4E RID: 2894
	public static class WorkshopService
	{
		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06005212 RID: 21010 RVA: 0x001FC9C4 File Offset: 0x001FABC4
		// (remove) Token: 0x06005213 RID: 21011 RVA: 0x001FC9F8 File Offset: 0x001FABF8
		public static event Action ItemsChanged;

		// Token: 0x06005214 RID: 21012 RVA: 0x001FCA2B File Offset: 0x001FAC2B
		public static bool TryConnect()
		{
			return SteamWorkshopBackend.EnsureAvailable() != null;
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x001FCA38 File Offset: 0x001FAC38
		public static IReadOnlyList<WorkshopInstalledItem> GetInstalledSubscriptions()
		{
			List<WorkshopInstalledItem> list = new List<WorkshopInstalledItem>();
			if (!WorkshopService.IsAvailable)
			{
				return list;
			}
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems(false);
			if (numSubscribedItems == 0U)
			{
				return list;
			}
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			uint subscribedItems = SteamUGC.GetSubscribedItems(array, numSubscribedItems, false);
			int num = 0;
			while ((long)num < (long)((ulong)Math.Min(subscribedItems, numSubscribedItems)))
			{
				WorkshopInstalledItem workshopInstalledItem;
				if (WorkshopService.TryGetInstalledItem(array[num].m_PublishedFileId, out workshopInstalledItem))
				{
					list.Add(workshopInstalledItem);
				}
				num++;
			}
			return list;
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x001FCAA8 File Offset: 0x001FACA8
		public static bool TryGetInstalledItem(ulong id, out WorkshopInstalledItem item)
		{
			item = null;
			if (!WorkshopService.IsAvailable || id == 0UL)
			{
				return false;
			}
			WorkshopInstallState installState = WorkshopService.GetInstallState(id);
			if (!installState.IsSubscribed || !installState.IsInstalled || installState.NeedsUpdate || installState.IsDownloading || installState.IsDownloadPending || installState.ErrorCode != WorkshopErrorCode.None)
			{
				return false;
			}
			ulong num;
			string text;
			uint num2;
			if (!SteamUGC.GetItemInstallInfo(new PublishedFileId_t(id), out num, out text, 4096U, out num2) || string.IsNullOrEmpty(text))
			{
				return false;
			}
			item = new WorkshopInstalledItem(id, text, num, num2);
			return true;
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06005217 RID: 21015 RVA: 0x001FCB31 File Offset: 0x001FAD31
		public static bool IsAvailable
		{
			get
			{
				return Application.isPlaying && RankedPlatformPolicy.CanInitializeSteam && RankedLeaderboardService.IsAvailable;
			}
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x001FCB48 File Offset: 0x001FAD48
		public static IDisposable RequestPage(WorkshopScope scope, int page, string search, Action<WorkshopPage> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (!Enum.IsDefined(typeof(WorkshopScope), scope))
			{
				scope = WorkshopScope.Available;
			}
			page = WorkshopRules.NormalizePage(page);
			WorkshopService.CallbackLease<WorkshopPage> lease = new WorkshopService.CallbackLease<WorkshopPage>(callback);
			lease.Deliver(new WorkshopPage(scope, page, WorkshopPageState.Loading, null, 0, WorkshopErrorCode.None), false);
			IWorkshopBackend workshopBackend = WorkshopService.ResolveBackend();
			if (workshopBackend == null || !workshopBackend.IsAvailable)
			{
				lease.Deliver(new WorkshopPage(scope, page, WorkshopPageState.Unavailable, null, 0, WorkshopErrorCode.SteamUnavailable), true);
			}
			else
			{
				try
				{
					lease.Attach(workshopBackend.RequestPage(scope, page, (scope == WorkshopScope.Available) ? WorkshopRules.NormalizeSearch(search) : string.Empty, delegate(WorkshopPage result)
					{
						lease.Deliver(result ?? new WorkshopPage(scope, page, WorkshopPageState.Error, null, 0, WorkshopErrorCode.QueryFailed), true);
					}));
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[Workshop] Request failed: " + ex.GetType().Name);
					lease.Deliver(new WorkshopPage(scope, page, WorkshopPageState.Error, null, 0, WorkshopErrorCode.QueryFailed), true);
				}
			}
			return lease;
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x001FCCA4 File Offset: 0x001FAEA4
		public static IDisposable SetSubscribed(ulong id, bool subscribed, Action<WorkshopOperationResult> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			WorkshopService.CallbackLease<WorkshopOperationResult> lease = new WorkshopService.CallbackLease<WorkshopOperationResult>(callback);
			IWorkshopBackend workshopBackend = WorkshopService.ResolveBackend();
			if (workshopBackend == null || !workshopBackend.IsAvailable || id == 0UL)
			{
				lease.Deliver(new WorkshopOperationResult(WorkshopErrorCode.SteamUnavailable), true);
			}
			else
			{
				string text;
				if (!subscribed && WorkshopModManager.GetStatus(id).Enabled && !WorkshopModManager.SetEnabled(id, false, out text))
				{
					Debug.LogWarning("[Workshop] Unsubscribe stopped because local activation could not be disabled: " + text);
					lease.Deliver(new WorkshopOperationResult(WorkshopErrorCode.SubscriptionFailed), true);
					return lease;
				}
				try
				{
					lease.Attach(workshopBackend.SetSubscribed(id, subscribed, delegate(WorkshopOperationResult result)
					{
						lease.Deliver(result, true);
					}));
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[Workshop] Subscription request failed: " + ex.GetType().Name);
					lease.Deliver(new WorkshopOperationResult(WorkshopErrorCode.SubscriptionFailed), true);
				}
			}
			return lease;
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001FCDA8 File Offset: 0x001FAFA8
		public static WorkshopInstallState GetInstallState(ulong id)
		{
			if (WorkshopService.IsAvailable && id != 0UL)
			{
				return SteamWorkshopBackend.ReadInstallState(id);
			}
			return default(WorkshopInstallState);
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x001FCDD0 File Offset: 0x001FAFD0
		public static bool Download(ulong id)
		{
			IWorkshopBackend workshopBackend = WorkshopService.ResolveBackend();
			return workshopBackend != null && workshopBackend.IsAvailable && id != 0UL && workshopBackend.Download(id);
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001FCDFA File Offset: 0x001FAFFA
		public static void OpenItem(ulong id)
		{
			if (id != 0UL)
			{
				WorkshopService.OpenUrl("https://steamcommunity.com/sharedfiles/filedetails/?id=" + id.ToString(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001FCE1C File Offset: 0x001FB01C
		public static void OpenWorkshop()
		{
			WorkshopService.OpenUrl("https://steamcommunity.com/app/" + 4688700U.ToString() + "/workshop/");
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x001FCE4A File Offset: 0x001FB04A
		private static void OpenUrl(string url)
		{
			if (WorkshopService.IsAvailable && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x001FCE68 File Offset: 0x001FB068
		internal static void NotifyItemsChanged()
		{
			Action itemsChanged = WorkshopService.ItemsChanged;
			if (itemsChanged == null)
			{
				return;
			}
			itemsChanged();
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x001FCE79 File Offset: 0x001FB079
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			WorkshopService.ItemsChanged = null;
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x001FCE81 File Offset: 0x001FB081
		private static IWorkshopBackend ResolveBackend()
		{
			return SteamWorkshopBackend.EnsureAvailable();
		}

		// Token: 0x02000B4F RID: 2895
		private sealed class CallbackLease<T> : IDisposable
		{
			// Token: 0x06005222 RID: 21026 RVA: 0x001FCE88 File Offset: 0x001FB088
			public CallbackLease(Action<T> callback)
			{
				this._callback = callback;
			}

			// Token: 0x06005223 RID: 21027 RVA: 0x001FCE97 File Offset: 0x001FB097
			public void Attach(IDisposable request)
			{
				if (this._closed)
				{
					if (request != null)
					{
						request.Dispose();
						return;
					}
				}
				else
				{
					this._request = request;
				}
			}

			// Token: 0x06005224 RID: 21028 RVA: 0x001FCEB2 File Offset: 0x001FB0B2
			public void Deliver(T value, bool terminal = true)
			{
				if (this._closed)
				{
					return;
				}
				Action<T> callback = this._callback;
				if (terminal)
				{
					this.Dispose();
				}
				if (callback == null)
				{
					return;
				}
				callback(value);
			}

			// Token: 0x06005225 RID: 21029 RVA: 0x001FCED7 File Offset: 0x001FB0D7
			public void Dispose()
			{
				if (this._closed)
				{
					return;
				}
				this._closed = true;
				this._callback = null;
				IDisposable request = this._request;
				this._request = null;
				if (request == null)
				{
					return;
				}
				request.Dispose();
			}

			// Token: 0x040052B3 RID: 21171
			private Action<T> _callback;

			// Token: 0x040052B4 RID: 21172
			private IDisposable _request;

			// Token: 0x040052B5 RID: 21173
			private bool _closed;
		}

		// Token: 0x02000B50 RID: 2896
		private sealed class ActionDisposable : IDisposable
		{
			// Token: 0x06005226 RID: 21030 RVA: 0x001FCF07 File Offset: 0x001FB107
			public ActionDisposable(Action action)
			{
				this._action = action;
			}

			// Token: 0x06005227 RID: 21031 RVA: 0x001FCF16 File Offset: 0x001FB116
			public void Dispose()
			{
				Action action = this._action;
				this._action = null;
				if (action == null)
				{
					return;
				}
				action();
			}

			// Token: 0x040052B6 RID: 21174
			private Action _action;
		}
	}
}
