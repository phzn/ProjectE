using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x0200131B RID: 4891
	[Serializable]
	public class CustomDialogueDoc
	{
		// Token: 0x04009CBB RID: 40123
		public string id;

		// Token: 0x04009CBC RID: 40124
		public string displayName;

		// Token: 0x04009CBD RID: 40125
		public PersonalityType personality;

		// Token: 0x04009CBE RID: 40126
		public DialogueGender gender;

		// Token: 0x04009CBF RID: 40127
		public DialogueActorGroup actorGroup;

		// Token: 0x04009CC0 RID: 40128
		[Range(1f, 100f)]
		public int macroWeight = 10;

		// Token: 0x04009CC1 RID: 40129
		public List<CustomDialoguePhase> phases = new List<CustomDialoguePhase>();
	}
}
