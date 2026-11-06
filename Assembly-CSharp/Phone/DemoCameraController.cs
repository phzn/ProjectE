using System;
using UnityEngine;

namespace Phone
{
	// Token: 0x02000432 RID: 1074
	public class DemoCameraController : MonoBehaviour
	{
		// Token: 0x06001B18 RID: 6936 RVA: 0x000A8F33 File Offset: 0x000A7133
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000A8F58 File Offset: 0x000A7158
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

		// Token: 0x06001B1A RID: 6938 RVA: 0x000A8FEC File Offset: 0x000A71EC
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

		// Token: 0x04001BF0 RID: 7152
		private DemoCameraController.CameraState m_TargetCameraState = new DemoCameraController.CameraState();

		// Token: 0x04001BF1 RID: 7153
		private DemoCameraController.CameraState m_InterpolatingCameraState = new DemoCameraController.CameraState();

		// Token: 0x04001BF2 RID: 7154
		[Header("Movement Settings")]
		public float boost = 3.5f;

		// Token: 0x04001BF3 RID: 7155
		public float positionLerpTime = 0.2f;

		// Token: 0x04001BF4 RID: 7156
		[Header("Rotation Settings")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04001BF5 RID: 7157
		public float rotationLerpTime = 0.01f;

		// Token: 0x04001BF6 RID: 7158
		public bool invertY;

		// Token: 0x02000433 RID: 1075
		private class CameraState
		{
			// Token: 0x06001B1C RID: 6940 RVA: 0x000A9214 File Offset: 0x000A7414
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x06001B1D RID: 6941 RVA: 0x000A9288 File Offset: 0x000A7488
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x06001B1E RID: 6942 RVA: 0x000A92EC File Offset: 0x000A74EC
			public void LerpTowards(DemoCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x06001B1F RID: 6943 RVA: 0x000A9389 File Offset: 0x000A7589
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04001BF7 RID: 7159
			public float yaw;

			// Token: 0x04001BF8 RID: 7160
			public float pitch;

			// Token: 0x04001BF9 RID: 7161
			public float roll;

			// Token: 0x04001BFA RID: 7162
			public float x;

			// Token: 0x04001BFB RID: 7163
			public float y;

			// Token: 0x04001BFC RID: 7164
			public float z;
		}
	}
}
