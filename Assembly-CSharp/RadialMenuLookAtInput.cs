using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200016B RID: 363
public class RadialMenuLookAtInput : MonoBehaviour
{
	// Token: 0x06000BA8 RID: 2984 RVA: 0x00050A38 File Offset: 0x0004EC38
	private void Start()
	{
		if (!base.GetComponent<UltimateRadialMenu>())
		{
			Debug.LogError("This component is not attached to an Ultimate Radial Menu gameObject. Disabling this component to avoid errors.");
			base.enabled = false;
			return;
		}
		if (base.GetComponent<UltimateRadialMenuInputManager>())
		{
			this.inputManager = base.GetComponent<UltimateRadialMenuInputManager>();
			return;
		}
		this.inputManager = UltimateRadialMenuInputManager.Instance;
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00050A8C File Offset: 0x0004EC8C
	private void Update()
	{
		Vector2 vector = Vector2.zero;
		if (this.inputManager.keyboardInput && this.inputManager.CurrentInputDevice == UltimateRadialMenuInputManager.InputDevice.Mouse)
		{
			Mouse device = InputSystem.GetDevice<Mouse>();
			if (device != null)
			{
				vector = device.position.ReadValue();
				vector -= new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				vector /= (float)(((Screen.width > Screen.height) ? Screen.width : Screen.height) / 2);
			}
		}
		if (this.inputManager.controllerInput && this.inputManager.CurrentInputDevice == UltimateRadialMenuInputManager.InputDevice.Controller)
		{
			Vector2 vector2 = Vector2.zero;
			Gamepad device2 = InputSystem.GetDevice<Gamepad>();
			if (device2 != null)
			{
				vector2 = ((this.inputManager.joystick == UltimateRadialMenuInputManager.Joysticks.Left) ? device2.leftStick.ReadValue() : device2.rightStick.ReadValue());
			}
			if (vector2 != Vector2.zero)
			{
				vector = vector2;
			}
		}
		Vector3 vector3 = this.cameraTransform.TransformPoint(new Vector3(vector.x * (float)(this.invert ? 1 : (-1)) * this.distanceModifier, vector.y * (float)(this.invert ? 1 : (-1)) * this.distanceModifier, 25f));
		base.transform.LookAt(vector3);
		base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, 0f);
	}

	// Token: 0x04000B72 RID: 2930
	private UltimateRadialMenuInputManager inputManager;

	// Token: 0x04000B73 RID: 2931
	public Transform cameraTransform;

	// Token: 0x04000B74 RID: 2932
	public bool invert;

	// Token: 0x04000B75 RID: 2933
	public float distanceModifier = 7.5f;
}
