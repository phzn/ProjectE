using System;
using UnityEngine;

namespace Invector.IK
{
	// Token: 0x0200051E RID: 1310
	public class vArmAimAlign
	{
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x000BC5D4 File Offset: 0x000BA7D4
		// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x000BC5DC File Offset: 0x000BA7DC
		public Transform upperArmHelper { get; protected set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x000BC5E5 File Offset: 0x000BA7E5
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x000BC5ED File Offset: 0x000BA7ED
		public Transform foreArmHelper { get; protected set; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x000BC5F6 File Offset: 0x000BA7F6
		// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x000BC5FE File Offset: 0x000BA7FE
		public Transform handHelper { get; protected set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x000BC607 File Offset: 0x000BA807
		// (set) Token: 0x06001FBB RID: 8123 RVA: 0x000BC60F File Offset: 0x000BA80F
		public Transform aimReferenceHelper { get; protected set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x000BC618 File Offset: 0x000BA818
		// (set) Token: 0x06001FBD RID: 8125 RVA: 0x000BC620 File Offset: 0x000BA820
		public float smooth { get; set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001FBE RID: 8126 RVA: 0x000BC629 File Offset: 0x000BA829
		// (set) Token: 0x06001FBF RID: 8127 RVA: 0x000BC631 File Offset: 0x000BA831
		public float maxVerticalAligmentAngle { get; set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x000BC63A File Offset: 0x000BA83A
		// (set) Token: 0x06001FC1 RID: 8129 RVA: 0x000BC642 File Offset: 0x000BA842
		public float maxHorizontalAligmentAngle { get; set; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x000BC64B File Offset: 0x000BA84B
		// (set) Token: 0x06001FC3 RID: 8131 RVA: 0x000BC653 File Offset: 0x000BA853
		public Transform aimReference { get; set; }

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000BC65C File Offset: 0x000BA85C
		public vArmAimAlign(Transform upperArm, Transform foreArm, Transform hand)
		{
			this.upperArm = upperArm;
			this.foreArm = foreArm;
			this.hand = hand;
			if (!upperArm || !foreArm || !hand)
			{
				return;
			}
			this.root = upperArm.parent;
			if (!this.upperArmHelper)
			{
				this.upperArmHelper = new GameObject("UpperArmHelper").transform;
				this.upperArmHelper.gameObject.tag = "Ignore Ragdoll";
			}
			if (!this.foreArmHelper)
			{
				this.foreArmHelper = new GameObject("ForeArmHelper").transform;
				this.foreArmHelper.gameObject.tag = "Ignore Ragdoll";
			}
			if (!this.handHelper)
			{
				this.handHelper = new GameObject("HandHelper").transform;
				this.handHelper.gameObject.tag = "Ignore Ragdoll";
			}
			if (!this.aimReferenceHelper)
			{
				this.aimReferenceHelper = new GameObject("AimReferenceHelper").transform;
				this.aimReferenceHelper.gameObject.tag = "Ignore Ragdoll";
			}
			this.upperArmHelper.parent = this.root;
			this.foreArmHelper.parent = this.upperArmHelper;
			this.handHelper.parent = this.foreArmHelper;
			this.aimReferenceHelper.parent = this.handHelper;
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x000BC7D0 File Offset: 0x000BA9D0
		public bool IsValid
		{
			get
			{
				return this.upperArm && this.foreArm && this.hand && this.aimReference;
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000BC808 File Offset: 0x000BAA08
		public void UpdateDefaultAlignment()
		{
			if (!this.IsValid)
			{
				return;
			}
			this.upperArmHelper.SetPositionAndRotation(this.upperArm.position, this.upperArm.rotation);
			this.upperArmHelper.localScale = this.upperArm.localScale;
			this.foreArmHelper.SetPositionAndRotation(this.foreArm.position, this.foreArm.rotation);
			this.foreArmHelper.localScale = this.foreArm.localScale;
			this.handHelper.SetPositionAndRotation(this.hand.position, this.hand.rotation);
			this.handHelper.localScale = this.hand.localScale;
			this.aimReferenceHelper.SetPositionAndRotation(this.aimReference.position, this.aimReference.rotation);
			this.aimReferenceHelper.localScale = this.aimReference.localScale;
			this._upperArmRotation = this.upperArmHelper.localRotation;
			this._foreArmRotation = this.foreArmHelper.localRotation;
			this._handRotation = this.handHelper.localRotation;
			this._aimReferenceRotation = this.aimReferenceHelper.localRotation;
			this._upperArmPosition = this.upperArmHelper.localPosition;
			this._foreArmPosition = this.foreArmHelper.localPosition;
			this._handPosition = this.handHelper.localPosition;
			this._aimReferencePosition = this.aimReferenceHelper.localPosition;
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000BC984 File Offset: 0x000BAB84
		public void RestoreToLastAlignment()
		{
			if (!this.IsValid)
			{
				return;
			}
			this.upperArmHelper.localRotation = this._upperArmRotation;
			this.foreArmHelper.localRotation = this._foreArmRotation;
			this.handHelper.localRotation = this._handRotation;
			this.aimReferenceHelper.localRotation = this._aimReferenceRotation;
			this.upperArmHelper.localPosition = this._upperArmPosition;
			this.foreArmHelper.localPosition = this._foreArmPosition;
			this.handHelper.localPosition = this._handPosition;
			this.aimReferenceHelper.localPosition = this._aimReferencePosition;
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000BCA24 File Offset: 0x000BAC24
		public void AlignToArmToPosition(Vector3 targetPosition, float weight, bool alignUpperArm = true, bool alignHand = true)
		{
			if (!this.IsValid)
			{
				return;
			}
			if (this.smoothIKAlignmentPoint)
			{
				targetPosition = this.SmootAndClampPosition(targetPosition, this.maxHorizontalAligmentAngle, this.maxVerticalAligmentAngle);
			}
			if (alignUpperArm)
			{
				this.AlignUpperArm(targetPosition, alignHand ? (weight * 0.5f) : weight);
			}
			if (alignHand)
			{
				this.AlignHand(targetPosition, weight);
			}
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000BCA7B File Offset: 0x000BAC7B
		protected virtual void AlignUpperArm(Vector3 targetPosition, float weight)
		{
			this.AlignArmBone(this.upperArm, this.upperArmHelper, ref this._upperArmRotationSmooth, targetPosition, weight);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000BCA97 File Offset: 0x000BAC97
		protected virtual void AlignHand(Vector3 targetPosition, float weight)
		{
			this.AlignArmBone(this.hand, this.handHelper, ref this._handRotationSmooth, targetPosition, weight);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000BCAB4 File Offset: 0x000BACB4
		protected virtual void AlignArmBone(Transform bone, Transform boneHelper, ref Quaternion smoothRotation, Vector3 targetPosition, float weight)
		{
			if (!this.smoothIKAlignmentPoint && Math.Round((double)weight, 1) == 0.0)
			{
				smoothRotation = Quaternion.identity;
				return;
			}
			Vector3 vector = targetPosition - this.aimReferenceHelper.position;
			Quaternion quaternion = Quaternion.identity;
			if (!this.smoothIKAlignmentPoint)
			{
				quaternion = this.ClampRotation(Quaternion.LookRotation(vector, this.aimReferenceHelper.up), this.maxHorizontalAligmentAngle * weight, this.maxVerticalAligmentAngle * weight);
			}
			else
			{
				quaternion = Quaternion.LookRotation(vector, this.aimReferenceHelper.up);
			}
			Vector3 vector2 = boneHelper.InverseTransformDirection(this.aimReferenceHelper.forward);
			Vector3 vector3 = boneHelper.InverseTransformDirection(quaternion * Vector3.forward);
			Quaternion quaternion2 = Quaternion.FromToRotation(vector2, vector3);
			if (!float.IsNaN(quaternion2.x) && !float.IsNaN(quaternion2.y) && !float.IsNaN(quaternion2.z))
			{
				Quaternion quaternion3;
				if (!this.smoothIKAlignmentPoint)
				{
					smoothRotation = Quaternion.Slerp(smoothRotation, Quaternion.Lerp(Quaternion.identity, quaternion2, weight), this.smooth * Time.fixedDeltaTime);
					quaternion3 = smoothRotation;
				}
				else
				{
					quaternion3 = Quaternion.Lerp(Quaternion.identity, quaternion2, weight);
				}
				bone.localRotation *= quaternion3;
				boneHelper.localRotation *= quaternion3;
			}
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000BCC10 File Offset: 0x000BAE10
		protected virtual Quaternion ClampRotation(Quaternion rotation, float maxHorizontalAngle, float maxVerticalAngle)
		{
			Vector3 vector = rotation * Vector3.forward;
			Vector3 vector2 = Quaternion.LookRotation(this.aimReferenceHelper.InverseTransformDirection(vector)).eulerAngles.NormalizeAngle();
			vector2.y = Mathf.Clamp(vector2.y, -maxHorizontalAngle, maxHorizontalAngle);
			vector2.x = Mathf.Clamp(vector2.x, -maxVerticalAngle, maxVerticalAngle);
			vector2.z = 0f;
			vector = this.aimReferenceHelper.TransformDirection(Quaternion.Euler(vector2.NormalizeAngle()) * Vector3.forward);
			return Quaternion.LookRotation(vector, this.aimReferenceHelper.up);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000BCCB0 File Offset: 0x000BAEB0
		protected virtual Vector3 SmootAndClampPosition(Vector3 position, float maxHorizontalAngle, float maxVerticalAngle)
		{
			Vector3 vector = position - this.aimReferenceHelper.position;
			this.smoothRotationDirection = Quaternion.Slerp(this.ClampRotation(this.smoothRotationDirection, maxHorizontalAngle, maxVerticalAngle), Quaternion.LookRotation(position - this.aimReferenceHelper.position, this.aimReferenceHelper.up), this.smooth * Time.fixedDeltaTime);
			position = this.aimReferenceHelper.position + this.smoothRotationDirection * Vector3.forward * vector.magnitude;
			return position;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000BCD44 File Offset: 0x000BAF44
		public void DrawBones(Color color)
		{
			if (!this.IsValid)
			{
				return;
			}
			Debug.DrawLine(this.upperArm.position, this.foreArm.position, color);
			Debug.DrawLine(this.foreArm.position, this.hand.position, color);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000BCD94 File Offset: 0x000BAF94
		public void DrawHelpers(Color color)
		{
			if (!this.IsValid)
			{
				return;
			}
			Debug.DrawLine(this.upperArmHelper.position, this.foreArmHelper.position, color);
			Debug.DrawLine(this.foreArmHelper.position, this.handHelper.position, color);
		}

		// Token: 0x04002076 RID: 8310
		protected Transform root;

		// Token: 0x04002077 RID: 8311
		protected Transform upperArm;

		// Token: 0x04002078 RID: 8312
		protected Transform foreArm;

		// Token: 0x04002079 RID: 8313
		protected Transform hand;

		// Token: 0x0400207A RID: 8314
		public bool smoothIKAlignmentPoint = true;

		// Token: 0x04002083 RID: 8323
		protected Quaternion _upperArmRotation;

		// Token: 0x04002084 RID: 8324
		protected Quaternion _foreArmRotation;

		// Token: 0x04002085 RID: 8325
		protected Quaternion _handRotation;

		// Token: 0x04002086 RID: 8326
		protected Quaternion _aimReferenceRotation;

		// Token: 0x04002087 RID: 8327
		protected Quaternion _upperArmRotationSmooth;

		// Token: 0x04002088 RID: 8328
		protected Quaternion _handRotationSmooth;

		// Token: 0x04002089 RID: 8329
		protected Quaternion smoothRotationDirection;

		// Token: 0x0400208A RID: 8330
		protected Vector3 _upperArmPosition;

		// Token: 0x0400208B RID: 8331
		protected Vector3 _foreArmPosition;

		// Token: 0x0400208C RID: 8332
		protected Vector3 _handPosition;

		// Token: 0x0400208D RID: 8333
		protected Vector3 _aimReferencePosition;
	}
}
