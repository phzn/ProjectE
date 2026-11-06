using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000350 RID: 848
	public class TextureBorderDrawer
	{
		// Token: 0x0600166A RID: 5738 RVA: 0x000932EC File Offset: 0x000914EC
		public static Texture2D CreateTextureWithBorder(int width, int height, int leftWidth, int rightWidth, int topWidth, int bottomWidth, Color borderColor)
		{
			Texture2D texture2D = new Texture2D(width, height);
			Color color = new Color(0f, 0f, 0f, 0f);
			for (int i = 0; i < texture2D.height; i++)
			{
				for (int j = 0; j < texture2D.width; j++)
				{
					texture2D.SetPixel(j, i, color);
				}
			}
			if (leftWidth > 0)
			{
				TextureBorderDrawer.DrawLeftBorder(texture2D, leftWidth, borderColor);
			}
			if (rightWidth > 0)
			{
				TextureBorderDrawer.DrawRightBorder(texture2D, rightWidth, borderColor);
			}
			if (topWidth > 0)
			{
				TextureBorderDrawer.DrawTopBorder(texture2D, topWidth, borderColor);
			}
			if (bottomWidth > 0)
			{
				TextureBorderDrawer.DrawBottomBorder(texture2D, bottomWidth, borderColor);
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00093388 File Offset: 0x00091588
		private static void DrawLeftBorder(Texture2D texture, int borderWidth, Color borderColor)
		{
			for (int i = 0; i < borderWidth; i++)
			{
				for (int j = 0; j < texture.height; j++)
				{
					texture.SetPixel(i, j, borderColor);
				}
			}
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000933BC File Offset: 0x000915BC
		private static void DrawRightBorder(Texture2D texture, int borderWidth, Color borderColor)
		{
			for (int i = texture.width - borderWidth; i < texture.width; i++)
			{
				for (int j = 0; j < texture.height; j++)
				{
					texture.SetPixel(i, j, borderColor);
				}
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000933FC File Offset: 0x000915FC
		private static void DrawTopBorder(Texture2D texture, int borderWidth, Color borderColor)
		{
			for (int i = texture.height - borderWidth; i < texture.height; i++)
			{
				for (int j = 0; j < texture.width; j++)
				{
					texture.SetPixel(j, i, borderColor);
				}
			}
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0009343C File Offset: 0x0009163C
		private static void DrawBottomBorder(Texture2D texture, int borderWidth, Color borderColor)
		{
			for (int i = 0; i < borderWidth; i++)
			{
				for (int j = 0; j < texture.width; j++)
				{
					texture.SetPixel(j, i, borderColor);
				}
			}
		}
	}
}
