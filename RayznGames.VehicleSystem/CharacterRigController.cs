using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200000B RID: 11
	public class CharacterRigController : MonoBehaviour
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003BD1 File Offset: 0x00001DD1
		public bool IsVehicleRigActive
		{
			get
			{
				return this.vehicleRigActive;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003BD9 File Offset: 0x00001DD9
		public bool IsCoverRigActive
		{
			get
			{
				return this.coverRigActive;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003BE1 File Offset: 0x00001DE1
		public bool IsActive
		{
			get
			{
				return this.vehicleRigActive || this.coverRigActive;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003BF3 File Offset: 0x00001DF3
		public Rig VehicleRig
		{
			get
			{
				return this.vehicleRig;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003BFB File Offset: 0x00001DFB
		public Rig CoverPoseRig
		{
			get
			{
				return this.coverPoseRig;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003C03 File Offset: 0x00001E03
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			if (this.rigBuilder == null)
			{
				this.rigBuilder = base.GetComponent<RigBuilder>();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003C2B File Offset: 0x00001E2B
		private void OnEnable()
		{
			if (this.isInitialized && !this.vehicleRigActive)
			{
				this.ResetInactiveVehicleRig();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003C43 File Offset: 0x00001E43
		private void LateUpdate()
		{
			if (this.isInitialized && !this.vehicleRigActive)
			{
				this.ResetInactiveVehicleRig();
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003C5C File Offset: 0x00001E5C
		public bool Initialize()
		{
			if (this.isInitialized)
			{
				return true;
			}
			if (this.rigBuilder == null)
			{
				this.rigBuilder = base.GetComponent<RigBuilder>();
				if (this.rigBuilder == null)
				{
					Debug.LogError("[CharacterRigController] No RigBuilder found on " + base.gameObject.name);
					return false;
				}
			}
			this.PopulateVehicleConstraintCaches();
			this.PopulateCoverConstraintCaches();
			this.InitializeCoverIKAnchor();
			if (this.vehicleRig != null)
			{
				this.vehicleRig.weight = 0f;
			}
			if (this.coverPoseRig != null)
			{
				this.coverPoseRig.weight = 0f;
			}
			this.ZeroAllVehicleConstraints();
			this.ZeroAllCoverConstraints();
			this.SetVehicleRigLayerActive(false);
			this.isInitialized = true;
			Debug.Log("[CharacterRigController] Initialized on " + base.gameObject.name + " — " + string.Format("VehicleRig:{0}, CoverPoseRig:{1}", this.vehicleRig != null, this.coverPoseRig != null));
			return true;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003D6C File Offset: 0x00001F6C
		public void ActivateVehicleRig()
		{
			if (!this.isInitialized && !this.Initialize())
			{
				return;
			}
			if (this.vehicleRigActive)
			{
				return;
			}
			if (this.vehicleRig == null)
			{
				Debug.LogWarning("[CharacterRigController] No Vehicle Rig assigned on " + base.gameObject.name);
				return;
			}
			this.ZeroAllVehicleConstraints();
			this.vehicleRig.weight = 1f;
			this.vehicleRigActive = true;
			this.SetVehicleRigLayerActive(true);
			Debug.Log("[CharacterRigController] Vehicle rig activated on " + base.gameObject.name);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003DFC File Offset: 0x00001FFC
		public void ActivateVehicleRigOnly()
		{
			if (!this.isInitialized && !this.Initialize())
			{
				return;
			}
			if (this.vehicleRigActive)
			{
				return;
			}
			if (this.vehicleRig == null)
			{
				return;
			}
			this.ZeroAllVehicleConstraints();
			this.vehicleRig.weight = 1f;
			this.vehicleRigActive = true;
			this.SetVehicleRigLayerActive(true);
			Debug.Log("[CharacterRigController] Vehicle rig weight activated on " + base.gameObject.name);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003E70 File Offset: 0x00002070
		public void EnsureVehicleRigWeight()
		{
			if (!this.vehicleRigActive)
			{
				return;
			}
			this.SetVehicleRigLayerActive(true);
			if (this.vehicleRig != null && this.vehicleRig.weight != 1f)
			{
				this.vehicleRig.weight = 1f;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003EC0 File Offset: 0x000020C0
		public void DeactivateVehicleRig()
		{
			bool flag = this.vehicleRigActive;
			this.vehicleRigActive = false;
			this.vehicleUsedLimbs.Clear();
			this.suppressedVehicleLimbs.Clear();
			this.ResetInactiveVehicleRig();
			if (flag)
			{
				Debug.Log("[CharacterRigController] Vehicle rig deactivated on " + base.gameObject.name);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003F12 File Offset: 0x00002112
		private void ResetInactiveVehicleRig()
		{
			this.SetVehicleRigLayerActive(false);
			if (this.vehicleRig != null)
			{
				this.vehicleRig.weight = 0f;
			}
			this.ZeroAllVehicleConstraints();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003F40 File Offset: 0x00002140
		private void SetVehicleRigLayerActive(bool active)
		{
			if (this.rigBuilder == null || this.vehicleRig == null)
			{
				return;
			}
			bool flag = false;
			foreach (RigLayer rigLayer in this.rigBuilder.layers)
			{
				if (!(rigLayer.rig != this.vehicleRig) && rigLayer.active != active)
				{
					rigLayer.active = active;
					flag = true;
				}
			}
			if (flag && this.rigBuilder.graph.IsValid())
			{
				this.rigBuilder.SyncLayers();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003FF8 File Offset: 0x000021F8
		public void SetVehicleLimbPose(LimbType limb, Vector3 worldPosition, Quaternion worldRotation, float ikWeight)
		{
			Transform transform;
			if (this.vehicleLimbTargets.TryGetValue(limb, out transform) && transform != null)
			{
				transform.position = worldPosition;
				transform.rotation = worldRotation;
			}
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.vehicleLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null)
			{
				if (this.suppressedVehicleLimbs.Contains(limb))
				{
					twoBoneIKConstraint.weight = 0f;
					return;
				}
				twoBoneIKConstraint.weight = ikWeight;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004068 File Offset: 0x00002268
		public void SetVehicleHintPosition(LimbType limb, Vector3 worldPosition)
		{
			Transform transform;
			if (this.vehicleLimbHints.TryGetValue(limb, out transform) && transform != null)
			{
				transform.position = worldPosition;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004098 File Offset: 0x00002298
		public void SetPelvisPose(Vector3 worldPosition, Quaternion worldRotation, float weight)
		{
			if (this.pelvisTarget != null)
			{
				this.pelvisTarget.position = worldPosition;
				this.pelvisTarget.rotation = worldRotation;
			}
			if (this.pelvisConstraint != null)
			{
				this.pelvisConstraint.weight = weight;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000040E8 File Offset: 0x000022E8
		public void SetSpinePose(Vector3 worldPosition, Quaternion worldRotation, float weight)
		{
			if (this.spineTarget != null)
			{
				this.spineTarget.position = worldPosition;
				this.spineTarget.rotation = worldRotation;
			}
			if (this.spineRotationConstraint != null)
			{
				this.spineRotationConstraint.weight = weight;
			}
			if (this.spineAimConstraint != null)
			{
				this.spineAimConstraint.weight = weight;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000414F File Offset: 0x0000234F
		public void SetSpineAim(Vector3 worldTarget, float weight)
		{
			if (this.spineTarget != null)
			{
				this.spineTarget.position = worldTarget;
			}
			if (this.spineAimConstraint != null)
			{
				this.spineAimConstraint.weight = weight;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004185 File Offset: 0x00002385
		public void SetHeadLook(Vector3 worldTarget, float weight)
		{
			if (this.headTarget != null)
			{
				this.headTarget.position = worldTarget;
			}
			if (this.headAimConstraint != null)
			{
				this.headAimConstraint.weight = weight;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000041BC File Offset: 0x000023BC
		public Transform GetVehicleLimbTarget(LimbType limb)
		{
			Transform transform;
			this.vehicleLimbTargets.TryGetValue(limb, out transform);
			return transform;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000041DC File Offset: 0x000023DC
		public Transform GetVehicleLimbHint(LimbType limb)
		{
			Transform transform;
			this.vehicleLimbHints.TryGetValue(limb, out transform);
			return transform;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000041F9 File Offset: 0x000023F9
		public Transform GetPelvisTarget()
		{
			return this.pelvisTarget;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004201 File Offset: 0x00002401
		public Transform GetSpineTarget()
		{
			return this.spineTarget;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004209 File Offset: 0x00002409
		public Transform GetHeadTarget()
		{
			return this.headTarget;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004214 File Offset: 0x00002414
		public float GetVehicleLimbWeight(LimbType limb)
		{
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.vehicleLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null)
			{
				return twoBoneIKConstraint.weight;
			}
			return 0f;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004248 File Offset: 0x00002448
		public void SetVehicleLimbs(LimbType[] limbs)
		{
			this.vehicleUsedLimbs.Clear();
			if (limbs != null)
			{
				foreach (LimbType limbType in limbs)
				{
					this.vehicleUsedLimbs.Add(limbType);
				}
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004284 File Offset: 0x00002484
		public void ClearVehicleLimbs()
		{
			this.vehicleUsedLimbs.Clear();
			this.suppressedVehicleLimbs.Clear();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000429C File Offset: 0x0000249C
		public void SetVehicleLimbSuppressed(LimbType limb, bool suppressed)
		{
			if (!this.isInitialized && !this.Initialize())
			{
				return;
			}
			if (suppressed)
			{
				this.suppressedVehicleLimbs.Add(limb);
			}
			else
			{
				this.suppressedVehicleLimbs.Remove(limb);
			}
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.vehicleLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null && suppressed)
			{
				twoBoneIKConstraint.weight = 0f;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004302 File Offset: 0x00002502
		public void ClearVehicleLimbSuppressions()
		{
			this.suppressedVehicleLimbs.Clear();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000430F File Offset: 0x0000250F
		public bool IsLimbUsedByVehicle(LimbType limb)
		{
			return this.vehicleUsedLimbs.Contains(limb);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004320 File Offset: 0x00002520
		public void ActivateCoverRig()
		{
			if (!this.isInitialized && !this.Initialize())
			{
				return;
			}
			if (this.coverRigActive)
			{
				return;
			}
			if (this.coverPoseRig == null)
			{
				Debug.LogWarning("[CharacterRigController] No Cover Pose Rig assigned on " + base.gameObject.name);
				return;
			}
			this.coverPoseRig.weight = 1f;
			this.coverRigActive = true;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004387 File Offset: 0x00002587
		public void EnsureCoverRigWeight()
		{
			if (this.coverRigActive && this.coverPoseRig != null && this.coverPoseRig.weight != 1f)
			{
				this.coverPoseRig.weight = 1f;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000043C4 File Offset: 0x000025C4
		public void DeactivateCoverRig()
		{
			if (!this.coverRigActive)
			{
				return;
			}
			foreach (KeyValuePair<LimbType, TwoBoneIKConstraint> keyValuePair in this.coverLimbConstraints)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.weight = 0f;
				}
			}
			this.coverRigActive = false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004440 File Offset: 0x00002640
		public void SetCoverLimbPose(LimbType limb, Vector3 worldPosition, Quaternion worldRotation, float ikWeight)
		{
			Transform transform;
			if (this.coverLimbTargets.TryGetValue(limb, out transform) && transform != null)
			{
				transform.position = worldPosition;
				transform.rotation = worldRotation;
			}
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.coverLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null)
			{
				twoBoneIKConstraint.weight = ikWeight;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004498 File Offset: 0x00002698
		public void SetCoverLimbPoseWeighted(LimbType limb, Vector3 worldPosition, Quaternion worldRotation, float constraintWeight, float positionWeight, float rotationWeight, Vector3 hintWorldPosition, float hintWeight)
		{
			Transform transform;
			if (this.coverLimbTargets.TryGetValue(limb, out transform) && transform != null)
			{
				transform.position = worldPosition;
				transform.rotation = worldRotation;
			}
			Transform transform2;
			if (this.coverLimbHints.TryGetValue(limb, out transform2) && transform2 != null)
			{
				transform2.position = hintWorldPosition;
			}
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.coverLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null)
			{
				twoBoneIKConstraint.weight = constraintWeight;
				twoBoneIKConstraint.data.targetPositionWeight = positionWeight;
				twoBoneIKConstraint.data.targetRotationWeight = rotationWeight;
				twoBoneIKConstraint.data.hintWeight = hintWeight;
			}
			CharacterRigController.CoverHandGoal coverHandGoal = new CharacterRigController.CoverHandGoal
			{
				position = worldPosition,
				rotation = worldRotation,
				hint = hintWorldPosition,
				weight = constraintWeight,
				positionWeight = positionWeight,
				rotationWeight = rotationWeight,
				hintWeight = hintWeight
			};
			if (limb == LimbType.RightHand)
			{
				this.coverRightHandGoal = coverHandGoal;
				return;
			}
			if (limb == LimbType.LeftHand)
			{
				this.coverLeftHandGoal = coverHandGoal;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004594 File Offset: 0x00002794
		public void ZeroCoverLimb(LimbType limb)
		{
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.coverLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null)
			{
				twoBoneIKConstraint.weight = 0f;
			}
			if (limb == LimbType.RightHand)
			{
				this.coverRightHandGoal.weight = 0f;
				return;
			}
			if (limb == LimbType.LeftHand)
			{
				this.coverLeftHandGoal.weight = 0f;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000045F0 File Offset: 0x000027F0
		public void SetCoverHintPosition(LimbType limb, Vector3 worldPosition)
		{
			Transform transform;
			if (this.coverLimbHints.TryGetValue(limb, out transform) && transform != null)
			{
				transform.position = worldPosition;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004620 File Offset: 0x00002820
		public float GetCoverLimbWeight(LimbType limb)
		{
			TwoBoneIKConstraint twoBoneIKConstraint;
			if (this.coverLimbConstraints.TryGetValue(limb, out twoBoneIKConstraint) && twoBoneIKConstraint != null)
			{
				return twoBoneIKConstraint.weight;
			}
			return 0f;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004654 File Offset: 0x00002854
		public Transform GetCoverLimbTarget(LimbType limb)
		{
			Transform transform;
			this.coverLimbTargets.TryGetValue(limb, out transform);
			return transform;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004674 File Offset: 0x00002874
		public Transform GetCoverLimbHint(LimbType limb)
		{
			Transform transform;
			this.coverLimbHints.TryGetValue(limb, out transform);
			return transform;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004691 File Offset: 0x00002891
		public void ApplyCoverHandIKToCurrentPose()
		{
			if (!this.coverRigActive)
			{
				return;
			}
			CharacterRigController.SolveCoverHand(this.coverLeftHandIK, in this.coverLeftHandGoal);
			CharacterRigController.SolveCoverHand(this.coverRightHandIK, in this.coverRightHandGoal);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000046C0 File Offset: 0x000028C0
		private unsafe static void SolveCoverHand(TwoBoneIKConstraint constraint, in CharacterRigController.CoverHandGoal goal)
		{
			if (constraint == null || goal.weight <= 0f)
			{
				return;
			}
			TwoBoneIKConstraintData twoBoneIKConstraintData = *constraint.data;
			Transform root = twoBoneIKConstraintData.root;
			Transform mid = twoBoneIKConstraintData.mid;
			Transform tip = twoBoneIKConstraintData.tip;
			if (root == null || mid == null || tip == null)
			{
				return;
			}
			float num = ((twoBoneIKConstraintData.hint != null) ? (goal.hintWeight * goal.weight) : 0f);
			Vector3 position = root.position;
			Vector3 position2 = tip.position;
			Vector3 vector = mid.position - position;
			Vector3 vector2 = position2 - mid.position;
			Vector3 vector3 = Vector3.Lerp(position2, goal.position, goal.positionWeight * goal.weight) - position;
			Quaternion quaternion = Quaternion.Lerp(tip.rotation, goal.rotation, goal.rotationWeight * goal.weight);
			Vector3 vector4 = Vector3.Cross(vector, vector2);
			if (vector4.sqrMagnitude < 1E-08f && num > 0f)
			{
				vector4 = Vector3.Cross(goal.hint - position, vector2);
			}
			if (vector4.sqrMagnitude < 1E-08f)
			{
				vector4 = Vector3.Cross(vector3, vector2);
			}
			if (vector4.sqrMagnitude < 1E-08f)
			{
				vector4 = Vector3.up;
			}
			vector4.Normalize();
			float magnitude = vector.magnitude;
			float magnitude2 = vector2.magnitude;
			float num2 = 0.5f * (CharacterRigController.TriangleAngle((position2 - position).magnitude, magnitude, magnitude2) - CharacterRigController.TriangleAngle(vector3.magnitude, magnitude, magnitude2));
			float num3 = Mathf.Sin(num2);
			mid.rotation = new Quaternion(vector4.x * num3, vector4.y * num3, vector4.z * num3, Mathf.Cos(num2)) * mid.rotation;
			root.rotation = QuaternionExt.FromToRotation(tip.position - position, vector3) * root.rotation;
			if (num > 0f)
			{
				Vector3 vector5 = tip.position - position;
				if (vector5.sqrMagnitude > 0f)
				{
					Vector3 normalized = vector5.normalized;
					Vector3 vector6 = mid.position - position;
					Vector3 vector7 = goal.hint - position;
					Vector3 vector8 = vector6 - normalized * Vector3.Dot(vector6, normalized);
					Vector3 vector9 = vector7 - normalized * Vector3.Dot(vector7, normalized);
					float num4 = magnitude + magnitude2;
					if (vector8.sqrMagnitude > num4 * num4 * 0.001f && vector9.sqrMagnitude > 0f)
					{
						Quaternion quaternion2 = QuaternionExt.FromToRotation(vector8, vector9);
						quaternion2.x *= num;
						quaternion2.y *= num;
						quaternion2.z *= num;
						root.rotation = QuaternionExt.NormalizeSafe(quaternion2) * root.rotation;
					}
				}
			}
			tip.rotation = quaternion;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000049D6 File Offset: 0x00002BD6
		private static float TriangleAngle(float opposite, float sideA, float sideB)
		{
			return Mathf.Acos(Mathf.Clamp((sideA * sideA + sideB * sideB - opposite * opposite) / (sideA * sideB) / 2f, -1f, 1f));
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004A01 File Offset: 0x00002C01
		public Transform CoverIKAnchor
		{
			get
			{
				return this.coverIKAnchor;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004A09 File Offset: 0x00002C09
		public void Activate()
		{
			this.ActivateVehicleRig();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004A11 File Offset: 0x00002C11
		public void ActivateRigOnly()
		{
			this.ActivateVehicleRigOnly();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004A19 File Offset: 0x00002C19
		public void Deactivate()
		{
			this.DeactivateVehicleRig();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004A21 File Offset: 0x00002C21
		public void DeactivateRigOnly()
		{
			this.DeactivateVehicleRig();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004A29 File Offset: 0x00002C29
		public void SetLimbPose(LimbType limb, Vector3 worldPosition, Quaternion worldRotation, float ikWeight)
		{
			this.SetVehicleLimbPose(limb, worldPosition, worldRotation, ikWeight);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004A36 File Offset: 0x00002C36
		public void SetHintPosition(LimbType limb, Vector3 worldPosition, float hintWeight = 1f)
		{
			this.SetVehicleHintPosition(limb, worldPosition);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004A40 File Offset: 0x00002C40
		public float GetLimbWeight(LimbType limb)
		{
			return this.GetVehicleLimbWeight(limb);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004A49 File Offset: 0x00002C49
		public Transform GetLimbTarget(LimbType limb)
		{
			return this.GetVehicleLimbTarget(limb);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004A52 File Offset: 0x00002C52
		public Transform GetLimbHint(LimbType limb)
		{
			return this.GetVehicleLimbHint(limb);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004A5C File Offset: 0x00002C5C
		private void ZeroAllVehicleConstraints()
		{
			if (this.leftHandIK != null)
			{
				this.leftHandIK.weight = 0f;
			}
			if (this.rightHandIK != null)
			{
				this.rightHandIK.weight = 0f;
			}
			if (this.leftFootIK != null)
			{
				this.leftFootIK.weight = 0f;
			}
			if (this.rightFootIK != null)
			{
				this.rightFootIK.weight = 0f;
			}
			if (this.pelvisConstraint != null)
			{
				this.pelvisConstraint.weight = 0f;
			}
			if (this.spineRotationConstraint != null)
			{
				this.spineRotationConstraint.weight = 0f;
			}
			if (this.spineAimConstraint != null)
			{
				this.spineAimConstraint.weight = 0f;
			}
			if (this.headAimConstraint != null)
			{
				this.headAimConstraint.weight = 0f;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004B5C File Offset: 0x00002D5C
		private void ZeroAllCoverConstraints()
		{
			foreach (KeyValuePair<LimbType, TwoBoneIKConstraint> keyValuePair in this.coverLimbConstraints)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.weight = 0f;
				}
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004BC8 File Offset: 0x00002DC8
		private void PopulateVehicleConstraintCaches()
		{
			this.vehicleLimbConstraints.Clear();
			this.vehicleLimbTargets.Clear();
			this.vehicleLimbHints.Clear();
			if (this.leftHandIK != null)
			{
				this.vehicleLimbConstraints[LimbType.LeftHand] = this.leftHandIK;
				this.vehicleLimbTargets[LimbType.LeftHand] = this.leftHandIK.data.target;
				this.vehicleLimbHints[LimbType.LeftHand] = this.leftHandIK.data.hint;
			}
			if (this.rightHandIK != null)
			{
				this.vehicleLimbConstraints[LimbType.RightHand] = this.rightHandIK;
				this.vehicleLimbTargets[LimbType.RightHand] = this.rightHandIK.data.target;
				this.vehicleLimbHints[LimbType.RightHand] = this.rightHandIK.data.hint;
			}
			if (this.leftFootIK != null)
			{
				this.vehicleLimbConstraints[LimbType.LeftFoot] = this.leftFootIK;
				this.vehicleLimbTargets[LimbType.LeftFoot] = this.leftFootIK.data.target;
				this.vehicleLimbHints[LimbType.LeftFoot] = this.leftFootIK.data.hint;
			}
			if (this.rightFootIK != null)
			{
				this.vehicleLimbConstraints[LimbType.RightFoot] = this.rightFootIK;
				this.vehicleLimbTargets[LimbType.RightFoot] = this.rightFootIK.data.target;
				this.vehicleLimbHints[LimbType.RightFoot] = this.rightFootIK.data.hint;
			}
			if (this.pelvisConstraint != null)
			{
				WeightedTransformArray sourceObjects = this.pelvisConstraint.data.sourceObjects;
				if (sourceObjects.Count > 0)
				{
					this.pelvisTarget = sourceObjects[0].transform;
				}
			}
			if (this.spineRotationConstraint != null)
			{
				WeightedTransformArray sourceObjects2 = this.spineRotationConstraint.data.sourceObjects;
				if (sourceObjects2.Count > 0)
				{
					this.spineTarget = sourceObjects2[0].transform;
				}
			}
			else if (this.spineAimConstraint != null)
			{
				WeightedTransformArray sourceObjects3 = this.spineAimConstraint.data.sourceObjects;
				if (sourceObjects3.Count > 0)
				{
					this.spineTarget = sourceObjects3[0].transform;
				}
			}
			if (this.headAimConstraint != null)
			{
				WeightedTransformArray sourceObjects4 = this.headAimConstraint.data.sourceObjects;
				if (sourceObjects4.Count > 0)
				{
					this.headTarget = sourceObjects4[0].transform;
				}
			}
			Debug.Log(string.Concat(new string[]
			{
				"[CharacterRigController] Vehicle caches populated: ",
				string.Format("LH:{0} RH:{1} ", this.leftHandIK != null, this.rightHandIK != null),
				string.Format("LF:{0} RF:{1} ", this.leftFootIK != null, this.rightFootIK != null),
				string.Format("Pelvis:{0} ", this.pelvisConstraint != null),
				string.Format("SpineRot:{0} SpineAim:{1} ", this.spineRotationConstraint != null, this.spineAimConstraint != null),
				string.Format("Head:{0}", this.headAimConstraint != null)
			}));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004F2C File Offset: 0x0000312C
		private void InitializeCoverIKAnchor()
		{
			if (this.coverIKAnchorOverride != null)
			{
				this.coverIKAnchor = this.coverIKAnchorOverride;
				Debug.Log("[CharacterRigController] Using assigned Cover IK Anchor: " + this.coverIKAnchor.name);
				return;
			}
			if (this.animator == null)
			{
				this.animator = base.GetComponent<Animator>();
			}
			if (this.animator != null)
			{
				Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.Hips);
				if (boneTransform != null)
				{
					GameObject gameObject = new GameObject("CoverIKAnchor");
					gameObject.transform.SetParent(boneTransform, false);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					this.coverIKAnchor = gameObject.transform;
					Debug.Log("[CharacterRigController] Created Cover IK Anchor parented to " + boneTransform.name);
					return;
				}
				Debug.LogWarning("[CharacterRigController] Hips bone not found — Cover IK Anchor not created");
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005010 File Offset: 0x00003210
		private void PopulateCoverConstraintCaches()
		{
			this.coverLimbConstraints.Clear();
			this.coverLimbTargets.Clear();
			this.coverLimbHints.Clear();
			if (this.coverLeftHandIK != null)
			{
				this.coverLimbConstraints[LimbType.LeftHand] = this.coverLeftHandIK;
				this.coverLimbTargets[LimbType.LeftHand] = this.coverLeftHandIK.data.target;
				this.coverLimbHints[LimbType.LeftHand] = this.coverLeftHandIK.data.hint;
			}
			if (this.coverRightHandIK != null)
			{
				this.coverLimbConstraints[LimbType.RightHand] = this.coverRightHandIK;
				this.coverLimbTargets[LimbType.RightHand] = this.coverRightHandIK.data.target;
				this.coverLimbHints[LimbType.RightHand] = this.coverRightHandIK.data.hint;
			}
			if (this.coverLeftFootIK != null)
			{
				this.coverLimbConstraints[LimbType.LeftFoot] = this.coverLeftFootIK;
				this.coverLimbTargets[LimbType.LeftFoot] = this.coverLeftFootIK.data.target;
				this.coverLimbHints[LimbType.LeftFoot] = this.coverLeftFootIK.data.hint;
			}
			if (this.coverRightFootIK != null)
			{
				this.coverLimbConstraints[LimbType.RightFoot] = this.coverRightFootIK;
				this.coverLimbTargets[LimbType.RightFoot] = this.coverRightFootIK.data.target;
				this.coverLimbHints[LimbType.RightFoot] = this.coverRightFootIK.data.hint;
			}
			Debug.Log("[CharacterRigController] Cover caches populated: " + string.Format("LH:{0} RH:{1} ", this.coverLeftHandIK != null, this.coverRightHandIK != null) + string.Format("LF:{0} RF:{1}", this.coverLeftFootIK != null, this.coverRightFootIK != null));
		}

		// Token: 0x0400004F RID: 79
		private const float CoverIKEpsilon = 1E-08f;

		// Token: 0x04000050 RID: 80
		[Header("RigBuilder")]
		[SerializeField]
		private RigBuilder rigBuilder;

		// Token: 0x04000051 RID: 81
		[Header("Vehicle Rig")]
		[Tooltip("The Rig used for vehicle riding (e.g., BikeRig)")]
		[SerializeField]
		private Rig vehicleRig;

		// Token: 0x04000052 RID: 82
		[Header("Vehicle IK Constraints - Assign in Inspector")]
		[SerializeField]
		private TwoBoneIKConstraint leftHandIK;

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private TwoBoneIKConstraint rightHandIK;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		private TwoBoneIKConstraint leftFootIK;

		// Token: 0x04000055 RID: 85
		[SerializeField]
		private TwoBoneIKConstraint rightFootIK;

		// Token: 0x04000056 RID: 86
		[Header("Vehicle Body Constraints - Assign in Inspector")]
		[SerializeField]
		private MultiParentConstraint pelvisConstraint;

		// Token: 0x04000057 RID: 87
		[Tooltip("Use MultiRotationConstraint for direct rotation control (recommended for riding)")]
		[SerializeField]
		private MultiRotationConstraint spineRotationConstraint;

		// Token: 0x04000058 RID: 88
		[Tooltip("Legacy: MultiAimConstraint for aim-based spine control")]
		[SerializeField]
		private MultiAimConstraint spineAimConstraint;

		// Token: 0x04000059 RID: 89
		[Header("Vehicle Head Constraint - Assign in Inspector")]
		[Tooltip("MultiAimConstraint for head look direction")]
		[SerializeField]
		private MultiAimConstraint headAimConstraint;

		// Token: 0x0400005A RID: 90
		[Header("Cover Pose Rig")]
		[Tooltip("Separate Rig for cover pose IK (independent from vehicle rig)")]
		[SerializeField]
		private Rig coverPoseRig;

		// Token: 0x0400005B RID: 91
		[Header("Cover Pose IK Constraints - Assign in Inspector")]
		[SerializeField]
		private TwoBoneIKConstraint coverLeftHandIK;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		private TwoBoneIKConstraint coverRightHandIK;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private TwoBoneIKConstraint coverLeftFootIK;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private TwoBoneIKConstraint coverRightFootIK;

		// Token: 0x0400005F RID: 95
		[Header("Cover IK Anchor")]
		[Tooltip("Optional: assign a pre-existing anchor transform parented to hips. If null, one is created at runtime.")]
		[SerializeField]
		private Transform coverIKAnchorOverride;

		// Token: 0x04000060 RID: 96
		private Dictionary<LimbType, TwoBoneIKConstraint> vehicleLimbConstraints = new Dictionary<LimbType, TwoBoneIKConstraint>();

		// Token: 0x04000061 RID: 97
		private Dictionary<LimbType, Transform> vehicleLimbTargets = new Dictionary<LimbType, Transform>();

		// Token: 0x04000062 RID: 98
		private Dictionary<LimbType, Transform> vehicleLimbHints = new Dictionary<LimbType, Transform>();

		// Token: 0x04000063 RID: 99
		private Transform pelvisTarget;

		// Token: 0x04000064 RID: 100
		private Transform spineTarget;

		// Token: 0x04000065 RID: 101
		private Transform headTarget;

		// Token: 0x04000066 RID: 102
		private Dictionary<LimbType, TwoBoneIKConstraint> coverLimbConstraints = new Dictionary<LimbType, TwoBoneIKConstraint>();

		// Token: 0x04000067 RID: 103
		private Dictionary<LimbType, Transform> coverLimbTargets = new Dictionary<LimbType, Transform>();

		// Token: 0x04000068 RID: 104
		private Dictionary<LimbType, Transform> coverLimbHints = new Dictionary<LimbType, Transform>();

		// Token: 0x04000069 RID: 105
		private Transform coverIKAnchor;

		// Token: 0x0400006A RID: 106
		private CharacterRigController.CoverHandGoal coverLeftHandGoal;

		// Token: 0x0400006B RID: 107
		private CharacterRigController.CoverHandGoal coverRightHandGoal;

		// Token: 0x0400006C RID: 108
		private Animator animator;

		// Token: 0x0400006D RID: 109
		private bool isInitialized;

		// Token: 0x0400006E RID: 110
		private bool vehicleRigActive;

		// Token: 0x0400006F RID: 111
		private bool coverRigActive;

		// Token: 0x04000070 RID: 112
		private HashSet<LimbType> vehicleUsedLimbs = new HashSet<LimbType>();

		// Token: 0x04000071 RID: 113
		private HashSet<LimbType> suppressedVehicleLimbs = new HashSet<LimbType>();

		// Token: 0x02000025 RID: 37
		private struct CoverHandGoal
		{
			// Token: 0x04000160 RID: 352
			public Vector3 position;

			// Token: 0x04000161 RID: 353
			public Vector3 hint;

			// Token: 0x04000162 RID: 354
			public Quaternion rotation;

			// Token: 0x04000163 RID: 355
			public float weight;

			// Token: 0x04000164 RID: 356
			public float positionWeight;

			// Token: 0x04000165 RID: 357
			public float rotationWeight;

			// Token: 0x04000166 RID: 358
			public float hintWeight;
		}
	}
}
