using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B2A RID: 2858
	public sealed class BenchArmPoseTransition
	{
		// Token: 0x0600510B RID: 20747 RVA: 0x001F64AE File Offset: 0x001F46AE
		public BenchArmPoseTransition(params Transform[] bones)
		{
			this.bones = bones;
			this.previous = new Quaternion[bones.Length];
			this.correction = new Quaternion[bones.Length];
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001F64EC File Offset: 0x001F46EC
		public static BenchArmPoseTransition ForAnimator(Animator animator)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < 55; i++)
			{
				HumanBodyBones humanBodyBones = (HumanBodyBones)i;
				bool flag = humanBodyBones == HumanBodyBones.LeftShoulder || humanBodyBones == HumanBodyBones.RightShoulder || humanBodyBones == HumanBodyBones.LeftUpperArm || humanBodyBones == HumanBodyBones.RightUpperArm || humanBodyBones == HumanBodyBones.LeftLowerArm || humanBodyBones == HumanBodyBones.RightLowerArm || humanBodyBones == HumanBodyBones.LeftHand || humanBodyBones == HumanBodyBones.RightHand;
				bool flag2 = humanBodyBones >= HumanBodyBones.LeftThumbProximal && humanBodyBones <= HumanBodyBones.RightLittleDistal;
				if (flag || flag2)
				{
					Transform boneTransform = animator.GetBoneTransform(humanBodyBones);
					if (boneTransform != null)
					{
						list.Add(boneTransform);
					}
				}
			}
			return new BenchArmPoseTransition(list.ToArray());
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x001F6578 File Offset: 0x001F4778
		public void SetTarget(bool native, int key = 0, bool ready = true)
		{
			if (!native)
			{
				key = 0;
			}
			if (this.hasTarget && (this.nativeTarget != native || this.targetKey != key))
			{
				this.pending = true;
			}
			this.nativeTarget = native;
			this.targetKey = key;
			this.destinationReady = ready;
			this.hasTarget = true;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x001F65C8 File Offset: 0x001F47C8
		public void Apply(float deltaTime)
		{
			if (this.hasPrevious && this.nativeTarget && !this.destinationReady)
			{
				for (int i = 0; i < this.bones.Length; i++)
				{
					if (this.bones[i] != null)
					{
						this.bones[i].localRotation = this.previous[i];
					}
				}
				this.pending = true;
				return;
			}
			if (this.pending && this.hasPrevious)
			{
				for (int j = 0; j < this.bones.Length; j++)
				{
					if (this.bones[j] != null)
					{
						this.correction[j] = this.previous[j] * Quaternion.Inverse(this.bones[j].localRotation);
					}
				}
				this.elapsed = 0f;
			}
			else
			{
				this.elapsed = Mathf.Min(0.18f, this.elapsed + Mathf.Max(0f, deltaTime));
			}
			this.pending = false;
			float num = 1f - Mathf.SmoothStep(0f, 1f, this.elapsed / 0.18f);
			for (int k = 0; k < this.bones.Length; k++)
			{
				if (!(this.bones[k] == null))
				{
					if (this.hasPrevious && num > 0f)
					{
						this.bones[k].localRotation = Quaternion.Slerp(Quaternion.identity, this.correction[k], num) * this.bones[k].localRotation;
					}
					this.previous[k] = this.bones[k].localRotation;
				}
			}
			this.hasPrevious = true;
		}

		// Token: 0x040051B4 RID: 20916
		public const float Duration = 0.18f;

		// Token: 0x040051B5 RID: 20917
		private readonly Transform[] bones;

		// Token: 0x040051B6 RID: 20918
		private readonly Quaternion[] previous;

		// Token: 0x040051B7 RID: 20919
		private readonly Quaternion[] correction;

		// Token: 0x040051B8 RID: 20920
		private bool hasPrevious;

		// Token: 0x040051B9 RID: 20921
		private bool hasTarget;

		// Token: 0x040051BA RID: 20922
		private bool nativeTarget;

		// Token: 0x040051BB RID: 20923
		private bool pending;

		// Token: 0x040051BC RID: 20924
		private bool destinationReady = true;

		// Token: 0x040051BD RID: 20925
		private int targetKey;

		// Token: 0x040051BE RID: 20926
		private float elapsed = 0.18f;
	}
}
