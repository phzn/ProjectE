using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements
{
	// Token: 0x020002D1 RID: 721
	internal class InlineStyleAccess : StyleValueCollection, IStyle
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x00055480 File Offset: 0x00053680
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x000554B4 File Offset: 0x000536B4
		StyleEnum<Align> IStyle.alignContent
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.AlignContent);
				return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Align>(StylePropertyId.AlignContent, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.AlignContent = (YogaAlign)this.ve.computedStyle.alignContent;
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x00055504 File Offset: 0x00053704
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x00055538 File Offset: 0x00053738
		StyleEnum<Align> IStyle.alignItems
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.AlignItems);
				return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Align>(StylePropertyId.AlignItems, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.AlignItems = (YogaAlign)this.ve.computedStyle.alignItems;
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00055588 File Offset: 0x00053788
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x000555BC File Offset: 0x000537BC
		StyleEnum<Align> IStyle.alignSelf
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.AlignSelf);
				return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Align>(StylePropertyId.AlignSelf, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.AlignSelf = (YogaAlign)this.ve.computedStyle.alignSelf;
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x0005560C File Offset: 0x0005380C
		// (set) Token: 0x0600158D RID: 5517 RVA: 0x0005562C File Offset: 0x0005382C
		StyleColor IStyle.backgroundColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BackgroundColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00055660 File Offset: 0x00053860
		// (set) Token: 0x0600158F RID: 5519 RVA: 0x00055680 File Offset: 0x00053880
		StyleBackground IStyle.backgroundImage
		{
			get
			{
				return base.GetStyleBackground(StylePropertyId.BackgroundImage);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundImage, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x000556B4 File Offset: 0x000538B4
		// (set) Token: 0x06001591 RID: 5521 RVA: 0x000556D4 File Offset: 0x000538D4
		StyleBackgroundPosition IStyle.backgroundPositionX
		{
			get
			{
				return base.GetStyleBackgroundPosition(StylePropertyId.BackgroundPositionX);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundPositionX, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x00055708 File Offset: 0x00053908
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x00055728 File Offset: 0x00053928
		StyleBackgroundPosition IStyle.backgroundPositionY
		{
			get
			{
				return base.GetStyleBackgroundPosition(StylePropertyId.BackgroundPositionY);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundPositionY, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x0005575C File Offset: 0x0005395C
		// (set) Token: 0x06001595 RID: 5525 RVA: 0x0005577C File Offset: 0x0005397C
		StyleBackgroundRepeat IStyle.backgroundRepeat
		{
			get
			{
				return base.GetStyleBackgroundRepeat(StylePropertyId.BackgroundRepeat);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundRepeat, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x000557B0 File Offset: 0x000539B0
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x000557D0 File Offset: 0x000539D0
		StyleColor IStyle.borderBottomColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderBottomColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00055804 File Offset: 0x00053A04
		// (set) Token: 0x06001599 RID: 5529 RVA: 0x00055824 File Offset: 0x00053A24
		StyleLength IStyle.borderBottomLeftRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderBottomLeftRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomLeftRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x00055858 File Offset: 0x00053A58
		// (set) Token: 0x0600159B RID: 5531 RVA: 0x00055878 File Offset: 0x00053A78
		StyleLength IStyle.borderBottomRightRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderBottomRightRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomRightRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x000558AC File Offset: 0x00053AAC
		// (set) Token: 0x0600159D RID: 5533 RVA: 0x000558CC File Offset: 0x00053ACC
		StyleFloat IStyle.borderBottomWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderBottomWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderBottomWidth = this.ve.computedStyle.borderBottomWidth;
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x00055920 File Offset: 0x00053B20
		// (set) Token: 0x0600159F RID: 5535 RVA: 0x00055940 File Offset: 0x00053B40
		StyleColor IStyle.borderLeftColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderLeftColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderLeftColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x00055974 File Offset: 0x00053B74
		// (set) Token: 0x060015A1 RID: 5537 RVA: 0x00055994 File Offset: 0x00053B94
		StyleFloat IStyle.borderLeftWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderLeftWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderLeftWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderLeftWidth = this.ve.computedStyle.borderLeftWidth;
				}
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x000559E8 File Offset: 0x00053BE8
		// (set) Token: 0x060015A3 RID: 5539 RVA: 0x00055A08 File Offset: 0x00053C08
		StyleColor IStyle.borderRightColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderRightColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderRightColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00055A3C File Offset: 0x00053C3C
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x00055A5C File Offset: 0x00053C5C
		StyleFloat IStyle.borderRightWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderRightWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderRightWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderRightWidth = this.ve.computedStyle.borderRightWidth;
				}
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00055AB0 File Offset: 0x00053CB0
		// (set) Token: 0x060015A7 RID: 5543 RVA: 0x00055AD0 File Offset: 0x00053CD0
		StyleColor IStyle.borderTopColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderTopColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00055B04 File Offset: 0x00053D04
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x00055B24 File Offset: 0x00053D24
		StyleLength IStyle.borderTopLeftRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderTopLeftRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopLeftRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00055B58 File Offset: 0x00053D58
		// (set) Token: 0x060015AB RID: 5547 RVA: 0x00055B78 File Offset: 0x00053D78
		StyleLength IStyle.borderTopRightRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderTopRightRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopRightRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x00055BAC File Offset: 0x00053DAC
		// (set) Token: 0x060015AD RID: 5549 RVA: 0x00055BCC File Offset: 0x00053DCC
		StyleFloat IStyle.borderTopWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderTopWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderTopWidth = this.ve.computedStyle.borderTopWidth;
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x00055C20 File Offset: 0x00053E20
		// (set) Token: 0x060015AF RID: 5551 RVA: 0x00055C40 File Offset: 0x00053E40
		StyleLength IStyle.bottom
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Bottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Bottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Bottom = this.ve.computedStyle.bottom.ToYogaValue();
				}
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00055C94 File Offset: 0x00053E94
		// (set) Token: 0x060015B1 RID: 5553 RVA: 0x00055CB4 File Offset: 0x00053EB4
		StyleColor IStyle.color
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.Color);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Color, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00055CE8 File Offset: 0x00053EE8
		// (set) Token: 0x060015B3 RID: 5555 RVA: 0x00055D1C File Offset: 0x00053F1C
		StyleEnum<DisplayStyle> IStyle.display
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Display);
				return new StyleEnum<DisplayStyle>((DisplayStyle)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<DisplayStyle>(StylePropertyId.Display, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
					this.ve.yogaNode.Display = (YogaDisplay)this.ve.computedStyle.display;
				}
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x00055D70 File Offset: 0x00053F70
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x00055D90 File Offset: 0x00053F90
		StyleLength IStyle.flexBasis
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.FlexBasis);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FlexBasis, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexBasis = this.ve.computedStyle.flexBasis.ToYogaValue();
				}
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00055DE4 File Offset: 0x00053FE4
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00055E18 File Offset: 0x00054018
		StyleEnum<FlexDirection> IStyle.flexDirection
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.FlexDirection);
				return new StyleEnum<FlexDirection>((FlexDirection)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<FlexDirection>(StylePropertyId.FlexDirection, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexDirection = (YogaFlexDirection)this.ve.computedStyle.flexDirection;
				}
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x00055E68 File Offset: 0x00054068
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x00055E88 File Offset: 0x00054088
		StyleFloat IStyle.flexGrow
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.FlexGrow);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FlexGrow, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexGrow = this.ve.computedStyle.flexGrow;
				}
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x00055ED8 File Offset: 0x000540D8
		// (set) Token: 0x060015BB RID: 5563 RVA: 0x00055EF8 File Offset: 0x000540F8
		StyleFloat IStyle.flexShrink
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.FlexShrink);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FlexShrink, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexShrink = this.ve.computedStyle.flexShrink;
				}
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00055F48 File Offset: 0x00054148
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x00055F7C File Offset: 0x0005417C
		StyleEnum<Wrap> IStyle.flexWrap
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.FlexWrap);
				return new StyleEnum<Wrap>((Wrap)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Wrap>(StylePropertyId.FlexWrap, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Wrap = (YogaWrap)this.ve.computedStyle.flexWrap;
				}
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x00055FCC File Offset: 0x000541CC
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x00055FEC File Offset: 0x000541EC
		StyleLength IStyle.fontSize
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.FontSize);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FontSize, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x00056020 File Offset: 0x00054220
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x00056040 File Offset: 0x00054240
		StyleLength IStyle.height
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Height);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Height, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Height = this.ve.computedStyle.height.ToYogaValue();
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x00056094 File Offset: 0x00054294
		// (set) Token: 0x060015C3 RID: 5571 RVA: 0x000560C8 File Offset: 0x000542C8
		StyleEnum<Justify> IStyle.justifyContent
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.JustifyContent);
				return new StyleEnum<Justify>((Justify)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Justify>(StylePropertyId.JustifyContent, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.JustifyContent = (YogaJustify)this.ve.computedStyle.justifyContent;
				}
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00056118 File Offset: 0x00054318
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x00056138 File Offset: 0x00054338
		StyleLength IStyle.left
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Left);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Left, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Left = this.ve.computedStyle.left.ToYogaValue();
				}
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x0005618C File Offset: 0x0005438C
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x000561AC File Offset: 0x000543AC
		StyleLength IStyle.letterSpacing
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.LetterSpacing);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.LetterSpacing, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x000561E0 File Offset: 0x000543E0
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00056200 File Offset: 0x00054400
		StyleLength IStyle.marginBottom
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginBottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginBottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginBottom = this.ve.computedStyle.marginBottom.ToYogaValue();
				}
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00056254 File Offset: 0x00054454
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x00056274 File Offset: 0x00054474
		StyleLength IStyle.marginLeft
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginLeft);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginLeft, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginLeft = this.ve.computedStyle.marginLeft.ToYogaValue();
				}
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x000562C8 File Offset: 0x000544C8
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x000562E8 File Offset: 0x000544E8
		StyleLength IStyle.marginRight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginRight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginRight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginRight = this.ve.computedStyle.marginRight.ToYogaValue();
				}
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0005633C File Offset: 0x0005453C
		// (set) Token: 0x060015CF RID: 5583 RVA: 0x0005635C File Offset: 0x0005455C
		StyleLength IStyle.marginTop
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginTop);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginTop, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginTop = this.ve.computedStyle.marginTop.ToYogaValue();
				}
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x000563B0 File Offset: 0x000545B0
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x000563D0 File Offset: 0x000545D0
		StyleLength IStyle.maxHeight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MaxHeight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MaxHeight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MaxHeight = this.ve.computedStyle.maxHeight.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00056424 File Offset: 0x00054624
		// (set) Token: 0x060015D3 RID: 5587 RVA: 0x00056444 File Offset: 0x00054644
		StyleLength IStyle.maxWidth
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MaxWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MaxWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MaxWidth = this.ve.computedStyle.maxWidth.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00056498 File Offset: 0x00054698
		// (set) Token: 0x060015D5 RID: 5589 RVA: 0x000564B8 File Offset: 0x000546B8
		StyleLength IStyle.minHeight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MinHeight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MinHeight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MinHeight = this.ve.computedStyle.minHeight.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x0005650C File Offset: 0x0005470C
		// (set) Token: 0x060015D7 RID: 5591 RVA: 0x0005652C File Offset: 0x0005472C
		StyleLength IStyle.minWidth
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MinWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MinWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MinWidth = this.ve.computedStyle.minWidth.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x00056580 File Offset: 0x00054780
		// (set) Token: 0x060015D9 RID: 5593 RVA: 0x000565A0 File Offset: 0x000547A0
		StyleFloat IStyle.opacity
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.Opacity);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Opacity, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Opacity);
				}
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x000565D4 File Offset: 0x000547D4
		// (set) Token: 0x060015DB RID: 5595 RVA: 0x00056608 File Offset: 0x00054808
		StyleEnum<Overflow> IStyle.overflow
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Overflow);
				return new StyleEnum<Overflow>((Overflow)styleInt.value, styleInt.keyword);
			}
			set
			{
				StyleEnum<OverflowInternal> styleEnum = new StyleEnum<OverflowInternal>((OverflowInternal)value.value, value.keyword);
				bool flag = this.SetStyleValue<OverflowInternal>(StylePropertyId.Overflow, styleEnum);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Overflow);
					this.ve.yogaNode.Overflow = (YogaOverflow)this.ve.computedStyle.overflow;
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x0005666C File Offset: 0x0005486C
		// (set) Token: 0x060015DD RID: 5597 RVA: 0x0005668C File Offset: 0x0005488C
		StyleLength IStyle.paddingBottom
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingBottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingBottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingBottom = this.ve.computedStyle.paddingBottom.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x000566E0 File Offset: 0x000548E0
		// (set) Token: 0x060015DF RID: 5599 RVA: 0x00056700 File Offset: 0x00054900
		StyleLength IStyle.paddingLeft
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingLeft);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingLeft, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingLeft = this.ve.computedStyle.paddingLeft.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x00056754 File Offset: 0x00054954
		// (set) Token: 0x060015E1 RID: 5601 RVA: 0x00056774 File Offset: 0x00054974
		StyleLength IStyle.paddingRight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingRight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingRight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingRight = this.ve.computedStyle.paddingRight.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x000567C8 File Offset: 0x000549C8
		// (set) Token: 0x060015E3 RID: 5603 RVA: 0x000567E8 File Offset: 0x000549E8
		StyleLength IStyle.paddingTop
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingTop);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingTop, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingTop = this.ve.computedStyle.paddingTop.ToYogaValue();
				}
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x0005683C File Offset: 0x00054A3C
		// (set) Token: 0x060015E5 RID: 5605 RVA: 0x00056870 File Offset: 0x00054A70
		StyleEnum<Position> IStyle.position
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Position);
				return new StyleEnum<Position>((Position)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Position>(StylePropertyId.Position, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PositionType = (YogaPositionType)this.ve.computedStyle.position;
				}
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x000568C0 File Offset: 0x00054AC0
		// (set) Token: 0x060015E7 RID: 5607 RVA: 0x000568E0 File Offset: 0x00054AE0
		StyleLength IStyle.right
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Right);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Right, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Right = this.ve.computedStyle.right.ToYogaValue();
				}
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00056934 File Offset: 0x00054B34
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x00056968 File Offset: 0x00054B68
		StyleEnum<TextOverflow> IStyle.textOverflow
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.TextOverflow);
				return new StyleEnum<TextOverflow>((TextOverflow)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<TextOverflow>(StylePropertyId.TextOverflow, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0005699C File Offset: 0x00054B9C
		// (set) Token: 0x060015EB RID: 5611 RVA: 0x000569BC File Offset: 0x00054BBC
		StyleLength IStyle.top
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Top);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Top, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Top = this.ve.computedStyle.top.ToYogaValue();
				}
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x00056A10 File Offset: 0x00054C10
		// (set) Token: 0x060015ED RID: 5613 RVA: 0x00056A30 File Offset: 0x00054C30
		StyleList<TimeValue> IStyle.transitionDelay
		{
			get
			{
				return this.GetStyleList<TimeValue>(StylePropertyId.TransitionDelay);
			}
			set
			{
				bool flag = this.SetStyleValue<TimeValue>(StylePropertyId.TransitionDelay, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.TransitionProperty);
				}
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x00056A64 File Offset: 0x00054C64
		// (set) Token: 0x060015EF RID: 5615 RVA: 0x00056A84 File Offset: 0x00054C84
		StyleList<TimeValue> IStyle.transitionDuration
		{
			get
			{
				return this.GetStyleList<TimeValue>(StylePropertyId.TransitionDuration);
			}
			set
			{
				bool flag = this.SetStyleValue<TimeValue>(StylePropertyId.TransitionDuration, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.TransitionProperty);
				}
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x00056AB8 File Offset: 0x00054CB8
		// (set) Token: 0x060015F1 RID: 5617 RVA: 0x00056AD8 File Offset: 0x00054CD8
		StyleList<StylePropertyName> IStyle.transitionProperty
		{
			get
			{
				return this.GetStyleList<StylePropertyName>(StylePropertyId.TransitionProperty);
			}
			set
			{
				bool flag = this.SetStyleValue<StylePropertyName>(StylePropertyId.TransitionProperty, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.TransitionProperty);
				}
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x00056B0C File Offset: 0x00054D0C
		// (set) Token: 0x060015F3 RID: 5619 RVA: 0x00056B2C File Offset: 0x00054D2C
		StyleList<EasingFunction> IStyle.transitionTimingFunction
		{
			get
			{
				return this.GetStyleList<EasingFunction>(StylePropertyId.TransitionTimingFunction);
			}
			set
			{
				bool flag = this.SetStyleValue<EasingFunction>(StylePropertyId.TransitionTimingFunction, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x00056B5C File Offset: 0x00054D5C
		// (set) Token: 0x060015F5 RID: 5621 RVA: 0x00056B7C File Offset: 0x00054D7C
		StyleColor IStyle.unityBackgroundImageTintColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.UnityBackgroundImageTintColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityBackgroundImageTintColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x00056BB0 File Offset: 0x00054DB0
		// (set) Token: 0x060015F7 RID: 5623 RVA: 0x00056BD0 File Offset: 0x00054DD0
		StyleFont IStyle.unityFont
		{
			get
			{
				return base.GetStyleFont(StylePropertyId.UnityFont);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityFont, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x00056C04 File Offset: 0x00054E04
		// (set) Token: 0x060015F9 RID: 5625 RVA: 0x00056C24 File Offset: 0x00054E24
		StyleFontDefinition IStyle.unityFontDefinition
		{
			get
			{
				return base.GetStyleFontDefinition(StylePropertyId.UnityFontDefinition);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityFontDefinition, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x00056C58 File Offset: 0x00054E58
		// (set) Token: 0x060015FB RID: 5627 RVA: 0x00056C8C File Offset: 0x00054E8C
		StyleEnum<FontStyle> IStyle.unityFontStyleAndWeight
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityFontStyleAndWeight);
				return new StyleEnum<FontStyle>((FontStyle)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<FontStyle>(StylePropertyId.UnityFontStyleAndWeight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x00056CC0 File Offset: 0x00054EC0
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x00056CF4 File Offset: 0x00054EF4
		StyleEnum<OverflowClipBox> IStyle.unityOverflowClipBox
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityOverflowClipBox);
				return new StyleEnum<OverflowClipBox>((OverflowClipBox)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<OverflowClipBox>(StylePropertyId.UnityOverflowClipBox, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x00056D28 File Offset: 0x00054F28
		// (set) Token: 0x060015FF RID: 5631 RVA: 0x00056D48 File Offset: 0x00054F48
		StyleLength IStyle.unityParagraphSpacing
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.UnityParagraphSpacing);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityParagraphSpacing, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x00056D7C File Offset: 0x00054F7C
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x00056D9C File Offset: 0x00054F9C
		StyleInt IStyle.unitySliceBottom
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceBottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceBottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x00056DD0 File Offset: 0x00054FD0
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x00056DF0 File Offset: 0x00054FF0
		StyleInt IStyle.unitySliceLeft
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceLeft);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceLeft, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x00056E24 File Offset: 0x00055024
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x00056E44 File Offset: 0x00055044
		StyleInt IStyle.unitySliceRight
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceRight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceRight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x00056E78 File Offset: 0x00055078
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x00056E98 File Offset: 0x00055098
		StyleFloat IStyle.unitySliceScale
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.UnitySliceScale);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceScale, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00056ECC File Offset: 0x000550CC
		// (set) Token: 0x06001609 RID: 5641 RVA: 0x00056EEC File Offset: 0x000550EC
		StyleInt IStyle.unitySliceTop
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceTop);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceTop, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x00056F20 File Offset: 0x00055120
		// (set) Token: 0x0600160B RID: 5643 RVA: 0x00056F54 File Offset: 0x00055154
		StyleEnum<TextAnchor> IStyle.unityTextAlign
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityTextAlign);
				return new StyleEnum<TextAnchor>((TextAnchor)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<TextAnchor>(StylePropertyId.UnityTextAlign, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x00056F88 File Offset: 0x00055188
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x00056FA8 File Offset: 0x000551A8
		StyleColor IStyle.unityTextOutlineColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.UnityTextOutlineColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityTextOutlineColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x00056FDC File Offset: 0x000551DC
		// (set) Token: 0x0600160F RID: 5647 RVA: 0x00056FFC File Offset: 0x000551FC
		StyleFloat IStyle.unityTextOutlineWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.UnityTextOutlineWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityTextOutlineWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x00057030 File Offset: 0x00055230
		// (set) Token: 0x06001611 RID: 5649 RVA: 0x00057064 File Offset: 0x00055264
		StyleEnum<TextOverflowPosition> IStyle.unityTextOverflowPosition
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityTextOverflowPosition);
				return new StyleEnum<TextOverflowPosition>((TextOverflowPosition)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<TextOverflowPosition>(StylePropertyId.UnityTextOverflowPosition, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x00057098 File Offset: 0x00055298
		// (set) Token: 0x06001613 RID: 5651 RVA: 0x000570CC File Offset: 0x000552CC
		StyleEnum<Visibility> IStyle.visibility
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Visibility);
				return new StyleEnum<Visibility>((Visibility)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Visibility>(StylePropertyId.Visibility, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint | VersionChangeType.Picking);
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x00057100 File Offset: 0x00055300
		// (set) Token: 0x06001615 RID: 5653 RVA: 0x00057134 File Offset: 0x00055334
		StyleEnum<WhiteSpace> IStyle.whiteSpace
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.WhiteSpace);
				return new StyleEnum<WhiteSpace>((WhiteSpace)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<WhiteSpace>(StylePropertyId.WhiteSpace, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x00057164 File Offset: 0x00055364
		// (set) Token: 0x06001617 RID: 5655 RVA: 0x00057184 File Offset: 0x00055384
		StyleLength IStyle.width
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Width);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Width, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Width = this.ve.computedStyle.width.ToYogaValue();
				}
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x000571D8 File Offset: 0x000553D8
		// (set) Token: 0x06001619 RID: 5657 RVA: 0x000571F8 File Offset: 0x000553F8
		StyleLength IStyle.wordSpacing
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.WordSpacing);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.WordSpacing, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x00057229 File Offset: 0x00055429
		// (set) Token: 0x0600161B RID: 5659 RVA: 0x00057231 File Offset: 0x00055431
		private VisualElement ve { get; set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x0005723A File Offset: 0x0005543A
		public InlineStyleAccess.InlineRule inlineRule
		{
			get
			{
				return this.m_InlineRule;
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00057242 File Offset: 0x00055442
		public InlineStyleAccess(VisualElement ve)
		{
			this.ve = ve;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00057254 File Offset: 0x00055454
		protected override void Finalize()
		{
			try
			{
				StyleValue styleValue = default(StyleValue);
				bool flag = base.TryGetStyleValue(StylePropertyId.BackgroundImage, ref styleValue);
				if (flag)
				{
					bool isAllocated = styleValue.resource.IsAllocated;
					if (isAllocated)
					{
						styleValue.resource.Free();
					}
				}
				bool flag2 = base.TryGetStyleValue(StylePropertyId.UnityFont, ref styleValue);
				if (flag2)
				{
					bool isAllocated2 = styleValue.resource.IsAllocated;
					if (isAllocated2)
					{
						styleValue.resource.Free();
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x000572E8 File Offset: 0x000554E8
		public void SetInlineRule(StyleSheet sheet, StyleRule rule)
		{
			this.m_InlineRule.sheet = sheet;
			this.m_InlineRule.rule = rule;
			this.m_InlineRule.propertyIds = StyleSheetCache.GetPropertyIds(rule);
			this.ApplyInlineStyles(this.ve.computedStyle);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00057328 File Offset: 0x00055528
		public bool IsValueSet(StylePropertyId id)
		{
			foreach (StyleValue styleValue in this.m_Values)
			{
				bool flag = styleValue.id == id;
				if (flag)
				{
					return true;
				}
			}
			bool flag2 = this.m_ValuesManaged != null;
			if (flag2)
			{
				foreach (StyleValueManaged styleValueManaged in this.m_ValuesManaged)
				{
					bool flag3 = styleValueManaged.id == id;
					if (flag3)
					{
						return true;
					}
				}
			}
			if (id <= StylePropertyId.Cursor)
			{
				if (id == StylePropertyId.TextShadow)
				{
					return this.m_HasInlineTextShadow;
				}
				if (id == StylePropertyId.Cursor)
				{
					return this.m_HasInlineCursor;
				}
			}
			else
			{
				switch (id)
				{
				case StylePropertyId.Rotate:
					return this.m_HasInlineRotate;
				case StylePropertyId.Scale:
					return this.m_HasInlineScale;
				case StylePropertyId.TransformOrigin:
					return this.m_HasInlineTransformOrigin;
				case StylePropertyId.Translate:
					return this.m_HasInlineTranslate;
				default:
					if (id == StylePropertyId.BackgroundSize)
					{
						return this.m_HasInlineBackgroundSize;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0005748C File Offset: 0x0005568C
		public void ApplyInlineStyles(ref ComputedStyle computedStyle)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			ref ComputedStyle ptr;
			if (parent != null)
			{
				ref ComputedStyle computedStyle2 = ref parent.computedStyle;
				ptr = parent.computedStyle;
			}
			else
			{
				ptr = InitialStyle.Get();
			}
			ref ComputedStyle ptr2 = ref ptr;
			bool flag = this.m_InlineRule.sheet != null;
			if (flag)
			{
				InlineStyleAccess.s_StylePropertyReader.SetInlineContext(this.m_InlineRule.sheet, this.m_InlineRule.rule.properties, this.m_InlineRule.propertyIds, 1f);
				computedStyle.ApplyProperties(InlineStyleAccess.s_StylePropertyReader, ref ptr2);
			}
			foreach (StyleValue styleValue in this.m_Values)
			{
				computedStyle.ApplyStyleValue(styleValue, ref ptr2);
			}
			bool flag2 = this.m_ValuesManaged != null;
			if (flag2)
			{
				foreach (StyleValueManaged styleValueManaged in this.m_ValuesManaged)
				{
					computedStyle.ApplyStyleValueManaged(styleValueManaged, ref ptr2);
				}
			}
			bool flag3 = this.ve.style.cursor.keyword != StyleKeyword.Null;
			if (flag3)
			{
				computedStyle.ApplyStyleCursor(this.ve.style.cursor.value);
			}
			bool flag4 = this.ve.style.textShadow.keyword != StyleKeyword.Null;
			if (flag4)
			{
				computedStyle.ApplyStyleTextShadow(this.ve.style.textShadow.value);
			}
			bool hasInlineTransformOrigin = this.m_HasInlineTransformOrigin;
			if (hasInlineTransformOrigin)
			{
				computedStyle.ApplyStyleTransformOrigin(this.ve.style.transformOrigin.value);
			}
			bool hasInlineTranslate = this.m_HasInlineTranslate;
			if (hasInlineTranslate)
			{
				computedStyle.ApplyStyleTranslate(this.ve.style.translate.value);
			}
			bool hasInlineScale = this.m_HasInlineScale;
			if (hasInlineScale)
			{
				computedStyle.ApplyStyleScale(this.ve.style.scale.value);
			}
			bool hasInlineRotate = this.m_HasInlineRotate;
			if (hasInlineRotate)
			{
				computedStyle.ApplyStyleRotate(this.ve.style.rotate.value);
			}
			bool hasInlineBackgroundSize = this.m_HasInlineBackgroundSize;
			if (hasInlineBackgroundSize)
			{
				computedStyle.ApplyStyleBackgroundSize(this.ve.style.backgroundSize.value);
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001622 RID: 5666 RVA: 0x00057744 File Offset: 0x00055944
		// (set) Token: 0x06001623 RID: 5667 RVA: 0x00057774 File Offset: 0x00055974
		StyleCursor IStyle.cursor
		{
			get
			{
				StyleCursor styleCursor = default(StyleCursor);
				bool flag = this.TryGetInlineCursor(ref styleCursor);
				StyleCursor styleCursor2;
				if (flag)
				{
					styleCursor2 = styleCursor;
				}
				else
				{
					styleCursor2 = StyleKeyword.Null;
				}
				return styleCursor2;
			}
			set
			{
				bool flag = this.SetInlineCursor(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001624 RID: 5668 RVA: 0x000577A0 File Offset: 0x000559A0
		// (set) Token: 0x06001625 RID: 5669 RVA: 0x000577D0 File Offset: 0x000559D0
		StyleTextShadow IStyle.textShadow
		{
			get
			{
				StyleTextShadow styleTextShadow = default(StyleTextShadow);
				bool flag = this.TryGetInlineTextShadow(ref styleTextShadow);
				StyleTextShadow styleTextShadow2;
				if (flag)
				{
					styleTextShadow2 = styleTextShadow;
				}
				else
				{
					styleTextShadow2 = StyleKeyword.Null;
				}
				return styleTextShadow2;
			}
			set
			{
				bool flag = this.SetInlineTextShadow(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001626 RID: 5670 RVA: 0x000577FC File Offset: 0x000559FC
		// (set) Token: 0x06001627 RID: 5671 RVA: 0x0005782C File Offset: 0x00055A2C
		StyleBackgroundSize IStyle.backgroundSize
		{
			get
			{
				StyleBackgroundSize styleBackgroundSize = default(StyleBackgroundSize);
				bool flag = this.TryGetInlineBackgroundSize(ref styleBackgroundSize);
				StyleBackgroundSize styleBackgroundSize2;
				if (flag)
				{
					styleBackgroundSize2 = styleBackgroundSize;
				}
				else
				{
					styleBackgroundSize2 = StyleKeyword.Null;
				}
				return styleBackgroundSize2;
			}
			set
			{
				bool flag = this.SetInlineBackgroundSize(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00057858 File Offset: 0x00055A58
		private StyleList<T> GetStyleList<T>(StylePropertyId id)
		{
			StyleValueManaged styleValueManaged = default(StyleValueManaged);
			bool flag = this.TryGetStyleValueManaged(id, ref styleValueManaged);
			StyleList<T> styleList;
			if (flag)
			{
				styleList = new StyleList<T>(styleValueManaged.value as List<T>, styleValueManaged.keyword);
			}
			else
			{
				styleList = StyleKeyword.Null;
			}
			return styleList;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000578A0 File Offset: 0x00055AA0
		private void SetStyleValueManaged(StyleValueManaged value)
		{
			bool flag = this.m_ValuesManaged == null;
			if (flag)
			{
				this.m_ValuesManaged = new List<StyleValueManaged>();
			}
			for (int i = 0; i < this.m_ValuesManaged.Count; i++)
			{
				bool flag2 = this.m_ValuesManaged[i].id == value.id;
				if (flag2)
				{
					bool flag3 = value.keyword == StyleKeyword.Null;
					if (flag3)
					{
						this.m_ValuesManaged.RemoveAt(i);
					}
					else
					{
						this.m_ValuesManaged[i] = value;
					}
					return;
				}
			}
			this.m_ValuesManaged.Add(value);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00057940 File Offset: 0x00055B40
		private bool TryGetStyleValueManaged(StylePropertyId id, ref StyleValueManaged value)
		{
			value.id = StylePropertyId.Unknown;
			bool flag = this.m_ValuesManaged == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				foreach (StyleValueManaged styleValueManaged in this.m_ValuesManaged)
				{
					bool flag3 = styleValueManaged.id == id;
					if (flag3)
					{
						value = styleValueManaged;
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x000579CC File Offset: 0x00055BCC
		// (set) Token: 0x0600162C RID: 5676 RVA: 0x000579FC File Offset: 0x00055BFC
		StyleTransformOrigin IStyle.transformOrigin
		{
			get
			{
				StyleTransformOrigin styleTransformOrigin = default(StyleTransformOrigin);
				bool flag = this.TryGetInlineTransformOrigin(ref styleTransformOrigin);
				StyleTransformOrigin styleTransformOrigin2;
				if (flag)
				{
					styleTransformOrigin2 = styleTransformOrigin;
				}
				else
				{
					styleTransformOrigin2 = StyleKeyword.Null;
				}
				return styleTransformOrigin2;
			}
			set
			{
				bool flag = this.SetInlineTransformOrigin(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x00057A28 File Offset: 0x00055C28
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x00057A58 File Offset: 0x00055C58
		StyleTranslate IStyle.translate
		{
			get
			{
				StyleTranslate styleTranslate = default(StyleTranslate);
				bool flag = this.TryGetInlineTranslate(ref styleTranslate);
				StyleTranslate styleTranslate2;
				if (flag)
				{
					styleTranslate2 = styleTranslate;
				}
				else
				{
					styleTranslate2 = StyleKeyword.Null;
				}
				return styleTranslate2;
			}
			set
			{
				bool flag = this.SetInlineTranslate(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x00057A84 File Offset: 0x00055C84
		// (set) Token: 0x06001630 RID: 5680 RVA: 0x00057AB4 File Offset: 0x00055CB4
		StyleRotate IStyle.rotate
		{
			get
			{
				StyleRotate styleRotate = default(StyleRotate);
				bool flag = this.TryGetInlineRotate(ref styleRotate);
				StyleRotate styleRotate2;
				if (flag)
				{
					styleRotate2 = styleRotate;
				}
				else
				{
					styleRotate2 = StyleKeyword.Null;
				}
				return styleRotate2;
			}
			set
			{
				bool flag = this.SetInlineRotate(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00057AE0 File Offset: 0x00055CE0
		// (set) Token: 0x06001632 RID: 5682 RVA: 0x00057B10 File Offset: 0x00055D10
		StyleScale IStyle.scale
		{
			get
			{
				StyleScale styleScale = default(StyleScale);
				bool flag = this.TryGetInlineScale(ref styleScale);
				StyleScale styleScale2;
				if (flag)
				{
					styleScale2 = styleScale;
				}
				else
				{
					styleScale2 = StyleKeyword.Null;
				}
				return styleScale2;
			}
			set
			{
				bool flag = this.SetInlineScale(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00057B3C File Offset: 0x00055D3C
		private bool SetStyleValue(StylePropertyId id, StyleBackgroundPosition inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.position == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.position = inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00057BFC File Offset: 0x00055DFC
		private bool SetStyleValue(StylePropertyId id, StyleBackgroundRepeat inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.repeat == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.repeat = inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00057CBC File Offset: 0x00055EBC
		private bool SetStyleValue(StylePropertyId id, StyleLength inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.length == inlineValue.ToLength() && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.length = inlineValue.ToLength();
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00057D78 File Offset: 0x00055F78
		private bool SetStyleValue(StylePropertyId id, StyleFloat inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.number == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.number = inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00057E30 File Offset: 0x00056030
		private bool SetStyleValue(StylePropertyId id, StyleInt inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.number == (float)inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.number = (float)inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00057EEC File Offset: 0x000560EC
		private bool SetStyleValue(StylePropertyId id, StyleColor inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.color == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.color = inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00057FAC File Offset: 0x000561AC
		private bool SetStyleValue<T>(StylePropertyId id, StyleEnum<T> inlineValue) where T : struct, IConvertible
		{
			StyleValue styleValue = default(StyleValue);
			int num = UnsafeUtility.EnumToInt<T>(inlineValue.value);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.number == (float)num && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.number = (float)num;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0005806C File Offset: 0x0005626C
		private bool SetStyleValue(StylePropertyId id, StyleBackground inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				VectorImage vectorImage = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as VectorImage) : null);
				Sprite sprite = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Sprite) : null);
				Texture2D texture2D = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Texture2D) : null);
				RenderTexture renderTexture = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as RenderTexture) : null);
				bool flag2 = vectorImage == inlineValue.value.vectorImage && texture2D == inlineValue.value.texture && sprite == inlineValue.value.sprite && renderTexture == inlineValue.value.renderTexture && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
				bool isAllocated = styleValue.resource.IsAllocated;
				if (isAllocated)
				{
					styleValue.resource.Free();
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.value.vectorImage != null;
			if (flag4)
			{
				styleValue.resource = GCHandle.Alloc(inlineValue.value.vectorImage);
			}
			else
			{
				bool flag5 = inlineValue.value.sprite != null;
				if (flag5)
				{
					styleValue.resource = GCHandle.Alloc(inlineValue.value.sprite);
				}
				else
				{
					bool flag6 = inlineValue.value.texture != null;
					if (flag6)
					{
						styleValue.resource = GCHandle.Alloc(inlineValue.value.texture);
					}
					else
					{
						bool flag7 = inlineValue.value.renderTexture != null;
						if (flag7)
						{
							styleValue.resource = GCHandle.Alloc(inlineValue.value.renderTexture);
						}
						else
						{
							styleValue.resource = default(GCHandle);
						}
					}
				}
			}
			base.SetStyleValue(styleValue);
			bool flag8 = inlineValue.keyword == StyleKeyword.Null;
			bool flag9;
			if (flag8)
			{
				flag9 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag9 = true;
			}
			return flag9;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00058324 File Offset: 0x00056524
		private bool SetStyleValue(StylePropertyId id, StyleFontDefinition inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				Font font = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Font) : null);
				FontAsset fontAsset = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as FontAsset) : null);
				bool flag2 = font == inlineValue.value.font && fontAsset == inlineValue.value.fontAsset && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
				bool isAllocated = styleValue.resource.IsAllocated;
				if (isAllocated)
				{
					styleValue.resource.Free();
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.value.font != null;
			if (flag4)
			{
				styleValue.resource = GCHandle.Alloc(inlineValue.value.font);
			}
			else
			{
				bool flag5 = inlineValue.value.fontAsset != null;
				if (flag5)
				{
					styleValue.resource = GCHandle.Alloc(inlineValue.value.fontAsset);
				}
				else
				{
					styleValue.resource = default(GCHandle);
				}
			}
			base.SetStyleValue(styleValue);
			bool flag6 = inlineValue.keyword == StyleKeyword.Null;
			bool flag7;
			if (flag6)
			{
				flag7 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag7 = true;
			}
			return flag7;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000584E8 File Offset: 0x000566E8
		private bool SetStyleValue(StylePropertyId id, StyleFont inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				Font font = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Font) : null);
				bool flag2 = font == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
				bool isAllocated = styleValue.resource.IsAllocated;
				if (isAllocated)
				{
					styleValue.resource.Free();
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.resource = ((inlineValue.value != null) ? GCHandle.Alloc(inlineValue.value) : default(GCHandle));
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0005860C File Offset: 0x0005680C
		private bool SetStyleValue<T>(StylePropertyId id, StyleList<T> inlineValue)
		{
			StyleValueManaged styleValueManaged = default(StyleValueManaged);
			bool flag = this.TryGetStyleValueManaged(id, ref styleValueManaged);
			if (flag)
			{
				bool flag2 = styleValueManaged.keyword == inlineValue.keyword;
				if (flag2)
				{
					bool flag3 = styleValueManaged.value == null && inlineValue.value == null;
					if (flag3)
					{
						return false;
					}
					List<T> list = styleValueManaged.value as List<T>;
					bool flag4 = list != null && inlineValue.value != null && list.SequenceEqual<T>(inlineValue.value);
					if (flag4)
					{
						return false;
					}
				}
			}
			else
			{
				bool flag5 = inlineValue.keyword == StyleKeyword.Null;
				if (flag5)
				{
					return false;
				}
			}
			styleValueManaged.id = id;
			styleValueManaged.keyword = inlineValue.keyword;
			bool flag6 = inlineValue.value != null;
			if (flag6)
			{
				bool flag7 = styleValueManaged.value == null;
				if (flag7)
				{
					styleValueManaged.value = new List<T>(inlineValue.value);
				}
				else
				{
					List<T> list2 = (List<T>)styleValueManaged.value;
					list2.Clear();
					list2.AddRange(inlineValue.value);
				}
			}
			else
			{
				styleValueManaged.value = null;
			}
			this.SetStyleValueManaged(styleValueManaged);
			bool flag8 = inlineValue.keyword == StyleKeyword.Null;
			bool flag9;
			if (flag8)
			{
				flag9 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValueManaged);
				flag9 = true;
			}
			return flag9;
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0005876C File Offset: 0x0005696C
		private bool SetInlineCursor(StyleCursor inlineValue)
		{
			StyleCursor styleCursor = default(StyleCursor);
			bool flag = this.TryGetInlineCursor(ref styleCursor);
			if (flag)
			{
				bool flag2 = styleCursor.value == inlineValue.value && styleCursor.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleCursor.value = inlineValue.value;
			styleCursor.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineCursor = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Cursor);
			}
			else
			{
				this.m_InlineCursor = styleCursor;
				this.m_HasInlineCursor = true;
				this.ApplyStyleCursor(styleCursor);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00058838 File Offset: 0x00056A38
		private void ApplyStyleCursor(StyleCursor cursor)
		{
			this.ve.computedStyle.ApplyStyleCursor(cursor.value);
			BaseVisualElementPanel elementPanel = this.ve.elementPanel;
			bool flag = ((elementPanel != null) ? elementPanel.GetTopElementUnderPointer(PointerId.mousePointerId) : null) == this.ve;
			if (flag)
			{
				this.ve.elementPanel.cursorManager.SetCursor(cursor.value);
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000588A4 File Offset: 0x00056AA4
		private bool SetInlineTextShadow(StyleTextShadow inlineValue)
		{
			StyleTextShadow styleTextShadow = default(StyleTextShadow);
			bool flag = this.TryGetInlineTextShadow(ref styleTextShadow);
			if (flag)
			{
				bool flag2 = styleTextShadow.value == inlineValue.value && styleTextShadow.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleTextShadow.value = inlineValue.value;
			styleTextShadow.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineTextShadow = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.TextShadow);
			}
			else
			{
				this.m_InlineTextShadow = styleTextShadow;
				this.m_HasInlineTextShadow = true;
				this.ApplyStyleTextShadow(styleTextShadow);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00058970 File Offset: 0x00056B70
		private void ApplyStyleTextShadow(StyleTextShadow textShadow)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.TextShadow, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineTextShadow(this.ve, this.ve.computedStyle, textShadow, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.TextShadow);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleTextShadow(textShadow.value);
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00058A38 File Offset: 0x00056C38
		private bool SetInlineTransformOrigin(StyleTransformOrigin inlineValue)
		{
			StyleTransformOrigin styleTransformOrigin = default(StyleTransformOrigin);
			bool flag = this.TryGetInlineTransformOrigin(ref styleTransformOrigin);
			if (flag)
			{
				bool flag2 = styleTransformOrigin.value == inlineValue.value && styleTransformOrigin.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineTransformOrigin = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.TransformOrigin);
			}
			else
			{
				this.m_InlineTransformOrigin = inlineValue;
				this.m_HasInlineTransformOrigin = true;
				this.ApplyStyleTransformOrigin(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00058AE4 File Offset: 0x00056CE4
		private void ApplyStyleTransformOrigin(StyleTransformOrigin transformOrigin)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.TransformOrigin, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineTransformOrigin(this.ve, this.ve.computedStyle, transformOrigin, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.TransformOrigin);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleTransformOrigin(transformOrigin.value);
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00058BAC File Offset: 0x00056DAC
		private bool SetInlineTranslate(StyleTranslate inlineValue)
		{
			StyleTranslate styleTranslate = default(StyleTranslate);
			bool flag = this.TryGetInlineTranslate(ref styleTranslate);
			if (flag)
			{
				bool flag2 = styleTranslate.value == inlineValue.value && styleTranslate.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineTranslate = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Translate);
			}
			else
			{
				this.m_InlineTranslateOperation = inlineValue;
				this.m_HasInlineTranslate = true;
				this.ApplyStyleTranslate(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00058C58 File Offset: 0x00056E58
		private void ApplyStyleTranslate(StyleTranslate translate)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.Translate, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineTranslate(this.ve, this.ve.computedStyle, translate, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.Translate);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleTranslate(translate.value);
			}
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00058D20 File Offset: 0x00056F20
		private bool SetInlineScale(StyleScale inlineValue)
		{
			StyleScale styleScale = default(StyleScale);
			bool flag = this.TryGetInlineScale(ref styleScale);
			if (flag)
			{
				bool flag2 = styleScale.value == inlineValue.value && styleScale.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineScale = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Scale);
			}
			else
			{
				this.m_InlineScale = inlineValue;
				this.m_HasInlineScale = true;
				this.ApplyStyleScale(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00058DCC File Offset: 0x00056FCC
		private void ApplyStyleScale(StyleScale scale)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.Scale, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineScale(this.ve, this.ve.computedStyle, scale, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.Scale);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleScale(scale.value);
			}
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x00058E94 File Offset: 0x00057094
		private bool SetInlineRotate(StyleRotate inlineValue)
		{
			StyleRotate styleRotate = default(StyleRotate);
			bool flag = this.TryGetInlineRotate(ref styleRotate);
			if (flag)
			{
				bool flag2 = styleRotate.value == inlineValue.value && styleRotate.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineRotate = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Rotate);
			}
			else
			{
				this.m_InlineRotateOperation = inlineValue;
				this.m_HasInlineRotate = true;
				this.ApplyStyleRotate(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00058F40 File Offset: 0x00057140
		private void ApplyStyleRotate(StyleRotate rotate)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			if (parent != null)
			{
				ref ComputedStyle computedStyle = ref parent.computedStyle;
				ref ComputedStyle computedStyle2 = ref parent.computedStyle;
			}
			else
			{
				InitialStyle.Get();
			}
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.Rotate, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineRotate(this.ve, this.ve.computedStyle, rotate, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.Rotate);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleRotate(rotate.value);
			}
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0005903C File Offset: 0x0005723C
		private bool SetInlineBackgroundSize(StyleBackgroundSize inlineValue)
		{
			StyleBackgroundSize styleBackgroundSize = default(StyleBackgroundSize);
			bool flag = this.TryGetInlineBackgroundSize(ref styleBackgroundSize);
			if (flag)
			{
				bool flag2 = styleBackgroundSize.value == inlineValue.value && styleBackgroundSize.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineBackgroundSize = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.BackgroundSize);
			}
			else
			{
				this.m_InlineBackgroundSize = inlineValue;
				this.m_HasInlineBackgroundSize = true;
				this.ApplyStyleBackgroundSize(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000590E8 File Offset: 0x000572E8
		private void ApplyStyleBackgroundSize(StyleBackgroundSize backgroundSize)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.BackgroundSize, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineBackgroundSize(this.ve, this.ve.computedStyle, backgroundSize, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.TransformOrigin);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleBackgroundSize(backgroundSize.value);
			}
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000591B0 File Offset: 0x000573B0
		private void ApplyStyleValue(StyleValue value)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			ref ComputedStyle ptr;
			if (parent != null)
			{
				ref ComputedStyle computedStyle = ref parent.computedStyle;
				ptr = parent.computedStyle;
			}
			else
			{
				ptr = InitialStyle.Get();
			}
			ref ComputedStyle ptr2 = ref ptr;
			bool flag = false;
			bool flag2 = StylePropertyUtil.IsAnimatable(value.id);
			if (flag2)
			{
				ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
				ComputedTransitionProperty computedTransitionProperty;
				bool flag3 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(value.id, out computedTransitionProperty);
				if (flag3)
				{
					flag = ComputedStyle.StartAnimationInline(this.ve, value.id, this.ve.computedStyle, value, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
				}
				else
				{
					this.ve.styleAnimation.CancelAnimation(value.id);
				}
			}
			bool flag4 = !flag;
			if (flag4)
			{
				this.ve.computedStyle.ApplyStyleValue(value, ref ptr2);
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000592C4 File Offset: 0x000574C4
		private void ApplyStyleValue(StyleValueManaged value)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			ref ComputedStyle ptr;
			if (parent != null)
			{
				ref ComputedStyle computedStyle = ref parent.computedStyle;
				ptr = parent.computedStyle;
			}
			else
			{
				ptr = InitialStyle.Get();
			}
			ref ComputedStyle ptr2 = ref ptr;
			this.ve.computedStyle.ApplyStyleValueManaged(value, ref ptr2);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00059314 File Offset: 0x00057514
		private bool RemoveInlineStyle(StylePropertyId id)
		{
			long matchingRulesHash = this.ve.computedStyle.matchingRulesHash;
			bool flag = matchingRulesHash == 0L;
			bool flag2;
			if (flag)
			{
				this.ApplyFromComputedStyle(id, InitialStyle.Get());
				flag2 = true;
			}
			else
			{
				ComputedStyle computedStyle;
				bool flag3 = StyleCache.TryGetValue(matchingRulesHash, out computedStyle);
				if (flag3)
				{
					this.ApplyFromComputedStyle(id, ref computedStyle);
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00059370 File Offset: 0x00057570
		private void ApplyFromComputedStyle(StylePropertyId id, ref ComputedStyle newStyle)
		{
			bool flag = false;
			bool flag2 = StylePropertyUtil.IsAnimatable(id);
			if (flag2)
			{
				ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
				ComputedTransitionProperty computedTransitionProperty;
				bool flag3 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(id, out computedTransitionProperty);
				if (flag3)
				{
					flag = ComputedStyle.StartAnimation(this.ve, id, this.ve.computedStyle, ref newStyle, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
				}
				else
				{
					this.ve.styleAnimation.CancelAnimation(id);
				}
			}
			bool flag4 = !flag;
			if (flag4)
			{
				this.ve.computedStyle.ApplyFromComputedStyle(id, ref newStyle);
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0005943C File Offset: 0x0005763C
		public bool TryGetInlineCursor(ref StyleCursor value)
		{
			bool hasInlineCursor = this.m_HasInlineCursor;
			bool flag;
			if (hasInlineCursor)
			{
				value = this.m_InlineCursor;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0005946C File Offset: 0x0005766C
		public bool TryGetInlineTextShadow(ref StyleTextShadow value)
		{
			bool hasInlineTextShadow = this.m_HasInlineTextShadow;
			bool flag;
			if (hasInlineTextShadow)
			{
				value = this.m_InlineTextShadow;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0005949C File Offset: 0x0005769C
		public bool TryGetInlineTransformOrigin(ref StyleTransformOrigin value)
		{
			bool hasInlineTransformOrigin = this.m_HasInlineTransformOrigin;
			bool flag;
			if (hasInlineTransformOrigin)
			{
				value = this.m_InlineTransformOrigin;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000594CC File Offset: 0x000576CC
		public bool TryGetInlineTranslate(ref StyleTranslate value)
		{
			bool hasInlineTranslate = this.m_HasInlineTranslate;
			bool flag;
			if (hasInlineTranslate)
			{
				value = this.m_InlineTranslateOperation;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000594FC File Offset: 0x000576FC
		public bool TryGetInlineRotate(ref StyleRotate value)
		{
			bool hasInlineRotate = this.m_HasInlineRotate;
			bool flag;
			if (hasInlineRotate)
			{
				value = this.m_InlineRotateOperation;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0005952C File Offset: 0x0005772C
		public bool TryGetInlineScale(ref StyleScale value)
		{
			bool hasInlineScale = this.m_HasInlineScale;
			bool flag;
			if (hasInlineScale)
			{
				value = this.m_InlineScale;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0005955C File Offset: 0x0005775C
		public bool TryGetInlineBackgroundSize(ref StyleBackgroundSize value)
		{
			bool hasInlineBackgroundSize = this.m_HasInlineBackgroundSize;
			bool flag;
			if (hasInlineBackgroundSize)
			{
				value = this.m_InlineBackgroundSize;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x0005958C File Offset: 0x0005778C
		// (set) Token: 0x06001658 RID: 5720 RVA: 0x0005960C File Offset: 0x0005780C
		StyleEnum<ScaleMode> IStyle.unityBackgroundScaleMode
		{
			get
			{
				bool flag;
				return new StyleEnum<ScaleMode>(BackgroundPropertyHelper.ResolveUnityBackgroundScaleMode(this.ve.style.backgroundPositionX.value, this.ve.style.backgroundPositionY.value, this.ve.style.backgroundRepeat.value, this.ve.style.backgroundSize.value, out flag));
			}
			set
			{
				this.ve.style.backgroundPositionX = BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(value.value);
				this.ve.style.backgroundPositionY = BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(value.value);
				this.ve.style.backgroundRepeat = BackgroundPropertyHelper.ConvertScaleModeToBackgroundRepeat(value.value);
				this.ve.style.backgroundSize = BackgroundPropertyHelper.ConvertScaleModeToBackgroundSize(value.value);
			}
		}

		// Token: 0x040009DB RID: 2523
		private static StylePropertyReader s_StylePropertyReader = new StylePropertyReader();

		// Token: 0x040009DC RID: 2524
		private List<StyleValueManaged> m_ValuesManaged;

		// Token: 0x040009DE RID: 2526
		private bool m_HasInlineCursor;

		// Token: 0x040009DF RID: 2527
		private StyleCursor m_InlineCursor;

		// Token: 0x040009E0 RID: 2528
		private bool m_HasInlineTextShadow;

		// Token: 0x040009E1 RID: 2529
		private StyleTextShadow m_InlineTextShadow;

		// Token: 0x040009E2 RID: 2530
		private bool m_HasInlineTransformOrigin;

		// Token: 0x040009E3 RID: 2531
		private StyleTransformOrigin m_InlineTransformOrigin;

		// Token: 0x040009E4 RID: 2532
		private bool m_HasInlineTranslate;

		// Token: 0x040009E5 RID: 2533
		private StyleTranslate m_InlineTranslateOperation;

		// Token: 0x040009E6 RID: 2534
		private bool m_HasInlineRotate;

		// Token: 0x040009E7 RID: 2535
		private StyleRotate m_InlineRotateOperation;

		// Token: 0x040009E8 RID: 2536
		private bool m_HasInlineScale;

		// Token: 0x040009E9 RID: 2537
		private StyleScale m_InlineScale;

		// Token: 0x040009EA RID: 2538
		private bool m_HasInlineBackgroundSize;

		// Token: 0x040009EB RID: 2539
		public StyleBackgroundSize m_InlineBackgroundSize;

		// Token: 0x040009EC RID: 2540
		private InlineStyleAccess.InlineRule m_InlineRule;

		// Token: 0x020002D2 RID: 722
		internal struct InlineRule
		{
			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x0600165A RID: 5722 RVA: 0x000596AE File Offset: 0x000578AE
			public StyleProperty[] properties
			{
				get
				{
					return this.rule.properties;
				}
			}

			// Token: 0x040009ED RID: 2541
			public StyleSheet sheet;

			// Token: 0x040009EE RID: 2542
			public StyleRule rule;

			// Token: 0x040009EF RID: 2543
			public StylePropertyId[] propertyIds;
		}
	}
}
