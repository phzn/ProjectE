using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B21 RID: 2849
	public sealed class BenchAnimationPlayer : IDisposable
	{
		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060050C9 RID: 20681 RVA: 0x001F3ED4 File Offset: 0x001F20D4
		public bool IsValid
		{
			get
			{
				return !this.disposed && this.animator != null && this.animator.isHuman && this.animations != null && this.animations.IsValid(this.female, this.npcHands) && (this.seat == null || this.animations.HasAuthoredMotion(this.female));
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060050CA RID: 20682 RVA: 0x001F3F50 File Offset: 0x001F2150
		private float SitClipDuration
		{
			get
			{
				if (!(this.animations != null) || !(this.animations.GetSitDown(this.female, this.npcHands) != null))
				{
					return 0f;
				}
				return this.animations.GetSitDown(this.female, this.npcHands).length;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x001F3FAC File Offset: 0x001F21AC
		private float StandClipDuration
		{
			get
			{
				if (!(this.animations != null) || !(this.animations.GetStandUp(this.female, this.npcHands) != null))
				{
					return 0f;
				}
				return this.animations.GetStandUp(this.female, this.npcHands).length;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060050CC RID: 20684 RVA: 0x001F4008 File Offset: 0x001F2208
		public float SitDownDuration
		{
			get
			{
				return this.SitClipDuration / 1.75f;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x001F4016 File Offset: 0x001F2216
		public float StandUpDuration
		{
			get
			{
				return this.StandClipDuration / 1.75f;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x001F4024 File Offset: 0x001F2224
		public float ReleaseDuration
		{
			get
			{
				return 0.12f;
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x001F402C File Offset: 0x001F222C
		public bool IsComplete
		{
			get
			{
				if (this.stage == BenchAnimationPlayer.Stage.Enter)
				{
					return this.elapsed >= this.SitDownDuration;
				}
				if (this.stage != BenchAnimationPlayer.Stage.Exit)
				{
					return this.stage == BenchAnimationPlayer.Stage.Release && this.elapsed >= this.ReleaseDuration;
				}
				return this.elapsed >= this.StandUpDuration;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x001F408B File Offset: 0x001F228B
		public Vector3 EntryPosition
		{
			get
			{
				return this.RootOrigin + this.Facing * this.Entrance.Position(0f) * this.motionScale;
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x060050D1 RID: 20689 RVA: 0x001F40BE File Offset: 0x001F22BE
		public Quaternion EntryRotation
		{
			get
			{
				return this.Facing * this.Entrance.Rotation(0f);
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x060050D2 RID: 20690 RVA: 0x001F40DB File Offset: 0x001F22DB
		public Vector3 ExitPosition
		{
			get
			{
				return this.RootOrigin + this.Facing * this.Exit.Position(this.StandClipDuration) * (this.motionScale * this.exitScale);
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x001F4116 File Offset: 0x001F2316
		public Quaternion ExitRotation
		{
			get
			{
				return this.Facing * this.Exit.Rotation(this.StandClipDuration);
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x060050D4 RID: 20692 RVA: 0x001F4134 File Offset: 0x001F2334
		// (set) Token: 0x060050D5 RID: 20693 RVA: 0x001F413C File Offset: 0x001F233C
		public Vector3 RootPosition { get; private set; }

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x060050D6 RID: 20694 RVA: 0x001F4145 File Offset: 0x001F2345
		// (set) Token: 0x060050D7 RID: 20695 RVA: 0x001F414D File Offset: 0x001F234D
		public Quaternion RootRotation { get; private set; }

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x060050D8 RID: 20696 RVA: 0x001F4156 File Offset: 0x001F2356
		public Vector3 MeasuredSupportLocal
		{
			get
			{
				return this.supportLocal;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x001F415E File Offset: 0x001F235E
		public float ForwardFit
		{
			get
			{
				return this.forwardFit;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x001F4166 File Offset: 0x001F2366
		private BenchMotionTrack Entrance
		{
			get
			{
				return this.animations.GetEntrance(this.female);
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x060050DB RID: 20699 RVA: 0x001F4179 File Offset: 0x001F2379
		private BenchMotionTrack Exit
		{
			get
			{
				return this.animations.GetExit(this.female);
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x060050DC RID: 20700 RVA: 0x001F418C File Offset: 0x001F238C
		private Quaternion Facing
		{
			get
			{
				if (!(this.seat != null))
				{
					return this.originalRotation;
				}
				return this.seat.CushionRotation;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060050DD RID: 20701 RVA: 0x001F41B0 File Offset: 0x001F23B0
		private Vector3 RootOrigin
		{
			get
			{
				if (this.seat == null)
				{
					return this.originalPosition;
				}
				Vector3 vector = this.seat.CushionPosition - this.Facing * new Vector3(this.supportLocal.x, 0f, this.supportLocal.z) * this.rootScale;
				vector += this.Facing * Vector3.forward * this.forwardFit;
				vector.y = this.seat.SeatPosition.y;
				return vector;
			}
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x001F4254 File Offset: 0x001F2454
		public BenchAnimationPlayer(Animator animator, BenchSeat seat, bool female, float standingClearance = 0.29f, bool npcHands = false)
			: this(animator, (seat != null) ? seat.Animations : null, female, 0f, npcHands)
		{
			this.seat = seat;
			if (!this.IsValid || seat == null)
			{
				return;
			}
			this.footGrounding = new BenchFootGrounding(animator, seat);
			this.EnsureGraph();
			this.MeasureSupport();
			float num = this.Exit.Position(this.StandClipDuration).z * this.motionScale;
			float num2 = -this.supportLocal.z * this.rootScale + this.forwardFit;
			float num3 = seat.frontEdgeDistance + Mathf.Max(0.29f, standingClearance) - num2;
			if (num > 0.02f)
			{
				this.exitScale = Mathf.Clamp(num3 / num, 1f, 1.8f);
			}
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x001F4324 File Offset: 0x001F2524
		public BenchAnimationPlayer(Animator animator, BenchAnimationSet animations, bool female, float sittingHeightOffset = 0f, bool npcHands = false)
		{
			this.animator = animator;
			this.animations = animations;
			this.female = female;
			this.npcHands = npcHands;
			this.legacyHeightOffset = sittingHeightOffset;
			if (animator == null)
			{
				return;
			}
			this.originalPosition = (this.RootPosition = animator.transform.position);
			this.originalRotation = (this.RootRotation = animator.transform.rotation);
			this.previousRootMotion = animator.applyRootMotion;
			this.previousCulling = animator.cullingMode;
			this.rootScale = Mathf.Abs(animator.transform.lossyScale.y);
			this.motionScale = animator.humanScale * this.rootScale;
			if (animator.isHuman)
			{
				this.hips = animator.GetBoneTransform(HumanBodyBones.Hips);
				if (!npcHands)
				{
					this.upperPoseTransition = BenchArmPoseTransition.ForAnimator(animator);
				}
			}
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x001F4450 File Offset: 0x001F2650
		private void EnsureGraph()
		{
			if (this.graph.IsValid())
			{
				return;
			}
			this.SetRootMotionWithRigHandoff(false, null);
			this.animator.Update(0f);
			this.graph = this.animator.playableGraph;
			if (this.graph.IsValid())
			{
				for (int i = 0; i < this.graph.GetOutputCount(); i++)
				{
					PlayableOutput playableOutput = this.graph.GetOutput(i);
					if (!(playableOutput.GetPlayableOutputType() != typeof(AnimationPlayableOutput)))
					{
						AnimationPlayableOutput animationPlayableOutput = (AnimationPlayableOutput)playableOutput;
						if (!(animationPlayableOutput.GetTarget() != this.animator))
						{
							this.originalSource = animationPlayableOutput.GetSourcePlayable<AnimationPlayableOutput>();
							this.originalPort = animationPlayableOutput.GetSourceOutputPort<AnimationPlayableOutput>();
							break;
						}
					}
				}
			}
			if (!this.graph.IsValid() || !this.originalSource.IsValid<Playable>())
			{
				this.graph = PlayableGraph.Create("Bench pose: " + this.animator.name);
				this.graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
				this.output = AnimationPlayableOutput.Create(this.graph, "Bench pose", this.animator);
				this.ownsGraph = true;
			}
			else
			{
				this.output = AnimationPlayableOutput.Create(this.graph, "Bench body and arms overlay", this.animator);
				this.output.SetSortingOrder(10);
			}
			this.mixer = AnimationMixerPlayable.Create(this.graph, 2);
			this.armsMixer = AnimationMixerPlayable.Create(this.graph, 2);
			this.layers = AnimationLayerMixerPlayable.Create(this.graph, 3);
			if (this.originalSource.IsValid<Playable>())
			{
				this.graph.Connect<Playable, AnimationLayerMixerPlayable>(this.originalSource, this.originalPort, this.layers, 0);
				this.layers.SetInputWeight(0, 1f);
			}
			this.bodyMask = BenchAnimationPlayer.MakeMask(false);
			this.armsMask = BenchAnimationPlayer.MakeMask(true);
			this.graph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this.mixer, 0, this.layers, 1);
			this.graph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this.armsMixer, 0, this.layers, 2);
			this.layers.SetLayerMaskFromAvatarMask(1U, this.bodyMask);
			this.layers.SetLayerMaskFromAvatarMask(2U, this.armsMask);
			this.layers.SetInputWeight(1, 0f);
			this.layers.SetInputWeight(2, 0f);
			this.output.SetSourcePlayable(this.layers);
			if (this.ownsGraph)
			{
				this.graph.Play();
			}
			this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x001F46DC File Offset: 0x001F28DC
		private static AvatarMask MakeMask(bool arms)
		{
			AvatarMask avatarMask = new AvatarMask
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			for (int i = 0; i < 13; i++)
			{
				AvatarMaskBodyPart avatarMaskBodyPart = (AvatarMaskBodyPart)i;
				bool flag = avatarMaskBodyPart == AvatarMaskBodyPart.LeftArm || avatarMaskBodyPart == AvatarMaskBodyPart.RightArm || avatarMaskBodyPart == AvatarMaskBodyPart.LeftFingers || avatarMaskBodyPart == AvatarMaskBodyPart.RightFingers;
				bool flag2 = avatarMaskBodyPart == AvatarMaskBodyPart.Root || avatarMaskBodyPart == AvatarMaskBodyPart.Body || avatarMaskBodyPart == AvatarMaskBodyPart.LeftLeg || avatarMaskBodyPart == AvatarMaskBodyPart.RightLeg || avatarMaskBodyPart == AvatarMaskBodyPart.LeftFootIK || avatarMaskBodyPart == AvatarMaskBodyPart.RightFootIK;
				avatarMask.SetHumanoidBodyPartActive(avatarMaskBodyPart, arms ? flag : flag2);
			}
			return avatarMask;
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x001F4748 File Offset: 0x001F2948
		private void MeasureSupport()
		{
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(this.graph, this.animations.GetIdle(this.female, this.npcHands));
			AnimationClipPlayable animationClipPlayable2 = AnimationClipPlayable.Create(this.graph, this.animations.GetIdle(this.female, this.npcHands));
			animationClipPlayable.SetSpeed(0.0);
			animationClipPlayable.SetApplyFootIK(false);
			animationClipPlayable2.SetSpeed(0.0);
			animationClipPlayable2.SetApplyFootIK(false);
			this.graph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(animationClipPlayable, 0, this.mixer, 0);
			this.graph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(animationClipPlayable2, 0, this.armsMixer, 0);
			this.mixer.SetInputWeight(0, 1f);
			this.armsMixer.SetInputWeight(0, 1f);
			this.layers.SetInputWeight(1, 1f);
			this.layers.SetInputWeight(2, 1f);
			this.graph.Evaluate(0f);
			Vector3 vector = this.animator.transform.InverseTransformPoint(this.hips.position);
			this.supportLocal = vector - Vector3.up * (0.045f * this.animator.humanScale);
			this.supportLocal.z = this.supportLocal.z - 0.025f * this.animator.humanScale;
			List<float> list = new List<float>();
			Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
			Transform boneTransform2 = this.animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.animator.GetComponentsInChildren<SkinnedMeshRenderer>(false))
			{
				if (skinnedMeshRenderer.enabled && !(skinnedMeshRenderer.sharedMesh == null))
				{
					bool flag = false;
					bool flag2 = false;
					foreach (Transform transform in skinnedMeshRenderer.bones)
					{
						if (transform == this.hips || transform == boneTransform || transform == boneTransform2)
						{
							flag = true;
						}
						if (this.footGrounding != null && this.footGrounding.UsesFoot(transform))
						{
							flag2 = true;
						}
					}
					if (flag || flag2)
					{
						Mesh mesh = new Mesh();
						try
						{
							skinnedMeshRenderer.BakeMesh(mesh);
							foreach (Vector3 vector2 in mesh.vertices)
							{
								Vector3 vector3 = skinnedMeshRenderer.transform.TransformPoint(vector2);
								if (flag2)
								{
									BenchFootGrounding benchFootGrounding = this.footGrounding;
									if (benchFootGrounding != null)
									{
										benchFootGrounding.MeasureVertex(vector3);
									}
								}
								if (flag)
								{
									Vector3 vector4 = this.animator.transform.InverseTransformPoint(vector3);
									Vector3 vector5 = (vector4 - vector) / Mathf.Max(0.01f, this.animator.humanScale);
									if (Mathf.Abs(vector5.x) < 0.21f && vector5.z > -0.19f && vector5.z < -0.015f && vector5.y > -0.23f && vector5.y < 0.02f)
									{
										list.Add(vector4.y);
									}
								}
							}
						}
						finally
						{
							BenchAnimationPlayer.DestroyObject(mesh);
						}
					}
				}
			}
			if (list.Count >= 12)
			{
				list.Sort();
				this.supportLocal.y = list[Mathf.FloorToInt((float)(list.Count - 1) * 0.04f)] + 0.008f;
			}
			if (this.seat != null)
			{
				float num = Mathf.Min(this.RearCalfAtSeatHeight(HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot), this.RearCalfAtSeatHeight(HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot));
				float num2 = this.seat.frontEdgeDistance + 0.02f - (num - this.supportLocal.z) * this.rootScale;
				this.forwardFit = Mathf.Clamp(num2, 0f, Mathf.Max(0f, this.seat.frontEdgeDistance - 0.08f));
			}
			this.graph.Disconnect<AnimationMixerPlayable>(this.mixer, 0);
			this.graph.Disconnect<AnimationMixerPlayable>(this.armsMixer, 0);
			this.graph.DestroyPlayable<AnimationClipPlayable>(animationClipPlayable);
			this.graph.DestroyPlayable<AnimationClipPlayable>(animationClipPlayable2);
			this.layers.SetInputWeight(1, 0f);
			this.layers.SetInputWeight(2, 0f);
			this.graph.Evaluate(0f);
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x001F4BC8 File Offset: 0x001F2DC8
		private float RearCalfAtSeatHeight(HumanBodyBones kneeBone, HumanBodyBones ankleBone)
		{
			Transform boneTransform = this.animator.GetBoneTransform(kneeBone);
			Transform boneTransform2 = this.animator.GetBoneTransform(ankleBone);
			if (boneTransform == null || boneTransform2 == null)
			{
				return float.PositiveInfinity;
			}
			Vector3 vector = this.animator.transform.InverseTransformPoint(boneTransform.position);
			Vector3 vector2 = this.animator.transform.InverseTransformPoint(boneTransform2.position);
			float num = vector2.y - vector.y;
			if (Mathf.Abs(num) < 0.0001f)
			{
				return Mathf.Min(vector.z, vector2.z) - 0.045f * this.animator.humanScale;
			}
			float num2 = (this.supportLocal.y - 0.25f / this.rootScale - vector.y) / num;
			float num3 = (this.supportLocal.y + 0.025f / this.rootScale - vector.y) / num;
			float num4 = Mathf.Max(0f, Mathf.Min(num2, num3));
			float num5 = Mathf.Min(1f, Mathf.Max(num2, num3));
			if (num4 > num5)
			{
				return float.PositiveInfinity;
			}
			return Mathf.Min(Mathf.Lerp(vector.z, vector2.z, num4), Mathf.Lerp(vector.z, vector2.z, num5)) - 0.045f * this.animator.humanScale;
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x001F4D31 File Offset: 0x001F2F31
		public bool PlaySitDown()
		{
			return this.Play((this.animations != null) ? this.animations.GetSitDown(this.female, this.npcHands) : null, BenchAnimationPlayer.Stage.Enter);
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x001F4D62 File Offset: 0x001F2F62
		public void PlayIdle()
		{
			this.Play((this.animations != null) ? this.animations.GetIdle(this.female, this.npcHands) : null, BenchAnimationPlayer.Stage.Idle);
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x001F4D94 File Offset: 0x001F2F94
		public bool PlayStandUp()
		{
			return this.Play((this.animations != null) ? this.animations.GetStandUp(this.female, this.npcHands) : null, BenchAnimationPlayer.Stage.Exit);
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x001F4DC5 File Offset: 0x001F2FC5
		public void SetUpperBodyEnabled(bool enabled, int transitionKey = 0, bool nativeDestinationReady = true)
		{
			this.upperEnabled = enabled;
			this.upperTransitionKey = transitionKey;
			this.upperDestinationReady = nativeDestinationReady;
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x001F4DDC File Offset: 0x001F2FDC
		public void CompleteUpperBodyPose()
		{
			BenchArmPoseTransition benchArmPoseTransition = this.upperPoseTransition;
			if (benchArmPoseTransition == null)
			{
				return;
			}
			benchArmPoseTransition.Apply(this.lastDelta);
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x001F4DF4 File Offset: 0x001F2FF4
		public void BeginRelease()
		{
			if (this.disposed || !this.graph.IsValid())
			{
				return;
			}
			this.stage = BenchAnimationPlayer.Stage.Release;
			this.elapsed = 0f;
			this.releaseStartWeight = this.bodyWeight;
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x001F4E2C File Offset: 0x001F302C
		private bool Play(AnimationClip source, BenchAnimationPlayer.Stage nextStage)
		{
			if (!this.IsValid || source == null)
			{
				return false;
			}
			this.EnsureGraph();
			int num = ((this.active == 0) ? 1 : 0);
			if (this.clips[num].IsValid<AnimationClipPlayable>())
			{
				this.graph.Disconnect<AnimationMixerPlayable>(this.mixer, num);
				this.graph.DestroyPlayable<AnimationClipPlayable>(this.clips[num]);
			}
			if (this.armClips[num].IsValid<AnimationClipPlayable>())
			{
				this.graph.Disconnect<AnimationMixerPlayable>(this.armsMixer, num);
				this.graph.DestroyPlayable<AnimationClipPlayable>(this.armClips[num]);
			}
			this.sources[num] = source;
			this.times[num] = 0.0;
			this.clips[num] = AnimationClipPlayable.Create(this.graph, source);
			this.clips[num].SetApplyFootIK(false);
			this.clips[num].SetApplyPlayableIK(false);
			this.clips[num].SetSpeed(0.0);
			this.armClips[num] = AnimationClipPlayable.Create(this.graph, source);
			this.armClips[num].SetApplyFootIK(false);
			this.armClips[num].SetApplyPlayableIK(false);
			this.armClips[num].SetSpeed(0.0);
			this.graph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this.clips[num], 0, this.mixer, num);
			this.graph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this.armClips[num], 0, this.armsMixer, num);
			this.mixer.SetInputWeight(num, (float)((this.active < 0) ? 1 : 0));
			this.armsMixer.SetInputWeight(num, (float)((this.active < 0) ? 1 : 0));
			this.active = num;
			this.blendElapsed = ((this.sources[1 - this.active] == null) ? this.animations.blendDuration : 0f);
			this.stage = nextStage;
			this.elapsed = 0f;
			this.Tick(0f);
			return true;
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x001F5068 File Offset: 0x001F3268
		public void Tick(float deltaTime)
		{
			if (this.disposed || !this.graph.IsValid() || this.animator == null || this.active < 0)
			{
				return;
			}
			float num = Mathf.Max(0f, deltaTime);
			this.lastDelta = num;
			this.elapsed += num;
			this.blendElapsed += num;
			for (int i = 0; i < this.clips.Length; i++)
			{
				if (this.clips[i].IsValid<AnimationClipPlayable>() && !(this.sources[i] == null))
				{
					bool flag = this.sources[i] == this.animations.GetSitDown(this.female, this.npcHands) || this.sources[i] == this.animations.GetStandUp(this.female, this.npcHands);
					if (this.stage != BenchAnimationPlayer.Stage.Release)
					{
						this.times[i] += (double)(num * (flag ? 1.75f : 1f));
					}
					double num2 = Math.Max(0.001, (double)this.sources[i].length);
					this.clips[i].SetTime(this.sources[i].isLooping ? (this.times[i] % num2) : Math.Min(this.times[i], num2 - 1E-05));
					this.armClips[i].SetTime(this.sources[i].isLooping ? (this.times[i] % num2) : Math.Min(this.times[i], num2 - 1E-05));
				}
			}
			float num3 = this.animations.blendDuration / 1.75f;
			float num4 = ((num3 <= 0f) ? 1f : Mathf.Clamp01(this.blendElapsed / num3));
			this.mixer.SetInputWeight(this.active, num4);
			this.mixer.SetInputWeight(1 - this.active, 1f - num4);
			this.armsMixer.SetInputWeight(this.active, num4);
			this.armsMixer.SetInputWeight(1 - this.active, 1f - num4);
			this.bodyWeight = ((this.stage == BenchAnimationPlayer.Stage.Release) ? (this.releaseStartWeight * (1f - Mathf.SmoothStep(0f, 1f, this.elapsed / this.ReleaseDuration))) : Mathf.MoveTowards(this.bodyWeight, 1f, num / Mathf.Max(0.01f, num3)));
			bool flag2 = this.upperEnabled && this.stage == BenchAnimationPlayer.Stage.Idle;
			BenchArmPoseTransition benchArmPoseTransition = this.upperPoseTransition;
			if (benchArmPoseTransition != null)
			{
				benchArmPoseTransition.SetTarget(flag2, this.upperTransitionKey, this.upperDestinationReady);
			}
			this.layers.SetInputWeight(1, this.bodyWeight);
			this.layers.SetInputWeight(2, flag2 ? 0f : this.bodyWeight);
			float num5 = this.elapsed * 1.75f;
			float num6 = ((this.stage == BenchAnimationPlayer.Stage.Enter) ? this.Entrance.Contact(num5) : ((this.stage == BenchAnimationPlayer.Stage.Exit) ? this.Exit.Contact(num5) : ((float)((this.stage == BenchAnimationPlayer.Stage.Idle) ? 1 : 0))));
			if (this.seat != null)
			{
				Vector3 vector = ((this.stage == BenchAnimationPlayer.Stage.Enter) ? this.Entrance.Position(Mathf.Min(num5, this.SitClipDuration)) : ((this.stage == BenchAnimationPlayer.Stage.Idle) ? Vector3.zero : (this.Exit.Position((this.stage == BenchAnimationPlayer.Stage.Release) ? this.StandClipDuration : Mathf.Min(num5, this.StandClipDuration)) * this.exitScale)));
				Quaternion quaternion = ((this.stage == BenchAnimationPlayer.Stage.Enter) ? this.Entrance.Rotation(Mathf.Min(num5, this.SitClipDuration)) : ((this.stage == BenchAnimationPlayer.Stage.Idle) ? Quaternion.identity : this.Exit.Rotation((this.stage == BenchAnimationPlayer.Stage.Release) ? this.StandClipDuration : Mathf.Min(num5, this.StandClipDuration))));
				this.RootPosition = this.RootOrigin + this.Facing * vector * this.motionScale;
				this.RootRotation = this.Facing * quaternion;
				this.animator.transform.SetPositionAndRotation(this.RootPosition, this.RootRotation);
			}
			if (this.animator.applyRootMotion)
			{
				this.animator.applyRootMotion = false;
			}
			BenchFootGrounding benchFootGrounding = this.footGrounding;
			if (benchFootGrounding != null)
			{
				benchFootGrounding.Restore();
			}
			this.RestoreHipCorrection();
			this.graph.Evaluate(0f);
			if (this.seat != null)
			{
				this.animator.transform.SetPositionAndRotation(this.RootPosition, this.RootRotation);
			}
			if (this.hips != null)
			{
				float num7 = ((this.seat != null) ? (this.seat.CushionPosition.y - (this.RootOrigin.y + this.supportLocal.y * this.rootScale)) : this.legacyHeightOffset);
				this.uncorrectedHipsPosition = this.hips.localPosition;
				this.hips.position += Vector3.up * (num7 * num6 * this.bodyWeight);
				this.correctedHipsPosition = this.hips.localPosition;
				this.hipsCorrected = true;
			}
			BenchFootGrounding benchFootGrounding2 = this.footGrounding;
			if (benchFootGrounding2 == null)
			{
				return;
			}
			benchFootGrounding2.Apply(num6 * this.bodyWeight);
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x001F5638 File Offset: 0x001F3838
		private void RestoreHipCorrection()
		{
			if (this.hipsCorrected && this.hips != null && (this.hips.localPosition - this.correctedHipsPosition).sqrMagnitude < 1E-07f)
			{
				this.hips.localPosition = this.uncorrectedHipsPosition;
			}
			this.hipsCorrected = false;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x001F5698 File Offset: 0x001F3898
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			BenchFootGrounding benchFootGrounding = this.footGrounding;
			if (benchFootGrounding != null)
			{
				benchFootGrounding.Restore();
			}
			this.RestoreHipCorrection();
			bool flag = this.graph.IsValid();
			BenchAnimatorStateSnapshot benchAnimatorStateSnapshot = ((flag && this.animator != null) ? BenchAnimatorStateSnapshot.Capture(this.animator) : null);
			if (flag)
			{
				if (this.ownsGraph)
				{
					this.graph.Destroy();
				}
				else
				{
					if (this.output.IsOutputValid<AnimationPlayableOutput>())
					{
						this.graph.DestroyOutput<AnimationPlayableOutput>(this.output);
					}
					if (this.layers.IsValid<AnimationLayerMixerPlayable>())
					{
						this.graph.DestroyPlayable<AnimationLayerMixerPlayable>(this.layers);
					}
					if (this.mixer.IsValid<AnimationMixerPlayable>())
					{
						this.graph.DestroyPlayable<AnimationMixerPlayable>(this.mixer);
					}
					if (this.armsMixer.IsValid<AnimationMixerPlayable>())
					{
						this.graph.DestroyPlayable<AnimationMixerPlayable>(this.armsMixer);
					}
					foreach (AnimationClipPlayable animationClipPlayable in this.clips)
					{
						if (animationClipPlayable.IsValid<AnimationClipPlayable>())
						{
							this.graph.DestroyPlayable<AnimationClipPlayable>(animationClipPlayable);
						}
					}
					foreach (AnimationClipPlayable animationClipPlayable2 in this.armClips)
					{
						if (animationClipPlayable2.IsValid<AnimationClipPlayable>())
						{
							this.graph.DestroyPlayable<AnimationClipPlayable>(animationClipPlayable2);
						}
					}
				}
			}
			BenchAnimationPlayer.DestroyObject(this.bodyMask);
			BenchAnimationPlayer.DestroyObject(this.armsMask);
			if (this.animator == null)
			{
				return;
			}
			this.SetRootMotionWithRigHandoff(this.previousRootMotion, benchAnimatorStateSnapshot);
			this.animator.cullingMode = this.previousCulling;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x001F5834 File Offset: 0x001F3A34
		private void SetRootMotionWithRigHandoff(bool value, BenchAnimatorStateSnapshot nativeState = null)
		{
			if (this.animator == null || (this.animator.applyRootMotion == value && nativeState == null))
			{
				return;
			}
			RigBuilder component = this.animator.GetComponent<RigBuilder>();
			bool flag = component != null && component.isActiveAndEnabled && component.graph.IsValid();
			if (!flag && nativeState == null)
			{
				this.animator.applyRootMotion = value;
				return;
			}
			BenchAnimationPlayer.RigLayerState[] array = new BenchAnimationPlayer.RigLayerState[flag ? component.layers.Count : 0];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new BenchAnimationPlayer.RigLayerState(component.layers[i]);
			}
			bool flag2 = flag && component.graph.IsPlaying();
			DirectorUpdateMode directorUpdateMode = (flag ? component.graph.GetTimeUpdateMode() : DirectorUpdateMode.GameTime);
			Vector3 position = this.animator.transform.position;
			Quaternion rotation = this.animator.transform.rotation;
			Vector3 localScale = this.animator.transform.localScale;
			try
			{
				if (flag)
				{
					component.Clear();
				}
				this.animator.applyRootMotion = value;
				if (nativeState != null)
				{
					this.animator.Rebind();
					nativeState.RestoreBeforeEvaluation();
				}
				this.animator.Update(0f);
				if (nativeState != null)
				{
					nativeState.RestoreAfterEvaluation();
				}
			}
			finally
			{
				if (this.animator != null)
				{
					this.animator.transform.SetPositionAndRotation(position, rotation);
					this.animator.transform.localScale = localScale;
				}
				BenchAnimationPlayer.RigLayerState[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Restore();
				}
				if (flag && component != null && component.isActiveAndEnabled)
				{
					bool flag3 = component.Build();
					array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j].Restore();
					}
					if (flag3)
					{
						component.graph.SetTimeUpdateMode(directorUpdateMode);
						if (!flag2)
						{
							component.graph.Stop();
						}
						component.SyncLayers();
					}
					else
					{
						Debug.LogWarning("[BenchAnimationPlayer] Could not restore the existing animation rig after the bench handoff.", this.animator);
					}
				}
			}
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyObject(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0400514D RID: 20813
		public const float TransitionPlaybackSpeed = 1.75f;

		// Token: 0x0400514E RID: 20814
		private readonly Animator animator;

		// Token: 0x0400514F RID: 20815
		private readonly BenchAnimationSet animations;

		// Token: 0x04005150 RID: 20816
		private readonly BenchSeat seat;

		// Token: 0x04005151 RID: 20817
		private readonly bool female;

		// Token: 0x04005152 RID: 20818
		private readonly bool npcHands;

		// Token: 0x04005153 RID: 20819
		private readonly BenchArmPoseTransition upperPoseTransition;

		// Token: 0x04005154 RID: 20820
		private readonly float legacyHeightOffset;

		// Token: 0x04005155 RID: 20821
		private readonly Transform hips;

		// Token: 0x04005156 RID: 20822
		private readonly BenchFootGrounding footGrounding;

		// Token: 0x04005157 RID: 20823
		private readonly bool previousRootMotion;

		// Token: 0x04005158 RID: 20824
		private readonly AnimatorCullingMode previousCulling;

		// Token: 0x04005159 RID: 20825
		private readonly Vector3 originalPosition;

		// Token: 0x0400515A RID: 20826
		private readonly Quaternion originalRotation;

		// Token: 0x0400515B RID: 20827
		private PlayableGraph graph;

		// Token: 0x0400515C RID: 20828
		private AnimationPlayableOutput output;

		// Token: 0x0400515D RID: 20829
		private Playable originalSource;

		// Token: 0x0400515E RID: 20830
		private int originalPort;

		// Token: 0x0400515F RID: 20831
		private bool ownsGraph;

		// Token: 0x04005160 RID: 20832
		private AnimationMixerPlayable mixer;

		// Token: 0x04005161 RID: 20833
		private AnimationMixerPlayable armsMixer;

		// Token: 0x04005162 RID: 20834
		private AnimationLayerMixerPlayable layers;

		// Token: 0x04005163 RID: 20835
		private AvatarMask bodyMask;

		// Token: 0x04005164 RID: 20836
		private AvatarMask armsMask;

		// Token: 0x04005165 RID: 20837
		private readonly AnimationClipPlayable[] clips = new AnimationClipPlayable[2];

		// Token: 0x04005166 RID: 20838
		private readonly AnimationClipPlayable[] armClips = new AnimationClipPlayable[2];

		// Token: 0x04005167 RID: 20839
		private readonly AnimationClip[] sources = new AnimationClip[2];

		// Token: 0x04005168 RID: 20840
		private readonly double[] times = new double[2];

		// Token: 0x04005169 RID: 20841
		private int active = -1;

		// Token: 0x0400516A RID: 20842
		private BenchAnimationPlayer.Stage stage;

		// Token: 0x0400516B RID: 20843
		private float elapsed;

		// Token: 0x0400516C RID: 20844
		private float blendElapsed;

		// Token: 0x0400516D RID: 20845
		private float releaseStartWeight;

		// Token: 0x0400516E RID: 20846
		private float bodyWeight;

		// Token: 0x0400516F RID: 20847
		private float lastDelta;

		// Token: 0x04005170 RID: 20848
		private int upperTransitionKey;

		// Token: 0x04005171 RID: 20849
		private bool upperEnabled;

		// Token: 0x04005172 RID: 20850
		private bool upperDestinationReady = true;

		// Token: 0x04005173 RID: 20851
		private bool disposed;

		// Token: 0x04005174 RID: 20852
		private Vector3 supportLocal;

		// Token: 0x04005175 RID: 20853
		private float rootScale;

		// Token: 0x04005176 RID: 20854
		private float motionScale;

		// Token: 0x04005177 RID: 20855
		private float exitScale = 1f;

		// Token: 0x04005178 RID: 20856
		private float forwardFit;

		// Token: 0x04005179 RID: 20857
		private Vector3 uncorrectedHipsPosition;

		// Token: 0x0400517A RID: 20858
		private Vector3 correctedHipsPosition;

		// Token: 0x0400517B RID: 20859
		private bool hipsCorrected;

		// Token: 0x02000B22 RID: 2850
		private enum Stage
		{
			// Token: 0x0400517F RID: 20863
			None,
			// Token: 0x04005180 RID: 20864
			Enter,
			// Token: 0x04005181 RID: 20865
			Idle,
			// Token: 0x04005182 RID: 20866
			Exit,
			// Token: 0x04005183 RID: 20867
			Release
		}

		// Token: 0x02000B23 RID: 2851
		private sealed class RigLayerState
		{
			// Token: 0x060050F0 RID: 20720 RVA: 0x001F5A74 File Offset: 0x001F3C74
			public RigLayerState(RigLayer layer)
			{
				this.layer = layer;
				if (layer == null)
				{
					return;
				}
				this.active = layer.active;
				this.rigWeight = ((layer.rig != null) ? layer.rig.weight : 0f);
				if (layer.rig != null)
				{
					Transform[] componentsInChildren = layer.rig.GetComponentsInChildren<Transform>(true);
					this.transforms = new BenchAnimationPlayer.RigTransformState[componentsInChildren.Length];
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						this.transforms[i] = new BenchAnimationPlayer.RigTransformState(componentsInChildren[i]);
					}
				}
				this.constraints = layer.constraints;
				if (this.constraints == null)
				{
					return;
				}
				this.constraintWeights = new float[this.constraints.Length];
				for (int j = 0; j < this.constraints.Length; j++)
				{
					this.constraintWeights[j] = this.constraints[j].weight;
				}
			}

			// Token: 0x060050F1 RID: 20721 RVA: 0x001F5B60 File Offset: 0x001F3D60
			public void Restore()
			{
				if (this.layer == null)
				{
					return;
				}
				this.layer.active = this.active;
				if (this.layer.rig != null)
				{
					this.layer.rig.weight = this.rigWeight;
				}
				if (this.transforms != null)
				{
					foreach (BenchAnimationPlayer.RigTransformState rigTransformState in this.transforms)
					{
						rigTransformState.Restore();
					}
				}
				if (this.constraints == null)
				{
					return;
				}
				for (int j = 0; j < this.constraints.Length; j++)
				{
					if (this.constraints[j].component != null)
					{
						this.constraints[j].weight = this.constraintWeights[j];
					}
				}
			}

			// Token: 0x04005184 RID: 20868
			private readonly RigLayer layer;

			// Token: 0x04005185 RID: 20869
			private readonly bool active;

			// Token: 0x04005186 RID: 20870
			private readonly float rigWeight;

			// Token: 0x04005187 RID: 20871
			private readonly IRigConstraint[] constraints;

			// Token: 0x04005188 RID: 20872
			private readonly float[] constraintWeights;

			// Token: 0x04005189 RID: 20873
			private readonly BenchAnimationPlayer.RigTransformState[] transforms;
		}

		// Token: 0x02000B24 RID: 2852
		private readonly struct RigTransformState
		{
			// Token: 0x060050F2 RID: 20722 RVA: 0x001F5C22 File Offset: 0x001F3E22
			public RigTransformState(Transform transform)
			{
				this.transform = transform;
				this.position = transform.localPosition;
				this.rotation = transform.localRotation;
				this.scale = transform.localScale;
			}

			// Token: 0x060050F3 RID: 20723 RVA: 0x001F5C50 File Offset: 0x001F3E50
			public void Restore()
			{
				if (this.transform == null)
				{
					return;
				}
				this.transform.localPosition = this.position;
				this.transform.localRotation = this.rotation;
				this.transform.localScale = this.scale;
			}

			// Token: 0x0400518A RID: 20874
			private readonly Transform transform;

			// Token: 0x0400518B RID: 20875
			private readonly Vector3 position;

			// Token: 0x0400518C RID: 20876
			private readonly Vector3 scale;

			// Token: 0x0400518D RID: 20877
			private readonly Quaternion rotation;
		}
	}
}
