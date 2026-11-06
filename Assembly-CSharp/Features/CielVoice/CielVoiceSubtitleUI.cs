using System;
using System.Collections;
using DA_Assets.DAG;
using Features.Detection;
using Features.Localization;
using Invector.vCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielVoice
{
	// Token: 0x02001365 RID: 4965
	[DisallowMultipleComponent]
	public class CielVoiceSubtitleUI : MonoBehaviour
	{
		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x0600999F RID: 39327 RVA: 0x003EF2D9 File Offset: 0x003ED4D9
		// (set) Token: 0x060099A0 RID: 39328 RVA: 0x003EF2E0 File Offset: 0x003ED4E0
		public static CielVoiceSubtitleUI Instance { get; private set; }

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x060099A1 RID: 39329 RVA: 0x003EF2E8 File Offset: 0x003ED4E8
		public bool IsVisible
		{
			get
			{
				return this.dialoguePanel != null && this.dialoguePanel.activeInHierarchy && (this._panelGroup == null || this._panelGroup.alpha > 0.001f);
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x060099A2 RID: 39330 RVA: 0x003EF334 File Offset: 0x003ED534
		public string CurrentText
		{
			get
			{
				if (!(this.dialogueText != null))
				{
					return "";
				}
				return this.dialogueText.text;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x060099A3 RID: 39331 RVA: 0x003EF355 File Offset: 0x003ED555
		public float PanelAlpha
		{
			get
			{
				if (!(this._panelGroup != null))
				{
					return 0f;
				}
				return this._panelGroup.alpha;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x060099A4 RID: 39332 RVA: 0x003EF376 File Offset: 0x003ED576
		public float FadeInDuration
		{
			get
			{
				return this.fadeInDuration;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x060099A5 RID: 39333 RVA: 0x003EF37E File Offset: 0x003ED57E
		public float FadeOutDuration
		{
			get
			{
				return this.fadeOutDuration;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x060099A6 RID: 39334 RVA: 0x003EF386 File Offset: 0x003ED586
		// (set) Token: 0x060099A7 RID: 39335 RVA: 0x003EF38E File Offset: 0x003ED58E
		public float LastShowStartedRealtime { get; private set; } = -1f;

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x060099A8 RID: 39336 RVA: 0x003EF397 File Offset: 0x003ED597
		// (set) Token: 0x060099A9 RID: 39337 RVA: 0x003EF39F File Offset: 0x003ED59F
		public float LastRequestedHoldDuration { get; private set; }

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x060099AA RID: 39338 RVA: 0x003EF3A8 File Offset: 0x003ED5A8
		// (set) Token: 0x060099AB RID: 39339 RVA: 0x003EF3B0 File Offset: 0x003ED5B0
		public float LastHiddenRealtime { get; private set; } = -1f;

		// Token: 0x060099AC RID: 39340 RVA: 0x003EF3BC File Offset: 0x003ED5BC
		private void Awake()
		{
			CielVoiceSubtitleUI.Instance = this;
			this.MatchDetectionVisualStyle();
			LocalizedGameText localizedGameText = LocalizedGameText.BindFontOnly(this.dialogueText);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			LocalizedGameText.BindFontOnly(this.speakerNameText);
			this.EnsurePanelGroup();
			if (this._panelGroup != null)
			{
				this._panelGroup.alpha = 0f;
			}
			if (this.dialoguePanel != null)
			{
				this.dialoguePanel.SetActive(false);
			}
			if (this.continueIndicator != null)
			{
				this.continueIndicator.SetActive(false);
			}
		}

		// Token: 0x060099AD RID: 39341 RVA: 0x003EF450 File Offset: 0x003ED650
		private void MatchDetectionVisualStyle()
		{
			SimpleDialogueManager simpleDialogueManager = Object.FindObjectOfType<SimpleDialogueManager>(true);
			if (simpleDialogueManager == null || simpleDialogueManager.DialoguePanelVisual == null || simpleDialogueManager.DialoguePanelVisual == this.dialoguePanel)
			{
				return;
			}
			CielVoiceSubtitleUI.CopyRectTransform(simpleDialogueManager.DialoguePanelVisual.transform as RectTransform, (this.dialoguePanel != null) ? (this.dialoguePanel.transform as RectTransform) : null);
			CielVoiceSubtitleUI.CopyImage(simpleDialogueManager.DialoguePanelVisual.GetComponent<Image>(), (this.dialoguePanel != null) ? this.dialoguePanel.GetComponent<Image>() : null);
			CielVoiceSubtitleUI.CopyGradient(simpleDialogueManager.DialoguePanelVisual.GetComponent<DAGradient>(), (this.dialoguePanel != null) ? this.dialoguePanel.GetComponent<DAGradient>() : null);
			CielVoiceSubtitleUI.CopyTextStyle(simpleDialogueManager.SpeakerNameVisual, this.speakerNameText);
			CielVoiceSubtitleUI.CopyTextStyle(simpleDialogueManager.DialogueTextVisual, this.dialogueText);
		}

		// Token: 0x060099AE RID: 39342 RVA: 0x003EF540 File Offset: 0x003ED740
		private static void CopyRectTransform(RectTransform source, RectTransform target)
		{
			if (source == null || target == null)
			{
				return;
			}
			target.anchorMin = source.anchorMin;
			target.anchorMax = source.anchorMax;
			target.pivot = source.pivot;
			target.anchoredPosition = source.anchoredPosition;
			target.sizeDelta = source.sizeDelta;
			target.localRotation = source.localRotation;
			target.localScale = source.localScale;
		}

		// Token: 0x060099AF RID: 39343 RVA: 0x003EF5B4 File Offset: 0x003ED7B4
		private static void CopyImage(Image source, Image target)
		{
			if (source == null || target == null)
			{
				return;
			}
			target.sprite = source.sprite;
			target.overrideSprite = source.overrideSprite;
			target.type = source.type;
			target.color = source.color;
			target.material = source.material;
			target.preserveAspect = source.preserveAspect;
			target.fillCenter = source.fillCenter;
			target.fillMethod = source.fillMethod;
			target.fillAmount = source.fillAmount;
			target.fillClockwise = source.fillClockwise;
			target.fillOrigin = source.fillOrigin;
			target.pixelsPerUnitMultiplier = source.pixelsPerUnitMultiplier;
		}

		// Token: 0x060099B0 RID: 39344 RVA: 0x003EF664 File Offset: 0x003ED864
		private static void CopyGradient(DAGradient source, DAGradient target)
		{
			if (source == null || target == null)
			{
				return;
			}
			Gradient gradient = new Gradient();
			gradient.SetKeys(source.Gradient.colorKeys, source.Gradient.alphaKeys);
			gradient.mode = source.Gradient.mode;
			target.Gradient = gradient;
			target.BlendMode = source.BlendMode;
			target.Intensity = source.Intensity;
			target.Angle = source.Angle;
			target.enabled = source.enabled;
		}

		// Token: 0x060099B1 RID: 39345 RVA: 0x003EF6F0 File Offset: 0x003ED8F0
		private static void CopyTextStyle(TextMeshProUGUI source, TextMeshProUGUI target)
		{
			if (source == null || target == null)
			{
				return;
			}
			CielVoiceSubtitleUI.CopyRectTransform(source.rectTransform, target.rectTransform);
			target.font = LocalizedGameText.SourceFont(source);
			target.fontSharedMaterial = ((target.font == source.font) ? source.fontSharedMaterial : ((target.font != null) ? target.font.material : source.fontSharedMaterial));
			target.fontSize = source.fontSize;
			target.fontStyle = source.fontStyle;
			target.alignment = source.alignment;
			target.color = source.color;
			target.enableAutoSizing = source.enableAutoSizing;
			target.fontSizeMin = source.fontSizeMin;
			target.fontSizeMax = source.fontSizeMax;
			target.margin = source.margin;
			target.characterSpacing = source.characterSpacing;
			target.wordSpacing = source.wordSpacing;
			target.lineSpacing = source.lineSpacing;
			target.paragraphSpacing = source.paragraphSpacing;
			target.enableWordWrapping = source.enableWordWrapping;
			target.overflowMode = source.overflowMode;
			target.richText = source.richText;
		}

		// Token: 0x060099B2 RID: 39346 RVA: 0x003EF824 File Offset: 0x003EDA24
		private void EnsurePanelGroup()
		{
			if (this._panelGroup != null || this.dialoguePanel == null)
			{
				return;
			}
			this._panelGroup = this.dialoguePanel.GetComponent<CanvasGroup>();
			if (this._panelGroup == null)
			{
				this._panelGroup = this.dialoguePanel.AddComponent<CanvasGroup>();
			}
		}

		// Token: 0x060099B3 RID: 39347 RVA: 0x003EF87E File Offset: 0x003EDA7E
		private void OnEnable()
		{
			CielVoiceSubtitleUI.Instance = this;
			GameLocalization.LocaleChanged += this.RefreshLocalizedText;
			if (this._routine == null)
			{
				this.ClearPanel();
			}
		}

		// Token: 0x060099B4 RID: 39348 RVA: 0x003EF8A5 File Offset: 0x003EDAA5
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocalizedText;
			if (CielVoiceSubtitleUI.Instance == this)
			{
				CielVoiceSubtitleUI.Instance = null;
			}
			this.HideImmediately();
		}

		// Token: 0x060099B5 RID: 39349 RVA: 0x003EF8D1 File Offset: 0x003EDAD1
		private void OnDestroy()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocalizedText;
			if (CielVoiceSubtitleUI.Instance == this)
			{
				CielVoiceSubtitleUI.Instance = null;
			}
		}

		// Token: 0x060099B6 RID: 39350 RVA: 0x003EF8F7 File Offset: 0x003EDAF7
		public void Show(string text, float holdDuration)
		{
			this.Show(text, holdDuration, null);
		}

		// Token: 0x060099B7 RID: 39351 RVA: 0x003EF904 File Offset: 0x003EDB04
		public void Show(string text, float holdDuration, string speaker)
		{
			if (!base.isActiveAndEnabled)
			{
				this.HideImmediately();
				return;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.Hide();
				return;
			}
			this.StopRoutines();
			this.PrepareTextForShow(text);
			if (this.speakerNameText != null)
			{
				this.speakerNameText.text = speaker ?? "";
				this.speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));
			}
			if (this.continueIndicator != null)
			{
				this.continueIndicator.SetActive(false);
			}
			this.EnsurePanelGroup();
			if (this.dialoguePanel != null)
			{
				this.dialoguePanel.SetActive(true);
			}
			this.LastShowStartedRealtime = Time.realtimeSinceStartup;
			this.LastRequestedHoldDuration = Mathf.Max(0f, holdDuration);
			this.LastHiddenRealtime = -1f;
			this._routine = base.StartCoroutine(this.ShowRoutine(this.LastRequestedHoldDuration));
		}

		// Token: 0x060099B8 RID: 39352 RVA: 0x003EF9F4 File Offset: 0x003EDBF4
		public void ShowUntilSkipped(string text, string speaker, Action onSkipped)
		{
			if (!base.isActiveAndEnabled)
			{
				this.HideImmediately();
				if (onSkipped != null)
				{
					onSkipped();
				}
				return;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.Hide();
				if (onSkipped != null)
				{
					onSkipped();
				}
				return;
			}
			this.StopRoutines();
			this.PrepareTextForShow(text);
			if (this.speakerNameText != null)
			{
				this.speakerNameText.text = speaker ?? "";
				this.speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));
			}
			if (this.continueIndicator != null)
			{
				this.continueIndicator.SetActive(false);
			}
			this.EnsurePanelGroup();
			if (this.dialoguePanel != null)
			{
				this.dialoguePanel.SetActive(true);
			}
			this._pendingSkipCallback = onSkipped;
			this._routine = base.StartCoroutine(this.ShowUntilSkippedRoutine());
		}

		// Token: 0x060099B9 RID: 39353 RVA: 0x003EFAD0 File Offset: 0x003EDCD0
		private void PrepareTextForShow(string text)
		{
			this._sourceText = text;
			this._currentFullText = GameLocalization.GetSource("voice_subtitle", text);
			this._typewriterStepCount = text.Length;
			this._revealedSteps = ((this.useTypewriter && this.typewriterSpeed > 0f) ? 0 : this._typewriterStepCount);
			this._isTyping = false;
			if (this.dialogueText == null)
			{
				return;
			}
			this.dialogueText.text = ((this.useTypewriter && this.typewriterSpeed > 0f) ? string.Empty : this._currentFullText);
		}

		// Token: 0x060099BA RID: 39354 RVA: 0x003EFB68 File Offset: 0x003EDD68
		private void RefreshLocalizedText()
		{
			if (string.IsNullOrEmpty(this._sourceText))
			{
				return;
			}
			this._currentFullText = GameLocalization.GetSource("voice_subtitle", this._sourceText);
			this.RenderRevealedText();
		}

		// Token: 0x060099BB RID: 39355 RVA: 0x003EFB94 File Offset: 0x003EDD94
		private void RenderRevealedText()
		{
			if (this.dialogueText == null)
			{
				return;
			}
			int num = ((this._typewriterStepCount <= 0) ? this._currentFullText.Length : Mathf.Clamp((int)((long)this._currentFullText.Length * (long)this._revealedSteps / (long)this._typewriterStepCount), 0, this._currentFullText.Length));
			this.dialogueText.text = this._currentFullText.Substring(0, num);
		}

		// Token: 0x060099BC RID: 39356 RVA: 0x003EFC0D File Offset: 0x003EDE0D
		public void SetTypewriterSpeed(float secondsPerChar)
		{
			this.typewriterSpeed = Mathf.Max(0f, secondsPerChar);
		}

		// Token: 0x060099BD RID: 39357 RVA: 0x003EFC20 File Offset: 0x003EDE20
		public void Hide()
		{
			if (!base.isActiveAndEnabled)
			{
				this.HideImmediately();
				return;
			}
			this.StopRoutines();
			this._isTyping = false;
			if (this.dialoguePanel != null && this.dialoguePanel.activeSelf)
			{
				this._routine = base.StartCoroutine(this.FadeOutAndDisable());
			}
		}

		// Token: 0x060099BE RID: 39358 RVA: 0x003EFC76 File Offset: 0x003EDE76
		public void HideImmediately()
		{
			Action pendingSkipCallback = this._pendingSkipCallback;
			this._pendingSkipCallback = null;
			this.StopRoutines();
			this.ClearPanel();
			if (pendingSkipCallback == null)
			{
				return;
			}
			pendingSkipCallback();
		}

		// Token: 0x060099BF RID: 39359 RVA: 0x003EFC9C File Offset: 0x003EDE9C
		private void ClearPanel()
		{
			this._isTyping = false;
			this._currentFullText = string.Empty;
			this._sourceText = string.Empty;
			this._revealedSteps = (this._typewriterStepCount = 0);
			if (this.dialogueText != null)
			{
				this.dialogueText.text = string.Empty;
			}
			if (this.speakerNameText != null)
			{
				this.speakerNameText.text = string.Empty;
			}
			if (this.continueIndicator != null)
			{
				this.continueIndicator.SetActive(false);
			}
			this.EnsurePanelGroup();
			if (this._panelGroup != null)
			{
				this._panelGroup.alpha = 0f;
			}
			if (this.dialoguePanel != null)
			{
				this.dialoguePanel.SetActive(false);
			}
			this.LastHiddenRealtime = Time.realtimeSinceStartup;
		}

		// Token: 0x060099C0 RID: 39360 RVA: 0x003EFD75 File Offset: 0x003EDF75
		private IEnumerator ShowRoutine(float holdDuration)
		{
			yield return this.Fade((this._panelGroup != null) ? this._panelGroup.alpha : 0f, 1f, this.fadeInDuration);
			if (this.useTypewriter && this.typewriterSpeed > 0f && this.dialogueText != null)
			{
				this._isTyping = true;
				this.dialogueText.text = "";
				while (this._revealedSteps < this._typewriterStepCount)
				{
					this._revealedSteps++;
					this.RenderRevealedText();
					yield return this.WaitStep(this.typewriterSpeed);
				}
				this._isTyping = false;
			}
			else if (this.dialogueText != null)
			{
				this.dialogueText.text = this._currentFullText;
			}
			float t = 0f;
			while (t < holdDuration)
			{
				t += this.DeltaTime();
				yield return null;
			}
			yield return this.FadeOutAndDisable();
			yield break;
		}

		// Token: 0x060099C1 RID: 39361 RVA: 0x003EFD8B File Offset: 0x003EDF8B
		private IEnumerator ShowUntilSkippedRoutine()
		{
			yield return this.Fade((this._panelGroup != null) ? this._panelGroup.alpha : 0f, 1f, this.fadeInDuration);
			if (this.useTypewriter && this.typewriterSpeed > 0f && this.dialogueText != null)
			{
				this._isTyping = true;
				this.dialogueText.text = "";
				while (this._revealedSteps < this._typewriterStepCount)
				{
					this._revealedSteps++;
					this.RenderRevealedText();
					yield return this.WaitStep(this.typewriterSpeed);
				}
				this._isTyping = false;
			}
			else if (this.dialogueText != null)
			{
				this.dialogueText.text = this._currentFullText;
			}
			if (this.continueIndicator != null)
			{
				this.continueIndicator.SetActive(true);
			}
			float graceTimer = 0f;
			while (graceTimer < this.skipInputGrace)
			{
				graceTimer += this.DeltaTime();
				yield return null;
			}
			while (!this.advanceInput.GetButtonDown())
			{
				yield return null;
			}
			if (this.continueIndicator != null)
			{
				this.continueIndicator.SetActive(false);
			}
			yield return this.FadeOutAndDisable();
			Action pendingSkipCallback = this._pendingSkipCallback;
			this._pendingSkipCallback = null;
			if (pendingSkipCallback != null)
			{
				pendingSkipCallback();
			}
			yield break;
		}

		// Token: 0x060099C2 RID: 39362 RVA: 0x003EFD9A File Offset: 0x003EDF9A
		private IEnumerator FadeOutAndDisable()
		{
			yield return this.Fade((this._panelGroup != null) ? this._panelGroup.alpha : 1f, 0f, this.fadeOutDuration);
			this._routine = null;
			this.ClearPanel();
			yield break;
		}

		// Token: 0x060099C3 RID: 39363 RVA: 0x003EFDA9 File Offset: 0x003EDFA9
		private IEnumerator Fade(float from, float to, float duration)
		{
			this.EnsurePanelGroup();
			if (this._panelGroup == null)
			{
				yield break;
			}
			if (duration <= 0f)
			{
				this._panelGroup.alpha = to;
				yield break;
			}
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += this.DeltaTime();
				float num = Mathf.Clamp01(elapsed / duration);
				this._panelGroup.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(this.fadeCurve.Evaluate(num)));
				yield return null;
			}
			this._panelGroup.alpha = to;
			yield break;
		}

		// Token: 0x060099C4 RID: 39364 RVA: 0x003EFDD0 File Offset: 0x003EDFD0
		private void StopRoutines()
		{
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			if (this._typewriterCoroutine != null)
			{
				base.StopCoroutine(this._typewriterCoroutine);
				this._typewriterCoroutine = null;
			}
			if (this._pendingSkipCallback != null)
			{
				Action pendingSkipCallback = this._pendingSkipCallback;
				this._pendingSkipCallback = null;
				pendingSkipCallback();
			}
		}

		// Token: 0x060099C5 RID: 39365 RVA: 0x003EFE2D File Offset: 0x003EE02D
		private object WaitStep(float seconds)
		{
			if (!this.useUnscaledTime)
			{
				return new WaitForSeconds(seconds);
			}
			return new WaitForSecondsRealtime(seconds);
		}

		// Token: 0x060099C6 RID: 39366 RVA: 0x003EFE44 File Offset: 0x003EE044
		private float DeltaTime()
		{
			if (!this.useUnscaledTime)
			{
				return Time.deltaTime;
			}
			return Time.unscaledDeltaTime;
		}

		// Token: 0x04009EBF RID: 40639
		[Header("UI References (clone del DialoguePanel exit)")]
		[Tooltip("Root del pannello sottotitolo (quello col gradient). Attivato/disattivato per mostrare/nascondere.")]
		[SerializeField]
		private GameObject dialoguePanel;

		// Token: 0x04009EC0 RID: 40640
		[Tooltip("TMP del nome parlante. Per la voce di Ciel di norma vuoto/nascosto (versi non verbali). Se una battuta passa uno speaker viene mostrato.")]
		[SerializeField]
		private TextMeshProUGUI speakerNameText;

		// Token: 0x04009EC1 RID: 40641
		[Tooltip("TMP del testo sottotitolo.")]
		[SerializeField]
		private TextMeshProUGUI dialogueText;

		// Token: 0x04009EC2 RID: 40642
		[Tooltip("Indicatore 'continua' del dialogo NPC. Qui resta SEMPRE nascosto: i sottotitoli voce si auto-chiudono, non c'e' avanzamento manuale.")]
		[SerializeField]
		private GameObject continueIndicator;

		// Token: 0x04009EC3 RID: 40643
		[Header("Typewriter (come l'exit)")]
		[Tooltip("Se true il testo compare a macchina da scrivere, come nei dialoghi detection.")]
		[SerializeField]
		private bool useTypewriter = true;

		// Token: 0x04009EC4 RID: 40644
		[Tooltip("Secondi per carattere del typewriter (0.08 = default detection). NB: puo' essere sovrascritto a runtime dal CielVoiceController via SetTypewriterSpeed (velocita' sottotitoli centralizzata nel controller).")]
		[SerializeField]
		private float typewriterSpeed = 0.08f;

		// Token: 0x04009EC5 RID: 40645
		[Tooltip("Usa tempo non scalato, cosi' i sottotitoli restano fluidi anche in slow-mo / wheel aperta.")]
		[SerializeField]
		private bool useUnscaledTime = true;

		// Token: 0x04009EC6 RID: 40646
		[Header("Fade")]
		[Tooltip("Durata (s) del fade-in del pannello all'apparizione. 0 = comparsa istantanea.")]
		[Min(0f)]
		[SerializeField]
		private float fadeInDuration = 0.12f;

		// Token: 0x04009EC7 RID: 40647
		[Tooltip("Durata (s) del fade-out del pannello quando il sottotitolo si chiude. Rende l'uscita morbida invece di uno spegnimento secco. 0 = spegnimento istantaneo.")]
		[Min(0f)]
		[SerializeField]
		private float fadeOutDuration = 0.35f;

		// Token: 0x04009EC8 RID: 40648
		[Tooltip("Curva del fade (X=tempo normalizzato 0..1, Y=alpha 0..1). Default ease-in/out.")]
		[SerializeField]
		private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04009EC9 RID: 40649
		[Header("Skippable (click per continuare)")]
		[Tooltip("Input di avanzamento per i sottotitoli SKIPPABILI (ShowUntilSkipped): quando una battuta chiede l'interazione del player, il sottotitolo resta finche' non si preme questo tasto. Stesso default del dialogo detection (Space / gamepad A).")]
		[SerializeField]
		private GenericInput advanceInput = new GenericInput("Space", "A", "A");

		// Token: 0x04009ECA RID: 40650
		[Tooltip("Grace period (s) dopo l'apparizione del sottotitolo skippabile durante il quale l'input di avanzamento viene ignorato. Evita lo skip involontario se il player stava gia' premendo il tasto quando compare il sottotitolo.")]
		[Min(0f)]
		[SerializeField]
		private float skipInputGrace = 0.35f;

		// Token: 0x04009ECB RID: 40651
		private Coroutine _routine;

		// Token: 0x04009ECC RID: 40652
		private Coroutine _typewriterCoroutine;

		// Token: 0x04009ECD RID: 40653
		private string _currentFullText;

		// Token: 0x04009ECE RID: 40654
		private string _sourceText;

		// Token: 0x04009ECF RID: 40655
		private int _typewriterStepCount;

		// Token: 0x04009ED0 RID: 40656
		private int _revealedSteps;

		// Token: 0x04009ED1 RID: 40657
		private bool _isTyping;

		// Token: 0x04009ED2 RID: 40658
		private Action _pendingSkipCallback;

		// Token: 0x04009ED3 RID: 40659
		private CanvasGroup _panelGroup;
	}
}
