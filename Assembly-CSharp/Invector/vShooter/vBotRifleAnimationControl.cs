using System;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000641 RID: 1601
	public class vBotRifleAnimationControl : MonoBehaviour
	{
		// Token: 0x060024CE RID: 9422 RVA: 0x000CF04F File Offset: 0x000CD24F
		private void Start()
		{
			this.animator = base.GetComponent<Animator>();
			this.animator.SetFloat("PulseSpeed", this.pulseSpeed);
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000CF073 File Offset: 0x000CD273
		public void OnChangePowerChanger(float value)
		{
			this.animator.SetFloat("PowerCharger", value);
		}

		// Token: 0x0400248F RID: 9359
		public Animator animator;

		// Token: 0x04002490 RID: 9360
		public float pulseSpeed;
	}
}
