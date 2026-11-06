using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x02001315 RID: 4885
	[Serializable]
	public class CustomDialogueEntry
	{
		// Token: 0x04009CA9 RID: 40105
		public string id;

		// Token: 0x04009CAA RID: 40106
		public string displayName;

		// Token: 0x04009CAB RID: 40107
		public PersonalityType personality;

		// Token: 0x04009CAC RID: 40108
		public DialogueGender gender;

		// Token: 0x04009CAD RID: 40109
		public DialogueCategory category;

		// Token: 0x04009CAE RID: 40110
		[Range(1f, 100f)]
		public int weight = 10;

		// Token: 0x04009CAF RID: 40111
		public DialogueLine[] lines;
	}
}
