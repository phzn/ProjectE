using System;
using UnityEngine;

namespace MiragePro.Impostors.Elements
{
	// Token: 0x02000270 RID: 624
	public class ImpostorTextureUtilities
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x0006C904 File Offset: 0x0006AB04
		public static Texture2D ResizeSquared(Texture2D texture2D, int targetSide)
		{
			RenderTexture renderTexture = new RenderTexture(targetSide, targetSide, 24);
			RenderTexture.active = renderTexture;
			Graphics.Blit(texture2D, renderTexture);
			Texture2D texture2D2 = new Texture2D(targetSide, targetSide, texture2D.format, false);
			texture2D2.ReadPixels(new Rect(0f, 0f, (float)targetSide, (float)targetSide), 0, 0);
			texture2D2.Apply();
			return texture2D2;
		}
	}
}
