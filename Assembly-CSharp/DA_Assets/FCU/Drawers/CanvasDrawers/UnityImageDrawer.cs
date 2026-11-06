using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003B6 RID: 950
	[Serializable]
	public class UnityImageDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600187A RID: 6266 RVA: 0x0009BDCC File Offset: 0x00099FCC
		public void Draw(FObject fobject, Sprite sprite, GameObject target)
		{
			MaskableGraphic maskableGraphic;
			if (this.monoBeh.UsingRawImage())
			{
				RawImage rawImage;
				target.TryAddGraphic<RawImage>(out rawImage);
				maskableGraphic = rawImage;
				if (sprite != null)
				{
					rawImage.texture = sprite.texture;
				}
			}
			else
			{
				Image image;
				target.TryAddGraphic<Image>(out image);
				maskableGraphic = image;
				image.sprite = sprite;
				image.type = this.monoBeh.Settings.UnityImageSettings.Type;
				image.preserveAspect = this.monoBeh.Settings.UnityImageSettings.PreserveAspect;
			}
			maskableGraphic.raycastTarget = this.monoBeh.Settings.UnityImageSettings.RaycastTarget;
			maskableGraphic.maskable = this.monoBeh.Settings.UnityImageSettings.Maskable;
			maskableGraphic.raycastPadding = this.monoBeh.Settings.UnityImageSettings.RaycastPadding;
			if (this.monoBeh.UseImageLinearMaterial())
			{
				maskableGraphic.material = FcuConfig.ImageLinearMaterial;
			}
			else
			{
				maskableGraphic.material = null;
			}
			this.SetColor(fobject, maskableGraphic);
			this.monoBeh.CanvasDrawer.ImageDrawer.TryAddCornerRounder(fobject, target);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0009BEE4 File Offset: 0x0009A0E4
		public void SetColor(FObject fobject, MaskableGraphic img)
		{
			FGraphic graphic = fobject.Data.Graphic;
			FcuLogger.Debug(string.Format("SetUnityImageColor | {0} | {1} | graphic.HasFills: {2} | graphic.HasStrokes: {3}", new object[]
			{
				fobject.Data.NameHierarchy,
				fobject.Data.FcuImageType,
				graphic.HasFill,
				graphic.HasStroke
			}), FcuDebugSettingsFlags.LogComponentDrawer);
			if (fobject.IsDrawableType())
			{
				if (graphic.HasStroke && !graphic.HasFill)
				{
					img.color = default(Color);
				}
				else if (graphic.Fill.HasSolid)
				{
					img.color = graphic.Fill.SolidPaint.Color;
				}
				else if (graphic.HasStroke && graphic.Fill.HasGradient)
				{
					img.color = graphic.Fill.SingleColor;
				}
				else if (graphic.Fill.HasGradient)
				{
					img.color = Color.white;
					this.monoBeh.CanvasDrawer.ImageDrawer.AddGradient(fobject, graphic.Fill.GradientPaint, false);
				}
				if (graphic.HasFill && graphic.HasStroke)
				{
					this.monoBeh.CanvasDrawer.ImageDrawer.AddUnityOutline(fobject);
				}
				if (!graphic.HasStroke)
				{
					fobject.Data.GameObject.TryDestroyComponent<Outline>();
					return;
				}
			}
			else if (fobject.IsGenerativeType())
			{
				if (graphic.HasFill && graphic.HasStroke)
				{
					this.monoBeh.CanvasDrawer.ImageDrawer.AddUnityOutline(fobject);
					return;
				}
				if (graphic.HasFill)
				{
					if (graphic.Fill.HasSolid)
					{
						img.color = graphic.Fill.SolidPaint.Color;
						return;
					}
					if (graphic.Fill.HasGradient)
					{
						img.color = Color.white;
						this.monoBeh.CanvasDrawer.ImageDrawer.AddGradient(fobject, graphic.Fill.GradientPaint, false);
						return;
					}
				}
				else if (graphic.HasStroke)
				{
					if (graphic.Stroke.HasSolid)
					{
						img.color = graphic.Stroke.SolidPaint.Color;
						return;
					}
					img.color = Color.white;
					this.monoBeh.CanvasDrawer.ImageDrawer.AddGradient(fobject, graphic.Stroke.GradientPaint, false);
					return;
				}
			}
			else if (fobject.IsDownloadableType())
			{
				if (!fobject.Data.Graphic.SpriteSingleColor.IsDefault<Color>())
				{
					img.color = fobject.Data.Graphic.SpriteSingleColor;
					return;
				}
				if (fobject.Data.Graphic.HasSingleGradient && !this.monoBeh.Settings.ImageSpritesSettings.DownloadOptions.HasFlag(SpriteDownloadOptions.SupportedGradients))
				{
					this.monoBeh.CanvasDrawer.ImageDrawer.AddGradient(fobject, fobject.Data.Graphic.SpriteSingleLinearGradient, false);
					return;
				}
				img.color = Color.white;
			}
		}
	}
}
