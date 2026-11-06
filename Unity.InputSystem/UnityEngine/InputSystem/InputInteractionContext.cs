using System;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200002C RID: 44
	public struct InputInteractionContext
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		public InputAction action
		{
			get
			{
				return this.m_State.GetActionOrNull(ref this.m_TriggerState);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000E8F3 File Offset: 0x0000CAF3
		public InputControl control
		{
			get
			{
				return this.m_State.GetControl(ref this.m_TriggerState);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000E906 File Offset: 0x0000CB06
		public InputActionPhase phase
		{
			get
			{
				return this.m_TriggerState.phase;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000E913 File Offset: 0x0000CB13
		public double time
		{
			get
			{
				return this.m_TriggerState.time;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000E920 File Offset: 0x0000CB20
		public double startTime
		{
			get
			{
				return this.m_TriggerState.startTime;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000E92D File Offset: 0x0000CB2D
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000E93A File Offset: 0x0000CB3A
		public bool timerHasExpired
		{
			get
			{
				return (this.m_Flags & InputInteractionContext.Flags.TimerHasExpired) > (InputInteractionContext.Flags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_Flags |= InputInteractionContext.Flags.TimerHasExpired;
					return;
				}
				this.m_Flags &= ~InputInteractionContext.Flags.TimerHasExpired;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000E95D File Offset: 0x0000CB5D
		public bool isWaiting
		{
			get
			{
				return this.phase == InputActionPhase.Waiting;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000E968 File Offset: 0x0000CB68
		public bool isStarted
		{
			get
			{
				return this.phase == InputActionPhase.Started;
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E973 File Offset: 0x0000CB73
		public float ComputeMagnitude()
		{
			return this.m_TriggerState.magnitude;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000E980 File Offset: 0x0000CB80
		public bool ControlIsActuated(float threshold = 0f)
		{
			return InputActionState.IsActuated(ref this.m_TriggerState, threshold);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000E98E File Offset: 0x0000CB8E
		public void Started()
		{
			this.m_TriggerState.startTime = this.time;
			this.m_State.ChangePhaseOfInteraction(InputActionPhase.Started, ref this.m_TriggerState, InputActionPhase.Waiting, InputActionPhase.Waiting, true);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000E9B6 File Offset: 0x0000CBB6
		public void Performed()
		{
			if (this.m_TriggerState.phase == InputActionPhase.Waiting)
			{
				this.m_TriggerState.startTime = this.time;
			}
			this.m_State.ChangePhaseOfInteraction(InputActionPhase.Performed, ref this.m_TriggerState, InputActionPhase.Waiting, InputActionPhase.Waiting, true);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000E9EC File Offset: 0x0000CBEC
		public void PerformedAndStayStarted()
		{
			if (this.m_TriggerState.phase == InputActionPhase.Waiting)
			{
				this.m_TriggerState.startTime = this.time;
			}
			this.m_State.ChangePhaseOfInteraction(InputActionPhase.Performed, ref this.m_TriggerState, InputActionPhase.Started, InputActionPhase.Waiting, true);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000EA22 File Offset: 0x0000CC22
		public void PerformedAndStayPerformed()
		{
			if (this.m_TriggerState.phase == InputActionPhase.Waiting)
			{
				this.m_TriggerState.startTime = this.time;
			}
			this.m_State.ChangePhaseOfInteraction(InputActionPhase.Performed, ref this.m_TriggerState, InputActionPhase.Performed, InputActionPhase.Waiting, true);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000EA58 File Offset: 0x0000CC58
		public void Canceled()
		{
			if (this.m_TriggerState.phase != InputActionPhase.Canceled)
			{
				this.m_State.ChangePhaseOfInteraction(InputActionPhase.Canceled, ref this.m_TriggerState, InputActionPhase.Waiting, InputActionPhase.Waiting, true);
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000EA7D File Offset: 0x0000CC7D
		public void Waiting()
		{
			if (this.m_TriggerState.phase != InputActionPhase.Waiting)
			{
				this.m_State.ChangePhaseOfInteraction(InputActionPhase.Waiting, ref this.m_TriggerState, InputActionPhase.Waiting, InputActionPhase.Waiting, true);
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000EAA2 File Offset: 0x0000CCA2
		public void SetTimeout(float seconds)
		{
			this.m_State.StartTimeout(seconds, ref this.m_TriggerState);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000EAB6 File Offset: 0x0000CCB6
		public void SetTotalTimeoutCompletionTime(float seconds)
		{
			if (seconds <= 0f)
			{
				throw new ArgumentException("Seconds must be a positive value", "seconds");
			}
			this.m_State.SetTotalTimeoutCompletionTime(seconds, ref this.m_TriggerState);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000EAE2 File Offset: 0x0000CCE2
		public TValue ReadValue<TValue>() where TValue : struct
		{
			return this.m_State.ReadValue<TValue>(this.m_TriggerState.bindingIndex, this.m_TriggerState.controlIndex, false);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000EB06 File Offset: 0x0000CD06
		internal int mapIndex
		{
			get
			{
				return this.m_TriggerState.mapIndex;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000EB13 File Offset: 0x0000CD13
		internal int controlIndex
		{
			get
			{
				return this.m_TriggerState.controlIndex;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000EB20 File Offset: 0x0000CD20
		internal int bindingIndex
		{
			get
			{
				return this.m_TriggerState.bindingIndex;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000EB2D File Offset: 0x0000CD2D
		internal int interactionIndex
		{
			get
			{
				return this.m_TriggerState.interactionIndex;
			}
		}

		// Token: 0x04000101 RID: 257
		internal InputActionState m_State;

		// Token: 0x04000102 RID: 258
		internal InputInteractionContext.Flags m_Flags;

		// Token: 0x04000103 RID: 259
		internal InputActionState.TriggerState m_TriggerState;

		// Token: 0x02000189 RID: 393
		[Flags]
		internal enum Flags
		{
			// Token: 0x0400089B RID: 2203
			TimerHasExpired = 2
		}
	}
}
