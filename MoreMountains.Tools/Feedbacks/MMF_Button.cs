using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C4 RID: 452
	[Serializable]
	public class MMF_Button
	{
		// Token: 0x06000B0C RID: 2828 RVA: 0x00030DC9 File Offset: 0x0002EFC9
		public MMF_Button(string buttonText, MMF_Button.ButtonMethod method)
		{
			this.ButtonText = buttonText;
			this.TargetMethod = method;
		}

		// Token: 0x04000D04 RID: 3332
		public string ButtonText;

		// Token: 0x04000D05 RID: 3333
		public MMF_Button.ButtonMethod TargetMethod;

		// Token: 0x0200049B RID: 1179
		// (Invoke) Token: 0x06001B41 RID: 6977
		public delegate void ButtonMethod();
	}
}
