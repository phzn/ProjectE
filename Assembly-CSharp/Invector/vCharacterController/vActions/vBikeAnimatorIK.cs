using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000701 RID: 1793
	public class vBikeAnimatorIK : MonoBehaviour
	{
		// Token: 0x06002DEE RID: 11758 RVA: 0x00101ECF File Offset: 0x001000CF
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x00101EDD File Offset: 0x001000DD
		public void EnableIK(Transform leftHand, Transform rightHand, Transform leftFoot, Transform rightFoot, Transform lookAt = null)
		{
			this.leftHandTarget = leftHand;
			this.rightHandTarget = rightHand;
			this.leftFootTarget = leftFoot;
			this.rightFootTarget = rightFoot;
			this.lookAtTarget = lookAt;
			this.ikActive = true;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x00101F0B File Offset: 0x0010010B
		public void DisableIK()
		{
			this.ikActive = false;
			this.leftHandTarget = null;
			this.rightHandTarget = null;
			this.leftFootTarget = null;
			this.rightFootTarget = null;
			this.lookAtTarget = null;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x00101F38 File Offset: 0x00100138
		private void OnAnimatorIK(int layerIndex)
		{
			if (this.animator == null || !this.ikActive)
			{
				return;
			}
			if (this.leftHandTarget != null)
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, this.leftHandWeight);
				this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, this.leftHandWeight);
				this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.leftHandTarget.position);
				this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.leftHandTarget.rotation * Quaternion.Euler(this.leftHandRotationOffset));
			}
			if (this.rightHandTarget != null)
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, this.rightHandWeight);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, this.rightHandWeight);
				this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.rightHandTarget.position);
				this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.rightHandTarget.rotation * Quaternion.Euler(this.rightHandRotationOffset));
			}
			if (this.leftFootTarget != null)
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, this.leftFootWeight);
				this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, this.leftFootWeight);
				this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, this.leftFootTarget.position);
				this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, this.leftFootTarget.rotation);
			}
			if (this.rightFootTarget != null)
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, this.rightFootWeight);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, this.rightFootWeight);
				this.animator.SetIKPosition(AvatarIKGoal.RightFoot, this.rightFootTarget.position);
				this.animator.SetIKRotation(AvatarIKGoal.RightFoot, this.rightFootTarget.rotation);
			}
			if (this.lookAtTarget != null)
			{
				this.animator.SetLookAtWeight(1f);
				this.animator.SetLookAtPosition(this.lookAtTarget.position);
			}
		}

		// Token: 0x04002E27 RID: 11815
		[Header("IK Settings")]
		[Range(0f, 1f)]
		public float leftHandWeight = 1f;

		// Token: 0x04002E28 RID: 11816
		[Range(0f, 1f)]
		public float rightHandWeight = 1f;

		// Token: 0x04002E29 RID: 11817
		[Range(0f, 1f)]
		public float leftFootWeight = 1f;

		// Token: 0x04002E2A RID: 11818
		[Range(0f, 1f)]
		public float rightFootWeight = 1f;

		// Token: 0x04002E2B RID: 11819
		[Header("IK Targets (Set by vBikeController)")]
		public Transform leftHandTarget;

		// Token: 0x04002E2C RID: 11820
		public Transform rightHandTarget;

		// Token: 0x04002E2D RID: 11821
		public Transform leftFootTarget;

		// Token: 0x04002E2E RID: 11822
		public Transform rightFootTarget;

		// Token: 0x04002E2F RID: 11823
		public Transform lookAtTarget;

		// Token: 0x04002E30 RID: 11824
		[Header("Hand Rotation Offsets")]
		public Vector3 leftHandRotationOffset = Vector3.zero;

		// Token: 0x04002E31 RID: 11825
		public Vector3 rightHandRotationOffset = Vector3.zero;

		// Token: 0x04002E32 RID: 11826
		private Animator animator;

		// Token: 0x04002E33 RID: 11827
		private bool ikActive;
	}
}
