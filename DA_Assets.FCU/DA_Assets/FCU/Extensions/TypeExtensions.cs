using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x02000050 RID: 80
	public static class TypeExtensions
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x00006C98 File Offset: 0x00004E98
		public static bool IsAnyEffectInChildren(this FObject fobject)
		{
			if (fobject.Effects != null)
			{
				if (fobject.Effects.Any<Effect>((Effect effect) => effect.IsVisible()))
				{
					return true;
				}
			}
			if (fobject.Children != null)
			{
				using (List<FObject>.Enumerator enumerator = fobject.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsAnyEffectInChildren())
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00006D34 File Offset: 0x00004F34
		public static bool IsAnyImageOrVideoOrEmojiTypeInChildren(this FObject fobject)
		{
			if (fobject.IsAnyImageOrVideoOrEmojiType())
			{
				return true;
			}
			if (fobject.Children != null)
			{
				using (List<FObject>.Enumerator enumerator = fobject.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsAnyImageOrVideoOrEmojiTypeInChildren())
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00006DA4 File Offset: 0x00004FA4
		public static bool IsShadowType(this Effect effect)
		{
			return effect.Type.ToString().Contains("SHADOW");
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public static bool IsBlurType(this Effect effect)
		{
			return effect.Type.ToString().Contains("BLUR");
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006DFC File Offset: 0x00004FFC
		public static bool IsGradientType(this Paint paint)
		{
			return paint.Type.ToString().Contains("GRADIENT");
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00006E28 File Offset: 0x00005028
		public static bool IsAnyImageOrVideoOrEmojiType(this FObject fobject)
		{
			if (fobject.Fills.IsEmpty<Paint>())
			{
				return false;
			}
			return fobject.Fills.Any<Paint>((Paint fill) => fill.IsVisible() && (fill.Type == PaintType.IMAGE || fill.Type == PaintType.VIDEO || fill.Type == PaintType.EMOJI));
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00006E65 File Offset: 0x00005065
		public static bool IsSingleImageOrVideoOrEmojiType(this FObject fobject)
		{
			if (!fobject.IsAnyImageOrVideoOrEmojiType())
			{
				return false;
			}
			return fobject.Fills.Count<Paint>((Paint x) => x.IsVisible()) == 1;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00006E9F File Offset: 0x0000509F
		public static bool IsAnyMask(this FObject fobject)
		{
			return fobject.IsObjectMask() || fobject.IsClipMask() || fobject.IsFrameMask();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00006EB9 File Offset: 0x000050B9
		public static bool IsFrameMask(this FObject fobject)
		{
			return fobject.ContainsTag(FcuTag.Frame);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006EC2 File Offset: 0x000050C2
		public static bool IsClipMask(this FObject fobject)
		{
			return fobject.ClipsContent.ToBoolNullFalse();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00006ED0 File Offset: 0x000050D0
		public static bool IsObjectMask(this FObject fobject)
		{
			return fobject.IsMask.ToBoolNullFalse();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00006EDE File Offset: 0x000050DE
		public static bool IsGenerativeType(this FObject fobject)
		{
			return fobject.Data.FcuImageType == FcuImageType.Generative;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00006EEF File Offset: 0x000050EF
		public static bool IsDrawableType(this FObject fobject)
		{
			return fobject.Data.FcuImageType == FcuImageType.Drawable;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00006F00 File Offset: 0x00005100
		public static bool IsDownloadableType(this FObject fobject)
		{
			return fobject.Data.FcuImageType == FcuImageType.Downloadable;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00006F11 File Offset: 0x00005111
		public static bool IsMaskType(this FObject fobject)
		{
			return fobject.Data.FcuImageType == FcuImageType.Mask;
		}
	}
}
