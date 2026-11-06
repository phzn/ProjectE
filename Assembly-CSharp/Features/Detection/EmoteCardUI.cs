using System;
using Features.Localization;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x0200127D RID: 4733
	[RequireComponent(typeof(CanvasGroup))]
	public class EmoteCardUI : MonoBehaviour
	{
		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x060092FC RID: 37628 RVA: 0x003C59E2 File Offset: 0x003C3BE2
		public EmoteDefinitionSO EmoteData
		{
			get
			{
				return this._emoteData;
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x060092FD RID: 37629 RVA: 0x003C59EA File Offset: 0x003C3BEA
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x060092FE RID: 37630 RVA: 0x003C59F2 File Offset: 0x003C3BF2
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x060092FF RID: 37631 RVA: 0x003C59FC File Offset: 0x003C3BFC
		public RectTransform RectT
		{
			get
			{
				if (!(this._rectTransform != null))
				{
					return this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x06009300 RID: 37632 RVA: 0x003C5A2D File Offset: 0x003C3C2D
		private void Awake()
		{
			this._rectTransform = base.GetComponent<RectTransform>();
			this._canvasGroup = base.GetComponent<CanvasGroup>();
		}

		// Token: 0x06009301 RID: 37633 RVA: 0x003C5A48 File Offset: 0x003C3C48
		public void Setup(EmoteDefinitionSO emote)
		{
			this._emoteData = emote;
			if (this.Label != null)
			{
				LocalizedGameText.BindSource(this.Label, "gameplay_prompts", (emote != null) ? emote.DisplayName : "", null).FitTranslationToRectangle(false);
			}
			if (this.IconImage != null)
			{
				bool flag = emote != null && emote.Icon != null;
				this.IconImage.sprite = (flag ? emote.Icon : null);
				this.IconImage.enabled = flag;
			}
			this.RefreshSpecialMarker(emote);
		}

		// Token: 0x06009302 RID: 37634 RVA: 0x003C5AE8 File Offset: 0x003C3CE8
		private void RefreshSpecialMarker(EmoteDefinitionSO emote)
		{
			bool flag = emote != null && emote.IsMasturbationEmote;
			bool flag2 = flag && emote.IconWithBadge != null;
			if (flag && this.Label != null && this._specialBadge == null)
			{
				this._specialBadge = new GameObject("SpecialEmoteBadge", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				})
				{
					layer = base.gameObject.layer
				}.GetComponent<Image>();
				this._specialBadge.rectTransform.SetParent(this.Label.rectTransform, false);
				RectTransform rectTransform = this._specialBadge.rectTransform;
				RectTransform rectTransform2 = this._specialBadge.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform2.anchorMax = vector;
				rectTransform.anchorMin = vector;
				this._specialBadge.rectTransform.pivot = new Vector2(0.5f, 0f);
				this._specialBadge.rectTransform.anchoredPosition = new Vector2(0f, 3f);
				this._specialBadge.rectTransform.sizeDelta = new Vector2(18f, 18f);
				this._specialBadge.preserveAspect = true;
				this._specialBadge.raycastTarget = false;
				this._specialBadge.color = new Color(1f, 0.68f, 0.82f);
			}
			if (this._specialBadge != null)
			{
				this._specialBadge.sprite = (flag2 ? emote.IconWithBadge : null);
				this._specialBadge.gameObject.SetActive(flag2);
			}
			if (flag && !flag2 && this.Label != null && this._specialLabel == null)
			{
				this._specialLabel = new GameObject("SpecialEmoteLabel", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				})
				{
					layer = base.gameObject.layer
				}.GetComponent<TextMeshProUGUI>();
				this._specialLabel.rectTransform.SetParent(this.Label.rectTransform, false);
				RectTransform rectTransform3 = this._specialLabel.rectTransform;
				RectTransform rectTransform4 = this._specialLabel.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform4.anchorMax = vector;
				rectTransform3.anchorMin = vector;
				this._specialLabel.rectTransform.pivot = new Vector2(0.5f, 0f);
				this._specialLabel.rectTransform.anchoredPosition = new Vector2(0f, 3f);
				this._specialLabel.rectTransform.sizeDelta = new Vector2(100f, 18f);
				this._specialLabel.font = LocalizedGameText.SourceFont(this.Label);
				this._specialLabel.fontSize = 12f;
				this._specialLabel.alignment = TextAlignmentOptions.Center;
				this._specialLabel.characterSpacing = 2f;
				this._specialLabel.raycastTarget = false;
				this._specialLabel.color = new Color(1f, 0.68f, 0.82f);
				LocalizedGameText.BindSource(this._specialLabel, "gameplay_prompts", "SPECIAL", null).FitTranslationToRectangle(true);
			}
			if (this._specialLabel != null)
			{
				this._specialLabel.gameObject.SetActive(flag && !flag2);
			}
		}

		// Token: 0x06009303 RID: 37635 RVA: 0x003C5E7E File Offset: 0x003C407E
		public void SetAvailable(bool available)
		{
			this._isAvailable = available;
			if (this._canvasGroup == null)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
			}
			if (this._canvasGroup != null)
			{
				this._canvasGroup.interactable = available;
			}
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x06009304 RID: 37636 RVA: 0x003C5EBB File Offset: 0x003C40BB
		private float DisplayAlpha
		{
			get
			{
				return this._targetAlpha * (this._isAvailable ? 1f : 0.35f);
			}
		}

		// Token: 0x06009305 RID: 37637 RVA: 0x003C5ED8 File Offset: 0x003C40D8
		public void SetFocusState(bool focused, float scale, float alpha)
		{
			this._targetScale = scale;
			this._targetAlpha = alpha;
			if (focused != this._isFocused)
			{
				this._isFocused = focused;
				if (focused)
				{
					if (this.FocusFeedback != null)
					{
						this.FocusFeedback.PlayFeedbacks();
						return;
					}
				}
				else if (this.UnfocusFeedback != null)
				{
					this.UnfocusFeedback.PlayFeedbacks();
				}
			}
		}

		// Token: 0x06009306 RID: 37638 RVA: 0x003C5F3C File Offset: 0x003C413C
		public void ApplyInstant()
		{
			this.RectT.localScale = new Vector3(this._targetScale, this._targetScale, 1f);
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = this.DisplayAlpha;
			}
		}

		// Token: 0x06009307 RID: 37639 RVA: 0x003C5F8C File Offset: 0x003C418C
		public void AnimateTowardsTargets(float speed)
		{
			float num = Time.unscaledDeltaTime * speed;
			float num2 = Mathf.Lerp(this.RectT.localScale.x, this._targetScale, num);
			this.RectT.localScale = new Vector3(num2, num2, 1f);
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = Mathf.Lerp(this._canvasGroup.alpha, this.DisplayAlpha, num);
			}
		}

		// Token: 0x04009798 RID: 38808
		[Header("References")]
		[Tooltip("TMP text for the emote display name")]
		public TextMeshProUGUI Label;

		// Token: 0x04009799 RID: 38809
		[Tooltip("Optional icon image")]
		public Image IconImage;

		// Token: 0x0400979A RID: 38810
		[Header("Focus Feedback")]
		[Tooltip("Played when this card becomes the focused (center) selection")]
		public MMF_Player FocusFeedback;

		// Token: 0x0400979B RID: 38811
		[Tooltip("Played when this card loses focus")]
		public MMF_Player UnfocusFeedback;

		// Token: 0x0400979C RID: 38812
		private EmoteDefinitionSO _emoteData;

		// Token: 0x0400979D RID: 38813
		private RectTransform _rectTransform;

		// Token: 0x0400979E RID: 38814
		private CanvasGroup _canvasGroup;

		// Token: 0x0400979F RID: 38815
		private Image _specialBadge;

		// Token: 0x040097A0 RID: 38816
		private TextMeshProUGUI _specialLabel;

		// Token: 0x040097A1 RID: 38817
		private float _targetScale = 1f;

		// Token: 0x040097A2 RID: 38818
		private float _targetAlpha = 1f;

		// Token: 0x040097A3 RID: 38819
		private bool _isFocused;

		// Token: 0x040097A4 RID: 38820
		private bool _wasFocused;

		// Token: 0x040097A5 RID: 38821
		private bool _isAvailable = true;
	}
}
