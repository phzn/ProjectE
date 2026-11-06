using System;
using Features.CielOS.UI;
using Features.Rank;
using Features.RunModes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E2F RID: 3631
	[DisallowMultipleComponent]
	public sealed class PhoneAppAccessGate : MonoBehaviour
	{
		// Token: 0x06006C56 RID: 27734 RVA: 0x002B5588 File Offset: 0x002B3788
		public static void Bind(Transform icon, string appId)
		{
			if (icon == null || icon.GetComponent<Button>() == null)
			{
				return;
			}
			PhoneAppAccessGate phoneAppAccessGate = icon.GetComponent<PhoneAppAccessGate>();
			if (phoneAppAccessGate == null)
			{
				phoneAppAccessGate = icon.gameObject.AddComponent<PhoneAppAccessGate>();
			}
			phoneAppAccessGate._appId = appId;
			if (phoneAppAccessGate._button == null)
			{
				phoneAppAccessGate._button = icon.GetComponent<Button>();
				phoneAppAccessGate._launch = phoneAppAccessGate._button.onClick;
				phoneAppAccessGate._button.onClick = new Button.ButtonClickedEvent();
				phoneAppAccessGate._button.onClick.AddListener(new UnityAction(phoneAppAccessGate.Open));
				phoneAppAccessGate._group = icon.GetComponent<CanvasGroup>();
				if (phoneAppAccessGate._group == null)
				{
					phoneAppAccessGate._group = icon.gameObject.AddComponent<CanvasGroup>();
				}
				Transform transform = icon.Find("ComingSoonLock");
				phoneAppAccessGate._badge = ((transform != null) ? transform.gameObject : new GameObject("ComingSoonLock", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				}));
				phoneAppAccessGate._badge.transform.SetParent(icon, false);
				RectTransform rectTransform = (RectTransform)phoneAppAccessGate._badge.transform;
				rectTransform.anchorMin = new Vector2(0.64f, 0.64f);
				rectTransform.anchorMax = new Vector2(0.94f, 0.94f);
				rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
				Image component = phoneAppAccessGate._badge.GetComponent<Image>();
				CielOSIcon.Apply(component, CielOSIconId.Lock, Color.white, 64);
				component.raycastTarget = false;
			}
			phoneAppAccessGate.Refresh();
		}

		// Token: 0x06006C57 RID: 27735 RVA: 0x002B5727 File Offset: 0x002B3927
		private void OnEnable()
		{
			RankProgressionService.Changed += this.Refresh;
			RunModeService.Changed += this.Refresh;
			this.Refresh();
		}

		// Token: 0x06006C58 RID: 27736 RVA: 0x002B5751 File Offset: 0x002B3951
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.Refresh;
			RunModeService.Changed -= this.Refresh;
		}

		// Token: 0x06006C59 RID: 27737 RVA: 0x002B5775 File Offset: 0x002B3975
		private void Open()
		{
			if (PhoneAppAccessPolicy.TryOpen(this._appId))
			{
				Button.ButtonClickedEvent launch = this._launch;
				if (launch == null)
				{
					return;
				}
				launch.Invoke();
			}
		}

		// Token: 0x06006C5A RID: 27738 RVA: 0x002B5794 File Offset: 0x002B3994
		public void Refresh()
		{
			if (this._button == null)
			{
				return;
			}
			bool flag = PhoneAppAccessPolicy.GetAccess(this._appId) == PhoneAppAccess.Available;
			this._button.interactable = true;
			this._group.interactable = true;
			this._group.blocksRaycasts = true;
			this._group.alpha = (flag ? 1f : 0.46f);
			this._badge.SetActive(!flag);
		}

		// Token: 0x04006EC8 RID: 28360
		private string _appId;

		// Token: 0x04006EC9 RID: 28361
		private Button _button;

		// Token: 0x04006ECA RID: 28362
		private Button.ButtonClickedEvent _launch;

		// Token: 0x04006ECB RID: 28363
		private CanvasGroup _group;

		// Token: 0x04006ECC RID: 28364
		private GameObject _badge;
	}
}
