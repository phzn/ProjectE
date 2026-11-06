using System;
using Invector.vCharacterController;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x0200124F RID: 4687
	public class ChoiceButtonUI : MonoBehaviour
	{
		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x060091C5 RID: 37317 RVA: 0x003BBC41 File Offset: 0x003B9E41
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x060091C6 RID: 37318 RVA: 0x003BBC49 File Offset: 0x003B9E49
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x060091C7 RID: 37319 RVA: 0x003BBC51 File Offset: 0x003B9E51
		private void Awake()
		{
			if (this.ScaleTarget == null)
			{
				this.ScaleTarget = base.GetComponent<RectTransform>();
			}
			if (this.ButtonCanvasGroup == null)
			{
				this.ButtonCanvasGroup = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x060091C8 RID: 37320 RVA: 0x003BBC88 File Offset: 0x003B9E88
		public void Initialize(GenericInput input, float holdDuration, string displayLabel, Action onConfirm)
		{
			this._assignedInput = input;
			this._requiredDuration = holdDuration;
			this._onConfirm = onConfirm;
			this._currentHoldTime = 0f;
			this._isCompleted = false;
			this._isSelected = false;
			this._isDimmed = false;
			this._isPressing = false;
			this._targetScale = this.NormalScale;
			this._targetAlpha = this.NormalAlpha;
			this.RefreshBindingLabel();
			if (this.FillBar)
			{
				this.FillBar.fillAmount = 0f;
				this.FillBar.color = this.NormalColor;
			}
			if (this.HintGO != null)
			{
				this.HintGO.SetActive(true);
			}
			this.ApplyVisualsInstant();
		}

		// Token: 0x060091C9 RID: 37321 RVA: 0x003BBD40 File Offset: 0x003B9F40
		public void UpdateInput()
		{
			this.RefreshBindingLabel();
			if (this._isCompleted || this._assignedInput == null)
			{
				return;
			}
			if (this._requiredDuration <= 0f)
			{
				if (this._assignedInput.GetButtonDown())
				{
					this.CompleteSelection();
				}
			}
			else if (this._assignedInput.GetButton())
			{
				if (!this._isPressing)
				{
					this._isPressing = true;
					if (this.PulseFeedback != null)
					{
						this.PulseFeedback.PlayFeedbacks();
					}
				}
				this._currentHoldTime += Time.unscaledDeltaTime;
				if (this.FillBar)
				{
					this.FillBar.color = this.PressedColor;
					this.FillBar.fillAmount = Mathf.Clamp01(this._currentHoldTime / this._requiredDuration);
				}
				if (this._currentHoldTime >= this._requiredDuration)
				{
					this.CompleteSelection();
				}
			}
			else
			{
				if (this._isPressing)
				{
					this._isPressing = false;
					if (this.PulseFeedback != null)
					{
						this.PulseFeedback.StopFeedbacks();
					}
				}
				this._currentHoldTime = 0f;
				if (this.FillBar)
				{
					this.FillBar.color = this.NormalColor;
					this.FillBar.fillAmount = 0f;
				}
			}
			this.AnimateVisuals();
		}

		// Token: 0x060091CA RID: 37322 RVA: 0x003BBE8E File Offset: 0x003BA08E
		public void RefreshBindingLabel()
		{
			if (this.KeyText == null)
			{
				return;
			}
			this.KeyText.text = EncounterInputLabels.ForInput(this._assignedInput, vInput.instance.inputDevice);
		}

		// Token: 0x060091CB RID: 37323 RVA: 0x003BBEC0 File Offset: 0x003BA0C0
		public void SetSelectedState()
		{
			this._isSelected = true;
			this._isDimmed = false;
			this._targetScale = this.SelectedScale;
			this._targetAlpha = this.SelectedAlpha;
			if (this.FillBar)
			{
				this.FillBar.color = this.PressedColor;
			}
		}

		// Token: 0x060091CC RID: 37324 RVA: 0x003BBF11 File Offset: 0x003BA111
		public void SetDimmedState()
		{
			this._isSelected = false;
			this._isDimmed = true;
			this._targetScale = this.DimmedScale;
			this._targetAlpha = this.DimmedAlpha;
		}

		// Token: 0x060091CD RID: 37325 RVA: 0x003BBF39 File Offset: 0x003BA139
		public void HideHint()
		{
			if (this.HintGO != null)
			{
				this.HintGO.SetActive(false);
			}
		}

		// Token: 0x060091CE RID: 37326 RVA: 0x003BBF58 File Offset: 0x003BA158
		public void ResetState()
		{
			this._currentHoldTime = 0f;
			this._isCompleted = false;
			this._isSelected = false;
			this._isDimmed = false;
			this._isPressing = false;
			this._onConfirm = null;
			this._targetScale = this.NormalScale;
			this._targetAlpha = this.NormalAlpha;
			if (this.FillBar)
			{
				this.FillBar.fillAmount = 0f;
				this.FillBar.color = this.NormalColor;
			}
			if (this.HintGO != null)
			{
				this.HintGO.SetActive(true);
			}
			this.ApplyVisualsInstant();
		}

		// Token: 0x060091CF RID: 37327 RVA: 0x003BBFFC File Offset: 0x003BA1FC
		private void CompleteSelection()
		{
			this._isCompleted = true;
			this._isPressing = false;
			if (this.FillBar)
			{
				this.FillBar.fillAmount = 1f;
			}
			if (this.PulseFeedback != null)
			{
				this.PulseFeedback.StopFeedbacks();
			}
			this.SetSelectedState();
			if (this.ConfirmSFX != null)
			{
				this.ConfirmSFX.PlayFeedbacks();
			}
			this.HideHint();
			Action onConfirm = this._onConfirm;
			if (onConfirm == null)
			{
				return;
			}
			onConfirm();
		}

		// Token: 0x060091D0 RID: 37328 RVA: 0x003BC084 File Offset: 0x003BA284
		private void AnimateVisuals()
		{
			float num = Time.unscaledDeltaTime * this.TransitionSpeed;
			if (this.ScaleTarget != null)
			{
				float num2 = Mathf.Lerp(this.ScaleTarget.localScale.x, this._targetScale, num);
				this.ScaleTarget.localScale = new Vector3(num2, num2, 1f);
			}
			if (this.ButtonCanvasGroup != null)
			{
				this.ButtonCanvasGroup.alpha = Mathf.Lerp(this.ButtonCanvasGroup.alpha, this._targetAlpha, num);
			}
		}

		// Token: 0x060091D1 RID: 37329 RVA: 0x003BC110 File Offset: 0x003BA310
		private void ApplyVisualsInstant()
		{
			if (this.ScaleTarget != null)
			{
				this.ScaleTarget.localScale = new Vector3(this._targetScale, this._targetScale, 1f);
			}
			if (this.ButtonCanvasGroup != null)
			{
				this.ButtonCanvasGroup.alpha = this._targetAlpha;
			}
		}

		// Token: 0x0400960C RID: 38412
		[Header("References")]
		[Tooltip("Text showing the keybind (e.g. '1', '2')")]
		public TextMeshProUGUI KeyText;

		// Token: 0x0400960D RID: 38413
		[Tooltip("Fill bar for hold-to-confirm feedback")]
		public Image FillBar;

		// Token: 0x0400960E RID: 38414
		[Tooltip("CanvasGroup for controlling overall button opacity")]
		public CanvasGroup ButtonCanvasGroup;

		// Token: 0x0400960F RID: 38415
		[Tooltip("RectTransform to animate scale on (typically this GO or a child wrapper)")]
		public RectTransform ScaleTarget;

		// Token: 0x04009610 RID: 38416
		[Header("Hint")]
		[Tooltip("Child GO showing the key hint (e.g. 'Hold Q'). Hidden after choice is made.")]
		public GameObject HintGO;

		// Token: 0x04009611 RID: 38417
		[Header("Visual States — Idle")]
		[Tooltip("Scale when in normal/idle state")]
		public float NormalScale = 1f;

		// Token: 0x04009612 RID: 38418
		[Tooltip("Alpha when in normal/idle state")]
		public float NormalAlpha = 1f;

		// Token: 0x04009613 RID: 38419
		[Header("Visual States — Selected")]
		[Tooltip("Scale when this button is selected")]
		public float SelectedScale = 1.1f;

		// Token: 0x04009614 RID: 38420
		[Tooltip("Alpha when this button is selected")]
		public float SelectedAlpha = 1f;

		// Token: 0x04009615 RID: 38421
		[Header("Visual States — Dimmed (other selected)")]
		[Tooltip("Scale when the OTHER button is selected")]
		public float DimmedScale = 0.9f;

		// Token: 0x04009616 RID: 38422
		[Tooltip("Alpha when the OTHER button is selected")]
		public float DimmedAlpha = 0.4f;

		// Token: 0x04009617 RID: 38423
		[Header("Animation")]
		[Tooltip("Speed of visual state transitions")]
		public float TransitionSpeed = 8f;

		// Token: 0x04009618 RID: 38424
		[Header("Colors")]
		public Color NormalColor = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x04009619 RID: 38425
		public Color PressedColor = new Color(1f, 1f, 1f, 0.8f);

		// Token: 0x0400961A RID: 38426
		[Header("Feel Feedbacks")]
		[Tooltip("Pulse played while holding the button (visual feedback during press)")]
		public MMF_Player PulseFeedback;

		// Token: 0x0400961B RID: 38427
		[Tooltip("Played once when selection is confirmed")]
		public MMF_Player ConfirmSFX;

		// Token: 0x0400961C RID: 38428
		private GenericInput _assignedInput;

		// Token: 0x0400961D RID: 38429
		private float _requiredDuration;

		// Token: 0x0400961E RID: 38430
		private float _currentHoldTime;

		// Token: 0x0400961F RID: 38431
		private Action _onConfirm;

		// Token: 0x04009620 RID: 38432
		private bool _isCompleted;

		// Token: 0x04009621 RID: 38433
		private bool _isPressing;

		// Token: 0x04009622 RID: 38434
		private float _targetScale;

		// Token: 0x04009623 RID: 38435
		private float _targetAlpha;

		// Token: 0x04009624 RID: 38436
		private bool _isSelected;

		// Token: 0x04009625 RID: 38437
		private bool _isDimmed;
	}
}
