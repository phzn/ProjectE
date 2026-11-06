using System;
using TMPro;

namespace Features.Localization
{
	// Token: 0x020010C6 RID: 4294
	public static class InteractionLocalization
	{
		// Token: 0x0600823F RID: 33343 RVA: 0x00353438 File Offset: 0x00351638
		public static string ObjectName(string english)
		{
			string text;
			if (GameLocalization.TryGetSourceKey("items", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			if (GameLocalization.TryGetSourceKey("interaction_objects", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			if (GameLocalization.TryGetSourceKey("gameplay_prompts", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			return GameLocalization.GetSource("phone_story", english);
		}

		// Token: 0x06008240 RID: 33344 RVA: 0x00353498 File Offset: 0x00351698
		public static string ActionName(string english)
		{
			if (english != null && english.EndsWith("  (Hold: Swap)", StringComparison.Ordinal))
			{
				return GameLocalization.Format("interaction.quick_swap", "{0}  (Hold: Swap)", new object[] { InteractionLocalization.ActionName(english.Substring(0, english.Length - "  (Hold: Swap)".Length)) });
			}
			if (english != null && english.EndsWith("  (Hold: Wear)", StringComparison.Ordinal))
			{
				return GameLocalization.Format("interaction.quick_wear", "{0}  (Hold: Wear)", new object[] { InteractionLocalization.ActionName(english.Substring(0, english.Length - "  (Hold: Wear)".Length)) });
			}
			string text;
			if (GameLocalization.TryGetSourceKey("inventory_ui", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			if (GameLocalization.TryGetSourceKey("interaction_actions", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			if (GameLocalization.TryGetSourceKey("gameplay_prompts", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			string text2;
			if (InteractionLocalization.TryLocalizeCountedAction(english, out text2))
			{
				return text2;
			}
			if (GameLocalization.TryGetSourceKey("camera_ui", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			return GameLocalization.GetSource("phone_story", english);
		}

		// Token: 0x06008241 RID: 33345 RVA: 0x003535A8 File Offset: 0x003517A8
		private static bool TryLocalizeCountedAction(string english, out string localized)
		{
			localized = null;
			if (string.IsNullOrEmpty(english) || !english.EndsWith(")", StringComparison.Ordinal))
			{
				return false;
			}
			int num = english.LastIndexOf(" (", StringComparison.Ordinal);
			if (num <= 0 || num + 2 >= english.Length - 1)
			{
				return false;
			}
			for (int i = num + 2; i < english.Length - 1; i++)
			{
				if (english[i] < '0' || english[i] > '9')
				{
					return false;
				}
			}
			string text = english.Substring(0, num);
			string text2;
			if (!GameLocalization.TryGetSourceKey("inventory_ui", text, out text2) && !GameLocalization.TryGetSourceKey("interaction_actions", text, out text2) && !GameLocalization.TryGetSourceKey("gameplay_prompts", text, out text2))
			{
				return false;
			}
			localized = GameLocalization.Get(text2, text) + english.Substring(num);
			return true;
		}

		// Token: 0x06008242 RID: 33346 RVA: 0x0035366C File Offset: 0x0035186C
		public static void BindObject(TMP_Text text, string english)
		{
			LocalizedGameText.BindComputed(text, () => (InteractionLocalization.ObjectName(english) ?? string.Empty).ToUpper());
		}

		// Token: 0x06008243 RID: 33347 RVA: 0x0035369C File Offset: 0x0035189C
		public static void BindAction(TMP_Text text, string english)
		{
			LocalizedGameText.BindComputed(text, () => InteractionLocalization.ActionName(english));
		}

		// Token: 0x06008244 RID: 33348 RVA: 0x003536CC File Offset: 0x003518CC
		public static void BindKey(TMP_Text text, string english)
		{
			LocalizedGameText.BindComputed(text, () => InteractionLocalization.KeyName(english));
		}

		// Token: 0x06008245 RID: 33349 RVA: 0x003536FC File Offset: 0x003518FC
		public static string KeyName(string english)
		{
			string text;
			if (GameLocalization.TryGetSourceKey("input", english, out text) || GameLocalization.TryGetSourceKey("phone_story", english, out text))
			{
				return GameLocalization.Get(text, english);
			}
			if (GameLocalization.CurrentCode == "en")
			{
				return english;
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(english);
			if (num <= 441385463U)
			{
				if (num != 74419091U)
				{
					if (num != 403239141U)
					{
						if (num != 441385463U)
						{
							return english;
						}
						if (!(english == "MMB"))
						{
							return english;
						}
						return GameLocalization.GetSource("input", "Middle Mouse");
					}
					else if (!(english == "SPACE"))
					{
						return english;
					}
				}
				else
				{
					if (!(english == "CTRL + MW"))
					{
						return english;
					}
					return GameLocalization.GetSource("input", "CTRL + Wheel");
				}
			}
			else if (num <= 2368834032U)
			{
				if (num != 894689925U)
				{
					if (num != 2368834032U)
					{
						return english;
					}
					if (!(english == "LMB"))
					{
						return english;
					}
					return GameLocalization.GetSource("input", "Left Mouse");
				}
				else if (!(english == "space"))
				{
					return english;
				}
			}
			else if (num != 2984405022U)
			{
				if (num != 3027441364U)
				{
					return english;
				}
				if (!(english == "Space / Shift"))
				{
					return english;
				}
				return GameLocalization.GetSource("input", "Space") + " / Shift";
			}
			else
			{
				if (!(english == "RMB"))
				{
					return english;
				}
				return GameLocalization.GetSource("input", "Right Mouse");
			}
			return GameLocalization.GetSource("input", "Space");
		}
	}
}
