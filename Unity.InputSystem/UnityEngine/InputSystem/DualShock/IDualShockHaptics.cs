using System;
using UnityEngine.InputSystem.Haptics;

namespace UnityEngine.InputSystem.DualShock
{
	// Token: 0x020000A7 RID: 167
	public interface IDualShockHaptics : IDualMotorRumble, IHaptics
	{
		// Token: 0x06000CD4 RID: 3284
		void SetLightBarColor(Color color);
	}
}
