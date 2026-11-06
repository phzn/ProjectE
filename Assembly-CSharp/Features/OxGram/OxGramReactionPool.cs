using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000EB0 RID: 3760
	[CreateAssetMenu(fileName = "OxGramReactionPool", menuName = "OxGram/Reaction Pool")]
	public class OxGramReactionPool : ScriptableObject
	{
		// Token: 0x060070D1 RID: 28881 RVA: 0x002DC7B8 File Offset: 0x002DA9B8
		public List<string> ExposedLines()
		{
			if (this.exposedLines != null && this.exposedLines.Count > 0)
			{
				return this.exposedLines;
			}
			return OxGramReactionPool.DefaultExposed();
		}

		// Token: 0x060070D2 RID: 28882 RVA: 0x002DC7DC File Offset: 0x002DA9DC
		public int RollExposedCount()
		{
			int num = Mathf.Max(1, this.exposedReactionCount.x);
			int num2 = Mathf.Max(num, this.exposedReactionCount.y);
			return Random.Range(num, num2 + 1);
		}

		// Token: 0x060070D3 RID: 28883 RVA: 0x002DC814 File Offset: 0x002DAA14
		public List<string> LinesFor(string tag)
		{
			if (this.byTag != null)
			{
				foreach (OxGramReactionPool.TagReactions tagReactions in this.byTag)
				{
					if (tagReactions != null && tagReactions.tag == tag && tagReactions.lines != null && tagReactions.lines.Count > 0)
					{
						return tagReactions.lines;
					}
				}
			}
			return OxGramReactionPool.DefaultFor(tag);
		}

		// Token: 0x060070D4 RID: 28884 RVA: 0x002DC8A0 File Offset: 0x002DAAA0
		public int RollCount()
		{
			int num = Mathf.Max(1, this.reactionCount.x);
			int num2 = Mathf.Max(num, this.reactionCount.y);
			return Random.Range(num, num2 + 1);
		}

		// Token: 0x060070D5 RID: 28885 RVA: 0x002DC8D8 File Offset: 0x002DAAD8
		public static string FormatReply(string line, string displayName)
		{
			string text;
			string text2;
			if (!OxGramUser.TryNormalizeDisplayName(displayName, out text, out text2))
			{
				text = "anonymous";
			}
			string text3 = "@" + text;
			if (string.IsNullOrWhiteSpace(line))
			{
				return text3 + ", I saw your photo.";
			}
			bool flag = line.Contains("{name}") || line.Contains("{username}") || line.Contains("{mention}");
			string text4 = line.Replace("{mention}", text3).Replace("{username}", text).Replace("{name}", text)
				.Trim();
			if (flag || OxGramReactionPool.ContainsMention(text4, text3))
			{
				return text4;
			}
			return text3 + ", " + text4;
		}

		// Token: 0x060070D6 RID: 28886 RVA: 0x002DC980 File Offset: 0x002DAB80
		private static bool ContainsMention(string reply, string mention)
		{
			int num2;
			for (int i = 0; i < reply.Length; i = num2)
			{
				int num = reply.IndexOf(mention, i, StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					return false;
				}
				num2 = num + mention.Length;
				if (num2 == reply.Length || (!char.IsLetterOrDigit(reply[num2]) && reply[num2] != '_'))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060070D7 RID: 28887 RVA: 0x002DC9DC File Offset: 0x002DABDC
		public static List<string> DefaultFor(string tag)
		{
			if (tag == "tits")
			{
				return new List<string> { "WHAT. are you serious right now", "did you really just post your tits in here", "oh my god she actually did it", "I was NOT ready for that", "girl this is a public chat what are you doing", "I have to put my phone down", "no way. NO WAY", "my jaw is on the floor" };
			}
			if (tag == "ass")
			{
				return new List<string> { "okay that is way too much", "did you seriously just send THAT here", "I cannot believe what I am looking at", "the absolute nerve of this person", "this is a group chat, have some shame", "I am genuinely speechless", "who hurt you, why would you post this", "screenshotting this for the record, this is insane" };
			}
			if (tag == "pussy")
			{
				return new List<string> { "WHOA okay that is illegal to post here", "absolutely not, this is too far", "I am reporting this, are you out of your mind", "did everyone else just see that?? please tell me no", "I need to bleach my eyes", "this is a whole new level of unhinged", "ma'am this is a public group", "I have so many questions and I'm scared of the answers" };
			}
			if (!(tag == "face"))
			{
				if (!(tag == "fullbody"))
				{
				}
				return new List<string> { "okay WHO does this in a group chat", "the whole thing?? just like that??", "I did not log in for this today", "absolutely shameless, I'm shaking", "everybody is seeing this, you get that?", "I don't know whether to look away or not", "this person has zero filter my god", "well. that's one way to make an entrance" };
			}
			return new List<string> { "wait who even ARE you", "you just dropped your whole face in an anonymous chat??", "bold move posting your actual face here", "do you know how anonymous this place is supposed to be", "okay but why are you showing yourself", "this is either very brave or very dumb", "everyone can see you now you know that right" };
		}

		// Token: 0x060070D8 RID: 28888 RVA: 0x002DCC08 File Offset: 0x002DAE08
		public static List<string> DefaultExposed()
		{
			return new List<string>
			{
				"WAIT the censor glitched. I can see your face", "oh my god. OH MY GOD it's actually you", "{name}?? that's {name}, I KNOW her", "everyone the blur failed, we can all see who this is", "screenshotting RIGHT NOW before you delete it", "you're {name}, you sit two rows in front of me", "the face is fully showing, you are so done", "no censor this time huh. we got you now {name}", "I'm saving this. everyone is going to know it was you", "wait I recognize you. you're DONE, this is everywhere by tomorrow",
				"the whole group just saw your face. there's no taking that back", "caught. absolutely caught. that's {name} no question"
			};
		}

		// Token: 0x0400738E RID: 29582
		[Tooltip("Quante reazioni (NPC diversi) rispondono a una foto, range min..max.")]
		public Vector2Int reactionCount = new Vector2Int(2, 3);

		// Token: 0x0400738F RID: 29583
		[Tooltip("Set di reazioni per tag.")]
		public List<OxGramReactionPool.TagReactions> byTag = new List<OxGramReactionPool.TagReactions>();

		// Token: 0x04007390 RID: 29584
		[Header("Censorship glitch — 'exposed' reactions")]
		[Tooltip("Battute usate quando la censura FALLISCE e il volto del player viene mostrato: gli NPC lo riconoscono e si scatenano. Usa {name} per inserire il nome del player. Se vuoto, usa i default builtin (DefaultExposed). Niente emoji (il font non le ha).")]
		[TextArea]
		public List<string> exposedLines = new List<string>();

		// Token: 0x04007391 RID: 29585
		[Tooltip("Quante reazioni partono quando la censura fallisce (piu' alte del normale: il gruppo si scatena). Range min..max.")]
		public Vector2Int exposedReactionCount = new Vector2Int(4, 6);

		// Token: 0x02000EB1 RID: 3761
		[Serializable]
		public class TagReactions
		{
			// Token: 0x04007392 RID: 29586
			[Tooltip("Tag della parte del corpo (face/tits/pussy/ass/fullbody). Vedi OxGramPhotoTag.")]
			public string tag = "face";

			// Token: 0x04007393 RID: 29587
			[Tooltip("Battute possibili degli NPC per questo tag. Ne vengono pescate alcune a caso allo scatto. {name} / {username} = alias, {mention} = @alias. Senza placeholder viene aggiunta una menzione. Niente emoji (il font non le ha).")]
			[TextArea]
			public List<string> lines = new List<string>();
		}
	}
}
