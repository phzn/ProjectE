using System;

namespace TMPro
{
	// Token: 0x02000028 RID: 40
	internal interface ITweenValue
	{
		// Token: 0x0600013C RID: 316
		void TweenValue(float floatPercentage);

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600013D RID: 317
		bool ignoreTimeScale { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600013E RID: 318
		float duration { get; }

		// Token: 0x0600013F RID: 319
		bool ValidTarget();
	}
}
