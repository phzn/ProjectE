using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x020005FC RID: 1532
	public class vSetAnimatorLayerWeight : MonoBehaviour
	{
		// Token: 0x0600236B RID: 9067 RVA: 0x000C9865 File Offset: 0x000C7A65
		private void Update()
		{
			this.currentWeight = Mathf.Lerp(this.currentWeight, this.weight, this.speed * Time.deltaTime);
			this.animator.SetLayerWeight(this.layer, this.currentWeight);
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000C98A1 File Offset: 0x000C7AA1
		public void SetWeight(float value)
		{
			this.weight = value;
		}

		// Token: 0x0400237D RID: 9085
		public Animator animator;

		// Token: 0x0400237E RID: 9086
		public int layer;

		// Token: 0x0400237F RID: 9087
		public float weight;

		// Token: 0x04002380 RID: 9088
		public float speed;

		// Token: 0x04002381 RID: 9089
		[vReadOnly(true)]
		protected float currentWeight;
	}
}
