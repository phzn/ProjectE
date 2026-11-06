using System;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x0200000A RID: 10
	[AddComponentMenu("UI/PositionAnimator (DAB)", 32)]
	public class PositionAnimator : BasePropertyAnimator<Vector2>
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002D11 File Offset: 0x00000F11
		protected override void ApplyValue(float curveValue, Vector2 from, Vector2 to)
		{
			base._targetGraphic.rectTransform.anchoredPosition = Vector2.LerpUnclamped(from, to, curveValue);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D2B File Offset: 0x00000F2B
		protected override void SetIdleValue()
		{
			this._idleValue = base._targetGraphic.rectTransform.anchoredPosition;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002D43 File Offset: 0x00000F43
		protected override void OnStartAnimation()
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D45 File Offset: 0x00000F45
		private void Reset()
		{
			this._properties = SingletonScriptableObject<DabConfig>.Instance.DefaultPositionProps;
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}
	}
}
