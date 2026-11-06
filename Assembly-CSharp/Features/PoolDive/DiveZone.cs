using System;
using System.Collections.Generic;
using Features.GoldBikini;
using UnityEngine;

namespace Features.PoolDive
{
	// Token: 0x02000DD4 RID: 3540
	[RequireComponent(typeof(Collider))]
	public class DiveZone : MonoBehaviour
	{
		// Token: 0x060068BD RID: 26813 RVA: 0x0029E3EC File Offset: 0x0029C5EC
		private void Reset()
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				component.isTrigger = true;
			}
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x0029E410 File Offset: 0x0029C610
		private void OnTriggerEnter(Collider other)
		{
			if (!DiveZone.IsPlayerCollider(other))
			{
				return;
			}
			PoolDiveController poolDiveController = this.ResolveController(other);
			BikiniSlipDiveController bikiniSlipDiveController = ((this._resolvedSlipDive != null) ? this._resolvedSlipDive : (other.GetComponentInParent<BikiniSlipDiveController>(true) ?? other.GetComponentInChildren<BikiniSlipDiveController>(true)));
			if (poolDiveController == null && bikiniSlipDiveController == null)
			{
				return;
			}
			if (!this._playerColliders.Add(other) || this._playerColliders.Count > 1)
			{
				return;
			}
			if (poolDiveController != null)
			{
				this._resolved = poolDiveController;
				poolDiveController.SetPlayerInZone(this, true);
			}
			if (bikiniSlipDiveController != null)
			{
				this._resolvedSlipDive = bikiniSlipDiveController;
				bikiniSlipDiveController.SetPlayerInZone(this, true);
			}
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x0029E4B8 File Offset: 0x0029C6B8
		private void OnTriggerExit(Collider other)
		{
			if (!this._playerColliders.Remove(other) || this._playerColliders.Count > 0)
			{
				return;
			}
			PoolDiveController poolDiveController = ((this._resolved != null) ? this._resolved : this.ResolveController(other));
			if (poolDiveController != null)
			{
				poolDiveController.SetPlayerInZone(this, false);
			}
			if (this._resolvedSlipDive != null)
			{
				this._resolvedSlipDive.SetPlayerInZone(this, false);
			}
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x0029E52C File Offset: 0x0029C72C
		private void OnDisable()
		{
			if (this._playerColliders.Count == 0)
			{
				return;
			}
			this._playerColliders.Clear();
			if (this._resolved != null)
			{
				this._resolved.SetPlayerInZone(this, false);
			}
			if (this._resolvedSlipDive != null)
			{
				this._resolvedSlipDive.SetPlayerInZone(this, false);
			}
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x0029E588 File Offset: 0x0029C788
		private PoolDiveController ResolveController(Collider other)
		{
			if (this.diveController != null)
			{
				return this.diveController;
			}
			PoolDiveController poolDiveController = other.GetComponentInParent<PoolDiveController>(true);
			if (poolDiveController == null)
			{
				poolDiveController = other.GetComponentInChildren<PoolDiveController>(true);
			}
			return poolDiveController;
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x0029E5C4 File Offset: 0x0029C7C4
		private static bool IsPlayerCollider(Collider other)
		{
			return !(other == null) && (other.CompareTag("Player") || (other.GetComponentInParent<PoolDiveController>(true) != null || other.GetComponentInChildren<PoolDiveController>(true) != null) || other.GetComponentInParent<BikiniSlipDiveController>(true) != null || other.GetComponentInChildren<BikiniSlipDiveController>(true) != null);
		}

		// Token: 0x04006B50 RID: 27472
		[Tooltip("Optional explicit controller. If null, resolved from the entering player.")]
		[SerializeField]
		private PoolDiveController diveController;

		// Token: 0x04006B51 RID: 27473
		private PoolDiveController _resolved;

		// Token: 0x04006B52 RID: 27474
		private BikiniSlipDiveController _resolvedSlipDive;

		// Token: 0x04006B53 RID: 27475
		private readonly HashSet<Collider> _playerColliders = new HashSet<Collider>();
	}
}
