using System;
using System.Collections;
using UnityEngine;

namespace Invector.vCamera
{
	// Token: 0x0200063E RID: 1598
	public class vChangeCameraAngleTrigger : MonoBehaviour
	{
		// Token: 0x060024C3 RID: 9411 RVA: 0x000CEED4 File Offset: 0x000CD0D4
		private void OnDrawGizmos()
		{
			if (this.useSelfWorldAngle)
			{
				this.angle.x = base.transform.eulerAngles.y;
				this.angle.y = base.transform.eulerAngles.x;
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000CEF14 File Offset: 0x000CD114
		private IEnumerator Start()
		{
			this.tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			Collider collider = base.GetComponent<Collider>();
			if (collider)
			{
				collider.isTrigger = true;
				collider.enabled = false;
				yield return new WaitForEndOfFrame();
				collider.enabled = true;
			}
			yield break;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000CEF24 File Offset: 0x000CD124
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player") && this.tpCamera)
			{
				if (this.applyX)
				{
					this.tpCamera.lerpState.fixedAngle.x = this.angle.x;
				}
				if (this.applyY)
				{
					this.tpCamera.lerpState.fixedAngle.y = this.angle.y;
				}
			}
		}

		// Token: 0x04002486 RID: 9350
		public bool applyY;

		// Token: 0x04002487 RID: 9351
		public bool applyX;

		// Token: 0x04002488 RID: 9352
		public Vector2 angle;

		// Token: 0x04002489 RID: 9353
		public vThirdPersonCamera tpCamera;

		// Token: 0x0400248A RID: 9354
		public bool useSelfWorldAngle;
	}
}
