using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C7 RID: 967
	[Serializable]
	public class UnityTextDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060018D4 RID: 6356 RVA: 0x0009DE58 File Offset: 0x0009C058
		public Text Draw(FObject fobject)
		{
			Text text;
			fobject.Data.GameObject.TryAddGraphic<Text>(out text);
			this.SetStyle(text, fobject);
			this.SetFont(text, fobject);
			text.fontStyle = fobject.GetFontWeight();
			text.alignment = fobject.GetTextAnchor();
			return text;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0009DEA4 File Offset: 0x0009C0A4
		private void SetFont(Text text, FObject fobject)
		{
			Font fontFromArray = this.monoBeh.FontLoader.GetFontFromArray<Font>(fobject, this.monoBeh.FontLoader.TtfFonts);
			text.font = fontFromArray;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0009DEDC File Offset: 0x0009C0DC
		private void SetStyle(Text text, FObject fobject)
		{
			text.resizeTextForBestFit = this.monoBeh.Settings.UnityTextSettings.BestFit;
			text.text = fobject.GetText();
			text.resizeTextMinSize = 1;
			text.resizeTextMaxSize = Convert.ToInt32(fobject.Style.FontSize);
			text.fontSize = Convert.ToInt32(fobject.Style.FontSize);
			text.verticalOverflow = this.monoBeh.Settings.UnityTextSettings.VerticalWrapMode;
			text.horizontalOverflow = this.monoBeh.Settings.UnityTextSettings.HorizontalWrapMode;
			text.lineSpacing = this.monoBeh.Settings.UnityTextSettings.FontLineSpacing;
			FGraphic graphic = fobject.Data.Graphic;
			if (graphic.HasFill)
			{
				if (graphic.Fill.HasGradient)
				{
					text.color = graphic.Fill.GradientPaint.Color;
					return;
				}
				text.color = graphic.Fill.SolidPaint.Color;
			}
		}
	}
}
