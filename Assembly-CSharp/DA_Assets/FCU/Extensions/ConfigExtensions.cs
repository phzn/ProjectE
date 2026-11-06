using System;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200038C RID: 908
	public static class ConfigExtensions
	{
		// Token: 0x060017CD RID: 6093 RVA: 0x00097834 File Offset: 0x00095A34
		public static bool IsNova(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.MainSettings.UIFramework == UIFramework.NOVA;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00097849 File Offset: 0x00095A49
		public static bool IsUITK(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.MainSettings.UIFramework == UIFramework.UITK;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0009785E File Offset: 0x00095A5E
		public static bool IsUGUI(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.MainSettings.UIFramework == UIFramework.UGUI;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00097873 File Offset: 0x00095A73
		public static bool IsDebug(this FigmaConverterUnity fcu)
		{
			return FcuDebugSettings.Settings.HasFlag(FcuDebugSettingsFlags.DebugMode);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x0009788E File Offset: 0x00095A8E
		public static bool UsingAnyProceduralImage(this FigmaConverterUnity fcu)
		{
			return fcu.UsingJoshPui() || fcu.UsingDttPui() || fcu.UsingShapes2D() || fcu.UsingMPUIKit();
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000978B0 File Offset: 0x00095AB0
		public static bool UsingUnityButton(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ButtonSettings.ButtonComponent == ButtonComponent.UnityButton;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000978C5 File Offset: 0x00095AC5
		public static bool UsingDAButton(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ButtonSettings.ButtonComponent == ButtonComponent.DAButton;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000978DA File Offset: 0x00095ADA
		public static bool UsingTrueShadow(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ShadowSettings.ShadowComponent == ShadowComponent.TrueShadow;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000978EF File Offset: 0x00095AEF
		public static bool UsingUnityText(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.TextFontsSettings.TextComponent == TextComponent.UnityText;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00097904 File Offset: 0x00095B04
		public static bool UsingTextMesh(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.TextFontsSettings.TextComponent == TextComponent.TextMeshPro || fcu.Settings.TextFontsSettings.TextComponent == TextComponent.RTLTextMeshPro;
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0009792E File Offset: 0x00095B2E
		public static bool UsingRTLTextMeshPro(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.TextFontsSettings.TextComponent == TextComponent.RTLTextMeshPro;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00097943 File Offset: 0x00095B43
		public static bool UsingSpriteRenderer(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.SpriteRenderer;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00097958 File Offset: 0x00095B58
		public static bool UsingSvgImage(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.SvgImage;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0009796D File Offset: 0x00095B6D
		public static bool UsingSVG(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageFormat == ImageFormat.SVG;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00097982 File Offset: 0x00095B82
		public static bool UsingShapes2D(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.SubcShape;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00097997 File Offset: 0x00095B97
		public static bool UsingUnityImage(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.UnityImage;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000979AC File Offset: 0x00095BAC
		public static bool UsingRawImage(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.RawImage;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x000979C1 File Offset: 0x00095BC1
		public static bool UsingJoshPui(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.ProceduralImage;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x000979D6 File Offset: 0x00095BD6
		public static bool UsingDttPui(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.RoundedImage;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x000979EB File Offset: 0x00095BEB
		public static bool UsingUIBlock2D(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.UIBlock2D;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00097A00 File Offset: 0x00095C00
		public static bool UsingMPUIKit(this FigmaConverterUnity fcu)
		{
			return fcu.Settings.ImageSpritesSettings.ImageComponent == ImageComponent.MPImage;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public static bool UseImageLinearMaterial(this FigmaConverterUnity fcu)
		{
			return false;
		}
	}
}
