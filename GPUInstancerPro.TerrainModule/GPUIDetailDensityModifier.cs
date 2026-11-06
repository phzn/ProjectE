using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000019 RID: 25
	public class GPUIDetailDensityModifier : MonoBehaviour
	{
		// Token: 0x06000140 RID: 320 RVA: 0x000094F5 File Offset: 0x000076F5
		private void OnEnable()
		{
			this.SetDetailManager();
			this.ModifyDetailDensity();
			this._bounds = new Bounds(base.transform.position, this.boundsSize);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000951F File Offset: 0x0000771F
		private void Update()
		{
			if (this.applyEveryUpdate)
			{
				this.ModifyDetailDensity();
				return;
			}
			if (this._isExecuted)
			{
				base.enabled = false;
				return;
			}
			this.ModifyDetailDensity();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009546 File Offset: 0x00007746
		private void SetDetailManager()
		{
			if (this.detailManager == null)
			{
				this.detailManager = Object.FindAnyObjectByType<GPUIDetailManager>();
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009564 File Offset: 0x00007764
		private void ModifyDetailDensity()
		{
			if (this.detailManager != null && this.detailManager.IsInitialized)
			{
				if (this.useBounds)
				{
					this._bounds.center = base.transform.position;
					GPUITerrainUtility.SetDetailDensityInsideBounds(this.detailManager, this.densityValue, this._bounds, this.offset, this.selectedPrototypeIndexes);
				}
				else if (this.selectedColliders != null)
				{
					foreach (Collider collider in this.selectedColliders)
					{
						if (collider != null)
						{
							GPUITerrainUtility.SetDetailDensityInsideCollider(this.detailManager, collider, this.densityValue, this.offset, this.selectedPrototypeIndexes);
						}
					}
				}
				this._isExecuted = true;
			}
		}

		// Token: 0x040000E6 RID: 230
		public GPUIDetailManager detailManager;

		// Token: 0x040000E7 RID: 231
		public List<Collider> selectedColliders;

		// Token: 0x040000E8 RID: 232
		public bool useBounds;

		// Token: 0x040000E9 RID: 233
		public Vector3 boundsSize;

		// Token: 0x040000EA RID: 234
		public bool applyEveryUpdate;

		// Token: 0x040000EB RID: 235
		public float offset;

		// Token: 0x040000EC RID: 236
		public List<int> selectedPrototypeIndexes;

		// Token: 0x040000ED RID: 237
		[Range(0f, 255f)]
		public float densityValue;

		// Token: 0x040000EE RID: 238
		private Bounds _bounds;

		// Token: 0x040000EF RID: 239
		private bool _isExecuted;
	}
}
