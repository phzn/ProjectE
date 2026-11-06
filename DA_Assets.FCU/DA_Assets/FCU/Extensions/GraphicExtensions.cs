using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DA_Assets.Extensions;
using DA_Assets.FCU.Attributes;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200004C RID: 76
	public static class GraphicExtensions
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00005410 File Offset: 0x00003610
		public static bool GetBoundingSize(this FObject fobject, out Vector2 size)
		{
			size = default(Vector2);
			float? width = fobject.AbsoluteBoundingBox.Width;
			float? height = fobject.AbsoluteBoundingBox.Height;
			if (width == null || height == null)
			{
				return false;
			}
			float num = (float)Math.Round((double)width.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			float num2 = (float)Math.Round((double)height.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			size = new Vector2(num, num2);
			return true;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000549C File Offset: 0x0000369C
		public static bool GetBoundingPosition(this FObject fobject, out Vector2 position)
		{
			position = default(Vector2);
			float? x = fobject.AbsoluteBoundingBox.X;
			float? y = fobject.AbsoluteBoundingBox.Y;
			if (x == null || y == null)
			{
				return false;
			}
			float num = (float)Math.Round((double)x.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			float num2 = (float)Math.Round((double)y.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			position = new Vector2(num, num2);
			return true;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005528 File Offset: 0x00003728
		public static bool GetRenderSize(this FObject fobject, out Vector2 size)
		{
			size = default(Vector2);
			float? width = fobject.AbsoluteRenderBounds.Width;
			float? height = fobject.AbsoluteRenderBounds.Height;
			if (width == null || height == null)
			{
				return false;
			}
			float num = (float)Math.Round((double)width.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			float num2 = (float)Math.Round((double)height.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			size = new Vector2(num, num2);
			return true;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000055B4 File Offset: 0x000037B4
		public static bool GetRenderPosition(this FObject fobject, out Vector2 position)
		{
			position = default(Vector2);
			float? x = fobject.AbsoluteRenderBounds.X;
			float? y = fobject.AbsoluteRenderBounds.Y;
			if (x == null || y == null)
			{
				return false;
			}
			float num = (float)Math.Round((double)x.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			float num2 = (float)Math.Round((double)y.Value, FcuConfig.Rounding.GetBoundingSizeDigits);
			position = new Vector2(num, num2);
			return true;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005640 File Offset: 0x00003840
		public static bool IsZeroSize(this FObject fobject)
		{
			float? num = fobject.AbsoluteBoundingBox.Width;
			float num2 = 0f;
			if (!((num.GetValueOrDefault() <= num2) & (num != null)))
			{
				num = fobject.AbsoluteBoundingBox.Height;
				num2 = 0f;
				if (!((num.GetValueOrDefault() <= num2) & (num != null)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000056AA File Offset: 0x000038AA
		public static bool IsVisible(this FObject fobject)
		{
			return fobject.Visible.ToBoolNullTrue();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000056B8 File Offset: 0x000038B8
		public static bool IsVisible(this Paint paint)
		{
			return paint.Visible.ToBoolNullTrue();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000056C6 File Offset: 0x000038C6
		public static bool IsVisible(this Effect effect)
		{
			return effect.Visible.ToBoolNullTrue();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000056D4 File Offset: 0x000038D4
		public static bool IsSingleLinearGradient(this FObject fobject, out Paint paint)
		{
			paint = default(Paint);
			bool flag = false;
			if (fobject.Fills.IsEmpty<Paint>())
			{
				flag = false;
			}
			else
			{
				IEnumerable<Paint> enumerable = fobject.Fills.Where<Paint>((Paint x) => x.IsVisible());
				IEnumerable<Paint> enumerable2 = fobject.Fills.Where<Paint>((Paint x) => x.IsVisible() && x.Type == PaintType.GRADIENT_LINEAR);
				if (!fobject.Strokes.IsEmpty<Paint>())
				{
					flag = false;
				}
				else if (!fobject.Effects.IsEmpty<Effect>())
				{
					flag = false;
				}
				else if (enumerable.Count<Paint>() > 1)
				{
					flag = false;
				}
				else if (enumerable2.Count<Paint>() == 1)
				{
					paint = enumerable2.First<Paint>();
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000057A0 File Offset: 0x000039A0
		public static bool ContainsImageEmojiVideo(this FObject fobject)
		{
			if (fobject.Fills.IsEmpty<Paint>())
			{
				return false;
			}
			foreach (Paint paint in fobject.Fills)
			{
				if (!paint.IsVisible())
				{
					return false;
				}
				if (!paint.ImageRef.IsEmpty() || !paint.GifRef.IsEmpty())
				{
					return true;
				}
				PaintType type = paint.Type;
				if (type - PaintType.IMAGE <= 2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00005840 File Offset: 0x00003A40
		public static bool IsSingleColor(this FObject fobject, out Color color)
		{
			Dictionary<Color, float?> dictionary = new Dictionary<Color, float?>();
			List<bool> list = new List<bool>();
			GraphicExtensions.IsSingleColorRecursive(fobject, list, dictionary);
			if (list.Count > 0)
			{
				color = default(Color);
				return false;
			}
			if (dictionary.Count == 1)
			{
				color = dictionary.First<KeyValuePair<Color, float?>>().Key;
				return true;
			}
			color = default(Color);
			return false;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000589C File Offset: 0x00003A9C
		private static void IsSingleColorRecursive(FObject fobject, List<bool> flags, Dictionary<Color, float?> values)
		{
			if (!fobject.Fills.IsEmpty<Paint>())
			{
				foreach (Paint paint in fobject.Fills)
				{
					if (paint.IsVisible())
					{
						if (!paint.ImageRef.IsEmpty() || !paint.GifRef.IsEmpty())
						{
							flags.Add(true);
							return;
						}
						if (!paint.Type.ToString().Contains("SOLID"))
						{
							flags.Add(true);
							return;
						}
						values.TryAddValue(paint.Color, paint.Opacity);
					}
				}
			}
			if (!fobject.Strokes.IsEmpty<Paint>())
			{
				foreach (Paint paint2 in fobject.Strokes)
				{
					if (paint2.IsVisible())
					{
						if (!paint2.ImageRef.IsEmpty() || !paint2.GifRef.IsEmpty())
						{
							flags.Add(true);
							return;
						}
						if (!paint2.Type.ToString().Contains("SOLID"))
						{
							flags.Add(true);
							return;
						}
						values.TryAddValue(paint2.Color, paint2.Opacity);
					}
				}
			}
			if (!fobject.Effects.IsEmpty<Effect>())
			{
				foreach (Effect effect in fobject.Effects)
				{
					if (effect.IsVisible())
					{
						if (!effect.Type.ToString().Contains("SOLID"))
						{
							flags.Add(true);
							return;
						}
						values.TryAddValue(effect.Color, effect.Opacity);
					}
				}
			}
			if (fobject.Children.IsEmpty<FObject>())
			{
				return;
			}
			foreach (FObject fobject2 in fobject.Children)
			{
				if (fobject2.Type != NodeType.TEXT)
				{
					GraphicExtensions.IsSingleColorRecursive(fobject2, flags, values);
				}
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00005B2C File Offset: 0x00003D2C
		public static bool ContainsRoundedCorners(this FObject fobject)
		{
			float? cornerRadius = fobject.CornerRadius;
			float num = 0f;
			if (!((cornerRadius.GetValueOrDefault() > num) & (cornerRadius != null)))
			{
				List<float> cornerRadiuses = fobject.CornerRadiuses;
				bool? flag;
				if (cornerRadiuses == null)
				{
					flag = null;
				}
				else
				{
					flag = new bool?(cornerRadiuses.Any<float>((float radius) => radius > 0f));
				}
				return flag.ToBoolNullFalse();
			}
			return true;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005BA4 File Offset: 0x00003DA4
		public static bool IsArcDataFilled(this FObject fobject)
		{
			return !fobject.ArcData.Equals(default(ArcData)) && fobject.ArcData.EndingAngle < 6.28f;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00005BF0 File Offset: 0x00003DF0
		public static Paint GetFirstSolidPaint(this List<Paint> paints)
		{
			if (paints == null)
			{
				return default(Paint);
			}
			Paint paint = default(Paint);
			foreach (Paint paint2 in paints)
			{
				if (paint2.IsVisible() && paint2.Type == PaintType.SOLID)
				{
					paint = paint2;
					paint.Color = paint.Color.SetAlpha(paint2.Opacity);
					break;
				}
			}
			return paint;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00005C80 File Offset: 0x00003E80
		public static int GetPriority(this PaintType paintType)
		{
			MemberInfo[] member = paintType.GetType().GetMember(paintType.ToString());
			if (member.Length != 0)
			{
				PaintPriorityAttribute customAttribute = member[0].GetCustomAttribute<PaintPriorityAttribute>();
				if (customAttribute != null)
				{
					return customAttribute.Priority;
				}
			}
			return 0;
		}
	}
}
