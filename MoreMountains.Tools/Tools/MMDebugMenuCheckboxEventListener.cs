using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000031 RID: 49
	public class MMDebugMenuCheckboxEventListener : MonoBehaviour
	{
		// Token: 0x06000109 RID: 265 RVA: 0x0000596B File Offset: 0x00003B6B
		protected virtual void TestSetValue()
		{
			MMDebugMenuCheckboxEvent.Trigger(this.CheckboxEventName, this.TestValue, MMDebugMenuCheckboxEvent.EventModes.SetCheckbox);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005980 File Offset: 0x00003B80
		protected virtual void OnMMDebugMenuCheckboxEvent(string checkboxNameEvent, bool value, MMDebugMenuCheckboxEvent.EventModes eventMode)
		{
			if (eventMode == MMDebugMenuCheckboxEvent.EventModes.FromCheckbox && checkboxNameEvent == this.CheckboxEventName)
			{
				if (this.MMDPressedEvent != null)
				{
					this.MMDPressedEvent.Invoke(value);
				}
				if (value)
				{
					if (this.MMDTrueEvent != null)
					{
						this.MMDTrueEvent.Invoke();
						return;
					}
				}
				else if (this.MMDFalseEvent != null)
				{
					this.MMDFalseEvent.Invoke();
				}
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000059DC File Offset: 0x00003BDC
		public virtual void OnEnable()
		{
			MMDebugMenuCheckboxEvent.Register(new MMDebugMenuCheckboxEvent.Delegate(this.OnMMDebugMenuCheckboxEvent));
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000059F0 File Offset: 0x00003BF0
		public virtual void OnDisable()
		{
			MMDebugMenuCheckboxEvent.Unregister(new MMDebugMenuCheckboxEvent.Delegate(this.OnMMDebugMenuCheckboxEvent));
		}

		// Token: 0x04000136 RID: 310
		[Header("Events")]
		public string CheckboxEventName = "CheckboxEventName";

		// Token: 0x04000137 RID: 311
		public MMDCheckboxPressedEvent MMDPressedEvent;

		// Token: 0x04000138 RID: 312
		public MMDCheckboxTrueEvent MMDTrueEvent;

		// Token: 0x04000139 RID: 313
		public MMDCheckboxFalseEvent MMDFalseEvent;

		// Token: 0x0400013A RID: 314
		[Header("Test")]
		public bool TestValue = true;

		// Token: 0x0400013B RID: 315
		[MMInspectorButton("TestSetValue")]
		public bool TestSetValueButton;
	}
}
