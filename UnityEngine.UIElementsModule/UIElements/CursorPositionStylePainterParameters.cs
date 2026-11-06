using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000327 RID: 807
	internal struct CursorPositionStylePainterParameters
	{
		// Token: 0x06001B4B RID: 6987 RVA: 0x0006AFFC File Offset: 0x000691FC
		public unsafe static CursorPositionStylePainterParameters GetDefault(VisualElement ve, string text)
		{
			ComputedStyle computedStyle = *ve.computedStyle;
			return new CursorPositionStylePainterParameters
			{
				rect = ve.contentRect,
				text = text,
				font = TextUtilities.GetFont(ve),
				fontSize = (int)computedStyle.fontSize.value,
				fontStyle = computedStyle.unityFontStyleAndWeight,
				anchor = computedStyle.unityTextAlign,
				wordWrapWidth = ((computedStyle.whiteSpace == WhiteSpace.Normal) ? ve.contentRect.width : 0f),
				richText = false,
				cursorIndex = 0
			};
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0006B0B4 File Offset: 0x000692B4
		internal TextNativeSettings GetTextNativeSettings(float scaling)
		{
			return new TextNativeSettings
			{
				text = this.text,
				font = this.font,
				size = this.fontSize,
				scaling = scaling,
				style = this.fontStyle,
				color = Color.white,
				anchor = this.anchor,
				wordWrap = true,
				wordWrapWidth = this.wordWrapWidth,
				richText = this.richText
			};
		}

		// Token: 0x04000B5C RID: 2908
		public Rect rect;

		// Token: 0x04000B5D RID: 2909
		public string text;

		// Token: 0x04000B5E RID: 2910
		public Font font;

		// Token: 0x04000B5F RID: 2911
		public int fontSize;

		// Token: 0x04000B60 RID: 2912
		public FontStyle fontStyle;

		// Token: 0x04000B61 RID: 2913
		public TextAnchor anchor;

		// Token: 0x04000B62 RID: 2914
		public float wordWrapWidth;

		// Token: 0x04000B63 RID: 2915
		public bool richText;

		// Token: 0x04000B64 RID: 2916
		public int cursorIndex;
	}
}
