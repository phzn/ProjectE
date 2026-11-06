using System;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000940 RID: 2368
	[DisallowMultipleComponent]
	public sealed class WorldRelocationDespawn : MonoBehaviour
	{
		// Token: 0x06004079 RID: 16505 RVA: 0x0017F02D File Offset: 0x0017D22D
		private void OnEnable()
		{
			this._handled = false;
			SubwayTravelService.WorldRelocated += this.HandleWorldRelocated;
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0017F047 File Offset: 0x0017D247
		private void OnDisable()
		{
			SubwayTravelService.WorldRelocated -= this.HandleWorldRelocated;
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x0017F05C File Offset: 0x0017D25C
		private void HandleWorldRelocated(Transform keepAlive)
		{
			if (SubwayTravelService.HasPendingTrip)
			{
				return;
			}
			if (this._handled || this.IsProtectedByKeepAlive(keepAlive))
			{
				return;
			}
			this._handled = true;
			Debug.Log(string.Concat(new string[]
			{
				"[SubwayTravel] '",
				base.name,
				"' lasciato nel vecchio quartiere: ",
				this.disableInsteadOfDestroy ? "disattivato" : "rimosso",
				"."
			}));
			if (this.disableInsteadOfDestroy)
			{
				base.gameObject.SetActive(false);
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0017F0F2 File Offset: 0x0017D2F2
		private bool IsProtectedByKeepAlive(Transform keepAlive)
		{
			return !(keepAlive == null) && (base.transform == keepAlive || base.transform.IsChildOf(keepAlive) || keepAlive.IsChildOf(base.transform));
		}

		// Token: 0x0400404E RID: 16462
		[Tooltip("Usalo solo se un altro sistema deve poter riattivare questa stessa istanza. Di default l'oggetto del vecchio quartiere viene distrutto.")]
		[SerializeField]
		private bool disableInsteadOfDestroy;

		// Token: 0x0400404F RID: 16463
		private bool _handled;
	}
}
