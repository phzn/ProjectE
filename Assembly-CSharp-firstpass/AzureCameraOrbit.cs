using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000029 RID: 41
	public class AzureCameraOrbit : MonoBehaviour
	{
		// Token: 0x06000267 RID: 615 RVA: 0x00009FF5 File Offset: 0x000081F5
		private void Start()
		{
			this.m_startRotation = base.transform.eulerAngles;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A008 File Offset: 0x00008208
		private void LateUpdate()
		{
			if (Input.GetMouseButton(1))
			{
				this.m_mouseX += Input.GetAxis("Mouse X") * 2.5f;
				this.m_mouseY -= Input.GetAxis("Mouse Y") * 2.5f;
				base.transform.localRotation = Quaternion.Euler(new Vector3(this.m_mouseY, this.m_mouseX, base.transform.localRotation.z) + this.m_startRotation);
			}
		}

		// Token: 0x040001AB RID: 427
		private float m_mouseX;

		// Token: 0x040001AC RID: 428
		private float m_mouseY;

		// Token: 0x040001AD RID: 429
		private Vector3 m_startRotation;
	}
}
