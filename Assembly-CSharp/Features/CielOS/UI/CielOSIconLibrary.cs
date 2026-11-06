using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CielOS.UI
{
	// Token: 0x020013C0 RID: 5056
	public static class CielOSIconLibrary
	{
		// Token: 0x06009C57 RID: 40023 RVA: 0x003FE6C0 File Offset: 0x003FC8C0
		public static Sprite GetSprite(CielOSIconId id, int size = 64)
		{
			if (size < 8)
			{
				size = 8;
			}
			long num = ((long)id << 20) | (long)((ulong)size);
			Sprite sprite;
			if (CielOSIconLibrary._cache.TryGetValue(num, out sprite) && sprite != null)
			{
				return sprite;
			}
			Sprite sprite2 = Sprite.Create(CielOSIconLibrary.Render(id, size), new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = "CielOSIcon_" + id.ToString();
			CielOSIconLibrary._cache[num] = sprite2;
			return sprite2;
		}

		// Token: 0x06009C58 RID: 40024 RVA: 0x003FE758 File Offset: 0x003FC958
		public static Sprite GetColoredSprite(CielOSIconId id, int size = 64)
		{
			if (size < 8)
			{
				size = 8;
			}
			if (!CielOSIconLibrary.IsColoredAppIcon(id))
			{
				return CielOSIconLibrary.GetSprite(id, size);
			}
			long num = ((long)id << 20) | (long)((ulong)size);
			Sprite sprite;
			if (CielOSIconLibrary._coloredCache.TryGetValue(num, out sprite) && sprite != null)
			{
				return sprite;
			}
			Sprite sprite2 = Sprite.Create(CielOSIconLibrary.RenderColored(id, size), new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = "CielOSIconColored_" + id.ToString();
			CielOSIconLibrary._coloredCache[num] = sprite2;
			return sprite2;
		}

		// Token: 0x06009C59 RID: 40025 RVA: 0x003FE7FD File Offset: 0x003FC9FD
		public static bool IsColoredAppIcon(CielOSIconId id)
		{
			return id == CielOSIconId.CustomizeCharacter || id == CielOSIconId.StartRun || id == CielOSIconId.RunModes || id == CielOSIconId.ClassicRun || id == CielOSIconId.FreeMode || id == CielOSIconId.LongWayHome || id == CielOSIconId.GuidedRun || id == CielOSIconId.Maps || id == CielOSIconId.Leaderboard;
		}

		// Token: 0x06009C5A RID: 40026 RVA: 0x003FE82E File Offset: 0x003FCA2E
		public static void ClearCache()
		{
			CielOSIconLibrary._cache.Clear();
			CielOSIconLibrary._coloredCache.Clear();
			CielOSIconLibrary._logoCache.Clear();
			CielOSIconLibrary._logoMaskCache.Clear();
			CielOSIconLibrary._badgedCache.Clear();
		}

		// Token: 0x06009C5B RID: 40027 RVA: 0x003FE864 File Offset: 0x003FCA64
		public static Sprite GetAppLogoSprite(string resourcePath, CielOSIconId fallbackId, int size = 64)
		{
			if (size < 8)
			{
				size = 8;
			}
			string text = resourcePath + "#" + size.ToString();
			Sprite sprite;
			if (CielOSIconLibrary._logoCache.TryGetValue(text, out sprite) && sprite != null)
			{
				return sprite;
			}
			Texture2D texture2D = Resources.Load<Texture2D>("Icons/" + resourcePath);
			if (texture2D == null)
			{
				return CielOSIconLibrary.GetColoredSprite(fallbackId, size);
			}
			Texture2D texture2D2 = CielOSIconLibrary.RenderLogoBadge(texture2D, size);
			if (texture2D2 == null)
			{
				return CielOSIconLibrary.GetColoredSprite(fallbackId, size);
			}
			Sprite sprite2 = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = "CielOSIconLogo_" + resourcePath;
			CielOSIconLibrary._logoCache[text] = sprite2;
			return sprite2;
		}

		// Token: 0x06009C5C RID: 40028 RVA: 0x003FE930 File Offset: 0x003FCB30
		public static Sprite GetAppLogoMaskSprite(string resourcePath, CielOSIconId fallbackId, int size = 64)
		{
			if (size < 8)
			{
				size = 8;
			}
			string text = resourcePath + "#" + size.ToString();
			Sprite sprite;
			if (CielOSIconLibrary._logoMaskCache.TryGetValue(text, out sprite) && sprite != null)
			{
				return sprite;
			}
			Texture2D texture2D = Resources.Load<Texture2D>("Icons/" + resourcePath);
			if (texture2D == null)
			{
				return CielOSIconLibrary.GetSprite(fallbackId, size);
			}
			Texture2D texture2D2 = CielOSIconLibrary.RenderLogoMask(texture2D, size);
			if (texture2D2 == null)
			{
				return CielOSIconLibrary.GetSprite(fallbackId, size);
			}
			Sprite sprite2 = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = "CielOSIconLogoMask_" + resourcePath;
			CielOSIconLibrary._logoMaskCache[text] = sprite2;
			return sprite2;
		}

		// Token: 0x06009C5D RID: 40029 RVA: 0x003FE9FC File Offset: 0x003FCBFC
		private static Texture2D RenderLogoMask(Texture2D logo, int size)
		{
			Color32[] pixels;
			try
			{
				pixels = logo.GetPixels32();
			}
			catch
			{
				return null;
			}
			int width = logo.width;
			int height = logo.height;
			if (width <= 0 || height <= 0)
			{
				return null;
			}
			int num = size * 4;
			float[] array = new float[num * num];
			float num2 = 0.1f;
			float num3 = 0.9f - num2;
			float num4 = (float)width / (float)height;
			float num5 = num3;
			float num6 = num3;
			if (num4 >= 1f)
			{
				num6 = num3 / num4;
			}
			else
			{
				num5 = num3 * num4;
			}
			float num7 = num2 + (num3 - num5) * 0.5f;
			float num8 = num2 + (num3 - num6) * 0.5f;
			for (int i = 0; i < num; i++)
			{
				float num9 = ((float)i + 0.5f) / (float)num;
				if (num9 >= num8 && num9 <= num8 + num6)
				{
					for (int j = 0; j < num; j++)
					{
						float num10 = ((float)j + 0.5f) / (float)num;
						if (num10 >= num7 && num10 <= num7 + num5)
						{
							float num11 = (num10 - num7) / num5;
							float num12 = (num9 - num8) / num6;
							int num13 = Mathf.Clamp((int)(num11 * (float)width), 0, width - 1);
							int num14 = Mathf.Clamp((int)(num12 * (float)height), 0, height - 1);
							array[i * num + j] = (float)pixels[num14 * width + num13].a / 255f;
						}
					}
				}
			}
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			Color32[] array2 = new Color32[size * size];
			int num15 = 16;
			for (int k = 0; k < size; k++)
			{
				for (int l = 0; l < size; l++)
				{
					float num16 = 0f;
					int num17 = l * 4;
					int num18 = k * 4;
					for (int m = 0; m < 4; m++)
					{
						for (int n = 0; n < 4; n++)
						{
							num16 += array[(num18 + m) * num + (num17 + n)];
						}
					}
					num16 /= (float)num15;
					array2[k * size + l] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(Mathf.RoundToInt(num16 * 255f), 0, 255));
				}
			}
			texture2D.SetPixels32(array2);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x06009C5E RID: 40030 RVA: 0x003FEC5C File Offset: 0x003FCE5C
		private static Texture2D RenderLogoBadge(Texture2D logo, int size)
		{
			Color32[] pixels;
			try
			{
				pixels = logo.GetPixels32();
			}
			catch
			{
				return null;
			}
			int width = logo.width;
			int height = logo.height;
			if (width <= 0 || height <= 0)
			{
				return null;
			}
			int num = size * 4;
			int num2 = num * num;
			float[] array = new float[num2];
			float[] array2 = new float[num2];
			float[] array3 = new float[num2];
			float[] array4 = new float[num2];
			CielOSIconLibrary.FillViolaBadge(array, array2, array3, array4, num);
			int num3 = width;
			int num4 = height;
			int num5 = -1;
			int num6 = -1;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (pixels[i * width + j].a > 6)
					{
						if (j < num3)
						{
							num3 = j;
						}
						if (j > num5)
						{
							num5 = j;
						}
						if (i < num4)
						{
							num4 = i;
						}
						if (i > num6)
						{
							num6 = i;
						}
					}
				}
			}
			if (num5 < num3)
			{
				num3 = 0;
				num4 = 0;
				num5 = width - 1;
				num6 = height - 1;
			}
			float num7 = (float)(num5 - num3 + 1);
			float num8 = (float)(num6 - num4 + 1);
			float num9 = 0.24f;
			float num10 = 0.76f - num9;
			float num11 = num7 / num8;
			float num12 = num10;
			float num13 = num10;
			if (num11 >= 1f)
			{
				num13 = num10 / num11;
			}
			else
			{
				num12 = num10 * num11;
			}
			float num14 = num9 + (num10 - num12) * 0.5f;
			float num15 = num9 + (num10 - num13) * 0.5f;
			for (int k = 0; k < num; k++)
			{
				float num16 = ((float)k + 0.5f) / (float)num;
				if (num16 >= num15 && num16 <= num15 + num13)
				{
					for (int l = 0; l < num; l++)
					{
						float num17 = ((float)l + 0.5f) / (float)num;
						if (num17 >= num14 && num17 <= num14 + num12)
						{
							float num18 = (num17 - num14) / num12;
							float num19 = (num16 - num15) / num13;
							int num20 = Mathf.Clamp(num3 + (int)(num18 * num7), num3, num5);
							int num21 = Mathf.Clamp(num4 + (int)(num19 * num8), num4, num6);
							Color32 color = pixels[num21 * width + num20];
							if (color.a != 0)
							{
								int num22 = k * num + l;
								CielOSIconLibrary.Composite(array, array2, array3, array4, num22, new Color((float)color.r / 255f, (float)color.g / 255f, (float)color.b / 255f, 1f), (float)color.a / 255f);
							}
						}
					}
				}
			}
			return CielOSIconLibrary.Downsample(array, array2, array3, array4, size, num);
		}

		// Token: 0x06009C5F RID: 40031 RVA: 0x003FEEF8 File Offset: 0x003FD0F8
		public static Sprite GetBadgedSprite(CielOSIconId id, int size = 64)
		{
			if (size < 8)
			{
				size = 8;
			}
			long num = ((long)id << 20) | (long)((ulong)size);
			Sprite sprite;
			if (CielOSIconLibrary._badgedCache.TryGetValue(num, out sprite) && sprite != null)
			{
				return sprite;
			}
			int num2 = size * 4;
			int num3 = num2 * num2;
			float[] array = new float[num3];
			float[] array2 = new float[num3];
			float[] array3 = new float[num3];
			float[] array4 = new float[num3];
			CielOSIconLibrary.FillViolaBadge(array, array2, array3, array4, num2);
			float[] array5 = new float[num3];
			CielOSIconLibrary.Draw(id, new CielOSIconLibrary.Ctx(array5, num2));
			CielOSIconLibrary.NormalizeCoverage(array5, num2, 0.24f);
			Color color = new Color(0.97f, 0.92f, 1f, 1f);
			CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array5, color, num3);
			Sprite sprite2 = Sprite.Create(CielOSIconLibrary.Downsample(array, array2, array3, array4, size, num2), new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = "CielOSIconBadged_" + id.ToString();
			CielOSIconLibrary._badgedCache[num] = sprite2;
			return sprite2;
		}

		// Token: 0x06009C60 RID: 40032 RVA: 0x003FF01C File Offset: 0x003FD21C
		private static void NormalizeCoverage(float[] buf, int hi, float inset)
		{
			int num = hi;
			int num2 = hi;
			int num3 = -1;
			int num4 = -1;
			for (int i = 0; i < hi; i++)
			{
				int num5 = i * hi;
				for (int j = 0; j < hi; j++)
				{
					if (buf[num5 + j] > 0.02f)
					{
						if (j < num)
						{
							num = j;
						}
						if (j > num3)
						{
							num3 = j;
						}
						if (i < num2)
						{
							num2 = i;
						}
						if (i > num4)
						{
							num4 = i;
						}
					}
				}
			}
			if (num3 < num || num4 < num2)
			{
				return;
			}
			float num6 = (float)(num3 - num + 1);
			float num7 = (float)(num4 - num2 + 1);
			float num8 = inset * (float)hi;
			float num9 = (1f - inset) * (float)hi - num8;
			float num10 = num9 / Mathf.Max(num6, num7);
			float num11 = num6 * num10;
			float num12 = num7 * num10;
			float num13 = num8 + (num9 - num11) * 0.5f;
			float num14 = num8 + (num9 - num12) * 0.5f;
			float[] array = new float[hi * hi];
			for (int k = 0; k < hi; k++)
			{
				if ((float)k >= num14 && (float)k < num14 + num12)
				{
					int num15 = Mathf.Clamp((int)((float)num2 + ((float)k - num14) / num10), num2, num4);
					int num16 = k * hi;
					int num17 = num15 * hi;
					for (int l = 0; l < hi; l++)
					{
						if ((float)l >= num13 && (float)l < num13 + num11)
						{
							int num18 = Mathf.Clamp((int)((float)num + ((float)l - num13) / num10), num, num3);
							array[num16 + l] = buf[num17 + num18];
						}
					}
				}
			}
			Array.Copy(array, buf, buf.Length);
		}

		// Token: 0x06009C61 RID: 40033 RVA: 0x003FF18C File Offset: 0x003FD38C
		private static void FillViolaBadge(float[] cr, float[] cg, float[] cb, float[] ca, int hi)
		{
			Color color = new Color(0.663f, 0.376f, 0.788f, 1f);
			Color color2 = new Color(0.42f, 0.2f, 0.56f, 1f);
			int num = hi * hi;
			float[] array = new float[num];
			CielOSIconLibrary.RoundRectFill(new CielOSIconLibrary.Ctx(array, hi), 0.1f, 0.1f, 0.9f, 0.9f, 0.22f, false);
			for (int i = 0; i < num; i++)
			{
				if (array[i] > 0f)
				{
					float num2 = ((float)(i / hi) + 0.5f) / (float)hi;
					Color color3 = Color.Lerp(color2, color, Mathf.Clamp01(num2));
					CielOSIconLibrary.Composite(cr, cg, cb, ca, i, color3, array[i]);
				}
			}
		}

		// Token: 0x06009C62 RID: 40034 RVA: 0x003FF254 File Offset: 0x003FD454
		private static Texture2D Render(CielOSIconId id, int size)
		{
			int num = size * 4;
			float[] array = new float[num * num];
			CielOSIconLibrary.Ctx ctx = new CielOSIconLibrary.Ctx(array, num);
			CielOSIconLibrary.Draw(id, ctx);
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			Color32[] array2 = new Color32[size * size];
			int num2 = 16;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num3 = 0f;
					int num4 = j * 4;
					int num5 = i * 4;
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < 4; l++)
						{
							num3 += array[(num5 + k) * num + (num4 + l)];
						}
					}
					num3 /= (float)num2;
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num3 * 255f), 0, 255);
					array2[i * size + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
			texture2D.SetPixels32(array2);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x06009C63 RID: 40035 RVA: 0x003FF370 File Offset: 0x003FD570
		private static Texture2D RenderColored(CielOSIconId id, int size)
		{
			int num = size * 4;
			int num2 = num * num;
			float[] array = new float[num2];
			float[] array2 = new float[num2];
			float[] array3 = new float[num2];
			float[] array4 = new float[num2];
			Color color = new Color(0.663f, 0.376f, 0.788f, 1f);
			Color color2 = new Color(0.42f, 0.2f, 0.56f, 1f);
			Color color3 = new Color(0.97f, 0.92f, 1f, 1f);
			float[] array5 = new float[num2];
			CielOSIconLibrary.RoundRectFill(new CielOSIconLibrary.Ctx(array5, num), 0.1f, 0.1f, 0.9f, 0.9f, 0.22f, false);
			for (int i = 0; i < num2; i++)
			{
				if (array5[i] > 0f)
				{
					float num3 = ((float)(i / num) + 0.5f) / (float)num;
					Color color4 = Color.Lerp(color2, color, Mathf.Clamp01(num3));
					CielOSIconLibrary.Composite(array, array2, array3, array4, i, color4, array5[i]);
				}
			}
			if (id == CielOSIconId.CustomizeCharacter)
			{
				float[] array6 = new float[num2];
				CielOSIconLibrary.Ctx ctx = new CielOSIconLibrary.Ctx(array6, num);
				CielOSIconLibrary.Disc(ctx, 0.42f, 0.62f, 0.14f, false);
				CielOSIconLibrary.Arc(ctx, 0.42f, 0.19f, 0.24f, 28f, 152f, 0.11f, false);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array6, color3, num2);
				float[] array7 = new float[num2];
				CielOSIconLibrary.Ctx ctx2 = new CielOSIconLibrary.Ctx(array7, num);
				CielOSIconLibrary.Line(ctx2, CielOSIconLibrary.v(0.62f, 0.66f), CielOSIconLibrary.v(0.8f, 0.66f), 0.035f, false);
				CielOSIconLibrary.Line(ctx2, CielOSIconLibrary.v(0.62f, 0.43f), CielOSIconLibrary.v(0.8f, 0.43f), 0.035f, false);
				CielOSIconLibrary.Disc(ctx2, 0.69f, 0.66f, 0.055f, false);
				CielOSIconLibrary.Disc(ctx2, 0.75f, 0.43f, 0.055f, false);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array7, new Color(1f, 0.82f, 0.45f, 1f), num2);
			}
			else if (id == CielOSIconId.StartRun || id == CielOSIconId.RunModes)
			{
				float[] array8 = new float[num2];
				CielOSIconLibrary.Ctx ctx3 = new CielOSIconLibrary.Ctx(array8, num);
				CielOSIconLibrary.RoundRectFill(ctx3, 0.24f, 0.6f, 0.76f, 0.73f, 0.045f, false);
				CielOSIconLibrary.RoundRectFill(ctx3, 0.24f, 0.42f, 0.76f, 0.55f, 0.045f, false);
				CielOSIconLibrary.RoundRectFill(ctx3, 0.24f, 0.24f, 0.76f, 0.37f, 0.045f, false);
				CielOSIconLibrary.Disc(ctx3, 0.32f, 0.665f, 0.027f, true);
				CielOSIconLibrary.Disc(ctx3, 0.32f, 0.485f, 0.027f, true);
				CielOSIconLibrary.Disc(ctx3, 0.32f, 0.305f, 0.027f, true);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array8, color3, num2);
				float[] array9 = new float[num2];
				CielOSIconLibrary.Poly(new CielOSIconLibrary.Ctx(array9, num), new Vector2[]
				{
					CielOSIconLibrary.v(0.61f, 0.61f),
					CielOSIconLibrary.v(0.61f, 0.72f),
					CielOSIconLibrary.v(0.71f, 0.665f)
				}, false);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array9, new Color(0.42f, 0.92f, 0.66f, 1f), num2);
			}
			else if (id == CielOSIconId.Leaderboard)
			{
				float[] array10 = new float[num2];
				CielOSIconLibrary.Draw(CielOSIconId.Leaderboard, new CielOSIconLibrary.Ctx(array10, num));
				CielOSIconLibrary.NormalizeCoverage(array10, num, 0.24f);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array10, new Color(1f, 0.82f, 0.45f, 1f), num2);
			}
			else if (id == CielOSIconId.Maps)
			{
				float[] array11 = new float[num2];
				CielOSIconLibrary.Ctx ctx4 = new CielOSIconLibrary.Ctx(array11, num);
				CielOSIconLibrary.Poly(ctx4, new Vector2[]
				{
					CielOSIconLibrary.v(0.22f, 0.27f),
					CielOSIconLibrary.v(0.4f, 0.34f),
					CielOSIconLibrary.v(0.58f, 0.27f),
					CielOSIconLibrary.v(0.78f, 0.35f),
					CielOSIconLibrary.v(0.78f, 0.72f),
					CielOSIconLibrary.v(0.58f, 0.64f),
					CielOSIconLibrary.v(0.4f, 0.72f),
					CielOSIconLibrary.v(0.22f, 0.65f)
				}, false);
				CielOSIconLibrary.Line(ctx4, CielOSIconLibrary.v(0.4f, 0.34f), CielOSIconLibrary.v(0.4f, 0.72f), 0.025f, true);
				CielOSIconLibrary.Line(ctx4, CielOSIconLibrary.v(0.58f, 0.27f), CielOSIconLibrary.v(0.58f, 0.64f), 0.025f, true);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array11, color3, num2);
				float[] array12 = new float[num2];
				CielOSIconLibrary.Ctx ctx5 = new CielOSIconLibrary.Ctx(array12, num);
				CielOSIconLibrary.Disc(ctx5, 0.6f, 0.61f, 0.105f, false);
				CielOSIconLibrary.Poly(ctx5, new Vector2[]
				{
					CielOSIconLibrary.v(0.515f, 0.57f),
					CielOSIconLibrary.v(0.685f, 0.57f),
					CielOSIconLibrary.v(0.6f, 0.39f)
				}, false);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array12, new Color(0.42f, 0.92f, 0.9f, 1f), num2);
				float[] array13 = new float[num2];
				CielOSIconLibrary.Disc(new CielOSIconLibrary.Ctx(array13, num), 0.6f, 0.62f, 0.038f, false);
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array13, color2, num2);
			}
			else
			{
				float[] array14 = new float[num2];
				CielOSIconLibrary.Ctx ctx6 = new CielOSIconLibrary.Ctx(array14, num);
				if (id == CielOSIconId.ClassicRun)
				{
					CielOSIconLibrary.RoundRectStroke(ctx6, 0.28f, 0.22f, 0.67f, 0.78f, 0.06f, 0.06f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.38f, 0.5f), CielOSIconLibrary.v(0.77f, 0.5f), 0.055f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.77f, 0.5f), CielOSIconLibrary.v(0.64f, 0.62f), 0.055f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.77f, 0.5f), CielOSIconLibrary.v(0.64f, 0.38f), 0.055f, false);
				}
				else if (id == CielOSIconId.FreeMode)
				{
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.26f, 0.68f), CielOSIconLibrary.v(0.74f, 0.68f), 0.045f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.26f, 0.5f), CielOSIconLibrary.v(0.74f, 0.5f), 0.045f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.26f, 0.32f), CielOSIconLibrary.v(0.74f, 0.32f), 0.045f, false);
					CielOSIconLibrary.Disc(ctx6, 0.4f, 0.68f, 0.065f, false);
					CielOSIconLibrary.Disc(ctx6, 0.63f, 0.5f, 0.065f, false);
					CielOSIconLibrary.Disc(ctx6, 0.48f, 0.32f, 0.065f, false);
				}
				else if (id == CielOSIconId.LongWayHome)
				{
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.28f, 0.2f), CielOSIconLibrary.v(0.46f, 0.7f), 0.06f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.72f, 0.2f), CielOSIconLibrary.v(0.54f, 0.7f), 0.06f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.5f, 0.24f), CielOSIconLibrary.v(0.5f, 0.4f), 0.04f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.5f, 0.5f), CielOSIconLibrary.v(0.5f, 0.6f), 0.035f, false);
					CielOSIconLibrary.Line(ctx6, CielOSIconLibrary.v(0.24f, 0.72f), CielOSIconLibrary.v(0.76f, 0.72f), 0.04f, false);
				}
				else
				{
					CielOSIconLibrary.Ring(ctx6, 0.5f, 0.5f, 0.27f, 0.055f, false);
					CielOSIconLibrary.Poly(ctx6, new Vector2[]
					{
						CielOSIconLibrary.v(0.5f, 0.73f),
						CielOSIconLibrary.v(0.43f, 0.45f),
						CielOSIconLibrary.v(0.57f, 0.45f)
					}, false);
					CielOSIconLibrary.Disc(ctx6, 0.5f, 0.5f, 0.055f, true);
				}
				CielOSIconLibrary.CompositeLayer(array, array2, array3, array4, array14, color3, num2);
			}
			return CielOSIconLibrary.Downsample(array, array2, array3, array4, size, num);
		}

		// Token: 0x06009C64 RID: 40036 RVA: 0x003FFC70 File Offset: 0x003FDE70
		private static Texture2D Downsample(float[] cr, float[] cg, float[] cb, float[] ca, int size, int hi)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			Color32[] array = new Color32[size * size];
			int num = 16;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num2 = 0f;
					float num3 = 0f;
					float num4 = 0f;
					float num5 = 0f;
					int num6 = j * 4;
					int num7 = i * 4;
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < 4; l++)
						{
							int num8 = (num7 + k) * hi + (num6 + l);
							num2 += cr[num8];
							num3 += cg[num8];
							num4 += cb[num8];
							num5 += ca[num8];
						}
					}
					num2 /= (float)num;
					num3 /= (float)num;
					num4 /= (float)num;
					num5 /= (float)num;
					float num9 = ((num5 > 1E-05f) ? (1f / num5) : 0f);
					array[i * size + j] = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(num2 * num9 * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(num3 * num9 * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(num4 * num9 * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(num5 * 255f), 0, 255));
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x06009C65 RID: 40037 RVA: 0x003FFE10 File Offset: 0x003FE010
		private static void Composite(float[] cr, float[] cg, float[] cb, float[] ca, int i, Color col, float coverage)
		{
			float num = col.a * coverage;
			if (num <= 0f)
			{
				return;
			}
			float num2 = 1f - num;
			cr[i] = col.r * num + cr[i] * num2;
			cg[i] = col.g * num + cg[i] * num2;
			cb[i] = col.b * num + cb[i] * num2;
			ca[i] = num + ca[i] * num2;
		}

		// Token: 0x06009C66 RID: 40038 RVA: 0x003FFE84 File Offset: 0x003FE084
		private static void CompositeLayer(float[] cr, float[] cg, float[] cb, float[] ca, float[] mask, Color col, int n2)
		{
			for (int i = 0; i < n2; i++)
			{
				if (mask[i] > 0f)
				{
					CielOSIconLibrary.Composite(cr, cg, cb, ca, i, col, mask[i]);
				}
			}
		}

		// Token: 0x06009C67 RID: 40039 RVA: 0x003FFEBC File Offset: 0x003FE0BC
		private static void RoundRectFill(CielOSIconLibrary.Ctx c, float minX, float minY, float maxX, float maxY, float radius, bool erase = false)
		{
			float cx0 = minX + radius;
			float cx1 = maxX - radius;
			float cy0 = minY + radius;
			float cy1 = maxY - radius;
			CielOSIconLibrary.Raster(c, delegate(float x, float y)
			{
				float num = Mathf.Max(new float[]
				{
					0f,
					cx0 - x,
					x - cx1
				});
				float num2 = Mathf.Max(new float[]
				{
					0f,
					cy0 - y,
					y - cy1
				});
				return Mathf.Sqrt(num * num + num2 * num2) - radius;
			}, minX, minY, maxX, maxY, erase);
		}

		// Token: 0x06009C68 RID: 40040 RVA: 0x003FFF2C File Offset: 0x003FE12C
		private static void Draw(CielOSIconId id, CielOSIconLibrary.Ctx c)
		{
			switch (id)
			{
			case CielOSIconId.Wifi:
				CielOSIconLibrary.Arc(c, 0.5f, 0.3f, 0.4f, 55f, 125f, 0.055f, false);
				CielOSIconLibrary.Arc(c, 0.5f, 0.3f, 0.27f, 55f, 125f, 0.055f, false);
				CielOSIconLibrary.Arc(c, 0.5f, 0.3f, 0.14f, 55f, 125f, 0.055f, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.3f, 0.05f, false);
				return;
			case CielOSIconId.Volume:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.18f, 0.4f),
					CielOSIconLibrary.v(0.3f, 0.4f),
					CielOSIconLibrary.v(0.46f, 0.26f),
					CielOSIconLibrary.v(0.46f, 0.74f),
					CielOSIconLibrary.v(0.3f, 0.6f),
					CielOSIconLibrary.v(0.18f, 0.6f)
				}, false);
				CielOSIconLibrary.Arc(c, 0.46f, 0.5f, 0.16f, -55f, 55f, 0.05f, false);
				CielOSIconLibrary.Arc(c, 0.46f, 0.5f, 0.27f, -55f, 55f, 0.05f, false);
				return;
			case CielOSIconId.Bell:
				CielOSIconLibrary.Arc(c, 0.5f, 0.58f, 0.22f, 0f, 180f, 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.28f, 0.58f), CielOSIconLibrary.v(0.24f, 0.34f), 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.72f, 0.58f), CielOSIconLibrary.v(0.76f, 0.34f), 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.22f, 0.34f), CielOSIconLibrary.v(0.78f, 0.34f), 0.06f, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.26f, 0.055f, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.8f, 0.04f, false);
				return;
			case CielOSIconId.Power:
				CielOSIconLibrary.Arc(c, 0.5f, 0.48f, 0.26f, -65f, 245f, 0.065f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.5f), CielOSIconLibrary.v(0.5f, 0.84f), 0.065f, false);
				return;
			case CielOSIconId.Search:
				CielOSIconLibrary.Ring(c, 0.42f, 0.56f, 0.2f, 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.57f, 0.41f), CielOSIconLibrary.v(0.78f, 0.2f), 0.07f, false);
				return;
			case CielOSIconId.Menu:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.22f, 0.7f), CielOSIconLibrary.v(0.78f, 0.7f), 0.07f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.22f, 0.5f), CielOSIconLibrary.v(0.78f, 0.5f), 0.07f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.22f, 0.3f), CielOSIconLibrary.v(0.78f, 0.3f), 0.07f, false);
				return;
			case CielOSIconId.Terminal:
				CielOSIconLibrary.RoundRectStroke(c, 0.14f, 0.2f, 0.86f, 0.8f, 0.06f, 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.28f, 0.58f), CielOSIconLibrary.v(0.4f, 0.5f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.4f, 0.5f), CielOSIconLibrary.v(0.28f, 0.42f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.48f, 0.4f), CielOSIconLibrary.v(0.66f, 0.4f), 0.05f, false);
				return;
			case CielOSIconId.Files:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.28f, 0.16f),
					CielOSIconLibrary.v(0.28f, 0.84f),
					CielOSIconLibrary.v(0.6f, 0.84f),
					CielOSIconLibrary.v(0.72f, 0.72f),
					CielOSIconLibrary.v(0.72f, 0.16f)
				}, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.6f, 0.84f),
					CielOSIconLibrary.v(0.6f, 0.72f),
					CielOSIconLibrary.v(0.72f, 0.72f)
				}, true);
				CielOSIconLibrary.Rect(c, 0.36f, 0.58f, 0.64f, 0.62f, true);
				CielOSIconLibrary.Rect(c, 0.36f, 0.48f, 0.64f, 0.52f, true);
				CielOSIconLibrary.Rect(c, 0.36f, 0.38f, 0.55f, 0.42f, true);
				return;
			case CielOSIconId.Paint:
				CielOSIconLibrary.Disc(c, 0.5f, 0.5f, 0.34f, false);
				CielOSIconLibrary.Disc(c, 0.66f, 0.34f, 0.1f, true);
				CielOSIconLibrary.Disc(c, 0.38f, 0.66f, 0.055f, true);
				CielOSIconLibrary.Disc(c, 0.58f, 0.66f, 0.055f, true);
				CielOSIconLibrary.Disc(c, 0.3f, 0.48f, 0.055f, true);
				CielOSIconLibrary.Disc(c, 0.42f, 0.34f, 0.055f, true);
				return;
			case CielOSIconId.Music:
				CielOSIconLibrary.Disc(c, 0.34f, 0.3f, 0.1f, false);
				CielOSIconLibrary.Disc(c, 0.66f, 0.38f, 0.1f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.43f, 0.3f), CielOSIconLibrary.v(0.43f, 0.74f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.75f, 0.38f), CielOSIconLibrary.v(0.75f, 0.66f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.43f, 0.74f), CielOSIconLibrary.v(0.75f, 0.66f), 0.06f, false);
				return;
			case CielOSIconId.Settings:
			{
				CielOSIconLibrary.Ring(c, 0.5f, 0.5f, 0.24f, 0.1f, false);
				for (int i = 0; i < 8; i++)
				{
					float num = (float)i * 45f * 0.017453292f;
					float num2 = 0.5f + Mathf.Cos(num) * 0.31f;
					float num3 = 0.5f + Mathf.Sin(num) * 0.31f;
					CielOSIconLibrary.Disc(c, num2, num3, 0.07f, false);
				}
				CielOSIconLibrary.Disc(c, 0.5f, 0.5f, 0.1f, true);
				return;
			}
			case CielOSIconId.Weather:
				CielOSIconLibrary.Ring(c, 0.66f, 0.66f, 0.12f, 0.04f, false);
				CielOSIconLibrary.Disc(c, 0.36f, 0.42f, 0.13f, false);
				CielOSIconLibrary.Disc(c, 0.55f, 0.48f, 0.15f, false);
				CielOSIconLibrary.Disc(c, 0.68f, 0.4f, 0.12f, false);
				CielOSIconLibrary.Rect(c, 0.3f, 0.3f, 0.74f, 0.42f, false);
				return;
			case CielOSIconId.Folder:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.16f, 0.28f),
					CielOSIconLibrary.v(0.16f, 0.66f),
					CielOSIconLibrary.v(0.4f, 0.66f),
					CielOSIconLibrary.v(0.47f, 0.74f),
					CielOSIconLibrary.v(0.84f, 0.74f),
					CielOSIconLibrary.v(0.84f, 0.28f)
				}, false);
				return;
			case CielOSIconId.Avatar:
				CielOSIconLibrary.Disc(c, 0.5f, 0.62f, 0.16f, false);
				CielOSIconLibrary.Arc(c, 0.5f, 0.16f, 0.3f, 25f, 155f, 0.12f, false);
				CielOSIconLibrary.Rect(c, 0.2f, 0.1f, 0.8f, 0.2f, true);
				return;
			case CielOSIconId.DragonLogo:
				CielOSIconLibrary.DrawDragon(c);
				return;
			case CielOSIconId.ChevronDown:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.28f, 0.62f), CielOSIconLibrary.v(0.5f, 0.4f), 0.07f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.4f), CielOSIconLibrary.v(0.72f, 0.62f), 0.07f, false);
				return;
			case CielOSIconId.Close:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.3f, 0.3f), CielOSIconLibrary.v(0.7f, 0.7f), 0.08f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.7f, 0.3f), CielOSIconLibrary.v(0.3f, 0.7f), 0.08f, false);
				return;
			case CielOSIconId.Minimize:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.3f, 0.4f), CielOSIconLibrary.v(0.7f, 0.4f), 0.08f, false);
				return;
			case CielOSIconId.Workspace:
				CielOSIconLibrary.RoundRectStroke(c, 0.22f, 0.22f, 0.78f, 0.78f, 0.1f, 0.07f, false);
				return;
			case CielOSIconId.Grid:
				CielOSIconLibrary.Rect(c, 0.2f, 0.54f, 0.46f, 0.8f, false);
				CielOSIconLibrary.Rect(c, 0.54f, 0.54f, 0.8f, 0.8f, false);
				CielOSIconLibrary.Rect(c, 0.2f, 0.2f, 0.46f, 0.46f, false);
				CielOSIconLibrary.Rect(c, 0.54f, 0.2f, 0.8f, 0.46f, false);
				return;
			case CielOSIconId.ResizeGrip:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.78f, 0.22f), CielOSIconLibrary.v(0.86f, 0.3f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.62f, 0.22f), CielOSIconLibrary.v(0.86f, 0.46f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.46f, 0.22f), CielOSIconLibrary.v(0.86f, 0.62f), 0.05f, false);
				return;
			case CielOSIconId.Upload:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.3f), CielOSIconLibrary.v(0.5f, 0.74f), 0.07f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.74f), CielOSIconLibrary.v(0.36f, 0.58f), 0.07f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.74f), CielOSIconLibrary.v(0.64f, 0.58f), 0.07f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.24f, 0.24f), CielOSIconLibrary.v(0.76f, 0.24f), 0.06f, false);
				return;
			case CielOSIconId.Shuffle:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.18f, 0.32f), CielOSIconLibrary.v(0.5f, 0.32f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.32f), CielOSIconLibrary.v(0.78f, 0.66f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.18f, 0.66f), CielOSIconLibrary.v(0.5f, 0.66f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.66f), CielOSIconLibrary.v(0.78f, 0.32f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.78f, 0.66f), CielOSIconLibrary.v(0.66f, 0.66f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.78f, 0.66f), CielOSIconLibrary.v(0.78f, 0.54f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.78f, 0.32f), CielOSIconLibrary.v(0.66f, 0.32f), 0.05f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.78f, 0.32f), CielOSIconLibrary.v(0.78f, 0.44f), 0.05f, false);
				return;
			case CielOSIconId.Repeat:
			{
				float num4 = 0.055f;
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.34f, 0.72f), CielOSIconLibrary.v(0.62f, 0.72f), num4, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.38f, 0.28f), CielOSIconLibrary.v(0.66f, 0.28f), num4, false);
				CielOSIconLibrary.Arc(c, 0.34f, 0.5f, 0.22f, 90f, 270f, num4, false);
				CielOSIconLibrary.Arc(c, 0.66f, 0.5f, 0.22f, -90f, 90f, num4, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.62f, 0.72f), CielOSIconLibrary.v(0.55f, 0.8f), num4, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.62f, 0.72f), CielOSIconLibrary.v(0.55f, 0.64f), num4, false);
				return;
			}
			case CielOSIconId.Play:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.34f, 0.26f),
					CielOSIconLibrary.v(0.34f, 0.74f),
					CielOSIconLibrary.v(0.74f, 0.5f)
				}, false);
				return;
			case CielOSIconId.Pause:
				CielOSIconLibrary.Rect(c, 0.34f, 0.28f, 0.45f, 0.72f, false);
				CielOSIconLibrary.Rect(c, 0.55f, 0.28f, 0.66f, 0.72f, false);
				return;
			case CielOSIconId.SkipNext:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.28f, 0.28f),
					CielOSIconLibrary.v(0.28f, 0.72f),
					CielOSIconLibrary.v(0.56f, 0.5f)
				}, false);
				CielOSIconLibrary.Rect(c, 0.6f, 0.28f, 0.68f, 0.72f, false);
				return;
			case CielOSIconId.SkipPrev:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.72f, 0.28f),
					CielOSIconLibrary.v(0.72f, 0.72f),
					CielOSIconLibrary.v(0.44f, 0.5f)
				}, false);
				CielOSIconLibrary.Rect(c, 0.32f, 0.28f, 0.4f, 0.72f, false);
				return;
			case CielOSIconId.Snake:
				CielOSIconLibrary.Rect(c, 0.2f, 0.24f, 0.34f, 0.38f, false);
				CielOSIconLibrary.Rect(c, 0.2f, 0.38f, 0.34f, 0.62f, false);
				CielOSIconLibrary.Rect(c, 0.2f, 0.5f, 0.58f, 0.62f, false);
				CielOSIconLibrary.Disc(c, 0.66f, 0.56f, 0.1f, false);
				CielOSIconLibrary.Disc(c, 0.7f, 0.59f, 0.025f, true);
				CielOSIconLibrary.Disc(c, 0.74f, 0.3f, 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.74f, 0.36f), CielOSIconLibrary.v(0.78f, 0.42f), 0.025f, false);
				return;
			case CielOSIconId.CielX:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.24f, 0.24f), CielOSIconLibrary.v(0.76f, 0.76f), 0.1f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.76f, 0.24f), CielOSIconLibrary.v(0.24f, 0.76f), 0.1f, false);
				return;
			case CielOSIconId.Heart:
				CielOSIconLibrary.Disc(c, 0.36f, 0.62f, 0.16f, false);
				CielOSIconLibrary.Disc(c, 0.64f, 0.62f, 0.16f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.2f, 0.58f),
					CielOSIconLibrary.v(0.8f, 0.58f),
					CielOSIconLibrary.v(0.5f, 0.22f)
				}, false);
				return;
			case CielOSIconId.RepostArrows:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.28f, 0.4f), CielOSIconLibrary.v(0.72f, 0.4f), 0.06f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.72f, 0.32f),
					CielOSIconLibrary.v(0.72f, 0.48f),
					CielOSIconLibrary.v(0.84f, 0.4f)
				}, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.72f, 0.62f), CielOSIconLibrary.v(0.28f, 0.62f), 0.06f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.28f, 0.54f),
					CielOSIconLibrary.v(0.28f, 0.7f),
					CielOSIconLibrary.v(0.16f, 0.62f)
				}, false);
				return;
			case CielOSIconId.ReplyBubble:
				CielOSIconLibrary.RoundRectStroke(c, 0.18f, 0.34f, 0.82f, 0.78f, 0.1f, 0.05f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.34f, 0.34f),
					CielOSIconLibrary.v(0.46f, 0.34f),
					CielOSIconLibrary.v(0.3f, 0.2f)
				}, false);
				return;
			case CielOSIconId.BackArrow:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.5f), CielOSIconLibrary.v(0.8f, 0.5f), 0.055f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.5f), CielOSIconLibrary.v(0.46f, 0.7f), 0.055f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.5f), CielOSIconLibrary.v(0.46f, 0.3f), 0.055f, false);
				return;
			case CielOSIconId.Photos:
				CielOSIconLibrary.RoundRectStroke(c, 0.18f, 0.22f, 0.82f, 0.78f, 0.06f, 0.05f, false);
				CielOSIconLibrary.Disc(c, 0.36f, 0.6f, 0.06f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.22f, 0.3f),
					CielOSIconLibrary.v(0.46f, 0.54f),
					CielOSIconLibrary.v(0.34f, 0.3f)
				}, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.4f, 0.3f),
					CielOSIconLibrary.v(0.66f, 0.62f),
					CielOSIconLibrary.v(0.78f, 0.3f)
				}, false);
				return;
			case CielOSIconId.CustomizeCharacter:
				CielOSIconLibrary.Disc(c, 0.39f, 0.63f, 0.14f, false);
				CielOSIconLibrary.Arc(c, 0.39f, 0.2f, 0.24f, 28f, 152f, 0.11f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.6f, 0.66f), CielOSIconLibrary.v(0.8f, 0.66f), 0.045f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.6f, 0.43f), CielOSIconLibrary.v(0.8f, 0.43f), 0.045f, false);
				CielOSIconLibrary.Disc(c, 0.67f, 0.66f, 0.06f, false);
				CielOSIconLibrary.Disc(c, 0.74f, 0.43f, 0.06f, false);
				return;
			case CielOSIconId.StartRun:
			case CielOSIconId.RunModes:
				CielOSIconLibrary.RoundRectFill(c, 0.22f, 0.6f, 0.78f, 0.74f, 0.045f, false);
				CielOSIconLibrary.RoundRectFill(c, 0.22f, 0.42f, 0.78f, 0.56f, 0.045f, false);
				CielOSIconLibrary.RoundRectFill(c, 0.22f, 0.24f, 0.78f, 0.38f, 0.045f, false);
				CielOSIconLibrary.Disc(c, 0.31f, 0.67f, 0.03f, true);
				CielOSIconLibrary.Disc(c, 0.31f, 0.49f, 0.03f, true);
				CielOSIconLibrary.Disc(c, 0.31f, 0.31f, 0.03f, true);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.61f, 0.61f),
					CielOSIconLibrary.v(0.61f, 0.72f),
					CielOSIconLibrary.v(0.72f, 0.665f)
				}, false);
				return;
			case CielOSIconId.Info:
				CielOSIconLibrary.Ring(c, 0.5f, 0.5f, 0.32f, 0.06f, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.66f, 0.055f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.54f), CielOSIconLibrary.v(0.5f, 0.3f), 0.075f, false);
				return;
			case CielOSIconId.Check:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.2f, 0.5f), CielOSIconLibrary.v(0.42f, 0.3f), 0.09f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.42f, 0.3f), CielOSIconLibrary.v(0.8f, 0.7f), 0.09f, false);
				return;
			case CielOSIconId.Lock:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.26f, 0.14f),
					CielOSIconLibrary.v(0.26f, 0.5f),
					CielOSIconLibrary.v(0.74f, 0.5f),
					CielOSIconLibrary.v(0.74f, 0.14f)
				}, false);
				CielOSIconLibrary.Arc(c, 0.5f, 0.5f, 0.15f, 0f, 180f, 0.07f, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.34f, 0.055f, true);
				return;
			case CielOSIconId.LevelUp:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.46f), CielOSIconLibrary.v(0.5f, 0.68f), 0.075f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.68f), CielOSIconLibrary.v(0.74f, 0.46f), 0.075f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.18f), CielOSIconLibrary.v(0.5f, 0.4f), 0.075f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.4f), CielOSIconLibrary.v(0.74f, 0.18f), 0.075f, false);
				return;
			case CielOSIconId.MapPin:
				CielOSIconLibrary.Disc(c, 0.5f, 0.62f, 0.24f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.3f, 0.54f),
					CielOSIconLibrary.v(0.7f, 0.54f),
					CielOSIconLibrary.v(0.5f, 0.12f)
				}, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.62f, 0.095f, true);
				return;
			case CielOSIconId.Notebook:
				CielOSIconLibrary.RoundRectFill(c, 0.2f, 0.12f, 0.82f, 0.88f, 0.05f, false);
				CielOSIconLibrary.Disc(c, 0.27f, 0.24f, 0.035f, true);
				CielOSIconLibrary.Disc(c, 0.27f, 0.4f, 0.035f, true);
				CielOSIconLibrary.Disc(c, 0.27f, 0.56f, 0.035f, true);
				CielOSIconLibrary.Disc(c, 0.27f, 0.72f, 0.035f, true);
				CielOSIconLibrary.Rect(c, 0.38f, 0.64f, 0.72f, 0.675f, true);
				CielOSIconLibrary.Rect(c, 0.38f, 0.52f, 0.72f, 0.555f, true);
				CielOSIconLibrary.Rect(c, 0.38f, 0.4f, 0.62f, 0.435f, true);
				return;
			case CielOSIconId.ClassicRun:
				CielOSIconLibrary.RoundRectStroke(c, 0.28f, 0.22f, 0.67f, 0.78f, 0.06f, 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.38f, 0.5f), CielOSIconLibrary.v(0.77f, 0.5f), 0.055f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.77f, 0.5f), CielOSIconLibrary.v(0.64f, 0.62f), 0.055f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.77f, 0.5f), CielOSIconLibrary.v(0.64f, 0.38f), 0.055f, false);
				return;
			case CielOSIconId.FreeMode:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.68f), CielOSIconLibrary.v(0.74f, 0.68f), 0.045f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.5f), CielOSIconLibrary.v(0.74f, 0.5f), 0.045f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.26f, 0.32f), CielOSIconLibrary.v(0.74f, 0.32f), 0.045f, false);
				CielOSIconLibrary.Disc(c, 0.4f, 0.68f, 0.065f, false);
				CielOSIconLibrary.Disc(c, 0.63f, 0.5f, 0.065f, false);
				CielOSIconLibrary.Disc(c, 0.48f, 0.32f, 0.065f, false);
				return;
			case CielOSIconId.LongWayHome:
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.28f, 0.2f), CielOSIconLibrary.v(0.46f, 0.7f), 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.72f, 0.2f), CielOSIconLibrary.v(0.54f, 0.7f), 0.06f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.24f), CielOSIconLibrary.v(0.5f, 0.4f), 0.04f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.5f), CielOSIconLibrary.v(0.5f, 0.6f), 0.035f, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.24f, 0.72f), CielOSIconLibrary.v(0.76f, 0.72f), 0.04f, false);
				return;
			case CielOSIconId.GuidedRun:
				CielOSIconLibrary.Ring(c, 0.5f, 0.5f, 0.27f, 0.055f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.5f, 0.73f),
					CielOSIconLibrary.v(0.43f, 0.45f),
					CielOSIconLibrary.v(0.57f, 0.45f)
				}, false);
				CielOSIconLibrary.Disc(c, 0.5f, 0.5f, 0.055f, true);
				return;
			case CielOSIconId.Maps:
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.14f, 0.22f),
					CielOSIconLibrary.v(0.37f, 0.31f),
					CielOSIconLibrary.v(0.6f, 0.22f),
					CielOSIconLibrary.v(0.86f, 0.32f),
					CielOSIconLibrary.v(0.86f, 0.76f),
					CielOSIconLibrary.v(0.6f, 0.66f),
					CielOSIconLibrary.v(0.37f, 0.75f),
					CielOSIconLibrary.v(0.14f, 0.66f)
				}, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.37f, 0.31f), CielOSIconLibrary.v(0.37f, 0.75f), 0.03f, true);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.6f, 0.22f), CielOSIconLibrary.v(0.6f, 0.66f), 0.03f, true);
				CielOSIconLibrary.Disc(c, 0.62f, 0.64f, 0.12f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.52f, 0.59f),
					CielOSIconLibrary.v(0.72f, 0.59f),
					CielOSIconLibrary.v(0.62f, 0.37f)
				}, false);
				CielOSIconLibrary.Disc(c, 0.62f, 0.65f, 0.045f, true);
				return;
			case CielOSIconId.Leaderboard:
				CielOSIconLibrary.RoundRectStroke(c, 0.16f, 0.47f, 0.84f, 0.75f, 0.13f, 0.055f, false);
				CielOSIconLibrary.Poly(c, new Vector2[]
				{
					CielOSIconLibrary.v(0.29f, 0.81f),
					CielOSIconLibrary.v(0.71f, 0.81f),
					CielOSIconLibrary.v(0.66f, 0.48f),
					CielOSIconLibrary.v(0.58f, 0.4f),
					CielOSIconLibrary.v(0.42f, 0.4f),
					CielOSIconLibrary.v(0.34f, 0.48f)
				}, false);
				CielOSIconLibrary.Line(c, CielOSIconLibrary.v(0.5f, 0.42f), CielOSIconLibrary.v(0.5f, 0.24f), 0.07f, false);
				CielOSIconLibrary.RoundRectFill(c, 0.31f, 0.16f, 0.69f, 0.25f, 0.025f, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06009C69 RID: 40041 RVA: 0x00401CD4 File Offset: 0x003FFED4
		private static void DrawDragon(CielOSIconLibrary.Ctx c)
		{
			CielOSIconLibrary.Poly(c, new Vector2[]
			{
				CielOSIconLibrary.v(0.18f, 0.46f),
				CielOSIconLibrary.v(0.3f, 0.66f),
				CielOSIconLibrary.v(0.52f, 0.72f),
				CielOSIconLibrary.v(0.74f, 0.6f),
				CielOSIconLibrary.v(0.88f, 0.5f),
				CielOSIconLibrary.v(0.7f, 0.46f),
				CielOSIconLibrary.v(0.84f, 0.36f),
				CielOSIconLibrary.v(0.58f, 0.34f),
				CielOSIconLibrary.v(0.4f, 0.24f),
				CielOSIconLibrary.v(0.34f, 0.4f)
			}, false);
			CielOSIconLibrary.Poly(c, new Vector2[]
			{
				CielOSIconLibrary.v(0.42f, 0.66f),
				CielOSIconLibrary.v(0.3f, 0.86f),
				CielOSIconLibrary.v(0.5f, 0.72f)
			}, false);
			CielOSIconLibrary.Disc(c, 0.52f, 0.54f, 0.045f, true);
		}

		// Token: 0x06009C6A RID: 40042 RVA: 0x00401E31 File Offset: 0x00400031
		private static Vector2 v(float x, float y)
		{
			return new Vector2(x, y);
		}

		// Token: 0x06009C6B RID: 40043 RVA: 0x00401E3C File Offset: 0x0040003C
		private static void Plot(CielOSIconLibrary.Ctx c, int px, int py, float a, bool erase)
		{
			if (px < 0 || py < 0 || px >= c.N || py >= c.N)
			{
				return;
			}
			int num = py * c.N + px;
			if (erase)
			{
				c.Buf[num] *= 1f - a;
				return;
			}
			if (a > c.Buf[num])
			{
				c.Buf[num] = a;
			}
		}

		// Token: 0x06009C6C RID: 40044 RVA: 0x00401EA0 File Offset: 0x004000A0
		private static void Raster(CielOSIconLibrary.Ctx c, CielOSIconLibrary.Sdf sdf, float minX, float minY, float maxX, float maxY, bool erase)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt((minX - c.Aa) * (float)c.N));
			int num2 = Mathf.Max(0, Mathf.FloorToInt((minY - c.Aa) * (float)c.N));
			int num3 = Mathf.Min(c.N - 1, Mathf.CeilToInt((maxX + c.Aa) * (float)c.N));
			int num4 = Mathf.Min(c.N - 1, Mathf.CeilToInt((maxY + c.Aa) * (float)c.N));
			for (int i = num2; i <= num4; i++)
			{
				float num5 = ((float)i + 0.5f) / (float)c.N;
				for (int j = num; j <= num3; j++)
				{
					float num6 = ((float)j + 0.5f) / (float)c.N;
					float num7 = sdf(num6, num5);
					float num8 = Mathf.Clamp01(0.5f - num7 / c.Aa);
					if (num8 > 0f)
					{
						CielOSIconLibrary.Plot(c, j, i, num8, erase);
					}
				}
			}
		}

		// Token: 0x06009C6D RID: 40045 RVA: 0x00401FA4 File Offset: 0x004001A4
		private static void Disc(CielOSIconLibrary.Ctx c, float cx, float cy, float r, bool erase = false)
		{
			CielOSIconLibrary.Raster(c, (float x, float y) => Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy)) - r, cx - r, cy - r, cx + r, cy + r, erase);
		}

		// Token: 0x06009C6E RID: 40046 RVA: 0x00402014 File Offset: 0x00400214
		private static void Ring(CielOSIconLibrary.Ctx c, float cx, float cy, float r, float thickness, bool erase = false)
		{
			float h = thickness * 0.5f;
			CielOSIconLibrary.Raster(c, (float x, float y) => Mathf.Abs(Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy)) - r) - h, cx - r - h, cy - r - h, cx + r + h, cy + r + h, erase);
		}

		// Token: 0x06009C6F RID: 40047 RVA: 0x004020B0 File Offset: 0x004002B0
		private static void Rect(CielOSIconLibrary.Ctx c, float minX, float minY, float maxX, float maxY, bool erase = false)
		{
			CielOSIconLibrary.Raster(c, delegate(float x, float y)
			{
				float num = Mathf.Max(minX - x, x - maxX);
				float num2 = Mathf.Max(minY - y, y - maxY);
				return Mathf.Max(num, num2);
			}, minX, minY, maxX, maxY, erase);
		}

		// Token: 0x06009C70 RID: 40048 RVA: 0x0040210C File Offset: 0x0040030C
		private static void RoundRectStroke(CielOSIconLibrary.Ctx c, float minX, float minY, float maxX, float maxY, float radius, float thickness, bool erase = false)
		{
			float h = thickness * 0.5f;
			float cx0 = minX + radius;
			float cx1 = maxX - radius;
			float cy0 = minY + radius;
			float cy1 = maxY - radius;
			CielOSIconLibrary.Raster(c, delegate(float x, float y)
			{
				float num = Mathf.Max(new float[]
				{
					0f,
					cx0 - x,
					x - cx1
				});
				float num2 = Mathf.Max(new float[]
				{
					0f,
					cy0 - y,
					y - cy1
				});
				return Mathf.Abs(Mathf.Sqrt(num * num + num2 * num2) - radius) - h;
			}, minX - h, minY - h, maxX + h, maxY + h, erase);
		}

		// Token: 0x06009C71 RID: 40049 RVA: 0x004021A4 File Offset: 0x004003A4
		private static void Line(CielOSIconLibrary.Ctx c, Vector2 a, Vector2 b, float thickness, bool erase = false)
		{
			float h = thickness * 0.5f;
			Vector2 ab = b - a;
			float len2 = Mathf.Max(1E-06f, ab.sqrMagnitude);
			CielOSIconLibrary.Raster(c, delegate(float x, float y)
			{
				Vector2 vector = new Vector2(x - a.x, y - a.y);
				float num = Mathf.Clamp01(Vector2.Dot(vector, ab) / len2);
				float num2 = vector.x - ab.x * num;
				float num3 = vector.y - ab.y * num;
				return Mathf.Sqrt(num2 * num2 + num3 * num3) - h;
			}, Mathf.Min(a.x, b.x) - h, Mathf.Min(a.y, b.y) - h, Mathf.Max(a.x, b.x) + h, Mathf.Max(a.y, b.y) + h, erase);
		}

		// Token: 0x06009C72 RID: 40050 RVA: 0x00402280 File Offset: 0x00400480
		private static void Arc(CielOSIconLibrary.Ctx c, float cx, float cy, float r, float a0, float a1, float thickness, bool erase = false)
		{
			float h = thickness * 0.5f;
			float r0 = a0 * 0.017453292f;
			float r1 = a1 * 0.017453292f;
			CielOSIconLibrary.Raster(c, delegate(float x, float y)
			{
				float num = x - cx;
				float num2 = y - cy;
				float num3 = Mathf.Atan2(num2, num);
				float num4 = r1 - r0;
				float num5 = Mathf.Repeat(num3 - r0, 6.2831855f);
				float num6 = Mathf.Sqrt(num * num + num2 * num2);
				if (num5 <= num4)
				{
					return Mathf.Abs(num6 - r) - h;
				}
				Vector2 vector = new Vector2(cx + Mathf.Cos(r0) * r, cy + Mathf.Sin(r0) * r);
				Vector2 vector2 = new Vector2(cx + Mathf.Cos(r1) * r, cy + Mathf.Sin(r1) * r);
				float num7 = Vector2.Distance(new Vector2(x, y), vector);
				float num8 = Vector2.Distance(new Vector2(x, y), vector2);
				return Mathf.Min(num7, num8) - h;
			}, cx - r - h, cy - r - h, cx + r + h, cy + r + h, erase);
		}

		// Token: 0x06009C73 RID: 40051 RVA: 0x00402338 File Offset: 0x00400538
		private static void Poly(CielOSIconLibrary.Ctx c, Vector2[] pts, bool erase = false)
		{
			float num = 1f;
			float num2 = 1f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < pts.Length; i++)
			{
				num = Mathf.Min(num, pts[i].x);
				num2 = Mathf.Min(num2, pts[i].y);
				num3 = Mathf.Max(num3, pts[i].x);
				num4 = Mathf.Max(num4, pts[i].y);
			}
			CielOSIconLibrary.Raster(c, (float x, float y) => CielOSIconLibrary.PolySdf(pts, x, y), num, num2, num3, num4, erase);
		}

		// Token: 0x06009C74 RID: 40052 RVA: 0x00402400 File Offset: 0x00400600
		private static float PolySdf(Vector2[] p, float x, float y)
		{
			int num = p.Length;
			float num2 = float.MaxValue;
			bool flag = false;
			int i = 0;
			int num3 = num - 1;
			while (i < num)
			{
				Vector2 vector = p[i];
				Vector2 vector2 = p[num3];
				Vector2 vector3 = vector2 - vector;
				Vector2 vector4 = new Vector2(x - vector.x, y - vector.y);
				float num4 = Mathf.Clamp01(Vector2.Dot(vector4, vector3) / Mathf.Max(1E-06f, vector3.sqrMagnitude));
				float num5 = vector4.x - vector3.x * num4;
				float num6 = vector4.y - vector3.y * num4;
				num2 = Mathf.Min(num2, num5 * num5 + num6 * num6);
				if (vector.y > y != vector2.y > y && x < (vector2.x - vector.x) * (y - vector.y) / (vector2.y - vector.y + 1E-12f) + vector.x)
				{
					flag = !flag;
				}
				num3 = i++;
			}
			num2 = Mathf.Sqrt(num2);
			if (!flag)
			{
				return num2;
			}
			return -num2;
		}

		// Token: 0x0400A103 RID: 41219
		private const int Supersample = 4;

		// Token: 0x0400A104 RID: 41220
		private static readonly Dictionary<long, Sprite> _cache = new Dictionary<long, Sprite>();

		// Token: 0x0400A105 RID: 41221
		private static readonly Dictionary<long, Sprite> _coloredCache = new Dictionary<long, Sprite>();

		// Token: 0x0400A106 RID: 41222
		private static readonly Dictionary<string, Sprite> _logoCache = new Dictionary<string, Sprite>();

		// Token: 0x0400A107 RID: 41223
		private static readonly Dictionary<string, Sprite> _logoMaskCache = new Dictionary<string, Sprite>();

		// Token: 0x0400A108 RID: 41224
		private static readonly Dictionary<long, Sprite> _badgedCache = new Dictionary<long, Sprite>();

		// Token: 0x0400A109 RID: 41225
		private const float BadgeGlyphInset = 0.24f;

		// Token: 0x020013C1 RID: 5057
		private struct Ctx
		{
			// Token: 0x06009C76 RID: 40054 RVA: 0x00402557 File Offset: 0x00400757
			public Ctx(float[] buf, int n)
			{
				this.Buf = buf;
				this.N = n;
				this.Aa = 1.25f / (float)n;
			}

			// Token: 0x0400A10A RID: 41226
			public readonly float[] Buf;

			// Token: 0x0400A10B RID: 41227
			public readonly int N;

			// Token: 0x0400A10C RID: 41228
			public readonly float Aa;
		}

		// Token: 0x020013C2 RID: 5058
		// (Invoke) Token: 0x06009C78 RID: 40056
		private delegate float Sdf(float x, float y);
	}
}
