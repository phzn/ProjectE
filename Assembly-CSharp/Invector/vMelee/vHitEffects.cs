using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vMelee
{
	// Token: 0x0200050C RID: 1292
	[vClassHeader("Hit Effects", "Search for the 'AudioSource' prefab in the project or create your own custom AudioSource.")]
	public class vHitEffects : vMonoBehaviour
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x000BABF4 File Offset: 0x000B8DF4
		private void Start()
		{
			vMeleeWeapon component = base.GetComponent<vMeleeWeapon>();
			if (component)
			{
				component.onDamageHit.AddListener(new UnityAction<vHitInfo>(this.PlayHitEffects));
				component.onRecoilHit.AddListener(new UnityAction<vHitInfo>(this.PlayRecoilEffects));
				component.onDefense.AddListener(new UnityAction(this.PlayDefenseEffects));
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000BAC58 File Offset: 0x000B8E58
		public void PlayHitEffects(vHitInfo hitInfo)
		{
			if (this.audioSource != null && this.hitSounds.Length != 0)
			{
				AudioClip audioClip = this.hitSounds[Random.Range(0, this.hitSounds.Length)];
				Object.Instantiate<GameObject>(this.audioSource, base.transform.position, base.transform.rotation).GetComponent<AudioSource>().PlayOneShot(audioClip);
			}
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000BACC0 File Offset: 0x000B8EC0
		public void PlayRecoilEffects(vHitInfo hitInfo)
		{
			if (this.audioSource != null && this.recoilSounds.Length != 0)
			{
				AudioClip audioClip = this.recoilSounds[Random.Range(0, this.recoilSounds.Length)];
				Object.Instantiate<GameObject>(this.audioSource, base.transform.position, base.transform.rotation).GetComponent<AudioSource>().PlayOneShot(audioClip);
			}
			if (this.recoilParticles.Length != 0)
			{
				GameObject gameObject = this.recoilParticles[Random.Range(0, this.recoilParticles.Length)];
				Quaternion quaternion = Quaternion.LookRotation(new Vector3(base.transform.position.x, hitInfo.hitPoint.y, base.transform.position.z) - hitInfo.hitPoint);
				if (gameObject != null)
				{
					Object.Instantiate<GameObject>(gameObject, hitInfo.hitPoint, quaternion);
				}
			}
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000BADA0 File Offset: 0x000B8FA0
		public void PlayDefenseEffects()
		{
			if (this.audioSource != null && this.defSounds.Length != 0)
			{
				AudioClip audioClip = this.defSounds[Random.Range(0, this.defSounds.Length)];
				Object.Instantiate<GameObject>(this.audioSource, base.transform.position, base.transform.rotation).GetComponent<AudioSource>().PlayOneShot(audioClip);
			}
		}

		// Token: 0x04002002 RID: 8194
		public GameObject audioSource;

		// Token: 0x04002003 RID: 8195
		public AudioClip[] hitSounds;

		// Token: 0x04002004 RID: 8196
		public AudioClip[] recoilSounds;

		// Token: 0x04002005 RID: 8197
		public GameObject[] recoilParticles;

		// Token: 0x04002006 RID: 8198
		public AudioClip[] defSounds;
	}
}
