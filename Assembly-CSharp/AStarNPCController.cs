using System;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x020000B2 RID: 178
[RequireComponent(typeof(FollowerEntity))]
public class AStarNPCController : MonoBehaviour
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x00031EF9 File Offset: 0x000300F9
	// (set) Token: 0x060006FD RID: 1789 RVA: 0x00031F01 File Offset: 0x00030101
	public NPCInterestPoint.PostureType CurrentPosture { get; private set; }

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x00031F0A File Offset: 0x0003010A
	// (set) Token: 0x060006FF RID: 1791 RVA: 0x00031F12 File Offset: 0x00030112
	public NPCInterestPoint.ActionType CurrentAction { get; private set; }

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x00031F1B File Offset: 0x0003011B
	// (set) Token: 0x06000701 RID: 1793 RVA: 0x00031F23 File Offset: 0x00030123
	public bool IsManualMode { get; private set; }

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000702 RID: 1794 RVA: 0x00031F2C File Offset: 0x0003012C
	// (set) Token: 0x06000703 RID: 1795 RVA: 0x00031F34 File Offset: 0x00030134
	public bool IsNavigationLocked { get; private set; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x00031F3D File Offset: 0x0003013D
	// (set) Token: 0x06000705 RID: 1797 RVA: 0x00031F45 File Offset: 0x00030145
	public bool SuppressPOIBehavior
	{
		get
		{
			return this._suppressPOIBehavior;
		}
		set
		{
			this._suppressPOIBehavior = value;
		}
	}

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000706 RID: 1798 RVA: 0x00031F50 File Offset: 0x00030150
	// (remove) Token: 0x06000707 RID: 1799 RVA: 0x00031F88 File Offset: 0x00030188
	public event Action<NPCInterestPoint.ActionType> OnActionChanged;

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000708 RID: 1800 RVA: 0x00031FBD File Offset: 0x000301BD
	public bool IsManualModeReady
	{
		get
		{
			return this.IsManualMode && !this._pendingEnterManualMode;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x00031FD2 File Offset: 0x000301D2
	public bool IsNavigationReady
	{
		get
		{
			return !this.IsManualMode && !this._pendingExitManualMode;
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00031FE8 File Offset: 0x000301E8
	private void Awake()
	{
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
		this.follower = ((this.follower != null) ? this.follower : base.GetComponent<FollowerEntity>());
		this.animator = ((this.animator != null) ? this.animator : base.GetComponentInChildren<Animator>());
		this.equipmentManager = ((this.equipmentManager != null) ? this.equipmentManager : base.GetComponent<NPCEquipmentManager>());
		if (!string.IsNullOrEmpty(this.speedParam))
		{
			this._speedHash = Animator.StringToHash(this.speedParam);
		}
		if (!string.IsNullOrEmpty(this.postureParam))
		{
			this._postureHash = Animator.StringToHash(this.postureParam);
		}
		if (!string.IsNullOrEmpty(this.actionParam))
		{
			this._actionHash = Animator.StringToHash(this.actionParam);
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x000320C6 File Offset: 0x000302C6
	private void OnEnable()
	{
		NPCSimulationPause.RegisterParticipant(this);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x000320CE File Offset: 0x000302CE
	private void OnDestroy()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x000320E1 File Offset: 0x000302E1
	public static bool CanShiftWorldOrigin()
	{
		return BatchedEvents.Find<FollowerEntity, bool>(false, (FollowerEntity agent, bool _) => agent.entityExists && agent.isTraversingOffMeshLink) == null;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00032110 File Offset: 0x00030310
	private void OnWorldOriginShifted(Vector3 delta)
	{
		if (this._pendingExitManualMode)
		{
			this._pendingTeleportPosition += delta;
		}
		NPCStateMachine component = base.GetComponent<NPCStateMachine>();
		if (component != null && component.Context != null)
		{
			component.Context.LastSeenPlayerPosition += delta;
		}
		NPCSenses component2 = base.GetComponent<NPCSenses>();
		if (component2 != null)
		{
			component2.LastKnownPlayerPosition += delta;
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00032188 File Offset: 0x00030388
	public static void RebaseFollowers(Vector3 delta)
	{
		if (BatchedEvents.Find<FollowerEntity, bool>(false, (FollowerEntity agent, bool _) => true) == null)
		{
			return;
		}
		TransformAccessArray transformAccessArray;
		FollowerEntity[] array;
		int components = BatchedEvents.GetComponents<FollowerEntity>(BatchedEvents.Event.None, out transformAccessArray, out array);
		for (int i = 0; i < components; i++)
		{
			FollowerEntity followerEntity = array[i];
			if (followerEntity.entityExists)
			{
				Vector3 vector = followerEntity.position + delta;
				Vector3 destination = followerEntity.destination;
				Vector3 destinationFacingDirection = followerEntity.destinationFacingDirection;
				followerEntity.Teleport(vector, true);
				if (!float.IsInfinity(destination.x) && !float.IsInfinity(destination.y) && !float.IsInfinity(destination.z) && !float.IsNaN(destination.x) && !float.IsNaN(destination.y) && !float.IsNaN(destination.z))
				{
					followerEntity.SetDestination(destination + delta, destinationFacingDirection);
				}
			}
		}
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0003228B File Offset: 0x0003048B
	private void Update()
	{
		if (NPCSimulationPause.IsPaused)
		{
			return;
		}
		this.ApplyDeferredManualModeChanges();
		if (this.animator == null)
		{
			return;
		}
		this.UpdateAnimator();
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x000322B0 File Offset: 0x000304B0
	private void ApplyDeferredManualModeChanges()
	{
		if (this._pendingEnterManualMode && this.follower != null)
		{
			this.follower.enabled = false;
			this._pendingEnterManualMode = false;
			this.Log("Deferred: FollowerEntity disabled");
		}
		if (this._pendingExitManualMode && this.follower != null)
		{
			this.follower.Teleport(this._pendingTeleportPosition, true);
			this.follower.enabled = true;
			this._pendingExitManualMode = false;
			this.Log("Deferred: FollowerEntity enabled");
		}
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00032338 File Offset: 0x00030538
	private void OnAnimatorMove()
	{
		if (NPCSimulationPause.IsPaused)
		{
			return;
		}
		if (this.animator != null)
		{
			if (this.IsManualMode && !this.SuppressRootMotionPosition)
			{
				base.transform.position += this.animator.deltaPosition;
			}
			base.transform.rotation *= this.animator.deltaRotation;
		}
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x000323B0 File Offset: 0x000305B0
	private void UpdateAnimator()
	{
		float num = ((this._speedOverride >= 0f) ? this._speedOverride : ((this.follower != null && this.follower.entityExists) ? this.follower.velocity.magnitude : 0f));
		float num2;
		if (num <= this.walkMaxSpeed)
		{
			num2 = ((this.walkMaxSpeed > 0f) ? (0.5f * (num / this.walkMaxSpeed)) : 0f);
		}
		else
		{
			float num3 = Mathf.Max(0.0001f, this.runMaxSpeed - this.walkMaxSpeed);
			num2 = 0.5f + 0.5f * Mathf.Clamp01((num - this.walkMaxSpeed) / num3);
		}
		this.animator.SetFloat(this._speedHash, num2, 0.1f, NPCSimulationPause.SimulationDeltaTime);
		if (this.equipmentManager != null)
		{
			this.equipmentManager.SetMoving(num > this.walkThreshold);
		}
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x000324A8 File Offset: 0x000306A8
	public void SetPosture(NPCInterestPoint.PostureType posture)
	{
		this.CurrentPosture = posture;
		if (this.animator != null)
		{
			this.animator.SetInteger(this._postureHash, (int)posture);
		}
		this.Log(string.Format("Posture: {0}", posture));
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x000324E8 File Offset: 0x000306E8
	public void SetAction(NPCInterestPoint.ActionType action)
	{
		bool flag = this.CurrentAction != action;
		this.CurrentAction = action;
		if (this.animator != null)
		{
			this.animator.SetInteger(this._actionHash, (int)action);
		}
		if (this.equipmentManager != null)
		{
			this.equipmentManager.SetAction((int)action);
		}
		if (!flag)
		{
			return;
		}
		Action<NPCInterestPoint.ActionType> onActionChanged = this.OnActionChanged;
		if (onActionChanged != null)
		{
			onActionChanged(action);
		}
		this.Log(string.Format("Action: {0}", action));
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0003256D File Offset: 0x0003076D
	public void SetSpeedOverride(float speed)
	{
		this._speedOverride = speed;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00032576 File Offset: 0x00030776
	public void ClearSpeedOverride()
	{
		this._speedOverride = -1f;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00032583 File Offset: 0x00030783
	public void EnableRootMotion()
	{
		this._rootMotionActive = true;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0003258C File Offset: 0x0003078C
	public void DisableRootMotion()
	{
		this._rootMotionActive = false;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00032598 File Offset: 0x00030798
	public void EnterManualMode()
	{
		if (NPCSimulationPause.IsPaused)
		{
			return;
		}
		if (this.IsManualMode || this.follower == null)
		{
			return;
		}
		this.IsManualMode = true;
		this._pendingEnterManualMode = true;
		this._pendingExitManualMode = false;
		this.Log("Requested manual mode (pending)");
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000325E4 File Offset: 0x000307E4
	public void CancelManualModeForWaterRecovery()
	{
		this.CancelManualModeForEncounter();
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x000325EC File Offset: 0x000307EC
	public void CancelManualModeForEncounter()
	{
		this.IsManualMode = false;
		this._pendingEnterManualMode = false;
		this._pendingExitManualMode = false;
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00032604 File Offset: 0x00030804
	public void ExitManualMode(bool allowDuringSimulationPause = false)
	{
		if (NPCSimulationPause.IsPaused && !allowDuringSimulationPause)
		{
			return;
		}
		if (!this.IsManualMode || this.follower == null)
		{
			return;
		}
		this.IsManualMode = false;
		this._pendingExitManualMode = true;
		this._pendingEnterManualMode = false;
		this._pendingTeleportPosition = base.transform.position;
		this.Log("Requested exit manual mode (pending)");
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00032664 File Offset: 0x00030864
	public void SetDestination(Vector3 destination)
	{
		this.TrySetDestination(destination);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00032670 File Offset: 0x00030870
	public bool TrySetDestination(Vector3 destination)
	{
		if (NPCSimulationPause.IsPaused)
		{
			return false;
		}
		if (!base.enabled || this.follower == null || !this.follower.enabled || !this.follower.entityExists || this.IsManualMode || this.IsNavigationLocked || this._pendingExitManualMode)
		{
			return false;
		}
		if (!NPCPoolRecovery.TryConstrainDestination(base.transform, ref destination))
		{
			this.Stop();
			return false;
		}
		AstarPath active = AstarPath.active;
		NNInfo? nninfo = ((active != null) ? new NNInfo?(active.GetNearest(destination)) : null);
		if (((nninfo != null) ? nninfo.GetValueOrDefault().node : null) != null && nninfo.Value.node.Walkable)
		{
			this.follower.destination = nninfo.Value.position;
		}
		else
		{
			this.follower.destination = destination;
		}
		this.follower.isStopped = false;
		return true;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00032766 File Offset: 0x00030966
	public void Stop()
	{
		if (this.follower == null || !this.follower.entityExists)
		{
			return;
		}
		this.follower.isStopped = true;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00032790 File Offset: 0x00030990
	public void StopAndLock()
	{
		this.Stop();
		this.IsNavigationLocked = true;
		this._speedOverride = 0f;
		if (this.animator != null && this._speedHash != 0)
		{
			this.animator.SetFloat(this._speedHash, 0f);
		}
		if (this.equipmentManager != null)
		{
			this.equipmentManager.SetMoving(false);
		}
		this.Log("Navigation locked");
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00032808 File Offset: 0x00030A08
	public void UnlockNavigation(bool allowDuringSimulationPause = false)
	{
		if (NPCSimulationPause.IsPaused && !allowDuringSimulationPause)
		{
			return;
		}
		this.IsNavigationLocked = false;
		this._speedOverride = -1f;
		if (this.follower != null && this.follower.entityExists)
		{
			this.follower.isStopped = false;
		}
		this.Log("Navigation unlocked and resumed");
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00032864 File Offset: 0x00030A64
	public bool HasReachedDestination()
	{
		return this.follower != null && this.follower.entityExists && this.follower.reachedDestination;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00032890 File Offset: 0x00030A90
	public void ConfigureForPOI(NPCInterestPoint poi, bool ambientIdle = false)
	{
		if (poi == null || this.SuppressPOIBehavior)
		{
			return;
		}
		this.SetPosture(poi.posture);
		ambientIdle &= poi.AllowsAmbientLife;
		NPCInterestPoint.ActionType actionType = (ambientIdle ? NPCInterestPoint.ActionType.None : poi.GetRandomAction());
		this.SetAction(actionType);
		if (!ambientIdle && this.equipmentManager != null && poi.propsToEnable.Count > 0)
		{
			this.Log(string.Format("ConfigureForPOI({0}) - activating {1} props: [{2}] for action {3}", new object[]
			{
				poi.name,
				poi.propsToEnable.Count,
				string.Join(", ", poi.propsToEnable),
				actionType
			}));
			this.equipmentManager.ActivateProps(poi.propsToEnable);
		}
		this.Log("Configured for POI: " + poi.name);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00032970 File Offset: 0x00030B70
	public void ResetToDefault()
	{
		if (this.SuppressPOIBehavior)
		{
			return;
		}
		this.Log("ResetToDefault() called - unlocking props before SetAction(None)");
		this.SetPosture(NPCInterestPoint.PostureType.Standing);
		if (this.equipmentManager != null)
		{
			this.equipmentManager.UnlockAllProps();
		}
		this.SetAction(NPCInterestPoint.ActionType.None);
		this.DisableRootMotion();
		this.ClearSpeedOverride();
		if (this.IsManualMode)
		{
			this.ExitManualMode(false);
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x000329D3 File Offset: 0x00030BD3
	private void Log(string msg)
	{
		if (this.debugLog || NPCDebug.Verbose)
		{
			Debug.Log("[NPCController] " + base.gameObject.name + ": " + msg);
		}
	}

	// Token: 0x040005D3 RID: 1491
	[Header("References")]
	public FollowerEntity follower;

	// Token: 0x040005D4 RID: 1492
	public Animator animator;

	// Token: 0x040005D5 RID: 1493
	public NPCEquipmentManager equipmentManager;

	// Token: 0x040005D6 RID: 1494
	[Header("Animator Parameters")]
	public string speedParam = "Speed";

	// Token: 0x040005D7 RID: 1495
	public string postureParam = "PostureState";

	// Token: 0x040005D8 RID: 1496
	public string actionParam = "UpperBodyAction";

	// Token: 0x040005D9 RID: 1497
	[Header("Settings")]
	public float walkThreshold = 0.05f;

	// Token: 0x040005DA RID: 1498
	[Header("Speed Mapping (BlendTree 'Normal Movement')")]
	[Tooltip("Velocità (m/s) a cui il blend è completamente sull'animazione di Walk.\nSotto questa velocità: blend Idle→Walk. Sopra: blend Walk→Run.")]
	public float walkMaxSpeed = 2f;

	// Token: 0x040005DB RID: 1499
	[Tooltip("Velocità (m/s) a cui il blend è completamente sull'animazione di Run.\nCoincide tipicamente con FollowerEntity.maxSpeed quando l'NPC corre.")]
	public float runMaxSpeed = 4.5f;

	// Token: 0x040005DC RID: 1500
	[Header("Debug")]
	public bool debugLog;

	// Token: 0x040005DD RID: 1501
	private int _speedHash;

	// Token: 0x040005DE RID: 1502
	private int _postureHash;

	// Token: 0x040005DF RID: 1503
	private int _actionHash;

	// Token: 0x040005E0 RID: 1504
	private float _speedOverride = -1f;

	// Token: 0x040005E1 RID: 1505
	private bool _rootMotionActive;

	// Token: 0x040005E2 RID: 1506
	public bool SuppressRootMotionPosition;

	// Token: 0x040005E3 RID: 1507
	private bool _pendingEnterManualMode;

	// Token: 0x040005E4 RID: 1508
	private bool _pendingExitManualMode;

	// Token: 0x040005E5 RID: 1509
	private Vector3 _pendingTeleportPosition;

	// Token: 0x040005EA RID: 1514
	[Header("POI Behavior")]
	[Tooltip("When true, ConfigureForPOI()/SetAction()/ResetToDefault() calls from NPCWanderController are ignored.\nSet to true on prefabs that maintain a fixed visual identity (e.g. Reporter holding camera).\nPre-set in the prefab to avoid races where the wander cycle changes pose before runtime sets the flag.")]
	[SerializeField]
	private bool _suppressPOIBehavior;
}
