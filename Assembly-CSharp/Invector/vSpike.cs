using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004C3 RID: 1219
	public class vSpike : MonoBehaviour
	{
		// Token: 0x06001D80 RID: 7552 RVA: 0x000B5547 File Offset: 0x000B3747
		private void Start()
		{
			this.joint = base.GetComponent<HingeJoint>();
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x000B5558 File Offset: 0x000B3758
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.rigidbody != null && collision.collider.GetComponent<vDamageReceiver>() != null && !this.inConnect)
			{
				bool flag = this.control == null || !this.control.attachColliders.Contains(collision.collider.transform);
				if (this.control)
				{
					this.control.attachColliders.Add(collision.collider.transform);
				}
				if (flag)
				{
					this.inConnect = true;
					if (this.joint && collision.rigidbody)
					{
						this.joint.connectedBody = collision.rigidbody;
					}
					this.impaled = collision.transform;
					foreach (Rigidbody rigidbody in collision.transform.root.GetComponentsInChildren<Rigidbody>())
					{
						if (!rigidbody.isKinematic)
						{
							rigidbody.velocity = Vector3.zero;
						}
					}
					vDamageReceiver component = collision.collider.GetComponent<vDamageReceiver>();
					if (component && component.ragdoll && component.ragdoll.iChar != null)
					{
						component.ragdoll.iChar.ChangeHealth((int)(-(int)component.ragdoll.iChar.currentHealth));
					}
				}
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000B56B8 File Offset: 0x000B38B8
		private void OnTriggerExit(Collider other)
		{
			if (other.transform != null && this.impaled != null && other.transform == this.impaled)
			{
				if (this.joint)
				{
					this.joint.connectedBody = null;
				}
				this.impaled = null;
				if (this.control != null && this.control.attachColliders.Contains(this.impaled))
				{
					this.control.attachColliders.Remove(this.impaled);
				}
				this.inConnect = false;
			}
		}

		// Token: 0x04001EAC RID: 7852
		private HingeJoint joint;

		// Token: 0x04001EAD RID: 7853
		[HideInInspector]
		public vSpikeControl control;

		// Token: 0x04001EAE RID: 7854
		private bool inConnect;

		// Token: 0x04001EAF RID: 7855
		private Transform impaled;
	}
}
