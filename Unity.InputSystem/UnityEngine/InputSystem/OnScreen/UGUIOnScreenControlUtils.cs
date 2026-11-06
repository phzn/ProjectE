using System;

namespace UnityEngine.InputSystem.OnScreen
{
	// Token: 0x02000098 RID: 152
	internal static class UGUIOnScreenControlUtils
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x000400E4 File Offset: 0x0003E2E4
		public static RectTransform GetCanvasRectTransform(Transform transform)
		{
			if (!(transform.parent != null))
			{
				return null;
			}
			return transform.parent.GetComponentInParent<RectTransform>();
		}
	}
}
