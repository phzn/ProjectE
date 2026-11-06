using System;
using System.Runtime.CompilerServices;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.UI;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003B4 RID: 948
	[Serializable]
	public class SpriteRendererDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001876 RID: 6262 RVA: 0x0009BA5C File Offset: 0x00099C5C
		public void Draw(FObject fobject, Sprite sprite, GameObject target)
		{
			SpriteRenderer spriteRenderer;
			target.TryAddComponent<SpriteRenderer>(out spriteRenderer, false);
			spriteRenderer.sprite = sprite;
			spriteRenderer.sortingOrder = target.transform.GetSiblingIndex();
			if (sprite == null)
			{
				spriteRenderer.sprite = FcuConfig.WhiteSprite32px;
				spriteRenderer.drawMode = SpriteDrawMode.Tiled;
				Vector2 size = target.GetComponent<RectTransform>().rect.size;
				spriteRenderer.size = size;
				this.SetColor(fobject, spriteRenderer);
			}
			else
			{
				spriteRenderer.drawMode = SpriteDrawMode.Simple;
				if (fobject.Data.FcuImageType == FcuImageType.Generative || !fobject.Data.Graphic.SpriteSingleColor.IsDefault<Color>())
				{
					this.SetColor(fobject, spriteRenderer);
				}
				else
				{
					spriteRenderer.color = Color.white;
				}
			}
			spriteRenderer.flipX = this.monoBeh.Settings.SpriteRendererSettings.FlipX;
			spriteRenderer.flipY = this.monoBeh.Settings.SpriteRendererSettings.FlipY;
			spriteRenderer.maskInteraction = this.monoBeh.Settings.SpriteRendererSettings.MaskInteraction;
			spriteRenderer.spriteSortPoint = this.monoBeh.Settings.SpriteRendererSettings.SortPoint;
			spriteRenderer.sortingLayerName = this.monoBeh.Settings.SpriteRendererSettings.SortingLayer;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0009BB98 File Offset: 0x00099D98
		public void SetColor(FObject fobject, SpriteRenderer img)
		{
			SpriteRendererDrawer.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.img = img;
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
				Vector4 cornerRadius = this.monoBeh.GraphicHelpers.GetCornerRadius(fobject);
				if (graphic.HasStroke && !graphic.HasFill)
				{
					Color white = Color.white;
					white.a = 0f;
					CS$<>8__locals1.img.color = white;
					SpriteRendererDrawer.<SetColor>g__AddOutline|1_0(fobject.StrokeWeight, cornerRadius.x, graphic.Stroke.SingleColor, ref CS$<>8__locals1);
				}
				else
				{
					CS$<>8__locals1.img.color = graphic.Fill.SingleColor;
					if (graphic.HasStroke)
					{
						SpriteRendererDrawer.<SetColor>g__AddOutline|1_0(fobject.StrokeWeight, cornerRadius.x, graphic.Stroke.SingleColor, ref CS$<>8__locals1);
					}
				}
				if (!graphic.HasStroke)
				{
					fobject.Data.GameObject.TryDestroyComponent<SpriteOutline>();
					return;
				}
			}
			else if (fobject.IsGenerativeType())
			{
				if (graphic.HasFill)
				{
					CS$<>8__locals1.img.color = graphic.Fill.SingleColor;
					return;
				}
				if (graphic.HasStroke)
				{
					CS$<>8__locals1.img.color = graphic.Stroke.SingleColor;
					return;
				}
			}
			else if (fobject.IsDownloadableType())
			{
				if (!fobject.Data.Graphic.SpriteSingleColor.IsDefault<Color>())
				{
					CS$<>8__locals1.img.color = fobject.Data.Graphic.SpriteSingleColor;
					return;
				}
				CS$<>8__locals1.img.color = Color.white;
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0009BD84 File Offset: 0x00099F84
		[CompilerGenerated]
		internal static void <SetColor>g__AddOutline|1_0(float strokeWeight, float cornerRadius, Color color, ref SpriteRendererDrawer.<>c__DisplayClass1_0 A_3)
		{
			SpriteOutline spriteOutline;
			A_3.img.gameObject.TryAddComponent<SpriteOutline>(out spriteOutline, false);
			spriteOutline.OutlineWidth = strokeWeight;
			spriteOutline.CornerSegments = 10;
			spriteOutline.CornerRadius = cornerRadius;
			spriteOutline.FillCenter = false;
			spriteOutline.color = color;
		}
	}
}
