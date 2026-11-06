using System;
using System.Globalization;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200083F RID: 2111
	[Serializable]
	public class CharacterGeneralInfo
	{
		// Token: 0x060037F6 RID: 14326 RVA: 0x001475B8 File Offset: 0x001457B8
		public CharacterGeneralInfo()
		{
			this.characterName = "";
			this.surname = "";
			this.birthday = DateTime.Today.AddYears(-21).ToString("yyyyMMdd");
			this.attitude = "";
			this.description = "";
			this.gender = Gender.Male;
			this.personality = PersonalityType.Neutral;
			this.occupation = Occupation.Citizen;
			this.bio = "";
			this.dialogueOverrideId = "";
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x00147644 File Offset: 0x00145844
		public CharacterGeneralInfo(CharacterGeneralInfo original)
		{
			this.characterName = original.characterName;
			this.surname = original.surname;
			this.birthday = original.birthday;
			this.attitude = original.attitude;
			this.description = original.description;
			this.gender = original.gender;
			this.personality = original.personality;
			this.occupation = original.occupation;
			this.bio = original.bio;
			this.dialogueOverrideId = original.dialogueOverrideId;
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x001476D0 File Offset: 0x001458D0
		public DateTime GetBirthdayAsDateTime()
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(this.birthday, "yyyyMMdd", null, DateTimeStyles.None, out dateTime))
			{
				return dateTime;
			}
			return DateTime.Today.AddYears(-21);
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x00147704 File Offset: 0x00145904
		public void SetBirthdayFromDateTime(DateTime date)
		{
			this.birthday = date.ToString("yyyyMMdd");
		}

		// Token: 0x040038DB RID: 14555
		public string characterName;

		// Token: 0x040038DC RID: 14556
		public string surname;

		// Token: 0x040038DD RID: 14557
		public string birthday;

		// Token: 0x040038DE RID: 14558
		public string attitude;

		// Token: 0x040038DF RID: 14559
		public string description;

		// Token: 0x040038E0 RID: 14560
		public Gender gender;

		// Token: 0x040038E1 RID: 14561
		public PersonalityType personality;

		// Token: 0x040038E2 RID: 14562
		public Occupation occupation;

		// Token: 0x040038E3 RID: 14563
		public string bio;

		// Token: 0x040038E4 RID: 14564
		public string dialogueOverrideId;
	}
}
