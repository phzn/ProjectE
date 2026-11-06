using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MoreMountains.Tools
{
	// Token: 0x020000D5 RID: 213
	[Serializable]
	public struct MMSoundManagerPlayOptions
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00015CB0 File Offset: 0x00013EB0
		public static MMSoundManagerPlayOptions Default
		{
			get
			{
				return new MMSoundManagerPlayOptions
				{
					Initialized = true,
					MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx,
					Location = Vector3.zero,
					Loop = false,
					Volume = 1f,
					ID = 0,
					Fade = false,
					FadeInitialVolume = 0f,
					FadeDuration = 1f,
					FadeTween = MMTweenType.DefaultEaseInCubic,
					Persistent = false,
					RecycleAudioSource = null,
					AudioGroup = null,
					Pitch = 1f,
					PanStereo = 0f,
					SpatialBlend = 0f,
					SoloSingleTrack = false,
					SoloAllTracks = false,
					AutoUnSoloOnEnd = false,
					BypassEffects = false,
					BypassListenerEffects = false,
					BypassReverbZones = false,
					Priority = 128,
					ReverbZoneMix = 1f,
					DopplerLevel = 1f,
					Spread = 0,
					RolloffMode = AudioRolloffMode.Logarithmic,
					MinDistance = 1f,
					MaxDistance = 500f,
					DoNotAutoRecycleIfNotDonePlaying = true
				};
			}
		}

		// Token: 0x040005D3 RID: 1491
		[HideInInspector]
		public bool Initialized;

		// Token: 0x040005D4 RID: 1492
		[Header("Track")]
		public MMSoundManager.MMSoundManagerTracks MmSoundManagerTrack;

		// Token: 0x040005D5 RID: 1493
		public AudioMixerGroup AudioGroup;

		// Token: 0x040005D6 RID: 1494
		[Header("Sound")]
		public bool Loop;

		// Token: 0x040005D7 RID: 1495
		[Range(0f, 2f)]
		public float Volume;

		// Token: 0x040005D8 RID: 1496
		[Range(-3f, 3f)]
		public float Pitch;

		// Token: 0x040005D9 RID: 1497
		public int ID;

		// Token: 0x040005DA RID: 1498
		[Header("Fade")]
		public bool Fade;

		// Token: 0x040005DB RID: 1499
		[MMCondition("Fade", true)]
		public float FadeInitialVolume;

		// Token: 0x040005DC RID: 1500
		[MMCondition("Fade", true)]
		public float FadeDuration;

		// Token: 0x040005DD RID: 1501
		[MMCondition("Fade", true)]
		public MMTweenType FadeTween;

		// Token: 0x040005DE RID: 1502
		public bool Persistent;

		// Token: 0x040005DF RID: 1503
		public AudioSource RecycleAudioSource;

		// Token: 0x040005E0 RID: 1504
		[Header("Time")]
		public float PlaybackTime;

		// Token: 0x040005E1 RID: 1505
		public float PlaybackDuration;

		// Token: 0x040005E2 RID: 1506
		[Header("Spatial Settings")]
		[Range(-1f, 1f)]
		public float PanStereo;

		// Token: 0x040005E3 RID: 1507
		[Range(0f, 1f)]
		public float SpatialBlend;

		// Token: 0x040005E4 RID: 1508
		public Transform AttachToTransform;

		// Token: 0x040005E5 RID: 1509
		[Header("Solo")]
		public bool SoloSingleTrack;

		// Token: 0x040005E6 RID: 1510
		public bool SoloAllTracks;

		// Token: 0x040005E7 RID: 1511
		public bool AutoUnSoloOnEnd;

		// Token: 0x040005E8 RID: 1512
		public bool BypassEffects;

		// Token: 0x040005E9 RID: 1513
		public bool BypassListenerEffects;

		// Token: 0x040005EA RID: 1514
		public bool BypassReverbZones;

		// Token: 0x040005EB RID: 1515
		[Range(0f, 256f)]
		public int Priority;

		// Token: 0x040005EC RID: 1516
		[Range(0f, 1.1f)]
		public float ReverbZoneMix;

		// Token: 0x040005ED RID: 1517
		[Header("3D Sound Settings")]
		[Range(0f, 5f)]
		public float DopplerLevel;

		// Token: 0x040005EE RID: 1518
		public Vector3 Location;

		// Token: 0x040005EF RID: 1519
		[Range(0f, 360f)]
		public int Spread;

		// Token: 0x040005F0 RID: 1520
		public AudioRolloffMode RolloffMode;

		// Token: 0x040005F1 RID: 1521
		public float MinDistance;

		// Token: 0x040005F2 RID: 1522
		public float MaxDistance;

		// Token: 0x040005F3 RID: 1523
		public bool DoNotAutoRecycleIfNotDonePlaying;

		// Token: 0x040005F4 RID: 1524
		public bool UseCustomRolloffCurve;

		// Token: 0x040005F5 RID: 1525
		[MMCondition("UseCustomRolloffCurve", true)]
		public AnimationCurve CustomRolloffCurve;

		// Token: 0x040005F6 RID: 1526
		public bool UseSpatialBlendCurve;

		// Token: 0x040005F7 RID: 1527
		[MMCondition("UseSpatialBlendCurve", true)]
		public AnimationCurve SpatialBlendCurve;

		// Token: 0x040005F8 RID: 1528
		public bool UseReverbZoneMixCurve;

		// Token: 0x040005F9 RID: 1529
		[MMCondition("UseReverbZoneMixCurve", true)]
		public AnimationCurve ReverbZoneMixCurve;

		// Token: 0x040005FA RID: 1530
		public bool UseSpreadCurve;

		// Token: 0x040005FB RID: 1531
		[MMCondition("UseSpreadCurve", true)]
		public AnimationCurve SpreadCurve;
	}
}
