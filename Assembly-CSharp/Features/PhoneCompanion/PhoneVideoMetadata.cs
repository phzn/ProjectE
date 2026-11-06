using System;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E49 RID: 3657
	[Serializable]
	public sealed class PhoneVideoMetadata
	{
		// Token: 0x04006FFC RID: 28668
		public string fileName;

		// Token: 0x04006FFD RID: 28669
		public string createdUtc;

		// Token: 0x04006FFE RID: 28670
		public float durationSeconds;

		// Token: 0x04006FFF RID: 28671
		public int width;

		// Token: 0x04007000 RID: 28672
		public int height;

		// Token: 0x04007001 RID: 28673
		public int frameRate;

		// Token: 0x04007002 RID: 28674
		public bool hasAudio;
	}
}
