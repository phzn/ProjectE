using System;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x0200000C RID: 12
	[AddComponentMenu("UI/ScaleAnimator (DAB)", 32)]
	public class ScaleAnimator : BasePropertyAnimator<Vector2>
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002EA8 File Offset: 0x000010A8
		protected override void ApplyValue(float curveValue, Vector2 from, Vector2 to)
		{
			base._targetGraphic.rectTransform.localScale = Vector2.LerpUnclamped(from, to, curveValue);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002EC7 File Offset: 0x000010C7
		protected override void SetIdleValue()
		{
			this._idleValue = base._targetGraphic.rectTransform.localScale;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002EE4 File Offset: 0x000010E4
		protected override void OnStartAnimation()
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002EE6 File Offset: 0x000010E6
		private void Reset()
		{
			this._properties = SingletonScriptableObject<DabConfig>.Instance.DefaultScaleProps;
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}
	}
}
