using System;
using UnityEngine;

namespace Features.MallHack
{
	// Token: 0x0200102B RID: 4139
	internal static class HackMeTextureComposer
	{
		// Token: 0x06007D76 RID: 32118 RVA: 0x003365AC File Offset: 0x003347AC
		public static Texture2D CopyPhoto(Texture source)
		{
			if (source == null)
			{
				return null;
			}
			float num = Mathf.Min(1f, 1024f / (float)Mathf.Max(source.width, source.height));
			int num2 = Mathf.Max(1, Mathf.RoundToInt((float)source.width * num));
			int num3 = Mathf.Max(1, Mathf.RoundToInt((float)source.height * num));
			return HackMeTextureComposer.ReadTexture(source, num2, num3, "HackMe Photo");
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x00336620 File Offset: 0x00334820
		public static Texture2D CompositeAtlas(Texture atlas, Texture photo, Rect normalizedScreenRect, bool rotate180)
		{
			if (atlas == null || photo == null)
			{
				return null;
			}
			int num = Mathf.Max(1, atlas.width);
			int num2 = Mathf.Max(1, atlas.height);
			int num3 = Mathf.Clamp(Mathf.RoundToInt(normalizedScreenRect.x * (float)num), 0, num - 1);
			int num4 = Mathf.Clamp(Mathf.RoundToInt(normalizedScreenRect.y * (float)num2), 0, num2 - 1);
			int num5 = Mathf.Clamp(Mathf.RoundToInt(normalizedScreenRect.width * (float)num), 1, num - num3);
			int num6 = Mathf.Clamp(Mathf.RoundToInt(normalizedScreenRect.height * (float)num2), 1, num2 - num4);
			Texture2D texture2D = HackMeTextureComposer.ReadTexture(atlas, num, num2, atlas.name + " (HackMe)");
			if (texture2D == null)
			{
				return null;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(num5, num6, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			RenderTexture active = RenderTexture.active;
			Texture2D texture2D2;
			try
			{
				Vector2 vector;
				Vector2 vector2;
				HackMeTextureComposer.CalculateCoverTransform(photo, num5, num6, rotate180, out vector, out vector2);
				Graphics.Blit(photo, temporary, vector, vector2);
				RenderTexture.active = temporary;
				texture2D.ReadPixels(new Rect(0f, 0f, (float)num5, (float)num6), num3, num4, false);
				texture2D.Apply(false, false);
				texture2D2 = texture2D;
			}
			catch
			{
				Object.Destroy(texture2D);
				throw;
			}
			finally
			{
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
			}
			return texture2D2;
		}

		// Token: 0x06007D78 RID: 32120 RVA: 0x00336784 File Offset: 0x00334984
		private static Texture2D ReadTexture(Texture source, int width, int height, string outputName)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			RenderTexture active = RenderTexture.active;
			Texture2D texture2D2;
			try
			{
				Graphics.Blit(source, temporary);
				RenderTexture.active = temporary;
				Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
				texture2D.name = outputName;
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.wrapMode = TextureWrapMode.Clamp;
				texture2D.hideFlags = HideFlags.DontSave;
				texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0, false);
				texture2D.Apply(false, false);
				texture2D2 = texture2D;
			}
			finally
			{
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
			}
			return texture2D2;
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x0033681C File Offset: 0x00334A1C
		internal static void CalculateCoverTransform(Texture source, int targetWidth, int targetHeight, bool rotate180, out Vector2 uvScale, out Vector2 uvOffset)
		{
			float num = (float)source.width / (float)Mathf.Max(1, source.height);
			float num2 = (float)targetWidth / (float)Mathf.Max(1, targetHeight);
			uvScale = Vector2.one;
			if (num > num2)
			{
				uvScale.x = num2 / num;
			}
			else if (num < num2)
			{
				uvScale.y = num / num2;
			}
			uvOffset = (Vector2.one - uvScale) * 0.5f;
			if (!rotate180)
			{
				return;
			}
			uvOffset = Vector2.one - uvOffset;
			uvScale = -uvScale;
		}

		// Token: 0x04008156 RID: 33110
		private const int MaxPhotoDimension = 1024;
	}
}
