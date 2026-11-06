using System;
using UnityEngine.InputSystem.Haptics;

namespace UnityEngine.InputSystem.XInput
{
	// Token: 0x0200007B RID: 123
	public interface IXboxOneRumble : IDualMotorRumble, IHaptics
	{
		// Token: 0x06000A83 RID: 2691
		void SetMotorSpeeds(float lowFrequency, float highFrequency, float leftTrigger, float rightTrigger);
	}
}
