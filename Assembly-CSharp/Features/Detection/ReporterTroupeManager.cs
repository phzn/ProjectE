using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Features.NPC;
using Pathfinding;
using Pathfinding.ECS.RVO;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012F0 RID: 4848
	public class ReporterTroupeManager : MonoBehaviour
	{
		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x06009686 RID: 38534 RVA: 0x003DCADE File Offset: 0x003DACDE
		// (set) Token: 0x06009687 RID: 38535 RVA: 0x003DCAE5 File Offset: 0x003DACE5
		public static ReporterTroupeManager Instance { get; private set; }

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x06009688 RID: 38536 RVA: 0x003DCAED File Offset: 0x003DACED
		public IReadOnlyList<ReporterTroupe> ActiveTroupes
		{
			get
			{
				this._activeTroupes.RemoveAll((ReporterTroupe troupe) => troupe == null);
				return this._activeTroupes;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x06009689 RID: 38537 RVA: 0x003DCB20 File Offset: 0x003DAD20
		public int ActiveTroupeCount
		{
			get
			{
				this._activeTroupes.RemoveAll((ReporterTroupe troupe) => troupe == null);
				return this._activeTroupes.Count;
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x0600968A RID: 38538 RVA: 0x003DCB58 File Offset: 0x003DAD58
		public bool CanSpawn
		{
			get
			{
				return this.ActiveTroupeCount + this._pendingSpawns < this.maxActiveTroupes;
			}
		}

		// Token: 0x0600968B RID: 38539 RVA: 0x003DCB70 File Offset: 0x003DAD70
		private void Awake()
		{
			if (ReporterTroupeManager.Instance != null && ReporterTroupeManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			ReporterTroupeManager.Instance = this;
			if (this.bakedRegistry == null && this.autoLoadBakedRegistry)
			{
				this.bakedRegistry = Resources.Load<NPCReporterBakedRegistry>("NPCReporterBakedRegistry");
				if (this.bakedRegistry != null)
				{
					Debug.Log(string.Format("[ReporterTroupeManager] Loaded NPCReporterBakedRegistry with {0} entries (Resources autoload).", this.bakedRegistry.Count));
				}
			}
		}

		// Token: 0x0600968C RID: 38540 RVA: 0x003DCBFC File Offset: 0x003DADFC
		private void OnDestroy()
		{
			if (ReporterTroupeManager.Instance == this)
			{
				ReporterTroupeManager.Instance = null;
			}
		}

		// Token: 0x0600968D RID: 38541 RVA: 0x003DCC14 File Offset: 0x003DAE14
		public async Task<ReporterTroupe> SpawnTroupe(Vector3 center, Quaternion rotation)
		{
			ReporterTroupe reporterTroupe;
			if (!this.CanSpawn)
			{
				Debug.Log("[ReporterTroupeManager] Limite maxActiveTroupes raggiunto, spawn ignorato.");
				reporterTroupe = null;
			}
			else
			{
				if (this.spawner == null)
				{
					this.spawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
				}
				if (this.spawner == null && this.bakedRegistry == null)
				{
					Debug.LogError("[ReporterTroupeManager] NPCRuntimeSpawner non assegnato!");
					reporterTroupe = null;
				}
				else
				{
					NPCGroupConfigSO config = this.SelectConfig();
					if (config == null || config.members == null || config.members.Length == 0)
					{
						Debug.LogWarning("[ReporterTroupeManager] Nessuna configurazione valida disponibile.");
						reporterTroupe = null;
					}
					else
					{
						this._pendingSpawns++;
						this._pendingConfigs.Add(config);
						try
						{
							List<GameObject> list = await this.SpawnMembers(config, center, rotation);
							if (this == null || !base.isActiveAndEnabled)
							{
								foreach (GameObject gameObject in list)
								{
									if (gameObject != null)
									{
										Object.Destroy(gameObject);
									}
								}
								reporterTroupe = null;
							}
							else if (list.Count == 0)
							{
								Debug.LogError("[ReporterTroupeManager] Nessun membro spawnato, troupe non creata.");
								reporterTroupe = null;
							}
							else
							{
								ReporterTroupe reporterTroupe2 = this.BuildTroupe(list, config);
								if (reporterTroupe2 == null)
								{
									foreach (GameObject gameObject2 in list)
									{
										if (gameObject2 != null)
										{
											Object.Destroy(gameObject2);
										}
									}
									reporterTroupe = null;
								}
								else
								{
									this._activeTroupes.Add(reporterTroupe2);
									ReporterTroupe reporterTroupe3 = reporterTroupe2;
									reporterTroupe3.OnTroupeDisbanding = (Action<ReporterTroupe>)Delegate.Combine(reporterTroupe3.OnTroupeDisbanding, new Action<ReporterTroupe>(this.HandleTroupeDisbanding));
									Debug.Log(string.Format("[ReporterTroupeManager] Troupe '{0}' creata con {1} membri.", reporterTroupe2.name, list.Count));
									reporterTroupe = reporterTroupe2;
								}
							}
						}
						finally
						{
							this._pendingSpawns--;
							this._pendingConfigs.Remove(config);
						}
					}
				}
			}
			return reporterTroupe;
		}

		// Token: 0x0600968E RID: 38542 RVA: 0x003DCC68 File Offset: 0x003DAE68
		public void DespawnTroupe(ReporterTroupe troupe, bool underScreenFade = false)
		{
			if (troupe == null)
			{
				return;
			}
			troupe.OnTroupeDisbanding = (Action<ReporterTroupe>)Delegate.Remove(troupe.OnTroupeDisbanding, new Action<ReporterTroupe>(this.HandleTroupeDisbanding));
			this._activeTroupes.Remove(troupe);
			troupe.StopAllCoroutines();
			troupe.enabled = false;
			this.DespawnMemberNPC(troupe.Initiator, underScreenFade);
			if (troupe.AdditionalActors != null)
			{
				foreach (NPCSenses npcsenses in troupe.AdditionalActors)
				{
					this.DespawnMemberNPC(npcsenses, underScreenFade);
				}
			}
			Object.Destroy(troupe.gameObject);
			Debug.Log("[ReporterTroupeManager] Troupe despawnata.");
		}

		// Token: 0x0600968F RID: 38543 RVA: 0x003DCD28 File Offset: 0x003DAF28
		private void HandleTroupeDisbanding(ReporterTroupe troupe)
		{
			this.DespawnTroupe(troupe, ScreenFadeController.Instance != null && ScreenFadeController.Instance.IsOpaque);
		}

		// Token: 0x06009690 RID: 38544 RVA: 0x003DCD4C File Offset: 0x003DAF4C
		private async Task<List<GameObject>> SpawnMembers(NPCGroupConfigSO config, Vector3 center, Quaternion rotation)
		{
			List<GameObject> result = new List<GameObject>();
			int memberCount = config.members.Length;
			for (int i = 0; i < memberCount; i++)
			{
				TroupeMemberSlot slot = config.members[i];
				TroupeMemberSlot troupeMemberSlot = slot;
				if (((troupeMemberSlot != null) ? troupeMemberSlot.npcAsset : null) == null)
				{
					Debug.LogWarning(string.Format("[ReporterTroupeManager] Slot {0} ha npcAsset null, saltato.", i));
				}
				else
				{
					Vector3 vector = rotation * Vector3.right;
					int num = (i + 1) / 2 * ((i % 2 == 0) ? (-1) : 1);
					if (i == 0)
					{
						num = 0;
					}
					Vector3 vector2 = vector * ((float)num * this.memberSpacing);
					Vector3 vector3 = center + vector2;
					GameObject gameObject = null;
					GameObject gameObject2;
					if (this.bakedRegistry != null && this.bakedRegistry.TryGet(slot.npcAsset, out gameObject2) && gameObject2 != null)
					{
						gameObject = Object.Instantiate<GameObject>(gameObject2, vector3, rotation);
						string text = (slot.npcAsset.name.StartsWith("NPC_") ? slot.npcAsset.name.Substring(4) : slot.npcAsset.name);
						gameObject.name = "NPC_" + text;
						gameObject.tag = "NPC";
						if (gameObject.GetComponent<NPCSpawnFade>() == null)
						{
							gameObject.AddComponent<NPCSpawnFade>();
						}
					}
					else if (this.spawner != null)
					{
						gameObject = await this.spawner.SpawnNPC(slot.npcAsset, vector3, rotation, null, slot.basePrefabOverride, true);
					}
					if (gameObject == null)
					{
						Debug.LogWarning(string.Format("[ReporterTroupeManager] Spawn fallito per slot {0} ({1}).", i, slot.npcAsset.name));
					}
					else
					{
						if (slot.animatorOverride != null)
						{
							Animator componentInChildren = gameObject.GetComponentInChildren<Animator>();
							if (componentInChildren != null)
							{
								componentInChildren.runtimeAnimatorController = slot.animatorOverride;
							}
							else
							{
								Debug.LogWarning("[ReporterTroupeManager] " + gameObject.name + ": Animator non trovato, override non applicato.");
							}
						}
						if (slot.prop != null)
						{
							NPCPropHolder component = gameObject.GetComponent<NPCPropHolder>();
							if (component != null)
							{
								component.Equip(slot.prop);
							}
							else
							{
								Debug.LogWarning("[ReporterTroupeManager] " + gameObject.name + ": NPCPropHolder non trovato. Verifica che basePrefabOverride punti a NPCReporterBase/Interviewer.");
							}
						}
						if (i > 0)
						{
							NPCSenses component2 = gameObject.GetComponent<NPCSenses>();
							if (component2 != null)
							{
								component2.enabled = false;
							}
						}
						this.ApplyReporterPriority(gameObject);
						result.Add(gameObject);
						slot = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06009691 RID: 38545 RVA: 0x003DCDA8 File Offset: 0x003DAFA8
		private void ApplyReporterPriority(GameObject go)
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
			float num = 0.45f + (float)(this._reporterPriorityCounter % 11) * 0.01f;
			RVOAgent rvoSettings = component.rvoSettings;
			rvoSettings.priority = num;
			component.rvoSettings = rvoSettings;
			this._reporterPriorityCounter++;
		}

		// Token: 0x06009692 RID: 38546 RVA: 0x003DCE0C File Offset: 0x003DB00C
		private ReporterTroupe BuildTroupe(List<GameObject> npcs, NPCGroupConfigSO config)
		{
			GameObject gameObject = new GameObject(string.Format("ReporterTroupe_{0}", Time.frameCount));
			ReporterTroupe reporterTroupe = gameObject.AddComponent<ReporterTroupe>();
			NPCSenses component = npcs[0].GetComponent<NPCSenses>();
			List<NPCSenses> list = new List<NPCSenses>();
			for (int i = 1; i < npcs.Count; i++)
			{
				NPCSenses component2 = npcs[i].GetComponent<NPCSenses>();
				if (component2 != null)
				{
					list.Add(component2);
				}
				else
				{
					Debug.LogWarning("[ReporterTroupeManager] " + npcs[i].name + ": NPCSenses non trovato, membro escluso.");
				}
			}
			if (component == null)
			{
				Debug.LogError("[ReporterTroupeManager] members[0] (" + npcs[0].name + ") non ha NPCSenses — troupe non valida.");
				Object.Destroy(gameObject);
				return null;
			}
			foreach (GameObject gameObject2 in npcs)
			{
				gameObject2.AddComponent<TroupeDetectionProxy>().Initialize(reporterTroupe);
			}
			reporterTroupe.Initialize(component, list, config, this.tvStudio);
			return reporterTroupe;
		}

		// Token: 0x06009693 RID: 38547 RVA: 0x003DCF2C File Offset: 0x003DB12C
		private void DespawnMemberNPC(NPCSenses npc, bool underScreenFade = false)
		{
			if (npc == null)
			{
				return;
			}
			TroupeDetectionProxy component = npc.GetComponent<TroupeDetectionProxy>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			GameObject go = npc.gameObject;
			if (underScreenFade)
			{
				go.SetActive(false);
				Object.Destroy(go);
				return;
			}
			NPCSpawnFade npcspawnFade = go.GetComponent<NPCSpawnFade>();
			if (npcspawnFade == null)
			{
				npcspawnFade = go.AddComponent<NPCSpawnFade>();
				npcspawnFade.CompleteFadeInImmediately();
			}
			if (npcspawnFade != null)
			{
				npcspawnFade.StartFadeOut(delegate
				{
					if (go != null)
					{
						Object.Destroy(go);
					}
				});
				return;
			}
			if (this.spawner != null)
			{
				this.spawner.DespawnNPC(go, true);
				return;
			}
			Object.Destroy(go);
		}

		// Token: 0x06009694 RID: 38548 RVA: 0x003DCFF8 File Offset: 0x003DB1F8
		private NPCGroupConfigSO SelectConfig()
		{
			if (this.groupConfigs == null || this.groupConfigs.Length == 0)
			{
				return null;
			}
			bool flag = false;
			foreach (NPCGroupConfigSO npcgroupConfigSO in this.groupConfigs)
			{
				if (ReporterTroupeManager.IsUsableConfig(npcgroupConfigSO) && !this.IsConfigInUse(npcgroupConfigSO))
				{
					flag = true;
					break;
				}
			}
			float num = 0f;
			NPCGroupConfigSO npcgroupConfigSO2 = null;
			foreach (NPCGroupConfigSO npcgroupConfigSO3 in this.groupConfigs)
			{
				if (ReporterTroupeManager.IsUsableConfig(npcgroupConfigSO3) && (!flag || !this.IsConfigInUse(npcgroupConfigSO3)))
				{
					npcgroupConfigSO2 = npcgroupConfigSO3;
					num += Mathf.Max(0f, npcgroupConfigSO3.spawnWeight);
				}
			}
			if (num <= 0f)
			{
				return npcgroupConfigSO2;
			}
			float num2 = Random.Range(0f, num);
			float num3 = 0f;
			foreach (NPCGroupConfigSO npcgroupConfigSO4 in this.groupConfigs)
			{
				if (ReporterTroupeManager.IsUsableConfig(npcgroupConfigSO4) && (!flag || !this.IsConfigInUse(npcgroupConfigSO4)))
				{
					num3 += Mathf.Max(0f, npcgroupConfigSO4.spawnWeight);
					if (num2 <= num3)
					{
						return npcgroupConfigSO4;
					}
				}
			}
			return npcgroupConfigSO2;
		}

		// Token: 0x06009695 RID: 38549 RVA: 0x003DD11E File Offset: 0x003DB31E
		private static bool IsUsableConfig(NPCGroupConfigSO config)
		{
			return config != null && config.members != null && config.members.Length != 0 && config.spawnWeight > 0f;
		}

		// Token: 0x06009696 RID: 38550 RVA: 0x003DD14C File Offset: 0x003DB34C
		private bool IsConfigInUse(NPCGroupConfigSO config)
		{
			if (this._pendingConfigs.Contains(config))
			{
				return true;
			}
			foreach (ReporterTroupe reporterTroupe in this._activeTroupes)
			{
				if (reporterTroupe != null && reporterTroupe.Config == config)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04009B98 RID: 39832
		[Header("Configurazioni troupe disponibili")]
		[Tooltip("Lista di NPCGroupConfigSO da cui selezionare casualmente allo spawn.\nLa selezione è pesata tramite spawnWeight di ogni config.")]
		public NPCGroupConfigSO[] groupConfigs;

		// Token: 0x04009B99 RID: 39833
		[Header("Limiti")]
		[Tooltip("Numero massimo di troupe attive contemporaneamente nella scena.")]
		public int maxActiveTroupes = 2;

		// Token: 0x04009B9A RID: 39834
		[Header("Spawning")]
		[Tooltip("Riferimento al NPCRuntimeSpawner presente nella scena.")]
		public NPCRuntimeSpawner spawner;

		// Token: 0x04009B9B RID: 39835
		[Tooltip("Registry dei reporter pre-bakati. Se popolato, ogni membro viene instanziato direttamente dal prefab baked (sync, ~1ms) saltando LoadCharacter + MergeAsync. Auto-caricato da Resources/NPCReporterBakedRegistry.asset se null.")]
		public NPCReporterBakedRegistry bakedRegistry;

		// Token: 0x04009B9C RID: 39836
		[Tooltip("Se attivo e bakedRegistry è null, carica automaticamente Resources/NPCReporterBakedRegistry.asset in Awake.")]
		public bool autoLoadBakedRegistry = true;

		// Token: 0x04009B9D RID: 39837
		[Tooltip("Spaziatura tra i membri della troupe quando vengono spawnati. I membri vengono allineati lungo una linea ortogonale alla direzione di spawn, ognuno separato dal precedente di questa distanza. Tenere piccolo (0.5-1.5m) per evitare che finiscano dentro mesh/muri vicini al punto di spawn.")]
		public float memberSpacing = 0.8f;

		// Token: 0x04009B9E RID: 39838
		[Header("TV Studio (opzionale)")]
		[Tooltip("Riferimento al TVStudioController nella scena. Se assegnato, viene passato alla troupe per la sequenza pre-reaction e la modalità post-emote.")]
		public TVStudioController tvStudio;

		// Token: 0x04009B9F RID: 39839
		private readonly List<ReporterTroupe> _activeTroupes = new List<ReporterTroupe>();

		// Token: 0x04009BA0 RID: 39840
		private readonly List<NPCGroupConfigSO> _pendingConfigs = new List<NPCGroupConfigSO>();

		// Token: 0x04009BA1 RID: 39841
		private int _pendingSpawns;

		// Token: 0x04009BA2 RID: 39842
		private int _reporterPriorityCounter;
	}
}
