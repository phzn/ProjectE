using System;
using Features.Loading;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CDA RID: 3290
	public sealed class LongWayHomeOpeningReadinessWait
	{
		// Token: 0x06005FA0 RID: 24480 RVA: 0x0025E09C File Offset: 0x0025C29C
		public LongWayHomeOpeningReadinessWait(float timeoutSeconds)
		{
			this._timeoutSeconds = timeoutSeconds;
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06005FA1 RID: 24481 RVA: 0x0025E0AB File Offset: 0x0025C2AB
		public static bool HasUpstreamFailure
		{
			get
			{
				return GameSystemsBootstrapper.GameplayReadinessFailed || (LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.HasLoadFailure);
			}
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0025E0D0 File Offset: 0x0025C2D0
		public LongWayHomeOpeningReadinessWait.Result ObservePlayer(Scene destination)
		{
			GameSystemsBootstrapper instance = GameSystemsBootstrapper.Instance;
			bool flag = SceneManager.GetActiveScene() == destination;
			bool flag2 = instance != null && instance.InitializedSceneName == destination.name;
			bool flag3 = flag && SceneBootLoader.CoreReady && GameSystemsBootstrapper.GameplayReady && flag2;
			LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
			bool flag4 = (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning) || (flag && flag2 && instance.isActiveAndEnabled && !GameSystemsBootstrapper.GameplayReady) || (existingInstance != null && existingInstance.IsLoadingActive && (!existingInstance.IsManualLoadActive || !SceneBootLoader.CoreReady));
			return this.Tick(Time.realtimeSinceStartupAsDouble, Time.frameCount, flag3, flag4, false, LongWayHomeOpeningReadinessWait.HasUpstreamFailure);
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x0025E1A0 File Offset: 0x0025C3A0
		public LongWayHomeOpeningReadinessWait.Result Tick(double now, int frame, bool worldReady, bool worldOwned, bool playerReady, bool upstreamFailed)
		{
			if (upstreamFailed)
			{
				return LongWayHomeOpeningReadinessWait.Result.UpstreamFailed;
			}
			if (playerReady)
			{
				return LongWayHomeOpeningReadinessWait.Result.Ready;
			}
			this._localPreparationStarted = this._localPreparationStarted || worldReady;
			if (!this._localPreparationStarted && worldOwned)
			{
				this._watchdog = null;
				return LongWayHomeOpeningReadinessWait.Result.Waiting;
			}
			if (this._watchdog == null)
			{
				this._watchdog = new LoadingReadinessWatchdog(this._timeoutSeconds);
			}
			if (!this._watchdog.Tick(now, 0f, 0, this._localPreparationStarted ? 1 : 0, frame))
			{
				return LongWayHomeOpeningReadinessWait.Result.Waiting;
			}
			return LongWayHomeOpeningReadinessWait.Result.TimedOut;
		}

		// Token: 0x04006104 RID: 24836
		private readonly float _timeoutSeconds;

		// Token: 0x04006105 RID: 24837
		private LoadingReadinessWatchdog _watchdog;

		// Token: 0x04006106 RID: 24838
		private bool _localPreparationStarted;

		// Token: 0x02000CDB RID: 3291
		public enum Result
		{
			// Token: 0x04006108 RID: 24840
			Waiting,
			// Token: 0x04006109 RID: 24841
			Ready,
			// Token: 0x0400610A RID: 24842
			TimedOut,
			// Token: 0x0400610B RID: 24843
			UpstreamFailed
		}
	}
}
