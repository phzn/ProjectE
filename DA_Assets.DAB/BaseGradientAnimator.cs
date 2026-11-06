using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000003 RID: 3
	public abstract class BaseGradientAnimator : BasePropertyAnimator<AnimatedGradient>
	{
		// Token: 0x06000003 RID: 3
		protected abstract void SetGradient(Gradient gradient, float angle);

		// Token: 0x06000004 RID: 4
		protected abstract AnimatedGradient GetCurrentAnimatedGradient();

		// Token: 0x06000005 RID: 5 RVA: 0x000020C0 File Offset: 0x000002C0
		protected override void SetIdleValue()
		{
			AnimatedGradient currentAnimatedGradient = this.GetCurrentAnimatedGradient();
			this._idleValue = currentAnimatedGradient;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020DC File Offset: 0x000002DC
		protected override void ApplyValue(float curveValue, AnimatedGradient from, AnimatedGradient to)
		{
			if (from.Gradient == null || to.Gradient == null)
			{
				return;
			}
			if (!this.AreGradientsCompatible(from.Gradient, to.Gradient))
			{
				Debug.LogError("Gradients have a different number of keys. Animation is not possible.");
				return;
			}
			Gradient gradient = new Gradient();
			GradientColorKey[] array = new GradientColorKey[from.Gradient.colorKeys.Length];
			for (int i = 0; i < from.Gradient.colorKeys.Length; i++)
			{
				Color color = Color.Lerp(from.Gradient.colorKeys[i].color, to.Gradient.colorKeys[i].color, curveValue);
				float num = Mathf.Lerp(from.Gradient.colorKeys[i].time, to.Gradient.colorKeys[i].time, curveValue);
				array[i] = new GradientColorKey(color, num);
			}
			GradientAlphaKey[] array2 = new GradientAlphaKey[from.Gradient.alphaKeys.Length];
			for (int j = 0; j < from.Gradient.alphaKeys.Length; j++)
			{
				float num2 = Mathf.Lerp(from.Gradient.alphaKeys[j].alpha, to.Gradient.alphaKeys[j].alpha, curveValue);
				float num3 = Mathf.Lerp(from.Gradient.alphaKeys[j].time, to.Gradient.alphaKeys[j].time, curveValue);
				array2[j] = new GradientAlphaKey(num2, num3);
			}
			gradient.SetKeys(array, array2);
			float num4 = this.ApplyRotation(curveValue, from.Angle, to.Angle);
			this.SetGradient(gradient, num4);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022BC File Offset: 0x000004BC
		private float ApplyRotation(float curveValue, float fromAngle, float toAngle)
		{
			if (this._animations.Looped.Enabled && Mathf.Abs(toAngle) == 360f)
			{
				float angle = this.GetCurrentAnimatedGradient().Angle;
				this._rotationSpeed = 360f / this._targetAnimation.Duration;
				this._direction = ((toAngle > 0f) ? 1f : (-1f));
				this._rotationDelta = this._direction * this._rotationSpeed * Time.deltaTime;
				return (angle + this._rotationDelta) % 360f;
			}
			return Mathf.Lerp(fromAngle, toAngle, curveValue) % 360f;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000235F File Offset: 0x0000055F
		protected bool AreGradientsCompatible(Gradient a, Gradient b)
		{
			return a != null && b != null && a.colorKeys.Length == b.colorKeys.Length && a.alphaKeys.Length == b.alphaKeys.Length;
		}

		// Token: 0x04000001 RID: 1
		private float _currentRotation;

		// Token: 0x04000002 RID: 2
		private float _rotationSpeed;

		// Token: 0x04000003 RID: 3
		private float _direction;

		// Token: 0x04000004 RID: 4
		private float _rotationDelta;
	}
}
