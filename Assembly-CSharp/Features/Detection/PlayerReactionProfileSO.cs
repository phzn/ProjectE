using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012D5 RID: 4821
	[CreateAssetMenu(fileName = "New Player Reaction Profile", menuName = "Detection/Player Reaction Profile")]
	public class PlayerReactionProfileSO : ScriptableObject
	{
		// Token: 0x06009573 RID: 38259 RVA: 0x003D5AF0 File Offset: 0x003D3CF0
		public PlayerReactionEntry GetReactionForState(float shameLevel, float exhibitionLevel)
		{
			PlayerReactionSet[] array;
			if (shameLevel > exhibitionLevel && shameLevel > 10f)
			{
				array = this.shameReactions;
				Debug.Log(string.Format("[PlayerReactionProfile] Using Shame reactions (Shame: {0}, Exhibition: {1})", shameLevel, exhibitionLevel));
			}
			else if (exhibitionLevel > shameLevel && exhibitionLevel > 10f)
			{
				array = this.exhibitionReactions;
				Debug.Log(string.Format("[PlayerReactionProfile] Using Exhibition reactions (Shame: {0}, Exhibition: {1})", shameLevel, exhibitionLevel));
			}
			else
			{
				array = this.neutralReactions;
				Debug.Log(string.Format("[PlayerReactionProfile] Using Neutral reactions (Shame: {0}, Exhibition: {1})", shameLevel, exhibitionLevel));
			}
			return this.GetRandomReaction(array);
		}

		// Token: 0x06009574 RID: 38260 RVA: 0x003D5B88 File Offset: 0x003D3D88
		private PlayerReactionEntry GetRandomReaction(PlayerReactionSet[] reactions)
		{
			if (reactions == null || reactions.Length == 0)
			{
				Debug.LogWarning("[PlayerReactionProfile] No reactions configured for this state!");
				return new PlayerReactionEntry();
			}
			int num = 0;
			foreach (PlayerReactionSet playerReactionSet in reactions)
			{
				num += playerReactionSet.weight;
			}
			if (num == 0)
			{
				return reactions[0].reaction;
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			foreach (PlayerReactionSet playerReactionSet2 in reactions)
			{
				num3 += playerReactionSet2.weight;
				if (num2 < num3)
				{
					return playerReactionSet2.reaction;
				}
			}
			return reactions[0].reaction;
		}

		// Token: 0x06009575 RID: 38261 RVA: 0x003D5C20 File Offset: 0x003D3E20
		public PlayerReactionEntry GetShameReaction()
		{
			return this.GetRandomReaction(this.shameReactions);
		}

		// Token: 0x06009576 RID: 38262 RVA: 0x003D5C2E File Offset: 0x003D3E2E
		public PlayerReactionEntry GetExhibitionReaction()
		{
			return this.GetRandomReaction(this.exhibitionReactions);
		}

		// Token: 0x06009577 RID: 38263 RVA: 0x003D5C3C File Offset: 0x003D3E3C
		public PlayerReactionEntry GetNeutralReaction()
		{
			return this.GetRandomReaction(this.neutralReactions);
		}

		// Token: 0x04009A97 RID: 39575
		[Header("Player Info")]
		[Tooltip("Display name for the player in dialogue UI")]
		public string playerDisplayName = "???";

		// Token: 0x04009A98 RID: 39576
		[Header("Shame Reactions (Embarrassed State)")]
		[Tooltip("Reactions when player has high Shame level")]
		public PlayerReactionSet[] shameReactions;

		// Token: 0x04009A99 RID: 39577
		[Header("Exhibition Reactions (Confident State)")]
		[Tooltip("Reactions when player has high Exhibition level")]
		public PlayerReactionSet[] exhibitionReactions;

		// Token: 0x04009A9A RID: 39578
		[Header("Neutral Reactions")]
		[Tooltip("Reactions when neither emotion is dominant")]
		public PlayerReactionSet[] neutralReactions;
	}
}
