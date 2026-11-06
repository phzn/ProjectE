using System;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class AStarSetup : MonoBehaviour
{
	// Token: 0x0600018E RID: 398 RVA: 0x0000B5B8 File Offset: 0x000097B8
	private void Awake()
	{
		if (this.target == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this.target = gameObject.transform;
			}
		}
		this.SetupAstarPath();
		this.SetupProceduralGraphMover();
		if (this.enableLocalAvoidance)
		{
			this.SetupRVOSimulator();
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000B610 File Offset: 0x00009810
	private void SetupAstarPath()
	{
		this._astarPath = Object.FindObjectOfType<AstarPath>();
		if (this._astarPath == null)
		{
			GameObject gameObject = new GameObject("A* Pathfinding");
			this._astarPath = gameObject.AddComponent<AstarPath>();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarSetup] Creato nuovo AstarPath");
			}
		}
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
				Debug.Log("[AStarSetup] Creato nuovo RecastGraph");
			}
		}
		this.ConfigureRecastGraph(recastGraph);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000B6EC File Offset: 0x000098EC
	private void ConfigureRecastGraph(RecastGraph graph)
	{
		if (graph == null)
		{
			return;
		}
		graph.forcedBoundsCenter = ((this.target != null) ? this.target.position : Vector3.zero);
		graph.forcedBoundsSize = new Vector3(this.graphSize, 100f, this.graphSize);
		graph.useTiles = true;
		graph.editorTileSize = this.tileSize;
		graph.cellSize = this.cellSize;
		graph.characterRadius = this.agentRadius;
		graph.walkableHeight = this.agentHeight;
		graph.walkableClimb = this.maxClimb;
		graph.maxSlope = this.maxSlope;
		graph.mask = this.includeLayers;
		graph.rasterizeColliders = this.rasterizeColliders;
		graph.rasterizeMeshes = !this.rasterizeColliders;
		graph.minRegionSize = 10f;
		if (this.enableDebugLogs)
		{
			Debug.Log("[AStarSetup] RecastGraph configurato: " + string.Format("Size={0}, TileSize={1}, CellSize={2}", this.graphSize, this.tileSize, this.cellSize));
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000B804 File Offset: 0x00009A04
	private void SetupProceduralGraphMover()
	{
		this._graphMover = Object.FindObjectOfType<ProceduralGraphMover>();
		if (this._graphMover == null)
		{
			this._graphMover = base.gameObject.AddComponent<ProceduralGraphMover>();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarSetup] Creato ProceduralGraphMover");
			}
		}
		this._graphMover.target = this.target;
		this._graphMover.graphIndex = 0;
		if (this.enableDebugLogs)
		{
			string text = "[AStarSetup] ProceduralGraphMover configurato con target: ";
			Transform transform = this.target;
			Debug.Log(text + ((transform != null) ? transform.name : null));
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000B894 File Offset: 0x00009A94
	private void SetupRVOSimulator()
	{
		this._rvoSimulator = Object.FindObjectOfType<RVOSimulator>();
		if (this._rvoSimulator == null)
		{
			GameObject gameObject = new GameObject("RVO Simulator");
			this._rvoSimulator = gameObject.AddComponent<RVOSimulator>();
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarSetup] Creato RVOSimulator per local avoidance");
			}
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000B8E3 File Offset: 0x00009AE3
	private void Start()
	{
		if (this._astarPath != null)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarSetup] Avvio scansione grafo iniziale...");
			}
			this._astarPath.Scan(null);
			if (this.enableDebugLogs)
			{
				Debug.Log("[AStarSetup] Scansione completata!");
			}
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000B924 File Offset: 0x00009B24
	private void OnValidate()
	{
		this.graphSize = Mathf.Max(50f, this.graphSize);
		this.tileSize = Mathf.Clamp(this.tileSize, 32, 512);
		this.cellSize = Mathf.Clamp(this.cellSize, 0.1f, 1f);
		this.cellHeight = Mathf.Clamp(this.cellHeight, 0.05f, 0.5f);
		this.agentHeight = Mathf.Max(0.5f, this.agentHeight);
		this.agentRadius = Mathf.Max(0.1f, this.agentRadius);
		this.maxClimb = Mathf.Clamp(this.maxClimb, 0f, 2f);
		this.maxSlope = Mathf.Clamp(this.maxSlope, 0f, 85f);
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000B9F8 File Offset: 0x00009BF8
	private void OnDrawGizmosSelected()
	{
		if (this.target == null)
		{
			return;
		}
		Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
		Gizmos.DrawWireCube(this.target.position, new Vector3(this.graphSize, 10f, this.graphSize));
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.target.position, 2f);
	}

	// Token: 0x04000140 RID: 320
	[Header("Target")]
	[Tooltip("Il player che il grafo seguirà.")]
	public Transform target;

	// Token: 0x04000141 RID: 321
	[Header("Recast Graph Settings")]
	[Tooltip("Dimensione del grafo in unità world (es: 200 = 200x200).")]
	public float graphSize = 200f;

	// Token: 0x04000142 RID: 322
	[Tooltip("Dimensione delle tile (in voxel). Valori tipici: 64-256.")]
	public int tileSize = 128;

	// Token: 0x04000143 RID: 323
	[Tooltip("Dimensione cella (precisione). Più basso = più preciso ma più lento.")]
	public float cellSize = 0.3f;

	// Token: 0x04000144 RID: 324
	[Tooltip("Altezza cella per precisione verticale.")]
	public float cellHeight = 0.2f;

	// Token: 0x04000145 RID: 325
	[Header("Agent Settings")]
	[Tooltip("Altezza dell'agente.")]
	public float agentHeight = 2f;

	// Token: 0x04000146 RID: 326
	[Tooltip("Raggio dell'agente.")]
	public float agentRadius = 0.5f;

	// Token: 0x04000147 RID: 327
	[Tooltip("Altezza massima gradino.")]
	public float maxClimb = 0.4f;

	// Token: 0x04000148 RID: 328
	[Tooltip("Pendenza massima in gradi.")]
	public float maxSlope = 45f;

	// Token: 0x04000149 RID: 329
	[Header("Collision Settings")]
	[Tooltip("Layer da includere nel grafo.")]
	public LayerMask includeLayers = -1;

	// Token: 0x0400014A RID: 330
	[Tooltip("Usa collider invece di mesh (consigliato).")]
	public bool rasterizeColliders = true;

	// Token: 0x0400014B RID: 331
	[Header("Local Avoidance")]
	[Tooltip("Abilita RVO Simulator per local avoidance.")]
	public bool enableLocalAvoidance = true;

	// Token: 0x0400014C RID: 332
	[Header("Debug")]
	public bool showGraphGizmos = true;

	// Token: 0x0400014D RID: 333
	public bool enableDebugLogs = true;

	// Token: 0x0400014E RID: 334
	private AstarPath _astarPath;

	// Token: 0x0400014F RID: 335
	private ProceduralGraphMover _graphMover;

	// Token: 0x04000150 RID: 336
	private RVOSimulator _rvoSimulator;
}
