using System;

namespace DA_Assets.FCU
{
	// Token: 0x02000021 RID: 33
	[Flags]
	public enum FcuDebugSettingsFlags
	{
		// Token: 0x0400009B RID: 155
		None = 0,
		// Token: 0x0400009C RID: 156
		LogDefault = 1,
		// Token: 0x0400009D RID: 157
		LogSetTag = 2,
		// Token: 0x0400009E RID: 158
		LogIsDownloadable = 4,
		// Token: 0x0400009F RID: 159
		LogTransform = 8,
		// Token: 0x040000A0 RID: 160
		LogGameObjectDrawer = 16,
		// Token: 0x040000A1 RID: 161
		LogComponentDrawer = 32,
		// Token: 0x040000A2 RID: 162
		LogHashGenerator = 64,
		// Token: 0x040000A3 RID: 163
		DebugMode = 128,
		// Token: 0x040000A4 RID: 164
		LogError = 256,
		// Token: 0x040000A5 RID: 165
		LogAngle = 512,
		// Token: 0x040000A6 RID: 166
		LogMask = 1024
	}
}
