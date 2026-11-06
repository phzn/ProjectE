using System;
using Invector;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class vChangeAnimatorUpdateMode : MonoBehaviour
{
	// Token: 0x0600096C RID: 2412 RVA: 0x00043CE2 File Offset: 0x00041EE2
	public void Reset()
	{
		this.animator = base.GetComponentInParent<Animator>();
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00043CF0 File Offset: 0x00041EF0
	private void Start()
	{
		if (!this.animator)
		{
			this.animator = base.GetComponentInParent<Animator>();
		}
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00043D0B File Offset: 0x00041F0B
	public void ChangeToUnscaledTime()
	{
		if (Time.timeScale == 0f)
		{
			vTime.useUnscaledTime = true;
			this.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00043D2B File Offset: 0x00041F2B
	public void ChangeToAnimatePhysics()
	{
		this.animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
		vTime.useUnscaledTime = false;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00043D3F File Offset: 0x00041F3F
	public void ChangeToInitialState()
	{
		this.animator.updateMode = this.initialState;
		vTime.useUnscaledTime = this.initialState == AnimatorUpdateMode.UnscaledTime;
	}

	// Token: 0x040008A7 RID: 2215
	public Animator animator;

	// Token: 0x040008A8 RID: 2216
	private readonly AnimatorUpdateMode initialState = AnimatorUpdateMode.AnimatePhysics;
}
