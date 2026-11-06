using System;
using UnityEngine;

namespace Walking2
{
	// Token: 0x02000173 RID: 371
	public class DemoCameraController : MonoBehaviour
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x0005117A File Offset: 0x0004F37A
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000511A0 File Offset: 0x0004F3A0
		private Vector3 GetInputTranslationDirection()
		{
			Vector3 vector = default(Vector3);
			if (Input.GetKey(KeyCode.W))
			{
				vector += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				vector += Vector3.back;
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D))
			{
				vector += Vector3.right;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				vector += Vector3.down;
			}
			if (Input.GetKey(KeyCode.E))
			{
				vector += Vector3.up;
			}
			return vector;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00051234 File Offset: 0x0004F434
		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
			if (Input.GetMouseButtonDown(1))
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (Input.GetMouseButtonUp(1))
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			if (Input.GetMouseButton(1))
			{
				Vector2 vector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (float)(this.invertY ? 1 : (-1)));
				float num = this.mouseSensitivityCurve.Evaluate(vector.magnitude);
				this.m_TargetCameraState.yaw += vector.x * num;
				this.m_TargetCameraState.pitch += vector.y * num;
			}
			Vector3 vector2 = this.GetInputTranslationDirection() * Time.deltaTime;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector2 *= 10f;
			}
			this.boost += Input.mouseScrollDelta.y * 0.2f;
			vector2 *= Mathf.Pow(2f, this.boost);
			this.m_TargetCameraState.Translate(vector2);
			float num2 = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime);
			float num3 = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime);
			this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, num2, num3);
			this.m_InterpolatingCameraState.UpdateTransform(base.transform);
		}

		// Token: 0x04000B8C RID: 2956
		private DemoCameraController.CameraState m_TargetCameraState = new DemoCameraController.CameraState();

		// Token: 0x04000B8D RID: 2957
		private DemoCameraController.CameraState m_InterpolatingCameraState = new DemoCameraController.CameraState();

		// Token: 0x04000B8E RID: 2958
		[Header("Movement Settings")]
		public float boost = 3.5f;

		// Token: 0x04000B8F RID: 2959
		public float positionLerpTime = 0.2f;

		// Token: 0x04000B90 RID: 2960
		[Header("Rotation Settings")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04000B91 RID: 2961
		public float rotationLerpTime = 0.01f;

		// Token: 0x04000B92 RID: 2962
		public bool invertY;

		// Token: 0x02000174 RID: 372
		private class CameraState
		{
			// Token: 0x06000BC5 RID: 3013 RVA: 0x0005145C File Offset: 0x0004F65C
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x06000BC6 RID: 3014 RVA: 0x000514D0 File Offset: 0x0004F6D0
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x06000BC7 RID: 3015 RVA: 0x00051534 File Offset: 0x0004F734
			public void LerpTowards(DemoCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x06000BC8 RID: 3016 RVA: 0x000515D1 File Offset: 0x0004F7D1
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04000B93 RID: 2963
			public float yaw;

			// Token: 0x04000B94 RID: 2964
			public float pitch;

			// Token: 0x04000B95 RID: 2965
			public float roll;

			// Token: 0x04000B96 RID: 2966
			public float x;

			// Token: 0x04000B97 RID: 2967
			public float y;

			// Token: 0x04000B98 RID: 2968
			public float z;
		}
	}
}
