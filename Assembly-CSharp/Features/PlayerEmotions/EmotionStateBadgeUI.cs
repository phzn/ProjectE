using System;
using Features.Rank;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E17 RID: 3607
	public class EmotionStateBadgeUI : MonoBehaviour
	{
		// Token: 0x06006B50 RID: 27472 RVA: 0x002AF85E File Offset: 0x002ADA5E
		private void OnEnable()
		{
			this.CacheRestScales();
			this.StopSwitchAnimation();
			this.TrySubscribe();
			if (!this._rankSubscribed)
			{
				RankProgressionService.Changed += this.RefreshVisibility;
				this._rankSubscribed = true;
			}
			this.RefreshVisibility();
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x002AF898 File Offset: 0x002ADA98
		private void Update()
		{
			this.TrySubscribe();
			this.RefreshVisibility();
			this.AdvanceSwitchAnimation(Time.unscaledDeltaTime);
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x002AF8B1 File Offset: 0x002ADAB1
		private void OnDisable()
		{
			this.StopSwitchAnimation();
			this.UnsubscribeEmotion();
			if (this._rankSubscribed)
			{
				RankProgressionService.Changed -= this.RefreshVisibility;
				this._rankSubscribed = false;
			}
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x002AF8E0 File Offset: 0x002ADAE0
		private void RefreshVisibility()
		{
			if (this._group == null)
			{
				return;
			}
			bool flag = true;
			if (RankRuntimeController.Current != null)
			{
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				flag = currentRank == null || (currentRank.showEmotionBadge && currentRank.allowStanceSwitch);
			}
			this._group.alpha = (flag ? 1f : 0f);
			this._group.interactable = false;
			this._group.blocksRaycasts = false;
		}

		// Token: 0x06006B54 RID: 27476 RVA: 0x002AF964 File Offset: 0x002ADB64
		private void TrySubscribe()
		{
			EmotionManager instance = EmotionManager.Instance;
			if (this._boundManager == instance)
			{
				return;
			}
			this.UnsubscribeEmotion();
			this._boundManager = instance;
			this.StopSwitchAnimation();
			if (this._boundManager == null)
			{
				return;
			}
			this._boundManager.OnStanceChanged += this.OnStanceChanged;
			this.Refresh(this._boundManager.CurrentStance);
		}

		// Token: 0x06006B55 RID: 27477 RVA: 0x002AF9CB File Offset: 0x002ADBCB
		private void UnsubscribeEmotion()
		{
			if (this._boundManager != null)
			{
				this._boundManager.OnStanceChanged -= this.OnStanceChanged;
			}
			this._boundManager = null;
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x002AF9F9 File Offset: 0x002ADBF9
		private void OnStanceChanged(EmotionStance stance)
		{
			this.Refresh(stance);
			this.CacheRestScales();
			this._switchElapsed = 0f;
			this._switchAnimating = true;
			this.ApplyEyeScale(0.12f);
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x002AFA28 File Offset: 0x002ADC28
		private void Refresh(EmotionStance stance)
		{
			bool flag = stance == EmotionStance.Embarassed;
			Texture2D texture2D = (flag ? this._shyEyesTexture : this._confidentEyesTexture);
			Color color = (flag ? EmotionStateBadgeUI.ShyEye : EmotionStateBadgeUI.ExposeEye);
			Rect leftEyeCrop = EmotionStateBadgeUI.LeftEyeCrop;
			Rect rightEyeCrop = EmotionStateBadgeUI.RightEyeCrop;
			if (flag)
			{
				leftEyeCrop.y += leftEyeCrop.height * 0.11f;
				rightEyeCrop.y += rightEyeCrop.height * 0.11f;
			}
			EmotionStateBadgeUI.SetEye(this._leftEye, texture2D, leftEyeCrop, color);
			EmotionStateBadgeUI.SetEye(this._rightEye, texture2D, rightEyeCrop, color);
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x002AFABC File Offset: 0x002ADCBC
		private void CacheRestScales()
		{
			if (this._hasRestScales)
			{
				return;
			}
			this._leftRestScale = ((this._leftEye != null) ? this._leftEye.rectTransform.localScale : Vector3.one);
			this._rightRestScale = ((this._rightEye != null) ? this._rightEye.rectTransform.localScale : Vector3.one);
			this._hasRestScales = true;
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x002AFB30 File Offset: 0x002ADD30
		private void AdvanceSwitchAnimation(float unscaledDeltaTime)
		{
			if (!this._switchAnimating)
			{
				return;
			}
			this._switchElapsed += Mathf.Max(0f, unscaledDeltaTime);
			float num = this._switchElapsed / 0.24f;
			if (num >= 1f)
			{
				this.StopSwitchAnimation();
				return;
			}
			float num3;
			if (num < 0.68f)
			{
				float num2 = 1f - num / 0.68f;
				num3 = Mathf.LerpUnclamped(0.12f, 1.075f, 1f - num2 * num2 * num2);
			}
			else
			{
				num3 = Mathf.Lerp(1.075f, 1f, Mathf.SmoothStep(0f, 1f, (num - 0.68f) / 0.32f));
			}
			this.ApplyEyeScale(num3);
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x002AFBE0 File Offset: 0x002ADDE0
		private void ApplyEyeScale(float height)
		{
			float num = 1f + (1f - height) * 0.045f;
			if (this._leftEye != null)
			{
				this._leftEye.rectTransform.localScale = new Vector3(this._leftRestScale.x * num, this._leftRestScale.y * height, this._leftRestScale.z);
			}
			if (this._rightEye != null)
			{
				this._rightEye.rectTransform.localScale = new Vector3(this._rightRestScale.x * num, this._rightRestScale.y * height, this._rightRestScale.z);
			}
		}

		// Token: 0x06006B5B RID: 27483 RVA: 0x002AFC94 File Offset: 0x002ADE94
		private void StopSwitchAnimation()
		{
			this._switchAnimating = false;
			this._switchElapsed = 0f;
			if (!this._hasRestScales)
			{
				return;
			}
			if (this._leftEye != null)
			{
				this._leftEye.rectTransform.localScale = this._leftRestScale;
			}
			if (this._rightEye != null)
			{
				this._rightEye.rectTransform.localScale = this._rightRestScale;
			}
		}

		// Token: 0x06006B5C RID: 27484 RVA: 0x002AFD04 File Offset: 0x002ADF04
		private static void SetEye(RawImage eye, Texture2D artwork, Rect crop, Color tint)
		{
			if (eye == null)
			{
				return;
			}
			eye.texture = artwork;
			eye.uvRect = crop;
			eye.color = tint;
		}

		// Token: 0x04006E04 RID: 28164
		[Header("Eye indicator")]
		[SerializeField]
		private CanvasGroup _group;

		// Token: 0x04006E05 RID: 28165
		[FormerlySerializedAs("_shyEye")]
		[SerializeField]
		private RawImage _leftEye;

		// Token: 0x04006E06 RID: 28166
		[FormerlySerializedAs("_exposeEye")]
		[SerializeField]
		private RawImage _rightEye;

		// Token: 0x04006E07 RID: 28167
		[SerializeField]
		private Texture2D _shyEyesTexture;

		// Token: 0x04006E08 RID: 28168
		[SerializeField]
		private Texture2D _confidentEyesTexture;

		// Token: 0x04006E09 RID: 28169
		private static readonly Color ShyEye = new Color32(byte.MaxValue, 186, 207, byte.MaxValue);

		// Token: 0x04006E0A RID: 28170
		private static readonly Color ExposeEye = new Color32(231, 202, 248, byte.MaxValue);

		// Token: 0x04006E0B RID: 28171
		private static readonly Rect LeftEyeCrop = new Rect(0.025f, 0.32f, 0.5f, 0.289f);

		// Token: 0x04006E0C RID: 28172
		private static readonly Rect RightEyeCrop = new Rect(0.5f, 0.39f, 0.48f, 0.278f);

		// Token: 0x04006E0D RID: 28173
		private const float ShyCropVerticalOffset = 0.11f;

		// Token: 0x04006E0E RID: 28174
		private const float SwitchDuration = 0.24f;

		// Token: 0x04006E0F RID: 28175
		private EmotionManager _boundManager;

		// Token: 0x04006E10 RID: 28176
		private bool _rankSubscribed;

		// Token: 0x04006E11 RID: 28177
		private bool _hasRestScales;

		// Token: 0x04006E12 RID: 28178
		private Vector3 _leftRestScale;

		// Token: 0x04006E13 RID: 28179
		private Vector3 _rightRestScale;

		// Token: 0x04006E14 RID: 28180
		private bool _switchAnimating;

		// Token: 0x04006E15 RID: 28181
		private float _switchElapsed;
	}
}
