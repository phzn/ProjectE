using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200003B RID: 59
	public class MMDebugMenuItemCheckbox : MonoBehaviour
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00006C80 File Offset: 0x00004E80
		public virtual void TriggerCheckboxEvent()
		{
			if (this._valueSetThisFrame)
			{
				this._valueSetThisFrame = false;
				return;
			}
			MMDebugMenuCheckboxEvent.Trigger(this.CheckboxEventName, this.Switch.SwitchState, MMDebugMenuCheckboxEvent.EventModes.FromCheckbox);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006CA9 File Offset: 0x00004EA9
		public virtual void TriggerCheckboxEventTrue()
		{
			if (this._valueSetThisFrame)
			{
				this._valueSetThisFrame = false;
				return;
			}
			MMDebugMenuCheckboxEvent.Trigger(this.CheckboxEventName, true, MMDebugMenuCheckboxEvent.EventModes.FromCheckbox);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006CC8 File Offset: 0x00004EC8
		public virtual void TriggerCheckboxEventFalse()
		{
			if (this._valueSetThisFrame)
			{
				this._valueSetThisFrame = false;
				return;
			}
			MMDebugMenuCheckboxEvent.Trigger(this.CheckboxEventName, false, MMDebugMenuCheckboxEvent.EventModes.FromCheckbox);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006CE7 File Offset: 0x00004EE7
		protected virtual void OnMMDebugMenuCheckboxEvent(string checkboxEventName, bool value, MMDebugMenuCheckboxEvent.EventModes eventMode)
		{
			if (eventMode == MMDebugMenuCheckboxEvent.EventModes.SetCheckbox && checkboxEventName == this.CheckboxEventName)
			{
				this._valueSetThisFrame = true;
				if (value)
				{
					this.Switch.SetTrue();
					return;
				}
				this.Switch.SetFalse();
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006D1C File Offset: 0x00004F1C
		public virtual void OnEnable()
		{
			if (!this._listening)
			{
				this._listening = true;
				MMDebugMenuCheckboxEvent.Register(new MMDebugMenuCheckboxEvent.Delegate(this.OnMMDebugMenuCheckboxEvent));
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006D3F File Offset: 0x00004F3F
		public virtual void OnDestroy()
		{
			this._listening = false;
			MMDebugMenuCheckboxEvent.Unregister(new MMDebugMenuCheckboxEvent.Delegate(this.OnMMDebugMenuCheckboxEvent));
		}

		// Token: 0x0400019B RID: 411
		[Header("Bindings")]
		public MMDebugMenuSwitch Switch;

		// Token: 0x0400019C RID: 412
		public Text SwitchText;

		// Token: 0x0400019D RID: 413
		public string CheckboxEventName = "Checkbox";

		// Token: 0x0400019E RID: 414
		protected bool _valueSetThisFrame;

		// Token: 0x0400019F RID: 415
		protected bool _listening;
	}
}
