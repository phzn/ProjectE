using System;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000DE7 RID: 3559
	[DisallowMultipleComponent]
	public class AudioFadeOutAndDestroy : MonoBehaviour
	{
		// Token: 0x0600697C RID: 27004 RVA: 0x002A3C64 File Offset: 0x002A1E64
		public void Begin(AudioSource source, float fadeTime)
		{
			this._source = source;
			this._fadeTime = Mathf.Max(0.01f, fadeTime);
			this._startVolume = ((source != null) ? source.volume : 0f);
			this._elapsed = 0f;
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x002A3CB0 File Offset: 0x002A1EB0
		private void Update()
		{
			if (this._source == null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			this._elapsed += Time.deltaTime;
			float num = Mathf.Clamp01(this._elapsed / this._fadeTime);
			this._source.volume = Mathf.Lerp(this._startVolume, 0f, num);
			if (num >= 1f)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04006C4E RID: 27726
		private AudioSource _source;

		// Token: 0x04006C4F RID: 27727
		private float _startVolume;

		// Token: 0x04006C50 RID: 27728
		private float _fadeTime;

		// Token: 0x04006C51 RID: 27729
		private float _elapsed;
	}
}
