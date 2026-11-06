using System;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000E02 RID: 3586
	[DisallowMultipleComponent]
	public class TaserShockAudio : MonoBehaviour
	{
		// Token: 0x06006AD0 RID: 27344 RVA: 0x002ADB3B File Offset: 0x002ABD3B
		public void Bind(AudioSource src)
		{
			this._src = src;
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x002ADB44 File Offset: 0x002ABD44
		public void Play(AudioClip clip)
		{
			if (this._src == null || clip == null)
			{
				return;
			}
			this._src.Stop();
			this._src.clip = clip;
			this._src.volume = 0f;
			this._src.Play();
			this._fadeStart = Time.time;
			this._fading = true;
		}

		// Token: 0x06006AD2 RID: 27346 RVA: 0x002ADBB0 File Offset: 0x002ABDB0
		private void Update()
		{
			if (!this._fading || this._src == null)
			{
				return;
			}
			float num = Mathf.Clamp01((Time.time - this._fadeStart) / 0.08f);
			this._src.volume = Mathf.Lerp(0f, 0.35f, num);
			if (num >= 1f)
			{
				this._fading = false;
			}
		}

		// Token: 0x04006D84 RID: 28036
		private AudioSource _src;

		// Token: 0x04006D85 RID: 28037
		private const float FadeInTime = 0.08f;

		// Token: 0x04006D86 RID: 28038
		private const float TargetVolume = 0.35f;

		// Token: 0x04006D87 RID: 28039
		private float _fadeStart;

		// Token: 0x04006D88 RID: 28040
		private bool _fading;
	}
}
