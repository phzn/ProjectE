using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013B1 RID: 5041
	public static class CielOSAppAccess
	{
		// Token: 0x06009C09 RID: 39945 RVA: 0x003FB94C File Offset: 0x003F9B4C
		public static void ApplyLock(Image icon, bool locked)
		{
			if (icon == null)
			{
				return;
			}
			Transform transform = icon.transform.Find("PlatformLock");
			if (transform != null)
			{
				transform.gameObject.SetActive(locked);
			}
			if (!locked)
			{
				return;
			}
			icon.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			if (transform != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PlatformLock", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(icon.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = new Vector2(0.52f, -0.08f);
			rectTransform.anchorMax = new Vector2(1.08f, 0.48f);
			rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
			Image component = gameObject.GetComponent<Image>();
			component.raycastTarget = false;
			CielOSIcon.ApplyBadge(component, CielOSIconId.Lock, 64);
		}
	}
}
