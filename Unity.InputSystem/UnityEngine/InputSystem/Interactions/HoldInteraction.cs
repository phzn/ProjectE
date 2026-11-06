using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem.Interactions
{
	// Token: 0x02000124 RID: 292
	[DisplayName("Hold")]
	public class HoldInteraction : IInputInteraction
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0004F9C3 File Offset: 0x0004DBC3
		private float durationOrDefault
		{
			get
			{
				if ((double)this.duration <= 0.0)
				{
					return InputSystem.settings.defaultHoldTime;
				}
				return this.duration;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x0004F9E8 File Offset: 0x0004DBE8
		private float pressPointOrDefault
		{
			get
			{
				if ((double)this.pressPoint <= 0.0)
				{
					return ButtonControl.s_GlobalDefaultButtonPressPoint;
				}
				return this.pressPoint;
			}
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0004FA08 File Offset: 0x0004DC08
		public void Process(ref InputInteractionContext context)
		{
			if (context.timerHasExpired)
			{
				context.PerformedAndStayPerformed();
				return;
			}
			switch (context.phase)
			{
			case InputActionPhase.Waiting:
				if (context.ControlIsActuated(this.pressPointOrDefault))
				{
					this.m_TimePressed = context.time;
					context.Started();
					context.SetTimeout(this.durationOrDefault);
					return;
				}
				break;
			case InputActionPhase.Started:
				if (context.time - this.m_TimePressed >= (double)this.durationOrDefault)
				{
					context.PerformedAndStayPerformed();
				}
				if (!context.ControlIsActuated(0f))
				{
					context.Canceled();
					return;
				}
				break;
			case InputActionPhase.Performed:
				if (!context.ControlIsActuated(this.pressPointOrDefault))
				{
					context.Canceled();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0004FAB1 File Offset: 0x0004DCB1
		public void Reset()
		{
			this.m_TimePressed = 0.0;
		}

		// Token: 0x040006E3 RID: 1763
		public float duration;

		// Token: 0x040006E4 RID: 1764
		public float pressPoint;

		// Token: 0x040006E5 RID: 1765
		private double m_TimePressed;
	}
}
