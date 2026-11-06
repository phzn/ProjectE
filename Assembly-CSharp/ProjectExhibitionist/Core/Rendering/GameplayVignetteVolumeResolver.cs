using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A97 RID: 2711
	internal static class GameplayVignetteVolumeResolver
	{
		// Token: 0x06004D27 RID: 19751 RVA: 0x001DF150 File Offset: 0x001DD350
		internal static Volume Resolve(Volume preferred)
		{
			if (GameplayVignetteVolumeResolver.IsUsable(preferred))
			{
				return preferred;
			}
			Volume volume = null;
			Volume volume2 = null;
			foreach (Volume volume3 in Object.FindObjectsOfType<Volume>())
			{
				if (GameplayVignetteVolumeResolver.IsUsable(volume3) && volume3.isGlobal && volume3.weight > 0f)
				{
					Vignette vignette;
					if (volume3.profile.TryGet<Vignette>(out vignette) && (volume == null || volume3.priority < volume.priority))
					{
						volume = volume3;
					}
					if (volume2 == null || volume3.priority < volume2.priority)
					{
						volume2 = volume3;
					}
				}
			}
			if (!(volume != null))
			{
				return volume2;
			}
			return volume;
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x001DF1F8 File Offset: 0x001DD3F8
		private static bool IsUsable(Volume volume)
		{
			if (volume == null || !volume.isActiveAndEnabled || ScenePostFxQualityService.IsAccessibilityVolume(volume))
			{
				return false;
			}
			VolumeProfile profile = volume.profile;
			if (profile != null)
			{
				return profile.components.Exists((VolumeComponent component) => component != null && !(component is ChannelMixer));
			}
			return false;
		}
	}
}
