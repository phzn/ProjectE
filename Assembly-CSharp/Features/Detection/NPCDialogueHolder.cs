using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012BC RID: 4796
	public class NPCDialogueHolder : MonoBehaviour
	{
		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x060094A7 RID: 38055 RVA: 0x003D19F6 File Offset: 0x003CFBF6
		// (set) Token: 0x060094A8 RID: 38056 RVA: 0x003D19FE File Offset: 0x003CFBFE
		public PersonalityType Personality { get; private set; }

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x060094A9 RID: 38057 RVA: 0x003D1A07 File Offset: 0x003CFC07
		// (set) Token: 0x060094AA RID: 38058 RVA: 0x003D1A0F File Offset: 0x003CFC0F
		public Gender Gender { get; private set; }

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x060094AB RID: 38059 RVA: 0x003D1A18 File Offset: 0x003CFC18
		// (set) Token: 0x060094AC RID: 38060 RVA: 0x003D1A20 File Offset: 0x003CFC20
		public Occupation Occupation { get; private set; }

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x060094AD RID: 38061 RVA: 0x003D1A29 File Offset: 0x003CFC29
		// (set) Token: 0x060094AE RID: 38062 RVA: 0x003D1A31 File Offset: 0x003CFC31
		public string NpcIdentityId { get; private set; } = "";

		// Token: 0x060094AF RID: 38063 RVA: 0x003D1A3A File Offset: 0x003CFC3A
		public void SetNpcIdentity(string npcIdentityId)
		{
			this.NpcIdentityId = npcIdentityId ?? "";
		}

		// Token: 0x060094B0 RID: 38064 RVA: 0x003D1A4C File Offset: 0x003CFC4C
		public void Initialize(CharacterGeneralInfo info)
		{
			if (info == null)
			{
				return;
			}
			this.Personality = info.personality;
			this.Gender = info.gender;
			this.Occupation = info.occupation;
			this.FirstName = info.characterName;
			this.Surname = info.surname;
			this.FullName = (info.characterName + " " + info.surname).Trim();
			if (this.autoAssignFromPersonality)
			{
				this.AssignProfileFromPersonality();
			}
		}

		// Token: 0x060094B1 RID: 38065 RVA: 0x003D1AC8 File Offset: 0x003CFCC8
		public void AssignProfileFromPersonality()
		{
			if (NPCPersonalityManager.Instance != null)
			{
				this.PersonalityProfile = NPCPersonalityManager.Instance.GetProfileForPersonality(this.Personality);
				if (this.PersonalityProfile == null)
				{
					Debug.LogWarning(string.Format("[NPCDialogueHolder] {0}: No PersonalityProfile found for personality {1}", base.gameObject.name, this.Personality));
				}
			}
		}

		// Token: 0x060094B2 RID: 38066 RVA: 0x003D1B2B File Offset: 0x003CFD2B
		public NPCReactionProfileSO GetReactionProfile()
		{
			if (this.PersonalityProfile != null && this.PersonalityProfile.reactionProfile != null)
			{
				return this.PersonalityProfile.reactionProfile;
			}
			NPCSenses component = base.GetComponent<NPCSenses>();
			if (component == null)
			{
				return null;
			}
			return component.ReactionProfile;
		}

		// Token: 0x060094B3 RID: 38067 RVA: 0x003D1B6B File Offset: 0x003CFD6B
		public int GetExpressionForReaction(ReactionType reactionType)
		{
			if (this.PersonalityProfile != null)
			{
				return this.PersonalityProfile.GetExpressionForReaction(reactionType);
			}
			return 0;
		}

		// Token: 0x060094B4 RID: 38068 RVA: 0x003D1B89 File Offset: 0x003CFD89
		public void SetPersonality(PersonalityType personality, bool reassignProfile = true)
		{
			this.Personality = personality;
			if (reassignProfile)
			{
				this.AssignProfileFromPersonality();
			}
		}

		// Token: 0x040099AD RID: 39341
		[Header("Profiles")]
		[Tooltip("The personality profile for this NPC (reactions + facial expressions). Dialogue lines no longer live here: they are resolved at runtime by DialogueResolver from the dialogue library.")]
		public NPCPersonalityProfileSO PersonalityProfile;

		// Token: 0x040099AE RID: 39342
		[Header("Settings")]
		[Tooltip("If true, will auto-assign profile based on personality when initialized")]
		[SerializeField]
		private bool autoAssignFromPersonality = true;

		// Token: 0x040099AF RID: 39343
		[Header("Character Info (Runtime)")]
		[Tooltip("Full name of the NPC")]
		public string FullName;

		// Token: 0x040099B0 RID: 39344
		[Tooltip("First name")]
		public string FirstName;

		// Token: 0x040099B1 RID: 39345
		[Tooltip("Surname")]
		public string Surname;
	}
}
