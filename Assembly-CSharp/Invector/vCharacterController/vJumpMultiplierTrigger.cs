using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006AC RID: 1708
	public class vJumpMultiplierTrigger : MonoBehaviour
	{
		// Token: 0x0600298B RID: 10635 RVA: 0x000E9CD0 File Offset: 0x000E7ED0
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				vThirdPersonController component = other.GetComponent<vThirdPersonController>();
				if (component && (component.isJumping || !component.isGrounded) && component._rigidbody.velocity.y <= 0f)
				{
					component.SetJumpMultiplier(this.multiplier, this.timeToReset);
					component.isJumping = false;
					component.verticalVelocity = 0f;
					component.heightReached = base.transform.position.y;
					component.isGrounded = true;
					component.Jump(false);
				}
			}
		}

		// Token: 0x040029AF RID: 10671
		public float multiplier = 5f;

		// Token: 0x040029B0 RID: 10672
		public float timeToReset = 0.5f;
	}
}
