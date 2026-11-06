using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F6 RID: 246
	public interface IInputStateCallbackReceiver
	{
		// Token: 0x06000E9E RID: 3742
		void OnNextUpdate();

		// Token: 0x06000E9F RID: 3743
		void OnStateEvent(InputEventPtr eventPtr);

		// Token: 0x06000EA0 RID: 3744
		bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset);
	}
}
