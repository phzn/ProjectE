using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.RunModes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.MallHack
{
	// Token: 0x02001003 RID: 4099
	[DisallowMultipleComponent]
	public sealed class HackMeNpcDirector : MonoBehaviour
	{
		// Token: 0x06007C6B RID: 31851 RVA: 0x0032D188 File Offset: 0x0032B388
		internal static void EnsureFor(HackMeStoreZone store)
		{
			if (!Application.isPlaying || store == null || !store.gameObject.scene.IsValid())
			{
				return;
			}
			foreach (HackMeNpcDirector hackMeNpcDirector in Object.FindObjectsByType<HackMeNpcDirector>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (hackMeNpcDirector != null && hackMeNpcDirector.gameObject.scene == store.gameObject.scene)
				{
					return;
				}
			}
			GameObject gameObject = new GameObject("[HackMe] NPC Attraction Director");
			gameObject.hideFlags = HideFlags.DontSave;
			SceneManager.MoveGameObjectToScene(gameObject, store.gameObject.scene);
			gameObject.AddComponent<HackMeNpcDirector>();
		}

		// Token: 0x06007C6C RID: 31852 RVA: 0x0032D225 File Offset: 0x0032B425
		private void OnEnable()
		{
			this._nextScanTime = Time.time + HackMeNpcDirector.RandomRange(this.initialDelayRange, 0f);
		}

		// Token: 0x06007C6D RID: 31853 RVA: 0x0032D244 File Offset: 0x0032B444
		private void Update()
		{
			if (NPCSimulationPause.IsPaused || Time.time < this._nextScanTime)
			{
				return;
			}
			this._nextScanTime = Time.time + HackMeNpcDirector.RandomRange(this.scanIntervalRange, 0.5f);
			if (this._activeObservers >= Mathf.Max(1, this.maxConcurrentObservers))
			{
				return;
			}
			this.TryStartObserver(false);
		}

		// Token: 0x06007C6E RID: 31854 RVA: 0x0032D29F File Offset: 0x0032B49F
		[ContextMenu("Debug/Force Nearby NPC Observation")]
		public void ForceNearbyNpcObservation()
		{
			this.TryStartObserver(true);
		}

		// Token: 0x06007C6F RID: 31855 RVA: 0x0032D2AC File Offset: 0x0032B4AC
		internal void NotifySequenceFinished(HackMeNpcObserver observer, HackMeStoreZone store)
		{
			this._activeObservers = Mathf.Max(0, this._activeObservers - 1);
			int num;
			if (store != null && this._activeByStore.TryGetValue(store, out num))
			{
				num--;
				if (num > 0)
				{
					this._activeByStore[store] = num;
				}
				else
				{
					this._activeByStore.Remove(store);
				}
			}
			if (this.debugLogs && observer != null)
			{
				Debug.Log("[HackMeNPC] " + observer.name + ": observation finished.", observer);
			}
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x0032D338 File Offset: 0x0032B538
		private bool TryStartObserver(bool force)
		{
			this.BuildCandidates();
			if (this._candidates.Count == 0 || (!force && Random.value > this.attractionChancePerScan))
			{
				return false;
			}
			while (this._candidates.Count > 0)
			{
				int num = Random.Range(0, this._candidates.Count);
				HackMeNpcDirector.Candidate candidate = this._candidates[num];
				this._candidates.RemoveAt(num);
				if (force)
				{
					RunModeService.DisqualifyLeaderboardRun("An NPC reaction was forced with a debug control.");
				}
				HackMeNpcObserver hackMeNpcObserver = candidate.Npc.GetComponent<HackMeNpcObserver>() ?? candidate.Npc.gameObject.AddComponent<HackMeNpcObserver>();
				bool flag = Random.value < this.recordVideoChance;
				HackMeNpcObserver.SequenceSettings sequenceSettings = new HackMeNpcObserver.SequenceSettings
				{
					ApproachDistance = Mathf.Max(0.35f, this.approachDistance),
					ApproachTimeout = Mathf.Max(3f, this.approachTimeout),
					LingerAfterCapture = HackMeNpcDirector.OrderedRange(this.lingerAfterCaptureRange, 0f),
					DebugLogs = this.debugLogs
				};
				if (hackMeNpcObserver.Begin(candidate.Store, candidate.Target, candidate.NavigationPoint, flag, this, sequenceSettings))
				{
					this._activeObservers++;
					int num2;
					this._activeByStore.TryGetValue(candidate.Store, out num2);
					this._activeByStore[candidate.Store] = num2 + 1;
					this._npcCooldowns[candidate.Npc.gameObject.GetInstanceID()] = Time.time + HackMeNpcDirector.RandomRange(this.npcCooldownRange, 1f);
					if (this.debugLogs)
					{
						string text = (candidate.Target.IsLargeDisplay ? "TV/monitor" : "phone/tablet");
						Debug.Log(string.Concat(new string[]
						{
							"[HackMeNPC] ",
							candidate.Npc.name,
							" attracted to ",
							candidate.Store.DisplayName,
							": ",
							text,
							", ",
							flag ? "video" : "photo",
							"."
						}), candidate.Npc);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C71 RID: 31857 RVA: 0x0032D568 File Offset: 0x0032B768
		private void BuildCandidates()
		{
			this._candidates.Clear();
			this._availableStores.Clear();
			for (int i = 0; i < HackMeStoreZone.ActiveZones.Count; i++)
			{
				HackMeStoreZone hackMeStoreZone = HackMeStoreZone.ActiveZones[i];
				if (!(hackMeStoreZone == null) && hackMeStoreZone.CanProvokeNpcReactions && !(hackMeStoreZone.gameObject.scene != base.gameObject.scene))
				{
					int num;
					this._activeByStore.TryGetValue(hackMeStoreZone, out num);
					if (num < Mathf.Max(1, this.maxObserversPerStore))
					{
						this._availableStores.Add(hackMeStoreZone);
					}
				}
			}
			if (this._availableStores.Count == 0)
			{
				return;
			}
			foreach (NPCStateMachine npcstateMachine in Object.FindObjectsByType<NPCStateMachine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (HackMeNpcDirector.IsNpcAvailable(npcstateMachine))
				{
					int instanceID = npcstateMachine.gameObject.GetInstanceID();
					float num2;
					if (!this._npcCooldowns.TryGetValue(instanceID, out num2) || Time.time >= num2)
					{
						for (int k = 0; k < this._availableStores.Count; k++)
						{
							HackMeStoreZone hackMeStoreZone2 = this._availableStores[k];
							float num3;
							if (hackMeStoreZone2.DistanceToStore(npcstateMachine.transform.position, out num3) <= this.attractionRadius && num3 <= this.maximumVerticalDistance)
							{
								bool flag = Random.value < this.largeDisplayChance;
								HackMeObservationTarget hackMeObservationTarget;
								if (hackMeStoreZone2.TryGetObservationTarget(npcstateMachine.transform.position, flag, out hackMeObservationTarget))
								{
									this._candidates.Add(new HackMeNpcDirector.Candidate
									{
										Npc = npcstateMachine,
										Store = hackMeStoreZone2,
										Target = hackMeObservationTarget,
										NavigationPoint = hackMeObservationTarget.ApproachPoint
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C72 RID: 31858 RVA: 0x0032D72C File Offset: 0x0032B92C
		private static bool IsNpcAvailable(NPCStateMachine npc)
		{
			if (npc == null || !npc.isActiveAndEnabled || !npc.IsInitialized || (npc.CurrentStateType != NPCStateType.Idle && npc.CurrentStateType != NPCStateType.Wandering))
			{
				return false;
			}
			AStarNPCController component = npc.GetComponent<AStarNPCController>();
			NPCWanderController component2 = npc.GetComponent<NPCWanderController>();
			NPCSenses component3 = npc.GetComponent<NPCSenses>();
			NPCEquipmentManager component4 = npc.GetComponent<NPCEquipmentManager>();
			HackMeNpcObserver component5 = npc.GetComponent<HackMeNpcObserver>();
			Animator componentInChildren = npc.GetComponentInChildren<Animator>();
			return component != null && component.isActiveAndEnabled && !component.SuppressPOIBehavior && component2 != null && component2.isActiveAndEnabled && !component2.ExternalControl && component4 != null && (component3 == null || (!component3.IsAlert && !component3.IsDetected && !component3.IsPassiveReacting)) && (component5 == null || !component5.IsBusy) && componentInChildren != null && componentInChildren.isHuman && HackMeNpcDirector.HasAnimatorParameter(componentInChildren, "ReactionType", AnimatorControllerParameterType.Int) && HackMeNpcDirector.HasAnimatorParameter(componentInChildren, "ReactionTrigger", AnimatorControllerParameterType.Trigger);
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x0032D840 File Offset: 0x0032BA40
		private static bool HasAnimatorParameter(Animator animator, string name, AnimatorControllerParameterType type)
		{
			int num = Animator.StringToHash(name);
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.nameHash == num && animatorControllerParameter.type == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x0032D884 File Offset: 0x0032BA84
		private static Vector2 OrderedRange(Vector2 value, float minimum)
		{
			float num = Mathf.Max(minimum, Mathf.Min(value.x, value.y));
			float num2 = Mathf.Max(num, Mathf.Max(value.x, value.y));
			return new Vector2(num, num2);
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x0032D8C8 File Offset: 0x0032BAC8
		private static float RandomRange(Vector2 value, float minimum)
		{
			Vector2 vector = HackMeNpcDirector.OrderedRange(value, minimum);
			return Random.Range(vector.x, vector.y);
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x0032D8F0 File Offset: 0x0032BAF0
		private void OnValidate()
		{
			this.initialDelayRange = HackMeNpcDirector.OrderedRange(this.initialDelayRange, 0f);
			this.scanIntervalRange = HackMeNpcDirector.OrderedRange(this.scanIntervalRange, 0.5f);
			this.npcCooldownRange = HackMeNpcDirector.OrderedRange(this.npcCooldownRange, 1f);
			this.lingerAfterCaptureRange = HackMeNpcDirector.OrderedRange(this.lingerAfterCaptureRange, 0f);
			this.attractionRadius = Mathf.Max(1f, this.attractionRadius);
			this.maximumVerticalDistance = Mathf.Max(0.5f, this.maximumVerticalDistance);
			this.maxConcurrentObservers = Mathf.Max(1, this.maxConcurrentObservers);
			this.maxObserversPerStore = Mathf.Clamp(this.maxObserversPerStore, 1, this.maxConcurrentObservers);
			this.approachDistance = Mathf.Max(0.35f, this.approachDistance);
			this.approachTimeout = Mathf.Max(3f, this.approachTimeout);
		}

		// Token: 0x0400802E RID: 32814
		[Header("Attraction")]
		[SerializeField]
		private Vector2 initialDelayRange = new Vector2(1.5f, 3f);

		// Token: 0x0400802F RID: 32815
		[SerializeField]
		private Vector2 scanIntervalRange = new Vector2(1.25f, 2.25f);

		// Token: 0x04008030 RID: 32816
		[SerializeField]
		[Range(0f, 1f)]
		private float attractionChancePerScan = 0.72f;

		// Token: 0x04008031 RID: 32817
		[SerializeField]
		[Min(1f)]
		private float attractionRadius = 15f;

		// Token: 0x04008032 RID: 32818
		[SerializeField]
		[Min(0.5f)]
		private float maximumVerticalDistance = 3.25f;

		// Token: 0x04008033 RID: 32819
		[SerializeField]
		[Min(1f)]
		private int maxConcurrentObservers = 4;

		// Token: 0x04008034 RID: 32820
		[SerializeField]
		[Min(1f)]
		private int maxObserversPerStore = 2;

		// Token: 0x04008035 RID: 32821
		[SerializeField]
		private Vector2 npcCooldownRange = new Vector2(38f, 65f);

		// Token: 0x04008036 RID: 32822
		[Header("Device choice")]
		[SerializeField]
		[Range(0f, 1f)]
		private float largeDisplayChance = 0.48f;

		// Token: 0x04008037 RID: 32823
		[SerializeField]
		[Range(0f, 1f)]
		private float recordVideoChance = 0.42f;

		// Token: 0x04008038 RID: 32824
		[Header("Sequence")]
		[SerializeField]
		[Min(0.35f)]
		private float approachDistance = 0.8f;

		// Token: 0x04008039 RID: 32825
		[SerializeField]
		[Min(3f)]
		private float approachTimeout = 16f;

		// Token: 0x0400803A RID: 32826
		[SerializeField]
		private Vector2 lingerAfterCaptureRange = new Vector2(0.35f, 0.9f);

		// Token: 0x0400803B RID: 32827
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400803C RID: 32828
		private readonly List<HackMeNpcDirector.Candidate> _candidates = new List<HackMeNpcDirector.Candidate>(64);

		// Token: 0x0400803D RID: 32829
		private readonly List<HackMeStoreZone> _availableStores = new List<HackMeStoreZone>(4);

		// Token: 0x0400803E RID: 32830
		private readonly Dictionary<int, float> _npcCooldowns = new Dictionary<int, float>();

		// Token: 0x0400803F RID: 32831
		private readonly Dictionary<HackMeStoreZone, int> _activeByStore = new Dictionary<HackMeStoreZone, int>();

		// Token: 0x04008040 RID: 32832
		private float _nextScanTime;

		// Token: 0x04008041 RID: 32833
		private int _activeObservers;

		// Token: 0x02001004 RID: 4100
		private struct Candidate
		{
			// Token: 0x04008042 RID: 32834
			public NPCStateMachine Npc;

			// Token: 0x04008043 RID: 32835
			public HackMeStoreZone Store;

			// Token: 0x04008044 RID: 32836
			public HackMeObservationTarget Target;

			// Token: 0x04008045 RID: 32837
			public Vector3 NavigationPoint;
		}
	}
}
