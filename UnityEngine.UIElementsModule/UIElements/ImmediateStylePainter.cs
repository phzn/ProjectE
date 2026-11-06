using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.UIElements
{
	// Token: 0x02000279 RID: 633
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/ImmediateStylePainter.h")]
	[StructLayout(LayoutKind.Sequential)]
	internal class ImmediateStylePainter
	{
		// Token: 0x060011E9 RID: 4585 RVA: 0x0004106A File Offset: 0x0003F26A
		internal static void DrawRect(Rect screenRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
		{
			ImmediateStylePainter.DrawRect_Injected(ref screenRect, ref color, ref borderWidths, ref borderRadiuses);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004107C File Offset: 0x0003F27C
		internal static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder, bool usePremultiplyAlpha)
		{
			ImmediateStylePainter.DrawTexture_Injected(ref screenRect, texture, ref sourceRect, ref color, ref borderWidths, ref borderRadiuses, leftBorder, topBorder, rightBorder, bottomBorder, usePremultiplyAlpha);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000410A4 File Offset: 0x0003F2A4
		internal static void DrawText(Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping)
		{
			ImmediateStylePainter.DrawText_Injected(ref screenRect, text, font, fontSize, fontStyle, ref fontColor, anchor, wordWrap, wordWrapWidth, richText, textClipping);
		}

		// Token: 0x060011ED RID: 4589
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawRect_Injected(ref Rect screenRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses);

		// Token: 0x060011EE RID: 4590
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTexture_Injected(ref Rect screenRect, Texture texture, ref Rect sourceRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder, bool usePremultiplyAlpha);

		// Token: 0x060011EF RID: 4591
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawText_Injected(ref Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, ref Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping);
	}
}
