using System;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x02000425 RID: 1061
	public class MouseLook : MonoBehaviour
	{
		// Token: 0x06001ADE RID: 6878 RVA: 0x000A8509 File Offset: 0x000A6709
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x000A8514 File Offset: 0x000A6714
		private void Update()
		{
			float num = Input.GetAxis("Mouse X") * this.mouseXSensitivity * Time.deltaTime;
			float num2 = Input.GetAxis("Mouse Y") * this.mouseXSensitivity * Time.deltaTime;
			this.xRotation -= num2;
			this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
			base.transform.localRotation = Quaternion.Euler(this.xRotation, 0f, 0f);
			this.playerBody.Rotate(Vector3.up * num);
		}

		// Token: 0x04001BCA RID: 7114
		public float mouseXSensitivity = 100f;

		// Token: 0x04001BCB RID: 7115
		public Transform playerBody;

		// Token: 0x04001BCC RID: 7116
		private float xRotation;
	}
}
