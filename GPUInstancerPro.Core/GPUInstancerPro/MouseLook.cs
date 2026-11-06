using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000042 RID: 66
	public class MouseLook
	{
		// Token: 0x06000269 RID: 617 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
		public void Init(Transform character, Transform camera)
		{
			this.m_CharacterTargetRot = character.localRotation;
			this.m_CameraTargetRot = camera.localRotation;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D514 File Offset: 0x0000B714
		public void LookRotation(GPUIInputHandler inputHandler, Transform character, Transform camera)
		{
			if (this.m_cursorIsLocked)
			{
				float num = inputHandler.GetAxis("Mouse X") * this.XSensitivity;
				float num2 = inputHandler.GetAxis("Mouse Y") * this.YSensitivity;
				this.m_CharacterTargetRot *= Quaternion.Euler(0f, num, 0f);
				this.m_CameraTargetRot *= Quaternion.Euler(-num2, 0f, 0f);
				if (this.clampVerticalRotation)
				{
					this.m_CameraTargetRot = this.ClampRotationAroundXAxis(this.m_CameraTargetRot);
				}
				if (this.smooth)
				{
					character.localRotation = Quaternion.Slerp(character.localRotation, this.m_CharacterTargetRot, this.smoothTime * Time.deltaTime);
					camera.localRotation = Quaternion.Slerp(camera.localRotation, this.m_CameraTargetRot, this.smoothTime * Time.deltaTime);
				}
				else
				{
					character.localRotation = this.m_CharacterTargetRot;
					camera.localRotation = this.m_CameraTargetRot;
				}
			}
			this.UpdateCursorLock(inputHandler);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D61E File Offset: 0x0000B81E
		public void SetCursorLock(bool value)
		{
			this.lockCursor = value;
			if (!this.lockCursor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D63B File Offset: 0x0000B83B
		public void UpdateCursorLock(GPUIInputHandler inputHandler)
		{
			if (this.lockCursor)
			{
				this.InternalLockUpdate(inputHandler);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D64C File Offset: 0x0000B84C
		private void InternalLockUpdate(GPUIInputHandler inputHandler)
		{
			if (inputHandler.GetKeyUp(KeyCode.Escape))
			{
				this.m_cursorIsLocked = false;
			}
			else if (inputHandler.GetMouseButtonUp(0))
			{
				this.m_cursorIsLocked = true;
			}
			if (this.m_cursorIsLocked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				return;
			}
			if (!this.m_cursorIsLocked)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		private Quaternion ClampRotationAroundXAxis(Quaternion q)
		{
			q.x /= q.w;
			q.y /= q.w;
			q.z /= q.w;
			q.w = 1f;
			float num = 114.59156f * Mathf.Atan(q.x);
			num = Mathf.Clamp(num, this.MinimumX, this.MaximumX);
			q.x = Mathf.Tan(0.008726646f * num);
			return q;
		}

		// Token: 0x04000194 RID: 404
		public float XSensitivity = 2f;

		// Token: 0x04000195 RID: 405
		public float YSensitivity = 2f;

		// Token: 0x04000196 RID: 406
		public bool clampVerticalRotation = true;

		// Token: 0x04000197 RID: 407
		public float MinimumX = -90f;

		// Token: 0x04000198 RID: 408
		public float MaximumX = 90f;

		// Token: 0x04000199 RID: 409
		public bool smooth;

		// Token: 0x0400019A RID: 410
		public float smoothTime = 5f;

		// Token: 0x0400019B RID: 411
		public bool lockCursor = true;

		// Token: 0x0400019C RID: 412
		private Quaternion m_CharacterTargetRot;

		// Token: 0x0400019D RID: 413
		private Quaternion m_CameraTargetRot;

		// Token: 0x0400019E RID: 414
		private bool m_cursorIsLocked = true;
	}
}
