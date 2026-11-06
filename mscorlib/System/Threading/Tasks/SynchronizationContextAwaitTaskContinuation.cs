using System;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	// Token: 0x0200036F RID: 879
	internal sealed class SynchronizationContextAwaitTaskContinuation : AwaitTaskContinuation
	{
		// Token: 0x06002483 RID: 9347 RVA: 0x00082C6F File Offset: 0x00080E6F
		internal SynchronizationContextAwaitTaskContinuation(SynchronizationContext context, Action action, bool flowExecutionContext)
			: base(action, flowExecutionContext)
		{
			this.m_syncContext = context;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00082C80 File Offset: 0x00080E80
		internal sealed override void Run(Task ignored, bool canInlineContinuationTask)
		{
			if (canInlineContinuationTask && this.m_syncContext == SynchronizationContext.Current)
			{
				base.RunCallback(AwaitTaskContinuation.GetInvokeActionCallback(), this.m_action, ref Task.t_currentTask);
				return;
			}
			base.RunCallback(SynchronizationContextAwaitTaskContinuation.GetPostActionCallback(), this, ref Task.t_currentTask);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00082CBC File Offset: 0x00080EBC
		private static void PostAction(object state)
		{
			SynchronizationContextAwaitTaskContinuation synchronizationContextAwaitTaskContinuation = (SynchronizationContextAwaitTaskContinuation)state;
			synchronizationContextAwaitTaskContinuation.m_syncContext.Post(SynchronizationContextAwaitTaskContinuation.s_postCallback, synchronizationContextAwaitTaskContinuation.m_action);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x00082CE8 File Offset: 0x00080EE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ContextCallback GetPostActionCallback()
		{
			ContextCallback contextCallback = SynchronizationContextAwaitTaskContinuation.s_postActionCallback;
			if (contextCallback == null)
			{
				contextCallback = (SynchronizationContextAwaitTaskContinuation.s_postActionCallback = new ContextCallback(SynchronizationContextAwaitTaskContinuation.PostAction));
			}
			return contextCallback;
		}

		// Token: 0x04001D35 RID: 7477
		private static readonly SendOrPostCallback s_postCallback = delegate(object state)
		{
			((Action)state)();
		};

		// Token: 0x04001D36 RID: 7478
		private static ContextCallback s_postActionCallback;

		// Token: 0x04001D37 RID: 7479
		private readonly SynchronizationContext m_syncContext;
	}
}
