using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200038D RID: 909
	public static class FcuExtensions
	{
		// Token: 0x060017E3 RID: 6115 RVA: 0x00097A18 File Offset: 0x00095C18
		public static bool IsCancellationRequested(this FigmaConverterUnity fcu, TokenType type)
		{
			CancellationTokenSource token = fcu.CancellationTokenController.GetToken(type);
			return token != null && token.IsCancellationRequested;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00097A40 File Offset: 0x00095C40
		public static CancellationToken GetToken(this FigmaConverterUnity fcu, TokenType type)
		{
			CancellationTokenSource token = fcu.CancellationTokenController.GetToken(type);
			if (token == null)
			{
				return default(CancellationToken);
			}
			return token.Token;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0002AE8C File Offset: 0x0002908C
		public static bool IsJsonNetExists(this FigmaConverterUnity fcu)
		{
			return true;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00097A70 File Offset: 0x00095C70
		public static async Task ReEnableRectTransform(this FigmaConverterUnity fcu)
		{
			fcu.gameObject.SetActive(false);
			await Task.Delay(100);
			fcu.gameObject.SetActive(true);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00097AB4 File Offset: 0x00095CB4
		public static Type GetCurrentImageType(this FigmaConverterUnity fcu)
		{
			ImageComponent imageComponent = fcu.Settings.ImageSpritesSettings.ImageComponent;
			if (imageComponent == ImageComponent.UnityImage)
			{
				return typeof(Image);
			}
			if (imageComponent != ImageComponent.RawImage)
			{
				return null;
			}
			return typeof(RawImage);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00097AF4 File Offset: 0x00095CF4
		public static Type GetCurrentTextType(this FigmaConverterUnity fcu)
		{
			TextComponent textComponent = fcu.Settings.TextFontsSettings.TextComponent;
			if (textComponent == TextComponent.UnityText)
			{
				return typeof(Text);
			}
			if (textComponent != TextComponent.TextMeshPro)
			{
				return null;
			}
			return typeof(TextMeshProUGUI);
		}
	}
}
