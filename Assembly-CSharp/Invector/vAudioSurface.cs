using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Invector
{
	// Token: 0x02000455 RID: 1109
	public class vAudioSurface : ScriptableObject
	{
		// Token: 0x06001BA7 RID: 7079 RVA: 0x000AE2AB File Offset: 0x000AC4AB
		public vAudioSurface()
		{
			this.audioClips = new List<AudioClip>();
			this.TextureOrMaterialNames = new List<string>();
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000AE2E0 File Offset: 0x000AC4E0
		public virtual void SpawnSurfaceEffect(FootStepObject footStepObject)
		{
			if (this.randomSource == null)
			{
				this.randomSource = new vFisherYatesRandom();
			}
			if (footStepObject.spawnSoundEffect)
			{
				this.PlaySound(footStepObject);
			}
			if (footStepObject.spawnParticleEffect && this.particleObject && footStepObject.ground && this.stepLayer.ContainsLayer(footStepObject.ground.gameObject.layer))
			{
				this.SpawnParticle(footStepObject);
			}
			if (footStepObject.spawnStepMarkEffect && this.useStepMark)
			{
				this.StepMark(footStepObject);
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000AE36C File Offset: 0x000AC56C
		protected virtual void PlaySound(FootStepObject footStepObject)
		{
			if (this.audioClips == null || this.audioClips.Count == 0)
			{
				return;
			}
			AudioSource audioSource = null;
			if (this.audioSource != null)
			{
				audioSource = Object.Instantiate<AudioSource>(this.audioSource, footStepObject.sender.position, Quaternion.identity);
				audioSource.transform.SetParent(vObjectContainer.root, true);
			}
			if (this.audioSource && this.audioMixerGroup != null)
			{
				audioSource.outputAudioMixerGroup = this.audioMixerGroup;
			}
			int num = this.randomSource.Next(this.audioClips.Count);
			audioSource.PlayOneShot(this.audioClips[num], footStepObject.volume);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000AE422 File Offset: 0x000AC622
		protected virtual void SpawnParticle(FootStepObject footStepObject)
		{
			Object.Instantiate<GameObject>(this.particleObject, footStepObject.sender.position, footStepObject.sender.rotation).transform.SetParent(vObjectContainer.root, true);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000AE458 File Offset: 0x000AC658
		protected virtual void StepMark(FootStepObject footStep)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(footStep.sender.transform.position + new Vector3(0f, 0.25f, 0f), Vector3.down, out raycastHit, 1f, this.stepLayer) && this.stepMark)
			{
				Quaternion quaternion = Quaternion.FromToRotation(footStep.sender.up, raycastHit.normal);
				GameObject gameObject = Object.Instantiate<GameObject>(this.stepMark, raycastHit.point, quaternion * footStep.sender.rotation);
				gameObject.transform.SetParent(vObjectContainer.root, true);
				Object.Destroy(gameObject, this.timeToDestroy);
			}
		}

		// Token: 0x04001CE8 RID: 7400
		public AudioSource audioSource;

		// Token: 0x04001CE9 RID: 7401
		public AudioMixerGroup audioMixerGroup;

		// Token: 0x04001CEA RID: 7402
		public List<string> TextureOrMaterialNames;

		// Token: 0x04001CEB RID: 7403
		public List<AudioClip> audioClips;

		// Token: 0x04001CEC RID: 7404
		public GameObject particleObject;

		// Token: 0x04001CED RID: 7405
		private vFisherYatesRandom randomSource = new vFisherYatesRandom();

		// Token: 0x04001CEE RID: 7406
		public bool useStepMark;

		// Token: 0x04001CEF RID: 7407
		[vHideInInspector("useStepMark", false)]
		public GameObject stepMark;

		// Token: 0x04001CF0 RID: 7408
		[vHideInInspector("useStepMark", false)]
		public LayerMask stepLayer;

		// Token: 0x04001CF1 RID: 7409
		[vHideInInspector("useStepMark", false)]
		public float timeToDestroy = 5f;
	}
}
