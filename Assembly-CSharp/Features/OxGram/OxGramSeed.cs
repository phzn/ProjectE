using System;
using System.Collections.Generic;

namespace Features.OxGram
{
	// Token: 0x02000EB2 RID: 3762
	public static class OxGramSeed
	{
		// Token: 0x060070DB RID: 28891 RVA: 0x002DCCF4 File Offset: 0x002DAEF4
		public static List<OxGramChat> BuildChats()
		{
			return new List<OxGramChat>
			{
				OxGramSeed.BuildCompSciGroup(),
				OxGramSeed.BuildTokyoGroup(),
				OxGramSeed.BuildNightOwlsGroup()
			};
		}

		// Token: 0x060070DC RID: 28892 RVA: 0x002DCD1C File Offset: 0x002DAF1C
		private static OxGramChat BuildCompSciGroup()
		{
			return new OxGramChat
			{
				id = "group:oxpath-cs",
				title = "Oxpath CS — Year 2",
				isGroup = true,
				subtitle = "Emma, Liam, Sofia, Noah +12",
				members = new List<string> { "Emma", "Liam", "Sofia", "Noah", "Priya", "Marco" },
				ambientLines = new List<string>
				{
					"did the compiler assignment even compile for anyone", "lab moved to room 204, the linux machines are down again", "who actually understood today's lecture on pointers", "segfault. again. i give up", "anyone got the slides for data structures?", "the prof's code on the board didn't even run lol", "study group in the library at 6?", "is the deadline midnight or 9am??", "stack overflow is my real professor at this point", "rewrote it in rust, still broken",
					"does recursion just... never click for anyone else", "free pizza at the dev society meetup tn"
				},
				messages = new List<OxGramMessage>
				{
					OxGramMessage.Text(OxGramSender.Other, "Emma", "morning, did anyone finish lab 3?", "08:41"),
					OxGramMessage.Text(OxGramSender.Other, "Liam", "define 'finish' lol", "08:43"),
					OxGramMessage.Text(OxGramSender.Other, "Sofia", "mine compiles but the output is cursed", "08:44"),
					OxGramMessage.Text(OxGramSender.Other, "Noah", "same. off by one somewhere i bet", "08:45"),
					OxGramMessage.Text(OxGramSender.Other, "Priya", "it's always off by one", "08:46")
				}
			};
		}

		// Token: 0x060070DD RID: 28893 RVA: 0x002DCEC4 File Offset: 0x002DB0C4
		private static OxGramChat BuildTokyoGroup()
		{
			return new OxGramChat
			{
				id = "group:tokyo",
				title = "Tokyo",
				isGroup = true,
				subtitle = "2,418 members, 312 online",
				members = new List<string> { "Kenji", "Yuki", "Haru", "Aoi", "Ren", "Mei", "Sora", "Riku" },
				ambientLines = new List<string>
				{
					"anyone know a good ramen spot near Shibuya?", "the trains are insane today omg", "selling 2 tickets for the show tonight DM", "is it raining in Shinjuku rn", "best konbini snack? go", "the cherry blossoms in Ueno are unreal right now", "anyone up for karaoke this weekend", "lost my umbrella again. third one this month", "new cafe opened in Nakameguro, looks cute", "traffic on the loop line is brutal",
					"where do you all go to study late at night", "saw a street performer in Harajuku, incredible"
				},
				messages = new List<OxGramMessage>
				{
					OxGramMessage.Text(OxGramSender.Other, "Kenji", "morning Tokyo", "07:58"),
					OxGramMessage.Text(OxGramSender.Other, "Yuki", "is the Yamanote line delayed again??", "08:02"),
					OxGramMessage.Text(OxGramSender.Other, "Haru", "yeah avoid it, take the metro", "08:03"),
					OxGramMessage.Text(OxGramSender.Other, "Aoi", "anyone else stuck at Shinjuku rn", "08:05")
				}
			};
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x002DD068 File Offset: 0x002DB268
		private static OxGramChat BuildNightOwlsGroup()
		{
			return new OxGramChat
			{
				id = "group:night-owls",
				title = "Night Owls",
				isGroup = true,
				subtitle = "846 members, anonymous",
				members = new List<string> { "owl_77", "midnight", "ghost", "nova", "echo", "static" },
				ambientLines = new List<string> { "can't sleep again", "anyone else awake at this hour", "3am thoughts hit different", "what's everyone listening to tonight", "the city is so quiet right now", "coffee at midnight was a mistake", "tell me something interesting", "who else has work in 5 hours and is still here" },
				messages = new List<OxGramMessage>
				{
					OxGramMessage.Text(OxGramSender.Other, "midnight", "anyone up?", "02:14"),
					OxGramMessage.Text(OxGramSender.Other, "ghost", "always", "02:16"),
					OxGramMessage.Text(OxGramSender.Other, "nova", "couldn't sleep so here i am", "02:19")
				}
			};
		}
	}
}
