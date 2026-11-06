using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.Utils
{
	// Token: 0x0200060D RID: 1549
	[vClassHeader("Fade Canvas", true, "icon_v2", false, "")]
	public class vFadeCanvas : vMonoBehaviour
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x000CA2EB File Offset: 0x000C84EB
		// (set) Token: 0x060023AE RID: 9134 RVA: 0x000CA2F3 File Offset: 0x000C84F3
		protected virtual float currentValue { get; set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000CA2FC File Offset: 0x000C84FC
		// (set) Token: 0x060023B0 RID: 9136 RVA: 0x000CA304 File Offset: 0x000C8504
		protected virtual bool inFade { get; set; }

		// Token: 0x060023B1 RID: 9137 RVA: 0x000CA30D File Offset: 0x000C850D
		private void Awake()
		{
			if (!this.group)
			{
				this.group = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000CA328 File Offset: 0x000C8528
		protected virtual void Start()
		{
			this.InitilizeFadeEffect();
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000CA328 File Offset: 0x000C8528
		private void OnEnable()
		{
			this.InitilizeFadeEffect();
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000CA330 File Offset: 0x000C8530
		protected virtual void InitilizeFadeEffect()
		{
			if (this.fadeInStart)
			{
				this.FadeIn();
			}
			if (this.fadeOutStart)
			{
				this.FadeOut();
			}
			if (this.startWithAlphaZero)
			{
				this.AlphaZero();
			}
			if (this.startWithAlphaFull)
			{
				this.AlphaFull();
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000CA36A File Offset: 0x000C856A
		public virtual void AlphaZero()
		{
			if (this.group)
			{
				this.group.alpha = 0f;
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000CA389 File Offset: 0x000C8589
		public virtual void AlphaFull()
		{
			if (this.group)
			{
				this.group.alpha = 1f;
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000CA3A8 File Offset: 0x000C85A8
		public virtual void FadeIn()
		{
			base.StartCoroutine(this.Fade(1f));
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000CA3BC File Offset: 0x000C85BC
		public virtual void FadeOut()
		{
			base.StartCoroutine(this.Fade(0f));
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000CA3D0 File Offset: 0x000C85D0
		public IEnumerator Fade(float targetValue)
		{
			if (targetValue == 1f)
			{
				this.onStartFadeIn.Invoke();
				if (this.autoControlCanvasGroup && this.group)
				{
					this.group.interactable = false;
					this.group.blocksRaycasts = true;
				}
			}
			else
			{
				if (this.autoControlCanvasGroup && this.group)
				{
					this.group.interactable = false;
					this.group.blocksRaycasts = true;
				}
				this.onStartFadeOut.Invoke();
			}
			this.inFade = false;
			yield return new WaitForEndOfFrame();
			this.inFade = true;
			if (this.group)
			{
				this.currentValue = this.group.alpha;
			}
			while (((targetValue == 1f) ? (this.currentValue < 1f) : (this.currentValue > 0f)) && this.inFade)
			{
				yield return null;
				this.currentValue = ((targetValue == 1f) ? (this.currentValue + Time.unscaledDeltaTime * this.fadeSpeed) : (this.currentValue - Time.unscaledDeltaTime * this.fadeSpeed));
				if (this.group)
				{
					this.group.alpha = this.currentValue;
				}
				this.OnChangeValue.Invoke(this.currentValue);
			}
			if (targetValue == 1f)
			{
				this.onFinishFadeIn.Invoke();
				if (this.autoControlCanvasGroup && this.group)
				{
					this.group.interactable = true;
					this.group.blocksRaycasts = true;
				}
			}
			else
			{
				if (this.autoControlCanvasGroup && this.group)
				{
					this.group.interactable = false;
					this.group.blocksRaycasts = false;
				}
				this.onFinishFadeOut.Invoke();
			}
			yield break;
		}

		// Token: 0x040023B1 RID: 9137
		public CanvasGroup group;

		// Token: 0x040023B2 RID: 9138
		public float fadeSpeed = 2f;

		// Token: 0x040023B3 RID: 9139
		public UnityEvent onStartFadeIn;

		// Token: 0x040023B4 RID: 9140
		public UnityEvent onFinishFadeIn;

		// Token: 0x040023B5 RID: 9141
		public UnityEvent onStartFadeOut;

		// Token: 0x040023B6 RID: 9142
		public UnityEvent onFinishFadeOut;

		// Token: 0x040023B7 RID: 9143
		public Slider.SliderEvent OnChangeValue;

		// Token: 0x040023B8 RID: 9144
		public bool autoControlCanvasGroup;

		// Token: 0x040023B9 RID: 9145
		public bool fadeInStart;

		// Token: 0x040023BA RID: 9146
		public bool fadeOutStart;

		// Token: 0x040023BB RID: 9147
		public bool startWithAlphaZero = true;

		// Token: 0x040023BC RID: 9148
		public bool startWithAlphaFull;
	}
}
