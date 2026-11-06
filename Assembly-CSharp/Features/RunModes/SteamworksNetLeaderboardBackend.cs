using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using Features.Achievements;
using Steamworks;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D7A RID: 3450
	[DisallowMultipleComponent]
	public sealed class SteamworksNetLeaderboardBackend : MonoBehaviour, IRankedLeaderboardBackend, IRankedLeaderboardBrowser, IRankedLeaderboardIdentity, IRankedLeaderboardSubmissionStatus, IRankedLeaderboardConfirmedDistance, IAchievementBackend, IRankedLeaderboardRecordReset
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06006566 RID: 25958 RVA: 0x002871E7 File Offset: 0x002853E7
		// (set) Token: 0x06006567 RID: 25959 RVA: 0x002871EE File Offset: 0x002853EE
		public static string LastInitializationError { get; private set; } = string.Empty;

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06006568 RID: 25960 RVA: 0x002871F6 File Offset: 0x002853F6
		public string PlatformName
		{
			get
			{
				return "STEAM";
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06006569 RID: 25961 RVA: 0x002871FD File Offset: 0x002853FD
		public int? ConfirmedBestDistanceMeters
		{
			get
			{
				return this._scores.ConfirmedBestDistanceMeters;
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x0600656A RID: 25962 RVA: 0x0028720A File Offset: 0x0028540A
		public string AccountId
		{
			get
			{
				if (!this._initialized || this._accountId == 0UL || SteamUser.GetSteamID().m_SteamID != this._accountId)
				{
					return string.Empty;
				}
				return this._accountIdText;
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x0600656B RID: 25963 RVA: 0x0028723A File Offset: 0x0028543A
		public static bool ScoreUploadsEnabled
		{
			get
			{
				return RankedPlatformPolicy.CanUploadScores && (!Debug.isDebugBuild || RankedPlatformPolicy.EditorTestUploadsEnabled || RankedPlatformPolicy.CurrentBuildKind == RankedBuildKind.SteamLocalQa) && !Debugger.IsAttached;
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x0600656C RID: 25964 RVA: 0x00287263 File Offset: 0x00285463
		public string SubmissionDisabledReason
		{
			get
			{
				if (!this._scores.ResetRequested)
				{
					return this.PlatformSubmissionDisabledReason;
				}
				return "A leaderboard reset is pending. Finish or retry the reset before submitting another run.";
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x0600656D RID: 25965 RVA: 0x00287280 File Offset: 0x00285480
		private string PlatformSubmissionDisabledReason
		{
			get
			{
				if (!RankedPlatformPolicy.CanUploadScores)
				{
					return RankedPlatformPolicy.SubmissionDisabledReason;
				}
				if (Debug.isDebugBuild && !RankedPlatformPolicy.EditorTestUploadsEnabled && RankedPlatformPolicy.CurrentBuildKind != RankedBuildKind.SteamLocalQa)
				{
					return "Development builds cannot submit ranked scores.";
				}
				if (Debugger.IsAttached)
				{
					return "Scores cannot be submitted while a debugger is attached.";
				}
				if (!this._initialized || !this._licenseVerified)
				{
					return "Connect to Steam with a licensed account to submit scores.";
				}
				if (!string.IsNullOrEmpty(this._uploadContextError))
				{
					return this._uploadContextError;
				}
				if (!this.HasValidAccountAndLicense())
				{
					return "The Steam account or game license changed. Restart the game to compete.";
				}
				return string.Empty;
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x0600656E RID: 25966 RVA: 0x00287300 File Offset: 0x00285500
		public bool CanSubmitScores
		{
			get
			{
				return string.IsNullOrEmpty(this.SubmissionDisabledReason);
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x0600656F RID: 25967 RVA: 0x0028730D File Offset: 0x0028550D
		public bool IsAvailable
		{
			get
			{
				return this._initialized && this._licenseVerified && SteamUser.BLoggedOn() && SteamUser.GetSteamID().m_SteamID == this._accountId;
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06006570 RID: 25968 RVA: 0x00026B5B File Offset: 0x00024D5B
		private static double Now
		{
			get
			{
				return Time.realtimeSinceStartupAsDouble;
			}
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x0028733A File Offset: 0x0028553A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			SteamworksNetLeaderboardBackend.EnsureInitialized();
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x00287344 File Offset: 0x00285544
		public static bool EnsureInitialized()
		{
			if (!Application.isPlaying)
			{
				return false;
			}
			if (!RankedPlatformPolicy.CanInitializeSteam)
			{
				SteamworksNetLeaderboardBackend.LastInitializationError = RankedPlatformPolicy.SubmissionDisabledReason;
				return false;
			}
			if (SteamworksNetLeaderboardBackend._instance == null)
			{
				SteamworksNetLeaderboardBackend._instance = Object.FindObjectOfType<SteamworksNetLeaderboardBackend>(true);
			}
			if (SteamworksNetLeaderboardBackend._instance != null)
			{
				return SteamworksNetLeaderboardBackend._instance.TryInitialize();
			}
			GameObject gameObject = new GameObject("[Steam Platform]");
			Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<SteamworksNetLeaderboardBackend>()._initialized;
		}

		// Token: 0x06006573 RID: 25971 RVA: 0x002873B7 File Offset: 0x002855B7
		private void Awake()
		{
			if (SteamworksNetLeaderboardBackend._instance != null && SteamworksNetLeaderboardBackend._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			SteamworksNetLeaderboardBackend._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.TryInitialize();
		}

		// Token: 0x06006574 RID: 25972 RVA: 0x002873F8 File Offset: 0x002855F8
		private bool TryInitialize()
		{
			if (this._initialized)
			{
				return true;
			}
			if (this._restartRequested)
			{
				return false;
			}
			if (!RankedPlatformPolicy.CanInitializeSteam)
			{
				SteamworksNetLeaderboardBackend.LastInitializationError = RankedPlatformPolicy.SubmissionDisabledReason;
				return false;
			}
			SteamworksNetLeaderboardBackend.LastInitializationError = string.Empty;
			try
			{
				if (RankedPlatformPolicy.CurrentBuildKind == RankedBuildKind.SteamLocalQa && !SteamworksNetLeaderboardBackend.HasDevelopmentAppIdFile())
				{
					SteamworksNetLeaderboardBackend.LastInitializationError = "Local Steam QA requires steam_appid.txt beside the executable.";
					Debug.LogWarning("[Ranked/Steam] " + SteamworksNetLeaderboardBackend.LastInitializationError, this);
					return false;
				}
				if (RankedPlatformPolicy.CurrentBuildKind != RankedBuildKind.SteamLocalQa && SteamAPI.RestartAppIfNecessary(new AppId_t(4688700U)))
				{
					this._restartRequested = true;
					SteamworksNetLeaderboardBackend.LastInitializationError = "Relaunching the Steam edition through the Steam client.";
					Debug.Log("[Ranked/Steam] " + SteamworksNetLeaderboardBackend.LastInitializationError, this);
					Application.Quit();
					return false;
				}
				if (!SteamAPI.IsSteamRunning())
				{
					SteamworksNetLeaderboardBackend.LastInitializationError = "The Steam client is not running. Start Steam, sign in, and retry.";
				}
				else
				{
					string text;
					ESteamAPIInitResult esteamAPIInitResult = SteamAPI.InitEx(out text);
					this._initialized = esteamAPIInitResult == ESteamAPIInitResult.k_ESteamAPIInitResult_OK;
					if (!this._initialized)
					{
						SteamworksNetLeaderboardBackend.LastInitializationError = (string.IsNullOrWhiteSpace(text) ? string.Format("Steam initialization failed ({0}).", esteamAPIInitResult) : string.Format("Steam initialization failed ({0}): {1}", esteamAPIInitResult, text.Trim()));
					}
				}
			}
			catch (DllNotFoundException ex)
			{
				SteamworksNetLeaderboardBackend.LastInitializationError = "The Steam native library could not be loaded: " + ex.Message;
			}
			catch (EntryPointNotFoundException ex2)
			{
				SteamworksNetLeaderboardBackend.LastInitializationError = "The Steam native library is incompatible: " + ex2.Message;
			}
			catch (BadImageFormatException ex3)
			{
				SteamworksNetLeaderboardBackend.LastInitializationError = "The Steam native library has the wrong architecture: " + ex3.Message;
			}
			if (!this._initialized)
			{
				Debug.LogWarning("[Ranked/Steam] " + SteamworksNetLeaderboardBackend.LastInitializationError, this);
				return false;
			}
			uint appId = SteamUtils.GetAppID().m_AppId;
			this._licenseVerified = appId == 4688700U && SteamApps.BIsSubscribed();
			if (!this._licenseVerified)
			{
				SteamworksNetLeaderboardBackend.LastInitializationError = ((appId != 4688700U) ? string.Format("Steam initialized for App ID {0}; expected {1}.", appId, 4688700U) : "The active Steam user has no license for this game.");
				SteamAPI.Shutdown();
				this._initialized = false;
				Debug.LogWarning("[Ranked/Steam] " + SteamworksNetLeaderboardBackend.LastInitializationError, this);
				return false;
			}
			this._uploadContextError = ((RankedPlatformPolicy.CurrentBuildKind == RankedBuildKind.SteamLocalQa) ? string.Empty : (SteamworksNetLeaderboardBackend.HasDevelopmentAppIdFile() ? "Steam development launch: remove steam_appid.txt from the build and launch through your Steam library to compete." : string.Empty));
			this._findResult = CallResult<LeaderboardFindResult_t>.Create(new CallResult<LeaderboardFindResult_t>.APIDispatchDelegate(this.HandleLeaderboardFound));
			this._uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create(new CallResult<LeaderboardScoreUploaded_t>.APIDispatchDelegate(this.HandleScoreUploaded));
			this._gameId = new CGameID(SteamUtils.GetAppID()).m_GameID;
			this._accountId = SteamUser.GetSteamID().m_SteamID;
			this._accountIdText = this._accountId.ToString(CultureInfo.InvariantCulture);
			this._submissionIdentity = RankedPlatformPolicy.CurrentSubmissionIdentity;
			this._pendingScoreKey = string.Format("Ranked.PendingRun.v3.{0}.{1}.{2}", this._gameId, this._accountId, "classic_ranked_distance_v1");
			if (this._submissionIdentity != "PE_STEAM_BUILD.release.v1")
			{
				this._pendingScoreKey = this._pendingScoreKey + "." + this._submissionIdentity;
			}
			this.RestorePendingRecordReset();
			if (this.CanSubmitScores && PlayerPrefs.HasKey(this._pendingScoreKey))
			{
				RankedRunResult rankedRunResult;
				if (RankedPendingScoreRecord.TryRead(PlayerPrefs.GetString(this._pendingScoreKey), appId, this._accountId, "classic_ranked_distance_v1", this._submissionIdentity, out rankedRunResult))
				{
					this._scores.Enqueue(rankedRunResult);
				}
				else
				{
					Debug.LogWarning("[Ranked/Steam] Ignoring an invalid or modified pending score record.", this);
				}
			}
			this._userStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.HandleUserStatsStored));
			this._achievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.HandleAchievementStored));
			this._userStatsReady = true;
			RankedLeaderboardService.RegisterBackend(this);
			AchievementService.RegisterBackend(this);
			this._lastAvailability = this.IsAvailable;
			return true;
		}

		// Token: 0x06006575 RID: 25973 RVA: 0x002877CC File Offset: 0x002859CC
		private void Update()
		{
			if (!this._initialized)
			{
				return;
			}
			SteamAPI.RunCallbacks();
			bool isAvailable = this.IsAvailable;
			if (isAvailable != this._lastAvailability)
			{
				this._lastAvailability = isAvailable;
				this._userStatsReady = isAvailable;
				if (isAvailable)
				{
					this._nextFindAttempt = 0.0;
					this.FlushPendingAchievements();
				}
				else
				{
					this.RequeueInFlightAchievements();
					this.FailBrowseRequests(RankedLeaderboardState.Unavailable, "Steam is offline. Reconnect and refresh the leaderboard.");
				}
				RankedLeaderboardService.NotifyAvailabilityChanged(this);
				AchievementService.NotifyAvailabilityChanged(this);
			}
			double now = SteamworksNetLeaderboardBackend.Now;
			if (this._finding && now >= this._findDeadline)
			{
				this._findResult.Cancel();
				this.HandleFindFailure("Steam did not respond in time. Please try again.");
			}
			if (this._scores.InFlightResult != null && !this._uploadTimedOut && now >= this._uploadDeadline)
			{
				this._uploadTimedOut = true;
				RankedLeaderboardService.NotifySubmissionFailed(this, this._scores.InFlightResult.Value, "Steam has not confirmed the run result yet. It remains saved for retry.", true);
				Debug.LogWarning("[Ranked/Steam] Score upload is taking longer than expected; score remains saved for retry.");
			}
			for (int i = this._browseRequests.Count - 1; i >= 0; i--)
			{
				if (i < this._browseRequests.Count)
				{
					this._browseRequests[i].CheckTimeout(now);
				}
			}
			this.UpdateRecordReset(now);
			if (!isAvailable)
			{
				return;
			}
			if (this._leaderboard == new SteamLeaderboard_t(0UL))
			{
				if (this._scores.HasPending || this._browseRequests.Count > 0)
				{
					this.RequestLeaderboard();
					return;
				}
			}
			else
			{
				this.UploadPendingScore();
			}
		}

		// Token: 0x06006576 RID: 25974 RVA: 0x00287940 File Offset: 0x00285B40
		private void OnDestroy()
		{
			if (SteamworksNetLeaderboardBackend._instance != this)
			{
				return;
			}
			SteamworksNetLeaderboardBackend._instance = null;
			this.DisposeRecordReset();
			this.FailBrowseRequests(RankedLeaderboardState.Unavailable, "Steam is unavailable.");
			RankedLeaderboardService.UnregisterBackend(this);
			AchievementService.UnregisterBackend(this);
			CallResult<LeaderboardFindResult_t> findResult = this._findResult;
			if (findResult != null)
			{
				findResult.Dispose();
			}
			CallResult<LeaderboardScoreUploaded_t> uploadResult = this._uploadResult;
			if (uploadResult != null)
			{
				uploadResult.Dispose();
			}
			Callback<UserStatsStored_t> userStatsStored = this._userStatsStored;
			if (userStatsStored != null)
			{
				userStatsStored.Dispose();
			}
			Callback<UserAchievementStored_t> achievementStored = this._achievementStored;
			if (achievementStored != null)
			{
				achievementStored.Dispose();
			}
			if (!this._initialized)
			{
				return;
			}
			SteamAPI.Shutdown();
			this._initialized = false;
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x002879D8 File Offset: 0x00285BD8
		public bool TrySubmitRun(RankedRunResult result)
		{
			if (!this.CanSubmitScores)
			{
				Debug.Log("[Ranked/Steam] " + this.SubmissionDisabledReason);
				return false;
			}
			if (!result.IsValid || !RankedLeaderboardService.IsRunSubmissionAuthorized(result))
			{
				return false;
			}
			if (!this._initialized || this._accountId == 0UL || SteamUser.GetSteamID().m_SteamID != this._accountId)
			{
				return false;
			}
			if (this._scores.ConfirmedBestDistanceMeters != null && result.DistanceMeters <= this._scores.ConfirmedBestDistanceMeters.Value)
			{
				return true;
			}
			RankedRunResult? bestUnconfirmedResult = this._scores.BestUnconfirmedResult;
			RankedRunResult rankedRunResult = ((bestUnconfirmedResult != null && bestUnconfirmedResult.Value.DistanceMeters >= result.DistanceMeters) ? bestUnconfirmedResult.Value : result);
			if (!this.WritePendingResult(new RankedRunResult?(rankedRunResult)))
			{
				return false;
			}
			this._scores.Enqueue(result);
			if (!this.IsAvailable)
			{
				RankedLeaderboardService.NotifySubmissionFailed(this, result, "Steam is offline. The run result is saved and will retry when the connection returns.", true);
				return true;
			}
			if (this._leaderboard == new SteamLeaderboard_t(0UL))
			{
				this.RequestLeaderboard();
			}
			else
			{
				this.UploadPendingScore();
			}
			return true;
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x00287AFC File Offset: 0x00285CFC
		public IDisposable RequestEntries(RankedLeaderboardScope scope, int page, int pageSize, Action<RankedLeaderboardResult> completed)
		{
			SteamworksNetLeaderboardBackend.BrowseRequest browseRequest = new SteamworksNetLeaderboardBackend.BrowseRequest(this, scope, page, pageSize, completed);
			this._browseRequests.Add(browseRequest);
			if (!this.IsAvailable)
			{
				browseRequest.Fail(RankedLeaderboardState.Unavailable, "Connect to Steam to view Classic Ranked scores.");
			}
			else if (this._leaderboard != new SteamLeaderboard_t(0UL))
			{
				browseRequest.Start();
			}
			else if (!this._finding && SteamworksNetLeaderboardBackend.Now < this._nextFindAttempt)
			{
				browseRequest.Fail(RankedLeaderboardState.Error, "Steam is temporarily unavailable. Please try again shortly.");
			}
			else
			{
				this.RequestLeaderboard();
			}
			return browseRequest;
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x00287B7D File Offset: 0x00285D7D
		public bool TryUnlock(string apiName)
		{
			if (!this.IsAvailable || string.IsNullOrWhiteSpace(apiName))
			{
				return false;
			}
			this._pendingAchievements.Add(apiName.Trim());
			if (this._userStatsReady)
			{
				this.FlushPendingAchievements();
			}
			return true;
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x00287BB4 File Offset: 0x00285DB4
		private void RequestLeaderboard()
		{
			if (!this.IsAvailable || this._finding || SteamworksNetLeaderboardBackend.Now < this._nextFindAttempt || this._leaderboard != new SteamLeaderboard_t(0UL))
			{
				return;
			}
			this._finding = true;
			this._findDeadline = SteamworksNetLeaderboardBackend.Now + 25.0;
			SteamAPICall_t steamAPICall_t;
			try
			{
				steamAPICall_t = SteamUserStats.FindLeaderboard("classic_ranked_distance_v1");
			}
			catch (Exception ex)
			{
				this.HandleFindFailure("Steam could not resolve the leaderboard. Please try again.");
				Debug.LogWarning("[Ranked/Steam] Leaderboard lookup failed: " + ex.GetType().Name);
				return;
			}
			if (steamAPICall_t == SteamAPICall_t.Invalid)
			{
				this.HandleFindFailure("The Classic Ranked leaderboard is unavailable. Please try again.");
				return;
			}
			this._findResult.Set(steamAPICall_t, null);
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x00287C7C File Offset: 0x00285E7C
		private void HandleLeaderboardFound(LeaderboardFindResult_t result, bool ioFailure)
		{
			this._finding = false;
			if (ioFailure || result.m_bLeaderboardFound == 0 || result.m_hSteamLeaderboard == new SteamLeaderboard_t(0UL))
			{
				this.HandleFindFailure("The Classic Ranked leaderboard is unavailable. Please try again later.");
				return;
			}
			this._leaderboard = result.m_hSteamLeaderboard;
			if (SteamUserStats.GetLeaderboardSortMethod(this._leaderboard) != ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending || SteamUserStats.GetLeaderboardDisplayType(this._leaderboard) != ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric)
			{
				this._leaderboard = new SteamLeaderboard_t(0UL);
				this.HandleFindFailure("The Classic Ranked leaderboard is not configured correctly.");
				return;
			}
			SteamworksNetLeaderboardBackend.BrowseRequest[] array = this._browseRequests.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Start();
			}
			this.UploadPendingScore();
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x00287D24 File Offset: 0x00285F24
		private void HandleFindFailure(string message)
		{
			this._finding = false;
			this._nextFindAttempt = SteamworksNetLeaderboardBackend.Now + 30.0;
			this.FailRecordResetIfWaiting(message);
			this.FailBrowseRequests(RankedLeaderboardState.Error, message);
			if (this._scores.BestUnconfirmedResult != null)
			{
				RankedLeaderboardService.NotifySubmissionFailed(this, this._scores.BestUnconfirmedResult.Value, message + " The run result remains saved for retry.", true);
			}
			Debug.LogWarning("[Ranked/Steam] " + message);
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x00287DA8 File Offset: 0x00285FA8
		private void FailBrowseRequests(RankedLeaderboardState state, string message)
		{
			SteamworksNetLeaderboardBackend.BrowseRequest[] array = this._browseRequests.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Fail(state, message);
			}
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x00287DDC File Offset: 0x00285FDC
		private void UploadPendingScore()
		{
			RankedRunResult rankedRunResult;
			if (!this.CanSubmitScores || !this.IsAvailable || this._leaderboard == new SteamLeaderboard_t(0UL) || !this._scores.TryBeginUpload(SteamworksNetLeaderboardBackend.Now, out rankedRunResult))
			{
				return;
			}
			this._uploadTimedOut = false;
			this._uploadDeadline = SteamworksNetLeaderboardBackend.Now + 25.0;
			SteamAPICall_t steamAPICall_t;
			try
			{
				steamAPICall_t = SteamUserStats.UploadLeaderboardScore(this._leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, rankedRunResult.DistanceMeters, new int[] { 1, rankedRunResult.Score }, 2);
			}
			catch (Exception ex)
			{
				this._scores.CompleteUpload(false, SteamworksNetLeaderboardBackend.Now);
				RankedLeaderboardService.NotifySubmissionFailed(this, rankedRunResult, "Steam could not start the upload. The run result remains saved for retry.", true);
				Debug.LogWarning("[Ranked/Steam] Score upload failed: " + ex.GetType().Name);
				return;
			}
			if (steamAPICall_t == SteamAPICall_t.Invalid)
			{
				this._scores.CompleteUpload(false, SteamworksNetLeaderboardBackend.Now);
				this.PersistPendingScore();
				RankedLeaderboardService.NotifySubmissionFailed(this, rankedRunResult, "Steam could not start the upload. The run result remains saved for retry.", true);
				return;
			}
			this._uploadResult.Set(steamAPICall_t, null);
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x00287EF4 File Offset: 0x002860F4
		private void HandleScoreUploaded(LeaderboardScoreUploaded_t result, bool ioFailure)
		{
			RankedRunResult? inFlightResult = this._scores.InFlightResult;
			bool flag = !ioFailure && result.m_bSuccess > 0;
			this._scores.CompleteUpload(flag, SteamworksNetLeaderboardBackend.Now);
			this.PersistPendingScore();
			if (!flag)
			{
				if (inFlightResult != null)
				{
					RankedLeaderboardService.NotifySubmissionFailed(this, inFlightResult.Value, "Steam could not confirm the run result. It remains saved for retry.", true);
				}
				Debug.LogWarning("[Ranked/Steam] Run upload failed; the farthest run remains queued.");
				return;
			}
			RankedLeaderboardService.NotifySubmissionConfirmed(this, result.m_nScore);
			RankedLeaderboardService.NotifyEntriesChanged(this);
			Debug.Log(string.Format("[Ranked/Steam] Distance {0} m acknowledged. Global rank: {1}.", result.m_nScore, result.m_nGlobalRankNew));
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x00287F98 File Offset: 0x00286198
		private void PersistPendingScore()
		{
			try
			{
				if (!this.WritePendingResult(this._scores.BestUnconfirmedResult))
				{
					Debug.LogWarning("[Ranked/Steam] The saved retry record could not be updated.");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Ranked/Steam] Could not update the saved retry record: " + ex.GetType().Name);
			}
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x00287FF4 File Offset: 0x002861F4
		private bool WritePendingResult(RankedRunResult? result)
		{
			if (string.IsNullOrEmpty(this._pendingScoreKey) || this._accountId == 0UL)
			{
				return false;
			}
			if (result != null)
			{
				string text;
				if (!RankedPendingScoreRecord.TryCreate(4688700U, this._accountId, "classic_ranked_distance_v1", result.Value, this._submissionIdentity, out text))
				{
					return false;
				}
				PlayerPrefs.SetString(this._pendingScoreKey, text);
			}
			else
			{
				PlayerPrefs.DeleteKey(this._pendingScoreKey);
			}
			PlayerPrefs.Save();
			return true;
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x00288068 File Offset: 0x00286268
		private bool HasValidAccountAndLicense()
		{
			return this._initialized && this._accountId != 0UL && SteamUtils.GetAppID().m_AppId == 4688700U && SteamUser.GetSteamID().m_SteamID == this._accountId && SteamApps.BIsSubscribed();
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x002880A4 File Offset: 0x002862A4
		private static bool HasDevelopmentAppIdFile()
		{
			bool flag;
			try
			{
				flag = File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "steam_appid.txt")) || File.Exists(Path.Combine(Path.GetDirectoryName(Application.dataPath) ?? string.Empty, "steam_appid.txt"));
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException)
			{
				flag = RankedPlatformPolicy.CurrentBuildKind != RankedBuildKind.SteamLocalQa;
			}
			return flag;
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x00288140 File Offset: 0x00286340
		private static int? ReadRunScoreDetails(int[] details, int detailCount)
		{
			if (details == null || details.Length < 2 || detailCount != 2 || details[0] != 1 || details[1] < 0 || details[1] == 2147483647)
			{
				return null;
			}
			return new int?(details[1]);
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x00288184 File Offset: 0x00286384
		private void FlushPendingAchievements()
		{
			if (!this.IsAvailable || !this._userStatsReady || this._statsStorePending || this._pendingAchievements.Count == 0)
			{
				return;
			}
			List<string> list = new List<string>(this._pendingAchievements);
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				bool flag2;
				if (!SteamUserStats.GetAchievement(text, out flag2))
				{
					Debug.LogWarning("[Achievements/Steam] Unknown or unpublished API Name '" + text + "'.");
				}
				else
				{
					this._pendingAchievements.Remove(text);
					if (!flag2)
					{
						if (!SteamUserStats.SetAchievement(text))
						{
							this._pendingAchievements.Add(text);
							Debug.LogWarning("[Achievements/Steam] SetAchievement failed for '" + text + "'.");
						}
						else
						{
							this._inFlightAchievements.Add(text);
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this._statsStorePending = SteamUserStats.StoreStats();
			if (!this._statsStorePending)
			{
				Debug.LogWarning("[Achievements/Steam] StoreStats was rejected; unlocks remain queued.");
				this.RequeueInFlightAchievements();
			}
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x00288278 File Offset: 0x00286478
		private void HandleUserStatsStored(UserStatsStored_t result)
		{
			if (result.m_nGameID != this._gameId)
			{
				return;
			}
			this._statsStorePending = false;
			if (result.m_eResult == EResult.k_EResultOK)
			{
				this._inFlightAchievements.Clear();
				this.FlushPendingAchievements();
				return;
			}
			Debug.LogWarning(string.Format("[Achievements/Steam] StoreStats failed ({0}); unlocks remain queued.", result.m_eResult));
			this.RequeueInFlightAchievements();
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x002882D6 File Offset: 0x002864D6
		private void HandleAchievementStored(UserAchievementStored_t result)
		{
			if (result.m_nGameID == this._gameId && result.m_nCurProgress == 0U && result.m_nMaxProgress == 0U)
			{
				Debug.Log("[Achievements/Steam] Unlocked '" + result.m_rgchAchievementName + "'.");
			}
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x00288314 File Offset: 0x00286514
		private void RequeueInFlightAchievements()
		{
			foreach (string text in this._inFlightAchievements)
			{
				this._pendingAchievements.Add(text);
			}
			this._inFlightAchievements.Clear();
			this._statsStorePending = false;
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06006589 RID: 25993 RVA: 0x00288380 File Offset: 0x00286580
		public bool IsResettingOwnRecord
		{
			get
			{
				return this._recordResetActive || this._scores.IsResetInFlight;
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x0600658A RID: 25994 RVA: 0x00288397 File Offset: 0x00286597
		public bool HasPendingRecordReset
		{
			get
			{
				return this._scores.ResetRequested;
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x0600658B RID: 25995 RVA: 0x002883A4 File Offset: 0x002865A4
		public bool CanResetOwnRecord
		{
			get
			{
				return !this.IsResettingOwnRecord && this.IsAvailable && string.IsNullOrEmpty(this.PlatformSubmissionDisabledReason);
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x0600658C RID: 25996 RVA: 0x002883C3 File Offset: 0x002865C3
		private string RecordResetIntentKey
		{
			get
			{
				return string.Format("Ranked.RecordReset.v1.{0}.{1}.{2}", this._gameId, this._accountId, "classic_ranked_distance_v1");
			}
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x002883EA File Offset: 0x002865EA
		private void RestorePendingRecordReset()
		{
			if (this._accountId == 0UL || PlayerPrefs.GetInt(this.RecordResetIntentKey, 0) != 1)
			{
				return;
			}
			this._scores.RequestReset();
			this._resumeRecordReset = true;
		}

		// Token: 0x0600658E RID: 25998 RVA: 0x00288418 File Offset: 0x00286618
		public void ResetOwnRecord(Action<RankedRecordResetResult> completed)
		{
			if (completed == null)
			{
				throw new ArgumentNullException("completed");
			}
			if (!this.CanResetOwnRecord)
			{
				completed(new RankedRecordResetResult(false, "Connect to Steam in an upload-enabled session to reset your record."));
				return;
			}
			bool flag = PlayerPrefs.HasKey(this.RecordResetIntentKey);
			int @int = PlayerPrefs.GetInt(this.RecordResetIntentKey, 0);
			try
			{
				PlayerPrefs.SetInt(this.RecordResetIntentKey, 1);
				PlayerPrefs.Save();
			}
			catch (Exception ex)
			{
				this.RestoreRecordResetIntent(flag, @int);
				Debug.LogWarning("[Ranked/Steam] Could not persist record reset: " + ex.GetType().Name);
				completed(new RankedRecordResetResult(false, "The reset could not be saved on this device. No reset was sent."));
				return;
			}
			this._scores.RequestReset();
			this._recordResetCompleted = completed;
			this._resumeRecordReset = false;
			this.BeginRecordReset();
		}

		// Token: 0x0600658F RID: 25999 RVA: 0x002884E0 File Offset: 0x002866E0
		private void BeginRecordReset()
		{
			this._recordResetActive = true;
			this._recordResetDeadline = SteamworksNetLeaderboardBackend.Now + 30.0;
			RankedLeaderboardService.NotifyAvailabilityChanged(this);
		}

		// Token: 0x06006590 RID: 26000 RVA: 0x00288504 File Offset: 0x00286704
		private void UpdateRecordReset(double now)
		{
			if (this._resumeRecordReset && !this._recordResetActive && !this._scores.IsResetInFlight && this._scores.ResetRequested && this.IsAvailable && string.IsNullOrEmpty(this.PlatformSubmissionDisabledReason))
			{
				this._resumeRecordReset = false;
				this.BeginRecordReset();
			}
			if (!this._recordResetActive)
			{
				return;
			}
			if (!this.IsAvailable || !string.IsNullOrEmpty(this.PlatformSubmissionDisabledReason))
			{
				this.FinishRecordReset(new RankedRecordResetResult(false, "Steam disconnected or the account changed. Reconnect and retry the pending reset."));
				return;
			}
			if (now >= this._recordResetDeadline)
			{
				this.FinishRecordReset(new RankedRecordResetResult(false, this._scores.IsResetInFlight ? "Steam has not confirmed the reset yet. Please wait for its response." : "Steam is still processing an earlier upload or its upload limit. Please retry the reset shortly."));
				return;
			}
			if (this._scores.IsResetInFlight || this._scores.InFlightResult != null)
			{
				return;
			}
			if (this._leaderboard == new SteamLeaderboard_t(0UL))
			{
				this.RequestLeaderboard();
				return;
			}
			if (!this._scores.TryBeginResetUpload(now))
			{
				return;
			}
			this._recordResetDeadline = now + 30.0;
			this._recordResetResult = this._recordResetResult ?? CallResult<LeaderboardScoreUploaded_t>.Create(new CallResult<LeaderboardScoreUploaded_t>.APIDispatchDelegate(this.HandleRecordReset));
			SteamAPICall_t steamAPICall_t;
			try
			{
				SteamLeaderboard_t leaderboard = this._leaderboard;
				ELeaderboardUploadScoreMethod eleaderboardUploadScoreMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate;
				int num = 0;
				int[] array = new int[2];
				array[0] = 1;
				steamAPICall_t = SteamUserStats.UploadLeaderboardScore(leaderboard, eleaderboardUploadScoreMethod, num, array, 2);
			}
			catch (Exception ex)
			{
				this._scores.CompleteResetUpload(false, now);
				Debug.LogWarning("[Ranked/Steam] Record reset failed: " + ex.GetType().Name);
				this.FinishRecordReset(new RankedRecordResetResult(false, "Steam could not start the reset. Please retry."));
				return;
			}
			if (steamAPICall_t == SteamAPICall_t.Invalid)
			{
				this._scores.CompleteResetUpload(false, now);
				this.FinishRecordReset(new RankedRecordResetResult(false, "Steam could not start the reset. Please retry."));
				return;
			}
			this._recordResetResult.Set(steamAPICall_t, null);
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x002886DC File Offset: 0x002868DC
		private void HandleRecordReset(LeaderboardScoreUploaded_t result, bool ioFailure)
		{
			bool flag = this._initialized && this._accountId != 0UL && SteamUser.GetSteamID().m_SteamID == this._accountId;
			if (ioFailure || result.m_bSuccess == 0 || result.m_nScore != 0 || !(result.m_hSteamLeaderboard == this._leaderboard) || !flag)
			{
				this._scores.CompleteResetUpload(false, SteamworksNetLeaderboardBackend.Now);
				this.FinishRecordReset(new RankedRecordResetResult(false, flag ? "Steam could not confirm the reset. Your previous result will not be uploaded again; retry the reset." : "The Steam account changed. Sign in to the original account and retry the reset."));
				return;
			}
			try
			{
				this.ClearPendingRecordsAfterReset();
				PlayerPrefs.DeleteKey(this.RecordResetIntentKey);
				PlayerPrefs.Save();
			}
			catch (Exception ex)
			{
				this.RestoreRecordResetIntent(true, 1);
				this._scores.CompleteResetUpload(false, SteamworksNetLeaderboardBackend.Now);
				Debug.LogWarning("[Ranked/Steam] Could not finish local reset cleanup: " + ex.GetType().Name);
				this.FinishRecordReset(new RankedRecordResetResult(false, "Steam reset the record, but this device could not finish saving it. Please retry the reset."));
				return;
			}
			this._scores.CompleteResetUpload(true, SteamworksNetLeaderboardBackend.Now);
			this.FinishRecordReset(new RankedRecordResetResult(true, null));
		}

		// Token: 0x06006592 RID: 26002 RVA: 0x002887F8 File Offset: 0x002869F8
		private void ClearPendingRecordsAfterReset()
		{
			string text = this._gameId.ToString() + "." + this._accountId.ToString() + ".";
			foreach (string text2 in new string[]
			{
				"Ranked.PendingRun.v3." + text + "classic_ranked_distance_v1",
				"Ranked.PendingScore.v2." + text + "classic_ranked_distance_v1",
				"Ranked.PendingScore.v2." + text + "classic_ranked_score_v1",
				"Ranked.PendingScore." + text + "classic_ranked_score_v1"
			})
			{
				PlayerPrefs.DeleteKey(text2);
				PlayerPrefs.DeleteKey(text2 + ".PE_STEAM_EDITOR_TEST.v1");
				PlayerPrefs.DeleteKey(text2 + ".PE_STEAM_LOCAL_QA.v1");
			}
			if (!string.IsNullOrEmpty(this._pendingScoreKey))
			{
				PlayerPrefs.DeleteKey(this._pendingScoreKey);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06006593 RID: 26003 RVA: 0x002888D4 File Offset: 0x00286AD4
		private void RestoreRecordResetIntent(bool existed, int value)
		{
			try
			{
				if (existed)
				{
					PlayerPrefs.SetInt(this.RecordResetIntentKey, value);
				}
				else
				{
					PlayerPrefs.DeleteKey(this.RecordResetIntentKey);
				}
				PlayerPrefs.Save();
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Ranked/Steam] Could not restore the reset marker: " + ex.GetType().Name);
			}
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x00288934 File Offset: 0x00286B34
		private void FailRecordResetIfWaiting(string message)
		{
			if (this._recordResetActive && !this._scores.IsResetInFlight)
			{
				this.FinishRecordReset(new RankedRecordResetResult(false, message + " Retry the pending reset."));
			}
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x00288964 File Offset: 0x00286B64
		private void FinishRecordReset(RankedRecordResetResult result)
		{
			Action<RankedRecordResetResult> recordResetCompleted = this._recordResetCompleted;
			this._recordResetCompleted = null;
			this._recordResetActive = false;
			RankedLeaderboardService.NotifyAvailabilityChanged(this);
			if (result.Success && recordResetCompleted == null)
			{
				RankedLeaderboardService.NotifyOwnRecordReset(this);
			}
			if (recordResetCompleted != null)
			{
				recordResetCompleted(result);
			}
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x002889A8 File Offset: 0x00286BA8
		private void DisposeRecordReset()
		{
			CallResult<LeaderboardScoreUploaded_t> recordResetResult = this._recordResetResult;
			if (recordResetResult != null)
			{
				recordResetResult.Dispose();
			}
			this._recordResetResult = null;
			Action<RankedRecordResetResult> recordResetCompleted = this._recordResetCompleted;
			this._recordResetCompleted = null;
			this._recordResetActive = false;
			this._resumeRecordReset = false;
			if (recordResetCompleted == null)
			{
				return;
			}
			recordResetCompleted(new RankedRecordResetResult(false, "Steam closed before the reset finished. The pending reset will resume next time."));
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x06006597 RID: 26007 RVA: 0x002889FD File Offset: 0x00286BFD
		internal bool WorkshopNativeSessionActive
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x0400680B RID: 26635
		public const string LeaderboardName = "classic_ranked_distance_v1";

		// Token: 0x0400680C RID: 26636
		private const int RunDetailsVersion = 1;

		// Token: 0x0400680D RID: 26637
		private const double RequestTimeoutSeconds = 25.0;

		// Token: 0x0400680E RID: 26638
		private static SteamworksNetLeaderboardBackend _instance;

		// Token: 0x0400680F RID: 26639
		private CallResult<LeaderboardFindResult_t> _findResult;

		// Token: 0x04006810 RID: 26640
		private CallResult<LeaderboardScoreUploaded_t> _uploadResult;

		// Token: 0x04006811 RID: 26641
		private Callback<UserStatsStored_t> _userStatsStored;

		// Token: 0x04006812 RID: 26642
		private Callback<UserAchievementStored_t> _achievementStored;

		// Token: 0x04006813 RID: 26643
		private SteamLeaderboard_t _leaderboard;

		// Token: 0x04006814 RID: 26644
		private readonly HashSet<string> _pendingAchievements = new HashSet<string>();

		// Token: 0x04006815 RID: 26645
		private readonly HashSet<string> _inFlightAchievements = new HashSet<string>();

		// Token: 0x04006816 RID: 26646
		private readonly List<SteamworksNetLeaderboardBackend.BrowseRequest> _browseRequests = new List<SteamworksNetLeaderboardBackend.BrowseRequest>();

		// Token: 0x04006817 RID: 26647
		private readonly RankedScoreSubmissionQueue _scores = new RankedScoreSubmissionQueue();

		// Token: 0x04006818 RID: 26648
		private bool _initialized;

		// Token: 0x04006819 RID: 26649
		private bool _restartRequested;

		// Token: 0x0400681A RID: 26650
		private bool _licenseVerified;

		// Token: 0x0400681B RID: 26651
		private string _uploadContextError = string.Empty;

		// Token: 0x0400681C RID: 26652
		private bool _lastAvailability;

		// Token: 0x0400681D RID: 26653
		private bool _userStatsReady;

		// Token: 0x0400681E RID: 26654
		private bool _statsStorePending;

		// Token: 0x0400681F RID: 26655
		private bool _finding;

		// Token: 0x04006820 RID: 26656
		private bool _uploadTimedOut;

		// Token: 0x04006821 RID: 26657
		private double _findDeadline;

		// Token: 0x04006822 RID: 26658
		private double _nextFindAttempt;

		// Token: 0x04006823 RID: 26659
		private double _uploadDeadline;

		// Token: 0x04006824 RID: 26660
		private ulong _gameId;

		// Token: 0x04006825 RID: 26661
		private ulong _accountId;

		// Token: 0x04006826 RID: 26662
		private string _accountIdText = string.Empty;

		// Token: 0x04006827 RID: 26663
		private string _pendingScoreKey;

		// Token: 0x04006828 RID: 26664
		private string _submissionIdentity = "PE_STEAM_BUILD.release.v1";

		// Token: 0x0400682A RID: 26666
		private const double RecordResetTimeoutSeconds = 30.0;

		// Token: 0x0400682B RID: 26667
		private CallResult<LeaderboardScoreUploaded_t> _recordResetResult;

		// Token: 0x0400682C RID: 26668
		private Action<RankedRecordResetResult> _recordResetCompleted;

		// Token: 0x0400682D RID: 26669
		private bool _recordResetActive;

		// Token: 0x0400682E RID: 26670
		private bool _resumeRecordReset;

		// Token: 0x0400682F RID: 26671
		private double _recordResetDeadline;

		// Token: 0x02000D7B RID: 3451
		private sealed class BrowseRequest : IDisposable
		{
			// Token: 0x0600659A RID: 26010 RVA: 0x00288A74 File Offset: 0x00286C74
			public BrowseRequest(SteamworksNetLeaderboardBackend owner, RankedLeaderboardScope scope, int page, int pageSize, Action<RankedLeaderboardResult> completed)
			{
				this._owner = owner;
				this._scope = scope;
				this._page = page;
				this._pageSize = pageSize;
				this._completed = completed;
				this._deadline = SteamworksNetLeaderboardBackend.Now + 25.0;
			}

			// Token: 0x0600659B RID: 26011 RVA: 0x00288ACC File Offset: 0x00286CCC
			public void Start()
			{
				if (this._disposed || this._started)
				{
					return;
				}
				this._started = true;
				int num = ((this._scope == RankedLeaderboardScope.AroundPlayer) ? (-(this._pageSize / 2)) : (this._page * this._pageSize + 1));
				int num2 = num + this._pageSize - ((this._scope == RankedLeaderboardScope.Global) ? 0 : 1);
				ELeaderboardDataRequest eleaderboardDataRequest = ((this._scope == RankedLeaderboardScope.Friends) ? ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends : ((this._scope == RankedLeaderboardScope.AroundPlayer) ? ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser : ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal));
				this._pageCall = CallResult<LeaderboardScoresDownloaded_t>.Create(new CallResult<LeaderboardScoresDownloaded_t>.APIDispatchDelegate(this.HandlePage));
				SteamAPICall_t steamAPICall_t = SteamUserStats.DownloadLeaderboardEntries(this._owner._leaderboard, eleaderboardDataRequest, num, num2);
				if (steamAPICall_t == SteamAPICall_t.Invalid)
				{
					this.Fail(RankedLeaderboardState.Error, "Steam could not load this leaderboard. Please refresh.");
					return;
				}
				this._pageCall.Set(steamAPICall_t, null);
				if (this._scope == RankedLeaderboardScope.AroundPlayer)
				{
					return;
				}
				this._localCall = CallResult<LeaderboardScoresDownloaded_t>.Create(new CallResult<LeaderboardScoresDownloaded_t>.APIDispatchDelegate(this.HandleLocal));
				steamAPICall_t = SteamUserStats.DownloadLeaderboardEntries(this._owner._leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, 0, 0);
				if (steamAPICall_t == SteamAPICall_t.Invalid)
				{
					this.Fail(RankedLeaderboardState.Error, "Steam could not load your personal best. Please refresh.");
					return;
				}
				this._localCall.Set(steamAPICall_t, null);
			}

			// Token: 0x0600659C RID: 26012 RVA: 0x00288BF4 File Offset: 0x00286DF4
			private void HandlePage(LeaderboardScoresDownloaded_t result, bool ioFailure)
			{
				if (this._disposed)
				{
					return;
				}
				if (ioFailure || result.m_hSteamLeaderboard != this._owner._leaderboard)
				{
					this.Fail(RankedLeaderboardState.Error, "Steam could not load this leaderboard. Please refresh.");
					return;
				}
				List<RankedLeaderboardEntry> list = this.ReadEntries(result);
				if (list == null)
				{
					return;
				}
				list.Sort((RankedLeaderboardEntry a, RankedLeaderboardEntry b) => a.GlobalRank.CompareTo(b.GlobalRank));
				this._total = ((this._scope == RankedLeaderboardScope.Friends) ? list.Count : (-1));
				if (this._scope == RankedLeaderboardScope.Friends)
				{
					int num = Math.Min(list.Count, this._page * this._pageSize);
					this._entries = list.GetRange(num, Math.Min(this._pageSize, list.Count - num));
					this._hasNextPage = (long)num + (long)this._entries.Count < (long)list.Count;
				}
				else if (this._scope == RankedLeaderboardScope.Global)
				{
					this._hasNextPage = list.Count > this._pageSize;
					this._entries = list.GetRange(0, Math.Min(list.Count, this._pageSize));
				}
				else
				{
					this._entries = list;
				}
				if (this._scope == RankedLeaderboardScope.AroundPlayer)
				{
					this._localEntry = list.Find((RankedLeaderboardEntry entry) => entry.IsLocalPlayer);
					this._localReady = true;
					if (this._localEntry == null)
					{
						this._entries.Clear();
					}
				}
				this.TryComplete();
			}

			// Token: 0x0600659D RID: 26013 RVA: 0x00288D74 File Offset: 0x00286F74
			private void HandleLocal(LeaderboardScoresDownloaded_t result, bool ioFailure)
			{
				if (this._disposed)
				{
					return;
				}
				if (ioFailure || result.m_hSteamLeaderboard != this._owner._leaderboard)
				{
					this.Fail(RankedLeaderboardState.Error, "Steam could not load your personal best. Please refresh.");
					return;
				}
				List<RankedLeaderboardEntry> list = this.ReadEntries(result);
				if (list == null)
				{
					return;
				}
				this._localEntry = list.Find((RankedLeaderboardEntry entry) => entry.IsLocalPlayer);
				this._localReady = true;
				this.TryComplete();
			}

			// Token: 0x0600659E RID: 26014 RVA: 0x00288DF8 File Offset: 0x00286FF8
			private List<RankedLeaderboardEntry> ReadEntries(LeaderboardScoresDownloaded_t result)
			{
				List<RankedLeaderboardEntry> list = new List<RankedLeaderboardEntry>(Math.Max(0, result.m_cEntryCount));
				int[] array = new int[2];
				for (int i = 0; i < result.m_cEntryCount; i++)
				{
					array[0] = (array[1] = 0);
					LeaderboardEntry_t leaderboardEntry_t;
					if (!SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i, out leaderboardEntry_t, array, array.Length))
					{
						this.Fail(RankedLeaderboardState.Error, "Steam returned incomplete scores. Please refresh.");
						return null;
					}
					if (leaderboardEntry_t.m_nScore > 0)
					{
						bool flag = leaderboardEntry_t.m_steamIDUser.m_SteamID == this._owner._accountId;
						string text = (flag ? SteamFriends.GetPersonaName() : SteamFriends.GetFriendPersonaName(leaderboardEntry_t.m_steamIDUser));
						list.Add(new RankedLeaderboardEntry(leaderboardEntry_t.m_steamIDUser.m_SteamID.ToString(), text, leaderboardEntry_t.m_nGlobalRank, leaderboardEntry_t.m_nScore, SteamworksNetLeaderboardBackend.ReadRunScoreDetails(array, leaderboardEntry_t.m_cDetails), flag));
					}
				}
				return list;
			}

			// Token: 0x0600659F RID: 26015 RVA: 0x00288ED8 File Offset: 0x002870D8
			private void TryComplete()
			{
				if (this._entries == null || !this._localReady || this._disposed)
				{
					return;
				}
				if (!this._namesRequested)
				{
					this._namesRequested = true;
					this._nameDeadline = Math.Min(this._deadline, SteamworksNetLeaderboardBackend.Now + 3.0);
					if (this._entries.Count > 0)
					{
						this._personaChanged = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(this.HandlePersonaChanged));
						foreach (RankedLeaderboardEntry rankedLeaderboardEntry in this._entries)
						{
							ulong num;
							if (!rankedLeaderboardEntry.IsLocalPlayer && ulong.TryParse(rankedLeaderboardEntry.UserId, out num) && SteamFriends.RequestUserInformation(new CSteamID(num), true))
							{
								this._pendingNames.Add(num);
							}
						}
					}
				}
				if (this._pendingNames.Count > 0)
				{
					return;
				}
				for (int i = 0; i < this._entries.Count; i++)
				{
					RankedLeaderboardEntry rankedLeaderboardEntry2 = this._entries[i];
					ulong num2;
					if (!rankedLeaderboardEntry2.IsLocalPlayer && ulong.TryParse(rankedLeaderboardEntry2.UserId, out num2))
					{
						this._entries[i] = new RankedLeaderboardEntry(rankedLeaderboardEntry2.UserId, SteamFriends.GetFriendPersonaName(new CSteamID(num2)), rankedLeaderboardEntry2.GlobalRank, rankedLeaderboardEntry2.DistanceMeters, rankedLeaderboardEntry2.Score, false);
					}
				}
				this.Complete(new RankedLeaderboardResult(this._scope, (this._entries.Count == 0) ? RankedLeaderboardState.Empty : RankedLeaderboardState.Ready, this._page, this._pageSize, this._entries.ToArray(), this._localEntry, this._total, null, new bool?(this._hasNextPage)));
			}

			// Token: 0x060065A0 RID: 26016 RVA: 0x002890A0 File Offset: 0x002872A0
			private void HandlePersonaChanged(PersonaStateChange_t result)
			{
				if (!this._disposed && this._pendingNames.Remove(result.m_ulSteamID))
				{
					this.TryComplete();
				}
			}

			// Token: 0x060065A1 RID: 26017 RVA: 0x002890C4 File Offset: 0x002872C4
			public void CheckTimeout(double now)
			{
				if (!this._disposed && this._namesRequested && now >= this._nameDeadline)
				{
					this._pendingNames.Clear();
					this.TryComplete();
				}
				if (!this._disposed && now >= this._deadline)
				{
					this.Fail(RankedLeaderboardState.Error, "Steam did not respond in time. Please refresh.");
				}
			}

			// Token: 0x060065A2 RID: 26018 RVA: 0x00289118 File Offset: 0x00287318
			public void Fail(RankedLeaderboardState state, string message)
			{
				if (this._disposed)
				{
					return;
				}
				this.Complete(new RankedLeaderboardResult(this._scope, state, this._page, this._pageSize, null, null, 0, message, null));
			}

			// Token: 0x060065A3 RID: 26019 RVA: 0x00289159 File Offset: 0x00287359
			private void Complete(RankedLeaderboardResult result)
			{
				Action<RankedLeaderboardResult> completed = this._completed;
				this.Dispose();
				if (completed == null)
				{
					return;
				}
				completed(result);
			}

			// Token: 0x060065A4 RID: 26020 RVA: 0x00289174 File Offset: 0x00287374
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._completed = null;
				CallResult<LeaderboardScoresDownloaded_t> pageCall = this._pageCall;
				if (pageCall != null)
				{
					pageCall.Dispose();
				}
				CallResult<LeaderboardScoresDownloaded_t> localCall = this._localCall;
				if (localCall != null)
				{
					localCall.Dispose();
				}
				Callback<PersonaStateChange_t> personaChanged = this._personaChanged;
				if (personaChanged != null)
				{
					personaChanged.Dispose();
				}
				this._owner._browseRequests.Remove(this);
			}

			// Token: 0x04006830 RID: 26672
			private readonly SteamworksNetLeaderboardBackend _owner;

			// Token: 0x04006831 RID: 26673
			private readonly RankedLeaderboardScope _scope;

			// Token: 0x04006832 RID: 26674
			private readonly int _page;

			// Token: 0x04006833 RID: 26675
			private readonly int _pageSize;

			// Token: 0x04006834 RID: 26676
			private readonly double _deadline;

			// Token: 0x04006835 RID: 26677
			private Action<RankedLeaderboardResult> _completed;

			// Token: 0x04006836 RID: 26678
			private CallResult<LeaderboardScoresDownloaded_t> _pageCall;

			// Token: 0x04006837 RID: 26679
			private CallResult<LeaderboardScoresDownloaded_t> _localCall;

			// Token: 0x04006838 RID: 26680
			private Callback<PersonaStateChange_t> _personaChanged;

			// Token: 0x04006839 RID: 26681
			private readonly HashSet<ulong> _pendingNames = new HashSet<ulong>();

			// Token: 0x0400683A RID: 26682
			private List<RankedLeaderboardEntry> _entries;

			// Token: 0x0400683B RID: 26683
			private RankedLeaderboardEntry _localEntry;

			// Token: 0x0400683C RID: 26684
			private bool _localReady;

			// Token: 0x0400683D RID: 26685
			private bool _started;

			// Token: 0x0400683E RID: 26686
			private bool _disposed;

			// Token: 0x0400683F RID: 26687
			private bool _namesRequested;

			// Token: 0x04006840 RID: 26688
			private double _nameDeadline;

			// Token: 0x04006841 RID: 26689
			private int _total;

			// Token: 0x04006842 RID: 26690
			private bool _hasNextPage;
		}
	}
}
