using System;
using Features.PlayerEmotions;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x02001288 RID: 4744
	public class EmotionUIController : MonoBehaviour
	{
		// Token: 0x0600933B RID: 37691 RVA: 0x003C75D0 File Offset: 0x003C57D0
		private void Awake()
		{
			if (this.BalanceGradient == null || this.BalanceGradient.colorKeys.Length == 0)
			{
				this.BalanceGradient = new Gradient();
				this.BalanceGradient.SetKeys(new GradientColorKey[]
				{
					new GradientColorKey(new Color(0.8f, 0.2f, 0.4f), 0f),
					new GradientColorKey(new Color(0.7f, 0.7f, 0.7f), 0.5f),
					new GradientColorKey(new Color(1f, 0.4f, 0.6f), 1f)
				}, new GradientAlphaKey[]
				{
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(1f, 1f)
				});
			}
		}

		// Token: 0x0600933C RID: 37692 RVA: 0x003C76B4 File Offset: 0x003C58B4
		private void Start()
		{
			if (this.Mode == EmotionUIController.UIMode.SingleSlider && this.BalanceSlider != null)
			{
				this.BalanceSlider.minValue = -100f;
				this.BalanceSlider.maxValue = 100f;
				this.BalanceSlider.value = 0f;
			}
			if (this.Mode == EmotionUIController.UIMode.DualFill)
			{
				if (this.ShameFill != null)
				{
					this.ShameFill.color = this.ShameColor;
				}
				if (this.ExhibitionFill != null)
				{
					this.ExhibitionFill.color = this.ExhibitionColor;
				}
			}
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.OnEmotionChanged += this.OnEmotionManagerChanged;
			}
		}

		// Token: 0x0600933D RID: 37693 RVA: 0x003C7771 File Offset: 0x003C5971
		private void OnDestroy()
		{
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.OnEmotionChanged -= this.OnEmotionManagerChanged;
			}
		}

		// Token: 0x0600933E RID: 37694 RVA: 0x003C7798 File Offset: 0x003C5998
		private void Update()
		{
			float currentBalance = this.GetCurrentBalance();
			this._targetBalance = currentBalance;
			if (this.SmoothSpeed > 0f)
			{
				this._currentBalance = Mathf.Lerp(this._currentBalance, this._targetBalance, this.SmoothSpeed * Time.deltaTime);
			}
			else
			{
				this._currentBalance = this._targetBalance;
			}
			if (this.Mode == EmotionUIController.UIMode.SingleSlider)
			{
				this.UpdateSingleSliderMode();
				return;
			}
			this.UpdateDualFillMode();
		}

		// Token: 0x0600933F RID: 37695 RVA: 0x003C7806 File Offset: 0x003C5A06
		private float GetCurrentBalance()
		{
			if (EmotionManager.Instance != null)
			{
				return EmotionManager.Instance.EmotionBalance;
			}
			return 0f;
		}

		// Token: 0x06009340 RID: 37696 RVA: 0x003C7828 File Offset: 0x003C5A28
		private void UpdateSingleSliderMode()
		{
			if (this.BalanceSlider == null)
			{
				return;
			}
			this.BalanceSlider.value = this._currentBalance;
			if (this.UseColorGradient && this.BalanceFill != null && this.BalanceGradient != null)
			{
				float num = (this._currentBalance + 100f) / 200f;
				this.BalanceFill.color = this.BalanceGradient.Evaluate(num);
			}
		}

		// Token: 0x06009341 RID: 37697 RVA: 0x003C78A0 File Offset: 0x003C5AA0
		private void UpdateDualFillMode()
		{
			if (this.ShameFill != null)
			{
				float num = ((this._currentBalance < 0f) ? (-this._currentBalance / 100f) : 0f);
				this.ShameFill.fillAmount = num;
			}
			if (this.ExhibitionFill != null)
			{
				float num2 = ((this._currentBalance > 0f) ? (this._currentBalance / 100f) : 0f);
				this.ExhibitionFill.fillAmount = num2;
			}
		}

		// Token: 0x06009342 RID: 37698 RVA: 0x00004F31 File Offset: 0x00003131
		private void OnEmotionManagerChanged(EmotionChangedEventArgs args)
		{
		}

		// Token: 0x06009343 RID: 37699 RVA: 0x003C7924 File Offset: 0x003C5B24
		public void RefreshUI()
		{
			this._currentBalance = this.GetCurrentBalance();
			this._targetBalance = this._currentBalance;
			if (this.Mode == EmotionUIController.UIMode.SingleSlider)
			{
				this.UpdateSingleSliderMode();
				return;
			}
			this.UpdateDualFillMode();
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06009344 RID: 37700 RVA: 0x003C7953 File Offset: 0x003C5B53
		public float CurrentDisplayedBalance
		{
			get
			{
				return this._currentBalance;
			}
		}

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06009345 RID: 37701 RVA: 0x003C795B File Offset: 0x003C5B5B
		public float DisplayedShameLevel
		{
			get
			{
				if (this._currentBalance >= 0f)
				{
					return 0f;
				}
				return -this._currentBalance;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06009346 RID: 37702 RVA: 0x003C7977 File Offset: 0x003C5B77
		public float DisplayedExhibitionLevel
		{
			get
			{
				if (this._currentBalance <= 0f)
				{
					return 0f;
				}
				return this._currentBalance;
			}
		}

		// Token: 0x04009816 RID: 38934
		[Header("=== UI MODE ===")]
		[Tooltip("Use single slider (-100 to +100) or dual fill images from center")]
		public EmotionUIController.UIMode Mode;

		// Token: 0x04009817 RID: 38935
		[Header("=== SINGLE SLIDER MODE ===")]
		[Tooltip("Slider for EmotionBalance (-100 to +100)")]
		public Slider BalanceSlider;

		// Token: 0x04009818 RID: 38936
		[Tooltip("Fill image for the slider (for color gradient)")]
		public Image BalanceFill;

		// Token: 0x04009819 RID: 38937
		[Header("=== DUAL FILL MODE ===")]
		[Tooltip("Fill image for Shame (expands left from center)")]
		public Image ShameFill;

		// Token: 0x0400981A RID: 38938
		[Tooltip("Fill image for Exhibition (expands right from center)")]
		public Image ExhibitionFill;

		// Token: 0x0400981B RID: 38939
		[Header("=== COLOR SETTINGS ===")]
		[Tooltip("If true, fill color changes based on level")]
		public bool UseColorGradient = true;

		// Token: 0x0400981C RID: 38940
		[Tooltip("Color gradient for balance (0=Shame, 0.5=Neutral, 1=Exhibition)")]
		public Gradient BalanceGradient;

		// Token: 0x0400981D RID: 38941
		[Tooltip("Color for Shame fill (dual mode)")]
		public Color ShameColor = new Color(0.8f, 0.2f, 0.4f);

		// Token: 0x0400981E RID: 38942
		[Tooltip("Color for Exhibition fill (dual mode)")]
		public Color ExhibitionColor = new Color(1f, 0.4f, 0.6f);

		// Token: 0x0400981F RID: 38943
		[Header("=== SETTINGS ===")]
		[Tooltip("Smooth transition speed for values")]
		public float SmoothSpeed = 5f;

		// Token: 0x04009820 RID: 38944
		private float _targetBalance;

		// Token: 0x04009821 RID: 38945
		private float _currentBalance;

		// Token: 0x02001289 RID: 4745
		public enum UIMode
		{
			// Token: 0x04009823 RID: 38947
			SingleSlider,
			// Token: 0x04009824 RID: 38948
			DualFill
		}
	}
}
