using System;
using System.IO;

namespace Features.OxGram
{
	// Token: 0x02000EAE RID: 3758
	public static class OxGramPhotoTag
	{
		// Token: 0x060070C9 RID: 28873 RVA: 0x002DC48B File Offset: 0x002DA68B
		public static string Of(OxGramMessage message)
		{
			if (message == null || message.photo == null)
			{
				return "none";
			}
			return OxGramPhotoTag.FromName(message.photo.name);
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x002DC4B4 File Offset: 0x002DA6B4
		public static string FromName(string nameOrPath)
		{
			if (string.IsNullOrEmpty(nameOrPath))
			{
				return "none";
			}
			string text;
			try
			{
				text = Path.GetFileNameWithoutExtension(nameOrPath);
			}
			catch
			{
				text = nameOrPath;
			}
			if (string.IsNullOrEmpty(text))
			{
				return "none";
			}
			string[] array = text.Split('_', StringSplitOptions.None);
			if (array.Length >= 3 && array[0].Equals("IMG", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = array[1].ToLowerInvariant();
				if (OxGramPhotoTag.IsKnown(text2))
				{
					return text2;
				}
			}
			if (!OxGramPhotoTag.ShowsFace(nameOrPath))
			{
				return "none";
			}
			return "face";
		}

		// Token: 0x060070CB RID: 28875 RVA: 0x002DC544 File Offset: 0x002DA744
		public static bool ShowsCiel(string nameOrPath)
		{
			return OxGramPhotoTag.IsKnown(OxGramPhotoTag.FromName(nameOrPath));
		}

		// Token: 0x060070CC RID: 28876 RVA: 0x002DC554 File Offset: 0x002DA754
		public static bool ShowsFace(string nameOrPath)
		{
			if (string.IsNullOrEmpty(nameOrPath))
			{
				return false;
			}
			string text;
			try
			{
				text = Path.GetFileNameWithoutExtension(nameOrPath);
			}
			catch
			{
				text = nameOrPath;
			}
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (text.EndsWith(".censored", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - ".censored".Length);
			}
			string[] array = text.Split('_', StringSplitOptions.None);
			if (array.Length < 2 || !array[0].Equals("IMG", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (array[1].Equals("face", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			for (int i = 2; i < array.Length; i++)
			{
				if (array[i].Equals("face", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060070CD RID: 28877 RVA: 0x002DC610 File Offset: 0x002DA810
		public static bool ShowsFace(OxGramMessage message)
		{
			return message != null && !(message.photo == null) && OxGramPhotoTag.ShowsFace(message.photo.name);
		}

		// Token: 0x060070CE RID: 28878 RVA: 0x002DC638 File Offset: 0x002DA838
		public static bool IsKnown(string tag)
		{
			return tag == "face" || tag == "tits" || tag == "pussy" || tag == "ass" || tag == "fullbody";
		}

		// Token: 0x04007388 RID: 29576
		public const string Face = "face";

		// Token: 0x04007389 RID: 29577
		public const string Tits = "tits";

		// Token: 0x0400738A RID: 29578
		public const string Pussy = "pussy";

		// Token: 0x0400738B RID: 29579
		public const string Ass = "ass";

		// Token: 0x0400738C RID: 29580
		public const string FullBody = "fullbody";

		// Token: 0x0400738D RID: 29581
		public const string None = "none";
	}
}
