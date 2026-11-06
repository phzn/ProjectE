using System;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000032 RID: 50
	public static class RectTransformExtensions
	{
		// Token: 0x060000DE RID: 222 RVA: 0x000053F0 File Offset: 0x000035F0
		public static void CopyFrom(this RectTransform target, RectTransform source)
		{
			target.anchorMin = source.anchorMin;
			target.anchorMax = source.anchorMax;
			target.anchoredPosition = source.anchoredPosition;
			target.sizeDelta = source.sizeDelta;
			target.localRotation = source.localRotation;
			target.localScale = source.localScale;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005445 File Offset: 0x00003645
		public static float GetWidth(this RectTransform rt)
		{
			return (rt.anchorMax.x - rt.anchorMin.x) * (float)Screen.width + rt.sizeDelta.x;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005471 File Offset: 0x00003671
		public static float GetHeight(this RectTransform rt)
		{
			return (rt.anchorMax.y - rt.anchorMin.y) * (float)Screen.height + rt.sizeDelta.y;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000549D File Offset: 0x0000369D
		public static void SetLeft(this RectTransform rt, float left)
		{
			rt.offsetMin = new Vector2(left, rt.offsetMin.y);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000054B6 File Offset: 0x000036B6
		public static float GetLeft(this RectTransform rt)
		{
			return rt.offsetMin.x;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000054C3 File Offset: 0x000036C3
		public static void SetRight(this RectTransform rt, float right)
		{
			rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000054DD File Offset: 0x000036DD
		public static float GetRight(this RectTransform rt)
		{
			return -rt.offsetMax.x;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000054EB File Offset: 0x000036EB
		public static void SetTop(this RectTransform rt, float top)
		{
			rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005505 File Offset: 0x00003705
		public static float GetTop(this RectTransform rt)
		{
			return -rt.offsetMax.y;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005513 File Offset: 0x00003713
		public static void SetBottom(this RectTransform rt, float bottom)
		{
			rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000552C File Offset: 0x0000372C
		public static float GetBottom(this RectTransform rt)
		{
			return rt.offsetMin.y;
		}
	}
}
