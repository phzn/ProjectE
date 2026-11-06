using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200031C RID: 796
	[Serializable]
	public class GraphicHelpers : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015B0 RID: 5552 RVA: 0x0008C850 File Offset: 0x0008AA50
		internal FGraphic GetGraphic(FObject fobject)
		{
			FGraphic fgraphic = new FGraphic();
			Paint firstSolidPaint = fobject.Fills.GetFirstSolidPaint();
			Paint firstPaint = this.GetFirstPaint(fobject.Fills);
			Color color = ((!firstSolidPaint.IsDefault<Paint>()) ? firstSolidPaint.Color : firstPaint.ToGradientColorKeys().FirstOrDefault<GradientColorKey>().color);
			fgraphic.Fill = new FFill
			{
				HasSolid = !firstSolidPaint.IsDefault<Paint>(),
				HasGradient = !firstPaint.IsDefault<Paint>(),
				SolidPaint = firstSolidPaint,
				GradientPaint = firstPaint,
				SingleColor = color
			};
			bool flag = fobject.StrokeWeight > 0f;
			if (flag)
			{
				Paint firstSolidPaint2 = fobject.Strokes.GetFirstSolidPaint();
				Paint firstPaint2 = this.GetFirstPaint(fobject.Strokes);
				Color color2 = ((!firstSolidPaint2.IsDefault<Paint>()) ? firstSolidPaint2.Color : firstPaint2.ToGradientColorKeys().FirstOrDefault<GradientColorKey>().color);
				fgraphic.Stroke = new FStroke
				{
					Weight = fobject.StrokeWeight,
					HasSolid = !firstSolidPaint2.IsDefault<Paint>(),
					HasGradient = !firstPaint2.IsDefault<Paint>(),
					SolidPaint = firstSolidPaint2,
					GradientPaint = firstPaint2,
					SingleColor = color2
				};
			}
			if (fgraphic.Fill.SingleColor.a == 1f)
			{
				fgraphic.FillAlpha1 = true;
			}
			Color color3;
			fgraphic.HasSingleColor = fobject.IsSingleColor(out color3);
			fgraphic.SpriteSingleColor = color3;
			Paint paint;
			fgraphic.HasSingleGradient = fobject.IsSingleLinearGradient(out paint);
			fgraphic.SpriteSingleLinearGradient = paint;
			fgraphic.HasFill = !fgraphic.Fill.SolidPaint.IsDefault<Paint>() || !fgraphic.Fill.GradientPaint.IsDefault<Paint>();
			fgraphic.HasStroke = flag && (!fgraphic.Stroke.SolidPaint.IsDefault<Paint>() || !fgraphic.Stroke.GradientPaint.IsDefault<Paint>());
			FStroke stroke = fgraphic.Stroke;
			stroke.Align = this.GetStrokeAlign(fobject, fgraphic);
			fgraphic.Stroke = stroke;
			return fgraphic;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0008CA80 File Offset: 0x0008AC80
		private StrokeAlign GetStrokeAlign(FObject fobject, FGraphic graphic)
		{
			if (this.monoBeh.IsNova())
			{
				return fobject.StrokeAlign;
			}
			if (this.monoBeh.IsUITK())
			{
				return StrokeAlign.INSIDE;
			}
			if (this.monoBeh.UsingShapes2D())
			{
				if (!graphic.FillAlpha1)
				{
					return StrokeAlign.INSIDE;
				}
				if (graphic.Fill.HasSolid)
				{
					if (fobject.StrokeAlign == StrokeAlign.OUTSIDE)
					{
						return StrokeAlign.OUTSIDE;
					}
					return StrokeAlign.INSIDE;
				}
				else
				{
					if (graphic.Fill.HasGradient)
					{
						return StrokeAlign.OUTSIDE;
					}
					return StrokeAlign.NONE;
				}
			}
			else if (this.monoBeh.UsingJoshPui())
			{
				if (graphic.FillAlpha1)
				{
					return StrokeAlign.OUTSIDE;
				}
				return StrokeAlign.NONE;
			}
			else if (this.monoBeh.UsingDttPui())
			{
				if (graphic.HasStroke && graphic.Fill.GradientPaint.IsDefault<Paint>())
				{
					return StrokeAlign.OUTSIDE;
				}
				return StrokeAlign.NONE;
			}
			else
			{
				if (!this.monoBeh.UsingMPUIKit())
				{
					this.monoBeh.UsingSpriteRenderer();
					return StrokeAlign.OUTSIDE;
				}
				if (!graphic.FillAlpha1)
				{
					return StrokeAlign.INSIDE;
				}
				if (graphic.Fill.HasSolid)
				{
					if (fobject.StrokeAlign == StrokeAlign.OUTSIDE)
					{
						return StrokeAlign.OUTSIDE;
					}
					return StrokeAlign.INSIDE;
				}
				else
				{
					if (graphic.Fill.HasGradient)
					{
						return StrokeAlign.OUTSIDE;
					}
					return StrokeAlign.NONE;
				}
			}
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0008CB98 File Offset: 0x0008AD98
		public Color32 GetRectTransformColor(FObject fobject)
		{
			FGraphic graphic = fobject.Data.Graphic;
			Color32? color = null;
			if (graphic.HasFill)
			{
				color = new Color32?(graphic.Fill.SingleColor);
			}
			else if (graphic.HasStroke)
			{
				color = new Color32?(graphic.Stroke.SingleColor);
			}
			if (color == null)
			{
				color = new Color32?(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
			}
			else
			{
				Color32 value = color.Value;
				value.a = 128;
				color = new Color32?(value);
			}
			return color.Value;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0008CC4C File Offset: 0x0008AE4C
		public Vector4 GetCornerRadius(FObject fobject)
		{
			if (fobject.IsCircle())
			{
				return new Vector4
				{
					x = 9999f,
					y = 9999f,
					z = 9999f,
					w = 9999f
				};
			}
			if (fobject.CornerRadiuses.IsEmpty<float>())
			{
				return new Vector4
				{
					x = fobject.CornerRadius.ToFloat(),
					y = fobject.CornerRadius.ToFloat(),
					z = fobject.CornerRadius.ToFloat(),
					w = fobject.CornerRadius.ToFloat()
				};
			}
			if (this.monoBeh.IsUITK())
			{
				return new Vector4
				{
					x = fobject.CornerRadiuses[0],
					y = fobject.CornerRadiuses[3],
					z = fobject.CornerRadiuses[2],
					w = fobject.CornerRadiuses[1]
				};
			}
			if (this.monoBeh.IsNova())
			{
				return new Vector4
				{
					x = fobject.CornerRadiuses[0],
					y = fobject.CornerRadiuses[3],
					z = fobject.CornerRadiuses[2],
					w = fobject.CornerRadiuses[1]
				};
			}
			switch (this.monoBeh.Settings.ImageSpritesSettings.ImageComponent)
			{
			case ImageComponent.UnityImage:
				return new Vector4
				{
					x = fobject.CornerRadiuses[0],
					y = fobject.CornerRadiuses[1],
					z = fobject.CornerRadiuses[2],
					w = fobject.CornerRadiuses[3]
				};
			case ImageComponent.SubcShape:
				return new Vector4
				{
					x = fobject.CornerRadiuses[3],
					y = fobject.CornerRadiuses[2],
					z = fobject.CornerRadiuses[1],
					w = fobject.CornerRadiuses[0]
				};
			case ImageComponent.MPImage:
				return new Vector4
				{
					x = fobject.CornerRadiuses[3],
					y = fobject.CornerRadiuses[2],
					z = fobject.CornerRadiuses[1],
					w = fobject.CornerRadiuses[0]
				};
			case ImageComponent.ProceduralImage:
				return new Vector4
				{
					x = fobject.CornerRadiuses[0],
					y = fobject.CornerRadiuses[1],
					z = fobject.CornerRadiuses[2],
					w = fobject.CornerRadiuses[3]
				};
			case ImageComponent.SpriteRenderer:
				return new Vector4
				{
					x = fobject.CornerRadiuses[0],
					y = fobject.CornerRadiuses[0],
					z = fobject.CornerRadiuses[0],
					w = fobject.CornerRadiuses[0]
				};
			case ImageComponent.RoundedImage:
				return new Vector4
				{
					x = fobject.CornerRadiuses[0],
					y = fobject.CornerRadiuses[1],
					z = fobject.CornerRadiuses[3],
					w = fobject.CornerRadiuses[2]
				};
			}
			return new Vector4
			{
				x = fobject.CornerRadiuses[0],
				y = fobject.CornerRadiuses[1],
				z = fobject.CornerRadiuses[2],
				w = fobject.CornerRadiuses[3]
			};
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0008D080 File Offset: 0x0008B280
		public Paint GetFirstPaint(List<Paint> paints)
		{
			if (paints.IsEmpty<Paint>())
			{
				return default(Paint);
			}
			IEnumerable<Paint> enumerable = paints.Where<Paint>((Paint x) => x.Type.Contains("GRADIENT"));
			if (enumerable.IsEmpty<Paint>())
			{
				return default(Paint);
			}
			return enumerable.OrderBy<Paint, int>((Paint p) => p.Type.GetPriority()).First<Paint>();
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0008D104 File Offset: 0x0008B304
		private bool ContainsMultipleFills(FObject fobject, out string reason)
		{
			reason = null;
			FGraphic graphic = fobject.Data.Graphic;
			if (fobject.Fills.IsEmpty<Paint>() || fobject.Strokes.IsEmpty<Paint>())
			{
				reason = "no fills or strokes";
				return false;
			}
			bool flag = fobject.Fills.Count<Paint>((Paint x) => x.IsVisible()) > 1;
			bool flag2 = fobject.Strokes.Count<Paint>((Paint x) => x.IsVisible()) > 1;
			bool flag3 = graphic.HasFill && graphic.HasStroke;
			if (flag)
			{
				reason = "fillsMoreThanOne";
				return true;
			}
			if (flag2)
			{
				reason = "strokesMoreThanOne";
				return true;
			}
			if (flag3)
			{
				reason = "fillsAndStroke";
				return true;
			}
			return false;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0008D1DC File Offset: 0x0008B3DC
		public bool? IsDownloadableByFills(FObject fobject, out string reason)
		{
			reason = null;
			FGraphic graphic = fobject.Data.Graphic;
			if (fobject.ContainsImageEmojiVideo())
			{
				reason = "ContainsImageEmojiVideo";
				return new bool?(true);
			}
			if (this.monoBeh.IsUGUI())
			{
				if (graphic.HasStroke && graphic.HasFill && !graphic.FillAlpha1)
				{
					reason = "Fill with transparency + Stroke are not supported";
					return new bool?(true);
				}
				if ((fobject.Size.x < 48f || fobject.Size.y < 48f) && graphic.HasStroke && fobject.Data.Graphic.Stroke.Align == StrokeAlign.OUTSIDE)
				{
					reason = "sizeLowerThan48 + outside stroke";
					return new bool?(true);
				}
			}
			bool flag = this.monoBeh.UsingAnyProceduralImage() || this.monoBeh.IsUITK() || this.monoBeh.IsNova();
			string text;
			if ((this.monoBeh.Settings.ImageSpritesSettings.DownloadOptions.HasFlag(SpriteDownloadOptions.MultipleFills) || !flag) && this.ContainsMultipleFills(fobject, out text))
			{
				reason = text;
				return new bool?(true);
			}
			string text2;
			if ((this.monoBeh.Settings.ImageSpritesSettings.DownloadOptions.HasFlag(SpriteDownloadOptions.UnsupportedGradients) || !flag) && this.ContainsUnsupportedGradients(fobject, out text2))
			{
				reason = text2;
				return new bool?(true);
			}
			string text3;
			if ((this.monoBeh.Settings.ImageSpritesSettings.DownloadOptions.HasFlag(SpriteDownloadOptions.SupportedGradients) || !flag) && this.ContainsSupportedGradients(fobject, out text3))
			{
				reason = text3;
				return new bool?(true);
			}
			if (this.monoBeh.IsUGUI())
			{
				if (this.monoBeh.UsingUnityImage() || this.monoBeh.UsingRawImage() || this.monoBeh.UsingSpriteRenderer() || this.monoBeh.UsingSVG())
				{
					if (!fobject.IsRectangle())
					{
						reason = "not rectangle";
						return new bool?(true);
					}
					if (fobject.ContainsRoundedCorners())
					{
						reason = "ContainsRoundedCorners";
						return new bool?(true);
					}
					if (fobject.HasVisibleProperty<Paint>((FObject x) => x.Strokes))
					{
						reason = "has strokes";
						return new bool?(true);
					}
				}
				else if (this.monoBeh.UsingDttPui() && graphic.HasStroke && graphic.Fill.HasGradient)
				{
					reason = "gradient_fill + stroke is unsupported";
					return new bool?(true);
				}
			}
			return null;
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0008D48C File Offset: 0x0008B68C
		private bool ContainsSupportedGradients(FObject fobject, out string reason)
		{
			reason = null;
			FGraphic graphic = fobject.Data.Graphic;
			if (this.monoBeh.IsNova() || this.monoBeh.IsUITK())
			{
				return false;
			}
			switch (this.monoBeh.Settings.ImageSpritesSettings.ImageComponent)
			{
			case ImageComponent.UnityImage:
			case ImageComponent.RawImage:
			case ImageComponent.SpriteRenderer:
				return false;
			case ImageComponent.SubcShape:
			case ImageComponent.MPImage:
			case ImageComponent.ProceduralImage:
			case ImageComponent.RoundedImage:
				if ((graphic.Fill.HasGradient || graphic.Stroke.HasGradient) && graphic.Fill.GradientPaint.Type == PaintType.GRADIENT_LINEAR)
				{
					reason = PaintType.GRADIENT_LINEAR.ToString();
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0008D550 File Offset: 0x0008B750
		private bool ContainsUnsupportedGradients(FObject fobject, out string reason1)
		{
			reason1 = null;
			FGraphic graphic = fobject.Data.Graphic;
			if ((this.monoBeh.IsNova() || this.monoBeh.IsUITK()) && (graphic.Fill.HasGradient || graphic.Stroke.HasGradient))
			{
				reason1 = "has gradients";
				return true;
			}
			switch (this.monoBeh.Settings.ImageSpritesSettings.ImageComponent)
			{
			case ImageComponent.UnityImage:
			case ImageComponent.RawImage:
			case ImageComponent.SpriteRenderer:
				if (graphic.Fill.HasGradient || graphic.Stroke.HasGradient)
				{
					reason1 = "has gradients";
					return true;
				}
				break;
			case ImageComponent.SubcShape:
			case ImageComponent.MPImage:
			{
				PaintType paintType = graphic.Fill.GradientPaint.Type;
				if (paintType - PaintType.GRADIENT_ANGULAR <= 1)
				{
					reason1 = graphic.Fill.GradientPaint.Type.ToString();
					return true;
				}
				break;
			}
			case ImageComponent.ProceduralImage:
			{
				PaintType paintType = graphic.Fill.GradientPaint.Type;
				if (paintType - PaintType.GRADIENT_RADIAL <= 2)
				{
					reason1 = graphic.Fill.GradientPaint.Type.ToString();
					return true;
				}
				break;
			}
			case ImageComponent.RoundedImage:
				if (graphic.Fill.GradientPaint.Type == PaintType.GRADIENT_DIAMOND)
				{
					reason1 = graphic.Fill.GradientPaint.Type.ToString();
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0008D6F5 File Offset: 0x0008B8F5
		public float ToRadialAngle(FObject fobject, List<Vector2> gradientHandlePositions)
		{
			return 0f;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0008D6FC File Offset: 0x0008B8FC
		public float ToLinearAngle(FObject fobject, List<Vector2> gradientHandlePositions)
		{
			if (gradientHandlePositions.IsEmpty<Vector2>() || gradientHandlePositions.Count < 3)
			{
				DALogger.LogError("Can't calculate angle.");
				return 0f;
			}
			if (!fobject.CanUseUnityImage(this.monoBeh) && (this.monoBeh.UsingMPUIKit() || this.monoBeh.UsingDttPui()))
			{
				Vector2 vector = new Vector2(gradientHandlePositions[0].x, gradientHandlePositions[0].y * -1f);
				Vector2 vector2 = new Vector2(gradientHandlePositions[2].x, gradientHandlePositions[2].y * -1f) - vector;
				return ((1.5707964f - Mathf.Atan2(-vector2.y, vector2.x)) * 57.29578f + 360f) % 360f;
			}
			return (Mathf.Atan2(gradientHandlePositions[2].y - gradientHandlePositions[0].y, gradientHandlePositions[2].x - gradientHandlePositions[0].x) * 57.29578f + 360f) % 360f;
		}
	}
}
