using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DA RID: 218
[DisallowMultipleComponent]
public class PoolDivePoint : MonoBehaviour
{
	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x060008AE RID: 2222 RVA: 0x0004015F File Offset: 0x0003E35F
	public bool IsFree
	{
		get
		{
			return this._reservedBy == -1;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x0004016A File Offset: 0x0003E36A
	public Vector3 DiveStandPosition
	{
		get
		{
			return ((this.diveStandPoint != null) ? this.diveStandPoint : base.transform).position;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0004018D File Offset: 0x0003E38D
	public PoolDiveLayout Layout
	{
		get
		{
			if (!(this.layout != null))
			{
				return PoolDiveLayout.Instance;
			}
			return this.layout;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060008B1 RID: 2225 RVA: 0x000401A9 File Offset: 0x0003E3A9
	public float WaterlineY
	{
		get
		{
			if (!(this.Layout != null))
			{
				return -5.66f;
			}
			return this.Layout.WaterlineY;
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000401CA File Offset: 0x0003E3CA
	private void OnEnable()
	{
		if (!PoolDivePoint.All.Contains(this))
		{
			PoolDivePoint.All.Add(this);
		}
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x000401E4 File Offset: 0x0003E3E4
	private void OnDisable()
	{
		PoolDivePoint.All.Remove(this);
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x000401F4 File Offset: 0x0003E3F4
	public bool TryReserve(NPCDiveController npc)
	{
		if (npc == null)
		{
			return false;
		}
		int instanceID = npc.GetInstanceID();
		if (this._reservedBy == instanceID)
		{
			return true;
		}
		if (this._reservedBy != -1)
		{
			return false;
		}
		this._reservedBy = instanceID;
		return true;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00040231 File Offset: 0x0003E431
	public void Release(NPCDiveController npc)
	{
		if (npc != null && this._reservedBy == npc.GetInstanceID())
		{
			this._reservedBy = -1;
		}
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00040254 File Offset: 0x0003E454
	public static PoolDivePoint FindNearestFree(Vector3 from)
	{
		PoolDivePoint poolDivePoint = null;
		float num = float.MaxValue;
		for (int i = 0; i < PoolDivePoint.All.Count; i++)
		{
			PoolDivePoint poolDivePoint2 = PoolDivePoint.All[i];
			if (!(poolDivePoint2 == null) && poolDivePoint2.IsFree)
			{
				float sqrMagnitude = (poolDivePoint2.DiveStandPosition - from).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					poolDivePoint = poolDivePoint2;
				}
			}
		}
		return poolDivePoint;
	}

	// Token: 0x040007C6 RID: 1990
	public static readonly List<PoolDivePoint> All = new List<PoolDivePoint>();

	// Token: 0x040007C7 RID: 1991
	[Tooltip("Spot sul bordo (su navmesh) dove l'NPC si ferma e si tuffa. Vuoto = questo transform.")]
	[SerializeField]
	private Transform diveStandPoint;

	// Token: 0x040007C8 RID: 1992
	[Tooltip("Layout condiviso (waypoint di nuoto + climb points). Se vuoto usa PoolDiveLayout.Instance.")]
	[SerializeField]
	private PoolDiveLayout layout;

	// Token: 0x040007C9 RID: 1993
	private int _reservedBy = -1;
}
