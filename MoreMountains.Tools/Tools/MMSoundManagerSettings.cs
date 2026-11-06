using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000D6 RID: 214
	[Serializable]
	public class MMSoundManagerSettings
	{
		// Token: 0x040005FC RID: 1532
		public const float _minimalVolume = 0.0001f;

		// Token: 0x040005FD RID: 1533
		public const float _maxVolume = 10f;

		// Token: 0x040005FE RID: 1534
		public const float _defaultVolume = 1f;

		// Token: 0x040005FF RID: 1535
		[Header("Audio Mixer Control")]
		[Tooltip("whether or not the settings described below should override the ones defined in the AudioMixer")]
		public bool OverrideMixerSettings = true;

		// Token: 0x04000600 RID: 1536
		[Header("Audio Mixer Exposed Parameters")]
		[Tooltip("the name of the exposed MasterVolume parameter in the AudioMixer")]
		public string MasterVolumeParameter = "MasterVolume";

		// Token: 0x04000601 RID: 1537
		[Tooltip("the name of the exposed MusicVolume parameter in the AudioMixer")]
		public string MusicVolumeParameter = "MusicVolume";

		// Token: 0x04000602 RID: 1538
		[Tooltip("the name of the exposed SfxVolume parameter in the AudioMixer")]
		public string SfxVolumeParameter = "SfxVolume";

		// Token: 0x04000603 RID: 1539
		[Tooltip("the name of the exposed UIVolume parameter in the AudioMixer")]
		public string UIVolumeParameter = "UIVolume";

		// Token: 0x04000604 RID: 1540
		[Header("Master")]
		[Range(0.0001f, 10f)]
		[Tooltip("the master volume")]
		[MMReadOnly]
		public float MasterVolume = 1f;

		// Token: 0x04000605 RID: 1541
		[Tooltip("whether the master track is active at the moment or not")]
		[MMReadOnly]
		public bool MasterOn = true;

		// Token: 0x04000606 RID: 1542
		[Tooltip("the volume of the master track before it was muted")]
		[MMReadOnly]
		public float MutedMasterVolume;

		// Token: 0x04000607 RID: 1543
		[Header("Music")]
		[Range(0.0001f, 10f)]
		[Tooltip("the music volume")]
		[MMReadOnly]
		public float MusicVolume = 1f;

		// Token: 0x04000608 RID: 1544
		[Tooltip("whether the music track is active at the moment or not")]
		[MMReadOnly]
		public bool MusicOn = true;

		// Token: 0x04000609 RID: 1545
		[Tooltip("the volume of the music track before it was muted")]
		[MMReadOnly]
		public float MutedMusicVolume;

		// Token: 0x0400060A RID: 1546
		[Header("Sound Effects")]
		[Range(0.0001f, 10f)]
		[Tooltip("the sound fx volume")]
		[MMReadOnly]
		public float SfxVolume = 1f;

		// Token: 0x0400060B RID: 1547
		[Tooltip("whether the SFX track is active at the moment or not")]
		[MMReadOnly]
		public bool SfxOn = true;

		// Token: 0x0400060C RID: 1548
		[Tooltip("the volume of the SFX track before it was muted")]
		[MMReadOnly]
		public float MutedSfxVolume;

		// Token: 0x0400060D RID: 1549
		[Header("UI")]
		[Range(0.0001f, 10f)]
		[Tooltip("the UI sounds volume")]
		[MMReadOnly]
		public float UIVolume = 1f;

		// Token: 0x0400060E RID: 1550
		[Tooltip("whether the UI track is active at the moment or not")]
		[MMReadOnly]
		public bool UIOn = true;

		// Token: 0x0400060F RID: 1551
		[Tooltip("the volume of the UI track before it was muted")]
		[MMReadOnly]
		public float MutedUIVolume;

		// Token: 0x04000610 RID: 1552
		[Header("Save & Load")]
		[Tooltip("whether or not the MMSoundManager should automatically load settings when starting")]
		public bool AutoLoad = true;

		// Token: 0x04000611 RID: 1553
		[Tooltip("whether or not each change in the settings should be automaticall saved. If not, you'll have to call a save MMSoundManager event for settings to be saved.")]
		public bool AutoSave;
	}
}
