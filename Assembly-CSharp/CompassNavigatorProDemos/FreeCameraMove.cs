using System;
using CompassNavigatorPro;
using UnityEngine;

namespace CompassNavigatorProDemos
{
	// Token: 0x020002B0 RID: 688
	public class FreeCameraMove : MonoBehaviour
	{
		// Token: 0x06001375 RID: 4981 RVA: 0x0007E210 File Offset: 0x0007C410
		private void Update()
		{
			if (!Input.GetMouseButton(1))
			{
				return;
			}
			CompassPro instance = CompassPro.instance;
			if (instance != null && !instance.IsMouseOverMiniMap())
			{
				this.rotationX += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
				base.transform.localRotation = Quaternion.AngleAxis(this.rotationX, Vector3.up);
				this.rotationY += Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
				this.rotationY = Mathf.Clamp(this.rotationY, -45f, 45f);
				base.transform.localRotation *= Quaternion.AngleAxis(this.rotationY, Vector3.left);
			}
			Vector3 position = base.transform.position;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
				base.transform.position += base.transform.right * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
				base.transform.position += base.transform.right * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
			}
			else
			{
				base.transform.position += base.transform.forward * this.normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
				base.transform.position += base.transform.right * this.normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position -= base.transform.up * this.climbSpeed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position += base.transform.up * this.climbSpeed * Time.deltaTime;
			}
			base.transform.position = new Vector3(base.transform.position.x, 1f, base.transform.position.z);
			if (!this.bounds.Contains(base.transform.position))
			{
				base.transform.position = position;
			}
		}

		// Token: 0x04001554 RID: 5460
		public float cameraSensitivity = 150f;

		// Token: 0x04001555 RID: 5461
		public float climbSpeed = 20f;

		// Token: 0x04001556 RID: 5462
		public float normalMoveSpeed = 20f;

		// Token: 0x04001557 RID: 5463
		public float slowMoveFactor = 0.25f;

		// Token: 0x04001558 RID: 5464
		public float fastMoveFactor = 3f;

		// Token: 0x04001559 RID: 5465
		public Bounds bounds;

		// Token: 0x0400155A RID: 5466
		private float rotationX;

		// Token: 0x0400155B RID: 5467
		private float rotationY;
	}
}
