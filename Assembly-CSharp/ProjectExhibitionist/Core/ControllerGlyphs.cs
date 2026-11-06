using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A4A RID: 2634
	public static class ControllerGlyphs
	{
		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x001D2CD4 File Offset: 0x001D0ED4
		public static ControllerFamily Family
		{
			get
			{
				ControllerGlyphs.RefreshDevice();
				return ControllerGlyphs._family;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x001D2CE0 File Offset: 0x001D0EE0
		public static int Version
		{
			get
			{
				ControllerGlyphs.RefreshDevice();
				return ControllerGlyphs._version;
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x001D2CEC File Offset: 0x001D0EEC
		public static TMP_SpriteAsset SpriteAsset
		{
			get
			{
				if (!(ControllerGlyphs._sprites != null))
				{
					return ControllerGlyphs._sprites = Resources.Load<TMP_SpriteAsset>("Controller/PEControllerGlyphs");
				}
				return ControllerGlyphs._sprites;
			}
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x001D2D14 File Offset: 0x001D0F14
		public static ControllerFamily DetectFamily(string layout, string manufacturer, string product)
		{
			string text = string.Concat(new string[] { layout, " ", manufacturer, " ", product }).ToLowerInvariant();
			if (!text.Contains("dualshock") && !text.Contains("dualsense") && !text.Contains("playstation") && !text.Contains("sony") && !text.Contains("ps4") && !text.Contains("ps5"))
			{
				return ControllerFamily.Xbox;
			}
			return ControllerFamily.PlayStation;
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x001D2DA0 File Offset: 0x001D0FA0
		private static void RefreshDevice()
		{
			Gamepad current = Gamepad.current;
			int num = ((current != null && current.added) ? current.deviceId : (-1));
			if (num == ControllerGlyphs._deviceId)
			{
				return;
			}
			ControllerGlyphs._deviceId = num;
			ControllerGlyphs._family = ((current != null) ? ControllerGlyphs.DetectFamily(current.layout, current.description.manufacturer, current.description.product) : ControllerFamily.Xbox);
			ControllerGlyphs._version++;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x001D2E15 File Offset: 0x001D1015
		public static string Format(string canonical)
		{
			return ControllerGlyphs.Format(canonical, ControllerGlyphs.Family);
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x001D2E24 File Offset: 0x001D1024
		public static string Format(string canonical, ControllerFamily family)
		{
			if (string.IsNullOrEmpty(canonical) || canonical.Contains("<sprite"))
			{
				return canonical;
			}
			if (!ControllerGlyphs._registered)
			{
				TMP_Text.OnSpriteAssetRequest -= ControllerGlyphs.ResolveAsset;
				TMP_Text.OnSpriteAssetRequest += ControllerGlyphs.ResolveAsset;
				ControllerGlyphs._registered = true;
			}
			int family2 = (int)family;
			string text = family2.ToString() + ":" + canonical;
			string text2;
			if (ControllerGlyphs.Cache.TryGetValue(text, out text2))
			{
				return text2;
			}
			string text3 = ControllerGlyphs.Controls.Replace(canonical, delegate(Match match)
			{
				string text4 = ControllerGlyphs.CanonicalToken(match.Value);
				string text5 = ((text4 == "LB" || text4 == "RB" || text4 == "LT" || text4 == "RT" || text4 == "Menu" || text4 == "View") ? "160%" : "125%");
				return string.Concat(new string[]
				{
					"<size=",
					text5,
					"><sprite=\"PEControllerGlyphs\" name=\"",
					(family == ControllerFamily.PlayStation) ? "ps_" : "xb_",
					ControllerGlyphs.TokenId(text4),
					"\" tint=0></size>"
				});
			});
			if (ControllerGlyphs.Cache.Count > 512)
			{
				ControllerGlyphs.Cache.Clear();
			}
			ControllerGlyphs.Cache[text] = text3;
			return text3;
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x001D2EF0 File Offset: 0x001D10F0
		public static void Apply(TMP_Text text, string canonical)
		{
			if (text == null)
			{
				return;
			}
			text.richText = true;
			text.text = ControllerGlyphs.Format(canonical);
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x001D2F0F File Offset: 0x001D110F
		public static string Plain(string canonical, ControllerFamily family)
		{
			if (family != ControllerFamily.PlayStation || string.IsNullOrEmpty(canonical))
			{
				return canonical;
			}
			return ControllerGlyphs.Controls.Replace(canonical, delegate(Match match)
			{
				string text = ControllerGlyphs.CanonicalToken(match.Value);
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2095213421U)
				{
					if (num <= 1582017048U)
					{
						if (num != 12978010U)
						{
							if (num == 1582017048U)
							{
								if (text == "View")
								{
									return "Share";
								}
							}
						}
						else if (text == "Menu")
						{
							return "Options";
						}
					}
					else if (num != 1726105803U)
					{
						if (num != 1761926707U)
						{
							if (num == 2095213421U)
							{
								if (text == "LT")
								{
									return "L2";
								}
							}
						}
						else if (text == "RT")
						{
							return "R2";
						}
					}
					else if (text == "LB")
					{
						return "L1";
					}
				}
				else if (num <= 3289118412U)
				{
					if (num != 2131034325U)
					{
						if (num == 3289118412U)
						{
							if (text == "A")
							{
								return "Cross";
							}
						}
					}
					else if (text == "RB")
					{
						return "R1";
					}
				}
				else if (num != 3339451269U)
				{
					if (num != 3691781268U)
					{
						if (num == 3708558887U)
						{
							if (text == "X")
							{
								return "Square";
							}
						}
					}
					else if (text == "Y")
					{
						return "Triangle";
					}
				}
				else if (text == "B")
				{
					return "Circle";
				}
				return ControllerGlyphs.CanonicalToken(match.Value);
			});
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x001D2F49 File Offset: 0x001D1149
		public static string TokenId(string canonical)
		{
			return canonical.Replace("-", "").Replace(" ", "").ToLowerInvariant();
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x001D2F70 File Offset: 0x001D1170
		private static string CanonicalToken(string value)
		{
			if (value.Equals("Start", StringComparison.OrdinalIgnoreCase))
			{
				return "Menu";
			}
			if (value.Equals("Select", StringComparison.OrdinalIgnoreCase))
			{
				return "View";
			}
			if (value.Equals("Left stick", StringComparison.OrdinalIgnoreCase))
			{
				return "LS";
			}
			if (value.Equals("Right stick", StringComparison.OrdinalIgnoreCase))
			{
				return "RS";
			}
			string text = ControllerGlyphs.TokenId(value);
			foreach (string text2 in ControllerGlyphs.Tokens)
			{
				if (ControllerGlyphs.TokenId(text2) == text)
				{
					return text2;
				}
			}
			return value.ToUpperInvariant();
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x001D3002 File Offset: 0x001D1202
		private static TMP_SpriteAsset ResolveAsset(int hash, string name)
		{
			if (!(name == "PEControllerGlyphs"))
			{
				return null;
			}
			return ControllerGlyphs.SpriteAsset;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x001D3018 File Offset: 0x001D1218
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			ControllerGlyphs._sprites = null;
			ControllerGlyphs._deviceId = -2;
			ControllerGlyphs._version++;
			ControllerGlyphs.Cache.Clear();
			TMP_Text.OnSpriteAssetRequest -= ControllerGlyphs.ResolveAsset;
			TMP_Text.OnSpriteAssetRequest += ControllerGlyphs.ResolveAsset;
			ControllerGlyphs._registered = true;
		}

		// Token: 0x04004CDB RID: 19675
		public const string AssetName = "PEControllerGlyphs";

		// Token: 0x04004CDC RID: 19676
		public static readonly string[] Tokens = new string[]
		{
			"A", "B", "X", "Y", "LB", "RB", "LT", "RT", "L3", "R3",
			"Menu", "View", "D-Pad Up", "D-Pad Down", "D-Pad Left", "D-Pad Right", "LS", "RS", "D-Pad"
		};

		// Token: 0x04004CDD RID: 19677
		private static readonly Regex Controls = new Regex("(?<![\\p{L}\\p{N}_])(?:[Dd][ -]?[Pp][Aa][Dd](?:\\s*(?:Up|Down|Left|Right|UP|DOWN|LEFT|RIGHT))?|Left [Ss]tick|Right [Ss]tick|L3|R3|LS|RS|LB|RB|LT|RT|MENU|Menu|VIEW|View|[ABXY])(?![\\p{L}\\p{N}_])", RegexOptions.CultureInvariant);

		// Token: 0x04004CDE RID: 19678
		private static TMP_SpriteAsset _sprites;

		// Token: 0x04004CDF RID: 19679
		private static ControllerFamily _family;

		// Token: 0x04004CE0 RID: 19680
		private static int _deviceId = -1;

		// Token: 0x04004CE1 RID: 19681
		private static int _version;

		// Token: 0x04004CE2 RID: 19682
		private static bool _registered;

		// Token: 0x04004CE3 RID: 19683
		private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();
	}
}
