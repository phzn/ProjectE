using System;
using System.Collections.Generic;

namespace Features.Detection.Customization
{
	// Token: 0x0200131A RID: 4890
	[Serializable]
	public class CustomDialoguePhase
	{
		// Token: 0x04009CB9 RID: 40121
		public DialogueCategory category;

		// Token: 0x04009CBA RID: 40122
		public List<CustomDialogueSet> sets = new List<CustomDialogueSet>();
	}
}
