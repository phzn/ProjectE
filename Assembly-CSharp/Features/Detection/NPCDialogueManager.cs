using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012BD RID: 4797
	public class NPCDialogueManager : MonoBehaviour
	{
		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x060094B6 RID: 38070 RVA: 0x003D1BB5 File Offset: 0x003CFDB5
		// (set) Token: 0x060094B7 RID: 38071 RVA: 0x003D1BBC File Offset: 0x003CFDBC
		public static NPCDialogueManager Instance { get; private set; }

		// Token: 0x060094B8 RID: 38072 RVA: 0x003D1BC4 File Offset: 0x003CFDC4
		private void Awake()
		{
			if (NPCDialogueManager.Instance != null && NPCDialogueManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			NPCDialogueManager.Instance = this;
			this.InitializeProfilePools();
		}

		// Token: 0x060094B9 RID: 38073 RVA: 0x003D1BF8 File Offset: 0x003CFDF8
		private void InitializeProfilePools()
		{
			this._profilePools = new Dictionary<PersonalityType, NPCDialogueProfileSO[]>
			{
				{
					PersonalityType.Neutral,
					this.GetPoolOrSingle(this.neutralProfiles, this.neutralProfile)
				},
				{
					PersonalityType.Shy,
					this.GetPoolOrSingle(this.shyProfiles, this.shyProfile)
				},
				{
					PersonalityType.Bold,
					this.GetPoolOrSingle(this.boldProfiles, this.boldProfile)
				},
				{
					PersonalityType.Curious,
					this.GetPoolOrSingle(this.curiousProfiles, this.curiousProfile)
				},
				{
					PersonalityType.Hostile,
					this.GetPoolOrSingle(this.hostileProfiles, this.hostileProfile)
				},
				{
					PersonalityType.Friendly,
					this.GetPoolOrSingle(this.friendlyProfiles, this.friendlyProfile)
				}
			};
		}

		// Token: 0x060094BA RID: 38074 RVA: 0x003D1CA6 File Offset: 0x003CFEA6
		private NPCDialogueProfileSO[] GetPoolOrSingle(NPCDialogueProfileSO[] pool, NPCDialogueProfileSO single)
		{
			if (pool != null && pool.Length != 0)
			{
				return pool;
			}
			if (single != null)
			{
				return new NPCDialogueProfileSO[] { single };
			}
			return null;
		}

		// Token: 0x060094BB RID: 38075 RVA: 0x003D1CC8 File Offset: 0x003CFEC8
		public NPCDialogueProfileSO GetProfileForPersonality(PersonalityType personality)
		{
			if (this._profilePools == null)
			{
				this.InitializeProfilePools();
			}
			NPCDialogueProfileSO[] array;
			if (this._profilePools.TryGetValue(personality, out array) && array != null && array.Length != 0)
			{
				return array[Random.Range(0, array.Length)];
			}
			return this.defaultProfile;
		}

		// Token: 0x060094BC RID: 38076 RVA: 0x003D1D0C File Offset: 0x003CFF0C
		public NPCDialogueProfileSO GetProfileForCharacter(CharacterGeneralInfo info)
		{
			if (info == null)
			{
				return this.defaultProfile;
			}
			return this.GetProfileForPersonality(info.personality);
		}

		// Token: 0x060094BD RID: 38077 RVA: 0x003D1D24 File Offset: 0x003CFF24
		public NPCDialogueProfileSO GetRandomProfile()
		{
			Array values = Enum.GetValues(typeof(PersonalityType));
			PersonalityType personalityType = (PersonalityType)values.GetValue(Random.Range(0, values.Length));
			return this.GetProfileForPersonality(personalityType);
		}

		// Token: 0x040099B7 RID: 39351
		[Header("Dialogue Profiles by Personality")]
		[Tooltip("Default profile used when no specific personality match is found")]
		[SerializeField]
		private NPCDialogueProfileSO defaultProfile;

		// Token: 0x040099B8 RID: 39352
		[SerializeField]
		private NPCDialogueProfileSO neutralProfile;

		// Token: 0x040099B9 RID: 39353
		[SerializeField]
		private NPCDialogueProfileSO shyProfile;

		// Token: 0x040099BA RID: 39354
		[SerializeField]
		private NPCDialogueProfileSO boldProfile;

		// Token: 0x040099BB RID: 39355
		[SerializeField]
		private NPCDialogueProfileSO curiousProfile;

		// Token: 0x040099BC RID: 39356
		[SerializeField]
		private NPCDialogueProfileSO hostileProfile;

		// Token: 0x040099BD RID: 39357
		[SerializeField]
		private NPCDialogueProfileSO friendlyProfile;

		// Token: 0x040099BE RID: 39358
		[Header("Profile Pools (for random selection)")]
		[Tooltip("Multiple profiles per personality for variety")]
		[SerializeField]
		private NPCDialogueProfileSO[] neutralProfiles;

		// Token: 0x040099BF RID: 39359
		[SerializeField]
		private NPCDialogueProfileSO[] shyProfiles;

		// Token: 0x040099C0 RID: 39360
		[SerializeField]
		private NPCDialogueProfileSO[] boldProfiles;

		// Token: 0x040099C1 RID: 39361
		[SerializeField]
		private NPCDialogueProfileSO[] curiousProfiles;

		// Token: 0x040099C2 RID: 39362
		[SerializeField]
		private NPCDialogueProfileSO[] hostileProfiles;

		// Token: 0x040099C3 RID: 39363
		[SerializeField]
		private NPCDialogueProfileSO[] friendlyProfiles;

		// Token: 0x040099C4 RID: 39364
		private Dictionary<PersonalityType, NPCDialogueProfileSO[]> _profilePools;
	}
}
