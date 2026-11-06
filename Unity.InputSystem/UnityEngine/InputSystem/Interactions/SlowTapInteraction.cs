using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem.Interactions
{
	// Token: 0x02000128 RID: 296
	[DisplayName("Long Tap")]
	public class SlowTapInteraction : IInputInteraction
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0004FEA9 File Offset: 0x0004E0A9
		private float durationOrDefault
		{
			get
			{
				if (this.duration <= 0f)
				{
					return InputSystem.settings.defaultSlowTapTime;
				}
				return this.duration;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x0004FEC9 File Offset: 0x0004E0C9
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

		// Token: 0x060010A6 RID: 4262 RVA: 0x0004FEE4 File Offset: 0x0004E0E4
		public void Process(ref InputInteractionContext context)
		{
			if (context.isWaiting && context.ControlIsActuated(this.pressPointOrDefault))
			{
				this.m_SlowTapStartTime = context.time;
				context.Started();
				return;
			}
			if (context.isStarted && !context.ControlIsActuated(this.pressPointOrDefault))
			{
				if (context.time - this.m_SlowTapStartTime >= (double)this.durationOrDefault)
				{
					context.Performed();
					return;
				}
				context.Canceled();
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004FF53 File Offset: 0x0004E153
		public void Reset()
		{
			this.m_SlowTapStartTime = 0.0;
		}

		// Token: 0x040006F5 RID: 1781
		public float duration;

		// Token: 0x040006F6 RID: 1782
		public float pressPoint;

		// Token: 0x040006F7 RID: 1783
		private double m_SlowTapStartTime;
	}
}
