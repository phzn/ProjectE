using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000354 RID: 852
	[Serializable]
	public class SpriteSlicer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600167C RID: 5756 RVA: 0x000937F4 File Offset: 0x000919F4
		public async Task SliceSprites(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return;
				}
				if (fobject.IsSprite() && !fobject.Children.IsEmpty<FObject>() && fobject.Children.Count == 9)
				{
					Sprite sprite = this.monoBeh.SpriteProcessor.GetSprite(fobject);
					if (!(sprite == null))
					{
						FObject fobject2 = fobject.Children[0];
						FObject fobject3 = fobject.Children[1];
						FObject fobject4 = fobject.Children[2];
						FObject fobject5 = fobject.Children[3];
						FObject fobject6 = fobject.Children[4];
						FObject fobject7 = fobject.Children[5];
						FObject fobject8 = fobject.Children[6];
						FObject fobject9 = fobject.Children[7];
						FObject fobject10 = fobject.Children[8];
						float scale = fobject.Data.Scale;
						int num = (int)(fobject2.Size.x * scale);
						int num2 = (int)(fobject2.Size.y * scale);
						int num3 = (int)(fobject4.Size.x * scale);
						int num4 = (int)(fobject8.Size.y * scale);
						Vector4 vector = new Vector4((float)num, (float)num4, (float)num3, (float)num2);
						Texture2D texture2D = this.CreateMinimal9Slice(sprite, vector);
						this.monoBeh.SpriteProcessor.SaveSprite(texture2D, fobject);
						sprite = this.monoBeh.SpriteProcessor.GetSprite(fobject);
						this.monoBeh.DelegateHolder.SetSpriteRects(sprite, vector);
						await Task.Yield();
					}
				}
			}
			List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00093840 File Offset: 0x00091A40
		public Texture2D CreateMinimal9Slice(Sprite sourceSprite, Vector4 borders)
		{
			int num = (int)borders.x;
			int num2 = (int)borders.y;
			int num3 = (int)borders.z;
			int num4 = (int)borders.w;
			int num5 = num + num3;
			int num6 = num4 + num2;
			Texture2D texture2D = new Texture2D(num5, num6, sourceSprite.texture.format, false);
			Color[] pixels = sourceSprite.texture.GetPixels();
			int width = sourceSprite.texture.width;
			int height = sourceSprite.texture.height;
			Color[] array = new Color[num5 * num6];
			for (int i = 0; i < num4; i++)
			{
				for (int j = 0; j < num; j++)
				{
					int num7 = j + (height - 1 - i) * width;
					int num8 = j + (num6 - 1 - i) * num5;
					array[num8] = pixels[num7];
				}
			}
			for (int k = 0; k < num4; k++)
			{
				for (int l = 0; l < num3; l++)
				{
					int num9 = width - num3 + l + (height - 1 - k) * width;
					int num10 = num + l + (num6 - 1 - k) * num5;
					array[num10] = pixels[num9];
				}
			}
			for (int m = 0; m < num2; m++)
			{
				for (int n = 0; n < num; n++)
				{
					int num11 = n + m * width;
					int num12 = n + m * num5;
					array[num12] = pixels[num11];
				}
			}
			for (int num13 = 0; num13 < num2; num13++)
			{
				for (int num14 = 0; num14 < num3; num14++)
				{
					int num15 = width - num3 + num14 + num13 * width;
					int num16 = num + num14 + num13 * num5;
					array[num16] = pixels[num15];
				}
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}
	}
}
