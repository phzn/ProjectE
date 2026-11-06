using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x020005FB RID: 1531
	public class vResetTrigger : StateMachineBehaviour
	{
		// Token: 0x06002368 RID: 9064 RVA: 0x000C9839 File Offset: 0x000C7A39
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.resetOnEnter)
			{
				animator.ResetTrigger(this.trigger);
			}
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000C984F File Offset: 0x000C7A4F
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.resetOnExit)
			{
				animator.ResetTrigger(this.trigger);
			}
		}

		// Token: 0x0400237A RID: 9082
		public bool resetOnEnter;

		// Token: 0x0400237B RID: 9083
		public bool resetOnExit;

		// Token: 0x0400237C RID: 9084
		public string trigger;
	}
}
