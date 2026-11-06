using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200004A RID: 74
	public static class FObjectExtensionsAssembly
	{
		// Token: 0x060001AB RID: 427 RVA: 0x00004E68 File Offset: 0x00003068
		public static bool IsSupportedRenderSize(this Vector2 sourceSize, float imageScale, out Vector2Int spriteSize, out Vector2Int renderSize)
		{
			spriteSize = (sourceSize * imageScale).ToVector2Int();
			int maxRenderSize = FcuConfig.MaxRenderSize;
			int renderUpscaleFactor = FcuConfig.RenderUpscaleFactor;
			renderSize = spriteSize * renderUpscaleFactor;
			return renderSize.x <= maxRenderSize && renderSize.y <= maxRenderSize;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00004EBC File Offset: 0x000030BC
		public static bool IsSvgExtension(this FObject fobject)
		{
			if (fobject.Data.SpritePath.IsEmpty())
			{
				return false;
			}
			string text = Path.GetExtension(fobject.Data.SpritePath);
			if (text.StartsWith(".") && text.Length > 1)
			{
				text = text.Remove(0, 1);
			}
			return text.ToLower() == ImageFormat.SVG.ToLower();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00004F28 File Offset: 0x00003128
		public static void ExecuteWithTemporaryParent(this FObject fobject, Transform tempChildsParent, Func<FObject, GameObject> targetSelector, Action action)
		{
			GameObject gameObject = targetSelector(fobject);
			List<Transform> list = new List<Transform>();
			List<int> list2 = new List<int>();
			foreach (object obj in gameObject.transform)
			{
				Transform transform = (Transform)obj;
				list.Add(transform);
				list2.Add(transform.GetSiblingIndex());
			}
			foreach (Transform transform2 in list)
			{
				transform2.SetParent(tempChildsParent);
			}
			action();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetParent(gameObject.transform);
				list[i].SetSiblingIndex(list2[i]);
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005028 File Offset: 0x00003228
		public static bool IsSprite(this SyncData data)
		{
			return data.FcuImageType == FcuImageType.Downloadable || data.FcuImageType == FcuImageType.Generative;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000503E File Offset: 0x0000323E
		public static bool IsSprite(this FObject fobject)
		{
			return fobject.Data.FcuImageType == FcuImageType.Downloadable || fobject.Data.FcuImageType == FcuImageType.Generative;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005060 File Offset: 0x00003260
		public static bool IsCircle(this FObject fobject)
		{
			return fobject.Type == NodeType.ELLIPSE && fobject.Size.x.Round(FcuConfig.Rounding.IsCircleDigits) == fobject.Size.y.Round(FcuConfig.Rounding.IsCircleDigits);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000050B8 File Offset: 0x000032B8
		public static bool IsRectangle(this FObject fobject)
		{
			return fobject.Type == NodeType.RECTANGLE || (fobject.Type == NodeType.FRAME && (fobject.Type != NodeType.LINE || !fobject.IsSupportedLine()) && fobject.Children.IsEmpty<FObject>());
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005104 File Offset: 0x00003304
		public static void SetFlagToAllChilds(this FObject parent, Action<FObject> action)
		{
			if (parent.IsDefault<FObject>() || parent.Children.IsEmpty<FObject>())
			{
				return;
			}
			foreach (FObject fobject in parent.Children)
			{
				action(fobject);
				fobject.SetFlagToAllChilds(action);
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005178 File Offset: 0x00003378
		public static List<GradientAlphaKey> ToGradientAlphaKeys(this Paint gradient)
		{
			List<GradientAlphaKey> list = new List<GradientAlphaKey>();
			if (gradient.GradientStops.IsEmpty<GradientStop>())
			{
				return list;
			}
			foreach (GradientStop gradientStop in gradient.GradientStops)
			{
				list.Add(new GradientAlphaKey
				{
					alpha = gradientStop.Color.a,
					time = gradientStop.Position
				});
			}
			return list;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000520C File Offset: 0x0000340C
		public static List<GradientColorKey> ToGradientColorKeys(this Paint gradient)
		{
			List<GradientColorKey> list = new List<GradientColorKey>();
			if (gradient.GradientStops.IsEmpty<GradientStop>())
			{
				return list;
			}
			foreach (GradientStop gradientStop in gradient.GradientStops)
			{
				list.Add(new GradientColorKey
				{
					color = gradientStop.Color,
					time = gradientStop.Position
				});
			}
			return list;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000529C File Offset: 0x0000349C
		public static string GetText(this FObject fobject)
		{
			return fobject.Characters.Replace("\\r", " ").Replace("\\n", Environment.NewLine);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000052C3 File Offset: 0x000034C3
		public static bool IsSupportedLine(this FObject fobject)
		{
			return fobject.StrokeCap == StrokeCap.SQUARE || (fobject.StrokeCap == StrokeCap.ROUND && fobject.StrokeWeight >= 2f);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000052EC File Offset: 0x000034EC
		public static bool HasVisibleProperty<T>(this FObject fobject, Expression<Func<FObject, IEnumerable<T>>> propertySelector) where T : IVisible
		{
			IEnumerable<T> enumerable = propertySelector.Compile()(fobject);
			if (!enumerable.IsEmpty<T>())
			{
				return enumerable.Any<T>((T item) => item.Visible.ToBoolNullTrue());
			}
			return false;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005338 File Offset: 0x00003538
		public static bool TryGetLocalPosition(this FObject fobject, out Vector2 rtPos)
		{
			bool flag;
			try
			{
				rtPos = new Vector2(fobject.RelativeTransform[0][2].ToFloat(), -fobject.RelativeTransform[1][2].ToFloat());
				flag = true;
			}
			catch
			{
				rtPos = new Vector2(0f, 0f);
				flag = false;
			}
			return flag;
		}
	}
}
