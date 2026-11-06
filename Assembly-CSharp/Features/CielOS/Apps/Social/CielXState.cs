using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x0200145C RID: 5212
	public static class CielXState
	{
		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x0600A0D2 RID: 41170 RVA: 0x00429A43 File Offset: 0x00427C43
		public static bool HasAccount
		{
			get
			{
				return PlayerPrefs.GetInt("CielX.HasAccount", 0) == 1;
			}
		}

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600A0D3 RID: 41171 RVA: 0x00429A53 File Offset: 0x00427C53
		public static bool IsLoggedIn
		{
			get
			{
				return PlayerPrefs.GetInt("CielX.LoggedIn", 0) == 1;
			}
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600A0D4 RID: 41172 RVA: 0x00429A63 File Offset: 0x00427C63
		public static string Handle
		{
			get
			{
				return PlayerPrefs.GetString("CielX.Handle", "");
			}
		}

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600A0D5 RID: 41173 RVA: 0x00429A74 File Offset: 0x00427C74
		public static string DisplayHandle
		{
			get
			{
				string handle = CielXState.Handle;
				if (!string.IsNullOrEmpty(handle))
				{
					return "@" + handle;
				}
				return "@anon";
			}
		}

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x0600A0D6 RID: 41174 RVA: 0x00429AA0 File Offset: 0x00427CA0
		public static CielXAccount Me
		{
			get
			{
				return new CielXAccount(CielXState.DisplayHandle, string.IsNullOrEmpty(CielXState.Handle) ? "anon" : CielXState.Handle, false, true);
			}
		}

		// Token: 0x0600A0D7 RID: 41175 RVA: 0x00429AC8 File Offset: 0x00427CC8
		public static bool LogIn(string handle)
		{
			handle = CielXState.Sanitize(handle);
			if (string.IsNullOrEmpty(handle))
			{
				return false;
			}
			bool flag = !CielXState.HasAccount;
			PlayerPrefs.SetString("CielX.Handle", handle);
			PlayerPrefs.SetInt("CielX.HasAccount", 1);
			PlayerPrefs.SetInt("CielX.LoggedIn", 1);
			if (flag && !PlayerPrefs.HasKey("CielX.Followers"))
			{
				PlayerPrefs.SetInt("CielX.Followers", 312);
			}
			PlayerPrefs.Save();
			return true;
		}

		// Token: 0x0600A0D8 RID: 41176 RVA: 0x00429B33 File Offset: 0x00427D33
		public static void LogOut()
		{
			PlayerPrefs.SetInt("CielX.LoggedIn", 0);
			PlayerPrefs.Save();
		}

		// Token: 0x0600A0D9 RID: 41177 RVA: 0x00429B48 File Offset: 0x00427D48
		public static string Sanitize(string raw)
		{
			if (string.IsNullOrEmpty(raw))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(raw.Length);
			foreach (char c in raw)
			{
				if (c != '@')
				{
					if (c >= 'A' && c <= 'Z')
					{
						c += ' ';
					}
					if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_')
					{
						stringBuilder.Append(c);
					}
				}
			}
			if (stringBuilder.Length > 15)
			{
				stringBuilder.Length = 15;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600A0DA RID: 41178 RVA: 0x00429BD8 File Offset: 0x00427DD8
		public static int Followers
		{
			get
			{
				return PlayerPrefs.GetInt("CielX.Followers", 312);
			}
		}

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600A0DB RID: 41179 RVA: 0x00429BE9 File Offset: 0x00427DE9
		public static int TotalLikes
		{
			get
			{
				return PlayerPrefs.GetInt("CielX.TotalLikes", 0);
			}
		}

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x0600A0DC RID: 41180 RVA: 0x00429BF6 File Offset: 0x00427DF6
		public static int TotalReposts
		{
			get
			{
				return PlayerPrefs.GetInt("CielX.TotalReposts", 0);
			}
		}

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x0600A0DD RID: 41181 RVA: 0x00429C03 File Offset: 0x00427E03
		public static int TotalReplies
		{
			get
			{
				return PlayerPrefs.GetInt("CielX.TotalReplies", 0);
			}
		}

		// Token: 0x0600A0DE RID: 41182 RVA: 0x00429C10 File Offset: 0x00427E10
		public static void AddLifetime(int likes, int reposts, int replies)
		{
			if (likes != 0)
			{
				PlayerPrefs.SetInt("CielX.TotalLikes", Mathf.Max(0, CielXState.TotalLikes + likes));
			}
			if (reposts != 0)
			{
				PlayerPrefs.SetInt("CielX.TotalReposts", Mathf.Max(0, CielXState.TotalReposts + reposts));
			}
			if (replies != 0)
			{
				PlayerPrefs.SetInt("CielX.TotalReplies", Mathf.Max(0, CielXState.TotalReplies + replies));
			}
			PlayerPrefs.Save();
		}

		// Token: 0x0600A0DF RID: 41183 RVA: 0x00429C70 File Offset: 0x00427E70
		public static int AddFollowers(int delta)
		{
			int num = Mathf.Max(0, CielXState.Followers + delta);
			PlayerPrefs.SetInt("CielX.Followers", num);
			PlayerPrefs.Save();
			return num;
		}

		// Token: 0x0600A0E0 RID: 41184 RVA: 0x00429C9C File Offset: 0x00427E9C
		public static List<CielXPost> LoadMyPosts()
		{
			string @string = PlayerPrefs.GetString("CielX.MyPosts", "");
			if (string.IsNullOrEmpty(@string))
			{
				return new List<CielXPost>();
			}
			try
			{
				CielXPostList cielXPostList = JsonUtility.FromJson<CielXPostList>(@string);
				if (cielXPostList != null && cielXPostList.posts != null)
				{
					bool flag = false;
					foreach (CielXPost cielXPost in cielXPostList.posts)
					{
						if (cielXPost != null)
						{
							if (string.IsNullOrEmpty(cielXPost.id))
							{
								cielXPost.id = Guid.NewGuid().ToString("N");
								flag = true;
							}
							flag |= CielXSeed.ReconcileProtectedReplies(cielXPost);
						}
					}
					if (flag)
					{
						CielXState.SaveMyPosts(cielXPostList.posts);
					}
					return cielXPostList.posts;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielXState] Could not parse saved posts: " + ex.Message);
			}
			return new List<CielXPost>();
		}

		// Token: 0x0600A0E1 RID: 41185 RVA: 0x00429DA4 File Offset: 0x00427FA4
		public static void SaveMyPosts(List<CielXPost> posts)
		{
			CielXPostList cielXPostList = new CielXPostList
			{
				posts = (posts ?? new List<CielXPost>())
			};
			try
			{
				PlayerPrefs.SetString("CielX.MyPosts", JsonUtility.ToJson(cielXPostList));
				PlayerPrefs.Save();
			}
			catch (Exception ex)
			{
				Debug.LogError("[CielXState] Could not save posts: " + ex.Message);
			}
		}

		// Token: 0x0600A0E2 RID: 41186 RVA: 0x00429E08 File Offset: 0x00428008
		public static void PrependMyPost(CielXPost post)
		{
			List<CielXPost> list = CielXState.LoadMyPosts();
			list.Insert(0, post);
			CielXState.SaveMyPosts(list);
		}

		// Token: 0x0600A0E3 RID: 41187 RVA: 0x00429E1C File Offset: 0x0042801C
		public static bool DeleteMyPost(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return false;
			}
			List<CielXPost> list = CielXState.LoadMyPosts();
			int num = list.RemoveAll((CielXPost p) => p != null && p.id == id);
			if (num > 0)
			{
				CielXState.SaveMyPosts(list);
			}
			return num > 0;
		}

		// Token: 0x0600A0E4 RID: 41188 RVA: 0x00429E6C File Offset: 0x0042806C
		public static void ResetAll()
		{
			PlayerPrefs.DeleteKey("CielX.Handle");
			PlayerPrefs.DeleteKey("CielX.LoggedIn");
			PlayerPrefs.DeleteKey("CielX.Followers");
			PlayerPrefs.DeleteKey("CielX.MyPosts");
			PlayerPrefs.DeleteKey("CielX.HasAccount");
			PlayerPrefs.DeleteKey("CielX.TotalLikes");
			PlayerPrefs.DeleteKey("CielX.TotalReposts");
			PlayerPrefs.DeleteKey("CielX.TotalReplies");
			PlayerPrefs.Save();
		}

		// Token: 0x0400A490 RID: 42128
		private const string KeyHandle = "CielX.Handle";

		// Token: 0x0400A491 RID: 42129
		private const string KeyLoggedIn = "CielX.LoggedIn";

		// Token: 0x0400A492 RID: 42130
		private const string KeyFollowers = "CielX.Followers";

		// Token: 0x0400A493 RID: 42131
		private const string KeyPosts = "CielX.MyPosts";

		// Token: 0x0400A494 RID: 42132
		private const string KeyHasAccount = "CielX.HasAccount";

		// Token: 0x0400A495 RID: 42133
		public const int StartingFollowers = 312;

		// Token: 0x0400A496 RID: 42134
		private const string KeyTotalLikes = "CielX.TotalLikes";

		// Token: 0x0400A497 RID: 42135
		private const string KeyTotalReposts = "CielX.TotalReposts";

		// Token: 0x0400A498 RID: 42136
		private const string KeyTotalReplies = "CielX.TotalReplies";
	}
}
