using System;
using System.Collections;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace PoolWaterTools
{
	// Token: 0x02000724 RID: 1828
	[DisallowMultipleComponent]
	public class PoolEntryStateGuard : MonoBehaviour
	{
		// Token: 0x06002F59 RID: 12121 RVA: 0x0010B864 File Offset: 0x00109A64
		private void Awake()
		{
			if (this.swimming == null)
			{
				this.swimming = base.GetComponentInParent<vSwimming>();
			}
			if (this.swimming == null)
			{
				this.swimming = base.GetComponentInChildren<vSwimming>();
			}
			if (this.controller == null)
			{
				this.controller = base.GetComponentInParent<vThirdPersonController>();
			}
			if (this.controller == null && this.swimming != null)
			{
				this.controller = this.swimming.GetComponentInParent<vThirdPersonController>();
			}
			if (this.controller != null)
			{
				this._rb = ((this.controller._rigidbody != null) ? this.controller._rigidbody : base.GetComponent<Rigidbody>());
				this._capsule = ((this.controller._capsuleCollider != null) ? this.controller._capsuleCollider : base.GetComponent<CapsuleCollider>());
				this._genericAction = this.controller.GetComponent<vGenericAction>();
			}
			else
			{
				this._rb = base.GetComponent<Rigidbody>();
				this._capsule = base.GetComponent<CapsuleCollider>();
			}
			if (this.swimming == null)
			{
				Debug.LogWarning("[PoolEntryStateGuard] vSwimming not found on " + base.name + " - disabling.");
				base.enabled = false;
			}
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x0010B9AC File Offset: 0x00109BAC
		private void Update()
		{
			if (this.swimming == null)
			{
				return;
			}
			bool inTheWater = this.swimming.inTheWater;
			if (inTheWater && !this._wasInWater)
			{
				this.CaptureSnapshot("inTheWater rising edge");
			}
			if (!inTheWater && this._wasInWater)
			{
				if (this.ShouldWaitForExitAction(4f))
				{
					if (!this._restorePending)
					{
						this._restorePending = true;
						base.StartCoroutine(this.RestoreAfterCustomAction());
					}
				}
				else
				{
					this.RestoreSnapshot("inTheWater falling edge (no custom action)");
				}
			}
			this._wasInWater = inTheWater;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x0010BA32 File Offset: 0x00109C32
		private IEnumerator RestoreAfterCustomAction()
		{
			float timeout = 4f;
			for (;;)
			{
				if (!this.ShouldWaitForExitAction(timeout))
				{
					yield return null;
					yield return null;
					if (!this.ShouldWaitForExitAction(timeout))
					{
						break;
					}
				}
				else
				{
					if (this._genericAction == null || !this._genericAction.doingAction)
					{
						timeout -= Time.deltaTime;
					}
					yield return null;
				}
			}
			this._restorePending = false;
			if (this.swimming == null || !this.swimming.inTheWater)
			{
				this.RestoreSnapshot("after climb custom action");
			}
			yield break;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x0010BA44 File Offset: 0x00109C44
		private bool ShouldWaitForExitAction(float remainingTagTimeout)
		{
			return (this._genericAction != null && this._genericAction.doingAction) || (this.controller != null && this.controller.customAction && remainingTagTimeout > 0f);
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x0010BA93 File Offset: 0x00109C93
		public void NotifyDiveStarting()
		{
			this.CaptureSnapshot("dive starting");
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06002F5E RID: 12126 RVA: 0x0010BAA0 File Offset: 0x00109CA0
		public bool HasPendingWaterRestore
		{
			get
			{
				return this._captured || this._restorePending;
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x0010BAB2 File Offset: 0x00109CB2
		public void RestoreForEncounter()
		{
			base.StopAllCoroutines();
			this._restorePending = false;
			this.RestoreSnapshot("encounter recovery");
			this._wasInWater = false;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x0010BAD4 File Offset: 0x00109CD4
		public void CaptureSnapshot(string reason)
		{
			if (this._captured)
			{
				return;
			}
			if (this._rb != null)
			{
				this._rbKinematic = this._rb.isKinematic;
				this._rbUseGravity = this._rb.useGravity;
				this._rbDrag = this._rb.drag;
				this._rbAngularDrag = this._rb.angularDrag;
				this._rbConstraints = this._rb.constraints;
				this._rbInterpolation = this._rb.interpolation;
				this._rbCollisionDetection = this._rb.collisionDetectionMode;
				this._rbInterpolateSet = true;
			}
			if (this._capsule != null)
			{
				this._capIsTrigger = this._capsule.isTrigger;
				this._capRadius = this._capsule.radius;
				this._capHeight = this._capsule.height;
				this._capCenter = this._capsule.center;
				this._capCaptured = true;
			}
			if (this.controller != null)
			{
				this._ctrlMoveSpeed = this.controller.moveSpeed;
				this._ctrlDisableCheckGround = this.controller.disableCheckGround;
				this._ctrlDisableSnapToGround = this.controller.disableSnapToGround;
				this._ctrlLockSetMoveSpeed = this.controller.lockSetMoveSpeed;
				this._ctrlCustomAction = this.controller.customAction;
				this._ctrlCaptured = true;
			}
			this._captured = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[PoolEntryStateGuard] Snapshot captured (",
					reason,
					"): ",
					string.Format("rbKinematic={0} useGravity={1} drag={2} | ", this._rbKinematic, this._rbUseGravity, this._rbDrag),
					string.Format("capR={0} capH={1} | moveSpeed={2} | ", this._capRadius, this._capHeight, this._ctrlMoveSpeed),
					string.Format("customAction={0} disableCheckGround={1} ", this._ctrlCustomAction, this._ctrlDisableCheckGround),
					string.Format("disableSnapToGround={0} lockSetMoveSpeed={1}", this._ctrlDisableSnapToGround, this._ctrlLockSetMoveSpeed)
				}), this);
			}
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x0010BD14 File Offset: 0x00109F14
		public void RestoreSnapshot(string reason)
		{
			if (!this._captured)
			{
				return;
			}
			if (this._rb != null && this._rbInterpolateSet)
			{
				if (!this._rb.isKinematic)
				{
					this._rb.velocity = Vector3.zero;
				}
				this._rb.isKinematic = this._rbKinematic;
				this._rb.useGravity = this._rbUseGravity;
				this._rb.drag = this._rbDrag;
				this._rb.angularDrag = this._rbAngularDrag;
				this._rb.constraints = this._rbConstraints;
				this._rb.interpolation = this._rbInterpolation;
				this._rb.collisionDetectionMode = this._rbCollisionDetection;
				if (!this._rb.isKinematic)
				{
					this._rb.velocity = Vector3.zero;
				}
			}
			if (this._capsule != null && this._capCaptured)
			{
				this._capsule.isTrigger = this._capIsTrigger;
				this._capsule.radius = this._capRadius;
				this._capsule.height = this._capHeight;
				this._capsule.center = this._capCenter;
			}
			if (this.controller != null && this._ctrlCaptured)
			{
				this.controller.disableCheckGround = this._ctrlDisableCheckGround;
				this.controller.disableSnapToGround = this._ctrlDisableSnapToGround;
				this.controller.lockSetMoveSpeed = this._ctrlLockSetMoveSpeed;
				this.controller.customAction = this._ctrlCustomAction;
				this.controller.moveSpeed = this._ctrlMoveSpeed;
				if (this._capCaptured)
				{
					this.controller.colliderRadius = this._capRadius;
					this.controller.colliderHeight = this._capHeight;
					this.controller.colliderCenter = this._capCenter;
				}
			}
			this._captured = false;
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[PoolEntryStateGuard] Snapshot restored (",
					reason,
					"): ",
					string.Format("restored customAction={0} kinematic={1} ", this._ctrlCustomAction, this._rbKinematic),
					string.Format("useGravity={0} | controller.customAction(now)={1}", this._rbUseGravity, (this.controller != null) ? this.controller.customAction.ToString() : "n/a")
				}), this);
			}
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x0010BF94 File Offset: 0x0010A194
		private void OnDisable()
		{
			this.RestoreForEncounter();
		}

		// Token: 0x04002FCB RID: 12235
		[Header("Refs (auto-resolved)")]
		[SerializeField]
		private vSwimming swimming;

		// Token: 0x04002FCC RID: 12236
		[SerializeField]
		private vThirdPersonController controller;

		// Token: 0x04002FCD RID: 12237
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs = true;

		// Token: 0x04002FCE RID: 12238
		private bool _captured;

		// Token: 0x04002FCF RID: 12239
		private bool _wasInWater;

		// Token: 0x04002FD0 RID: 12240
		private bool _rbKinematic;

		// Token: 0x04002FD1 RID: 12241
		private bool _rbUseGravity;

		// Token: 0x04002FD2 RID: 12242
		private bool _rbInterpolateSet;

		// Token: 0x04002FD3 RID: 12243
		private float _rbDrag;

		// Token: 0x04002FD4 RID: 12244
		private float _rbAngularDrag;

		// Token: 0x04002FD5 RID: 12245
		private RigidbodyConstraints _rbConstraints;

		// Token: 0x04002FD6 RID: 12246
		private RigidbodyInterpolation _rbInterpolation;

		// Token: 0x04002FD7 RID: 12247
		private CollisionDetectionMode _rbCollisionDetection;

		// Token: 0x04002FD8 RID: 12248
		private bool _capCaptured;

		// Token: 0x04002FD9 RID: 12249
		private bool _capIsTrigger;

		// Token: 0x04002FDA RID: 12250
		private float _capRadius;

		// Token: 0x04002FDB RID: 12251
		private float _capHeight;

		// Token: 0x04002FDC RID: 12252
		private Vector3 _capCenter;

		// Token: 0x04002FDD RID: 12253
		private bool _ctrlCaptured;

		// Token: 0x04002FDE RID: 12254
		private float _ctrlMoveSpeed;

		// Token: 0x04002FDF RID: 12255
		private bool _ctrlDisableCheckGround;

		// Token: 0x04002FE0 RID: 12256
		private bool _ctrlDisableSnapToGround;

		// Token: 0x04002FE1 RID: 12257
		private bool _ctrlLockSetMoveSpeed;

		// Token: 0x04002FE2 RID: 12258
		private bool _ctrlCustomAction;

		// Token: 0x04002FE3 RID: 12259
		private Rigidbody _rb;

		// Token: 0x04002FE4 RID: 12260
		private CapsuleCollider _capsule;

		// Token: 0x04002FE5 RID: 12261
		private vGenericAction _genericAction;

		// Token: 0x04002FE6 RID: 12262
		private bool _restorePending;
	}
}
