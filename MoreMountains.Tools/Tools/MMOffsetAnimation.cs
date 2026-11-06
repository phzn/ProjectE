using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000181 RID: 385
	[RequireComponent(typeof(Animator))]
	[AddComponentMenu("More Mountains/Tools/Animation/MMOffsetAnimation")]
	public class MMOffsetAnimation : MonoBehaviour
	{
		// Token: 0x060008FD RID: 2301 RVA: 0x00029D98 File Offset: 0x00027F98
		protected virtual void Awake()
		{
			this._animator = base.gameObject.GetComponent<Animator>();
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00029DAB File Offset: 0x00027FAB
		protected virtual void Start()
		{
			if (!this.OffsetOnStart)
			{
				return;
			}
			this.OffsetCurrentAnimation();
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00029DBC File Offset: 0x00027FBC
		protected virtual void OnEnable()
		{
			if (!this.OffsetOnEnable)
			{
				return;
			}
			this.OffsetCurrentAnimation();
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00029DD0 File Offset: 0x00027FD0
		public virtual void OffsetCurrentAnimation()
		{
			this._stateInfo = this._animator.GetCurrentAnimatorStateInfo(this.AnimationLayerID);
			this._animator.Play(this._stateInfo.fullPathHash, -1, Random.Range(this.MinimumRandomRange, this.MaximumRandomRange));
			if (this.DisableAfterOffset)
			{
				base.enabled = false;
			}
		}

		// Token: 0x04000A96 RID: 2710
		public float MinimumRandomRange;

		// Token: 0x04000A97 RID: 2711
		public float MaximumRandomRange = 1f;

		// Token: 0x04000A98 RID: 2712
		public int AnimationLayerID;

		// Token: 0x04000A99 RID: 2713
		public bool OffsetOnStart = true;

		// Token: 0x04000A9A RID: 2714
		public bool OffsetOnEnable;

		// Token: 0x04000A9B RID: 2715
		public bool DisableAfterOffset = true;

		// Token: 0x04000A9C RID: 2716
		protected Animator _animator;

		// Token: 0x04000A9D RID: 2717
		protected AnimatorStateInfo _stateInfo;
	}
}
