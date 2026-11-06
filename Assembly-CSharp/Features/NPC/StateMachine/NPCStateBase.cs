using System;
using UnityEngine;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F0C RID: 3852
	public abstract class NPCStateBase : INPCState
	{
		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x060073DF RID: 29663
		public abstract NPCStateType StateType { get; }

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x060073E0 RID: 29664 RVA: 0x002F2AE6 File Offset: 0x002F0CE6
		public virtual int Priority
		{
			get
			{
				return this.StateType.GetPriority();
			}
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x060073E1 RID: 29665 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public virtual bool LockProps
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public virtual bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060073E3 RID: 29667 RVA: 0x002F2AF4 File Offset: 0x002F0CF4
		public virtual void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			context.StateEnteredTime = Time.time;
			if (this.LockProps)
			{
				context.LockProps();
			}
			else if (previousState != null && previousState.LockProps)
			{
				context.UnlockProps();
			}
			if (this.LockNavigation)
			{
				context.LockNavigation();
			}
			else if (previousState != null && previousState.LockNavigation)
			{
				context.UnlockNavigation();
			}
			this.LogTransition(context, (previousState != null) ? previousState.StateType : NPCStateType.None, this.StateType);
		}

		// Token: 0x060073E4 RID: 29668
		public abstract NPCStateType? OnUpdate(NPCFSMContext context);

		// Token: 0x060073E5 RID: 29669 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnExit(NPCFSMContext context, INPCState nextState)
		{
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x002F2B67 File Offset: 0x002F0D67
		public virtual bool CanTransitionTo(NPCStateType targetState)
		{
			return targetState.GetPriority() >= this.StateType.GetPriority();
		}

		// Token: 0x060073E7 RID: 29671 RVA: 0x002F2B7F File Offset: 0x002F0D7F
		protected void Log(NPCFSMContext context, string message)
		{
			bool enableLogging = this.EnableLogging;
		}

		// Token: 0x060073E8 RID: 29672 RVA: 0x002F2B7F File Offset: 0x002F0D7F
		protected void LogTransition(NPCFSMContext context, NPCStateType from, NPCStateType to)
		{
			bool enableLogging = this.EnableLogging;
		}

		// Token: 0x040076DC RID: 30428
		protected bool EnableLogging = true;
	}
}
