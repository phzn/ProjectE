using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000194 RID: 404
	[AddComponentMenu("More Mountains/Tools/Camera/MMGhostCamera")]
	public class MMGhostCamera : MonoBehaviour
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0002C368 File Offset: 0x0002A568
		protected virtual void Start()
		{
			if (this.AutoActivation)
			{
				this.ToggleFreeCamera();
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002C378 File Offset: 0x0002A578
		protected virtual void Update()
		{
			if (Input.GetKeyDown(this.ActivateButton))
			{
				this.ToggleFreeCamera();
			}
			if (!this.Active)
			{
				return;
			}
			this.GetInput();
			this.Translate();
			this.Rotate();
			this.Move();
			this.HandleMobileControls();
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002C3B4 File Offset: 0x0002A5B4
		protected virtual void GetInput()
		{
			if (!this.UseMobileControls || this.LeftStick == null)
			{
				this._currentInput.x = Input.GetAxis("Horizontal");
				this._currentInput.y = 0f;
				this._currentInput.z = Input.GetAxis("Vertical");
				this._mouseInput.x = Input.GetAxis("Mouse X");
				this._mouseInput.y = Input.GetAxis("Mouse Y");
			}
			else
			{
				this._currentInput.x = this.LeftStick.RawValue.x;
				this._currentInput.y = 0f;
				this._currentInput.z = this.LeftStick.RawValue.y;
			}
			bool key = Input.GetKey(this.UpButton);
			bool key2 = Input.GetKey(this.DownButton);
			bool key3 = Input.GetKey(this.RunButton);
			bool keyDown = Input.GetKeyDown(this.TimescaleModificationButton);
			this._currentInput.y = 0f;
			if (key)
			{
				this._currentInput.y = 1f;
			}
			if (key2)
			{
				this._currentInput.y = -1f;
			}
			this._speedMultiplier = (key3 ? this.RunFactor : 1f);
			this._normalizedInput = this._currentInput.normalized;
			if (keyDown)
			{
				this.ToggleSlowMotion();
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002C520 File Offset: 0x0002A720
		protected virtual void HandleMobileControls()
		{
			if (Input.GetKeyDown(this.ControlsModeSwitch))
			{
				this.UseMobileControls = !this.UseMobileControls;
			}
			if (this.UseMobileControls)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else if (this.Active)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			if (this.LeftStickContainer != null)
			{
				GameObject leftStickContainer = this.LeftStickContainer;
				if (leftStickContainer != null)
				{
					leftStickContainer.SetActive(this.UseMobileControls);
				}
			}
			if (this.RightStickContainer != null)
			{
				GameObject rightStickContainer = this.RightStickContainer;
				if (rightStickContainer == null)
				{
					return;
				}
				rightStickContainer.SetActive(this.UseMobileControls);
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002C5BC File Offset: 0x0002A7BC
		protected virtual void Translate()
		{
			if (!this.MovementEnabled)
			{
				return;
			}
			if (this.Acceleration == 0f || this.Deceleration == 0f)
			{
				this._lerpedInput = this._currentInput;
			}
			else if (this._normalizedInput.magnitude == 0f)
			{
				this._acceleration = Mathf.Lerp(this._acceleration, 0f, this.Deceleration * Time.deltaTime);
				this._lerpedInput = Vector3.Lerp(this._lerpedInput, this._lerpedInput * this._acceleration, Time.deltaTime * this.Deceleration);
			}
			else
			{
				this._acceleration = Mathf.Lerp(this._acceleration, 1f, this.Acceleration * Time.deltaTime);
				this._lerpedInput = Vector3.ClampMagnitude(this._normalizedInput, this._acceleration);
			}
			this._movementVector = this._lerpedInput;
			this._movementVector *= this.MovementSpeed * this._speedMultiplier;
			if (this._movementVector.magnitude > this.MovementSpeed * this._speedMultiplier)
			{
				this._movementVector = Vector3.ClampMagnitude(this._movementVector, this.MovementSpeed * this._speedMultiplier);
			}
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0002C6FC File Offset: 0x0002A8FC
		protected virtual void Rotate()
		{
			if (!this.RotationEnabled)
			{
				return;
			}
			this._newEulerAngles = base.transform.eulerAngles;
			if (!this.UseMobileControls || this.LeftStick == null)
			{
				this._newEulerAngles.x = this._newEulerAngles.x + -this._mouseInput.y * 359f * this.MouseSensitivity;
				this._newEulerAngles.y = this._newEulerAngles.y + this._mouseInput.x * 359f * this.MouseSensitivity;
			}
			else
			{
				this._newEulerAngles.x = this._newEulerAngles.x + -this.RightStick.RawValue.y * this.MobileStickSensitivity;
				this._newEulerAngles.y = this._newEulerAngles.y + this.RightStick.RawValue.x * this.MobileStickSensitivity;
			}
			this._newEulerAngles = Vector3.Lerp(base.transform.eulerAngles, this._newEulerAngles, Time.deltaTime * this.RotationSpeed);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0002C800 File Offset: 0x0002AA00
		protected virtual void Move()
		{
			base.transform.eulerAngles = this._newEulerAngles;
			base.transform.position += base.transform.rotation * this._movementVector * Time.deltaTime;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0002C854 File Offset: 0x0002AA54
		protected virtual void ToggleSlowMotion()
		{
			this.TimeAltered = !this.TimeAltered;
			if (this.TimeAltered)
			{
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, this.TimescaleModifier, 1f, true, 5f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
				return;
			}
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Unfreeze, 1f, 0f, false, 0f, false, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0002C8C4 File Offset: 0x0002AAC4
		protected virtual void ToggleFreeCamera()
		{
			this.Active = !this.Active;
			Cursor.lockState = (this.Active ? CursorLockMode.Locked : CursorLockMode.None);
			Cursor.visible = !this.Active;
		}

		// Token: 0x04000B50 RID: 2896
		[Header("Speed")]
		public float MovementSpeed = 10f;

		// Token: 0x04000B51 RID: 2897
		public float RunFactor = 4f;

		// Token: 0x04000B52 RID: 2898
		public float Acceleration = 5f;

		// Token: 0x04000B53 RID: 2899
		public float Deceleration = 5f;

		// Token: 0x04000B54 RID: 2900
		public float RotationSpeed = 40f;

		// Token: 0x04000B55 RID: 2901
		[Header("Controls")]
		public KeyCode ActivateButton = KeyCode.LeftShift;

		// Token: 0x04000B56 RID: 2902
		public string HorizontalAxisName = "Horizontal";

		// Token: 0x04000B57 RID: 2903
		public string VerticalAxisName = "Vertical";

		// Token: 0x04000B58 RID: 2904
		public KeyCode UpButton = KeyCode.Space;

		// Token: 0x04000B59 RID: 2905
		public KeyCode DownButton = KeyCode.C;

		// Token: 0x04000B5A RID: 2906
		public KeyCode ControlsModeSwitch = KeyCode.M;

		// Token: 0x04000B5B RID: 2907
		public KeyCode TimescaleModificationButton = KeyCode.F;

		// Token: 0x04000B5C RID: 2908
		public KeyCode RunButton = KeyCode.RightShift;

		// Token: 0x04000B5D RID: 2909
		[Header("Mouse")]
		public float MouseSensitivity = 0.02f;

		// Token: 0x04000B5E RID: 2910
		public float MobileStickSensitivity = 2f;

		// Token: 0x04000B5F RID: 2911
		[Header("Timescale Modification")]
		public float TimescaleModifier = 0.5f;

		// Token: 0x04000B60 RID: 2912
		[Header("Settings")]
		public bool AutoActivation = true;

		// Token: 0x04000B61 RID: 2913
		public bool MovementEnabled = true;

		// Token: 0x04000B62 RID: 2914
		public bool RotationEnabled = true;

		// Token: 0x04000B63 RID: 2915
		[MMReadOnly]
		public bool Active;

		// Token: 0x04000B64 RID: 2916
		[MMReadOnly]
		public bool TimeAltered;

		// Token: 0x04000B65 RID: 2917
		[Header("Virtual Joysticks")]
		public bool UseMobileControls;

		// Token: 0x04000B66 RID: 2918
		[MMCondition("UseMobileControls", true)]
		public GameObject LeftStickContainer;

		// Token: 0x04000B67 RID: 2919
		[MMCondition("UseMobileControls", true)]
		public GameObject RightStickContainer;

		// Token: 0x04000B68 RID: 2920
		[MMCondition("UseMobileControls", true)]
		public MMTouchJoystick LeftStick;

		// Token: 0x04000B69 RID: 2921
		[MMCondition("UseMobileControls", true)]
		public MMTouchJoystick RightStick;

		// Token: 0x04000B6A RID: 2922
		protected Vector3 _currentInput;

		// Token: 0x04000B6B RID: 2923
		protected Vector3 _lerpedInput;

		// Token: 0x04000B6C RID: 2924
		protected Vector3 _normalizedInput;

		// Token: 0x04000B6D RID: 2925
		protected float _acceleration;

		// Token: 0x04000B6E RID: 2926
		protected float _deceleration;

		// Token: 0x04000B6F RID: 2927
		protected Vector3 _movementVector = Vector3.zero;

		// Token: 0x04000B70 RID: 2928
		protected float _speedMultiplier;

		// Token: 0x04000B71 RID: 2929
		protected Vector3 _newEulerAngles;

		// Token: 0x04000B72 RID: 2930
		protected Vector2 _mouseInput;
	}
}
