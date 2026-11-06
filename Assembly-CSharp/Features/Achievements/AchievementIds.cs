using System;
using System.Collections.Generic;

namespace Features.Achievements
{
	// Token: 0x02001534 RID: 5428
	public static class AchievementIds
	{
		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x0600A812 RID: 43026 RVA: 0x00462EC9 File Offset: 0x004610C9
		public static IReadOnlyList<string> All
		{
			get
			{
				return AchievementIds.KnownIds;
			}
		}

		// Token: 0x0600A813 RID: 43027 RVA: 0x00462ED0 File Offset: 0x004610D0
		public static bool IsLegacy(string id)
		{
			return id == "ACH_FIRST_RUN" || id == "ACH_FIRST_RANKED_RUN" || id == "ACH_FIRST_WARDROBE_FIND" || id == "ACH_CLASSIC_FULLY_UNLOCKED";
		}

		// Token: 0x0400ACC7 RID: 44231
		public const string Distance500 = "ACH_DISTANCE_500";

		// Token: 0x0400ACC8 RID: 44232
		public const string Distance2000 = "ACH_DISTANCE_2000";

		// Token: 0x0400ACC9 RID: 44233
		public const string Distance5000 = "ACH_DISTANCE_5000";

		// Token: 0x0400ACCA RID: 44234
		public const string Distance10000 = "ACH_DISTANCE_10000";

		// Token: 0x0400ACCB RID: 44235
		public const string Distance50000 = "ACH_DISTANCE_50000";

		// Token: 0x0400ACCC RID: 44236
		public const string Photos50 = "ACH_NPC_PHOTOS_50";

		// Token: 0x0400ACCD RID: 44237
		public const string Photos200 = "ACH_NPC_PHOTOS_200";

		// Token: 0x0400ACCE RID: 44238
		public const string Photos500 = "ACH_NPC_PHOTOS_500";

		// Token: 0x0400ACCF RID: 44239
		public const string Photos1000 = "ACH_NPC_PHOTOS_1000";

		// Token: 0x0400ACD0 RID: 44240
		public const string Trashcans50 = "ACH_TRASHCANS_50";

		// Token: 0x0400ACD1 RID: 44241
		public const string Trashcans200 = "ACH_TRASHCANS_200";

		// Token: 0x0400ACD2 RID: 44242
		public const string Trashcans500 = "ACH_TRASHCANS_500";

		// Token: 0x0400ACD3 RID: 44243
		public const string Trashcans1000 = "ACH_TRASHCANS_1000";

		// Token: 0x0400ACD4 RID: 44244
		public const string Explorer = "ACH_EXPLORER";

		// Token: 0x0400ACD5 RID: 44245
		public const string HumanExplorer = "ACH_HUMAN_EXPLORER";

		// Token: 0x0400ACD6 RID: 44246
		public const string Clothes20 = "ACH_CLOTHES_20";

		// Token: 0x0400ACD7 RID: 44247
		public const string Clothes50 = "ACH_CLOTHES_50";

		// Token: 0x0400ACD8 RID: 44248
		public const string AllClothes = "ACH_CLOTHES_ALL";

		// Token: 0x0400ACD9 RID: 44249
		public const string Chameleon = "ACH_CHAMELEON";

		// Token: 0x0400ACDA RID: 44250
		public const string HealthStreak7 = "ACH_HEALTH_STREAK_7";

		// Token: 0x0400ACDB RID: 44251
		public const string RiskyHobby = "ACH_RISKY_HOBBY";

		// Token: 0x0400ACDC RID: 44252
		public const string Wanted = "ACH_WANTED";

		// Token: 0x0400ACDD RID: 44253
		public const string HeyDj = "ACH_HEY_DJ";

		// Token: 0x0400ACDE RID: 44254
		public const string YouAreLive = "ACH_YOU_ARE_LIVE";

		// Token: 0x0400ACDF RID: 44255
		public const string RankCurious = "ACH_RANK_CURIOUS";

		// Token: 0x0400ACE0 RID: 44256
		public const string RankAroused = "ACH_RANK_AROUSED";

		// Token: 0x0400ACE1 RID: 44257
		public const string RankHorny = "ACH_RANK_HORNY";

		// Token: 0x0400ACE2 RID: 44258
		public const string RankHornySlut = "ACH_RANK_HORNY_SLUT";

		// Token: 0x0400ACE3 RID: 44259
		public const string RankCertified = "ACH_RANK_CERTIFIED";

		// Token: 0x0400ACE4 RID: 44260
		public const string FirstPrison = "ACH_FIRST_PRISON";

		// Token: 0x0400ACE5 RID: 44261
		public const string Home1000 = "ACH_HOME_1000";

		// Token: 0x0400ACE6 RID: 44262
		public const string Home2000 = "ACH_HOME_2000";

		// Token: 0x0400ACE7 RID: 44263
		public const string Home5000 = "ACH_HOME_5000";

		// Token: 0x0400ACE8 RID: 44264
		public const string Home10000 = "ACH_HOME_10000";

		// Token: 0x0400ACE9 RID: 44265
		public const string LongWayHome = "ACH_LONG_WAY_HOME";

		// Token: 0x0400ACEA RID: 44266
		public const string FirstRunCompleted = "ACH_FIRST_RUN";

		// Token: 0x0400ACEB RID: 44267
		public const string FirstRankedRunCompleted = "ACH_FIRST_RANKED_RUN";

		// Token: 0x0400ACEC RID: 44268
		public const string FirstWardrobeFind = "ACH_FIRST_WARDROBE_FIND";

		// Token: 0x0400ACED RID: 44269
		public const string ClassicFullyUnlocked = "ACH_CLASSIC_FULLY_UNLOCKED";

		// Token: 0x0400ACEE RID: 44270
		private static readonly string[] KnownIds = new string[]
		{
			"ACH_DISTANCE_500", "ACH_DISTANCE_2000", "ACH_DISTANCE_5000", "ACH_DISTANCE_10000", "ACH_DISTANCE_50000", "ACH_NPC_PHOTOS_50", "ACH_NPC_PHOTOS_200", "ACH_NPC_PHOTOS_500", "ACH_NPC_PHOTOS_1000", "ACH_TRASHCANS_50",
			"ACH_TRASHCANS_200", "ACH_TRASHCANS_500", "ACH_TRASHCANS_1000", "ACH_EXPLORER", "ACH_HUMAN_EXPLORER", "ACH_CLOTHES_20", "ACH_CLOTHES_50", "ACH_CLOTHES_ALL", "ACH_CHAMELEON", "ACH_HEALTH_STREAK_7",
			"ACH_RISKY_HOBBY", "ACH_WANTED", "ACH_HEY_DJ", "ACH_YOU_ARE_LIVE", "ACH_RANK_CURIOUS", "ACH_RANK_AROUSED", "ACH_RANK_HORNY", "ACH_RANK_HORNY_SLUT", "ACH_RANK_CERTIFIED", "ACH_FIRST_PRISON",
			"ACH_HOME_1000", "ACH_HOME_2000", "ACH_HOME_5000", "ACH_HOME_10000", "ACH_LONG_WAY_HOME"
		};
	}
}
