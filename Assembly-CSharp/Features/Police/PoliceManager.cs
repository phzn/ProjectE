using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.Suspicion;
using Pathfinding;
using Pathfinding.ECS.RVO;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Features.Police
{
	// Token: 0x02000DEF RID: 3567
	public class PoliceManager : MonoBehaviour
	{
		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x060069CA RID: 27082 RVA: 0x002A6465 File Offset: 0x002A4665
		// (set) Token: 0x060069CB RID: 27083 RVA: 0x002A646C File Offset: 0x002A466C
		public static PoliceManager Instance { get; private set; }

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x060069CC RID: 27084 RVA: 0x002A6474 File Offset: 0x002A4674
		public bool HasActivePatrolResponse
		{
			get
			{
				return this._responsePatrolTarget > 0 && this.GetLiveSuspicionLevel() >= 2;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x060069CD RID: 27085 RVA: 0x002A648D File Offset: 0x002A468D
		public int ResponsePatrolTarget
		{
			get
			{
				return this._responsePatrolTarget;
			}
		}

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x060069CE RID: 27086 RVA: 0x002A6495 File Offset: 0x002A4695
		public int ResponseMembersPerPatrol
		{
			get
			{
				return this._responseMembersPerPatrol;
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x060069CF RID: 27087 RVA: 0x002A649D File Offset: 0x002A469D
		public int ActiveResponsePatrolCount
		{
			get
			{
				return this.CountAliveResponsePatrols();
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x060069D0 RID: 27088 RVA: 0x002A64A5 File Offset: 0x002A46A5
		public int WantedSoloTarget
		{
			get
			{
				if (this._currentLevel != 1)
				{
					return 0;
				}
				return Mathf.Clamp(this.maxSoloAlive, 0, 1);
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x060069D1 RID: 27089 RVA: 0x002A64BF File Offset: 0x002A46BF
		public int WantedPatrolTarget
		{
			get
			{
				return PoliceManager.GetPatrolTargetForLevel(this._currentLevel);
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x060069D2 RID: 27090 RVA: 0x002A64CC File Offset: 0x002A46CC
		public int ActivePatrolCount
		{
			get
			{
				return this.CountAlivePatrols();
			}
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x060069D3 RID: 27091 RVA: 0x002A64D4 File Offset: 0x002A46D4
		public bool AnyOfficerHasLineOfSight
		{
			get
			{
				for (int i = 0; i < this._activeSoloOfficers.Count; i++)
				{
					GameObject gameObject = this._activeSoloOfficers[i];
					if (!(gameObject == null))
					{
						NPCSenses component = gameObject.GetComponent<NPCSenses>();
						if (component != null && component.HasLineOfSight)
						{
							return true;
						}
					}
				}
				for (int j = 0; j < this._activePatrols.Count; j++)
				{
					PolicePatrol policePatrol = this._activePatrols[j];
					if (!(policePatrol == null))
					{
						if (policePatrol.Initiator != null && policePatrol.Initiator.HasLineOfSight)
						{
							return true;
						}
						IReadOnlyList<NPCSenses> additionalActors = policePatrol.AdditionalActors;
						if (additionalActors != null)
						{
							for (int k = 0; k < additionalActors.Count; k++)
							{
								NPCSenses npcsenses = additionalActors[k];
								if (npcsenses != null && npcsenses.HasLineOfSight)
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x002A65BC File Offset: 0x002A47BC
		public bool TryRequestPatrolResponse(int patrolCount = 2, int membersPerPatrol = 3)
		{
			if (!base.isActiveAndEnabled || patrolCount <= 0 || membersPerPatrol <= 0)
			{
				return false;
			}
			if (this.GetLiveSuspicionLevel() < 2)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[PoliceManager] Patrol response rejected: suspicion must be level 2 or higher.");
				}
				return false;
			}
			if (!this.HasPatrolConfigForMemberCount(membersPerPatrol))
			{
				if (this.debugLogs)
				{
					Debug.LogWarning(string.Format("[PoliceManager] Patrol response rejected: no config with exactly {0} valid members.", membersPerPatrol));
				}
				return false;
			}
			if (this._responsePatrolTarget > 0 && this._responseMembersPerPatrol != membersPerPatrol)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[PoliceManager] Patrol response rejected: active response uses " + string.Format("{0} members per patrol, requested {1}.", this._responseMembersPerPatrol, membersPerPatrol));
				}
				return false;
			}
			this._responseMembersPerPatrol = membersPerPatrol;
			this._responsePatrolTarget = PoliceManager.GetPatrolTargetForLevel(this.GetLiveSuspicionLevel());
			this.AdoptCompatiblePatrolsForResponse();
			this._lastRebalance = Time.time - this.rebalanceInterval;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PoliceManager] Patrol response requested: {0}x", this._responsePatrolTarget) + string.Format("{0} (active={1}).", this._responseMembersPerPatrol, this.CountAliveResponsePatrols()));
			}
			return true;
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x002A66E0 File Offset: 0x002A48E0
		private void Awake()
		{
			if (PoliceManager.Instance != null && PoliceManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PoliceManager.Instance = this;
			if (this.bakedRegistry == null && this.autoLoadBakedRegistry)
			{
				this.bakedRegistry = Resources.Load<NPCPoliceBakedRegistry>("NPCPoliceBakedRegistry");
				if (this.bakedRegistry != null && this.debugLogs)
				{
					Debug.Log(string.Format("[PoliceManager] Loaded NPCPoliceBakedRegistry with {0} entries.", this.bakedRegistry.Count));
				}
			}
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x002A6774 File Offset: 0x002A4974
		private void Start()
		{
			if (this.player == null && PlayerProximityProvider.Instance != null)
			{
				this.player = PlayerProximityProvider.Instance.Player;
			}
			if (this.player == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				this.player = ((gameObject != null) ? gameObject.transform : null);
			}
			if (SuspicionManager.Instance != null)
			{
				SuspicionManager.Instance.OnLevelChanged.AddListener(new UnityAction<int>(this.OnSuspicionLevelChanged));
				this._currentLevel = SuspicionManager.Instance.CurrentLevel;
			}
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x002A680E File Offset: 0x002A4A0E
		private void OnEnable()
		{
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x002A6844 File Offset: 0x002A4A44
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			this.ClearPatrolResponseState();
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x002A6870 File Offset: 0x002A4A70
		private void OnDestroy()
		{
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			if (SuspicionManager.Instance != null)
			{
				SuspicionManager.Instance.OnLevelChanged.RemoveListener(new UnityAction<int>(this.OnSuspicionLevelChanged));
			}
			if (PoliceManager.Instance == this)
			{
				PoliceManager.Instance = null;
			}
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x002A68CC File Offset: 0x002A4ACC
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			this._lastRebalance += NPCSimulationPause.FrameTimeLoss;
			if (NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (SpecialNPCProximity.PopulationChangesBlocked)
			{
				return;
			}
			if (this._hasPendingLevelChange)
			{
				int pendingLevel = this._pendingLevel;
				this._hasPendingLevelChange = false;
				this.ApplySuspicionLevelChanged(pendingLevel);
			}
			this.player = this.GetLivePlayerTransform();
			if (this.player == null)
			{
				return;
			}
			if (Time.time - this._lastRebalance < this.rebalanceInterval)
			{
				return;
			}
			this._lastRebalance = Time.time;
			this.DespawnFar();
			this.Rebalance();
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x002A6978 File Offset: 0x002A4B78
		private void OnSuspicionLevelChanged(int level)
		{
			if (SpecialNPCProximity.PopulationChangesBlocked || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				this._pendingLevel = level;
				this._hasPendingLevelChange = true;
				return;
			}
			this.ApplySuspicionLevelChanged(level);
		}

		// Token: 0x060069DC RID: 27100 RVA: 0x002A69A4 File Offset: 0x002A4BA4
		private void ApplySuspicionLevelChanged(int level)
		{
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PoliceManager] Suspicion level → {0}", level));
			}
			int currentLevel = this._currentLevel;
			this._currentLevel = level;
			if (level < 2)
			{
				this.ClearPatrolResponseState();
			}
			else if (this._responsePatrolTarget > 0)
			{
				this._responsePatrolTarget = PoliceManager.GetPatrolTargetForLevel(level);
			}
			if (level == 0)
			{
				this.DespawnAllSolos();
				this.DespawnAllPatrols();
			}
			else if (level >= 2 && currentLevel < 2)
			{
				this.DespawnAllSolos();
			}
			else if (level == 1 && currentLevel >= 2)
			{
				this.DespawnAllPatrols();
			}
			else if (level >= 2)
			{
				this.TrimPatrolsToTarget(this.WantedPatrolTarget);
			}
			this._lastRebalance = Time.time - this.rebalanceInterval;
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x002A6A50 File Offset: 0x002A4C50
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x002A6A62 File Offset: 0x002A4C62
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x002A6A7F File Offset: 0x002A4C7F
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			this._lastRebalance += Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			this._simulationPauseActive = false;
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x002A6AB4 File Offset: 0x002A4CB4
		private void Rebalance()
		{
			if (this._currentLevel == 0)
			{
				return;
			}
			if (this._currentLevel == 1)
			{
				this.RebalanceLv1();
				return;
			}
			if (this._currentLevel >= 2)
			{
				this.RebalanceLv2();
			}
		}

		// Token: 0x060069E1 RID: 27105 RVA: 0x002A6AE0 File Offset: 0x002A4CE0
		private void RebalanceLv1()
		{
			if (this.bakedRegistry == null || this.bakedRegistry.Count == 0)
			{
				return;
			}
			int num = this.WantedSoloTarget - this.CountAliveSolos();
			if (num <= 0)
			{
				return;
			}
			int num2 = 0;
			while (num2 < num && this.TrySpawnSoloOfficer())
			{
				num2++;
			}
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x002A6B30 File Offset: 0x002A4D30
		private void RebalanceLv2()
		{
			if (this.patrolConfigs == null || this.patrolConfigs.Length == 0)
			{
				return;
			}
			int wantedPatrolTarget = this.WantedPatrolTarget;
			this.TrimPatrolsToTarget(wantedPatrolTarget);
			if (this.CountAlivePatrols() >= wantedPatrolTarget)
			{
				return;
			}
			if (this.HasActivePatrolResponse)
			{
				this._responsePatrolTarget = wantedPatrolTarget;
				this.RebalancePatrolResponse();
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			int num = this.CountAlivePatrolMembers();
			int num2 = Mathf.Max(0, this.maxPatrolMembersAlive) * wantedPatrolTarget - num;
			if (num2 < 2)
			{
				return;
			}
			NPCGroupConfigSO npcgroupConfigSO = this.SelectPatrolConfig(0, Mathf.Min(this.maxPatrolMembersAlive, num2));
			if (npcgroupConfigSO != null)
			{
				PolicePatrol policePatrol;
				this.TrySpawnNearbyPatrol(npcgroupConfigSO, out policePatrol);
			}
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x002A6BCC File Offset: 0x002A4DCC
		private void RebalancePatrolResponse()
		{
			this.AdoptCompatiblePatrolsForResponse();
			if (this.CountAliveResponsePatrols() >= this._responsePatrolTarget)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			NPCGroupConfigSO npcgroupConfigSO = this.SelectPatrolConfig(this._responseMembersPerPatrol, 0);
			if (npcgroupConfigSO == null)
			{
				return;
			}
			PolicePatrol policePatrol;
			if (this.TrySpawnNearbyPatrol(npcgroupConfigSO, out policePatrol))
			{
				this._activeResponsePatrols.Add(policePatrol);
			}
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x002A6C28 File Offset: 0x002A4E28
		private bool TrySpawnNearbyPatrol(NPCGroupConfigSO config, out PolicePatrol patrol)
		{
			patrol = null;
			int num = PoliceManager.CountConfiguredMembers(config);
			if (num <= 0 || !this.CanSpawnCompleteRoster(config))
			{
				return false;
			}
			NPCSpawnPoint npcspawnPoint = null;
			Transform transform = null;
			Vector3 vector;
			Quaternion quaternion;
			if (!this.TryGetAuthoredResponseSpawnPosition(num, out vector, out quaternion, out npcspawnPoint, out transform) && !this.TryGetSeparatedResponseFallback(num, out vector, out quaternion))
			{
				return false;
			}
			if (this.TrySpawnPatrolAt(config, true, vector, quaternion, out patrol))
			{
				if (npcspawnPoint != null)
				{
					this._usedResponseSpawnMarkers.Add(npcspawnPoint);
				}
				if (transform != null)
				{
					this._usedResponseSpawnGroups.Add(transform);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x002A6CAF File Offset: 0x002A4EAF
		private static int GetPatrolTargetForLevel(int level)
		{
			if (level >= 3)
			{
				return 2;
			}
			if (level != 2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x002A6CBE File Offset: 0x002A4EBE
		private int CountAlivePatrols()
		{
			this._activePatrols.RemoveAll((PolicePatrol patrol) => patrol == null);
			return this._activePatrols.Count;
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x002A6CF8 File Offset: 0x002A4EF8
		private void TrimPatrolsToTarget(int target)
		{
			this.CountAlivePatrols();
			for (int i = this._activePatrols.Count - 1; i >= Mathf.Max(0, target); i--)
			{
				PolicePatrol policePatrol = this._activePatrols[i];
				this._activePatrols.RemoveAt(i);
				this.DespawnPatrol(policePatrol);
			}
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x002A6D4C File Offset: 0x002A4F4C
		private int CountAliveSolos()
		{
			int num = 0;
			for (int i = this._activeSoloOfficers.Count - 1; i >= 0; i--)
			{
				if (this._activeSoloOfficers[i] == null)
				{
					this._activeSoloOfficers.RemoveAt(i);
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x002A6D9C File Offset: 0x002A4F9C
		private int CountAlivePatrolMembers()
		{
			int num = 0;
			for (int i = this._activePatrols.Count - 1; i >= 0; i--)
			{
				PolicePatrol policePatrol = this._activePatrols[i];
				if (policePatrol == null)
				{
					this._activePatrols.RemoveAt(i);
				}
				else
				{
					if (policePatrol.Initiator != null)
					{
						num++;
					}
					if (policePatrol.AdditionalActors != null)
					{
						using (IEnumerator<NPCSenses> enumerator = policePatrol.AdditionalActors.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current != null)
								{
									num++;
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x002A6E48 File Offset: 0x002A5048
		private static int CountAliveMembers(PolicePatrol patrol)
		{
			if (patrol == null)
			{
				return 0;
			}
			int num = ((patrol.Initiator != null) ? 1 : 0);
			if (patrol.AdditionalActors != null)
			{
				for (int i = 0; i < patrol.AdditionalActors.Count; i++)
				{
					if (patrol.AdditionalActors[i] != null)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x002A6EAC File Offset: 0x002A50AC
		private int CountAliveResponsePatrols()
		{
			for (int i = this._activeResponsePatrols.Count - 1; i >= 0; i--)
			{
				PolicePatrol policePatrol = this._activeResponsePatrols[i];
				if (policePatrol == null || !this._activePatrols.Contains(policePatrol))
				{
					this._activeResponsePatrols.RemoveAt(i);
				}
			}
			return this._activeResponsePatrols.Count;
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x002A6F0C File Offset: 0x002A510C
		private void AdoptCompatiblePatrolsForResponse()
		{
			if (this._responsePatrolTarget <= 0 || this._responseMembersPerPatrol <= 0)
			{
				return;
			}
			this.CountAliveResponsePatrols();
			int num = 0;
			while (num < this._activePatrols.Count && this._activeResponsePatrols.Count < this._responsePatrolTarget)
			{
				PolicePatrol policePatrol = this._activePatrols[num];
				if (!(policePatrol == null) && !this._activeResponsePatrols.Contains(policePatrol) && PoliceManager.CountAliveMembers(policePatrol) == this._responseMembersPerPatrol)
				{
					this._activeResponsePatrols.Add(policePatrol);
				}
				num++;
			}
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x002A6F9C File Offset: 0x002A519C
		private bool TrySpawnSoloOfficer()
		{
			Vector3 vector;
			Quaternion quaternion;
			if (!this.TryGetSpawnPosition(out vector, out quaternion))
			{
				return false;
			}
			GameObject randomBakedPrefab = this.bakedRegistry.GetRandomBakedPrefab();
			if (randomBakedPrefab == null)
			{
				return false;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(randomBakedPrefab, vector, quaternion, (this.spawnParent != null) ? this.spawnParent : base.transform);
			gameObject.name = string.Format("PoliceSolo_{0}", gameObject.GetInstanceID());
			if (gameObject.GetComponent<NPCSpawnFade>() == null)
			{
				gameObject.AddComponent<NPCSpawnFade>();
			}
			this.ApplyPolicePriority(gameObject);
			TaserOfficer component = gameObject.GetComponent<TaserOfficer>();
			if (component != null)
			{
				component.enabled = false;
				component.SetTaserVisible(false);
			}
			this._activeSoloOfficers.Add(gameObject);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PoliceManager] Spawned solo officer at {0}", vector));
			}
			return true;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x002A7078 File Offset: 0x002A5278
		private bool TrySpawnPatrolAt(NPCGroupConfigSO config, bool requireFullRoster, Vector3 center, Quaternion rot, out PolicePatrol spawnedPatrol)
		{
			spawnedPatrol = null;
			if (this.CountAlivePatrols() >= this.WantedPatrolTarget)
			{
				return false;
			}
			if (config == null || config.members == null || config.members.Length == 0)
			{
				return false;
			}
			if (this.bakedRegistry == null || this.bakedRegistry.Count == 0)
			{
				return false;
			}
			int num = PoliceManager.CountConfiguredMembers(config);
			if (num <= 0)
			{
				return false;
			}
			if (requireFullRoster && !this.CanSpawnCompleteRoster(config))
			{
				return false;
			}
			List<NPCSenses> list = new List<NPCSenses>();
			List<GameObject> list2 = new List<GameObject>();
			GameObject gameObject = null;
			NPCSenses npcsenses = null;
			for (int i = 0; i < config.members.Length; i++)
			{
				TroupeMemberSlot troupeMemberSlot = config.members[i];
				if (!(((troupeMemberSlot != null) ? troupeMemberSlot.npcAsset : null) == null))
				{
					GameObject gameObject2;
					if (!this.bakedRegistry.TryGet(troupeMemberSlot.npcAsset, out gameObject2) || gameObject2 == null)
					{
						if (this.debugLogs)
						{
							Debug.LogWarning(string.Format("[PoliceManager] No baked prefab for {0}, skipping slot {1}", troupeMemberSlot.npcAsset.name, i));
						}
					}
					else
					{
						Vector3 vector = rot * Vector3.right;
						int num2 = (i + 1) / 2 * ((i % 2 == 0) ? (-1) : 1);
						if (i == 0)
						{
							num2 = 0;
						}
						Vector3 vector2 = center + vector * ((float)num2 * 0.8f);
						GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2, vector2, rot, (this.spawnParent != null) ? this.spawnParent : base.transform);
						list2.Add(gameObject3);
						gameObject3.name = string.Format("PolicePatrolMember_{0}_{1}", i, gameObject3.GetInstanceID());
						if (gameObject3.GetComponent<NPCSpawnFade>() == null)
						{
							gameObject3.AddComponent<NPCSpawnFade>();
						}
						this.ApplyPolicePriority(gameObject3);
						if (troupeMemberSlot.prop != null)
						{
							NPCPropHolder component = gameObject3.GetComponent<NPCPropHolder>();
							if (component != null)
							{
								component.Equip(troupeMemberSlot.prop);
							}
						}
						if (troupeMemberSlot.animatorOverride != null)
						{
							Animator componentInChildren = gameObject3.GetComponentInChildren<Animator>();
							if (componentInChildren != null)
							{
								componentInChildren.runtimeAnimatorController = troupeMemberSlot.animatorOverride;
							}
						}
						PoliceOfficerChaser component2 = gameObject3.GetComponent<PoliceOfficerChaser>();
						if (component2 != null)
						{
							component2.enabled = false;
						}
						NPCSenses component3 = gameObject3.GetComponent<NPCSenses>();
						if (component3 == null)
						{
							if (this.debugLogs)
							{
								Debug.LogWarning("[PoliceManager] " + gameObject3.name + ": NPCSenses missing, member excluded.");
							}
							Object.Destroy(gameObject3);
						}
						else
						{
							if (i == 0)
							{
								gameObject = gameObject3;
							}
							list.Add(component3);
							if (npcsenses == null && !string.IsNullOrEmpty(troupeMemberSlot.roleTag) && troupeMemberSlot.roleTag.IndexOf("taser", StringComparison.OrdinalIgnoreCase) >= 0)
							{
								npcsenses = component3;
							}
						}
					}
				}
			}
			if (gameObject == null || list.Count == 0 || (requireFullRoster && list.Count != num))
			{
				for (int j = 0; j < list2.Count; j++)
				{
					if (list2[j] != null)
					{
						Object.Destroy(list2[j]);
					}
				}
				return false;
			}
			GameObject gameObject4 = new GameObject(string.Format("PolicePatrol_{0}", Time.frameCount));
			gameObject4.transform.SetParent((this.spawnParent != null) ? this.spawnParent : base.transform);
			PolicePatrol policePatrol = gameObject4.AddComponent<PolicePatrol>();
			List<NPCSenses> list3 = new List<NPCSenses>();
			for (int k = 1; k < list.Count; k++)
			{
				list3.Add(list[k]);
			}
			policePatrol.captureProfile = this.patrolCaptureProfile;
			policePatrol.Initialize(list[0], list3, npcsenses);
			PolicePatrol policePatrol2 = policePatrol;
			policePatrol2.OnPatrolDisbanding = (Action<PolicePatrol>)Delegate.Combine(policePatrol2.OnPatrolDisbanding, new Action<PolicePatrol>(this.HandlePatrolDisbanding));
			this._activePatrols.Add(policePatrol);
			spawnedPatrol = policePatrol;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PoliceManager] Spawned patrol with {0} members at {1}", list.Count, center));
			}
			return true;
		}

		// Token: 0x060069EF RID: 27119 RVA: 0x002A7480 File Offset: 0x002A5680
		private NPCGroupConfigSO SelectPatrolConfig(int requiredMemberCount = 0, int maximumMemberCount = 0)
		{
			NPCGroupConfigSO npcgroupConfigSO = null;
			NPCGroupConfigSO npcgroupConfigSO2 = null;
			float num = 0f;
			if (this.patrolConfigs == null)
			{
				return null;
			}
			foreach (NPCGroupConfigSO npcgroupConfigSO3 in this.patrolConfigs)
			{
				int num2 = PoliceManager.CountConfiguredMembers(npcgroupConfigSO3);
				if (!(npcgroupConfigSO3 == null) && num2 > 0 && (requiredMemberCount <= 0 || num2 == requiredMemberCount) && (maximumMemberCount <= 0 || (num2 >= 2 && num2 <= maximumMemberCount)))
				{
					if (npcgroupConfigSO == null)
					{
						npcgroupConfigSO = npcgroupConfigSO3;
					}
					npcgroupConfigSO2 = npcgroupConfigSO3;
					num += npcgroupConfigSO3.spawnWeight;
				}
			}
			if (npcgroupConfigSO == null)
			{
				return null;
			}
			if (num <= 0f)
			{
				return npcgroupConfigSO;
			}
			float num3 = Random.Range(0f, num);
			float num4 = 0f;
			foreach (NPCGroupConfigSO npcgroupConfigSO4 in this.patrolConfigs)
			{
				int num5 = PoliceManager.CountConfiguredMembers(npcgroupConfigSO4);
				if (!(npcgroupConfigSO4 == null) && num5 > 0 && (requiredMemberCount <= 0 || num5 == requiredMemberCount) && (maximumMemberCount <= 0 || (num5 >= 2 && num5 <= maximumMemberCount)))
				{
					num4 += npcgroupConfigSO4.spawnWeight;
					if (num3 <= num4)
					{
						return npcgroupConfigSO4;
					}
				}
			}
			return npcgroupConfigSO2;
		}

		// Token: 0x060069F0 RID: 27120 RVA: 0x002A759C File Offset: 0x002A579C
		private bool HasPatrolConfigForMemberCount(int memberCount)
		{
			if (memberCount <= 0 || this.patrolConfigs == null)
			{
				return false;
			}
			for (int i = 0; i < this.patrolConfigs.Length; i++)
			{
				if (PoliceManager.CountConfiguredMembers(this.patrolConfigs[i]) == memberCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069F1 RID: 27121 RVA: 0x002A75E0 File Offset: 0x002A57E0
		private static int CountConfiguredMembers(NPCGroupConfigSO config)
		{
			if (config == null || config.members == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < config.members.Length; i++)
			{
				TroupeMemberSlot troupeMemberSlot = config.members[i];
				if (((troupeMemberSlot != null) ? troupeMemberSlot.npcAsset : null) != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060069F2 RID: 27122 RVA: 0x002A7638 File Offset: 0x002A5838
		private bool CanSpawnCompleteRoster(NPCGroupConfigSO config)
		{
			if (!(config == null) && config.members != null && config.members.Length != 0)
			{
				TroupeMemberSlot troupeMemberSlot = config.members[0];
				if (!(((troupeMemberSlot != null) ? troupeMemberSlot.npcAsset : null) == null) && !(this.bakedRegistry == null))
				{
					for (int i = 0; i < config.members.Length; i++)
					{
						TroupeMemberSlot troupeMemberSlot2 = config.members[i];
						GameObject gameObject;
						if (!(((troupeMemberSlot2 != null) ? troupeMemberSlot2.npcAsset : null) == null) && (!this.bakedRegistry.TryGet(troupeMemberSlot2.npcAsset, out gameObject) || gameObject == null || gameObject.GetComponent<NPCSenses>() == null))
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069F3 RID: 27123 RVA: 0x002A76EC File Offset: 0x002A58EC
		private bool TryGetAuthoredResponseSpawnPosition(int memberCount, out Vector3 position, out Quaternion rotation, out NPCSpawnPoint selectedMarker, out Transform selectedGroup)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			selectedMarker = null;
			selectedGroup = null;
			Transform livePlayerTransform = this.GetLivePlayerTransform();
			SceneContext sceneContext = SceneContext.Current;
			Transform transform = ((sceneContext != null) ? sceneContext.NpcSpawnContainer : null);
			if (livePlayerTransform == null || transform == null || AstarPath.active == null)
			{
				return false;
			}
			NNInfo nearest = AstarPath.active.GetNearest(livePlayerTransform.position, NearestNodeConstraint.Walkable);
			if (nearest.node == null || !nearest.node.Walkable)
			{
				return false;
			}
			uint area = nearest.node.Area;
			NPCSpawnPoint[] componentsInChildren = transform.GetComponentsInChildren<NPCSpawnPoint>(true);
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return false;
			}
			List<Vector3> list = new List<Vector3>(this._activePatrols.Count);
			this.CollectActiveResponsePositions(list);
			Vector3 position2 = livePlayerTransform.position;
			float num = float.NegativeInfinity;
			foreach (NPCSpawnPoint npcspawnPoint in componentsInChildren)
			{
				Vector3 vector;
				if (!(npcspawnPoint == null) && npcspawnPoint.gameObject.activeInHierarchy && !this._usedResponseSpawnMarkers.Contains(npcspawnPoint) && PoliceManager.IsResponseMarkerGeometryEligible(npcspawnPoint.transform.position, position2, npcspawnPoint.maxVerticalDistanceFromPlayer, this.minSpawnDistance, this.spawnRadius, list, 12f) && PoliceManager.TryResolveResponseMarker(npcspawnPoint, area, out vector) && PoliceManager.IsResponseMarkerOccludedWhenRequired(npcspawnPoint, vector, livePlayerTransform) && SpecialNPCProximity.IsHiddenFromCamera(vector, livePlayerTransform, null))
				{
					Quaternion quaternion = PoliceManager.ResolveResponseMarkerRotation(npcspawnPoint.transform, vector, position2);
					if (PoliceManager.IsResponseSquadFootprintClear(vector, quaternion, memberCount, area))
					{
						Transform responseMarkerGroup = PoliceManager.GetResponseMarkerGroup(npcspawnPoint.transform, transform);
						float num2 = Random.value;
						if (responseMarkerGroup != null && !this._usedResponseSpawnGroups.Contains(responseMarkerGroup))
						{
							num2 += 1000f;
						}
						if (list.Count > 0)
						{
							num2 += PoliceManager.GetMinimumHorizontalDistance(vector, list);
						}
						float num3 = (this.minSpawnDistance + this.spawnRadius) * 0.5f;
						num2 -= Mathf.Abs(Vector3.Distance(vector, position2) - num3) * 0.05f;
						if (num2 > num)
						{
							num = num2;
							position = vector;
							rotation = quaternion;
							selectedMarker = npcspawnPoint;
							selectedGroup = responseMarkerGroup;
						}
					}
				}
			}
			return selectedMarker != null;
		}

		// Token: 0x060069F4 RID: 27124 RVA: 0x002A7950 File Offset: 0x002A5B50
		private bool TryGetSeparatedResponseFallback(int memberCount, out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			List<Vector3> list = new List<Vector3>(this._activePatrols.Count);
			this.CollectActiveResponsePositions(list);
			int num = Mathf.Max(1, this.spawnPointAttempts);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector;
				Quaternion quaternion;
				if (this.TryGetSpawnPosition(out vector, out quaternion) && PoliceManager.IsSeparatedFromResponsePatrols(vector, list, 12f))
				{
					NNInfo nearest = AstarPath.active.GetNearest(vector, NearestNodeConstraint.Walkable);
					if (nearest.node != null && PoliceManager.IsResponseSquadFootprintClear(vector, quaternion, memberCount, nearest.node.Area))
					{
						position = vector;
						rotation = quaternion;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060069F5 RID: 27125 RVA: 0x002A7A04 File Offset: 0x002A5C04
		private void CollectActiveResponsePositions(List<Vector3> positions)
		{
			if (positions == null)
			{
				return;
			}
			for (int i = 0; i < this._activePatrols.Count; i++)
			{
				PolicePatrol policePatrol = this._activePatrols[i];
				if (policePatrol != null && policePatrol.Initiator != null)
				{
					positions.Add(policePatrol.Initiator.transform.position);
				}
			}
		}

		// Token: 0x060069F6 RID: 27126 RVA: 0x002A7A68 File Offset: 0x002A5C68
		private static bool IsResponseMarkerGeometryEligible(Vector3 markerPosition, Vector3 playerPosition, float maxVerticalDistance, float minimumDistance, float maximumDistance, IReadOnlyList<Vector3> activePatrolPositions, float minimumPatrolSeparation)
		{
			Vector3 vector = markerPosition - playerPosition;
			if (maxVerticalDistance > 0f && Mathf.Abs(vector.y) > maxVerticalDistance)
			{
				return false;
			}
			float magnitude = vector.magnitude;
			return magnitude >= Mathf.Max(0f, minimumDistance) && magnitude <= Mathf.Max(minimumDistance, maximumDistance) && PoliceManager.IsSeparatedFromResponsePatrols(markerPosition, activePatrolPositions, minimumPatrolSeparation);
		}

		// Token: 0x060069F7 RID: 27127 RVA: 0x002A7AC4 File Offset: 0x002A5CC4
		private static bool IsSeparatedFromResponsePatrols(Vector3 candidate, IReadOnlyList<Vector3> activePatrolPositions, float minimumSeparation)
		{
			if (activePatrolPositions == null || minimumSeparation <= 0f)
			{
				return true;
			}
			float num = minimumSeparation * minimumSeparation;
			for (int i = 0; i < activePatrolPositions.Count; i++)
			{
				Vector3 vector = candidate - activePatrolPositions[i];
				vector.y = 0f;
				if (vector.sqrMagnitude < num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060069F8 RID: 27128 RVA: 0x002A7B1C File Offset: 0x002A5D1C
		private static float GetMinimumHorizontalDistance(Vector3 candidate, IReadOnlyList<Vector3> activePatrolPositions)
		{
			if (activePatrolPositions == null || activePatrolPositions.Count == 0)
			{
				return 0f;
			}
			float num = float.PositiveInfinity;
			for (int i = 0; i < activePatrolPositions.Count; i++)
			{
				Vector3 vector = candidate - activePatrolPositions[i];
				vector.y = 0f;
				num = Mathf.Min(num, vector.sqrMagnitude);
			}
			return Mathf.Sqrt(num);
		}

		// Token: 0x060069F9 RID: 27129 RVA: 0x002A7B80 File Offset: 0x002A5D80
		private static bool TryResolveResponseMarker(NPCSpawnPoint marker, uint requiredArea, out Vector3 resolvedPosition)
		{
			Vector3 vector = ((marker != null) ? marker.transform.position : Vector3.zero);
			resolvedPosition = vector;
			if (marker == null || AstarPath.active == null)
			{
				return false;
			}
			NNInfo nearest = AstarPath.active.GetNearest(vector, NearestNodeConstraint.Walkable);
			if (nearest.node == null || !nearest.node.Walkable)
			{
				return false;
			}
			if (marker.maxVerticalDistanceFromPlayer > 0f && nearest.node.Area != requiredArea)
			{
				return false;
			}
			Vector3 position = nearest.position;
			Vector2 vector2 = new Vector2(position.x - vector.x, position.z - vector.z);
			if (vector2.magnitude > 1.5f)
			{
				return false;
			}
			if (Mathf.Abs(position.y - vector.y) > 2f)
			{
				return false;
			}
			resolvedPosition = position;
			return true;
		}

		// Token: 0x060069FA RID: 27130 RVA: 0x002A7C68 File Offset: 0x002A5E68
		private static Quaternion ResolveResponseMarkerRotation(Transform marker, Vector3 resolvedPosition, Vector3 playerPosition)
		{
			Vector3 vector = ((marker != null) ? marker.forward : Vector3.zero);
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = playerPosition - resolvedPosition;
				vector.y = 0f;
			}
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x002A7CE0 File Offset: 0x002A5EE0
		private static Transform GetResponseMarkerGroup(Transform marker, Transform container)
		{
			if (marker == null || container == null)
			{
				return null;
			}
			Transform transform = marker;
			while (transform.parent != null && transform.parent != container)
			{
				transform = transform.parent;
			}
			if (!(transform.parent == container))
			{
				return null;
			}
			return transform;
		}

		// Token: 0x060069FC RID: 27132 RVA: 0x002A7D3C File Offset: 0x002A5F3C
		private static bool IsResponseSquadFootprintClear(Vector3 center, Quaternion rotation, int memberCount, uint requiredArea)
		{
			if (AstarPath.active == null || memberCount <= 0)
			{
				return false;
			}
			Vector3 vector = rotation * Vector3.right;
			for (int i = 0; i < memberCount; i++)
			{
				int num = (i + 1) / 2 * ((i % 2 == 0) ? (-1) : 1);
				if (i == 0)
				{
					num = 0;
				}
				Vector3 vector2 = center + vector * ((float)num * 0.8f);
				if (!SpecialNPCProximity.IsHiddenFromCamera(vector2, PlayerRegistry.Player, null))
				{
					return false;
				}
				NNInfo nearest = AstarPath.active.GetNearest(vector2, NearestNodeConstraint.Walkable);
				if (nearest.node == null || !nearest.node.Walkable || nearest.node.Area != requiredArea)
				{
					return false;
				}
				Vector2 vector3 = new Vector2(nearest.position.x - vector2.x, nearest.position.z - vector2.z);
				if (vector3.magnitude > 0.35f || Mathf.Abs(nearest.position.y - vector2.y) > 0.5f)
				{
					return false;
				}
				Vector3 vector4 = vector2 + Vector3.up * 0.45f;
				Vector3 vector5 = vector2 + Vector3.up * 1.55f;
				if (Physics.CheckCapsule(vector4, vector5, 0.35f, -1, QueryTriggerInteraction.Ignore))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060069FD RID: 27133 RVA: 0x002A7E8C File Offset: 0x002A608C
		private static bool IsResponseMarkerOccludedWhenRequired(NPCSpawnPoint marker, Vector3 resolvedPosition, Transform livePlayer)
		{
			if (marker == null || !marker.requireCameraOcclusion)
			{
				return true;
			}
			Camera main = Camera.main;
			return main == null || (PoliceManager.IsResponseCameraSampleHidden(main, resolvedPosition + Vector3.up * 0.2f, livePlayer) && PoliceManager.IsResponseCameraSampleHidden(main, resolvedPosition + Vector3.up * 1.05f, livePlayer) && PoliceManager.IsResponseCameraSampleHidden(main, resolvedPosition + Vector3.up * 1.75f, livePlayer));
		}

		// Token: 0x060069FE RID: 27134 RVA: 0x002A7F18 File Offset: 0x002A6118
		private static bool IsResponseCameraSampleHidden(Camera gameplayCamera, Vector3 target, Transform livePlayer)
		{
			Vector3 vector = gameplayCamera.WorldToViewportPoint(target);
			if (vector.z <= 0f || vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f)
			{
				return true;
			}
			Vector3 position = gameplayCamera.transform.position;
			Vector3 vector2 = target - position;
			float magnitude = vector2.magnitude;
			if (magnitude <= 0.25f)
			{
				return false;
			}
			vector2 /= magnitude;
			int num = Physics.RaycastNonAlloc(position, vector2, PoliceManager.ResponseOcclusionHits, magnitude - 0.2f, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Collider collider = PoliceManager.ResponseOcclusionHits[i].collider;
				if (!(collider == null) && !(collider.transform == gameplayCamera.transform) && !collider.transform.IsChildOf(gameplayCamera.transform) && (!(livePlayer != null) || (!(collider.transform == livePlayer) && !collider.transform.IsChildOf(livePlayer))) && !PoliceManager.IsTransparentResponseOccluder(collider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069FF RID: 27135 RVA: 0x002A804C File Offset: 0x002A624C
		private static bool IsTransparentResponseOccluder(Collider collider)
		{
			Transform transform = ((collider != null) ? collider.transform : null);
			int num = 0;
			while (transform != null && num < 4)
			{
				string name = transform.name;
				if (PoliceManager.ContainsIgnoreCase(name, "glass") || PoliceManager.ContainsIgnoreCase(name, "window") || PoliceManager.ContainsIgnoreCase(name, "vetr") || PoliceManager.ContainsIgnoreCase(name, "crystal"))
				{
					return true;
				}
				num++;
				transform = transform.parent;
			}
			Renderer renderer = ((collider != null) ? collider.GetComponent<Renderer>() : null);
			Material material = ((renderer != null) ? renderer.sharedMaterial : null);
			if (material == null)
			{
				return false;
			}
			string name2 = material.name;
			return PoliceManager.ContainsIgnoreCase(name2, "glass") || PoliceManager.ContainsIgnoreCase(name2, "window") || PoliceManager.ContainsIgnoreCase(name2, "vetr") || PoliceManager.ContainsIgnoreCase(name2, "crystal");
		}

		// Token: 0x06006A00 RID: 27136 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		private static bool ContainsIgnoreCase(string value, string token)
		{
			return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06006A01 RID: 27137 RVA: 0x002A813C File Offset: 0x002A633C
		private Transform GetLivePlayerTransform()
		{
			PlayerProximityProvider instance = PlayerProximityProvider.Instance;
			if (instance != null && instance.Player != null)
			{
				return instance.Player;
			}
			if (PlayerRegistry.Player != null)
			{
				return PlayerRegistry.Player;
			}
			return this.player;
		}

		// Token: 0x06006A02 RID: 27138 RVA: 0x002A8188 File Offset: 0x002A6388
		private bool TryGetSpawnPosition(out Vector3 pos, out Quaternion rot)
		{
			pos = Vector3.zero;
			rot = Quaternion.identity;
			if (this.player == null)
			{
				return false;
			}
			for (int i = 0; i < this.spawnPointAttempts; i++)
			{
				float num = Random.Range(0f, 6.2831855f);
				float num2 = Random.Range(this.minSpawnDistance, this.spawnRadius);
				Vector3 vector = this.player.position + new Vector3(Mathf.Cos(num) * num2, 0f, Mathf.Sin(num) * num2);
				if (SpecialNPCProximity.TryResolveSpawnPosition(vector, this.player.position, out vector))
				{
					float num3 = Vector3.Distance(vector, this.player.position);
					if (num3 >= this.minSpawnDistance && num3 <= this.spawnRadius && SpecialNPCProximity.IsHiddenFromCamera(vector, this.player, null) && !Physics.CheckCapsule(vector + Vector3.up * 0.45f, vector + Vector3.up * 1.55f, 0.35f, -1, QueryTriggerInteraction.Ignore))
					{
						Vector3 normalized = (vector - this.player.position).normalized;
						pos = vector;
						rot = Quaternion.LookRotation(-normalized);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006A03 RID: 27139 RVA: 0x002A82E0 File Offset: 0x002A64E0
		private void DespawnFar()
		{
			if (this.player == null || SpecialNPCProximity.PopulationChangesBlocked)
			{
				return;
			}
			for (int i = this._activeSoloOfficers.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this._activeSoloOfficers[i];
				if (gameObject == null)
				{
					this._activeSoloOfficers.RemoveAt(i);
				}
				else
				{
					Vector3 vector = this.player.position - gameObject.transform.position;
					vector.y = 0f;
					if (vector.sqrMagnitude > this.despawnRadius * this.despawnRadius && SpecialNPCProximity.IsHiddenFromCamera(gameObject.transform.position, this.player, gameObject.transform))
					{
						this.DespawnSolo(gameObject);
						this._activeSoloOfficers.RemoveAt(i);
					}
				}
			}
			for (int j = this._activePatrols.Count - 1; j >= 0; j--)
			{
				PolicePatrol policePatrol = this._activePatrols[j];
				if (policePatrol == null)
				{
					this._activePatrols.RemoveAt(j);
				}
				else if (!policePatrol.IsAwaitingPostStunCapture && SpecialNPCProximity.CanRecycleGroup(policePatrol.Initiator, policePatrol.AdditionalActors, this.player, this.despawnRadius))
				{
					this.DespawnPatrol(policePatrol);
					this._activePatrols.RemoveAt(j);
				}
			}
		}

		// Token: 0x06006A04 RID: 27140 RVA: 0x002A8430 File Offset: 0x002A6630
		private void DespawnAllSolos()
		{
			foreach (GameObject gameObject in this._activeSoloOfficers)
			{
				if (gameObject != null)
				{
					this.DespawnSolo(gameObject);
				}
			}
			this._activeSoloOfficers.Clear();
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x002A8498 File Offset: 0x002A6698
		private void DespawnAllPatrols()
		{
			foreach (PolicePatrol policePatrol in this._activePatrols)
			{
				if (policePatrol != null)
				{
					this.DespawnPatrol(policePatrol);
				}
			}
			this._activePatrols.Clear();
			this._activeResponsePatrols.Clear();
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x002A850C File Offset: 0x002A670C
		private void DespawnSolo(GameObject go)
		{
			NPCSenses component = go.GetComponent<NPCSenses>();
			if (component != null)
			{
				component.enabled = false;
			}
			PoliceOfficerChaser component2 = go.GetComponent<PoliceOfficerChaser>();
			if (component2 != null)
			{
				component2.StopAllCoroutines();
				component2.enabled = false;
			}
			TaserOfficer component3 = go.GetComponent<TaserOfficer>();
			if (component3 != null)
			{
				component3.StopAllCoroutines();
				component3.enabled = false;
			}
			NPCSpawnFade component4 = go.GetComponent<NPCSpawnFade>();
			if (component4 != null)
			{
				component4.StartFadeOut(delegate
				{
					if (go != null)
					{
						Object.Destroy(go);
					}
				});
				return;
			}
			Object.Destroy(go);
		}

		// Token: 0x06006A07 RID: 27143 RVA: 0x002A85BC File Offset: 0x002A67BC
		private void DespawnPatrol(PolicePatrol patrol)
		{
			if (patrol == null)
			{
				return;
			}
			this._activeResponsePatrols.Remove(patrol);
			patrol.OnPatrolDisbanding = (Action<PolicePatrol>)Delegate.Remove(patrol.OnPatrolDisbanding, new Action<PolicePatrol>(this.HandlePatrolDisbanding));
			patrol.StopAllCoroutines();
			patrol.enabled = false;
			if (patrol.Initiator != null)
			{
				this.DespawnSolo(patrol.Initiator.gameObject);
			}
			if (patrol.AdditionalActors != null)
			{
				foreach (NPCSenses npcsenses in patrol.AdditionalActors)
				{
					if (npcsenses != null)
					{
						this.DespawnSolo(npcsenses.gameObject);
					}
				}
			}
			Object.Destroy(patrol.gameObject, 2f);
		}

		// Token: 0x06006A08 RID: 27144 RVA: 0x002A8694 File Offset: 0x002A6894
		private void HandlePatrolDisbanding(PolicePatrol patrol)
		{
			this._activeResponsePatrols.Remove(patrol);
			this._activePatrols.Remove(patrol);
		}

		// Token: 0x06006A09 RID: 27145 RVA: 0x002A86B0 File Offset: 0x002A68B0
		private int GetLiveSuspicionLevel()
		{
			if (!(SuspicionManager.Instance != null))
			{
				return this._currentLevel;
			}
			return SuspicionManager.Instance.CurrentLevel;
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x002A86D0 File Offset: 0x002A68D0
		private void ClearPatrolResponseState()
		{
			this._responsePatrolTarget = 0;
			this._responseMembersPerPatrol = 0;
			this._activeResponsePatrols.Clear();
			this._usedResponseSpawnMarkers.Clear();
			this._usedResponseSpawnGroups.Clear();
		}

		// Token: 0x06006A0B RID: 27147 RVA: 0x002A8704 File Offset: 0x002A6904
		private void HandleActiveSceneChanged(Scene previousScene, Scene nextScene)
		{
			if (this._responsePatrolTarget <= 0 && this._activeResponsePatrols.Count == 0)
			{
				return;
			}
			List<PolicePatrol> list = new List<PolicePatrol>(this._activeResponsePatrols);
			this.ClearPatrolResponseState();
			for (int i = 0; i < list.Count; i++)
			{
				PolicePatrol policePatrol = list[i];
				if (!(policePatrol == null))
				{
					this._activePatrols.Remove(policePatrol);
					this.DespawnPatrol(policePatrol);
				}
			}
		}

		// Token: 0x06006A0C RID: 27148 RVA: 0x002A8770 File Offset: 0x002A6970
		private void ApplyPolicePriority(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			FollowerEntity component = go.GetComponent<FollowerEntity>();
			if (component == null)
			{
				return;
			}
			float num = 0.75f + (float)(this._policePriorityCounter % 11) * 0.01f;
			RVOAgent rvoSettings = component.rvoSettings;
			rvoSettings.priority = num;
			component.rvoSettings = rvoSettings;
			this._policePriorityCounter++;
		}

		// Token: 0x04006CB1 RID: 27825
		[Header("Baked Resources")]
		[Tooltip("Registry mapping police CharacterObjects → baked prefabs. Used for spawning solo officers (Lv1).")]
		public NPCPoliceBakedRegistry bakedRegistry;

		// Token: 0x04006CB2 RID: 27826
		[Tooltip("Auto-load Resources/NPCPoliceBakedRegistry.asset in Awake if registry is null.")]
		public bool autoLoadBakedRegistry = true;

		// Token: 0x04006CB3 RID: 27827
		[Header("Patrol Configs (Lv2+)")]
		[Tooltip("Group configs used to spawn patrols at Suspicion Lv2 and Lv3.\nSelection is weighted via NPCGroupConfigSO.spawnWeight.\nEach config's members[0] is the leader; the Taser member is auto-detected via TaserOfficer component on the spawned GO.")]
		public NPCGroupConfigSO[] patrolConfigs;

		// Token: 0x04006CB4 RID: 27828
		[Header("Capture (Director encounter)")]
		[Tooltip("EncounterProfileSO usato dalle PolicePatrol che spawniamo a runtime per la cattura player. Tipicamente EncounterProfile_PoliceCapture (skip choice → emote Police).")]
		public EncounterProfileSO patrolCaptureProfile;

		// Token: 0x04006CB5 RID: 27829
		[Header("Population Cap (separati per tipo)")]
		[Tooltip("Solo officer budget at Suspicion Lv1. The wanted response uses at most one officer; 0 disables solo spawning. At Lv2+ solo officers are replaced by patrols.")]
		[Min(0f)]
		public int maxSoloAlive = 1;

		// Token: 0x04006CB6 RID: 27830
		[Tooltip("Total ambient patrol member budget at Suspicion Lv2 (one patrol). The budget doubles at Lv3+ (two patrols). Complete rosters must fit; explicit responses may override the member budget, but never the wanted tier's patrol count.")]
		[Min(0f)]
		public int maxPatrolMembersAlive = 3;

		// Token: 0x04006CB7 RID: 27831
		[Header("Spawn Radii")]
		[Tooltip("Officers/patrols are spawned within this radius from the player (and only at points the player can't see).")]
		public float spawnRadius = 40f;

		// Token: 0x04006CB8 RID: 27832
		[Tooltip("Minimum distance from player at which spawning is allowed (avoid materializing in front of the player).")]
		public float minSpawnDistance = 18f;

		// Token: 0x04006CB9 RID: 27833
		[Tooltip("Officers/patrols beyond this distance are despawned (faded out).")]
		public float despawnRadius = 80f;

		// Token: 0x04006CBA RID: 27834
		[Header("Spawn Cadence")]
		[Tooltip("How often (s) the manager re-evaluates population vs target.")]
		public float rebalanceInterval = 1.5f;

		// Token: 0x04006CBB RID: 27835
		[Tooltip("Try this many spawn point candidates before giving up for a tick. Each candidate is a random ring point.")]
		public int spawnPointAttempts = 8;

		// Token: 0x04006CBC RID: 27836
		[Header("Wiring")]
		[Tooltip("Reference to the player Transform. Auto-found from PlayerProximityProvider if null.")]
		public Transform player;

		// Token: 0x04006CBD RID: 27837
		[Tooltip("Parent transform for spawned officers/patrols (organizational only). If null, uses this transform.")]
		public Transform spawnParent;

		// Token: 0x04006CBE RID: 27838
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x04006CBF RID: 27839
		private readonly List<GameObject> _activeSoloOfficers = new List<GameObject>();

		// Token: 0x04006CC0 RID: 27840
		private readonly List<PolicePatrol> _activePatrols = new List<PolicePatrol>();

		// Token: 0x04006CC1 RID: 27841
		private readonly List<PolicePatrol> _activeResponsePatrols = new List<PolicePatrol>();

		// Token: 0x04006CC2 RID: 27842
		private readonly HashSet<NPCSpawnPoint> _usedResponseSpawnMarkers = new HashSet<NPCSpawnPoint>();

		// Token: 0x04006CC3 RID: 27843
		private readonly HashSet<Transform> _usedResponseSpawnGroups = new HashSet<Transform>();

		// Token: 0x04006CC4 RID: 27844
		private float _lastRebalance;

		// Token: 0x04006CC5 RID: 27845
		private int _currentLevel;

		// Token: 0x04006CC6 RID: 27846
		private bool _simulationPauseActive;

		// Token: 0x04006CC7 RID: 27847
		private float _simulationPauseStartedAt;

		// Token: 0x04006CC8 RID: 27848
		private bool _hasPendingLevelChange;

		// Token: 0x04006CC9 RID: 27849
		private int _pendingLevel;

		// Token: 0x04006CCA RID: 27850
		private int _responsePatrolTarget;

		// Token: 0x04006CCB RID: 27851
		private int _responseMembersPerPatrol;

		// Token: 0x04006CCC RID: 27852
		private const float ResponsePatrolMinSeparation = 12f;

		// Token: 0x04006CCD RID: 27853
		private const float ResponseNavmeshMaxHorizontalCorrection = 1.5f;

		// Token: 0x04006CCE RID: 27854
		private const float ResponseNavmeshMaxVerticalCorrection = 2f;

		// Token: 0x04006CCF RID: 27855
		private const float ResponseMemberSpacing = 0.8f;

		// Token: 0x04006CD0 RID: 27856
		private const float ResponseOccupancyRadius = 0.35f;

		// Token: 0x04006CD1 RID: 27857
		private static readonly RaycastHit[] ResponseOcclusionHits = new RaycastHit[24];

		// Token: 0x04006CD2 RID: 27858
		private int _policePriorityCounter;
	}
}
