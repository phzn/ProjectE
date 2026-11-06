using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Features.Detection;
using Features.Suspicion;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000F00 RID: 3840
	public class ReporterProceduralSpawner : MonoBehaviour
	{
		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x0600731A RID: 29466 RVA: 0x002EEDAC File Offset: 0x002ECFAC
		// (set) Token: 0x0600731B RID: 29467 RVA: 0x002EEDB3 File Offset: 0x002ECFB3
		public static ReporterProceduralSpawner Instance { get; private set; }

		// Token: 0x0600731C RID: 29468 RVA: 0x002EEDBB File Offset: 0x002ECFBB
		public static int DesiredTroupeCount(int suspicionLevel)
		{
			if (suspicionLevel >= 3)
			{
				return 2;
			}
			if (suspicionLevel < 1)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600731D RID: 29469 RVA: 0x002EEDCA File Offset: 0x002ECFCA
		private void Awake()
		{
			if (ReporterProceduralSpawner.Instance != null && ReporterProceduralSpawner.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			ReporterProceduralSpawner.Instance = this;
		}

		// Token: 0x0600731E RID: 29470 RVA: 0x002EEDF4 File Offset: 0x002ECFF4
		private void Start()
		{
			if (this.player == null && GameSystemsBootstrapper.Instance != null)
			{
				this.player = GameSystemsBootstrapper.Instance.Player;
			}
			if (this.player == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.player = gameObject.transform;
				}
			}
			if (this.troupeManager == null)
			{
				this.troupeManager = ReporterTroupeManager.Instance;
			}
			if (this.troupeManager == null)
			{
				this.troupeManager = Object.FindObjectOfType<ReporterTroupeManager>();
			}
			if (this.player == null)
			{
				Debug.LogError("[ReporterProceduralSpawner] Player not found, disabling.");
				base.enabled = false;
				return;
			}
			if (this.troupeManager == null)
			{
				Debug.LogError("[ReporterProceduralSpawner] ReporterTroupeManager not found, disabling.");
				base.enabled = false;
				return;
			}
			if (ChunkManager.Instance != null && ChunkManager.Instance.enabled)
			{
				if (ChunkManager.Instance.IsInitialGenerationComplete)
				{
					this._gateOpen = true;
				}
				else
				{
					ChunkManager.Instance.OnInitialGenerationComplete += this.OnInitialGenerationCompleteHandler;
				}
				ChunkManager.Instance.OnChunkRemoved += this.OnChunkRemovedHandler;
			}
			else
			{
				this._gateOpen = true;
			}
			this._nextEvaluationTime = Time.time;
		}

		// Token: 0x0600731F RID: 29471 RVA: 0x002EEF3C File Offset: 0x002ED13C
		private void OnDestroy()
		{
			if (ChunkManager.Instance != null)
			{
				ChunkManager.Instance.OnInitialGenerationComplete -= this.OnInitialGenerationCompleteHandler;
				ChunkManager.Instance.OnChunkRemoved -= this.OnChunkRemovedHandler;
			}
			if (ReporterProceduralSpawner.Instance == this)
			{
				ReporterProceduralSpawner.Instance = null;
			}
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x002EEF95 File Offset: 0x002ED195
		private void OnInitialGenerationCompleteHandler()
		{
			this._gateOpen = true;
			if (this.enableDebugLogs)
			{
				Debug.Log("[ReporterProceduralSpawner] Procedural map ready — opening reporter spawn gate.");
			}
		}

		// Token: 0x06007321 RID: 29473 RVA: 0x002EEFB0 File Offset: 0x002ED1B0
		private void OnChunkRemovedHandler(ChunkInstance chunk)
		{
			if (this.troupeManager == null || chunk == null || chunk.chunkRoot == null || SpecialNPCProximity.PopulationChangesBlocked)
			{
				return;
			}
			IReadOnlyList<ReporterTroupe> activeTroupes = this.troupeManager.ActiveTroupes;
			for (int i = activeTroupes.Count - 1; i >= 0; i--)
			{
				ReporterTroupe reporterTroupe = activeTroupes[i];
				if (!(reporterTroupe == null) && reporterTroupe.CurrentState != TroupeState.Engaging && !(reporterTroupe.Initiator == null) && reporterTroupe.Initiator.transform.IsChildOf(chunk.chunkRoot.transform))
				{
					this.troupeManager.DespawnTroupe(reporterTroupe, false);
					this._nextEvaluationTime = Time.time;
				}
			}
		}

		// Token: 0x06007322 RID: 29474 RVA: 0x002EF060 File Offset: 0x002ED260
		private void Update()
		{
			if (SpecialNPCProximity.PopulationChangesBlocked)
			{
				return;
			}
			if (!this._gateOpen)
			{
				return;
			}
			if (Time.time < this._nextEvaluationTime)
			{
				return;
			}
			this._nextEvaluationTime = Time.time + Mathf.Min(2f, Mathf.Max(0.1f, this.evaluationInterval));
			if (PlayerProximityProvider.Instance != null && PlayerProximityProvider.Instance.Player != null)
			{
				this.player = PlayerProximityProvider.Instance.Player;
			}
			else if (PlayerRegistry.Player != null)
			{
				this.player = PlayerRegistry.Player;
			}
			if (this.player == null || this.troupeManager == null || this._spawnInProgress)
			{
				return;
			}
			int num = ReporterProceduralSpawner.DesiredTroupeCount((SuspicionManager.Instance != null) ? SuspicionManager.Instance.CurrentLevel : 0);
			IReadOnlyList<ReporterTroupe> activeTroupes = this.troupeManager.ActiveTroupes;
			for (int i = activeTroupes.Count - 1; i >= 0; i--)
			{
				ReporterTroupe reporterTroupe = activeTroupes[i];
				if (!(reporterTroupe == null) && reporterTroupe.CurrentState != TroupeState.Engaging)
				{
					float num2 = ((activeTroupes.Count > num) ? 0f : Mathf.Max(this.despawnDistance, this.maxSpawnDistance + 5f));
					if (SpecialNPCProximity.CanRecycleGroup(reporterTroupe.Initiator, reporterTroupe.AdditionalActors, this.player, num2))
					{
						this.troupeManager.DespawnTroupe(reporterTroupe, false);
					}
				}
			}
			if (this.troupeManager.ActiveTroupeCount >= num)
			{
				return;
			}
			if (!this.troupeManager.CanSpawn)
			{
				return;
			}
			NPCSpawnPoint npcspawnPoint;
			Vector3 vector;
			if (!this.TryFindSpawnPoint(out npcspawnPoint, out vector))
			{
				return;
			}
			this._spawnInProgress = true;
			this.SpawnTroupeAtAsync(vector, npcspawnPoint.transform.rotation);
		}

		// Token: 0x06007323 RID: 29475 RVA: 0x002EF21C File Offset: 0x002ED41C
		private async Task SpawnTroupeAtAsync(Vector3 position, Quaternion rotation)
		{
			try
			{
				ReporterTroupeManager manager = this.troupeManager;
				ReporterTroupe reporterTroupe = await manager.SpawnTroupe(position, rotation);
				if (this == null || !base.isActiveAndEnabled)
				{
					if (manager != null && reporterTroupe != null)
					{
						manager.DespawnTroupe(reporterTroupe, false);
					}
				}
				else
				{
					if (reporterTroupe != null)
					{
						this._nextEvaluationTime = Time.time;
					}
					if (this.enableDebugLogs && reporterTroupe != null)
					{
						Debug.Log(string.Format("[ReporterProceduralSpawner] Spawned troupe at {0}.", position));
					}
					manager = null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReporterProceduralSpawner] SpawnTroupe failed: " + ex.Message);
			}
			finally
			{
				this._spawnInProgress = false;
			}
		}

		// Token: 0x06007324 RID: 29476 RVA: 0x002EF270 File Offset: 0x002ED470
		private bool TryFindSpawnPoint(out NPCSpawnPoint result, out Vector3 position)
		{
			result = null;
			position = Vector3.zero;
			this._candidates.Clear();
			Vector3 position2 = this.player.position;
			float num = this.minSpawnDistance * this.minSpawnDistance;
			float num2 = this.maxSpawnDistance * this.maxSpawnDistance;
			ChunkManager instance = ChunkManager.Instance;
			if (instance != null && instance.enabled && instance.SpawnedChunks.Count > 0)
			{
				using (IEnumerator<KeyValuePair<Vector2Int, ChunkInstance>> enumerator = instance.SpawnedChunks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Vector2Int, ChunkInstance> keyValuePair = enumerator.Current;
						ChunkInstance value = keyValuePair.Value;
						if (!(((value != null) ? value.chunkRoot : null) == null) && value.chunkRoot.activeInHierarchy)
						{
							Vector3 vector = position2 - value.worldPosition;
							if (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z <= (this.maxSpawnDistance + 50f) * (this.maxSpawnDistance + 50f))
							{
								this.AddCandidatesFromChunk(value.chunkRoot, position2, num, num2);
							}
						}
					}
					goto IL_01D8;
				}
			}
			foreach (NPCSpawnPoint npcspawnPoint in Object.FindObjectsOfType<NPCSpawnPoint>())
			{
				if (!(npcspawnPoint == null) && npcspawnPoint.gameObject.activeInHierarchy)
				{
					Vector3 vector2 = position2 - npcspawnPoint.transform.position;
					float num3 = vector2.x * vector2.x + vector2.y * vector2.y + vector2.z * vector2.z;
					if (num3 >= num && num3 <= num2)
					{
						this._candidates.Add(npcspawnPoint);
					}
				}
			}
			IL_01D8:
			if (this._candidates.Count == 0)
			{
				return false;
			}
			int num4 = Mathf.Min(Mathf.Max(1, this.maxCandidates), this._candidates.Count);
			for (int j = 0; j < num4; j++)
			{
				int num5 = Random.Range(0, this._candidates.Count);
				NPCSpawnPoint npcspawnPoint2 = this._candidates[num5];
				this._candidates.RemoveAt(num5);
				Vector3 position3 = npcspawnPoint2.transform.position;
				Vector3 vector3;
				if ((npcspawnPoint2.maxVerticalDistanceFromPlayer <= 0f || Mathf.Abs(position3.y - position2.y) <= npcspawnPoint2.maxVerticalDistanceFromPlayer) && SpecialNPCProximity.TryResolveSpawnPosition(position3, position2, out vector3))
				{
					float sqrMagnitude = (vector3 - position2).sqrMagnitude;
					if (sqrMagnitude >= num && sqrMagnitude <= num2 && this.IsSeparatedFromActiveTroupes(vector3) && this.IsSpawnFootprintHiddenAndClear(vector3, npcspawnPoint2.transform.rotation))
					{
						result = npcspawnPoint2;
						position = vector3;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007325 RID: 29477 RVA: 0x002EF568 File Offset: 0x002ED768
		private void AddCandidatesFromChunk(GameObject chunkRoot, Vector3 playerPos, float minSqr, float maxSqr)
		{
			foreach (NPCSpawnPoint npcspawnPoint in chunkRoot.GetComponentsInChildren<NPCSpawnPoint>(true))
			{
				if (!(npcspawnPoint == null) && npcspawnPoint.gameObject.activeInHierarchy)
				{
					Vector3 vector = playerPos - npcspawnPoint.transform.position;
					float num = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
					if (num >= minSqr && num <= maxSqr)
					{
						this._candidates.Add(npcspawnPoint);
					}
				}
			}
		}

		// Token: 0x06007326 RID: 29478 RVA: 0x002EF5FC File Offset: 0x002ED7FC
		private bool IsSpawnFootprintHiddenAndClear(Vector3 position, Quaternion rotation)
		{
			int num = 1;
			if (this.troupeManager.groupConfigs != null)
			{
				foreach (NPCGroupConfigSO npcgroupConfigSO in this.troupeManager.groupConfigs)
				{
					if (npcgroupConfigSO != null && npcgroupConfigSO.members != null)
					{
						num = Mathf.Max(num, npcgroupConfigSO.members.Length);
					}
				}
			}
			for (int j = 0; j < num; j++)
			{
				int num2 = (j + 1) / 2 * ((j % 2 == 0) ? (-1) : 1);
				Vector3 vector = position + rotation * Vector3.right * ((float)num2 * this.troupeManager.memberSpacing);
				Vector3 vector2;
				if (!SpecialNPCProximity.TryResolveSpawnPosition(vector, this.player.position, out vector2) || (vector2 - vector).sqrMagnitude > 0.25f)
				{
					return false;
				}
				if (!SpecialNPCProximity.IsHiddenFromCamera(vector2, this.player, null))
				{
					return false;
				}
				if (Physics.CheckCapsule(vector2 + Vector3.up * 0.45f, vector2 + Vector3.up * 1.55f, 0.35f, -1, QueryTriggerInteraction.Ignore))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007327 RID: 29479 RVA: 0x002EF728 File Offset: 0x002ED928
		private bool IsSeparatedFromActiveTroupes(Vector3 position)
		{
			float num = this.minimumTroupeSeparation * this.minimumTroupeSeparation;
			foreach (ReporterTroupe reporterTroupe in this.troupeManager.ActiveTroupes)
			{
				if (!(reporterTroupe == null))
				{
					if (ReporterProceduralSpawner.IsMemberNear(reporterTroupe.Initiator, position, num))
					{
						return false;
					}
					if (reporterTroupe.AdditionalActors != null)
					{
						using (IEnumerator<NPCSenses> enumerator2 = reporterTroupe.AdditionalActors.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (ReporterProceduralSpawner.IsMemberNear(enumerator2.Current, position, num))
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06007328 RID: 29480 RVA: 0x002EF7F0 File Offset: 0x002ED9F0
		private static bool IsMemberNear(NPCSenses member, Vector3 position, float distanceSqr)
		{
			if (member == null)
			{
				return false;
			}
			Vector3 vector = member.transform.position - position;
			vector.y = 0f;
			return vector.sqrMagnitude < distanceSqr;
		}

		// Token: 0x04007655 RID: 30293
		[Header("References")]
		[Tooltip("Player transform. If null, found via GameSystemsBootstrapper or Player tag.")]
		[SerializeField]
		private Transform player;

		// Token: 0x04007656 RID: 30294
		[Tooltip("Reporter troupe manager. If null, found via singleton at Start.")]
		[SerializeField]
		private ReporterTroupeManager troupeManager;

		// Token: 0x04007657 RID: 30295
		[Header("Spawning")]
		[Tooltip("Maximum seconds between population checks (capped at 2 for suspicion changes).")]
		[Range(2f, 120f)]
		[SerializeField]
		private float evaluationInterval = 12f;

		// Token: 0x04007658 RID: 30296
		[Header("Spawn Geometry")]
		[Tooltip("Min distance from player where the troupe may appear.")]
		[Range(5f, 80f)]
		[SerializeField]
		private float minSpawnDistance = 20f;

		// Token: 0x04007659 RID: 30297
		[Tooltip("Max distance from player where the troupe may appear.")]
		[Range(10f, 200f)]
		[SerializeField]
		private float maxSpawnDistance = 60f;

		// Token: 0x0400765A RID: 30298
		[Tooltip("Recycle the troupe once every member is farther away and hidden from the camera.")]
		[Min(10f)]
		[SerializeField]
		private float despawnDistance = 80f;

		// Token: 0x0400765B RID: 30299
		[Tooltip("How many candidate spawn points to try before giving up this tick.")]
		[Range(1f, 16f)]
		[SerializeField]
		private int maxCandidates = 6;

		// Token: 0x0400765C RID: 30300
		[Tooltip("Keep separate troupes in different parts of the nearby streets.")]
		[SerializeField]
		[Min(2f)]
		private float minimumTroupeSeparation = 15f;

		// Token: 0x0400765D RID: 30301
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x0400765E RID: 30302
		private List<NPCSpawnPoint> _candidates = new List<NPCSpawnPoint>(32);

		// Token: 0x0400765F RID: 30303
		private float _nextEvaluationTime;

		// Token: 0x04007660 RID: 30304
		private bool _gateOpen;

		// Token: 0x04007661 RID: 30305
		private bool _spawnInProgress;
	}
}
