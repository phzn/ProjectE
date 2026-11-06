using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.Constants;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002D1 RID: 721
	[Serializable]
	public class NameHumanizer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600141F RID: 5151 RVA: 0x000818CC File Offset: 0x0007FACC
		public string GetHumanizedTextPrefabName(FObject fobject)
		{
			string text = fobject.FontNameToString(true, true, null, false);
			string text2 = fobject.Style.FontSize.ToString();
			string text3 = fobject.Style.TextAlignHorizontal.ToPascalCase();
			string text4 = fobject.Style.TextAlignVertical.ToPascalCase();
			string text5 = text3 + text4;
			string colorString = this.GetColorString(fobject);
			string text6 = string.Concat(new string[] { text, "-", text2, "-", text5 });
			if (!colorString.IsEmpty())
			{
				text6 = text6 + "-" + colorString;
			}
			return string.Format("{0} {1}", this.monoBeh.Settings.TextFontsSettings.TextComponent, text6);
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x000819AC File Offset: 0x0007FBAC
		private string GetColorString(FObject fobject)
		{
			FGraphic graphic = fobject.Data.Graphic;
			string text = "";
			try
			{
				if (graphic.HasFill && graphic.HasStroke)
				{
					string text2 = "";
					if (graphic.Fill.HasSolid)
					{
						text2 = this.GetSolidName(graphic.Fill.SolidPaint);
					}
					else if (graphic.Fill.HasGradient)
					{
						text2 = this.GetGradientName(graphic.Fill.GradientPaint);
					}
					string text3 = "";
					if (graphic.Stroke.HasSolid)
					{
						text3 = this.GetSolidName(graphic.Stroke.SolidPaint);
					}
					else if (graphic.Stroke.HasGradient)
					{
						text3 = this.GetGradientName(graphic.Stroke.GradientPaint);
					}
					text = "fill-" + text2 + "_stroke-" + text3;
				}
				else if (graphic.HasFill)
				{
					string text4 = "";
					if (graphic.Fill.HasSolid)
					{
						text4 = this.GetSolidName(graphic.Fill.SolidPaint);
					}
					else if (graphic.Fill.HasGradient)
					{
						text4 = this.GetGradientName(graphic.Fill.GradientPaint);
					}
					text = text4;
				}
				else if (graphic.HasStroke)
				{
					string text5 = "";
					if (graphic.Stroke.HasSolid)
					{
						text5 = this.GetSolidName(graphic.Stroke.SolidPaint);
					}
					else if (graphic.Stroke.HasGradient)
					{
						text5 = this.GetGradientName(graphic.Stroke.GradientPaint);
					}
					text = text5;
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00081B94 File Offset: 0x0007FD94
		private string GetSolidName(Paint paint)
		{
			Color32 color = paint.Color;
			TextPrefabNameType textPrefabNameType = this.monoBeh.Settings.PrefabSettings.TextPrefabNameType;
			if (textPrefabNameType != TextPrefabNameType.HumanizedColorString)
			{
				if (textPrefabNameType != TextPrefabNameType.HumanizedColorHEX)
				{
				}
				return ColorUtility.ToHtmlStringRGB(color);
			}
			string key = this.GetNearestColor(paint.Color).Key;
			return string.Format("{0}{1}", key, color.a);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00081C08 File Offset: 0x0007FE08
		private string GetGradientName(Paint paint)
		{
			Color32 color = paint.GradientStops.First<GradientStop>().Color;
			Color32 color2 = paint.GradientStops.Last<GradientStop>().Color;
			TextPrefabNameType textPrefabNameType = this.monoBeh.Settings.PrefabSettings.TextPrefabNameType;
			if (textPrefabNameType != TextPrefabNameType.HumanizedColorString)
			{
				if (textPrefabNameType != TextPrefabNameType.HumanizedColorHEX)
				{
				}
				string text = ColorUtility.ToHtmlStringRGB(color);
				string text2 = ColorUtility.ToHtmlStringRGB(color2);
				return text + "-" + text2;
			}
			string key = this.GetNearestColor(color).Key;
			string key2 = this.GetNearestColor(color2).Key;
			return string.Format("{0}{1}-{2}{3}", new object[] { key, color.a, key2, color2.a });
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00081CF4 File Offset: 0x0007FEF4
		private KeyValuePair<string, Color> GetNearestColor(Color inputColor)
		{
			KeyValuePair<NetKnownColor, Color32> keyValuePair = default(KeyValuePair<NetKnownColor, Color32>);
			float num = float.MaxValue;
			foreach (KeyValuePair<NetKnownColor, Color32> keyValuePair2 in NetKnownColors.KnownColors)
			{
				float num2 = Mathf.Pow((float)keyValuePair2.Value.r - inputColor.r, 2f);
				float num3 = Mathf.Pow((float)keyValuePair2.Value.g - inputColor.g, 2f);
				float num4 = Mathf.Pow((float)keyValuePair2.Value.b - inputColor.b, 2f);
				float num5 = Mathf.Sqrt(num2 + num3 + num4);
				if (num5 == 0f)
				{
					keyValuePair = keyValuePair2;
					break;
				}
				if (num5 < num)
				{
					num = num5;
					keyValuePair = keyValuePair2;
				}
			}
			if (keyValuePair.IsDefault<KeyValuePair<NetKnownColor, Color32>>())
			{
				return new KeyValuePair<string, Color>("UnknownColor", Color.black);
			}
			return new KeyValuePair<string, Color>(keyValuePair.Key.ToString(), keyValuePair.Value);
		}
	}
}
