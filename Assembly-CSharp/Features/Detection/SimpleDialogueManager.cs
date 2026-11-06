using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Localization;
using Invector.vCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x020012E5 RID: 4837
	public class SimpleDialogueManager : MonoBehaviour
	{
		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x060095FC RID: 38396 RVA: 0x003D9A32 File Offset: 0x003D7C32
		// (set) Token: 0x060095FD RID: 38397 RVA: 0x003D9A39 File Offset: 0x003D7C39
		public static SimpleDialogueManager Instance { get; private set; }

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x060095FE RID: 38398 RVA: 0x003D9A41 File Offset: 0x003D7C41
		public bool IsDialogueActive
		{
			get
			{
				return this._isDialogueActive;
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x060095FF RID: 38399 RVA: 0x003D9A49 File Offset: 0x003D7C49
		public bool IsAutoDialogue
		{
			get
			{
				return this._isAutoDialogue;
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x06009600 RID: 38400 RVA: 0x003D9A51 File Offset: 0x003D7C51
		public GameObject DialoguePanelVisual
		{
			get
			{
				return this.dialoguePanel;
			}
		}

		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x06009601 RID: 38401 RVA: 0x003D9A59 File Offset: 0x003D7C59
		public TextMeshProUGUI SpeakerNameVisual
		{
			get
			{
				return this.speakerNameText;
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x06009602 RID: 38402 RVA: 0x003D9A61 File Offset: 0x003D7C61
		public TextMeshProUGUI DialogueTextVisual
		{
			get
			{
				return this.dialogueText;
			}
		}

		// Token: 0x06009603 RID: 38403 RVA: 0x003D9A69 File Offset: 0x003D7C69
		private void Awake()
		{
			SimpleDialogueManager.Instance = this;
			this.InitializeAudio();
			LocalizedGameText.BindFontOnly(this.dialogueText);
			LocalizedGameText.BindFontOnly(this.speakerNameText);
		}

		// Token: 0x06009604 RID: 38404 RVA: 0x003D9A90 File Offset: 0x003D7C90
		private void InitializeAudio()
		{
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
				if (this.audioSource == null)
				{
					this.audioSource = base.gameObject.AddComponent<AudioSource>();
				}
			}
			this.audioSource.playOnAwake = false;
			this.audioSource.spatialBlend = 0f;
		}

		// Token: 0x06009605 RID: 38405 RVA: 0x003D9AF2 File Offset: 0x003D7CF2
		private void OnEnable()
		{
			SimpleDialogueManager.Instance = this;
			GameLocalization.LocaleChanged += this.RefreshLocalizedPresentation;
			this.RefreshLocalizedPresentation();
		}

		// Token: 0x06009606 RID: 38406 RVA: 0x003D9B11 File Offset: 0x003D7D11
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocalizedPresentation;
			if (SimpleDialogueManager.Instance == this)
			{
				SimpleDialogueManager.Instance = null;
			}
		}

		// Token: 0x06009607 RID: 38407 RVA: 0x003D9B37 File Offset: 0x003D7D37
		private void Update()
		{
			if (!this._isDialogueActive)
			{
				return;
			}
			if (this._isAutoDialogue)
			{
				return;
			}
			if (this.advanceInput.GetButtonDown())
			{
				this.HandleAdvanceInput();
				return;
			}
			this.TickOwnedLineAutoAdvance(Time.deltaTime);
		}

		// Token: 0x06009608 RID: 38408 RVA: 0x003D9B6A File Offset: 0x003D7D6A
		private void HandleAdvanceInput()
		{
			if (!this._isDialogueActive || this._isAutoDialogue || FlyCameraPhotoMode.AnyOpen)
			{
				return;
			}
			if (this._isTyping)
			{
				this.CompleteTypewriter();
				return;
			}
			this.AdvanceDialogue();
		}

		// Token: 0x06009609 RID: 38409 RVA: 0x003D9B9C File Offset: 0x003D7D9C
		public void StartAutoDialogue(DialogueLine[] lines, float dwellDuration, Action onComplete = null)
		{
			if (this._isDialogueActive)
			{
				return;
			}
			if (lines == null || lines.Length == 0)
			{
				if (onComplete != null)
				{
					onComplete();
				}
				return;
			}
			this.StartDialogue(lines, null);
			this._isAutoDialogue = true;
			if (this._autoCloseCoroutine != null)
			{
				base.StopCoroutine(this._autoCloseCoroutine);
			}
			this._autoCloseCoroutine = base.StartCoroutine(this.AutoCloseRoutine(dwellDuration, onComplete));
		}

		// Token: 0x0600960A RID: 38410 RVA: 0x003D9BFA File Offset: 0x003D7DFA
		private IEnumerator AutoCloseRoutine(float dwellDuration, Action onComplete)
		{
			while (this._isTyping)
			{
				yield return null;
			}
			this.ShowContinueIndicator(false);
			float elapsed = 0f;
			while (elapsed < dwellDuration || FlyCameraPhotoMode.AnyOpen)
			{
				if (!FlyCameraPhotoMode.AnyOpen)
				{
					elapsed += Time.deltaTime;
				}
				yield return null;
			}
			this._autoCloseCoroutine = null;
			this._isAutoDialogue = false;
			this._dialogueQueue.Clear();
			this.EndDialogue();
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}

		// Token: 0x0600960B RID: 38411 RVA: 0x003D9C17 File Offset: 0x003D7E17
		public void StartDialogue(DialogueLine[] lines, Action onComplete = null)
		{
			this.StartDialogueInternal(lines, onComplete, null);
		}

		// Token: 0x0600960C RID: 38412 RVA: 0x003D9C22 File Offset: 0x003D7E22
		public void StartOwnedDialogue(object owner, DialogueLine[] lines, Action onComplete = null)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.StartDialogueInternal(lines, onComplete, owner);
		}

		// Token: 0x0600960D RID: 38413 RVA: 0x003D9C3B File Offset: 0x003D7E3B
		public bool IsDialogueOwnedBy(object owner)
		{
			return owner != null && this._isDialogueActive && this._dialogueOwner == owner;
		}

		// Token: 0x0600960E RID: 38414 RVA: 0x003D9C53 File Offset: 0x003D7E53
		public bool SetOwnedLineAutoAdvance(object owner, float dwellDuration)
		{
			if (!this.IsDialogueOwnedBy(owner))
			{
				return false;
			}
			this._ownedLineDwellDuration = ((float.IsNaN(dwellDuration) || float.IsInfinity(dwellDuration)) ? 0f : Mathf.Max(0f, dwellDuration));
			this.ResetOwnedLineDwell();
			return true;
		}

		// Token: 0x0600960F RID: 38415 RVA: 0x003D9C8F File Offset: 0x003D7E8F
		private void ResetOwnedLineDwell()
		{
			this._ownedLineDwellElapsed = 0f;
			this._ownedLineReadyForDwell = false;
		}

		// Token: 0x06009610 RID: 38416 RVA: 0x003D9CA4 File Offset: 0x003D7EA4
		private void TickOwnedLineAutoAdvance(float deltaTime)
		{
			if (!this._isDialogueActive || this._isAutoDialogue || this._dialogueOwner == null || this._ownedLineDwellDuration <= 0f)
			{
				return;
			}
			if (this._isTyping)
			{
				this.ResetOwnedLineDwell();
				return;
			}
			if (FlyCameraPhotoMode.AnyOpen)
			{
				return;
			}
			if (!this._ownedLineReadyForDwell)
			{
				this._ownedLineReadyForDwell = true;
				return;
			}
			this._ownedLineDwellElapsed += Mathf.Max(0f, deltaTime);
			if (this._ownedLineDwellElapsed >= this._ownedLineDwellDuration)
			{
				this.AdvanceDialogue();
			}
		}

		// Token: 0x06009611 RID: 38417 RVA: 0x003D9D2A File Offset: 0x003D7F2A
		public bool TryEndDialogue(object owner, bool invokeCompletion = true)
		{
			if (!this.IsDialogueOwnedBy(owner))
			{
				return false;
			}
			if (invokeCompletion)
			{
				this.ForceEndDialogue();
			}
			else
			{
				this.HideImmediate();
			}
			return true;
		}

		// Token: 0x06009612 RID: 38418 RVA: 0x003D9D4C File Offset: 0x003D7F4C
		private void StartDialogueInternal(DialogueLine[] lines, Action onComplete, object owner)
		{
			if (lines == null || lines.Length == 0)
			{
				if (onComplete != null)
				{
					onComplete();
				}
				return;
			}
			this.ClearDialogueState();
			foreach (DialogueLine dialogueLine in lines)
			{
				this._dialogueQueue.Enqueue(dialogueLine);
			}
			this._onDialogueComplete = onComplete;
			this._dialogueOwner = owner;
			this._isDialogueActive = true;
			if (this.dialoguePanel != null)
			{
				this.dialoguePanel.SetActive(true);
			}
			this.PlaySound(this.dialogueStartSound, this.advanceVolume);
			UnityEvent onDialogueStarted = this.OnDialogueStarted;
			if (onDialogueStarted != null)
			{
				onDialogueStarted.Invoke();
			}
			this.DisplayNextLine();
		}

		// Token: 0x06009613 RID: 38419 RVA: 0x003D9DE8 File Offset: 0x003D7FE8
		public void StartSimpleDialogue(string[] texts, Action onComplete = null)
		{
			DialogueLine[] array = new DialogueLine[texts.Length];
			for (int i = 0; i < texts.Length; i++)
			{
				array[i] = new DialogueLine
				{
					text = texts[i]
				};
			}
			this.StartDialogue(array, onComplete);
		}

		// Token: 0x06009614 RID: 38420 RVA: 0x003D9E28 File Offset: 0x003D8028
		public void StartNPCDialogue(string npcName, string[] texts, Action onComplete = null)
		{
			DialogueLine[] array = new DialogueLine[texts.Length];
			for (int i = 0; i < texts.Length; i++)
			{
				array[i] = new DialogueLine
				{
					speakerName = npcName,
					text = texts[i]
				};
			}
			this.StartDialogue(array, onComplete);
		}

		// Token: 0x06009615 RID: 38421 RVA: 0x003D9E6C File Offset: 0x003D806C
		private void DisplayNextLine()
		{
			this.ResetOwnedLineDwell();
			if (this._dialogueQueue.Count == 0)
			{
				this.EndDialogue();
				return;
			}
			DialogueLine dialogueLine = this._dialogueQueue.Dequeue();
			if (this.speakerNameText != null)
			{
				this.speakerNameText.text = SimpleDialogueManager.ResolveSpeakerName(dialogueLine);
				this.speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(dialogueLine.speakerName));
			}
			this._currentSourceLine = dialogueLine;
			this._currentFullText = SimpleDialogueManager.ResolveLineText(dialogueLine);
			if (this.useTypewriter && this.typewriterSpeed > 0f)
			{
				if (this._typewriterCoroutine != null)
				{
					base.StopCoroutine(this._typewriterCoroutine);
				}
				this._typewriterCoroutine = base.StartCoroutine(this.TypewriterRoutine());
			}
			else
			{
				if (this.dialogueText != null)
				{
					this.dialogueText.text = this._currentFullText;
				}
				this._isTyping = false;
				this.ShowContinueIndicator(true);
			}
			UnityEvent onLineAdvanced = this.OnLineAdvanced;
			if (onLineAdvanced == null)
			{
				return;
			}
			onLineAdvanced.Invoke();
		}

		// Token: 0x06009616 RID: 38422 RVA: 0x003D9F69 File Offset: 0x003D8169
		private IEnumerator TypewriterRoutine()
		{
			this._isTyping = true;
			this.ShowContinueIndicator(false);
			if (this.dialogueText != null)
			{
				this.dialogueText.text = "";
				this._typewriterStep = 0;
				DialogueLine currentSourceLine = this._currentSourceLine;
				this._typewriterTotalSteps = (((currentSourceLine != null) ? currentSourceLine.text : null) ?? this._currentFullText).Length;
				while (this._typewriterStep < this._typewriterTotalSteps)
				{
					this._typewriterStep++;
					int num = this.TypewriterVisibleCharacters();
					this.dialogueText.text = this._currentFullText.Substring(0, num);
					char c = ((num > 0) ? this._currentFullText[num - 1] : ' ');
					if (this.typewriterSound != null && !char.IsWhiteSpace(c) && this._typewriterStep % this.typewriterSoundInterval == 0)
					{
						this.PlayTypewriterSound();
					}
					yield return new WaitForSecondsRealtime(this.typewriterSpeed);
				}
			}
			this._isTyping = false;
			this.ShowContinueIndicator(true);
			this._typewriterCoroutine = null;
			yield break;
		}

		// Token: 0x06009617 RID: 38423 RVA: 0x003D9F78 File Offset: 0x003D8178
		private static string ResolveLineText(DialogueLine line)
		{
			if (line == null)
			{
				return "";
			}
			if (string.IsNullOrEmpty(line.localizationScope))
			{
				return line.text;
			}
			string text;
			if (line.localizationScope == "dialogue" && !GameLocalization.TryGetSourceKey("dialogue", line.text, out text))
			{
				return GameLocalization.GetSource("dialogues_ui", line.text);
			}
			return GameLocalization.GetSource(line.localizationScope, line.text);
		}

		// Token: 0x06009618 RID: 38424 RVA: 0x003D9FEC File Offset: 0x003D81EC
		private static string ResolveSpeakerName(DialogueLine line)
		{
			string text = line.speakerName ?? "";
			if (string.IsNullOrEmpty(line.speakerLocalizationScope))
			{
				return text;
			}
			string text2;
			if (!GameLocalization.TryGetSourceKey(line.speakerLocalizationScope, text, out text2))
			{
				return GameLocalization.GetSource("dialogues_ui", text);
			}
			return GameLocalization.Get(text2, text);
		}

		// Token: 0x06009619 RID: 38425 RVA: 0x003DA03B File Offset: 0x003D823B
		private int TypewriterVisibleCharacters()
		{
			if (this._typewriterTotalSteps > 0)
			{
				return Mathf.Clamp(Mathf.RoundToInt((float)this._currentFullText.Length * ((float)this._typewriterStep / (float)this._typewriterTotalSteps)), 0, this._currentFullText.Length);
			}
			return 0;
		}

		// Token: 0x0600961A RID: 38426 RVA: 0x003DA07C File Offset: 0x003D827C
		private void RefreshLocalizedPresentation()
		{
			if (!this._isDialogueActive || this._currentSourceLine == null)
			{
				return;
			}
			this._currentFullText = SimpleDialogueManager.ResolveLineText(this._currentSourceLine);
			if (this.speakerNameText != null)
			{
				this.speakerNameText.text = SimpleDialogueManager.ResolveSpeakerName(this._currentSourceLine);
			}
			if (this.dialogueText != null)
			{
				this.dialogueText.text = (this._isTyping ? this._currentFullText.Substring(0, this.TypewriterVisibleCharacters()) : this._currentFullText);
			}
		}

		// Token: 0x0600961B RID: 38427 RVA: 0x003DA10C File Offset: 0x003D830C
		private void PlayTypewriterSound()
		{
			if (this.audioSource != null && this.typewriterSound != null)
			{
				this.audioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
				this.audioSource.PlayOneShot(this.typewriterSound, this.typewriterVolume);
			}
		}

		// Token: 0x0600961C RID: 38428 RVA: 0x003DA16C File Offset: 0x003D836C
		private void CompleteTypewriter()
		{
			if (this._typewriterCoroutine != null)
			{
				base.StopCoroutine(this._typewriterCoroutine);
				this._typewriterCoroutine = null;
			}
			if (this.dialogueText != null)
			{
				this.dialogueText.text = this._currentFullText;
			}
			this._isTyping = false;
			this.ResetOwnedLineDwell();
			this.ShowContinueIndicator(true);
		}

		// Token: 0x0600961D RID: 38429 RVA: 0x003DA1C7 File Offset: 0x003D83C7
		private void AdvanceDialogue()
		{
			this.PlaySound(this.advanceSound, this.advanceVolume);
			this.DisplayNextLine();
		}

		// Token: 0x0600961E RID: 38430 RVA: 0x003DA1E1 File Offset: 0x003D83E1
		public void HideImmediate()
		{
			this.ClearDialogueState();
			this.ShowContinueIndicator(false);
			if (this.dialoguePanel != null)
			{
				this.dialoguePanel.SetActive(false);
			}
		}

		// Token: 0x0600961F RID: 38431 RVA: 0x003DA20A File Offset: 0x003D840A
		private void EndDialogue()
		{
			Action onDialogueComplete = this._onDialogueComplete;
			this.HideImmediate();
			this.PlaySound(this.dialogueEndSound, this.advanceVolume);
			UnityEvent onDialogueEnded = this.OnDialogueEnded;
			if (onDialogueEnded != null)
			{
				onDialogueEnded.Invoke();
			}
			if (onDialogueComplete == null)
			{
				return;
			}
			onDialogueComplete();
		}

		// Token: 0x06009620 RID: 38432 RVA: 0x003DA245 File Offset: 0x003D8445
		private void PlaySound(AudioClip clip, float volume)
		{
			if (this.audioSource != null && clip != null)
			{
				this.audioSource.pitch = 1f;
				this.audioSource.PlayOneShot(clip, volume);
			}
		}

		// Token: 0x06009621 RID: 38433 RVA: 0x003DA27B File Offset: 0x003D847B
		private void ShowContinueIndicator(bool show)
		{
			if (this.continueIndicator != null)
			{
				if (show)
				{
					this.RefreshContinueKeyHint();
				}
				this.continueIndicator.SetActive(show);
			}
		}

		// Token: 0x06009622 RID: 38434 RVA: 0x003DA2A0 File Offset: 0x003D84A0
		private void RefreshContinueKeyHint()
		{
			if (this._continueKeyText == null)
			{
				Transform transform = this.continueIndicator.transform.Find("NextIcon");
				if (transform == null)
				{
					return;
				}
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					component.sprite = null;
					component.color = Color.white;
					component.raycastTarget = false;
				}
				RectTransform rectTransform = transform as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.sizeDelta = new Vector2(64f, 28f);
				}
				GameObject gameObject = new GameObject("AdvanceKey", new Type[] { typeof(RectTransform) });
				gameObject.transform.SetParent(transform, false);
				this._continueKeyText = gameObject.AddComponent<TextMeshProUGUI>();
				RectTransform rectTransform2 = this._continueKeyText.rectTransform;
				rectTransform2.anchorMin = Vector2.zero;
				rectTransform2.anchorMax = Vector2.one;
				rectTransform2.offsetMin = new Vector2(3f, 2f);
				rectTransform2.offsetMax = new Vector2(-3f, -2f);
				if (this.dialogueText != null)
				{
					this._continueKeyText.font = LocalizedGameText.SourceFont(this.dialogueText);
				}
				this._continueKeyText.fontSize = 14f;
				this._continueKeyText.enableAutoSizing = true;
				this._continueKeyText.fontSizeMin = 9f;
				this._continueKeyText.fontSizeMax = 14f;
				this._continueKeyText.alignment = TextAlignmentOptions.Center;
				this._continueKeyText.color = new Color(0.15f, 0.1f, 0.16f);
				this._continueKeyText.raycastTarget = false;
			}
			string inputName = ((this.advanceInput != null) ? this.advanceInput.buttonName : "Space");
			LocalizedGameText.BindComputed(this._continueKeyText, delegate
			{
				string text;
				return (GameLocalization.TryGetSourceKey("input", inputName, out text) ? GameLocalization.Get(text, inputName) : GameLocalization.GetSource("phone_story", inputName)).ToUpperInvariant();
			});
		}

		// Token: 0x06009623 RID: 38435 RVA: 0x003DA489 File Offset: 0x003D8689
		public void ForceEndDialogue()
		{
			this.EndDialogue();
		}

		// Token: 0x06009624 RID: 38436 RVA: 0x003DA494 File Offset: 0x003D8694
		private void ClearDialogueState()
		{
			if (this._typewriterCoroutine != null)
			{
				base.StopCoroutine(this._typewriterCoroutine);
				this._typewriterCoroutine = null;
			}
			if (this._autoCloseCoroutine != null)
			{
				base.StopCoroutine(this._autoCloseCoroutine);
				this._autoCloseCoroutine = null;
			}
			this._isDialogueActive = false;
			this._isTyping = false;
			this._isAutoDialogue = false;
			this._ownedLineDwellDuration = 0f;
			this.ResetOwnedLineDwell();
			this._dialogueQueue.Clear();
			this._onDialogueComplete = null;
			this._dialogueOwner = null;
			this._currentSourceLine = null;
			this._typewriterStep = (this._typewriterTotalSteps = 0);
		}

		// Token: 0x04009B1F RID: 39711
		[Header("UI References")]
		[SerializeField]
		private GameObject dialoguePanel;

		// Token: 0x04009B20 RID: 39712
		[SerializeField]
		private TextMeshProUGUI speakerNameText;

		// Token: 0x04009B21 RID: 39713
		[SerializeField]
		private TextMeshProUGUI dialogueText;

		// Token: 0x04009B22 RID: 39714
		[SerializeField]
		private GameObject continueIndicator;

		// Token: 0x04009B23 RID: 39715
		[Header("Typewriter Effect")]
		[SerializeField]
		private bool useTypewriter = true;

		// Token: 0x04009B24 RID: 39716
		[SerializeField]
		private float typewriterSpeed = 0.03f;

		// Token: 0x04009B25 RID: 39717
		[Header("Input")]
		[SerializeField]
		private GenericInput advanceInput = new GenericInput("Space", "A", "A");

		// Token: 0x04009B26 RID: 39718
		[SerializeField]
		private GenericInput skipTypewriterInput = new GenericInput("Space", "A", "A");

		// Token: 0x04009B27 RID: 39719
		[Header("Audio Settings")]
		[Tooltip("AudioSource for dialogue sounds (auto-created if null)")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04009B28 RID: 39720
		[Tooltip("Sound played for each character during typewriter effect")]
		[SerializeField]
		private AudioClip typewriterSound;

		// Token: 0x04009B29 RID: 39721
		[Tooltip("Volume for typewriter sound")]
		[Range(0f, 1f)]
		[SerializeField]
		private float typewriterVolume = 0.3f;

		// Token: 0x04009B2A RID: 39722
		[Tooltip("Play typewriter sound every N characters (1 = every char, 2 = every other, etc.)")]
		[Range(1f, 5f)]
		[SerializeField]
		private int typewriterSoundInterval = 2;

		// Token: 0x04009B2B RID: 39723
		[Tooltip("Sound played when advancing to next dialogue line")]
		[SerializeField]
		private AudioClip advanceSound;

		// Token: 0x04009B2C RID: 39724
		[Tooltip("Volume for advance sound")]
		[Range(0f, 1f)]
		[SerializeField]
		private float advanceVolume = 0.5f;

		// Token: 0x04009B2D RID: 39725
		[Tooltip("Sound played when dialogue starts")]
		[SerializeField]
		private AudioClip dialogueStartSound;

		// Token: 0x04009B2E RID: 39726
		[Tooltip("Sound played when dialogue ends")]
		[SerializeField]
		private AudioClip dialogueEndSound;

		// Token: 0x04009B2F RID: 39727
		[Header("Events")]
		public UnityEvent OnDialogueStarted;

		// Token: 0x04009B30 RID: 39728
		public UnityEvent OnDialogueEnded;

		// Token: 0x04009B31 RID: 39729
		public UnityEvent OnLineAdvanced;

		// Token: 0x04009B32 RID: 39730
		private Queue<DialogueLine> _dialogueQueue = new Queue<DialogueLine>();

		// Token: 0x04009B33 RID: 39731
		private bool _isDialogueActive;

		// Token: 0x04009B34 RID: 39732
		private bool _isTyping;

		// Token: 0x04009B35 RID: 39733
		private string _currentFullText;

		// Token: 0x04009B36 RID: 39734
		private DialogueLine _currentSourceLine;

		// Token: 0x04009B37 RID: 39735
		private int _typewriterStep;

		// Token: 0x04009B38 RID: 39736
		private int _typewriterTotalSteps;

		// Token: 0x04009B39 RID: 39737
		private Coroutine _typewriterCoroutine;

		// Token: 0x04009B3A RID: 39738
		private Action _onDialogueComplete;

		// Token: 0x04009B3B RID: 39739
		private object _dialogueOwner;

		// Token: 0x04009B3C RID: 39740
		private bool _isAutoDialogue;

		// Token: 0x04009B3D RID: 39741
		private Coroutine _autoCloseCoroutine;

		// Token: 0x04009B3E RID: 39742
		private TextMeshProUGUI _continueKeyText;

		// Token: 0x04009B3F RID: 39743
		private float _ownedLineDwellDuration;

		// Token: 0x04009B40 RID: 39744
		private float _ownedLineDwellElapsed;

		// Token: 0x04009B41 RID: 39745
		private bool _ownedLineReadyForDwell;
	}
}
