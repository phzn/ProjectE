using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EE6 RID: 3814
	[Serializable]
	public sealed class NPCRosterEntry
	{
		// Token: 0x06007236 RID: 29238 RVA: 0x002E74F0 File Offset: 0x002E56F0
		public CharacterObject CreateCustomCharacter()
		{
			if (!this.isCustom || string.IsNullOrEmpty(this.lookJson))
			{
				return null;
			}
			CharacterData characterData = JsonUtility.FromJson<CharacterData>(this.lookJson);
			if (!NPCRosterSession.IsValidCustomLook(characterData, this.gender))
			{
				return null;
			}
			CharacterObject characterObject = ScriptableObject.CreateInstance<CharacterObject>();
			characterObject.hideFlags = HideFlags.DontSave;
			characterObject.name = this.presetId;
			characterObject.data = characterData;
			return characterObject;
		}

		// Token: 0x0400754F RID: 30031
		public string presetId;

		// Token: 0x04007550 RID: 30032
		public string resourceName;

		// Token: 0x04007551 RID: 30033
		public string displayName;

		// Token: 0x04007552 RID: 30034
		public Gender gender;

		// Token: 0x04007553 RID: 30035
		public bool isCustom;

		// Token: 0x04007554 RID: 30036
		public string lookJson;
	}
}
