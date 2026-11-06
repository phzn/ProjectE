using System;
using System.Globalization;
using Features.DebugTools;
using Features.Detection;
using Features.Flash;
using Features.GameOver;
using Features.MatchRecap;
using Features.Rank;
using Features.RunModes;
using Features.Tutorials;
using Invector.vCharacterController;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000960 RID: 2400
	[DisallowMultipleComponent]
	public sealed class PoiScenePortal : MonoBehaviour, IInteractionCandidate
	{
		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x00185F73 File Offset: 0x00184173
		public PoiScenePortal.PortalMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x00185F7B File Offset: 0x0018417B
		public string DestinationScene
		{
			get
			{
				return this.destinationScene;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x00185F83 File Offset: 0x00184183
		public Transform MapReturnPoint
		{
			get
			{
				return this.mapReturnPoint;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x00185F8B File Offset: 0x0018418B
		public string PoiDisplayName
		{
			get
			{
				return this.poiDisplayName;
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x00185F93 File Offset: 0x00184193
		public float InteractionRadius
		{
			get
			{
				return this.interactionRadius;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x00185F9B File Offset: 0x0018419B
		public float AwarenessRadius
		{
			get
			{
				return this.awarenessRadius;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x00185FA3 File Offset: 0x001841A3
		public bool WorldPresentationEnabled
		{
			get
			{
				return this.enableWorldPresentation;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06004194 RID: 16788 RVA: 0x00185FAB File Offset: 0x001841AB
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistance;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x00185FB4 File Offset: 0x001841B4
		public bool IsCandidateEligible
		{
			get
			{
				return this._portalContextActive && this._travelStateSafe && (this.mode != PoiScenePortal.PortalMode.ReturnHome || this.CanReturnHome) && this.IsRankDestinationAllowed && !this._requestCommitted && this._player != null && this._sqrDistance <= this.interactionRadius * this.interactionRadius;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06004196 RID: 16790 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x0018601A File Offset: 0x0018421A
		private bool IsRankDestinationAllowed
		{
			get
			{
				return this.mode != PoiScenePortal.PortalMode.VisitPoi || RankPoiPolicy.IsDestinationAllowed(this.destinationScene);
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x00186031 File Offset: 0x00184231
		private bool CanReturnHome
		{
			get
			{
				return RunModeService.HomeReturnEnabled && MatchStatsService.Instance != null && MatchStatsService.Instance.IsRunTrackingActive && !MatchStatsService.Instance.IsMocked;
			}
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x00186062 File Offset: 0x00184262
		private void Awake()
		{
			if (this.interactionInput == null)
			{
				this.interactionInput = new GenericInput("E", "A", "A");
			}
			this.ResolvePlayer();
			this.EnsureWorldPresentation();
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x00186092 File Offset: 0x00184292
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.StopPresenting();
			this.ResetRequestLatch();
			this._homeReturnRequested = false;
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x001860B0 File Offset: 0x001842B0
		private void Update()
		{
			if (this._player == null)
			{
				this.ResolvePlayer();
			}
			this.RefreshRequestLatch();
			this.ApplyPortalContext(this.IsPortalContextActive());
			if (!this._portalContextActive)
			{
				this._sqrDistance = float.MaxValue;
				this.StopPresenting();
				return;
			}
			if (!(this._player == null))
			{
				this._travelStateSafe = this.IsTravelStateSafe();
				this._sqrDistance = (base.transform.position - this._player.transform.position).sqrMagnitude;
				bool flag = this.IsCandidateEligible && InteractionCandidateRegistry.IsWinner(this);
				bool flag2 = !InteractionCandidateRegistry.CanStartWorldInteraction;
				if (flag)
				{
					this.StartPresenting();
				}
				else
				{
					this.StopPresenting();
				}
				PoiPortalPresentation presentation = this._presentation;
				if (presentation != null)
				{
					presentation.SetPlayerState(this._player.transform, this._sqrDistance, flag, this._requestCommitted, this.ResolveInputLabel(), !this._travelStateSafe || flag2);
				}
				if (flag && this.WasInteractionPressed())
				{
					if (this.mode == PoiScenePortal.PortalMode.ReturnHome)
					{
						this._homeReturnRequested = true;
						return;
					}
					this.RequestTravel();
				}
				return;
			}
			this.ResolvePlayer();
			this._sqrDistance = float.MaxValue;
			this.StopPresenting();
			PoiPortalPresentation presentation2 = this._presentation;
			if (presentation2 == null)
			{
				return;
			}
			presentation2.SetPlayerState(null, float.MaxValue, false, this._requestCommitted, this.ResolveInputLabel(), false);
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0018620C File Offset: 0x0018440C
		private void LateUpdate()
		{
			if (!this._homeReturnRequested)
			{
				return;
			}
			this._homeReturnRequested = false;
			if (!this.IsPortalContextActive() || this._player == null || !this.IsTravelStateSafe() || !this.CanReturnHome || !InteractionCandidateRegistry.IsWinner(this) || (base.transform.position - this._player.transform.position).sqrMagnitude > this.interactionRadius * this.interactionRadius)
			{
				return;
			}
			GameOverManager instance = GameOverManager.Instance;
			if (instance == null || !instance.TryCompleteHomeReturn())
			{
				return;
			}
			InteractionCandidateRegistry.MarkInteract();
			this._requestCommitted = true;
			this._requestCommittedAt = Time.unscaledTime;
			this.StopPresenting();
			PoiPortalPresentation presentation = this._presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.PlayCommit();
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x001862D4 File Offset: 0x001844D4
		private void ApplyPortalContext(bool active)
		{
			this._portalContextActive = active;
			if (!active && this._requestCommitted)
			{
				this.ResetRequestLatch();
			}
			PoiPortalPresentation presentation = this._presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.SetContextActive(active);
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x00186300 File Offset: 0x00184500
		private void ResolvePlayer()
		{
			GameObject gameObject = PlayerRegistry.PlayerObject;
			if (gameObject == null)
			{
				gameObject = GameObject.FindGameObjectWithTag("Player");
			}
			if (gameObject == this._player)
			{
				return;
			}
			this._player = gameObject;
			this._coverController = ((this._player != null) ? this._player.GetComponentInChildren<vBasicCoverController>(true) : null);
			this._locomotion = ((this._player != null) ? this._player.GetComponentInChildren<vThirdPersonController>(true) : null);
			this._ragdollRecovery = ((this._player != null) ? this._player.GetComponentInChildren<CielRagdollRecovery>(true) : null);
			this._ragdoll = ((this._player != null) ? this._player.GetComponentInChildren<vRagdoll>(true) : null);
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x001863CC File Offset: 0x001845CC
		private bool IsPortalContextActive()
		{
			if (!this.IsRankDestinationAllowed)
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			string text = ((instance != null) ? instance.CurrentGameplayScene : null);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = SceneManager.GetActiveScene().name;
			}
			if (this.mode == PoiScenePortal.PortalMode.ReturnHome)
			{
				MatchStatsService instance2 = MatchStatsService.Instance;
				return text == "Game - Current" && base.gameObject.scene.name == text && RunModeService.HomeReturnEnabled && instance2 != null && instance2.IsRunTrackingActive && !instance2.IsMocked && this.HasLeftHomeAfterIntroduction() && (!(GameOverManager.Instance != null) || !GameOverManager.Instance.IsGameOver);
			}
			if (this.mode == PoiScenePortal.PortalMode.VisitPoi)
			{
				return !string.IsNullOrWhiteSpace(text) && !PoiTravelState.IsPoiSceneName(text) && !string.Equals(text, this.destinationScene, StringComparison.Ordinal) && ChunkManager.Instance != null && ChunkManager.Instance.isActiveAndEnabled;
			}
			return string.Equals(base.gameObject.scene.name, text, StringComparison.Ordinal) && !this.IsArrivalCinematicBlockingExit();
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x001864F8 File Offset: 0x001846F8
		private bool HasLeftHomeAfterIntroduction()
		{
			bool flag = GameplayInputGate.IsLoading || PoiArrivalPresentation.IsPresentingInActiveScene || FlyCamera.IsActive || GraphicsSettingsRuntimeUI.AnyOpen;
			return this._player != null && this.UpdateHomeDeparture(flag, this._player.transform.position);
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x0018654C File Offset: 0x0018474C
		private bool UpdateHomeDeparture(bool introductionActive, Vector3 playerPosition)
		{
			if (introductionActive)
			{
				if (!this._homeDepartureRecorded)
				{
					this._homeDepartureOriginSet = false;
				}
				return false;
			}
			if (this._homeDepartureRecorded)
			{
				return true;
			}
			Vector3 vector = playerPosition - base.transform.position;
			vector.y = 0f;
			if (!this._homeDepartureOriginSet)
			{
				this._homeDepartureOrigin = vector;
				this._homeDepartureOriginSet = true;
				return false;
			}
			this._homeDepartureRecorded = (vector - this._homeDepartureOrigin).sqrMagnitude >= 4f && vector.sqrMagnitude > this.interactionRadius * this.interactionRadius;
			return this._homeDepartureRecorded;
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x001865F0 File Offset: 0x001847F0
		private bool IsArrivalCinematicBlockingExit()
		{
			if (this.mode != PoiScenePortal.PortalMode.ReturnToMap)
			{
				return false;
			}
			Scene scene = base.gameObject.scene;
			if (PoiArrivalPresentation.IsPresentingInScene(scene))
			{
				return true;
			}
			LongWayHomeUniversitySequence presenterForScene = LongWayHomeUniversitySequence.GetPresenterForScene(scene);
			if (presenterForScene != null && presenterForScene.OwnsDestinationReveal)
			{
				return true;
			}
			LongWayHomeStadiumSequence presenterForScene2 = LongWayHomeStadiumSequence.GetPresenterForScene(scene);
			if (presenterForScene2 != null && presenterForScene2.OwnsDestinationReveal)
			{
				return true;
			}
			LongWayHomeGymShowerSequence presenterForScene3 = LongWayHomeGymShowerSequence.GetPresenterForScene(scene);
			return presenterForScene3 != null && presenterForScene3.OwnsDestinationReveal;
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x0018666C File Offset: 0x0018486C
		private bool IsTravelStateSafe()
		{
			return (this.mode != PoiScenePortal.PortalMode.ReturnHome || (!DetectionTransitionGate.IsBlocked && !DetectionRecoveryGate.IsPlayerRecovering && (!(GameOverManager.Instance != null) || !GameOverManager.Instance.HasPendingCapture) && (!(ExhibitionEventDirector.Instance != null) || (!ExhibitionEventDirector.Instance.IsEventActive && !ExhibitionEventDirector.Instance.HasPendingEncounter)))) && !ClothActionGate.IsBusy && (!(this._coverController != null) || (!this._coverController.inCover && !this._coverController.goingToCoverPoint)) && (!(this._locomotion != null) || (!this._locomotion.customAction && !this._locomotion.isJumping && !this._locomotion.isRolling && !this._locomotion.isSliding && this._locomotion.isGrounded && !this._locomotion.ragdolled && !this._locomotion.isDead)) && (!(this._ragdollRecovery != null) || !this._ragdollRecovery.IsRecovering) && (this._ragdoll == null || !this._ragdoll.isActive);
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x001867AC File Offset: 0x001849AC
		private void RefreshRequestLatch()
		{
			if (!this._requestCommitted)
			{
				return;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance != null && instance.IsTransitioning)
			{
				this._sawTransitionInProgress = true;
				return;
			}
			if (this._sawTransitionInProgress || Time.unscaledTime - this._requestCommittedAt > 1f)
			{
				this.ResetRequestLatch();
			}
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x00186802 File Offset: 0x00184A02
		private void ResetRequestLatch()
		{
			this._requestCommitted = false;
			this._sawTransitionInProgress = false;
			this._requestCommittedAt = 0f;
			PoiPortalPresentation presentation = this._presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.CancelCommit();
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x00186830 File Offset: 0x00184A30
		private void RequestTravel()
		{
			if (this.mode == PoiScenePortal.PortalMode.ReturnHome)
			{
				return;
			}
			if (!this.IsRankDestinationAllowed)
			{
				return;
			}
			if (TutorialService.TryShowFirstUse("poi_travel"))
			{
				return;
			}
			if (this._requestCommitted)
			{
				return;
			}
			if (!this._portalContextActive || this._player == null || !this.IsTravelStateSafe() || !InteractionCandidateRegistry.CanStartWorldInteraction)
			{
				return;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance == null || instance.IsTransitioning)
			{
				if (this.enableDebugLogs)
				{
					Debug.LogWarning("[PoiScenePortal] SceneTransitionManager unavailable or busy.", this);
				}
				return;
			}
			string text = this.ResolveDestinationScene();
			if (string.IsNullOrWhiteSpace(text) || !Application.CanStreamedLevelBeLoaded(text))
			{
				Debug.LogError("[PoiScenePortal] Scene '" + text + "' is not available in Build Settings.", this);
				return;
			}
			string text2 = null;
			if (this.mode == PoiScenePortal.PortalMode.VisitPoi)
			{
				text2 = PoiTravelState.ResolveSourceSceneName(instance.CurrentGameplayScene, SceneManager.GetActiveScene().name, base.gameObject.scene.name);
				if (string.IsNullOrWhiteSpace(text2))
				{
					Debug.LogError("[PoiScenePortal] Unable to resolve the gameplay scene to return to.", this);
					return;
				}
			}
			if (!InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			if (this.mode == PoiScenePortal.PortalMode.VisitPoi)
			{
				Vector3 vector = ((this.mapReturnPoint != null) ? this.mapReturnPoint.position : this._player.transform.position);
				Quaternion rotation = this._player.transform.rotation;
				PoiTravelState.BeginVisit(text2, vector, rotation, PoiTravelOrigin.ProceduralMap, text, this.GetDiscoveryId());
			}
			this._requestCommitted = true;
			this._sawTransitionInProgress = false;
			this._requestCommittedAt = Time.unscaledTime;
			this.StopPresenting();
			PoiPortalPresentation presentation = this._presentation;
			if (presentation != null)
			{
				presentation.PlayCommit();
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[PoiScenePortal] {0}: '{1}' -> '{2}'.", this.mode, text2 ?? base.gameObject.scene.name, text), this);
			}
			instance.LoadGameplayScene(text);
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x00186A04 File Offset: 0x00184C04
		public string GetDiscoveryId()
		{
			ChunkManager instance = ChunkManager.Instance;
			int num;
			if (instance == null || !instance.TryGetSessionSeed(out num))
			{
				return null;
			}
			ChunkInstance chunkContaining = instance.GetChunkContaining(base.transform);
			if (chunkContaining == null)
			{
				return null;
			}
			Vector2Int vector2Int = chunkContaining.originCell + instance.TravelOffset;
			return string.Format(CultureInfo.InvariantCulture, "city:{0}:{1}:{2}", num, vector2Int.x, vector2Int.y);
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x00186A7D File Offset: 0x00184C7D
		private string ResolveDestinationScene()
		{
			if (this.mode == PoiScenePortal.PortalMode.ReturnToMap && PoiTravelState.HasUsablePendingReturn)
			{
				return PoiTravelState.SourceSceneName;
			}
			return this.destinationScene;
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x00186A9B File Offset: 0x00184C9B
		private void StartPresenting()
		{
			PoiPortalPresentation presentation = this._presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.SetInteractionAvailable(true);
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x00186AAE File Offset: 0x00184CAE
		private void StopPresenting()
		{
			PoiPortalPresentation presentation = this._presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.SetInteractionAvailable(false);
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x00186AC1 File Offset: 0x00184CC1
		private bool WasInteractionPressed()
		{
			return (this.interactionInput != null && this.interactionInput.GetButtonDown()) || GameplayInput.GetKeyDown(this.interactionKey);
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x00186AE8 File Offset: 0x00184CE8
		private string ResolveInputLabel()
		{
			if (this.interactionInput != null)
			{
				string buttonName = this.interactionInput.buttonName;
				if (!string.IsNullOrWhiteSpace(buttonName))
				{
					return buttonName.ToUpperInvariant();
				}
				if (!string.IsNullOrWhiteSpace(this.interactionInput.keyboard))
				{
					return this.interactionInput.keyboard.ToUpperInvariant();
				}
			}
			return this.interactionKey.ToString().ToUpperInvariant();
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x00186B54 File Offset: 0x00184D54
		private void EnsureWorldPresentation()
		{
			if (!this.enableWorldPresentation)
			{
				return;
			}
			if (this._presentation == null)
			{
				this._presentation = base.GetComponent<PoiPortalPresentation>();
			}
			if (this._presentation == null)
			{
				this._presentation = base.gameObject.AddComponent<PoiPortalPresentation>();
			}
			this._presentation.Configure(this.mode, this.poiDisplayName, this.ResolveInputLabel(), this.interactionRadius, this.awarenessRadius);
			if (!this.IsRankDestinationAllowed)
			{
				this._presentation.SetContextActive(false);
			}
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x00186BE0 File Offset: 0x00184DE0
		public void Configure(PoiScenePortal.PortalMode portalMode, string scene, string displayName, Transform returnPoint = null, float radius = 2.25f)
		{
			this.mode = portalMode;
			this.destinationScene = scene;
			this.poiDisplayName = displayName;
			this.mapReturnPoint = returnPoint;
			this.interactionRadius = Mathf.Max(0.5f, radius);
			if (Application.isPlaying)
			{
				this.EnsureWorldPresentation();
			}
		}

		// Token: 0x04004181 RID: 16769
		[Header("Destination")]
		[SerializeField]
		private PoiScenePortal.PortalMode mode;

		// Token: 0x04004182 RID: 16770
		[SerializeField]
		private string destinationScene = "";

		// Token: 0x04004183 RID: 16771
		[SerializeField]
		private string poiDisplayName = "POI";

		// Token: 0x04004184 RID: 16772
		[Tooltip("Optional map-side point used when returning from the POI. If empty, the player's interaction position is saved.")]
		[SerializeField]
		private Transform mapReturnPoint;

		// Token: 0x04004185 RID: 16773
		[Header("Interaction")]
		[SerializeField]
		[Min(0.5f)]
		private float interactionRadius = 2.25f;

		// Token: 0x04004186 RID: 16774
		[SerializeField]
		private GenericInput interactionInput = new GenericInput("E", "A", "A");

		// Token: 0x04004187 RID: 16775
		[SerializeField]
		private KeyCode interactionKey = KeyCode.E;

		// Token: 0x04004188 RID: 16776
		[Header("World presentation")]
		[SerializeField]
		private bool enableWorldPresentation = true;

		// Token: 0x04004189 RID: 16777
		[SerializeField]
		[Min(8f)]
		private float awarenessRadius = 34f;

		// Token: 0x0400418A RID: 16778
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x0400418B RID: 16779
		private GameObject _player;

		// Token: 0x0400418C RID: 16780
		private PoiPortalPresentation _presentation;

		// Token: 0x0400418D RID: 16781
		private vBasicCoverController _coverController;

		// Token: 0x0400418E RID: 16782
		private vThirdPersonController _locomotion;

		// Token: 0x0400418F RID: 16783
		private CielRagdollRecovery _ragdollRecovery;

		// Token: 0x04004190 RID: 16784
		private vRagdoll _ragdoll;

		// Token: 0x04004191 RID: 16785
		private float _sqrDistance = float.MaxValue;

		// Token: 0x04004192 RID: 16786
		private bool _requestCommitted;

		// Token: 0x04004193 RID: 16787
		private bool _portalContextActive;

		// Token: 0x04004194 RID: 16788
		private bool _travelStateSafe = true;

		// Token: 0x04004195 RID: 16789
		private bool _sawTransitionInProgress;

		// Token: 0x04004196 RID: 16790
		private float _requestCommittedAt;

		// Token: 0x04004197 RID: 16791
		private bool _homeReturnRequested;

		// Token: 0x04004198 RID: 16792
		private bool _homeDepartureRecorded;

		// Token: 0x04004199 RID: 16793
		private bool _homeDepartureOriginSet;

		// Token: 0x0400419A RID: 16794
		private Vector3 _homeDepartureOrigin;

		// Token: 0x02000961 RID: 2401
		public enum PortalMode
		{
			// Token: 0x0400419C RID: 16796
			VisitPoi,
			// Token: 0x0400419D RID: 16797
			ReturnToMap,
			// Token: 0x0400419E RID: 16798
			ReturnHome
		}
	}
}
