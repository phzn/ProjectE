using System;
using UnityEngine.InputSystem;

namespace UnityEngine.Rendering
{
	// Token: 0x02000065 RID: 101
	internal class DebugActionState
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000EB00 File Offset: 0x0000CD00
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000EB08 File Offset: 0x0000CD08
		internal bool runningAction { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000EB11 File Offset: 0x0000CD11
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0000EB19 File Offset: 0x0000CD19
		internal float actionState { get; private set; }

		// Token: 0x06000361 RID: 865 RVA: 0x0000EB24 File Offset: 0x0000CD24
		private void Trigger(int triggerCount, float state)
		{
			this.actionState = state;
			this.runningAction = true;
			this.m_Timer = 0f;
			this.m_TriggerPressedUp = new bool[triggerCount];
			for (int i = 0; i < this.m_TriggerPressedUp.Length; i++)
			{
				this.m_TriggerPressedUp[i] = false;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000EB74 File Offset: 0x0000CD74
		public void TriggerWithButton(InputAction action, float state)
		{
			this.inputAction = action;
			this.Trigger(action.bindings.Count, state);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000EB9D File Offset: 0x0000CD9D
		private void Reset()
		{
			this.runningAction = false;
			this.m_Timer = 0f;
			this.m_TriggerPressedUp = null;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
		public void Update(DebugActionDesc desc)
		{
			this.actionState = 0f;
			if (this.m_TriggerPressedUp != null)
			{
				this.m_Timer += Time.deltaTime;
				for (int i = 0; i < this.m_TriggerPressedUp.Length; i++)
				{
					if (this.inputAction != null)
					{
						this.m_TriggerPressedUp[i] |= Mathf.Approximately(this.inputAction.ReadValue<float>(), 0f);
					}
				}
				bool flag = true;
				foreach (bool flag2 in this.m_TriggerPressedUp)
				{
					flag = flag && flag2;
				}
				if (flag || (this.m_Timer > desc.repeatDelay && desc.repeatMode == DebugActionRepeatMode.Delay))
				{
					this.Reset();
				}
			}
		}

		// Token: 0x040001E4 RID: 484
		private DebugActionState.DebugActionKeyType m_Type;

		// Token: 0x040001E5 RID: 485
		private InputAction inputAction;

		// Token: 0x040001E6 RID: 486
		private bool[] m_TriggerPressedUp;

		// Token: 0x040001E7 RID: 487
		private float m_Timer;

		// Token: 0x0200016C RID: 364
		private enum DebugActionKeyType
		{
			// Token: 0x04000615 RID: 1557
			Button,
			// Token: 0x04000616 RID: 1558
			Axis,
			// Token: 0x04000617 RID: 1559
			Key
		}
	}
}
