using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.RunModes;
using Features.WantedPoster;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000995 RID: 2453
	[DisallowMultipleComponent]
	public sealed class PhotoBoothPrintNpcDirector : MonoBehaviour
	{
		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06004455 RID: 17493 RVA: 0x00199506 File Offset: 0x00197706
		public static PhotoBoothPrintNpcDirector Instance
		{
			get
			{
				return PhotoBoothPrintNpcDirector._instance;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004456 RID: 17494 RVA: 0x0019950D File Offset: 0x0019770D
		public int ActiveSequenceCount
		{
			get
			{
				return this._activeSequences;
			}
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x00199515 File Offset: 0x00197715
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			PhotoBoothPrintNpcDirector._instance = null;
			PhotoBoothPrintNpcDirector.RegisteredPrints.Clear();
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x00199527 File Offset: 0x00197727
		internal static void RegisterWorldPrint(PhotoBoothPrint print)
		{
			if (!Application.isPlaying || print == null)
			{
				return;
			}
			if (!PhotoBoothPrintNpcDirector.RegisteredPrints.Contains(print))
			{
				PhotoBoothPrintNpcDirector.RegisteredPrints.Add(print);
			}
			PhotoBoothPrintNpcDirector.EnsureInstance();
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x00199557 File Offset: 0x00197757
		internal static void UnregisterWorldPrint(PhotoBoothPrint print)
		{
			if (print != null)
			{
				PhotoBoothPrintNpcDirector.RegisteredPrints.Remove(print);
			}
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x00199570 File Offset: 0x00197770
		private static void EnsureInstance()
		{
			if (PhotoBoothPrintNpcDirector._instance != null)
			{
				return;
			}
			PhotoBoothPrintNpcDirector._instance = Object.FindObjectOfType<PhotoBoothPrintNpcDirector>(true);
			if (PhotoBoothPrintNpcDirector._instance != null)
			{
				return;
			}
			PhotoBoothPrintNpcDirector._instance = new GameObject("PhotoBoothPrintNpcDirector")
			{
				hideFlags = HideFlags.DontSave
			}.AddComponent<PhotoBoothPrintNpcDirector>();
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x001995C0 File Offset: 0x001977C0
		private void Awake()
		{
			if (PhotoBoothPrintNpcDirector._instance != null && PhotoBoothPrintNpcDirector._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PhotoBoothPrintNpcDirector._instance = this;
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x001995EE File Offset: 0x001977EE
		private void OnEnable()
		{
			this._nextScanTime = Time.time + PhotoBoothPrintNpcDirector.RandomRange(this.initialDelayRange, 0f);
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0019960C File Offset: 0x0019780C
		private void Update()
		{
			if (Time.time < this._nextScanTime)
			{
				return;
			}
			this._nextScanTime = Time.time + PhotoBoothPrintNpcDirector.RandomRange(this.scanIntervalRange, 1f);
			if (this._activeSequences >= Mathf.Max(1, this.maxConcurrentSequences) || Time.time < this._nextEventTime)
			{
				return;
			}
			this.TryStartSequence(false, null, null);
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x00199680 File Offset: 0x00197880
		[ContextMenu("Debug/Force Eligible NPC Reaction")]
		public void ForceEligibleNpcReaction()
		{
			this.TryStartSequence(true, null, null);
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x001996A7 File Offset: 0x001978A7
		public bool ForceEligibleNpcReaction(bool forceTakePhoto, bool forceTakePrint)
		{
			return this.TryStartSequence(true, new bool?(forceTakePhoto), new bool?(forceTakePrint));
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x001996BC File Offset: 0x001978BC
		internal void NotifySequenceFinished(NPCPhotoBoothPrintBehaviour behaviour)
		{
			this._activeSequences = Mathf.Max(0, this._activeSequences - 1);
			if (this.debugLogs && behaviour != null)
			{
				Debug.Log("[PhotoBoothPrintNPC] " + behaviour.name + ": sequenza conclusa.");
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x00199708 File Offset: 0x00197908
		private bool TryStartSequence(bool ignoreProbabilityAndCooldown, bool? forcedTakePhoto, bool? forcedTakePrint)
		{
			if (!ignoreProbabilityAndCooldown && (this._activeSequences >= Mathf.Max(1, this.maxConcurrentSequences) || Time.time < this._nextEventTime))
			{
				return false;
			}
			this.BuildCandidates();
			if (this._candidates.Count == 0)
			{
				return false;
			}
			if (!ignoreProbabilityAndCooldown && Random.value > this.noticeChancePerScan)
			{
				return false;
			}
			if (ignoreProbabilityAndCooldown)
			{
				RunModeService.DisqualifyLeaderboardRun("An NPC reaction was forced with a debug control.");
			}
			PhotoBoothPrintNpcDirector.Candidate candidate = this._candidates[Random.Range(0, this._candidates.Count)];
			bool flag = forcedTakePhoto ?? (Random.value < this.takePhotoChance);
			bool flag2 = forcedTakePrint ?? (Random.value < this.takePrintChance);
			if (!flag && !flag2)
			{
				flag = true;
			}
			NPCPhotoBoothPrintBehaviour npcphotoBoothPrintBehaviour = candidate.Npc.GetComponent<NPCPhotoBoothPrintBehaviour>() ?? candidate.Npc.gameObject.AddComponent<NPCPhotoBoothPrintBehaviour>();
			NPCPhotoBoothPrintBehaviour.SequenceSettings sequenceSettings = new NPCPhotoBoothPrintBehaviour.SequenceSettings
			{
				ApproachDistance = Mathf.Max(0.55f, this.approachDistance),
				ApproachTimeout = Mathf.Max(3f, this.approachTimeout),
				WalkAwayDistance = PhotoBoothPrintNpcDirector.OrderedRange(this.walkAwayDistanceRange, 2f),
				CarryDuration = PhotoBoothPrintNpcDirector.OrderedRange(this.carryDurationRange, 2f),
				CarryLocalPosition = this.carryLocalPosition,
				CarryLocalEulerAngles = this.carryLocalEulerAngles,
				CarryLocalScale = this.carryLocalScale,
				CarryArmRaise = this.carryArmRaise,
				MaximumPlayerObservationDistance = this.maximumPlayerObservationDistance,
				PlayerViewportPadding = this.playerViewportPadding,
				PlayerObstructionMask = this.obstructionMask,
				DebugLogs = this.debugLogs
			};
			if (!npcphotoBoothPrintBehaviour.Begin(candidate.Print, flag, flag2, this, sequenceSettings))
			{
				return false;
			}
			this._activeSequences++;
			this._nextEventTime = Time.time + PhotoBoothPrintNpcDirector.RandomRange(this.eventCooldownRange, 1f);
			if (this.debugLogs)
			{
				Debug.Log("[PhotoBoothPrintNPC] " + candidate.Npc.name + " ha notato " + string.Format("{0} (foto={1}, raccoglie={2}).", candidate.Print.name, flag, flag2));
			}
			return true;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x00199950 File Offset: 0x00197B50
		private void BuildCandidates()
		{
			this._candidates.Clear();
			this._observablePrints.Clear();
			for (int i = PhotoBoothPrintNpcDirector.RegisteredPrints.Count - 1; i >= 0; i--)
			{
				if (PhotoBoothPrintNpcDirector.RegisteredPrints[i] == null)
				{
					PhotoBoothPrintNpcDirector.RegisteredPrints.RemoveAt(i);
				}
			}
			if (PhotoBoothPrintNpcDirector.RegisteredPrints.Count == 0)
			{
				return;
			}
			Camera camera;
			Transform transform;
			if (!NpcInterestPlayerVisibility.TryResolveContext(out camera, out transform))
			{
				return;
			}
			for (int j = 0; j < PhotoBoothPrintNpcDirector.RegisteredPrints.Count; j++)
			{
				PhotoBoothPrint photoBoothPrint = PhotoBoothPrintNpcDirector.RegisteredPrints[j];
				if (!(photoBoothPrint == null) && photoBoothPrint.IsNpcWallInterestEligible && !photoBoothPrint.IsReservedForNpc && NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, photoBoothPrint.NpcInterestPoint, photoBoothPrint.PivotTransform, this.maximumPlayerObservationDistance, this.playerViewportPadding, this.obstructionMask))
				{
					this._observablePrints.Add(photoBoothPrint);
				}
			}
			if (this._observablePrints.Count == 0)
			{
				return;
			}
			foreach (NPCStateMachine npcstateMachine in Object.FindObjectsByType<NPCStateMachine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (PhotoBoothPrintNpcDirector.IsNpcAvailable(npcstateMachine))
				{
					Transform transform2 = npcstateMachine.transform;
					NPCSenses component = npcstateMachine.GetComponent<NPCSenses>();
					Animator componentInChildren = npcstateMachine.GetComponentInChildren<Animator>();
					Vector3 vector = PhotoBoothPrintNpcDirector.ResolveEyePosition(transform2, component, componentInChildren);
					Vector3 vector2 = ((component != null && component.EyeTransform != null) ? component.EyeTransform.forward : transform2.forward);
					vector2.y = 0f;
					if (vector2.sqrMagnitude < 0.001f)
					{
						vector2 = transform2.forward;
					}
					vector2.Normalize();
					if (NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, vector, transform2, this.maximumPlayerObservationDistance, this.playerViewportPadding, this.obstructionMask))
					{
						for (int l = 0; l < this._observablePrints.Count; l++)
						{
							PhotoBoothPrint photoBoothPrint2 = this._observablePrints[l];
							Vector3 vector3 = photoBoothPrint2.NpcInterestPoint - vector;
							float magnitude = vector3.magnitude;
							if (magnitude >= this.minimumNoticeDistance && magnitude <= this.maximumNoticeDistance)
							{
								Vector3 vector4 = Vector3.ProjectOnPlane(vector3, Vector3.up);
								if (vector4.sqrMagnitude >= 0.001f)
								{
									vector4.Normalize();
									float num = Mathf.Cos(this.noticeViewAngle * 0.5f * 0.017453292f);
									if (Vector3.Dot(vector2, vector4) >= num && this.HasLineOfSight(transform2, vector, photoBoothPrint2, vector3, magnitude))
									{
										this._candidates.Add(new PhotoBoothPrintNpcDirector.Candidate
										{
											Npc = npcstateMachine,
											Print = photoBoothPrint2
										});
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x00199C00 File Offset: 0x00197E00
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
			NPCPhotoBoothPrintBehaviour component4 = npc.GetComponent<NPCPhotoBoothPrintBehaviour>();
			NPCWantedPosterBehaviour component5 = npc.GetComponent<NPCWantedPosterBehaviour>();
			Animator componentInChildren = npc.GetComponentInChildren<Animator>();
			return component != null && component.isActiveAndEnabled && !component.SuppressPOIBehavior && component2 != null && component2.isActiveAndEnabled && !component2.ExternalControl && (component3 == null || (!component3.IsAlert && !component3.IsDetected)) && (component4 == null || !component4.IsBusy) && (component5 == null || !component5.IsBusy) && componentInChildren != null && componentInChildren.isHuman;
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x00199CEC File Offset: 0x00197EEC
		private bool HasLineOfSight(Transform npcRoot, Vector3 origin, PhotoBoothPrint print, Vector3 direction, float distance)
		{
			RaycastHit[] array = Physics.RaycastAll(origin, direction.normalized, distance + 0.1f, this.obstructionMask, QueryTriggerInteraction.Ignore);
			if (array.Length == 0)
			{
				return true;
			}
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			Transform pivotTransform = print.PivotTransform;
			foreach (RaycastHit raycastHit in array)
			{
				Transform transform = raycastHit.transform;
				if (!(transform == null) && !transform.IsChildOf(npcRoot))
				{
					return transform == print.transform || transform.IsChildOf(print.transform) || transform == pivotTransform || transform.IsChildOf(pivotTransform);
				}
			}
			return true;
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x00199DBC File Offset: 0x00197FBC
		private static Vector3 ResolveEyePosition(Transform npc, NPCSenses senses, Animator animator)
		{
			if (animator != null && animator.isHuman)
			{
				Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Head);
				if (boneTransform != null)
				{
					return boneTransform.position;
				}
			}
			if (senses != null && senses.EyeTransform != null && senses.EyeTransform != npc)
			{
				return senses.EyeTransform.position;
			}
			return npc.position + Vector3.up * 1.55f;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x00199E40 File Offset: 0x00198040
		private static Vector2 OrderedRange(Vector2 value, float minimum)
		{
			float num = Mathf.Max(minimum, Mathf.Min(value.x, value.y));
			float num2 = Mathf.Max(num, Mathf.Max(value.x, value.y));
			return new Vector2(num, num2);
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x00199E84 File Offset: 0x00198084
		private static float RandomRange(Vector2 value, float minimum)
		{
			Vector2 vector = PhotoBoothPrintNpcDirector.OrderedRange(value, minimum);
			return Random.Range(vector.x, vector.y);
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x00199EAC File Offset: 0x001980AC
		private void OnValidate()
		{
			this.initialDelayRange = PhotoBoothPrintNpcDirector.OrderedRange(this.initialDelayRange, 0f);
			this.scanIntervalRange = PhotoBoothPrintNpcDirector.OrderedRange(this.scanIntervalRange, 1f);
			this.eventCooldownRange = PhotoBoothPrintNpcDirector.OrderedRange(this.eventCooldownRange, 1f);
			this.walkAwayDistanceRange = PhotoBoothPrintNpcDirector.OrderedRange(this.walkAwayDistanceRange, 2f);
			this.carryDurationRange = PhotoBoothPrintNpcDirector.OrderedRange(this.carryDurationRange, 2f);
			this.minimumNoticeDistance = Mathf.Max(0f, this.minimumNoticeDistance);
			this.maximumNoticeDistance = Mathf.Max(this.minimumNoticeDistance, this.maximumNoticeDistance);
			this.approachDistance = Mathf.Max(0.55f, this.approachDistance);
			this.approachTimeout = Mathf.Max(3f, this.approachTimeout);
			this.maxConcurrentSequences = Mathf.Max(1, this.maxConcurrentSequences);
			this.carryArmRaise = Mathf.Clamp01(this.carryArmRaise);
			this.maximumPlayerObservationDistance = Mathf.Max(1f, this.maximumPlayerObservationDistance);
			this.playerViewportPadding = Mathf.Clamp(this.playerViewportPadding, 0f, 0.2f);
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x00199FD4 File Offset: 0x001981D4
		private void OnDestroy()
		{
			if (PhotoBoothPrintNpcDirector._instance == this)
			{
				PhotoBoothPrintNpcDirector._instance = null;
			}
		}

		// Token: 0x040043F7 RID: 17399
		[Header("Frequency")]
		[SerializeField]
		private Vector2 initialDelayRange = new Vector2(2f, 5f);

		// Token: 0x040043F8 RID: 17400
		[SerializeField]
		private Vector2 scanIntervalRange = new Vector2(2f, 3.5f);

		// Token: 0x040043F9 RID: 17401
		[Range(0f, 1f)]
		[SerializeField]
		private float noticeChancePerScan = 0.85f;

		// Token: 0x040043FA RID: 17402
		[SerializeField]
		private Vector2 eventCooldownRange = new Vector2(14f, 24f);

		// Token: 0x040043FB RID: 17403
		[Min(1f)]
		[SerializeField]
		private int maxConcurrentSequences = 1;

		// Token: 0x040043FC RID: 17404
		[Header("Perception")]
		[SerializeField]
		private float minimumNoticeDistance = 1.25f;

		// Token: 0x040043FD RID: 17405
		[SerializeField]
		private float maximumNoticeDistance = 18f;

		// Token: 0x040043FE RID: 17406
		[Range(20f, 180f)]
		[SerializeField]
		private float noticeViewAngle = 150f;

		// Token: 0x040043FF RID: 17407
		[SerializeField]
		private LayerMask obstructionMask = -1;

		// Token: 0x04004400 RID: 17408
		[Header("Player observation gate")]
		[Tooltip("Both the wall print and the selected NPC must be this close to the player and visible on Camera.main before an ambient sequence may start.")]
		[Min(1f)]
		[SerializeField]
		private float maximumPlayerObservationDistance = 24f;

		// Token: 0x04004401 RID: 17409
		[Tooltip("Keeps the relevant points away from clipped screen edges.")]
		[Range(0f, 0.2f)]
		[SerializeField]
		private float playerViewportPadding = 0.015f;

		// Token: 0x04004402 RID: 17410
		[Header("Possible reactions")]
		[Range(0f, 1f)]
		[SerializeField]
		private float takePhotoChance = 0.85f;

		// Token: 0x04004403 RID: 17411
		[Range(0f, 1f)]
		[SerializeField]
		private float takePrintChance = 0.75f;

		// Token: 0x04004404 RID: 17412
		[Header("Sequence")]
		[SerializeField]
		private float approachDistance = 0.95f;

		// Token: 0x04004405 RID: 17413
		[SerializeField]
		private float approachTimeout = 14f;

		// Token: 0x04004406 RID: 17414
		[SerializeField]
		private Vector2 walkAwayDistanceRange = new Vector2(8f, 12f);

		// Token: 0x04004407 RID: 17415
		[SerializeField]
		private Vector2 carryDurationRange = new Vector2(9f, 14f);

		// Token: 0x04004408 RID: 17416
		[Header("Ciel photo hold pose - right hand")]
		[SerializeField]
		private Vector3 carryLocalPosition = new Vector3(0.1364f, -0.0136f, 0.0845f);

		// Token: 0x04004409 RID: 17417
		[SerializeField]
		private Vector3 carryLocalEulerAngles = new Vector3(175.82f, 94.614f, -6.760986f);

		// Token: 0x0400440A RID: 17418
		[SerializeField]
		private Vector3 carryLocalScale = new Vector3(0.7142859f, 0.7142863f, 0.714286f);

		// Token: 0x0400440B RID: 17419
		[Tooltip("Uses the same low/raised arms blend and default height as carried Wanted Posters.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float carryArmRaise = 0.28f;

		// Token: 0x0400440C RID: 17420
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400440D RID: 17421
		private static readonly List<PhotoBoothPrint> RegisteredPrints = new List<PhotoBoothPrint>(16);

		// Token: 0x0400440E RID: 17422
		private static PhotoBoothPrintNpcDirector _instance;

		// Token: 0x0400440F RID: 17423
		private readonly List<PhotoBoothPrintNpcDirector.Candidate> _candidates = new List<PhotoBoothPrintNpcDirector.Candidate>(32);

		// Token: 0x04004410 RID: 17424
		private readonly List<PhotoBoothPrint> _observablePrints = new List<PhotoBoothPrint>(16);

		// Token: 0x04004411 RID: 17425
		private float _nextScanTime;

		// Token: 0x04004412 RID: 17426
		private float _nextEventTime;

		// Token: 0x04004413 RID: 17427
		private int _activeSequences;

		// Token: 0x02000996 RID: 2454
		private struct Candidate
		{
			// Token: 0x04004414 RID: 17428
			public NPCStateMachine Npc;

			// Token: 0x04004415 RID: 17429
			public PhotoBoothPrint Print;
		}
	}
}
