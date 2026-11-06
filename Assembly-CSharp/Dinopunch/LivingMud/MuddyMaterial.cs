using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000756 RID: 1878
	public class MuddyMaterial : MonoBehaviour
	{
		// Token: 0x06003092 RID: 12434 RVA: 0x00112550 File Offset: 0x00110750
		public void Awake()
		{
			if (this.OriginalMaterial == null)
			{
				Renderer renderer;
				if (!base.TryGetComponent<Renderer>(out renderer))
				{
					return;
				}
				if (renderer.material == null)
				{
					return;
				}
				this.OriginalMaterial = renderer.material;
			}
			this.material = new Material(this.OriginalMaterial);
			base.GetComponent<Renderer>().material = this.material;
			if (this.material && this.material.HasProperty("_Mud_Amount"))
			{
				this.mudAmount = this.material.GetFloat("_Mud_Amount");
				this.material.SetFloat("_Mud_Amount", 0f);
			}
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x00112600 File Offset: 0x00110800
		public void MakeMuddy()
		{
			if (this.material && this.material.HasProperty("_Mud_Amount"))
			{
				float @float = this.material.GetFloat("_Mud_Amount");
				this.material.SetFloat("_Mud_Amount", @float + this.MudAmount);
			}
		}

		// Token: 0x04003109 RID: 12553
		public Material OriginalMaterial;

		// Token: 0x0400310A RID: 12554
		public float MudAmount = 0.1f;

		// Token: 0x0400310B RID: 12555
		private Material material;

		// Token: 0x0400310C RID: 12556
		private float mudAmount;
	}
}
