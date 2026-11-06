using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001044 RID: 4164
	public sealed class MainMenuButtonFx : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		// Token: 0x06007E74 RID: 32372 RVA: 0x0033BA8A File Offset: 0x00339C8A
		public void Initialize(MainMenuPresentationDirector presentation)
		{
			this._presentation = presentation;
		}

		// Token: 0x06007E75 RID: 32373 RVA: 0x0033BA93 File Offset: 0x00339C93
		private void Awake()
		{
			this._rect = base.transform as RectTransform;
			this._baseScale = base.transform.localScale;
			this.CacheGraphics();
		}

		// Token: 0x06007E76 RID: 32374 RVA: 0x0033BABD File Offset: 0x00339CBD
		public void SetTintChildGraphics(bool enabled)
		{
			if (this.tintChildGraphics == enabled && this._graphics != null)
			{
				return;
			}
			this.RestoreColors();
			this.tintChildGraphics = enabled;
			this.CacheGraphics();
		}

		// Token: 0x06007E77 RID: 32375 RVA: 0x0033BAE4 File Offset: 0x00339CE4
		public void SetScaleEffectsEnabled(bool enabled)
		{
			if (this._baseScale.sqrMagnitude < 1E-06f)
			{
				this._baseScale = Vector3.one;
			}
			this.enableScaleEffects = enabled;
			if (!enabled)
			{
				if (this._scaleRoutine != null)
				{
					base.StopCoroutine(this._scaleRoutine);
					this._scaleRoutine = null;
				}
				base.transform.localScale = this._baseScale;
			}
		}

		// Token: 0x06007E78 RID: 32376 RVA: 0x0033BB44 File Offset: 0x00339D44
		private void CacheGraphics()
		{
			Graphic[] array;
			if (!this.tintChildGraphics)
			{
				(array = new Graphic[1])[0] = base.GetComponent<Graphic>();
			}
			else
			{
				array = base.GetComponentsInChildren<Graphic>(true);
			}
			this._graphics = array;
			this._baseColors = new Color[this._graphics.Length];
			for (int i = 0; i < this._graphics.Length; i++)
			{
				this._baseColors[i] = ((this._graphics[i] != null) ? this._graphics[i].color : Color.white);
			}
		}

		// Token: 0x06007E79 RID: 32377 RVA: 0x0033BBD0 File Offset: 0x00339DD0
		private void OnDisable()
		{
			base.StopAllCoroutines();
			base.transform.localScale = this._baseScale;
			this.RestoreColors();
			this._hovered = false;
			this._scaleRoutine = null;
			this._colorRoutine = null;
			this._pulseRoutine = null;
			if (this._activePulse != null)
			{
				Object.Destroy(this._activePulse);
				this._activePulse = null;
			}
		}

		// Token: 0x06007E7A RID: 32378 RVA: 0x0033BC38 File Offset: 0x00339E38
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this == null)
			{
				return;
			}
			this._hovered = true;
			this.PlayHover();
			if (this.enableScaleEffects)
			{
				this.TweenScale(this._baseScale * 1.055f, 0.18f, true);
			}
			this.TweenColors(1.12f, 0.16f);
		}

		// Token: 0x06007E7B RID: 32379 RVA: 0x0033BC90 File Offset: 0x00339E90
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this == null)
			{
				return;
			}
			this._hovered = false;
			if (this.enableScaleEffects)
			{
				this.TweenScale(this._baseScale, 0.16f, false);
			}
			this.TweenColors(1f, 0.18f);
		}

		// Token: 0x06007E7C RID: 32380 RVA: 0x0033BCCD File Offset: 0x00339ECD
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this == null)
			{
				return;
			}
			if (this.enableScaleEffects)
			{
				this.TweenScale(this._baseScale * 0.965f, 0.07f, false);
			}
		}

		// Token: 0x06007E7D RID: 32381 RVA: 0x0033BCFD File Offset: 0x00339EFD
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this == null)
			{
				return;
			}
			if (this.enableScaleEffects)
			{
				this.TweenScale(this._baseScale * (this._hovered ? 1.055f : 1f), 0.14f, true);
			}
		}

		// Token: 0x06007E7E RID: 32382 RVA: 0x0033BD3C File Offset: 0x00339F3C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this == null)
			{
				return;
			}
			this.PlayClick();
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.enableScaleEffects)
			{
				this.TweenScale(this._baseScale * 1.075f, 0.12f, true);
			}
			if (this._rect != null)
			{
				if (this._pulseRoutine != null)
				{
					base.StopCoroutine(this._pulseRoutine);
				}
				if (this._activePulse != null)
				{
					Object.Destroy(this._activePulse);
				}
				this._pulseRoutine = base.StartCoroutine(this.ClickPulse());
			}
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x06007E7F RID: 32383 RVA: 0x0033BDE0 File Offset: 0x00339FE0
		private MainMenuPresentationDirector Presentation
		{
			get
			{
				if (this._presentation == null)
				{
					this._presentation = Object.FindObjectOfType<MainMenuPresentationDirector>();
				}
				return this._presentation;
			}
		}

		// Token: 0x06007E80 RID: 32384 RVA: 0x0033BE04 File Offset: 0x0033A004
		private void PlayHover()
		{
			MainMenuPresentationDirector presentation = this.Presentation;
			if (presentation != null)
			{
				presentation.PlayButtonHover();
				return;
			}
			MainMenuUiFeedbackAudio.PlayHover();
		}

		// Token: 0x06007E81 RID: 32385 RVA: 0x0033BE30 File Offset: 0x0033A030
		private void PlayClick()
		{
			MainMenuPresentationDirector presentation = this.Presentation;
			if (presentation != null)
			{
				presentation.PlayButtonClick();
				return;
			}
			MainMenuUiFeedbackAudio.PlayClick();
		}

		// Token: 0x06007E82 RID: 32386 RVA: 0x0033BE5C File Offset: 0x0033A05C
		private void TweenScale(Vector3 target, float duration, bool overshoot)
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this._scaleRoutine != null)
			{
				base.StopCoroutine(this._scaleRoutine);
			}
			this._scaleRoutine = base.StartCoroutine(this.ScaleRoutine(target, duration, overshoot));
		}

		// Token: 0x06007E83 RID: 32387 RVA: 0x0033BEA8 File Offset: 0x0033A0A8
		private IEnumerator ScaleRoutine(Vector3 target, float duration, bool overshoot)
		{
			Vector3 start = base.transform.localScale;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / Mathf.Max(0.001f, duration));
				float num2 = (overshoot ? (1f - Mathf.Pow(1f - num, 3f) + Mathf.Sin(num * 3.1415927f) * 0.08f) : (1f - Mathf.Pow(1f - num, 3f)));
				base.transform.localScale = Vector3.LerpUnclamped(start, target, num2);
				yield return null;
			}
			base.transform.localScale = target;
			this._scaleRoutine = null;
			yield break;
		}

		// Token: 0x06007E84 RID: 32388 RVA: 0x0033BECC File Offset: 0x0033A0CC
		private void TweenColors(float brightness, float duration)
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this._colorRoutine != null)
			{
				base.StopCoroutine(this._colorRoutine);
			}
			this._colorRoutine = base.StartCoroutine(this.ColorRoutine(brightness, duration));
		}

		// Token: 0x06007E85 RID: 32389 RVA: 0x0033BF0C File Offset: 0x0033A10C
		private IEnumerator ColorRoutine(float brightness, float duration)
		{
			Color[] starts = new Color[this._graphics.Length];
			for (int i = 0; i < this._graphics.Length; i++)
			{
				starts[i] = ((this._graphics[i] != null) ? this._graphics[i].color : Color.white);
			}
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = 1f - Mathf.Pow(1f - Mathf.Clamp01(elapsed / Mathf.Max(0.001f, duration)), 3f);
				for (int j = 0; j < this._graphics.Length; j++)
				{
					if (!(this._graphics[j] == null))
					{
						Color color = this._baseColors[j] * brightness;
						color.a = this._baseColors[j].a;
						this._graphics[j].color = Color.Lerp(starts[j], color, num);
					}
				}
				yield return null;
			}
			this._colorRoutine = null;
			yield break;
		}

		// Token: 0x06007E86 RID: 32390 RVA: 0x0033BF29 File Offset: 0x0033A129
		private IEnumerator ClickPulse()
		{
			GameObject pulseObject = new GameObject("Creamy Click Pulse", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			this._activePulse = pulseObject;
			RectTransform pulseRect = pulseObject.GetComponent<RectTransform>();
			pulseRect.SetParent(base.transform, false);
			pulseRect.anchorMin = Vector2.zero;
			pulseRect.anchorMax = Vector2.one;
			pulseRect.offsetMin = new Vector2(-7f, -7f);
			pulseRect.offsetMax = new Vector2(7f, 7f);
			Image pulse = pulseObject.GetComponent<Image>();
			pulse.raycastTarget = false;
			pulse.color = new Color(1f, 0.48f, 0.68f, 0.24f);
			Vector3 startScale = Vector3.one * 0.94f;
			Vector3 endScale = Vector3.one * 1.13f;
			float elapsed = 0f;
			while (elapsed < 0.28f && pulseRect != null && pulse != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.28f);
				pulseRect.localScale = Vector3.Lerp(startScale, endScale, 1f - Mathf.Pow(1f - num, 3f));
				Color color = pulse.color;
				color.a = 0.24f * (1f - num);
				pulse.color = color;
				yield return null;
			}
			if (pulseObject != null)
			{
				Object.Destroy(pulseObject);
			}
			if (this._activePulse == pulseObject)
			{
				this._activePulse = null;
			}
			this._pulseRoutine = null;
			yield break;
		}

		// Token: 0x06007E87 RID: 32391 RVA: 0x0033BF38 File Offset: 0x0033A138
		private void RestoreColors()
		{
			if (this._graphics == null || this._baseColors == null)
			{
				return;
			}
			for (int i = 0; i < this._graphics.Length; i++)
			{
				if (this._graphics[i] != null)
				{
					this._graphics[i].color = this._baseColors[i];
				}
			}
		}

		// Token: 0x0400820C RID: 33292
		[Tooltip("Se disattivo, l'hover schiarisce solo il Graphic del bottone. Utile per le Collection card: icone e rarita' mantengono i loro colori.")]
		[SerializeField]
		private bool tintChildGraphics = true;

		// Token: 0x0400820D RID: 33293
		[Tooltip("Disattivare sui controlli inseriti in layout stretti (tab/options): mantiene audio e highlight senza far invadere il rettangolo vicino.")]
		[SerializeField]
		private bool enableScaleEffects = true;

		// Token: 0x0400820E RID: 33294
		private RectTransform _rect;

		// Token: 0x0400820F RID: 33295
		private Graphic[] _graphics;

		// Token: 0x04008210 RID: 33296
		private Color[] _baseColors;

		// Token: 0x04008211 RID: 33297
		private Vector3 _baseScale;

		// Token: 0x04008212 RID: 33298
		private MainMenuPresentationDirector _presentation;

		// Token: 0x04008213 RID: 33299
		private Coroutine _scaleRoutine;

		// Token: 0x04008214 RID: 33300
		private Coroutine _colorRoutine;

		// Token: 0x04008215 RID: 33301
		private Coroutine _pulseRoutine;

		// Token: 0x04008216 RID: 33302
		private GameObject _activePulse;

		// Token: 0x04008217 RID: 33303
		private bool _hovered;
	}
}
