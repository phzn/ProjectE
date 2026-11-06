using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x0200131E RID: 4894
	[CreateAssetMenu(fileName = "DialogueLibraryRegistry", menuName = "Detection/Dialogue Library Registry")]
	public class DialogueLibraryRegistrySO : ScriptableObject
	{
		// Token: 0x06009768 RID: 38760 RVA: 0x003E141C File Offset: 0x003DF61C
		public static DialogueLibraryRegistrySO LoadDefault()
		{
			return Resources.Load<DialogueLibraryRegistrySO>("DialogueLibraryRegistry");
		}

		// Token: 0x04009CC8 RID: 40136
		public const string ResourcePath = "DialogueLibraryRegistry";

		// Token: 0x04009CC9 RID: 40137
		[Tooltip("Every system dialogue profile, tagged with its personality and target gender.")]
		public List<DialogueLibraryRegistrySO.SystemProfileEntry> systemProfiles = new List<DialogueLibraryRegistrySO.SystemProfileEntry>();

		// Token: 0x0200131F RID: 4895
		[Serializable]
		public class SystemProfileEntry
		{
			// Token: 0x04009CCA RID: 40138
			public NPCDialogueProfileSO profile;

			// Token: 0x04009CCB RID: 40139
			public PersonalityType personality;

			// Token: 0x04009CCC RID: 40140
			public DialogueGender gender;

			// Token: 0x04009CCD RID: 40141
			[Tooltip("Which ACTOR database this profile belongs to (Civilians / Police / Journalists / Referee / Stadium Commentator). Drives the 'NPC Groups' navigation in the Dialogues app.")]
			public DialogueActorGroup actorGroup;

			// Token: 0x04009CCE RID: 40142
			[Tooltip("Override-only profile (Police, Reporter, ...). Stays visible/editable in the catalog UI but is NEVER drawn by the generic personality pool — only used when explicitly assigned (encounter dialogueOverride, or a per-NPC whitelist).")]
			public bool excludeFromPool;
		}
	}
}
