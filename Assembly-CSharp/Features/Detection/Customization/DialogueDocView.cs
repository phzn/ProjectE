using System;
using Bozo.AnimeCharacters;

namespace Features.Detection.Customization
{
	// Token: 0x02001323 RID: 4899
	public readonly struct DialogueDocView
	{
		// Token: 0x06009771 RID: 38769 RVA: 0x003E15AC File Offset: 0x003DF7AC
		public DialogueDocView(string id, bool isSystem, string dialogueName, PersonalityType personality, DialogueGender gender, DialogueActorGroup actorGroup, int macroWeight, DialoguePhaseView[] phases)
		{
			this.id = id;
			this.isSystem = isSystem;
			this.dialogueName = dialogueName ?? "";
			this.personality = personality;
			this.gender = gender;
			this.actorGroup = actorGroup;
			this.macroWeight = macroWeight;
			this.phases = phases ?? Array.Empty<DialoguePhaseView>();
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x06009772 RID: 38770 RVA: 0x003E1608 File Offset: 0x003DF808
		public string DisplayLabel
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.dialogueName))
				{
					return "(unnamed)";
				}
				return this.dialogueName;
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x06009773 RID: 38771 RVA: 0x003E1624 File Offset: 0x003DF824
		public int TotalLines
		{
			get
			{
				int num = 0;
				foreach (DialoguePhaseView dialoguePhaseView in this.phases)
				{
					num += dialoguePhaseView.LineCount;
				}
				return num;
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06009774 RID: 38772 RVA: 0x003E165C File Offset: 0x003DF85C
		public int CustomAlternatives
		{
			get
			{
				int num = 0;
				DialoguePhaseView[] array = this.phases;
				for (int i = 0; i < array.Length; i++)
				{
					DialogueAltView[] alternatives = array[i].alternatives;
					for (int j = 0; j < alternatives.Length; j++)
					{
						if (!alternatives[j].isSystem)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x04009CE1 RID: 40161
		public readonly string id;

		// Token: 0x04009CE2 RID: 40162
		public readonly bool isSystem;

		// Token: 0x04009CE3 RID: 40163
		public readonly string dialogueName;

		// Token: 0x04009CE4 RID: 40164
		public readonly PersonalityType personality;

		// Token: 0x04009CE5 RID: 40165
		public readonly DialogueGender gender;

		// Token: 0x04009CE6 RID: 40166
		public readonly DialogueActorGroup actorGroup;

		// Token: 0x04009CE7 RID: 40167
		public readonly int macroWeight;

		// Token: 0x04009CE8 RID: 40168
		public readonly DialoguePhaseView[] phases;
	}
}
