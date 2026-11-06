using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200002C RID: 44
	public class MMDebugMenuButtonEventListener : MonoBehaviour
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00005843 File Offset: 0x00003A43
		protected virtual void TestSetValue()
		{
			MMDebugMenuButtonEvent.Trigger(this.ButtonEventName, this.TestValue, MMDebugMenuButtonEvent.EventModes.SetButton);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005857 File Offset: 0x00003A57
		protected virtual void OnMMDebugMenuButtonEvent(string buttonEventName, bool value, MMDebugMenuButtonEvent.EventModes eventMode)
		{
			if (eventMode == MMDebugMenuButtonEvent.EventModes.FromButton && buttonEventName == this.ButtonEventName && this.MMDEvent != null)
			{
				this.MMDEvent.Invoke();
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000587D File Offset: 0x00003A7D
		public virtual void OnEnable()
		{
			MMDebugMenuButtonEvent.Register(new MMDebugMenuButtonEvent.Delegate(this.OnMMDebugMenuButtonEvent));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005891 File Offset: 0x00003A91
		public virtual void OnDisable()
		{
			MMDebugMenuButtonEvent.Unregister(new MMDebugMenuButtonEvent.Delegate(this.OnMMDebugMenuButtonEvent));
		}

		// Token: 0x04000131 RID: 305
		[Header("Event")]
		public string ButtonEventName = "Button";

		// Token: 0x04000132 RID: 306
		public MMDButtonPressedEvent MMDEvent;

		// Token: 0x04000133 RID: 307
		[Header("Test")]
		public bool TestValue = true;

		// Token: 0x04000134 RID: 308
		[MMInspectorButton("TestSetValue")]
		public bool TestSetValueButton;
	}
}
