using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CielOS.UI
{
	// Token: 0x020013CB RID: 5067
	public static class CielOSShapes
	{
		// Token: 0x06009C8B RID: 40075 RVA: 0x00402898 File Offset: 0x00400A98
		public static Sprite RoundedRect(int radius = 10)
		{
			radius = Mathf.Max(1, radius);
			Sprite sprite;
			if (CielOSShapes.RoundedByRadius.TryGetValue(radius, out sprite) && sprite != null)
			{
				return sprite;
			}
			int num = 4;
			int num2 = 4;
			int num3 = radius * 2 + num2;
			int num4 = num3 * num;
			float num5 = (float)(radius * num);
			Color32[] array = new Color32[num3 * num3];
			float[] array2 = new float[num4 * num4];
			float num6 = num5;
			float num7 = num5;
			float num8 = (float)num4 - num5;
			float num9 = (float)num4 - num5;
			for (int i = 0; i < num4; i++)
			{
				for (int j = 0; j < num4; j++)
				{
					float num10 = Mathf.Max(new float[]
					{
						0f,
						num6 - ((float)j + 0.5f),
						(float)j + 0.5f - num8
					});
					float num11 = Mathf.Max(new float[]
					{
						0f,
						num7 - ((float)i + 0.5f),
						(float)i + 0.5f - num9
					});
					float num12 = Mathf.Sqrt(num10 * num10 + num11 * num11) - num5;
					array2[i * num4 + j] = Mathf.Clamp01(0.5f - num12);
				}
			}
			CielOSShapes.Downsample(array2, num4, array, num3, num);
			Texture2D texture2D = CielOSShapes.MakeTex(num3);
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			Sprite sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, (float)num3, (float)num3), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect, new Vector4((float)radius, (float)radius, (float)radius, (float)radius));
			sprite2.name = string.Format("CielOS_RoundedRect_R{0}", radius);
			CielOSShapes.RoundedByRadius[radius] = sprite2;
			return sprite2;
		}

		// Token: 0x06009C8C RID: 40076 RVA: 0x00402A44 File Offset: 0x00400C44
		public static Sprite Circle(int size = 64)
		{
			if (CielOSShapes._circle != null)
			{
				return CielOSShapes._circle;
			}
			int num = 4;
			int num2 = size * num;
			float num3 = (float)num2 * 0.5f;
			float num4 = (float)num2 * 0.5f - (float)num;
			Color32[] array = new Color32[size * size];
			float[] array2 = new float[num2 * num2];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num5 = (float)j + 0.5f - num3;
					float num6 = (float)i + 0.5f - num3;
					float num7 = Mathf.Sqrt(num5 * num5 + num6 * num6) - num4;
					array2[i * num2 + j] = Mathf.Clamp01(0.5f - num7);
				}
			}
			CielOSShapes.Downsample(array2, num2, array, size, num);
			Texture2D texture2D = CielOSShapes.MakeTex(size);
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			CielOSShapes._circle = Sprite.Create(texture2D, new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f);
			CielOSShapes._circle.name = "CielOS_Circle";
			return CielOSShapes._circle;
		}

		// Token: 0x06009C8D RID: 40077 RVA: 0x00402B5A File Offset: 0x00400D5A
		private static Texture2D MakeTex(int size)
		{
			return new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
		}

		// Token: 0x06009C8E RID: 40078 RVA: 0x00402B74 File Offset: 0x00400D74
		private static void Downsample(float[] buf, int hi, Color32[] outPx, int size, int ss)
		{
			int num = ss * ss;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num2 = 0f;
					int num3 = j * ss;
					int num4 = i * ss;
					for (int k = 0; k < ss; k++)
					{
						for (int l = 0; l < ss; l++)
						{
							num2 += buf[(num4 + k) * hi + (num3 + l)];
						}
					}
					num2 /= (float)num;
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num2 * 255f), 0, 255);
					outPx[i * size + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
		}

		// Token: 0x0400A12E RID: 41262
		private static readonly Dictionary<int, Sprite> RoundedByRadius = new Dictionary<int, Sprite>();

		// Token: 0x0400A12F RID: 41263
		private static Sprite _circle;
	}
}
