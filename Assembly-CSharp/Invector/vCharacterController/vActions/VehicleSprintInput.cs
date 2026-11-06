using System;
using ProjectExhibitionist.Core;
using UnityEngine.InputSystem;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000708 RID: 1800
	public sealed class VehicleSprintInput
	{
		// Token: 0x06002E46 RID: 11846 RVA: 0x001043FC File Offset: 0x001025FC
		public bool Read(GenericInput binding, float movementMagnitude, bool braking)
		{
			if (!ControllerInput.IsActive)
			{
				this.Reset();
				return binding != null && binding.useInput && binding.GetButton();
			}
			int deviceId = Gamepad.current.deviceId;
			if (this.controllerDeviceId != deviceId)
			{
				this.Reset();
				this.controllerDeviceId = deviceId;
			}
			if (binding == null || !binding.useInput || movementMagnitude <= 0.01f || braking || !ControllerGameplayBridge.AllowsAxes || ControllerWorldPointer.BlocksContextInput || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				this.Reset();
				return false;
			}
			if (!ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.LeftStick))
			{
				this.controllerLatched = !this.controllerLatched;
			}
			return this.controllerLatched;
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x001044AB File Offset: 0x001026AB
		public void Reset()
		{
			this.controllerLatched = false;
			this.controllerDeviceId = -1;
		}

		// Token: 0x04002E89 RID: 11913
		private bool controllerLatched;

		// Token: 0x04002E8A RID: 11914
		private int controllerDeviceId = -1;
	}
}
