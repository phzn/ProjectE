using System;
using System.Reflection;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000015 RID: 21
	[AddComponentMenu("UI/UIGradientAnimator (DAB)", 32)]
	internal class UIGradientAnimator : BaseGradientAnimator
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003184 File Offset: 0x00001384
		private void Awake()
		{
			if (UIGradientAnimator._uiGradientType == null)
			{
				UIGradientAnimator._uiGradientType = Type.GetType("JoshH.UI.UIGradient");
			}
			if (UIGradientAnimator._uiGradientType != null)
			{
				UIGradientAnimator._linearGradientProperty = UIGradientAnimator._uiGradientType.GetProperty("LinearGradient");
				UIGradientAnimator._angleProperty = UIGradientAnimator._uiGradientType.GetProperty("Angle");
				return;
			}
			Debug.LogError("UIGradient type not found via reflection.");
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000031F0 File Offset: 0x000013F0
		private Component GetCachedComponent()
		{
			if (this._cachedComponent == null)
			{
				this._cachedComponent = base._targetGraphic.GetComponent(UIGradientAnimator._uiGradientType);
				if (this._cachedComponent == null)
				{
					Debug.LogError("UIGradient component not found on " + base._targetGraphic.name);
				}
			}
			return this._cachedComponent;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003250 File Offset: 0x00001450
		protected override void SetGradient(Gradient gradient, float angle)
		{
			Component cachedComponent = this.GetCachedComponent();
			if (cachedComponent != null)
			{
				UIGradientAnimator._linearGradientProperty.SetValue(cachedComponent, gradient, null);
				UIGradientAnimator._angleProperty.SetValue(cachedComponent, angle, null);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000328C File Offset: 0x0000148C
		protected override AnimatedGradient GetCurrentAnimatedGradient()
		{
			Component cachedComponent = this.GetCachedComponent();
			if (cachedComponent != null)
			{
				object value = UIGradientAnimator._linearGradientProperty.GetValue(cachedComponent, null);
				object value2 = UIGradientAnimator._angleProperty.GetValue(cachedComponent, null);
				Gradient gradient = value as Gradient;
				float num = ((value2 is float) ? ((float)value2) : 0f);
				return new AnimatedGradient(gradient, num);
			}
			return default(AnimatedGradient);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000032ED File Offset: 0x000014ED
		protected override void OnStartAnimation()
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000032EF File Offset: 0x000014EF
		private void Reset()
		{
			this._properties = SingletonScriptableObject<DabConfig>.Instance.DefaultGradientProps;
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}

		// Token: 0x04000045 RID: 69
		private static Type _uiGradientType;

		// Token: 0x04000046 RID: 70
		private static PropertyInfo _linearGradientProperty;

		// Token: 0x04000047 RID: 71
		private static PropertyInfo _angleProperty;

		// Token: 0x04000048 RID: 72
		private Component _cachedComponent;
	}
}
