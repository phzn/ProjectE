using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001AA RID: 426
	internal class EventCallbackFunctor<TEventType> : EventCallbackFunctorBase where TEventType : EventBase<TEventType>, new()
	{
		// Token: 0x06000D1A RID: 3354 RVA: 0x00033412 File Offset: 0x00031612
		public EventCallbackFunctor(EventCallback<TEventType> callback, CallbackPhase phase, InvokePolicy invokePolicy = InvokePolicy.Default)
			: base(phase, invokePolicy)
		{
			this.m_Callback = callback;
			this.m_EventTypeId = EventBase<TEventType>.TypeId();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00033430 File Offset: 0x00031630
		public override void Invoke(EventBase evt, PropagationPhase propagationPhase)
		{
			bool flag = evt == null;
			if (flag)
			{
				throw new ArgumentNullException("evt");
			}
			bool flag2 = evt.eventTypeId != this.m_EventTypeId;
			if (!flag2)
			{
				bool flag3 = base.PhaseMatches(propagationPhase);
				if (flag3)
				{
					using (new EventDebuggerLogCall(this.m_Callback, evt))
					{
						this.m_Callback(evt as TEventType);
					}
				}
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000334C0 File Offset: 0x000316C0
		public override bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			return this.m_EventTypeId == eventTypeId && this.m_Callback == callback && base.phase == phase;
		}

		// Token: 0x0400062F RID: 1583
		private readonly EventCallback<TEventType> m_Callback;

		// Token: 0x04000630 RID: 1584
		private readonly long m_EventTypeId;
	}
}
