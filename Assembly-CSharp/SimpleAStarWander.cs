using System;
using Features.NPC;
using Features.RenderingPrecision;
using Pathfinding;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class SimpleAStarWander : MonoBehaviour
{
	// Token: 0x060008F5 RID: 2293 RVA: 0x00041491 File Offset: 0x0003F691
	private void Awake()
	{
		this._follower = base.GetComponent<FollowerEntity>();
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x000414A0 File Offset: 0x0003F6A0
	private void Start()
	{
		float num = this.initialDelay + Random.Range(0f, this.initialDelayVariance);
		this._nextActionTime = Time.time + num;
		this._initialized = true;
		this._failedAttempts = 0;
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[SimpleAStarWander] {0} inizierà a vagare tra {1:F1}s", base.gameObject.name, num));
		}
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00041508 File Offset: 0x0003F708
	private void Update()
	{
		if (NPCSimulationPause.IsPaused)
		{
			this.BeginSimulationPauseIfNeeded();
			return;
		}
		this.ResumeSimulationIfNeeded();
		if (!this._initialized)
		{
			return;
		}
		float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
		if (frameTimeLoss > 0f)
		{
			this._nextActionTime += frameTimeLoss;
		}
		if (this._follower == null)
		{
			this._follower = base.GetComponent<FollowerEntity>();
			if (this._follower == null)
			{
				return;
			}
		}
		if (!(AstarPath.active == null))
		{
			AStarGraphManager instance = AStarGraphManager.Instance;
			if (instance == null || instance.IsReady)
			{
				if (Time.time < this._nextActionTime)
				{
					return;
				}
				if (this._hasDestination)
				{
					if (this._follower.reachedDestination || Vector3.Distance(base.transform.position, this._currentDestination) <= this.arriveDistance)
					{
						this._hasDestination = false;
						this._follower.isStopped = true;
						this._nextActionTime = Time.time + this.waitTime + Random.Range(-this.waitTimeVariance, this.waitTimeVariance);
						if (this.enableDebugLogs)
						{
							Debug.Log("[SimpleAStarWander] " + base.gameObject.name + " raggiunta destinazione");
						}
					}
					return;
				}
				if (this.FindNewDestination())
				{
					this._failedAttempts = 0;
					return;
				}
				this._failedAttempts++;
				this._nextActionTime = Time.time + this.retryDelay * (float)Mathf.Min(this._failedAttempts, 3);
				return;
			}
		}
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0004167C File Offset: 0x0003F87C
	private void OnDisable()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
		if (this._follower != null)
		{
			this._follower.isStopped = true;
		}
		this._hasDestination = false;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x000416CC File Offset: 0x0003F8CC
	private void OnEnable()
	{
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
		this._simulationPauseActive = false;
		NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
		NPCSimulationPause.RegisterParticipant(this);
		this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		if (this._initialized)
		{
			this._nextActionTime = Time.time + Random.Range(0.5f, 1.5f);
			this._hasDestination = false;
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0004173D File Offset: 0x0003F93D
	private void OnWorldOriginShifted(Vector3 delta)
	{
		if (this._hasDestination)
		{
			this._currentDestination += delta;
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00041759 File Offset: 0x0003F959
	private void HandleSimulationPauseChanged(bool paused)
	{
		if (paused)
		{
			this.BeginSimulationPauseIfNeeded();
			return;
		}
		this.ResumeSimulationIfNeeded();
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0004176B File Offset: 0x0003F96B
	private void BeginSimulationPauseIfNeeded()
	{
		if (this._simulationPauseActive)
		{
			return;
		}
		this._simulationPauseActive = true;
		this._simulationPauseStartedAt = Time.time;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00041788 File Offset: 0x0003F988
	private void ResumeSimulationIfNeeded()
	{
		if (!this._simulationPauseActive)
		{
			return;
		}
		this._nextActionTime += Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
		this._simulationPauseActive = false;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x000417C0 File Offset: 0x0003F9C0
	private bool FindNewDestination()
	{
		if (AstarPath.active == null)
		{
			return false;
		}
		NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, NearestNodeConstraint.Walkable);
		if (nearest.node == null)
		{
			return false;
		}
		for (int i = 0; i < this.maxAttempts; i++)
		{
			float num = Random.Range(this.minWanderDistance, this.maxWanderDistance);
			float num2 = Random.Range(0f, 360f) * 0.017453292f;
			Vector3 vector = base.transform.position + new Vector3(Mathf.Cos(num2) * num, 0f, Mathf.Sin(num2) * num);
			NNInfo nearest2 = AstarPath.active.GetNearest(vector, NearestNodeConstraint.Walkable);
			if (nearest2.node != null && nearest2.node.Walkable && PathUtilities.IsPathPossible(nearest.node, nearest2.node))
			{
				this._currentDestination = nearest2.position;
				this._hasDestination = true;
				this._follower.destination = this._currentDestination;
				this._follower.isStopped = false;
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[SimpleAStarWander] {0} -> {1}", base.gameObject.name, this._currentDestination));
				}
				return true;
			}
		}
		if (this.enableDebugLogs)
		{
			Debug.LogWarning("[SimpleAStarWander] " + base.gameObject.name + " no valid destination found");
		}
		return false;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00041938 File Offset: 0x0003FB38
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
		Gizmos.DrawWireSphere(base.transform.position, this.minWanderDistance);
		Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
		Gizmos.DrawWireSphere(base.transform.position, this.maxWanderDistance);
		if (this._hasDestination)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(base.transform.position, this._currentDestination);
			Gizmos.DrawWireSphere(this._currentDestination, 0.5f);
		}
	}

	// Token: 0x04000806 RID: 2054
	[Header("Wander Settings")]
	[Tooltip("Raggio minimo per la destinazione casuale.")]
	public float minWanderDistance = 5f;

	// Token: 0x04000807 RID: 2055
	[Tooltip("Raggio massimo per la destinazione casuale.")]
	public float maxWanderDistance = 20f;

	// Token: 0x04000808 RID: 2056
	[Tooltip("Tempo di attesa alla destinazione prima di scegliere una nuova.")]
	public float waitTime = 2f;

	// Token: 0x04000809 RID: 2057
	[Tooltip("Variazione casuale del tempo di attesa.")]
	public float waitTimeVariance = 1f;

	// Token: 0x0400080A RID: 2058
	[Tooltip("Numero massimo di tentativi per trovare una posizione valida.")]
	public int maxAttempts = 10;

	// Token: 0x0400080B RID: 2059
	[Tooltip("Distanza alla quale considerare raggiunta la destinazione.")]
	public float arriveDistance = 0.5f;

	// Token: 0x0400080C RID: 2060
	[Tooltip("Delay iniziale prima di iniziare a vagare.")]
	public float initialDelay = 0.5f;

	// Token: 0x0400080D RID: 2061
	[Tooltip("Variazione casuale del delay iniziale.")]
	public float initialDelayVariance = 2f;

	// Token: 0x0400080E RID: 2062
	[Header("Retry Settings")]
	[Tooltip("Tempo di attesa se non si trova un path valido.")]
	public float retryDelay = 3f;

	// Token: 0x0400080F RID: 2063
	[Header("Debug")]
	public bool enableDebugLogs;

	// Token: 0x04000810 RID: 2064
	private FollowerEntity _follower;

	// Token: 0x04000811 RID: 2065
	private Vector3 _currentDestination;

	// Token: 0x04000812 RID: 2066
	private float _nextActionTime;

	// Token: 0x04000813 RID: 2067
	private bool _hasDestination;

	// Token: 0x04000814 RID: 2068
	private bool _simulationPauseActive;

	// Token: 0x04000815 RID: 2069
	private float _simulationPauseStartedAt;

	// Token: 0x04000816 RID: 2070
	private bool _initialized;

	// Token: 0x04000817 RID: 2071
	private int _failedAttempts;
}
