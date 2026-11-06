using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000015 RID: 21
	public class VehiclePoseDriver : MonoBehaviour
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00005A31 File Offset: 0x00003C31
		public VehiclePoseState CurrentState
		{
			get
			{
				return this.currentState;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005A39 File Offset: 0x00003C39
		public VehiclePoseState TargetState
		{
			get
			{
				return this.targetState;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00005A41 File Offset: 0x00003C41
		public float BlendProgress
		{
			get
			{
				return this.blendProgress;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005A49 File Offset: 0x00003C49
		public bool IsBlending
		{
			get
			{
				return this.blendProgress < 1f;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005A58 File Offset: 0x00003C58
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005A60 File Offset: 0x00003C60
		public VehiclePosePreset PosePreset
		{
			get
			{
				return this.posePreset;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005A68 File Offset: 0x00003C68
		public RiderPoseData CurrentPose
		{
			get
			{
				return this.currentPose;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005A70 File Offset: 0x00003C70
		public bool UsesLimb(LimbType limb)
		{
			return this.isActive && this.posePreset != null && this.posePreset.UsesLimb(limb);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005A98 File Offset: 0x00003C98
		public void Initialize(IVehicleAnchorProvider anchors, CharacterRigController rig, Transform rider)
		{
			if (this.isActive)
			{
				this.Shutdown();
			}
			this.anchorProvider = anchors;
			this.rigController = rig;
			this.riderTransform = rider;
			if (rider != null)
			{
				this.animator = rider.GetComponent<Animator>();
				if (this.animator != null)
				{
					this.faceLayerIndex = this.animator.GetLayerIndex("Face");
				}
			}
			if (this.posePreset == null)
			{
				Debug.LogError("[VehiclePoseDriver] No VehiclePosePreset assigned!");
				return;
			}
			RiderPoseData idlePose = this.posePreset.idlePose;
			this.currentPose = ((idlePose != null) ? idlePose.Clone() : null) ?? new RiderPoseData("Idle");
			this.sourcePose = this.currentPose.Clone();
			this.targetPose = this.currentPose.Clone();
			this.currentState = VehiclePoseState.Idle;
			this.targetState = VehiclePoseState.Idle;
			this.blendProgress = 1f;
			this.normalizedVehicleSpeed = 0f;
			this.rigController.SetVehicleLimbs(this.posePreset.usedLimbs);
			this.rigController.ActivateRigOnly();
			this.isActive = true;
			this.ApplyPose(this.currentPose);
			if (this.debugLog)
			{
				Transform anchor = this.anchorProvider.GetAnchor(VehicleAnchorType.Seat);
				Debug.Log(string.Format("[VehiclePoseDriver] Initialize - anchorProvider: {0}, seat: {1}", this.anchorProvider, (anchor != null) ? (anchor.name + " at " + anchor.position.ToString()) : "NULL"));
				Debug.Log(string.Format("[VehiclePoseDriver] Initialize - riderTransform: {0} at {1}", this.riderTransform.name, this.riderTransform.position));
				Debug.Log(string.Format("[VehiclePoseDriver] Initialize - currentPose.seatPositionOffset: {0}", this.currentPose.seatPositionOffset));
			}
			if (this.animator != null)
			{
				this.animatorUpdateModeLease = AnimatorNormalUpdateModeLease.Acquire(this.animator);
				this.EnsureRideAnimationParameters();
				this.originalAnimatorSpeed = this.animator.speed;
				this.animator.speed = this.originalAnimatorSpeed * this.animationSpeedMultiplier;
			}
			if (this.debugLog)
			{
				Debug.Log("[VehiclePoseDriver] Initialized and pose applied");
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005CCC File Offset: 0x00003ECC
		public void Shutdown()
		{
			if (!this.isActive)
			{
				return;
			}
			AnimatorNormalUpdateModeLease animatorNormalUpdateModeLease = this.animatorUpdateModeLease;
			if (animatorNormalUpdateModeLease != null)
			{
				animatorNormalUpdateModeLease.Dispose();
			}
			this.animatorUpdateModeLease = null;
			if (this.animator != null)
			{
				this.animator.speed = this.originalAnimatorSpeed;
				this.animator.SetBool(VehiclePoseDriver.IsRidingHash, false);
				this.animator.SetFloat(VehiclePoseDriver.VehicleSpeedHash, 0f);
			}
			if (this.rigController != null)
			{
				this.rigController.ClearVehicleLimbs();
				this.rigController.Deactivate();
			}
			this.isActive = false;
			this.normalizedVehicleSpeed = 0f;
			this.anchorProvider = null;
			this.rigController = null;
			this.riderTransform = null;
			this.animator = null;
			this.faceLayerIndex = -1;
			if (this.debugLog)
			{
				Debug.Log("[VehiclePoseDriver] Shutdown");
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005DAA File Offset: 0x00003FAA
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public void TransitionTo(VehiclePoseState newState, float? duration = null)
		{
			if (!this.isActive || this.posePreset == null)
			{
				return;
			}
			if (newState == this.targetState && this.blendProgress >= 1f)
			{
				return;
			}
			this.sourcePose = this.currentPose.Clone();
			RiderPoseData pose = this.posePreset.GetPose(newState);
			this.targetPose = ((pose != null) ? pose.Clone() : null) ?? this.sourcePose;
			this.targetState = newState;
			this.blendProgress = 0f;
			this.blendDuration = duration ?? this.posePreset.defaultBlendDuration;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[VehiclePoseDriver] Transitioning {0} -> {1} over {2}s", this.currentState, newState, this.blendDuration));
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005E94 File Offset: 0x00004094
		public void UpdateFromSpeed(float speed)
		{
			if (!this.isActive || this.posePreset == null)
			{
				return;
			}
			float num = ((this.posePreset.normalMaxSpeed > 0f) ? this.posePreset.normalMaxSpeed : 8f);
			float num2 = ((this.posePreset.fastSpeedThreshold > 0f) ? this.posePreset.fastSpeedThreshold : 12f);
			float num3;
			if (speed <= num)
			{
				num3 = speed / num * 0.5f;
			}
			else
			{
				float num4 = num2 - num;
				float num5 = ((num4 > 0f) ? ((speed - num) / num4) : 1f);
				num3 = 0.5f + Mathf.Clamp01(num5) * 0.5f;
			}
			num3 = Mathf.Clamp01(num3);
			this.normalizedVehicleSpeed = num3;
			if (this.animator != null)
			{
				this.animator.SetFloat(VehiclePoseDriver.VehicleSpeedHash, num3);
			}
			VehiclePoseState vehiclePoseState = this.posePreset.DetermineState(speed, this.currentState);
			if (vehiclePoseState == VehiclePoseState.MovingHighSpeed && num3 < 0.65f)
			{
				vehiclePoseState = VehiclePoseState.MovingLowSpeed;
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[VehiclePoseDriver] UpdateFromSpeed: speed={0:F2}, currentState={1}, targetState={2}, newState={3}", new object[] { speed, this.currentState, this.targetState, vehiclePoseState }));
			}
			if (vehiclePoseState != this.targetState)
			{
				this.TransitionTo(vehiclePoseState, null);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005FF8 File Offset: 0x000041F8
		public void SetPoseImmediate(VehiclePoseState state)
		{
			if (!this.isActive || this.posePreset == null)
			{
				return;
			}
			RiderPoseData pose = this.posePreset.GetPose(state);
			this.currentPose = ((pose != null) ? pose.Clone() : null) ?? this.currentPose;
			this.sourcePose = this.currentPose.Clone();
			this.targetPose = this.currentPose.Clone();
			this.currentState = state;
			this.targetState = state;
			this.blendProgress = 1f;
			this.ApplyPose(this.currentPose);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000608B File Offset: 0x0000428B
		public void SetIdleAnimationIndex(float index)
		{
			if (this.animator != null)
			{
				this.animator.SetFloat(VehiclePoseDriver.VehicleIdleIndexHash, index);
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000060AC File Offset: 0x000042AC
		public void RandomizeIdleAnimation(int poolSize)
		{
			if (poolSize <= 0)
			{
				return;
			}
			this.SetIdleAnimationIndex((float)Random.Range(0, poolSize));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000060C4 File Offset: 0x000042C4
		private void Update()
		{
			if (!this.isActive || this.rigController == null || this.anchorProvider == null)
			{
				return;
			}
			this.EnsureRideAnimationParameters();
			if (this.blendProgress < 1f)
			{
				this.blendProgress += Time.deltaTime / this.blendDuration;
				this.blendProgress = Mathf.Clamp01(this.blendProgress);
				float num = this.posePreset.EvaluateBlend(this.blendProgress);
				this.currentPose = RiderPoseData.Lerp(this.sourcePose, this.targetPose, num);
				if (this.blendProgress >= 1f)
				{
					this.currentState = this.targetState;
					if (this.debugLog)
					{
						Debug.Log(string.Format("[VehiclePoseDriver] Blend complete: {0}", this.currentState));
					}
				}
			}
			this.ApplyPose(this.currentPose);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000061A4 File Offset: 0x000043A4
		private void EnsureRideAnimationParameters()
		{
			if (!this.isActive || this.animator == null || this.posePreset == null)
			{
				return;
			}
			if (!this.animator.GetBool(VehiclePoseDriver.IsRidingHash))
			{
				this.animator.SetBool(VehiclePoseDriver.IsRidingHash, true);
			}
			if (this.animator.GetFloat(VehiclePoseDriver.VehicleTypeHash) != this.posePreset.vehicleType)
			{
				this.animator.SetFloat(VehiclePoseDriver.VehicleTypeHash, this.posePreset.vehicleType);
			}
			if (this.animator.GetFloat(VehiclePoseDriver.VehicleSpeedHash) != this.normalizedVehicleSpeed)
			{
				this.animator.SetFloat(VehiclePoseDriver.VehicleSpeedHash, this.normalizedVehicleSpeed);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006260 File Offset: 0x00004460
		private void ApplyPose(RiderPoseData pose)
		{
			if (!this.isActive || pose == null || this.anchorProvider == null || this.rigController == null)
			{
				return;
			}
			this.rigController.EnsureVehicleRigWeight();
			Transform anchor = this.anchorProvider.GetAnchor(VehicleAnchorType.Seat);
			Transform anchor2 = this.anchorProvider.GetAnchor(VehicleAnchorType.LeftHandGrip);
			Transform anchor3 = this.anchorProvider.GetAnchor(VehicleAnchorType.RightHandGrip);
			Transform anchor4 = this.anchorProvider.GetAnchor(VehicleAnchorType.LeftFootRest);
			Transform anchor5 = this.anchorProvider.GetAnchor(VehicleAnchorType.RightFootRest);
			Transform anchor6 = this.anchorProvider.GetAnchor(VehicleAnchorType.SpineAimReference);
			Transform transform = this.anchorProvider.GetAnchor(VehicleAnchorType.PelvisReference) ?? anchor;
			if (!this.anchorProvider.UsesInvertedHierarchy && anchor != null && this.riderTransform != null)
			{
				Vector3 vector = anchor.TransformPoint(pose.seatPositionOffset);
				if (this.debugLog && Time.frameCount % 60 == 0)
				{
					Debug.Log(string.Format("[VehiclePoseDriver] ApplyPose - seat: {0} at {1}, seatPositionOffset: {2}", anchor.name, anchor.position, pose.seatPositionOffset));
					Debug.Log(string.Format("[VehiclePoseDriver] ApplyPose - rider BEFORE: {0}, AFTER will be: {1}", this.riderTransform.position, vector));
				}
				if (vector.y < -5f)
				{
					Debug.LogError(string.Format("[VehiclePoseDriver] SUSPICIOUS Y VALUE! newPos={0}, seat.position={1}, seatPositionOffset={2}", vector, anchor.position, pose.seatPositionOffset));
				}
				this.riderTransform.position = vector;
				this.riderTransform.rotation = anchor.rotation * Quaternion.Euler(pose.seatRotationOffset);
			}
			else if (this.anchorProvider.UsesInvertedHierarchy && this.debugLog && Time.frameCount % 60 == 0)
			{
				Debug.Log("[VehiclePoseDriver] ApplyPose - SKIPPING root position (inverted hierarchy)");
			}
			if (this.applyPelvisIK && transform != null)
			{
				this.rigController.SetPelvisPose(transform.TransformPoint(pose.pelvis.positionOffset), transform.rotation * Quaternion.Euler(pose.pelvis.rotationOffset), pose.pelvis.ikWeight);
			}
			if (anchor2 != null && this.posePreset.UsesLimb(LimbType.LeftHand))
			{
				this.rigController.SetLimbPose(LimbType.LeftHand, anchor2.TransformPoint(pose.leftHand.positionOffset), anchor2.rotation * Quaternion.Euler(pose.leftHand.rotationOffset), pose.leftHand.ikWeight);
			}
			if (anchor3 != null && this.posePreset.UsesLimb(LimbType.RightHand))
			{
				this.rigController.SetLimbPose(LimbType.RightHand, anchor3.TransformPoint(pose.rightHand.positionOffset), anchor3.rotation * Quaternion.Euler(pose.rightHand.rotationOffset), pose.rightHand.ikWeight);
			}
			object obj = this.currentState == VehiclePoseState.Idle || this.targetState == VehiclePoseState.Idle;
			float num;
			if (this.IsBlending)
			{
				if (this.targetState == VehiclePoseState.Idle)
				{
					num = this.blendProgress;
				}
				else if (this.currentState == VehiclePoseState.Idle)
				{
					num = 1f - this.blendProgress;
				}
				else
				{
					num = 0f;
				}
			}
			else
			{
				num = ((this.currentState == VehiclePoseState.Idle) ? 1f : 0f);
			}
			object obj2 = obj;
			bool flag = obj2 != null && (pose.groundedFoot == GroundedFoot.Left || pose.groundedFoot == GroundedFoot.Auto);
			bool flag2 = obj2 != null && pose.groundedFoot == GroundedFoot.Right;
			if (anchor4 != null && this.posePreset.UsesLimb(LimbType.LeftFoot))
			{
				if (flag)
				{
					Vector3 vector2 = this.anchorProvider.VehicleTransform.TransformPoint(pose.groundedFootOffset);
					Quaternion quaternion = this.anchorProvider.VehicleTransform.rotation * Quaternion.Euler(pose.groundedFootRotation);
					Vector3 vector3 = anchor4.TransformPoint(pose.leftFoot.positionOffset);
					Quaternion quaternion2 = anchor4.rotation * Quaternion.Euler(pose.leftFoot.rotationOffset);
					this.rigController.SetLimbPose(LimbType.LeftFoot, Vector3.Lerp(vector3, vector2, num), Quaternion.Slerp(quaternion2, quaternion, num), Mathf.Lerp(pose.leftFoot.ikWeight, 1f, num));
				}
				else
				{
					this.rigController.SetLimbPose(LimbType.LeftFoot, anchor4.TransformPoint(pose.leftFoot.positionOffset), anchor4.rotation * Quaternion.Euler(pose.leftFoot.rotationOffset), pose.leftFoot.ikWeight);
				}
			}
			if (anchor5 != null && this.posePreset.UsesLimb(LimbType.RightFoot))
			{
				if (flag2)
				{
					Vector3 vector4 = this.anchorProvider.VehicleTransform.TransformPoint(pose.groundedFootOffset);
					Quaternion quaternion3 = this.anchorProvider.VehicleTransform.rotation * Quaternion.Euler(pose.groundedFootRotation);
					Vector3 vector5 = anchor5.TransformPoint(pose.rightFoot.positionOffset);
					Quaternion quaternion4 = anchor5.rotation * Quaternion.Euler(pose.rightFoot.rotationOffset);
					this.rigController.SetLimbPose(LimbType.RightFoot, Vector3.Lerp(vector5, vector4, num), Quaternion.Slerp(quaternion4, quaternion3, num), Mathf.Lerp(pose.rightFoot.ikWeight, 1f, num));
				}
				else
				{
					this.rigController.SetLimbPose(LimbType.RightFoot, anchor5.TransformPoint(pose.rightFoot.positionOffset), anchor5.rotation * Quaternion.Euler(pose.rightFoot.rotationOffset), pose.rightFoot.ikWeight);
				}
			}
			if (this.applySpineIK)
			{
				if (anchor6 != null)
				{
					this.rigController.SetSpinePose(anchor6.TransformPoint(pose.spine.positionOffset), anchor6.rotation * Quaternion.Euler(pose.spine.rotationOffset), pose.spineWeight);
				}
				else if (anchor2 != null && anchor3 != null)
				{
					Vector3 vector6 = (anchor2.position + anchor3.position) * 0.5f;
					Quaternion quaternion5 = Quaternion.Slerp(anchor2.rotation, anchor3.rotation, 0.5f);
					this.rigController.SetSpinePose(vector6 + pose.spine.positionOffset, quaternion5 * Quaternion.Euler(pose.spine.rotationOffset), pose.spineWeight);
				}
			}
			if (this.applyHeadIK && this.riderTransform != null)
			{
				Vector3 vector7 = this.anchorProvider.VehicleTransform.TransformPoint(pose.headLookOffset);
				this.rigController.SetHeadLook(vector7, pose.headWeight);
			}
			if (this.riderTransform != null)
			{
				this.rigController.SetHintPosition(LimbType.LeftHand, this.riderTransform.TransformPoint(pose.leftElbowHintOffset), 1f);
				this.rigController.SetHintPosition(LimbType.RightHand, this.riderTransform.TransformPoint(pose.rightElbowHintOffset), 1f);
				this.rigController.SetHintPosition(LimbType.LeftFoot, this.riderTransform.TransformPoint(pose.leftKneeHintOffset), 1f);
				this.rigController.SetHintPosition(LimbType.RightFoot, this.riderTransform.TransformPoint(pose.rightKneeHintOffset), 1f);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000069A1 File Offset: 0x00004BA1
		public void SetPosePreset(VehiclePosePreset preset)
		{
			this.posePreset = preset;
			if (this.isActive && preset != null)
			{
				this.SetPoseImmediate(this.currentState);
			}
		}

		// Token: 0x040000B0 RID: 176
		[Header("Configuration")]
		[SerializeField]
		private VehiclePosePreset posePreset;

		// Token: 0x040000B1 RID: 177
		[Header("Foot Placement")]
		[Tooltip("Layer mask for ground detection raycast")]
		[SerializeField]
		private LayerMask groundLayerMask = 1;

		// Token: 0x040000B2 RID: 178
		[Tooltip("Maximum raycast distance for foot placement")]
		[SerializeField]
		private float footRaycastDistance = 1.5f;

		// Token: 0x040000B3 RID: 179
		[Header("IK Toggles (for testing)")]
		[SerializeField]
		private bool applyHeadIK = true;

		// Token: 0x040000B4 RID: 180
		[SerializeField]
		private bool applySpineIK = true;

		// Token: 0x040000B5 RID: 181
		[SerializeField]
		private bool applyPelvisIK = true;

		// Token: 0x040000B6 RID: 182
		[Header("Animation Speed")]
		[Tooltip("Multiplier applied to the rider animator's speed while mounted. Use values < 1 to slow down the pedaling/riding animation. 1 = default speed. Restored to original value on dismount.")]
		[SerializeField]
		[Range(0.1f, 2f)]
		private float animationSpeedMultiplier = 1f;

		// Token: 0x040000B7 RID: 183
		[Header("Debug")]
		[SerializeField]
		private bool debugLog = true;

		// Token: 0x040000B8 RID: 184
		private IVehicleAnchorProvider anchorProvider;

		// Token: 0x040000B9 RID: 185
		private CharacterRigController rigController;

		// Token: 0x040000BA RID: 186
		private Transform riderTransform;

		// Token: 0x040000BB RID: 187
		private Animator animator;

		// Token: 0x040000BC RID: 188
		private int faceLayerIndex = -1;

		// Token: 0x040000BD RID: 189
		private AnimatorNormalUpdateModeLease animatorUpdateModeLease;

		// Token: 0x040000BE RID: 190
		private float originalAnimatorSpeed = 1f;

		// Token: 0x040000BF RID: 191
		private float normalizedVehicleSpeed;

		// Token: 0x040000C0 RID: 192
		private static readonly int IsRidingHash = Animator.StringToHash("IsRiding");

		// Token: 0x040000C1 RID: 193
		private static readonly int VehicleSpeedHash = Animator.StringToHash("VehicleSpeed");

		// Token: 0x040000C2 RID: 194
		private static readonly int VehicleTypeHash = Animator.StringToHash("VehicleType");

		// Token: 0x040000C3 RID: 195
		private static readonly int VehicleIdleIndexHash = Animator.StringToHash("VehicleIdleIndex");

		// Token: 0x040000C4 RID: 196
		private VehiclePoseState currentState;

		// Token: 0x040000C5 RID: 197
		private VehiclePoseState targetState;

		// Token: 0x040000C6 RID: 198
		private RiderPoseData currentPose;

		// Token: 0x040000C7 RID: 199
		private RiderPoseData sourcePose;

		// Token: 0x040000C8 RID: 200
		private RiderPoseData targetPose;

		// Token: 0x040000C9 RID: 201
		private float blendProgress = 1f;

		// Token: 0x040000CA RID: 202
		private float blendDuration = 0.3f;

		// Token: 0x040000CB RID: 203
		private bool isActive;
	}
}
