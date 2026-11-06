using System;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000E8E RID: 3726
	public static class OxGramBubbleSprite
	{
		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x0600700E RID: 28686 RVA: 0x002D5218 File Offset: 0x002D3418
		public static Sprite Body
		{
			get
			{
				if (!(OxGramBubbleSprite._body != null))
				{
					return OxGramBubbleSprite._body = OxGramBubbleSprite.Build();
				}
				return OxGramBubbleSprite._body;
			}
		}

		// Token: 0x0600700F RID: 28687 RVA: 0x002D5238 File Offset: 0x002D3438
		private static Sprite Build()
		{
			int num = 64;
			Texture2D texture2D = new Texture2D(num, num, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp
			};
			Color32[] array = new Color32[num * num];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					int num2 = 0;
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < 4; l++)
						{
							float num3 = (float)j + ((float)l + 0.5f) / 4f;
							float num4 = (float)i + ((float)k + 0.5f) / 4f;
							if (OxGramBubbleSprite.InsideRounded(num3, num4, num, num, 20f))
							{
								num2++;
							}
						}
					}
					byte b = (byte)(255 * num2 / 16);
					array[i * num + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			Vector4 vector = new Vector4(20f, 20f, 20f, 20f);
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)num, (float)num), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect, vector);
		}

		// Token: 0x06007010 RID: 28688 RVA: 0x002D5380 File Offset: 0x002D3580
		private static bool InsideRounded(float x, float y, int w, int h, float r)
		{
			if (x < 0f || y < 0f || x > (float)w || y > (float)h)
			{
				return false;
			}
			float num = Mathf.Clamp(x, r, (float)w - r);
			float num2 = Mathf.Clamp(y, r, (float)h - r);
			float num3 = x - num;
			float num4 = y - num2;
			return num3 * num3 + num4 * num4 <= r * r;
		}

		// Token: 0x040072CD RID: 29389
		private static Sprite _body;

		// Token: 0x040072CE RID: 29390
		private const int Size = 64;

		// Token: 0x040072CF RID: 29391
		private const float Corner = 20f;

		// Token: 0x040072D0 RID: 29392
		private const int AA = 4;
	}
}
