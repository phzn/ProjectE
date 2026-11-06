using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Pathfinding;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Util;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000004 RID: 4
[ExecuteInEditMode]
[AddComponentMenu("Pathfinding/AstarPath")]
[DisallowMultipleComponent]
[HelpURL("https://arongranberg.com/astar/documentation/stable/astarpath.html")]
public class AstarPath : VersionedMonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
	public NavGraph[] graphs
	{
		get
		{
			return this.data.graphs;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000004 RID: 4 RVA: 0x00002065 File Offset: 0x00000265
	public float maxNearestNodeDistanceSqr
	{
		get
		{
			return this.maxNearestNodeDistance * this.maxNearestNodeDistance;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
	public float effectiveHeuristicScale
	{
		get
		{
			if (this.heuristic != Heuristic.None)
			{
				return this.heuristicScale;
			}
			return 0f;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000006 RID: 6 RVA: 0x0000208B File Offset: 0x0000028B
	// (set) Token: 0x06000007 RID: 7 RVA: 0x00002093 File Offset: 0x00000293
	public float lastScanTime { get; private set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000008 RID: 8 RVA: 0x0000209C File Offset: 0x0000029C
	// (set) Token: 0x06000009 RID: 9 RVA: 0x000020A4 File Offset: 0x000002A4
	public bool isScanning { get; private set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000020AD File Offset: 0x000002AD
	public int NumParallelThreads
	{
		get
		{
			return this.pathProcessor.NumThreads;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600000B RID: 11 RVA: 0x000020BA File Offset: 0x000002BA
	public bool IsUsingMultithreading
	{
		get
		{
			return this.pathProcessor.IsUsingMultithreading;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600000C RID: 12 RVA: 0x000020C7 File Offset: 0x000002C7
	public bool IsAnyGraphUpdateQueued
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateQueued;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600000D RID: 13 RVA: 0x000020D4 File Offset: 0x000002D4
	public bool IsAnyGraphUpdateInProgress
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateInProgress;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600000E RID: 14 RVA: 0x000020E1 File Offset: 0x000002E1
	public bool IsAnyWorkItemInProgress
	{
		get
		{
			return this.workItems.workItemsInProgress;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600000F RID: 15 RVA: 0x000020EE File Offset: 0x000002EE
	internal bool IsInsideWorkItem
	{
		get
		{
			return this.workItems.workItemsInProgressRightNow;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000010 RID: 16 RVA: 0x000020FB File Offset: 0x000002FB
	public bool IsNavigationSuspended
	{
		get
		{
			return this.navigationSafetyLock.Held;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002108 File Offset: 0x00000308
	internal void SuspendNavigation(Exception exception)
	{
		if (this.IsNavigationSuspended)
		{
			return;
		}
		this.navigationSafetyLock = this.PausePathfindingSoon();
		this.navigationRecoveryPending = true;
		Debug.LogError("A* navigation suspended after an invalid graph rebuild. A single safe recovery will be attempted next update.\n" + ((exception != null) ? exception.ToString() : null), this);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002143 File Offset: 0x00000343
	public void RequestNavigationRecovery()
	{
		if (!this.IsNavigationSuspended || this.navigationRecoveryQueued)
		{
			return;
		}
		this.navigationRecoveryPending = false;
		this.navigationRecoveryQueued = true;
		this.AddWorkItem(new AstarWorkItem(delegate
		{
			try
			{
				this.TryRecoverNavigation();
			}
			finally
			{
				this.navigationRecoveryQueued = false;
			}
		}, null));
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000217C File Offset: 0x0000037C
	private void TryRecoverNavigation()
	{
		this.navigationRecoveryPending = false;
		if (this.hierarchicalGraph.TryRecover())
		{
			this.navigationSafetyLock.Release();
			Debug.LogWarning("A* navigation recovered after rebuilding its hierarchy and navmesh obstacles.", this);
			return;
		}
		Debug.LogError("A* navigation recovery failed. Navigation remains suspended; repair or reload the graph before retrying.", this);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000021B4 File Offset: 0x000003B4
	private AstarPath()
	{
		this.pathReturnQueue = new PathReturnQueue(this, delegate
		{
			if (AstarPath.OnPathsCalculated != null)
			{
				AstarPath.OnPathsCalculated();
			}
		});
		this.nodeStorage = new GlobalNodeStorage(this);
		this.hierarchicalGraph = new HierarchicalGraph(this.nodeStorage);
		this.pathProcessor = new PathProcessor(this, this.pathReturnQueue, 1, false);
		this.offMeshLinks = new OffMeshLinks(this);
		this.workItems = new WorkItemProcessor(this);
		this.graphUpdates = new GraphUpdateProcessor(this);
		this.navmeshUpdates.astar = this;
		this.data = new AstarData(this);
		this.workItems.OnGraphsUpdated += delegate
		{
			if (AstarPath.OnGraphsUpdated != null)
			{
				try
				{
					AstarPath.OnGraphsUpdated(this);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
		};
		this.pathProcessor.OnPathPreSearch += delegate(Path path)
		{
			OnPathDelegate onPathPreSearch = AstarPath.OnPathPreSearch;
			if (onPathPreSearch != null)
			{
				onPathPreSearch(path);
			}
		};
		this.pathProcessor.OnPathPostSearch += delegate(PathHandler pathHandler, Path path)
		{
			this.LogPathResults(pathHandler.DebugStringBuilder, path);
			OnPathDelegate onPathPostSearch = AstarPath.OnPathPostSearch;
			if (onPathPostSearch != null)
			{
				onPathPostSearch(path);
			}
		};
		this.pathProcessor.OnQueueUnblocked += delegate
		{
			if (this.euclideanEmbedding.dirty)
			{
				this.euclideanEmbedding.RecalculateCosts();
			}
		};
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002378 File Offset: 0x00000578
	public string[] GetTagNames()
	{
		if (this.tagNames == null || this.tagNames.Length != 32)
		{
			this.tagNames = new string[32];
			for (int i = 0; i < this.tagNames.Length; i++)
			{
				this.tagNames[i] = i.ToString() ?? "";
			}
			this.tagNames[0] = "Basic Ground";
		}
		return this.tagNames;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000023E4 File Offset: 0x000005E4
	public static void FindAstarPath()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (AstarPath.active == null)
		{
			AstarPath.active = UnityCompatibility.FindAnyObjectByType<AstarPath>();
		}
		if (AstarPath.active != null && (AstarPath.active.data.graphs == null || AstarPath.active.data.graphs.Length == 0))
		{
			AstarPath.active.data.DeserializeGraphs();
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000244F File Offset: 0x0000064F
	public static string[] FindTagNames()
	{
		AstarPath.FindAstarPath();
		if (!(AstarPath.active != null))
		{
			return new string[] { "There is no AstarPath component in the scene" };
		}
		return AstarPath.active.GetTagNames();
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000247C File Offset: 0x0000067C
	internal ushort GetNextPathID()
	{
		if (this.nextFreePathID == 0)
		{
			this.nextFreePathID += 1;
			if (AstarPath.On65KOverflow != null)
			{
				Action on65KOverflow = AstarPath.On65KOverflow;
				AstarPath.On65KOverflow = null;
				on65KOverflow();
			}
		}
		ushort num = this.nextFreePathID;
		this.nextFreePathID = num + 1;
		return num;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000024C9 File Offset: 0x000006C9
	private void RecalculateDebugLimits()
	{
		this.debugFloor = 0f;
		this.debugRoof = 1f;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000024E4 File Offset: 0x000006E4
	public override void DrawGizmos()
	{
		if (AstarPath.active != this || this.graphs == null)
		{
			return;
		}
		this.InitializeColors();
		bool flag = (Application.isEditor ? this.showNavGraphs : this.showGraphsInStandalonePlayer);
		bool flag2 = this.showGraphsInStandalonePlayer && !Application.isEditor;
		if (flag && !this.manualDebugFloorRoof)
		{
			this.RecalculateDebugLimits();
		}
		for (int i = 0; i < this.graphs.Length; i++)
		{
			if (this.graphs[i] != null && this.graphs[i].drawGizmos)
			{
				this.graphs[i].OnDrawGizmos(DrawingManager.instance.gizmos, flag, default(RedrawScope), flag2);
			}
		}
		if (flag)
		{
			this.euclideanEmbedding.OnDrawGizmos();
			if (this.debugMode == GraphDebugMode.HierarchicalNode)
			{
				this.hierarchicalGraph.OnDrawGizmos(DrawingManager.instance.gizmos, flag2);
			}
			if (this.debugMode == GraphDebugMode.NavmeshBorderObstacles)
			{
				this.hierarchicalGraph.navmeshEdges.OnDrawGizmos(DrawingManager.instance.gizmos, flag2);
			}
		}
		this.workItems.DrawGizmos();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000025F4 File Offset: 0x000007F4
	private void OnGUI()
	{
		if (this.logPathResults == PathLog.InGame && this.inGameDebugPath != "")
		{
			GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002640 File Offset: 0x00000840
	private void LogPathResults(StringBuilder debugStringBuilder, Path path)
	{
		if (this.logPathResults != PathLog.None && (path.error || this.logPathResults != PathLog.OnlyErrors))
		{
			debugStringBuilder.Clear();
			((IPathInternals)path).DebugString(debugStringBuilder, this.logPathResults);
			string text = debugStringBuilder.ToString();
			if (this.logPathResults == PathLog.InGame)
			{
				this.inGameDebugPath = text;
				return;
			}
			if (path.error)
			{
				Debug.LogWarning(text);
				return;
			}
			Debug.Log(text);
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000026A8 File Offset: 0x000008A8
	private void Update()
	{
		if (this.navigationRecoveryPending && !this.isScanning)
		{
			this.RequestNavigationRecovery();
		}
		this.navmeshUpdates.Update();
		if (!Application.isPlaying)
		{
			return;
		}
		this.TryQueueGraphUpdates();
		if (!this.isScanning)
		{
			this.PerformBlockingActions(false);
		}
		if (!this.pathProcessor.IsUsingMultithreading)
		{
			this.pathProcessor.TickNonMultithreaded();
		}
		this.pathReturnQueue.ReturnPaths(true);
		if (!Application.isEditor && this.showGraphsInStandalonePlayer)
		{
			this.showGraphs = this.showGraphsInStandalonePlayer;
			this.DrawGizmos();
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002738 File Offset: 0x00000938
	private void PerformBlockingActions(bool force = false)
	{
		if (this.workItemLock.Held && this.pathProcessor.queue.allReceiversBlocked)
		{
			this.pathReturnQueue.ReturnPaths(false);
			if (this.workItems.ProcessWorkItemsForUpdate(force))
			{
				this.workItemLock.Release();
			}
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002789 File Offset: 0x00000989
	public void AddWorkItem(Action callback)
	{
		this.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002798 File Offset: 0x00000998
	public void AddWorkItem(Action<IWorkItemContext> callback)
	{
		this.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000027A7 File Offset: 0x000009A7
	public void AddWorkItem(AstarWorkItem item)
	{
		this.workItems.AddWorkItem(item);
		if (!this.workItemLock.Held)
		{
			this.workItemLock = this.PausePathfindingSoon();
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000027D0 File Offset: 0x000009D0
	public void QueueGraphUpdates()
	{
		if (!this.graphUpdatesWorkItemAdded)
		{
			this.graphUpdatesWorkItemAdded = true;
			AstarWorkItem workItem = this.graphUpdates.GetWorkItem();
			this.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext context)
			{
				this.graphUpdatesWorkItemAdded = false;
				this.lastGraphUpdate = Time.realtimeSinceStartup;
				workItem.initWithContext(context);
			}, workItem.updateWithContext));
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000282C File Offset: 0x00000A2C
	private void TryQueueGraphUpdates()
	{
		if (this.IsAnyGraphUpdateQueued && (!this.batchGraphUpdates || Time.realtimeSinceStartup >= this.lastGraphUpdate + this.graphUpdateBatchingInterval))
		{
			this.QueueGraphUpdates();
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002858 File Offset: 0x00000A58
	public void UpdateGraphs(Bounds bounds, float delay)
	{
		this.UpdateGraphs(new GraphUpdateObject(bounds), delay);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002867 File Offset: 0x00000A67
	public void UpdateGraphs(GraphUpdateObject ob, float delay)
	{
		base.StartCoroutine(this.UpdateGraphsInternal(ob, delay));
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002878 File Offset: 0x00000A78
	private IEnumerator UpdateGraphsInternal(GraphUpdateObject ob, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.UpdateGraphs(ob);
		yield break;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002895 File Offset: 0x00000A95
	public void UpdateGraphs(Bounds bounds)
	{
		this.UpdateGraphs(new GraphUpdateObject(bounds));
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000028A3 File Offset: 0x00000AA3
	public void UpdateGraphs(GraphUpdateObject ob)
	{
		if (ob.internalStage != -1)
		{
			throw new Exception("You are trying to update graphs using the same graph update object twice. Please create a new GraphUpdateObject instead.");
		}
		ob.internalStage = -2;
		this.graphUpdates.AddToQueue(ob);
		this.TryQueueGraphUpdates();
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000028D3 File Offset: 0x00000AD3
	public void FlushGraphUpdates()
	{
		if (this.IsAnyGraphUpdateQueued || this.IsAnyGraphUpdateInProgress)
		{
			this.QueueGraphUpdates();
			this.FlushWorkItems();
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000028F4 File Offset: 0x00000AF4
	public void FlushWorkItems()
	{
		if (this.workItems.anyQueued || this.workItems.workItemsInProgress)
		{
			if (AstarPath.active != this)
			{
				throw new Exception("This AstarPath component is not initialized in a scene. Are you trying to add work items to a prefab or a disabled AstarPath component?");
			}
			using (this.PausePathfinding())
			{
				this.PerformBlockingActions(true);
			}
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002964 File Offset: 0x00000B64
	private void InitializePathProcessor()
	{
		int num = this.threadCount.ToConcreteThreadCount();
		if (!Application.isPlaying)
		{
			num = 0;
		}
		int num2 = Mathf.Max(num, 1);
		bool flag = num > 0;
		this.pathProcessor.StopThreads();
		this.pathProcessor.SetThreadCount(num2, flag);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000029AB File Offset: 0x00000BAB
	private void InitializeColors()
	{
		this.colorSettings = this.colorSettings ?? new AstarColor();
		this.colorSettings.PushToStatic();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000029D0 File Offset: 0x00000BD0
	private void ShutdownPathfindingThreads()
	{
		PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
		this.navmeshUpdates.OnDisable();
		this.euclideanEmbedding.dirty = false;
		this.graphUpdates.DiscardQueued();
		this.FlushWorkItems();
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Processing Possible Work Items");
		}
		this.pathProcessor.StopThreads();
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Returning Paths");
		}
		this.pathReturnQueue.ReturnPaths(false);
		graphUpdateLock.Release();
		if (this.navigationSafetyLock.Held)
		{
			this.navigationSafetyLock.Release();
		}
		this.navigationRecoveryPending = false;
		this.navigationRecoveryQueued = false;
		this.euclideanEmbedding.OnDisable();
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002A84 File Offset: 0x00000C84
	private void OnEnable()
	{
		if (AstarPath.active != null)
		{
			if (AstarPath.active != this && Application.isPlaying)
			{
				if (base.enabled)
				{
					Debug.LogWarning("Another A* component is already in the scene. More than one A* component cannot be active at the same time. Disabling this one.", this);
				}
				base.enabled = false;
			}
			return;
		}
		AstarPath.active = this;
		base.useGUILayout = false;
		if (AstarPath.OnAwakeSettings != null)
		{
			AstarPath.OnAwakeSettings();
		}
		this.hierarchicalGraph.OnEnable();
		GraphModifier.FindAllModifiers();
		RelevantGraphSurface.FindAllGraphSurfaces();
		this.InitializeColors();
		this.navmeshUpdates.OnEnable();
		this.data.OnEnable();
		this.FlushWorkItems();
		this.euclideanEmbedding.dirty = true;
		this.InitializePathProcessor();
		if (Application.isPlaying && this.scanOnStartup && !this.hasScannedGraphAtStartup && (!this.data.cacheStartup || this.data.file_cachedStartup == null))
		{
			this.hasScannedGraphAtStartup = true;
			this.Scan(null);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002B7C File Offset: 0x00000D7C
	private void OnDisable()
	{
		if (AstarPath.active == this)
		{
			if (this.asyncScanTask != null)
			{
				Debug.LogWarning("An async scan was running when the AstarPath component was disabled. Blocking until the async scan is complete.", this);
				this.BlockUntilAsyncScanComplete();
			}
			this.graphDataLock.WriteSync().Unlock();
			this.ShutdownPathfindingThreads();
			this.data.DestroyAllNodes();
			this.data.DisposeUnmanagedData();
			this.hierarchicalGraph.OnDisable();
			this.nodeStorage.OnDisable();
			this.offMeshLinks.OnDisable();
			AstarPath.active = null;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002C08 File Offset: 0x00000E08
	private void OnDestroy()
	{
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("AstarPath Component Destroyed - Cleaning Up Pathfinding Data");
		}
		AstarPath astarPath = AstarPath.active;
		AstarPath.active = this;
		this.ShutdownPathfindingThreads();
		this.pathProcessor.Dispose();
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Destroying Graphs");
		}
		if (this.data != null)
		{
			this.data.OnDestroy();
		}
		AstarPath.active = astarPath;
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Cleaning up variables");
		}
		if (AstarPath.active == this)
		{
			AstarPath.OnAwakeSettings = null;
			AstarPath.OnGraphPreScan = null;
			AstarPath.OnGraphPostScan = null;
			AstarPath.OnPathPreSearch = null;
			AstarPath.OnPathPostSearch = null;
			AstarPath.OnPreScan = null;
			AstarPath.OnPostScan = null;
			AstarPath.OnLatePostScan = null;
			AstarPath.On65KOverflow = null;
			AstarPath.OnGraphsUpdated = null;
			AstarPath.active = null;
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002CD1 File Offset: 0x00000ED1
	public JobHandle AllocateNodes<T>(T[] result, int count, Func<T> createNode, uint variantsPerNode) where T : GraphNode
	{
		if (!this.pathProcessor.queue.allReceiversBlocked)
		{
			throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.html#direct");
		}
		return this.nodeStorage.AllocateNodesJob<T>(result, count, createNode, variantsPerNode);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002D00 File Offset: 0x00000F00
	internal void InitializeNode(GraphNode node)
	{
		if (!this.pathProcessor.queue.allReceiversBlocked)
		{
			throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.html#direct");
		}
		this.nodeStorage.InitializeNode(node);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002D2C File Offset: 0x00000F2C
	internal void InitializeNodes(GraphNode[] nodes)
	{
		if (!this.pathProcessor.queue.allReceiversBlocked)
		{
			throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.html#direct");
		}
		for (int i = 0; i < nodes.Length; i++)
		{
			this.nodeStorage.InitializeNode(nodes[i]);
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002D72 File Offset: 0x00000F72
	internal void DestroyNode(GraphNode node)
	{
		this.nodeStorage.DestroyNode(node);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002D80 File Offset: 0x00000F80
	public PathProcessor.GraphUpdateLock PausePathfinding()
	{
		this.graphDataLock.WriteSync().Unlock();
		return this.pathProcessor.PausePathfinding(true);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002DAC File Offset: 0x00000FAC
	public PathProcessor.GraphUpdateLock PausePathfindingSoon()
	{
		return this.pathProcessor.PausePathfinding(false);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002DBA File Offset: 0x00000FBA
	private void BlockUntilAsyncScanComplete()
	{
		while (this.asyncScanTask != null && this.asyncScanTask.MoveNext())
		{
		}
		this.asyncScanTask = null;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002DD8 File Offset: 0x00000FD8
	public void Scan(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		this.Scan(new NavGraph[] { graphToScan });
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002DF4 File Offset: 0x00000FF4
	public void Scan(NavGraph[] graphsToScan = null)
	{
		ScanningStage scanningStage = (ScanningStage)(-1);
		if (this.asyncScanTask != null)
		{
			Debug.LogWarning("An async scan was already running when a new scan was requested. Blocking until it is complete. You can check if a scan is currently in progress using the AstarPath.active.isScanning property.", this);
			this.BlockUntilAsyncScanComplete();
		}
		foreach (Progress progress in this.ScanInternal(graphsToScan, false))
		{
			if (scanningStage != progress.stage)
			{
				scanningStage = progress.stage;
			}
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002E68 File Offset: 0x00001068
	public IEnumerable<Progress> ScanAsync(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		return this.ScanAsync(new NavGraph[] { graphToScan });
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002E84 File Offset: 0x00001084
	public IEnumerable<Progress> ScanAsync(NavGraph[] graphsToScan = null)
	{
		if (this.asyncScanTask != null)
		{
			Debug.LogWarning("An async scan was already running when a new async scan was requested. Blocking until the previous one is complete. You can check if a scan is currently in progress using the AstarPath.active.isScanning property.", this);
			this.BlockUntilAsyncScanComplete();
		}
		this.asyncScanTask = this.ScanInternal(graphsToScan, true).GetEnumerator();
		try
		{
			this.asyncScanTask.MoveNext();
		}
		catch
		{
			this.asyncScanTask = null;
			throw;
		}
		return this.TickAsyncScanUntilCompletion(this.asyncScanTask);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002EF4 File Offset: 0x000010F4
	private IEnumerable<Progress> TickAsyncScanUntilCompletion(IEnumerator<Progress> task)
	{
		for (;;)
		{
			try
			{
				if (!task.MoveNext())
				{
					break;
				}
			}
			catch
			{
				if (this.asyncScanTask == task)
				{
					this.asyncScanTask = null;
				}
				throw;
			}
			yield return task.Current;
		}
		if (this.asyncScanTask == task)
		{
			this.asyncScanTask = null;
		}
		yield break;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002F0B File Offset: 0x0000110B
	private IEnumerable<Progress> ScanInternal(NavGraph[] graphsToScan, bool async)
	{
		if (graphsToScan == null)
		{
			graphsToScan = this.graphs;
		}
		if (graphsToScan == null || graphsToScan.Length == 0)
		{
			yield break;
		}
		if (!base.enabled)
		{
			throw new InvalidOperationException("The AstarPath object must be enabled to scan graphs");
		}
		if (AstarPath.active != this)
		{
			throw new InvalidOperationException("The AstarPath object is not enabled in a scene");
		}
		this.isScanning = true;
		PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
		this.pathReturnQueue.ReturnPaths(false);
		this.workItems.ProcessWorkItemsForScan(true);
		if (!Application.isPlaying)
		{
			this.data.FindGraphTypes();
			GraphModifier.FindAllModifiers();
		}
		yield return new Progress(0.05f, ScanningStage.PreProcessingGraphs, 0, 0);
		using (this.graphDataLock.WriteSync())
		{
			try
			{
				if (AstarPath.OnPreScan != null)
				{
					AstarPath.OnPreScan(this);
				}
				GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
				GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
			}
			catch
			{
				this.isScanning = false;
				graphUpdateLock.Release();
				throw;
			}
		}
		this.data.LockGraphStructure(false);
		Physics.SyncTransforms();
		Physics2D.SyncTransforms();
		Stopwatch watch = Stopwatch.StartNew();
		if (!async)
		{
			using (this.graphDataLock.WriteSync())
			{
				for (int i = 0; i < graphsToScan.Length; i++)
				{
					if (graphsToScan[i] != null)
					{
						((IGraphInternals)graphsToScan[i]).DestroyAllNodes();
					}
				}
			}
		}
		if (AstarPath.OnGraphPreScan != null)
		{
			using (this.graphDataLock.WriteSync())
			{
				try
				{
					for (int j = 0; j < graphsToScan.Length; j++)
					{
						if (graphsToScan[j] != null)
						{
							AstarPath.OnGraphPreScan(graphsToScan[j]);
						}
					}
				}
				catch
				{
					this.isScanning = false;
					this.data.UnlockGraphStructure();
					graphUpdateLock.Release();
					throw;
				}
			}
		}
		List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>(graphsToScan.Length);
		for (int k = 0; k < graphsToScan.Length; k++)
		{
			if (graphsToScan[k] != null)
			{
				IGraphUpdatePromise graphUpdatePromise;
				if ((graphUpdatePromise = ((IGraphInternals)graphsToScan[k]).ScanInternal(async)) == null)
				{
					(graphUpdatePromise = new AstarPath.DestroyGraphPromise()).graph = graphsToScan[k];
				}
				IGraphUpdatePromise graphUpdatePromise2 = graphUpdatePromise;
				IEnumerator<JobHandle> enumerator = graphUpdatePromise2.Prepare();
				promises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise2, enumerator));
			}
		}
		for (;;)
		{
			int num;
			try
			{
				num = GraphUpdateProcessor.PrepareGraphUpdatePromises(promises, async ? TimeSlice.MillisFromNow(2f) : TimeSlice.Infinite);
			}
			catch
			{
				this.isScanning = false;
				this.data.UnlockGraphStructure();
				graphUpdateLock.Release();
				throw;
			}
			if (num == -1)
			{
				break;
			}
			float num2 = 0f;
			for (int l = 0; l < promises.Count; l++)
			{
				num2 += promises[l].Item1.Progress;
			}
			num2 /= (float)promises.Count;
			yield return new Progress(Mathf.Lerp(0.1f, 0.8f, num2), ScanningStage.ScanningGraph, num, promises.Count);
		}
		yield return new Progress(0.95f, ScanningStage.FinishingScans, 0, 0);
		RWLock.LockSync lockSync4 = this.graphDataLock.WriteSync();
		AstarPath.DummyGraphUpdateContext dummyGraphUpdateContext = new AstarPath.DummyGraphUpdateContext();
		try
		{
			GraphUpdateProcessor.ApplyGraphUpdatePromises(promises, dummyGraphUpdateContext);
		}
		catch
		{
			this.isScanning = false;
			this.data.UnlockGraphStructure();
			graphUpdateLock.Release();
			lockSync4.Unlock();
			throw;
		}
		for (int m = 0; m < graphsToScan.Length; m++)
		{
			if (graphsToScan[m] != null)
			{
				if (AstarPath.OnGraphPostScan != null)
				{
					try
					{
						AstarPath.OnGraphPostScan(graphsToScan[m]);
					}
					catch
					{
						this.isScanning = false;
						this.data.UnlockGraphStructure();
						graphUpdateLock.Release();
						lockSync4.Unlock();
						throw;
					}
				}
				if (!(graphsToScan[m] is LinkGraph))
				{
					this.offMeshLinks.DirtyBounds(graphsToScan[m].bounds);
				}
			}
		}
		this.data.UnlockGraphStructure();
		try
		{
			if (AstarPath.OnPostScan != null)
			{
				AstarPath.OnPostScan(this);
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
		}
		catch
		{
			this.isScanning = false;
			graphUpdateLock.Release();
			lockSync4.Unlock();
			throw;
		}
		if (this.workItemLock.Held)
		{
			this.workItems.ProcessWorkItemsForScan(true);
			this.workItemLock.Release();
		}
		this.offMeshLinks.Refresh();
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdateBeforeAreaRecalculation);
		if (this.IsNavigationSuspended)
		{
			this.TryRecoverNavigation();
		}
		else
		{
			this.hierarchicalGraph.RecalculateIfNecessary();
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
		if (AstarPath.OnGraphsUpdated != null)
		{
			try
			{
				AstarPath.OnGraphsUpdated(this);
			}
			catch
			{
				this.isScanning = false;
				graphUpdateLock.Release();
				lockSync4.Unlock();
				throw;
			}
		}
		this.isScanning = false;
		try
		{
			if (AstarPath.OnLatePostScan != null)
			{
				AstarPath.OnLatePostScan(this);
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
		}
		catch
		{
			graphUpdateLock.Release();
			lockSync4.Unlock();
			throw;
		}
		lockSync4.Unlock();
		this.euclideanEmbedding.dirty = true;
		this.euclideanEmbedding.RecalculatePivots();
		this.FlushWorkItems();
		graphUpdateLock.Release();
		watch.Stop();
		this.lastScanTime = (float)watch.Elapsed.TotalSeconds;
		if (this.logPathResults != PathLog.None && this.logPathResults != PathLog.OnlyErrors)
		{
			Debug.Log("Scanned graphs in " + (this.lastScanTime * 1000f).ToString("0") + " ms");
		}
		yield break;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002F29 File Offset: 0x00001129
	internal void DirtyBounds(Bounds bounds)
	{
		this.offMeshLinks.DirtyBounds(bounds);
		this.workItems.DirtyBounds(bounds);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002F43 File Offset: 0x00001143
	internal void VisualizeOriginalGraphUpdateBounds(Bounds bounds)
	{
		this.workItems.VisualizeOriginalGraphUpdateBounds(bounds);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002F54 File Offset: 0x00001154
	public static void BlockUntilCalculated(Path path)
	{
		if (AstarPath.active == null)
		{
			throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (AstarPath.active.pathProcessor.queue.isClosed)
		{
			return;
		}
		if (path.PipelineState == PathState.Created)
		{
			throw new Exception("The specified path has not been started yet.");
		}
		AstarPath.waitForPathDepth++;
		if (AstarPath.waitForPathDepth == 5)
		{
			Debug.LogError("You are calling the BlockUntilCalculated function recursively (maybe from a path callback). Please don't do this.");
		}
		if (path.PipelineState < PathState.ReturnQueue)
		{
			if (AstarPath.active.IsUsingMultithreading)
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (AstarPath.active.pathProcessor.queue.isClosed)
					{
						AstarPath.waitForPathDepth--;
						throw new Exception("Pathfinding Threads seem to have crashed.");
					}
					Thread.Sleep(1);
					AstarPath.active.PerformBlockingActions(true);
				}
			}
			else
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (AstarPath.active.pathProcessor.queue.isEmpty && path.PipelineState != PathState.Processing)
					{
						AstarPath.waitForPathDepth--;
						throw new Exception("Critical error. Path Queue is empty but the path state is '" + path.PipelineState.ToString() + "'");
					}
					AstarPath.active.pathProcessor.TickNonMultithreaded();
					AstarPath.active.PerformBlockingActions(true);
				}
			}
		}
		AstarPath.active.pathReturnQueue.ReturnPaths(false);
		AstarPath.waitForPathDepth--;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000030C8 File Offset: 0x000012C8
	public static void StartPath(Path path, bool pushToFront = false, bool assumeInPlayMode = false)
	{
		AstarPath astarPath = AstarPath.active;
		if (astarPath == null)
		{
			Debug.LogError("There is no AstarPath object in the scene or it has not been initialized yet");
			return;
		}
		if (path.PipelineState != PathState.Created)
		{
			throw new Exception(string.Concat(new string[]
			{
				"The path has an invalid state. Expected ",
				PathState.Created.ToString(),
				" found ",
				path.PipelineState.ToString(),
				"\nMake sure you are not requesting the same path twice"
			}));
		}
		if (astarPath.pathProcessor.queue.isClosed)
		{
			path.FailWithError("No new paths are accepted");
			return;
		}
		if (astarPath.IsNavigationSuspended)
		{
			path.FailWithError("Navigation is suspended after an invalid graph rebuild");
			path.Claim(astarPath);
			((IPathInternals)path).AdvanceState(PathState.ReturnQueue);
			astarPath.pathReturnQueue.Enqueue(path);
			return;
		}
		if (astarPath.graphs == null || astarPath.graphs.Length == 0)
		{
			Debug.LogError("There are no graphs in the scene");
			path.FailWithError("There are no graphs in the scene");
			Debug.LogError(path.errorLog);
			return;
		}
		path.Claim(astarPath);
		((IPathInternals)path).AdvanceState(PathState.PathQueue);
		if (pushToFront)
		{
			astarPath.pathProcessor.queue.PushFront(path);
		}
		else
		{
			astarPath.pathProcessor.queue.Push(path);
		}
		if (!assumeInPlayMode && !JobsUtility.IsExecutingJob && !Application.isPlaying)
		{
			AstarPath.BlockUntilCalculated(path);
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003210 File Offset: 0x00001410
	public bool IsPointOnNavmesh(Vector3 position)
	{
		NearestNodeConstraint none = NearestNodeConstraint.None;
		none.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
		none.maxDistanceSqr = 0.0001f;
		NNInfo nearest = this.GetNearest(position, none);
		return nearest.node != null && nearest.node.Walkable;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003259 File Offset: 0x00001459
	public NNInfo GetNearest(Vector3 position)
	{
		return this.GetNearest(position, NearestNodeConstraint.None);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003267 File Offset: 0x00001467
	[Obsolete("NNConstraint is obsolete. Use the overload that takes a NearestNodeConstraint instead")]
	public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
	{
		return this.GetNearest(position, (constraint != null) ? constraint.ToNearestNodeConstraint() : NearestNodeConstraint.None);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003280 File Offset: 0x00001480
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public NNInfo GetNearest(Vector3 position, NearestNodeConstraint constraint)
	{
		return this.GetNearest(position, ref constraint);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000328C File Offset: 0x0000148C
	internal unsafe NNInfo GetNearest(Vector3 position, ref NearestNodeConstraint constraint)
	{
		NavGraph[] graphs = this.graphs;
		constraint.maxDistanceSqr = constraint.maxDistanceSqrOrDefault(AstarPath.active);
		NNInfo nninfo = NNInfo.Empty;
		if (graphs == null || graphs.Length == 0)
		{
			return nninfo;
		}
		if (graphs.Length == 1)
		{
			NavGraph navGraph = graphs[0];
			if (navGraph == null || !constraint.graphMask.Contains(navGraph))
			{
				return nninfo;
			}
			nninfo = navGraph.GetNearest(position, ref constraint);
		}
		else
		{
			UnsafeSpan<ValueTuple<float, uint>> unsafeSpan;
			int num;
			checked
			{
				ValueTuple<float, int>* ptr = stackalloc ValueTuple<float, int>[unchecked((UIntPtr)graphs.Length) * (UIntPtr)sizeof(ValueTuple<float, int>)];
				unsafeSpan = new UnsafeSpan<ValueTuple<float, uint>>((void*)ptr, graphs.Length, Allocator.None);
				num = 0;
			}
			for (uint num2 = 0U; num2 < (uint)graphs.Length; num2 += 1U)
			{
				NavGraph navGraph2 = graphs[(int)num2];
				if (navGraph2 != null && constraint.graphMask.Contains(num2))
				{
					float num3 = navGraph2.NearestNodeDistanceSqrLowerBound(position, ref constraint);
					if (num3 <= constraint.maxDistanceSqr)
					{
						*unsafeSpan[num++] = new ValueTuple<float, uint>(num3, num2);
					}
				}
			}
			unsafeSpan = unsafeSpan.Slice(0, num);
			unsafeSpan.Sort<ValueTuple<float, uint>>();
			int num4 = 0;
			while (num4 < unsafeSpan.Length && unsafeSpan[num4].Item1 <= constraint.maxDistanceSqr)
			{
				NNInfo nearest = graphs[(int)unsafeSpan[num4].Item2].GetNearest(position, ref constraint);
				if (nearest.distanceCostSqr < constraint.maxDistanceSqr)
				{
					constraint.maxDistanceSqr = nearest.distanceCostSqr;
					nninfo = nearest;
				}
				num4++;
			}
		}
		return nninfo;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000033E4 File Offset: 0x000015E4
	public bool GetNearestBorder(Vector3 position, NearestNodeConstraint constraint, out GraphHitInfo hit)
	{
		NNInfo nearest = this.GetNearest(position, constraint);
		return this.GetNearestBorder(position, nearest.node, out hit);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003408 File Offset: 0x00001608
	public bool GetNearestBorder(Vector3 position, GraphNode sourceNode, out GraphHitInfo hit)
	{
		if (sourceNode is PointNode)
		{
			hit = new GraphHitInfo
			{
				origin = position,
				point = Vector3.positiveInfinity
			};
			return false;
		}
		NativeList<int> nativeList = new NativeList<int>(Allocator.Temp);
		nativeList.Clear();
		RWLock.CombinedReadLockAsync combinedReadLockAsync;
		NavmeshEdges.NavmeshBorderData navmeshBorderData = this.GetNavmeshBorderData(out combinedReadLockAsync);
		combinedReadLockAsync.dependency.Complete();
		hit = navmeshBorderData.GetClosestEdge((sourceNode != null && sourceNode.Walkable) ? sourceNode.HierarchicalNodeIndex : (-1), position, nativeList);
		combinedReadLockAsync.Unlock();
		return hit.point.x != float.PositiveInfinity;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000034B4 File Offset: 0x000016B4
	public bool Linecast(Vector3 start, Vector3 end)
	{
		IRaycastableGraph raycastableGraph = this.ClosestRaycastableGraph(start);
		return raycastableGraph == null || raycastableGraph.Linecast(start, end);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000034D8 File Offset: 0x000016D8
	public bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit)
	{
		IRaycastableGraph raycastableGraph = this.ClosestRaycastableGraph(start);
		if (raycastableGraph == null)
		{
			hit = new GraphHitInfo
			{
				origin = start,
				point = start
			};
			return true;
		}
		return raycastableGraph.Linecast(start, end, out hit);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000351C File Offset: 0x0000171C
	private IRaycastableGraph ClosestRaycastableGraph(Vector3 point)
	{
		if (this.data.graphs == null)
		{
			return null;
		}
		IRaycastableGraph raycastableGraph = null;
		int num = 0;
		for (int i = 0; i < this.data.graphs.Length; i++)
		{
			IRaycastableGraph raycastableGraph2 = this.data.graphs[i] as IRaycastableGraph;
			if (raycastableGraph2 != null)
			{
				raycastableGraph = raycastableGraph2;
				num++;
			}
		}
		if (num > 1)
		{
			GraphNode node = this.GetNearest(point).node;
			raycastableGraph = ((node != null) ? node.Graph : null) as IRaycastableGraph;
		}
		return raycastableGraph;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003594 File Offset: 0x00001794
	public GraphNode GetNearest(Ray ray)
	{
		if (this.graphs == null)
		{
			return null;
		}
		float minDist = float.PositiveInfinity;
		GraphNode nearestNode = null;
		Vector3 lineDirection = ray.direction;
		Vector3 lineOrigin = ray.origin;
		Action<GraphNode> <>9__0;
		for (int i = 0; i < this.graphs.Length; i++)
		{
			NavGraph navGraph = this.graphs[i];
			Action<GraphNode> action;
			if ((action = <>9__0) == null)
			{
				action = (<>9__0 = delegate(GraphNode node)
				{
					Vector3 vector = (Vector3)node.position;
					Vector3 vector2 = lineOrigin + Vector3.Dot(vector - lineOrigin, lineDirection) * lineDirection;
					float num = Mathf.Abs(vector2.x - vector.x);
					if (num * num > minDist)
					{
						return;
					}
					float num2 = Mathf.Abs(vector2.z - vector.z);
					if (num2 * num2 > minDist)
					{
						return;
					}
					float sqrMagnitude = (vector2 - vector).sqrMagnitude;
					if (sqrMagnitude < minDist)
					{
						minDist = sqrMagnitude;
						nearestNode = node;
					}
				});
			}
			navGraph.GetNodes(action);
		}
		return nearestNode;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003624 File Offset: 0x00001824
	public GraphSnapshot Snapshot(Bounds bounds, GraphMask graphMask)
	{
		List<IGraphSnapshot> list = new List<IGraphSnapshot>();
		for (uint num = 0U; num < (uint)this.graphs.Length; num += 1U)
		{
			if (this.graphs[(int)num] != null && graphMask.Contains(num))
			{
				IGraphSnapshot graphSnapshot = this.graphs[(int)num].Snapshot(bounds);
				if (graphSnapshot != null)
				{
					list.Add(graphSnapshot);
				}
			}
		}
		return new GraphSnapshot(list);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0000367D File Offset: 0x0000187D
	public RWLock.ReadLockAsync LockGraphDataForReading()
	{
		return this.graphDataLock.Read();
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000368A File Offset: 0x0000188A
	public RWLock.WriteLockAsync LockGraphDataForWriting()
	{
		return this.graphDataLock.Write();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003697 File Offset: 0x00001897
	public RWLock.LockSync LockGraphDataForWritingSync()
	{
		return this.graphDataLock.WriteSync();
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000036A4 File Offset: 0x000018A4
	public NavmeshEdges.NavmeshBorderData GetNavmeshBorderData(out RWLock.CombinedReadLockAsync readLock)
	{
		return this.hierarchicalGraph.navmeshEdges.GetNavmeshEdgeData(out readLock);
	}

	// Token: 0x04000001 RID: 1
	public static readonly Version Version = new Version(5, 4, 5);

	// Token: 0x04000002 RID: 2
	public static readonly AstarPath.AstarDistribution Distribution = AstarPath.AstarDistribution.AssetStore;

	// Token: 0x04000003 RID: 3
	public static readonly string Branch = "master";

	// Token: 0x04000004 RID: 4
	[FormerlySerializedAs("astarData")]
	public AstarData data;

	// Token: 0x04000005 RID: 5
	public static AstarPath active;

	// Token: 0x04000006 RID: 6
	private bool hasScannedGraphAtStartup;

	// Token: 0x04000007 RID: 7
	public bool showNavGraphs = true;

	// Token: 0x04000008 RID: 8
	public bool showUnwalkableNodes = true;

	// Token: 0x04000009 RID: 9
	public GraphDebugMode debugMode;

	// Token: 0x0400000A RID: 10
	public GraphUpdateDebugMode graphUpdateDebugMode;

	// Token: 0x0400000B RID: 11
	public float debugFloor;

	// Token: 0x0400000C RID: 12
	public float debugRoof = 20000f;

	// Token: 0x0400000D RID: 13
	public bool manualDebugFloorRoof;

	// Token: 0x0400000E RID: 14
	public bool showSearchTree;

	// Token: 0x0400000F RID: 15
	public float unwalkableNodeDebugSize = 0.3f;

	// Token: 0x04000010 RID: 16
	public PathLog logPathResults = PathLog.Normal;

	// Token: 0x04000011 RID: 17
	public float maxNearestNodeDistance = 100f;

	// Token: 0x04000012 RID: 18
	public bool scanOnStartup = true;

	// Token: 0x04000013 RID: 19
	[Obsolete("This setting has been removed. It is now always true", true)]
	public bool fullGetNearestSearch;

	// Token: 0x04000014 RID: 20
	[Obsolete("This setting has been removed. It was always a bit of a hack. Use TraversalConstraint.graphMask if you want to choose which graphs are searched.", true)]
	public bool prioritizeGraphs;

	// Token: 0x04000015 RID: 21
	[Obsolete("This setting has been removed. It was always a bit of a hack. Use TraversalConstraint.graphMask if you want to choose which graphs are searched.", true)]
	public float prioritizeGraphsLimit = 1f;

	// Token: 0x04000016 RID: 22
	public AstarColor colorSettings;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	protected string[] tagNames;

	// Token: 0x04000018 RID: 24
	public Heuristic heuristic = Heuristic.Euclidean;

	// Token: 0x04000019 RID: 25
	public float heuristicScale = 1f;

	// Token: 0x0400001A RID: 26
	public ThreadCount threadCount = ThreadCount.One;

	// Token: 0x0400001B RID: 27
	public float maxFrameTime = 1f;

	// Token: 0x0400001C RID: 28
	public bool batchGraphUpdates;

	// Token: 0x0400001D RID: 29
	public float graphUpdateBatchingInterval = 0.2f;

	// Token: 0x0400001F RID: 31
	[NonSerialized]
	internal PathHandler debugPathData;

	// Token: 0x04000020 RID: 32
	[NonSerialized]
	internal ushort debugPathID;

	// Token: 0x04000021 RID: 33
	private string inGameDebugPath;

	// Token: 0x04000023 RID: 35
	public static Action OnAwakeSettings;

	// Token: 0x04000024 RID: 36
	public static OnGraphDelegate OnGraphPreScan;

	// Token: 0x04000025 RID: 37
	public static OnGraphDelegate OnGraphPostScan;

	// Token: 0x04000026 RID: 38
	public static OnPathDelegate OnPathPreSearch;

	// Token: 0x04000027 RID: 39
	public static OnPathDelegate OnPathPostSearch;

	// Token: 0x04000028 RID: 40
	public static OnScanDelegate OnPreScan;

	// Token: 0x04000029 RID: 41
	public static OnScanDelegate OnPostScan;

	// Token: 0x0400002A RID: 42
	public static OnScanDelegate OnLatePostScan;

	// Token: 0x0400002B RID: 43
	public static OnScanDelegate OnGraphsUpdated;

	// Token: 0x0400002C RID: 44
	public static Action On65KOverflow;

	// Token: 0x0400002D RID: 45
	public static Action OnPathsCalculated;

	// Token: 0x0400002E RID: 46
	private readonly GraphUpdateProcessor graphUpdates;

	// Token: 0x0400002F RID: 47
	internal readonly HierarchicalGraph hierarchicalGraph;

	// Token: 0x04000030 RID: 48
	private PathProcessor.GraphUpdateLock navigationSafetyLock;

	// Token: 0x04000031 RID: 49
	private bool navigationRecoveryPending;

	// Token: 0x04000032 RID: 50
	private bool navigationRecoveryQueued;

	// Token: 0x04000033 RID: 51
	public readonly OffMeshLinks offMeshLinks;

	// Token: 0x04000034 RID: 52
	public NavmeshUpdates navmeshUpdates = new NavmeshUpdates();

	// Token: 0x04000035 RID: 53
	private readonly WorkItemProcessor workItems;

	// Token: 0x04000036 RID: 54
	internal readonly PathProcessor pathProcessor;

	// Token: 0x04000037 RID: 55
	internal GlobalNodeStorage nodeStorage;

	// Token: 0x04000038 RID: 56
	private RWLock graphDataLock = new RWLock();

	// Token: 0x04000039 RID: 57
	private bool graphUpdatesWorkItemAdded;

	// Token: 0x0400003A RID: 58
	private float lastGraphUpdate = float.NegativeInfinity;

	// Token: 0x0400003B RID: 59
	private PathProcessor.GraphUpdateLock workItemLock;

	// Token: 0x0400003C RID: 60
	internal readonly PathReturnQueue pathReturnQueue;

	// Token: 0x0400003D RID: 61
	public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();

	// Token: 0x0400003E RID: 62
	private IEnumerator<Progress> asyncScanTask;

	// Token: 0x0400003F RID: 63
	public bool showGraphs;

	// Token: 0x04000040 RID: 64
	public bool showGraphsInStandalonePlayer;

	// Token: 0x04000041 RID: 65
	private ushort nextFreePathID = 1;

	// Token: 0x04000042 RID: 66
	private static int waitForPathDepth = 0;

	// Token: 0x02000005 RID: 5
	public enum AstarDistribution
	{
		// Token: 0x04000044 RID: 68
		WebsiteDownload,
		// Token: 0x04000045 RID: 69
		AssetStore,
		// Token: 0x04000046 RID: 70
		PackageManager
	}

	// Token: 0x02000006 RID: 6
	private class DummyGraphUpdateContext : IGraphUpdateContext
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003788 File Offset: 0x00001988
		public void DirtyBounds(Bounds bounds)
		{
		}
	}

	// Token: 0x02000007 RID: 7
	private class DestroyGraphPromise : IGraphUpdatePromise
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003792 File Offset: 0x00001992
		public IEnumerator<JobHandle> Prepare()
		{
			return null;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003795 File Offset: 0x00001995
		public void Apply(IGraphUpdateContext context)
		{
			this.graph.DestroyAllNodes();
		}

		// Token: 0x04000047 RID: 71
		public IGraphInternals graph;
	}
}
