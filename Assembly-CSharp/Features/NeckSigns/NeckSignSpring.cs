using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.NeckSigns
{
	// Token: 0x02000F76 RID: 3958
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(12000)]
	public sealed class NeckSignSpring : MonoBehaviour
	{
		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06007724 RID: 30500 RVA: 0x00306E3D File Offset: 0x0030503D
		public IReadOnlyList<Vector3> SimulationPoints
		{
			get
			{
				return this.positions;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06007725 RID: 30501 RVA: 0x00306E45 File Offset: 0x00305045
		public int SuspensionCount
		{
			get
			{
				return this.paths.Length;
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06007726 RID: 30502 RVA: 0x00306E4F File Offset: 0x0030504F
		public int FittedPoseCount
		{
			get
			{
				return this.fittedPoses.Length;
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06007727 RID: 30503 RVA: 0x00306E59 File Offset: 0x00305059
		public Vector2 SwayDegrees
		{
			get
			{
				return this.sway;
			}
		}

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06007728 RID: 30504 RVA: 0x00306E61 File Offset: 0x00305061
		public float SwayAngleDegrees
		{
			get
			{
				return this.sway.magnitude;
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06007729 RID: 30505 RVA: 0x00306E6E File Offset: 0x0030506E
		public float MaxSwayDegrees
		{
			get
			{
				return Mathf.Clamp(this.maximumSwayDegrees, 0f, 2f);
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x0600772A RID: 30506 RVA: 0x00306E85 File Offset: 0x00305085
		public float FitSize
		{
			get
			{
				if (!(this.fit != null))
				{
					return 3f;
				}
				return this.fit.FitSize;
			}
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x0600772B RID: 30507 RVA: 0x00306EA6 File Offset: 0x003050A6
		public float FitWeight
		{
			get
			{
				return Mathf.Clamp01((this.FitSize - 3f) * 0.5f);
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x0600772C RID: 30508 RVA: 0x00306EC0 File Offset: 0x003050C0
		public Vector3 BoardCenter
		{
			get
			{
				if (this.initialized)
				{
					return (this.positions[0] + this.positions[1] + this.positions[2] + this.positions[3]) * 0.25f;
				}
				if (!(this.board != null))
				{
					return base.transform.position;
				}
				return this.board.position;
			}
		}

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x0600772D RID: 30509 RVA: 0x00306F44 File Offset: 0x00305144
		public Vector3 BoardNormal
		{
			get
			{
				if (!this.initialized)
				{
					return base.transform.forward;
				}
				return Vector3.Cross(this.positions[1] - this.positions[0], this.positions[0] - this.positions[2]).normalized;
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x0600772E RID: 30510 RVA: 0x00306FAC File Offset: 0x003051AC
		public float MaximumLengthError
		{
			get
			{
				if (!this.initialized)
				{
					return 0f;
				}
				float num = 0f;
				int num2 = 0;
				foreach (int[] array2 in this.paths)
				{
					int num3 = 0;
					while (num3 + 1 < array2.Length)
					{
						num = Mathf.Max(num, Mathf.Abs(Vector3.Distance(this.positions[array2[num3]], this.positions[array2[num3 + 1]]) - this.linkLengths[num2]));
						num3++;
						num2++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600772F RID: 30511 RVA: 0x0030703F File Offset: 0x0030523F
		public Vector3 GetBoardCorner(int index)
		{
			return this.positions[Mathf.Clamp(index, 0, 3)];
		}

		// Token: 0x06007730 RID: 30512 RVA: 0x00307054 File Offset: 0x00305254
		public IReadOnlyList<Vector3> GetSuspensionPoints(int index)
		{
			if (index < 0 || index >= this.paths.Length)
			{
				return Array.Empty<Vector3>();
			}
			Vector3[] array = new Vector3[this.paths[index].Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.positions[this.paths[index][i]];
			}
			return array;
		}

		// Token: 0x06007731 RID: 30513 RVA: 0x003070B4 File Offset: 0x003052B4
		public void Configure(Transform writingBoard, Vector3[] corners, Transform[] bones, NeckSignSpring.SuspensionRope[] suspensionRopes, Vector3[] anchors, Vector3[] boardAttachments = null, float backDepth = 0.0045f)
		{
			this.ReleaseResources();
			this.board = writingBoard;
			this.boardLocalCorners = corners ?? Array.Empty<Vector3>();
			this.boardLocalAttachments = boardAttachments ?? Array.Empty<Vector3>();
			this.chainBones = bones ?? Array.Empty<Transform>();
			this.ropes = suspensionRopes ?? Array.Empty<NeckSignSpring.SuspensionRope>();
			this.anchorPointsLocal = anchors ?? Array.Empty<Vector3>();
			this.needsReset = true;
		}

		// Token: 0x06007732 RID: 30514 RVA: 0x00307128 File Offset: 0x00305328
		public void ConfigureFittedPoses(NeckSignSpring.FittedPose[] sizes, NeckSignSpring.FittedPose[] additional = null)
		{
			this.ReleaseResources();
			this.fittedPoses = NeckSignSpring.CopyPoses(sizes);
			this.additionalPoses = NeckSignSpring.CopyPoses(additional);
			this.maximumSwayDegrees = 2f;
			this.swaySmoothTime = 1.1f;
			this.needsReset = true;
		}

		// Token: 0x06007733 RID: 30515 RVA: 0x00307168 File Offset: 0x00305368
		public void ConfigureFittedPoses(Vector3[][] sizes)
		{
			NeckSignSpring.FittedPose[] array = new NeckSignSpring.FittedPose[sizes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new NeckSignSpring.FittedPose
				{
					points = sizes[i]
				};
			}
			this.ConfigureFittedPoses(array, null);
		}

		// Token: 0x06007734 RID: 30516 RVA: 0x003071A8 File Offset: 0x003053A8
		private static NeckSignSpring.FittedPose[] CopyPoses(NeckSignSpring.FittedPose[] source)
		{
			if (source == null)
			{
				return Array.Empty<NeckSignSpring.FittedPose>();
			}
			NeckSignSpring.FittedPose[] array = new NeckSignSpring.FittedPose[source.Length];
			for (int i = 0; i < array.Length; i++)
			{
				NeckSignSpring.FittedPose[] array2 = array;
				int num = i;
				NeckSignSpring.FittedPose fittedPose = new NeckSignSpring.FittedPose();
				NeckSignSpring.FittedPose fittedPose2 = source[i];
				fittedPose.points = ((((fittedPose2 != null) ? fittedPose2.points : null) != null) ? ((Vector3[])source[i].points.Clone()) : Array.Empty<Vector3>());
				array2[num] = fittedPose;
			}
			return array;
		}

		// Token: 0x06007735 RID: 30517 RVA: 0x0030720E File Offset: 0x0030540E
		private void OnEnable()
		{
			this.needsReset = true;
			this.fullBodyAnimationOwned = PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation);
			PlayerActivityGate.ResourcesChanged += this.OnResourcesChanged;
		}

		// Token: 0x06007736 RID: 30518 RVA: 0x00307234 File Offset: 0x00305434
		private void OnDisable()
		{
			PlayerActivityGate.ResourcesChanged -= this.OnResourcesChanged;
			this.fullBodyAnimationOwned = false;
			this.needsReset = true;
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x00307258 File Offset: 0x00305458
		private void OnResourcesChanged(PlayerContextResource resources)
		{
			bool flag = (resources & PlayerContextResource.FullBodyAnimation) > PlayerContextResource.None;
			if (this.fullBodyAnimationOwned && !flag)
			{
				this.needsReset = true;
			}
			this.fullBodyAnimationOwned = flag;
		}

		// Token: 0x06007738 RID: 30520 RVA: 0x00307285 File Offset: 0x00305485
		private void LateUpdate()
		{
			this.StepSimulation(Time.deltaTime);
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x00307294 File Offset: 0x00305494
		private bool Initialize()
		{
			if (this.initialized)
			{
				return true;
			}
			if (this.board == null || this.boardLocalCorners.Length != 4 || this.anchorPointsLocal.Length == 0)
			{
				return false;
			}
			this.fit = base.GetComponent<NeckSignChestClearance>();
			this.boardRestPosition = this.board.localPosition;
			this.boardRestRotation = this.board.localRotation;
			this.boardRestScale = this.board.localScale;
			this.boardLocalCenter = (this.boardLocalCorners[0] + this.boardLocalCorners[1] + this.boardLocalCorners[2] + this.boardLocalCorners[3]) * 0.25f;
			Vector3 normalized = (this.boardLocalCorners[1] - this.boardLocalCorners[0]).normalized;
			Vector3 normalized2 = (this.boardLocalCorners[0] - this.boardLocalCorners[2]).normalized;
			this.boardLocalBasis = Quaternion.LookRotation(Vector3.Cross(normalized, normalized2), normalized2);
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(base.transform.InverseTransformPoint(this.board.TransformPoint(this.boardLocalCorners[i])));
			}
			int num = ((this.chainBones.Length > 1) ? 1 : this.ropes.Length);
			if (num == 0 || this.anchorPointsLocal.Length < num)
			{
				return false;
			}
			for (int j = 0; j < num; j++)
			{
				list.Add((this.boardLocalAttachments.Length > j) ? base.transform.InverseTransformPoint(this.board.TransformPoint(this.boardLocalAttachments[j])) : ((num == 1) ? ((list[0] + list[1]) * 0.5f) : list[j]));
			}
			this.boardPointCount = list.Count;
			this.paths = new int[num][];
			List<float> list2 = new List<float>();
			for (int k = 0; k < num; k++)
			{
				int num2 = ((this.chainBones.Length > 1) ? (this.chainBones.Length + 1) : this.ropes[k].points.Length);
				if (num2 < 2)
				{
					return false;
				}
				this.paths[k] = new int[num2];
				for (int l = 0; l < num2 - 1; l++)
				{
					Vector3 vector = ((this.chainBones.Length > 1) ? base.transform.InverseTransformPoint(this.chainBones[l].position) : this.ropes[k].points[l]);
					this.paths[k][l] = list.Count;
					list.Add((l == 0) ? this.anchorPointsLocal[k] : vector);
				}
				this.paths[k][num2 - 1] = 4 + k;
				int num3 = 0;
				while (num3 + 1 < num2)
				{
					list2.Add(Vector3.Distance(base.transform.TransformPoint(list[this.paths[k][num3]]), base.transform.TransformPoint(list[this.paths[k][num3 + 1]])));
					num3++;
				}
			}
			this.linkLengths = list2.ToArray();
			Vector3[] array = list.ToArray();
			NeckSignSpring.PoseData poseData = NeckSignSpring.MakePose(array);
			this.boardOffsets = new Vector3[this.boardPointCount];
			for (int m = 0; m < this.boardPointCount; m++)
			{
				this.boardOffsets[m] = Quaternion.Inverse(poseData.rotation) * (array[m] - poseData.center);
			}
			this.poses = new NeckSignSpring.PoseData[6];
			for (int n = 0; n < this.poses.Length; n++)
			{
				this.poses[n] = NeckSignSpring.MakePose((this.fittedPoses.Length == 6 && this.fittedPoses[n].points.Length == array.Length) ? this.fittedPoses[n].points : array);
			}
			this.extras = new NeckSignSpring.PoseData[this.additionalPoses.Length];
			for (int num4 = 0; num4 < this.extras.Length; num4++)
			{
				this.extras[num4] = NeckSignSpring.MakePose((this.additionalPoses[num4].points.Length == array.Length) ? this.additionalPoses[num4].points : this.poses[3].points);
			}
			this.positions = new Vector3[array.Length];
			this.localPoints = new Vector3[array.Length];
			this.bonePositions = new Vector3[this.chainBones.Length];
			this.boneDirections = new Vector3[this.chainBones.Length];
			this.boneRotations = new Quaternion[this.chainBones.Length];
			this.boneRootRotations = new Quaternion[this.chainBones.Length];
			for (int num5 = 0; num5 < this.chainBones.Length; num5++)
			{
				Transform transform = this.chainBones[num5];
				this.bonePositions[num5] = transform.localPosition;
				this.boneRotations[num5] = transform.localRotation;
				this.boneRootRotations[num5] = Quaternion.Inverse(base.transform.rotation) * transform.rotation;
				Vector3 vector2 = ((num5 + 1 < this.chainBones.Length) ? this.chainBones[num5 + 1].position : base.transform.TransformPoint(array[4]));
				this.boneDirections[num5] = Quaternion.Inverse(transform.rotation) * (vector2 - transform.position).normalized;
				if (num5 + 1 == this.chainBones.Length)
				{
					this.lastBoneEndpoint = transform.InverseTransformPoint(vector2);
				}
			}
			this.ropeVisuals = new NeckSignSpring.RopeVisual[this.ropes.Length];
			for (int num6 = 0; num6 < this.ropes.Length; num6++)
			{
				this.ropeVisuals[num6] = this.BindRope(this.ropes[num6]);
			}
			this.initialized = true;
			return true;
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x003078F0 File Offset: 0x00305AF0
		private static NeckSignSpring.PoseData MakePose(Vector3[] points)
		{
			Vector3 normalized = (points[1] - points[0]).normalized;
			Vector3 normalized2 = (points[0] - points[2]).normalized;
			return new NeckSignSpring.PoseData
			{
				points = points,
				center = (points[0] + points[1] + points[2] + points[3]) * 0.25f,
				rotation = Quaternion.LookRotation(Vector3.Cross(normalized, normalized2), normalized2)
			};
		}

		// Token: 0x0600773B RID: 30523 RVA: 0x00307990 File Offset: 0x00305B90
		public void ResetSimulation()
		{
			if (!this.Initialize())
			{
				return;
			}
			NeckSignChestClearance neckSignChestClearance = this.fit;
			if (neckSignChestClearance != null)
			{
				neckSignChestClearance.RefreshFit(true);
			}
			this.sway = (this.swayVelocity = Vector2.zero);
			this.filteredVelocity = Vector3.zero;
			this.previousAnchor = base.transform.TransformPoint(this.anchorPointsLocal[0]);
			this.previousFrameRotation = ((this.fit != null) ? this.fit.FitRotation : base.transform.rotation);
			this.ApplyFit(this.previousFrameRotation);
			this.needsReset = false;
		}

		// Token: 0x0600773C RID: 30524 RVA: 0x00307A33 File Offset: 0x00305C33
		public void PrepareForPainting()
		{
			this.ResetSimulation();
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x00307A3C File Offset: 0x00305C3C
		public void StepSimulation(float deltaTime)
		{
			if (deltaTime <= 0f || float.IsNaN(deltaTime) || !base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.Initialize())
			{
				return;
			}
			Vector3 vector = base.transform.TransformPoint(this.anchorPointsLocal[0]);
			if (this.needsReset || float.IsInfinity(deltaTime) || (vector - this.previousAnchor).sqrMagnitude > this.teleportDistance * this.teleportDistance)
			{
				this.ResetSimulation();
				return;
			}
			NeckSignChestClearance neckSignChestClearance = this.fit;
			if (neckSignChestClearance != null)
			{
				neckSignChestClearance.RefreshFit(false);
			}
			Quaternion quaternion = ((this.fit != null) ? this.fit.FitRotation : base.transform.rotation);
			float num = Mathf.Min(deltaTime, 0.05f);
			Vector3 vector2 = (vector - this.previousAnchor) / deltaTime;
			Vector3 vector3 = this.filteredVelocity;
			this.filteredVelocity = Vector3.Lerp(this.filteredVelocity, vector2, 1f - Mathf.Exp(-num / 0.2f));
			Vector3 vector4 = Quaternion.Inverse(quaternion) * ((this.filteredVelocity - vector3) / num);
			float num2;
			Vector3 vector5;
			(quaternion * Quaternion.Inverse(this.previousFrameRotation)).ToAngleAxis(out num2, out vector5);
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			Vector3 vector6 = ((Mathf.Abs(num2) > 0.0001f) ? (Quaternion.Inverse(quaternion) * vector5 * (num2 / deltaTime)) : Vector3.zero);
			Vector2 vector7 = new Vector2(Mathf.Abs(vector4.z) * 0.12f + Mathf.Abs(vector6.x) * 0.008f, -vector4.x * 0.12f - vector6.z * 0.008f);
			vector7 = Vector2.ClampMagnitude(vector7, this.MaxSwayDegrees);
			this.sway.x = Mathf.SmoothDamp(this.sway.x, vector7.x, ref this.swayVelocity.x, this.swaySmoothTime, 3f, num);
			this.sway.y = Mathf.SmoothDamp(this.sway.y, vector7.y, ref this.swayVelocity.y, this.swaySmoothTime, 3f, num);
			this.sway = Vector2.ClampMagnitude(this.sway, this.MaxSwayDegrees);
			this.previousAnchor = vector;
			this.previousFrameRotation = quaternion;
			this.ApplyFit(quaternion);
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x00307CB8 File Offset: 0x00305EB8
		private void ApplyFit(Quaternion frameRotation)
		{
			float num = Mathf.Clamp(this.FitSize, 0f, 5f);
			int num2 = Mathf.FloorToInt(num);
			int num3 = Mathf.Min(num2 + 1, 5);
			float num4 = num - (float)num2;
			Vector3 vector = Vector3.Lerp(this.poses[num2].center, this.poses[num3].center, num4);
			Quaternion quaternion = Quaternion.Slerp(this.poses[num2].rotation, this.poses[num3].rotation, num4);
			for (int i = this.boardPointCount; i < this.localPoints.Length; i++)
			{
				this.localPoints[i] = Vector3.Lerp(this.poses[num2].points[i], this.poses[num3].points[i], num4);
			}
			Vector2 vector2 = ((this.fit != null) ? this.fit.AdditionalFitWeights : Vector2.zero);
			for (int j = 0; j < Mathf.Min(2, this.extras.Length); j++)
			{
				float num5 = Mathf.Clamp01(vector2[j]);
				vector += (this.extras[j].center - this.poses[3].center) * num5;
				quaternion = Quaternion.Slerp(Quaternion.identity, this.extras[j].rotation * Quaternion.Inverse(this.poses[3].rotation), num5) * quaternion;
				for (int k = this.boardPointCount; k < this.localPoints.Length; k++)
				{
					this.localPoints[k] += (this.extras[j].points[k] - this.poses[3].points[k]) * num5;
				}
			}
			for (int l = 0; l < this.boardPointCount; l++)
			{
				this.localPoints[l] = vector + quaternion * this.boardOffsets[l];
			}
			Vector3 lossyScale = base.transform.lossyScale;
			bool flag = this.chainBones.Length < 2 && this.paths.Length > 1;
			Matrix4x4 matrix4x = Matrix4x4.TRS(flag ? base.transform.position : (base.transform.TransformPoint(this.anchorPointsLocal[0]) - frameRotation * Vector3.Scale(this.anchorPointsLocal[0], lossyScale)), frameRotation, lossyScale);
			for (int m = 0; m < this.positions.Length; m++)
			{
				this.positions[m] = matrix4x.MultiplyPoint3x4(this.localPoints[m]);
			}
			for (int n = 0; n < this.paths.Length; n++)
			{
				this.positions[this.paths[n][0]] = base.transform.TransformPoint(this.anchorPointsLocal[n]);
			}
			Quaternion quaternion2 = frameRotation * quaternion;
			if (flag)
			{
				Vector3 vector3 = quaternion2 * Vector3.forward * (0.01f * Mathf.Abs(lossyScale.x));
				for (int num6 = 0; num6 < this.boardPointCount; num6++)
				{
					this.positions[num6] += vector3;
				}
			}
			Quaternion quaternion3 = Quaternion.AngleAxis(this.sway.y, quaternion2 * Vector3.forward) * Quaternion.AngleAxis(-this.sway.x, quaternion2 * Vector3.right);
			Vector3 vector4 = ((this.chainBones.Length > 1) ? this.positions[this.paths[0][Mathf.Max(0, this.paths[0].Length - 3)]] : ((this.positions[0] + this.positions[1]) * 0.5f));
			for (int num7 = 0; num7 < this.boardPointCount; num7++)
			{
				this.positions[num7] = vector4 + quaternion3 * (this.positions[num7] - vector4);
			}
			if (this.chainBones.Length > 1)
			{
				for (int num8 = Mathf.Max(1, this.paths[0].Length - 2); num8 < this.paths[0].Length - 1; num8++)
				{
					int num9 = this.paths[0][num8];
					this.positions[num9] = vector4 + quaternion3 * (this.positions[num9] - vector4);
				}
			}
			else
			{
				for (int num10 = 0; num10 < this.paths.Length; num10++)
				{
					Vector3 vector5 = this.positions[4 + num10] - matrix4x.MultiplyPoint3x4(this.localPoints[4 + num10]);
					Vector3 vector6 = (flag ? (this.positions[this.paths[num10][0]] - matrix4x.MultiplyPoint3x4(this.anchorPointsLocal[num10])) : Vector3.zero);
					for (int num11 = 1; num11 < this.paths[num10].Length - 1; num11++)
					{
						float num12 = (float)num11 / (float)(this.paths[num10].Length - 1);
						this.positions[this.paths[num10][num11]] += vector5 * (num12 * num12) + vector6 * ((1f - num12) * (1f - num12));
					}
				}
			}
			this.ApplyVisuals(frameRotation);
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x003082B0 File Offset: 0x003064B0
		private void ApplyVisuals(Quaternion frameRotation)
		{
			for (int i = 0; i < this.chainBones.Length; i++)
			{
				Quaternion quaternion = frameRotation * this.boneRootRotations[i];
				Vector3 vector = this.positions[this.paths[0][i + 1]] - this.chainBones[i].position;
				if (vector.sqrMagnitude > 1E-08f)
				{
					this.chainBones[i].rotation = Quaternion.FromToRotation(quaternion * this.boneDirections[i], vector) * quaternion;
				}
				this.positions[this.paths[0][i]] = this.chainBones[i].position;
			}
			if (this.chainBones.Length > 1)
			{
				Vector3 vector2 = this.chainBones[this.chainBones.Length - 1].TransformPoint(this.lastBoneEndpoint) - this.positions[4];
				for (int j = 0; j < this.boardPointCount; j++)
				{
					this.positions[j] += vector2;
				}
			}
			Vector3 normalized = (this.positions[1] - this.positions[0]).normalized;
			Vector3 normalized2 = (this.positions[0] - this.positions[2]).normalized;
			this.board.rotation = Quaternion.LookRotation(Vector3.Cross(normalized, normalized2), normalized2) * Quaternion.Inverse(this.boardLocalBasis);
			this.board.position = this.BoardCenter - this.board.TransformVector(this.boardLocalCenter);
			for (int k = 0; k < this.ropeVisuals.Length; k++)
			{
				NeckSignSpring.RopeVisual ropeVisual = this.ropeVisuals[k];
				if (ropeVisual != null)
				{
					Matrix4x4 worldToLocalMatrix = ropeVisual.filter.transform.worldToLocalMatrix;
					Quaternion quaternion2 = Quaternion.Inverse(ropeVisual.filter.transform.rotation);
					for (int l = 0; l < ropeVisual.vertices.Length; l++)
					{
						int num = ropeVisual.segments[l];
						Vector3 vector3 = this.positions[this.paths[k][num]];
						Vector3 vector4 = this.positions[this.paths[k][num + 1]];
						Quaternion quaternion3 = Quaternion.FromToRotation(frameRotation * ropeVisual.restDirections[l], vector4 - vector3);
						Vector3 vector5 = Vector3.Lerp(vector3, vector4, ropeVisual.fractions[l]) + quaternion3 * (frameRotation * Vector3.Scale(ropeVisual.offsets[l], base.transform.lossyScale));
						ropeVisual.vertices[l] = worldToLocalMatrix.MultiplyPoint3x4(vector5);
						if (ropeVisual.normals.Length > l)
						{
							ropeVisual.normals[l] = quaternion2 * (quaternion3 * (frameRotation * ropeVisual.restNormals[l]));
						}
						if (ropeVisual.tangents.Length > l)
						{
							Vector4 vector6 = ropeVisual.restTangents[l];
							Vector3 vector7 = quaternion2 * (quaternion3 * (frameRotation * new Vector3(vector6.x, vector6.y, vector6.z)));
							ropeVisual.tangents[l] = new Vector4(vector7.x, vector7.y, vector7.z, vector6.w);
						}
					}
					ropeVisual.owned.vertices = ropeVisual.vertices;
					if (ropeVisual.normals.Length != 0)
					{
						ropeVisual.owned.normals = ropeVisual.normals;
					}
					if (ropeVisual.tangents.Length != 0)
					{
						ropeVisual.owned.tangents = ropeVisual.tangents;
					}
					ropeVisual.owned.RecalculateBounds();
				}
			}
		}

		// Token: 0x06007740 RID: 30528 RVA: 0x003086C8 File Offset: 0x003068C8
		private NeckSignSpring.RopeVisual BindRope(NeckSignSpring.SuspensionRope rope)
		{
			if (rope.mesh == null || rope.mesh.sharedMesh == null)
			{
				return null;
			}
			NeckSignSpring.RopeVisual ropeVisual = new NeckSignSpring.RopeVisual
			{
				filter = rope.mesh,
				original = rope.mesh.sharedMesh
			};
			ropeVisual.owned = Object.Instantiate<Mesh>(ropeVisual.original);
			ropeVisual.owned.name = ropeVisual.original.name + " (gentle sway)";
			ropeVisual.owned.hideFlags = HideFlags.DontSave;
			ropeVisual.vertices = ropeVisual.original.vertices;
			int num = ropeVisual.vertices.Length;
			ropeVisual.offsets = new Vector3[num];
			ropeVisual.restDirections = new Vector3[num];
			ropeVisual.segments = new int[num];
			ropeVisual.fractions = new float[num];
			ropeVisual.normals = ropeVisual.original.normals;
			ropeVisual.restNormals = (Vector3[])ropeVisual.normals.Clone();
			ropeVisual.tangents = ropeVisual.original.tangents;
			ropeVisual.restTangents = (Vector4[])ropeVisual.tangents.Clone();
			Quaternion quaternion = Quaternion.Inverse(base.transform.rotation) * rope.mesh.transform.rotation;
			for (int i = 0; i < ropeVisual.restNormals.Length; i++)
			{
				ropeVisual.restNormals[i] = quaternion * ropeVisual.restNormals[i];
			}
			for (int j = 0; j < ropeVisual.restTangents.Length; j++)
			{
				Vector4 vector = ropeVisual.restTangents[j];
				Vector3 vector2 = quaternion * new Vector3(vector.x, vector.y, vector.z);
				ropeVisual.restTangents[j] = new Vector4(vector2.x, vector2.y, vector2.z, vector.w);
			}
			for (int k = 0; k < num; k++)
			{
				Vector3 vector3 = base.transform.InverseTransformPoint(rope.mesh.transform.TransformPoint(ropeVisual.vertices[k]));
				float num2 = float.PositiveInfinity;
				int num3 = 0;
				while (num3 + 1 < rope.points.Length)
				{
					Vector3 vector4 = rope.points[num3];
					Vector3 vector5 = rope.points[num3 + 1] - vector4;
					float num4 = Mathf.Clamp01(Vector3.Dot(vector3 - vector4, vector5) / Mathf.Max(1E-08f, vector5.sqrMagnitude));
					Vector3 vector6 = vector3 - (vector4 + vector5 * num4);
					if (vector6.sqrMagnitude < num2)
					{
						num2 = vector6.sqrMagnitude;
						ropeVisual.segments[k] = num3;
						ropeVisual.fractions[k] = num4;
						ropeVisual.offsets[k] = vector6;
						ropeVisual.restDirections[k] = vector5.normalized;
					}
					num3++;
				}
			}
			rope.mesh.sharedMesh = ropeVisual.owned;
			return ropeVisual;
		}

		// Token: 0x06007741 RID: 30529 RVA: 0x003089EC File Offset: 0x00306BEC
		public void ReleaseResources()
		{
			if (!this.initialized)
			{
				return;
			}
			for (int i = 0; i < this.chainBones.Length; i++)
			{
				if (this.chainBones[i] != null)
				{
					this.chainBones[i].localPosition = this.bonePositions[i];
					this.chainBones[i].localRotation = this.boneRotations[i];
				}
			}
			if (this.board != null)
			{
				this.board.localPosition = this.boardRestPosition;
				this.board.localRotation = this.boardRestRotation;
				this.board.localScale = this.boardRestScale;
			}
			foreach (NeckSignSpring.RopeVisual ropeVisual in this.ropeVisuals)
			{
				if (ropeVisual != null)
				{
					if (ropeVisual.filter != null && ropeVisual.filter.sharedMesh == ropeVisual.owned)
					{
						ropeVisual.filter.sharedMesh = ropeVisual.original;
					}
					if (ropeVisual.owned != null)
					{
						if (Application.isPlaying)
						{
							Object.Destroy(ropeVisual.owned);
						}
						else
						{
							Object.DestroyImmediate(ropeVisual.owned);
						}
					}
				}
			}
			this.initialized = false;
			this.needsReset = true;
			this.positions = Array.Empty<Vector3>();
			this.paths = Array.Empty<int[]>();
		}

		// Token: 0x06007742 RID: 30530 RVA: 0x00308B3C File Offset: 0x00306D3C
		private void OnDestroy()
		{
			this.ReleaseResources();
		}

		// Token: 0x04007A61 RID: 31329
		private const float SignSurfaceClearance = 0.01f;

		// Token: 0x04007A62 RID: 31330
		[SerializeField]
		private Transform board;

		// Token: 0x04007A63 RID: 31331
		[SerializeField]
		private Vector3[] boardLocalCorners = Array.Empty<Vector3>();

		// Token: 0x04007A64 RID: 31332
		[SerializeField]
		private Vector3[] boardLocalAttachments = Array.Empty<Vector3>();

		// Token: 0x04007A65 RID: 31333
		[SerializeField]
		private Transform[] chainBones = Array.Empty<Transform>();

		// Token: 0x04007A66 RID: 31334
		[SerializeField]
		private NeckSignSpring.SuspensionRope[] ropes = Array.Empty<NeckSignSpring.SuspensionRope>();

		// Token: 0x04007A67 RID: 31335
		[SerializeField]
		private Vector3[] anchorPointsLocal = Array.Empty<Vector3>();

		// Token: 0x04007A68 RID: 31336
		[SerializeField]
		private NeckSignSpring.FittedPose[] fittedPoses = Array.Empty<NeckSignSpring.FittedPose>();

		// Token: 0x04007A69 RID: 31337
		[SerializeField]
		private NeckSignSpring.FittedPose[] additionalPoses = Array.Empty<NeckSignSpring.FittedPose>();

		// Token: 0x04007A6A RID: 31338
		[SerializeField]
		[Range(0f, 2f)]
		private float maximumSwayDegrees = 2f;

		// Token: 0x04007A6B RID: 31339
		[SerializeField]
		[Min(0.5f)]
		private float swaySmoothTime = 1.1f;

		// Token: 0x04007A6C RID: 31340
		[SerializeField]
		[Min(0.1f)]
		private float teleportDistance = 1.5f;

		// Token: 0x04007A6D RID: 31341
		private NeckSignChestClearance fit;

		// Token: 0x04007A6E RID: 31342
		private Vector3[] positions = Array.Empty<Vector3>();

		// Token: 0x04007A6F RID: 31343
		private Vector3[] localPoints;

		// Token: 0x04007A70 RID: 31344
		private Vector3[] boardOffsets;

		// Token: 0x04007A71 RID: 31345
		private int[][] paths = Array.Empty<int[]>();

		// Token: 0x04007A72 RID: 31346
		private NeckSignSpring.PoseData[] poses;

		// Token: 0x04007A73 RID: 31347
		private NeckSignSpring.PoseData[] extras;

		// Token: 0x04007A74 RID: 31348
		private NeckSignSpring.RopeVisual[] ropeVisuals;

		// Token: 0x04007A75 RID: 31349
		private Vector3[] bonePositions;

		// Token: 0x04007A76 RID: 31350
		private Vector3[] boneDirections;

		// Token: 0x04007A77 RID: 31351
		private Quaternion[] boneRotations;

		// Token: 0x04007A78 RID: 31352
		private Quaternion[] boneRootRotations;

		// Token: 0x04007A79 RID: 31353
		private Vector3 lastBoneEndpoint;

		// Token: 0x04007A7A RID: 31354
		private Vector3 boardRestPosition;

		// Token: 0x04007A7B RID: 31355
		private Vector3 boardRestScale;

		// Token: 0x04007A7C RID: 31356
		private Vector3 boardLocalCenter;

		// Token: 0x04007A7D RID: 31357
		private Quaternion boardRestRotation;

		// Token: 0x04007A7E RID: 31358
		private Quaternion boardLocalBasis;

		// Token: 0x04007A7F RID: 31359
		private int boardPointCount;

		// Token: 0x04007A80 RID: 31360
		private float[] linkLengths;

		// Token: 0x04007A81 RID: 31361
		private Vector2 sway;

		// Token: 0x04007A82 RID: 31362
		private Vector2 swayVelocity;

		// Token: 0x04007A83 RID: 31363
		private Vector3 previousAnchor;

		// Token: 0x04007A84 RID: 31364
		private Vector3 filteredVelocity;

		// Token: 0x04007A85 RID: 31365
		private Quaternion previousFrameRotation;

		// Token: 0x04007A86 RID: 31366
		private bool initialized;

		// Token: 0x04007A87 RID: 31367
		private bool needsReset = true;

		// Token: 0x04007A88 RID: 31368
		private bool fullBodyAnimationOwned;

		// Token: 0x02000F77 RID: 3959
		[Serializable]
		public sealed class SuspensionRope
		{
			// Token: 0x04007A89 RID: 31369
			public Vector3[] points = Array.Empty<Vector3>();

			// Token: 0x04007A8A RID: 31370
			public MeshFilter mesh;
		}

		// Token: 0x02000F78 RID: 3960
		[Serializable]
		public sealed class FittedPose
		{
			// Token: 0x04007A8B RID: 31371
			public Vector3[] points = Array.Empty<Vector3>();
		}

		// Token: 0x02000F79 RID: 3961
		private sealed class PoseData
		{
			// Token: 0x04007A8C RID: 31372
			public Vector3[] points;

			// Token: 0x04007A8D RID: 31373
			public Vector3 center;

			// Token: 0x04007A8E RID: 31374
			public Quaternion rotation;
		}

		// Token: 0x02000F7A RID: 3962
		private sealed class RopeVisual
		{
			// Token: 0x04007A8F RID: 31375
			public MeshFilter filter;

			// Token: 0x04007A90 RID: 31376
			public Mesh original;

			// Token: 0x04007A91 RID: 31377
			public Mesh owned;

			// Token: 0x04007A92 RID: 31378
			public Vector3[] vertices;

			// Token: 0x04007A93 RID: 31379
			public Vector3[] offsets;

			// Token: 0x04007A94 RID: 31380
			public Vector3[] restDirections;

			// Token: 0x04007A95 RID: 31381
			public Vector3[] restNormals;

			// Token: 0x04007A96 RID: 31382
			public Vector3[] normals;

			// Token: 0x04007A97 RID: 31383
			public Vector4[] restTangents;

			// Token: 0x04007A98 RID: 31384
			public Vector4[] tangents;

			// Token: 0x04007A99 RID: 31385
			public int[] segments;

			// Token: 0x04007A9A RID: 31386
			public float[] fractions;
		}
	}
}
