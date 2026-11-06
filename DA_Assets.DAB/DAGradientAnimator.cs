using System;
using DA_Assets.DAG;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000009 RID: 9
	[AddComponentMenu("UI/GradientAnimator (DAB)", 32)]
	internal class DAGradientAnimator : BaseGradientAnimator
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002C78 File Offset: 0x00000E78
		protected override void SetGradient(Gradient gradient, float angle)
		{
			DAGradient component = base._targetGraphic.GetComponent<DAGradient>();
			if (component != null)
			{
				component.Gradient = gradient;
				component.Angle = angle;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002CA8 File Offset: 0x00000EA8
		protected override AnimatedGradient GetCurrentAnimatedGradient()
		{
			DAGradient component = base._targetGraphic.GetComponent<DAGradient>();
			if (component != null)
			{
				return new AnimatedGradient(component.Gradient, component.Angle);
			}
			return default(AnimatedGradient);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002CE5 File Offset: 0x00000EE5
		protected override void OnStartAnimation()
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002CE7 File Offset: 0x00000EE7
		private void Reset()
		{
			this._properties = SingletonScriptableObject<DabConfig>.Instance.DefaultGradientProps;
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}
	}
}
