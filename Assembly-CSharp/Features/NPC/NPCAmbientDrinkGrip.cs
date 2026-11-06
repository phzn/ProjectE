using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ECA RID: 3786
	[DefaultExecutionOrder(10000)]
	public sealed class NPCAmbientDrinkGrip : MonoBehaviour
	{
		// Token: 0x06007149 RID: 29001 RVA: 0x002DFCA8 File Offset: 0x002DDEA8
		private bool Cache()
		{
			if (this.animator != null)
			{
				return this.hand != null && this.mouth != null && this.contact != null;
			}
			this.animator = base.GetComponentInParent<Animator>();
			if (this.animator == null || !this.animator.isHuman)
			{
				return false;
			}
			this.owner = this.animator.GetComponent<NPCWanderController>();
			this.upper = this.animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
			this.forearm = this.animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
			this.hand = this.animator.GetBoneTransform(HumanBodyBones.RightHand);
			this.head = this.animator.GetBoneTransform(HumanBodyBones.Head);
			this.mouth = ((this.head != null) ? this.head.Find("ToonMouth") : null);
			this.contact = base.transform.Find("MouthContact");
			return this.upper != null && this.forearm != null && this.hand != null && this.mouth != null && this.contact != null;
		}

		// Token: 0x0600714A RID: 29002 RVA: 0x002DFDF7 File Offset: 0x002DDFF7
		private void OnEnable()
		{
			this.lastCorrection = Vector3.zero;
		}

		// Token: 0x0600714B RID: 29003 RVA: 0x002DFE04 File Offset: 0x002DE004
		private void LateUpdate()
		{
			if (!this.Cache())
			{
				return;
			}
			NPCAmbientLife npcambientLife = ((this.owner != null) ? this.owner.AmbientLife : null);
			float num = ((npcambientLife != null) ? npcambientLife.DrinkContactWeight : 0f);
			bool flag = npcambientLife != null && npcambientLife.IsTakingSip;
			this.ApplyContact(num, flag);
		}

		// Token: 0x0600714C RID: 29004 RVA: 0x002DFE60 File Offset: 0x002DE060
		public void ApplyContact(float weight, bool atMouth)
		{
			if (weight <= 0f || !base.isActiveAndEnabled || !this.Cache())
			{
				return;
			}
			Vector3 vector;
			if (atMouth)
			{
				vector = Vector3.ClampMagnitude(this.mouth.position + this.head.up * 0.02f + this.head.forward * 0.003f - this.contact.position, 0.035f);
				this.lastCorrection = this.head.InverseTransformVector(vector);
			}
			else
			{
				vector = this.head.TransformVector(this.lastCorrection);
			}
			vector *= Mathf.Clamp01(weight);
			if (vector.sqrMagnitude < 1E-08f)
			{
				return;
			}
			Vector3 position = this.upper.position;
			Vector3 position2 = this.forearm.position;
			Vector3 position3 = this.hand.position;
			float num = Vector3.Distance(position, position2);
			float num2 = Vector3.Distance(position2, position3);
			Vector3 vector2 = position3 + vector;
			Vector3 vector3 = vector2 - position;
			float num3 = Mathf.Clamp(vector3.magnitude, Mathf.Abs(num - num2) + 0.0001f, num + num2 - 0.0001f);
			if (num < 0.001f || num2 < 0.001f || vector3.sqrMagnitude < 1E-06f)
			{
				return;
			}
			vector3.Normalize();
			vector2 = position + vector3 * num3;
			Vector3 vector4 = Vector3.ProjectOnPlane(position2 - position, vector3);
			if (vector4.sqrMagnitude < 1E-06f)
			{
				return;
			}
			vector4.Normalize();
			float num4 = Mathf.Clamp((num * num + num3 * num3 - num2 * num2) / (2f * num * num3), -1f, 1f);
			Vector3 vector5 = position + vector3 * (num * num4) + vector4 * (num * Mathf.Sqrt(1f - num4 * num4));
			Quaternion rotation = this.hand.rotation;
			this.upper.rotation = Quaternion.FromToRotation(position2 - position, vector5 - position) * this.upper.rotation;
			this.forearm.rotation = Quaternion.FromToRotation(this.hand.position - this.forearm.position, vector2 - this.forearm.position) * this.forearm.rotation;
			this.hand.rotation = rotation;
		}

		// Token: 0x04007421 RID: 29729
		private Animator animator;

		// Token: 0x04007422 RID: 29730
		private NPCWanderController owner;

		// Token: 0x04007423 RID: 29731
		private Transform upper;

		// Token: 0x04007424 RID: 29732
		private Transform forearm;

		// Token: 0x04007425 RID: 29733
		private Transform hand;

		// Token: 0x04007426 RID: 29734
		private Transform head;

		// Token: 0x04007427 RID: 29735
		private Transform mouth;

		// Token: 0x04007428 RID: 29736
		private Transform contact;

		// Token: 0x04007429 RID: 29737
		private Vector3 lastCorrection;
	}
}
