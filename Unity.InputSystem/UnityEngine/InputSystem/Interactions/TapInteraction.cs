using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem.Interactions
{
	// Token: 0x02000129 RID: 297
	[DisplayName("Tap")]
	public class TapInteraction : IInputInteraction
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x0004FF6C File Offset: 0x0004E16C
		private float durationOrDefault
		{
			get
			{
				if ((double)this.duration <= 0.0)
				{
					return InputSystem.settings.defaultTapTime;
				}
				return this.duration;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x0004FF91 File Offset: 0x0004E191
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

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x0004FFAC File Offset: 0x0004E1AC
		private float releasePointOrDefault
		{
			get
			{
				return this.pressPointOrDefault * ButtonControl.s_GlobalDefaultButtonReleaseThreshold;
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0004FFBC File Offset: 0x0004E1BC
		public void Process(ref InputInteractionContext context)
		{
			if (context.timerHasExpired)
			{
				context.Canceled();
				this.canceledFromTimerExpired = true;
				return;
			}
			if (context.isWaiting && context.ControlIsActuated(this.pressPointOrDefault) && !this.canceledFromTimerExpired)
			{
				this.m_TapStartTime = context.time;
				context.Started();
				context.SetTimeout(this.durationOrDefault + 1E-05f);
				return;
			}
			if (context.isStarted && !context.ControlIsActuated(this.releasePointOrDefault))
			{
				if (context.time - this.m_TapStartTime <= (double)this.durationOrDefault)
				{
					context.Performed();
				}
				else
				{
					context.Canceled();
				}
			}
			if (!context.ControlIsActuated(this.releasePointOrDefault))
			{
				this.canceledFromTimerExpired = false;
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00050071 File Offset: 0x0004E271
		public void Reset()
		{
			this.m_TapStartTime = 0.0;
		}

		// Token: 0x040006F8 RID: 1784
		public float duration;

		// Token: 0x040006F9 RID: 1785
		public float pressPoint;

		// Token: 0x040006FA RID: 1786
		private double m_TapStartTime;

		// Token: 0x040006FB RID: 1787
		private bool canceledFromTimerExpired;
	}
}
