using System;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000036 RID: 54
	public static class TextureExtensions
	{
		// Token: 0x06000110 RID: 272 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public static Texture2D Merge(this Texture2D largeTexture, Texture2D smallTexture)
		{
			Texture2D texture2D = new Texture2D(largeTexture.width, largeTexture.height, TextureFormat.RGBA32, false);
			Color[] pixels = largeTexture.GetPixels();
			texture2D.SetPixels(pixels);
			int num = (largeTexture.width - smallTexture.width) / 2;
			int num2 = (largeTexture.height - smallTexture.height) / 2;
			Color[] pixels2 = smallTexture.GetPixels();
			for (int i = 0; i < smallTexture.width; i++)
			{
				for (int j = 0; j < smallTexture.height; j++)
				{
					int num3 = num + i;
					int num4 = num2 + j;
					Color color = pixels2[i + j * smallTexture.width];
					Color color2 = Color.Lerp(texture2D.GetPixel(num3, num4), color, color.a);
					texture2D.SetPixel(num3, num4, color2);
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005E78 File Offset: 0x00004078
		public static void Colorize(this Texture2D texture, Color32 color)
		{
			Color32[] pixels = texture.GetPixels32();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = new Color32(color.r, color.g, color.b, pixels[i].a);
			}
			texture.SetPixels32(pixels);
			texture.Apply();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005ED4 File Offset: 0x000040D4
		public static void Resize(this Texture2D texture2D, Vector2Int targetSize, int depth, FilterMode filterMode, RenderTextureFormat rtFormat)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(targetSize.x, targetSize.y, depth, rtFormat, RenderTextureReadWrite.Default);
			RenderTexture.active = temporary;
			Graphics.Blit(texture2D, temporary);
			texture2D.Reinitialize(targetSize.x, targetSize.y, texture2D.format, false);
			texture2D.filterMode = filterMode;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)targetSize.x, (float)targetSize.y), 0, 0);
			texture2D.Apply();
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.active = null;
		}
	}
}
