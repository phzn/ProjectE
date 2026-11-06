using System;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000006 RID: 6
	[AddComponentMenu("UI/ColorAnimator (DAB)", 32)]
	public class ColorAnimator : BasePropertyAnimator<Color>
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002902 File Offset: 0x00000B02
		protected override void ApplyValue(float curveValue, Color from, Color to)
		{
			base._targetGraphic.color = Color.LerpUnclamped(from, to, curveValue);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002917 File Offset: 0x00000B17
		protected override void SetIdleValue()
		{
			this._idleValue = base._targetGraphic.color;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000292A File Offset: 0x00000B2A
		protected override void OnStartAnimation()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000292C File Offset: 0x00000B2C
		private void Reset()
		{
			this._properties = SingletonScriptableObject<DabConfig>.Instance.DefaultColorProps;
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}
	}
}
