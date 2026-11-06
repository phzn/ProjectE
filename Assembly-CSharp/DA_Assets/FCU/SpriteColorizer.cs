using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000337 RID: 823
	[Serializable]
	public class SpriteColorizer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600161A RID: 5658 RVA: 0x000908C4 File Offset: 0x0008EAC4
		public async Task ColorizeSprites(List<FObject> fobjects)
		{
			if (!this.monoBeh.UsingSVG())
			{
				foreach (FObject fobject in fobjects)
				{
					if (this.monoBeh.IsCancellationRequested(TokenType.Import))
					{
						return;
					}
					if (fobject.Data.FcuImageType == FcuImageType.Downloadable)
					{
						if (this.monoBeh.UsingSpriteRenderer())
						{
							if (!fobject.Data.Graphic.HasSingleColor)
							{
								continue;
							}
						}
						else if (this.monoBeh.IsUITK() || this.monoBeh.IsNova())
						{
							if (!fobject.Data.Graphic.HasSingleColor)
							{
								continue;
							}
							if (fobject.Data.Graphic.HasSingleGradient)
							{
								continue;
							}
						}
						else if ((!fobject.Data.Graphic.HasSingleColor && !fobject.Data.Graphic.HasSingleGradient) || (fobject.Data.Graphic.HasSingleGradient && this.monoBeh.Settings.ImageSpritesSettings.DownloadOptions.HasFlag(SpriteDownloadOptions.SupportedGradients)))
						{
							continue;
						}
						if (File.Exists(fobject.Data.SpritePath.GetFullAssetPath()))
						{
							byte[] array = File.ReadAllBytes(fobject.Data.SpritePath.GetFullAssetPath());
							Texture2D texture2D = new Texture2D(fobject.Data.SpriteSize.x, fobject.Data.SpriteSize.y);
							texture2D.LoadImage(array);
							texture2D.Colorize(Color.white);
							byte[] array2 = new byte[0];
							ImageFormat imageFormat = this.monoBeh.Settings.ImageSpritesSettings.ImageFormat;
							if (imageFormat != ImageFormat.PNG)
							{
								if (imageFormat == ImageFormat.JPG)
								{
									array2 = texture2D.EncodeToJPG();
								}
							}
							else
							{
								array2 = texture2D.EncodeToPNG();
							}
							File.WriteAllBytes(fobject.Data.SpritePath, array2);
							await Task.Yield();
						}
					}
				}
				List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
			}
		}
	}
}
