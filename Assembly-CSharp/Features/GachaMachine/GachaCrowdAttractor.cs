using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011DE RID: 4574
	[DisallowMultipleComponent]
	public sealed class GachaCrowdAttractor : MonoBehaviour
	{
		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x06008D07 RID: 36103 RVA: 0x0039937F File Offset: 0x0039757F
		public int SpectatorCount
		{
			get
			{
				return this._spectators.Count;
			}
		}

		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x06008D08 RID: 36104 RVA: 0x0039938C File Offset: 0x0039758C
		public int StaticWatcherCount
		{
			get
			{
				return this._staticWatchers.Count;
			}
		}

		// Token: 0x06008D09 RID: 36105 RVA: 0x00399399 File Offset: 0x00397599
		private void Awake()
		{
			if (this.controller == null)
			{
				this.controller = base.GetComponentInParent<GachaMachineController>();
			}
		}

		// Token: 0x06008D0A RID: 36106 RVA: 0x003993B8 File Offset: 0x003975B8
		private void Update()
		{
			if (this.controller == null)
			{
				return;
			}
			if (this.controller.State != GachaMachineController.SessionState.Inside)
			{
				if (this._showRunning)
				{
					this.EndShow();
				}
				return;
			}
			if (!this._showRunning)
			{
				this.BeginShow();
			}
			this.PruneFinishedSpectators();
			if (Time.time < this._nextScanTime)
			{
				return;
			}
			this._nextScanTime = Time.time + Random.Range(Mathf.Min(this.scanIntervalRange.x, this.scanIntervalRange.y), Mathf.Max(this.scanIntervalRange.x, this.scanIntervalRange.y));
			this.RecruitSpectators();
			this.RecruitStaticWatchers();
		}

		// Token: 0x06008D0B RID: 36107 RVA: 0x00399468 File Offset: 0x00397668
		private void BeginShow()
		{
			this._showRunning = true;
			this._nextScanTime = 0f;
			this.BuildSlots();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaCrowd] Show started: {0} spectator slots.", this._slots.Count), this);
			}
		}

		// Token: 0x06008D0C RID: 36108 RVA: 0x003994B8 File Offset: 0x003976B8
		private void EndShow()
		{
			this._showRunning = false;
			for (int i = this._spectators.Count - 1; i >= 0; i--)
			{
				GachaSpectatorBehaviour gachaSpectatorBehaviour = this._spectators[i];
				if (gachaSpectatorBehaviour != null)
				{
					gachaSpectatorBehaviour.Release();
				}
			}
			this._spectators.Clear();
			for (int j = 0; j < this._staticWatchers.Count; j++)
			{
				GachaCrowdAttractor.StaticWatcher staticWatcher = this._staticWatchers[j];
				if (staticWatcher.LookAt != null)
				{
					staticWatcher.LookAt.StopLooking();
					staticWatcher.LookAt.horizontalOnly = staticWatcher.HadHorizontalOnly;
				}
				GachaCrowdAttractor.SetWatcherExpression(staticWatcher.Animator, 0);
				if (staticWatcher.PausedDriver != null)
				{
					staticWatcher.PausedDriver.enabled = true;
				}
			}
			this._staticWatchers.Clear();
			for (int k = 0; k < this._slotTaken.Length; k++)
			{
				this._slotTaken[k] = false;
			}
			if (this.debugLogs)
			{
				Debug.Log("[GachaCrowd] Show ended; spectators released.", this);
			}
		}

		// Token: 0x06008D0D RID: 36109 RVA: 0x003995C0 File Offset: 0x003977C0
		private void RecruitStaticWatchers()
		{
			Transform transform = ((this.lookTarget != null) ? this.lookTarget : base.transform);
			Vector3 position = base.transform.position;
			float num = this.staticWatcherRadius * this.staticWatcherRadius;
			foreach (NPCSimpleLookAt npcsimpleLookAt in Object.FindObjectsByType<NPCSimpleLookAt>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!(npcsimpleLookAt == null) && !this.IsAlreadyWatching(npcsimpleLookAt) && this.IsOnSameFloorWithin(npcsimpleLookAt.transform.position, position, num))
				{
					NPCStateMachine componentInParent = npcsimpleLookAt.GetComponentInParent<NPCStateMachine>();
					if (!(componentInParent != null) || !componentInParent.IsInitialized)
					{
						FieldPlayerLookAtCiel fieldPlayerLookAtCiel = npcsimpleLookAt.GetComponentInParent<FieldPlayerLookAtCiel>();
						if (fieldPlayerLookAtCiel != null && fieldPlayerLookAtCiel.isActiveAndEnabled)
						{
							fieldPlayerLookAtCiel.enabled = false;
						}
						else
						{
							fieldPlayerLookAtCiel = null;
						}
						Animator componentInChildren = ((componentInParent != null) ? componentInParent.transform : npcsimpleLookAt.transform).GetComponentInChildren<Animator>(true);
						bool horizontalOnly = npcsimpleLookAt.horizontalOnly;
						npcsimpleLookAt.horizontalOnly = false;
						npcsimpleLookAt.StartLookingAt(transform);
						GachaCrowdAttractor.SetWatcherExpression(componentInChildren, 1);
						this._staticWatchers.Add(new GachaCrowdAttractor.StaticWatcher
						{
							LookAt = npcsimpleLookAt,
							PausedDriver = fieldPlayerLookAtCiel,
							Animator = componentInChildren,
							HadHorizontalOnly = horizontalOnly
						});
						if (this.debugLogs)
						{
							Debug.Log("[GachaCrowd] Static watcher '" + npcsimpleLookAt.name + "' now staring.", this);
						}
					}
				}
			}
		}

		// Token: 0x06008D0E RID: 36110 RVA: 0x00399748 File Offset: 0x00397948
		private bool IsOnSameFloorWithin(Vector3 npcPosition, Vector3 center, float radiusSqr)
		{
			if (Mathf.Abs(npcPosition.y - center.y) > this.maximumFloorHeightDifference)
			{
				return false;
			}
			Vector3 vector = npcPosition - center;
			vector.y = 0f;
			return vector.sqrMagnitude <= radiusSqr;
		}

		// Token: 0x06008D0F RID: 36111 RVA: 0x00399794 File Offset: 0x00397994
		private bool IsAlreadyWatching(NPCSimpleLookAt lookAt)
		{
			for (int i = 0; i < this._staticWatchers.Count; i++)
			{
				if (this._staticWatchers[i].LookAt == lookAt)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008D10 RID: 36112 RVA: 0x003997D4 File Offset: 0x003979D4
		private static void SetWatcherExpression(Animator animator, int value)
		{
			if (animator == null)
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Int && animatorControllerParameter.nameHash == GachaCrowdAttractor.FaceExpressionHash)
				{
					animator.SetInteger(GachaCrowdAttractor.FaceExpressionHash, value);
					return;
				}
			}
		}

		// Token: 0x06008D11 RID: 36113 RVA: 0x00399828 File Offset: 0x00397A28
		private void BuildSlots()
		{
			this._slots.Clear();
			for (int i = 0; i < this._slotTaken.Length; i++)
			{
				this._slotTaken[i] = false;
			}
			int num = Mathf.Min(this.maximumSpectators, this._slotTaken.Length);
			Vector3 position = base.transform.position;
			Vector3 vector = base.transform.forward;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			vector.Normalize();
			for (int j = 0; j < num; j++)
			{
				float num2 = (float)((j + 1) / 2);
				float num3 = ((j % 2 == 0) ? 1f : (-1f));
				float num4 = ((num > 1) ? (this.ringArcDegrees / (float)(num - 1)) : 0f);
				Vector3 vector2 = Quaternion.AngleAxis(num2 * num4 * num3, Vector3.up) * vector;
				this._slots.Add(position + vector2 * this.ringRadius);
			}
		}

		// Token: 0x06008D12 RID: 36114 RVA: 0x00399928 File Offset: 0x00397B28
		private void PruneFinishedSpectators()
		{
			for (int i = this._spectators.Count - 1; i >= 0; i--)
			{
				GachaSpectatorBehaviour gachaSpectatorBehaviour = this._spectators[i];
				if (gachaSpectatorBehaviour == null || !gachaSpectatorBehaviour.IsBusy)
				{
					if (gachaSpectatorBehaviour != null)
					{
						this.FreeSlot(gachaSpectatorBehaviour.SlotIndex);
					}
					this._spectators.RemoveAt(i);
				}
			}
		}

		// Token: 0x06008D13 RID: 36115 RVA: 0x0039998C File Offset: 0x00397B8C
		private void RecruitSpectators()
		{
			if (this._spectators.Count >= this.maximumSpectators)
			{
				return;
			}
			NPCStateMachine[] array = Object.FindObjectsByType<NPCStateMachine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			Vector3 position = base.transform.position;
			float num = this.attractRadius * this.attractRadius;
			int num2 = 0;
			foreach (NPCStateMachine npcstateMachine in array)
			{
				if (num2 >= this.recruitsPerScan || this._spectators.Count >= this.maximumSpectators)
				{
					break;
				}
				if (this.IsOnSameFloorWithin(npcstateMachine.transform.position, position, num) && GachaCrowdAttractor.IsNpcAvailable(npcstateMachine))
				{
					int num3;
					Vector3 vector;
					if (!this.TryClaimNearestSlot(npcstateMachine.transform.position, out num3, out vector))
					{
						break;
					}
					GachaSpectatorBehaviour gachaSpectatorBehaviour = npcstateMachine.GetComponent<GachaSpectatorBehaviour>() ?? npcstateMachine.gameObject.AddComponent<GachaSpectatorBehaviour>();
					GachaSpectatorBehaviour.Settings settings = new GachaSpectatorBehaviour.Settings
					{
						SlotPosition = vector,
						SlotIndex = num3,
						LookTarget = ((this.lookTarget != null) ? this.lookTarget : base.transform),
						ApproachTimeout = this.approachTimeout,
						WatchingExpressionIndex = this.watchingExpressionIndex,
						DebugLogs = this.debugLogs
					};
					if (!gachaSpectatorBehaviour.Begin(this, settings))
					{
						this.FreeSlot(num3);
					}
					else
					{
						this._spectators.Add(gachaSpectatorBehaviour);
						num2++;
						if (this.debugLogs)
						{
							Debug.Log(string.Format("[GachaCrowd] Recruited '{0}' to slot {1}.", npcstateMachine.name, num3), this);
						}
					}
				}
			}
		}

		// Token: 0x06008D14 RID: 36116 RVA: 0x00399B20 File Offset: 0x00397D20
		private bool TryClaimNearestSlot(Vector3 from, out int slotIndex, out Vector3 slotPosition)
		{
			slotIndex = -1;
			slotPosition = Vector3.zero;
			float num = float.PositiveInfinity;
			for (int i = 0; i < this._slots.Count; i++)
			{
				if (!this._slotTaken[i])
				{
					float sqrMagnitude = (this._slots[i] - from).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						slotIndex = i;
						slotPosition = this._slots[i];
					}
				}
			}
			if (slotIndex < 0)
			{
				return false;
			}
			this._slotTaken[slotIndex] = true;
			return true;
		}

		// Token: 0x06008D15 RID: 36117 RVA: 0x00399BA9 File Offset: 0x00397DA9
		internal void FreeSlot(int slotIndex)
		{
			if (slotIndex >= 0 && slotIndex < this._slotTaken.Length)
			{
				this._slotTaken[slotIndex] = false;
			}
		}

		// Token: 0x06008D16 RID: 36118 RVA: 0x00399BC4 File Offset: 0x00397DC4
		private static bool IsNpcAvailable(NPCStateMachine npc)
		{
			if (npc == null || !npc.isActiveAndEnabled || !npc.IsInitialized)
			{
				return false;
			}
			if (npc.CurrentStateType != NPCStateType.Idle && npc.CurrentStateType != NPCStateType.Wandering)
			{
				return false;
			}
			AStarNPCController component = npc.GetComponent<AStarNPCController>();
			NPCWanderController component2 = npc.GetComponent<NPCWanderController>();
			NPCSenses component3 = npc.GetComponent<NPCSenses>();
			GachaSpectatorBehaviour component4 = npc.GetComponent<GachaSpectatorBehaviour>();
			Animator componentInChildren = npc.GetComponentInChildren<Animator>();
			return component != null && component.isActiveAndEnabled && !component.SuppressPOIBehavior && component2 != null && component2.isActiveAndEnabled && !component2.ExternalControl && (component3 == null || (!component3.IsAlert && !component3.IsDetected)) && (component4 == null || !component4.IsBusy) && componentInChildren != null && componentInChildren.isHuman;
		}

		// Token: 0x06008D17 RID: 36119 RVA: 0x00399C95 File Offset: 0x00397E95
		private void OnDisable()
		{
			if (this._showRunning)
			{
				this.EndShow();
			}
		}

		// Token: 0x06008D18 RID: 36120 RVA: 0x00399CA8 File Offset: 0x00397EA8
		private void OnValidate()
		{
			this.maximumSpectators = Mathf.Clamp(this.maximumSpectators, 1, this._slotTaken.Length);
			this.recruitsPerScan = Mathf.Max(1, this.recruitsPerScan);
			this.attractRadius = Mathf.Max(2f, this.attractRadius);
			this.ringRadius = Mathf.Max(1f, this.ringRadius);
			this.approachTimeout = Mathf.Max(3f, this.approachTimeout);
		}

		// Token: 0x06008D19 RID: 36121 RVA: 0x00399D24 File Offset: 0x00397F24
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(1f, 0.4f, 0.8f, 0.7f);
			Gizmos.DrawWireSphere(base.transform.position, this.attractRadius);
			if (this._slots.Count == 0)
			{
				return;
			}
			Gizmos.color = new Color(0.3f, 1f, 0.5f, 0.9f);
			foreach (Vector3 vector in this._slots)
			{
				Gizmos.DrawWireSphere(vector + Vector3.up * 0.1f, 0.25f);
			}
		}

		// Token: 0x04008FF6 RID: 36854
		[Header("Wiring")]
		[SerializeField]
		private GachaMachineController controller;

		// Token: 0x04008FF7 RID: 36855
		[Tooltip("Point on the display glass the spectators stare at.")]
		[SerializeField]
		private Transform lookTarget;

		// Token: 0x04008FF8 RID: 36856
		[Header("Recruiting")]
		[SerializeField]
		[Min(1f)]
		private int maximumSpectators = 6;

		// Token: 0x04008FF9 RID: 36857
		[SerializeField]
		[Min(2f)]
		private float attractRadius = 24f;

		// Token: 0x04008FFA RID: 36858
		[SerializeField]
		[Min(1f)]
		private int recruitsPerScan = 2;

		// Token: 0x04008FFB RID: 36859
		[SerializeField]
		private Vector2 scanIntervalRange = new Vector2(1.4f, 2.4f);

		// Token: 0x04008FFC RID: 36860
		[Header("Ring")]
		[Tooltip("World distance of the spectator arc from the machine centre.")]
		[SerializeField]
		[Min(1f)]
		private float ringRadius = 2.7f;

		// Token: 0x04008FFD RID: 36861
		[Tooltip("Arc centred on the machine's front where the slots are placed.")]
		[SerializeField]
		[Range(60f, 300f)]
		private float ringArcDegrees = 190f;

		// Token: 0x04008FFE RID: 36862
		[SerializeField]
		[Min(3f)]
		private float approachTimeout = 16f;

		// Token: 0x04008FFF RID: 36863
		[Header("Static watchers")]
		[Tooltip("POI scenes are populated with baked/seated NPCs whose FSM never initializes; they cannot walk over, but everyone within this radius turns their head to stare at the glass.")]
		[SerializeField]
		[Min(2f)]
		private float staticWatcherRadius = 15f;

		// Token: 0x04009000 RID: 36864
		[Tooltip("NPCs on other mall floors are geometrically close but must not crane at the ceiling; only recruit within this height difference.")]
		[SerializeField]
		[Min(0.5f)]
		private float maximumFloorHeightDifference = 2.5f;

		// Token: 0x04009001 RID: 36865
		[Header("Spectator look")]
		[Tooltip("FaceExpressionIndex applied while watching; -1 disables.")]
		[SerializeField]
		private int watchingExpressionIndex = 1;

		// Token: 0x04009002 RID: 36866
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04009003 RID: 36867
		private static readonly int FaceExpressionHash = Animator.StringToHash("FaceExpressionIndex");

		// Token: 0x04009004 RID: 36868
		private readonly List<GachaSpectatorBehaviour> _spectators = new List<GachaSpectatorBehaviour>(8);

		// Token: 0x04009005 RID: 36869
		private readonly List<GachaCrowdAttractor.StaticWatcher> _staticWatchers = new List<GachaCrowdAttractor.StaticWatcher>(24);

		// Token: 0x04009006 RID: 36870
		private readonly List<Vector3> _slots = new List<Vector3>(8);

		// Token: 0x04009007 RID: 36871
		private readonly bool[] _slotTaken = new bool[16];

		// Token: 0x04009008 RID: 36872
		private float _nextScanTime;

		// Token: 0x04009009 RID: 36873
		private bool _showRunning;

		// Token: 0x020011DF RID: 4575
		private struct StaticWatcher
		{
			// Token: 0x0400900A RID: 36874
			public NPCSimpleLookAt LookAt;

			// Token: 0x0400900B RID: 36875
			public FieldPlayerLookAtCiel PausedDriver;

			// Token: 0x0400900C RID: 36876
			public Animator Animator;

			// Token: 0x0400900D RID: 36877
			public bool HadHorizontalOnly;
		}
	}
}
