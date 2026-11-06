using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200045D RID: 1117
	public class vFootStepTrigger : MonoBehaviour
	{
		// Token: 0x06001BD1 RID: 7121 RVA: 0x000AEECC File Offset: 0x000AD0CC
		private void OnDrawGizmos()
		{
			if (!this.trigger)
			{
				return;
			}
			Color green = Color.green;
			green.a = 0.5f;
			Gizmos.color = green;
			if (this.trigger is SphereCollider)
			{
				Gizmos.DrawSphere(this.trigger.bounds.center, (this.trigger as SphereCollider).radius);
			}
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000AEF34 File Offset: 0x000AD134
		private void Start()
		{
			this._fT = base.GetComponentInParent<vFootStepBase>();
			Rigidbody component = base.gameObject.GetComponent<Rigidbody>();
			if (component == null)
			{
				base.gameObject.AddComponent<Rigidbody>().isKinematic = true;
			}
			else
			{
				component.isKinematic = true;
			}
			if (this._fT == null)
			{
				Debug.Log(base.gameObject.name + " can't find the FootStepFromTexture");
				base.gameObject.SetActive(false);
				return;
			}
			foreach (Collider collider in this._fT.gameObject.GetComponentsInChildren<Collider>(true))
			{
				if (collider != null && collider.gameObject != this.trigger.gameObject)
				{
					Physics.IgnoreCollision(collider, this.trigger, true);
				}
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000AF003 File Offset: 0x000AD203
		public Collider trigger
		{
			get
			{
				if (this._trigger == null)
				{
					this._trigger = base.gameObject.GetComponent<Collider>();
				}
				return this._trigger;
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000AF02C File Offset: 0x000AD22C
		private void OnTriggerEnter(Collider other)
		{
			if (this._fT == null)
			{
				return;
			}
			if (this.lastCollider == null || this.lastCollider != other || this.footstepObj == null)
			{
				this.footstepObj = new FootStepObject(base.transform, other);
				this.lastCollider = other;
			}
			if (this.footstepObj.isTerrain)
			{
				this._fT.StepOnTerrain(this.footstepObj);
				this.OnStep.Invoke();
				return;
			}
			this._fT.StepOnMesh(this.footstepObj);
			this.OnStep.Invoke();
		}

		// Token: 0x04001D11 RID: 7441
		protected Collider _trigger;

		// Token: 0x04001D12 RID: 7442
		protected vFootStepBase _fT;

		// Token: 0x04001D13 RID: 7443
		public UnityEvent OnStep;

		// Token: 0x04001D14 RID: 7444
		protected Collider lastCollider;

		// Token: 0x04001D15 RID: 7445
		internal FootStepObject footstepObj;
	}
}
