using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000041 RID: 65
	[RequireComponent(typeof(CharacterController))]
	public class GPUIFPController : GPUIInputHandler
	{
		// Token: 0x06000262 RID: 610 RVA: 0x0000D184 File Offset: 0x0000B384
		protected override void Start()
		{
			base.Start();
			this.m_CharacterController = base.GetComponent<CharacterController>();
			this.m_Camera = Camera.main;
			this.m_Jumping = false;
			this.m_MouseLook = new MouseLook();
			this.m_MouseLook.Init(base.transform, this.m_Camera.transform);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D1DC File Offset: 0x0000B3DC
		private void Update()
		{
			this.RotateView();
			if (!this.m_Jump && Cursor.lockState == CursorLockMode.Locked)
			{
				this.m_Jump = base.GetKey(KeyCode.Space);
			}
			if (!this.m_PreviouslyGrounded && this.m_CharacterController.isGrounded)
			{
				this.m_MoveDir.y = 0f;
				this.m_Jumping = false;
			}
			if (!this.m_CharacterController.isGrounded && !this.m_Jumping && this.m_PreviouslyGrounded)
			{
				this.m_MoveDir.y = 0f;
			}
			this.m_PreviouslyGrounded = this.m_CharacterController.isGrounded;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000D278 File Offset: 0x0000B478
		private void FixedUpdate()
		{
			float num;
			this.GetInput(out num);
			Vector3 vector = base.transform.forward * this.m_Input.y + base.transform.right * this.m_Input.x;
			RaycastHit raycastHit;
			Physics.SphereCast(base.transform.position, this.m_CharacterController.radius, Vector3.down, out raycastHit, this.m_CharacterController.height / 2f, -1, QueryTriggerInteraction.Ignore);
			vector = Vector3.ProjectOnPlane(vector, raycastHit.normal).normalized;
			this.m_MoveDir.x = vector.x * num;
			this.m_MoveDir.z = vector.z * num;
			if (this.m_CharacterController.isGrounded)
			{
				this.m_MoveDir.y = -this.m_StickToGroundForce;
				if (this.m_Jump)
				{
					this.m_MoveDir.y = this.m_JumpSpeed;
					this.m_Jump = false;
					this.m_Jumping = true;
				}
			}
			else
			{
				this.m_MoveDir += Physics.gravity * this.m_GravityMultiplier * Time.fixedDeltaTime;
			}
			this.m_CollisionFlags = this.m_CharacterController.Move(this.m_MoveDir * Time.fixedDeltaTime);
			this.m_MouseLook.UpdateCursorLock(this);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000D3DC File Offset: 0x0000B5DC
		private void GetInput(out float speed)
		{
			float axis = base.GetAxis("Horizontal");
			float axis2 = base.GetAxis("Vertical");
			this.m_IsWalking = !base.GetKey(KeyCode.LeftShift);
			speed = (this.m_IsWalking ? this.m_WalkSpeed : this.m_RunSpeed);
			this.m_Input = new Vector2(axis, axis2);
			if (this.m_Input.sqrMagnitude > 1f)
			{
				this.m_Input.Normalize();
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000D457 File Offset: 0x0000B657
		private void RotateView()
		{
			this.m_MouseLook.LookRotation(this, base.transform, this.m_Camera.transform);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000D478 File Offset: 0x0000B678
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
			if (this.m_CollisionFlags == CollisionFlags.Below)
			{
				return;
			}
			if (attachedRigidbody == null || attachedRigidbody.isKinematic)
			{
				return;
			}
			attachedRigidbody.AddForceAtPosition(this.m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
		}

		// Token: 0x04000184 RID: 388
		[SerializeField]
		public float m_WalkSpeed;

		// Token: 0x04000185 RID: 389
		[SerializeField]
		public float m_RunSpeed;

		// Token: 0x04000186 RID: 390
		[SerializeField]
		public float m_JumpSpeed;

		// Token: 0x04000187 RID: 391
		private bool m_IsWalking;

		// Token: 0x04000188 RID: 392
		private MouseLook m_MouseLook;

		// Token: 0x04000189 RID: 393
		private Camera m_Camera;

		// Token: 0x0400018A RID: 394
		private bool m_Jump;

		// Token: 0x0400018B RID: 395
		private float m_YRotation;

		// Token: 0x0400018C RID: 396
		private Vector2 m_Input;

		// Token: 0x0400018D RID: 397
		private Vector3 m_MoveDir = Vector3.zero;

		// Token: 0x0400018E RID: 398
		private CharacterController m_CharacterController;

		// Token: 0x0400018F RID: 399
		private CollisionFlags m_CollisionFlags;

		// Token: 0x04000190 RID: 400
		private bool m_PreviouslyGrounded;

		// Token: 0x04000191 RID: 401
		private bool m_Jumping;

		// Token: 0x04000192 RID: 402
		private float m_StickToGroundForce = 10f;

		// Token: 0x04000193 RID: 403
		private float m_GravityMultiplier = 2f;
	}
}
