using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000039 RID: 57
	internal struct WordWrapState
	{
		// Token: 0x04000269 RID: 617
		public int previousWordBreak;

		// Token: 0x0400026A RID: 618
		public int totalCharacterCount;

		// Token: 0x0400026B RID: 619
		public int visibleCharacterCount;

		// Token: 0x0400026C RID: 620
		public int visibleSpaceCount;

		// Token: 0x0400026D RID: 621
		public int visibleSpriteCount;

		// Token: 0x0400026E RID: 622
		public int visibleLinkCount;

		// Token: 0x0400026F RID: 623
		public int firstCharacterIndex;

		// Token: 0x04000270 RID: 624
		public int firstVisibleCharacterIndex;

		// Token: 0x04000271 RID: 625
		public int lastCharacterIndex;

		// Token: 0x04000272 RID: 626
		public int lastVisibleCharIndex;

		// Token: 0x04000273 RID: 627
		public int lineNumber;

		// Token: 0x04000274 RID: 628
		public float maxCapHeight;

		// Token: 0x04000275 RID: 629
		public float maxAscender;

		// Token: 0x04000276 RID: 630
		public float maxDescender;

		// Token: 0x04000277 RID: 631
		public float maxLineAscender;

		// Token: 0x04000278 RID: 632
		public float maxLineDescender;

		// Token: 0x04000279 RID: 633
		public float startOfLineAscender;

		// Token: 0x0400027A RID: 634
		public float xAdvance;

		// Token: 0x0400027B RID: 635
		public float preferredWidth;

		// Token: 0x0400027C RID: 636
		public float preferredHeight;

		// Token: 0x0400027D RID: 637
		public float previousLineScale;

		// Token: 0x0400027E RID: 638
		public float pageAscender;

		// Token: 0x0400027F RID: 639
		public int wordCount;

		// Token: 0x04000280 RID: 640
		public FontStyles fontStyle;

		// Token: 0x04000281 RID: 641
		public float fontScale;

		// Token: 0x04000282 RID: 642
		public float fontScaleMultiplier;

		// Token: 0x04000283 RID: 643
		public int italicAngle;

		// Token: 0x04000284 RID: 644
		public float currentFontSize;

		// Token: 0x04000285 RID: 645
		public float baselineOffset;

		// Token: 0x04000286 RID: 646
		public float lineOffset;

		// Token: 0x04000287 RID: 647
		public TextInfo textInfo;

		// Token: 0x04000288 RID: 648
		public LineInfo lineInfo;

		// Token: 0x04000289 RID: 649
		public Color32 vertexColor;

		// Token: 0x0400028A RID: 650
		public Color32 underlineColor;

		// Token: 0x0400028B RID: 651
		public Color32 strikethroughColor;

		// Token: 0x0400028C RID: 652
		public Color32 highlightColor;

		// Token: 0x0400028D RID: 653
		public HighlightState highlightState;

		// Token: 0x0400028E RID: 654
		public FontStyleStack basicStyleStack;

		// Token: 0x0400028F RID: 655
		public TextProcessingStack<int> italicAngleStack;

		// Token: 0x04000290 RID: 656
		public TextProcessingStack<Color32> colorStack;

		// Token: 0x04000291 RID: 657
		public TextProcessingStack<Color32> underlineColorStack;

		// Token: 0x04000292 RID: 658
		public TextProcessingStack<Color32> strikethroughColorStack;

		// Token: 0x04000293 RID: 659
		public TextProcessingStack<Color32> highlightColorStack;

		// Token: 0x04000294 RID: 660
		public TextProcessingStack<HighlightState> highlightStateStack;

		// Token: 0x04000295 RID: 661
		public TextProcessingStack<TextColorGradient> colorGradientStack;

		// Token: 0x04000296 RID: 662
		public TextProcessingStack<float> sizeStack;

		// Token: 0x04000297 RID: 663
		public TextProcessingStack<float> indentStack;

		// Token: 0x04000298 RID: 664
		public TextProcessingStack<TextFontWeight> fontWeightStack;

		// Token: 0x04000299 RID: 665
		public TextProcessingStack<int> styleStack;

		// Token: 0x0400029A RID: 666
		public TextProcessingStack<float> baselineStack;

		// Token: 0x0400029B RID: 667
		public TextProcessingStack<int> actionStack;

		// Token: 0x0400029C RID: 668
		public TextProcessingStack<MaterialReference> materialReferenceStack;

		// Token: 0x0400029D RID: 669
		public TextProcessingStack<TextAlignment> lineJustificationStack;

		// Token: 0x0400029E RID: 670
		public int lastBaseGlyphIndex;

		// Token: 0x0400029F RID: 671
		public int spriteAnimationId;

		// Token: 0x040002A0 RID: 672
		public FontAsset currentFontAsset;

		// Token: 0x040002A1 RID: 673
		public SpriteAsset currentSpriteAsset;

		// Token: 0x040002A2 RID: 674
		public Material currentMaterial;

		// Token: 0x040002A3 RID: 675
		public int currentMaterialIndex;

		// Token: 0x040002A4 RID: 676
		public Extents meshExtents;

		// Token: 0x040002A5 RID: 677
		public bool tagNoParsing;

		// Token: 0x040002A6 RID: 678
		public bool isNonBreakingSpace;

		// Token: 0x040002A7 RID: 679
		public bool isDrivenLineSpacing;

		// Token: 0x040002A8 RID: 680
		public Vector3 fxScale;

		// Token: 0x040002A9 RID: 681
		public Quaternion fxRotation;
	}
}
