using System;
using System.Collections;
using System.Collections.Generic;
using Features.RunModes;
using Pathfinding;
using Pathfinding.RVO;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000024 RID: 36
public class AStarGraphManager : MonoBehaviour
{
	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000133 RID: 307 RVA: 0x0000957A File Offset: 0x0000777A
	// (set) Token: 0x06000134 RID: 308 RVA: 0x00009581 File Offset: 0x00007781
	public static AStarGraphManager Instance { get; private set; }

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000135 RID: 309 RVA: 0x00009589 File Offset: 0x00007789
	// (set) Token: 0x06000136 RID: 310 RVA: 0x00009591 File Offset: 0x00007791
	public bool IsWaitingForPreparationFocus { get; private set; }

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000137 RID: 311 RVA: 0x0000959A File Offset: 0x0000779A
	// (set) Token: 0x06000138 RID: 312 RVA: 0x000095A2 File Offset: 0x000077A2
	public int CoalescedUpdateRequestCount { get; private set; }

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000139 RID: 313 RVA: 0x000095AB File Offset: 0x000077AB
	// (set) Token: 0x0600013A RID: 314 RVA: 0x000095B3 File Offset: 0x000077B3
	public int CoalescedUpdateFlushCount { get; private set; }

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600013B RID: 315 RVA: 0x000095BC File Offset: 0x000077BC
	public int PendingZoneUpdateCount
	{
		get
		{
			return this._pendingZoneUpdateBounds.Count;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600013C RID: 316 RVA: 0x000095C9 File Offset: 0x000077C9
	// (set) Token: 0x0600013D RID: 317 RVA: 0x000095D1 File Offset: 0x000077D1
	public int PeakPendingZoneUpdateCount { get; private set; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600013E RID: 318 RVA: 0x000095DA File Offset: 0x000077DA
	// (set) Token: 0x0600013F RID: 319 RVA: 0x000095E2 File Offset: 0x000077E2
	public int StaleZoneUpdateDropCount { get; private set; }

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000140 RID: 320 RVA: 0x000095EB File Offset: 0x000077EB
	// (set) Token: 0x06000141 RID: 321 RVA: 0x000095F3 File Offset: 0x000077F3
	public int CapacityZoneUpdateDropCount { get; private set; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000142 RID: 322 RVA: 0x000095FC File Offset: 0x000077FC
	public bool IsReady
	{
		get
		{
			return this._scanComplete && !this.IsNavigationSuspended && this._astarPath != null && this._astarPath.data.graphs.Length != 0;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000143 RID: 323 RVA: 0x00009634 File Offset: 0x00007834
	public bool CanShiftWorldOrigin
	{
		get
		{
			AstarPath astarPath = ((this._astarPath != null) ? this._astarPath : AstarPath.active);
			return (astarPath == null || !astarPath.IsNavigationSuspended) && (this._graphMover == null || !this._graphMover.updatingGraph) && (astarPath == null || (!astarPath.isScanning && !astarPath.IsAnyWorkItemInProgress && !astarPath.IsAnyGraphUpdateQueued && !astarPath.IsAnyGraphUpdateInProgress));
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x000096B9 File Offset: 0x000078B9
	private void Awake()
	{
		if (AStarGraphManager.Instance != null && AStarGraphManager.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		AStarGraphManager.Instance = this;
		PlayerRegistry.PlayerChanged += this.OnRegisteredPlayerChanged;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x000096F8 File Offset: 0x000078F8
	private void OnDestroy()
	{
		PlayerRegistry.PlayerChanged -= this.OnRegisteredPlayerChanged;
		IDisposable scanNavigationPause = this._scanNavigationPause;
		if (scanNavigationPause != null)
		{
			scanNavigationPause.Dispose();
		}
		this._scanNavigationPause = null;
		this._navigationPauseCount = 0;
		this.RestoreNavigationState();
		if (AStarGraphManager.Instance == this)
		{
			AStarGraphManager.Instance = null;
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000974E File Offset: 0x0000794E
	private void Start()
	{
		if (string.IsNullOrEmpty(this._initializedForScene))
		{
			this.Initialize();
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00009764 File Offset: 0x00007964
	public void ApplyWorldOriginShift(Vector3 delta)
	{
		for (int i = 0; i < this._pendingZoneUpdateBounds.Count; i++)
		{
			Bounds bounds = this._pendingZoneUpdateBounds[i];
			bounds.center += delta;
			this._pendingZoneUpdateBounds[i] = bounds;
		}
		AstarPath astar = ((this._astarPath != null) ? this._astarPath : AstarPath.active);
		if (astar == null || astar.data == null || astar.data.graphs == null)
		{
			return;
		}
		astar.AddWorkItem(new AstarWorkItem(delegate
		{
			NavGraph[] graphs = astar.data.graphs;
			for (int j = 0; j < graphs.Length; j++)
			{
				AStarGraphManager.RelocateGraphForWorldOrigin(graphs[j], delta);
			}
		}, null));
		astar.FlushWorkItems();
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00009840 File Offset: 0x00007A40
	private static void RelocateGraphForWorldOrigin(NavGraph graph, Vector3 delta)
	{
		RecastGraph recastGraph = graph as RecastGraph;
		if (recastGraph != null)
		{
			recastGraph.forcedBoundsCenter += delta;
			if (recastGraph.isScanned)
			{
				recastGraph.RelocateNodesForWorldOrigin(delta);
				return;
			}
		}
		else
		{
			NavMeshGraph navMeshGraph = graph as NavMeshGraph;
			if (navMeshGraph != null)
			{
				navMeshGraph.offset += delta;
				if (navMeshGraph.isScanned)
				{
					navMeshGraph.RelocateNodesForWorldOrigin(delta);
					return;
				}
			}
			else
			{
				GridGraph gridGraph = graph as GridGraph;
				if (gridGraph != null)
				{
					Vector3 vector = gridGraph.center + delta;
					if (gridGraph.isScanned)
					{
						gridGraph.RelocateNodes(vector, Quaternion.Euler(gridGraph.rotation), gridGraph.nodeSize, gridGraph.aspectRatio, gridGraph.isometricAngle);
						return;
					}
					gridGraph.center = vector;
					return;
				}
				else if (graph != null && graph.isScanned)
				{
					graph.RelocateNodes(Matrix4x4.Translate(delta));
				}
			}
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000990C File Offset: 0x00007B0C
	public void Initialize()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this._scanNavigationPause != null || (this._astarPath != null && this._astarPath.isScanning))
		{
			this._pendingReinitializeScene = SceneManager.GetActiveScene().name;
			return;
		}
		IDisposable disposable = this.AcquireNavigationPause();
		this._scanNavigationPause = disposable;
		try
		{
			this._initializedForScene = SceneManager.GetActiveScene().name;
			this._scanComplete = false;
			this._hasPendingZoneUpdate = false;
			this._pendingZoneUpdateBounds.Clear();
			this.SuspendNavmeshCutting();
			this.SetupAstarPath();
			this.SuspendNavmeshCutting();
			if (!this.disableGraphMover)
			{
				this.SetupProceduralGraphMover();
				this.PauseGraphMover();
			}
			else
			{
				Debug.LogWarning("[AStarGraphManager] ProceduralGraphMover DISABLED via diag flag");
			}
			if (this.enableLocalAvoidance && !this.disableRVOSimulator)
			{
				this.SetupRVOSimulator();
			}
			else if (this.disableRVOSimulator)
			{
				Debug.LogWarning("[AStarGraphManager] RVOSimulator DISABLED via diag flag");
			}
			Coroutine coroutine = base.StartCoroutine(this.ScanRoutine());
			if (this._scanNavigationPause == disposable)
			{
				this._scanCoroutine = coroutine;
			}
		}
		catch
		{
			if (this._scanNavigationPause == disposable)
			{
				this._scanNavigationPause = null;
			}
			disposable.Dispose();
			throw;
		}
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00009A38 File Offset: 0x00007C38
	public void ReinitializeForCurrentScene()
	{
		string name = SceneManager.GetActiveScene().name;
		if (this._initializedForScene == name && (this._scanCoroutine != null || (this._astarPath != null && this._astarPath.isScanning) || this.IsReady))
		{
			this._pendingReinitializeScene = name;
			return;
		}
		if (this._astarPath != null && this._astarPath.isScanning)
		{
			this._pendingReinitializeScene = name;
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarGraphManager] Scan in corso; reinitialize accodato per '" + name + "'.");
			}
			return;
		}
		if (this._scanCoroutine != null)
		{
			base.StopCoroutine(this._scanCoroutine);
			this._scanCoroutine = null;
		}
		this.IsWaitingForPreparationFocus = false;
		this._pendingReinitializeScene = null;
		this.Initialize();
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009B03 File Offset: 0x00007D03
	private IEnumerator ScanRoutine()
	{
		string rescanScene = null;
		try
		{
			if (this.ShouldAwaitPreparationFocus())
			{
				this.IsWaitingForPreparationFocus = true;
				float focusDeadline = Time.realtimeSinceStartup + 120f;
				while (base.isActiveAndEnabled && this.ShouldAwaitPreparationFocus() && Time.realtimeSinceStartup < focusDeadline)
				{
					ChunkManager instance = ChunkManager.Instance;
					Vector3 vector;
					if (instance != null && instance.TryGetPreparationFocus(out vector))
					{
						if (this._astarPath.data.graphs.Length == 0)
						{
							break;
						}
						RecastGraph recastGraph = this._astarPath.data.graphs[0] as RecastGraph;
						if (recastGraph != null)
						{
							recastGraph.forcedBoundsCenter = vector;
							break;
						}
						break;
					}
					else
					{
						yield return null;
					}
				}
				if (Time.realtimeSinceStartup >= focusDeadline)
				{
					Debug.LogWarning("[AStarGraphManager] Destination selection timed out; resuming ordinary scan.");
				}
				this.IsWaitingForPreparationFocus = false;
			}
			if (!base.isActiveAndEnabled || this._astarPath == null || AstarPath.active != this._astarPath)
			{
				yield break;
			}
			Physics.SyncTransforms();
			this.RestoreCuttingForScan();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarGraphManager] Avvio scansione asincrona...");
			}
			foreach (Progress progress in this._astarPath.ScanAsync(null))
			{
				yield return null;
			}
			IEnumerator<Progress> enumerator = null;
			if (!string.IsNullOrEmpty(this._pendingReinitializeScene) && this._pendingReinitializeScene != this._initializedForScene)
			{
				rescanScene = this._pendingReinitializeScene;
				this._pendingReinitializeScene = null;
			}
			else
			{
				if (!base.isActiveAndEnabled || this._astarPath == null || AstarPath.active != this._astarPath)
				{
					yield break;
				}
				RecastGraph recastGraph2 = ((this._astarPath.data.graphs != null && this._astarPath.data.graphs.Length != 0) ? (this._astarPath.data.graphs[0] as RecastGraph) : null);
				if (recastGraph2 != null)
				{
					foreach (object obj in this.RecoverPostScanGeometry(recastGraph2))
					{
						yield return null;
					}
					IEnumerator<object> enumerator2 = null;
				}
				if (base.isActiveAndEnabled && this._astarPath != null && AstarPath.active == this._astarPath)
				{
					this._astarPath.navmeshUpdates.ForceUpdate();
					this._astarPath.FlushWorkItems();
					this._scanComplete = true;
					if (this.enableDebugLogs)
					{
						Debug.Log("[AStarGraphManager] Scansione asincrona completata!");
					}
				}
			}
		}
		finally
		{
			this.IsWaitingForPreparationFocus = false;
			this._scanCoroutine = null;
			IDisposable scanNavigationPause = this._scanNavigationPause;
			this._scanNavigationPause = null;
			try
			{
				if (!string.IsNullOrEmpty(rescanScene) && base.isActiveAndEnabled && SceneManager.GetActiveScene().name == rescanScene)
				{
					this.Initialize();
				}
			}
			finally
			{
				if (scanNavigationPause != null)
				{
					scanNavigationPause.Dispose();
				}
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00009B12 File Offset: 0x00007D12
	private IEnumerable<object> RecoverPostScanGeometry(RecastGraph graph)
	{
		List<Bounds> recoveryBounds = new List<Bounds>
		{
			new Bounds(graph.forcedBoundsCenter, graph.forcedBoundsSize)
		};
		this._collectRecoveryZoneUpdates = true;
		try
		{
			while (base.isActiveAndEnabled && this._astarPath != null && AstarPath.active == this._astarPath)
			{
				Physics.SyncTransforms();
				using (List<Bounds>.Enumerator enumerator = recoveryBounds.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Bounds bounds = enumerator.Current;
						this._astarPath.UpdateGraphs(new GraphUpdateObject(bounds)
						{
							updatePhysics = true
						});
					}
					goto IL_00D1;
				}
				goto IL_00B4;
				IL_00D1:
				if (!(this._astarPath != null) || !(AstarPath.active == this._astarPath) || (!this._astarPath.IsAnyGraphUpdateQueued && !this._astarPath.IsAnyGraphUpdateInProgress && !this._astarPath.IsAnyWorkItemInProgress))
				{
					if (!base.isActiveAndEnabled || this._astarPath == null || AstarPath.active != this._astarPath)
					{
						yield break;
					}
					this.PruneStaleZoneUpdateBounds();
					if (this._pendingZoneUpdateBounds.Count == 0)
					{
						yield break;
					}
					recoveryBounds.Clear();
					recoveryBounds.AddRange(this._pendingZoneUpdateBounds);
					this._pendingZoneUpdateBounds.Clear();
					this._hasPendingZoneUpdate = false;
					continue;
				}
				IL_00B4:
				yield return null;
				goto IL_00D1;
			}
		}
		finally
		{
			this._collectRecoveryZoneUpdates = false;
		}
		yield break;
		yield break;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00009B2C File Offset: 0x00007D2C
	private bool ShouldAwaitPreparationFocus()
	{
		if (!RunModeService.IsLongWayHome || this._initializedForScene != "Game - Current" || SceneManager.GetActiveScene().name != "Game - Current")
		{
			return false;
		}
		LongWayHomeRuntimeController instance = LongWayHomeRuntimeController.Instance;
		return instance == null || instance.Phase == LongWayHomeRuntimeController.RunPhase.Idle || instance.Phase == LongWayHomeRuntimeController.RunPhase.Preparing;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00009B90 File Offset: 0x00007D90
	private void SetupAstarPath()
	{
		this._astarPath = Object.FindObjectOfType<AstarPath>();
		if (this._astarPath == null)
		{
			GameObject gameObject = new GameObject("A* Pathfinding");
			this._astarPath = gameObject.AddComponent<AstarPath>();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarGraphManager] Creato nuovo AstarPath");
			}
		}
		this.MoveRuntimeRootToCore(this._astarPath.gameObject);
		this._astarPath.logPathResults = PathLog.None;
		this._astarPath.showNavGraphs = this.showGraphGizmos;
		RecastGraph recastGraph = null;
		if (this._astarPath.data.graphs != null && this._astarPath.data.graphs.Length != 0)
		{
			recastGraph = this._astarPath.data.graphs[0] as RecastGraph;
		}
		if (recastGraph == null)
		{
			recastGraph = this._astarPath.data.AddGraph(typeof(RecastGraph)) as RecastGraph;
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarGraphManager] Creato nuovo RecastGraph");
			}
		}
		this.ConfigureRecastGraph(recastGraph);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00009C88 File Offset: 0x00007E88
	private void ConfigureRecastGraph(RecastGraph graph)
	{
		if (graph == null)
		{
			return;
		}
		graph.forcedBoundsSize = new Vector3(this.graphSize, 100f, this.graphSize);
		graph.useTiles = true;
		graph.editorTileSize = this.tileSize;
		graph.cellSize = this.cellSize;
		graph.walkableHeight = this.agentHeight;
		graph.walkableClimb = this.maxClimb;
		graph.maxSlope = this.maxSlope;
		graph.characterRadius = this.agentRadius;
		graph.mask = this.includeLayers;
		graph.rasterizeColliders = this.rasterizeColliders;
		graph.rasterizeMeshes = this.rasterizeMeshes;
		graph.rasterizeTerrain = this.rasterizeTerrain;
		PlayerProximityProvider instance = PlayerProximityProvider.Instance;
		Transform transform = ((instance != null) ? instance.Player : null);
		if (transform != null)
		{
			graph.forcedBoundsCenter = transform.position;
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[AStarGraphManager] RecastGraph configurato: Size={0}, TileSize={1}", this.graphSize, this.tileSize));
		}
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00009D88 File Offset: 0x00007F88
	private void SetupProceduralGraphMover()
	{
		this._graphMover = Object.FindObjectOfType<ProceduralGraphMover>();
		if (this._graphMover == null)
		{
			GameObject gameObject = new GameObject("ProceduralGraphMover");
			this._graphMover = gameObject.AddComponent<ProceduralGraphMover>();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarGraphManager] Creato ProceduralGraphMover");
			}
		}
		this.MoveRuntimeRootToCore(this._graphMover.gameObject);
		this._graphMover.maxRecastTilesPerMove = Mathf.Max(0, this.maxRecastTilesPerMove);
		NavGraph[] array = ((this._astarPath != null) ? this._astarPath.data.graphs : null);
		if (array == null || array.Length == 0)
		{
			this._graphMover.graph = null;
		}
		else if (this._graphMover.graph == null || Array.IndexOf<NavGraph>(array, this._graphMover.graph) < 0)
		{
			this._graphMover.graph = array[Mathf.Clamp(this._graphMover.graphIndex, 0, array.Length - 1)];
		}
		Transform transform = AStarGraphManager.ResolveGraphMoverTarget();
		this._graphMover.BindTarget(transform);
		if (transform != null && this.enableDebugLogs)
		{
			Debug.Log("[AStarGraphManager] ProceduralGraphMover target: " + transform.name);
		}
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00009EB4 File Offset: 0x000080B4
	private static Transform ResolveGraphMoverTarget()
	{
		Transform player = PlayerRegistry.Player;
		if (player != null)
		{
			return player;
		}
		if (!(PlayerProximityProvider.Instance != null))
		{
			return null;
		}
		return PlayerProximityProvider.Instance.Player;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00009EEB File Offset: 0x000080EB
	private void OnRegisteredPlayerChanged(Transform player)
	{
		if (this._graphMover != null && !this.IsNavigationSuspended)
		{
			this._graphMover.BindTarget(player);
		}
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00009F10 File Offset: 0x00008110
	private void SetupRVOSimulator()
	{
		this._rvoSimulator = Object.FindObjectOfType<RVOSimulator>();
		if (this._rvoSimulator == null)
		{
			GameObject gameObject = new GameObject("RVO Simulator");
			this._rvoSimulator = gameObject.AddComponent<RVOSimulator>();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarGraphManager] Creato RVOSimulator");
			}
		}
		this.MoveRuntimeRootToCore(this._rvoSimulator.gameObject);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00009F70 File Offset: 0x00008170
	private void MoveRuntimeRootToCore(GameObject target)
	{
		if (target == null)
		{
			return;
		}
		GameObject gameObject = target.transform.root.gameObject;
		Scene scene = base.gameObject.scene;
		if (scene.IsValid() && scene.isLoaded && gameObject.scene != scene)
		{
			SceneManager.MoveGameObjectToScene(gameObject, scene);
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00009FCC File Offset: 0x000081CC
	public void RequestRescan()
	{
		if (this._rescanPending)
		{
			return;
		}
		float num = Time.time - this._lastRescanTime;
		if (num < this.minRescanInterval)
		{
			if (this._rescanCoroutine != null)
			{
				base.StopCoroutine(this._rescanCoroutine);
			}
			this._rescanCoroutine = base.StartCoroutine(this.DelayedRescan(this.minRescanInterval - num + this.rescanDelay));
			return;
		}
		if (this._rescanCoroutine != null)
		{
			base.StopCoroutine(this._rescanCoroutine);
		}
		this._rescanCoroutine = base.StartCoroutine(this.DelayedRescan(this.rescanDelay));
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000A05A File Offset: 0x0000825A
	private IEnumerator DelayedRescan()
	{
		this._rescanPending = true;
		yield return new WaitForSeconds(this.rescanDelay);
		this.PerformRescan();
		this._rescanPending = false;
		yield break;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000A069 File Offset: 0x00008269
	private IEnumerator DelayedRescan(float delay)
	{
		this._rescanPending = true;
		yield return new WaitForSeconds(delay);
		this.PerformRescan();
		this._rescanPending = false;
		yield break;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000A080 File Offset: 0x00008280
	private void PerformRescan()
	{
		if (this._astarPath == null || !base.isActiveAndEnabled || AstarPath.active == null)
		{
			return;
		}
		this._lastRescanTime = Time.time;
		this._graphMover != null;
		if (this.enableDebugLogs)
		{
			Debug.Log("[AStarGraphManager] Graph update triggered");
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000A0DC File Offset: 0x000082DC
	public void UpdateGraphInBounds(Bounds bounds)
	{
		if (this._astarPath == null)
		{
			return;
		}
		if (!this._scanComplete || this.IsNavigationSuspended)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[AStarGraphManager] Update grafo IGNORATO (scan iniziale in corso) per bounds {0}.", bounds.center));
			}
			return;
		}
		GraphUpdateObject graphUpdateObject = new GraphUpdateObject(bounds);
		graphUpdateObject.updatePhysics = true;
		AstarPath.active.UpdateGraphs(graphUpdateObject);
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[AStarGraphManager] Graph updated in bounds: {0}, size: {1}", bounds.center, bounds.size));
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000A174 File Offset: 0x00008374
	public void OnChunkSpawned(Vector3 position, Vector3 size)
	{
		Bounds bounds = new Bounds(position + Vector3.up * (size.y * 0.5f), size);
		this.QueueCoalescedZoneUpdate(bounds);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000A1AC File Offset: 0x000083AC
	public void OnChunkZonesReady(Vector3 position, Vector3 size)
	{
		if (this._astarPath == null || !base.isActiveAndEnabled)
		{
			return;
		}
		Bounds bounds = new Bounds(position + Vector3.up * (size.y * 0.5f), size);
		this.QueueCoalescedZoneUpdate(bounds);
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[AStarGraphManager] Zone pronte: bounds accodato su {0} (size {1}).", bounds.center, bounds.size));
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000A22C File Offset: 0x0000842C
	private void QueueCoalescedZoneUpdate(Bounds bounds)
	{
		if (this._astarPath == null || !base.isActiveAndEnabled || ((!this._scanComplete || this.IsNavigationSuspended) && !this._collectRecoveryZoneUpdates))
		{
			return;
		}
		this.PruneStaleZoneUpdateBounds();
		int num;
		if (!this.IntersectsCurrentGraphXZ(bounds))
		{
			num = this.StaleZoneUpdateDropCount;
			this.StaleZoneUpdateDropCount = num + 1;
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (!this._hasPendingZoneUpdate || this._pendingZoneUpdateBounds.Count == 0)
		{
			this._pendingZoneUpdateFirstAt = realtimeSinceStartup;
			this._hasPendingZoneUpdate = true;
		}
		bool flag = false;
		for (int i = 0; i < this._pendingZoneUpdateBounds.Count; i++)
		{
			Bounds bounds2 = this._pendingZoneUpdateBounds[i];
			if ((bounds2.center - bounds.center).sqrMagnitude < 0.01f && (bounds2.size - bounds.size).sqrMagnitude < 0.01f)
			{
				this._pendingZoneUpdateBounds[i] = bounds;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			while (this._pendingZoneUpdateBounds.Count >= 64)
			{
				this._pendingZoneUpdateBounds.RemoveAt(0);
				num = this.CapacityZoneUpdateDropCount;
				this.CapacityZoneUpdateDropCount = num + 1;
			}
			this._pendingZoneUpdateBounds.Add(bounds);
			this.PeakPendingZoneUpdateCount = Mathf.Max(this.PeakPendingZoneUpdateCount, this._pendingZoneUpdateBounds.Count);
		}
		this._pendingZoneUpdateLastAt = realtimeSinceStartup;
		num = this.CoalescedUpdateRequestCount;
		this.CoalescedUpdateRequestCount = num + 1;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000A3A0 File Offset: 0x000085A0
	private void LateUpdate()
	{
		if (!this._hasPendingZoneUpdate || !this._scanComplete || this.IsNavigationSuspended || this._astarPath == null)
		{
			return;
		}
		this.PruneStaleZoneUpdateBounds();
		if (this._pendingZoneUpdateBounds.Count == 0)
		{
			this._hasPendingZoneUpdate = false;
			return;
		}
		if (AstarPath.active == null)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		bool flag = realtimeSinceStartup - this._pendingZoneUpdateLastAt >= this.zoneUpdateCoalesceSeconds;
		bool flag2 = realtimeSinceStartup - this._pendingZoneUpdateFirstAt >= this.zoneUpdateMaxLatency;
		if (!flag && !flag2)
		{
			return;
		}
		if ((this._graphMover != null && (this._graphMover.updatingGraph || this._graphMover.HasPendingRecastShift)) || this._astarPath.IsAnyWorkItemInProgress || this._astarPath.IsAnyGraphUpdateQueued || this._astarPath.IsAnyGraphUpdateInProgress)
		{
			return;
		}
		int num = this._pendingZoneUpdateBounds.Count - 1;
		Bounds bounds = this._pendingZoneUpdateBounds[num];
		this._pendingZoneUpdateBounds.RemoveAt(num);
		this._hasPendingZoneUpdate = this._pendingZoneUpdateBounds.Count > 0;
		using (AStarGraphManager.QueueMergedUpdateMarker.Auto())
		{
			this.UpdateGraphInBounds(bounds);
		}
		int coalescedUpdateFlushCount = this.CoalescedUpdateFlushCount;
		this.CoalescedUpdateFlushCount = coalescedUpdateFlushCount + 1;
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[AStarGraphManager] Flush update bounded #{0}: ", this.CoalescedUpdateFlushCount) + string.Format("{0}, size {1}, pending={2}.", bounds.center, bounds.size, this._pendingZoneUpdateBounds.Count));
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000A560 File Offset: 0x00008760
	private void PruneStaleZoneUpdateBounds()
	{
		for (int i = this._pendingZoneUpdateBounds.Count - 1; i >= 0; i--)
		{
			if (!this.IntersectsCurrentGraphXZ(this._pendingZoneUpdateBounds[i]))
			{
				this._pendingZoneUpdateBounds.RemoveAt(i);
				int staleZoneUpdateDropCount = this.StaleZoneUpdateDropCount;
				this.StaleZoneUpdateDropCount = staleZoneUpdateDropCount + 1;
			}
		}
		if (this._pendingZoneUpdateBounds.Count == 0)
		{
			this._hasPendingZoneUpdate = false;
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000A5CC File Offset: 0x000087CC
	private bool IntersectsCurrentGraphXZ(Bounds candidate)
	{
		RecastGraph recastGraph = ((this._graphMover != null) ? (this._graphMover.graph as RecastGraph) : null);
		if (recastGraph == null)
		{
			AstarPath astarPath = this._astarPath;
			bool flag;
			if (astarPath == null)
			{
				flag = null != null;
			}
			else
			{
				AstarData data = astarPath.data;
				flag = ((data != null) ? data.graphs : null) != null;
			}
			if (flag)
			{
				NavGraph[] graphs = this._astarPath.data.graphs;
				for (int i = 0; i < graphs.Length; i++)
				{
					recastGraph = graphs[i] as RecastGraph;
					if (recastGraph != null)
					{
						break;
					}
				}
			}
		}
		if (recastGraph == null)
		{
			return true;
		}
		Bounds bounds = new Bounds(recastGraph.forcedBoundsCenter, recastGraph.forcedBoundsSize);
		return candidate.max.x >= bounds.min.x && candidate.min.x <= bounds.max.x && candidate.max.z >= bounds.min.z && candidate.min.z <= bounds.max.z;
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000160 RID: 352 RVA: 0x0000A6D0 File Offset: 0x000088D0
	public bool IsNavigationSuspended
	{
		get
		{
			return this._navigationPauseCount > 0 || this._navigationPausedMover != null || this._navigationCutStates.Count > 0 || (this._astarPath != null && this._astarPath.IsNavigationSuspended);
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000161 RID: 353 RVA: 0x0000A71F File Offset: 0x0000891F
	public bool IsNavigationInitializing
	{
		get
		{
			return this._scanCoroutine != null || (this._astarPath != null && this._astarPath.isScanning);
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000A748 File Offset: 0x00008948
	public IDisposable SuspendForSceneTransition()
	{
		IDisposable disposable = this.AcquireNavigationPause();
		if (this._rescanCoroutine != null)
		{
			base.StopCoroutine(this._rescanCoroutine);
		}
		this._rescanCoroutine = null;
		this._rescanPending = false;
		this._hasPendingZoneUpdate = false;
		this._pendingZoneUpdateBounds.Clear();
		if (this._graphMover != null)
		{
			this._graphMover.BindTarget(null);
		}
		return disposable;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000A7AC File Offset: 0x000089AC
	public void RetireForSceneUnload()
	{
		base.enabled = false;
		if (this._graphMover != null)
		{
			this._graphMover.BindTarget(null);
			this._graphMover.enabled = false;
		}
		AstarPath astarPath = ((this._astarPath != null) ? this._astarPath : AstarPath.active);
		if (astarPath != null && AstarPath.active == astarPath)
		{
			astarPath.enabled = false;
		}
		if (this._scanCoroutine != null)
		{
			base.StopCoroutine(this._scanCoroutine);
		}
		this._scanCoroutine = null;
		IDisposable scanNavigationPause = this._scanNavigationPause;
		if (scanNavigationPause != null)
		{
			scanNavigationPause.Dispose();
		}
		this._scanNavigationPause = null;
		if (this._rescanCoroutine != null)
		{
			base.StopCoroutine(this._rescanCoroutine);
		}
		this._rescanCoroutine = null;
		this._rescanPending = false;
		this._navigationPauseCount = 0;
		this._navigationPausedMover = null;
		this._navigationCutStates.Clear();
		this._hasPendingZoneUpdate = false;
		this._pendingZoneUpdateBounds.Clear();
		this._pendingReinitializeScene = null;
		this._scanComplete = false;
		this._collectRecoveryZoneUpdates = false;
		this.IsWaitingForPreparationFocus = false;
		this._astarPath = null;
		this._graphMover = null;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000A8CC File Offset: 0x00008ACC
	private IDisposable AcquireNavigationPause()
	{
		int navigationPauseCount = this._navigationPauseCount;
		this._navigationPauseCount = navigationPauseCount + 1;
		if (navigationPauseCount == 0)
		{
			try
			{
				if (this._astarPath == null)
				{
					this._astarPath = AstarPath.active;
				}
				this.PauseGraphMover();
				if (this._astarPath != null && AstarPath.active == this._astarPath)
				{
					this._astarPath.FlushGraphUpdates();
					this._astarPath.FlushWorkItems();
					this.SuspendNavmeshCutting();
				}
			}
			catch
			{
				this.ReleaseNavigationPause();
				throw;
			}
		}
		return new AStarGraphManager.NavigationPause(this);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000A968 File Offset: 0x00008B68
	private void PauseGraphMover()
	{
		if (this._graphMover == null)
		{
			this._graphMover = Object.FindObjectOfType<ProceduralGraphMover>();
		}
		if (this._graphMover == null || this._navigationPausedMover != null)
		{
			return;
		}
		this._navigationPausedMover = this._graphMover;
		this._navigationMoverWasEnabled = this._graphMover.enabled;
		this._graphMover.enabled = false;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000A9D4 File Offset: 0x00008BD4
	private void SuspendNavmeshCutting()
	{
		if (this._astarPath == null || AstarPath.active != this._astarPath)
		{
			return;
		}
		NavGraph[] graphs = this._astarPath.graphs;
		for (int i = 0; i < graphs.Length; i++)
		{
			NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
			if (navmeshBase != null)
			{
				if (!this._navigationCutStates.ContainsKey(navmeshBase))
				{
					this._navigationCutStates.Add(navmeshBase, navmeshBase.enableNavmeshCutting);
				}
				navmeshBase.enableNavmeshCutting = false;
			}
		}
		this._astarPath.navmeshUpdates.ForceUpdate();
		this._astarPath.FlushWorkItems();
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000AA6C File Offset: 0x00008C6C
	private void RestoreCuttingForScan()
	{
		foreach (KeyValuePair<NavmeshBase, bool> keyValuePair in this._navigationCutStates)
		{
			keyValuePair.Key.enableNavmeshCutting = keyValuePair.Value;
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000AACC File Offset: 0x00008CCC
	private void ReleaseNavigationPause()
	{
		if (this._navigationPauseCount != 0)
		{
			int num = this._navigationPauseCount - 1;
			this._navigationPauseCount = num;
			if (num == 0)
			{
				if (!base.isActiveAndEnabled)
				{
					return;
				}
				this.RestoreNavigationState();
				return;
			}
		}
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000AB04 File Offset: 0x00008D04
	private void RestoreNavigationState()
	{
		this.RestoreCuttingForScan();
		this._navigationCutStates.Clear();
		try
		{
			if (this._astarPath != null && AstarPath.active == this._astarPath && !this._astarPath.isScanning)
			{
				this._astarPath.navmeshUpdates.ForceUpdate();
				this._astarPath.FlushWorkItems();
			}
		}
		finally
		{
			if (this._navigationPausedMover != null)
			{
				this._navigationPausedMover.BindTarget(AStarGraphManager.ResolveGraphMoverTarget());
				this._navigationPausedMover.enabled = this._navigationMoverWasEnabled;
			}
			this._navigationPausedMover = null;
		}
	}

	// Token: 0x040000F6 RID: 246
	private static readonly ProfilerMarker QueueMergedUpdateMarker = new ProfilerMarker("AStarGraphManager.QueueMergedUpdate");

	// Token: 0x040000F8 RID: 248
	[Header("Graph Settings")]
	[SerializeField]
	private float graphSize = 200f;

	// Token: 0x040000F9 RID: 249
	[SerializeField]
	private int tileSize = 128;

	// Token: 0x040000FA RID: 250
	[SerializeField]
	private float cellSize = 0.15f;

	// Token: 0x040000FB RID: 251
	[Header("Agent Settings")]
	[SerializeField]
	private float agentHeight = 2f;

	// Token: 0x040000FC RID: 252
	[SerializeField]
	private float agentRadius = 0.5f;

	// Token: 0x040000FD RID: 253
	[SerializeField]
	private float maxClimb = 0.4f;

	// Token: 0x040000FE RID: 254
	[SerializeField]
	private float maxSlope = 45f;

	// Token: 0x040000FF RID: 255
	[Header("Collision")]
	[SerializeField]
	private LayerMask includeLayers = -1;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	private bool rasterizeColliders = true;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private bool rasterizeMeshes = true;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private bool rasterizeTerrain;

	// Token: 0x04000103 RID: 259
	[Header("Local Avoidance")]
	[SerializeField]
	private bool enableLocalAvoidance = true;

	// Token: 0x04000104 RID: 260
	[Header("Diagnostics - TEMPORARY (for profiling)")]
	[Tooltip("Skips ProceduralGraphMover creation to measure its perf cost.")]
	[SerializeField]
	private bool disableGraphMover;

	// Token: 0x04000105 RID: 261
	[Tooltip("Skips RVOSimulator creation to measure its perf cost.")]
	[SerializeField]
	private bool disableRVOSimulator;

	// Token: 0x04000106 RID: 262
	[Header("Update Settings")]
	[Tooltip("Tempo minimo tra rescan (per evitare spam).")]
	[SerializeField]
	private float minRescanInterval = 1f;

	// Token: 0x04000107 RID: 263
	[Tooltip("Ritardo prima del rescan dopo spawn chunk.")]
	[SerializeField]
	private float rescanDelay = 0.5f;

	// Token: 0x04000108 RID: 264
	[Tooltip("Finestra di quiete usata per fondere gli update di piu' chunk in un solo bounds.")]
	[SerializeField]
	[Min(0f)]
	private float zoneUpdateCoalesceSeconds = 0.2f;

	// Token: 0x04000109 RID: 265
	[Tooltip("Latenza massima prima di provare il flush anche se continuano ad arrivare chunk.")]
	[SerializeField]
	[Min(0.05f)]
	private float zoneUpdateMaxLatency = 0.5f;

	// Token: 0x0400010A RID: 266
	[Tooltip("Massimo numero di tile Recast traslate per work item. 0 lascia il comportamento del package.")]
	[SerializeField]
	[Min(0f)]
	private int maxRecastTilesPerMove = 1;

	// Token: 0x0400010B RID: 267
	[Header("Debug")]
	[SerializeField]
	private bool enableDebugLogs = true;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	private bool showGraphGizmos = true;

	// Token: 0x0400010D RID: 269
	private AstarPath _astarPath;

	// Token: 0x0400010E RID: 270
	private ProceduralGraphMover _graphMover;

	// Token: 0x0400010F RID: 271
	private RVOSimulator _rvoSimulator;

	// Token: 0x04000110 RID: 272
	private float _lastRescanTime;

	// Token: 0x04000111 RID: 273
	private bool _rescanPending;

	// Token: 0x04000112 RID: 274
	private Coroutine _rescanCoroutine;

	// Token: 0x04000113 RID: 275
	private Coroutine _scanCoroutine;

	// Token: 0x04000115 RID: 277
	private bool _scanComplete;

	// Token: 0x04000116 RID: 278
	private bool _collectRecoveryZoneUpdates;

	// Token: 0x04000117 RID: 279
	private string _initializedForScene;

	// Token: 0x04000118 RID: 280
	private string _pendingReinitializeScene;

	// Token: 0x04000119 RID: 281
	private bool _hasPendingZoneUpdate;

	// Token: 0x0400011A RID: 282
	private readonly List<Bounds> _pendingZoneUpdateBounds = new List<Bounds>(32);

	// Token: 0x0400011B RID: 283
	private float _pendingZoneUpdateFirstAt;

	// Token: 0x0400011C RID: 284
	private float _pendingZoneUpdateLastAt;

	// Token: 0x0400011D RID: 285
	private const int MaxPendingZoneUpdateBounds = 64;

	// Token: 0x04000123 RID: 291
	private int _navigationPauseCount;

	// Token: 0x04000124 RID: 292
	private ProceduralGraphMover _navigationPausedMover;

	// Token: 0x04000125 RID: 293
	private bool _navigationMoverWasEnabled;

	// Token: 0x04000126 RID: 294
	private readonly Dictionary<NavmeshBase, bool> _navigationCutStates = new Dictionary<NavmeshBase, bool>();

	// Token: 0x04000127 RID: 295
	private IDisposable _scanNavigationPause;

	// Token: 0x02000025 RID: 37
	private sealed class NavigationPause : IDisposable
	{
		// Token: 0x0600016C RID: 364 RVA: 0x0000AC9F File Offset: 0x00008E9F
		public NavigationPause(AStarGraphManager owner)
		{
			this._owner = owner;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		public void Dispose()
		{
			AStarGraphManager owner = this._owner;
			this._owner = null;
			if (owner != null)
			{
				owner.ReleaseNavigationPause();
			}
		}

		// Token: 0x04000128 RID: 296
		private AStarGraphManager _owner;
	}
}
