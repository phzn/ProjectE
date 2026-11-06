using System;
using MagicaCloth2;
using UnityEngine;

namespace RayznGames.DressRigger
{
	// Token: 0x0200077E RID: 1918
	[DisallowMultipleComponent]
	public sealed class AnimatedGarmentCloth : MonoBehaviour
	{
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x0011D1F8 File Offset: 0x0011B3F8
		public Mesh ReferenceMesh
		{
			get
			{
				return this.referenceMesh;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x0011D200 File Offset: 0x0011B400
		public float ContactAllowance
		{
			get
			{
				return this.contactAllowance;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x0011D208 File Offset: 0x0011B408
		public bool SupportsAnatomy
		{
			get
			{
				return base.isActiveAndEnabled && !this.IsOpening;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x0011D220 File Offset: 0x0011B420
		private bool IsOpening
		{
			get
			{
				if (this.garment == null || this.indices == null)
				{
					return false;
				}
				foreach (int num in this.indices)
				{
					if (num >= 0 && this.garment.GetBlendShapeWeight(num) > 0.01f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0011D278 File Offset: 0x0011B478
		private void OnEnable()
		{
			if (this.garment == null || this.garment.sharedMesh == null)
			{
				return;
			}
			this.indices = new int[this.openingShapes.Length];
			for (int i = 0; i < this.indices.Length; i++)
			{
				this.indices[i] = this.garment.sharedMesh.GetBlendShapeIndex(this.openingShapes[i]);
			}
			MagicaManager.OnPreSimulation = (Action)Delegate.Combine(MagicaManager.OnPreSimulation, new Action(this.UpdateOpening));
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x0011D310 File Offset: 0x0011B510
		private void OnDisable()
		{
			MagicaManager.OnPreSimulation = (Action)Delegate.Remove(MagicaManager.OnPreSimulation, new Action(this.UpdateOpening));
			if (this.suspended && this.cloth != null)
			{
				this.cloth.enabled = true;
			}
			this.suspended = false;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x0011D366 File Offset: 0x0011B566
		private void LateUpdate()
		{
			this.UpdateOpening();
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x0011D370 File Offset: 0x0011B570
		private void UpdateOpening()
		{
			if (this.cloth == null || this.garment == null || this.indices == null)
			{
				return;
			}
			bool isOpening = this.IsOpening;
			if (isOpening == this.suspended)
			{
				return;
			}
			if (!isOpening)
			{
				this.cloth.enabled = true;
				this.cloth.ResetCloth(false);
				this.suspended = false;
				return;
			}
			if (!this.cloth.enabled)
			{
				return;
			}
			this.cloth.enabled = false;
			this.suspended = true;
		}

		// Token: 0x04003317 RID: 13079
		[SerializeField]
		private MagicaCloth cloth;

		// Token: 0x04003318 RID: 13080
		[SerializeField]
		private SkinnedMeshRenderer garment;

		// Token: 0x04003319 RID: 13081
		[SerializeField]
		private Mesh referenceMesh;

		// Token: 0x0400331A RID: 13082
		[SerializeField]
		private string[] openingShapes = Array.Empty<string>();

		// Token: 0x0400331B RID: 13083
		[SerializeField]
		[Min(0f)]
		private float contactAllowance = 0.008f;

		// Token: 0x0400331C RID: 13084
		private int[] indices;

		// Token: 0x0400331D RID: 13085
		private bool suspended;
	}
}
