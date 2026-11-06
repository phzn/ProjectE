using System;
using System.Runtime.CompilerServices;
using Features.OxGram;
using UnityEngine;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001450 RID: 5200
	public static class CielXFollowerModel
	{
		// Token: 0x0600A09C RID: 41116 RVA: 0x004279E4 File Offset: 0x00425BE4
		public static float LikeRateFor(string photoName)
		{
			if (string.IsNullOrEmpty(photoName))
			{
				return 0.4f;
			}
			string text = OxGramPhotoTag.FromName(photoName);
			if (text == "pussy")
			{
				return 14f;
			}
			if (text == "tits")
			{
				return 9f;
			}
			if (text == "ass")
			{
				return 6f;
			}
			if (text == "face")
			{
				return 3.2f;
			}
			if (!(text == "fullbody"))
			{
				return 0.02f;
			}
			return 2.4f;
		}

		// Token: 0x0600A09D RID: 41117 RVA: 0x00427A6C File Offset: 0x00425C6C
		public static int LikesOverTime(int baseLikes, float ratePerMinute, double minutes)
		{
			if (minutes <= 0.0 || ratePerMinute <= 0f)
			{
				return baseLikes;
			}
			double num = Math.Min(minutes, 60.0);
			if (minutes > 60.0)
			{
				num += 60.0 * Math.Sqrt((minutes - 60.0) / 60.0);
			}
			return baseLikes + (int)((double)ratePerMinute * num);
		}

		// Token: 0x0600A09E RID: 41118 RVA: 0x00427ADC File Offset: 0x00425CDC
		public static int RepostsOverTime(int baseReposts, int likesNow, int baseLikes)
		{
			int num = Math.Max(0, likesNow - baseLikes);
			return baseReposts + (int)((float)num * 0.14f);
		}

		// Token: 0x0600A09F RID: 41119 RVA: 0x00427B00 File Offset: 0x00425D00
		public static int RepliesOverTime(int baseReplies, int likesNow, int baseLikes)
		{
			int num = Math.Max(0, likesNow - baseLikes);
			return baseReplies + (int)((float)num * 0.06f);
		}

		// Token: 0x0600A0A0 RID: 41120 RVA: 0x00427B24 File Offset: 0x00425D24
		[return: TupleElementNames(new string[] { "min", "max" })]
		private static ValueTuple<int, int> FollowerRangeFor(string tag, bool textOnly)
		{
			if (textOnly)
			{
				return new ValueTuple<int, int>(3, 25);
			}
			if (tag == "pussy")
			{
				return new ValueTuple<int, int>(220, 900);
			}
			if (tag == "tits")
			{
				return new ValueTuple<int, int>(140, 560);
			}
			if (tag == "ass")
			{
				return new ValueTuple<int, int>(90, 380);
			}
			if (tag == "face")
			{
				return new ValueTuple<int, int>(60, 240);
			}
			if (!(tag == "fullbody"))
			{
				return new ValueTuple<int, int>(30, 160);
			}
			return new ValueTuple<int, int>(40, 200);
		}

		// Token: 0x0600A0A1 RID: 41121 RVA: 0x00427BD4 File Offset: 0x00425DD4
		private static int Next(int minInclusive, int maxInclusive)
		{
			if (!CielXFollowerModel._rngSeeded)
			{
				CielXFollowerModel._rng = (int)(Time.realtimeSinceStartup * 1000f) | 1;
				CielXFollowerModel._rngSeeded = true;
			}
			if (maxInclusive <= minInclusive)
			{
				return minInclusive;
			}
			uint num = (uint)CielXFollowerModel._rng;
			num ^= num << 13;
			num ^= num >> 17;
			num ^= num << 5;
			CielXFollowerModel._rng = (int)num;
			int num2 = maxInclusive - minInclusive + 1;
			return minInclusive + (int)(num % (uint)num2);
		}

		// Token: 0x0600A0A2 RID: 41122 RVA: 0x00427C34 File Offset: 0x00425E34
		public static CielXFollowerModel.Reward RewardFor(string photoName, int followerBase)
		{
			bool flag = string.IsNullOrEmpty(photoName);
			string text = (flag ? "fullbody" : OxGramPhotoTag.FromName(photoName));
			if (!flag && !OxGramPhotoTag.IsKnown(text))
			{
				return new CielXFollowerModel.Reward
				{
					likes = CielXFollowerModel.Next(0, 3),
					likeRatePerMinute = CielXFollowerModel.LikeRateFor(photoName)
				};
			}
			ValueTuple<int, int> valueTuple = CielXFollowerModel.FollowerRangeFor(text, flag);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			int num = CielXFollowerModel.Next(item, item2);
			if (!flag && OxGramPhotoTag.ShowsFace(photoName))
			{
				num += CielXFollowerModel.Next(20, 80);
			}
			num += Mathf.RoundToInt((float)(followerBase * CielXFollowerModel.Next(0, 15)) / 1000f);
			int num2;
			if (flag)
			{
				num2 = CielXFollowerModel.Next(0, 8);
			}
			else if (!(text == "pussy"))
			{
				if (!(text == "tits"))
				{
					if (!(text == "ass"))
					{
						if (!(text == "face"))
						{
							num2 = CielXFollowerModel.Next(10, 50);
						}
						else
						{
							num2 = CielXFollowerModel.Next(15, 70);
						}
					}
					else
					{
						num2 = CielXFollowerModel.Next(25, 120);
					}
				}
				else
				{
					num2 = CielXFollowerModel.Next(40, 170);
				}
			}
			else
			{
				num2 = CielXFollowerModel.Next(60, 260);
			}
			int num3 = num2;
			int num4 = Mathf.Max(0, Mathf.RoundToInt((float)num3 * ((float)CielXFollowerModel.Next(8, 18) / 100f)));
			int num5 = Mathf.Max(0, Mathf.RoundToInt((float)num3 * ((float)CielXFollowerModel.Next(5, 12) / 100f)));
			float num6 = CielXFollowerModel.LikeRateFor(photoName) * ((float)CielXFollowerModel.Next(70, 160) / 100f);
			return new CielXFollowerModel.Reward
			{
				followers = num,
				likes = num3,
				reposts = num4,
				replies = num5,
				likeRatePerMinute = num6
			};
		}

		// Token: 0x0600A0A3 RID: 41123 RVA: 0x00427DFC File Offset: 0x00425FFC
		public static string Compact(int n)
		{
			if (n < 1000)
			{
				return n.ToString();
			}
			if (n < 1000000)
			{
				float num = (float)n / 1000f;
				return ((num < 10f) ? num.ToString("0.0") : num.ToString("0")) + "K";
			}
			float num2 = (float)n / 1000000f;
			return ((num2 < 10f) ? num2.ToString("0.0") : num2.ToString("0")) + "M";
		}

		// Token: 0x0400A44D RID: 42061
		public const int SceneryLikeLimit = 5;

		// Token: 0x0400A44E RID: 42062
		private static int _rng;

		// Token: 0x0400A44F RID: 42063
		private static bool _rngSeeded;

		// Token: 0x02001451 RID: 5201
		public struct Reward
		{
			// Token: 0x0400A450 RID: 42064
			public int followers;

			// Token: 0x0400A451 RID: 42065
			public int likes;

			// Token: 0x0400A452 RID: 42066
			public int reposts;

			// Token: 0x0400A453 RID: 42067
			public int replies;

			// Token: 0x0400A454 RID: 42068
			public float likeRatePerMinute;
		}
	}
}
