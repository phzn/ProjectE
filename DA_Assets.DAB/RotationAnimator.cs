using System;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x0200000B RID: 11
	[AddComponentMenu("UI/RotationAnimator (DAB)", 32)]
	internal class RotationAnimator : BasePropertyAnimator<float>
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002D70 File Offset: 0x00000F70
		protected override void ApplyValue(float curveValue, float from, float to)
		{
			if (this._animations.Looped.Enabled && Mathf.Abs(to) == 360f)
			{
				this._currentRotation = base._targetGraphic.rectTransform.localEulerAngles.z;
				this._rotationSpeed = 360f / this._targetAnimation.Duration;
				this._direction = ((to > 0f) ? (-1f) : 1f);
				this._rotationDelta = this._direction * this._rotationSpeed * Time.deltaTime;
				base._targetGraphic.rectTransform.localEulerAngles = new Vector3(0f, 0f, this._currentRotation + this._rotationDelta);
				return;
			}
			base._targetGraphic.rectTransform.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpUnclamped(from, -to, curveValue));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002E5F File Offset: 0x0000105F
		protected override void SetIdleValue()
		{
			this._idleValue = base._targetGraphic.rectTransform.localScale.z;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002E7C File Offset: 0x0000107C
		protected override void OnStartAnimation()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E7E File Offset: 0x0000107E
		private void Reset()
		{
			this._properties = SingletonScriptableObject<DabConfig>.Instance.DefaulRotationProps;
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}

		// Token: 0x04000022 RID: 34
		private float _currentRotation;

		// Token: 0x04000023 RID: 35
		private float _rotationSpeed;

		// Token: 0x04000024 RID: 36
		private float _direction;

		// Token: 0x04000025 RID: 37
		private float _rotationDelta;
	}
}
