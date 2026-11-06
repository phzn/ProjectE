using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000488 RID: 1160
	public class vPickupItem : MonoBehaviour
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x000B0A91 File Offset: 0x000AEC91
		private void Start()
		{
			this._audioSource = base.GetComponent<AudioSource>();
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000B0AA0 File Offset: 0x000AECA0
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player") && !this._audioSource.isPlaying)
			{
				Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				this._audioSource.PlayOneShot(this._audioClip);
				Object.Destroy(base.gameObject, this._audioClip.length);
			}
		}

		// Token: 0x04001DB2 RID: 7602
		private AudioSource _audioSource;

		// Token: 0x04001DB3 RID: 7603
		public AudioClip _audioClip;

		// Token: 0x04001DB4 RID: 7604
		public GameObject _particle;
	}
}
