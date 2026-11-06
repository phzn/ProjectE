using System;
using Features.NPC;
using Features.RenderingPrecision;
using Pathfinding;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012F5 RID: 4853
	[RequireComponent(typeof(AStarNPCController))]
	public class SimplePOINavigator : MonoBehaviour
	{
		// Token: 0x060096A2 RID: 38562 RVA: 0x003DD98E File Offset: 0x003DBB8E
		private void Awake()
		{
			this._ctrl = base.GetComponent<AStarNPCController>();
			this._instanceId = base.gameObject.GetInstanceID();
		}

		// Token: 0x060096A3 RID: 38563 RVA: 0x003DD9AD File Offset: 0x003DBBAD
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x060096A4 RID: 38564 RVA: 0x003DD9E9 File Offset: 0x003DBBE9
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this.ReleasePOI();
			this._state = SimplePOINavigator.NavState.PickingPOI;
		}

		// Token: 0x060096A5 RID: 38565 RVA: 0x003DDA1A File Offset: 0x003DBC1A
		private void OnWorldOriginShifted(Vector3 delta)
		{
			this._navigationTarget += delta;
			this._progressPosition += delta;
		}

		// Token: 0x060096A6 RID: 38566 RVA: 0x003DDA40 File Offset: 0x003DBC40
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
			if (frameTimeLoss > 0f)
			{
				if (this._state == SimplePOINavigator.NavState.IdleAtPOI)
				{
					this._stateTimer += frameTimeLoss;
				}
				this._lastProgressTime += frameTimeLoss;
				this._nextProgressSample += frameTimeLoss;
			}
			if (NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this._ctrl == null)
			{
				return;
			}
			if (this.NavigationOwnedElsewhere())
			{
				this.ResetNavigationProgress();
				return;
			}
			switch (this._state)
			{
			case SimplePOINavigator.NavState.PickingPOI:
				this.TickPick();
				return;
			case SimplePOINavigator.NavState.Navigating:
				this.TickNav();
				return;
			case SimplePOINavigator.NavState.IdleAtPOI:
				this.TickIdle();
				return;
			default:
				return;
			}
		}

		// Token: 0x060096A7 RID: 38567 RVA: 0x003DDAFB File Offset: 0x003DBCFB
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060096A8 RID: 38568 RVA: 0x003DDB0D File Offset: 0x003DBD0D
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x060096A9 RID: 38569 RVA: 0x003DDB2C File Offset: 0x003DBD2C
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			if (this._state == SimplePOINavigator.NavState.IdleAtPOI)
			{
				this._stateTimer += Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			}
			this.ResetNavigationProgress();
			this._simulationPauseActive = false;
		}

		// Token: 0x060096AA RID: 38570 RVA: 0x003DDB7C File Offset: 0x003DBD7C
		private void TickPick()
		{
			if (this.TryReservePOI(out this._poi))
			{
				this.BeginNavigation(this._navigationTarget);
				return;
			}
			Vector3 vector;
			if (this.TryRandomPoint(out vector))
			{
				this.BeginNavigation(vector);
				return;
			}
			this._stateTimer = Time.time + 1f;
			this._state = SimplePOINavigator.NavState.IdleAtPOI;
		}

		// Token: 0x060096AB RID: 38571 RVA: 0x003DDBD0 File Offset: 0x003DBDD0
		private void TickNav()
		{
			if (this._poi != null && !this._poi.isActiveAndEnabled)
			{
				this.AbandonNavigation();
				return;
			}
			if (this._ctrl.HasReachedDestination())
			{
				this._ctrl.Stop();
				float num = ((this._poi != null) ? Random.Range(this.idleTimeRange.x, this.idleTimeRange.y) : this.fallbackIdleTime);
				this._stateTimer = Time.time + num;
				this._state = SimplePOINavigator.NavState.IdleAtPOI;
				if (this.debugLog)
				{
					Debug.Log(string.Format("[ReporterPOI] {0}: arrivato, idle {1:F1}s", base.name, num));
				}
				return;
			}
			if (Time.time < this._nextProgressSample)
			{
				return;
			}
			this._nextProgressSample = Time.time + 0.5f;
			if ((base.transform.position - this._progressPosition).sqrMagnitude >= 0.0225f)
			{
				this._progressPosition = base.transform.position;
				this._lastProgressTime = Time.time;
				return;
			}
			if (Time.time - this._lastProgressTime >= Mathf.Max(1f, this.navigationStallTimeout))
			{
				this.AbandonNavigation();
			}
		}

		// Token: 0x060096AC RID: 38572 RVA: 0x003DDD09 File Offset: 0x003DBF09
		private void TickIdle()
		{
			if (Time.time < this._stateTimer)
			{
				return;
			}
			this.ReleasePOI();
			this._state = SimplePOINavigator.NavState.PickingPOI;
		}

		// Token: 0x060096AD RID: 38573 RVA: 0x003DDD28 File Offset: 0x003DBF28
		private bool TryReservePOI(out NPCInterestPoint poi)
		{
			poi = null;
			if (NPCZoneManager.Instance == null)
			{
				return false;
			}
			for (int i = 0; i < this.maxSearchAttempts; i++)
			{
				NPCInterestPoint randomInterestPoint = NPCZoneManager.Instance.GetRandomInterestPoint(base.transform.position, this.searchRadius);
				Vector3 vector;
				if (!(randomInterestPoint == null) && randomInterestPoint.isActiveAndEnabled && !(randomInterestPoint == this._lastPoi) && this.TryGetNavigablePosition(randomInterestPoint.NavigationPosition, out vector) && randomInterestPoint.TryReserve(this._instanceId))
				{
					poi = randomInterestPoint;
					this._navigationTarget = vector;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060096AE RID: 38574 RVA: 0x003DDDC0 File Offset: 0x003DBFC0
		private bool TryRandomPoint(out Vector3 result)
		{
			result = Vector3.zero;
			if (AstarPath.active == null)
			{
				return false;
			}
			for (int i = 0; i < 5; i++)
			{
				Vector3 vector = base.transform.position + Random.insideUnitSphere * this.fallbackWanderRadius;
				vector.y = base.transform.position.y;
				if (this.TryGetNavigablePosition(vector, out result))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060096AF RID: 38575 RVA: 0x003DDE38 File Offset: 0x003DC038
		private bool NavigationOwnedElsewhere()
		{
			if (!this._ctrl.enabled || !this._ctrl.IsNavigationReady || this._ctrl.IsNavigationLocked || DetectionTransitionGate.IsBlocked || (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive) || NPCPoolRecovery.IsActorRecovering(this))
			{
				return true;
			}
			NPCRagdollRecovery component = base.GetComponent<NPCRagdollRecovery>();
			return component != null && component.IsRecovering;
		}

		// Token: 0x060096B0 RID: 38576 RVA: 0x003DDEB0 File Offset: 0x003DC0B0
		private bool TryGetNavigablePosition(Vector3 target, out Vector3 result)
		{
			result = default(Vector3);
			if (AstarPath.active == null || !NPCPoolRecovery.TryConstrainDestination(base.transform, ref target))
			{
				return false;
			}
			NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, walkable);
			NNInfo nearest2 = AstarPath.active.GetNearest(target, walkable);
			if (nearest.node == null || nearest2.node == null || !PathUtilities.IsPathPossible(nearest.node, nearest2.node))
			{
				return false;
			}
			result = nearest2.position;
			Vector2 vector = new Vector2(result.x - target.x, result.z - target.z);
			return vector.sqrMagnitude <= 4f && Mathf.Abs(result.y - target.y) <= 1.5f;
		}

		// Token: 0x060096B1 RID: 38577 RVA: 0x003DDF8C File Offset: 0x003DC18C
		private void BeginNavigation(Vector3 destination)
		{
			if (!this._ctrl.TrySetDestination(destination))
			{
				this.AbandonNavigation();
				return;
			}
			this._navigationTarget = destination;
			this.ResetNavigationProgress();
			this._state = SimplePOINavigator.NavState.Navigating;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[ReporterPOI] {0}: navigating to {1}", base.name, destination));
			}
		}

		// Token: 0x060096B2 RID: 38578 RVA: 0x003DDFE5 File Offset: 0x003DC1E5
		private void ResetNavigationProgress()
		{
			this._progressPosition = base.transform.position;
			this._lastProgressTime = Time.time;
			this._nextProgressSample = Time.time + 0.5f;
		}

		// Token: 0x060096B3 RID: 38579 RVA: 0x003DE014 File Offset: 0x003DC214
		private void AbandonNavigation()
		{
			this._ctrl.Stop();
			this.ReleasePOI();
			this._stateTimer = Time.time + 1f;
			this._state = SimplePOINavigator.NavState.IdleAtPOI;
			if (this.debugLog)
			{
				Debug.Log("[ReporterPOI] " + base.name + ": destination unavailable, retrying");
			}
		}

		// Token: 0x060096B4 RID: 38580 RVA: 0x003DE06C File Offset: 0x003DC26C
		private void ReleasePOI()
		{
			if (this._poi == null)
			{
				return;
			}
			this._lastPoi = this._poi;
			this._poi.Release(this._instanceId);
			this._poi = null;
		}

		// Token: 0x04009BB9 RID: 39865
		[Header("Search")]
		[Tooltip("Raggio di ricerca POI dalla posizione corrente.")]
		public float searchRadius = 30f;

		// Token: 0x04009BBA RID: 39866
		[Tooltip("Tentativi di prenotazione POI prima di fallback a punto random A*.")]
		public int maxSearchAttempts = 5;

		// Token: 0x04009BBB RID: 39867
		[Header("Idle at POI")]
		[Tooltip("Range (s) di permanenza al POI prima di scegliere il prossimo. Pesca uniformemente in [x,y].")]
		public Vector2 idleTimeRange = new Vector2(2f, 5f);

		// Token: 0x04009BBC RID: 39868
		[Header("Fallback")]
		[Tooltip("Raggio entro cui pescare un punto random sull'A* graph se nessun POI è prenotabile.")]
		public float fallbackWanderRadius = 15f;

		// Token: 0x04009BBD RID: 39869
		[Tooltip("Tempo (s) di idle al fallback point.")]
		public float fallbackIdleTime = 2f;

		// Token: 0x04009BBE RID: 39870
		[Tooltip("Retry a different destination after this many seconds without movement.")]
		[Min(1f)]
		public float navigationStallTimeout = 5f;

		// Token: 0x04009BBF RID: 39871
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009BC0 RID: 39872
		private SimplePOINavigator.NavState _state;

		// Token: 0x04009BC1 RID: 39873
		private NPCInterestPoint _poi;

		// Token: 0x04009BC2 RID: 39874
		private NPCInterestPoint _lastPoi;

		// Token: 0x04009BC3 RID: 39875
		private AStarNPCController _ctrl;

		// Token: 0x04009BC4 RID: 39876
		private int _instanceId;

		// Token: 0x04009BC5 RID: 39877
		private float _stateTimer;

		// Token: 0x04009BC6 RID: 39878
		private bool _simulationPauseActive;

		// Token: 0x04009BC7 RID: 39879
		private float _simulationPauseStartedAt;

		// Token: 0x04009BC8 RID: 39880
		private Vector3 _navigationTarget;

		// Token: 0x04009BC9 RID: 39881
		private Vector3 _progressPosition;

		// Token: 0x04009BCA RID: 39882
		private float _lastProgressTime;

		// Token: 0x04009BCB RID: 39883
		private float _nextProgressSample;

		// Token: 0x020012F6 RID: 4854
		private enum NavState
		{
			// Token: 0x04009BCD RID: 39885
			PickingPOI,
			// Token: 0x04009BCE RID: 39886
			Navigating,
			// Token: 0x04009BCF RID: 39887
			IdleAtPOI
		}
	}
}
