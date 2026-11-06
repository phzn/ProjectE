using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B72 RID: 2930
	[DisallowMultipleComponent]
	[RequireComponent(typeof(WantedPosterPhotoSystem))]
	public sealed class WantedPosterNpcDirector : MonoBehaviour
	{
		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06005344 RID: 21316 RVA: 0x00204E54 File Offset: 0x00203054
		public int ActiveSequenceCount
		{
			get
			{
				return this._activeSequences;
			}
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x00204E5C File Offset: 0x0020305C
		private void Awake()
		{
			this._photoSystem = base.GetComponent<WantedPosterPhotoSystem>();
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x00204E6A File Offset: 0x0020306A
		private void OnEnable()
		{
			this._nextScanTime = Time.time + WantedPosterNpcDirector.RandomRange(this.initialDelayRange, 0f);
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x00204E88 File Offset: 0x00203088
		private void Update()
		{
			if (!this.CanRunAmbientSequences())
			{
				return;
			}
			if (Time.time < this._nextScanTime)
			{
				return;
			}
			this._nextScanTime = Time.time + WantedPosterNpcDirector.RandomRange(this.scanIntervalRange, 1f);
			if (this._activeSequences >= Mathf.Max(1, this.maxConcurrentSequences) || Time.time < this._nextEventTime)
			{
				return;
			}
			this.TryStartSequence(false, null, null);
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x00204F04 File Offset: 0x00203104
		[ContextMenu("Debug/Force Eligible NPC Reaction")]
		public void ForceEligibleNpcReaction()
		{
			this.TryStartSequence(true, null, null);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x00204F2B File Offset: 0x0020312B
		public bool ForceEligibleNpcReaction(bool forceTakePhoto, bool forceTakePoster)
		{
			return this.TryStartSequence(true, new bool?(forceTakePhoto), new bool?(forceTakePoster));
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x00204F40 File Offset: 0x00203140
		internal void NotifySequenceFinished(NPCWantedPosterBehaviour behaviour)
		{
			this._activeSequences = Mathf.Max(0, this._activeSequences - 1);
			if (this.debugLogs && behaviour != null)
			{
				Debug.Log("[WantedPosterNPC] " + behaviour.name + ": sequenza conclusa.");
			}
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x00204F8C File Offset: 0x0020318C
		private bool TryStartSequence(bool ignoreProbabilityAndCooldown, bool? forcedTakePhoto, bool? forcedTakePoster)
		{
			if (!this.CanRunAmbientSequences())
			{
				return false;
			}
			WantedPosterPhotoSystem photoSystem = this._photoSystem;
			if (photoSystem == null || !photoSystem.HasPhoto || photoSystem.CurrentRuntimeMaterial == null)
			{
				return false;
			}
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
			WantedPosterNpcDirector.Candidate candidate = this._candidates[Random.Range(0, this._candidates.Count)];
			bool flag = forcedTakePhoto ?? (Random.value < this.takePhotoChance);
			bool flag2 = forcedTakePoster ?? (Random.value < this.takePosterChance);
			if (!flag && !flag2)
			{
				flag = true;
			}
			NPCWantedPosterBehaviour npcwantedPosterBehaviour = candidate.Npc.GetComponent<NPCWantedPosterBehaviour>() ?? candidate.Npc.gameObject.AddComponent<NPCWantedPosterBehaviour>();
			NPCWantedPosterBehaviour.SequenceSettings sequenceSettings = new NPCWantedPosterBehaviour.SequenceSettings
			{
				ApproachDistance = Mathf.Max(0.55f, this.approachDistance),
				ApproachTimeout = Mathf.Max(3f, this.approachTimeout),
				WalkAwayDistance = WantedPosterNpcDirector.OrderedRange(this.walkAwayDistanceRange, 2f),
				CarryDuration = WantedPosterNpcDirector.OrderedRange(this.carryDurationRange, 2f),
				CarryLocalPosition = this.carryLocalPosition,
				CarryLocalEulerAngles = this.carryLocalEulerAngles,
				CarryLocalScale = this.carryLocalScale,
				CarryArmRaise = this.carryArmRaise,
				MaximumPlayerObservationDistance = this.maximumPlayerObservationDistance,
				PlayerViewportPadding = this.playerViewportPadding,
				PlayerObstructionMask = this.obstructionMask,
				DebugLogs = this.debugLogs
			};
			if (!npcwantedPosterBehaviour.Begin(candidate.Poster, flag, flag2, this, sequenceSettings))
			{
				return false;
			}
			this._activeSequences++;
			this._nextEventTime = Time.time + WantedPosterNpcDirector.RandomRange(this.eventCooldownRange, 1f);
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[WantedPosterNPC] ",
					candidate.Npc.name,
					" ha notato ",
					candidate.Poster.name,
					" ",
					string.Format("(foto={0}, raccoglie={1}).", flag, flag2)
				}));
			}
			return true;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x00205224 File Offset: 0x00203424
		private bool CanRunAmbientSequences()
		{
			if (this._photoSystem == null)
			{
				this._photoSystem = base.GetComponent<WantedPosterPhotoSystem>();
			}
			if (this._photoSystem == null || !this._photoSystem.isActiveAndEnabled || WantedPosterPhotoSystem.Instance != this._photoSystem)
			{
				return false;
			}
			if (!GameSystemsBootstrapper.IsRunReady(true))
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			return instance == null || !instance.IsTransitioning;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x002052A0 File Offset: 0x002034A0
		private void BuildCandidates()
		{
			this._candidates.Clear();
			this._observablePosters.Clear();
			IReadOnlyList<WantedPoster> registeredPosters = WantedPosterPhotoSystem.RegisteredPosters;
			if (registeredPosters == null || registeredPosters.Count == 0)
			{
				return;
			}
			Camera camera;
			Transform transform;
			if (!NpcInterestPlayerVisibility.TryResolveContext(out camera, out transform))
			{
				return;
			}
			for (int i = 0; i < registeredPosters.Count; i++)
			{
				WantedPoster wantedPoster = registeredPosters[i];
				if (!(wantedPoster == null) && wantedPoster.IsNpcInterestEligible && !wantedPoster.IsReservedForNpc && NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, wantedPoster.NpcInterestPoint, wantedPoster.transform, this.maximumPlayerObservationDistance, this.playerViewportPadding, this.obstructionMask))
				{
					this._observablePosters.Add(wantedPoster);
				}
			}
			if (this._observablePosters.Count == 0)
			{
				return;
			}
			foreach (NPCStateMachine npcstateMachine in Object.FindObjectsByType<NPCStateMachine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (WantedPosterNpcDirector.IsNpcAvailable(npcstateMachine))
				{
					Transform transform2 = npcstateMachine.transform;
					NPCSenses component = npcstateMachine.GetComponent<NPCSenses>();
					Animator componentInChildren = npcstateMachine.GetComponentInChildren<Animator>();
					Vector3 vector = WantedPosterNpcDirector.ResolveEyePosition(transform2, component, componentInChildren);
					Vector3 vector2 = ((component != null && component.EyeTransform != null) ? component.EyeTransform.forward : transform2.forward);
					vector2.y = 0f;
					if (vector2.sqrMagnitude < 0.001f)
					{
						vector2 = transform2.forward;
					}
					vector2.Normalize();
					if (NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, vector, transform2, this.maximumPlayerObservationDistance, this.playerViewportPadding, this.obstructionMask))
					{
						for (int k = 0; k < this._observablePosters.Count; k++)
						{
							WantedPoster wantedPoster2 = this._observablePosters[k];
							Vector3 vector3 = wantedPoster2.NpcInterestPoint - vector;
							float magnitude = vector3.magnitude;
							if (magnitude >= this.minimumNoticeDistance && magnitude <= this.maximumNoticeDistance)
							{
								Vector3 vector4 = Vector3.ProjectOnPlane(vector3, Vector3.up);
								if (vector4.sqrMagnitude >= 0.001f)
								{
									vector4.Normalize();
									float num = Mathf.Cos(this.noticeViewAngle * 0.5f * 0.017453292f);
									if (Vector3.Dot(vector2, vector4) >= num && this.HasLineOfSight(transform2, vector, wantedPoster2, vector3, magnitude))
									{
										this._candidates.Add(new WantedPosterNpcDirector.Candidate
										{
											Npc = npcstateMachine,
											Poster = wantedPoster2
										});
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x00205518 File Offset: 0x00203718
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
			NPCWantedPosterBehaviour component4 = npc.GetComponent<NPCWantedPosterBehaviour>();
			NPCPhotoBoothPrintBehaviour component5 = npc.GetComponent<NPCPhotoBoothPrintBehaviour>();
			Animator componentInChildren = npc.GetComponentInChildren<Animator>();
			return component != null && component.isActiveAndEnabled && !component.SuppressPOIBehavior && component2 != null && component2.isActiveAndEnabled && !component2.ExternalControl && (component3 == null || (!component3.IsAlert && !component3.IsDetected)) && (component4 == null || !component4.IsBusy) && (component5 == null || !component5.IsBusy) && componentInChildren != null && componentInChildren.isHuman;
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x00205604 File Offset: 0x00203804
		private bool HasLineOfSight(Transform npcRoot, Vector3 origin, WantedPoster poster, Vector3 direction, float distance)
		{
			RaycastHit[] array = Physics.RaycastAll(origin, direction.normalized, distance + 0.1f, this.obstructionMask, QueryTriggerInteraction.Ignore);
			if (array.Length == 0)
			{
				return true;
			}
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			foreach (RaycastHit raycastHit in array)
			{
				Transform transform = raycastHit.transform;
				if (!(transform == null) && !transform.IsChildOf(npcRoot))
				{
					return transform == poster.transform || transform.IsChildOf(poster.transform) || poster.transform.IsChildOf(transform);
				}
			}
			return true;
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x002056C8 File Offset: 0x002038C8
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

		// Token: 0x06005351 RID: 21329 RVA: 0x0020574C File Offset: 0x0020394C
		private static Vector2 OrderedRange(Vector2 value, float minimum)
		{
			float num = Mathf.Max(minimum, Mathf.Min(value.x, value.y));
			float num2 = Mathf.Max(num, Mathf.Max(value.x, value.y));
			return new Vector2(num, num2);
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x00205790 File Offset: 0x00203990
		private static float RandomRange(Vector2 value, float minimum)
		{
			Vector2 vector = WantedPosterNpcDirector.OrderedRange(value, minimum);
			return Random.Range(vector.x, vector.y);
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x002057B8 File Offset: 0x002039B8
		private void OnValidate()
		{
			this.initialDelayRange = WantedPosterNpcDirector.OrderedRange(this.initialDelayRange, 0f);
			this.scanIntervalRange = WantedPosterNpcDirector.OrderedRange(this.scanIntervalRange, 1f);
			this.eventCooldownRange = WantedPosterNpcDirector.OrderedRange(this.eventCooldownRange, 1f);
			this.walkAwayDistanceRange = WantedPosterNpcDirector.OrderedRange(this.walkAwayDistanceRange, 2f);
			this.carryDurationRange = WantedPosterNpcDirector.OrderedRange(this.carryDurationRange, 2f);
			this.minimumNoticeDistance = Mathf.Max(0f, this.minimumNoticeDistance);
			this.maximumNoticeDistance = Mathf.Max(this.minimumNoticeDistance, this.maximumNoticeDistance);
			this.approachDistance = Mathf.Max(0.55f, this.approachDistance);
			this.approachTimeout = Mathf.Max(3f, this.approachTimeout);
			this.maxConcurrentSequences = Mathf.Max(1, this.maxConcurrentSequences);
			this.carryArmRaise = Mathf.Clamp01(this.carryArmRaise);
			this.maximumPlayerObservationDistance = Mathf.Max(1f, this.maximumPlayerObservationDistance);
			this.playerViewportPadding = Mathf.Clamp(this.playerViewportPadding, 0f, 0.2f);
		}

		// Token: 0x040053BE RID: 21438
		[Header("Frequency")]
		[Tooltip("Random delay before the first possible observation after entering Play Mode.")]
		[SerializeField]
		private Vector2 initialDelayRange = new Vector2(2f, 5f);

		// Token: 0x040053BF RID: 21439
		[Tooltip("How often the lightweight NPC/poster eligibility scan runs.")]
		[SerializeField]
		private Vector2 scanIntervalRange = new Vector2(2f, 3.5f);

		// Token: 0x040053C0 RID: 21440
		[Tooltip("Chance per eligible scan that an NPC actually decides to react.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float noticeChancePerScan = 0.85f;

		// Token: 0x040053C1 RID: 21441
		[Tooltip("Global pause after a sequence starts, so the behaviour remains occasional.")]
		[SerializeField]
		private Vector2 eventCooldownRange = new Vector2(14f, 24f);

		// Token: 0x040053C2 RID: 21442
		[Min(1f)]
		[SerializeField]
		private int maxConcurrentSequences = 1;

		// Token: 0x040053C3 RID: 21443
		[Header("Perception")]
		[SerializeField]
		private float minimumNoticeDistance = 1.25f;

		// Token: 0x040053C4 RID: 21444
		[SerializeField]
		private float maximumNoticeDistance = 18f;

		// Token: 0x040053C5 RID: 21445
		[Range(20f, 180f)]
		[SerializeField]
		private float noticeViewAngle = 150f;

		// Token: 0x040053C6 RID: 21446
		[SerializeField]
		private LayerMask obstructionMask = -1;

		// Token: 0x040053C7 RID: 21447
		[Header("Player observation gate")]
		[Tooltip("Both the poster and the selected NPC must be this close to the player and visible on Camera.main before an ambient sequence may start.")]
		[Min(1f)]
		[SerializeField]
		private float maximumPlayerObservationDistance = 24f;

		// Token: 0x040053C8 RID: 21448
		[Tooltip("Keeps the relevant points away from clipped screen edges.")]
		[Range(0f, 0.2f)]
		[SerializeField]
		private float playerViewportPadding = 0.015f;

		// Token: 0x040053C9 RID: 21449
		[Header("Possible reactions")]
		[Range(0f, 1f)]
		[SerializeField]
		private float takePhotoChance = 0.85f;

		// Token: 0x040053CA RID: 21450
		[Range(0f, 1f)]
		[SerializeField]
		private float takePosterChance = 0.75f;

		// Token: 0x040053CB RID: 21451
		[Header("Sequence")]
		[SerializeField]
		private float approachDistance = 1.05f;

		// Token: 0x040053CC RID: 21452
		[SerializeField]
		private float approachTimeout = 14f;

		// Token: 0x040053CD RID: 21453
		[SerializeField]
		private Vector2 walkAwayDistanceRange = new Vector2(9f, 14f);

		// Token: 0x040053CE RID: 21454
		[SerializeField]
		private Vector2 carryDurationRange = new Vector2(10f, 15f);

		// Token: 0x040053CF RID: 21455
		[Header("Carried poster pose")]
		[Tooltip("Poster root relative to the NPC. The mesh pivot is at the bottom of the paper.")]
		[SerializeField]
		private Vector3 carryLocalPosition = new Vector3(0f, 0.96f, 0.36f);

		// Token: 0x040053D0 RID: 21456
		[SerializeField]
		private Vector3 carryLocalEulerAngles = new Vector3(0f, 180f, 0f);

		// Token: 0x040053D1 RID: 21457
		[SerializeField]
		private Vector3 carryLocalScale = new Vector3(0.58f, 0.58f, 0.58f);

		// Token: 0x040053D2 RID: 21458
		[Tooltip("Blend tra la posa bassa (0) e quella alta (1) delle braccia.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float carryArmRaise = 0.28f;

		// Token: 0x040053D3 RID: 21459
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040053D4 RID: 21460
		private readonly List<WantedPosterNpcDirector.Candidate> _candidates = new List<WantedPosterNpcDirector.Candidate>(32);

		// Token: 0x040053D5 RID: 21461
		private readonly List<WantedPoster> _observablePosters = new List<WantedPoster>(16);

		// Token: 0x040053D6 RID: 21462
		private float _nextScanTime;

		// Token: 0x040053D7 RID: 21463
		private float _nextEventTime;

		// Token: 0x040053D8 RID: 21464
		private int _activeSequences;

		// Token: 0x040053D9 RID: 21465
		private WantedPosterPhotoSystem _photoSystem;

		// Token: 0x02000B73 RID: 2931
		private struct Candidate
		{
			// Token: 0x040053DA RID: 21466
			public NPCStateMachine Npc;

			// Token: 0x040053DB RID: 21467
			public WantedPoster Poster;
		}
	}
}
