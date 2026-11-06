using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000764 RID: 1892
	public class StayOnMudSurface : MonoBehaviour
	{
		// Token: 0x060030F0 RID: 12528 RVA: 0x00113A09 File Offset: 0x00111C09
		private void Start()
		{
			this.defaultPosition = base.transform.localPosition;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x00113A1C File Offset: 0x00111C1C
		private void FixedUpdate()
		{
			if (this.RefreshOnFixedUpdate)
			{
				this.SnapToMudSurface();
			}
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x00113A2C File Offset: 0x00111C2C
		public void SnapToMudSurface()
		{
			base.transform.position + Vector3.up * this.RaycastDistance;
			this.hits = Physics.RaycastAll(base.transform.position + Vector3.up * this.RaycastDistance, Vector3.down, this.RaycastDistance * 4f);
			foreach (RaycastHit raycastHit in this.hits)
			{
				DynamicMudSurface dynamicMudSurface;
				if (raycastHit.collider.gameObject.TryGetComponent<DynamicMudSurface>(out dynamicMudSurface))
				{
					base.transform.position = raycastHit.point;
					return;
				}
			}
			base.transform.localPosition = this.defaultPosition;
		}

		// Token: 0x0400315A RID: 12634
		public float RaycastDistance = 1f;

		// Token: 0x0400315B RID: 12635
		[SerializeField]
		private bool RefreshOnFixedUpdate = true;

		// Token: 0x0400315C RID: 12636
		private Vector3 defaultPosition;

		// Token: 0x0400315D RID: 12637
		private RaycastHit[] hits;
	}
}
