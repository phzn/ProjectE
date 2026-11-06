using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000018 RID: 24
	[Flags]
	public enum ClipCaps
	{
		// Token: 0x04000090 RID: 144
		None = 0,
		// Token: 0x04000091 RID: 145
		Looping = 1,
		// Token: 0x04000092 RID: 146
		Extrapolation = 2,
		// Token: 0x04000093 RID: 147
		ClipIn = 4,
		// Token: 0x04000094 RID: 148
		SpeedMultiplier = 8,
		// Token: 0x04000095 RID: 149
		Blending = 16,
		// Token: 0x04000096 RID: 150
		AutoScale = 40,
		// Token: 0x04000097 RID: 151
		All = -1
	}
}
