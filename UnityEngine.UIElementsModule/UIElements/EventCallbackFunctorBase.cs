using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A9 RID: 425
	internal abstract class EventCallbackFunctorBase
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0003338F File Offset: 0x0003158F
		public CallbackPhase phase { get; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00033397 File Offset: 0x00031597
		public InvokePolicy invokePolicy { get; }

		// Token: 0x06000D16 RID: 3350 RVA: 0x0003339F File Offset: 0x0003159F
		protected EventCallbackFunctorBase(CallbackPhase phase, InvokePolicy invokePolicy)
		{
			this.phase = phase;
			this.invokePolicy = invokePolicy;
		}

		// Token: 0x06000D17 RID: 3351
		public abstract void Invoke(EventBase evt, PropagationPhase propagationPhase);

		// Token: 0x06000D18 RID: 3352
		public abstract bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase);

		// Token: 0x06000D19 RID: 3353 RVA: 0x000333B8 File Offset: 0x000315B8
		protected bool PhaseMatches(PropagationPhase propagationPhase)
		{
			CallbackPhase phase = this.phase;
			CallbackPhase callbackPhase = phase;
			if (callbackPhase != CallbackPhase.TargetAndBubbleUp)
			{
				if (callbackPhase == CallbackPhase.TrickleDownAndTarget)
				{
					bool flag = propagationPhase != PropagationPhase.TrickleDown && propagationPhase != PropagationPhase.AtTarget;
					if (flag)
					{
						return false;
					}
				}
			}
			else
			{
				bool flag2 = propagationPhase != PropagationPhase.AtTarget && propagationPhase != PropagationPhase.BubbleUp;
				if (flag2)
				{
					return false;
				}
			}
			return true;
		}
	}
}
