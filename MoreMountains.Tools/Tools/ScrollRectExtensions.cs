using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x020000EE RID: 238
	public static class ScrollRectExtensions
	{
		// Token: 0x06000501 RID: 1281 RVA: 0x00016FA3 File Offset: 0x000151A3
		public static void MMScrollToTop(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 1f);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00016FBA File Offset: 0x000151BA
		public static void MMScrollToBottom(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 0f);
		}
	}
}
