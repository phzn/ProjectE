using System;
using Features.MatchRecap;
using Features.Workshop;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D5B RID: 3419
	public static class RankedLeaderboardService
	{
		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06006461 RID: 25697 RVA: 0x00282B74 File Offset: 0x00280D74
		// (set) Token: 0x06006462 RID: 25698 RVA: 0x00282B7B File Offset: 0x00280D7B
		public static string LastSubmissionMessage { get; private set; } = string.Empty;

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06006463 RID: 25699 RVA: 0x00282B83 File Offset: 0x00280D83
		// (set) Token: 0x06006464 RID: 25700 RVA: 0x00282B8A File Offset: 0x00280D8A
		public static string LastSubmissionRunId { get; private set; } = string.Empty;

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06006465 RID: 25701 RVA: 0x00282B94 File Offset: 0x00280D94
		// (remove) Token: 0x06006466 RID: 25702 RVA: 0x00282BC8 File Offset: 0x00280DC8
		public static event Action SubmissionStatusChanged;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06006467 RID: 25703 RVA: 0x00282BFC File Offset: 0x00280DFC
		// (remove) Token: 0x06006468 RID: 25704 RVA: 0x00282C30 File Offset: 0x00280E30
		public static event Action<RankedSubmissionFailure> SubmissionFailed;

		// Token: 0x06006469 RID: 25705 RVA: 0x00282C63 File Offset: 0x00280E63
		private static void SetSubmissionMessage(string message)
		{
			RankedLeaderboardService.LastSubmissionMessage = message;
			Action submissionStatusChanged = RankedLeaderboardService.SubmissionStatusChanged;
			if (submissionStatusChanged == null)
			{
				return;
			}
			submissionStatusChanged();
		}

		// Token: 0x0600646A RID: 25706 RVA: 0x00282C7C File Offset: 0x00280E7C
		public static void NotifySubmissionConfirmed(IRankedLeaderboardBackend backend, int distanceMeters)
		{
			if (RankedLeaderboardService._backend != backend || RankedLeaderboardService._lastAcceptedResult == null || distanceMeters <= 0 || distanceMeters == 2147483647 || distanceMeters < RankedLeaderboardService._lastAcceptedResult.Value.DistanceMeters)
			{
				return;
			}
			RankedLeaderboardService._lastAcceptedResult = null;
			RankedLeaderboardService.SetSubmissionMessage("Steam confirmed your result. Your longest run and its score are kept on the leaderboard.");
		}

		// Token: 0x0600646B RID: 25707 RVA: 0x00282CD4 File Offset: 0x00280ED4
		public static void NotifySubmissionFailed(IRankedLeaderboardBackend backend, RankedRunResult result, string message, bool retainedForRetry)
		{
			RankedRunResult? authorizedResult = RankedLeaderboardService._authorizedResult;
			RankedRunResult? rankedRunResult = ((authorizedResult != null) ? authorizedResult : RankedLeaderboardService._lastAcceptedResult);
			if (backend == null || RankedLeaderboardService._backend != backend || rankedRunResult == null || !result.IsValid || result.DistanceMeters < rankedRunResult.Value.DistanceMeters || string.IsNullOrEmpty(RankedLeaderboardService.LastSubmissionRunId))
			{
				return;
			}
			if (RankedLeaderboardService._authorizedResult != null)
			{
				RankedLeaderboardService._failedDuringSubmission = true;
			}
			RankedLeaderboardService.SetSubmissionMessage(message);
			Action<RankedSubmissionFailure> submissionFailed = RankedLeaderboardService.SubmissionFailed;
			if (submissionFailed == null)
			{
				return;
			}
			submissionFailed(new RankedSubmissionFailure(RankedLeaderboardService.LastSubmissionRunId, message, retainedForRetry));
		}

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x0600646C RID: 25708 RVA: 0x00282D70 File Offset: 0x00280F70
		// (remove) Token: 0x0600646D RID: 25709 RVA: 0x00282DA4 File Offset: 0x00280FA4
		public static event Action AvailabilityChanged;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x0600646E RID: 25710 RVA: 0x00282DD8 File Offset: 0x00280FD8
		// (remove) Token: 0x0600646F RID: 25711 RVA: 0x00282E0C File Offset: 0x0028100C
		public static event Action EntriesChanged;

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06006470 RID: 25712 RVA: 0x00282E3F File Offset: 0x0028103F
		public static bool IsAvailable
		{
			get
			{
				return RankedLeaderboardService._backend != null && RankedLeaderboardService._backend.IsAvailable;
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06006471 RID: 25713 RVA: 0x00282E54 File Offset: 0x00281054
		public static bool IsSupported
		{
			get
			{
				return RankedLeaderboardService._backend is IRankedLeaderboardBrowser;
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06006472 RID: 25714 RVA: 0x00282E63 File Offset: 0x00281063
		public static string CurrentAccountId
		{
			get
			{
				IRankedLeaderboardIdentity rankedLeaderboardIdentity = RankedLeaderboardService._backend as IRankedLeaderboardIdentity;
				return ((rankedLeaderboardIdentity != null) ? rankedLeaderboardIdentity.AccountId : null) ?? string.Empty;
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06006473 RID: 25715 RVA: 0x00282E84 File Offset: 0x00281084
		public static string PlatformName
		{
			get
			{
				if (!RankedLeaderboardService.IsAvailable)
				{
					return string.Empty;
				}
				return RankedLeaderboardService._backend.PlatformName;
			}
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06006474 RID: 25716 RVA: 0x00282EA0 File Offset: 0x002810A0
		public static bool CanSubmitScores
		{
			get
			{
				if (!WorkshopModManager.HasModdedSession && RankedLeaderboardService._backend != null)
				{
					IRankedLeaderboardSubmissionStatus rankedLeaderboardSubmissionStatus = RankedLeaderboardService._backend as IRankedLeaderboardSubmissionStatus;
					return rankedLeaderboardSubmissionStatus == null || rankedLeaderboardSubmissionStatus.CanSubmitScores;
				}
				return false;
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06006475 RID: 25717 RVA: 0x00282ED4 File Offset: 0x002810D4
		public static string SubmissionDisabledReason
		{
			get
			{
				if (WorkshopModManager.HasModdedSession)
				{
					return RankedPlatformPolicy.SubmissionDisabledReason;
				}
				IRankedLeaderboardSubmissionStatus rankedLeaderboardSubmissionStatus = RankedLeaderboardService._backend as IRankedLeaderboardSubmissionStatus;
				if (rankedLeaderboardSubmissionStatus != null)
				{
					return rankedLeaderboardSubmissionStatus.SubmissionDisabledReason;
				}
				if (RankedLeaderboardService._backend != null)
				{
					return string.Empty;
				}
				return RankedPlatformPolicy.SubmissionDisabledReason;
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06006476 RID: 25718 RVA: 0x00282F18 File Offset: 0x00281118
		public static bool CanResetOwnRecord
		{
			get
			{
				if (RankedLeaderboardService.IsAvailable)
				{
					IRankedLeaderboardRecordReset rankedLeaderboardRecordReset = RankedLeaderboardService._backend as IRankedLeaderboardRecordReset;
					if (rankedLeaderboardRecordReset != null)
					{
						return rankedLeaderboardRecordReset.CanResetOwnRecord;
					}
				}
				return false;
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06006477 RID: 25719 RVA: 0x00282F44 File Offset: 0x00281144
		public static bool IsResettingOwnRecord
		{
			get
			{
				IRankedLeaderboardRecordReset rankedLeaderboardRecordReset = RankedLeaderboardService._backend as IRankedLeaderboardRecordReset;
				return rankedLeaderboardRecordReset != null && rankedLeaderboardRecordReset.IsResettingOwnRecord;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06006478 RID: 25720 RVA: 0x00282F68 File Offset: 0x00281168
		public static bool HasPendingRecordReset
		{
			get
			{
				IRankedLeaderboardRecordReset rankedLeaderboardRecordReset = RankedLeaderboardService._backend as IRankedLeaderboardRecordReset;
				return rankedLeaderboardRecordReset != null && rankedLeaderboardRecordReset.HasPendingRecordReset;
			}
		}

		// Token: 0x06006479 RID: 25721 RVA: 0x00282F8C File Offset: 0x0028118C
		internal static bool IsRunSubmissionAuthorized(RankedRunResult result)
		{
			return RankedLeaderboardService._authorizedResult != null && RankedLeaderboardService._authorizedResult.Value.DistanceMeters == result.DistanceMeters && RankedLeaderboardService._authorizedResult.Value.Score == result.Score;
		}

		// Token: 0x0600647A RID: 25722 RVA: 0x00282FE0 File Offset: 0x002811E0
		public static void ResetOwnRecord(Action<RankedRecordResetResult> completed)
		{
			if (completed == null)
			{
				throw new ArgumentNullException("completed");
			}
			if (RankedLeaderboardService._authorizedResult == null && RankedLeaderboardService.CanResetOwnRecord && !RankedLeaderboardService.IsResettingOwnRecord)
			{
				IRankedLeaderboardRecordReset rankedLeaderboardRecordReset = RankedLeaderboardService._backend as IRankedLeaderboardRecordReset;
				if (rankedLeaderboardRecordReset != null)
				{
					IRankedLeaderboardBackend owner = RankedLeaderboardService._backend;
					string accountId = RankedLeaderboardService.CurrentAccountId;
					RankedLeaderboardService._lastAcceptedResult = null;
					RankedLeaderboardService.LastSubmissionRunId = string.Empty;
					RankedLeaderboardService.SetSubmissionMessage(string.Empty);
					bool delivered = false;
					Action<RankedRecordResetResult> action = delegate(RankedRecordResetResult result)
					{
						if (delivered)
						{
							return;
						}
						delivered = true;
						if (RankedLeaderboardService._backend != owner || !string.Equals(accountId, RankedLeaderboardService.CurrentAccountId, StringComparison.Ordinal))
						{
							result = new RankedRecordResetResult(false, "The Steam account changed while resetting the record.");
						}
						result = result ?? new RankedRecordResetResult(false, "Steam did not confirm the record reset.");
						if (result.Success)
						{
							RankedLeaderboardService.NotifyOwnRecordReset(owner);
						}
						completed(result);
					};
					try
					{
						rankedLeaderboardRecordReset.ResetOwnRecord(action);
					}
					catch (Exception ex)
					{
						Debug.LogWarning("[Ranked] Record reset failed: " + ex.GetType().Name);
						action(new RankedRecordResetResult(false, "The Steam record could not be reset. Please try again."));
					}
					return;
				}
			}
			completed(new RankedRecordResetResult(false, "The Steam record cannot be reset right now."));
		}

		// Token: 0x0600647B RID: 25723 RVA: 0x002830DC File Offset: 0x002812DC
		public static void NotifyOwnRecordReset(IRankedLeaderboardBackend backend)
		{
			if (backend == null || RankedLeaderboardService._backend != backend)
			{
				return;
			}
			RankedLeaderboardService._lastAcceptedResult = null;
			RankedLeaderboardService.LastSubmissionRunId = string.Empty;
			RankedLeaderboardService.SetSubmissionMessage(string.Empty);
			Action entriesChanged = RankedLeaderboardService.EntriesChanged;
			if (entriesChanged == null)
			{
				return;
			}
			entriesChanged();
		}

		// Token: 0x0600647C RID: 25724 RVA: 0x00283118 File Offset: 0x00281318
		public static IDisposable RequestEntries(RankedLeaderboardScope scope, int page, int pageSize, Action<RankedLeaderboardResult> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (!Enum.IsDefined(typeof(RankedLeaderboardScope), scope))
			{
				scope = RankedLeaderboardScope.Global;
			}
			pageSize = Mathf.Clamp(pageSize, 1, 100);
			page = ((scope == RankedLeaderboardScope.AroundPlayer) ? 0 : Mathf.Clamp(page, 0, (int.MaxValue - pageSize - 1) / pageSize));
			RankedLeaderboardService.BrowseSubscription browseSubscription = new RankedLeaderboardService.BrowseSubscription(callback);
			browseSubscription.Deliver(new RankedLeaderboardResult(scope, RankedLeaderboardState.Loading, page, pageSize, null, null, 0, null, null));
			if (RankedLeaderboardService.IsAvailable)
			{
				IRankedLeaderboardBrowser rankedLeaderboardBrowser = RankedLeaderboardService._backend as IRankedLeaderboardBrowser;
				if (rankedLeaderboardBrowser != null)
				{
					try
					{
						browseSubscription.Attach(rankedLeaderboardBrowser.RequestEntries(scope, page, pageSize, new Action<RankedLeaderboardResult>(browseSubscription.Deliver)));
					}
					catch (Exception ex)
					{
						Debug.LogWarning("[Ranked] Leaderboard request failed: " + ex.GetType().Name);
						browseSubscription.Deliver(new RankedLeaderboardResult(scope, RankedLeaderboardState.Error, page, pageSize, null, null, 0, "The leaderboard could not be loaded. Please try again.", null));
					}
					return browseSubscription;
				}
			}
			browseSubscription.Deliver(new RankedLeaderboardResult(scope, RankedLeaderboardState.Unavailable, page, pageSize, null, null, 0, "Connect to Steam to view Classic Ranked scores.", null));
			return browseSubscription;
		}

		// Token: 0x0600647D RID: 25725 RVA: 0x0028323C File Offset: 0x0028143C
		public static void RegisterBackend(IRankedLeaderboardBackend backend)
		{
			if (backend == null || RankedLeaderboardService._backend == backend)
			{
				return;
			}
			RankedLeaderboardService._lastAcceptedResult = null;
			RankedLeaderboardService._backend = backend;
			RunModeService.ValidateRankedResumeEnvironment();
			Action availabilityChanged = RankedLeaderboardService.AvailabilityChanged;
			if (availabilityChanged == null)
			{
				return;
			}
			availabilityChanged();
		}

		// Token: 0x0600647E RID: 25726 RVA: 0x00283270 File Offset: 0x00281470
		public static void UnregisterBackend(IRankedLeaderboardBackend backend)
		{
			if (RankedLeaderboardService._backend != backend)
			{
				return;
			}
			RankedLeaderboardService._lastAcceptedResult = null;
			RankedLeaderboardService._backend = null;
			Action availabilityChanged = RankedLeaderboardService.AvailabilityChanged;
			if (availabilityChanged == null)
			{
				return;
			}
			availabilityChanged();
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x0028329B File Offset: 0x0028149B
		public static void NotifyAvailabilityChanged(IRankedLeaderboardBackend backend)
		{
			if (RankedLeaderboardService._backend == backend)
			{
				Action availabilityChanged = RankedLeaderboardService.AvailabilityChanged;
				if (availabilityChanged == null)
				{
					return;
				}
				availabilityChanged();
			}
		}

		// Token: 0x06006480 RID: 25728 RVA: 0x002832B4 File Offset: 0x002814B4
		public static void NotifyEntriesChanged(IRankedLeaderboardBackend backend)
		{
			if (RankedLeaderboardService._backend == backend)
			{
				Action entriesChanged = RankedLeaderboardService.EntriesChanged;
				if (entriesChanged == null)
				{
					return;
				}
				entriesChanged();
			}
		}

		// Token: 0x06006481 RID: 25729 RVA: 0x002832D0 File Offset: 0x002814D0
		public static bool TrySubmitCurrentRunResult(float distanceMeters, int score)
		{
			if (RankedLeaderboardService._authorizedResult != null)
			{
				return false;
			}
			if (!RunModeService.IsRankedRun)
			{
				return false;
			}
			string currentRunId = RunModeService.CurrentRunId;
			if (!string.Equals(currentRunId, RankedLeaderboardService.LastSubmissionRunId, StringComparison.Ordinal))
			{
				RankedLeaderboardService._lastAcceptedResult = null;
			}
			RankedLeaderboardService.LastSubmissionRunId = currentRunId;
			if (!RankedLeaderboardService.CanSubmitScores)
			{
				RankedLeaderboardService.SetSubmissionMessage(RankedLeaderboardService.SubmissionDisabledReason);
				return false;
			}
			if (!RunModeService.ValidateRankedSessionForSubmission())
			{
				RankedLeaderboardService.SetSubmissionMessage(RunModeService.LeaderboardIneligibilityReason);
				return false;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			string text;
			if (instance == null || !instance.ValidateCompletedRankedResult(distanceMeters, score, out text))
			{
				RankedLeaderboardService.SetSubmissionMessage("The final distance and score could not be verified. No result was submitted.");
				RunModeService.DisqualifyLeaderboardRun(RankedLeaderboardService.LastSubmissionMessage);
				return false;
			}
			RankedRunResult rankedRunResult;
			if (!RankedRunResult.TryCreate(distanceMeters, score, out rankedRunResult))
			{
				RankedLeaderboardService.SetSubmissionMessage("No distance recorded. No result was submitted.");
				return false;
			}
			bool flag = false;
			bool flag4;
			try
			{
				RankedLeaderboardService._lastAcceptedResult = null;
				RankedLeaderboardService._failedDuringSubmission = false;
				RankedLeaderboardService._authorizedResult = new RankedRunResult?(rankedRunResult);
				flag = RankedLeaderboardService._backend.TrySubmitRun(rankedRunResult);
				bool flag2;
				if (flag)
				{
					IRankedLeaderboardConfirmedDistance rankedLeaderboardConfirmedDistance = RankedLeaderboardService._backend as IRankedLeaderboardConfirmedDistance;
					if (rankedLeaderboardConfirmedDistance != null && rankedLeaderboardConfirmedDistance.ConfirmedBestDistanceMeters != null)
					{
						flag2 = rankedLeaderboardConfirmedDistance.ConfirmedBestDistanceMeters.Value >= rankedRunResult.DistanceMeters;
						goto IL_0116;
					}
				}
				flag2 = false;
				IL_0116:
				bool flag3 = flag2;
				if (!RankedLeaderboardService._failedDuringSubmission)
				{
					if (flag)
					{
						RankedLeaderboardService.SetSubmissionMessage(flag3 ? "Your longest run is already confirmed on Steam." : "Run result saved for Steam submission.");
					}
					else
					{
						RankedLeaderboardService.NotifySubmissionFailed(RankedLeaderboardService._backend, rankedRunResult, "Steam could not accept the run result. No result was submitted.", false);
					}
				}
				if (flag)
				{
					RankedLeaderboardService._lastAcceptedResult = (flag3 ? null : new RankedRunResult?(rankedRunResult));
					RunModeService.MarkRankedSubmissionAccepted();
				}
				else
				{
					Debug.LogWarning("[Ranked] " + RankedLeaderboardService.LastSubmissionMessage);
				}
				flag4 = flag;
			}
			catch (Exception ex)
			{
				if (!RankedLeaderboardService._failedDuringSubmission)
				{
					RankedLeaderboardService.NotifySubmissionFailed(RankedLeaderboardService._backend, rankedRunResult, flag ? "The run result was accepted, but submission bookkeeping could not finish." : "The run result could not be submitted. Please check the Steam connection.", flag);
				}
				Debug.LogWarning("[Ranked] Score submission failed: " + ex.GetType().Name);
				flag4 = false;
			}
			finally
			{
				RankedLeaderboardService._authorizedResult = null;
			}
			return flag4;
		}

		// Token: 0x06006482 RID: 25730 RVA: 0x002834EC File Offset: 0x002816EC
		public static bool TrySubmitCurrentRunScore(int score)
		{
			MatchStatsService instance = MatchStatsService.Instance;
			RankedRunResult rankedRunResult;
			return instance != null && instance.TryGetCompletedRankedResult(out rankedRunResult) && RankedLeaderboardService.TrySubmitCurrentRunResult((float)rankedRunResult.DistanceMeters, score);
		}

		// Token: 0x0400674D RID: 26445
		private static IRankedLeaderboardBackend _backend;

		// Token: 0x0400674E RID: 26446
		private static RankedRunResult? _authorizedResult;

		// Token: 0x04006751 RID: 26449
		private static RankedRunResult? _lastAcceptedResult;

		// Token: 0x04006752 RID: 26450
		private static bool _failedDuringSubmission;

		// Token: 0x02000D5C RID: 3420
		private sealed class BrowseSubscription : IDisposable
		{
			// Token: 0x06006484 RID: 25732 RVA: 0x00283538 File Offset: 0x00281738
			public BrowseSubscription(Action<RankedLeaderboardResult> callback)
			{
				this._callback = callback;
			}

			// Token: 0x06006485 RID: 25733 RVA: 0x00283547 File Offset: 0x00281747
			public void Attach(IDisposable request)
			{
				if (this._finished)
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

			// Token: 0x06006486 RID: 25734 RVA: 0x00283562 File Offset: 0x00281762
			public void Deliver(RankedLeaderboardResult result)
			{
				if (this._finished || result == null)
				{
					return;
				}
				Action<RankedLeaderboardResult> callback = this._callback;
				if (result.State != RankedLeaderboardState.Loading)
				{
					this.Dispose();
				}
				if (callback == null)
				{
					return;
				}
				callback(result);
			}

			// Token: 0x06006487 RID: 25735 RVA: 0x0028358F File Offset: 0x0028178F
			public void Dispose()
			{
				if (this._finished)
				{
					return;
				}
				this._finished = true;
				this._callback = null;
				IDisposable request = this._request;
				if (request != null)
				{
					request.Dispose();
				}
				this._request = null;
			}

			// Token: 0x04006757 RID: 26455
			private Action<RankedLeaderboardResult> _callback;

			// Token: 0x04006758 RID: 26456
			private IDisposable _request;

			// Token: 0x04006759 RID: 26457
			private bool _finished;
		}
	}
}
