using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000025 RID: 37
	internal class TextGenerationSettings : IEquatable<TextGenerationSettings>
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00008DAC File Offset: 0x00006FAC
		public bool Equals(TextGenerationSettings other)
		{
			bool flag = other == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this == other;
				flag2 = flag3 || (this.text == other.text && this.screenRect.Equals(other.screenRect) && this.margins.Equals(other.margins) && this.scale.Equals(other.scale) && object.Equals(this.fontAsset, other.fontAsset) && object.Equals(this.material, other.material) && object.Equals(this.spriteAsset, other.spriteAsset) && object.Equals(this.styleSheet, other.styleSheet) && this.fontStyle == other.fontStyle && object.Equals(this.textSettings, other.textSettings) && this.textAlignment == other.textAlignment && this.overflowMode == other.overflowMode && this.wordWrap == other.wordWrap && this.wordWrappingRatio.Equals(other.wordWrappingRatio) && this.color.Equals(other.color) && object.Equals(this.fontColorGradient, other.fontColorGradient) && object.Equals(this.fontColorGradientPreset, other.fontColorGradientPreset) && this.tintSprites == other.tintSprites && this.overrideRichTextColors == other.overrideRichTextColors && this.shouldConvertToLinearSpace == other.shouldConvertToLinearSpace && this.fontSize.Equals(other.fontSize) && this.autoSize == other.autoSize && this.fontSizeMin.Equals(other.fontSizeMin) && this.fontSizeMax.Equals(other.fontSizeMax) && this.enableKerning == other.enableKerning && this.richText == other.richText && this.isRightToLeft == other.isRightToLeft && this.extraPadding == other.extraPadding && this.parseControlCharacters == other.parseControlCharacters && this.isOrthographic == other.isOrthographic && this.tagNoParsing == other.tagNoParsing && this.characterSpacing.Equals(other.characterSpacing) && this.wordSpacing.Equals(other.wordSpacing) && this.lineSpacing.Equals(other.lineSpacing) && this.paragraphSpacing.Equals(other.paragraphSpacing) && this.lineSpacingMax.Equals(other.lineSpacingMax) && this.textWrappingMode == other.textWrappingMode && this.maxVisibleCharacters == other.maxVisibleCharacters && this.maxVisibleWords == other.maxVisibleWords && this.maxVisibleLines == other.maxVisibleLines && this.firstVisibleCharacter == other.firstVisibleCharacter && this.useMaxVisibleDescender == other.useMaxVisibleDescender && this.fontWeight == other.fontWeight && this.pageToDisplay == other.pageToDisplay && this.horizontalMapping == other.horizontalMapping && this.verticalMapping == other.verticalMapping && this.uvLineOffset.Equals(other.uvLineOffset) && this.geometrySortingOrder == other.geometrySortingOrder && this.inverseYAxis == other.inverseYAxis && this.charWidthMaxAdj.Equals(other.charWidthMaxAdj) && this.inputSource == other.inputSource);
			}
			return flag2;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000919C File Offset: 0x0000739C
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this == obj;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = obj.GetType() != base.GetType();
					flag2 = !flag4 && this.Equals((TextGenerationSettings)obj);
				}
			}
			return flag2;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000091EC File Offset: 0x000073EC
		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			hashCode.Add<string>(this.text);
			hashCode.Add<Rect>(this.screenRect);
			hashCode.Add<Vector4>(this.margins);
			hashCode.Add<float>(this.scale);
			hashCode.Add<FontAsset>(this.fontAsset);
			hashCode.Add<Material>(this.material);
			hashCode.Add<SpriteAsset>(this.spriteAsset);
			hashCode.Add<TextStyleSheet>(this.styleSheet);
			hashCode.Add<int>((int)this.fontStyle);
			hashCode.Add<TextSettings>(this.textSettings);
			hashCode.Add<int>((int)this.textAlignment);
			hashCode.Add<int>((int)this.overflowMode);
			hashCode.Add<bool>(this.wordWrap);
			hashCode.Add<float>(this.wordWrappingRatio);
			hashCode.Add<Color>(this.color);
			hashCode.Add<TextColorGradient>(this.fontColorGradient);
			hashCode.Add<TextColorGradient>(this.fontColorGradientPreset);
			hashCode.Add<bool>(this.tintSprites);
			hashCode.Add<bool>(this.overrideRichTextColors);
			hashCode.Add<bool>(this.shouldConvertToLinearSpace);
			hashCode.Add<float>(this.fontSize);
			hashCode.Add<bool>(this.autoSize);
			hashCode.Add<float>(this.fontSizeMin);
			hashCode.Add<float>(this.fontSizeMax);
			hashCode.Add<bool>(this.enableKerning);
			hashCode.Add<bool>(this.richText);
			hashCode.Add<bool>(this.isRightToLeft);
			hashCode.Add<float>(this.extraPadding);
			hashCode.Add<bool>(this.parseControlCharacters);
			hashCode.Add<bool>(this.isOrthographic);
			hashCode.Add<bool>(this.tagNoParsing);
			hashCode.Add<float>(this.characterSpacing);
			hashCode.Add<float>(this.wordSpacing);
			hashCode.Add<float>(this.lineSpacing);
			hashCode.Add<float>(this.paragraphSpacing);
			hashCode.Add<float>(this.lineSpacingMax);
			hashCode.Add<int>((int)this.textWrappingMode);
			hashCode.Add<int>(this.maxVisibleCharacters);
			hashCode.Add<int>(this.maxVisibleWords);
			hashCode.Add<int>(this.maxVisibleLines);
			hashCode.Add<int>(this.firstVisibleCharacter);
			hashCode.Add<bool>(this.useMaxVisibleDescender);
			hashCode.Add<int>((int)this.fontWeight);
			hashCode.Add<int>(this.pageToDisplay);
			hashCode.Add<int>((int)this.horizontalMapping);
			hashCode.Add<int>((int)this.verticalMapping);
			hashCode.Add<float>(this.uvLineOffset);
			hashCode.Add<int>((int)this.geometrySortingOrder);
			hashCode.Add<bool>(this.inverseYAxis);
			hashCode.Add<float>(this.charWidthMaxAdj);
			hashCode.Add<int>((int)this.inputSource);
			return hashCode.ToHashCode();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000094D8 File Offset: 0x000076D8
		public static bool operator ==(TextGenerationSettings left, TextGenerationSettings right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000094F4 File Offset: 0x000076F4
		public static bool operator !=(TextGenerationSettings left, TextGenerationSettings right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009510 File Offset: 0x00007710
		public override string ToString()
		{
			return string.Format("{0}: {1}\n {2}: {3}\n {4}: {5}\n {6}: {7}\n {8}: {9}\n {10}: {11}\n {12}: {13}\n {14}: {15}\n {16}: {17}\n {18}: {19}\n {20}: {21}\n {22}: {23}\n {24}: {25}\n {26}: {27}\n {28}: {29}\n {30}: {31}\n {32}: {33}\n {34}: {35}\n {36}: {37}\n {38}: {39}\n {40}: {41}\n {42}: {43}\n {44}: {45}\n {46}: {47}\n {48}: {49}\n {50}: {51}\n {52}: {53}\n {54}: {55}\n {56}: {57}\n {58}: {59}\n {60}: {61}\n {62}: {63}\n {64}: {65}\n {66}: {67}\n {68}: {69}\n {70}: {71}\n {72}: {73}\n {74}: {75}\n {76}: {77}\n {78}: {79}\n {80}: {81}\n {82}: {83}\n {84}: {85}\n {86}: {87}\n {88}: {89}\n {90}: {91}\n {92}: {93}\n {94}: {95}\n {96}: {97}\n {98}: {99}\n {100}: {101}", new object[]
			{
				"text", this.text, "screenRect", this.screenRect, "margins", this.margins, "scale", this.scale, "fontAsset", this.fontAsset,
				"material", this.material, "spriteAsset", this.spriteAsset, "styleSheet", this.styleSheet, "fontStyle", this.fontStyle, "textSettings", this.textSettings,
				"textAlignment", this.textAlignment, "overflowMode", this.overflowMode, "wordWrap", this.wordWrap, "wordWrappingRatio", this.wordWrappingRatio, "color", this.color,
				"fontColorGradient", this.fontColorGradient, "fontColorGradientPreset", this.fontColorGradientPreset, "tintSprites", this.tintSprites, "overrideRichTextColors", this.overrideRichTextColors, "shouldConvertToLinearSpace", this.shouldConvertToLinearSpace,
				"fontSize", this.fontSize, "autoSize", this.autoSize, "fontSizeMin", this.fontSizeMin, "fontSizeMax", this.fontSizeMax, "enableKerning", this.enableKerning,
				"richText", this.richText, "isRightToLeft", this.isRightToLeft, "extraPadding", this.extraPadding, "parseControlCharacters", this.parseControlCharacters, "isOrthographic", this.isOrthographic,
				"tagNoParsing", this.tagNoParsing, "characterSpacing", this.characterSpacing, "wordSpacing", this.wordSpacing, "lineSpacing", this.lineSpacing, "paragraphSpacing", this.paragraphSpacing,
				"lineSpacingMax", this.lineSpacingMax, "textWrappingMode", this.textWrappingMode, "maxVisibleCharacters", this.maxVisibleCharacters, "maxVisibleWords", this.maxVisibleWords, "maxVisibleLines", this.maxVisibleLines,
				"firstVisibleCharacter", this.firstVisibleCharacter, "useMaxVisibleDescender", this.useMaxVisibleDescender, "fontWeight", this.fontWeight, "pageToDisplay", this.pageToDisplay, "horizontalMapping", this.horizontalMapping,
				"verticalMapping", this.verticalMapping, "uvLineOffset", this.uvLineOffset, "geometrySortingOrder", this.geometrySortingOrder, "inverseYAxis", this.inverseYAxis, "charWidthMaxAdj", this.charWidthMaxAdj,
				"inputSource", this.inputSource
			});
		}

		// Token: 0x0400011D RID: 285
		public string text;

		// Token: 0x0400011E RID: 286
		public Rect screenRect;

		// Token: 0x0400011F RID: 287
		public Vector4 margins;

		// Token: 0x04000120 RID: 288
		public float scale = 1f;

		// Token: 0x04000121 RID: 289
		public FontAsset fontAsset;

		// Token: 0x04000122 RID: 290
		public Material material;

		// Token: 0x04000123 RID: 291
		public SpriteAsset spriteAsset;

		// Token: 0x04000124 RID: 292
		public TextStyleSheet styleSheet;

		// Token: 0x04000125 RID: 293
		public FontStyles fontStyle = FontStyles.Normal;

		// Token: 0x04000126 RID: 294
		public TextSettings textSettings;

		// Token: 0x04000127 RID: 295
		public TextAlignment textAlignment = TextAlignment.TopLeft;

		// Token: 0x04000128 RID: 296
		public TextOverflowMode overflowMode = TextOverflowMode.Overflow;

		// Token: 0x04000129 RID: 297
		public bool wordWrap = false;

		// Token: 0x0400012A RID: 298
		public float wordWrappingRatio;

		// Token: 0x0400012B RID: 299
		public Color color = Color.white;

		// Token: 0x0400012C RID: 300
		public TextColorGradient fontColorGradient;

		// Token: 0x0400012D RID: 301
		public TextColorGradient fontColorGradientPreset;

		// Token: 0x0400012E RID: 302
		public bool tintSprites;

		// Token: 0x0400012F RID: 303
		public bool overrideRichTextColors;

		// Token: 0x04000130 RID: 304
		public bool shouldConvertToLinearSpace = true;

		// Token: 0x04000131 RID: 305
		public float fontSize = 18f;

		// Token: 0x04000132 RID: 306
		public bool autoSize;

		// Token: 0x04000133 RID: 307
		public float fontSizeMin;

		// Token: 0x04000134 RID: 308
		public float fontSizeMax;

		// Token: 0x04000135 RID: 309
		public bool enableKerning = true;

		// Token: 0x04000136 RID: 310
		public bool richText;

		// Token: 0x04000137 RID: 311
		public bool isRightToLeft;

		// Token: 0x04000138 RID: 312
		public float extraPadding = 6f;

		// Token: 0x04000139 RID: 313
		public bool parseControlCharacters = true;

		// Token: 0x0400013A RID: 314
		public bool isOrthographic = true;

		// Token: 0x0400013B RID: 315
		public bool tagNoParsing = false;

		// Token: 0x0400013C RID: 316
		public float characterSpacing;

		// Token: 0x0400013D RID: 317
		public float wordSpacing;

		// Token: 0x0400013E RID: 318
		public float lineSpacing;

		// Token: 0x0400013F RID: 319
		public float paragraphSpacing;

		// Token: 0x04000140 RID: 320
		public float lineSpacingMax;

		// Token: 0x04000141 RID: 321
		public TextWrappingMode textWrappingMode = TextWrappingMode.Normal;

		// Token: 0x04000142 RID: 322
		public int maxVisibleCharacters = 99999;

		// Token: 0x04000143 RID: 323
		public int maxVisibleWords = 99999;

		// Token: 0x04000144 RID: 324
		public int maxVisibleLines = 99999;

		// Token: 0x04000145 RID: 325
		public int firstVisibleCharacter = 0;

		// Token: 0x04000146 RID: 326
		public bool useMaxVisibleDescender;

		// Token: 0x04000147 RID: 327
		public TextFontWeight fontWeight = TextFontWeight.Regular;

		// Token: 0x04000148 RID: 328
		public int pageToDisplay = 1;

		// Token: 0x04000149 RID: 329
		public TextureMapping horizontalMapping = TextureMapping.Character;

		// Token: 0x0400014A RID: 330
		public TextureMapping verticalMapping = TextureMapping.Character;

		// Token: 0x0400014B RID: 331
		public float uvLineOffset;

		// Token: 0x0400014C RID: 332
		public VertexSortingOrder geometrySortingOrder = VertexSortingOrder.Normal;

		// Token: 0x0400014D RID: 333
		public bool inverseYAxis;

		// Token: 0x0400014E RID: 334
		public float charWidthMaxAdj;

		// Token: 0x0400014F RID: 335
		internal TextInputSource inputSource = TextInputSource.TextString;
	}
}
