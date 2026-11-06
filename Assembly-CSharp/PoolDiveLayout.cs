using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D7 RID: 215
[DisallowMultipleComponent]
public class PoolDiveLayout : MonoBehaviour
{
	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0003FEAE File Offset: 0x0003E0AE
	// (set) Token: 0x060008A3 RID: 2211 RVA: 0x0003FEB5 File Offset: 0x0003E0B5
	public static PoolDiveLayout Instance { get; private set; }

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003FEBD File Offset: 0x0003E0BD
	private void Awake()
	{
		PoolDiveLayout.Instance = this;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0003FEC5 File Offset: 0x0003E0C5
	private void OnDestroy()
	{
		if (PoolDiveLayout.Instance == this)
		{
			PoolDiveLayout.Instance = null;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0003FEDA File Offset: 0x0003E0DA
	public bool HasClimbPoints
	{
		get
		{
			return this.climbPoints != null && this.climbPoints.Length != 0;
		}
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0003FEF0 File Offset: 0x0003E0F0
	public List<Vector3> BuildSwimRoute(Vector3 from)
	{
		List<Vector3> list = new List<Vector3>();
		if (this.swimWaypoints == null || this.swimWaypoints.Length == 0)
		{
			return list;
		}
		List<Transform> list2 = new List<Transform>();
		foreach (Transform transform in this.swimWaypoints)
		{
			if (transform != null)
			{
				list2.Add(transform);
			}
		}
		if (list2.Count == 0)
		{
			return list;
		}
		int num = Mathf.Clamp(Random.Range(this.minWaypoints, this.maxWaypoints + 1), 1, list2.Count);
		Vector3 cur = from;
		Comparison<Transform> <>9__0;
		for (int j = 0; j < num; j++)
		{
			List<Transform> list3 = list2;
			Comparison<Transform> comparison;
			if ((comparison = <>9__0) == null)
			{
				comparison = (<>9__0 = (Transform a, Transform b) => (b.position - cur).sqrMagnitude.CompareTo((a.position - cur).sqrMagnitude));
			}
			list3.Sort(comparison);
			int num2 = ((list2.Count >= 2 && Random.value < 0.5f) ? 1 : 0);
			Transform transform2 = list2[num2];
			list.Add(transform2.position);
			cur = transform2.position;
			list2.RemoveAt(num2);
			if (list2.Count == 0)
			{
				break;
			}
		}
		return list;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00040014 File Offset: 0x0003E214
	public PoolClimbPoint PickClimbPoint(Vector3 from)
	{
		if (!this.HasClimbPoints)
		{
			return null;
		}
		List<PoolClimbPoint> list = new List<PoolClimbPoint>();
		foreach (PoolClimbPoint poolClimbPoint in this.climbPoints)
		{
			if (poolClimbPoint != null)
			{
				list.Add(poolClimbPoint);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		list.Sort((PoolClimbPoint a, PoolClimbPoint b) => (a.SwimApproachPosition - from).sqrMagnitude.CompareTo((b.SwimApproachPosition - from).sqrMagnitude));
		int num = ((list.Count >= 2 && Random.value < 0.4f) ? 1 : 0);
		return list[num];
	}

	// Token: 0x040007BE RID: 1982
	[Header("Swim waypoints (in water, shared)")]
	[SerializeField]
	private Transform[] swimWaypoints;

	// Token: 0x040007BF RID: 1983
	[Header("Climb points (exit spots, shared)")]
	[SerializeField]
	private PoolClimbPoint[] climbPoints;

	// Token: 0x040007C0 RID: 1984
	[Header("Route randomisation")]
	[Tooltip("Minimo numero di waypoint percorsi a nuoto prima di uscire.")]
	[SerializeField]
	private int minWaypoints = 1;

	// Token: 0x040007C1 RID: 1985
	[Tooltip("Massimo numero di waypoint percorsi a nuoto prima di uscire.")]
	[SerializeField]
	private int maxWaypoints = 3;

	// Token: 0x040007C2 RID: 1986
	[Header("Water")]
	public float WaterlineY = -5.66f;
}
