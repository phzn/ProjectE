using System;
using System.Collections.Generic;
using Invector;
using Invector.vCharacterController.vActions;
using UnityEngine.Events;

// Token: 0x020000F5 RID: 245
[vClassHeader("Trigger Action Event", true, "icon_v2", false, "", helpBoxText = "Use this to filter a specific TriggerAction so you can use Events with the Controller or components attached to the Controller", useHelpBox = true)]
public class vTriggerActionEvent : vMonoBehaviour
{
	// Token: 0x0600095E RID: 2398 RVA: 0x00043A34 File Offset: 0x00041C34
	public void TriggerEvent(vTriggerGenericAction action)
	{
		vTriggerActionEvent.ActionEvent actionEvent = this.actionFinders.Find((vTriggerActionEvent.ActionEvent a) => a.actionName.Equals(action.gameObject.name));
		if (actionEvent != null)
		{
			actionEvent.onTriggerEvent.Invoke();
		}
	}

	// Token: 0x04000896 RID: 2198
	public List<vTriggerActionEvent.ActionEvent> actionFinders;

	// Token: 0x020000F6 RID: 246
	[Serializable]
	public class ActionEvent
	{
		// Token: 0x04000897 RID: 2199
		public string actionName;

		// Token: 0x04000898 RID: 2200
		public UnityEvent onTriggerEvent;
	}
}
