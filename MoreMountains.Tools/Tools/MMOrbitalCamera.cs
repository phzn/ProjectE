using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x0200001F RID: 31
	[AddComponentMenu("More Mountains/Tools/Camera/MMOrbitalCamera")]
	public class MMOrbitalCamera : MonoBehaviour
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x000036FD File Offset: 0x000018FD
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003708 File Offset: 0x00001908
		public virtual void Initialization()
		{
			if (this.Target == null)
			{
				Debug.LogError(base.gameObject.name + " : the MMOrbitalCamera doesn't have a target.");
				return;
			}
			this.DistanceToTarget = Vector3.Distance(this.Target.position, base.transform.position);
			this._currentDistance = this.DistanceToTarget;
			this._desiredDistance = this.DistanceToTarget;
			this._position = base.transform.position;
			this._rotation = base.transform.rotation;
			this._currentRotation = base.transform.rotation;
			this._desiredRotation = base.transform.rotation;
			this._angleX = Vector3.Angle(Vector3.right, base.transform.right);
			this._angleY = Vector3.Angle(Vector3.up, base.transform.up);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000037F1 File Offset: 0x000019F1
		protected virtual void LateUpdate()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Rotation();
			this.Zoom();
			this.StepDetection();
			this.ApplyMovement();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000381C File Offset: 0x00001A1C
		protected virtual void Rotation()
		{
			if (!this.RotationEnabled)
			{
				return;
			}
			if (this.Mode == MMOrbitalCamera.Modes.Touch && Input.touchCount > 0)
			{
				if (Input.touches[0].phase == TouchPhase.Moved && Input.touchCount == 1)
				{
					float num = (float)Screen.currentResolution.height;
					if (Input.touches[0].position.y < num / 4f)
					{
						return;
					}
					float num2 = Input.touches[0].deltaPosition.magnitude / Input.touches[0].deltaTime;
					this._angleX += Input.touches[0].deltaPosition.x * this.RotationSpeed.x * Time.deltaTime * num2 * 1E-05f;
					this._angleY -= Input.touches[0].deltaPosition.y * this.RotationSpeed.y * Time.deltaTime * num2 * 1E-05f;
					this._stepBuffer += Input.touches[0].deltaPosition.x;
					this._angleY = MMMaths.ClampAngle(this._angleY, (float)this.MinVerticalAngleLimit, (float)this.MaxVerticalAngleLimit);
					this._desiredRotation = Quaternion.Euler(this._angleY, this._angleX, 0f);
					this._currentRotation = base.transform.rotation;
					this._rotation = Quaternion.Lerp(this._currentRotation, this._desiredRotation, Time.deltaTime * this.ZoomDampening);
					base.transform.rotation = this._rotation;
				}
				else if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
				{
					this._desiredRotation = base.transform.rotation;
				}
				if (base.transform.rotation != this._desiredRotation)
				{
					this._rotation = Quaternion.Lerp(base.transform.rotation, this._desiredRotation, Time.deltaTime * this.ZoomDampening);
					base.transform.rotation = this._rotation;
					return;
				}
			}
			else if (this.Mode == MMOrbitalCamera.Modes.Mouse)
			{
				this._angleX += Input.GetAxis("Mouse X") * this.RotationSpeed.x * Time.deltaTime;
				this._angleY += -Input.GetAxis("Mouse Y") * this.RotationSpeed.y * Time.deltaTime;
				this._angleY = Mathf.Clamp(this._angleY, (float)this.MinVerticalAngleLimit, (float)this.MaxVerticalAngleLimit);
				this._desiredRotation = Quaternion.Euler(new Vector3(this._angleY, this._angleX, 0f));
				this._currentRotation = base.transform.rotation;
				this._rotation = Quaternion.Lerp(this._currentRotation, this._desiredRotation, Time.deltaTime * this.ZoomDampening);
				base.transform.rotation = this._rotation;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003B41 File Offset: 0x00001D41
		protected virtual void StepDetection()
		{
			if (Mathf.Abs(this._stepBuffer) > this.StepThreshold)
			{
				UnityEvent stepFeedback = this.StepFeedback;
				if (stepFeedback != null)
				{
					stepFeedback.Invoke();
				}
				this._stepBuffer = 0f;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003B74 File Offset: 0x00001D74
		protected virtual void Zoom()
		{
			if (!this.ZoomEnabled)
			{
				return;
			}
			if (this.Mode == MMOrbitalCamera.Modes.Touch && Input.touchCount > 0)
			{
				if (Input.touchCount == 2)
				{
					Touch touch = Input.GetTouch(0);
					Touch touch2 = Input.GetTouch(1);
					Vector2 vector = touch.position - touch.deltaPosition;
					Vector2 vector2 = touch2.position - touch2.deltaPosition;
					float magnitude = (vector - vector2).magnitude;
					float magnitude2 = (touch.position - touch2.position).magnitude;
					float num = magnitude - magnitude2;
					this._desiredDistance += num * Time.deltaTime * (float)this.ZoomSpeed * Mathf.Abs(this._desiredDistance) * 0.001f;
					this._desiredDistance = Mathf.Clamp(this._desiredDistance, this.MinimumZoomDistance, this.MaximumZoomDistance);
					this._currentDistance = Mathf.Lerp(this._currentDistance, this._desiredDistance, Time.deltaTime * this.ZoomDampening);
					return;
				}
			}
			else if (this.Mode == MMOrbitalCamera.Modes.Mouse)
			{
				this._scrollWheelAmount += -Input.GetAxis("Mouse ScrollWheel") * this.MouseWheelSpeed;
				this._scrollWheelAmount = Mathf.Clamp(this._scrollWheelAmount, -this.MaxMouseWheelClamp, this.MaxMouseWheelClamp);
				float scrollWheelAmount = this._scrollWheelAmount;
				this._desiredDistance += scrollWheelAmount * Time.deltaTime * (float)this.ZoomSpeed * Mathf.Abs(this._desiredDistance) * 0.001f;
				this._desiredDistance = Mathf.Clamp(this._desiredDistance, this.MinimumZoomDistance, this.MaximumZoomDistance);
				this._currentDistance = Mathf.Lerp(this._currentDistance, this._desiredDistance, Time.deltaTime * this.ZoomDampening);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003D40 File Offset: 0x00001F40
		protected virtual void ApplyMovement()
		{
			this._position = this.Target.position - (this._rotation * Vector3.forward * this._currentDistance + this.TargetOffset);
			base.transform.position = this._position;
		}

		// Token: 0x040000A5 RID: 165
		[Header("Setup")]
		public MMOrbitalCamera.Modes Mode = MMOrbitalCamera.Modes.Touch;

		// Token: 0x040000A6 RID: 166
		public Transform Target;

		// Token: 0x040000A7 RID: 167
		public Vector3 TargetOffset;

		// Token: 0x040000A8 RID: 168
		[MMReadOnly]
		public float DistanceToTarget = 5f;

		// Token: 0x040000A9 RID: 169
		[Header("Rotation")]
		public bool RotationEnabled = true;

		// Token: 0x040000AA RID: 170
		public Vector2 RotationSpeed = new Vector2(200f, 200f);

		// Token: 0x040000AB RID: 171
		public int MinVerticalAngleLimit = -80;

		// Token: 0x040000AC RID: 172
		public int MaxVerticalAngleLimit = 80;

		// Token: 0x040000AD RID: 173
		[Header("Zoom")]
		public bool ZoomEnabled = true;

		// Token: 0x040000AE RID: 174
		public float MinimumZoomDistance = 0.6f;

		// Token: 0x040000AF RID: 175
		public float MaximumZoomDistance = 20f;

		// Token: 0x040000B0 RID: 176
		public int ZoomSpeed = 40;

		// Token: 0x040000B1 RID: 177
		public float ZoomDampening = 5f;

		// Token: 0x040000B2 RID: 178
		[Header("Mouse Zoom")]
		public float MouseWheelSpeed = 10f;

		// Token: 0x040000B3 RID: 179
		public float MaxMouseWheelClamp = 10f;

		// Token: 0x040000B4 RID: 180
		[Header("Steps")]
		public float StepThreshold = 1f;

		// Token: 0x040000B5 RID: 181
		public UnityEvent StepFeedback;

		// Token: 0x040000B6 RID: 182
		protected float _angleX;

		// Token: 0x040000B7 RID: 183
		protected float _angleY;

		// Token: 0x040000B8 RID: 184
		protected float _currentDistance;

		// Token: 0x040000B9 RID: 185
		protected float _desiredDistance;

		// Token: 0x040000BA RID: 186
		protected Quaternion _currentRotation;

		// Token: 0x040000BB RID: 187
		protected Quaternion _desiredRotation;

		// Token: 0x040000BC RID: 188
		protected Quaternion _rotation;

		// Token: 0x040000BD RID: 189
		protected Vector3 _position;

		// Token: 0x040000BE RID: 190
		protected float _scrollWheelAmount;

		// Token: 0x040000BF RID: 191
		protected float _stepBuffer;

		// Token: 0x020003A5 RID: 933
		public enum Modes
		{
			// Token: 0x04001D67 RID: 7527
			Mouse,
			// Token: 0x04001D68 RID: 7528
			Touch
		}
	}
}
