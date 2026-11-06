using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000465 RID: 1125
	public class vTriggerSoundByState : StateMachineBehaviour
	{
		// Token: 0x06001BE8 RID: 7144 RVA: 0x000AF5C6 File Offset: 0x000AD7C6
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.isTrigger = false;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000AF5D0 File Offset: 0x000AD7D0
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.normalizedTime % 1f >= this.triggerTime && !this.isTrigger)
			{
				this.TriggerSound(animator, stateInfo, layerIndex);
				return;
			}
			if (stateInfo.normalizedTime % 1f < this.triggerTime && this.isTrigger)
			{
				this.isTrigger = false;
			}
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000AF628 File Offset: 0x000AD828
		private void TriggerSound(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this._random == null)
			{
				this._random = new vFisherYatesRandom();
			}
			this.isTrigger = true;
			GameObject gameObject;
			if (this.audioSource != null)
			{
				gameObject = Object.Instantiate<GameObject>(this.audioSource.gameObject, animator.transform.position, Quaternion.identity);
			}
			else
			{
				gameObject = new GameObject("audioObject");
				gameObject.transform.position = animator.transform.position;
			}
			if (gameObject != null)
			{
				AudioSource component = gameObject.gameObject.GetComponent<AudioSource>();
				AudioClip audioClip = this.sounds[this._random.Next(this.sounds.Count)];
				component.PlayOneShot(audioClip);
			}
		}

		// Token: 0x04001D40 RID: 7488
		public GameObject audioSource;

		// Token: 0x04001D41 RID: 7489
		public List<AudioClip> sounds;

		// Token: 0x04001D42 RID: 7490
		public float triggerTime;

		// Token: 0x04001D43 RID: 7491
		private vFisherYatesRandom _random;

		// Token: 0x04001D44 RID: 7492
		private bool isTrigger;
	}
}
