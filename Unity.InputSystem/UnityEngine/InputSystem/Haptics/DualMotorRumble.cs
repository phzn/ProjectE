using System;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Haptics
{
	// Token: 0x020000B0 RID: 176
	internal struct DualMotorRumble
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00043F83 File Offset: 0x00042183
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x00043F8B File Offset: 0x0004218B
		public float lowFrequencyMotorSpeed { readonly get; private set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00043F94 File Offset: 0x00042194
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x00043F9C File Offset: 0x0004219C
		public float highFrequencyMotorSpeed { readonly get; private set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00043FA5 File Offset: 0x000421A5
		public bool isRumbling
		{
			get
			{
				return !Mathf.Approximately(this.lowFrequencyMotorSpeed, 0f) || !Mathf.Approximately(this.highFrequencyMotorSpeed, 0f);
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00043FD0 File Offset: 0x000421D0
		public void PauseHaptics(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			if (!this.isRumbling)
			{
				return;
			}
			DualMotorRumbleCommand dualMotorRumbleCommand = DualMotorRumbleCommand.Create(0f, 0f);
			device.ExecuteCommand<DualMotorRumbleCommand>(ref dualMotorRumbleCommand);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0004400D File Offset: 0x0004220D
		public void ResumeHaptics(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			if (!this.isRumbling)
			{
				return;
			}
			this.SetMotorSpeeds(device, this.lowFrequencyMotorSpeed, this.highFrequencyMotorSpeed);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00044039 File Offset: 0x00042239
		public void ResetHaptics(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			if (!this.isRumbling)
			{
				return;
			}
			this.SetMotorSpeeds(device, 0f, 0f);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00044064 File Offset: 0x00042264
		public void SetMotorSpeeds(InputDevice device, float lowFrequency, float highFrequency)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			this.lowFrequencyMotorSpeed = Mathf.Clamp(lowFrequency, 0f, 1f);
			this.highFrequencyMotorSpeed = Mathf.Clamp(highFrequency, 0f, 1f);
			DualMotorRumbleCommand dualMotorRumbleCommand = DualMotorRumbleCommand.Create(this.lowFrequencyMotorSpeed, this.highFrequencyMotorSpeed);
			device.ExecuteCommand<DualMotorRumbleCommand>(ref dualMotorRumbleCommand);
		}
	}
}
