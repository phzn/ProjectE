using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C2 RID: 4802
	public class NPCPersonalityManager : MonoBehaviour
	{
		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x060094CA RID: 38090 RVA: 0x003D1EA9 File Offset: 0x003D00A9
		// (set) Token: 0x060094CB RID: 38091 RVA: 0x003D1EB0 File Offset: 0x003D00B0
		public static NPCPersonalityManager Instance { get; private set; }

		// Token: 0x060094CC RID: 38092 RVA: 0x003D1EB8 File Offset: 0x003D00B8
		private void Awake()
		{
			if (NPCPersonalityManager.Instance != null && NPCPersonalityManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			NPCPersonalityManager.Instance = this;
			this.InitializeProfileDictionary();
		}

		// Token: 0x060094CD RID: 38093 RVA: 0x003D1EEC File Offset: 0x003D00EC
		private void InitializeProfileDictionary()
		{
			this._profilesByType = new Dictionary<PersonalityType, NPCPersonalityProfileSO>();
			foreach (NPCPersonalityProfileSO npcpersonalityProfileSO in this.personalityProfiles)
			{
				if (npcpersonalityProfileSO != null && !this._profilesByType.ContainsKey(npcpersonalityProfileSO.personalityType))
				{
					this._profilesByType[npcpersonalityProfileSO.personalityType] = npcpersonalityProfileSO;
				}
			}
			if (this.defaultProfile != null && !this._profilesByType.ContainsKey(this.defaultProfile.personalityType))
			{
				this._profilesByType[this.defaultProfile.personalityType] = this.defaultProfile;
			}
		}

		// Token: 0x060094CE RID: 38094 RVA: 0x003D1FB4 File Offset: 0x003D01B4
		public NPCPersonalityProfileSO GetProfileForPersonality(PersonalityType personality)
		{
			if (this._profilesByType == null)
			{
				this.InitializeProfileDictionary();
			}
			NPCPersonalityProfileSO npcpersonalityProfileSO;
			if (this._profilesByType.TryGetValue(personality, out npcpersonalityProfileSO))
			{
				return npcpersonalityProfileSO;
			}
			return this.defaultProfile;
		}

		// Token: 0x060094CF RID: 38095 RVA: 0x003D1FE7 File Offset: 0x003D01E7
		public NPCPersonalityProfileSO GetProfileForCharacter(CharacterGeneralInfo info)
		{
			if (info == null)
			{
				return this.defaultProfile;
			}
			return this.GetProfileForPersonality(info.personality);
		}

		// Token: 0x060094D0 RID: 38096 RVA: 0x003D1FFF File Offset: 0x003D01FF
		public NPCPersonalityProfileSO GetRandomProfile()
		{
			if (this.personalityProfiles == null || this.personalityProfiles.Count == 0)
			{
				return this.defaultProfile;
			}
			return this.personalityProfiles[Random.Range(0, this.personalityProfiles.Count)];
		}

		// Token: 0x060094D1 RID: 38097 RVA: 0x003D2039 File Offset: 0x003D0239
		public List<NPCPersonalityProfileSO> GetAllProfiles()
		{
			return new List<NPCPersonalityProfileSO>(this.personalityProfiles);
		}

		// Token: 0x060094D2 RID: 38098 RVA: 0x003D2046 File Offset: 0x003D0246
		public bool HasProfileForPersonality(PersonalityType personality)
		{
			if (this._profilesByType == null)
			{
				this.InitializeProfileDictionary();
			}
			return this._profilesByType.ContainsKey(personality);
		}

		// Token: 0x040099DB RID: 39387
		[Header("Personality Profiles")]
		[Tooltip("Default profile used when no specific personality match is found")]
		[SerializeField]
		private NPCPersonalityProfileSO defaultProfile;

		// Token: 0x040099DC RID: 39388
		[Tooltip("All available personality profiles")]
		[SerializeField]
		private List<NPCPersonalityProfileSO> personalityProfiles = new List<NPCPersonalityProfileSO>();

		// Token: 0x040099DD RID: 39389
		private Dictionary<PersonalityType, NPCPersonalityProfileSO> _profilesByType;
	}
}
