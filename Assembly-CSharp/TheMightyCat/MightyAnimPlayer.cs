using System;
using UnityEngine;

namespace TheMightyCat
{
	// Token: 0x0200017B RID: 379
	public class MightyAnimPlayer : MonoBehaviour
	{
		// Token: 0x06000BE2 RID: 3042 RVA: 0x0005286C File Offset: 0x00050A6C
		private void Start()
		{
			this._animatorComp = base.GetComponent<Animator>();
			if (!this._animatorComp)
			{
				Debug.LogError("Animator component not found on this GameObject!");
				return;
			}
			if (this.animationClip)
			{
				AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(this._animatorComp.runtimeAnimatorController);
				animatorOverrideController["clip"] = this.animationClip;
				this.animationClip.wrapMode = WrapMode.Loop;
				this._animatorComp.runtimeAnimatorController = animatorOverrideController;
				this._animatorComp.Play("PlayAnimation", 0, 0f);
				return;
			}
			Debug.LogError("No animation clip assigned!");
		}

		// Token: 0x04000BE7 RID: 3047
		public AnimationClip animationClip;

		// Token: 0x04000BE8 RID: 3048
		private Animator _animatorComp;
	}
}
