using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000021 RID: 33
	internal static class AlignmentUtils
	{
		// Token: 0x06000158 RID: 344 RVA: 0x00003C24 File Offset: 0x00001E24
		internal static float RoundToPixelGrid(float v, float pixelsPerPoint, float offset = 0.02f)
		{
			return Mathf.Floor(v * pixelsPerPoint + 0.5f + offset) / pixelsPerPoint;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003C48 File Offset: 0x00001E48
		internal static float CeilToPixelGrid(float v, float pixelsPerPoint, float offset = -0.02f)
		{
			return Mathf.Ceil(v * pixelsPerPoint + offset) / pixelsPerPoint;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003C68 File Offset: 0x00001E68
		internal static float FloorToPixelGrid(float v, float pixelsPerPoint, float offset = 0.02f)
		{
			return Mathf.Floor(v * pixelsPerPoint + offset) / pixelsPerPoint;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00003C88 File Offset: 0x00001E88
		internal static float RoundToPanelPixelSize(this VisualElement ve, float v)
		{
			return AlignmentUtils.RoundToPixelGrid(v, ve.scaledPixelsPerPoint, 0.02f);
		}
	}
}
