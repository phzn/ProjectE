using System;
using System.Runtime.CompilerServices;
using DA_Assets.CR;
using DA_Assets.DAG;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003B2 RID: 946
	[Serializable]
	public class ImageDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600186D RID: 6253 RVA: 0x0009B510 File Offset: 0x00099710
		public void Draw(FObject fobject, GameObject customGameObject = null)
		{
			GameObject gameObject = ((customGameObject == null) ? fobject.Data.GameObject : customGameObject);
			Graphic graphic;
			if (!fobject.Data.GameObject.IsPartOfAnyPrefab() && gameObject.TryGetComponentSafe<Graphic>(out graphic))
			{
				Type currentImageType = this.monoBeh.GetCurrentImageType();
				if (!graphic.GetType().Equals(currentImageType))
				{
					graphic.RemoveComponentsDependingOn();
					graphic.Destroy();
				}
			}
			Sprite sprite = this.monoBeh.SpriteProcessor.GetSprite(fobject);
			if (sprite == null && (fobject.IsSingleImageOrVideoOrEmojiType() || fobject.IsSprite()))
			{
				sprite = FcuConfig.MissingImageTexture128px;
			}
			if (!this.monoBeh.IsNova())
			{
				if (this.monoBeh.UsingUnityImage() || this.monoBeh.UsingRawImage() || fobject.IsObjectMask() || fobject.CanUseUnityImage(this.monoBeh))
				{
					this.UnityImageDrawer.Draw(fobject, sprite, gameObject);
				}
				else if (!this.monoBeh.UsingSvgImage())
				{
					if (this.monoBeh.UsingSpriteRenderer())
					{
						this.SpriteRendererDrawer.Draw(fobject, sprite, gameObject);
					}
					else if (!this.monoBeh.UsingShapes2D() && !this.monoBeh.UsingJoshPui() && !this.monoBeh.UsingDttPui())
					{
						this.monoBeh.UsingMPUIKit();
					}
				}
			}
			Image image;
			if (fobject.ContainsTag(FcuTag.Slice9) && !this.monoBeh.IsNova() && gameObject.TryGetComponent<Image>(out image))
			{
				image.type = Image.Type.Sliced;
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0009B688 File Offset: 0x00099888
		public void AddUnityOutline(FObject fobject)
		{
			FGraphic graphic = fobject.Data.Graphic;
			Outline outline;
			fobject.Data.GameObject.TryAddComponent<Outline>(out outline, false);
			outline.useGraphicAlpha = false;
			outline.effectDistance = new Vector2(fobject.StrokeWeight, -fobject.StrokeWeight);
			if (graphic.Stroke.HasSolid)
			{
				outline.effectColor = graphic.Stroke.SolidPaint.Color;
				return;
			}
			if (graphic.Stroke.HasGradient)
			{
				outline.effectColor = graphic.Stroke.SingleColor;
				return;
			}
			outline.effectColor = default(Color);
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0009B73C File Offset: 0x0009993C
		public void AddGradient(FObject fobject, Paint gradientColor, bool strokeOnly = false)
		{
			ImageDrawer.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.gameObject = fobject.Data.GameObject;
			CS$<>8__locals1.gradientColorKeys = gradientColor.ToGradientColorKeys();
			CS$<>8__locals1.gradientAlphaKeys = gradientColor.ToGradientAlphaKeys();
			if (gradientColor.Type == PaintType.GRADIENT_RADIAL)
			{
				CS$<>8__locals1.angle = this.monoBeh.GraphicHelpers.ToRadialAngle(fobject, gradientColor.GradientHandlePositions);
			}
			else
			{
				CS$<>8__locals1.angle = this.monoBeh.GraphicHelpers.ToLinearAngle(fobject, gradientColor.GradientHandlePositions);
			}
			if (this.monoBeh.UsingShapes2D() && !strokeOnly)
			{
				ImageDrawer.<AddGradient>g__AddDAGradient|2_0(ref CS$<>8__locals1);
				return;
			}
			if (!this.monoBeh.UsingMPUIKit() && !this.monoBeh.UsingDttPui())
			{
				ImageDrawer.<AddGradient>g__AddDAGradient|2_0(ref CS$<>8__locals1);
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0009B7FC File Offset: 0x000999FC
		public bool TryAddCornerRounder(FObject fobject, GameObject target)
		{
			if (fobject.IsSprite())
			{
				return false;
			}
			if (fobject.ContainsRoundedCorners())
			{
				CornerRounder cornerRounder;
				target.TryAddComponent<CornerRounder>(out cornerRounder, false);
				Vector4 cornerRadius = this.monoBeh.GraphicHelpers.GetCornerRadius(fobject);
				cornerRounder.SetRadii(cornerRadius);
			}
			return false;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0009B840 File Offset: 0x00099A40
		public void SetProceduralColor(FObject fobject, Image img, Action setStrokeOnlyWidth, Action setStroke)
		{
			FGraphic graphic = fobject.Data.Graphic;
			FcuLogger.Debug(string.Format("SetUnityImageColor | {0} | {1} | hasFills: {2} | hasStroke: {3}", new object[]
			{
				fobject.Data.NameHierarchy,
				fobject.Data.FcuImageType,
				graphic.HasFill,
				graphic.HasStroke
			}), FcuDebugSettingsFlags.LogComponentDrawer);
			if (graphic.HasStroke && !graphic.HasFill)
			{
				setStrokeOnlyWidth();
				if (graphic.Stroke.HasSolid)
				{
					img.color = graphic.Stroke.SolidPaint.Color;
				}
				else if (graphic.Stroke.HasGradient)
				{
					img.color = Color.white;
					this.monoBeh.CanvasDrawer.ImageDrawer.AddGradient(fobject, graphic.Stroke.GradientPaint, false);
				}
			}
			else
			{
				if (graphic.Fill.HasSolid)
				{
					img.color = graphic.Fill.SolidPaint.Color;
				}
				else if (graphic.Fill.HasGradient)
				{
					img.color = Color.white;
					this.monoBeh.CanvasDrawer.ImageDrawer.AddGradient(fobject, graphic.Fill.GradientPaint, false);
				}
				if (graphic.HasStroke)
				{
					setStroke();
				}
			}
			if (!graphic.HasStroke)
			{
				fobject.Data.GameObject.TryDestroyComponent<Outline>();
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x0009B9D9 File Offset: 0x00099BD9
		[SerializeProperty("unityImageDrawer")]
		public UnityImageDrawer UnityImageDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.unityImageDrawer);
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x0009B9EC File Offset: 0x00099BEC
		[SerializeProperty("spriteRendererDrawer")]
		public SpriteRendererDrawer SpriteRendererDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.spriteRendererDrawer);
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0009BA00 File Offset: 0x00099C00
		[CompilerGenerated]
		internal static void <AddGradient>g__AddDAGradient|2_0(ref ImageDrawer.<>c__DisplayClass2_0 A_0)
		{
			DAGradient dagradient;
			A_0.gameObject.TryAddComponent<DAGradient>(out dagradient, false);
			dagradient.Angle = A_0.angle;
			dagradient.BlendMode = DAColorBlendMode.Multiply;
			dagradient.Gradient.colorKeys = A_0.gradientColorKeys.ToArray();
			dagradient.Gradient.alphaKeys = A_0.gradientAlphaKeys.ToArray();
		}

		// Token: 0x040019A2 RID: 6562
		[SerializeField]
		private UnityImageDrawer unityImageDrawer;

		// Token: 0x040019A3 RID: 6563
		[SerializeField]
		private SpriteRendererDrawer spriteRendererDrawer;
	}
}
