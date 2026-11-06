using System;
using Features.Localization;
using TMPro;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x0200139B RID: 5019
	public static class CielOSContentText
	{
		// Token: 0x06009B60 RID: 39776 RVA: 0x003F846F File Offset: 0x003F666F
		public static string Get(string english)
		{
			return GameLocalization.GetSource("cielos_content", english);
		}

		// Token: 0x06009B61 RID: 39777 RVA: 0x003F847C File Offset: 0x003F667C
		public static string Format(string english, params object[] arguments)
		{
			return GameLocalization.FormatSource("cielos_content", english, arguments);
		}

		// Token: 0x06009B62 RID: 39778 RVA: 0x003F848A File Offset: 0x003F668A
		public static void Bind(TMP_Text text, string english)
		{
			if (text != null)
			{
				LocalizedGameText.BindSource(text, "cielos_content", english, null).FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06009B63 RID: 39779 RVA: 0x003F84A8 File Offset: 0x003F66A8
		public static void BindFormat(TMP_Text text, string english, params object[] arguments)
		{
			if (text != null)
			{
				LocalizedGameText.BindFormat(text, "cielos_content", english, arguments).FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06009B64 RID: 39780 RVA: 0x003F84C6 File Offset: 0x003F66C6
		public static void BindButton(Button button, string english)
		{
			if (button != null)
			{
				CielOSContentText.Bind(button.GetComponentInChildren<TMP_Text>(true), english);
			}
		}

		// Token: 0x06009B65 RID: 39781 RVA: 0x003F84E0 File Offset: 0x003F66E0
		public static string Preferred(string scope, string english)
		{
			string text;
			if (!GameLocalization.TryGetSourceKey(scope, english, out text))
			{
				return CielOSContentText.Get(english);
			}
			return GameLocalization.Get(text, english);
		}

		// Token: 0x0400A043 RID: 41027
		public const string Scope = "cielos_content";
	}
}
