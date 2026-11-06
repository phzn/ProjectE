using System;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200098D RID: 2445
	public static class PhotoBoothPhotoLayout
	{
		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060043A0 RID: 17312 RVA: 0x00194B30 File Offset: 0x00192D30
		public static int Count
		{
			get
			{
				return PhotoBoothPhotoLayout.Names.Length;
			}
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x00194B39 File Offset: 0x00192D39
		public static string GetName(int index)
		{
			return PhotoBoothPhotoLayout.Names[PhotoBoothPhotoLayout.Wrap(index, PhotoBoothPhotoLayout.Names.Length)];
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x00194B50 File Offset: 0x00192D50
		public static void ApplyToTexture(Texture2D texture, int index)
		{
			if (texture == null)
			{
				return;
			}
			index = PhotoBoothPhotoLayout.Wrap(index, PhotoBoothPhotoLayout.Count);
			if (index == 0)
			{
				return;
			}
			Color32[] pixels = texture.GetPixels32();
			Color32[] array = PhotoBoothPhotoLayout.BuildOverlay(texture.width, texture.height, index);
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = PhotoBoothPhotoLayout.Blend(pixels[i], array[i]);
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, false);
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x00194BCC File Offset: 0x00192DCC
		public static Texture2D CreateOverlayTexture(int width, int height, int index)
		{
			width = Mathf.Max(8, width);
			height = Mathf.Max(8, height);
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
			texture2D.name = "Photo Booth Layout - " + PhotoBoothPhotoLayout.GetName(index);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.hideFlags = HideFlags.DontSave;
			texture2D.SetPixels32(PhotoBoothPhotoLayout.BuildOverlay(width, height, PhotoBoothPhotoLayout.Wrap(index, PhotoBoothPhotoLayout.Count)));
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x00194C44 File Offset: 0x00192E44
		private static Color32[] BuildOverlay(int width, int height, int index)
		{
			Color32[] array = new Color32[width * height];
			switch (index)
			{
			case 1:
				PhotoBoothPhotoLayout.DrawPolaroid(array, width, height);
				break;
			case 2:
				PhotoBoothPhotoLayout.DrawNeonNights(array, width, height);
				break;
			case 3:
				PhotoBoothPhotoLayout.DrawLoveLetter(array, width, height);
				break;
			case 4:
				PhotoBoothPhotoLayout.DrawFilmStrip(array, width, height);
				break;
			}
			return array;
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x00194C9C File Offset: 0x00192E9C
		private static void DrawPolaroid(Color32[] pixels, int width, int height)
		{
			int num = Mathf.Max(2, Mathf.RoundToInt((float)width * 0.055f));
			int num2 = Mathf.Max(2, Mathf.RoundToInt((float)height * 0.045f));
			int num3 = Mathf.Max(4, Mathf.RoundToInt((float)height * 0.17f));
			Color32 color = new Color32(248, 242, 228, byte.MaxValue);
			Color32 color2 = new Color32(67, 42, 58, 120);
			PhotoBoothPhotoLayout.FillMargins(pixels, width, height, num, num, num3, num2, color);
			PhotoBoothPhotoLayout.DrawRectOutline(pixels, width, height, num, num3, width - num - 1, height - num2 - 1, Mathf.Max(1, width / 160), color2);
			PhotoBoothPhotoLayout.DrawHorizontal(pixels, width, height, num3 / 2, num + width / 12, width - num - width / 12, Mathf.Max(1, height / 480), new Color32(217, 165, 187, 150));
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x00194D80 File Offset: 0x00192F80
		private static void DrawNeonNights(Color32[] pixels, int width, int height)
		{
			int num = Mathf.Max(3, Mathf.RoundToInt((float)width * 0.038f));
			Color32 color = new Color32(10, 14, 34, 245);
			Color32 color2 = new Color32(byte.MaxValue, 56, 159, byte.MaxValue);
			Color32 color3 = new Color32(47, 225, byte.MaxValue, byte.MaxValue);
			PhotoBoothPhotoLayout.FillMargins(pixels, width, height, num, num, num, num, color);
			int num2 = Mathf.Max(1, width / 180);
			PhotoBoothPhotoLayout.DrawRectOutline(pixels, width, height, num, num, width - num - 1, height - num - 1, num2, color2);
			PhotoBoothPhotoLayout.DrawRectOutline(pixels, width, height, num + num2 * 3, num + num2 * 3, width - num - num2 * 3 - 1, height - num - num2 * 3 - 1, num2, color3);
			int num3 = Mathf.Max(8, width / 7);
			PhotoBoothPhotoLayout.DrawHorizontal(pixels, width, height, height - num / 2, num, num + num3, num2 * 2, color3);
			PhotoBoothPhotoLayout.DrawVertical(pixels, width, height, width - num / 2, num, num + num3 * 2, num2 * 2, color2);
			PhotoBoothPhotoLayout.DrawHorizontal(pixels, width, height, num / 2, width - num - num3, width - num, num2 * 2, color2);
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x00194E98 File Offset: 0x00193098
		private static void DrawLoveLetter(Color32[] pixels, int width, int height)
		{
			int num = Mathf.Max(3, Mathf.RoundToInt((float)width * 0.048f));
			Color32 color = new Color32(239, 151, 181, 244);
			Color32 color2 = new Color32(byte.MaxValue, 239, 225, byte.MaxValue);
			Color32 color3 = new Color32(164, 32, 88, byte.MaxValue);
			PhotoBoothPhotoLayout.FillMargins(pixels, width, height, num, num, num, num, color);
			PhotoBoothPhotoLayout.DrawRectOutline(pixels, width, height, num, num, width - num - 1, height - num - 1, Mathf.Max(1, width / 190), color2);
			int num2 = Mathf.Max(4, width / 30);
			PhotoBoothPhotoLayout.DrawHeart(pixels, width, height, num / 2, height - num / 2, num2, color2);
			PhotoBoothPhotoLayout.DrawHeart(pixels, width, height, width - num / 2 - 1, num / 2, num2, color3);
			PhotoBoothPhotoLayout.DrawHeart(pixels, width, height, width - num / 2 - 1, height - num / 2, Mathf.Max(3, num2 * 2 / 3), color2);
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x00194F88 File Offset: 0x00193188
		private static void DrawFilmStrip(Color32[] pixels, int width, int height)
		{
			int num = Mathf.Max(6, Mathf.RoundToInt((float)width * 0.09f));
			int num2 = Mathf.Max(2, Mathf.RoundToInt((float)height * 0.025f));
			Color32 color = new Color32(15, 15, 18, byte.MaxValue);
			Color32 color2 = new Color32(249, 225, 179, byte.MaxValue);
			PhotoBoothPhotoLayout.FillMargins(pixels, width, height, num, num, num2, num2, color);
			PhotoBoothPhotoLayout.DrawRectOutline(pixels, width, height, num, num2, width - num - 1, height - num2 - 1, Mathf.Max(1, width / 200), color2);
			int num3 = Mathf.Max(3, num / 2);
			int num4 = Mathf.Max(5, height / 38);
			int num5 = Mathf.Max(num4 + 2, height / 15);
			for (int i = num5 / 2; i < height; i += num5)
			{
				PhotoBoothPhotoLayout.FillRect(pixels, width, height, (num - num3) / 2, i - num4 / 2, (num + num3) / 2, i + num4 / 2, color2);
				PhotoBoothPhotoLayout.FillRect(pixels, width, height, width - (num + num3) / 2, i - num4 / 2, width - (num - num3) / 2, i + num4 / 2, color2);
			}
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x001950A0 File Offset: 0x001932A0
		private static void FillMargins(Color32[] pixels, int width, int height, int left, int right, int bottom, int top, Color32 color)
		{
			for (int i = 0; i < height; i++)
			{
				int num = i * width;
				bool flag = i < bottom || i >= height - top;
				for (int j = 0; j < width; j++)
				{
					if (flag || j < left || j >= width - right)
					{
						pixels[num + j] = color;
					}
				}
			}
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x001950F8 File Offset: 0x001932F8
		private static void DrawRectOutline(Color32[] pixels, int width, int height, int left, int bottom, int right, int top, int thickness, Color32 color)
		{
			PhotoBoothPhotoLayout.FillRect(pixels, width, height, left, bottom, right, bottom + thickness, color);
			PhotoBoothPhotoLayout.FillRect(pixels, width, height, left, top - thickness, right, top, color);
			PhotoBoothPhotoLayout.FillRect(pixels, width, height, left, bottom, left + thickness, top, color);
			PhotoBoothPhotoLayout.FillRect(pixels, width, height, right - thickness, bottom, right, top, color);
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x00195158 File Offset: 0x00193358
		private static void DrawHorizontal(Color32[] pixels, int width, int height, int y, int x0, int x1, int thickness, Color32 color)
		{
			PhotoBoothPhotoLayout.FillRect(pixels, width, height, x0, y - thickness / 2, x1, y + Mathf.Max(1, thickness / 2), color);
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x00195188 File Offset: 0x00193388
		private static void DrawVertical(Color32[] pixels, int width, int height, int x, int y0, int y1, int thickness, Color32 color)
		{
			PhotoBoothPhotoLayout.FillRect(pixels, width, height, x - thickness / 2, y0, x + Mathf.Max(1, thickness / 2), y1, color);
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x001951B8 File Offset: 0x001933B8
		private static void FillRect(Color32[] pixels, int width, int height, int x0, int y0, int x1, int y1, Color32 color)
		{
			x0 = Mathf.Clamp(x0, 0, width - 1);
			x1 = Mathf.Clamp(x1, 0, width - 1);
			y0 = Mathf.Clamp(y0, 0, height - 1);
			y1 = Mathf.Clamp(y1, 0, height - 1);
			for (int i = y0; i <= y1; i++)
			{
				int num = i * width;
				for (int j = x0; j <= x1; j++)
				{
					pixels[num + j] = color;
				}
			}
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x00195224 File Offset: 0x00193424
		private static void DrawHeart(Color32[] pixels, int width, int height, int centerX, int centerY, int radius, Color32 color)
		{
			int num = radius * 2;
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					float num2 = (float)j / (float)Mathf.Max(1, radius);
					float num3 = (float)i / (float)Mathf.Max(1, radius);
					if (Mathf.Pow(num2 * num2 + num3 * num3 - 1f, 3f) - num2 * num2 * num3 * num3 * num3 <= 0f)
					{
						int num4 = centerX + j;
						int num5 = centerY + i;
						if (num4 >= 0 && num4 < width && num5 >= 0 && num5 < height)
						{
							pixels[num5 * width + num4] = color;
						}
					}
				}
			}
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x001952CC File Offset: 0x001934CC
		private static Color32 Blend(Color32 background, Color32 foreground)
		{
			if (foreground.a == 0)
			{
				return background;
			}
			if (foreground.a == 255)
			{
				return foreground;
			}
			int a = (int)foreground.a;
			int num = 255 - a;
			return new Color32((byte)(((int)foreground.r * a + (int)background.r * num) / 255), (byte)(((int)foreground.g * a + (int)background.g * num) / 255), (byte)(((int)foreground.b * a + (int)background.b * num) / 255), byte.MaxValue);
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0018CFF6 File Offset: 0x0018B1F6
		private static int Wrap(int value, int count)
		{
			return (value % count + count) % count;
		}

		// Token: 0x0400436D RID: 17261
		private static readonly string[] Names = new string[] { "CLEAN", "POLAROID", "NEON NIGHTS", "LOVE LETTER", "FILM STRIP" };
	}
}
