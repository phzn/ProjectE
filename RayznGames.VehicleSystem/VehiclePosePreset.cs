using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000018 RID: 24
	[CreateAssetMenu(fileName = "NewVehiclePosePreset", menuName = "RayznGames/Vehicles/Pose Preset")]
	public class VehiclePosePreset : ScriptableObject
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00006AF8 File Offset: 0x00004CF8
		public bool UsesLimb(LimbType limb)
		{
			if (this.usedLimbs == null)
			{
				return false;
			}
			for (int i = 0; i < this.usedLimbs.Length; i++)
			{
				if (this.usedLimbs[i] == limb)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006B30 File Offset: 0x00004D30
		public RiderPoseData GetPose(VehiclePoseState state)
		{
			RiderPoseData riderPoseData;
			switch (state)
			{
			case VehiclePoseState.Idle:
				riderPoseData = this.idlePose;
				break;
			case VehiclePoseState.MovingLowSpeed:
				riderPoseData = this.movingLowSpeedPose;
				break;
			case VehiclePoseState.MovingHighSpeed:
				riderPoseData = this.movingHighSpeedPose;
				break;
			case VehiclePoseState.Mounting:
				riderPoseData = this.mountingPose ?? this.idlePose;
				break;
			case VehiclePoseState.Dismounting:
				riderPoseData = this.dismountingPose ?? this.idlePose;
				break;
			default:
				riderPoseData = this.idlePose;
				break;
			}
			return riderPoseData;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006BA2 File Offset: 0x00004DA2
		public float EvaluateBlend(float progress)
		{
			return this.blendCurve.Evaluate(Mathf.Clamp01(progress));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006BB8 File Offset: 0x00004DB8
		public VehicleCoverPoseStateOffsets GetCoverOffsets(VehiclePoseState state, bool confident)
		{
			if (this.coverPoseIK == null || !this.coverPoseIK.enabled)
			{
				return null;
			}
			VehicleCoverPoseStateOffsets[] array = (confident ? this.coverPoseIK.confidentOffsets : this.coverPoseIK.embarrassedOffsets);
			VehicleCoverPoseStateOffsets[] array2 = (confident ? this.coverPoseIK.embarrassedOffsets : this.coverPoseIK.confidentOffsets);
			VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets = VehiclePosePreset.FindCoverOffsets(array, state);
			if (vehicleCoverPoseStateOffsets != null)
			{
				return vehicleCoverPoseStateOffsets;
			}
			vehicleCoverPoseStateOffsets = VehiclePosePreset.FindCoverOffsets(array2, state);
			if (vehicleCoverPoseStateOffsets != null)
			{
				return vehicleCoverPoseStateOffsets;
			}
			VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets2 = VehiclePosePreset.FindCoverOffsets(array, VehiclePoseState.Idle);
			if (vehicleCoverPoseStateOffsets2 != null)
			{
				return vehicleCoverPoseStateOffsets2;
			}
			return VehiclePosePreset.FindCoverOffsets(array2, VehiclePoseState.Idle);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006C44 File Offset: 0x00004E44
		public VehicleCoverPoseStateOffsets GetOrCreateCoverOffsets(VehiclePoseState state, bool confident)
		{
			if (this.coverPoseIK == null)
			{
				this.coverPoseIK = new VehicleCoverPosePresetData();
			}
			VehicleCoverPoseStateOffsets[] array = (confident ? this.coverPoseIK.confidentOffsets : this.coverPoseIK.embarrassedOffsets);
			if (array == null)
			{
				array = new VehicleCoverPoseStateOffsets[0];
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].state == state)
				{
					return array[i];
				}
			}
			VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets = new VehicleCoverPoseStateOffsets
			{
				state = state
			};
			int num = array.Length;
			VehicleCoverPoseStateOffsets[] array2 = new VehicleCoverPoseStateOffsets[num + 1];
			for (int j = 0; j < num; j++)
			{
				array2[j] = array[j];
			}
			array2[num] = vehicleCoverPoseStateOffsets;
			if (confident)
			{
				this.coverPoseIK.confidentOffsets = array2;
			}
			else
			{
				this.coverPoseIK.embarrassedOffsets = array2;
			}
			return vehicleCoverPoseStateOffsets;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006D08 File Offset: 0x00004F08
		private static VehicleCoverPoseStateOffsets FindCoverOffsets(VehicleCoverPoseStateOffsets[] source, VehiclePoseState state)
		{
			if (source == null || source.Length == 0)
			{
				return null;
			}
			foreach (VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets in source)
			{
				if (vehicleCoverPoseStateOffsets != null && vehicleCoverPoseStateOffsets.state == state)
				{
					return vehicleCoverPoseStateOffsets;
				}
			}
			return null;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006D40 File Offset: 0x00004F40
		public VehiclePoseState DetermineState(float speed, VehiclePoseState currentState)
		{
			switch (currentState)
			{
			case VehiclePoseState.Idle:
				if (speed > this.idleToMovingThreshold)
				{
					return VehiclePoseState.MovingLowSpeed;
				}
				break;
			case VehiclePoseState.MovingLowSpeed:
				if (speed < this.movingToIdleThreshold)
				{
					return VehiclePoseState.Idle;
				}
				if (speed > this.lowToHighSpeedThreshold)
				{
					return VehiclePoseState.MovingHighSpeed;
				}
				break;
			case VehiclePoseState.MovingHighSpeed:
				if (speed < this.highToLowSpeedThreshold)
				{
					return VehiclePoseState.MovingLowSpeed;
				}
				break;
			}
			return currentState;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006D90 File Offset: 0x00004F90
		private void OnValidate()
		{
			if (this.movingToIdleThreshold > this.idleToMovingThreshold)
			{
				this.movingToIdleThreshold = this.idleToMovingThreshold * 0.5f;
			}
			if (this.highToLowSpeedThreshold > this.lowToHighSpeedThreshold)
			{
				this.highToLowSpeedThreshold = this.lowToHighSpeedThreshold * 0.75f;
			}
			if (this.coverPoseIK == null)
			{
				this.coverPoseIK = new VehicleCoverPosePresetData();
			}
			if (this.idlePose == null)
			{
				this.idlePose = new RiderPoseData("Idle");
			}
			if (this.movingLowSpeedPose == null)
			{
				this.movingLowSpeedPose = new RiderPoseData("MovingLowSpeed");
			}
			if (this.movingHighSpeedPose == null)
			{
				this.movingHighSpeedPose = new RiderPoseData("MovingHighSpeed");
			}
		}

		// Token: 0x040000D6 RID: 214
		[Header("Vehicle Configuration")]
		[Tooltip("Type of vehicle for animator selection")]
		public float vehicleType;

		// Token: 0x040000D7 RID: 215
		[Tooltip("Which limb IK constraints this vehicle actually controls. Limbs NOT listed here are free for other systems (e.g. CoverPose).")]
		public LimbType[] usedLimbs = new LimbType[]
		{
			LimbType.LeftHand,
			LimbType.RightHand,
			LimbType.LeftFoot,
			LimbType.RightFoot
		};

		// Token: 0x040000D8 RID: 216
		[Header("Speed Normalization")]
		[Tooltip("Max speed reachable WITHOUT sprint. VehicleSpeed param will be normalized so this maps to ~0.5 (slow anim range). Sprint can push beyond to reach fast anim (>0.7).")]
		public float normalMaxSpeed = 8f;

		// Token: 0x040000D9 RID: 217
		[Header("Pose Library")]
		[Tooltip("Pose when vehicle is stationary (one foot on ground)")]
		public RiderPoseData idlePose = new RiderPoseData("Idle");

		// Token: 0x040000DA RID: 218
		[Tooltip("Pose when moving at low speed (casual pedaling)")]
		public RiderPoseData movingLowSpeedPose = new RiderPoseData("MovingLowSpeed");

		// Token: 0x040000DB RID: 219
		[Tooltip("Pose when moving at high speed (intense pedaling, more forward lean)")]
		public RiderPoseData movingHighSpeedPose = new RiderPoseData("MovingHighSpeed");

		// Token: 0x040000DC RID: 220
		[Header("Blend Settings")]
		[Tooltip("Curve for blending between poses (0-1)")]
		public AnimationCurve blendCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040000DD RID: 221
		[Tooltip("Default duration for pose transitions")]
		[Range(0.1f, 2f)]
		public float defaultBlendDuration = 0.3f;

		// Token: 0x040000DE RID: 222
		[Header("Transition Thresholds")]
		[Tooltip("Speed above which to transition from Idle to MovingLowSpeed")]
		public float idleToMovingThreshold = 1f;

		// Token: 0x040000DF RID: 223
		[Tooltip("Speed below which to transition from MovingLowSpeed to Idle")]
		public float movingToIdleThreshold = 0.5f;

		// Token: 0x040000E0 RID: 224
		[Tooltip("Speed above which to transition from MovingLowSpeed to MovingHighSpeed")]
		public float lowToHighSpeedThreshold = 8f;

		// Token: 0x040000E1 RID: 225
		[Tooltip("Speed below which to transition from MovingHighSpeed to MovingLowSpeed")]
		public float highToLowSpeedThreshold = 6f;

		// Token: 0x040000E2 RID: 226
		[Tooltip("Maximum speed for normalizing VehicleSpeed animator parameter (0-1 range). This is the speed at which VehicleSpeed=1.0 (full sprint).")]
		public float fastSpeedThreshold = 12f;

		// Token: 0x040000E3 RID: 227
		[Header("Mount/Dismount (Optional)")]
		[Tooltip("Pose during mounting animation")]
		public RiderPoseData mountingPose;

		// Token: 0x040000E4 RID: 228
		[Tooltip("Pose during dismounting animation")]
		public RiderPoseData dismountingPose;

		// Token: 0x040000E5 RID: 229
		[Header("Cover IK (Optional)")]
		[Tooltip("Vehicle-level Cover IK offsets. Keeps base CoverPoseController profiles as fallback when empty/disabled.")]
		public VehicleCoverPosePresetData coverPoseIK = new VehicleCoverPosePresetData();
	}
}
