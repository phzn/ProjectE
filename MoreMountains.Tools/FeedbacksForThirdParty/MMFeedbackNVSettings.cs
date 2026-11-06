using System;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000314 RID: 788
	[Serializable]
	public class MMFeedbackNVSettings
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x0005D304 File Offset: 0x0005B504
		public virtual void SetGamepad()
		{
			if (this.ForceGamepadID)
			{
				GamepadRumbler.SetCurrentGamepad(this.GamepadID);
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0005D319 File Offset: 0x0005B519
		public virtual bool CanPlay()
		{
			return (!this.OnlyPlayIfAdvancedRequirementsMet || DeviceCapabilities.meetsAdvancedRequirements) && (!this.OnlyPlayIfAmplitudeModulationSupported || DeviceCapabilities.hasAmplitudeModulation) && (!this.OnlyPlayIfFrequencyModulationSupported || DeviceCapabilities.hasFrequencyModulation);
		}

		// Token: 0x04001924 RID: 6436
		[Tooltip("whether or not to force this haptic to play on a specific gamepad")]
		public bool ForceGamepadID;

		// Token: 0x04001925 RID: 6437
		[Tooltip("The ID of the gamepad on which to play this haptic")]
		public int GamepadID;

		// Token: 0x04001926 RID: 6438
		[Tooltip("whether or not this haptic should play only if haptics are supported")]
		public bool OnlyPlayIfHapticsSupported = true;

		// Token: 0x04001927 RID: 6439
		[Tooltip("whether or not this haptic should play only if advanced haptics requirements are met on the device")]
		public bool OnlyPlayIfAdvancedRequirementsMet;

		// Token: 0x04001928 RID: 6440
		[Tooltip("whether or not this haptic should play only if the device supports amplitude modulation")]
		public bool OnlyPlayIfAmplitudeModulationSupported;

		// Token: 0x04001929 RID: 6441
		[Tooltip("whether or not this haptic should play only if the device supports frequency modulation")]
		public bool OnlyPlayIfFrequencyModulationSupported;
	}
}
