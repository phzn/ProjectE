using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000FB4 RID: 4020
	public static class MissionsPhoneInstaller
	{
		// Token: 0x06007966 RID: 31078 RVA: 0x00319058 File Offset: 0x00317258
		public static void Ensure(GameObject phone)
		{
			if (phone == null || phone.GetComponentInChildren<MissionsPhoneApp>(true) != null)
			{
				return;
			}
			Transform transform = phone.transform.Find("Canvas/Panel");
			Transform transform2 = ((transform != null) ? transform.Find("Home") : null);
			Transform transform3 = ((transform != null) ? transform.Find("mTrophies") : null);
			Transform transform4 = ((transform2 != null) ? transform2.Find("Widget 2") : null);
			if (transform3 == null || transform4 == null)
			{
				Debug.LogWarning("[Missions] Native phone mission page or widget was not found.", phone);
				return;
			}
			TMP_Text componentInChildren = transform4.GetComponentInChildren<TMP_Text>(true);
			TMP_FontAsset tmp_FontAsset = ((componentInChildren != null) ? componentInChildren.font : TMP_Settings.defaultFontAsset);
			MissionsPhoneApp missionsPhoneApp = transform3.gameObject.AddComponent<MissionsPhoneApp>();
			missionsPhoneApp.Initialize(tmp_FontAsset);
			MissionsPhoneWidget missionsPhoneWidget = transform4.GetComponent<MissionsPhoneWidget>();
			if (missionsPhoneWidget == null)
			{
				missionsPhoneWidget = transform4.gameObject.AddComponent<MissionsPhoneWidget>();
			}
			missionsPhoneWidget.Initialize(tmp_FontAsset);
			Button component = transform4.GetComponent<Button>();
			if (component != null)
			{
				component.onClick = new Button.ButtonClickedEvent();
				component.onClick.AddListener(new UnityAction(missionsPhoneApp.OpenApp));
			}
		}
	}
}
