using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.RenderingPrecision;
using Pathfinding;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D40 RID: 3392
	public sealed class LongWayHomeWorldRoute : MonoBehaviour
	{
		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06006367 RID: 25447 RVA: 0x0027C438 File Offset: 0x0027A638
		// (set) Token: 0x06006368 RID: 25448 RVA: 0x0027C440 File Offset: 0x0027A640
		public bool IsReady { get; private set; }

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06006369 RID: 25449 RVA: 0x0027C449 File Offset: 0x0027A649
		// (set) Token: 0x0600636A RID: 25450 RVA: 0x0027C451 File Offset: 0x0027A651
		public string Error { get; private set; }

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x0600636B RID: 25451 RVA: 0x0027C45A File Offset: 0x0027A65A
		// (set) Token: 0x0600636C RID: 25452 RVA: 0x0027C462 File Offset: 0x0027A662
		public Vector3 SpawnPosition { get; private set; }

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x0600636D RID: 25453 RVA: 0x0027C46B File Offset: 0x0027A66B
		// (set) Token: 0x0600636E RID: 25454 RVA: 0x0027C473 File Offset: 0x0027A673
		public Quaternion SpawnRotation { get; private set; }

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x0600636F RID: 25455 RVA: 0x0027C47C File Offset: 0x0027A67C
		// (set) Token: 0x06006370 RID: 25456 RVA: 0x0027C484 File Offset: 0x0027A684
		public Vector3 HomePosition { get; private set; }

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06006371 RID: 25457 RVA: 0x0027C48D File Offset: 0x0027A68D
		// (set) Token: 0x06006372 RID: 25458 RVA: 0x0027C495 File Offset: 0x0027A695
		public float PreparationProgress { get; private set; }

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06006373 RID: 25459 RVA: 0x0027C49E File Offset: 0x0027A69E
		// (set) Token: 0x06006374 RID: 25460 RVA: 0x0027C4A6 File Offset: 0x0027A6A6
		public float StartingDistance { get; private set; }

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06006375 RID: 25461 RVA: 0x0027C4AF File Offset: 0x0027A6AF
		public IReadOnlyList<Vector3> ValidatedStreetCheckpoints
		{
			get
			{
				return this._checkpoints;
			}
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x0027C4B7 File Offset: 0x0027A6B7
		public IEnumerator Prepare(Transform player, float startingDistance, Action<Vector3> onDistrictSelected = null, ProceduralPoiKind exitPoi = ProceduralPoiKind.None)
		{
			this.Clear();
			this._metadataSeconds = (this._geometrySeconds = (this._populationSeconds = 0f));
			this.StartingDistance = startingDistance;
			this._player = player;
			this._manager = ChunkManager.Instance;
			this._deadline = Time.realtimeSinceStartup + 150f;
			if (player == null)
			{
				this.Fail("The player is unavailable.");
				yield break;
			}
			if (float.IsNaN(startingDistance) || startingDistance < 1000f || startingDistance > 7000f)
			{
				this.Fail("The starting distance is outside the configured range.");
				yield break;
			}
			float homeDeadline = Mathf.Min(this._deadline, Time.realtimeSinceStartup + 25f);
			Vector3 entrance = default(Vector3);
			bool homeRegistered = false;
			while (Time.realtimeSinceStartup < homeDeadline)
			{
				this._manager = ChunkManager.Instance;
				int num;
				homeRegistered = this._manager != null && this._manager.isActiveAndEnabled && !this._manager.IsPoiTravelSuspended && AstarPath.active != null && this._manager.TryGetSessionSeed(out num) && LongWayHomeWorldRoute.TryResolveHomeEntrance(this._manager, out entrance);
				if (homeRegistered)
				{
					break;
				}
				yield return null;
			}
			if (!homeRegistered)
			{
				this.Fail("Ciel's actual house entrance did not finish loading in the city.");
				yield break;
			}
			this.HomePosition = entrance;
			this._mover = Object.FindFirstObjectByType<ProceduralGraphMover>();
			if (this._mover == null || (!this._mover.isActiveAndEnabled && (!(AStarGraphManager.Instance != null) || !AStarGraphManager.Instance.IsNavigationSuspended)))
			{
				this.Fail("The moving street navigation is unavailable.");
				yield break;
			}
			this._focus = new GameObject("LongWayHomePreparationFocus").transform;
			this._focus.SetParent(base.transform, false);
			this._focus.position = this.HomePosition;
			this._originalGraphTarget = this._mover.target;
			this._originalGraphTileLimit = this._mover.maxRecastTilesPerMove;
			this._mover.target = this._focus;
			this._ownsFocus = true;
			int seed;
			if (!this._manager.TryGetSessionSeed(out seed))
			{
				this.Fail("The city session seed is unavailable.");
				yield break;
			}
			if (exitPoi != ProceduralPoiKind.None)
			{
				yield return this.PreparePoiExit(seed, exitPoi, onDistrictSelected);
				yield break;
			}
			Debug.Log(string.Format("[LongWayHome] Preparing local hidden-alley spawn: seed={0}, home={1}, ", seed, this.HomePosition) + string.Format("startingDistance={0:0}m.", this.StartingDistance));
			Random random = new Random(seed);
			Vector3 homeCorner = LongWayHomeWorldRoute.BuildStreetCheckpoints(this.HomePosition, this._manager.CellSize, 1, 0)[0];
			this.PreparationProgress = 0.1f;
			if (!this._manager.BeginPreparationFocusSelection(this))
			{
				this.Fail("The city streaming focus is already owned.");
				yield break;
			}
			float metadataStart = Time.realtimeSinceStartup;
			yield return this.FindDistantAlleyRoutes(random);
			this._metadataSeconds += Time.realtimeSinceStartup - metadataStart;
			if (this._alleyRouteOffsets.Count == 0)
			{
				this.Fail("No authored hidden-alley district is available between 1 and 7 km from home.");
				yield break;
			}
			int districts = 0;
			int routeIndex = 0;
			while (routeIndex < this._alleyRouteOffsets.Count && Time.realtimeSinceStartup < this._deadline)
			{
				Vector2Int vector2Int = this._alleyRouteOffsets[routeIndex];
				int num = districts;
				districts = num + 1;
				Vector3 vector = homeCorner + new Vector3((float)vector2Int.x, 0f, (float)vector2Int.y) * this._manager.CellSize;
				if (NPCZoneManager.Instance == null || !NPCZoneManager.Instance.TrySetPreparationFocus(this, vector))
				{
					this.Fail("The local NPC population is unavailable for alley preparation.");
					yield break;
				}
				this._mover.maxRecastTilesPerMove = 0;
				if (onDistrictSelected != null)
				{
					onDistrictSelected(vector);
				}
				this.PreparationProgress = 0.25f + 0.65f * (float)routeIndex / (float)Mathf.Max(1, this._alleyRouteOffsets.Count - 1);
				yield return this.PrepareHiddenAlleySpawn(vector, random);
				if (this.IsReady)
				{
					float num2 = LongWayHomeWorldRoute.PlanarDistance(this.SpawnPosition, this.HomePosition);
					this.PreparationProgress = 1f;
					Debug.Log(string.Format("[LongWayHome] Validated hidden alley {0:0}m from home after {1} districts; ", num2, districts) + string.Format("metadata={0:F2}s, geometry/navigation={1:F2}s, ", this._metadataSeconds, this._geometrySeconds) + string.Format("population={0:F2}s; no corridor pre-generation.", this._populationSeconds));
					yield break;
				}
				num = routeIndex;
				routeIndex = num + 1;
			}
			Debug.LogWarning(string.Format("[LongWayHome] Hidden-alley search exhausted: seed={0}, districts={1}, ", seed, districts) + string.Format("metadataCandidates={0}, deadlineReached={1}.", this._alleyRouteOffsets.Count, Time.realtimeSinceStartup >= this._deadline));
			this.Fail("No safe hidden alley with a local street connection was ready between 1 and 7 km from home.");
			yield break;
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x0027C4E3 File Offset: 0x0027A6E3
		private IEnumerator FindDistantAlleyRoutes(Random random)
		{
			this._alleyRouteOffsets.Clear();
			Dictionary<Vector2Int, Vector3?> cells = new Dictionary<Vector2Int, Vector3?>();
			HashSet<Vector2Int> selectedOffsets = new HashSet<Vector2Int>();
			int inspected = 0;
			Vector3 corner = LongWayHomeWorldRoute.BuildStreetCheckpoints(this.HomePosition, this._manager.CellSize, 1, 0)[0];
			foreach (Vector2Int vector2Int in LongWayHomeWorldRoute.EnumerateDistantAlleyOffsets(this.HomePosition, this._manager.CellSize, random.Next(), this.StartingDistance))
			{
				if (Time.realtimeSinceStartup >= this._deadline)
				{
					yield break;
				}
				Vector3 vector = corner + new Vector3((float)vector2Int.x, 0f, (float)vector2Int.y) * this._manager.CellSize;
				for (int i = -1; i <= 1; i += 2)
				{
					for (int j = -1; j <= 1; j += 2)
					{
						Vector2Int vector2Int2 = this._manager.WorldToCell(vector + new Vector3((float)i, 0f, (float)j) * (this._manager.CellSize * 0.25f));
						Vector3? vector2;
						if (!cells.TryGetValue(vector2Int2, out vector2))
						{
							Vector3 vector3;
							vector2 = (this.TryResolveAuthoredAlleyStreet(vector2Int2, out vector3) ? new Vector3?(vector3) : null);
							cells.Add(vector2Int2, vector2);
						}
						if (vector2 != null)
						{
							Vector3 vector4 = (vector2.Value - corner) / this._manager.CellSize;
							Vector2Int vector2Int3 = new Vector2Int(Mathf.RoundToInt(vector4.x), Mathf.RoundToInt(vector4.z));
							if (selectedOffsets.Add(vector2Int3))
							{
								this._alleyRouteOffsets.Add(vector2Int3);
							}
							if (this._alleyRouteOffsets.Count >= 16)
							{
								yield break;
							}
						}
					}
				}
				int num = inspected + 1;
				inspected = num;
				if (num % 8 == 0)
				{
					this.PreparationProgress = 0.1f + 0.15f * (float)inspected / 768f;
					yield return null;
				}
			}
			IEnumerator<Vector2Int> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x0027C4FC File Offset: 0x0027A6FC
		private bool TryResolveAuthoredAlleyStreet(Vector2Int cell, out Vector3 street)
		{
			street = default(Vector3);
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			if (this._manager.TryResolveMiniMapPreview(cell, out spawnEntry) && !(spawnEntry.chunkData == null) && !(spawnEntry.chunkData.gridSize != Vector2Int.one))
			{
				ChunkTemplate template = spawnEntry.chunkData.template;
				if (!(((template != null) ? template.chunkPrefab : null) == null) && spawnEntry.zoneIndices != null && spawnEntry.chunkData.availableZones != null)
				{
					GameObject chunkPrefab = spawnEntry.chunkData.template.chunkPrefab;
					Transform[] array;
					if (!this._prefabBaseTransforms.TryGetValue(chunkPrefab, out array))
					{
						List<Transform> list = new List<Transform>();
						foreach (Transform transform in chunkPrefab.GetComponentsInChildren<Transform>(true))
						{
							if (transform != chunkPrefab.transform && transform.name.Contains("Base"))
							{
								list.Add(transform);
							}
						}
						array = list.ToArray();
						this._prefabBaseTransforms.Add(chunkPrefab, array);
					}
					Quaternion quaternion = Quaternion.Euler(0f, (float)spawnEntry.rotation * 90f, 0f);
					Vector3 vector = new Vector3((float)cell.x, 0f, (float)cell.y) * this._manager.CellSize;
					for (int j = 0; j < spawnEntry.zoneIndices.Length; j++)
					{
						int num = spawnEntry.zoneIndices[j];
						if (num >= 0 && num < spawnEntry.chunkData.availableZones.Count)
						{
							ZoneData zoneData = spawnEntry.chunkData.availableZones[num];
							if (!(((zoneData != null) ? zoneData.zonePrefab : null) == null) && LongWayHomeWorldRoute.IsSupportedAlleyPrefabName(zoneData.zonePrefab.name))
							{
								Transform transform2 = ((j < array.Length) ? array[j] : chunkPrefab.transform);
								Vector3 vector2 = chunkPrefab.transform.InverseTransformPoint(transform2.position);
								Quaternion quaternion2 = Quaternion.Inverse(chunkPrefab.transform.rotation) * transform2.rotation;
								Vector3 vector3 = vector + quaternion * Vector3.Scale(vector2, chunkPrefab.transform.localScale);
								Quaternion quaternion3 = quaternion * quaternion2;
								foreach (Vector3 vector4 in LongWayHomeWorldRoute.AuthoredAlleyPoses)
								{
									Vector3 vector5 = vector3 + quaternion3 * Vector3.Scale(vector4, zoneData.zonePrefab.transform.localScale);
									if (LongWayHomeWorldRoute.IsStartingDistanceNearTarget(vector5, this.HomePosition, this.StartingDistance, this._manager.CellSize))
									{
										street = LongWayHomeWorldRoute.ClosestChunkStreetCorner(vector, vector5, this._manager.CellSize);
										return true;
									}
								}
							}
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x0027C7D8 File Offset: 0x0027A9D8
		public static Vector3 ClosestChunkStreetCorner(Vector3 chunkCenter, Vector3 pose, float cellSize)
		{
			float num = cellSize * 0.5f;
			return new Vector3(chunkCenter.x + ((pose.x < chunkCenter.x) ? (-num) : num), 0f, chunkCenter.z + ((pose.z < chunkCenter.z) ? (-num) : num));
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x0027C82B File Offset: 0x0027AA2B
		public static float SelectStartingDistance(int seed)
		{
			return 1000f + (float)new Random(seed).NextDouble() * 6000f;
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x0027C845 File Offset: 0x0027AA45
		public static bool IsStartingDistanceNearTarget(Vector3 position, Vector3 home, float startingDistance, float cellSize)
		{
			return LongWayHomeWorldRoute.IsStartingDistanceInRange(position, home) && Mathf.Abs(LongWayHomeWorldRoute.PlanarDistance(position, home) - startingDistance) <= cellSize * 2f;
		}

		// Token: 0x0600637C RID: 25468 RVA: 0x0027C86C File Offset: 0x0027AA6C
		public static bool IsStartingDistanceInRange(Vector3 position, Vector3 home)
		{
			float num = LongWayHomeWorldRoute.PlanarDistance(position, home);
			return num >= 1000f && num <= 7000f;
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x0027C896 File Offset: 0x0027AA96
		public static IEnumerable<Vector2Int> EnumerateDistantAlleyOffsets(Vector3 home, float cellSize, int seed, float startingDistance)
		{
			if (cellSize <= 0f || float.IsNaN(startingDistance) || startingDistance < 1000f || startingDistance > 7000f)
			{
				yield break;
			}
			Random random = new Random(seed);
			HashSet<Vector2Int> unique = new HashSet<Vector2Int>();
			Vector3 corner = LongWayHomeWorldRoute.BuildStreetCheckpoints(home, cellSize, 1, 0)[0];
			int num2;
			for (int i = 0; i < 768; i = num2 + 1)
			{
				float num = (float)random.NextDouble() * 3.1415927f * 2f;
				Vector3 vector = home + new Vector3(Mathf.Cos(num), 0f, Mathf.Sin(num)) * startingDistance;
				Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt((vector.x - corner.x) / cellSize), Mathf.RoundToInt((vector.z - corner.z) / cellSize));
				if (LongWayHomeWorldRoute.IsStartingDistanceInRange(corner + new Vector3((float)vector2Int.x, 0f, (float)vector2Int.y) * cellSize, home) && unique.Add(vector2Int))
				{
					yield return vector2Int;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600637E RID: 25470 RVA: 0x0027C8BC File Offset: 0x0027AABC
		public static List<Vector2Int> CollectStreetCorridorCells(IReadOnlyList<Vector3> points, float cellSize)
		{
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			List<Vector2Int> list = new List<Vector2Int>();
			foreach (Vector3 vector in points)
			{
				for (int i = -1; i <= 1; i += 2)
				{
					for (int j = -1; j <= 1; j += 2)
					{
						Vector3 vector2 = vector + new Vector3((float)i, 0f, (float)j) * (cellSize * 0.25f);
						Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(vector2.x / cellSize), Mathf.RoundToInt(vector2.z / cellSize));
						if (hashSet.Add(vector2Int))
						{
							list.Add(vector2Int);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x0027C98C File Offset: 0x0027AB8C
		private bool CellContainsAuthoredAlley(Vector2Int cell)
		{
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			if (!this._manager.TryResolveMiniMapPreview(cell, out spawnEntry) || spawnEntry.chunkData == null || spawnEntry.chunkData.gridSize != Vector2Int.one || spawnEntry.zoneIndices == null || spawnEntry.chunkData.availableZones == null)
			{
				return false;
			}
			foreach (int num in spawnEntry.zoneIndices)
			{
				if (num >= 0 && num < spawnEntry.chunkData.availableZones.Count)
				{
					ZoneData zoneData = spawnEntry.chunkData.availableZones[num];
					if (zoneData != null && zoneData.zonePrefab != null && LongWayHomeWorldRoute.IsSupportedAlleyPrefabName(zoneData.zonePrefab.name))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x0027CA57 File Offset: 0x0027AC57
		public static bool IsSupportedAlleyPrefabName(string name)
		{
			return !string.IsNullOrEmpty(name) && (name.StartsWith("Residential_2 - Half - Chunk1x1", StringComparison.Ordinal) || string.Equals(name, "Zone_Residential_2 - Half - Chunk1x1", StringComparison.Ordinal));
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x0027CA7F File Offset: 0x0027AC7F
		private IEnumerator PrepareHiddenAlleySpawn(Vector3 street, Random random)
		{
			float districtDeadline = Mathf.Min(this._deadline, Time.realtimeSinceStartup + (this._manager.IsInitialGenerationComplete ? 18f : 90f));
			this._focus.position = street;
			this._manager.SetPreparationFocus(this, street);
			NPCZoneManager population = NPCZoneManager.Instance;
			if (!(population == null))
			{
				SensoryManager instance = SensoryManager.Instance;
				if (!(((instance != null) ? instance.GlobalConfig : null) == null))
				{
					int stableFrames = 0;
					float geometryStart = Time.realtimeSinceStartup;
					while (Time.realtimeSinceStartup < districtDeadline)
					{
						this.CollectAuthoredSpawnCandidates(street);
						bool flag = false;
						foreach (LongWayHomeWorldRoute.SpawnCandidate spawnCandidate in this._candidates)
						{
							Vector3 vector;
							if (this.IsInAlleyRange(spawnCandidate.position, street) && this.TryGroundedPose(spawnCandidate.position, spawnCandidate.rotation, out vector))
							{
								flag = true;
								break;
							}
						}
						stableFrames = (((flag || !this._alleyGeometryPending) && this.IsNavigationSettled() && this.IsAlleyIntersectionReady(street)) ? (stableFrames + 1) : 0);
						if (stableFrames >= 3)
						{
							break;
						}
						yield return null;
					}
					this._geometrySeconds += Time.realtimeSinceStartup - geometryStart;
					if (Time.realtimeSinceStartup >= districtDeadline)
					{
						Debug.LogWarning(string.Format("[LongWayHome] Alley district rejected at {0}: candidates={1}, ", street, this._candidates.Count) + string.Format("readiness timeout, intersectionReady={0}, ", this.IsAlleyIntersectionReady(street)) + string.Format("alleyGeometryPending={0}.", this._alleyGeometryPending));
						yield break;
					}
					for (int j = this._candidates.Count - 1; j > 0; j--)
					{
						int num = random.Next(j + 1);
						LongWayHomeWorldRoute.SpawnCandidate spawnCandidate2 = this._candidates[j];
						this._candidates[j] = this._candidates[num];
						this._candidates[num] = spawnCandidate2;
					}
					int outOfDistanceRange = 0;
					int outsideWindow = 0;
					int badGround = 0;
					int disconnected = 0;
					int exposed = 0;
					int witnesses = 0;
					bool populationTimeout = false;
					string firstConnectionFailure = null;
					int i = 0;
					while (i < this._candidates.Count && Time.realtimeSinceStartup < districtDeadline)
					{
						LongWayHomeWorldRoute.SpawnCandidate candidate = this._candidates[i];
						int num2;
						Vector3 grounded;
						Vector3 vector2;
						if (!LongWayHomeWorldRoute.IsStartingDistanceNearTarget(candidate.position, this.HomePosition, this.StartingDistance, this._manager.CellSize))
						{
							num2 = outOfDistanceRange;
							outOfDistanceRange = num2 + 1;
						}
						else if (LongWayHomeWorldRoute.PlanarDistance(candidate.position, street) > this._manager.CellSize)
						{
							num2 = outsideWindow;
							outsideWindow = num2 + 1;
						}
						else if (!this.TryGroundedPose(candidate.position, candidate.rotation, out grounded))
						{
							num2 = badGround;
							badGround = num2 + 1;
						}
						else if (!LongWayHomeWorldRoute.TryConnectedPosition(street, grounded, 0.5f, out vector2))
						{
							num2 = disconnected;
							disconnected = num2 + 1;
							if (firstConnectionFailure == null)
							{
								firstConnectionFailure = string.Concat(new string[]
								{
									string.Format("before population, pose={0}, ", grounded),
									"streetNode=",
									LongWayHomeWorldRoute.DescribeNearest(street),
									", alleyNode=",
									LongWayHomeWorldRoute.DescribeNearest(grounded)
								});
							}
						}
						else if (!this.TryGroundedPose(vector2, candidate.rotation, out grounded))
						{
							num2 = badGround;
							badGround = num2 + 1;
						}
						else if (!this.IsHiddenAlleyPose(grounded, candidate.chunk, candidate.zone))
						{
							num2 = exposed;
							exposed = num2 + 1;
						}
						else if (this.IsVisibleFromAuthoredNpcSpawn(grounded, candidate.chunk))
						{
							num2 = witnesses;
							witnesses = num2 + 1;
						}
						else
						{
							this._focus.position = grounded;
							this._manager.SetPreparationFocus(this, grounded);
							if (!population.TrySetPreparationFocus(this, grounded))
							{
								yield break;
							}
							stableFrames = 0;
							float populationStart = Time.realtimeSinceStartup;
							while (Time.realtimeSinceStartup < districtDeadline)
							{
								stableFrames = ((this.IsNavigationSettled() && this._manager.IsPreparationAreaReady(grounded, 0) && population.IsInitialPopulationComplete) ? (stableFrames + 1) : 0);
								if (stableFrames >= 3)
								{
									break;
								}
								yield return null;
							}
							this._populationSeconds += Time.realtimeSinceStartup - populationStart;
							if (Time.realtimeSinceStartup >= districtDeadline)
							{
								populationTimeout = true;
								break;
							}
							Vector3 vector;
							if (!this.TryGroundedPose(grounded, candidate.rotation, out grounded))
							{
								num2 = badGround;
								badGround = num2 + 1;
							}
							else if (!LongWayHomeWorldRoute.TryConnectedPosition(street, grounded, 0.5f, out vector))
							{
								num2 = disconnected;
								disconnected = num2 + 1;
								if (firstConnectionFailure == null)
								{
									firstConnectionFailure = string.Concat(new string[]
									{
										string.Format("after population, pose={0}, ", grounded),
										"streetNode=",
										LongWayHomeWorldRoute.DescribeNearest(street),
										", alleyNode=",
										LongWayHomeWorldRoute.DescribeNearest(grounded)
									});
								}
							}
							else if (!this.IsHiddenAlleyPose(grounded, candidate.chunk, candidate.zone))
							{
								num2 = exposed;
								exposed = num2 + 1;
							}
							else if (this.IsVisibleFromAuthoredNpcSpawn(grounded, candidate.chunk) || this.IsObservedByNearbyNpc(grounded))
							{
								num2 = witnesses;
								witnesses = num2 + 1;
							}
							else
							{
								if (LongWayHomeWorldRoute.IsStartingDistanceNearTarget(grounded, this.HomePosition, this.StartingDistance, this._manager.CellSize))
								{
									this.SpawnPosition = grounded;
									this.SpawnRotation = candidate.rotation;
									this.IsReady = true;
									yield break;
								}
								num2 = outOfDistanceRange;
								outOfDistanceRange = num2 + 1;
							}
						}
						num2 = i;
						i = num2 + 1;
					}
					Debug.LogWarning(string.Format("[LongWayHome] Alley district rejected at {0}: candidates={1}, ", street, this._candidates.Count) + string.Format("distanceRange={0}, outsideWindow={1}, ground={2}, disconnected={3}, ", new object[] { outOfDistanceRange, outsideWindow, badGround, disconnected }) + string.Format("exposed={0}, witnesses={1}, populationTimeout={2}.", exposed, witnesses, populationTimeout) + ((firstConnectionFailure != null) ? (" First disconnected: " + firstConnectionFailure + ".") : string.Empty));
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x06006382 RID: 25474 RVA: 0x0027CA9C File Offset: 0x0027AC9C
		private bool IsAlleyIntersectionReady(Vector3 street)
		{
			if (this._manager == null)
			{
				return false;
			}
			float num = this._manager.CellSize * 0.25f;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					if (!this._manager.IsPreparationAreaReady(street + new Vector3((float)i, 0f, (float)j) * num, 0))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06006383 RID: 25475 RVA: 0x0027CB10 File Offset: 0x0027AD10
		private bool IsNavigationSettled()
		{
			if (this._ownsFocus && this._mover != null && this._focus != null)
			{
				this._mover.target = this._focus;
				this._mover.maxRecastTilesPerMove = 0;
			}
			return AstarPath.active != null && this._mover != null && (AStarGraphManager.Instance == null || AStarGraphManager.Instance.IsReady) && !this._mover.updatingGraph && !this._mover.HasPendingRecastShift && !AstarPath.active.isScanning && !AstarPath.active.IsAnyGraphUpdateQueued && !AstarPath.active.IsAnyGraphUpdateInProgress && !AstarPath.active.IsAnyWorkItemInProgress && (AStarGraphManager.Instance == null || AStarGraphManager.Instance.PendingZoneUpdateCount == 0);
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x0027CC00 File Offset: 0x0027AE00
		private IEnumerator PrepareCheckpoint(Vector3 previous, Vector3 desired, float nearestTolerance, bool resolveHomeExit = false, bool sampleStreetGround = false)
		{
			this._stepReady = false;
			Vector3 vector = (previous + desired) * 0.5f;
			this._focus.position = vector;
			if (!this._manager.SetPreparationFocus(this, vector))
			{
				yield break;
			}
			float deadline = Mathf.Min(this._deadline, Time.realtimeSinceStartup + 25f);
			int settledFrames = 0;
			while (Time.realtimeSinceStartup < deadline)
			{
				if (this._manager == null || this._mover == null || AstarPath.active == null)
				{
					yield break;
				}
				settledFrames = ((this._manager.IsPreparationAreaReady(desired, 0) && this._manager.IsPreparationAreaReady(previous, 0) && !this._mover.updatingGraph && !this._mover.HasPendingRecastShift && !AstarPath.active.isScanning && !AstarPath.active.IsAnyGraphUpdateQueued && !AstarPath.active.IsAnyGraphUpdateInProgress && !AstarPath.active.IsAnyWorkItemInProgress && (AStarGraphManager.Instance == null || AStarGraphManager.Instance.PendingZoneUpdateCount == 0)) ? (settledFrames + 1) : 0);
				Vector3 vector3;
				if (settledFrames >= 3 && (resolveHomeExit ? this.TryResolveHomeStreetExit(desired, nearestTolerance, out this._stepPosition) : LongWayHomeWorldRoute.TryConnectedPosition(previous, desired, nearestTolerance, out this._stepPosition)))
				{
					Physics.SyncTransforms();
					Vector3 vector2 = (resolveHomeExit ? this._stepPosition : previous);
					if (LongWayHomeWorldRoute.IsWalkingSegmentClear(vector2, vector2, this._player, this._overlaps, this._groundHits) && LongWayHomeWorldRoute.IsWalkingSegmentClear(this._stepPosition, this._stepPosition, this._player, this._overlaps, this._groundHits) && (!sampleStreetGround || LongWayHomeWorldRoute.HasStreetGroundSupport(previous, this._stepPosition, this._player)))
					{
						this._stepReady = true;
						yield break;
					}
				}
				else if (settledFrames >= 3 && !resolveHomeExit && LongWayHomeWorldRoute.HasPhysicalStreetConnection(previous, desired, this._manager.CellSize, this._player, this._overlaps, this._groundHits) && LongWayHomeWorldRoute.TryGetWalkingGround(desired, this._player, this._groundHits, out vector3, false))
				{
					this._stepPosition = vector3;
					this._stepReady = true;
					yield break;
				}
				yield return null;
			}
			Physics.SyncTransforms();
			Vector3 vector4 = (resolveHomeExit ? this._stepPosition : previous);
			bool flag = LongWayHomeWorldRoute.IsWalkingSegmentClear(vector4, vector4, this._player, this._overlaps, this._groundHits);
			bool flag2 = LongWayHomeWorldRoute.IsWalkingSegmentClear(this._stepPosition, this._stepPosition, this._player, this._overlaps, this._groundHits);
			bool flag3 = !sampleStreetGround || LongWayHomeWorldRoute.HasStreetGroundSupport(previous, this._stepPosition, this._player);
			string[] array = new string[10];
			array[0] = string.Format("[LongWayHome] Route checkpoint timed out: from={0}, to={1}, ", previous, desired);
			array[1] = string.Format("worldReady={0}, ", this._manager != null && this._manager.IsPreparationAreaReady(desired, 0) && this._manager.IsPreparationAreaReady(previous, 0));
			array[2] = string.Format("graphMoving={0}, ", this._mover != null && (this._mover.updatingGraph || this._mover.HasPendingRecastShift));
			int num = 3;
			string text = "pendingZoneUpdates={0}, ";
			AStarGraphManager instance = AStarGraphManager.Instance;
			array[num] = string.Format(text, (instance != null) ? instance.PendingZoneUpdateCount : (-1));
			array[4] = string.Format("fromClear={0}, toClear={1}, streetGround={2}, ", flag, flag2, flag3);
			array[5] = "fromNode=";
			array[6] = LongWayHomeWorldRoute.DescribeNearest(previous);
			array[7] = ", toNode=";
			array[8] = LongWayHomeWorldRoute.DescribeNearest(desired);
			array[9] = ".";
			Debug.LogWarning(string.Concat(array));
			yield break;
		}

		// Token: 0x06006385 RID: 25477 RVA: 0x0027CC34 File Offset: 0x0027AE34
		private static string DescribeNearest(Vector3 point)
		{
			if (AstarPath.active == null)
			{
				return "no graph";
			}
			NNInfo nearest = AstarPath.active.GetNearest(point, NearestNodeConstraint.Walkable);
			if (nearest.node != null)
			{
				return string.Format("{0} (area {1}, delta {2:0.00})", nearest.position, nearest.node.Area, Vector3.Distance(point, nearest.position));
			}
			return "none";
		}

		// Token: 0x06006386 RID: 25478 RVA: 0x0027CCAC File Offset: 0x0027AEAC
		private bool TryResolveHomeStreetExit(Vector3 desiredStreet, float tolerance, out Vector3 exit)
		{
			exit = this.HomePosition;
			AstarPath active = AstarPath.active;
			if (active == null)
			{
				return false;
			}
			NNInfo nearest = active.GetNearest(desiredStreet, NearestNodeConstraint.Walkable);
			if (nearest.node == null || Vector3.Distance(nearest.position, desiredStreet) > tolerance)
			{
				return false;
			}
			NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
			walkable.area = (int)nearest.node.Area;
			NNInfo nearest2 = active.GetNearest(this.HomePosition, walkable);
			if (nearest2.node == null || Vector3.Distance(nearest2.position, this.HomePosition) > 3f || !PathUtilities.IsPathPossible(nearest2.node, nearest.node) || !LongWayHomeWorldRoute.IsShortHomeWalkConnectionClear(this.HomePosition, nearest2.position, this._player))
			{
				return false;
			}
			exit = nearest2.position;
			return true;
		}

		// Token: 0x06006387 RID: 25479 RVA: 0x0027CD80 File Offset: 0x0027AF80
		public static bool IsShortHomeWalkConnectionClear(Vector3 from, Vector3 to, Transform ignoredPlayer = null)
		{
			if (Vector3.Distance(from, to) > 3f)
			{
				return false;
			}
			Physics.SyncTransforms();
			Collider[] array = new Collider[32];
			RaycastHit[] array2 = new RaycastHit[32];
			if (LongWayHomeWorldRoute.IsWalkingSegmentClear(from, to, ignoredPlayer, array, array2))
			{
				return true;
			}
			Vector3 vector = new Vector3(to.x, from.y, from.z);
			if (LongWayHomeWorldRoute.IsWalkingSegmentClear(from, vector, ignoredPlayer, array, array2) && LongWayHomeWorldRoute.IsWalkingSegmentClear(vector, to, ignoredPlayer, array, array2))
			{
				return true;
			}
			Vector3 vector2 = new Vector3(from.x, from.y, to.z);
			return LongWayHomeWorldRoute.IsWalkingSegmentClear(from, vector2, ignoredPlayer, array, array2) && LongWayHomeWorldRoute.IsWalkingSegmentClear(vector2, to, ignoredPlayer, array, array2);
		}

		// Token: 0x06006388 RID: 25480 RVA: 0x0027CE24 File Offset: 0x0027B024
		public static bool HasStreetGroundSupport(Vector3 from, Vector3 to, Transform ignoredPlayer = null)
		{
			Physics.SyncTransforms();
			RaycastHit[] array = new RaycastHit[32];
			if (LongWayHomeWorldRoute.HasStreetGroundSupportTrack(from, to, ignoredPlayer, array))
			{
				return true;
			}
			Vector3 vector = Vector3.Cross(Vector3.up, to - from).normalized * 0.2f;
			return vector.sqrMagnitude > 0f && (LongWayHomeWorldRoute.HasStreetGroundSupportTrack(from + vector, to + vector, ignoredPlayer, array) || LongWayHomeWorldRoute.HasStreetGroundSupportTrack(from - vector, to - vector, ignoredPlayer, array));
		}

		// Token: 0x06006389 RID: 25481 RVA: 0x0027CEB0 File Offset: 0x0027B0B0
		private static bool HasStreetGroundSupportTrack(Vector3 from, Vector3 to, Transform ignoredPlayer, RaycastHit[] hits)
		{
			int num = Mathf.Max(1, Mathf.CeilToInt(LongWayHomeWorldRoute.PlanarDistance(from, to) / 0.5f));
			Vector3 vector = default(Vector3);
			for (int i = 0; i <= num; i++)
			{
				Vector3 vector2;
				if (!LongWayHomeWorldRoute.TryGetWalkingGround(Vector3.Lerp(from, to, (float)i / (float)num), ignoredPlayer, hits, out vector2, true) || (i > 0 && Mathf.Abs(vector2.y - vector.y) > 0.3f))
				{
					return false;
				}
				vector = vector2;
			}
			return true;
		}

		// Token: 0x0600638A RID: 25482 RVA: 0x0027CF24 File Offset: 0x0027B124
		public static bool HasPhysicalStreetConnection(Vector3 from, Vector3 to, float maximumStreetLength = 60f, Transform ignoredPlayer = null)
		{
			return LongWayHomeWorldRoute.HasPhysicalStreetConnection(from, to, maximumStreetLength, ignoredPlayer, new Collider[32], new RaycastHit[32]);
		}

		// Token: 0x0600638B RID: 25483 RVA: 0x0027CF40 File Offset: 0x0027B140
		private static bool HasPhysicalStreetConnection(Vector3 from, Vector3 to, float maximumStreetLength, Transform ignoredPlayer, Collider[] overlaps, RaycastHit[] hits)
		{
			if (maximumStreetLength <= 0f || Vector3.Distance(from, to) > maximumStreetLength + 0.1f)
			{
				return false;
			}
			Physics.SyncTransforms();
			if (LongWayHomeWorldRoute.IsWalkingSegmentClear(from, to, ignoredPlayer, overlaps, hits))
			{
				return true;
			}
			Vector3 vector = Vector3.Cross(Vector3.up, to - from).normalized * 0.2f;
			if (vector.sqrMagnitude == 0f)
			{
				return false;
			}
			for (int i = -1; i <= 1; i += 2)
			{
				Vector3 vector2 = vector * (float)i;
				if (LongWayHomeWorldRoute.IsWalkingSegmentClear(from, from + vector2, ignoredPlayer, overlaps, hits) && LongWayHomeWorldRoute.IsWalkingSegmentClear(from + vector2, to + vector2, ignoredPlayer, overlaps, hits) && LongWayHomeWorldRoute.IsWalkingSegmentClear(to + vector2, to, ignoredPlayer, overlaps, hits))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600638C RID: 25484 RVA: 0x0027D00C File Offset: 0x0027B20C
		private static bool TryGetWalkingGround(Vector3 sample, Transform ignoredPlayer, RaycastHit[] hits, out Vector3 ground, bool ignoreStreetObstacles = false)
		{
			ground = default(Vector3);
			int num = Physics.RaycastNonAlloc(sample + Vector3.up * 1.2f, Vector3.down, hits, 2.4f, -5, QueryTriggerInteraction.Ignore);
			if (num == hits.Length)
			{
				return false;
			}
			float num2 = float.PositiveInfinity;
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < num; i++)
			{
				if (!LongWayHomeWorldRoute.IsIgnoredPlayerCollider(hits[i].collider, ignoredPlayer) && hits[i].distance < num2 && (!ignoreStreetObstacles || (hits[i].normal.y >= 0.85f && Mathf.Abs(hits[i].point.y - sample.y) <= 0.6f && (!(hits[i].rigidbody != null) || hits[i].rigidbody.isKinematic))))
				{
					raycastHit = hits[i];
					num2 = hits[i].distance;
				}
			}
			if (raycastHit.collider == null || raycastHit.normal.y < 0.85f || Mathf.Abs(raycastHit.point.y - sample.y) > 0.6f || (raycastHit.rigidbody != null && !raycastHit.rigidbody.isKinematic))
			{
				return false;
			}
			ground = raycastHit.point;
			return true;
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x0027D184 File Offset: 0x0027B384
		private static bool IsWalkingSegmentClear(Vector3 from, Vector3 to, Transform ignoredPlayer, Collider[] overlaps, RaycastHit[] hits)
		{
			int num = Mathf.Max(1, Mathf.CeilToInt(LongWayHomeWorldRoute.PlanarDistance(from, to) / 0.2f));
			Vector3 vector = default(Vector3);
			for (int i = 0; i <= num; i++)
			{
				Vector3 vector2;
				if (!LongWayHomeWorldRoute.TryGetWalkingGround(Vector3.Lerp(from, to, (float)i / (float)num), ignoredPlayer, hits, out vector2, false))
				{
					return false;
				}
				Vector3 vector3 = vector2 + Vector3.up * 0.4f;
				Vector3 vector4 = vector2 + Vector3.up * 1.45f;
				int num2 = Physics.OverlapCapsuleNonAlloc(vector3, vector4, 0.3f, overlaps, -5, QueryTriggerInteraction.Ignore);
				if (num2 == overlaps.Length)
				{
					return false;
				}
				for (int j = 0; j < num2; j++)
				{
					if (!LongWayHomeWorldRoute.IsIgnoredPlayerCollider(overlaps[j], ignoredPlayer))
					{
						return false;
					}
				}
				if (i > 0)
				{
					if (Mathf.Abs(vector2.y - vector.y) > 0.3f)
					{
						return false;
					}
					Vector3 vector5 = vector2 - vector;
					if (vector5.sqrMagnitude > 0.0001f)
					{
						num2 = Physics.CapsuleCastNonAlloc(vector + Vector3.up * 0.4f, vector + Vector3.up * 1.45f, 0.3f, vector5.normalized, hits, vector5.magnitude, -5, QueryTriggerInteraction.Ignore);
						if (num2 == hits.Length)
						{
							return false;
						}
						for (int k = 0; k < num2; k++)
						{
							if (!LongWayHomeWorldRoute.IsIgnoredPlayerCollider(hits[k].collider, ignoredPlayer))
							{
								return false;
							}
						}
					}
				}
				vector = vector2;
			}
			return true;
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x0027D2FD File Offset: 0x0027B4FD
		private static bool IsIgnoredPlayerCollider(Collider collider, Transform player)
		{
			return collider != null && player != null && collider.transform.IsChildOf(player);
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x0027D320 File Offset: 0x0027B520
		private static bool TryConnectedPosition(Vector3 previous, Vector3 desired, float tolerance, out Vector3 result)
		{
			result = desired;
			AstarPath active = AstarPath.active;
			if (active == null)
			{
				return false;
			}
			NNInfo nearest = active.GetNearest(previous, NearestNodeConstraint.Walkable);
			NNInfo nearest2 = active.GetNearest(desired, NearestNodeConstraint.Walkable);
			if (nearest.node == null || nearest2.node == null || !nearest.node.Walkable || !nearest2.node.Walkable || Vector3.Distance(nearest.position, previous) > 3f || Vector3.Distance(nearest2.position, desired) > tolerance || !PathUtilities.IsPathPossible(nearest.node, nearest2.node))
			{
				return false;
			}
			result = nearest2.position;
			return true;
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x0027D3D0 File Offset: 0x0027B5D0
		private void CollectAuthoredSpawnCandidates(Vector3 street)
		{
			this._candidates.Clear();
			this._alleyGeometryPending = false;
			foreach (ChunkInstance chunkInstance in this._manager.SpawnedChunks.Values)
			{
				if (chunkInstance != null && !(chunkInstance.chunkRoot == null) && this._manager.IsChunkPresentationVisible(chunkInstance) && LongWayHomeWorldRoute.PlanarDistance(chunkInstance.worldPosition, street) <= this._manager.CellSize * 1.6f)
				{
					foreach (LongWayHomeWorldSpawnPoint longWayHomeWorldSpawnPoint in chunkInstance.chunkRoot.transform.GetComponentsInChildren<LongWayHomeWorldSpawnPoint>())
					{
						if (longWayHomeWorldSpawnPoint.isActiveAndEnabled)
						{
							this._candidates.Add(new LongWayHomeWorldRoute.SpawnCandidate(longWayHomeWorldSpawnPoint.transform.position, longWayHomeWorldSpawnPoint.transform.rotation, chunkInstance, longWayHomeWorldSpawnPoint.transform.parent));
						}
					}
					if (!(chunkInstance.chunkData == null) && !(chunkInstance.chunkData.gridSize != Vector2Int.one))
					{
						bool flag = false;
						foreach (GameObject gameObject in chunkInstance.spawnedZones)
						{
							if (!(gameObject == null))
							{
								bool flag2 = LongWayHomeWorldRoute.IsSupportedAlleyPrefabName(gameObject.name);
								bool flag3 = flag2 && this.CouldOfferAlleyInRange(gameObject.transform, street);
								flag = flag || flag2;
								if (!gameObject.activeInHierarchy)
								{
									this._alleyGeometryPending = this._alleyGeometryPending || flag3;
								}
								else
								{
									Transform transform = gameObject.transform;
									IncrementalPooledZoneActivation component = gameObject.GetComponent<IncrementalPooledZoneActivation>();
									HeavyZoneStreamingProxy component2 = gameObject.GetComponent<HeavyZoneStreamingProxy>();
									if ((component != null && !component.IsOperationalReady) || (component2 != null && !component2.IsOperationalReady))
									{
										this._alleyGeometryPending = this._alleyGeometryPending || flag3;
									}
									else if (flag2)
									{
										foreach (Vector3 vector in LongWayHomeWorldRoute.AuthoredAlleyPoses)
										{
											this.AddAlleyCandidate(chunkInstance, transform, vector, 180f);
										}
									}
								}
							}
						}
						if (!flag && this._manager.HasPendingZoneContent(chunkInstance) && this.CellContainsAuthoredAlley(this._manager.WorldToCell(chunkInstance.worldPosition)))
						{
							this._alleyGeometryPending = true;
						}
					}
				}
			}
		}

		// Token: 0x06006391 RID: 25489 RVA: 0x0027D684 File Offset: 0x0027B884
		private void AddAlleyCandidate(ChunkInstance chunk, Transform zone, Vector3 local, float yaw)
		{
			this._candidates.Add(new LongWayHomeWorldRoute.SpawnCandidate(zone.TransformPoint(local), zone.rotation * Quaternion.Euler(0f, yaw, 0f), chunk, zone));
		}

		// Token: 0x06006392 RID: 25490 RVA: 0x0027D6BB File Offset: 0x0027B8BB
		private bool IsInAlleyRange(Vector3 position, Vector3 street)
		{
			return LongWayHomeWorldRoute.IsStartingDistanceNearTarget(position, this.HomePosition, this.StartingDistance, this._manager.CellSize) && LongWayHomeWorldRoute.PlanarDistance(position, street) <= this._manager.CellSize;
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x0027D6F8 File Offset: 0x0027B8F8
		private bool CouldOfferAlleyInRange(Transform zone, Vector3 street)
		{
			foreach (Vector3 vector in LongWayHomeWorldRoute.AuthoredAlleyPoses)
			{
				if (this.IsInAlleyRange(zone.TransformPoint(vector), street))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006394 RID: 25492 RVA: 0x0027D734 File Offset: 0x0027B934
		private bool IsHiddenAlleyPose(Vector3 position, ChunkInstance chunk, Transform zone)
		{
			if (((chunk != null) ? chunk.chunkRoot : null) == null || zone == null || !zone.gameObject.activeInHierarchy)
			{
				return false;
			}
			Transform transform = chunk.chunkRoot.transform;
			Vector3 vector = transform.InverseTransformPoint(position);
			float num = this._manager.CellSize * 0.5f - 2f;
			if (Mathf.Abs(vector.x) > num - 6f || Mathf.Abs(vector.z) > num - 6f)
			{
				return false;
			}
			if (this.HasStaticScreen(position + Vector3.up * 1.8f, position + Vector3.up * 501.8f, new int?(-5)))
			{
				return false;
			}
			Vector3 vector2 = position + Vector3.up * 1.2f;
			bool flag = this.HasStaticScreen(vector2, vector2 + zone.right * 8f, null) && this.HasStaticScreen(vector2, vector2 - zone.right * 8f, null);
			bool flag2 = this.HasStaticScreen(vector2, vector2 + zone.forward * 8f, null) && this.HasStaticScreen(vector2, vector2 - zone.forward * 8f, null);
			if (!flag && !flag2)
			{
				return false;
			}
			ChunkData chunkData = chunk.chunkData;
			bool flag3 = ((chunkData != null) ? chunkData.template : null) != null && chunk.chunkData.template.shape != ChunkShape.MonoShape;
			Vector3 vector3 = transform.InverseTransformPoint(zone.position);
			vector3.y = 0f;
			if (vector3.sqrMagnitude < 0.01f)
			{
				vector3 = transform.InverseTransformDirection(-zone.forward);
			}
			for (int i = 0; i < (flag3 ? 6 : 4); i++)
			{
				for (float num2 = -num; num2 <= num + 0.01f; num2 += 4f)
				{
					ChunkData chunkData2 = chunk.chunkData;
					if (!(((chunkData2 != null) ? chunkData2.template : null) != null) || chunk.chunkData.template.shape != ChunkShape.TShape || !LongWayHomeWorldRoute.IsSupportedAlleyPrefabName(zone.name) || ((i != 4 || Mathf.Abs(vector3.z) <= Mathf.Abs(vector3.x) || num2 * vector3.z <= 0f) && (i != 5 || Mathf.Abs(vector3.x) <= Mathf.Abs(vector3.z) || num2 * vector3.x <= 0f)))
					{
						Vector3 vector4 = ((i < 2) ? new Vector3((i == 0) ? (-num) : num, 0f, num2) : ((i < 4) ? new Vector3(num2, 0f, (i == 2) ? (-num) : num) : ((i == 4) ? new Vector3(0f, 0f, num2) : new Vector3(num2, 0f, 0f))));
						Vector3 vector5 = transform.TransformPoint(vector4);
						vector5.y = position.y + 1.65f;
						if (LongWayHomeWorldRoute.PlanarDistance(vector5, position) <= 16f && (!this.HasStaticScreen(vector5, position + Vector3.up * 1.2f, null) || !this.HasStaticScreen(vector5, position + Vector3.up * 1.65f, null)))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06006395 RID: 25493 RVA: 0x0027DAFC File Offset: 0x0027BCFC
		private bool HasStaticScreen(Vector3 eye, Vector3 target, int? obstacleMask = null)
		{
			Vector3 vector = target - eye;
			SensoryManager instance = SensoryManager.Instance;
			DetectionConfigSO detectionConfigSO = ((instance != null) ? instance.GlobalConfig : null);
			int num = obstacleMask ?? ((detectionConfigSO != null) ? detectionConfigSO.VisionObstacles.value : (-5));
			int num2 = Physics.RaycastNonAlloc(eye, vector.normalized, this._groundHits, vector.magnitude, num, QueryTriggerInteraction.Ignore);
			if (num2 == this._groundHits.Length)
			{
				return false;
			}
			for (int i = 0; i < num2; i++)
			{
				Collider collider = this._groundHits[i].collider;
				if (collider != null && collider.attachedRigidbody == null && !LongWayHomeWorldRoute.IsIgnoredPlayerCollider(collider, this._player) && collider.GetComponentInParent<NPCSenses>(true) == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x0027DBDC File Offset: 0x0027BDDC
		private bool IsVisibleFromAuthoredNpcSpawn(Vector3 position, ChunkInstance chunk)
		{
			if (((chunk != null) ? chunk.chunkRoot : null) == null)
			{
				return true;
			}
			NPCSpawnPoint[] componentsInChildren = chunk.chunkRoot.GetComponentsInChildren<NPCSpawnPoint>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 vector = componentsInChildren[i].transform.position + Vector3.up * 1.65f;
				if (LongWayHomeWorldRoute.PlanarDistance(vector, position) <= 15f && (!this.HasStaticScreen(vector, position + Vector3.up * 1.2f, null) || !this.HasStaticScreen(vector, position + Vector3.up * 1.65f, null)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x0027DCA4 File Offset: 0x0027BEA4
		private bool TryGroundedPose(Vector3 desired, Quaternion rotation, out Vector3 grounded)
		{
			grounded = desired;
			Physics.SyncTransforms();
			RaycastHit raycastHit;
			if (!Physics.Raycast(desired + Vector3.up * 2f, Vector3.down, out raycastHit, 4f, -5, QueryTriggerInteraction.Ignore) || raycastHit.normal.y < 0.85f)
			{
				return false;
			}
			if (raycastHit.rigidbody != null && !raycastHit.rigidbody.isKinematic)
			{
				return false;
			}
			grounded = raycastHit.point + Vector3.up * 0.04f;
			if (this.HasSolidOverlap(grounded + Vector3.up * 0.4f, grounded + Vector3.up * 1.45f, 0.3f))
			{
				return false;
			}
			Vector3 vector = rotation * Vector3.forward * 0.65f;
			return !this.HasSolidOverlap(grounded + Vector3.up * 0.35f - vector, grounded + Vector3.up * 0.35f + vector, 0.25f);
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x0027DDE4 File Offset: 0x0027BFE4
		private bool HasSolidOverlap(Vector3 a, Vector3 b, float radius)
		{
			int num = Physics.OverlapCapsuleNonAlloc(a, b, radius, this._overlaps, -5, QueryTriggerInteraction.Ignore);
			if (num == this._overlaps.Length)
			{
				return true;
			}
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._overlaps[i];
				if (collider != null && (this._player == null || !collider.transform.IsChildOf(this._player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006399 RID: 25497 RVA: 0x0027DE54 File Offset: 0x0027C054
		private bool IsObservedByNearbyNpc(Vector3 position)
		{
			foreach (NPCSenses npcsenses in Object.FindObjectsByType<NPCSenses>(FindObjectsSortMode.None))
			{
				if (npcsenses.isActiveAndEnabled)
				{
					Vector3 eyePosition = npcsenses.EyePosition;
					if ((position + Vector3.up * 1.65f - eyePosition).sqrMagnitude <= Mathf.Pow(Mathf.Max(15f, npcsenses.EffectiveViewRadius), 2f) && (!this.HasStaticScreen(eyePosition, position + Vector3.up * 1.2f, null) || !this.HasStaticScreen(eyePosition, position + Vector3.up * 1.65f, null)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600639A RID: 25498 RVA: 0x0027DF28 File Offset: 0x0027C128
		public static bool TryResolveHomeEntrance(ChunkManager manager, out Vector3 position)
		{
			Vector3 vector;
			return LongWayHomeWorldRoute.TryResolveHomeEntrance(manager, out position, out vector);
		}

		// Token: 0x0600639B RID: 25499 RVA: 0x0027DF40 File Offset: 0x0027C140
		public static bool TryResolveHomeEntrance(ChunkManager manager, out Vector3 position, out Vector3 cardAnchor)
		{
			position = default(Vector3);
			cardAnchor = default(Vector3);
			if (manager == null)
			{
				return false;
			}
			foreach (ChunkInstance chunkInstance in manager.SpawnedChunks.Values)
			{
				if (chunkInstance != null && !(chunkInstance.chunkRoot == null) && !(chunkInstance.chunkData != null) && !(chunkInstance.originCell != Vector2Int.zero) && chunkInstance.chunkRoot.activeInHierarchy && !(chunkInstance.chunkRoot.scene.name != "Game - Current"))
				{
					foreach (Transform transform in chunkInstance.chunkRoot.GetComponentsInChildren<Transform>(true))
					{
						if (transform.name.StartsWith("CielHouse", StringComparison.OrdinalIgnoreCase))
						{
							position = LongWayHomeWorldRoute.ResolveHouseEntrance(transform);
							cardAnchor = LongWayHomeWorldRoute.ResolveHouseDoorCenter(transform) - transform.forward * 0.15f;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600639C RID: 25500 RVA: 0x0027E088 File Offset: 0x0027C288
		public static Vector3 ResolveHouseDoorCenter(Transform house)
		{
			Vector3 vector = house.TransformPoint(new Vector3(-0.4f, 1.71f, 2.58f));
			foreach (Transform transform in house.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform.name != "SM_Door_Front_01"))
				{
					Renderer componentInChildren = transform.GetComponentInChildren<Renderer>(true);
					if (componentInChildren != null)
					{
						return componentInChildren.bounds.center;
					}
				}
			}
			return vector;
		}

		// Token: 0x0600639D RID: 25501 RVA: 0x0027E104 File Offset: 0x0027C304
		public static Vector3 ResolveHouseEntrance(Transform house)
		{
			Vector3 vector = house.TransformPoint(new Vector3(-0.88f, 0.72f, 2.56f));
			foreach (Transform transform in house.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == "SM_Door_Front_01")
				{
					vector = transform.position;
					break;
				}
			}
			return vector - house.forward * 1.35f;
		}

		// Token: 0x0600639E RID: 25502 RVA: 0x0027E178 File Offset: 0x0027C378
		public bool IsAtHome(Vector3 position)
		{
			return this.IsReady && SceneManager.GetActiveScene().name == "Game - Current" && LongWayHomeWorldRoute.IsInsideHomeArrival(position, this.HomePosition);
		}

		// Token: 0x0600639F RID: 25503 RVA: 0x0027E1B4 File Offset: 0x0027C3B4
		public static bool IsInsideHomeArrival(Vector3 position, Vector3 home)
		{
			return Mathf.Abs(position.y - home.y) <= 1.5f && LongWayHomeWorldRoute.PlanarDistance(position, home) <= 2f;
		}

		// Token: 0x060063A0 RID: 25504 RVA: 0x0027E1E4 File Offset: 0x0027C3E4
		public static List<Vector3> BuildStreetCheckpoints(Vector3 home, float cellSize, int blocks, int direction)
		{
			float num = Mathf.Max(1f, cellSize);
			float num2 = num * 0.5f;
			Vector3 vector = new Vector3(Mathf.Round((home.x - num2) / num) * num + num2, 0f, Mathf.Round((home.z - num2) / num) * num + num2);
			Vector3 vector2 = ((direction == 0) ? Vector3.forward : ((direction == 1) ? Vector3.right : ((direction == 2) ? Vector3.back : Vector3.left)));
			List<Vector3> list = new List<Vector3> { vector };
			for (int i = 1; i <= Mathf.Clamp(blocks, 1, 256); i++)
			{
				list.Add(vector + vector2 * num * (float)i);
			}
			return list;
		}

		// Token: 0x060063A1 RID: 25505 RVA: 0x0027E2A4 File Offset: 0x0027C4A4
		public static List<Vector3> BuildStreetCheckpointsToOffset(Vector3 home, float cellSize, Vector2Int offset, bool xFirst)
		{
			int num = Mathf.Abs(offset.x) + Mathf.Abs(offset.y);
			if (num < 1 || num > 256)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			float num2 = Mathf.Max(1f, cellSize);
			return LongWayHomeWorldRoute.BuildCardinalStreetConnection(LongWayHomeWorldRoute.BuildStreetCheckpoints(home, num2, 3, 0)[0], num2, offset, xFirst);
		}

		// Token: 0x060063A2 RID: 25506 RVA: 0x0027E308 File Offset: 0x0027C508
		private static List<Vector3> BuildCardinalStreetConnection(Vector3 from, float cellSize, Vector2Int offset, bool xFirst)
		{
			List<Vector3> list = new List<Vector3>(Mathf.Abs(offset.x) + Mathf.Abs(offset.y) + 1) { from };
			for (int i = 0; i < 2; i++)
			{
				bool flag = i == 0 == xFirst;
				int num = (flag ? offset.x : offset.y);
				Vector3 vector = (flag ? Vector3.right : Vector3.forward) * ((float)Math.Sign(num) * cellSize);
				for (int j = 0; j < Mathf.Abs(num); j++)
				{
					list.Add(list[list.Count - 1] + vector);
				}
			}
			return list;
		}

		// Token: 0x060063A3 RID: 25507 RVA: 0x0027E3B4 File Offset: 0x0027C5B4
		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			a.y = (b.y = 0f);
			return Vector3.Distance(a, b);
		}

		// Token: 0x060063A4 RID: 25508 RVA: 0x0027E3E0 File Offset: 0x0027C5E0
		private void LateUpdate()
		{
			if (this.IsReady && this._ownsFocus && this._player != null && LongWayHomeWorldRoute.PlanarDistance(this._player.position, this.SpawnPosition) < 5f)
			{
				this.ReleaseFocus();
			}
		}

		// Token: 0x060063A5 RID: 25509 RVA: 0x0027E42E File Offset: 0x0027C62E
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
		}

		// Token: 0x060063A6 RID: 25510 RVA: 0x0027E441 File Offset: 0x0027C641
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
		}

		// Token: 0x060063A7 RID: 25511 RVA: 0x0027E454 File Offset: 0x0027C654
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			this.HomePosition += delta;
			this.SpawnPosition += delta;
			this._stepPosition += delta;
			for (int i = 0; i < this._checkpoints.Count; i++)
			{
				List<Vector3> checkpoints = this._checkpoints;
				int num = i;
				checkpoints[num] += delta;
			}
			for (int j = 0; j < this._candidates.Count; j++)
			{
				LongWayHomeWorldRoute.SpawnCandidate spawnCandidate = this._candidates[j];
				spawnCandidate.position += delta;
				this._candidates[j] = spawnCandidate;
			}
		}

		// Token: 0x060063A8 RID: 25512 RVA: 0x0027E514 File Offset: 0x0027C714
		private void Fail(string reason)
		{
			Debug.LogWarning(string.Format("[LongWayHome] Route preparation failed: metadata={0:F2}s, ", this._metadataSeconds) + string.Format("geometry/navigation={0:F2}s, population={1:F2}s; {2}", this._geometrySeconds, this._populationSeconds, reason));
			this.Error = reason;
			this.IsReady = false;
			this.ReleaseFocus();
		}

		// Token: 0x060063A9 RID: 25513 RVA: 0x0027E578 File Offset: 0x0027C778
		private void ReleaseFocus()
		{
			if (this._manager != null)
			{
				this._manager.ReleasePreparationFocus(this);
			}
			if (NPCZoneManager.Instance != null)
			{
				NPCZoneManager.Instance.ReleasePreparationFocus(this);
			}
			if (this._ownsFocus && this._mover != null)
			{
				if (this._mover.target == this._focus)
				{
					this._mover.target = this._originalGraphTarget;
				}
				this._mover.maxRecastTilesPerMove = this._originalGraphTileLimit;
			}
			this._ownsFocus = false;
			if (this._focus != null)
			{
				Object.Destroy(this._focus.gameObject);
			}
			this._focus = null;
		}

		// Token: 0x060063AA RID: 25514 RVA: 0x0027E634 File Offset: 0x0027C834
		public void Clear()
		{
			this.ReleaseFocus();
			this.IsReady = false;
			this.Error = null;
			this.PreparationProgress = 0f;
			this.StartingDistance = 0f;
			this._checkpoints.Clear();
			this._candidates.Clear();
			this._alleyRouteOffsets.Clear();
			this._prefabBaseTransforms.Clear();
			this._alleyGeometryPending = false;
			this._player = null;
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x0027E6A5 File Offset: 0x0027C8A5
		private void OnDestroy()
		{
			this.Clear();
		}

		// Token: 0x060063AC RID: 25516 RVA: 0x0027E6AD File Offset: 0x0027C8AD
		private IEnumerator PreparePoiExit(int seed, ProceduralPoiKind kind, Action<Vector3> onDistrictSelected)
		{
			string scene = LongWayHomeWorldRoute.PoiExitScene(kind);
			if (scene == null)
			{
				this.Fail("No POI opening continues from a " + kind.ToString() + ".");
				yield break;
			}
			if (!this._manager.BeginPreparationFocusSelection(this))
			{
				this.Fail("The city streaming focus is already owned.");
				yield break;
			}
			List<Vector2Int> candidates = new List<Vector2Int>();
			HashSet<Vector2Int> inspected = new HashSet<Vector2Int>();
			Vector3 corner = LongWayHomeWorldRoute.BuildStreetCheckpoints(this.HomePosition, this._manager.CellSize, 1, 0)[0];
			int inspectedCount = 0;
			float metadataStart = Time.realtimeSinceStartup;
			foreach (Vector2Int vector2Int in LongWayHomeWorldRoute.EnumerateDistantAlleyOffsets(this.HomePosition, this._manager.CellSize, seed, this.StartingDistance))
			{
				Vector2Int nearby = this._manager.WorldToCell(corner + new Vector3((float)vector2Int.x, 0f, (float)vector2Int.y) * this._manager.CellSize);
				int x = -3;
				while (x <= 1 && candidates.Count < 12)
				{
					int z = -3;
					int num;
					while (z <= 1 && candidates.Count < 12)
					{
						Vector2Int vector2Int2 = nearby + new Vector2Int(x, z);
						if (inspected.Add(vector2Int2))
						{
							ChunkSpawnResolver.SpawnEntry spawnEntry;
							if (this._manager.TryGetPreparationPoiPreview(this, vector2Int2, kind, out spawnEntry) && LongWayHomeWorldRoute.IsStartingDistanceNearTarget(this._manager.CellToWorld(vector2Int2, spawnEntry.chunkData.gridSize), this.HomePosition, this.StartingDistance, this._manager.CellSize))
							{
								candidates.Add(vector2Int2);
							}
							num = inspectedCount + 1;
							inspectedCount = num;
							if (num % 24 == 0)
							{
								yield return null;
							}
							if (Time.realtimeSinceStartup >= this._deadline)
							{
								break;
							}
						}
						num = z;
						z = num + 1;
					}
					num = x;
					x = num + 1;
				}
				if (candidates.Count >= 12 || Time.realtimeSinceStartup >= this._deadline)
				{
					break;
				}
				nearby = default(Vector2Int);
			}
			IEnumerator<Vector2Int> enumerator = null;
			this._metadataSeconds += Time.realtimeSinceStartup - metadataStart;
			this.PreparationProgress = 0.25f;
			yield return this.PreparePoiExitCandidates(candidates, kind, scene, seed, false, onDistrictSelected);
			if (this.IsReady || this.Error != null)
			{
				yield break;
			}
			List<Vector2Int> nearest = new List<Vector2Int>();
			metadataStart = Time.realtimeSinceStartup;
			yield return this.FindNearestPoiExits(kind, candidates, nearest);
			this._metadataSeconds += Time.realtimeSinceStartup - metadataStart;
			yield return this.PreparePoiExitCandidates(nearest, kind, scene, seed, true, onDistrictSelected);
			if (this.IsReady || this.Error != null)
			{
				yield break;
			}
			this.Fail("No safe exterior entrance of the " + kind.ToString() + " finished loading between 1 and 7 km from home.");
			yield break;
			yield break;
		}

		// Token: 0x060063AD RID: 25517 RVA: 0x0027E6D1 File Offset: 0x0027C8D1
		private IEnumerator PreparePoiExitCandidates(List<Vector2Int> cells, ProceduralPoiKind kind, string scene, int seed, bool nearest, Action<Vector3> onDistrictSelected)
		{
			int i = 0;
			while (i < cells.Count && Time.realtimeSinceStartup < this._deadline)
			{
				ChunkSpawnResolver.SpawnEntry spawnEntry;
				if (this._manager.TryReserveLongWayHomePoiExit(this, cells[i], kind, out spawnEntry))
				{
					Vector3 vector = this._manager.CellToWorld(cells[i], spawnEntry.chunkData.gridSize);
					if (onDistrictSelected != null)
					{
						onDistrictSelected(vector);
					}
					if (!this.SetPoiPreparationFocus(vector))
					{
						this.Fail("The " + kind.ToString() + "'s city population or streaming focus is unavailable.");
						yield break;
					}
					yield return this.PreparePoiDistrict(cells[i], scene, nearest ? LongWayHomeWorldRoute.PlanarDistance(vector, this.HomePosition) : this.StartingDistance);
					if (this.IsReady)
					{
						this.PreparationProgress = 1f;
						Debug.Log(string.Format("[LongWayHome] Prepared real {0} exit {1:0}m ", kind, LongWayHomeWorldRoute.PlanarDistance(this.SpawnPosition, this.HomePosition)) + string.Format("from home at {0}; seed={1}, cell={2}", this.SpawnPosition, seed, cells[i]) + (nearest ? string.Format(", nearest to the planned {0:0}m.", this.StartingDistance) : "."));
						yield break;
					}
				}
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060063AE RID: 25518 RVA: 0x0027E70D File Offset: 0x0027C90D
		private IEnumerator FindNearestPoiExits(ProceduralPoiKind kind, ICollection<Vector2Int> tried, List<Vector2Int> found)
		{
			Vector2Int homeCell = this._manager.WorldToCell(this.HomePosition);
			Vector3 vector = this.HomePosition - this._manager.CellToWorld(homeCell, Vector2Int.one);
			List<float> gaps = new List<float>();
			float num = Time.realtimeSinceStartup;
			foreach (Vector2Int vector2Int in LongWayHomeWorldRoute.OrderCellOffsetsByStartingDistance(vector, this._manager.CellSize, this.StartingDistance))
			{
				Vector2Int vector2Int2 = homeCell + vector2Int;
				ChunkSpawnResolver.SpawnEntry spawnEntry;
				if (!tried.Contains(vector2Int2) && this._manager.TryGetPreparationPoiPreview(this, vector2Int2, kind, out spawnEntry))
				{
					Vector3 vector2 = this._manager.CellToWorld(vector2Int2, spawnEntry.chunkData.gridSize);
					if (LongWayHomeWorldRoute.IsStartingDistanceInRange(vector2, this.HomePosition))
					{
						float num2 = Mathf.Abs(LongWayHomeWorldRoute.PlanarDistance(vector2, this.HomePosition) - this.StartingDistance);
						int num3 = 0;
						while (num3 < gaps.Count && gaps[num3] <= num2)
						{
							num3++;
						}
						gaps.Insert(num3, num2);
						found.Insert(num3, vector2Int2);
						if (found.Count >= 4)
						{
							yield break;
						}
					}
				}
				if (Time.realtimeSinceStartup >= this._deadline)
				{
					yield break;
				}
				if (Time.realtimeSinceStartup - num >= 0.008f)
				{
					yield return null;
					num = Time.realtimeSinceStartup;
				}
			}
			List<Vector2Int>.Enumerator enumerator = default(List<Vector2Int>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060063AF RID: 25519 RVA: 0x0027E734 File Offset: 0x0027C934
		public static List<Vector2Int> OrderCellOffsetsByStartingDistance(Vector3 homeInCell, float cellSize, float startingDistance)
		{
			if (cellSize <= 0f || float.IsNaN(startingDistance) || startingDistance < 1000f || startingDistance > 7000f)
			{
				return new List<Vector2Int>();
			}
			float num = cellSize * 2f;
			int num2 = Mathf.CeilToInt((7000f + num) / cellSize) + 1;
			List<Vector2Int> list = new List<Vector2Int>();
			List<float> list2 = new List<float>();
			for (int i = -num2; i <= num2; i++)
			{
				for (int j = -num2; j <= num2; j++)
				{
					float magnitude = new Vector2((float)i * cellSize - homeInCell.x, (float)j * cellSize - homeInCell.z).magnitude;
					if (magnitude >= 1000f - num && magnitude <= 7000f + num)
					{
						list.Add(new Vector2Int(i, j));
						list2.Add(Mathf.Abs(magnitude - startingDistance));
					}
				}
			}
			Vector2Int[] array = list.ToArray();
			Array.Sort<float, Vector2Int>(list2.ToArray(), array);
			return new List<Vector2Int>(array);
		}

		// Token: 0x060063B0 RID: 25520 RVA: 0x0027E826 File Offset: 0x0027CA26
		private static string PoiExitScene(ProceduralPoiKind kind)
		{
			if (kind == ProceduralPoiKind.Mall)
			{
				return "POI - Mall";
			}
			if (kind == ProceduralPoiKind.University)
			{
				return "POI - University";
			}
			if (kind != ProceduralPoiKind.Stadium)
			{
				return null;
			}
			return "POI - Stadium";
		}

		// Token: 0x060063B1 RID: 25521 RVA: 0x0027E848 File Offset: 0x0027CA48
		private bool SetPoiPreparationFocus(Vector3 position)
		{
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (this._focus == null || instance == null || !this._manager.SetPreparationFocus(this, position) || !instance.TrySetPreparationFocus(this, position))
			{
				return false;
			}
			this._focus.position = position;
			this._mover.target = this._focus;
			this._mover.maxRecastTilesPerMove = 0;
			return true;
		}

		// Token: 0x060063B2 RID: 25522 RVA: 0x0027E8B7 File Offset: 0x0027CAB7
		private IEnumerator PreparePoiDistrict(Vector2Int cell, string scene, float targetDistance)
		{
			float districtDeadline = Mathf.Min(this._deadline, Time.realtimeSinceStartup + (this._manager.IsInitialGenerationComplete ? 30f : 90f));
			float geometryStart = Time.realtimeSinceStartup;
			ChunkInstance chunk = null;
			int stableFrames = 0;
			while (Time.realtimeSinceStartup < districtDeadline)
			{
				chunk = this._manager.GetChunkAt(cell);
				stableFrames = ((this.IsNavigationSettled() && this.IsPoiContentReady(chunk, cell, scene) && this._manager.IsPreparationAreaReady(this._focus.position, 0)) ? (stableFrames + 1) : 0);
				if (stableFrames >= 3)
				{
					break;
				}
				yield return null;
			}
			this._geometrySeconds += Time.realtimeSinceStartup - geometryStart;
			if (stableFrames < 3 || chunk == null)
			{
				yield break;
			}
			foreach (GameObject gameObject in chunk.spawnedZones)
			{
				if (!(gameObject == null))
				{
					PoiScenePortal[] array = gameObject.GetComponentsInChildren<PoiScenePortal>();
					for (int i = 0; i < array.Length; i++)
					{
						Vector3 desired;
						Quaternion rotation;
						if (LongWayHomeWorldRoute.TryResolvePoiExitPose(array[i], scene, out desired, out rotation) && LongWayHomeWorldRoute.IsStartingDistanceNearTarget(desired, this.HomePosition, targetDistance, this._manager.CellSize))
						{
							if (!this.SetPoiPreparationFocus(desired))
							{
								yield break;
							}
							stableFrames = 0;
							float populationStart = Time.realtimeSinceStartup;
							while (Time.realtimeSinceStartup < districtDeadline)
							{
								stableFrames = ((this.IsNavigationSettled() && this._manager.IsPreparationAreaReady(desired, 0) && NPCZoneManager.Instance != null && NPCZoneManager.Instance.IsInitialPopulationComplete) ? (stableFrames + 1) : 0);
								if (stableFrames >= 3)
								{
									break;
								}
								yield return null;
							}
							this._populationSeconds += Time.realtimeSinceStartup - populationStart;
							Vector3 vector;
							if (stableFrames >= 3 && this.IsPoiContentReady(chunk, cell, scene) && LongWayHomeWorldRoute.TryGetWalkingGround(desired, this._player, this._groundHits, out vector, false) && LongWayHomeWorldRoute.IsWalkingSegmentClear(vector, vector, this._player, this._overlaps, this._groundHits) && this.HasPoiStreetConnection(chunk, vector))
							{
								this.SpawnPosition = vector + Vector3.up * 0.04f;
								this.SpawnRotation = rotation;
								this.IsReady = true;
								yield break;
							}
						}
					}
					array = null;
				}
			}
			List<GameObject>.Enumerator enumerator = default(List<GameObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060063B3 RID: 25523 RVA: 0x0027E8DC File Offset: 0x0027CADC
		private bool IsPoiContentReady(ChunkInstance chunk, Vector2Int cell, string scene)
		{
			if (((chunk != null) ? chunk.chunkRoot : null) == null || chunk.originCell != cell || !this._manager.IsChunkPresentationVisible(chunk) || this._manager.HasPendingZoneContent(chunk))
			{
				return false;
			}
			bool flag = false;
			foreach (GameObject gameObject in chunk.spawnedZones)
			{
				if (!(gameObject == null) && gameObject.activeInHierarchy)
				{
					HeavyZoneStreamingProxy component = gameObject.GetComponent<HeavyZoneStreamingProxy>();
					IncrementalPooledZoneActivation component2 = gameObject.GetComponent<IncrementalPooledZoneActivation>();
					if ((component != null && !component.IsOperationalReady) || (component2 != null && !component2.IsOperationalReady))
					{
						return false;
					}
					foreach (PoiScenePortal poiScenePortal in gameObject.GetComponentsInChildren<PoiScenePortal>())
					{
						Vector3 vector;
						Quaternion quaternion;
						flag |= LongWayHomeWorldRoute.TryResolvePoiExitPose(poiScenePortal, scene, out vector, out quaternion);
					}
				}
			}
			return flag;
		}

		// Token: 0x060063B4 RID: 25524 RVA: 0x0027E9F0 File Offset: 0x0027CBF0
		private bool HasPoiStreetConnection(ChunkInstance chunk, Vector3 entrance)
		{
			if (chunk.chunkData == null)
			{
				return false;
			}
			Vector3 position = chunk.chunkRoot.transform.position;
			float num = (float)chunk.chunkData.gridSize.x * this._manager.CellSize * 0.5f - 2f;
			float num2 = (float)chunk.chunkData.gridSize.y * this._manager.CellSize * 0.5f - 2f;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					Vector3 vector;
					if ((i != 0 || j != 0) && LongWayHomeWorldRoute.TryConnectedPosition(entrance, position + new Vector3((float)i * num, 0f, (float)j * num2), 3f, out vector))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060063B5 RID: 25525 RVA: 0x0027EAC0 File Offset: 0x0027CCC0
		public static bool TryResolveMallExitPose(PoiScenePortal portal, out Vector3 position, out Quaternion rotation)
		{
			return LongWayHomeWorldRoute.TryResolvePoiExitPose(portal, "POI - Mall", out position, out rotation);
		}

		// Token: 0x060063B6 RID: 25526 RVA: 0x0027EAD0 File Offset: 0x0027CCD0
		public static bool TryResolvePoiExitPose(PoiScenePortal portal, string scene, out Vector3 position, out Quaternion rotation)
		{
			position = default(Vector3);
			rotation = Quaternion.identity;
			if (portal == null || !portal.isActiveAndEnabled || string.IsNullOrEmpty(scene) || portal.Mode != PoiScenePortal.PortalMode.VisitPoi || portal.DestinationScene != scene || portal.MapReturnPoint == null)
			{
				return false;
			}
			position = portal.MapReturnPoint.position;
			Vector3 vector = position - portal.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.01f)
			{
				return false;
			}
			rotation = Quaternion.LookRotation(vector);
			return true;
		}

		// Token: 0x0400665E RID: 26206
		private const float PreparationTimeout = 150f;

		// Token: 0x0400665F RID: 26207
		private const float CheckpointTimeout = 25f;

		// Token: 0x04006660 RID: 26208
		private const float ArrivalRadius = 2f;

		// Token: 0x04006661 RID: 26209
		private const int MaximumAlleyRouteCandidates = 16;

		// Token: 0x04006662 RID: 26210
		private float _metadataSeconds;

		// Token: 0x04006663 RID: 26211
		private float _geometrySeconds;

		// Token: 0x04006664 RID: 26212
		private float _populationSeconds;

		// Token: 0x04006665 RID: 26213
		public const float MinimumStartingDistance = 1000f;

		// Token: 0x04006666 RID: 26214
		public const float MaximumStartingDistance = 7000f;

		// Token: 0x04006667 RID: 26215
		private static readonly Vector3[] AuthoredAlleyPoses = new Vector3[]
		{
			new Vector3(0f, 0.07f, -10f),
			new Vector3(1f, 0.07f, -11f),
			new Vector3(0.7f, 0.07f, -11.5f)
		};

		// Token: 0x0400666F RID: 26223
		private readonly List<Vector3> _checkpoints = new List<Vector3>();

		// Token: 0x04006670 RID: 26224
		private readonly List<LongWayHomeWorldRoute.SpawnCandidate> _candidates = new List<LongWayHomeWorldRoute.SpawnCandidate>();

		// Token: 0x04006671 RID: 26225
		private readonly List<Vector2Int> _alleyRouteOffsets = new List<Vector2Int>();

		// Token: 0x04006672 RID: 26226
		private readonly Dictionary<GameObject, Transform[]> _prefabBaseTransforms = new Dictionary<GameObject, Transform[]>();

		// Token: 0x04006673 RID: 26227
		private readonly Collider[] _overlaps = new Collider[32];

		// Token: 0x04006674 RID: 26228
		private readonly RaycastHit[] _groundHits = new RaycastHit[32];

		// Token: 0x04006675 RID: 26229
		private ChunkManager _manager;

		// Token: 0x04006676 RID: 26230
		private Transform _player;

		// Token: 0x04006677 RID: 26231
		private Transform _focus;

		// Token: 0x04006678 RID: 26232
		private ProceduralGraphMover _mover;

		// Token: 0x04006679 RID: 26233
		private Transform _originalGraphTarget;

		// Token: 0x0400667A RID: 26234
		private int _originalGraphTileLimit;

		// Token: 0x0400667B RID: 26235
		private bool _ownsFocus;

		// Token: 0x0400667C RID: 26236
		private float _deadline;

		// Token: 0x0400667D RID: 26237
		private bool _stepReady;

		// Token: 0x0400667E RID: 26238
		private Vector3 _stepPosition;

		// Token: 0x0400667F RID: 26239
		private bool _alleyGeometryPending;

		// Token: 0x04006680 RID: 26240
		private const int MaximumNearestPoiExits = 4;

		// Token: 0x02000D41 RID: 3393
		private struct SpawnCandidate
		{
			// Token: 0x060063B9 RID: 25529 RVA: 0x0027EC45 File Offset: 0x0027CE45
			public SpawnCandidate(Vector3 position, Quaternion rotation, ChunkInstance chunk, Transform zone)
			{
				this.position = position;
				this.rotation = rotation;
				this.chunk = chunk;
				this.zone = zone;
			}

			// Token: 0x04006681 RID: 26241
			public Vector3 position;

			// Token: 0x04006682 RID: 26242
			public Quaternion rotation;

			// Token: 0x04006683 RID: 26243
			public ChunkInstance chunk;

			// Token: 0x04006684 RID: 26244
			public Transform zone;
		}
	}
}
