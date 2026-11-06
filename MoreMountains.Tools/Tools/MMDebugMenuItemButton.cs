using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200003A RID: 58
	public class MMDebugMenuItemButton : MonoBehaviour
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00006BF2 File Offset: 0x00004DF2
		public virtual void TriggerButtonEvent()
		{
			MMDebugMenuButtonEvent.Trigger(this.ButtonEventName, true, MMDebugMenuButtonEvent.EventModes.FromButton);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006C01 File Offset: 0x00004E01
		protected virtual void OnMMDebugMenuButtonEvent(string checkboxEventName, bool active, MMDebugMenuButtonEvent.EventModes eventMode)
		{
			if (eventMode == MMDebugMenuButtonEvent.EventModes.SetButton && checkboxEventName == this.ButtonEventName && this.TargetButton != null)
			{
				this.TargetButton.interactable = active;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006C2F File Offset: 0x00004E2F
		public virtual void OnEnable()
		{
			if (!this._listening)
			{
				this._listening = true;
				MMDebugMenuButtonEvent.Register(new MMDebugMenuButtonEvent.Delegate(this.OnMMDebugMenuButtonEvent));
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006C52 File Offset: 0x00004E52
		public virtual void OnDestroy()
		{
			this._listening = false;
			MMDebugMenuButtonEvent.Unregister(new MMDebugMenuButtonEvent.Delegate(this.OnMMDebugMenuButtonEvent));
		}

		// Token: 0x04000196 RID: 406
		[Header("Bindings")]
		public Button TargetButton;

		// Token: 0x04000197 RID: 407
		public Text ButtonText;

		// Token: 0x04000198 RID: 408
		public Image ButtonBg;

		// Token: 0x04000199 RID: 409
		public string ButtonEventName = "Button";

		// Token: 0x0400019A RID: 410
		protected bool _listening;
	}
}
