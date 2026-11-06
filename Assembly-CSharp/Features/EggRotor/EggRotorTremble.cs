using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x0200121E RID: 4638
	public sealed class EggRotorTremble
	{
		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x06008F6A RID: 36714 RVA: 0x003AA2F0 File Offset: 0x003A84F0
		public Transform Root
		{
			get
			{
				return this._root;
			}
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x06008F6B RID: 36715 RVA: 0x003AA2F8 File Offset: 0x003A84F8
		public bool IsBound
		{
			get
			{
				return this._root != null && this._channels[0].Transform != null;
			}
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x003AA321 File Offset: 0x003A8521
		public Transform GetBone(EggRotorTremble.Bone bone)
		{
			return this._channels[(int)bone].Transform;
		}

		// Token: 0x06008F6D RID: 36717 RVA: 0x003AA334 File Offset: 0x003A8534
		public bool Bind(Animator animator)
		{
			this.Unbind();
			if (animator == null || !animator.isHuman || animator.avatar == null)
			{
				return false;
			}
			this._root = animator.transform;
			for (int i = 0; i < 15; i++)
			{
				this._channels[i].Transform = animator.GetBoneTransform(EggRotorTremble.HumanBones[i]);
			}
			return this.IsBound;
		}

		// Token: 0x06008F6E RID: 36718 RVA: 0x003AA3A8 File Offset: 0x003A85A8
		public bool Bind(Transform root, Transform[] bones)
		{
			this.Unbind();
			if (root == null || bones == null)
			{
				return false;
			}
			this._root = root;
			int num = 0;
			while (num < 15 && num < bones.Length)
			{
				this._channels[num].Transform = bones[num];
				num++;
			}
			return this.IsBound;
		}

		// Token: 0x06008F6F RID: 36719 RVA: 0x003AA400 File Offset: 0x003A8600
		public void Unbind()
		{
			this.Restore();
			for (int i = 0; i < 15; i++)
			{
				this._channels[i] = default(EggRotorTremble.Channel);
			}
			this._root = null;
			this._hipsHasWritten = false;
		}

		// Token: 0x06008F70 RID: 36720 RVA: 0x003AA440 File Offset: 0x003A8640
		public bool TrySample(out EggRotorPoseSample sample)
		{
			sample = default(EggRotorPoseSample);
			if (!this.IsBound)
			{
				return false;
			}
			sample.Ground = this._root.position;
			sample.Up = this._root.up;
			sample.Hips = this._channels[0].Transform.position;
			sample.HasHead = this.TryGetPosition(EggRotorTremble.Bone.Head, out sample.Head) || this.TryGetPosition(EggRotorTremble.Bone.Neck, out sample.Head);
			bool flag = this.TryGetPosition(EggRotorTremble.Bone.LeftUpperLeg, out sample.LeftThigh);
			flag &= this.TryGetPosition(EggRotorTremble.Bone.LeftLowerLeg, out sample.LeftKnee);
			flag &= this.TryGetPosition(EggRotorTremble.Bone.LeftFoot, out sample.LeftFoot);
			flag &= this.TryGetPosition(EggRotorTremble.Bone.RightUpperLeg, out sample.RightThigh);
			flag &= this.TryGetPosition(EggRotorTremble.Bone.RightLowerLeg, out sample.RightKnee);
			flag &= this.TryGetPosition(EggRotorTremble.Bone.RightFoot, out sample.RightFoot);
			sample.HasLegs = flag;
			sample.HasLeftHand = this.TryGetPosition(EggRotorTremble.Bone.LeftHand, out sample.LeftHand);
			sample.HasRightHand = this.TryGetPosition(EggRotorTremble.Bone.RightHand, out sample.RightHand);
			return true;
		}

		// Token: 0x06008F71 RID: 36721 RVA: 0x003AA554 File Offset: 0x003A8754
		private bool TryGetPosition(EggRotorTremble.Bone bone, out Vector3 position)
		{
			Transform transform = this._channels[(int)bone].Transform;
			position = ((transform != null) ? transform.position : Vector3.zero);
			return transform != null;
		}

		// Token: 0x06008F72 RID: 36722 RVA: 0x003AA598 File Offset: 0x003A8798
		public void Apply(in EggRotorTremblePose pose)
		{
			if (!this.IsBound)
			{
				return;
			}
			for (int i = 0; i < 15; i++)
			{
				EggRotorTremble.Capture(ref this._channels[i]);
			}
			Transform transform = this._channels[0].Transform;
			this.CaptureHipsPosition(transform);
			Vector3 right = this._root.right;
			Vector3 forward = this._root.forward;
			Vector3 up = this._root.up;
			bool flag = pose.Weights.Legs > 0f;
			bool flag2 = flag || pose.Weights.Pelvis > 0f;
			Quaternion quaternion = (flag2 ? EggRotorTremble.Rotation(pose.HipsPitch, right, pose.HipsRoll, forward) : Quaternion.identity);
			Quaternion quaternion2 = EggRotorTremble.Rotation(-pose.LeftKneeFlex, right, pose.KneesTogether, forward);
			Quaternion quaternion3 = EggRotorTremble.Rotation(pose.LeftKneeFlex, right, -pose.KneesTogether, forward);
			Quaternion quaternion4 = EggRotorTremble.Rotation(-pose.RightKneeFlex, right, -pose.KneesTogether, forward);
			Quaternion quaternion5 = EggRotorTremble.Rotation(pose.RightKneeFlex, right, pose.KneesTogether, forward);
			if (flag2)
			{
				Vector3 position = transform.position;
				if (flag)
				{
					float num = 0f;
					int num2 = 0;
					this.AddLegRise(EggRotorTremble.Bone.LeftUpperLeg, EggRotorTremble.Bone.LeftLowerLeg, EggRotorTremble.Bone.LeftFoot, position, quaternion, quaternion2, quaternion3, up, ref num, ref num2);
					this.AddLegRise(EggRotorTremble.Bone.RightUpperLeg, EggRotorTremble.Bone.RightLowerLeg, EggRotorTremble.Bone.RightFoot, position, quaternion, quaternion4, quaternion5, up, ref num, ref num2);
					float num3 = ((num2 > 0) ? Mathf.Clamp(num / (float)num2, -0.02f, 0.06f) : 0f);
					if (num3 != 0f && !float.IsNaN(num3))
					{
						transform.position = position - up * num3;
					}
				}
				transform.rotation = quaternion * transform.rotation;
			}
			this.RotateWorld(EggRotorTremble.Bone.Spine, EggRotorTremble.Rotation(pose.SpinePitch, right, pose.SpineRoll, forward));
			this.RotateWorld(EggRotorTremble.Bone.Chest, EggRotorTremble.Rotation(pose.ChestPitch, right, pose.ChestRoll, forward));
			this.RotateWorld(EggRotorTremble.Bone.Neck, EggRotorTremble.Rotation(pose.NeckPitch, right, 0f, forward));
			this.RotateWorld(EggRotorTremble.Bone.Head, EggRotorTremble.Rotation(pose.HeadPitch, right, pose.HeadRoll, forward));
			if (flag2)
			{
				this.ApplyLeg(EggRotorTremble.Bone.LeftUpperLeg, EggRotorTremble.Bone.LeftLowerLeg, EggRotorTremble.Bone.LeftFoot, quaternion, quaternion2, quaternion3, flag);
				this.ApplyLeg(EggRotorTremble.Bone.RightUpperLeg, EggRotorTremble.Bone.RightLowerLeg, EggRotorTremble.Bone.RightFoot, quaternion, quaternion4, quaternion5, flag);
			}
			this.RotateWorld(EggRotorTremble.Bone.LeftUpperArm, EggRotorTremble.Rotation(pose.LeftArmPitch, right, pose.LeftArmRoll, forward));
			this.RotateWorld(EggRotorTremble.Bone.RightUpperArm, EggRotorTremble.Rotation(pose.RightArmPitch, right, pose.RightArmRoll, forward));
			this.RotateWorld(EggRotorTremble.Bone.LeftHand, EggRotorTremble.Rotation(pose.LeftHandPitch, right, pose.LeftHandRoll, forward));
			this.RotateWorld(EggRotorTremble.Bone.RightHand, EggRotorTremble.Rotation(pose.RightHandPitch, right, pose.RightHandRoll, forward));
			for (int j = 0; j < 15; j++)
			{
				if (!(this._channels[j].Transform == null))
				{
					this._channels[j].Written = this._channels[j].Transform.localRotation;
					this._channels[j].HasWritten = true;
				}
			}
			this._hipsWritten = transform.localPosition;
			this._hipsHasWritten = true;
		}

		// Token: 0x06008F73 RID: 36723 RVA: 0x003AA8D0 File Offset: 0x003A8AD0
		public void Restore()
		{
			for (int i = 0; i < 15; i++)
			{
				Transform transform = this._channels[i].Transform;
				if (transform != null && this._channels[i].HasWritten && EggRotorTremble.Same(transform.localRotation, this._channels[i].Written))
				{
					transform.localRotation = this._channels[i].Base;
				}
				this._channels[i].HasWritten = false;
			}
			Transform transform2 = this._channels[0].Transform;
			if (transform2 != null && this._hipsHasWritten && EggRotorTremble.Same(transform2.localPosition, this._hipsWritten))
			{
				transform2.localPosition = this._hipsBase;
			}
			this._hipsHasWritten = false;
		}

		// Token: 0x06008F74 RID: 36724 RVA: 0x003AA9A8 File Offset: 0x003A8BA8
		private static void Capture(ref EggRotorTremble.Channel channel)
		{
			if (channel.Transform == null)
			{
				channel.HasWritten = false;
				return;
			}
			Quaternion localRotation = channel.Transform.localRotation;
			if (channel.HasWritten && EggRotorTremble.Same(localRotation, channel.Written))
			{
				channel.Transform.localRotation = channel.Base;
			}
			else
			{
				channel.Base = localRotation;
			}
			channel.HasWritten = false;
		}

		// Token: 0x06008F75 RID: 36725 RVA: 0x003AAA10 File Offset: 0x003A8C10
		private void CaptureHipsPosition(Transform hips)
		{
			Vector3 localPosition = hips.localPosition;
			if (this._hipsHasWritten && EggRotorTremble.Same(localPosition, this._hipsWritten))
			{
				hips.localPosition = this._hipsBase;
			}
			else
			{
				this._hipsBase = localPosition;
			}
			this._hipsHasWritten = false;
		}

		// Token: 0x06008F76 RID: 36726 RVA: 0x003AAA58 File Offset: 0x003A8C58
		private void AddLegRise(EggRotorTremble.Bone thighBone, EggRotorTremble.Bone shinBone, EggRotorTremble.Bone footBone, Vector3 pivot, Quaternion hipsDelta, Quaternion thighNet, Quaternion shinNet, Vector3 up, ref float rise, ref int legs)
		{
			Transform transform = this._channels[(int)thighBone].Transform;
			Transform transform2 = this._channels[(int)shinBone].Transform;
			Transform transform3 = this._channels[(int)footBone].Transform;
			if (transform == null || transform2 == null || transform3 == null)
			{
				return;
			}
			float num = EggRotorClimaxMath.LegRise(pivot, transform.position, transform2.position, transform3.position, hipsDelta, thighNet, shinNet, up);
			if (float.IsNaN(num) || float.IsInfinity(num))
			{
				return;
			}
			rise += num;
			legs++;
		}

		// Token: 0x06008F77 RID: 36727 RVA: 0x003AAAFC File Offset: 0x003A8CFC
		private void ApplyLeg(EggRotorTremble.Bone thighBone, EggRotorTremble.Bone shinBone, EggRotorTremble.Bone footBone, Quaternion hipsDelta, Quaternion thighNet, Quaternion shinNet, bool bend)
		{
			Transform transform = this._channels[(int)thighBone].Transform;
			if (transform == null)
			{
				return;
			}
			if (!bend)
			{
				transform.rotation = Quaternion.Inverse(hipsDelta) * transform.rotation;
				return;
			}
			transform.rotation = thighNet * Quaternion.Inverse(hipsDelta) * transform.rotation;
			Transform transform2 = this._channels[(int)shinBone].Transform;
			if (transform2 == null)
			{
				return;
			}
			transform2.rotation = shinNet * Quaternion.Inverse(thighNet) * transform2.rotation;
			Transform transform3 = this._channels[(int)footBone].Transform;
			if (transform3 == null)
			{
				return;
			}
			transform3.rotation = Quaternion.Inverse(shinNet) * transform3.rotation;
		}

		// Token: 0x06008F78 RID: 36728 RVA: 0x003AABD0 File Offset: 0x003A8DD0
		private void RotateWorld(EggRotorTremble.Bone bone, Quaternion delta)
		{
			Transform transform = this._channels[(int)bone].Transform;
			if (transform != null)
			{
				transform.rotation = delta * transform.rotation;
			}
		}

		// Token: 0x06008F79 RID: 36729 RVA: 0x003AAC0A File Offset: 0x003A8E0A
		private static Quaternion Rotation(float pitch, Vector3 right, float roll, Vector3 forward)
		{
			return Quaternion.AngleAxis(pitch, right) * Quaternion.AngleAxis(roll, forward);
		}

		// Token: 0x06008F7A RID: 36730 RVA: 0x003AAC20 File Offset: 0x003A8E20
		private static bool Same(Quaternion a, Quaternion b)
		{
			return Mathf.Abs(a.x - b.x) <= 1E-05f && Mathf.Abs(a.y - b.y) <= 1E-05f && Mathf.Abs(a.z - b.z) <= 1E-05f && Mathf.Abs(a.w - b.w) <= 1E-05f;
		}

		// Token: 0x06008F7B RID: 36731 RVA: 0x003AAC98 File Offset: 0x003A8E98
		private static bool Same(Vector3 a, Vector3 b)
		{
			return Mathf.Abs(a.x - b.x) <= 1E-06f && Mathf.Abs(a.y - b.y) <= 1E-06f && Mathf.Abs(a.z - b.z) <= 1E-06f;
		}

		// Token: 0x04009309 RID: 37641
		public const int BoneCount = 15;

		// Token: 0x0400930A RID: 37642
		private const float MaxDrop = 0.06f;

		// Token: 0x0400930B RID: 37643
		private const float MaxRise = 0.02f;

		// Token: 0x0400930C RID: 37644
		private const float RotationEpsilon = 1E-05f;

		// Token: 0x0400930D RID: 37645
		private const float PositionEpsilon = 1E-06f;

		// Token: 0x0400930E RID: 37646
		public static readonly HumanBodyBones[] HumanBones = new HumanBodyBones[]
		{
			HumanBodyBones.Hips,
			HumanBodyBones.Spine,
			HumanBodyBones.Chest,
			HumanBodyBones.Neck,
			HumanBodyBones.Head,
			HumanBodyBones.LeftUpperLeg,
			HumanBodyBones.LeftLowerLeg,
			HumanBodyBones.LeftFoot,
			HumanBodyBones.RightUpperLeg,
			HumanBodyBones.RightLowerLeg,
			HumanBodyBones.RightFoot,
			HumanBodyBones.LeftUpperArm,
			HumanBodyBones.RightUpperArm,
			HumanBodyBones.LeftHand,
			HumanBodyBones.RightHand
		};

		// Token: 0x0400930F RID: 37647
		private readonly EggRotorTremble.Channel[] _channels = new EggRotorTremble.Channel[15];

		// Token: 0x04009310 RID: 37648
		private Transform _root;

		// Token: 0x04009311 RID: 37649
		private Vector3 _hipsBase;

		// Token: 0x04009312 RID: 37650
		private Vector3 _hipsWritten;

		// Token: 0x04009313 RID: 37651
		private bool _hipsHasWritten;

		// Token: 0x0200121F RID: 4639
		public enum Bone
		{
			// Token: 0x04009315 RID: 37653
			Hips,
			// Token: 0x04009316 RID: 37654
			Spine,
			// Token: 0x04009317 RID: 37655
			Chest,
			// Token: 0x04009318 RID: 37656
			Neck,
			// Token: 0x04009319 RID: 37657
			Head,
			// Token: 0x0400931A RID: 37658
			LeftUpperLeg,
			// Token: 0x0400931B RID: 37659
			LeftLowerLeg,
			// Token: 0x0400931C RID: 37660
			LeftFoot,
			// Token: 0x0400931D RID: 37661
			RightUpperLeg,
			// Token: 0x0400931E RID: 37662
			RightLowerLeg,
			// Token: 0x0400931F RID: 37663
			RightFoot,
			// Token: 0x04009320 RID: 37664
			LeftUpperArm,
			// Token: 0x04009321 RID: 37665
			RightUpperArm,
			// Token: 0x04009322 RID: 37666
			LeftHand,
			// Token: 0x04009323 RID: 37667
			RightHand
		}

		// Token: 0x02001220 RID: 4640
		private struct Channel
		{
			// Token: 0x04009324 RID: 37668
			public Transform Transform;

			// Token: 0x04009325 RID: 37669
			public Quaternion Base;

			// Token: 0x04009326 RID: 37670
			public Quaternion Written;

			// Token: 0x04009327 RID: 37671
			public bool HasWritten;
		}
	}
}
