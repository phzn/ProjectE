using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F2F RID: 3887
	[CreateAssetMenu(fileName = "NewSceneDefinition", menuName = "NPC/Scene System/Scene Definition")]
	public class SceneDefinitionSO : ScriptableObject
	{
		// Token: 0x060074B9 RID: 29881 RVA: 0x002F51C8 File Offset: 0x002F33C8
		public SceneNPCSlot GetSlot(string slotId)
		{
			return this.npcSlots.Find((SceneNPCSlot s) => s.slotId == slotId);
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x002F51FC File Offset: 0x002F33FC
		public bool Validate(out string error)
		{
			if (this.npcSlots == null || this.npcSlots.Count == 0)
			{
				error = "Scene has no NPC slots defined";
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (SceneNPCSlot sceneNPCSlot in this.npcSlots)
			{
				if (string.IsNullOrEmpty(sceneNPCSlot.slotId))
				{
					error = "NPC slot has empty slotId";
					return false;
				}
				if (!hashSet.Add(sceneNPCSlot.slotId))
				{
					error = "Duplicate NPC slot ID: " + sceneNPCSlot.slotId;
					return false;
				}
				if (sceneNPCSlot.characterData == null)
				{
					error = "NPC slot '" + sceneNPCSlot.slotId + "' has no CharacterObject assigned";
					return false;
				}
			}
			if (this.actionSequence == null || this.actionSequence.Count == 0)
			{
				error = "Scene has no actions defined";
				return false;
			}
			error = null;
			return true;
		}

		// Token: 0x0400777B RID: 30587
		[Header("Scene Info")]
		[Tooltip("Display name for this scene")]
		public string sceneName;

		// Token: 0x0400777C RID: 30588
		[TextArea(2, 4)]
		[Tooltip("Description of what happens in this scene")]
		public string description;

		// Token: 0x0400777D RID: 30589
		[Header("NPC Participants")]
		[Tooltip("List of NPCs that participate in this scene")]
		public List<SceneNPCSlot> npcSlots = new List<SceneNPCSlot>();

		// Token: 0x0400777E RID: 30590
		[Header("Action Sequence")]
		[Tooltip("Ordered list of actions that make up this scene")]
		public List<SceneActionEntry> actionSequence = new List<SceneActionEntry>();

		// Token: 0x0400777F RID: 30591
		[Header("Scene Settings")]
		[Tooltip("If true, the scene loops back to the beginning after completing")]
		public bool loopSequence;

		// Token: 0x04007780 RID: 30592
		[Tooltip("If true, the player can exit the scene at any time")]
		public bool allowPlayerExit = true;

		// Token: 0x04007781 RID: 30593
		[Tooltip("Prompt shown to player when they can exit (e.g. 'Press E to leave')")]
		public string exitPrompt = "Press E to leave";
	}
}
