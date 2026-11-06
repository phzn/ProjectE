using System;
using UnityEngine;

namespace UnityTemplateProject
{
	// Token: 0x02000179 RID: 377
	public class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x06000BD9 RID: 3033 RVA: 0x000523D6 File Offset: 0x000505D6
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000523FC File Offset: 0x000505FC
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

		// Token: 0x06000BDB RID: 3035 RVA: 0x00052490 File Offset: 0x00050690
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

		// Token: 0x04000BDA RID: 3034
		private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000BDB RID: 3035
		private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000BDC RID: 3036
		[Header("Movement Settings")]
		[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
		public float boost = 3.5f;

		// Token: 0x04000BDD RID: 3037
		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float positionLerpTime = 0.2f;

		// Token: 0x04000BDE RID: 3038
		[Header("Rotation Settings")]
		[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04000BDF RID: 3039
		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float rotationLerpTime = 0.01f;

		// Token: 0x04000BE0 RID: 3040
		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool invertY;

		// Token: 0x0200017A RID: 378
		private class CameraState
		{
			// Token: 0x06000BDD RID: 3037 RVA: 0x000526B8 File Offset: 0x000508B8
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x06000BDE RID: 3038 RVA: 0x0005272C File Offset: 0x0005092C
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x06000BDF RID: 3039 RVA: 0x00052790 File Offset: 0x00050990
			public void LerpTowards(SimpleCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x06000BE0 RID: 3040 RVA: 0x0005282D File Offset: 0x00050A2D
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04000BE1 RID: 3041
			public float yaw;

			// Token: 0x04000BE2 RID: 3042
			public float pitch;

			// Token: 0x04000BE3 RID: 3043
			public float roll;

			// Token: 0x04000BE4 RID: 3044
			public float x;

			// Token: 0x04000BE5 RID: 3045
			public float y;

			// Token: 0x04000BE6 RID: 3046
			public float z;
		}
	}
}
