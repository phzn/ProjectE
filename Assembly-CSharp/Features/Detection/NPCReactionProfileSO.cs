using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C7 RID: 4807
	[CreateAssetMenu(fileName = "New Reaction Profile", menuName = "Detection/Reaction Profile")]
	public class NPCReactionProfileSO : ScriptableObject
	{
		// Token: 0x060094DA RID: 38106 RVA: 0x003D224C File Offset: 0x003D044C
		public NPCReactionEntry GetRandomReaction()
		{
			if (this.Reactions == null || this.Reactions.Count == 0)
			{
				return null;
			}
			int num = 0;
			foreach (NPCReactionEntry npcreactionEntry in this.Reactions)
			{
				num += npcreactionEntry.Weight;
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			foreach (NPCReactionEntry npcreactionEntry2 in this.Reactions)
			{
				num3 += npcreactionEntry2.Weight;
				if (num2 < num3)
				{
					return npcreactionEntry2;
				}
			}
			return this.Reactions[0];
		}

		// Token: 0x060094DB RID: 38107 RVA: 0x003D2328 File Offset: 0x003D0528
		public NPCReactionEntry GetPassiveReactionEntry()
		{
			if (this.Reactions == null || this.Reactions.Count == 0)
			{
				return null;
			}
			List<NPCReactionEntry> list = new List<NPCReactionEntry>();
			foreach (NPCReactionEntry npcreactionEntry in this.Reactions)
			{
				if (npcreactionEntry.Type == ReactionType.TakePhoto || npcreactionEntry.Type == ReactionType.TakeVideo)
				{
					list.Add(npcreactionEntry);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			int num = 0;
			foreach (NPCReactionEntry npcreactionEntry2 in list)
			{
				num += npcreactionEntry2.Weight;
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			foreach (NPCReactionEntry npcreactionEntry3 in list)
			{
				num3 += npcreactionEntry3.Weight;
				if (num2 < num3)
				{
					return npcreactionEntry3;
				}
			}
			return list[0];
		}

		// Token: 0x060094DC RID: 38108 RVA: 0x003D2460 File Offset: 0x003D0660
		public int GetPassiveReactionAnimatorType()
		{
			NPCReactionEntry passiveReactionEntry = this.GetPassiveReactionEntry();
			if (passiveReactionEntry == null)
			{
				return 1;
			}
			if (passiveReactionEntry.AnimatorReactionIndex > 0)
			{
				return passiveReactionEntry.AnimatorReactionIndex;
			}
			if (passiveReactionEntry.Type != ReactionType.TakeVideo)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x040099FE RID: 39422
		public List<NPCReactionEntry> Reactions;
	}
}
