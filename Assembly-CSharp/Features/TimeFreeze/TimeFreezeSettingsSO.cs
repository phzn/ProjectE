using System;
using UnityEngine;

namespace Features.TimeFreeze
{
	// Token: 0x02000C1A RID: 3098
	[CreateAssetMenu(fileName = "TimeFreezeSettings", menuName = "Features/Time Freeze/Settings")]
	public sealed class TimeFreezeSettingsSO : ScriptableObject
	{
		// Token: 0x04005A72 RID: 23154
		[Header("Activation")]
		public AnimationClip activationClip;

		// Token: 0x04005A73 RID: 23155
		[Tooltip("Dedicated final Animator layer masked to both arms only.")]
		public string activationLayerName = "TimeFreezeArms";

		// Token: 0x04005A74 RID: 23156
		[Tooltip("State on activationLayerName that directly owns activationClip.")]
		public string activationStateName = "00_TimeStop_A";

		// Token: 0x04005A75 RID: 23157
		[Min(0f)]
		public float activationBlendInDuration = 0.26f;

		// Token: 0x04005A76 RID: 23158
		[Tooltip("Normalized point at which the authored return-to-arms-down tail is replaced by a blend back to Ciel's current stance.")]
		[Range(0.1f, 0.95f)]
		public float activationFadeOutNormalizedTime = 0.68f;

		// Token: 0x04005A77 RID: 23159
		[Min(0f)]
		public float activationBlendOutDuration = 0.32f;

		// Token: 0x04005A78 RID: 23160
		[Min(0f)]
		public float activationDelay = 0.35f;

		// Token: 0x04005A79 RID: 23161
		[Min(0.05f)]
		public float slowDownDuration = 1.8f;

		// Token: 0x04005A7A RID: 23162
		[Header("Effect")]
		[Range(1f, 60f)]
		public float frozenDuration = 60f;

		// Token: 0x04005A7B RID: 23163
		[Min(0.05f)]
		public float resumeDuration = 1.6f;

		// Token: 0x04005A7C RID: 23164
		[Header("Audio")]
		[Range(0.01f, 0.25f)]
		public float minimumWorldPitch = 0.12f;

		// Token: 0x04005A7D RID: 23165
		[Range(0f, 1f)]
		public float transitionVolume = 0.36f;

		// Token: 0x04005A7E RID: 23166
		[Range(0f, 1f)]
		public float resumeVolumeMultiplier = 0.8f;

		// Token: 0x04005A7F RID: 23167
		[Range(0f, 1f)]
		public float lockCueVolume = 0.55f;

		// Token: 0x04005A80 RID: 23168
		[Range(0f, 0.25f)]
		public float frozenHumVolume = 0.022f;

		// Token: 0x04005A81 RID: 23169
		[Min(0f)]
		public float frozenHumFadeDuration = 0.28f;

		// Token: 0x04005A82 RID: 23170
		[Header("Visual")]
		[Range(-100f, 0f)]
		public float frozenSaturation = -32f;

		// Token: 0x04005A83 RID: 23171
		[Range(0f, 1f)]
		public float frozenVignette = 0.22f;

		// Token: 0x04005A84 RID: 23172
		[Range(0f, 1f)]
		public float frozenChromaticAberration = 0.08f;
	}
}
