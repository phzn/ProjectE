using System;
using UnityEngine;

namespace DA_Assets.CR
{
	// Token: 0x02000004 RID: 4
	internal static class Extensions
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002506 File Offset: 0x00000706
		public static float GetWidth(this RectTransform rt)
		{
			return (rt.anchorMax.x - rt.anchorMin.x) * (float)Screen.width + rt.sizeDelta.x;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002532 File Offset: 0x00000732
		public static float GetHeight(this RectTransform rt)
		{
			return (rt.anchorMax.y - rt.anchorMin.y) * (float)Screen.height + rt.sizeDelta.y;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002560 File Offset: 0x00000760
		public static int NormalizeAngleToSize(this float val, float width, float height)
		{
			if (val <= 0f)
			{
				val = 0f;
				return (int)val;
			}
			float num = width / val;
			float num2 = height / val;
			if (num < 2f || num2 < 2f)
			{
				val = (float)Mathf.Min((int)(width / 2f), (int)(height / 2f));
			}
			if (val <= 0f)
			{
				val = 0f;
			}
			return (int)val;
		}
	}
}
