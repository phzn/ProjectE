using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class vAnimatorSetTrigger : StateMachineBehaviour
{
	// Token: 0x06000969 RID: 2409 RVA: 0x00043CAE File Offset: 0x00041EAE
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.setOnEnter)
		{
			animator.SetTrigger(this.trigger);
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00043CC4 File Offset: 0x00041EC4
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.setOnExit)
		{
			animator.SetTrigger(this.trigger);
		}
	}

	// Token: 0x040008A4 RID: 2212
	public bool setOnEnter;

	// Token: 0x040008A5 RID: 2213
	public bool setOnExit;

	// Token: 0x040008A6 RID: 2214
	public string trigger;
}
