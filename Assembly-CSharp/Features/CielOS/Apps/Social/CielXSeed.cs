using System;
using System.Collections.Generic;
using Features.OxGram;
using UnityEngine;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001459 RID: 5209
	public static class CielXSeed
	{
		// Token: 0x0600A0BC RID: 41148 RVA: 0x004286A0 File Offset: 0x004268A0
		public static List<CielXPost> BuildFeed()
		{
			return new List<CielXPost>
			{
				CielXSeed.Make(CielXSeed.Net, "Welcome to People. Share what's happening, follow who you like, and watch the numbers climb.", 12L, 4203, 880, 311),
				CielXSeed.Make(CielXSeed.NightCity, "the city never really sleeps. who else is up", 47L, 188, 12, 41),
				CielXSeed.Make(CielXSeed.Lurker, "scrolling at 3am again. this app is dangerous", 96L, 521, 33, 77),
				CielXSeed.Make(CielXSeed.Mods, "your daily reminder that the timeline waits for no one. post something.", 150L, 942, 210, 64),
				CielXSeed.Make(CielXSeed.Weather, "Clear skies tonight, 16C. Good night for a walk.", 220L, 77, 4, 9),
				CielXSeed.Make(CielXSeed.NightCity, "someone in this city is way too confident lately and honestly? respect.", 310L, 663, 58, 120)
			};
		}

		// Token: 0x0600A0BD RID: 41149 RVA: 0x00428786 File Offset: 0x00426986
		private static CielXPost Make(CielXAccount a, string body, long age, int likes, int reposts, int replies)
		{
			CielXPost cielXPost = CielXPost.Text(a, body, age);
			cielXPost.likes = likes;
			cielXPost.reposts = reposts;
			cielXPost.replyCount = replies;
			return cielXPost;
		}

		// Token: 0x0600A0BE RID: 41150 RVA: 0x004287A8 File Offset: 0x004269A8
		public static CielXSeed.Trend[] Trending()
		{
			return new CielXSeed.Trend[]
			{
				new CielXSeed.Trend
				{
					topic = "Trending in your city",
					tag = "#WhoIsShe",
					posts = "12.4K posts"
				},
				new CielXSeed.Trend
				{
					topic = "Trending",
					tag = "#CaughtInPublic",
					posts = "8,921 posts"
				},
				new CielXSeed.Trend
				{
					topic = "Nightlife",
					tag = "#AfterDarkCity",
					posts = "6,503 posts"
				},
				new CielXSeed.Trend
				{
					topic = "Trending",
					tag = "#NoFaceNoCase",
					posts = "5,210 posts"
				},
				new CielXSeed.Trend
				{
					topic = "Trending in your city",
					tag = "#StreetExhibit",
					posts = "4,087 posts"
				},
				new CielXSeed.Trend
				{
					topic = "Trending",
					tag = "#AnonAfterMidnight",
					posts = "3,344 posts"
				}
			};
		}

		// Token: 0x0600A0BF RID: 41151 RVA: 0x004288F4 File Offset: 0x00426AF4
		public static List<CielXReply> RepliesFor(string tag, int count, Func<int, int, int> rng, int maxLikes = 30, bool faceVisible = false)
		{
			if (!string.IsNullOrEmpty(tag) && !OxGramPhotoTag.IsKnown(tag))
			{
				return new List<CielXReply>();
			}
			bool flag = string.IsNullOrEmpty(tag) || (!faceVisible && !CielXSeed.Cores.ContainsKey(tag));
			string[] array = (flag ? null : (faceVisible ? CielXSeed.RecognitionCores : CielXSeed.Cores[tag]));
			int num = Mathf.Clamp(count, 0, 30);
			int num2 = Mathf.Max(0, maxLikes);
			List<CielXReply> list = new List<CielXReply>();
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			int num3 = 0;
			while (list.Count < num && num3++ < num * 40)
			{
				string text2;
				string text3;
				string text4;
				bool flag2;
				string text = (flag ? CielXSeed.BuildTextComment(rng, out text2, out text3, out text4, out flag2) : CielXSeed.BuildTagComment(array, rng, out text2, out text3, out text4, out flag2));
				if (!string.IsNullOrEmpty(text) && !hashSet.Contains(text))
				{
					hashSet.Add(text);
					string text5 = CielXSeed.BuildHandle(rng);
					if (hashSet2.Contains(text5))
					{
						text5 = CielXSeed.BuildHandle(rng);
					}
					hashSet2.Add(text5);
					list.Add(new CielXReply
					{
						handle = text5,
						displayName = text5.TrimStart('@'),
						text = text,
						sourceOpener = text2,
						sourceCore = text3,
						sourceCloser = text4,
						capitalizeSource = flag2,
						likes = ((num2 <= 0) ? 0 : rng(0, num2))
					});
				}
			}
			return list;
		}

		// Token: 0x0600A0C0 RID: 41152 RVA: 0x00428A70 File Offset: 0x00426C70
		private static string BuildTagComment(string[] cores, Func<int, int, int> rng, out string opener, out string sourceCore, out string closer, out bool capitalize)
		{
			string text = CielXSeed.Openers[rng(0, CielXSeed.Openers.Length - 1)];
			string text2 = cores[rng(0, cores.Length - 1)];
			string text3 = CielXSeed.Closers[rng(0, CielXSeed.Closers.Length - 1)];
			opener = text;
			sourceCore = text2;
			closer = text3;
			capitalize = true;
			return CielXSeed.Cap(string.IsNullOrEmpty(text) ? text2 : (text + " " + text2)) + text3;
		}

		// Token: 0x0600A0C1 RID: 41153 RVA: 0x00428AEC File Offset: 0x00426CEC
		private static string BuildTextComment(Func<int, int, int> rng, out string opener, out string sourceCore, out string closer, out bool capitalize)
		{
			string text = CielXSeed.TextReactions[rng(0, CielXSeed.TextReactions.Length - 1)];
			opener = string.Empty;
			sourceCore = text;
			closer = string.Empty;
			capitalize = false;
			if (rng(0, 9) < 4)
			{
				string text2 = CielXSeed.Closers[rng(0, CielXSeed.Closers.Length - 1)];
				closer = text2;
				capitalize = true;
				return CielXSeed.Cap(text) + text2;
			}
			return text;
		}

		// Token: 0x0600A0C2 RID: 41154 RVA: 0x00428B60 File Offset: 0x00426D60
		public static bool TryResolveReplySource(string text, out string opener, out string core, out string closer, out bool capitalize)
		{
			string text2;
			closer = (text2 = null);
			core = (text2 = text2);
			opener = text2;
			capitalize = false;
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			foreach (string text3 in CielXSeed.TextReactions)
			{
				if (string.Equals(text, text3, StringComparison.Ordinal))
				{
					closer = (text2 = string.Empty);
					opener = text2;
					core = text3;
					return true;
				}
				string text4 = CielXSeed.Cap(text3);
				if (text.StartsWith(text4, StringComparison.Ordinal))
				{
					foreach (string text5 in CielXSeed.Closers)
					{
						if (CielXSeed.MatchesEnding(text, text4.Length, text5))
						{
							opener = string.Empty;
							core = text3;
							closer = text5;
							capitalize = true;
							return true;
						}
					}
				}
			}
			foreach (string[] array in CielXSeed.RecoverablePhotoCores())
			{
				foreach (string text6 in array)
				{
					foreach (string text7 in CielXSeed.Openers)
					{
						string text8 = CielXSeed.Cap(string.IsNullOrEmpty(text7) ? text6 : (text7 + " " + text6));
						if (text.StartsWith(text8, StringComparison.Ordinal))
						{
							foreach (string text9 in CielXSeed.Closers)
							{
								if (CielXSeed.MatchesEnding(text, text8.Length, text9))
								{
									opener = text7;
									core = text6;
									closer = text9;
									capitalize = true;
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A0C3 RID: 41155 RVA: 0x00428D14 File Offset: 0x00426F14
		private static IEnumerable<string[]> RecoverablePhotoCores()
		{
			foreach (string[] array in CielXSeed.Cores.Values)
			{
				yield return array;
			}
			Dictionary<string, string[]>.ValueCollection.Enumerator enumerator = default(Dictionary<string, string[]>.ValueCollection.Enumerator);
			yield return CielXSeed.RecognitionCores;
			yield return CielXSeed.LegacyFaceCores;
			yield break;
			yield break;
		}

		// Token: 0x0600A0C4 RID: 41156 RVA: 0x00428D20 File Offset: 0x00426F20
		internal static bool ReconcileProtectedReplies(CielXPost post)
		{
			if (post == null || !post.HasPhoto || post.faceVisible || post.replies == null)
			{
				return false;
			}
			bool flag = false;
			foreach (CielXReply cielXReply in post.replies)
			{
				if (cielXReply != null)
				{
					string sourceOpener = cielXReply.sourceOpener;
					string text = cielXReply.sourceCore;
					string sourceCloser = cielXReply.sourceCloser;
					bool capitalizeSource = cielXReply.capitalizeSource;
					if (!string.IsNullOrEmpty(text) || CielXSeed.TryResolveReplySource(cielXReply.text, out sourceOpener, out text, out sourceCloser, out capitalizeSource))
					{
						int num = Array.IndexOf<string>(CielXSeed.LegacyFaceCores, text);
						if (num < 0)
						{
							num = Array.IndexOf<string>(CielXSeed.RecognitionCores, text);
						}
						if (num >= 0)
						{
							string text2 = (string.IsNullOrEmpty(sourceOpener) ? text : (sourceOpener + " " + text));
							if (string.Equals(cielXReply.text, (capitalizeSource ? CielXSeed.Cap(text2) : text2) + sourceCloser, StringComparison.Ordinal))
							{
								text = CielXSeed.AnonymousFaceCores[num % CielXSeed.AnonymousFaceCores.Length];
								string text3 = (string.IsNullOrEmpty(sourceOpener) ? text : (sourceOpener + " " + text));
								cielXReply.text = (capitalizeSource ? CielXSeed.Cap(text3) : text3) + sourceCloser;
								cielXReply.sourceOpener = sourceOpener;
								cielXReply.sourceCore = text;
								cielXReply.sourceCloser = sourceCloser;
								cielXReply.capitalizeSource = capitalizeSource;
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600A0C5 RID: 41157 RVA: 0x00428EB8 File Offset: 0x004270B8
		private static bool MatchesEnding(string text, int start, string ending)
		{
			return text.Length - start == ending.Length && string.CompareOrdinal(text, start, ending, 0, ending.Length) == 0;
		}

		// Token: 0x0600A0C6 RID: 41158 RVA: 0x00428EE0 File Offset: 0x004270E0
		private static string BuildHandle(Func<int, int, int> rng)
		{
			string text = CielXSeed.HandleWords[rng(0, CielXSeed.HandleWords.Length - 1)];
			string text2 = CielXSeed.HandleSuffix[rng(0, CielXSeed.HandleSuffix.Length - 1)];
			if (string.IsNullOrEmpty(text2) && rng(0, 1) == 0)
			{
				text2 = rng(2, 99).ToString();
			}
			return "@" + text + text2;
		}

		// Token: 0x0600A0C7 RID: 41159 RVA: 0x00428F4C File Offset: 0x0042714C
		private static string Cap(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			char c = s[0];
			if (c >= 'a' && c <= 'z')
			{
				return char.ToUpperInvariant(c).ToString() + s.Substring(1);
			}
			return s;
		}

		// Token: 0x0600A0C8 RID: 41160 RVA: 0x00428F94 File Offset: 0x00427194
		// Note: this type is marked as 'beforefieldinit'.
		static CielXSeed()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary["tits"] = new string[]
			{
				"this is unfair", "i had to put my phone down", "best post on my feed today", "the way these look", "no notes, perfect", "saving this for research", "you broke the algorithm", "instantly my favorite account", "the confidence is everything", "how am i supposed to focus now",
				"absolutely stunning", "this is art", "the lighting is doing numbers", "i'm not okay after this", "blessed timeline"
			};
			dictionary["ass"] = new string[]
			{
				"absolutely criminal", "this should be illegal", "the confidence is unreal", "reposting before it gets taken down", "how is this allowed", "you win the timeline today", "the gym is paying off", "i forgot what i was doing", "framed like a masterpiece", "ok that's just showing off",
				"the squats are squatting", "respectfully, wow", "no thoughts left", "i'm on the floor", "10/10 no flaws"
			};
			dictionary["pussy"] = new string[]
			{
				"wait WHAT", "the app wasn't ready for this", "i peaked too hard reading this feed", "bold, very bold", "followers about to explode", "i can't believe you posted this", "the audacity (positive)", "this is a lot and i'm here for it", "going straight to the top", "absolutely fearless",
				"you have no shame and i respect it", "screenshotting before mods see", "the whole city is gonna see this", "unreal", "this changed me"
			};
			dictionary["face"] = CielXSeed.AnonymousFaceCores;
			dictionary["fullbody"] = new string[]
			{
				"the whole vibe is immaculate", "outfit check passed", "main character energy", "this is genuinely art", "the framing is perfect", "model behavior", "head to toe perfect", "the fit is fire", "you carry every photo", "serving looks",
				"could be a magazine cover", "the proportions are insane", "effortless", "every post is a 10", "the camera loves you"
			};
			CielXSeed.Cores = dictionary;
			CielXSeed.TextReactions = new string[]
			{
				":)", ":D", "<3", "xD", ";)", ":o", "^_^", ">.<", ":')", "o7",
				"lol", "lmao", "real", "facts", "so true", "needed this", "couldn't agree more", "this hit different", "based", "well said",
				"exactly", "preach", "100%", "no notes", "subbed, sub back?", "followed! follow back?", "following, return the favor?", "liked + followed, do the same?", "f4f?", "new here, follow back?",
				"support back?", "great post! checking your page", "instant follow", "you just earned a follower", "first!", "early gang", "notifications on for you", "underrated account fr"
			};
		}

		// Token: 0x0400A47B RID: 42107
		private static readonly CielXAccount Net = new CielXAccount("@people", "People", true, false);

		// Token: 0x0400A47C RID: 42108
		private static readonly CielXAccount NightCity = new CielXAccount("@nightcity_after", "night city", false, false);

		// Token: 0x0400A47D RID: 42109
		private static readonly CielXAccount Lurker = new CielXAccount("@just_a_lurker", "lurker", false, false);

		// Token: 0x0400A47E RID: 42110
		private static readonly CielXAccount Mods = new CielXAccount("@daily_feed", "daily feed", false, false);

		// Token: 0x0400A47F RID: 42111
		private static readonly CielXAccount Weather = new CielXAccount("@city_weather", "City Weather", true, false);

		// Token: 0x0400A480 RID: 42112
		private static readonly string[] HandleWords = new string[]
		{
			"alex", "mike", "jay", "sam", "chris", "nick", "leo", "max", "dan", "ryan",
			"tom", "kev", "luca", "marco", "theo", "noah", "kai", "finn", "zane", "ace",
			"ghost", "shadow", "night", "pixel", "lurker", "wolf", "raven", "storm", "echo", "void",
			"neo", "rex", "blaze", "drift", "lonely", "quiet", "city", "urban", "midnight", "after_dark",
			"no_filter", "just_me"
		};

		// Token: 0x0400A481 RID: 42113
		private static readonly string[] HandleSuffix = new string[]
		{
			"", "", "", "_", "_99", "_22", "_x", "_77", "01", "_official",
			"tv", "_ttv", "_irl", "_real", "x", "_yt", "88", "_dm", "2k", "_uk",
			"_jp", "_eu", "z"
		};

		// Token: 0x0400A482 RID: 42114
		private static readonly string[] Openers = new string[]
		{
			"ok", "bro", "honestly", "no joke", "wait", "stop", "nah", "listen", "real talk", "i mean",
			"ngl", "ok but", "yo", "ma'am", "miss", "hello??", "excuse me", ""
		};

		// Token: 0x0400A483 RID: 42115
		private static readonly string[] Closers = new string[]
		{
			"", "", "", " :)", " <3", " ;)", " xD", " :o", " 0_0", " >.<",
			" ...", "!!!", " fr", " no cap", " i'm done", " goodbye", " send help", " touch grass (jk)", " instant follow", " liked + saved",
			" reposting this"
		};

		// Token: 0x0400A484 RID: 42116
		private static readonly string[] AnonymousFaceCores = new string[] { "the mystery stays intact", "anonymous and still making the timeline interesting", "the framing works even with the face blurred", "no face, just the mood", "keeping the identity private, fair enough", "the blur is doing its job", "the lighting sets the mood", "still wondering who's behind this account" };

		// Token: 0x0400A485 RID: 42117
		private static readonly string[] RecognitionCores = new string[] { "i've seen her around town", "hey, she goes to my university!", "i think we crossed paths on the street", "that's the person i saw near campus", "i recognize her from the neighborhood", "pretty sure i've seen her at the bus stop", "we've walked past each other in town", "small world, i recognize her" };

		// Token: 0x0400A486 RID: 42118
		private static readonly string[] LegacyFaceCores = new string[]
		{
			"you're stunning", "those eyes", "the prettiest on here", "i'm in love a little", "model material", "the smile got me", "instant crush", "absolutely gorgeous", "you light up the feed", "best face on the app",
			"marry me (respectfully)", "the glow is unreal", "this is wallpaper material", "perfection", "how is this real"
		};

		// Token: 0x0400A487 RID: 42119
		private static readonly Dictionary<string, string[]> Cores;

		// Token: 0x0400A488 RID: 42120
		private static readonly string[] TextReactions;

		// Token: 0x0200145A RID: 5210
		public struct Trend
		{
			// Token: 0x0400A489 RID: 42121
			public string topic;

			// Token: 0x0400A48A RID: 42122
			public string tag;

			// Token: 0x0400A48B RID: 42123
			public string posts;
		}
	}
}
