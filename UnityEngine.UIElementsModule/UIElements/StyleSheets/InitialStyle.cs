using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000482 RID: 1154
	internal static class InitialStyle
	{
		// Token: 0x060023E7 RID: 9191 RVA: 0x00092A20 File Offset: 0x00090C20
		public static ref ComputedStyle Get()
		{
			return ref InitialStyle.s_InitialStyle;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00092A38 File Offset: 0x00090C38
		public static ComputedStyle Acquire()
		{
			return InitialStyle.s_InitialStyle.Acquire();
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00092A54 File Offset: 0x00090C54
		static InitialStyle()
		{
			InitialStyle.s_InitialStyle.layoutData.Write().alignContent = Align.FlexStart;
			InitialStyle.s_InitialStyle.layoutData.Write().alignItems = Align.Stretch;
			InitialStyle.s_InitialStyle.layoutData.Write().alignSelf = Align.Auto;
			InitialStyle.s_InitialStyle.visualData.Write().backgroundColor = Color.clear;
			InitialStyle.s_InitialStyle.visualData.Write().backgroundImage = default(Background);
			InitialStyle.s_InitialStyle.visualData.Write().backgroundPositionX = BackgroundPosition.Initial();
			InitialStyle.s_InitialStyle.visualData.Write().backgroundPositionY = BackgroundPosition.Initial();
			InitialStyle.s_InitialStyle.visualData.Write().backgroundRepeat = BackgroundRepeat.Initial();
			InitialStyle.s_InitialStyle.visualData.Write().backgroundSize = BackgroundSize.Initial();
			InitialStyle.s_InitialStyle.visualData.Write().borderBottomColor = Color.clear;
			InitialStyle.s_InitialStyle.visualData.Write().borderBottomLeftRadius = 0f;
			InitialStyle.s_InitialStyle.visualData.Write().borderBottomRightRadius = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().borderBottomWidth = 0f;
			InitialStyle.s_InitialStyle.visualData.Write().borderLeftColor = Color.clear;
			InitialStyle.s_InitialStyle.layoutData.Write().borderLeftWidth = 0f;
			InitialStyle.s_InitialStyle.visualData.Write().borderRightColor = Color.clear;
			InitialStyle.s_InitialStyle.layoutData.Write().borderRightWidth = 0f;
			InitialStyle.s_InitialStyle.visualData.Write().borderTopColor = Color.clear;
			InitialStyle.s_InitialStyle.visualData.Write().borderTopLeftRadius = 0f;
			InitialStyle.s_InitialStyle.visualData.Write().borderTopRightRadius = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().borderTopWidth = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().bottom = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.inheritedData.Write().color = Color.black;
			InitialStyle.s_InitialStyle.rareData.Write().cursor = default(Cursor);
			InitialStyle.s_InitialStyle.layoutData.Write().display = DisplayStyle.Flex;
			InitialStyle.s_InitialStyle.layoutData.Write().flexBasis = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.layoutData.Write().flexDirection = FlexDirection.Column;
			InitialStyle.s_InitialStyle.layoutData.Write().flexGrow = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().flexShrink = 1f;
			InitialStyle.s_InitialStyle.layoutData.Write().flexWrap = Wrap.NoWrap;
			InitialStyle.s_InitialStyle.inheritedData.Write().fontSize = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().height = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.layoutData.Write().justifyContent = Justify.FlexStart;
			InitialStyle.s_InitialStyle.layoutData.Write().left = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.inheritedData.Write().letterSpacing = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().marginBottom = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().marginLeft = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().marginRight = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().marginTop = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().maxHeight = StyleKeyword.None.ToLength();
			InitialStyle.s_InitialStyle.layoutData.Write().maxWidth = StyleKeyword.None.ToLength();
			InitialStyle.s_InitialStyle.layoutData.Write().minHeight = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.layoutData.Write().minWidth = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.visualData.Write().opacity = 1f;
			InitialStyle.s_InitialStyle.visualData.Write().overflow = OverflowInternal.Visible;
			InitialStyle.s_InitialStyle.layoutData.Write().paddingBottom = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().paddingLeft = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().paddingRight = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().paddingTop = 0f;
			InitialStyle.s_InitialStyle.layoutData.Write().position = Position.Relative;
			InitialStyle.s_InitialStyle.layoutData.Write().right = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.transformData.Write().rotate = StyleKeyword.None.ToRotate();
			InitialStyle.s_InitialStyle.transformData.Write().scale = StyleKeyword.None.ToScale();
			InitialStyle.s_InitialStyle.rareData.Write().textOverflow = TextOverflow.Clip;
			InitialStyle.s_InitialStyle.inheritedData.Write().textShadow = default(TextShadow);
			InitialStyle.s_InitialStyle.layoutData.Write().top = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.transformData.Write().transformOrigin = TransformOrigin.Initial();
			InitialStyle.s_InitialStyle.transitionData.Write().transitionDelay = new List<TimeValue> { 0f };
			InitialStyle.s_InitialStyle.transitionData.Write().transitionDuration = new List<TimeValue> { 0f };
			InitialStyle.s_InitialStyle.transitionData.Write().transitionProperty = new List<StylePropertyName> { "all" };
			InitialStyle.s_InitialStyle.transitionData.Write().transitionTimingFunction = new List<EasingFunction> { EasingMode.Ease };
			InitialStyle.s_InitialStyle.transformData.Write().translate = StyleKeyword.None.ToTranslate();
			InitialStyle.s_InitialStyle.rareData.Write().unityBackgroundImageTintColor = Color.white;
			InitialStyle.s_InitialStyle.inheritedData.Write().unityFont = null;
			InitialStyle.s_InitialStyle.inheritedData.Write().unityFontDefinition = default(FontDefinition);
			InitialStyle.s_InitialStyle.inheritedData.Write().unityFontStyleAndWeight = FontStyle.Normal;
			InitialStyle.s_InitialStyle.rareData.Write().unityOverflowClipBox = OverflowClipBox.PaddingBox;
			InitialStyle.s_InitialStyle.inheritedData.Write().unityParagraphSpacing = 0f;
			InitialStyle.s_InitialStyle.rareData.Write().unitySliceBottom = 0;
			InitialStyle.s_InitialStyle.rareData.Write().unitySliceLeft = 0;
			InitialStyle.s_InitialStyle.rareData.Write().unitySliceRight = 0;
			InitialStyle.s_InitialStyle.rareData.Write().unitySliceScale = 1f;
			InitialStyle.s_InitialStyle.rareData.Write().unitySliceTop = 0;
			InitialStyle.s_InitialStyle.inheritedData.Write().unityTextAlign = TextAnchor.UpperLeft;
			InitialStyle.s_InitialStyle.inheritedData.Write().unityTextOutlineColor = Color.clear;
			InitialStyle.s_InitialStyle.inheritedData.Write().unityTextOutlineWidth = 0f;
			InitialStyle.s_InitialStyle.rareData.Write().unityTextOverflowPosition = TextOverflowPosition.End;
			InitialStyle.s_InitialStyle.inheritedData.Write().visibility = Visibility.Visible;
			InitialStyle.s_InitialStyle.inheritedData.Write().whiteSpace = WhiteSpace.Normal;
			InitialStyle.s_InitialStyle.layoutData.Write().width = StyleKeyword.Auto.ToLength();
			InitialStyle.s_InitialStyle.inheritedData.Write().wordSpacing = 0f;
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000932A3 File Offset: 0x000914A3
		public static Align alignContent
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().alignContent;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x000932B9 File Offset: 0x000914B9
		public static Align alignItems
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().alignItems;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000932CF File Offset: 0x000914CF
		public static Align alignSelf
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().alignSelf;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x000932E5 File Offset: 0x000914E5
		public static Color backgroundColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().backgroundColor;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000932FB File Offset: 0x000914FB
		public static Background backgroundImage
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().backgroundImage;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x00093311 File Offset: 0x00091511
		public static BackgroundPosition backgroundPositionX
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().backgroundPositionX;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x00093327 File Offset: 0x00091527
		public static BackgroundPosition backgroundPositionY
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().backgroundPositionY;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x0009333D File Offset: 0x0009153D
		public static BackgroundRepeat backgroundRepeat
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().backgroundRepeat;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x00093353 File Offset: 0x00091553
		public static BackgroundSize backgroundSize
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().backgroundSize;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x00093369 File Offset: 0x00091569
		public static Color borderBottomColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderBottomColor;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x0009337F File Offset: 0x0009157F
		public static Length borderBottomLeftRadius
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderBottomLeftRadius;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x00093395 File Offset: 0x00091595
		public static Length borderBottomRightRadius
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderBottomRightRadius;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000933AB File Offset: 0x000915AB
		public static float borderBottomWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().borderBottomWidth;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x000933C1 File Offset: 0x000915C1
		public static Color borderLeftColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderLeftColor;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000933D7 File Offset: 0x000915D7
		public static float borderLeftWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().borderLeftWidth;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000933ED File Offset: 0x000915ED
		public static Color borderRightColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderRightColor;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x00093403 File Offset: 0x00091603
		public static float borderRightWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().borderRightWidth;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x00093419 File Offset: 0x00091619
		public static Color borderTopColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderTopColor;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x0009342F File Offset: 0x0009162F
		public static Length borderTopLeftRadius
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderTopLeftRadius;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x00093445 File Offset: 0x00091645
		public static Length borderTopRightRadius
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().borderTopRightRadius;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x0009345B File Offset: 0x0009165B
		public static float borderTopWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().borderTopWidth;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x00093471 File Offset: 0x00091671
		public static Length bottom
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().bottom;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x00093487 File Offset: 0x00091687
		public static Color color
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().color;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x0009349D File Offset: 0x0009169D
		public static Cursor cursor
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().cursor;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x000934B3 File Offset: 0x000916B3
		public static DisplayStyle display
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().display;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x000934C9 File Offset: 0x000916C9
		public static Length flexBasis
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().flexBasis;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002404 RID: 9220 RVA: 0x000934DF File Offset: 0x000916DF
		public static FlexDirection flexDirection
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().flexDirection;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x000934F5 File Offset: 0x000916F5
		public static float flexGrow
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().flexGrow;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x0009350B File Offset: 0x0009170B
		public static float flexShrink
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().flexShrink;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002407 RID: 9223 RVA: 0x00093521 File Offset: 0x00091721
		public static Wrap flexWrap
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().flexWrap;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002408 RID: 9224 RVA: 0x00093537 File Offset: 0x00091737
		public static Length fontSize
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().fontSize;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x0009354D File Offset: 0x0009174D
		public static Length height
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().height;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x00093563 File Offset: 0x00091763
		public static Justify justifyContent
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().justifyContent;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x00093579 File Offset: 0x00091779
		public static Length left
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().left;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0009358F File Offset: 0x0009178F
		public static Length letterSpacing
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().letterSpacing;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x000935A5 File Offset: 0x000917A5
		public static Length marginBottom
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().marginBottom;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x000935BB File Offset: 0x000917BB
		public static Length marginLeft
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().marginLeft;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x000935D1 File Offset: 0x000917D1
		public static Length marginRight
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().marginRight;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000935E7 File Offset: 0x000917E7
		public static Length marginTop
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().marginTop;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x000935FD File Offset: 0x000917FD
		public static Length maxHeight
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().maxHeight;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x00093613 File Offset: 0x00091813
		public static Length maxWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().maxWidth;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x00093629 File Offset: 0x00091829
		public static Length minHeight
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().minHeight;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x0009363F File Offset: 0x0009183F
		public static Length minWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().minWidth;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x00093655 File Offset: 0x00091855
		public static float opacity
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().opacity;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x0009366B File Offset: 0x0009186B
		public static OverflowInternal overflow
		{
			get
			{
				return InitialStyle.s_InitialStyle.visualData.Read().overflow;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x00093681 File Offset: 0x00091881
		public static Length paddingBottom
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().paddingBottom;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x00093697 File Offset: 0x00091897
		public static Length paddingLeft
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().paddingLeft;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000936AD File Offset: 0x000918AD
		public static Length paddingRight
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().paddingRight;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000936C3 File Offset: 0x000918C3
		public static Length paddingTop
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().paddingTop;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x000936D9 File Offset: 0x000918D9
		public static Position position
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().position;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000936EF File Offset: 0x000918EF
		public static Length right
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().right;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x00093705 File Offset: 0x00091905
		public static Rotate rotate
		{
			get
			{
				return InitialStyle.s_InitialStyle.transformData.Read().rotate;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0009371B File Offset: 0x0009191B
		public static Scale scale
		{
			get
			{
				return InitialStyle.s_InitialStyle.transformData.Read().scale;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x00093731 File Offset: 0x00091931
		public static TextOverflow textOverflow
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().textOverflow;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x00093747 File Offset: 0x00091947
		public static TextShadow textShadow
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().textShadow;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0009375D File Offset: 0x0009195D
		public static Length top
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().top;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x00093773 File Offset: 0x00091973
		public static TransformOrigin transformOrigin
		{
			get
			{
				return InitialStyle.s_InitialStyle.transformData.Read().transformOrigin;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x00093789 File Offset: 0x00091989
		public static List<TimeValue> transitionDelay
		{
			get
			{
				return InitialStyle.s_InitialStyle.transitionData.Read().transitionDelay;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x0009379F File Offset: 0x0009199F
		public static List<TimeValue> transitionDuration
		{
			get
			{
				return InitialStyle.s_InitialStyle.transitionData.Read().transitionDuration;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002425 RID: 9253 RVA: 0x000937B5 File Offset: 0x000919B5
		public static List<StylePropertyName> transitionProperty
		{
			get
			{
				return InitialStyle.s_InitialStyle.transitionData.Read().transitionProperty;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x000937CB File Offset: 0x000919CB
		public static List<EasingFunction> transitionTimingFunction
		{
			get
			{
				return InitialStyle.s_InitialStyle.transitionData.Read().transitionTimingFunction;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x000937E1 File Offset: 0x000919E1
		public static Translate translate
		{
			get
			{
				return InitialStyle.s_InitialStyle.transformData.Read().translate;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x000937F7 File Offset: 0x000919F7
		public static Color unityBackgroundImageTintColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unityBackgroundImageTintColor;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x0009380D File Offset: 0x00091A0D
		public static Font unityFont
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityFont;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x00093823 File Offset: 0x00091A23
		public static FontDefinition unityFontDefinition
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityFontDefinition;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x00093839 File Offset: 0x00091A39
		public static FontStyle unityFontStyleAndWeight
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityFontStyleAndWeight;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x0009384F File Offset: 0x00091A4F
		public static OverflowClipBox unityOverflowClipBox
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unityOverflowClipBox;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x00093865 File Offset: 0x00091A65
		public static Length unityParagraphSpacing
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityParagraphSpacing;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x0009387B File Offset: 0x00091A7B
		public static int unitySliceBottom
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unitySliceBottom;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x00093891 File Offset: 0x00091A91
		public static int unitySliceLeft
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unitySliceLeft;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000938A7 File Offset: 0x00091AA7
		public static int unitySliceRight
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unitySliceRight;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000938BD File Offset: 0x00091ABD
		public static float unitySliceScale
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unitySliceScale;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x000938D3 File Offset: 0x00091AD3
		public static int unitySliceTop
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unitySliceTop;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x000938E9 File Offset: 0x00091AE9
		public static TextAnchor unityTextAlign
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityTextAlign;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002434 RID: 9268 RVA: 0x000938FF File Offset: 0x00091AFF
		public static Color unityTextOutlineColor
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityTextOutlineColor;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x00093915 File Offset: 0x00091B15
		public static float unityTextOutlineWidth
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().unityTextOutlineWidth;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x0009392B File Offset: 0x00091B2B
		public static TextOverflowPosition unityTextOverflowPosition
		{
			get
			{
				return InitialStyle.s_InitialStyle.rareData.Read().unityTextOverflowPosition;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x00093941 File Offset: 0x00091B41
		public static Visibility visibility
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().visibility;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x00093957 File Offset: 0x00091B57
		public static WhiteSpace whiteSpace
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().whiteSpace;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x0009396D File Offset: 0x00091B6D
		public static Length width
		{
			get
			{
				return InitialStyle.s_InitialStyle.layoutData.Read().width;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x00093983 File Offset: 0x00091B83
		public static Length wordSpacing
		{
			get
			{
				return InitialStyle.s_InitialStyle.inheritedData.Read().wordSpacing;
			}
		}

		// Token: 0x040010E3 RID: 4323
		private static ComputedStyle s_InitialStyle = ComputedStyle.CreateInitial();
	}
}
