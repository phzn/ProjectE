using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000F RID: 15
public static class CustomizationScrollResetter
{
	// Token: 0x0600006E RID: 110 RVA: 0x0000518A File Offset: 0x0000338A
	public static void ResetToTopNextFrame(MonoBehaviour host, Transform scope)
	{
		CustomizationScrollResetter.ResetVerticalScrollRects(scope);
		if (host != null && host.isActiveAndEnabled && scope != null)
		{
			host.StartCoroutine(CustomizationScrollResetter.ResetAfterLayout(scope));
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000051B9 File Offset: 0x000033B9
	private static IEnumerator ResetAfterLayout(Transform scope)
	{
		yield return null;
		Canvas.ForceUpdateCanvases();
		CustomizationScrollResetter.ResetVerticalScrollRects(scope);
		yield break;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x000051C8 File Offset: 0x000033C8
	private static void ResetVerticalScrollRects(Transform scope)
	{
		if (scope == null)
		{
			return;
		}
		foreach (ScrollRect scrollRect in scope.GetComponentsInChildren<ScrollRect>(true))
		{
			if (!(scrollRect == null) && scrollRect.vertical && !(scrollRect.content == null))
			{
				scrollRect.StopMovement();
				scrollRect.velocity = Vector2.zero;
				scrollRect.verticalNormalizedPosition = 1f;
			}
		}
	}
}
