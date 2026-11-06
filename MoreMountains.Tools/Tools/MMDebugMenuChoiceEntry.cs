using System;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class MMDebugMenuChoiceEntry
	{
		// Token: 0x040001A0 RID: 416
		public Button TargetButton;

		// Token: 0x040001A1 RID: 417
		public Text ButtonText;

		// Token: 0x040001A2 RID: 418
		public Image ButtonBg;

		// Token: 0x040001A3 RID: 419
		public string ButtonEventName = "ButtonEvent";
	}
}
