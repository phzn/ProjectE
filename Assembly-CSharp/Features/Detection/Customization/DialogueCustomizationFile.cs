using System;
using System.Collections.Generic;

namespace Features.Detection.Customization
{
	// Token: 0x0200131C RID: 4892
	[Serializable]
	public class DialogueCustomizationFile
	{
		// Token: 0x04009CC2 RID: 40130
		public List<CustomDialogueEntry> customEntries = new List<CustomDialogueEntry>();

		// Token: 0x04009CC3 RID: 40131
		public List<CustomDialogueDoc> customDocs = new List<CustomDialogueDoc>();

		// Token: 0x04009CC4 RID: 40132
		public List<DialogueWeightOverride> weightOverrides = new List<DialogueWeightOverride>();

		// Token: 0x04009CC5 RID: 40133
		public List<DialogueMacroWeightOverride> macroWeightOverrides = new List<DialogueMacroWeightOverride>();

		// Token: 0x04009CC6 RID: 40134
		public List<NpcDialogueAssignment> npcAssignments = new List<NpcDialogueAssignment>();
	}
}
