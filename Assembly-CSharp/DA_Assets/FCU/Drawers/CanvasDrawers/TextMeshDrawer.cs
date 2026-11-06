using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using TMPro;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C6 RID: 966
	[Serializable]
	public class TextMeshDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0009D7D2 File Offset: 0x0009B9D2
		public List<FObject> Texts
		{
			get
			{
				return this.texts;
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x00004F31 File Offset: 0x00003131
		public void DrawRTL(FObject fobject)
		{
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0009D7DC File Offset: 0x0009B9DC
		public void DrawTMP(FObject fobject)
		{
			TextMeshProUGUI textMeshProUGUI;
			fobject.Data.GameObject.TryAddGraphic<TextMeshProUGUI>(out textMeshProUGUI);
			TMP_Text tmp_Text = textMeshProUGUI;
			this.Draw(fobject, tmp_Text);
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00004F31 File Offset: 0x00003131
		public void DrawNovaTMP(FObject fobject)
		{
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0009D808 File Offset: 0x0009BA08
		private void Draw(FObject fobject, TMP_Text text)
		{
			string text2 = fobject.GetText();
			text.text = text2;
			text.overrideColorTags = this.monoBeh.Settings.TextMeshSettings.OverrideTags;
			text.enableAutoSizing = this.monoBeh.Settings.TextMeshSettings.AutoSize;
			text.enableWordWrapping = this.monoBeh.Settings.TextMeshSettings.Wrapping;
			if (this.monoBeh.IsNova())
			{
				text.isOrthographic = this.monoBeh.Settings.TextMeshSettings.OrthographicMode;
			}
			text.richText = this.monoBeh.Settings.TextMeshSettings.RichText;
			text.raycastTarget = this.monoBeh.Settings.TextMeshSettings.RaycastTarget;
			text.parseCtrlCharacters = this.monoBeh.Settings.TextMeshSettings.ParseEscapeCharacters;
			text.useMaxVisibleDescender = this.monoBeh.Settings.TextMeshSettings.VisibleDescender;
			text.enableKerning = this.monoBeh.Settings.TextMeshSettings.Kerning;
			text.extraPadding = this.monoBeh.Settings.TextMeshSettings.ExtraPadding;
			text.horizontalMapping = this.monoBeh.Settings.TextMeshSettings.HorizontalMapping;
			text.verticalMapping = this.monoBeh.Settings.TextMeshSettings.VerticalMapping;
			text.geometrySortingOrder = this.monoBeh.Settings.TextMeshSettings.GeometrySorting;
			if (this.monoBeh.Settings.TextMeshSettings.Shader != null)
			{
				text.fontMaterial.shader = this.monoBeh.Settings.TextMeshSettings.Shader;
			}
			this.SetFont(text, fobject);
			text.alignment = fobject.GetTextAnchor().ToTextMeshAnchor();
			this.SetFontSize(text, fobject);
			this.SetFontCase(text, fobject);
			this.SetOverflowMode(text, fobject);
			this.SetColor(text, fobject);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0009DA08 File Offset: 0x0009BC08
		private void SetFontSize(TMP_Text text, FObject fobject)
		{
			if (this.monoBeh.Settings.TextMeshSettings.AutoSize)
			{
				text.fontSizeMin = 1f;
				text.fontSizeMax = fobject.Style.FontSize;
			}
			else
			{
				text.fontSize = fobject.Style.FontSize;
			}
			if (this.monoBeh.IsNova() && !this.monoBeh.Settings.TextMeshSettings.OrthographicMode)
			{
				text.fontSize *= 10f;
			}
			if (!fobject.Style.FontFamily.IsEmpty())
			{
				text.characterSpacing = (float)Math.Round((double)(fobject.Style.LetterSpacing / text.fontSize * 100f), FcuConfig.Rounding.FontDigits);
				text.lineSpacing = (float)Math.Round((double)(fobject.Style.LineHeightPx / fobject.Style.FontSize * 100f - text.font.faceInfo.lineHeight), FcuConfig.Rounding.FontDigits);
				text.paragraphSpacing = (float)Math.Round((double)(fobject.Style.ParagraphSpacing / fobject.Style.FontSize * 100f), FcuConfig.Rounding.FontDigits);
			}
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0009DB74 File Offset: 0x0009BD74
		private void SetFont(TMP_Text text, FObject fobject)
		{
			TMP_FontAsset fontFromArray = this.monoBeh.FontLoader.GetFontFromArray<TMP_FontAsset>(fobject, this.monoBeh.FontLoader.TmpFonts);
			text.font = fontFromArray;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0009DBAC File Offset: 0x0009BDAC
		private void SetFontCase(TMP_Text text, FObject fobject)
		{
			FontStyles fontStyles = FontStyles.Normal;
			FontStyles fontStyles2 = FontStyles.Normal;
			FontStyles fontStyles3 = FontStyles.Normal;
			FontStyles fontStyles4 = FontStyles.Normal;
			if (!fobject.Data.HasFontAsset)
			{
				if (fobject.Style.Italic.ToBoolNullFalse())
				{
					fontStyles3 = FontStyles.Italic;
				}
				if (fobject.Style.FontWeight > 600)
				{
					fontStyles4 = FontStyles.Bold;
				}
			}
			string text2 = fobject.Style.TextDecoration;
			if (!(text2 == "UNDERLINE"))
			{
				if (text2 == "STRIKETHROUGH")
				{
					fontStyles = FontStyles.Strikethrough;
				}
			}
			else
			{
				fontStyles = FontStyles.Underline;
			}
			text2 = fobject.Style.TextCase;
			if (!(text2 == "UPPER"))
			{
				if (!(text2 == "LOWER"))
				{
					if (!(text2 == "TITLE"))
					{
						if (text2 == "SMALL_CAPS")
						{
							fontStyles2 = FontStyles.SmallCaps;
						}
					}
					else
					{
						fontStyles2 = FontStyles.Normal;
					}
				}
				else
				{
					fontStyles2 = FontStyles.LowerCase;
				}
			}
			else
			{
				fontStyles2 = FontStyles.UpperCase;
			}
			FontStyles fontStyles5 = fontStyles | fontStyles2 | fontStyles3 | fontStyles4;
			text.fontStyle = fontStyles5;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0009DCA8 File Offset: 0x0009BEA8
		private void SetOverflowMode(TMP_Text text, FObject fobject)
		{
			TextOverflowModes textOverflowModes = this.monoBeh.Settings.TextMeshSettings.Overflow;
			if ((fobject.Style.TextAutoResize == TextAutoResize.NONE || fobject.Style.TextAutoResize == TextAutoResize.TRUNCATE) && fobject.Style.TextCase == "ENDING")
			{
				textOverflowModes = TextOverflowModes.Ellipsis;
			}
			text.overflowMode = textOverflowModes;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0009DD14 File Offset: 0x0009BF14
		private void SetColor(TMP_Text text, FObject fobject)
		{
			FGraphic graphic = fobject.Data.Graphic;
			text.enableVertexGradient = false;
			if (graphic.Fill.HasSolid)
			{
				text.color = graphic.Fill.SolidPaint.Color;
			}
			else if (graphic.Fill.HasGradient)
			{
				List<GradientColorKey> list = graphic.Fill.GradientPaint.ToGradientColorKeys();
				if (!list.IsEmpty<GradientColorKey>())
				{
					text.color = list.First<GradientColorKey>().color;
				}
			}
			if (graphic.HasStroke && fobject.StrokeAlign == StrokeAlign.INSIDE)
			{
				float num = fobject.StrokeWeight / text.preferredHeight;
				text.outlineWidth = num;
				if (graphic.Stroke.HasSolid)
				{
					text.outlineColor = graphic.Stroke.SolidPaint.Color;
					return;
				}
				if (graphic.Stroke.HasGradient)
				{
					List<GradientColorKey> list2 = graphic.Stroke.GradientPaint.ToGradientColorKeys();
					if (!list2.IsEmpty<GradientColorKey>())
					{
						text.outlineColor = list2.First<GradientColorKey>().color;
						return;
					}
				}
			}
			else
			{
				text.outlineWidth = 0f;
			}
		}

		// Token: 0x040019D8 RID: 6616
		[SerializeField]
		private List<FObject> texts;

		// Token: 0x040019D9 RID: 6617
		private const float _sizeFixForNonOrthographicNovaText = 10f;
	}
}
