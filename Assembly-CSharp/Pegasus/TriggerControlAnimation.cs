using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001C4 RID: 452
	public class TriggerControlAnimation : TriggerBase
	{
		// Token: 0x06000D65 RID: 3429 RVA: 0x0005C784 File Offset: 0x0005A984
		private void Start()
		{
			if (this.m_targetAnimation != null)
			{
				foreach (object obj in this.m_targetAnimation)
				{
					AnimationState animationState = (AnimationState)obj;
					this.m_animations.Add(animationState);
				}
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0005C7F0 File Offset: 0x0005A9F0
		public override void OnStart(PegasusPoi poi)
		{
			if (poi == null)
			{
				Debug.LogWarning(string.Format("Poi was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_targetAnimation == null)
			{
				Debug.LogWarning(string.Format("Animation was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_triggerAtStart)
			{
				if (this.m_actionOnStart == PegasusConstants.PoiAnimationTriggerAction.PlayAnimation)
				{
					this.m_targetAnimation.Play(this.m_animations[this.m_startAnimationIdx].name);
					return;
				}
				if (this.m_actionOnStart == PegasusConstants.PoiAnimationTriggerAction.StopAnimation)
				{
					this.m_targetAnimation.Stop();
				}
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0005C888 File Offset: 0x0005AA88
		public override void OnEnd(PegasusPoi poi)
		{
			if (poi == null)
			{
				Debug.LogWarning(string.Format("Poi was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_targetAnimation == null)
			{
				Debug.LogWarning(string.Format("Animation was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_triggerAtEnd)
			{
				if (this.m_actionOnStart == PegasusConstants.PoiAnimationTriggerAction.PlayAnimation)
				{
					this.m_targetAnimation.Play(this.m_animations[this.m_startAnimationIdx].name);
					return;
				}
				if (this.m_actionOnStart == PegasusConstants.PoiAnimationTriggerAction.StopAnimation)
				{
					this.m_targetAnimation.Stop();
				}
			}
		}

		// Token: 0x04000DAD RID: 3501
		public Animation m_targetAnimation;

		// Token: 0x04000DAE RID: 3502
		public PegasusConstants.PoiAnimationTriggerAction m_actionOnStart;

		// Token: 0x04000DAF RID: 3503
		public PegasusConstants.PoiAnimationTriggerAction m_actionOnEnd = PegasusConstants.PoiAnimationTriggerAction.DoNothing;

		// Token: 0x04000DB0 RID: 3504
		public int m_startAnimationIdx;

		// Token: 0x04000DB1 RID: 3505
		public int m_endAnimation;

		// Token: 0x04000DB2 RID: 3506
		private List<AnimationState> m_animations = new List<AnimationState>();
	}
}
