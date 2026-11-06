using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F7 RID: 247
	public interface IInputStateChangeMonitor
	{
		// Token: 0x06000EA1 RID: 3745
		void NotifyControlStateChanged(InputControl control, double time, InputEventPtr eventPtr, long monitorIndex);

		// Token: 0x06000EA2 RID: 3746
		void NotifyTimerExpired(InputControl control, double time, long monitorIndex, int timerIndex);
	}
}
