using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200016E RID: 366
	[AddComponentMenu("More Mountains/Tools/Achievements/MMAchievementDisplayItem")]
	public class MMAchievementDisplayItem : MonoBehaviour
	{
		// Token: 0x04000A49 RID: 2633
		public Image BackgroundLocked;

		// Token: 0x04000A4A RID: 2634
		public Image BackgroundUnlocked;

		// Token: 0x04000A4B RID: 2635
		public Image Icon;

		// Token: 0x04000A4C RID: 2636
		public Text Title;

		// Token: 0x04000A4D RID: 2637
		public Text Description;

		// Token: 0x04000A4E RID: 2638
		public MMProgressBar ProgressBarDisplay;
	}
}
