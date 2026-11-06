using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000718 RID: 1816
	[vClassHeader("Action Receiver", true, "icon_v2", false, "")]
	public class vGenericActionReceiver : vMonoBehaviour
	{
		// Token: 0x06002F13 RID: 12051 RVA: 0x00109B18 File Offset: 0x00107D18
		private void Start()
		{
			vGenericAction componentInParent = base.gameObject.GetComponentInParent<vGenericAction>();
			if (componentInParent)
			{
				componentInParent.OnEnterTriggerAction.AddListener(new UnityAction<vTriggerGenericAction>(this.OnEnterTriggerAction));
				componentInParent.OnExitTriggerAction.AddListener(new UnityAction<vTriggerGenericAction>(this.OnExitTriggerAction));
				componentInParent.OnStartAction.AddListener(new UnityAction<vTriggerGenericAction>(this.OnStartAction));
				componentInParent.OnCancelAction.AddListener(new UnityAction<vTriggerGenericAction>(this.OnCancelAction));
				componentInParent.OnEndAction.AddListener(new UnityAction<vTriggerGenericAction>(this.OnEndAction));
			}
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x00109BB4 File Offset: 0x00107DB4
		private void OnDestroy()
		{
			vGenericAction componentInParent = base.GetComponentInParent<vGenericAction>();
			if (componentInParent)
			{
				componentInParent.OnEnterTriggerAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.OnEnterTriggerAction));
				componentInParent.OnExitTriggerAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.OnExitTriggerAction));
				componentInParent.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.OnStartAction));
				componentInParent.OnCancelAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.OnCancelAction));
				componentInParent.OnEndAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.OnEndAction));
			}
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x00109C48 File Offset: 0x00107E48
		protected virtual bool IsValidAction(vTriggerGenericAction actionInfo)
		{
			return base.enabled && base.gameObject.activeInHierarchy && actionInfo != null && this.supportedActionNames.Contains(actionInfo.actionName);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x00109C7B File Offset: 0x00107E7B
		public virtual void OnEnterTriggerAction(vTriggerGenericAction actionInfo)
		{
			if (this.IsValidAction(actionInfo))
			{
				this.onEnterTriggerAction.Invoke();
			}
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x00109C91 File Offset: 0x00107E91
		public virtual void OnExitTriggerAction(vTriggerGenericAction actionInfo)
		{
			if (this.IsValidAction(actionInfo))
			{
				this.onExitTriggerAction.Invoke();
			}
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x00109CA7 File Offset: 0x00107EA7
		public virtual void OnStartAction(vTriggerGenericAction actionInfo)
		{
			if (this.IsValidAction(actionInfo))
			{
				this.onStartAction.Invoke();
			}
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x00109CBD File Offset: 0x00107EBD
		public virtual void OnCancelAction(vTriggerGenericAction actionInfo)
		{
			if (this.IsValidAction(actionInfo))
			{
				this.onCancelAction.Invoke();
			}
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x00109CD3 File Offset: 0x00107ED3
		public virtual void OnEndAction(vTriggerGenericAction actionInfo)
		{
			if (this.IsValidAction(actionInfo))
			{
				this.onEndAction.Invoke();
			}
		}

		// Token: 0x04002F43 RID: 12099
		public List<string> supportedActionNames = new List<string> { "Action" };

		// Token: 0x04002F44 RID: 12100
		public UnityEvent onEnterTriggerAction;

		// Token: 0x04002F45 RID: 12101
		public UnityEvent onExitTriggerAction;

		// Token: 0x04002F46 RID: 12102
		public UnityEvent onStartAction;

		// Token: 0x04002F47 RID: 12103
		public UnityEvent onCancelAction;

		// Token: 0x04002F48 RID: 12104
		public UnityEvent onEndAction;
	}
}
