using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AFC RID: 2812
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-10000)]
	public sealed class CustomizationPreviewPhysicsGuard : MonoBehaviour
	{
		// Token: 0x06004FA3 RID: 20387 RVA: 0x001EDD19 File Offset: 0x001EBF19
		private void Awake()
		{
			this.CaptureAnchor();
			this.Stabilize();
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x001EDD19 File Offset: 0x001EBF19
		private void OnEnable()
		{
			this.CaptureAnchor();
			this.Stabilize();
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001EDD27 File Offset: 0x001EBF27
		private void FixedUpdate()
		{
			if (this.targetBody != null && (!this.targetBody.isKinematic || this.targetBody.useGravity))
			{
				this.Stabilize();
			}
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x001EDD58 File Offset: 0x001EBF58
		public void Stabilize()
		{
			if (this.targetBody == null)
			{
				this.targetBody = base.GetComponent<Rigidbody>();
			}
			if (this.targetBody != null)
			{
				if (!this.targetBody.isKinematic)
				{
					this.targetBody.velocity = Vector3.zero;
					this.targetBody.angularVelocity = Vector3.zero;
				}
				this.targetBody.useGravity = false;
				this.targetBody.isKinematic = true;
				this.targetBody.constraints = RigidbodyConstraints.FreezeAll;
			}
			if (this.keepLocalPositionAtOrigin)
			{
				base.transform.localPosition = this.anchoredLocalPosition;
			}
			if (this._hasCapturedRotation)
			{
				base.transform.localRotation = this._anchoredLocalRotation;
			}
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x001EDE11 File Offset: 0x001EC011
		private void CaptureAnchor()
		{
			if (this._hasCapturedRotation)
			{
				return;
			}
			this._anchoredLocalRotation = base.transform.localRotation;
			this._hasCapturedRotation = true;
		}

		// Token: 0x04005099 RID: 20633
		[SerializeField]
		private Rigidbody targetBody;

		// Token: 0x0400509A RID: 20634
		[SerializeField]
		private bool keepLocalPositionAtOrigin = true;

		// Token: 0x0400509B RID: 20635
		[SerializeField]
		private Vector3 anchoredLocalPosition = Vector3.zero;

		// Token: 0x0400509C RID: 20636
		private Quaternion _anchoredLocalRotation;

		// Token: 0x0400509D RID: 20637
		private bool _hasCapturedRotation;
	}
}
