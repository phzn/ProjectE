using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class vAnimatorIncreaseDecreaseValue : StateMachineBehaviour
{
	// Token: 0x06000967 RID: 2407 RVA: 0x00043C40 File Offset: 0x00041E40
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!this.decrease)
		{
			this.time += Time.deltaTime * this.speed;
		}
		else
		{
			this.time -= Time.deltaTime * this.speed;
		}
		animator.SetFloat(this.targetFloat, this.time);
	}

	// Token: 0x040008A0 RID: 2208
	public string targetFloat;

	// Token: 0x040008A1 RID: 2209
	public bool decrease;

	// Token: 0x040008A2 RID: 2210
	private float time;

	// Token: 0x040008A3 RID: 2211
	public float speed = 1f;
}
