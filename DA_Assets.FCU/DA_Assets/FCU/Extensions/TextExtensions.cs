using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200004E RID: 78
	public static class TextExtensions
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x00006020 File Offset: 0x00004220
		public static FontStyle GetFontWeight(this FObject fobject)
		{
			string fontPostScriptName = fobject.Style.FontPostScriptName;
			if (!fontPostScriptName.IsEmpty())
			{
				if (fontPostScriptName.Contains(FontStyle.Bold.ToString()))
				{
					if (fobject.Style.Italic.ToBoolNullFalse())
					{
						return FontStyle.BoldAndItalic;
					}
					return FontStyle.Bold;
				}
				else if (fobject.Style.Italic.ToBoolNullFalse())
				{
					return FontStyle.Italic;
				}
			}
			return FontStyle.Normal;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006090 File Offset: 0x00004290
		public static TextAnchor GetTextAnchor(this FObject fobject)
		{
			string text = fobject.Style.TextAlignVertical + " " + fobject.Style.TextAlignHorizontal;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1451492718U)
			{
				if (num <= 645222069U)
				{
					if (num != 404177299U)
					{
						if (num == 645222069U)
						{
							if (text == "TOP LEFT")
							{
								return TextAnchor.UpperLeft;
							}
						}
					}
					else if (text == "BOTTOM CENTER")
					{
						return TextAnchor.LowerCenter;
					}
				}
				else if (num != 1197105300U)
				{
					if (num == 1451492718U)
					{
						if (text == "TOP RIGHT")
						{
							return TextAnchor.UpperRight;
						}
					}
				}
				else if (text == "BOTTOM RIGHT")
				{
					return TextAnchor.LowerRight;
				}
			}
			else if (num <= 2578779901U)
			{
				if (num != 2136135531U)
				{
					if (num == 2578779901U)
					{
						if (text == "CENTER LEFT")
						{
							return TextAnchor.MiddleLeft;
						}
					}
				}
				else if (text == "BOTTOM LEFT")
				{
					return TextAnchor.LowerLeft;
				}
			}
			else if (num != 2849903958U)
			{
				if (num != 2878838533U)
				{
					if (num == 3616756893U)
					{
						if (text == "TOP CENTER")
						{
							return TextAnchor.UpperCenter;
						}
					}
				}
				else if (text == "CENTER CENTER")
				{
					return TextAnchor.MiddleCenter;
				}
			}
			else if (text == "CENTER RIGHT")
			{
				return TextAnchor.MiddleRight;
			}
			return TextAnchor.MiddleCenter;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000061E4 File Offset: 0x000043E4
		public static TextAlignmentOptions ToTextMeshAnchor(this TextAnchor textAnchor)
		{
			switch (textAnchor)
			{
			case TextAnchor.UpperLeft:
				return TextAlignmentOptions.TopLeft;
			case TextAnchor.UpperCenter:
				return TextAlignmentOptions.Top;
			case TextAnchor.UpperRight:
				return TextAlignmentOptions.TopRight;
			case TextAnchor.MiddleLeft:
				return TextAlignmentOptions.Left;
			case TextAnchor.MiddleCenter:
				return TextAlignmentOptions.Center;
			case TextAnchor.MiddleRight:
				return TextAlignmentOptions.Right;
			case TextAnchor.LowerLeft:
				return TextAlignmentOptions.BottomLeft;
			case TextAnchor.LowerCenter:
				return TextAlignmentOptions.Bottom;
			case TextAnchor.LowerRight:
				return TextAlignmentOptions.BottomRight;
			default:
				return TextAlignmentOptions.Center;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006258 File Offset: 0x00004458
		public static string ToUITKAnchor(this TextAnchor textAnchor)
		{
			switch (textAnchor)
			{
			case TextAnchor.UpperLeft:
				return "upper-left";
			case TextAnchor.UpperCenter:
				return "upper-center";
			case TextAnchor.UpperRight:
				return "upper-right";
			case TextAnchor.MiddleLeft:
				return "middle-left";
			case TextAnchor.MiddleCenter:
				return "middle-center";
			case TextAnchor.MiddleRight:
				return "middle-right";
			case TextAnchor.LowerLeft:
				return "lower-left";
			case TextAnchor.LowerCenter:
				return "lower-center";
			case TextAnchor.LowerRight:
				return "lower-right";
			default:
				return "middle-center";
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000062CC File Offset: 0x000044CC
		public static string GetText(this GameObject go)
		{
			TMP_Text tmp_Text;
			if (go.TryGetComponent<TMP_Text>(out tmp_Text))
			{
				return tmp_Text.text;
			}
			Text text;
			if (go.TryGetComponent<Text>(out text))
			{
				return text.text;
			}
			return null;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000062FC File Offset: 0x000044FC
		public static void SetText(this GameObject go, string text)
		{
			TextExtensions.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.go = go;
			CS$<>8__locals1.text = text;
			TextExtensions.<SetText>g__SetNovaText|5_0();
			if (!TextExtensions.<SetText>g__SetTMPText|5_1(ref CS$<>8__locals1))
			{
				TextExtensions.<SetText>g__SetUnityText|5_2(ref CS$<>8__locals1);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006330 File Offset: 0x00004530
		public static string FontWeightToString(this int weight)
		{
			if (weight <= 400)
			{
				if (weight <= 200)
				{
					if (weight == 100)
					{
						return "Thin";
					}
					if (weight == 200)
					{
						return "ExtraLight";
					}
				}
				else
				{
					if (weight == 300)
					{
						return "Light";
					}
					if (weight == 400)
					{
						return "Regular";
					}
				}
			}
			else if (weight <= 600)
			{
				if (weight == 500)
				{
					return "Medium";
				}
				if (weight == 600)
				{
					return "SemiBold";
				}
			}
			else
			{
				if (weight == 700)
				{
					return "Bold";
				}
				if (weight == 800)
				{
					return "ExtraBold";
				}
				if (weight == 900)
				{
					return "Black";
				}
			}
			return weight.ToString();
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000063E0 File Offset: 0x000045E0
		public static string FontNameToString(this FontMetadata fontMetadata, bool includeWeight = true, bool includeItalic = true, FontSubset? fontSubset = null, bool format = false)
		{
			string text = fontMetadata.Family;
			if (includeWeight)
			{
				text = text + "-" + fontMetadata.Weight.FontWeightToString();
			}
			if (includeItalic && fontMetadata.FontStyle == FontStyle.Italic)
			{
				text += string.Format("-{0}", FontStyle.Italic);
			}
			if (fontSubset != null)
			{
				text += string.Format("-{0}", fontSubset);
			}
			if (format)
			{
				text = text.FormatFontName();
			}
			return text;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006460 File Offset: 0x00004660
		public static FontMetadata GetFontMetadata(this FObject fobject)
		{
			string text = null;
			int num = 0;
			FontStyle fontStyle = FontStyle.Normal;
			if (!fobject.StyleOverrideTable.IsEmpty<KeyValuePair<string, Style>>())
			{
				if (!fobject.StyleOverrideTable.First<KeyValuePair<string, Style>>().Value.FontFamily.IsEmpty())
				{
					text = fobject.StyleOverrideTable.First<KeyValuePair<string, Style>>().Value.FontFamily;
				}
				if (fobject.StyleOverrideTable.First<KeyValuePair<string, Style>>().Value.FontWeight > 0)
				{
					num = fobject.StyleOverrideTable.First<KeyValuePair<string, Style>>().Value.FontWeight;
				}
				fontStyle = (fobject.StyleOverrideTable.First<KeyValuePair<string, Style>>().Value.Italic.ToBoolNullFalse() ? FontStyle.Italic : FontStyle.Normal);
			}
			if (text == null || num == 0)
			{
				text = fobject.Style.FontFamily;
				num = fobject.Style.FontWeight;
				fontStyle = (fobject.Style.Italic.ToBoolNullFalse() ? FontStyle.Italic : FontStyle.Normal);
			}
			return new FontMetadata
			{
				Family = text,
				Weight = num,
				FontStyle = fontStyle
			};
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00006593 File Offset: 0x00004793
		public static string FontNameToString(this FObject fobject, bool includeWeight = true, bool includeItalic = true, FontSubset? fontSubset = null, bool format = false)
		{
			return fobject.GetFontMetadata().FontNameToString(includeWeight, includeItalic, fontSubset, format);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000065A5 File Offset: 0x000047A5
		[CompilerGenerated]
		internal static bool <SetText>g__SetNovaText|5_0()
		{
			return false;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000065A8 File Offset: 0x000047A8
		[CompilerGenerated]
		internal static bool <SetText>g__SetTMPText|5_1(ref TextExtensions.<>c__DisplayClass5_0 A_0)
		{
			TMP_Text tmp_Text;
			if (A_0.go.TryGetComponentSafe<TMP_Text>(out tmp_Text))
			{
				tmp_Text.text = A_0.text;
				return true;
			}
			return false;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000065D4 File Offset: 0x000047D4
		[CompilerGenerated]
		internal static void <SetText>g__SetUnityText|5_2(ref TextExtensions.<>c__DisplayClass5_0 A_0)
		{
			Text text;
			if (A_0.go.TryGetComponent<Text>(out text))
			{
				text.text = A_0.text;
			}
		}
	}
}
