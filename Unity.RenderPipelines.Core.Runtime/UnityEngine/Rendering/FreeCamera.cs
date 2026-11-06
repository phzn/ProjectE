using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.Rendering
{
	// Token: 0x02000033 RID: 51
	public class FreeCamera : MonoBehaviour
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00009EFA File Offset: 0x000080FA
		private void OnEnable()
		{
			this.RegisterInputs();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009F04 File Offset: 0x00008104
		private void RegisterInputs()
		{
			InputActionMap inputActionMap = new InputActionMap("Free Camera");
			this.lookAction = inputActionMap.AddAction("look", InputActionType.Value, "<Mouse>/delta", null, null, null, null);
			this.moveAction = inputActionMap.AddAction("move", InputActionType.Value, "<Gamepad>/leftStick", null, null, null, null);
			this.speedAction = inputActionMap.AddAction("speed", InputActionType.Value, "<Gamepad>/dpad", null, null, null, null);
			this.yMoveAction = inputActionMap.AddAction("yMove", InputActionType.Value, null, null, null, null, null);
			this.lookAction.AddBinding("<Gamepad>/rightStick", null, null, null).WithProcessor("scaleVector2(x=15, y=15)");
			this.moveAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/w", null, null).With("Up", "<Keyboard>/upArrow", null, null)
				.With("Down", "<Keyboard>/s", null, null)
				.With("Down", "<Keyboard>/downArrow", null, null)
				.With("Left", "<Keyboard>/a", null, null)
				.With("Left", "<Keyboard>/leftArrow", null, null)
				.With("Right", "<Keyboard>/d", null, null)
				.With("Right", "<Keyboard>/rightArrow", null, null);
			this.speedAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/home", null, null).With("Down", "<Keyboard>/end", null, null);
			this.yMoveAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/pageUp", null, null).With("Down", "<Keyboard>/pageDown", null, null)
				.With("Up", "<Keyboard>/e", null, null)
				.With("Down", "<Keyboard>/q", null, null)
				.With("Up", "<Gamepad>/rightshoulder", null, null)
				.With("Down", "<Gamepad>/leftshoulder", null, null);
			this.moveAction.Enable();
			this.lookAction.Enable();
			this.speedAction.Enable();
			this.yMoveAction.Enable();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A14C File Offset: 0x0000834C
		private void UpdateInputs()
		{
			this.inputRotateAxisX = 0f;
			this.inputRotateAxisY = 0f;
			this.leftShiftBoost = false;
			this.fire1 = false;
			Vector2 vector = this.lookAction.ReadValue<Vector2>();
			this.inputRotateAxisX = vector.x * this.m_LookSpeedMouse * 0.01f;
			this.inputRotateAxisY = vector.y * this.m_LookSpeedMouse * 0.01f;
			Keyboard current = Keyboard.current;
			bool? flag;
			if (current == null)
			{
				flag = null;
			}
			else
			{
				KeyControl leftShiftKey = current.leftShiftKey;
				flag = ((leftShiftKey != null) ? new bool?(leftShiftKey.isPressed) : null);
			}
			bool? flag2 = flag;
			this.leftShift = flag2.GetValueOrDefault();
			Mouse current2 = Mouse.current;
			bool flag3;
			if (current2 == null)
			{
				flag3 = false;
			}
			else
			{
				ButtonControl leftButton = current2.leftButton;
				flag2 = ((leftButton != null) ? new bool?(leftButton.isPressed) : null);
				bool flag4 = true;
				flag3 = (flag2.GetValueOrDefault() == flag4) & (flag2 != null);
			}
			bool flag5;
			if (!flag3)
			{
				Gamepad current3 = Gamepad.current;
				if (current3 == null)
				{
					flag5 = false;
				}
				else
				{
					ButtonControl xButton = current3.xButton;
					flag2 = ((xButton != null) ? new bool?(xButton.isPressed) : null);
					bool flag4 = true;
					flag5 = (flag2.GetValueOrDefault() == flag4) & (flag2 != null);
				}
			}
			else
			{
				flag5 = true;
			}
			this.fire1 = flag5;
			this.inputChangeSpeed = this.speedAction.ReadValue<Vector2>().y;
			Vector2 vector2 = this.moveAction.ReadValue<Vector2>();
			this.inputVertical = vector2.y;
			this.inputHorizontal = vector2.x;
			this.inputYAxis = this.yMoveAction.ReadValue<Vector2>().y;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A2D8 File Offset: 0x000084D8
		private void Update()
		{
			if (DebugManager.instance.displayRuntimeUI)
			{
				return;
			}
			this.UpdateInputs();
			if (this.inputChangeSpeed != 0f)
			{
				this.m_MoveSpeed += this.inputChangeSpeed * this.m_MoveSpeedIncrement;
				if (this.m_MoveSpeed < this.m_MoveSpeedIncrement)
				{
					this.m_MoveSpeed = this.m_MoveSpeedIncrement;
				}
			}
			if (this.inputRotateAxisX != 0f || this.inputRotateAxisY != 0f || this.inputVertical != 0f || this.inputHorizontal != 0f || this.inputYAxis != 0f)
			{
				float x = base.transform.localEulerAngles.x;
				float num = base.transform.localEulerAngles.y + this.inputRotateAxisX;
				float num2 = x - this.inputRotateAxisY;
				if (x <= 90f && num2 >= 0f)
				{
					num2 = Mathf.Clamp(num2, 0f, 90f);
				}
				if (x >= 270f)
				{
					num2 = Mathf.Clamp(num2, 270f, 360f);
				}
				base.transform.localRotation = Quaternion.Euler(num2, num, base.transform.localEulerAngles.z);
				float num3 = Time.deltaTime * this.m_MoveSpeed;
				if (this.fire1 || (this.leftShiftBoost && this.leftShift))
				{
					num3 *= this.m_Turbo;
				}
				base.transform.position += base.transform.forward * num3 * this.inputVertical;
				base.transform.position += base.transform.right * num3 * this.inputHorizontal;
				base.transform.position += Vector3.up * num3 * this.inputYAxis;
			}
		}

		// Token: 0x04000120 RID: 288
		private const float k_MouseSensitivityMultiplier = 0.01f;

		// Token: 0x04000121 RID: 289
		public float m_LookSpeedController = 120f;

		// Token: 0x04000122 RID: 290
		public float m_LookSpeedMouse = 4f;

		// Token: 0x04000123 RID: 291
		public float m_MoveSpeed = 10f;

		// Token: 0x04000124 RID: 292
		public float m_MoveSpeedIncrement = 2.5f;

		// Token: 0x04000125 RID: 293
		public float m_Turbo = 10f;

		// Token: 0x04000126 RID: 294
		private InputAction lookAction;

		// Token: 0x04000127 RID: 295
		private InputAction moveAction;

		// Token: 0x04000128 RID: 296
		private InputAction speedAction;

		// Token: 0x04000129 RID: 297
		private InputAction yMoveAction;

		// Token: 0x0400012A RID: 298
		private float inputRotateAxisX;

		// Token: 0x0400012B RID: 299
		private float inputRotateAxisY;

		// Token: 0x0400012C RID: 300
		private float inputChangeSpeed;

		// Token: 0x0400012D RID: 301
		private float inputVertical;

		// Token: 0x0400012E RID: 302
		private float inputHorizontal;

		// Token: 0x0400012F RID: 303
		private float inputYAxis;

		// Token: 0x04000130 RID: 304
		private bool leftShiftBoost;

		// Token: 0x04000131 RID: 305
		private bool leftShift;

		// Token: 0x04000132 RID: 306
		private bool fire1;
	}
}
