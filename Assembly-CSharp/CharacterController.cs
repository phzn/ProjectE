using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class CharacterController : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000013 RID: 19 RVA: 0x0000239A File Offset: 0x0000059A
	// (set) Token: 0x06000014 RID: 20 RVA: 0x000023D6 File Offset: 0x000005D6
	public Animator animator
	{
		get
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponentInParent<Animator>();
				if (this._animator == null)
				{
					this._animator = base.GetComponentInChildren<Animator>();
				}
			}
			return this._animator;
		}
		private set
		{
			this._animator = value;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000015 RID: 21 RVA: 0x000023DF File Offset: 0x000005DF
	// (set) Token: 0x06000016 RID: 22 RVA: 0x000023E7 File Offset: 0x000005E7
	public float stance { get; private set; }

	// Token: 0x06000017 RID: 23 RVA: 0x000023F0 File Offset: 0x000005F0
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000023F8 File Offset: 0x000005F8
	private void Init()
	{
		this.animator = base.GetComponentInParent<Animator>();
		if (this.animator == null)
		{
			this.animator = base.GetComponentInChildren<Animator>();
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002420 File Offset: 0x00000620
	public void ToggleWalk(bool value)
	{
		this.animator.SetBool("isWalk", value);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002433 File Offset: 0x00000633
	public void SetStance(float value)
	{
		this.animator.SetFloat("Stance", value);
		this.stance = value;
	}

	// Token: 0x04000008 RID: 8
	public Animator _animator;
}
