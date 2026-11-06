using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000524 RID: 1316
	public class vChangeInputTypeTrigger : MonoBehaviour
	{
		// Token: 0x06001FE4 RID: 8164 RVA: 0x000BD759 File Offset: 0x000BB959
		private void Start()
		{
			vInput.instance.onChangeInputType -= this.OnChangeInput;
			vInput.instance.onChangeInputType += this.OnChangeInput;
			this.OnChangeInput(vInput.instance.inputDevice);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000BD797 File Offset: 0x000BB997
		public void OnChangeInput(InputDevice type)
		{
			switch (type)
			{
			case InputDevice.MouseKeyboard:
				this.OnChangeToKeyboard.Invoke();
				return;
			case InputDevice.Joystick:
				this.OnChangeToJoystick.Invoke();
				return;
			case InputDevice.Mobile:
				this.OnChangeToMobile.Invoke();
				return;
			default:
				return;
			}
		}

		// Token: 0x040020A4 RID: 8356
		[Header("Events called when InputType changed")]
		public UnityEvent OnChangeToKeyboard;

		// Token: 0x040020A5 RID: 8357
		public UnityEvent OnChangeToMobile;

		// Token: 0x040020A6 RID: 8358
		public UnityEvent OnChangeToJoystick;
	}
}
