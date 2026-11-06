using System;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x02000426 RID: 1062
	public class PlayerMovement : MonoBehaviour
	{
		// Token: 0x06001AE1 RID: 6881 RVA: 0x000A85C4 File Offset: 0x000A67C4
		private void Update()
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			Vector3 vector = base.transform.right * axis + base.transform.forward * axis2;
			this.controller.Move(vector * this.speed * Time.deltaTime);
			this.velocity.y = this.velocity.y + this.gravity * Time.deltaTime;
			this.controller.Move(this.velocity * Time.deltaTime);
		}

		// Token: 0x04001BCD RID: 7117
		public global::UnityEngine.CharacterController controller;

		// Token: 0x04001BCE RID: 7118
		public float speed = 5f;

		// Token: 0x04001BCF RID: 7119
		public float gravity = -15f;

		// Token: 0x04001BD0 RID: 7120
		private Vector3 velocity;

		// Token: 0x04001BD1 RID: 7121
		private bool isGrounded;
	}
}
