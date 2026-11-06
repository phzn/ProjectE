using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200023F RID: 575
	[Serializable]
	public struct MMCameraShakeProperties
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x00046E4A File Offset: 0x0004504A
		public MMCameraShakeProperties(float duration, float amplitude, float frequency, float amplitudeX = 0f, float amplitudeY = 0f, float amplitudeZ = 0f)
		{
			this.Duration = duration;
			this.Amplitude = amplitude;
			this.Frequency = frequency;
			this.AmplitudeX = amplitudeX;
			this.AmplitudeY = amplitudeY;
			this.AmplitudeZ = amplitudeZ;
		}

		// Token: 0x0400126B RID: 4715
		public float Duration;

		// Token: 0x0400126C RID: 4716
		public float Amplitude;

		// Token: 0x0400126D RID: 4717
		public float Frequency;

		// Token: 0x0400126E RID: 4718
		public float AmplitudeX;

		// Token: 0x0400126F RID: 4719
		public float AmplitudeY;

		// Token: 0x04001270 RID: 4720
		public float AmplitudeZ;
	}
}
