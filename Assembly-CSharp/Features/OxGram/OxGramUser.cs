using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000EB7 RID: 3767
	public static class OxGramUser
	{
		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x060070EF RID: 28911 RVA: 0x002DDB73 File Offset: 0x002DBD73
		public static bool HasUsername
		{
			get
			{
				return OxGramUser.Username.Length > 0;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x060070F0 RID: 28912 RVA: 0x002DDB84 File Offset: 0x002DBD84
		// (set) Token: 0x060070F1 RID: 28913 RVA: 0x002DDBB4 File Offset: 0x002DBDB4
		public static string Username
		{
			get
			{
				string text;
				string text2;
				if (!OxGramUser.TryNormalizeDisplayName(PlayerPrefs.GetString("OxGram.Username", ""), out text, out text2))
				{
					return "";
				}
				return text;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					OxGramUser.Clear();
					return;
				}
				string text;
				OxGramUser.TrySetDisplayName(value, out text);
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x060070F2 RID: 28914 RVA: 0x002DDBD8 File Offset: 0x002DBDD8
		public static string DisplayName
		{
			get
			{
				string username = OxGramUser.Username;
				if (username.Length <= 0)
				{
					return "Me";
				}
				return username;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x060070F3 RID: 28915 RVA: 0x002DDBFB File Offset: 0x002DBDFB
		public static string Mention
		{
			get
			{
				return "@" + OxGramUser.DisplayName;
			}
		}

		// Token: 0x060070F4 RID: 28916 RVA: 0x002DDC0C File Offset: 0x002DBE0C
		public static bool IsValidDisplayName(string value, out string error)
		{
			string text;
			return OxGramUser.TryNormalizeDisplayName(value, out text, out error);
		}

		// Token: 0x060070F5 RID: 28917 RVA: 0x002DDC24 File Offset: 0x002DBE24
		public static bool TrySetDisplayName(string value, out string error)
		{
			string text;
			if (!OxGramUser.TryNormalizeDisplayName(value, out text, out error))
			{
				return false;
			}
			PlayerPrefs.SetString("OxGram.Username", text);
			PlayerPrefs.Save();
			return true;
		}

		// Token: 0x060070F6 RID: 28918 RVA: 0x002DDC50 File Offset: 0x002DBE50
		internal static bool TryNormalizeDisplayName(string value, out string name, out string error)
		{
			name = "";
			error = "Choose an alias to continue.";
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			try
			{
				name = value.Normalize(NormalizationForm.FormKC).Trim();
				goto IL_0047;
			}
			catch (ArgumentException)
			{
				error = "Use a name without special formatting.";
				return false;
			}
			IL_0038:
			name = name.Substring(1).TrimStart();
			IL_0047:
			if (name.StartsWith("@", StringComparison.Ordinal))
			{
				goto IL_0038;
			}
			if (name.Length == 0)
			{
				return false;
			}
			if (string.Equals(name, "Ciel", StringComparison.OrdinalIgnoreCase))
			{
				error = "Ciel is reserved. Choose an alias to stay anonymous.";
				return false;
			}
			if (name.Length > 20)
			{
				error = "Keep your alias to 20 characters or fewer.";
				return false;
			}
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
				if (char.IsControl(c) || (char.IsWhiteSpace(c) && c != ' ') || unicodeCategory == UnicodeCategory.Format || c == '<' || c == '>' || c == '@' || c == '{' || c == '}')
				{
					error = "Use a name without tags, line breaks or special formatting.";
					return false;
				}
			}
			error = "";
			return true;
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x002DDD64 File Offset: 0x002DBF64
		public static void Clear()
		{
			PlayerPrefs.DeleteKey("OxGram.Username");
			PlayerPrefs.Save();
		}

		// Token: 0x040073B0 RID: 29616
		private const string Key = "OxGram.Username";

		// Token: 0x040073B1 RID: 29617
		public const int MaxDisplayNameLength = 20;

		// Token: 0x040073B2 RID: 29618
		public const string ReservedNameError = "Ciel is reserved. Choose an alias to stay anonymous.";
	}
}
