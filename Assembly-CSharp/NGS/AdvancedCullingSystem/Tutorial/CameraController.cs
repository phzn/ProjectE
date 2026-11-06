using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Tutorial
{
	// Token: 0x0200042E RID: 1070
	public class CameraController : MonoBehaviour
	{
		// Token: 0x06001B08 RID: 6920 RVA: 0x000A8A79 File Offset: 0x000A6C79
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			this._currentHeight = base.transform.parent.position.y;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000A8A9C File Offset: 0x000A6C9C
		private void Update()
		{
			float num = Input.GetAxis("Mouse X") * this.mouseSensitivity * 100f * Time.deltaTime;
			float num2 = Input.GetAxis("Mouse Y") * this.mouseSensitivity * 100f * Time.deltaTime;
			num = Mathf.Clamp(num, -10f, 10f);
			num2 = Mathf.Clamp(num2, -10f, 10f);
			this.Rotate(num, num2);
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			Vector3 vector = this.CalculateMovement(axis, axis2);
			Vector3 vector2 = base.transform.parent.position;
			this._currentHeight = Mathf.Lerp(this._currentHeight, this.GetHeight(), this.movementSpeed * Time.deltaTime);
			vector2 += vector * this.movementSpeed * Time.deltaTime;
			vector2.y = this._currentHeight;
			this.MoveToPoint(vector2);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000A8B9C File Offset: 0x000A6D9C
		private void Rotate(float mouseX, float mouseY)
		{
			this._xRotation -= mouseY;
			this._xRotation = Mathf.Clamp(this._xRotation, -90f, 90f);
			base.transform.localRotation = Quaternion.Euler(this._xRotation, 0f, 0f);
			base.transform.parent.Rotate(Vector3.up * mouseX);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000A8C10 File Offset: 0x000A6E10
		private Vector3 CalculateMovement(float horizontal, float vertical)
		{
			Vector3 vector = base.transform.right * horizontal + base.transform.forward * vertical;
			vector.y = 0f;
			return vector.normalized;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000A8C58 File Offset: 0x000A6E58
		private void MoveToPoint(Vector3 targetPoint)
		{
			Vector3 position = base.transform.parent.position;
			position.y = targetPoint.y;
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(position, targetPoint - position), out raycastHit, 3f))
			{
				return;
			}
			base.transform.parent.position = targetPoint;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000A8CB0 File Offset: 0x000A6EB0
		private float GetHeight()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(base.transform.parent.position + Vector3.up * 20f, -base.transform.parent.up), out raycastHit))
			{
				return raycastHit.point.y + this._height;
			}
			return this._height;
		}

		// Token: 0x04001BE4 RID: 7140
		[SerializeField]
		private float mouseSensitivity = 5f;

		// Token: 0x04001BE5 RID: 7141
		[SerializeField]
		private float movementSpeed = 5f;

		// Token: 0x04001BE6 RID: 7142
		[SerializeField]
		private float _height = 5f;

		// Token: 0x04001BE7 RID: 7143
		private float _xRotation;

		// Token: 0x04001BE8 RID: 7144
		private float _currentHeight;
	}
}
