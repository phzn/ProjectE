using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004BD RID: 1213
	public struct StyleValues
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x0009E988 File Offset: 0x0009CB88
		// (set) Token: 0x060025A0 RID: 9632 RVA: 0x0009E9B2 File Offset: 0x0009CBB2
		public float top
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Top).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Top, value);
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x0009E9C4 File Offset: 0x0009CBC4
		// (set) Token: 0x060025A2 RID: 9634 RVA: 0x0009E9EE File Offset: 0x0009CBEE
		public float left
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Left).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Left, value);
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x0009EA00 File Offset: 0x0009CC00
		// (set) Token: 0x060025A4 RID: 9636 RVA: 0x0009EA2A File Offset: 0x0009CC2A
		public float width
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Width).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Width, value);
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x0009EA3C File Offset: 0x0009CC3C
		// (set) Token: 0x060025A6 RID: 9638 RVA: 0x0009EA66 File Offset: 0x0009CC66
		public float height
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Height).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Height, value);
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x0009EA78 File Offset: 0x0009CC78
		// (set) Token: 0x060025A8 RID: 9640 RVA: 0x0009EAA2 File Offset: 0x0009CCA2
		public float right
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Right).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Right, value);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x0009EAB4 File Offset: 0x0009CCB4
		// (set) Token: 0x060025AA RID: 9642 RVA: 0x0009EADE File Offset: 0x0009CCDE
		public float bottom
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Bottom).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Bottom, value);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x0009EAF0 File Offset: 0x0009CCF0
		// (set) Token: 0x060025AC RID: 9644 RVA: 0x0009EB1A File Offset: 0x0009CD1A
		public Color color
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.Color).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Color, value);
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x0009EB2C File Offset: 0x0009CD2C
		// (set) Token: 0x060025AE RID: 9646 RVA: 0x0009EB56 File Offset: 0x0009CD56
		public Color backgroundColor
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.BackgroundColor).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BackgroundColor, value);
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x0009EB68 File Offset: 0x0009CD68
		// (set) Token: 0x060025B0 RID: 9648 RVA: 0x0009EB92 File Offset: 0x0009CD92
		public Color unityBackgroundImageTintColor
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.UnityBackgroundImageTintColor).value;
			}
			set
			{
				this.SetValue(StylePropertyId.UnityBackgroundImageTintColor, value);
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x0009EBA4 File Offset: 0x0009CDA4
		// (set) Token: 0x060025B2 RID: 9650 RVA: 0x0009EBCE File Offset: 0x0009CDCE
		public Color borderColor
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.BorderColor).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderColor, value);
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x0009EBE0 File Offset: 0x0009CDE0
		// (set) Token: 0x060025B4 RID: 9652 RVA: 0x0009EC0A File Offset: 0x0009CE0A
		public float marginLeft
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginLeft).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginLeft, value);
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x0009EC1C File Offset: 0x0009CE1C
		// (set) Token: 0x060025B6 RID: 9654 RVA: 0x0009EC46 File Offset: 0x0009CE46
		public float marginTop
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginTop).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginTop, value);
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x0009EC58 File Offset: 0x0009CE58
		// (set) Token: 0x060025B8 RID: 9656 RVA: 0x0009EC82 File Offset: 0x0009CE82
		public float marginRight
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginRight).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginRight, value);
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x0009EC94 File Offset: 0x0009CE94
		// (set) Token: 0x060025BA RID: 9658 RVA: 0x0009ECBE File Offset: 0x0009CEBE
		public float marginBottom
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginBottom).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginBottom, value);
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x0009ECD0 File Offset: 0x0009CED0
		// (set) Token: 0x060025BC RID: 9660 RVA: 0x0009ECFA File Offset: 0x0009CEFA
		public float paddingLeft
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingLeft).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingLeft, value);
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x0009ED0C File Offset: 0x0009CF0C
		// (set) Token: 0x060025BE RID: 9662 RVA: 0x0009ED36 File Offset: 0x0009CF36
		public float paddingTop
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingTop).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingTop, value);
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x0009ED48 File Offset: 0x0009CF48
		// (set) Token: 0x060025C0 RID: 9664 RVA: 0x0009ED72 File Offset: 0x0009CF72
		public float paddingRight
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingRight).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingRight, value);
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x0009ED84 File Offset: 0x0009CF84
		// (set) Token: 0x060025C2 RID: 9666 RVA: 0x0009EDAE File Offset: 0x0009CFAE
		public float paddingBottom
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingBottom).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingBottom, value);
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x0009EDC0 File Offset: 0x0009CFC0
		// (set) Token: 0x060025C4 RID: 9668 RVA: 0x0009EDEA File Offset: 0x0009CFEA
		public float borderLeftWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderLeftWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderLeftWidth, value);
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060025C5 RID: 9669 RVA: 0x0009EDFC File Offset: 0x0009CFFC
		// (set) Token: 0x060025C6 RID: 9670 RVA: 0x0009EE26 File Offset: 0x0009D026
		public float borderRightWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderRightWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderRightWidth, value);
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060025C7 RID: 9671 RVA: 0x0009EE38 File Offset: 0x0009D038
		// (set) Token: 0x060025C8 RID: 9672 RVA: 0x0009EE62 File Offset: 0x0009D062
		public float borderTopWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderTopWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderTopWidth, value);
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x0009EE74 File Offset: 0x0009D074
		// (set) Token: 0x060025CA RID: 9674 RVA: 0x0009EE9E File Offset: 0x0009D09E
		public float borderBottomWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderBottomWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderBottomWidth, value);
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x0009EEB0 File Offset: 0x0009D0B0
		// (set) Token: 0x060025CC RID: 9676 RVA: 0x0009EEDA File Offset: 0x0009D0DA
		public float borderTopLeftRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderTopLeftRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderTopLeftRadius, value);
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060025CD RID: 9677 RVA: 0x0009EEEC File Offset: 0x0009D0EC
		// (set) Token: 0x060025CE RID: 9678 RVA: 0x0009EF16 File Offset: 0x0009D116
		public float borderTopRightRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderTopRightRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderTopRightRadius, value);
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060025CF RID: 9679 RVA: 0x0009EF28 File Offset: 0x0009D128
		// (set) Token: 0x060025D0 RID: 9680 RVA: 0x0009EF52 File Offset: 0x0009D152
		public float borderBottomLeftRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderBottomLeftRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderBottomLeftRadius, value);
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060025D1 RID: 9681 RVA: 0x0009EF64 File Offset: 0x0009D164
		// (set) Token: 0x060025D2 RID: 9682 RVA: 0x0009EF8E File Offset: 0x0009D18E
		public float borderBottomRightRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderBottomRightRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderBottomRightRadius, value);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x0009EFA0 File Offset: 0x0009D1A0
		// (set) Token: 0x060025D4 RID: 9684 RVA: 0x0009EFCA File Offset: 0x0009D1CA
		public float opacity
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Opacity).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Opacity, value);
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060025D5 RID: 9685 RVA: 0x0009EFDC File Offset: 0x0009D1DC
		// (set) Token: 0x060025D6 RID: 9686 RVA: 0x0009F006 File Offset: 0x0009D206
		public float flexGrow
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.FlexGrow).value;
			}
			set
			{
				this.SetValue(StylePropertyId.FlexGrow, value);
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060025D7 RID: 9687 RVA: 0x0009F018 File Offset: 0x0009D218
		// (set) Token: 0x060025D8 RID: 9688 RVA: 0x0009F006 File Offset: 0x0009D206
		public float flexShrink
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.FlexShrink).value;
			}
			set
			{
				this.SetValue(StylePropertyId.FlexGrow, value);
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x0009F044 File Offset: 0x0009D244
		internal void SetValue(StylePropertyId id, float value)
		{
			StyleValue styleValue = default(StyleValue);
			styleValue.id = id;
			styleValue.number = value;
			this.Values().SetStyleValue(styleValue);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x0009F078 File Offset: 0x0009D278
		internal void SetValue(StylePropertyId id, Color value)
		{
			StyleValue styleValue = default(StyleValue);
			styleValue.id = id;
			styleValue.color = value;
			this.Values().SetStyleValue(styleValue);
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x0009F0AC File Offset: 0x0009D2AC
		internal StyleValueCollection Values()
		{
			bool flag = this.m_StyleValues == null;
			if (flag)
			{
				this.m_StyleValues = new StyleValueCollection();
			}
			return this.m_StyleValues;
		}

		// Token: 0x0400123F RID: 4671
		internal StyleValueCollection m_StyleValues;
	}
}
