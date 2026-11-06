using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200127C RID: 4732
	[CreateAssetMenu(fileName = "DialoguePack_", menuName = "Detection/Dialogue Pack")]
	public class DialoguePackSO : ScriptableObject
	{
		// Token: 0x04009794 RID: 38804
		[Header("Reaction — NPC scopre il player")]
		public PackDialogueLine[] reactionLines;

		// Token: 0x04009795 RID: 38805
		[Header("Post-Shame")]
		public PackDialogueLine[] postShameLines;

		// Token: 0x04009796 RID: 38806
		[Header("Post-Exhibit")]
		public PackDialogueLine[] postExhibitLines;

		// Token: 0x04009797 RID: 38807
		[Header("Exit")]
		public PackDialogueLine[] exitLines;
	}
}
