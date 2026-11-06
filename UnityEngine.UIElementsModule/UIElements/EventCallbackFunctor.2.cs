using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001AB RID: 427
	internal class EventCallbackFunctor<TEventType, TCallbackArgs> : EventCallbackFunctorBase where TEventType : EventBase<TEventType>, new()
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x000334F5 File Offset: 0x000316F5
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x000334FD File Offset: 0x000316FD
		internal TCallbackArgs userArgs { get; set; }

		// Token: 0x06000D1F RID: 3359 RVA: 0x00033506 File Offset: 0x00031706
		public EventCallbackFunctor(EventCallback<TEventType, TCallbackArgs> callback, TCallbackArgs userArgs, CallbackPhase phase, InvokePolicy invokePolicy)
			: base(phase, invokePolicy)
		{
			this.userArgs = userArgs;
			this.m_Callback = callback;
			this.m_EventTypeId = EventBase<TEventType>.TypeId();
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00033530 File Offset: 0x00031730
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
						this.m_Callback(evt as TEventType, this.userArgs);
					}
				}
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000335C4 File Offset: 0x000317C4
		public override bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			return this.m_EventTypeId == eventTypeId && this.m_Callback == callback && base.phase == phase;
		}

		// Token: 0x04000631 RID: 1585
		private readonly EventCallback<TEventType, TCallbackArgs> m_Callback;

		// Token: 0x04000632 RID: 1586
		private readonly long m_EventTypeId;
	}
}
