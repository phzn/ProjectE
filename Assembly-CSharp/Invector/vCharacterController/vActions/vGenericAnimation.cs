using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000719 RID: 1817
	[vClassHeader("Generic Animation", "Use this script to trigger a simple animation.")]
	public class vGenericAnimation : vMonoBehaviour
	{
		// Token: 0x06002F1C RID: 12060 RVA: 0x00109D07 File Offset: 0x00107F07
		protected virtual void Start()
		{
			this.tpInput = base.GetComponent<vThirdPersonInput>();
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x00109D15 File Offset: 0x00107F15
		protected virtual void LateUpdate()
		{
			this.TriggerAnimation();
			this.AnimationBehaviour();
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x00109D24 File Offset: 0x00107F24
		protected virtual void TriggerAnimation()
		{
			bool flag = !this.isPlaying && !this.tpInput.cc.customAction && !string.IsNullOrEmpty(this.animationClip);
			if (this.actionInput.GetButtonDown() && flag)
			{
				this.PlayAnimation();
			}
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x00109D72 File Offset: 0x00107F72
		public virtual void PlayAnimation()
		{
			this.triggerOnce = true;
			this.OnPlayAnimation.Invoke();
			this.tpInput.cc.animator.CrossFadeInFixedTime(this.animationClip, 0.1f);
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x00109DA8 File Offset: 0x00107FA8
		protected virtual void AnimationBehaviour()
		{
			this.isPlaying = this.tpInput.cc.baseLayerInfo.IsName(this.animationClip);
			if (this.isPlaying && this.tpInput.cc.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= this.animationEnd && this.triggerOnce)
			{
				this.triggerOnce = false;
				this.OnEndAnimation.Invoke();
			}
		}

		// Token: 0x04002F49 RID: 12105
		[Tooltip("Input to trigger the custom animation")]
		public GenericInput actionInput = new GenericInput("L", "A", "A");

		// Token: 0x04002F4A RID: 12106
		[Tooltip("Name of the animation clip")]
		public string animationClip;

		// Token: 0x04002F4B RID: 12107
		[Tooltip("Where in the end of the animation will trigger the event OnEndAnimation")]
		public float animationEnd = 0.8f;

		// Token: 0x04002F4C RID: 12108
		public UnityEvent OnPlayAnimation;

		// Token: 0x04002F4D RID: 12109
		public UnityEvent OnEndAnimation;

		// Token: 0x04002F4E RID: 12110
		protected bool isPlaying;

		// Token: 0x04002F4F RID: 12111
		protected bool triggerOnce;

		// Token: 0x04002F50 RID: 12112
		protected vThirdPersonInput tpInput;
	}
}
