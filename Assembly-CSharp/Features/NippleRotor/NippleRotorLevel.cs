using System;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F53 RID: 3923
	[Serializable]
	public struct NippleRotorLevel
	{
		// Token: 0x060075A4 RID: 30116 RVA: 0x002FA581 File Offset: 0x002F8781
		public NippleRotorLevel(float noiseRadius, float jitterAmplitude, float jitterHz, float wobbleDegrees, float volume, float pitch)
		{
			this.noiseRadius = noiseRadius;
			this.jitterAmplitude = jitterAmplitude;
			this.jitterHz = jitterHz;
			this.wobbleDegrees = wobbleDegrees;
			this.volume = volume;
			this.pitch = pitch;
		}

		// Token: 0x0400787D RID: 30845
		[Tooltip("NPC hearing radius in metres (StealthActor external noise).")]
		public float noiseRadius;

		// Token: 0x0400787E RID: 30846
		[Tooltip("Cup jitter in anchor-local units (Ciel mesh units, about 1/1.4 m).")]
		public float jitterAmplitude;

		// Token: 0x0400787F RID: 30847
		[Tooltip("Retarget rate of the jitter noise.")]
		public float jitterHz;

		// Token: 0x04007880 RID: 30848
		[Tooltip("Small random roll/tilt of each cup.")]
		public float wobbleDegrees;

		// Token: 0x04007881 RID: 30849
		[Range(0f, 1f)]
		public float volume;

		// Token: 0x04007882 RID: 30850
		public float pitch;
	}
}
