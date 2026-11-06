using System;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000023 RID: 35
	public static class AdaptivityExtensions
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000034F0 File Offset: 0x000016F0
		private static void SetSmartPivot(RectTransform rect, float value, int axis, bool smart, bool parentSpace)
		{
			Vector3 rectReferenceCorner = AdaptivityExtensions.GetRectReferenceCorner(rect, !parentSpace);
			Vector2 pivot = rect.pivot;
			pivot[axis] = value;
			rect.pivot = pivot;
			if (smart)
			{
				Vector3 vector = AdaptivityExtensions.GetRectReferenceCorner(rect, !parentSpace) - rectReferenceCorner;
				rect.anchoredPosition -= vector;
				Vector3 position = rect.transform.position;
				position.z -= vector.z;
				rect.transform.position = position;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003578 File Offset: 0x00001778
		private static Vector3 GetRectReferenceCorner(RectTransform gui, bool worldSpace)
		{
			Vector3[] array = new Vector3[4];
			if (!worldSpace)
			{
				return gui.rect.min + gui.transform.localPosition;
			}
			Transform transform = gui.transform;
			gui.GetWorldCorners(array);
			if (transform.parent)
			{
				return transform.parent.InverseTransformPoint(array[0]);
			}
			return array[0];
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000035E8 File Offset: 0x000017E8
		private static AdaptivityExtensions.AnchorMinMax GetAnchor(AnchorType anchorPreset)
		{
			AdaptivityExtensions.AnchorMinMax anchorMinMax = default(AdaptivityExtensions.AnchorMinMax);
			switch (anchorPreset)
			{
			case AnchorType.TopLeft:
				anchorMinMax.Min = new Vector2(0f, 1f);
				anchorMinMax.Max = new Vector2(0f, 1f);
				break;
			case AnchorType.TopCenter:
				anchorMinMax.Min = new Vector2(0.5f, 1f);
				anchorMinMax.Max = new Vector2(0.5f, 1f);
				break;
			case AnchorType.TopRight:
				anchorMinMax.Min = new Vector2(1f, 1f);
				anchorMinMax.Max = new Vector2(1f, 1f);
				break;
			case AnchorType.MiddleLeft:
				anchorMinMax.Min = new Vector2(0f, 0.5f);
				anchorMinMax.Max = new Vector2(0f, 0.5f);
				break;
			case AnchorType.MiddleCenter:
				anchorMinMax.Min = new Vector2(0.5f, 0.5f);
				anchorMinMax.Max = new Vector2(0.5f, 0.5f);
				break;
			case AnchorType.MiddleRight:
				anchorMinMax.Min = new Vector2(1f, 0.5f);
				anchorMinMax.Max = new Vector2(1f, 0.5f);
				break;
			case AnchorType.BottomLeft:
				anchorMinMax.Min = new Vector2(0f, 0f);
				anchorMinMax.Max = new Vector2(0f, 0f);
				break;
			case AnchorType.BottomCenter:
				anchorMinMax.Min = new Vector2(0.5f, 0f);
				anchorMinMax.Max = new Vector2(0.5f, 0f);
				break;
			case AnchorType.BottomRight:
				anchorMinMax.Min = new Vector2(1f, 0f);
				anchorMinMax.Max = new Vector2(1f, 0f);
				break;
			case AnchorType.VertStretchLeft:
				anchorMinMax.Min = new Vector2(0f, 0f);
				anchorMinMax.Max = new Vector2(0f, 1f);
				break;
			case AnchorType.VertStretchRight:
				anchorMinMax.Min = new Vector2(1f, 0f);
				anchorMinMax.Max = new Vector2(1f, 1f);
				break;
			case AnchorType.VertStretchCenter:
				anchorMinMax.Min = new Vector2(0.5f, 0f);
				anchorMinMax.Max = new Vector2(0.5f, 1f);
				break;
			case AnchorType.HorStretchTop:
				anchorMinMax.Min = new Vector2(0f, 1f);
				anchorMinMax.Max = new Vector2(1f, 1f);
				break;
			case AnchorType.HorStretchMiddle:
				anchorMinMax.Min = new Vector2(0f, 0.5f);
				anchorMinMax.Max = new Vector2(1f, 0.5f);
				break;
			case AnchorType.HorStretchBottom:
				anchorMinMax.Min = new Vector2(0f, 0f);
				anchorMinMax.Max = new Vector2(1f, 0f);
				break;
			case AnchorType.StretchAll:
				anchorMinMax.Min = new Vector2(0f, 0f);
				anchorMinMax.Max = new Vector2(1f, 1f);
				break;
			}
			return anchorMinMax;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003954 File Offset: 0x00001B54
		private static Vector2 GetPivot(PivotType preset)
		{
			switch (preset)
			{
			case PivotType.TopLeft:
				return new Vector2(0f, 1f);
			case PivotType.TopCenter:
				return new Vector2(0.5f, 1f);
			case PivotType.TopRight:
				return new Vector2(1f, 1f);
			case PivotType.MiddleLeft:
				return new Vector2(0f, 0.5f);
			case PivotType.MiddleCenter:
				return new Vector2(0.5f, 0.5f);
			case PivotType.MiddleRight:
				return new Vector2(1f, 0.5f);
			case PivotType.BottomLeft:
				return new Vector2(0f, 0f);
			case PivotType.BottomCenter:
				return new Vector2(0.5f, 0f);
			case PivotType.BottomRight:
				return new Vector2(1f, 0f);
			default:
				return AdaptivityExtensions.GetPivot(PivotType.MiddleCenter);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003A28 File Offset: 0x00001C28
		private static void SetSmartAnchor(this RectTransform rect, AdaptivityExtensions.RectAxis rectAxis, float value, bool isMax)
		{
			bool flag = true;
			RectTransform rectTransform = null;
			if (rect.transform.parent == null)
			{
				flag = false;
			}
			else
			{
				rectTransform = rect.transform.parent.GetComponent<RectTransform>();
				if (rectTransform == null)
				{
					flag = false;
				}
			}
			float num = 0f;
			float num2 = 0f;
			if (flag)
			{
				float num3 = (isMax ? rect.anchorMax[(int)rectAxis] : rect.anchorMin[(int)rectAxis]);
				num = (value - num3) * rectTransform.rect.size[(int)rectAxis];
				float num4 = 0f;
				Canvas componentInParent = rect.gameObject.GetComponentInParent<Canvas>();
				if (componentInParent != null && componentInParent.renderMode != RenderMode.WorldSpace)
				{
					num4 = Mathf.Round(num) - num;
				}
				num += num4;
				num2 = (isMax ? (num * rect.pivot[(int)rectAxis]) : (num * (1f - rect.pivot[(int)rectAxis])));
			}
			if (isMax)
			{
				Vector2 anchorMax = rect.anchorMax;
				anchorMax[(int)rectAxis] = value;
				rect.anchorMax = anchorMax;
				Vector2 anchorMin = rect.anchorMin;
				rect.anchorMin = anchorMin;
			}
			else
			{
				Vector2 anchorMin2 = rect.anchorMin;
				anchorMin2[(int)rectAxis] = value;
				rect.anchorMin = anchorMin2;
				Vector2 anchorMax2 = rect.anchorMax;
				rect.anchorMax = anchorMax2;
			}
			if (flag)
			{
				Vector2 anchoredPosition = rect.anchoredPosition;
				ref Vector2 ptr = ref anchoredPosition;
				ptr[(int)rectAxis] = ptr[(int)rectAxis] - num2;
				rect.anchoredPosition = anchoredPosition;
				Vector2 sizeDelta = rect.sizeDelta;
				ptr = ref sizeDelta;
				ptr[(int)rectAxis] = ptr[(int)rectAxis] + num * (float)(isMax ? (-1) : 1);
				rect.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003BF4 File Offset: 0x00001DF4
		public static void SetSmartAnchor(this RectTransform rect, AnchorType anchorType)
		{
			AdaptivityExtensions.AnchorMinMax anchor = AdaptivityExtensions.GetAnchor(anchorType);
			rect.SetSmartAnchor(AdaptivityExtensions.RectAxis.Hor, anchor.Min.x, false);
			rect.SetSmartAnchor(AdaptivityExtensions.RectAxis.Hor, anchor.Max.x, true);
			rect.SetSmartAnchor(AdaptivityExtensions.RectAxis.Vert, anchor.Min.y, false);
			rect.SetSmartAnchor(AdaptivityExtensions.RectAxis.Vert, anchor.Max.y, true);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003C54 File Offset: 0x00001E54
		public static void SetSmartPivot(this RectTransform rect, PivotType pivotType)
		{
			Vector2 pivot = AdaptivityExtensions.GetPivot(pivotType);
			AdaptivityExtensions.SetSmartPivot(rect, pivot.x, 0, true, false);
			AdaptivityExtensions.SetSmartPivot(rect, pivot.y, 1, true, false);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003C86 File Offset: 0x00001E86
		public static void SetRotation(this Transform source, float value)
		{
			source.localRotation = Quaternion.Euler(source.localRotation.x, source.localRotation.y, value);
		}

		// Token: 0x02000056 RID: 86
		private enum RectAxis
		{
			// Token: 0x040001F6 RID: 502
			Hor,
			// Token: 0x040001F7 RID: 503
			Vert
		}

		// Token: 0x02000057 RID: 87
		private struct AnchorMinMax
		{
			// Token: 0x040001F8 RID: 504
			public Vector2 Min;

			// Token: 0x040001F9 RID: 505
			public Vector2 Max;
		}
	}
}
