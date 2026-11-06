using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200083E RID: 2110
	[Serializable]
	public class CharacterDataExtended
	{
		// Token: 0x060037F3 RID: 14323 RVA: 0x001474C8 File Offset: 0x001456C8
		public CharacterDataExtended()
		{
			this.characterId = Guid.NewGuid().ToString();
			this.createdDate = DateTime.Now;
			this.lastModified = DateTime.Now;
			this.generalInfo = new CharacterGeneralInfo();
			this.stateData = new CharacterStateData();
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x00147528 File Offset: 0x00145728
		public CharacterDataExtended(CharacterDataExtended original)
			: this()
		{
			this.characterName = original.characterName + " (Copy)";
			this.description = original.description;
			this.location = original.location;
			this.isMainCharacter = false;
			this.createdFrom = "duplicate";
			this.originalCharacterId = original.characterId;
			this.generalInfo = new CharacterGeneralInfo(original.generalInfo);
			this.stateData = new CharacterStateData(original.stateData);
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x001475A9 File Offset: 0x001457A9
		public void UpdateLastModified()
		{
			this.lastModified = DateTime.Now;
		}

		// Token: 0x040038CE RID: 14542
		[Header("Basic Info")]
		public string characterId;

		// Token: 0x040038CF RID: 14543
		public string characterName;

		// Token: 0x040038D0 RID: 14544
		public string description;

		// Token: 0x040038D1 RID: 14545
		public Sprite icon;

		// Token: 0x040038D2 RID: 14546
		[Header("General Info")]
		public CharacterGeneralInfo generalInfo;

		// Token: 0x040038D3 RID: 14547
		[Header("Character State Data")]
		public CharacterStateData stateData;

		// Token: 0x040038D4 RID: 14548
		[Header("Character Specific")]
		public string location;

		// Token: 0x040038D5 RID: 14549
		public bool isActive = true;

		// Token: 0x040038D6 RID: 14550
		public bool isMainCharacter;

		// Token: 0x040038D7 RID: 14551
		[Header("Metadata")]
		public DateTime createdDate;

		// Token: 0x040038D8 RID: 14552
		public DateTime lastModified;

		// Token: 0x040038D9 RID: 14553
		public string createdFrom;

		// Token: 0x040038DA RID: 14554
		public string originalCharacterId;
	}
}
