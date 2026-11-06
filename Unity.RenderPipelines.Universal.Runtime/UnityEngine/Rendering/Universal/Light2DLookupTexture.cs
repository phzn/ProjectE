using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000032 RID: 50
	internal static class Light2DLookupTexture
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000EEC2 File Offset: 0x0000D0C2
		public static Texture GetLightLookupTexture()
		{
			if (Light2DLookupTexture.s_PointLightLookupTexture == null)
			{
				Light2DLookupTexture.s_PointLightLookupTexture = Light2DLookupTexture.CreatePointLightLookupTexture();
			}
			return Light2DLookupTexture.s_PointLightLookupTexture;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000EEE0 File Offset: 0x0000D0E0
		private static Texture2D CreatePointLightLookupTexture()
		{
			GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
			if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.SetPixels))
			{
				graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
			}
			else if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R32G32B32A32_SFloat, FormatUsage.SetPixels))
			{
				graphicsFormat = GraphicsFormat.R32G32B32A32_SFloat;
			}
			Texture2D texture2D = new Texture2D(256, 256, graphicsFormat, TextureCreationFlags.None);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			Vector2 vector = new Vector2(128f, 128f);
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 256; j++)
				{
					Vector2 vector2 = new Vector2((float)j, (float)i);
					float num = Vector2.Distance(vector2, vector);
					Vector2 vector3 = vector2 - vector;
					Vector2 vector4 = vector - vector2;
					vector4.Normalize();
					float num2;
					if (j == 255 || i == 255)
					{
						num2 = 0f;
					}
					else
					{
						num2 = Mathf.Clamp(1f - 2f * num / 256f, 0f, 1f);
					}
					float num3 = Mathf.Acos(Vector2.Dot(Vector2.down, vector3.normalized)) / 3.1415927f;
					float num4 = Mathf.Clamp(1f - num3, 0f, 1f);
					float x = vector4.x;
					float y = vector4.y;
					Color color = new Color(num2, num4, x, y);
					texture2D.SetPixel(j, i, color);
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x0400010A RID: 266
		private static Texture2D s_PointLightLookupTexture;
	}
}
