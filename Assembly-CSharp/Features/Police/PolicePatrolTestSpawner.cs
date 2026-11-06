using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.RunModes;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000DFB RID: 3579
	public class PolicePatrolTestSpawner : MonoBehaviour
	{
		// Token: 0x06006A84 RID: 27268 RVA: 0x002AB3A5 File Offset: 0x002A95A5
		private void Awake()
		{
			if (this.bakedRegistry == null && this.autoLoadRegistry)
			{
				this.bakedRegistry = Resources.Load<NPCPoliceBakedRegistry>("NPCPoliceBakedRegistry");
			}
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x002AB3D0 File Offset: 0x002A95D0
		private void Start()
		{
			if (this.player == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.player = gameObject.transform;
				}
			}
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x002AB40B File Offset: 0x002A960B
		private void Update()
		{
			if (GameplayInput.GetKeyDown(this.spawnKey))
			{
				this.SpawnTestPatrol();
			}
			if (GameplayInput.GetKeyDown(this.despawnKey))
			{
				this.DespawnAllTestPatrols();
			}
		}

		// Token: 0x06006A87 RID: 27271 RVA: 0x002AB434 File Offset: 0x002A9634
		[ContextMenu("Spawn Test Patrol")]
		public void SpawnTestPatrol()
		{
			if (this.player == null)
			{
				Debug.LogError("[PolicePatrolTestSpawner] No player reference. Set 'player' in inspector or tag the player as 'Player'.");
				return;
			}
			if (this.patrolConfig == null || this.patrolConfig.members == null || this.patrolConfig.members.Length == 0)
			{
				Debug.LogError("[PolicePatrolTestSpawner] No patrolConfig set or empty members.");
				return;
			}
			if (this.bakedRegistry == null || this.bakedRegistry.Count == 0)
			{
				Debug.LogError("[PolicePatrolTestSpawner] No bakedRegistry available.");
				return;
			}
			Vector3 vector = this.player.forward;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			vector.Normalize();
			Vector3 vector2 = this.player.position + vector * this.spawnDistance;
			Quaternion quaternion = (this.facePlayer ? Quaternion.LookRotation(-vector) : this.player.rotation);
			List<NPCSenses> list = new List<NPCSenses>();
			NPCSenses npcsenses = null;
			for (int i = 0; i < this.patrolConfig.members.Length; i++)
			{
				TroupeMemberSlot troupeMemberSlot = this.patrolConfig.members[i];
				if (!(((troupeMemberSlot != null) ? troupeMemberSlot.npcAsset : null) == null))
				{
					GameObject gameObject;
					if (!this.bakedRegistry.TryGet(troupeMemberSlot.npcAsset, out gameObject) || gameObject == null)
					{
						Debug.LogWarning(string.Format("[PolicePatrolTestSpawner] Nessun baked prefab per '{0}', slot {1} saltato.", troupeMemberSlot.npcAsset.name, i));
					}
					else
					{
						Vector3 vector3 = quaternion * Vector3.right;
						int num = (i + 1) / 2 * ((i % 2 == 0) ? (-1) : 1);
						if (i == 0)
						{
							num = 0;
						}
						Vector3 vector4 = vector2 + vector3 * ((float)num * this.memberSpacing);
						RunModeService.DisqualifyLeaderboardRun("Police were spawned with a debug control.");
						GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, vector4, quaternion, base.transform);
						gameObject2.name = string.Format("[TEST]PolicePatrolMember_{0}_{1}", i, gameObject2.GetInstanceID());
						if (troupeMemberSlot.animatorOverride != null)
						{
							Animator componentInChildren = gameObject2.GetComponentInChildren<Animator>();
							if (componentInChildren != null)
							{
								componentInChildren.runtimeAnimatorController = troupeMemberSlot.animatorOverride;
							}
						}
						PoliceOfficerChaser component = gameObject2.GetComponent<PoliceOfficerChaser>();
						if (component != null)
						{
							component.enabled = false;
						}
						NPCSenses component2 = gameObject2.GetComponent<NPCSenses>();
						if (component2 == null)
						{
							Debug.LogWarning("[PolicePatrolTestSpawner] " + gameObject2.name + ": NPCSenses mancante, membro escluso.");
							Object.Destroy(gameObject2);
						}
						else
						{
							list.Add(component2);
							if (npcsenses == null && !string.IsNullOrEmpty(troupeMemberSlot.roleTag) && troupeMemberSlot.roleTag.IndexOf("taser", StringComparison.OrdinalIgnoreCase) >= 0)
							{
								npcsenses = component2;
							}
						}
					}
				}
			}
			if (list.Count == 0)
			{
				Debug.LogError("[PolicePatrolTestSpawner] Nessun membro spawnato.");
				return;
			}
			GameObject gameObject3 = new GameObject(string.Format("[TEST]PolicePatrol_{0}", Time.frameCount));
			gameObject3.transform.SetParent(base.transform);
			PolicePatrol policePatrol = gameObject3.AddComponent<PolicePatrol>();
			List<NPCSenses> list2 = new List<NPCSenses>();
			for (int j = 1; j < list.Count; j++)
			{
				list2.Add(list[j]);
			}
			policePatrol.captureProfile = this.captureProfile;
			policePatrol.Initialize(list[0], list2, npcsenses);
			this._testPatrols.Add(policePatrol);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PolicePatrolTestSpawner] Spawned test patrol di {0} membri a {1} ", list.Count, vector2) + "(Taser: " + ((npcsenses != null) ? npcsenses.name : "nessuno") + ")");
			}
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x002AB7E8 File Offset: 0x002A99E8
		[ContextMenu("Despawn All Test Patrols")]
		public void DespawnAllTestPatrols()
		{
			int num = 0;
			foreach (PolicePatrol policePatrol in this._testPatrols)
			{
				if (!(policePatrol == null))
				{
					RunModeService.DisqualifyLeaderboardRun("Police were removed with a debug control.");
					if (policePatrol.Initiator != null)
					{
						Object.Destroy(policePatrol.Initiator.gameObject);
					}
					foreach (NPCSenses npcsenses in policePatrol.AdditionalActors)
					{
						if (npcsenses != null)
						{
							Object.Destroy(npcsenses.gameObject);
						}
					}
					Object.Destroy(policePatrol.gameObject);
					num++;
				}
			}
			this._testPatrols.Clear();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PolicePatrolTestSpawner] Despawned {0} test patrols", num));
			}
		}

		// Token: 0x04006D2D RID: 27949
		[Header("Patrol Config")]
		[Tooltip("La config NPCGroupConfigSO da usare. Es. NPCGroupConfig_PolicePatrol3.")]
		public NPCGroupConfigSO patrolConfig;

		// Token: 0x04006D2E RID: 27950
		[Tooltip("Registry baked dei prefab Police (lo stesso usato dal PoliceManager).")]
		public NPCPoliceBakedRegistry bakedRegistry;

		// Token: 0x04006D2F RID: 27951
		[Tooltip("EncounterProfileSO usato per la cattura player. Tipicamente EncounterProfile_PoliceCapture.")]
		public EncounterProfileSO captureProfile;

		// Token: 0x04006D30 RID: 27952
		[Tooltip("Auto-load Resources/NPCPoliceBakedRegistry.asset se null.")]
		public bool autoLoadRegistry = true;

		// Token: 0x04006D31 RID: 27953
		[Header("Spawn")]
		[Tooltip("Tasto che triggera lo spawn. Default = P (di Police).")]
		public KeyCode spawnKey = KeyCode.P;

		// Token: 0x04006D32 RID: 27954
		[Tooltip("Tasto che dispawna tutte le patrol test attive. Default = O.")]
		public KeyCode despawnKey = KeyCode.O;

		// Token: 0x04006D33 RID: 27955
		[Tooltip("Distanza (m) davanti al player dove far apparire il leader della patrol.")]
		public float spawnDistance = 8f;

		// Token: 0x04006D34 RID: 27956
		[Tooltip("Spazio laterale (m) fra i membri della patrol.")]
		public float memberSpacing = 0.8f;

		// Token: 0x04006D35 RID: 27957
		[Tooltip("Se true, la patrol guarda verso il player al momento dello spawn (consigliato per test detection immediato).")]
		public bool facePlayer = true;

		// Token: 0x04006D36 RID: 27958
		[Header("Refs (auto-found if null)")]
		[Tooltip("Reference al player. Se null, FindGameObjectWithTag('Player').")]
		public Transform player;

		// Token: 0x04006D37 RID: 27959
		[Header("Debug")]
		public bool debugLogs = true;

		// Token: 0x04006D38 RID: 27960
		private readonly List<PolicePatrol> _testPatrols = new List<PolicePatrol>();
	}
}
