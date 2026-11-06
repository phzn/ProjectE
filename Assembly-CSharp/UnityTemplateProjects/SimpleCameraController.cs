using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTemplateProjects
{
	// Token: 0x02000414 RID: 1044
	public class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x06001A78 RID: 6776 RVA: 0x000A74A0 File Offset: 0x000A56A0
		private void Start()
		{
			InputActionMap inputActionMap = new InputActionMap("Simple Camera Controller");
			this.lookAction = inputActionMap.AddAction("look", InputActionType.Value, "<Mouse>/delta", null, null, null, null);
			this.movementAction = inputActionMap.AddAction("move", InputActionType.Value, "<Gamepad>/leftStick", null, null, null, null);
			this.verticalMovementAction = inputActionMap.AddAction("Vertical Movement", InputActionType.Value, null, null, null, null, null);
			this.boostFactorAction = inputActionMap.AddAction("Boost Factor", InputActionType.Value, "<Mouse>/scroll", null, null, null, null);
			this.lookAction.AddBinding("<Gamepad>/rightStick", null, null, null).WithProcessor("scaleVector2(x=15, y=15)");
			this.movementAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/w", null, null).With("Up", "<Keyboard>/upArrow", null, null)
				.With("Down", "<Keyboard>/s", null, null)
				.With("Down", "<Keyboard>/downArrow", null, null)
				.With("Left", "<Keyboard>/a", null, null)
				.With("Left", "<Keyboard>/leftArrow", null, null)
				.With("Right", "<Keyboard>/d", null, null)
				.With("Right", "<Keyboard>/rightArrow", null, null);
			this.verticalMovementAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/pageUp", null, null).With("Down", "<Keyboard>/pageDown", null, null)
				.With("Up", "<Keyboard>/e", null, null)
				.With("Down", "<Keyboard>/q", null, null)
				.With("Up", "<Gamepad>/rightshoulder", null, null)
				.With("Down", "<Gamepad>/leftshoulder", null, null);
			this.boostFactorAction.AddBinding("<Gamepad>/Dpad", null, null, null).WithProcessor("scaleVector2(x=1, y=4)");
			this.movementAction.Enable();
			this.lookAction.Enable();
			this.verticalMovementAction.Enable();
			this.boostFactorAction.Enable();
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000A76CC File Offset: 0x000A58CC
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000A76F0 File Offset: 0x000A58F0
		private Vector3 GetInputTranslationDirection()
		{
			Vector3 zero = Vector3.zero;
			Vector2 vector = this.movementAction.ReadValue<Vector2>();
			zero.x = vector.x;
			zero.z = vector.y;
			zero.y = this.verticalMovementAction.ReadValue<Vector2>().y;
			return zero;
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x000A7744 File Offset: 0x000A5944
		private void Update()
		{
			if (this.IsEscapePressed())
			{
				Application.Quit();
			}
			if (this.IsRightMouseButtonDown())
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (this.IsRightMouseButtonUp())
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			if (this.IsCameraRotationAllowed())
			{
				Vector2 vector = this.GetInputLookRotation() * Time.deltaTime * 5f;
				if (this.invertY)
				{
					vector.y = -vector.y;
				}
				float num = this.mouseSensitivityCurve.Evaluate(vector.magnitude);
				this.m_TargetCameraState.yaw += vector.x * num;
				this.m_TargetCameraState.pitch += vector.y * num;
			}
			Vector3 vector2 = this.GetInputTranslationDirection() * Time.deltaTime;
			if (this.IsBoostPressed())
			{
				vector2 *= 10f;
			}
			this.boost += this.GetBoostFactor();
			vector2 *= Mathf.Pow(2f, this.boost);
			this.m_TargetCameraState.Translate(vector2);
			float num2 = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime);
			float num3 = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime);
			this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, num2, num3);
			this.m_InterpolatingCameraState.UpdateTransform(base.transform);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000A78C8 File Offset: 0x000A5AC8
		private float GetBoostFactor()
		{
			return this.boostFactorAction.ReadValue<Vector2>().y * 0.01f;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000A78E0 File Offset: 0x000A5AE0
		private Vector2 GetInputLookRotation()
		{
			return this.lookAction.ReadValue<Vector2>();
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000A78ED File Offset: 0x000A5AED
		private bool IsBoostPressed()
		{
			return (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed) | (Gamepad.current != null && Gamepad.current.xButton.isPressed);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000A7922 File Offset: 0x000A5B22
		private bool IsEscapePressed()
		{
			return Keyboard.current != null && Keyboard.current.escapeKey.isPressed;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000A793C File Offset: 0x000A5B3C
		private bool IsCameraRotationAllowed()
		{
			return (Mouse.current != null && Mouse.current.rightButton.isPressed) | (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0f);
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000A798B File Offset: 0x000A5B8B
		private bool IsRightMouseButtonDown()
		{
			return Mouse.current != null && Mouse.current.rightButton.isPressed;
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x000A79A5 File Offset: 0x000A5BA5
		private bool IsRightMouseButtonUp()
		{
			return Mouse.current != null && !Mouse.current.rightButton.isPressed;
		}

		// Token: 0x04001B8B RID: 7051
		private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04001B8C RID: 7052
		private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04001B8D RID: 7053
		[Header("Movement Settings")]
		[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
		public float boost = 3.5f;

		// Token: 0x04001B8E RID: 7054
		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float positionLerpTime = 0.2f;

		// Token: 0x04001B8F RID: 7055
		[Header("Rotation Settings")]
		[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04001B90 RID: 7056
		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float rotationLerpTime = 0.01f;

		// Token: 0x04001B91 RID: 7057
		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool invertY;

		// Token: 0x04001B92 RID: 7058
		private InputAction movementAction;

		// Token: 0x04001B93 RID: 7059
		private InputAction verticalMovementAction;

		// Token: 0x04001B94 RID: 7060
		private InputAction lookAction;

		// Token: 0x04001B95 RID: 7061
		private InputAction boostFactorAction;

		// Token: 0x04001B96 RID: 7062
		private bool mouseRightButtonPressed;

		// Token: 0x02000415 RID: 1045
		private class CameraState
		{
			// Token: 0x06001A84 RID: 6788 RVA: 0x000A7A60 File Offset: 0x000A5C60
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x06001A85 RID: 6789 RVA: 0x000A7AD4 File Offset: 0x000A5CD4
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x06001A86 RID: 6790 RVA: 0x000A7B38 File Offset: 0x000A5D38
			public void LerpTowards(SimpleCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x06001A87 RID: 6791 RVA: 0x000A7BD5 File Offset: 0x000A5DD5
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04001B97 RID: 7063
			public float yaw;

			// Token: 0x04001B98 RID: 7064
			public float pitch;

			// Token: 0x04001B99 RID: 7065
			public float roll;

			// Token: 0x04001B9A RID: 7066
			public float x;

			// Token: 0x04001B9B RID: 7067
			public float y;

			// Token: 0x04001B9C RID: 7068
			public float z;
		}
	}
}
