using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class vUseRootMotion : StateMachineBehaviour
{
	// Token: 0x06000972 RID: 2418 RVA: 0x00043D73 File Offset: 0x00041F73
	public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ApplyBuiltinRootMotion();
	}
}
