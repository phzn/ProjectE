using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.Utils
{
	// Token: 0x02000604 RID: 1540
	[RequireComponent(typeof(Slider))]
	[RequireComponent(typeof(CanvasGroup))]
	[vClassHeader("ShowHideSlideControl", false, "icon_v2", false, "")]
	public class ShowHideSlideControl : vMonoBehaviour
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x000C9DC0 File Offset: 0x000C7FC0
		private float slideValue
		{
			get
			{
				return (float)Math.Round((double)this.slider.value, 1);
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000C9DD8 File Offset: 0x000C7FD8
		private void Start()
		{
			if (this.slider == null)
			{
				this.slider = base.GetComponent<Slider>();
			}
			if (this.canvasGroup == null)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			if (this.slider != null)
			{
				this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnChangeSlideValue));
				this.CheckValue();
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000C9E49 File Offset: 0x000C8049
		private void CheckValue()
		{
			if (this.slideValue >= this.slider.maxValue)
			{
				this.HideBar();
				return;
			}
			this.ShowBar();
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000C9E6C File Offset: 0x000C806C
		public void ShowBar()
		{
			if (this.inFadeIn || this.canvasGroup.alpha >= 1f)
			{
				return;
			}
			this.inFadeIn = false;
			this.inFadeOut = false;
			if (this.currentRoutine != null)
			{
				base.StopCoroutine(this.currentRoutine);
			}
			this.currentRoutine = base.StartCoroutine(this.ControllBarAlphaRoutine(true));
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000C9ECC File Offset: 0x000C80CC
		public void HideBar()
		{
			if (this.inFadeOut || this.canvasGroup.alpha <= 0f)
			{
				return;
			}
			this.inFadeIn = false;
			this.inFadeOut = false;
			if (this.currentRoutine != null)
			{
				base.StopCoroutine(this.currentRoutine);
			}
			this.currentRoutine = base.StartCoroutine(this.ControllBarAlphaRoutine(false));
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000C9F29 File Offset: 0x000C8129
		private IEnumerator ControllBarAlphaRoutine(bool show = true)
		{
			float alpha = this.canvasGroup.alpha;
			if (show)
			{
				this.inFadeIn = true;
				this.onStartFadeIn.Invoke();
				while (this.canvasGroup.alpha < 1f)
				{
					this.canvasGroup.alpha += Time.deltaTime * this.fadeIn;
					yield return null;
				}
				this.canvasGroup.alpha = 1f;
				this.inFadeIn = false;
				this.onFinishFadeIn.Invoke();
			}
			else
			{
				this.inFadeOut = true;
				this.onStartFadeOut.Invoke();
				while (this.canvasGroup.alpha > 0f)
				{
					this.canvasGroup.alpha -= Time.deltaTime * this.fadeOut;
					yield return null;
				}
				this.canvasGroup.alpha = 0f;
				this.inFadeOut = false;
				this.onFinishFadeOut.Invoke();
			}
			this.currentRoutine = null;
			yield break;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000C9F3F File Offset: 0x000C813F
		private void OnChangeSlideValue(float arg0)
		{
			this.CheckValue();
		}

		// Token: 0x04002396 RID: 9110
		public Slider slider;

		// Token: 0x04002397 RID: 9111
		public CanvasGroup canvasGroup;

		// Token: 0x04002398 RID: 9112
		public float fadeIn = 1f;

		// Token: 0x04002399 RID: 9113
		public float fadeOut = 1f;

		// Token: 0x0400239A RID: 9114
		public UnityEvent onStartFadeIn;

		// Token: 0x0400239B RID: 9115
		public UnityEvent onStartFadeOut;

		// Token: 0x0400239C RID: 9116
		public UnityEvent onFinishFadeIn;

		// Token: 0x0400239D RID: 9117
		public UnityEvent onFinishFadeOut;

		// Token: 0x0400239E RID: 9118
		private Coroutine currentRoutine;

		// Token: 0x0400239F RID: 9119
		private bool inFadeIn;

		// Token: 0x040023A0 RID: 9120
		private bool inFadeOut;
	}
}
