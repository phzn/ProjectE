using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000023 RID: 35
	internal struct TextElementInfo
	{
		// Token: 0x0600011D RID: 285 RVA: 0x000086F8 File Offset: 0x000068F8
		public override string ToString()
		{
			return string.Format("{0}: {1}\n{2}: {3}\n{4}: {5}\n{6}: {7}\n{8}: {9}\n{10}: {11}\n{12}: {13}\n{14}: {15}\n{16}: {17}\n{18}: {19}\n{20}: {21}\n{22}: {23}\n{24}: {25}\n{26}: {27}\n{28}: {29}\n{30}: {31}\n{32}: {33}\n{34}: {35}\n{36}: {37}\n{38}: {39}\n{40}: {41}\n{42}: {43}\n{44}: {45}\n{46}: {47}\n{48}: {49}\n{50}: {51}\n{52}: {53}\n{54}: {55}\n{56}: {57}\n{58}: {59}\n{60}: {61}\n{62}: {63}\n{64}: {65}\n{66}: {67}\n{68}: {69}\n{70}: {71}\n{72}: {73}\n{74}: {75}\n{76}: {77}\n{78}: {79}\n{80}: {81}\n{82}: {83}\n{84}: {85}", new object[]
			{
				"character", this.character, "index", this.index, "elementType", this.elementType, "stringLength", this.stringLength, "textElement", this.textElement,
				"alternativeGlyph", this.alternativeGlyph, "fontAsset", this.fontAsset, "spriteAsset", this.spriteAsset, "spriteIndex", this.spriteIndex, "material", this.material,
				"materialReferenceIndex", this.materialReferenceIndex, "isUsingAlternateTypeface", this.isUsingAlternateTypeface, "pointSize", this.pointSize, "lineNumber", this.lineNumber, "pageNumber", this.pageNumber,
				"vertexIndex", this.vertexIndex, "vertexTopLeft", this.vertexTopLeft, "vertexBottomLeft", this.vertexBottomLeft, "vertexTopRight", this.vertexTopRight, "vertexBottomRight", this.vertexBottomRight,
				"topLeft", this.topLeft, "bottomLeft", this.bottomLeft, "topRight", this.topRight, "bottomRight", this.bottomRight, "origin", this.origin,
				"ascender", this.ascender, "baseLine", this.baseLine, "descender", this.descender, "adjustedAscender", this.adjustedAscender, "adjustedDescender", this.adjustedDescender,
				"adjustedHorizontalAdvance", this.adjustedHorizontalAdvance, "xAdvance", this.xAdvance, "aspectRatio", this.aspectRatio, "scale", this.scale, "color", this.color,
				"underlineColor", this.underlineColor, "underlineVertexIndex", this.underlineVertexIndex, "strikethroughColor", this.strikethroughColor, "strikethroughVertexIndex", this.strikethroughVertexIndex, "highlightColor", this.highlightColor,
				"highlightState", this.highlightState, "style", this.style, "isVisible", this.isVisible
			});
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008B04 File Offset: 0x00006D04
		internal string ToStringTest()
		{
			return string.Concat(new string[]
			{
				"topLeft.x: ",
				this.topLeft.x.ToString("F4"),
				"\n topLeft.y: ",
				this.topLeft.y.ToString("F4"),
				"\n topRight.x: ",
				this.topRight.x.ToString("F4"),
				"\n topRight.y: ",
				this.topRight.y.ToString("F4"),
				"\n  bottomLeft.x: ",
				this.bottomLeft.x.ToString("F4"),
				"\n bottomLeft.y: ",
				this.bottomLeft.y.ToString("F4"),
				"\n  bottomRight.x: ",
				this.bottomRight.x.ToString("F4"),
				"\n bottomRight.y: ",
				this.bottomRight.y.ToString("F4"),
				"\norigin: ",
				this.origin.ToString("F4"),
				"\nxAdvance: ",
				this.xAdvance.ToString("F4"),
				"\n"
			});
		}

		// Token: 0x040000E6 RID: 230
		public char character;

		// Token: 0x040000E7 RID: 231
		public int index;

		// Token: 0x040000E8 RID: 232
		public TextElementType elementType;

		// Token: 0x040000E9 RID: 233
		public int stringLength;

		// Token: 0x040000EA RID: 234
		public TextElement textElement;

		// Token: 0x040000EB RID: 235
		public Glyph alternativeGlyph;

		// Token: 0x040000EC RID: 236
		public FontAsset fontAsset;

		// Token: 0x040000ED RID: 237
		public SpriteAsset spriteAsset;

		// Token: 0x040000EE RID: 238
		public int spriteIndex;

		// Token: 0x040000EF RID: 239
		public Material material;

		// Token: 0x040000F0 RID: 240
		public int materialReferenceIndex;

		// Token: 0x040000F1 RID: 241
		public bool isUsingAlternateTypeface;

		// Token: 0x040000F2 RID: 242
		public float pointSize;

		// Token: 0x040000F3 RID: 243
		public int lineNumber;

		// Token: 0x040000F4 RID: 244
		public int pageNumber;

		// Token: 0x040000F5 RID: 245
		public int vertexIndex;

		// Token: 0x040000F6 RID: 246
		public TextVertex vertexTopLeft;

		// Token: 0x040000F7 RID: 247
		public TextVertex vertexBottomLeft;

		// Token: 0x040000F8 RID: 248
		public TextVertex vertexTopRight;

		// Token: 0x040000F9 RID: 249
		public TextVertex vertexBottomRight;

		// Token: 0x040000FA RID: 250
		public Vector3 topLeft;

		// Token: 0x040000FB RID: 251
		public Vector3 bottomLeft;

		// Token: 0x040000FC RID: 252
		public Vector3 topRight;

		// Token: 0x040000FD RID: 253
		public Vector3 bottomRight;

		// Token: 0x040000FE RID: 254
		public float origin;

		// Token: 0x040000FF RID: 255
		public float ascender;

		// Token: 0x04000100 RID: 256
		public float baseLine;

		// Token: 0x04000101 RID: 257
		public float descender;

		// Token: 0x04000102 RID: 258
		internal float adjustedAscender;

		// Token: 0x04000103 RID: 259
		internal float adjustedDescender;

		// Token: 0x04000104 RID: 260
		internal float adjustedHorizontalAdvance;

		// Token: 0x04000105 RID: 261
		public float xAdvance;

		// Token: 0x04000106 RID: 262
		public float aspectRatio;

		// Token: 0x04000107 RID: 263
		public float scale;

		// Token: 0x04000108 RID: 264
		public Color32 color;

		// Token: 0x04000109 RID: 265
		public Color32 underlineColor;

		// Token: 0x0400010A RID: 266
		public int underlineVertexIndex;

		// Token: 0x0400010B RID: 267
		public Color32 strikethroughColor;

		// Token: 0x0400010C RID: 268
		public int strikethroughVertexIndex;

		// Token: 0x0400010D RID: 269
		public Color32 highlightColor;

		// Token: 0x0400010E RID: 270
		public HighlightState highlightState;

		// Token: 0x0400010F RID: 271
		public FontStyles style;

		// Token: 0x04000110 RID: 272
		public bool isVisible;
	}
}
