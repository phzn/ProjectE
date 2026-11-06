using System;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x0200051C RID: 1308
	public class vRandomAttackBehaviour : StateMachineBehaviour
	{
		// Token: 0x06001FAA RID: 8106 RVA: 0x000BC2DE File Offset: 0x000BA4DE
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetInteger("RandomAttack", Random.Range(0, this.attackCount));
		}

		// Token: 0x04002071 RID: 8305
		public int attackCount;
	}
}
