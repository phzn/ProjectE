using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x0200144E RID: 5198
	public static class CielXAvatars
	{
		// Token: 0x0600A096 RID: 41110 RVA: 0x00427484 File Offset: 0x00425684
		public static Sprite ForHandle(string handle, int size = 96)
		{
			int num = CielXAvatars.StableHash(handle);
			CielXAvatars.Gender gender = (((num & 1) == 0) ? CielXAvatars.Gender.Female : CielXAvatars.Gender.Male);
			Color color = CielXAvatars.Palette[(num >> 1) % CielXAvatars.Palette.Length];
			return CielXAvatars.Get(gender, color, size);
		}

		// Token: 0x0600A097 RID: 41111 RVA: 0x004274BE File Offset: 0x004256BE
		public static Sprite ForMe(Color accent, int size = 96)
		{
			return CielXAvatars.Get(CielXAvatars.Gender.Female, accent, size);
		}

		// Token: 0x0600A098 RID: 41112 RVA: 0x004274C8 File Offset: 0x004256C8
		public static Sprite Get(CielXAvatars.Gender gender, Color bg, int size)
		{
			long num = (((gender == CielXAvatars.Gender.Male) ? 1L : 0L) << 40) ^ ((long)(bg.r * 255f) << 24) ^ ((long)(bg.g * 255f) << 16) ^ ((long)(bg.b * 255f) << 8) ^ (long)size;
			Sprite sprite;
			if (CielXAvatars._cache.TryGetValue(num, out sprite) && sprite != null)
			{
				return sprite;
			}
			Sprite sprite2 = Sprite.Create(CielXAvatars.Render(gender, bg, size), new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = "CielXAvatar_" + gender.ToString();
			CielXAvatars._cache[num] = sprite2;
			return sprite2;
		}

		// Token: 0x0600A099 RID: 41113 RVA: 0x00427590 File Offset: 0x00425790
		private static Texture2D Render(CielXAvatars.Gender gender, Color bg, int size)
		{
			int num = 2;
			int num2 = size * num;
			Color32[] array = new Color32[num2 * num2];
			Color color = new Color(1f, 1f, 1f, 0.92f);
			Color color2 = new Color(0f, 0f, 0f, 0f);
			float num3 = (float)num2 * 0.5f;
			float num4 = (float)num2 * 0.5f;
			float num5 = (float)num2 * 0.5f - 1f;
			float num6 = 0.5f;
			float num7 = 0.62f;
			float num8 = 0.2f;
			float num9 = ((gender == CielXAvatars.Gender.Female) ? 0.3f : 0.34f);
			float num10 = 0.3f;
			float num11 = ((gender == CielXAvatars.Gender.Female) ? 0.16f : 0.12f);
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num12 = (float)j / (float)num2;
					float num13 = 1f - (float)i / (float)num2;
					float num14 = (float)j - num3;
					float num15 = (float)i - num4;
					bool flag = num14 * num14 + num15 * num15 <= num5 * num5;
					Color color3 = (flag ? bg : color2);
					if (flag)
					{
						bool flag2 = false;
						float num16 = num12 - num6;
						float num17 = num13 - num7;
						if (num16 * num16 + num17 * num17 <= num8 * num8)
						{
							flag2 = true;
						}
						if (!flag2 && gender == CielXAvatars.Gender.Female)
						{
							float num18 = num8 + 0.05f;
							if (num16 * num16 + num17 * num17 <= num18 * num18 && num13 > num7 - 0.02f)
							{
								flag2 = true;
							}
						}
						if (!flag2 && num13 < num10 + num11)
						{
							float num19 = num9 + num11 * Mathf.Clamp01((num10 - num13) / num10);
							float num20 = num10 + num11 * (1f - Mathf.Abs(num12 - 0.5f) / Mathf.Max(0.0001f, num9));
							if (Mathf.Abs(num12 - 0.5f) <= num19 && num13 <= num20)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							color3 = color;
						}
					}
					array[(num2 - 1 - i) * num2 + j] = color3;
				}
			}
			Color32[] array2 = new Color32[size * size];
			for (int k = 0; k < size; k++)
			{
				for (int l = 0; l < size; l++)
				{
					float num21 = 0f;
					float num22 = 0f;
					float num23 = 0f;
					float num24 = 0f;
					for (int m = 0; m < num; m++)
					{
						for (int n = 0; n < num; n++)
						{
							Color32 color4 = array[(k * num + m) * num2 + (l * num + n)];
							num21 += (float)color4.r;
							num22 += (float)color4.g;
							num23 += (float)color4.b;
							num24 += (float)color4.a;
						}
					}
					int num25 = num * num;
					array2[k * size + l] = new Color32((byte)(num21 / (float)num25), (byte)(num22 / (float)num25), (byte)(num23 / (float)num25), (byte)(num24 / (float)num25));
				}
			}
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.SetPixels32(array2);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x0600A09A RID: 41114 RVA: 0x004278A4 File Offset: 0x00425AA4
		private static int StableHash(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return 7;
			}
			int num = 17;
			foreach (char c in s)
			{
				num = num * 31 + (int)c;
			}
			return num & int.MaxValue;
		}

		// Token: 0x0400A448 RID: 42056
		private static readonly Dictionary<long, Sprite> _cache = new Dictionary<long, Sprite>();

		// Token: 0x0400A449 RID: 42057
		private static readonly Color[] Palette = new Color[]
		{
			new Color(0.55f, 0.4f, 0.78f),
			new Color(0.32f, 0.53f, 0.78f),
			new Color(0.78f, 0.42f, 0.55f),
			new Color(0.36f, 0.62f, 0.52f),
			new Color(0.8f, 0.55f, 0.32f),
			new Color(0.45f, 0.55f, 0.78f),
			new Color(0.7f, 0.45f, 0.7f),
			new Color(0.4f, 0.6f, 0.7f)
		};

		// Token: 0x0200144F RID: 5199
		public enum Gender
		{
			// Token: 0x0400A44B RID: 42059
			Female,
			// Token: 0x0400A44C RID: 42060
			Male
		}
	}
}
