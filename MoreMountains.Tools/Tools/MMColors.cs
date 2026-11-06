using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F6 RID: 246
	public static class MMColors
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x0001758C File Offset: 0x0001578C
		public static Color RandomColor()
		{
			return MMColors.GetColorAt(Random.Range(0, 140));
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001759E File Offset: 0x0001579E
		public static Color GetColorAt(int index)
		{
			if (MMColors.ColorDictionary == null)
			{
				MMColors.InitializeDictionary();
			}
			if (index < MMColors.ColorDictionary.Count)
			{
				return MMColors.ColorDictionary[index];
			}
			return Color.white;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000175CC File Offset: 0x000157CC
		public static void InitializeDictionary()
		{
			MMColors.ColorDictionary = new Dictionary<int, Color>
			{
				{
					0,
					MMColors.AliceBlue
				},
				{
					1,
					MMColors.AntiqueWhite
				},
				{
					2,
					MMColors.Aqua
				},
				{
					3,
					MMColors.Aquamarine
				},
				{
					4,
					MMColors.Azure
				},
				{
					5,
					MMColors.Beige
				},
				{
					6,
					MMColors.Bisque
				},
				{
					7,
					MMColors.Black
				},
				{
					8,
					MMColors.BlanchedAlmond
				},
				{
					9,
					MMColors.Blue
				},
				{
					10,
					MMColors.BlueViolet
				},
				{
					11,
					MMColors.Brown
				},
				{
					12,
					MMColors.Burlywood
				},
				{
					13,
					MMColors.CadetBlue
				},
				{
					14,
					MMColors.Chartreuse
				},
				{
					15,
					MMColors.Chocolate
				},
				{
					16,
					MMColors.Coral
				},
				{
					17,
					MMColors.CornflowerBlue
				},
				{
					18,
					MMColors.Cornsilk
				},
				{
					19,
					MMColors.Crimson
				},
				{
					20,
					MMColors.Cyan
				},
				{
					21,
					MMColors.DarkBlue
				},
				{
					22,
					MMColors.DarkCyan
				},
				{
					23,
					MMColors.DarkGoldenrod
				},
				{
					24,
					MMColors.DarkGray
				},
				{
					25,
					MMColors.DarkGreen
				},
				{
					26,
					MMColors.DarkKhaki
				},
				{
					27,
					MMColors.DarkMagenta
				},
				{
					28,
					MMColors.DarkOliveGreen
				},
				{
					29,
					MMColors.DarkOrange
				},
				{
					30,
					MMColors.DarkOrchid
				},
				{
					31,
					MMColors.DarkRed
				},
				{
					32,
					MMColors.DarkSalmon
				},
				{
					33,
					MMColors.DarkSeaGreen
				},
				{
					34,
					MMColors.DarkSlateBlue
				},
				{
					35,
					MMColors.DarkSlateGray
				},
				{
					36,
					MMColors.DarkTurquoise
				},
				{
					37,
					MMColors.DarkViolet
				},
				{
					38,
					MMColors.DeepPink
				},
				{
					39,
					MMColors.DeepSkyBlue
				},
				{
					40,
					MMColors.DimGray
				},
				{
					41,
					MMColors.DodgerBlue
				},
				{
					42,
					MMColors.FireBrick
				},
				{
					43,
					MMColors.FloralWhite
				},
				{
					44,
					MMColors.ForestGreen
				},
				{
					45,
					MMColors.Fuchsia
				},
				{
					46,
					MMColors.Gainsboro
				},
				{
					47,
					MMColors.GhostWhite
				},
				{
					48,
					MMColors.Gold
				},
				{
					49,
					MMColors.Goldenrod
				},
				{
					50,
					MMColors.Gray
				},
				{
					51,
					MMColors.Green
				},
				{
					52,
					MMColors.GreenYellow
				},
				{
					53,
					MMColors.Honeydew
				},
				{
					54,
					MMColors.HotPink
				},
				{
					55,
					MMColors.IndianRed
				},
				{
					56,
					MMColors.Indigo
				},
				{
					57,
					MMColors.Ivory
				},
				{
					58,
					MMColors.Khaki
				},
				{
					59,
					MMColors.Lavender
				},
				{
					60,
					MMColors.Lavenderblush
				},
				{
					61,
					MMColors.LawnGreen
				},
				{
					62,
					MMColors.LemonChiffon
				},
				{
					63,
					MMColors.LightBlue
				},
				{
					64,
					MMColors.LightCoral
				},
				{
					65,
					MMColors.LightCyan
				},
				{
					66,
					MMColors.LightGoldenodYellow
				},
				{
					67,
					MMColors.LightGray
				},
				{
					68,
					MMColors.LightGreen
				},
				{
					69,
					MMColors.LightPink
				},
				{
					70,
					MMColors.LightSalmon
				},
				{
					71,
					MMColors.LightSeaGreen
				},
				{
					72,
					MMColors.LightSkyBlue
				},
				{
					73,
					MMColors.LightSlateGray
				},
				{
					74,
					MMColors.LightSteelBlue
				},
				{
					75,
					MMColors.LightYellow
				},
				{
					76,
					MMColors.Lime
				},
				{
					77,
					MMColors.LimeGreen
				},
				{
					78,
					MMColors.Linen
				},
				{
					79,
					MMColors.Magenta
				},
				{
					80,
					MMColors.Maroon
				},
				{
					81,
					MMColors.MediumAquamarine
				},
				{
					82,
					MMColors.MediumBlue
				},
				{
					83,
					MMColors.MediumOrchid
				},
				{
					84,
					MMColors.MediumPurple
				},
				{
					85,
					MMColors.MediumSeaGreen
				},
				{
					86,
					MMColors.MediumSlateBlue
				},
				{
					87,
					MMColors.MediumSpringGreen
				},
				{
					88,
					MMColors.MediumTurquoise
				},
				{
					89,
					MMColors.MediumVioletRed
				},
				{
					90,
					MMColors.MidnightBlue
				},
				{
					91,
					MMColors.Mintcream
				},
				{
					92,
					MMColors.MistyRose
				},
				{
					93,
					MMColors.Moccasin
				},
				{
					94,
					MMColors.NavajoWhite
				},
				{
					95,
					MMColors.Navy
				},
				{
					96,
					MMColors.OldLace
				},
				{
					97,
					MMColors.Olive
				},
				{
					98,
					MMColors.Olivedrab
				},
				{
					99,
					MMColors.Orange
				},
				{
					100,
					MMColors.Orangered
				},
				{
					101,
					MMColors.Orchid
				},
				{
					102,
					MMColors.PaleGoldenrod
				},
				{
					103,
					MMColors.PaleGreen
				},
				{
					104,
					MMColors.PaleTurquoise
				},
				{
					105,
					MMColors.PaleVioletred
				},
				{
					106,
					MMColors.PapayaWhip
				},
				{
					107,
					MMColors.PeachPuff
				},
				{
					108,
					MMColors.Peru
				},
				{
					109,
					MMColors.Pink
				},
				{
					110,
					MMColors.Plum
				},
				{
					111,
					MMColors.PowderBlue
				},
				{
					112,
					MMColors.Purple
				},
				{
					113,
					MMColors.Red
				},
				{
					114,
					MMColors.RosyBrown
				},
				{
					115,
					MMColors.RoyalBlue
				},
				{
					116,
					MMColors.SaddleBrown
				},
				{
					117,
					MMColors.Salmon
				},
				{
					118,
					MMColors.SandyBrown
				},
				{
					119,
					MMColors.SeaGreen
				},
				{
					120,
					MMColors.Seashell
				},
				{
					121,
					MMColors.Sienna
				},
				{
					122,
					MMColors.Silver
				},
				{
					123,
					MMColors.SkyBlue
				},
				{
					124,
					MMColors.SlateBlue
				},
				{
					125,
					MMColors.SlateGray
				},
				{
					126,
					MMColors.Snow
				},
				{
					127,
					MMColors.SpringGreen
				},
				{
					128,
					MMColors.SteelBlue
				},
				{
					129,
					MMColors.Tan
				},
				{
					130,
					MMColors.Teal
				},
				{
					131,
					MMColors.Thistle
				},
				{
					132,
					MMColors.Tomato
				},
				{
					133,
					MMColors.Turquoise
				},
				{
					134,
					MMColors.Violet
				},
				{
					135,
					MMColors.Wheat
				},
				{
					136,
					MMColors.White
				},
				{
					137,
					MMColors.WhiteSmoke
				},
				{
					138,
					MMColors.Yellow
				},
				{
					139,
					MMColors.YellowGreen
				},
				{
					140,
					MMColors.ReunoYellow
				},
				{
					141,
					MMColors.BestRed
				}
			};
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00017D3C File Offset: 0x00015F3C
		public static Color MMRandomColor(this Color color, Color min, Color max)
		{
			return new Color
			{
				r = Random.Range(min.r, max.r),
				g = Random.Range(min.g, max.g),
				b = Random.Range(min.b, max.b),
				a = Random.Range(min.a, max.a)
			};
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00017DB4 File Offset: 0x00015FB4
		public static Gradient FlatGradient(Color32 color, float alpha = 1f)
		{
			return new Gradient
			{
				colorKeys = new GradientColorKey[]
				{
					new GradientColorKey(color, 0f),
					new GradientColorKey(color, 1f)
				},
				alphaKeys = new GradientAlphaKey[]
				{
					new GradientAlphaKey(alpha, 0f),
					new GradientAlphaKey(alpha, 1f)
				}
			};
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00017E34 File Offset: 0x00016034
		public static Gradient SimpleGradient(Color32 startColor, Color32 endColor, float startAlpha = 1f, float endAlpha = 1f)
		{
			return new Gradient
			{
				colorKeys = new GradientColorKey[]
				{
					new GradientColorKey(startColor, 0f),
					new GradientColorKey(endColor, 1f)
				},
				alphaKeys = new GradientAlphaKey[]
				{
					new GradientAlphaKey(startAlpha, 0f),
					new GradientAlphaKey(endAlpha, 1f)
				}
			};
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00017EB4 File Offset: 0x000160B4
		public static Color MMColorize(this Color originalColor, Color targetColor, MMColors.ColoringMode coloringMode, float lerpAmount = 1f)
		{
			Color color = Color.white;
			switch (coloringMode)
			{
			case MMColors.ColoringMode.Tint:
			{
				float num;
				float num2;
				float num3;
				Color.RGBToHSV(originalColor, out num, out num2, out num3);
				float num4;
				float num5;
				float num6;
				Color.RGBToHSV(targetColor, out num4, out num5, out num6);
				color = Color.HSVToRGB(num4, num5, num3 * num6);
				color.a = originalColor.a * targetColor.a;
				break;
			}
			case MMColors.ColoringMode.Multiply:
				color = originalColor * targetColor;
				break;
			case MMColors.ColoringMode.Replace:
				color = targetColor;
				break;
			case MMColors.ColoringMode.ReplaceKeepAlpha:
				color = targetColor;
				color.a = originalColor.a;
				break;
			case MMColors.ColoringMode.Add:
				color = originalColor + targetColor;
				break;
			}
			return Color.Lerp(originalColor, color, lerpAmount);
		}

		// Token: 0x0400063C RID: 1596
		public static readonly Color ReunoYellow = new Color32(byte.MaxValue, 196, 0, byte.MaxValue);

		// Token: 0x0400063D RID: 1597
		public static readonly Color BestRed = new Color32(byte.MaxValue, 24, 0, byte.MaxValue);

		// Token: 0x0400063E RID: 1598
		public static readonly Color AliceBlue = new Color32(240, 248, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400063F RID: 1599
		public static readonly Color AntiqueWhite = new Color32(250, 235, 215, byte.MaxValue);

		// Token: 0x04000640 RID: 1600
		public static readonly Color Aqua = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000641 RID: 1601
		public static readonly Color Aquamarine = new Color32(127, byte.MaxValue, 212, byte.MaxValue);

		// Token: 0x04000642 RID: 1602
		public static readonly Color Azure = new Color32(240, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000643 RID: 1603
		public static readonly Color Beige = new Color32(245, 245, 220, byte.MaxValue);

		// Token: 0x04000644 RID: 1604
		public static readonly Color Bisque = new Color32(byte.MaxValue, 228, 196, byte.MaxValue);

		// Token: 0x04000645 RID: 1605
		public static readonly Color Black = new Color32(0, 0, 0, byte.MaxValue);

		// Token: 0x04000646 RID: 1606
		public static readonly Color BlanchedAlmond = new Color32(byte.MaxValue, 235, 205, byte.MaxValue);

		// Token: 0x04000647 RID: 1607
		public static readonly Color Blue = new Color32(0, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000648 RID: 1608
		public static readonly Color BlueViolet = new Color32(138, 43, 226, byte.MaxValue);

		// Token: 0x04000649 RID: 1609
		public static readonly Color Brown = new Color32(165, 42, 42, byte.MaxValue);

		// Token: 0x0400064A RID: 1610
		public static readonly Color Burlywood = new Color32(222, 184, 135, byte.MaxValue);

		// Token: 0x0400064B RID: 1611
		public static readonly Color CadetBlue = new Color32(95, 158, 160, byte.MaxValue);

		// Token: 0x0400064C RID: 1612
		public static readonly Color Chartreuse = new Color32(127, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x0400064D RID: 1613
		public static readonly Color Chocolate = new Color32(210, 105, 30, byte.MaxValue);

		// Token: 0x0400064E RID: 1614
		public static readonly Color Coral = new Color32(byte.MaxValue, 127, 80, byte.MaxValue);

		// Token: 0x0400064F RID: 1615
		public static readonly Color CornflowerBlue = new Color32(100, 149, 237, byte.MaxValue);

		// Token: 0x04000650 RID: 1616
		public static readonly Color Cornsilk = new Color32(byte.MaxValue, 248, 220, byte.MaxValue);

		// Token: 0x04000651 RID: 1617
		public static readonly Color Crimson = new Color32(220, 20, 60, byte.MaxValue);

		// Token: 0x04000652 RID: 1618
		public static readonly Color Cyan = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000653 RID: 1619
		public static readonly Color DarkBlue = new Color32(0, 0, 139, byte.MaxValue);

		// Token: 0x04000654 RID: 1620
		public static readonly Color DarkCyan = new Color32(0, 139, 139, byte.MaxValue);

		// Token: 0x04000655 RID: 1621
		public static readonly Color DarkGoldenrod = new Color32(184, 134, 11, byte.MaxValue);

		// Token: 0x04000656 RID: 1622
		public static readonly Color DarkGray = new Color32(169, 169, 169, byte.MaxValue);

		// Token: 0x04000657 RID: 1623
		public static readonly Color DarkGreen = new Color32(0, 100, 0, byte.MaxValue);

		// Token: 0x04000658 RID: 1624
		public static readonly Color DarkKhaki = new Color32(189, 183, 107, byte.MaxValue);

		// Token: 0x04000659 RID: 1625
		public static readonly Color DarkMagenta = new Color32(139, 0, 139, byte.MaxValue);

		// Token: 0x0400065A RID: 1626
		public static readonly Color DarkOliveGreen = new Color32(85, 107, 47, byte.MaxValue);

		// Token: 0x0400065B RID: 1627
		public static readonly Color DarkOrange = new Color32(byte.MaxValue, 140, 0, byte.MaxValue);

		// Token: 0x0400065C RID: 1628
		public static readonly Color DarkOrchid = new Color32(153, 50, 204, byte.MaxValue);

		// Token: 0x0400065D RID: 1629
		public static readonly Color DarkRed = new Color32(139, 0, 0, byte.MaxValue);

		// Token: 0x0400065E RID: 1630
		public static readonly Color DarkSalmon = new Color32(233, 150, 122, byte.MaxValue);

		// Token: 0x0400065F RID: 1631
		public static readonly Color DarkSeaGreen = new Color32(143, 188, 143, byte.MaxValue);

		// Token: 0x04000660 RID: 1632
		public static readonly Color DarkSlateBlue = new Color32(72, 61, 139, byte.MaxValue);

		// Token: 0x04000661 RID: 1633
		public static readonly Color DarkSlateGray = new Color32(47, 79, 79, byte.MaxValue);

		// Token: 0x04000662 RID: 1634
		public static readonly Color DarkTurquoise = new Color32(0, 206, 209, byte.MaxValue);

		// Token: 0x04000663 RID: 1635
		public static readonly Color DarkViolet = new Color32(148, 0, 211, byte.MaxValue);

		// Token: 0x04000664 RID: 1636
		public static readonly Color DeepPink = new Color32(byte.MaxValue, 20, 147, byte.MaxValue);

		// Token: 0x04000665 RID: 1637
		public static readonly Color DeepSkyBlue = new Color32(0, 191, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000666 RID: 1638
		public static readonly Color DimGray = new Color32(105, 105, 105, byte.MaxValue);

		// Token: 0x04000667 RID: 1639
		public static readonly Color DodgerBlue = new Color32(30, 144, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000668 RID: 1640
		public static readonly Color FireBrick = new Color32(178, 34, 34, byte.MaxValue);

		// Token: 0x04000669 RID: 1641
		public static readonly Color FloralWhite = new Color32(byte.MaxValue, 250, 240, byte.MaxValue);

		// Token: 0x0400066A RID: 1642
		public static readonly Color ForestGreen = new Color32(34, 139, 34, byte.MaxValue);

		// Token: 0x0400066B RID: 1643
		public static readonly Color Fuchsia = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400066C RID: 1644
		public static readonly Color Gainsboro = new Color32(220, 220, 220, byte.MaxValue);

		// Token: 0x0400066D RID: 1645
		public static readonly Color GhostWhite = new Color32(248, 248, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400066E RID: 1646
		public static readonly Color Gold = new Color32(byte.MaxValue, 215, 0, byte.MaxValue);

		// Token: 0x0400066F RID: 1647
		public static readonly Color Goldenrod = new Color32(218, 165, 32, byte.MaxValue);

		// Token: 0x04000670 RID: 1648
		public static readonly Color Gray = new Color32(128, 128, 128, byte.MaxValue);

		// Token: 0x04000671 RID: 1649
		public static readonly Color Green = new Color32(0, 128, 0, byte.MaxValue);

		// Token: 0x04000672 RID: 1650
		public static readonly Color GreenYellow = new Color32(173, byte.MaxValue, 47, byte.MaxValue);

		// Token: 0x04000673 RID: 1651
		public static readonly Color Honeydew = new Color32(240, byte.MaxValue, 240, byte.MaxValue);

		// Token: 0x04000674 RID: 1652
		public static readonly Color HotPink = new Color32(byte.MaxValue, 105, 180, byte.MaxValue);

		// Token: 0x04000675 RID: 1653
		public static readonly Color IndianRed = new Color32(205, 92, 92, byte.MaxValue);

		// Token: 0x04000676 RID: 1654
		public static readonly Color Indigo = new Color32(75, 0, 130, byte.MaxValue);

		// Token: 0x04000677 RID: 1655
		public static readonly Color Ivory = new Color32(byte.MaxValue, byte.MaxValue, 240, byte.MaxValue);

		// Token: 0x04000678 RID: 1656
		public static readonly Color Khaki = new Color32(240, 230, 140, byte.MaxValue);

		// Token: 0x04000679 RID: 1657
		public static readonly Color Lavender = new Color32(230, 230, 250, byte.MaxValue);

		// Token: 0x0400067A RID: 1658
		public static readonly Color Lavenderblush = new Color32(byte.MaxValue, 240, 245, byte.MaxValue);

		// Token: 0x0400067B RID: 1659
		public static readonly Color LawnGreen = new Color32(124, 252, 0, byte.MaxValue);

		// Token: 0x0400067C RID: 1660
		public static readonly Color LemonChiffon = new Color32(byte.MaxValue, 250, 205, byte.MaxValue);

		// Token: 0x0400067D RID: 1661
		public static readonly Color LightBlue = new Color32(173, 216, 230, byte.MaxValue);

		// Token: 0x0400067E RID: 1662
		public static readonly Color LightCoral = new Color32(240, 128, 128, byte.MaxValue);

		// Token: 0x0400067F RID: 1663
		public static readonly Color LightCyan = new Color32(224, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000680 RID: 1664
		public static readonly Color LightGoldenodYellow = new Color32(250, 250, 210, byte.MaxValue);

		// Token: 0x04000681 RID: 1665
		public static readonly Color LightGray = new Color32(211, 211, 211, byte.MaxValue);

		// Token: 0x04000682 RID: 1666
		public static readonly Color LightGreen = new Color32(144, 238, 144, byte.MaxValue);

		// Token: 0x04000683 RID: 1667
		public static readonly Color LightPink = new Color32(byte.MaxValue, 182, 193, byte.MaxValue);

		// Token: 0x04000684 RID: 1668
		public static readonly Color LightSalmon = new Color32(byte.MaxValue, 160, 122, byte.MaxValue);

		// Token: 0x04000685 RID: 1669
		public static readonly Color LightSeaGreen = new Color32(32, 178, 170, byte.MaxValue);

		// Token: 0x04000686 RID: 1670
		public static readonly Color LightSkyBlue = new Color32(135, 206, 250, byte.MaxValue);

		// Token: 0x04000687 RID: 1671
		public static readonly Color LightSlateGray = new Color32(119, 136, 153, byte.MaxValue);

		// Token: 0x04000688 RID: 1672
		public static readonly Color LightSteelBlue = new Color32(176, 196, 222, byte.MaxValue);

		// Token: 0x04000689 RID: 1673
		public static readonly Color LightYellow = new Color32(byte.MaxValue, byte.MaxValue, 224, byte.MaxValue);

		// Token: 0x0400068A RID: 1674
		public static readonly Color Lime = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x0400068B RID: 1675
		public static readonly Color LimeGreen = new Color32(50, 205, 50, byte.MaxValue);

		// Token: 0x0400068C RID: 1676
		public static readonly Color Linen = new Color32(250, 240, 230, byte.MaxValue);

		// Token: 0x0400068D RID: 1677
		public static readonly Color Magenta = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400068E RID: 1678
		public static readonly Color Maroon = new Color32(128, 0, 0, byte.MaxValue);

		// Token: 0x0400068F RID: 1679
		public static readonly Color MediumAquamarine = new Color32(102, 205, 170, byte.MaxValue);

		// Token: 0x04000690 RID: 1680
		public static readonly Color MediumBlue = new Color32(0, 0, 205, byte.MaxValue);

		// Token: 0x04000691 RID: 1681
		public static readonly Color MediumOrchid = new Color32(186, 85, 211, byte.MaxValue);

		// Token: 0x04000692 RID: 1682
		public static readonly Color MediumPurple = new Color32(147, 112, 219, byte.MaxValue);

		// Token: 0x04000693 RID: 1683
		public static readonly Color MediumSeaGreen = new Color32(60, 179, 113, byte.MaxValue);

		// Token: 0x04000694 RID: 1684
		public static readonly Color MediumSlateBlue = new Color32(123, 104, 238, byte.MaxValue);

		// Token: 0x04000695 RID: 1685
		public static readonly Color MediumSpringGreen = new Color32(0, 250, 154, byte.MaxValue);

		// Token: 0x04000696 RID: 1686
		public static readonly Color MediumTurquoise = new Color32(72, 209, 204, byte.MaxValue);

		// Token: 0x04000697 RID: 1687
		public static readonly Color MediumVioletRed = new Color32(199, 21, 133, byte.MaxValue);

		// Token: 0x04000698 RID: 1688
		public static readonly Color MidnightBlue = new Color32(25, 25, 112, byte.MaxValue);

		// Token: 0x04000699 RID: 1689
		public static readonly Color Mintcream = new Color32(245, byte.MaxValue, 250, byte.MaxValue);

		// Token: 0x0400069A RID: 1690
		public static readonly Color MistyRose = new Color32(byte.MaxValue, 228, 225, byte.MaxValue);

		// Token: 0x0400069B RID: 1691
		public static readonly Color Moccasin = new Color32(byte.MaxValue, 228, 181, byte.MaxValue);

		// Token: 0x0400069C RID: 1692
		public static readonly Color NavajoWhite = new Color32(byte.MaxValue, 222, 173, byte.MaxValue);

		// Token: 0x0400069D RID: 1693
		public static readonly Color Navy = new Color32(0, 0, 128, byte.MaxValue);

		// Token: 0x0400069E RID: 1694
		public static readonly Color OldLace = new Color32(253, 245, 230, byte.MaxValue);

		// Token: 0x0400069F RID: 1695
		public static readonly Color Olive = new Color32(128, 128, 0, byte.MaxValue);

		// Token: 0x040006A0 RID: 1696
		public static readonly Color Olivedrab = new Color32(107, 142, 35, byte.MaxValue);

		// Token: 0x040006A1 RID: 1697
		public static readonly Color Orange = new Color32(byte.MaxValue, 165, 0, byte.MaxValue);

		// Token: 0x040006A2 RID: 1698
		public static readonly Color Orangered = new Color32(byte.MaxValue, 69, 0, byte.MaxValue);

		// Token: 0x040006A3 RID: 1699
		public static readonly Color Orchid = new Color32(218, 112, 214, byte.MaxValue);

		// Token: 0x040006A4 RID: 1700
		public static readonly Color PaleGoldenrod = new Color32(238, 232, 170, byte.MaxValue);

		// Token: 0x040006A5 RID: 1701
		public static readonly Color PaleGreen = new Color32(152, 251, 152, byte.MaxValue);

		// Token: 0x040006A6 RID: 1702
		public static readonly Color PaleTurquoise = new Color32(175, 238, 238, byte.MaxValue);

		// Token: 0x040006A7 RID: 1703
		public static readonly Color PaleVioletred = new Color32(219, 112, 147, byte.MaxValue);

		// Token: 0x040006A8 RID: 1704
		public static readonly Color PapayaWhip = new Color32(byte.MaxValue, 239, 213, byte.MaxValue);

		// Token: 0x040006A9 RID: 1705
		public static readonly Color PeachPuff = new Color32(byte.MaxValue, 218, 185, byte.MaxValue);

		// Token: 0x040006AA RID: 1706
		public static readonly Color Peru = new Color32(205, 133, 63, byte.MaxValue);

		// Token: 0x040006AB RID: 1707
		public static readonly Color Pink = new Color32(byte.MaxValue, 192, 203, byte.MaxValue);

		// Token: 0x040006AC RID: 1708
		public static readonly Color Plum = new Color32(221, 160, 221, byte.MaxValue);

		// Token: 0x040006AD RID: 1709
		public static readonly Color PowderBlue = new Color32(176, 224, 230, byte.MaxValue);

		// Token: 0x040006AE RID: 1710
		public static readonly Color Purple = new Color32(128, 0, 128, byte.MaxValue);

		// Token: 0x040006AF RID: 1711
		public static readonly Color Red = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		// Token: 0x040006B0 RID: 1712
		public static readonly Color RosyBrown = new Color32(188, 143, 143, byte.MaxValue);

		// Token: 0x040006B1 RID: 1713
		public static readonly Color RoyalBlue = new Color32(65, 105, 225, byte.MaxValue);

		// Token: 0x040006B2 RID: 1714
		public static readonly Color SaddleBrown = new Color32(139, 69, 19, byte.MaxValue);

		// Token: 0x040006B3 RID: 1715
		public static readonly Color Salmon = new Color32(250, 128, 114, byte.MaxValue);

		// Token: 0x040006B4 RID: 1716
		public static readonly Color SandyBrown = new Color32(244, 164, 96, byte.MaxValue);

		// Token: 0x040006B5 RID: 1717
		public static readonly Color SeaGreen = new Color32(46, 139, 87, byte.MaxValue);

		// Token: 0x040006B6 RID: 1718
		public static readonly Color Seashell = new Color32(byte.MaxValue, 245, 238, byte.MaxValue);

		// Token: 0x040006B7 RID: 1719
		public static readonly Color Sienna = new Color32(160, 82, 45, byte.MaxValue);

		// Token: 0x040006B8 RID: 1720
		public static readonly Color Silver = new Color32(192, 192, 192, byte.MaxValue);

		// Token: 0x040006B9 RID: 1721
		public static readonly Color SkyBlue = new Color32(135, 206, 235, byte.MaxValue);

		// Token: 0x040006BA RID: 1722
		public static readonly Color SlateBlue = new Color32(106, 90, 205, byte.MaxValue);

		// Token: 0x040006BB RID: 1723
		public static readonly Color SlateGray = new Color32(112, 128, 144, byte.MaxValue);

		// Token: 0x040006BC RID: 1724
		public static readonly Color Snow = new Color32(byte.MaxValue, 250, 250, byte.MaxValue);

		// Token: 0x040006BD RID: 1725
		public static readonly Color SpringGreen = new Color32(0, byte.MaxValue, 127, byte.MaxValue);

		// Token: 0x040006BE RID: 1726
		public static readonly Color SteelBlue = new Color32(70, 130, 180, byte.MaxValue);

		// Token: 0x040006BF RID: 1727
		public static readonly Color Tan = new Color32(210, 180, 140, byte.MaxValue);

		// Token: 0x040006C0 RID: 1728
		public static readonly Color Teal = new Color32(0, 128, 128, byte.MaxValue);

		// Token: 0x040006C1 RID: 1729
		public static readonly Color Thistle = new Color32(216, 191, 216, byte.MaxValue);

		// Token: 0x040006C2 RID: 1730
		public static readonly Color Tomato = new Color32(byte.MaxValue, 99, 71, byte.MaxValue);

		// Token: 0x040006C3 RID: 1731
		public static readonly Color Turquoise = new Color32(64, 224, 208, byte.MaxValue);

		// Token: 0x040006C4 RID: 1732
		public static readonly Color Violet = new Color32(238, 130, 238, byte.MaxValue);

		// Token: 0x040006C5 RID: 1733
		public static readonly Color Wheat = new Color32(245, 222, 179, byte.MaxValue);

		// Token: 0x040006C6 RID: 1734
		public static readonly Color White = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x040006C7 RID: 1735
		public static readonly Color WhiteSmoke = new Color32(245, 245, 245, byte.MaxValue);

		// Token: 0x040006C8 RID: 1736
		public static readonly Color Yellow = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x040006C9 RID: 1737
		public static readonly Color YellowGreen = new Color32(154, 205, 50, byte.MaxValue);

		// Token: 0x040006CA RID: 1738
		public static Dictionary<int, Color> ColorDictionary;

		// Token: 0x0200041D RID: 1053
		public enum ColoringMode
		{
			// Token: 0x04001F22 RID: 7970
			Tint,
			// Token: 0x04001F23 RID: 7971
			Multiply,
			// Token: 0x04001F24 RID: 7972
			Replace,
			// Token: 0x04001F25 RID: 7973
			ReplaceKeepAlpha,
			// Token: 0x04001F26 RID: 7974
			Add
		}
	}
}
