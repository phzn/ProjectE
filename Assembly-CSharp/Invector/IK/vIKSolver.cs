using System;
using UnityEngine;

namespace Invector.IK
{
	// Token: 0x0200051F RID: 1311
	[Serializable]
	public class vIKSolver
	{
		// Token: 0x06001FD0 RID: 8144 RVA: 0x000BCDE2 File Offset: 0x000BAFE2
		public vIKSolver(Transform rootTransform, Transform rootBone, Transform middleBone, Transform endBone)
		{
			this.rootTransform = rootTransform;
			this.rootBone = rootBone;
			this.middleBone = middleBone;
			this.endBone = endBone;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000BCE08 File Offset: 0x000BB008
		public vIKSolver(Animator animator, AvatarIKGoal ikGoal)
		{
			if (animator == null)
			{
				return;
			}
			this.rootTransform = animator.transform;
			if (animator.isHuman)
			{
				switch (ikGoal)
				{
				case AvatarIKGoal.LeftFoot:
					this.rootBone = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
					this.middleBone = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
					this.endBone = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
					this.endTag = "LeftFoot";
					this.middleTag = "LeftHint";
					break;
				case AvatarIKGoal.RightFoot:
					this.rootBone = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
					this.middleBone = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
					this.endBone = animator.GetBoneTransform(HumanBodyBones.RightFoot);
					this.endTag = "RightFoot";
					this.middleTag = "RightHint";
					break;
				case AvatarIKGoal.LeftHand:
					this.rootBone = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
					this.middleBone = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
					this.endBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);
					this.endTag = "LeftHand";
					this.middleTag = "LeftHint";
					break;
				case AvatarIKGoal.RightHand:
					this.rootBone = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
					this.middleBone = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
					this.endBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
					this.endTag = "RightHand";
					this.middleTag = "RightHint";
					break;
				}
			}
			this.CreateBones();
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x000BCF60 File Offset: 0x000BB160
		public bool isValidBones
		{
			get
			{
				return this.rootBone && this.middleBone && this.endBone && this.endBoneRef && this.middleBoneRef && this.endBoneOffset && this.middleBoneOffset;
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000BCFC8 File Offset: 0x000BB1C8
		private void CreateBones()
		{
			if (this.rootTransform && this.rootBone && this.middleBone && this.endBone)
			{
				if (!this.endBoneRef)
				{
					this.endBoneRef = new GameObject(this.endTag + "Ref").transform;
					this.endBoneRef.hideFlags = HideFlags.HideInHierarchy;
					this.endBoneRef.SetParent(this.rootTransform);
				}
				if (!this.middleBoneRef)
				{
					this.middleBoneRef = new GameObject(this.middleTag + "Ref").transform;
					this.middleBoneRef.hideFlags = HideFlags.HideInHierarchy;
					this.middleBoneRef.SetParent(this.rootTransform);
				}
				if (!this.endBoneOffset)
				{
					this.endBoneOffset = new GameObject(this.endTag + "Offset").transform;
					this.endBoneOffset.SetParent(this.endBoneRef);
					this.endBoneOffset.localPosition = Vector3.zero;
					this.endBoneOffset.localEulerAngles = Vector3.zero;
				}
				if (!this.middleBoneOffset)
				{
					this.middleBoneOffset = new GameObject(this.middleTag + "Offset").transform;
					this.middleBoneOffset.SetParent(this.middleBoneRef);
					this.middleBoneOffset.localPosition = Vector3.zero;
					this.middleBoneOffset.localEulerAngles = Vector3.zero;
				}
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000BD165 File Offset: 0x000BB365
		public virtual float ikWeight
		{
			get
			{
				return this._weight;
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000BD16D File Offset: 0x000BB36D
		public virtual void SetIKWeight(float weight)
		{
			this._weight = weight;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000BD178 File Offset: 0x000BB378
		public void UpdateIK()
		{
			if (this.endBoneRef)
			{
				this.endBoneRef.position = this.endBone.position;
				this.endBoneRef.rotation = this.endBone.rotation;
			}
			if (this.middleBoneRef)
			{
				this.middleBoneRef.position = this.middleBone.position;
				this.middleBoneRef.rotation = this.middleBone.rotation;
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000BD1F8 File Offset: 0x000BB3F8
		public virtual void AnimationToIK()
		{
			if (!this.isValidBones)
			{
				this.CreateBones();
				return;
			}
			this.UpdateIK();
			this.SetIKHintPosition(this.middleBoneOffset.position);
			this.SetIKPosition(this.endBoneOffset.position);
			this.SetIKRotation(this.endBoneOffset.rotation);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000BD250 File Offset: 0x000BB450
		public virtual void SetIKPosition(Vector3 ikPosition)
		{
			if (this.ikWeight <= 0f)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			if (this.hintPosition != null)
			{
				vector = this.hintPosition.Value - this.rootBone.position;
			}
			else
			{
				vector = Vector3.Cross(this.endBone.position - this.rootBone.position, Vector3.Cross(this.endBone.position - this.rootBone.position, this.endBone.position - this.middleBone.position));
			}
			float magnitude = (this.middleBone.position - this.rootBone.position).magnitude;
			float magnitude2 = (this.endBone.position - this.middleBone.position).magnitude;
			Vector3 vector2 = this.GetHintPosition(this.rootBone.position, ikPosition, magnitude, magnitude2, vector);
			Quaternion quaternion = Quaternion.FromToRotation(this.middleBone.position - this.rootBone.position, vector2 - this.rootBone.position) * this.rootBone.rotation;
			if (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y) && !float.IsNaN(quaternion.z))
			{
				this.rootBone.rotation = Quaternion.Slerp(this.rootBone.rotation, quaternion, this.ikWeight);
				Quaternion quaternion2 = Quaternion.FromToRotation(this.endBone.position - this.middleBone.position, ikPosition - vector2) * this.middleBone.rotation;
				this.middleBone.rotation = Quaternion.Slerp(this.middleBone.rotation, quaternion2, this.ikWeight);
			}
			this.hintPosition = null;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000BD458 File Offset: 0x000BB658
		public virtual void SetIKRotation(Quaternion rotation)
		{
			if (!this.rootBone || !this.middleBone || !this.endBone || this.ikWeight <= 0f)
			{
				return;
			}
			this.endBone.rotation = Quaternion.Slerp(this.endBone.rotation, rotation, this.ikWeight);
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x000BD4BE File Offset: 0x000BB6BE
		public virtual void SetIKHintPosition(Vector3 hintPosition)
		{
			this.hintPosition = new Vector3?(hintPosition);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000BD4CC File Offset: 0x000BB6CC
		protected virtual Vector3 GetHintPosition(Vector3 rootPos, Vector3 endPos, float rootBoneLength, float middleBoneLength, Vector3 middleBoneDirection)
		{
			Vector3 vector = endPos - rootPos;
			float num = vector.magnitude;
			float num2 = (rootBoneLength + middleBoneLength) * 0.999f;
			if (num > num2)
			{
				endPos = rootPos + vector.normalized * num2;
				vector = endPos - rootPos;
				num = num2;
			}
			float num3 = Mathf.Abs(rootBoneLength - middleBoneLength) * 1.001f;
			if (num < num3)
			{
				endPos = rootPos + vector.normalized * num3;
				vector = endPos - rootPos;
				num = num3;
			}
			float num4 = (num * num + rootBoneLength * rootBoneLength - middleBoneLength * middleBoneLength) * 0.5f / num;
			float num5 = Mathf.Sqrt(rootBoneLength * rootBoneLength - num4 * num4);
			Vector3 vector2 = Vector3.Cross(vector, Vector3.Cross(middleBoneDirection, vector));
			return rootPos + num4 * vector.normalized + num5 * vector2.normalized;
		}

		// Token: 0x0400208E RID: 8334
		public Transform rootTransform;

		// Token: 0x0400208F RID: 8335
		public Transform rootBone;

		// Token: 0x04002090 RID: 8336
		public Transform middleBone;

		// Token: 0x04002091 RID: 8337
		public Transform endBone;

		// Token: 0x04002092 RID: 8338
		[Header("Optional")]
		public Transform endBoneRef;

		// Token: 0x04002093 RID: 8339
		public Transform middleBoneRef;

		// Token: 0x04002094 RID: 8340
		public Transform endBoneOffset;

		// Token: 0x04002095 RID: 8341
		public Transform middleBoneOffset;

		// Token: 0x04002096 RID: 8342
		private string middleTag;

		// Token: 0x04002097 RID: 8343
		private string endTag;

		// Token: 0x04002098 RID: 8344
		private float _weight;

		// Token: 0x04002099 RID: 8345
		private Vector3? hintPosition;
	}
}
