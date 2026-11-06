using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006B0 RID: 1712
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class vHeadTrackSensor : MonoBehaviour
	{
		// Token: 0x060029F7 RID: 10743 RVA: 0x000EBD74 File Offset: 0x000E9F74
		private void OnDrawGizmos()
		{
			if (Application.isPlaying && this.sphere && this.headTrack)
			{
				this.sphere.radius = this.headTrack.distanceToDetect;
			}
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000EBDB0 File Offset: 0x000E9FB0
		private void Start()
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			this.sphere = base.GetComponent<SphereCollider>();
			this.sphere.isTrigger = true;
			component.useGravity = false;
			component.isKinematic = true;
			component.constraints = RigidbodyConstraints.FreezeAll;
			if (this.headTrack)
			{
				this.sphere.radius = this.headTrack.distanceToDetect;
			}
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x000EBE13 File Offset: 0x000EA013
		private void OnTriggerEnter(Collider other)
		{
			if (this.headTrack != null)
			{
				this.headTrack.OnDetect(other);
			}
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000EBE2F File Offset: 0x000EA02F
		private void OnTriggerExit(Collider other)
		{
			if (this.headTrack != null)
			{
				this.headTrack.OnLost(other);
			}
		}

		// Token: 0x04002A07 RID: 10759
		[HideInInspector]
		public vHeadTrack headTrack;

		// Token: 0x04002A08 RID: 10760
		public SphereCollider sphere;
	}
}
