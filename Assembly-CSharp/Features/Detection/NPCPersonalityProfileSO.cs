using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C4 RID: 4804
	[CreateAssetMenu(fileName = "New Personality Profile", menuName = "Detection/NPC Personality Profile")]
	public class NPCPersonalityProfileSO : ScriptableObject
	{
		// Token: 0x060094D5 RID: 38101 RVA: 0x003D2078 File Offset: 0x003D0278
		public int GetExpressionForReaction(ReactionType reactionType)
		{
			foreach (NPCFacialExpressionEntry npcfacialExpressionEntry in this.facialExpressions)
			{
				if (npcfacialExpressionEntry.reactionType == reactionType)
				{
					return npcfacialExpressionEntry.expressionIndex;
				}
			}
			return this.neutralExpressionIndex;
		}

		// Token: 0x060094D6 RID: 38102 RVA: 0x003D20E0 File Offset: 0x003D02E0
		public NPCFacialExpressionEntry GetExpressionEntryForReaction(ReactionType reactionType)
		{
			foreach (NPCFacialExpressionEntry npcfacialExpressionEntry in this.facialExpressions)
			{
				if (npcfacialExpressionEntry.reactionType == reactionType)
				{
					return npcfacialExpressionEntry;
				}
			}
			return null;
		}

		// Token: 0x060094D7 RID: 38103 RVA: 0x003D213C File Offset: 0x003D033C
		public bool HasReactionType(ReactionType reactionType)
		{
			if (this.reactionProfile == null || this.reactionProfile.Reactions == null)
			{
				return false;
			}
			using (List<NPCReactionEntry>.Enumerator enumerator = this.reactionProfile.Reactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Type == reactionType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040099E1 RID: 39393
		[Header("Personality Info")]
		[Tooltip("The personality type this profile represents")]
		public PersonalityType personalityType;

		// Token: 0x040099E2 RID: 39394
		[Tooltip("Display name for this personality (for debugging/UI)")]
		public string displayName = "Neutral";

		// Token: 0x040099E3 RID: 39395
		[TextArea(2, 4)]
		[Tooltip("Description of this personality's behavior")]
		public string description;

		// Token: 0x040099E4 RID: 39396
		[Header("Reaction Configuration")]
		[Tooltip("Reaction profile containing available reactions for this personality")]
		public NPCReactionProfileSO reactionProfile;

		// Token: 0x040099E5 RID: 39397
		[Header("Facial Expressions")]
		[Tooltip("Name of the animator layer for facial expressions")]
		public string faceLayerName = "Face";

		// Token: 0x040099E6 RID: 39398
		[Tooltip("Animator parameter name for expression index")]
		public string expressionParamName = "FaceExpressionIndex";

		// Token: 0x040099E7 RID: 39399
		[Tooltip("Default/neutral expression index")]
		public int neutralExpressionIndex;

		// Token: 0x040099E8 RID: 39400
		[Tooltip("Expression index to use when NPC enters alert states (LookAt, StopAndStare, Searching, etc.)")]
		[Header("Alert Expression (0=Neutral, 1=Surprised, 2=Angry, 3=Happy, 4=Embarrassed, 5=Scared)")]
		public int alertExpressionIndex = 1;

		// Token: 0x040099E9 RID: 39401
		[Tooltip("Facial expressions mapped to reaction types")]
		public List<NPCFacialExpressionEntry> facialExpressions = new List<NPCFacialExpressionEntry>();

		// Token: 0x040099EA RID: 39402
		[Header("Behavior Modifiers")]
		[Tooltip("Multiplier for attention gain rate (higher = notices player faster)")]
		[Range(0.5f, 2f)]
		public float attentionGainMultiplier = 1f;

		// Token: 0x040099EB RID: 39403
		[Tooltip("Multiplier for attention decay rate (higher = loses interest faster)")]
		[Range(0.5f, 2f)]
		public float attentionDecayMultiplier = 1f;

		// Token: 0x040099EC RID: 39404
		[Tooltip("Base probability to trigger a reaction when detecting player (0-1)")]
		[Range(0f, 1f)]
		public float reactionProbability = 1f;
	}
}
