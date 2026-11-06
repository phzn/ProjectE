using System;
using Features.Detection.Customization;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012BE RID: 4798
	[CreateAssetMenu(fileName = "New NPC Dialogue Profile", menuName = "Detection/NPC Dialogue Profile")]
	public class NPCDialogueProfileSO : ScriptableObject
	{
		// Token: 0x060094BF RID: 38079 RVA: 0x003D1D60 File Offset: 0x003CFF60
		public DialogueLine[] GetRandomDialogue(DialogueSet[] dialogues)
		{
			if (dialogues == null || dialogues.Length == 0)
			{
				return null;
			}
			int num = 0;
			foreach (DialogueSet dialogueSet in dialogues)
			{
				num += dialogueSet.weight;
			}
			if (num == 0)
			{
				return DialogueLine.CloneForPresentation(dialogues[0].lines, "dialogue", "dialogue_speaker");
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			foreach (DialogueSet dialogueSet2 in dialogues)
			{
				num3 += dialogueSet2.weight;
				if (num2 < num3)
				{
					return DialogueLine.CloneForPresentation(dialogueSet2.lines, "dialogue", "dialogue_speaker");
				}
			}
			return DialogueLine.CloneForPresentation(dialogues[0].lines, "dialogue", "dialogue_speaker");
		}

		// Token: 0x060094C0 RID: 38080 RVA: 0x003D1E17 File Offset: 0x003D0017
		public DialogueLine[] GetDetectionDialogue()
		{
			return this.GetRandomDialogue(this.detectionDialogues);
		}

		// Token: 0x060094C1 RID: 38081 RVA: 0x003D1E25 File Offset: 0x003D0025
		public DialogueLine[] GetReactionDialogue()
		{
			return this.GetRandomDialogue(this.reactionDialogues);
		}

		// Token: 0x060094C2 RID: 38082 RVA: 0x003D1E33 File Offset: 0x003D0033
		public DialogueLine[] GetShameDialogue()
		{
			return this.GetRandomDialogue(this.shameDialogues);
		}

		// Token: 0x060094C3 RID: 38083 RVA: 0x003D1E41 File Offset: 0x003D0041
		public DialogueLine[] GetExhibitDialogue()
		{
			return this.GetRandomDialogue(this.exhibitDialogues);
		}

		// Token: 0x060094C4 RID: 38084 RVA: 0x003D1E4F File Offset: 0x003D004F
		public DialogueLine[] GetExitDialogue()
		{
			return this.GetRandomDialogue(this.exitDialogues);
		}

		// Token: 0x060094C5 RID: 38085 RVA: 0x003D1E5D File Offset: 0x003D005D
		public DialogueLine[] GetChasingDialogue()
		{
			return this.GetRandomDialogue(this.chasingDialogues);
		}

		// Token: 0x040099C5 RID: 39365
		[Header("NPC Info")]
		public string npcDisplayName = "NPC";

		// Token: 0x040099C6 RID: 39366
		[Header("Dialogue Identity")]
		[Tooltip("Human-readable name for this DIALOGUE (the whole SO / phase flow), shown in the Dialogues app so it can be recognized, assigned and searched. This names the dialogue, NOT the character.")]
		public string dialogueName;

		// Token: 0x040099C7 RID: 39367
		[Tooltip("MACRO weight: when several dialogues match the same personality (+gender) binding, this is the relative chance THIS dialogue is picked for an NPC at spawn. Higher = more likely. (Per-phase alternatives still use each DialogueSet.weight as the MICRO weight.)")]
		[Range(1f, 100f)]
		public int macroWeight = 10;

		// Token: 0x040099C8 RID: 39368
		[Tooltip("Which gender this profile's lines are written for. 'Any' fits NPCs of either gender. The dialogue library/registry is the single source of truth for the personality+gender binding.")]
		public DialogueGender targetGender;

		// Token: 0x040099C9 RID: 39369
		[Header("Detection Dialogues")]
		[Tooltip("Dialogues shown when NPC first detects player")]
		public DialogueSet[] detectionDialogues;

		// Token: 0x040099CA RID: 39370
		[Header("Reaction Dialogues")]
		[Tooltip("Dialogues shown during NPC reaction (after detection)")]
		public DialogueSet[] reactionDialogues;

		// Token: 0x040099CB RID: 39371
		[Header("Post-Action Dialogues")]
		[Tooltip("Dialogues shown after player performs Shame action")]
		public DialogueSet[] shameDialogues;

		// Token: 0x040099CC RID: 39372
		[Tooltip("Dialogues shown after player performs Exhibit action")]
		public DialogueSet[] exhibitDialogues;

		// Token: 0x040099CD RID: 39373
		[Header("Exit Dialogues")]
		[Tooltip("Dialogues shown when NPC leaves")]
		public DialogueSet[] exitDialogues;

		// Token: 0x040099CE RID: 39374
		[Header("Chasing Dialogues")]
		[Tooltip("Dialogues shown while chasing the player (before encounter starts). One phrase shown per chase, timer-based.")]
		public DialogueSet[] chasingDialogues;
	}
}
