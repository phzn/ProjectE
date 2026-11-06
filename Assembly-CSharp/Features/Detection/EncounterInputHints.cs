using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Localization;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x0200128A RID: 4746
	[DisallowMultipleComponent]
	public sealed class EncounterInputHints : MonoBehaviour
	{
		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06009348 RID: 37704 RVA: 0x003C79ED File Offset: 0x003C5BED
		// (set) Token: 0x06009349 RID: 37705 RVA: 0x003C79F5 File Offset: 0x003C5BF5
		public Canvas PresentationCanvas { get; private set; }

		// Token: 0x0600934A RID: 37706 RVA: 0x003C79FE File Offset: 0x003C5BFE
		public void EmphasizeSkipHint()
		{
			if (this._director != null && this._director.CanSkipEncounter)
			{
				this._skipPulseStartedAt = Time.unscaledTime;
			}
		}

		// Token: 0x0600934B RID: 37707 RVA: 0x003C7A28 File Offset: 0x003C5C28
		public void SetSkipHintVisible(bool visible)
		{
			bool flag = false;
			ScreenFadeController instance = ScreenFadeController.Instance;
			visible &= instance == null || (!instance.IsFading && !instance.IsOpaque);
			GameObject gameObject = ((this._director != null) ? this._director.SkipHintUI : null);
			if (this._presentedEscapeRoot != null && (this._presentedEscapeRoot != gameObject || !visible))
			{
				this.RestoreEscapePresentation();
			}
			if (!visible || gameObject == null)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
				if (this._director == null || !this._director.isActiveAndEnabled || !this._director.CanSkipEncounter)
				{
					this._skipPulseStartedAt = -1f;
					this._skipPulseHasStarted = false;
				}
				return;
			}
			if (this._presentedEscapeRoot == null)
			{
				this.RefreshEscapeHint();
				if (this._escapeText == null)
				{
					return;
				}
				this.EnsurePresentation();
				Transform transform = this._escapeText.transform;
				this._presentedEscapeRoot = gameObject;
				this._escapeBackground = gameObject.GetComponent<Graphic>();
				if (this._escapeBackground != null)
				{
					this._escapeBackgroundWasEnabled = this._escapeBackground.enabled;
					this._escapeBackground.enabled = false;
				}
				this._escapeParent = transform.parent;
				this._escapeSiblingIndex = transform.GetSiblingIndex();
				this._escapeLocalPosition = transform.localPosition;
				this._escapeLocalScale = transform.localScale;
				this._escapeLocalRotation = transform.localRotation;
				RectTransform rectTransform = transform as RectTransform;
				if (rectTransform != null)
				{
					this._escapeAnchorMin = rectTransform.anchorMin;
					this._escapeAnchorMax = rectTransform.anchorMax;
					this._escapePivot = rectTransform.pivot;
					this._escapeSizeDelta = rectTransform.sizeDelta;
					this._escapeAnchoredPosition = rectTransform.anchoredPosition3D;
				}
				transform.SetParent(this.PresentationCanvas.transform, false);
				RectTransform rectTransform2 = this._escapeText.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform2.anchorMax = vector;
				rectTransform2.anchorMin = vector;
				rectTransform2.pivot = new Vector2(0.5f, 1f);
				rectTransform2.anchoredPosition3D = new Vector3(0f, -26f, 0f);
				rectTransform2.sizeDelta = new Vector2(640f, 36f);
				this._escapeText.alignment = TextAlignmentOptions.Center;
				this._escapeText.fontStyle = FontStyles.Normal;
				this._escapeText.characterSpacing = 0f;
				this._escapeText.raycastTarget = false;
				this._escapeBinding.SetSourceLayout(true, false, 16f, 24f);
				this._escapeBinding.FitTranslationToRectangle(true);
				this._pulseTarget = transform;
				if (this._pulseTarget != null)
				{
					this._pulseBaseScale = this._pulseTarget.localScale;
				}
				if (!this._skipPulseHasStarted)
				{
					this._skipPulseStartedAt = Time.unscaledTime;
					this._skipPulseHasStarted = true;
					flag = true;
				}
			}
			gameObject.SetActive(true);
			if (this._pulseTarget != null)
			{
				this._pulseTarget.localScale = this._pulseBaseScale * EncounterInputHints.SkipHintPulseScale(Time.unscaledTime - this._skipPulseStartedAt);
			}
			if (flag)
			{
				UnityEvent onSkipHintShown = this._director.OnSkipHintShown;
				if (onSkipHintShown == null)
				{
					return;
				}
				onSkipHintShown.Invoke();
			}
		}

		// Token: 0x0600934C RID: 37708 RVA: 0x003C7D6C File Offset: 0x003C5F6C
		private static float SkipHintPulseScale(float elapsed)
		{
			if (elapsed < 0f || elapsed >= 3f)
			{
				return 1f;
			}
			float num = Mathf.Sin(elapsed * 3.1415927f * 2f);
			return 1f + 0.08f * num * num * (1f - elapsed / 3f);
		}

		// Token: 0x0600934D RID: 37709 RVA: 0x003C7DC0 File Offset: 0x003C5FC0
		private void RestoreEscapePresentation()
		{
			if (this._pulseTarget != null)
			{
				this._pulseTarget.localScale = this._pulseBaseScale;
			}
			this._pulseTarget = null;
			if (this._presentedEscapeRoot != null)
			{
				this._presentedEscapeRoot.SetActive(false);
				if (this._escapeText != null)
				{
					Transform transform = this._escapeText.transform;
					transform.SetParent(this._escapeParent, false);
					transform.SetSiblingIndex(this._escapeSiblingIndex);
					this.RestoreEscapeGeometry(transform);
				}
			}
			if (this._escapeBackground != null)
			{
				this._escapeBackground.enabled = this._escapeBackgroundWasEnabled;
			}
			this._escapeBackground = null;
			this._presentedEscapeRoot = null;
			this._escapeParent = null;
		}

		// Token: 0x0600934E RID: 37710 RVA: 0x003C7E7C File Offset: 0x003C607C
		private void RestoreEscapeGeometry(Transform panel)
		{
			panel.localPosition = this._escapeLocalPosition;
			panel.localRotation = this._escapeLocalRotation;
			panel.localScale = this._escapeLocalScale;
			RectTransform rectTransform = panel as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.anchorMin = this._escapeAnchorMin;
				rectTransform.anchorMax = this._escapeAnchorMax;
				rectTransform.pivot = this._escapePivot;
				rectTransform.sizeDelta = this._escapeSizeDelta;
				rectTransform.anchoredPosition3D = this._escapeAnchoredPosition;
			}
		}

		// Token: 0x0600934F RID: 37711 RVA: 0x003C7EF4 File Offset: 0x003C60F4
		public void Bind(ExhibitionEventDirector director)
		{
			this._director = director;
			if (director == null)
			{
				return;
			}
			ControllerKeyHint.BindMatching(director.EmoteHintUI, director.SkipInput);
			ControllerKeyHint.BindMatching(director.EmoteHintUI, director.FlyingCameraEnterInput);
			if (director.EmoteHintUI != null)
			{
				foreach (TMP_Text tmp_Text in director.EmoteHintUI.GetComponentsInChildren<TMP_Text>(true))
				{
					if (tmp_Text.transform.parent != null && tmp_Text.transform.parent.name == "Keyboard Input" && string.Equals(tmp_Text.text, "F", StringComparison.OrdinalIgnoreCase))
					{
						ControllerKeyHint.Bind(tmp_Text, director.FlyingCameraEnterInput);
					}
				}
			}
			if (director.DetectionCameraCtrl != null)
			{
				ControllerKeyHint.BindMatching(director.EmoteHintUI, director.DetectionCameraCtrl.CycleCameraInput);
			}
		}

		// Token: 0x06009350 RID: 37712 RVA: 0x003C7FD4 File Offset: 0x003C61D4
		private void LateUpdate()
		{
			this._lastDevice = (ControllerInput.IsActive ? InputDevice.Joystick : InputDevice.MouseKeyboard);
			this.RefreshEscapeHint();
			if (this._director == null || !this._director.IsChoosingEncounterCategory)
			{
				if (this._text != null)
				{
					this._text.enabled = false;
				}
				return;
			}
			this.RefreshPresentation(vInput.instance.inputDevice, FlyCameraPhotoMode.AnyOpen || GameplayInputGate.BlocksGameplayInput);
		}

		// Token: 0x06009351 RID: 37713 RVA: 0x003C8050 File Offset: 0x003C6250
		public void RefreshPresentation(InputDevice device, bool presentationBlocked)
		{
			bool flag = this._director != null && this._director.isActiveAndEnabled && this._director.IsChoosingEncounterCategory && !presentationBlocked;
			this._lastDevice = device;
			this.RefreshEscapeHint();
			string text = (flag ? EncounterInputHints.BuildCategoryHint(this._director.AvailableChoices, device) : string.Empty);
			if (string.IsNullOrEmpty(text))
			{
				if (this._text != null)
				{
					this._text.enabled = false;
				}
				return;
			}
			this.EnsurePresentation();
			if (this._binding == null)
			{
				this._binding = LocalizedGameText.BindComputed(this._text, delegate
				{
					if (!(this._director != null))
					{
						return string.Empty;
					}
					return EncounterInputHints.BuildCategoryHint(this._director.AvailableChoices, this._lastDevice);
				});
				this._binding.FitTranslationToRectangle(false);
			}
			else
			{
				this._text.text = text;
			}
			this._text.enabled = true;
		}

		// Token: 0x06009352 RID: 37714 RVA: 0x003C8130 File Offset: 0x003C6330
		private void RefreshEscapeHint()
		{
			GameObject gameObject = ((this._director != null) ? this._director.SkipHintUI : null);
			if (gameObject == null)
			{
				return;
			}
			string text = EncounterInputLabels.ForInput(this._director.SkipInput, this._lastDevice);
			if (this._escapeRoot != gameObject || this._escapeText == null || this._escapeBinding == null)
			{
				this._escapeRoot = gameObject;
				this._escapeText = null;
				this._escapeBinding = null;
				foreach (TMP_Text tmp_Text in gameObject.GetComponentsInChildren<TMP_Text>(true))
				{
					if (!(tmp_Text.name != "SkipText"))
					{
						this._escapeText = tmp_Text;
						break;
					}
				}
				if (this._escapeText == null)
				{
					return;
				}
				this._escapeBinding = LocalizedGameText.BindComputed(this._escapeText, delegate
				{
					string text2 = EncounterInputLabels.ForInput((this._director != null) ? this._director.SkipInput : null, this._lastDevice);
					if (!string.IsNullOrEmpty(text2))
					{
						return GameLocalization.FormatSource("gameplay_prompts", "Press {0} to Escape", new object[] { text2 });
					}
					return string.Empty;
				});
				this._escapeBinding.FitTranslationToRectangle(false);
			}
			else if (this._escapeDevice != this._lastDevice || this._escapeGlyphVersion != ControllerGlyphs.Version || this._escapeLocale != GameLocalization.CurrentCode || this._escapeSource != text)
			{
				this._escapeBinding.Refresh();
			}
			this._escapeLocale = GameLocalization.CurrentCode;
			this._escapeSource = text;
			this._escapeDevice = this._lastDevice;
			this._escapeGlyphVersion = ControllerGlyphs.Version;
		}

		// Token: 0x06009353 RID: 37715 RVA: 0x003C829E File Offset: 0x003C649E
		private void OnDisable()
		{
			if (this._text != null)
			{
				this._text.enabled = false;
			}
			this.RestoreEscapePresentation();
			this._skipPulseStartedAt = -1f;
			this._skipPulseHasStarted = false;
		}

		// Token: 0x06009354 RID: 37716 RVA: 0x003C82D4 File Offset: 0x003C64D4
		private void EnsurePresentation()
		{
			if (this._text != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("EncounterInputHints", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			gameObject.layer = 5;
			gameObject.transform.SetParent(base.transform, false);
			this.PresentationCanvas = gameObject.GetComponent<Canvas>();
			this.PresentationCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this.PresentationCanvas.sortingOrder = 110;
			GameplayHudVisibility.RegisterCanvas(this.PresentationCanvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			TextMeshProUGUI textMeshProUGUI = ((this._director.EmoteWheel != null) ? this._director.EmoteWheel.ResolveInstructionText() : null);
			this._text = EncounterInputHints.CreateText(gameObject.transform, textMeshProUGUI);
			this._text.enabled = false;
			this._text.rectTransform.anchoredPosition = new Vector2(0f, 64f);
		}

		// Token: 0x06009355 RID: 37717 RVA: 0x003C8404 File Offset: 0x003C6604
		internal static TextMeshProUGUI CreateText(Transform parent, TextMeshProUGUI template = null)
		{
			GameObject gameObject = new GameObject("Instructions", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMin = new Vector2(0.1f, 0f);
			rectTransform.anchorMax = new Vector2(0.9f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.anchoredPosition = new Vector2(0f, 16f);
			rectTransform.sizeDelta = new Vector2(0f, 80f);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			if (template != null)
			{
				component.font = LocalizedGameText.SourceFont(template);
				if (component.font != null)
				{
					component.fontSharedMaterial = ((component.font == template.font) ? template.fontSharedMaterial : component.font.material);
				}
				component.fontStyle = template.fontStyle;
			}
			component.fontSize = 20f;
			component.color = Color.white;
			component.alignment = TextAlignmentOptions.Center;
			component.richText = true;
			component.raycastTarget = false;
			component.enableWordWrapping = true;
			return component;
		}

		// Token: 0x06009356 RID: 37718 RVA: 0x003C856C File Offset: 0x003C676C
		public static string BuildCategoryHint(IList<ExhibitionEventDirector.ChoiceActionConfig> choices, InputDevice device)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (choices != null)
			{
				foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig in choices)
				{
					if (choiceActionConfig != null && !(choiceActionConfig.ButtonUI == null) && choiceActionConfig.ButtonUI.isActiveAndEnabled)
					{
						string text = EncounterInputLabels.ForInput(choiceActionConfig.Input, device);
						if (!string.IsNullOrEmpty(text))
						{
							List<string> list3 = ((choiceActionConfig.HoldDuration > 0f) ? list : list2);
							if (!list3.Contains(text))
							{
								list3.Add(text);
							}
						}
					}
				}
			}
			string text2 = EncounterInputHints.FormatChoiceInstruction("Hold", list);
			string text3 = EncounterInputHints.FormatChoiceInstruction("Press", list2);
			if (string.IsNullOrEmpty(text2))
			{
				return text3;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				return text2 + "\n" + text3;
			}
			return text2;
		}

		// Token: 0x06009357 RID: 37719 RVA: 0x003C8660 File Offset: 0x003C6860
		private static string FormatChoiceInstruction(string verb, List<string> labels)
		{
			if (labels.Count != 0)
			{
				return GameLocalization.FormatSource("gameplay_prompts", (verb == "Hold") ? "Hold <b>{0}</b> to confirm your choice" : "Press <b>{0}</b> to confirm your choice", new object[] { EncounterInputHints.JoinInputAlternatives(labels) });
			}
			return string.Empty;
		}

		// Token: 0x06009358 RID: 37720 RVA: 0x003C86B0 File Offset: 0x003C68B0
		internal static string JoinInputAlternatives(IList<string> labels)
		{
			if (labels == null || labels.Count == 0)
			{
				return string.Empty;
			}
			string text = labels[0];
			for (int i = 1; i < labels.Count; i++)
			{
				text = GameLocalization.FormatSource("gameplay_prompts", "{0}</b> or <b>{1}", new object[]
				{
					text,
					labels[i]
				});
			}
			return text;
		}

		// Token: 0x04009825 RID: 38949
		private ExhibitionEventDirector _director;

		// Token: 0x04009826 RID: 38950
		private TextMeshProUGUI _text;

		// Token: 0x04009827 RID: 38951
		private InputDevice _lastDevice;

		// Token: 0x04009828 RID: 38952
		private LocalizedGameText _binding;

		// Token: 0x04009829 RID: 38953
		private GameObject _escapeRoot;

		// Token: 0x0400982A RID: 38954
		private TMP_Text _escapeText;

		// Token: 0x0400982B RID: 38955
		private LocalizedGameText _escapeBinding;

		// Token: 0x0400982C RID: 38956
		private InputDevice _escapeDevice;

		// Token: 0x0400982D RID: 38957
		private int _escapeGlyphVersion = -1;

		// Token: 0x0400982E RID: 38958
		private string _escapeLocale;

		// Token: 0x0400982F RID: 38959
		private string _escapeSource;

		// Token: 0x04009830 RID: 38960
		private GameObject _presentedEscapeRoot;

		// Token: 0x04009831 RID: 38961
		private Graphic _escapeBackground;

		// Token: 0x04009832 RID: 38962
		private bool _escapeBackgroundWasEnabled;

		// Token: 0x04009833 RID: 38963
		private Transform _escapeParent;

		// Token: 0x04009834 RID: 38964
		private int _escapeSiblingIndex;

		// Token: 0x04009835 RID: 38965
		private Vector3 _escapeLocalPosition;

		// Token: 0x04009836 RID: 38966
		private Vector3 _escapeLocalScale;

		// Token: 0x04009837 RID: 38967
		private Quaternion _escapeLocalRotation;

		// Token: 0x04009838 RID: 38968
		private Vector2 _escapeAnchorMin;

		// Token: 0x04009839 RID: 38969
		private Vector2 _escapeAnchorMax;

		// Token: 0x0400983A RID: 38970
		private Vector2 _escapePivot;

		// Token: 0x0400983B RID: 38971
		private Vector2 _escapeSizeDelta;

		// Token: 0x0400983C RID: 38972
		private Vector3 _escapeAnchoredPosition;

		// Token: 0x0400983D RID: 38973
		private Transform _pulseTarget;

		// Token: 0x0400983E RID: 38974
		private Vector3 _pulseBaseScale;

		// Token: 0x0400983F RID: 38975
		private float _skipPulseStartedAt = -1f;

		// Token: 0x04009840 RID: 38976
		private bool _skipPulseHasStarted;
	}
}
