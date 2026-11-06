using System;
using UnityEngine;
using UnityEngine.UI;

namespace CompassNavigatorPro
{
	// Token: 0x020002A6 RID: 678
	public class LetterAnimator : MonoBehaviour
	{
		// Token: 0x06001346 RID: 4934 RVA: 0x0007CCF6 File Offset: 0x0007AEF6
		private void Awake()
		{
			base.enabled = false;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0007CD00 File Offset: 0x0007AF00
		private void Update()
		{
			float time = Time.time;
			float num = time - this.startTime;
			if (num < this.revealDuration)
			{
				float num2 = Mathf.Clamp01(num / this.revealDuration);
				this.UpdateTextScale(num2);
				this.UpdateTextAlpha(num2);
				return;
			}
			if (time < this.startFadeTime)
			{
				this.UpdateTextScale(1f);
				this.UpdateTextAlpha(1f);
				return;
			}
			if (time < this.startFadeTime + this.fadeDuration)
			{
				float num3 = Mathf.Clamp01(1f - (time - this.startFadeTime) / this.fadeDuration);
				this.UpdateTextAlpha(num3);
				return;
			}
			this.OnAnimationEnds(this.poolIndex);
			base.enabled = false;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0007CDAD File Offset: 0x0007AFAD
		public void Play()
		{
			if (this.originalScale.z == 0f)
			{
				this.originalScale = this.text.transform.localScale;
			}
			base.enabled = true;
			this.Update();
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0007CDE4 File Offset: 0x0007AFE4
		private void UpdateTextScale(float t)
		{
			Vector3 vector = this.originalScale;
			vector.x *= t;
			vector.y *= t;
			this.text.transform.localScale = vector;
			this.textShadow.transform.localScale = vector;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0007CE34 File Offset: 0x0007B034
		private void UpdateTextAlpha(float t)
		{
			this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, t);
			this.textShadow.color = new Color(0f, 0f, 0f, t);
		}

		// Token: 0x040014A9 RID: 5289
		public float startTime;

		// Token: 0x040014AA RID: 5290
		public float revealDuration;

		// Token: 0x040014AB RID: 5291
		public float startFadeTime;

		// Token: 0x040014AC RID: 5292
		public float fadeDuration;

		// Token: 0x040014AD RID: 5293
		public Text text;

		// Token: 0x040014AE RID: 5294
		public Text textShadow;

		// Token: 0x040014AF RID: 5295
		public int poolIndex;

		// Token: 0x040014B0 RID: 5296
		public OnAnimationEndDelegate OnAnimationEnds;

		// Token: 0x040014B1 RID: 5297
		public bool used;

		// Token: 0x040014B2 RID: 5298
		private Vector3 originalScale;
	}
}
