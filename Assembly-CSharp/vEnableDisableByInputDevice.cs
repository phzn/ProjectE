using System;
using Invector.vCharacterController;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class vEnableDisableByInputDevice : MonoBehaviour
{
	// Token: 0x060009A7 RID: 2471 RVA: 0x00044856 File Offset: 0x00042A56
	private void Start()
	{
		vInput.instance.onChangeInputType -= this.OnChangeInput;
		vInput.instance.onChangeInputType += this.OnChangeInput;
		this.OnChangeInput(vInput.instance.inputDevice);
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00044894 File Offset: 0x00042A94
	public void OnChangeInput(InputDevice type)
	{
		bool flag = ((this.methodToCheck == vEnableDisableByInputDevice.CheckMethod.Different) ? (type != this.inputDevice) : (type == this.inputDevice));
		if (base.gameObject.activeSelf != flag)
		{
			base.gameObject.SetActive(flag);
		}
	}

	// Token: 0x040008F9 RID: 2297
	public InputDevice inputDevice;

	// Token: 0x040008FA RID: 2298
	public vEnableDisableByInputDevice.CheckMethod methodToCheck;

	// Token: 0x02000113 RID: 275
	public enum CheckMethod
	{
		// Token: 0x040008FC RID: 2300
		Equals,
		// Token: 0x040008FD RID: 2301
		Different
	}
}
