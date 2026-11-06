using System;

namespace UnityEngine.Localization.SmartFormat.Utilities
{
	// Token: 0x02000046 RID: 70
	[Flags]
	public enum TimeSpanFormatOptions
	{
		// Token: 0x04000098 RID: 152
		InheritDefaults = 0,
		// Token: 0x04000099 RID: 153
		Abbreviate = 1,
		// Token: 0x0400009A RID: 154
		AbbreviateOff = 2,
		// Token: 0x0400009B RID: 155
		LessThan = 4,
		// Token: 0x0400009C RID: 156
		LessThanOff = 8,
		// Token: 0x0400009D RID: 157
		TruncateShortest = 16,
		// Token: 0x0400009E RID: 158
		TruncateAuto = 32,
		// Token: 0x0400009F RID: 159
		TruncateFill = 64,
		// Token: 0x040000A0 RID: 160
		TruncateFull = 128,
		// Token: 0x040000A1 RID: 161
		RangeMilliSeconds = 256,
		// Token: 0x040000A2 RID: 162
		RangeSeconds = 512,
		// Token: 0x040000A3 RID: 163
		RangeMinutes = 1024,
		// Token: 0x040000A4 RID: 164
		RangeHours = 2048,
		// Token: 0x040000A5 RID: 165
		RangeDays = 4096,
		// Token: 0x040000A6 RID: 166
		RangeWeeks = 8192
	}
}
