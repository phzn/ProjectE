using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class BucketRandomizer : MonoBehaviour
{
	// Token: 0x06000A60 RID: 2656 RVA: 0x0004712F File Offset: 0x0004532F
	private void Start()
	{
		this.animator = base.GetComponent<Animator>();
		base.StartCoroutine(this.RandomDump());
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0004714A File Offset: 0x0004534A
	private IEnumerator RandomDump()
	{
		for (;;)
		{
			float num = Random.Range(this.minWaitTime, this.maxWaitTime);
			yield return new WaitForSeconds(num);
			int randomIndex = Random.Range(0, this.animationTriggers.Length);
			this.animator.SetTrigger(this.animationTriggers[randomIndex]);
			yield return new WaitForSeconds(this.dumpAnimations[randomIndex].length);
			this.animator.ResetTrigger(this.animationTriggers[randomIndex]);
			this.animator.SetTrigger("IdleTrigger");
		}
		yield break;
	}

	// Token: 0x04000999 RID: 2457
	public float minWaitTime = 5f;

	// Token: 0x0400099A RID: 2458
	public float maxWaitTime = 10f;

	// Token: 0x0400099B RID: 2459
	public AnimationClip[] dumpAnimations;

	// Token: 0x0400099C RID: 2460
	public string[] animationTriggers;

	// Token: 0x0400099D RID: 2461
	private Animator animator;
}
