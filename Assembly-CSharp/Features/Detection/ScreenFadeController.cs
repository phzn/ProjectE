using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x020012DF RID: 4831
	[DefaultExecutionOrder(-100)]
	public class ScreenFadeController : MonoBehaviour
	{
		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x060095BF RID: 38335 RVA: 0x003D7BA7 File Offset: 0x003D5DA7
		// (set) Token: 0x060095C0 RID: 38336 RVA: 0x003D7BAE File Offset: 0x003D5DAE
		public static ScreenFadeController Instance { get; private set; }

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x060095C1 RID: 38337 RVA: 0x003D7BB6 File Offset: 0x003D5DB6
		public bool IsFading
		{
			get
			{
				return this._isFading;
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x060095C2 RID: 38338 RVA: 0x003D7BBE File Offset: 0x003D5DBE
		public bool IsOpaque
		{
			get
			{
				return this.fadeImage != null && this.fadeImage.isActiveAndEnabled && this.fadeImage.color.a >= 0.999f;
			}
		}

		// Token: 0x060095C3 RID: 38339 RVA: 0x003D7BF8 File Offset: 0x003D5DF8
		private void Awake()
		{
			if (!(ScreenFadeController.Instance == null))
			{
				Object.Destroy(base.gameObject);
				return;
			}
			ScreenFadeController.Instance = this;
			if (this.fadeImage == null)
			{
				Debug.LogError("[ScreenFadeController] FadeImage is NULL! Assign it in Inspector.");
				return;
			}
			if (this.startBlack)
			{
				this.fadeImage.color = new Color(this.fadeColor.r, this.fadeColor.g, this.fadeColor.b, 1f);
				this.fadeImage.raycastTarget = true;
				this.fadeImage.gameObject.SetActive(true);
				return;
			}
			this.fadeImage.color = new Color(this.fadeColor.r, this.fadeColor.g, this.fadeColor.b, 0f);
			this.fadeImage.raycastTarget = false;
			this.fadeImage.gameObject.SetActive(false);
		}

		// Token: 0x060095C4 RID: 38340 RVA: 0x003D7CEE File Offset: 0x003D5EEE
		private void OnDestroy()
		{
			if (ScreenFadeController.Instance == this)
			{
				ScreenFadeController.Instance = null;
			}
		}

		// Token: 0x060095C5 RID: 38341 RVA: 0x003D7D04 File Offset: 0x003D5F04
		public void FadeOut(float duration = -1f, Action onComplete = null)
		{
			if (this.fadeImage == null)
			{
				Debug.LogWarning("[ScreenFadeController] No fade image assigned!");
				if (onComplete != null)
				{
					onComplete();
				}
				return;
			}
			float num = ((duration > 0f) ? duration : this.defaultFadeDuration);
			this.StartFade(1f, num, onComplete);
		}

		// Token: 0x060095C6 RID: 38342 RVA: 0x003D7D54 File Offset: 0x003D5F54
		public void FadeIn(float duration = -1f, Action onComplete = null)
		{
			if (this.fadeImage == null)
			{
				Debug.LogWarning("[ScreenFadeController] No fade image assigned!");
				if (onComplete != null)
				{
					onComplete();
				}
				return;
			}
			float num = ((duration > 0f) ? duration : this.defaultFadeDuration);
			this.StartFade(0f, num, onComplete);
		}

		// Token: 0x060095C7 RID: 38343 RVA: 0x003D7DA2 File Offset: 0x003D5FA2
		public void FadeOutIn(Action middleAction, float fadeOutDuration = -1f, float fadeInDuration = -1f, float holdDuration = 0.1f, Action onComplete = null)
		{
			base.StartCoroutine(this.FadeOutInRoutine(middleAction, fadeOutDuration, fadeInDuration, holdDuration, onComplete));
		}

		// Token: 0x060095C8 RID: 38344 RVA: 0x003D7DB8 File Offset: 0x003D5FB8
		private IEnumerator FadeOutInRoutine(Action middleAction, float fadeOutDuration, float fadeInDuration, float holdDuration, Action onComplete)
		{
			bool fadeOutComplete = false;
			this.FadeOut(fadeOutDuration, delegate
			{
				fadeOutComplete = true;
			});
			while (!fadeOutComplete)
			{
				yield return null;
			}
			if (middleAction != null)
			{
				middleAction();
			}
			if (holdDuration > 0f)
			{
				yield return new WaitForSecondsRealtime(holdDuration);
			}
			bool fadeInComplete = false;
			this.FadeIn(fadeInDuration, delegate
			{
				fadeInComplete = true;
			});
			while (!fadeInComplete)
			{
				yield return null;
			}
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}

		// Token: 0x060095C9 RID: 38345 RVA: 0x003D7DEC File Offset: 0x003D5FEC
		public void SetBlack()
		{
			if (this._currentFade != null)
			{
				base.StopCoroutine(this._currentFade);
				this._currentFade = null;
			}
			if (this.fadeImage != null)
			{
				this.fadeImage.gameObject.SetActive(true);
				this.fadeImage.color = new Color(this.fadeColor.r, this.fadeColor.g, this.fadeColor.b, 1f);
				this.fadeImage.raycastTarget = true;
			}
			this._isFading = false;
		}

		// Token: 0x060095CA RID: 38346 RVA: 0x003D7E7C File Offset: 0x003D607C
		public void SetClear()
		{
			base.StopAllCoroutines();
			this._currentFade = null;
			if (this.fadeImage != null)
			{
				this.fadeImage.color = new Color(this.fadeColor.r, this.fadeColor.g, this.fadeColor.b, 0f);
				this.fadeImage.raycastTarget = false;
			}
			this._isFading = false;
		}

		// Token: 0x060095CB RID: 38347 RVA: 0x003D7EF0 File Offset: 0x003D60F0
		private void StartFade(float targetAlpha, float duration, Action onComplete)
		{
			if (this._currentFade != null)
			{
				base.StopCoroutine(this._currentFade);
			}
			if (this.fadeImage != null && !this.fadeImage.gameObject.activeSelf)
			{
				this.fadeImage.gameObject.SetActive(true);
			}
			this._currentFade = base.StartCoroutine(this.FadeRoutine(targetAlpha, duration, onComplete));
		}

		// Token: 0x060095CC RID: 38348 RVA: 0x003D7F57 File Offset: 0x003D6157
		private IEnumerator FadeRoutine(float targetAlpha, float duration, Action onComplete)
		{
			this._isFading = true;
			float startAlpha = this.fadeImage.color.a;
			float elapsed = 0f;
			if (targetAlpha > 0.5f)
			{
				this.fadeImage.raycastTarget = true;
			}
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = Mathf.Lerp(startAlpha, targetAlpha, num);
				this.fadeImage.color = new Color(this.fadeColor.r, this.fadeColor.g, this.fadeColor.b, num2);
				yield return null;
			}
			this.fadeImage.color = new Color(this.fadeColor.r, this.fadeColor.g, this.fadeColor.b, targetAlpha);
			if (targetAlpha < 0.1f)
			{
				this.fadeImage.raycastTarget = false;
				this.fadeImage.gameObject.SetActive(false);
			}
			this._isFading = false;
			this._currentFade = null;
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}

		// Token: 0x04009AE2 RID: 39650
		[Header("Configuration")]
		[Tooltip("Image used for fade effect (should be full-screen black)")]
		[SerializeField]
		private Image fadeImage;

		// Token: 0x04009AE3 RID: 39651
		[Tooltip("Default fade duration in seconds")]
		[SerializeField]
		private float defaultFadeDuration = 0.5f;

		// Token: 0x04009AE4 RID: 39652
		[Tooltip("Color to fade to (usually black)")]
		[SerializeField]
		private Color fadeColor = Color.black;

		// Token: 0x04009AE5 RID: 39653
		[Tooltip("Se true, la scena parte con lo schermo GIÀ NERO: in Awake la fadeImage NON viene azzerata ma lasciata opaca+attiva, così non si vede alcun frame della scena finché qualcuno non chiama FadeIn(). Usare per scene cutscene (es. Prison intro).")]
		[SerializeField]
		private bool startBlack;

		// Token: 0x04009AE6 RID: 39654
		private Coroutine _currentFade;

		// Token: 0x04009AE7 RID: 39655
		private bool _isFading;
	}
}
