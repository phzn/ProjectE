using System;
using UnityEngine;

// Token: 0x020000D6 RID: 214
[DisallowMultipleComponent]
public class PoolClimbPoint : MonoBehaviour
{
	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x0600089E RID: 2206 RVA: 0x0003FE60 File Offset: 0x0003E060
	public Vector3 SwimApproachPosition
	{
		get
		{
			return ((this.swimApproach != null) ? this.swimApproach : base.transform).position;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x0600089F RID: 2207 RVA: 0x0003FE83 File Offset: 0x0003E083
	public Transform MatchTarget
	{
		get
		{
			return this.matchTarget;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0003FE8B File Offset: 0x0003E08B
	public Vector3 StandPosition
	{
		get
		{
			return ((this.standPoint != null) ? this.standPoint : base.transform).position;
		}
	}

	// Token: 0x040007BA RID: 1978
	[Tooltip("Punto in acqua, davanti al bordo, verso cui l'NPC nuota prima di arrampicarsi. Se vuoto usa questo transform proiettato alla superficie.")]
	[SerializeField]
	private Transform swimApproach;

	// Token: 0x040007BB RID: 1979
	[Tooltip("Il transform 'target' del vTriggerGenericAction Invector: punto ESATTO a cui si allinea la mano durante la clip ClimbUp (lo snap preciso del player).")]
	[SerializeField]
	private Transform matchTarget;

	// Token: 0x040007BC RID: 1980
	[Tooltip("Spot ASCIUTTO sul bordo (su navmesh) dove l'NPC cammina DOPO il climb, prima di riprendere il wandering.")]
	[SerializeField]
	private Transform standPoint;
}
