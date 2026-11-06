using System;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E52 RID: 3666
	internal static class PhoneUiSpriteFactory
	{
		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06006D82 RID: 28034 RVA: 0x002BFC66 File Offset: 0x002BDE66
		public static Sprite Circle
		{
			get
			{
				if (PhoneUiSpriteFactory._circle == null)
				{
					PhoneUiSpriteFactory._circle = PhoneUiSpriteFactory.CreateShape("Phone UI Circle", 64, 31f, false);
				}
				return PhoneUiSpriteFactory._circle;
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06006D83 RID: 28035 RVA: 0x002BFC91 File Offset: 0x002BDE91
		public static Sprite Rounded
		{
			get
			{
				if (PhoneUiSpriteFactory._rounded == null)
				{
					PhoneUiSpriteFactory._rounded = PhoneUiSpriteFactory.CreateShape("Phone UI Rounded", 32, 7f, true);
				}
				return PhoneUiSpriteFactory._rounded;
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06006D84 RID: 28036 RVA: 0x002BFCBC File Offset: 0x002BDEBC
		public static Sprite Widget
		{
			get
			{
				if (PhoneUiSpriteFactory._widget == null)
				{
					PhoneUiSpriteFactory._widget = PhoneUiSpriteFactory.CreateShape("Phone UI Widget", 256, 36f, true);
				}
				return PhoneUiSpriteFactory._widget;
			}
		}

		// Token: 0x06006D85 RID: 28037 RVA: 0x002BFCEC File Offset: 0x002BDEEC
		private static Sprite CreateShape(string name, int size, float radius, bool roundedRectangle)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false, true)
			{
				name = name + " Texture",
				hideFlags = HideFlags.HideAndDontSave,
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp
			};
			Color32[] array = new Color32[size * size];
			float num = (float)(size - 1) * 0.5f;
			float num2 = (roundedRectangle ? (num - radius) : 0f);
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num3 = Mathf.Max(Mathf.Abs((float)j - num) - num2, 0f);
					float num4 = Mathf.Max(Mathf.Abs((float)i - num) - num2, 0f);
					float num5 = Mathf.Clamp01(radius - Mathf.Sqrt(num3 * num3 + num4 * num4) + 0.5f);
					array[i * size + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.RoundToInt(num5 * 255f));
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply(false, true);
			Vector4 vector = (roundedRectangle ? new Vector4(radius + 1f, radius + 1f, radius + 1f, radius + 1f) : Vector4.zero);
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)size, (float)size), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect, vector);
			sprite.name = name;
			sprite.hideFlags = HideFlags.HideAndDontSave;
			return sprite;
		}

		// Token: 0x04007024 RID: 28708
		private static Sprite _circle;

		// Token: 0x04007025 RID: 28709
		private static Sprite _rounded;

		// Token: 0x04007026 RID: 28710
		private static Sprite _widget;
	}
}
