using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B2C RID: 2860
	internal sealed class BenchFootGrounding
	{
		// Token: 0x06005113 RID: 20755 RVA: 0x001F6884 File Offset: 0x001F4A84
		public BenchFootGrounding(Animator animator, BenchSeat seat)
		{
			this.actor = animator.transform;
			this.seat = seat;
			this.left = new BenchFootGrounding.Leg(animator, true);
			this.right = new BenchFootGrounding.Leg(animator, false);
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x001F68D1 File Offset: 0x001F4AD1
		public void MeasureVertex(Vector3 worldPoint)
		{
			this.left.MeasureVertex(worldPoint);
			this.right.MeasureVertex(worldPoint);
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x001F68EB File Offset: 0x001F4AEB
		public bool UsesFoot(Transform bone)
		{
			return this.left.UsesFoot(bone) || this.right.UsesFoot(bone);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001F6909 File Offset: 0x001F4B09
		public void Restore()
		{
			this.left.Restore();
			this.right.Restore();
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x001F6921 File Offset: 0x001F4B21
		public void Apply(float contact)
		{
			if (this.seat == null || contact <= 0f)
			{
				return;
			}
			this.Apply(this.left, Mathf.Clamp01(contact));
			this.Apply(this.right, Mathf.Clamp01(contact));
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001F6960 File Offset: 0x001F4B60
		private void Apply(BenchFootGrounding.Leg leg, float contact)
		{
			if (!leg.IsValid)
			{
				return;
			}
			Vector3 position = leg.Foot.position;
			position.y = Mathf.Min(this.seat.SeatPosition.y + 0.3f, this.seat.CushionPosition.y - 0.04f);
			int num = Physics.RaycastNonAlloc(position, Vector3.down, this.hits, 0.85f, -5, QueryTriggerInteraction.Ignore);
			if (num == this.hits.Length)
			{
				return;
			}
			float num2 = float.NegativeInfinity;
			for (int i = 0; i < num; i++)
			{
				Collider collider = this.hits[i].collider;
				if (!(collider == null) && !collider.transform.IsChildOf(this.actor) && this.hits[i].normal.y >= 0.6f)
				{
					num2 = Mathf.Max(num2, this.hits[i].point.y);
				}
			}
			if (float.IsNegativeInfinity(num2))
			{
				return;
			}
			float num3 = num2 + 0.005f - leg.LowestPoint();
			if (num3 > 0f)
			{
				leg.Lift(num3 * contact);
			}
		}

		// Token: 0x040051C4 RID: 20932
		private readonly Transform actor;

		// Token: 0x040051C5 RID: 20933
		private readonly BenchSeat seat;

		// Token: 0x040051C6 RID: 20934
		private readonly BenchFootGrounding.Leg left;

		// Token: 0x040051C7 RID: 20935
		private readonly BenchFootGrounding.Leg right;

		// Token: 0x040051C8 RID: 20936
		private readonly RaycastHit[] hits = new RaycastHit[16];

		// Token: 0x02000B2D RID: 2861
		private sealed class Leg
		{
			// Token: 0x17000D82 RID: 3458
			// (get) Token: 0x06005119 RID: 20761 RVA: 0x001F6A8D File Offset: 0x001F4C8D
			public bool IsValid
			{
				get
				{
					return this.thigh != null && this.shin != null && this.Foot != null;
				}
			}

			// Token: 0x0600511A RID: 20762 RVA: 0x001F6ABC File Offset: 0x001F4CBC
			public Leg(Animator animator, bool left)
			{
				this.thigh = animator.GetBoneTransform(left ? HumanBodyBones.LeftUpperLeg : HumanBodyBones.RightUpperLeg);
				this.shin = animator.GetBoneTransform(left ? HumanBodyBones.LeftLowerLeg : HumanBodyBones.RightLowerLeg);
				this.Foot = animator.GetBoneTransform(left ? HumanBodyBones.LeftFoot : HumanBodyBones.RightFoot);
				this.toes = animator.GetBoneTransform(left ? HumanBodyBones.LeftToes : HumanBodyBones.RightToes);
				this.scale = Mathf.Max(0.01f, animator.humanScale * Mathf.Abs(animator.transform.lossyScale.y));
				this.sole = (left ? animator.leftFeetBottomHeight : animator.rightFeetBottomHeight) * Mathf.Abs(animator.transform.lossyScale.y);
			}

			// Token: 0x0600511B RID: 20763 RVA: 0x001F6B81 File Offset: 0x001F4D81
			public bool UsesFoot(Transform bone)
			{
				return bone != null && (bone == this.Foot || bone == this.toes);
			}

			// Token: 0x0600511C RID: 20764 RVA: 0x001F6BAC File Offset: 0x001F4DAC
			public void MeasureVertex(Vector3 world)
			{
				if (!this.IsValid || world.y > this.Foot.position.y + 0.025f * this.scale || world.y < this.Foot.position.y - 0.22f * this.scale)
				{
					return;
				}
				Vector3 position = this.Foot.position;
				Vector3 vector = ((this.toes != null) ? this.toes.position : position);
				position.y = (vector.y = world.y);
				Vector3 vector2 = vector - position;
				float num = ((vector2.sqrMagnitude > 1E-06f) ? Mathf.Clamp01(Vector3.Dot(world - position, vector2) / vector2.sqrMagnitude) : 0f);
				if ((world - (position + vector2 * num)).sqrMagnitude < 0.0121f * this.scale * this.scale)
				{
					this.solePoints.Add(this.Foot.InverseTransformPoint(world));
				}
			}

			// Token: 0x0600511D RID: 20765 RVA: 0x001F6CD0 File Offset: 0x001F4ED0
			public float LowestPoint()
			{
				float num = this.Foot.position.y - this.sole;
				if (this.toes != null)
				{
					num = Mathf.Min(num, this.toes.position.y - 0.018f * this.scale);
				}
				Matrix4x4 localToWorldMatrix = this.Foot.localToWorldMatrix;
				for (int i = 0; i < this.solePoints.Count; i++)
				{
					num = Mathf.Min(num, localToWorldMatrix.MultiplyPoint3x4(this.solePoints[i]).y);
				}
				return num;
			}

			// Token: 0x0600511E RID: 20766 RVA: 0x001F6D6C File Offset: 0x001F4F6C
			public void Lift(float amount)
			{
				this.thighBefore = this.thigh.localRotation;
				this.shinBefore = this.shin.localRotation;
				this.footBefore = this.Foot.localRotation;
				Quaternion rotation = this.Foot.rotation;
				Vector3 position = this.thigh.position;
				Vector3 vector = this.shin.position - position;
				Vector3 vector2 = this.Foot.position - this.shin.position;
				Vector3 vector3 = this.Foot.position + Vector3.up * amount - position;
				Vector3 vector4 = Vector3.Cross(vector, vector2);
				if (vector4.sqrMagnitude < 1E-06f)
				{
					vector4 = Vector3.Cross(vector3, vector2);
				}
				if (vector4.sqrMagnitude < 1E-06f)
				{
					return;
				}
				float num = BenchFootGrounding.Leg.Angle((this.Foot.position - position).magnitude, vector.magnitude, vector2.magnitude) - BenchFootGrounding.Leg.Angle(vector3.magnitude, vector.magnitude, vector2.magnitude);
				this.shin.rotation = Quaternion.AngleAxis(num * 57.29578f, vector4.normalized) * this.shin.rotation;
				this.thigh.rotation = Quaternion.FromToRotation(this.Foot.position - position, vector3) * this.thigh.rotation;
				this.Foot.rotation = rotation;
				this.thighAfter = this.thigh.localRotation;
				this.shinAfter = this.shin.localRotation;
				this.footAfter = this.Foot.localRotation;
				this.corrected = true;
			}

			// Token: 0x0600511F RID: 20767 RVA: 0x001F6F37 File Offset: 0x001F5137
			private static float Angle(float opposite, float a, float b)
			{
				return Mathf.Acos(Mathf.Clamp((a * a + b * b - opposite * opposite) / Mathf.Max(1E-06f, 2f * a * b), -1f, 1f));
			}

			// Token: 0x06005120 RID: 20768 RVA: 0x001F6F6C File Offset: 0x001F516C
			public void Restore()
			{
				if (!this.corrected || !this.IsValid)
				{
					return;
				}
				if (Quaternion.Angle(this.thigh.localRotation, this.thighAfter) < 0.01f)
				{
					this.thigh.localRotation = this.thighBefore;
				}
				if (Quaternion.Angle(this.shin.localRotation, this.shinAfter) < 0.01f)
				{
					this.shin.localRotation = this.shinBefore;
				}
				if (Quaternion.Angle(this.Foot.localRotation, this.footAfter) < 0.01f)
				{
					this.Foot.localRotation = this.footBefore;
				}
				this.corrected = false;
			}

			// Token: 0x040051C9 RID: 20937
			private readonly Transform thigh;

			// Token: 0x040051CA RID: 20938
			private readonly Transform shin;

			// Token: 0x040051CB RID: 20939
			private readonly Transform toes;

			// Token: 0x040051CC RID: 20940
			public readonly Transform Foot;

			// Token: 0x040051CD RID: 20941
			private readonly float scale;

			// Token: 0x040051CE RID: 20942
			private readonly float sole;

			// Token: 0x040051CF RID: 20943
			private readonly List<Vector3> solePoints = new List<Vector3>();

			// Token: 0x040051D0 RID: 20944
			private Quaternion thighBefore;

			// Token: 0x040051D1 RID: 20945
			private Quaternion shinBefore;

			// Token: 0x040051D2 RID: 20946
			private Quaternion footBefore;

			// Token: 0x040051D3 RID: 20947
			private Quaternion thighAfter;

			// Token: 0x040051D4 RID: 20948
			private Quaternion shinAfter;

			// Token: 0x040051D5 RID: 20949
			private Quaternion footAfter;

			// Token: 0x040051D6 RID: 20950
			private bool corrected;
		}
	}
}
