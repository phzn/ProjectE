using System;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem.Interactions
{
	// Token: 0x02000125 RID: 293
	public class MultiTapInteraction : IInputInteraction<float>, IInputInteraction
	{
		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x0004FACA File Offset: 0x0004DCCA
		private float tapTimeOrDefault
		{
			get
			{
				if ((double)this.tapTime <= 0.0)
				{
					return InputSystem.settings.defaultTapTime;
				}
				return this.tapTime;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0004FAEF File Offset: 0x0004DCEF
		internal float tapDelayOrDefault
		{
			get
			{
				if ((double)this.tapDelay <= 0.0)
				{
					return InputSystem.settings.multiTapDelayTime;
				}
				return this.tapDelay;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0004FB14 File Offset: 0x0004DD14
		private float pressPointOrDefault
		{
			get
			{
				if (this.pressPoint <= 0f)
				{
					return ButtonControl.s_GlobalDefaultButtonPressPoint;
				}
				return this.pressPoint;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x0004FB2F File Offset: 0x0004DD2F
		private float releasePointOrDefault
		{
			get
			{
				return this.pressPointOrDefault * ButtonControl.s_GlobalDefaultButtonReleaseThreshold;
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0004FB40 File Offset: 0x0004DD40
		public void Process(ref InputInteractionContext context)
		{
			if (context.timerHasExpired)
			{
				context.Canceled();
				return;
			}
			switch (this.m_CurrentTapPhase)
			{
			case MultiTapInteraction.TapPhase.None:
				if (context.ControlIsActuated(this.pressPointOrDefault))
				{
					this.m_CurrentTapPhase = MultiTapInteraction.TapPhase.WaitingForNextRelease;
					this.m_CurrentTapStartTime = context.time;
					context.Started();
					float tapTimeOrDefault = this.tapTimeOrDefault;
					float tapDelayOrDefault = this.tapDelayOrDefault;
					context.SetTimeout(tapTimeOrDefault);
					context.SetTotalTimeoutCompletionTime(tapTimeOrDefault * (float)this.tapCount + (float)(this.tapCount - 1) * tapDelayOrDefault);
					return;
				}
				break;
			case MultiTapInteraction.TapPhase.WaitingForNextRelease:
				if (!context.ControlIsActuated(this.releasePointOrDefault))
				{
					if (context.time - this.m_CurrentTapStartTime > (double)this.tapTimeOrDefault)
					{
						context.Canceled();
						return;
					}
					this.m_CurrentTapCount++;
					if (this.m_CurrentTapCount >= this.tapCount)
					{
						context.Performed();
						return;
					}
					this.m_CurrentTapPhase = MultiTapInteraction.TapPhase.WaitingForNextPress;
					this.m_LastTapReleaseTime = context.time;
					context.SetTimeout(this.tapDelayOrDefault);
					return;
				}
				break;
			case MultiTapInteraction.TapPhase.WaitingForNextPress:
				if (context.ControlIsActuated(this.pressPointOrDefault))
				{
					if (context.time - this.m_LastTapReleaseTime <= (double)this.tapDelayOrDefault)
					{
						this.m_CurrentTapPhase = MultiTapInteraction.TapPhase.WaitingForNextRelease;
						this.m_CurrentTapStartTime = context.time;
						context.SetTimeout(this.tapTimeOrDefault);
						return;
					}
					context.Canceled();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0004FC8C File Offset: 0x0004DE8C
		public void Reset()
		{
			this.m_CurrentTapPhase = MultiTapInteraction.TapPhase.None;
			this.m_CurrentTapCount = 0;
			this.m_CurrentTapStartTime = 0.0;
			this.m_LastTapReleaseTime = 0.0;
		}

		// Token: 0x040006E6 RID: 1766
		[Tooltip("The maximum time (in seconds) allowed to elapse between pressing and releasing a control for it to register as a tap.")]
		public float tapTime;

		// Token: 0x040006E7 RID: 1767
		[Tooltip("The maximum delay (in seconds) allowed between each tap. If this time is exceeded, the multi-tap is canceled.")]
		public float tapDelay;

		// Token: 0x040006E8 RID: 1768
		[Tooltip("How many taps need to be performed in succession. Two means double-tap, three means triple-tap, and so on.")]
		public int tapCount = 2;

		// Token: 0x040006E9 RID: 1769
		public float pressPoint;

		// Token: 0x040006EA RID: 1770
		private MultiTapInteraction.TapPhase m_CurrentTapPhase;

		// Token: 0x040006EB RID: 1771
		private int m_CurrentTapCount;

		// Token: 0x040006EC RID: 1772
		private double m_CurrentTapStartTime;

		// Token: 0x040006ED RID: 1773
		private double m_LastTapReleaseTime;

		// Token: 0x0200023F RID: 575
		private enum TapPhase
		{
			// Token: 0x04000C60 RID: 3168
			None,
			// Token: 0x04000C61 RID: 3169
			WaitingForNextRelease,
			// Token: 0x04000C62 RID: 3170
			WaitingForNextPress
		}
	}
}
