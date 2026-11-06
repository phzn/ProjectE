using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public class RiderPoseData
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005404 File Offset: 0x00003604
		public RiderPoseData()
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005544 File Offset: 0x00003744
		public RiderPoseData(string name)
		{
			this.poseName = name;
			this.leftHand = LimbPoseData.Default;
			this.rightHand = LimbPoseData.Default;
			this.leftFoot = LimbPoseData.Default;
			this.rightFoot = LimbPoseData.Default;
			this.pelvis = LimbPoseData.DefaultPelvis;
			this.spine = LimbPoseData.DefaultSpine;
			this.head = LimbPoseData.Default;
			this.headWeight = 1f;
			this.spineWeight = 0.5f;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000056EC File Offset: 0x000038EC
		public RiderPoseData Clone()
		{
			return new RiderPoseData
			{
				poseName = this.poseName,
				seatPositionOffset = this.seatPositionOffset,
				seatRotationOffset = this.seatRotationOffset,
				pelvis = this.pelvis,
				spine = this.spine,
				spineWeight = this.spineWeight,
				head = this.head,
				headWeight = this.headWeight,
				headLookOffset = this.headLookOffset,
				leftHand = this.leftHand,
				rightHand = this.rightHand,
				leftFoot = this.leftFoot,
				rightFoot = this.rightFoot,
				groundedFoot = this.groundedFoot,
				groundedFootOffset = this.groundedFootOffset,
				groundedFootRotation = this.groundedFootRotation,
				leftElbowHintOffset = this.leftElbowHintOffset,
				rightElbowHintOffset = this.rightElbowHintOffset,
				leftKneeHintOffset = this.leftKneeHintOffset,
				rightKneeHintOffset = this.rightKneeHintOffset
			};
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000057F0 File Offset: 0x000039F0
		public static RiderPoseData Lerp(RiderPoseData a, RiderPoseData b, float t)
		{
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			return new RiderPoseData
			{
				poseName = ((t < 0.5f) ? a.poseName : b.poseName),
				seatPositionOffset = Vector3.Lerp(a.seatPositionOffset, b.seatPositionOffset, t),
				seatRotationOffset = RiderPoseData.LerpEuler(a.seatRotationOffset, b.seatRotationOffset, t),
				pelvis = LimbPoseData.Lerp(a.pelvis, b.pelvis, t),
				spine = LimbPoseData.Lerp(a.spine, b.spine, t),
				spineWeight = Mathf.Lerp(a.spineWeight, b.spineWeight, t),
				head = LimbPoseData.Lerp(a.head, b.head, t),
				headWeight = Mathf.Lerp(a.headWeight, b.headWeight, t),
				headLookOffset = Vector3.Lerp(a.headLookOffset, b.headLookOffset, t),
				leftHand = LimbPoseData.Lerp(a.leftHand, b.leftHand, t),
				rightHand = LimbPoseData.Lerp(a.rightHand, b.rightHand, t),
				leftFoot = LimbPoseData.Lerp(a.leftFoot, b.leftFoot, t),
				rightFoot = LimbPoseData.Lerp(a.rightFoot, b.rightFoot, t),
				groundedFoot = ((t < 0.5f) ? a.groundedFoot : b.groundedFoot),
				groundedFootOffset = Vector3.Lerp(a.groundedFootOffset, b.groundedFootOffset, t),
				groundedFootRotation = RiderPoseData.LerpEuler(a.groundedFootRotation, b.groundedFootRotation, t),
				leftElbowHintOffset = Vector3.Lerp(a.leftElbowHintOffset, b.leftElbowHintOffset, t),
				rightElbowHintOffset = Vector3.Lerp(a.rightElbowHintOffset, b.rightElbowHintOffset, t),
				leftKneeHintOffset = Vector3.Lerp(a.leftKneeHintOffset, b.leftKneeHintOffset, t),
				rightKneeHintOffset = Vector3.Lerp(a.rightKneeHintOffset, b.rightKneeHintOffset, t)
			};
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000059F4 File Offset: 0x00003BF4
		private static Vector3 LerpEuler(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(Mathf.LerpAngle(a.x, b.x, t), Mathf.LerpAngle(a.y, b.y, t), Mathf.LerpAngle(a.z, b.z, t));
		}

		// Token: 0x04000089 RID: 137
		[Header("Pose Identity")]
		public string poseName = "Unnamed";

		// Token: 0x0400008A RID: 138
		[Header("Root Position (relative to Seat anchor)")]
		[Tooltip("Character root position offset from seat")]
		public Vector3 seatPositionOffset = Vector3.zero;

		// Token: 0x0400008B RID: 139
		[Tooltip("Character root rotation offset from seat (euler)")]
		public Vector3 seatRotationOffset = Vector3.zero;

		// Token: 0x0400008C RID: 140
		[Header("Pelvis")]
		public LimbPoseData pelvis = LimbPoseData.DefaultPelvis;

		// Token: 0x0400008D RID: 141
		[Header("Spine")]
		public LimbPoseData spine = LimbPoseData.DefaultSpine;

		// Token: 0x0400008E RID: 142
		[Range(0f, 1f)]
		[Tooltip("Overall spine constraint weight")]
		public float spineWeight = 0.5f;

		// Token: 0x0400008F RID: 143
		[Header("Head")]
		public LimbPoseData head = LimbPoseData.Default;

		// Token: 0x04000090 RID: 144
		[Range(0f, 1f)]
		[Tooltip("Head look constraint weight")]
		public float headWeight = 1f;

		// Token: 0x04000091 RID: 145
		[Tooltip("Head look target offset (relative to bike forward direction)")]
		public Vector3 headLookOffset = new Vector3(0f, 1.5f, 3f);

		// Token: 0x04000092 RID: 146
		[Header("Left Hand")]
		public LimbPoseData leftHand = LimbPoseData.Default;

		// Token: 0x04000093 RID: 147
		[Header("Right Hand")]
		public LimbPoseData rightHand = LimbPoseData.Default;

		// Token: 0x04000094 RID: 148
		[Header("Left Foot")]
		public LimbPoseData leftFoot = LimbPoseData.Default;

		// Token: 0x04000095 RID: 149
		[Header("Right Foot")]
		public LimbPoseData rightFoot = LimbPoseData.Default;

		// Token: 0x04000096 RID: 150
		[Header("Idle Foot Placement")]
		[Tooltip("Which foot touches the ground when stopped")]
		public GroundedFoot groundedFoot;

		// Token: 0x04000097 RID: 151
		[Tooltip("Offset for the grounded foot position (relative to bike, Y is height above ground)")]
		public Vector3 groundedFootOffset = new Vector3(-0.3f, 0f, 0f);

		// Token: 0x04000098 RID: 152
		[Tooltip("Rotation offset for the grounded foot (applied after ground alignment)")]
		public Vector3 groundedFootRotation = Vector3.zero;

		// Token: 0x04000099 RID: 153
		[Header("IK Hints (relative to character root)")]
		[Tooltip("Left elbow hint offset from character")]
		public Vector3 leftElbowHintOffset = new Vector3(-0.3f, -0.2f, -0.3f);

		// Token: 0x0400009A RID: 154
		[Tooltip("Right elbow hint offset from character")]
		public Vector3 rightElbowHintOffset = new Vector3(0.3f, -0.2f, -0.3f);

		// Token: 0x0400009B RID: 155
		[Tooltip("Left knee hint offset from character")]
		public Vector3 leftKneeHintOffset = new Vector3(0f, 0f, 0.5f);

		// Token: 0x0400009C RID: 156
		[Tooltip("Right knee hint offset from character")]
		public Vector3 rightKneeHintOffset = new Vector3(0f, 0f, 0.5f);
	}
}
