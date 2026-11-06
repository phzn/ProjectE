using System;
using UnityEngine.InputSystem.XR.Haptics;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000067 RID: 103
	public class XRControllerWithRumble : XRController
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x000371AC File Offset: 0x000353AC
		public void SendImpulse(float amplitude, float duration)
		{
			SendHapticImpulseCommand sendHapticImpulseCommand = SendHapticImpulseCommand.Create(0, amplitude, duration);
			base.ExecuteCommand<SendHapticImpulseCommand>(ref sendHapticImpulseCommand);
		}
	}
}
