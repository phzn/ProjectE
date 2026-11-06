using System;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200009A RID: 154
	public class SimulationManager : IManager, IDisposable, IValid
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0001722A File Offset: 0x0001542A
		public int ParticleCount
		{
			get
			{
				ExNativeArray<float3> exNativeArray = this.nextPosArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0001723D File Offset: 0x0001543D
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x00017245 File Offset: 0x00015445
		internal int SimulationStepCount { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0001724E File Offset: 0x0001544E
		internal int WorkerCount
		{
			get
			{
				return JobsUtility.JobWorkerCount;
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00017258 File Offset: 0x00015458
		public void Dispose()
		{
			this.isValid = false;
			ExNativeArray<short> exNativeArray = this.teamIdArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<float3> exNativeArray2 = this.nextPosArray;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<float3> exNativeArray3 = this.oldPosArray;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray4 = this.oldRotArray;
			if (exNativeArray4 != null)
			{
				exNativeArray4.Dispose();
			}
			ExNativeArray<float3> exNativeArray5 = this.basePosArray;
			if (exNativeArray5 != null)
			{
				exNativeArray5.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray6 = this.baseRotArray;
			if (exNativeArray6 != null)
			{
				exNativeArray6.Dispose();
			}
			ExNativeArray<float3> exNativeArray7 = this.oldPositionArray;
			if (exNativeArray7 != null)
			{
				exNativeArray7.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray8 = this.oldRotationArray;
			if (exNativeArray8 != null)
			{
				exNativeArray8.Dispose();
			}
			ExNativeArray<float3> exNativeArray9 = this.velocityPosArray;
			if (exNativeArray9 != null)
			{
				exNativeArray9.Dispose();
			}
			ExNativeArray<float3> exNativeArray10 = this.dispPosArray;
			if (exNativeArray10 != null)
			{
				exNativeArray10.Dispose();
			}
			ExNativeArray<float3> exNativeArray11 = this.velocityArray;
			if (exNativeArray11 != null)
			{
				exNativeArray11.Dispose();
			}
			ExNativeArray<float3> exNativeArray12 = this.realVelocityArray;
			if (exNativeArray12 != null)
			{
				exNativeArray12.Dispose();
			}
			ExNativeArray<float> exNativeArray13 = this.frictionArray;
			if (exNativeArray13 != null)
			{
				exNativeArray13.Dispose();
			}
			ExNativeArray<float> exNativeArray14 = this.staticFrictionArray;
			if (exNativeArray14 != null)
			{
				exNativeArray14.Dispose();
			}
			ExNativeArray<float3> exNativeArray15 = this.collisionNormalArray;
			if (exNativeArray15 != null)
			{
				exNativeArray15.Dispose();
			}
			this.teamIdArray = null;
			this.nextPosArray = null;
			this.oldPosArray = null;
			this.oldRotArray = null;
			this.basePosArray = null;
			this.baseRotArray = null;
			this.oldPositionArray = null;
			this.oldRotationArray = null;
			this.velocityPosArray = null;
			this.dispPosArray = null;
			this.velocityArray = null;
			this.realVelocityArray = null;
			this.frictionArray = null;
			this.staticFrictionArray = null;
			this.collisionNormalArray = null;
			if (this.stepBasicPositionBuffer.IsCreated)
			{
				this.stepBasicPositionBuffer.Dispose();
			}
			if (this.stepBasicRotationBuffer.IsCreated)
			{
				this.stepBasicRotationBuffer.Dispose();
			}
			if (this.tempVectorBufferA.IsCreated)
			{
				this.tempVectorBufferA.Dispose();
			}
			if (this.tempVectorBufferB.IsCreated)
			{
				this.tempVectorBufferB.Dispose();
			}
			if (this.tempCountBuffer.IsCreated)
			{
				this.tempCountBuffer.Dispose();
			}
			if (this.tempFloatBufferA.IsCreated)
			{
				this.tempFloatBufferA.Dispose();
			}
			if (this.tempRotationBufferA.IsCreated)
			{
				this.tempRotationBufferA.Dispose();
			}
			if (this.tempRotationBufferB.IsCreated)
			{
				this.tempRotationBufferB.Dispose();
			}
			DistanceConstraint distanceConstraint = this.distanceConstraint;
			if (distanceConstraint != null)
			{
				distanceConstraint.Dispose();
			}
			TriangleBendingConstraint triangleBendingConstraint = this.bendingConstraint;
			if (triangleBendingConstraint != null)
			{
				triangleBendingConstraint.Dispose();
			}
			TetherConstraint tetherConstraint = this.tetherConstraint;
			if (tetherConstraint != null)
			{
				tetherConstraint.Dispose();
			}
			AngleConstraint angleConstraint = this.angleConstraint;
			if (angleConstraint != null)
			{
				angleConstraint.Dispose();
			}
			InertiaConstraint inertiaConstraint = this.inertiaConstraint;
			if (inertiaConstraint != null)
			{
				inertiaConstraint.Dispose();
			}
			ColliderCollisionConstraint colliderCollisionConstraint = this.colliderCollisionConstraint;
			if (colliderCollisionConstraint != null)
			{
				colliderCollisionConstraint.Dispose();
			}
			MotionConstraint motionConstraint = this.motionConstraint;
			if (motionConstraint != null)
			{
				motionConstraint.Dispose();
			}
			SelfCollisionConstraint selfCollisionConstraint = this.selfCollisionConstraint;
			if (selfCollisionConstraint != null)
			{
				selfCollisionConstraint.Dispose();
			}
			this.distanceConstraint = null;
			this.bendingConstraint = null;
			this.tetherConstraint = null;
			this.angleConstraint = null;
			this.inertiaConstraint = null;
			this.colliderCollisionConstraint = null;
			this.motionConstraint = null;
			this.selfCollisionConstraint = null;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00017554 File Offset: 0x00015754
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0001755C File Offset: 0x0001575C
		public void Initialize()
		{
			this.Dispose();
			this.teamIdArray = new ExNativeArray<short>(0, false);
			this.nextPosArray = new ExNativeArray<float3>(0, false);
			this.oldPosArray = new ExNativeArray<float3>(0, false);
			this.oldRotArray = new ExNativeArray<quaternion>(0, false);
			this.basePosArray = new ExNativeArray<float3>(0, false);
			this.baseRotArray = new ExNativeArray<quaternion>(0, false);
			this.oldPositionArray = new ExNativeArray<float3>(0, false);
			this.oldRotationArray = new ExNativeArray<quaternion>(0, false);
			this.velocityPosArray = new ExNativeArray<float3>(0, false);
			this.dispPosArray = new ExNativeArray<float3>(0, false);
			this.velocityArray = new ExNativeArray<float3>(0, false);
			this.realVelocityArray = new ExNativeArray<float3>(0, false);
			this.frictionArray = new ExNativeArray<float>(0, false);
			this.staticFrictionArray = new ExNativeArray<float>(0, false);
			this.collisionNormalArray = new ExNativeArray<float3>(0, false);
			this.distanceConstraint = new DistanceConstraint();
			this.bendingConstraint = new TriangleBendingConstraint();
			this.tetherConstraint = new TetherConstraint();
			this.angleConstraint = new AngleConstraint();
			this.inertiaConstraint = new InertiaConstraint();
			this.colliderCollisionConstraint = new ColliderCollisionConstraint();
			this.motionConstraint = new MotionConstraint();
			this.selfCollisionConstraint = new SelfCollisionConstraint();
			this.SimulationStepCount = 0;
			this.isValid = true;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00017698 File Offset: 0x00015898
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000176A0 File Offset: 0x000158A0
		internal void RegisterProxyMesh(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			int teamId = cprocess.TeamId;
			VirtualMesh shareVirtualMesh = cprocess.ProxyMeshContainer.shareVirtualMesh;
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			int vertexCount = shareVirtualMesh.VertexCount;
			teamDataRef.particleChunk = this.teamIdArray.AddRange(vertexCount, (short)teamId);
			this.nextPosArray.AddRange(vertexCount);
			this.oldPosArray.AddRange(vertexCount);
			this.oldRotArray.AddRange(vertexCount);
			this.basePosArray.AddRange(vertexCount);
			this.baseRotArray.AddRange(vertexCount);
			this.oldPositionArray.AddRange(vertexCount);
			this.oldRotationArray.AddRange(vertexCount);
			this.velocityPosArray.AddRange(vertexCount);
			this.dispPosArray.AddRange(vertexCount);
			this.velocityArray.AddRange(vertexCount);
			this.realVelocityArray.AddRange(vertexCount);
			this.frictionArray.AddRange(vertexCount);
			this.staticFrictionArray.AddRange(vertexCount);
			this.collisionNormalArray.AddRange(vertexCount);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000177A4 File Offset: 0x000159A4
		internal void RegisterConstraint(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			int teamId = cprocess.TeamId;
			MagicaManager.Team.centerDataArray[teamId] = cprocess.inertiaConstraintData.centerData;
			this.distanceConstraint.Register(cprocess);
			this.bendingConstraint.Register(cprocess);
			this.inertiaConstraint.Register(cprocess);
			this.selfCollisionConstraint.Register(cprocess);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0001780C File Offset: 0x00015A0C
		internal void ExitProxyMesh(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			int teamId = cprocess.TeamId;
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			teamDataRef.flag.SetBits(8, true);
			DataChunk particleChunk = teamDataRef.particleChunk;
			this.teamIdArray.RemoveAndFill(particleChunk, 0);
			this.nextPosArray.Remove(particleChunk);
			this.oldPosArray.Remove(particleChunk);
			this.oldRotArray.Remove(particleChunk);
			this.basePosArray.Remove(particleChunk);
			this.baseRotArray.Remove(particleChunk);
			this.oldPositionArray.Remove(particleChunk);
			this.oldRotationArray.Remove(particleChunk);
			this.velocityPosArray.Remove(particleChunk);
			this.dispPosArray.Remove(particleChunk);
			this.velocityArray.Remove(particleChunk);
			this.realVelocityArray.Remove(particleChunk);
			this.frictionArray.Remove(particleChunk);
			this.staticFrictionArray.Remove(particleChunk);
			this.collisionNormalArray.Remove(particleChunk);
			teamDataRef.particleChunk.Clear();
			this.distanceConstraint.Exit(cprocess);
			this.bendingConstraint.Exit(cprocess);
			this.inertiaConstraint.Exit(cprocess);
			this.selfCollisionConstraint.Exit(cprocess);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00017938 File Offset: 0x00015B38
		internal void WorkBufferUpdate()
		{
			int particleCount = this.ParticleCount;
			(ref this.stepBasicPositionBuffer).MC2Resize<float3>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.stepBasicRotationBuffer).MC2Resize<quaternion>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.tempVectorBufferA).MC2Resize<float3>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.tempVectorBufferB).MC2Resize<float3>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.tempCountBuffer).MC2Resize<int>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.tempFloatBufferA).MC2Resize<float>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.tempRotationBufferA).MC2Resize<quaternion>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			(ref this.tempRotationBufferB).MC2Resize<quaternion>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.selfCollisionConstraint.WorkBufferUpdate();
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000179C8 File Offset: 0x00015BC8
		internal JobHandle ClothSimulationSchedule(JobHandle jobHandle)
		{
			TeamManager team = MagicaManager.Team;
			TransformManager bone = MagicaManager.Bone;
			VirtualMeshManager vmesh = MagicaManager.VMesh;
			WindManager wind = MagicaManager.Wind;
			ColliderManager collider = MagicaManager.Collider;
			TimeManager time = MagicaManager.Time;
			int length = team.batchNormalClothTeamList.Length;
			int length2 = team.batchSplitClothTeamList.Length;
			bool flag = length > 0;
			bool flag2 = length2 > 0;
			if (!flag && !flag2)
			{
				return jobHandle;
			}
			int teamMaxUpdateCount = team.TeamMaxUpdateCount;
			int num = math.max(this.WorkerCount, 1);
			num *= 5;
			JobHandle jobHandle2 = default(JobHandle);
			JobHandle jobHandle3 = default(JobHandle);
			JobHandle jobHandle4 = default(JobHandle);
			JobHandle jobHandle5 = default(JobHandle);
			if (flag2)
			{
				this.selfCollisionConstraint.contactQueue.Clear();
				this.selfCollisionConstraint.contactList.Clear();
				this.selfCollisionConstraint.intersectQueue.Clear();
				this.selfCollisionConstraint.intersectList.Clear();
				bool flag3 = team.teamStatus.Value.z > 0;
				bool flag4 = team.teamStatus.Value.w > 0;
				bool flag5 = this.selfCollisionConstraint.IntersectCount > 0;
				jobHandle3 = new SimulationManager.SplitPre_A_Job
				{
					workerCount = num,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					transformLocalToWorldMatrixArray = bone.localToWorldMatrixArray.GetNativeArray(),
					attributes = vmesh.attributes.GetNativeArray(),
					localPositions = vmesh.localPositions.GetNativeArray(),
					localNormals = vmesh.localNormals.GetNativeArray(),
					localTangents = vmesh.localTangents.GetNativeArray(),
					boneWeights = vmesh.boneWeights.GetNativeArray(),
					skinBoneTransformIndices = vmesh.skinBoneTransformIndices.GetNativeArray(),
					skinBoneBindPoses = vmesh.skinBoneBindPoses.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray()
				}.Schedule(length2 * num, 1, jobHandle);
				jobHandle3 = new SimulationManager.SplitPre_B_Job
				{
					simulationDeltaTime = MagicaManager.Time.SimulationDeltaTime,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					centerDataArray = team.centerDataArray.GetNativeArray(),
					teamWindArray = team.teamWindArray.GetNativeArray(),
					parameterArray = team.parameterArray.GetNativeArray(),
					windZoneCount = wind.WindCount,
					windDataArray = wind.windDataArray.GetNativeArray(),
					transformPositionArray = bone.positionArray.GetNativeArray(),
					transformRotationArray = bone.rotationArray.GetNativeArray(),
					transformScaleArray = bone.scaleArray.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray(),
					vertexBindPoseRotations = vmesh.vertexBindPoseRotations.GetNativeArray(),
					fixedArray = this.inertiaConstraint.fixedArray.GetNativeArray()
				}.Schedule(length2, 1, jobHandle3);
				jobHandle3 = new SimulationManager.SplitPre_C_Job
				{
					workerCount = num,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					centerDataArray = team.centerDataArray.GetNativeArray(),
					parameterArray = team.parameterArray.GetNativeArray(),
					transformPositionArray = bone.positionArray.GetNativeArray(),
					transformRotationArray = bone.rotationArray.GetNativeArray(),
					transformScaleArray = bone.scaleArray.GetNativeArray(),
					transformLocalPositionArray = bone.localPositionArray.GetNativeArray(),
					transformLocalRotationArray = bone.localRotationArray.GetNativeArray(),
					transformLocalScaleArray = bone.localScaleArray.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray(),
					vertexDepths = vmesh.vertexDepths.GetNativeArray(),
					nextPosArray = this.nextPosArray.GetNativeArray(),
					oldPosArray = this.oldPosArray.GetNativeArray(),
					oldRotArray = this.oldRotArray.GetNativeArray(),
					basePosArray = this.basePosArray.GetNativeArray(),
					baseRotArray = this.baseRotArray.GetNativeArray(),
					oldPositionArray = this.oldPositionArray.GetNativeArray(),
					oldRotationArray = this.oldRotationArray.GetNativeArray(),
					velocityPosArray = this.velocityPosArray.GetNativeArray(),
					dispPosArray = this.dispPosArray.GetNativeArray(),
					velocityArray = this.velocityArray.GetNativeArray(),
					realVelocityArray = this.realVelocityArray.GetNativeArray(),
					frictionArray = this.frictionArray.GetNativeArray(),
					staticFrictionArray = this.staticFrictionArray.GetNativeArray(),
					collisionNormalArray = this.collisionNormalArray.GetNativeArray(),
					colliderFlagArray = collider.flagArray.GetNativeArray(),
					colliderCenterArray = collider.centerArray.GetNativeArray(),
					colliderFramePositions = collider.framePositions.GetNativeArray(),
					colliderFrameRotations = collider.frameRotations.GetNativeArray(),
					colliderFrameScales = collider.frameScales.GetNativeArray(),
					colliderOldFramePositions = collider.oldFramePositions.GetNativeArray(),
					colliderOldFrameRotations = collider.oldFrameRotations.GetNativeArray(),
					colliderNowPositions = collider.nowPositions.GetNativeArray(),
					colliderNowRotations = collider.nowRotations.GetNativeArray(),
					colliderOldPositions = collider.oldPositions.GetNativeArray(),
					colliderOldRotations = collider.oldRotations.GetNativeArray(),
					colliderMainColliderIndices = collider.mainColliderIndices.GetNativeArray()
				}.Schedule(length2 * num, 1, jobHandle3);
				bool flag6 = false;
				if (flag4 && teamMaxUpdateCount > 0 && flag5)
				{
					jobHandle4 = new SelfCollisionConstraint.SelfDetectionIntersectJob
					{
						workerCount = num,
						frameIndex = Time.frameCount % 2,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						primitiveArrayB = this.selfCollisionConstraint.primitiveArrayB.GetNativeArray(),
						uniformGridStartCountBuffer = this.selfCollisionConstraint.uniformGridStartCountBuffer.GetNativeArray(),
						intersectQueue = this.selfCollisionConstraint.intersectQueue.AsParallelWriter()
					}.Schedule(length2 * num, 1, jobHandle);
					jobHandle4 = new SelfCollisionConstraint.SelfConvertIntersectListJob
					{
						intersectQueue = this.selfCollisionConstraint.intersectQueue,
						intersectList = this.selfCollisionConstraint.intersectList
					}.Schedule(jobHandle4);
					flag6 = true;
				}
				for (int i = 0; i < teamMaxUpdateCount; i++)
				{
					bool flag7 = i == 0;
					jobHandle3 = new SimulationManager.SplitStep_A_Job
					{
						updateIndex = i,
						simulationPower = time.SimulationPower,
						simulationDeltaTime = time.SimulationDeltaTime,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						centerDataArray = team.centerDataArray.GetNativeArray(),
						teamWindArray = team.teamWindArray.GetNativeArray(),
						parameterArray = team.parameterArray.GetNativeArray(),
						colliderFlagArray = collider.flagArray.GetNativeArray(),
						colliderSizeArray = collider.sizeArray.GetNativeArray(),
						colliderFramePositions = collider.framePositions.GetNativeArray(),
						colliderFrameRotations = collider.frameRotations.GetNativeArray(),
						colliderFrameScales = collider.frameScales.GetNativeArray(),
						colliderOldFramePositions = collider.oldFramePositions.GetNativeArray(),
						colliderOldFrameRotations = collider.oldFrameRotations.GetNativeArray(),
						colliderNowPositions = collider.nowPositions.GetNativeArray(),
						colliderNowRotations = collider.nowRotations.GetNativeArray(),
						colliderOldPositions = collider.oldPositions.GetNativeArray(),
						colliderOldRotations = collider.oldRotations.GetNativeArray(),
						colliderWorkDataArray = collider.workDataArray.GetNativeArray()
					}.Schedule(length2, 1, jobHandle3);
					jobHandle3 = new SimulationManager.SplitStep_B_Job
					{
						workerCount = num,
						updateIndex = i,
						simulationPower = time.SimulationPower,
						simulationDeltaTime = time.SimulationDeltaTime,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						centerDataArray = team.centerDataArray.GetNativeArray(),
						teamWindArray = team.teamWindArray.GetNativeArray(),
						parameterArray = team.parameterArray.GetNativeArray(),
						windZoneCount = wind.WindCount,
						windDataArray = wind.windDataArray.GetNativeArray(),
						attributes = vmesh.attributes.GetNativeArray(),
						depthArray = vmesh.vertexDepths.GetNativeArray(),
						positions = vmesh.positions.GetNativeArray(),
						rotations = vmesh.rotations.GetNativeArray(),
						vertexRootIndices = vmesh.vertexRootIndices.GetNativeArray(),
						nextPosArray = this.nextPosArray.GetNativeArray(),
						oldPosArray = this.oldPosArray.GetNativeArray(),
						basePosArray = this.basePosArray.GetNativeArray(),
						baseRotArray = this.baseRotArray.GetNativeArray(),
						oldPositionArray = this.oldPositionArray.GetNativeArray(),
						oldRotationArray = this.oldRotationArray.GetNativeArray(),
						velocityPosArray = this.velocityPosArray.GetNativeArray(),
						velocityArray = this.velocityArray.GetNativeArray(),
						frictionArray = this.frictionArray.GetNativeArray(),
						stepBasicPositionBuffer = this.stepBasicPositionBuffer,
						stepBasicRotationBuffer = this.stepBasicRotationBuffer
					}.Schedule(length2 * num, 1, jobHandle3);
					jobHandle3 = new SimulationManager.SplitStep_C_Job
					{
						workerCount = num,
						updateIndex = i,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						attributes = vmesh.attributes.GetNativeArray(),
						vertexRootIndices = vmesh.vertexRootIndices.GetNativeArray(),
						vertexParentIndices = vmesh.vertexParentIndices.GetNativeArray(),
						baseLineStartDataIndices = vmesh.baseLineStartDataIndices.GetNativeArray(),
						baseLineDataCounts = vmesh.baseLineDataCounts.GetNativeArray(),
						baseLineData = vmesh.baseLineData.GetNativeArray(),
						vertexLocalPositions = vmesh.vertexLocalPositions.GetNativeArray(),
						vertexLocalRotations = vmesh.vertexLocalRotations.GetNativeArray(),
						basePosArray = this.basePosArray.GetNativeArray(),
						baseRotArray = this.baseRotArray.GetNativeArray(),
						stepBasicPositionBuffer = this.stepBasicPositionBuffer,
						stepBasicRotationBuffer = this.stepBasicRotationBuffer
					}.Schedule(length2 * num, 1, jobHandle3);
					jobHandle3 = new SimulationManager.SplitStep_D_Job
					{
						workerCount = num,
						updateIndex = i,
						simulationPower = time.SimulationPower,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						centerDataArray = team.centerDataArray.GetNativeArray(),
						parameterArray = team.parameterArray.GetNativeArray(),
						attributes = vmesh.attributes.GetNativeArray(),
						depthArray = vmesh.vertexDepths.GetNativeArray(),
						vertexRootIndices = vmesh.vertexRootIndices.GetNativeArray(),
						nextPosArray = this.nextPosArray.GetNativeArray(),
						basePosArray = this.basePosArray.GetNativeArray(),
						velocityPosArray = this.velocityPosArray.GetNativeArray(),
						frictionArray = this.frictionArray.GetNativeArray(),
						distanceIndexArray = this.distanceConstraint.indexArray.GetNativeArray(),
						distanceDataArray = this.distanceConstraint.dataArray.GetNativeArray(),
						distanceDistanceArray = this.distanceConstraint.distanceArray.GetNativeArray(),
						stepBasicPositionBuffer = this.stepBasicPositionBuffer
					}.Schedule(length2 * num, 1, jobHandle3);
					jobHandle3 = new SimulationManager.SplitStep_Angle_Job
					{
						workerCount = num,
						updateIndex = i,
						simulationPower = time.SimulationPower,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						parameterArray = team.parameterArray.GetNativeArray(),
						attributes = vmesh.attributes.GetNativeArray(),
						depthArray = vmesh.vertexDepths.GetNativeArray(),
						vertexRootIndices = vmesh.vertexRootIndices.GetNativeArray(),
						vertexParentIndices = vmesh.vertexParentIndices.GetNativeArray(),
						baseLineStartDataIndices = vmesh.baseLineStartDataIndices.GetNativeArray(),
						baseLineDataCounts = vmesh.baseLineDataCounts.GetNativeArray(),
						baseLineData = vmesh.baseLineData.GetNativeArray(),
						nextPosArray = this.nextPosArray.GetNativeArray(),
						velocityPosArray = this.velocityPosArray.GetNativeArray(),
						frictionArray = this.frictionArray.GetNativeArray(),
						distanceIndexArray = this.distanceConstraint.indexArray.GetNativeArray(),
						distanceDataArray = this.distanceConstraint.dataArray.GetNativeArray(),
						distanceDistanceArray = this.distanceConstraint.distanceArray.GetNativeArray(),
						stepBasicPositionBuffer = this.stepBasicPositionBuffer,
						stepBasicRotationBuffer = this.stepBasicRotationBuffer,
						tempVectorBufferA = this.tempVectorBufferA,
						tempVectorBufferB = this.tempVectorBufferB,
						tempFloatBufferA = this.tempFloatBufferA,
						tempRotationBufferA = this.tempRotationBufferA,
						tempRotationBufferB = this.tempRotationBufferB
					}.Schedule(length2 * num, 1, jobHandle3);
					jobHandle3 = new SimulationManager.SplitStep_Triangle_Job
					{
						workerCount = num,
						updateIndex = i,
						simulationPower = time.SimulationPower,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						parameterArray = team.parameterArray.GetNativeArray(),
						attributes = vmesh.attributes.GetNativeArray(),
						depthArray = vmesh.vertexDepths.GetNativeArray(),
						nextPosArray = this.nextPosArray.GetNativeArray(),
						frictionArray = this.frictionArray.GetNativeArray(),
						bendingTrianglePairArray = this.bendingConstraint.trianglePairArray.GetNativeArray(),
						bendingRestAngleOrVolumeArray = this.bendingConstraint.restAngleOrVolumeArray.GetNativeArray(),
						bendingSignOrVolumeArray = this.bendingConstraint.signOrVolumeArray.GetNativeArray(),
						tempVectorBufferA = this.tempVectorBufferA,
						tempCountBuffer = this.tempCountBuffer
					}.Schedule(length2 * num, 1, jobHandle3);
					jobHandle3 = new SimulationManager.SplitStep_E_Job
					{
						workerCount = num,
						updateIndex = i,
						simulationPower = time.SimulationPower,
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						parameterArray = team.parameterArray.GetNativeArray(),
						attributes = vmesh.attributes.GetNativeArray(),
						depthArray = vmesh.vertexDepths.GetNativeArray(),
						nextPosArray = this.nextPosArray.GetNativeArray(),
						basePosArray = this.basePosArray.GetNativeArray(),
						velocityPosArray = this.velocityPosArray.GetNativeArray(),
						frictionArray = this.frictionArray.GetNativeArray(),
						collisionNormalArray = this.collisionNormalArray.GetNativeArray(),
						colliderFlagArray = collider.flagArray.GetNativeArray(),
						colliderWorkDataArray = collider.workDataArray.GetNativeArray(),
						tempVectorBufferA = this.tempVectorBufferA,
						tempCountBuffer = this.tempCountBuffer
					}.Schedule(length2 * num, 1, jobHandle3);
					if (flag3)
					{
						jobHandle3 = new SimulationManager.SplitStep_Edge_Job
						{
							workerCount = num,
							updateIndex = i,
							simulationPower = time.SimulationPower,
							batchSelfTeamList = team.batchSplitClothTeamList,
							teamDataArray = team.teamDataArray.GetNativeArray(),
							parameterArray = team.parameterArray.GetNativeArray(),
							attributes = vmesh.attributes.GetNativeArray(),
							depthArray = vmesh.vertexDepths.GetNativeArray(),
							edges = vmesh.edges.GetNativeArray(),
							nextPosArray = this.nextPosArray.GetNativeArray(),
							colliderFlagArray = collider.flagArray.GetNativeArray(),
							colliderWorkDataArray = collider.workDataArray.GetNativeArray(),
							tempVectorBufferA = this.tempVectorBufferA,
							tempVectorBufferB = this.tempVectorBufferB,
							tempCountBuffer = this.tempCountBuffer,
							tempFloatBufferA = this.tempFloatBufferA
						}.Schedule(length2 * num, 1, jobHandle3);
					}
					if (flag4)
					{
						jobHandle3 = new SimulationManager.SplitStep_F_Self_Job
						{
							workerCount = num,
							updateIndex = i,
							simulationPower = time.SimulationPower,
							batchSelfTeamList = team.batchSplitClothTeamList,
							teamDataArray = team.teamDataArray.GetNativeArray(),
							parameterArray = team.parameterArray.GetNativeArray(),
							attributes = vmesh.attributes.GetNativeArray(),
							depthArray = vmesh.vertexDepths.GetNativeArray(),
							nextPosArray = this.nextPosArray.GetNativeArray(),
							basePosArray = this.basePosArray.GetNativeArray(),
							baseRotArray = this.baseRotArray.GetNativeArray(),
							velocityPosArray = this.velocityPosArray.GetNativeArray(),
							frictionArray = this.frictionArray.GetNativeArray(),
							collisionNormalArray = this.collisionNormalArray.GetNativeArray(),
							distanceIndexArray = this.distanceConstraint.indexArray.GetNativeArray(),
							distanceDataArray = this.distanceConstraint.dataArray.GetNativeArray(),
							distanceDistanceArray = this.distanceConstraint.distanceArray.GetNativeArray(),
							tempVectorBufferA = this.tempVectorBufferA,
							tempVectorBufferB = this.tempVectorBufferB,
							tempCountBuffer = this.tempCountBuffer,
							tempFloatBufferA = this.tempFloatBufferA
						}.Schedule(length2 * num, 1, jobHandle3);
						if (flag7 && flag6)
						{
							jobHandle3 = JobHandle.CombineDependencies(jobHandle4, jobHandle3);
						}
						if (flag7)
						{
							jobHandle3 = new SelfCollisionConstraint.SelfStep_UpdatePrimitiveJob
							{
								workerCount = 3,
								updateIndex = i,
								simulationPower = time.SimulationPower,
								batchSelfTeamList = team.batchSplitClothTeamList,
								teamDataArray = team.teamDataArray.GetNativeArray(),
								parameterArray = team.parameterArray.GetNativeArray(),
								nextPosArray = this.nextPosArray.GetNativeArray(),
								oldPosArray = this.oldPosArray.GetNativeArray(),
								frictionArray = this.frictionArray.GetNativeArray(),
								useIntersect = flag5,
								primitiveArrayB = this.selfCollisionConstraint.primitiveArrayB.GetNativeArray(),
								intersectFlagArray = this.selfCollisionConstraint.intersectFlagArray
							}.Schedule(length2 * 3, 1, jobHandle3);
							jobHandle3 = new SelfCollisionConstraint.SelfStep_UpdateGridJob
							{
								kindCount = 3,
								updateIndex = i,
								simulationPower = time.SimulationPower,
								batchSelfTeamList = team.batchSplitClothTeamList,
								teamDataArray = team.teamDataArray.GetNativeArray(),
								primitiveArrayB = this.selfCollisionConstraint.primitiveArrayB.GetNativeArray(),
								uniformGridStartCountBuffer = this.selfCollisionConstraint.uniformGridStartCountBuffer.GetNativeArray()
							}.Schedule(length2 * 3, 1, jobHandle3);
							jobHandle3 = new SelfCollisionConstraint.SelfStep_DetectionContactJob
							{
								updateIndex = i,
								workerCount = num,
								teamCount = length2,
								batchSelfTeamList = team.batchSplitClothTeamList,
								teamDataArray = team.teamDataArray.GetNativeArray(),
								nextPosArray = this.nextPosArray.GetNativeArray(),
								oldPosArray = this.oldPosArray.GetNativeArray(),
								primitiveArrayB = this.selfCollisionConstraint.primitiveArrayB.GetNativeArray(),
								uniformGridStartCountBuffer = this.selfCollisionConstraint.uniformGridStartCountBuffer.GetNativeArray(),
								contactQueue = this.selfCollisionConstraint.contactQueue.AsParallelWriter()
							}.Schedule(length2 * 6 * num, 1, jobHandle3);
							jobHandle3 = new SelfCollisionConstraint.SelfStep_ConvertContactListJob
							{
								contactQueue = this.selfCollisionConstraint.contactQueue,
								contactList = this.selfCollisionConstraint.contactList
							}.Schedule(jobHandle3);
						}
						else
						{
							jobHandle3 = new SelfCollisionConstraint.SelfStep_UpdateContactJob
							{
								first = (i == 0),
								contactList = this.selfCollisionConstraint.contactList,
								nextPosArray = this.nextPosArray.GetNativeArray(),
								oldPosArray = this.oldPosArray.GetNativeArray(),
								primitiveArrayB = this.selfCollisionConstraint.primitiveArrayB.GetNativeArray()
							}.Schedule(this.selfCollisionConstraint.contactList, 256, jobHandle3);
						}
						for (int j = 0; j < 4; j++)
						{
							jobHandle3 = new SelfCollisionConstraint.SelfStep_SolverContactJob
							{
								nextPosArray = this.nextPosArray.GetNativeArray(),
								primitiveArrayB = this.selfCollisionConstraint.primitiveArrayB.GetNativeArray(),
								contactList = this.selfCollisionConstraint.contactList,
								tempVectorBufferA = this.tempVectorBufferA,
								tempCountBuffer = this.tempCountBuffer
							}.Schedule(this.selfCollisionConstraint.contactList, 128, jobHandle3);
							jobHandle3 = new SelfCollisionConstraint.SelfStep_SumContactJob
							{
								updateIndex = i,
								batchSelfTeamList = team.batchSplitClothTeamList,
								teamDataArray = team.teamDataArray.GetNativeArray(),
								nextPosArray = this.nextPosArray.GetNativeArray(),
								tempVectorBufferA = this.tempVectorBufferA,
								tempCountBuffer = this.tempCountBuffer
							}.Schedule(length2, 1, jobHandle3);
						}
						jobHandle3 = new SimulationManager.SplitStep_G_Self_Job
						{
							workerCount = num,
							updateIndex = i,
							simulationDeltaTime = time.SimulationDeltaTime,
							batchSelfTeamList = team.batchSplitClothTeamList,
							teamDataArray = team.teamDataArray.GetNativeArray(),
							centerDataArray = team.centerDataArray.GetNativeArray(),
							parameterArray = team.parameterArray.GetNativeArray(),
							attributes = vmesh.attributes.GetNativeArray(),
							depthArray = vmesh.vertexDepths.GetNativeArray(),
							nextPosArray = this.nextPosArray.GetNativeArray(),
							oldPosArray = this.oldPosArray.GetNativeArray(),
							velocityPosArray = this.velocityPosArray.GetNativeArray(),
							velocityArray = this.velocityArray.GetNativeArray(),
							realVelocityArray = this.realVelocityArray.GetNativeArray(),
							frictionArray = this.frictionArray.GetNativeArray(),
							staticFrictionArray = this.staticFrictionArray.GetNativeArray(),
							collisionNormalArray = this.collisionNormalArray.GetNativeArray(),
							colliderNowPositions = collider.nowPositions.GetNativeArray(),
							colliderNowRotations = collider.nowRotations.GetNativeArray(),
							colliderOldPositions = collider.oldPositions.GetNativeArray(),
							colliderOldRotations = collider.oldRotations.GetNativeArray()
						}.Schedule(length2 * num, 1, jobHandle3);
					}
					else
					{
						jobHandle3 = new SimulationManager.SplitStep_FG_NoSelf_Job
						{
							workerCount = num,
							updateIndex = i,
							simulationPower = time.SimulationPower,
							simulationDeltaTime = time.SimulationDeltaTime,
							batchSelfTeamList = team.batchSplitClothTeamList,
							teamDataArray = team.teamDataArray.GetNativeArray(),
							centerDataArray = team.centerDataArray.GetNativeArray(),
							parameterArray = team.parameterArray.GetNativeArray(),
							attributes = vmesh.attributes.GetNativeArray(),
							depthArray = vmesh.vertexDepths.GetNativeArray(),
							nextPosArray = this.nextPosArray.GetNativeArray(),
							oldPosArray = this.oldPosArray.GetNativeArray(),
							basePosArray = this.basePosArray.GetNativeArray(),
							baseRotArray = this.baseRotArray.GetNativeArray(),
							velocityPosArray = this.velocityPosArray.GetNativeArray(),
							velocityArray = this.velocityArray.GetNativeArray(),
							realVelocityArray = this.realVelocityArray.GetNativeArray(),
							frictionArray = this.frictionArray.GetNativeArray(),
							staticFrictionArray = this.staticFrictionArray.GetNativeArray(),
							collisionNormalArray = this.collisionNormalArray.GetNativeArray(),
							colliderNowPositions = collider.nowPositions.GetNativeArray(),
							colliderNowRotations = collider.nowRotations.GetNativeArray(),
							colliderOldPositions = collider.oldPositions.GetNativeArray(),
							colliderOldRotations = collider.oldRotations.GetNativeArray(),
							distanceIndexArray = this.distanceConstraint.indexArray.GetNativeArray(),
							distanceDataArray = this.distanceConstraint.dataArray.GetNativeArray(),
							distanceDistanceArray = this.distanceConstraint.distanceArray.GetNativeArray(),
							tempVectorBufferA = this.tempVectorBufferA,
							tempVectorBufferB = this.tempVectorBufferB,
							tempCountBuffer = this.tempCountBuffer,
							tempFloatBufferA = this.tempFloatBufferA
						}.Schedule(length2 * num, 1, jobHandle3);
					}
				}
				jobHandle3 = new SimulationManager.SplitPost_DisplayPos_Job
				{
					workerCount = num,
					simulationDeltaTime = MagicaManager.Time.SimulationDeltaTime,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					attributes = vmesh.attributes.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray(),
					vertexRootIndices = vmesh.vertexRootIndices.GetNativeArray(),
					oldPosArray = this.oldPosArray.GetNativeArray(),
					oldPositionArray = this.oldPositionArray.GetNativeArray(),
					oldRotationArray = this.oldRotationArray.GetNativeArray(),
					dispPosArray = this.dispPosArray.GetNativeArray(),
					realVelocityArray = this.realVelocityArray.GetNativeArray()
				}.Schedule(length2 * num, 1, jobHandle3);
				if (flag6)
				{
					jobHandle5 = new SelfCollisionConstraint.SelfClearIntersectJob
					{
						batchSelfTeamList = team.batchSplitClothTeamList,
						teamDataArray = team.teamDataArray.GetNativeArray(),
						intersectFlagArray = this.selfCollisionConstraint.intersectFlagArray
					}.Schedule(length2, 1, jobHandle3);
					jobHandle5 = new SelfCollisionConstraint.SelfSolverIntersectJob
					{
						nextPosArray = this.nextPosArray.GetNativeArray(),
						intersectList = this.selfCollisionConstraint.intersectList,
						intersectFlagArray = this.selfCollisionConstraint.intersectFlagArray
					}.Schedule(this.selfCollisionConstraint.intersectList, 128, jobHandle5);
				}
				jobHandle3 = new SimulationManager.SplitPost_CalcProxy_Job
				{
					workerCount = num,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					parameterArray = team.parameterArray.GetNativeArray(),
					attributes = vmesh.attributes.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray(),
					baseLineStartDataIndices = vmesh.baseLineStartDataIndices.GetNativeArray(),
					baseLineDataCounts = vmesh.baseLineDataCounts.GetNativeArray(),
					baseLineData = vmesh.baseLineData.GetNativeArray(),
					vertexLocalPositions = vmesh.vertexLocalPositions.GetNativeArray(),
					vertexLocalRotations = vmesh.vertexLocalRotations.GetNativeArray(),
					vertexChildIndexArray = vmesh.vertexChildIndexArray.GetNativeArray(),
					vertexChildDataArray = vmesh.vertexChildDataArray.GetNativeArray(),
					baseLineFlags = vmesh.baseLineFlags.GetNativeArray()
				}.Schedule(length2 * num, 1, jobHandle3);
				jobHandle3 = new SimulationManager.SplitPost_CalcProxyTriangle_Job
				{
					workerCount = num,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					triangles = vmesh.triangles.GetNativeArray(),
					triangleNormals = vmesh.triangleNormals.GetNativeArray(),
					triangleTangents = vmesh.triangleTangents.GetNativeArray(),
					uvs = vmesh.uv.GetNativeArray()
				}.Schedule(length2 * num, 1, jobHandle3);
				jobHandle3 = new SimulationManager.SplitPost_SumProxyTriangleAndTransform_Job
				{
					workerCount = num,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					transformPositionArray = bone.positionArray.GetNativeArray(),
					transformRotationArray = bone.rotationArray.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray(),
					triangleNormals = vmesh.triangleNormals.GetNativeArray(),
					triangleTangents = vmesh.triangleTangents.GetNativeArray(),
					vertexToTriangles = vmesh.vertexToTriangles.GetNativeArray(),
					normalAdjustmentRotations = vmesh.normalAdjustmentRotations.GetNativeArray(),
					vertexToTransformRotations = vmesh.vertexToTransformRotations.GetNativeArray()
				}.Schedule(length2 * num, 1, jobHandle3);
				jobHandle3 = new SimulationManager.SplitPost_TeamCollider_Job
				{
					simulationDeltaTime = MagicaManager.Time.SimulationDeltaTime,
					batchSelfTeamList = team.batchSplitClothTeamList,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					centerDataArray = team.centerDataArray.GetNativeArray(),
					colliderFramePositions = collider.framePositions.GetNativeArray(),
					colliderFrameRotations = collider.frameRotations.GetNativeArray(),
					colliderOldFramePositions = collider.oldFramePositions.GetNativeArray(),
					colliderOldFrameRotations = collider.oldFrameRotations.GetNativeArray(),
					transformPositionArray = bone.positionArray.GetNativeArray(),
					transformRotationArray = bone.rotationArray.GetNativeArray(),
					transformScaleArray = bone.scaleArray.GetNativeArray(),
					transformLocalPositionArray = bone.localPositionArray.GetNativeArray(),
					transformLocalRotationArray = bone.localRotationArray.GetNativeArray(),
					attributes = vmesh.attributes.GetNativeArray(),
					vertexParentIndices = vmesh.vertexParentIndices.GetNativeArray()
				}.Schedule(length2, 1, jobHandle3);
				if (flag6)
				{
					jobHandle3 = JobHandle.CombineDependencies(jobHandle3, jobHandle5);
				}
			}
			if (flag)
			{
				jobHandle2 = new SimulationManager.SimulationNormalJob
				{
					batchNormalTeamList = team.batchNormalClothTeamList,
					simulationPower = time.SimulationPower,
					simulationDeltaTime = time.SimulationDeltaTime,
					mappingCount = team.MappingCount,
					teamDataArray = team.teamDataArray.GetNativeArray(),
					centerDataArray = team.centerDataArray.GetNativeArray(),
					teamWindArray = team.teamWindArray.GetNativeArray(),
					parameterArray = team.parameterArray.GetNativeArray(),
					windZoneCount = wind.WindCount,
					windDataArray = wind.windDataArray.GetNativeArray(),
					transformPositionArray = bone.positionArray.GetNativeArray(),
					transformRotationArray = bone.rotationArray.GetNativeArray(),
					transformScaleArray = bone.scaleArray.GetNativeArray(),
					transformLocalToWorldMatrixArray = bone.localToWorldMatrixArray.GetNativeArray(),
					transformLocalPositionArray = bone.localPositionArray.GetNativeArray(),
					transformLocalRotationArray = bone.localRotationArray.GetNativeArray(),
					transformLocalScaleArray = bone.localScaleArray.GetNativeArray(),
					attributes = vmesh.attributes.GetNativeArray(),
					depthArray = vmesh.vertexDepths.GetNativeArray(),
					localPositions = vmesh.localPositions.GetNativeArray(),
					localNormals = vmesh.localNormals.GetNativeArray(),
					localTangents = vmesh.localTangents.GetNativeArray(),
					boneWeights = vmesh.boneWeights.GetNativeArray(),
					skinBoneTransformIndices = vmesh.skinBoneTransformIndices.GetNativeArray(),
					skinBoneBindPoses = vmesh.skinBoneBindPoses.GetNativeArray(),
					positions = vmesh.positions.GetNativeArray(),
					rotations = vmesh.rotations.GetNativeArray(),
					vertexBindPoseRotations = vmesh.vertexBindPoseRotations.GetNativeArray(),
					vertexDepths = vmesh.vertexDepths.GetNativeArray(),
					vertexRootIndices = vmesh.vertexRootIndices.GetNativeArray(),
					vertexParentIndices = vmesh.vertexParentIndices.GetNativeArray(),
					baseLineStartDataIndices = vmesh.baseLineStartDataIndices.GetNativeArray(),
					baseLineDataCounts = vmesh.baseLineDataCounts.GetNativeArray(),
					baseLineData = vmesh.baseLineData.GetNativeArray(),
					vertexLocalPositions = vmesh.vertexLocalPositions.GetNativeArray(),
					vertexLocalRotations = vmesh.vertexLocalRotations.GetNativeArray(),
					vertexChildIndexArray = vmesh.vertexChildIndexArray.GetNativeArray(),
					vertexChildDataArray = vmesh.vertexChildDataArray.GetNativeArray(),
					baseLineFlags = vmesh.baseLineFlags.GetNativeArray(),
					triangles = vmesh.triangles.GetNativeArray(),
					triangleNormals = vmesh.triangleNormals.GetNativeArray(),
					triangleTangents = vmesh.triangleTangents.GetNativeArray(),
					uvs = vmesh.uv.GetNativeArray(),
					vertexToTriangles = vmesh.vertexToTriangles.GetNativeArray(),
					normalAdjustmentRotations = vmesh.normalAdjustmentRotations.GetNativeArray(),
					vertexToTransformRotations = vmesh.vertexToTransformRotations.GetNativeArray(),
					edges = vmesh.edges.GetNativeArray(),
					nextPosArray = this.nextPosArray.GetNativeArray(),
					oldPosArray = this.oldPosArray.GetNativeArray(),
					oldRotArray = this.oldRotArray.GetNativeArray(),
					basePosArray = this.basePosArray.GetNativeArray(),
					baseRotArray = this.baseRotArray.GetNativeArray(),
					oldPositionArray = this.oldPositionArray.GetNativeArray(),
					oldRotationArray = this.oldRotationArray.GetNativeArray(),
					velocityPosArray = this.velocityPosArray.GetNativeArray(),
					dispPosArray = this.dispPosArray.GetNativeArray(),
					velocityArray = this.velocityArray.GetNativeArray(),
					realVelocityArray = this.realVelocityArray.GetNativeArray(),
					frictionArray = this.frictionArray.GetNativeArray(),
					staticFrictionArray = this.staticFrictionArray.GetNativeArray(),
					collisionNormalArray = this.collisionNormalArray.GetNativeArray(),
					colliderFlagArray = collider.flagArray.GetNativeArray(),
					colliderCenterArray = collider.centerArray.GetNativeArray(),
					colliderSizeArray = collider.sizeArray.GetNativeArray(),
					colliderFramePositions = collider.framePositions.GetNativeArray(),
					colliderFrameRotations = collider.frameRotations.GetNativeArray(),
					colliderFrameScales = collider.frameScales.GetNativeArray(),
					colliderOldFramePositions = collider.oldFramePositions.GetNativeArray(),
					colliderOldFrameRotations = collider.oldFrameRotations.GetNativeArray(),
					colliderNowPositions = collider.nowPositions.GetNativeArray(),
					colliderNowRotations = collider.nowRotations.GetNativeArray(),
					colliderOldPositions = collider.oldPositions.GetNativeArray(),
					colliderOldRotations = collider.oldRotations.GetNativeArray(),
					colliderWorkDataArray = collider.workDataArray.GetNativeArray(),
					colliderMainColliderIndices = collider.mainColliderIndices.GetNativeArray(),
					fixedArray = this.inertiaConstraint.fixedArray.GetNativeArray(),
					distanceIndexArray = this.distanceConstraint.indexArray.GetNativeArray(),
					distanceDataArray = this.distanceConstraint.dataArray.GetNativeArray(),
					distanceDistanceArray = this.distanceConstraint.distanceArray.GetNativeArray(),
					bendingTrianglePairArray = this.bendingConstraint.trianglePairArray.GetNativeArray(),
					bendingRestAngleOrVolumeArray = this.bendingConstraint.restAngleOrVolumeArray.GetNativeArray(),
					bendingSignOrVolumeArray = this.bendingConstraint.signOrVolumeArray.GetNativeArray(),
					stepBasicPositionBuffer = this.stepBasicPositionBuffer,
					stepBasicRotationBuffer = this.stepBasicRotationBuffer,
					tempVectorBufferA = this.tempVectorBufferA,
					tempVectorBufferB = this.tempVectorBufferB,
					tempCountBuffer = this.tempCountBuffer,
					tempFloatBufferA = this.tempFloatBufferA,
					tempRotationBufferA = this.tempRotationBufferA,
					tempRotationBufferB = this.tempRotationBufferB
				}.Schedule(length, 1, jobHandle);
			}
			jobHandle = JobHandle.CombineDependencies(jobHandle3, jobHandle2);
			if (MagicaManager.Team.MappingCount > 0)
			{
				JobHandle jobHandle6 = vmesh.PostMappingMeshUpdateBatchSchedule(jobHandle, num);
				JobHandle jobHandle7 = bone.WriteTransformSchedule(jobHandle);
				jobHandle = JobHandle.CombineDependencies(jobHandle6, jobHandle7);
			}
			else
			{
				jobHandle = bone.WriteTransformSchedule(jobHandle);
			}
			return jobHandle;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0001A098 File Offset: 0x00018298
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Simulation Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Simulation Manager. Invalid");
			}
			else
			{
				stringBuilder.AppendLine(string.Format("Simulation Manager. Particle:{0}", this.ParticleCount));
				stringBuilder.AppendLine("  -teamIdArray:" + this.teamIdArray.ToSummary());
				stringBuilder.AppendLine("  -nextPosArray:" + this.nextPosArray.ToSummary());
				stringBuilder.AppendLine("  -oldPosArray:" + this.oldPosArray.ToSummary());
				stringBuilder.AppendLine("  -oldRotArray:" + this.oldRotArray.ToSummary());
				stringBuilder.AppendLine("  -basePosArray:" + this.basePosArray.ToSummary());
				stringBuilder.AppendLine("  -baseRotArray:" + this.baseRotArray.ToSummary());
				stringBuilder.AppendLine("  -oldPositionArray:" + this.oldPositionArray.ToSummary());
				stringBuilder.AppendLine("  -oldRotationArray:" + this.oldRotationArray.ToSummary());
				stringBuilder.AppendLine("  -velocityPosArray:" + this.velocityPosArray.ToSummary());
				stringBuilder.AppendLine("  -dispPosArray:" + this.dispPosArray.ToSummary());
				stringBuilder.AppendLine("  -velocityArray:" + this.velocityArray.ToSummary());
				stringBuilder.AppendLine("  -realVelocityArray:" + this.realVelocityArray.ToSummary());
				stringBuilder.AppendLine("  -frictionArray:" + this.frictionArray.ToSummary());
				stringBuilder.AppendLine("  -staticFrictionArray:" + this.staticFrictionArray.ToSummary());
				stringBuilder.AppendLine("  -collisionNormalArray:" + this.collisionNormalArray.ToSummary());
				stringBuilder.Append(this.distanceConstraint.ToString());
				stringBuilder.Append(this.bendingConstraint.ToString());
				stringBuilder.Append(this.angleConstraint.ToString());
				stringBuilder.Append(this.inertiaConstraint.ToString());
				stringBuilder.Append(this.colliderCollisionConstraint.ToString());
				stringBuilder.Append(this.selfCollisionConstraint.ToString());
				stringBuilder.AppendLine("[Buffer]");
				stringBuilder.AppendLine(string.Format("  -stepBasicPositionBuffer:{0}", this.stepBasicPositionBuffer.IsCreated ? this.stepBasicPositionBuffer.Length : 0));
				stringBuilder.AppendLine(string.Format("  -stepBasicRotationBuffer:{0}", this.stepBasicRotationBuffer.IsCreated ? this.stepBasicRotationBuffer.Length : 0));
				stringBuilder.AppendLine(string.Format("  -tempVectorBufferA:{0}", this.tempVectorBufferA.IsCreated ? this.tempVectorBufferA.Length : 0));
				stringBuilder.AppendLine(string.Format("  -tempVectorBufferB:{0}", this.tempVectorBufferB.IsCreated ? this.tempVectorBufferB.Length : 0));
				stringBuilder.AppendLine(string.Format("  -tempCountBuffer:{0}", this.tempCountBuffer.IsCreated ? this.tempCountBuffer.Length : 0));
				stringBuilder.AppendLine(string.Format("  -tempFloatBufferA:{0}", this.tempFloatBufferA.IsCreated ? this.tempFloatBufferA.Length : 0));
				stringBuilder.AppendLine(string.Format("  -tempRotationBufferA:{0}", this.tempRotationBufferA.IsCreated ? this.tempRotationBufferA.Length : 0));
				stringBuilder.AppendLine(string.Format("  -tempRotationBufferB:{0}", this.tempRotationBufferB.IsCreated ? this.tempRotationBufferB.Length : 0));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0001A4B4 File Offset: 0x000186B4
		private static void SimulationPreTeamUpdate(DataChunk chunk, ref TeamManager.TeamData tdata, in ClothParameters param, in InertiaConstraint.CenterData cdata, in NativeArray<float3> positions, in NativeArray<quaternion> rotations, in NativeArray<float> vertexDepths, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> oldPosArray, ref NativeArray<quaternion> oldRotArray, ref NativeArray<float3> basePosArray, ref NativeArray<quaternion> baseRotArray, ref NativeArray<float3> oldPositionArray, ref NativeArray<quaternion> oldRotationArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float3> dispPosArray, ref NativeArray<float3> velocityArray, ref NativeArray<float3> realVelocityArray, ref NativeArray<float> frictionArray, ref NativeArray<float> staticFrictionArray, ref NativeArray<float3> collisionNormalArray)
		{
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			if (tdata.IsReset)
			{
				int i = 0;
				while (i < chunk.dataLength)
				{
					NativeArray<float3> nativeArray = positions;
					float3 @float = nativeArray[num2];
					NativeArray<quaternion> nativeArray2 = rotations;
					quaternion quaternion = nativeArray2[num2];
					nextPosArray[num] = @float;
					oldPosArray[num] = @float;
					oldRotArray[num] = quaternion;
					basePosArray[num] = @float;
					baseRotArray[num] = quaternion;
					oldPositionArray[num] = @float;
					oldRotationArray[num] = quaternion;
					velocityPosArray[num] = @float;
					dispPosArray[num] = @float;
					velocityArray[num] = 0;
					realVelocityArray[num] = 0;
					frictionArray[num] = 0f;
					staticFrictionArray[num] = 0f;
					collisionNormalArray[num] = 0;
					i++;
					num++;
					num2++;
				}
				return;
			}
			if (tdata.IsInertiaShift || tdata.IsNegativeScaleTeleport)
			{
				int j = 0;
				while (j < chunk.dataLength)
				{
					float3 float2 = oldPosArray[num];
					quaternion quaternion2 = oldRotArray[num];
					float3 float3 = oldPositionArray[num];
					quaternion quaternion3 = oldRotationArray[num];
					float3 float4 = dispPosArray[num];
					float3 float5 = velocityArray[num];
					float3 float6 = realVelocityArray[num];
					if (tdata.IsNegativeScaleTeleport)
					{
						float4x4 negativeScaleMatrix = cdata.negativeScaleMatrix;
						float2 = MathUtility.TransformPoint(in float2, in negativeScaleMatrix);
						float3 float7 = 1;
						quaternion2 = MathUtility.TransformRotation(in quaternion2, in negativeScaleMatrix, in float7);
						float3 = MathUtility.TransformPoint(in float3, in negativeScaleMatrix);
						float7 = 1;
						quaternion3 = MathUtility.TransformRotation(in quaternion3, in negativeScaleMatrix, in float7);
						float4 = MathUtility.TransformPoint(in float4, in negativeScaleMatrix);
						float5 = MathUtility.TransformVector(in float5, in negativeScaleMatrix);
						float6 = MathUtility.TransformVector(in float6, in negativeScaleMatrix);
					}
					if (tdata.IsInertiaShift)
					{
						float2 = MathUtility.ShiftPosition(in float2, in cdata.oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
						quaternion2 = math.mul(cdata.frameComponentShiftRotation, quaternion2);
						float3 = MathUtility.ShiftPosition(in float3, in cdata.oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
						quaternion3 = math.mul(cdata.frameComponentShiftRotation, quaternion3);
						float4 = MathUtility.ShiftPosition(in float4, in cdata.oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
						float5 = math.mul(cdata.frameComponentShiftRotation, float5);
						float6 = math.mul(cdata.frameComponentShiftRotation, float6);
					}
					oldPosArray[num] = float2;
					oldRotArray[num] = quaternion2;
					oldPositionArray[num] = float3;
					oldRotationArray[num] = quaternion3;
					dispPosArray[num] = float4;
					velocityArray[num] = float5;
					realVelocityArray[num] = float6;
					j++;
					num++;
					num2++;
				}
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0001A79C File Offset: 0x0001899C
		private static float3 WindBatchJob(int teamId, in WindParams windParams, int vindex, int pindex, float depth, ref NativeArray<int> vertexRootIndices, ref TeamWindData teamWindData, ref NativeArray<WindManager.WindData> windDataArray, ref NativeArray<float> frictionArray)
		{
			float3 @float = 0;
			int num = vertexRootIndices[vindex];
			float3 float2 = (float)(teamId + 1) * 4.1923065f + (float)num * 0.0023963f * (1f - windParams.synchronization) * 100f;
			int zoneCount = teamWindData.ZoneCount;
			for (int i = 0; i < zoneCount; i++)
			{
				TeamWindInfo teamWindInfo = teamWindData.windZoneList[i];
				WindManager.WindData windData = windDataArray[teamWindInfo.windId];
				@float += SimulationManager.WindForceBlendBatchJob(in teamWindInfo, in windParams, in float2, windData.turbulence);
			}
			if (windParams.movingWind > 0.01f)
			{
				@float += SimulationManager.WindForceBlendBatchJob(in teamWindData.movingWind, in windParams, in float2, 1f);
			}
			float num2 = windParams.influence;
			float num3 = frictionArray[pindex];
			num2 *= 1f - num3;
			float num4 = depth * depth;
			num2 *= math.lerp(1f, num4, windParams.depthWeight);
			return @float * num2;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0001A8A4 File Offset: 0x00018AA4
		private static void SimulationStepUpdateParticles(DataChunk chunk, float4 simulationPower, float simulationDeltaTime, int teamId, ref TeamManager.TeamData tdata, ref InertiaConstraint.CenterData cdata, ref ClothParameters param, ref TeamWindData wdata, ref NativeArray<WindManager.WindData> windDataArray, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> depthArray, ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, ref NativeArray<int> vertexRootIndices, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> oldPosArray, ref NativeArray<float3> basePosArray, ref NativeArray<quaternion> baseRotArray, ref NativeArray<float3> oldPositionArray, ref NativeArray<quaternion> oldRotationArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float3> velocityArray, ref NativeArray<float> frictionArray, ref NativeArray<float3> stepBasicPositionBuffer, ref NativeArray<quaternion> stepBasicRotationBuffer)
		{
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				VertexAttribute vertexAttribute = attributes[num2];
				float num3 = depthArray[num2];
				float3 @float = oldPosArray[num];
				float3 float2 = @float;
				float3 float3 = @float;
				float3 float4 = oldPositionArray[num];
				quaternion quaternion = oldRotationArray[num];
				float3 float5 = positions[num2];
				quaternion quaternion2 = rotations[num2];
				float3 float6 = math.lerp(float4, float5, tdata.frameInterpolation);
				quaternion quaternion3 = math.slerp(quaternion, quaternion2, tdata.frameInterpolation);
				quaternion3 = math.normalize(quaternion3);
				basePosArray[num] = float6;
				baseRotArray[num] = quaternion3;
				stepBasicPositionBuffer[num] = float6;
				stepBasicRotationBuffer[num] = quaternion3;
				if (vertexAttribute.IsMove() || tdata.IsSpring)
				{
					float3 float7 = velocityArray[num];
					float3 float8 = cdata.inertiaVector;
					quaternion inertiaRotation = cdata.inertiaRotation;
					float num4 = param.inertiaConstraint.depthInertia * (1f - num3 * num3);
					float8 = math.lerp(float8, cdata.stepVector, num4);
					quaternion quaternion4 = math.slerp(inertiaRotation, cdata.stepRotation, num4);
					float3 float9 = @float - cdata.oldWorldPosition;
					float9 = math.mul(quaternion4, float9);
					float9 += float8;
					float3 float10 = cdata.oldWorldPosition + float9;
					float3 float11 = float10 - float2;
					float2 = float10;
					float3 += float11;
					float7 = math.mul(quaternion4, float7);
					float7 *= tdata.velocityWeight;
					float num5 = (in param.dampingCurveData).MC2EvaluateCurveClamp01(num3);
					float7 *= math.saturate(1f - num5 * simulationPower.z);
					float3 float12 = 0;
					float3 float13 = param.worldGravityDirection * (param.gravity * tdata.gravityRatio);
					float12 += float13;
					float3 float14 = 0;
					float num6 = MathUtility.CalcMass(num3);
					ClothForceMode forceMode = tdata.forceMode;
					if (forceMode <= ClothForceMode.VelocityChange)
					{
						if (forceMode != ClothForceMode.VelocityAdd)
						{
							if (forceMode == ClothForceMode.VelocityChange)
							{
								float14 = tdata.impactForce / num6;
								float7 = 0;
							}
						}
						else
						{
							float14 = tdata.impactForce / num6;
						}
					}
					else if (forceMode != ClothForceMode.VelocityAddWithoutDepth)
					{
						if (forceMode == ClothForceMode.VelocityChangeWithoutDepth)
						{
							float14 = tdata.impactForce;
							float7 = 0;
						}
					}
					else
					{
						float14 = tdata.impactForce;
					}
					float12 += float14;
					float12 += SimulationManager.WindBatchJob(teamId, in param.wind, num2, num, num3, ref vertexRootIndices, ref wdata, ref windDataArray, ref frictionArray);
					float12 *= tdata.scaleRatio;
					float7 += float12 * simulationDeltaTime;
					float2 += float7 * simulationDeltaTime;
				}
				else
				{
					float2 = float6;
					float3 = float6;
				}
				if (tdata.IsSpring && vertexAttribute.IsFixed())
				{
					SimulationManager.SpringBatchJob(in param.springConstraint, param.normalAxis, ref float2, in float6, in quaternion3, (tdata.time + (float)num * 49.6198f) * 2.4512f + math.csum(float2), tdata.scaleRatio);
				}
				velocityPosArray[num] = float3;
				nextPosArray[num] = float2;
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0001AC10 File Offset: 0x00018E10
		private static void SpringBatchJob(in SpringConstraint.SpringConstraintParams springParams, ClothNormalAxis normalAxis, ref float3 nextPos, in float3 basePos, in quaternion baseRot, float noiseTime, float scaleRatio)
		{
			float3 @float = nextPos - basePos;
			float3 float2 = math.up();
			switch (normalAxis)
			{
			case ClothNormalAxis.Right:
				float2 = math.right();
				break;
			case ClothNormalAxis.Up:
				float2 = math.up();
				break;
			case ClothNormalAxis.Forward:
				float2 = math.forward();
				break;
			case ClothNormalAxis.InverseRight:
				float2 = -math.right();
				break;
			case ClothNormalAxis.InverseUp:
				float2 = -math.up();
				break;
			case ClothNormalAxis.InverseForward:
				float2 = -math.forward();
				break;
			}
			float2 = math.mul(baseRot, float2);
			float num = springParams.limitDistance * scaleRatio;
			if (num > 1E-08f)
			{
				float num2 = math.length(@float);
				if (num2 > num)
				{
					@float *= num / num2;
				}
				if (springParams.normalLimitRatio < 1f)
				{
					float num3 = math.dot(float2, @float);
					float num4 = math.cos(math.asin(math.length(@float - float2 * num3) / num));
					num4 *= num * springParams.normalLimitRatio;
					if (math.abs(num3) > num4)
					{
						@float -= (math.abs(num3) - num4) * math.sign(num3) * float2;
					}
				}
			}
			else
			{
				@float = float3.zero;
			}
			float num5 = springParams.springPower;
			if (springParams.springNoise > 0f)
			{
				float num6 = math.sin(noiseTime);
				num6 *= springParams.springNoise * 0.6f;
				num5 = math.max(num5 + num5 * num6, 0f);
			}
			@float -= @float * num5;
			nextPos = basePos + @float;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001ADA4 File Offset: 0x00018FA4
		private static void SimulationStepUpdateBaseLinePose(DataChunk chunk, ref TeamManager.TeamData tdata, ref NativeArray<VertexAttribute> attributes, ref NativeArray<int> vertexParentIndices, ref NativeArray<ushort> baseLineStartDataIndices, ref NativeArray<ushort> baseLineDataCounts, ref NativeArray<ushort> baseLineData, ref NativeArray<float3> vertexLocalPositions, ref NativeArray<quaternion> vertexLocalRotations, ref NativeArray<float3> basePosArray, ref NativeArray<quaternion> baseRotArray, ref NativeArray<float3> stepBasicPositionBuffer, ref NativeArray<quaternion> stepBasicRotationBuffer)
		{
			float animationPoseRatio = tdata.animationPoseRatio;
			if (animationPoseRatio <= 0.99f)
			{
				int startIndex = tdata.baseLineDataChunk.startIndex;
				int startIndex2 = tdata.particleChunk.startIndex;
				int startIndex3 = tdata.proxyCommonChunk.startIndex;
				float3 @float = tdata.initScale * tdata.scaleRatio;
				int num = tdata.baseLineChunk.startIndex + chunk.startIndex;
				int i = 0;
				while (i < chunk.dataLength)
				{
					int num2 = (int)baseLineStartDataIndices[num];
					int num3 = (int)baseLineDataCounts[num];
					int num4 = num2 + startIndex;
					int j = 0;
					while (j < num3)
					{
						int num5 = (int)baseLineData[num4];
						int num6 = startIndex2 + num5;
						int num7 = startIndex3 + num5;
						int num8 = vertexParentIndices[num7];
						int num9 = num8 + startIndex2;
						if (attributes[num7].IsMove() && num8 >= 0)
						{
							float3 float2 = vertexLocalPositions[num7];
							quaternion quaternion = vertexLocalRotations[num7];
							float3 float3 = stepBasicPositionBuffer[num9];
							quaternion quaternion2 = stepBasicRotationBuffer[num9];
							float2 *= tdata.negativeScaleDirection;
							quaternion = quaternion.value * tdata.negativeScaleQuaternionValue;
							stepBasicPositionBuffer[num6] = math.mul(quaternion2, float2 * @float) + float3;
							stepBasicRotationBuffer[num6] = math.mul(quaternion2, quaternion);
						}
						else
						{
							quaternion quaternion3 = stepBasicRotationBuffer[num6];
							float4x4 float4x = float4x4.TRS(0, quaternion3, tdata.negativeScaleDirection);
							float3 xyz = float4x.c1.xyz;
							float3 xyz2 = float4x.c2.xyz;
							quaternion quaternion4 = MathUtility.ToRotation(in xyz, in xyz2);
							stepBasicRotationBuffer[num6] = quaternion4;
						}
						j++;
						num4++;
					}
					if (animationPoseRatio > 1E-08f)
					{
						num4 = num2 + startIndex;
						int k = 0;
						while (k < num3)
						{
							int num10 = (int)baseLineData[num4];
							int num11 = startIndex2 + num10;
							float3 float4 = basePosArray[num11];
							quaternion quaternion5 = baseRotArray[num11];
							stepBasicPositionBuffer[num11] = math.lerp(stepBasicPositionBuffer[num11], float4, animationPoseRatio);
							stepBasicRotationBuffer[num11] = math.slerp(stepBasicRotationBuffer[num11], quaternion5, animationPoseRatio);
							k++;
							num4++;
						}
					}
					i++;
					num++;
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001B008 File Offset: 0x00019208
		private static float3 WindForceBlendBatchJob(in TeamWindInfo windInfo, in WindParams windParams, in float3 windPos, float windTurbulence)
		{
			float num = windInfo.main;
			if (num < 0.01f)
			{
				return 0;
			}
			float num2 = num / 7.5f;
			float2 @float = math.sin((windPos + windInfo.time * 10f).xy);
			float3 float2 = windPos + windInfo.time * 2.3132f;
			float2 float3 = new float2(noise.cnoise(float2.xy), noise.cnoise(float2.yx));
			float3 *= 2.3f;
			float2 float4 = math.lerp(@float, float3, windParams.blend);
			windTurbulence *= windParams.turbulence;
			float2 float5 = math.radians(float4 * 45f);
			float5.y *= math.lerp(0.1f, 0.5f, windParams.blend);
			float5 *= windTurbulence;
			quaternion quaternion = quaternion.Euler(float5.x, float5.y, 0f, math.RotationOrder.ZXY);
			float3 float6 = math.forward(math.mul(MathUtility.AxisQuaternion(windInfo.direction), quaternion));
			float num3 = math.saturate(1f - num2 * 1f);
			float num4 = math.unlerp(-1f, 1f, float4.x);
			num4 *= num3 * windTurbulence;
			num -= num * num4;
			return float6 * num;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0001B168 File Offset: 0x00019368
		private static void SimulationStepPostTeam(DataChunk chunk, float simulationDeltaTime, int teamId, ref TeamManager.TeamData tdata, ref InertiaConstraint.CenterData cdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> depthArray, ref NativeArray<float3> oldPosArray, ref NativeArray<float3> velocityArray, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float> frictionArray, ref NativeArray<float> staticFrictionArray, ref NativeArray<float3> collisionNormalArray, ref NativeArray<float3> realVelocityArray)
		{
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				VertexAttribute vertexAttribute = attributes[num2];
				float num3 = depthArray[num2];
				float3 @float = nextPosArray[num];
				float3 float2 = oldPosArray[num];
				if (vertexAttribute.IsMove() || tdata.IsSpring)
				{
					float3 float3 = velocityPosArray[num];
					float num4 = frictionArray[num];
					float3 float4 = collisionNormalArray[num];
					object obj = math.lengthsq(float4) > 1E-08f && num4 > 1E-08f;
					float num5 = param.colliderCollisionConstraint.staticFriction * tdata.scaleRatio;
					float dynamicFriction = param.colliderCollisionConstraint.dynamicFriction;
					float num6 = staticFrictionArray[num];
					object obj2 = obj;
					if (obj2 != null && num5 > 0f)
					{
						float3 float5 = @float - float2;
						float3 float6 = float5 - MathUtility.Project(in float5, in float4);
						float num7 = math.length(float6) / simulationDeltaTime;
						if (num7 < num5)
						{
							num6 = math.saturate(num6 + 0.04f);
						}
						else
						{
							float num8 = math.max((num7 - num5) / 0.2f, 0.05f);
							num6 = math.saturate(num6 - num8);
						}
						float6 *= num6;
						@float -= float6;
						float3 -= float6;
					}
					else
					{
						num6 = math.saturate(num6 - 0.05f);
					}
					staticFrictionArray[num] = num6;
					float3 float7 = (@float - float3) / simulationDeltaTime;
					float num9 = math.lengthsq(float7);
					float3 float8 = ((num9 > 1E-08f) ? math.normalize(float7) : 0);
					if (obj2 != null && dynamicFriction > 0f && num9 >= 1E-08f)
					{
						float num10 = math.dot(float4, float8);
						num10 = 0.5f + 0.5f * num10;
						num10 *= num10;
						num10 = 1f - num10;
						float7 -= float7 * (num10 * math.saturate(num4 * dynamicFriction));
					}
					num4 *= 0.6f;
					frictionArray[num] = num4;
					if (param.inertiaConstraint.particleSpeedLimit >= 0f)
					{
						float7 = MathUtility.ClampVector(float7, param.inertiaConstraint.particleSpeedLimit * tdata.scaleRatio);
					}
					if (cdata.angularVelocity > 1E-08f && param.inertiaConstraint.centrifualAcceleration > 1E-08f && num9 >= 1E-08f)
					{
						float3 float9 = @float - cdata.nowWorldPosition;
						float3 float10 = MathUtility.ProjectOnPlane(in float9, in cdata.rotationAxis);
						float num11 = math.length(float10);
						if (num11 > 1E-08f)
						{
							float3 float11 = float10 / num11;
							float angularVelocity = cdata.angularVelocity;
							float num12 = (1f + (1f - num3)) * angularVelocity * angularVelocity * num11;
							float3 float12 = math.normalize(math.cross(cdata.rotationAxis, float11));
							num12 *= math.saturate(math.dot(float8, float12));
							float7 += float11 * (num12 * param.inertiaConstraint.centrifualAcceleration * 0.02f);
						}
					}
					float7 *= tdata.velocityWeight;
					velocityArray[num] = float7;
				}
				float3 float13 = (@float - float2) / simulationDeltaTime;
				realVelocityArray[num] = float13;
				oldPosArray[num] = @float;
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0001B4FC File Offset: 0x000196FC
		private static void SimulationCalcDisplayPosition(DataChunk chunk, float simulationDeltaTime, ref TeamManager.TeamData tdata, ref NativeArray<float3> oldPosArray, ref NativeArray<float3> realVelocityArray, ref NativeArray<float3> oldPositionArray, ref NativeArray<quaternion> oldRotationArray, ref NativeArray<float3> dispPosArray, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, ref NativeArray<int> vertexRootIndices)
		{
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int startIndex = tdata.proxyCommonChunk.startIndex;
			int num2 = startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				VertexAttribute vertexAttribute = attributes[num2];
				float3 @float = positions[num2];
				quaternion quaternion = rotations[num2];
				if (vertexAttribute.IsMove() || tdata.IsSpring)
				{
					float3 float2 = oldPosArray[num];
					float3 float3 = realVelocityArray[num] * simulationDeltaTime;
					float3 float4 = float2 + float3;
					float num3 = tdata.nowUpdateTime + simulationDeltaTime - tdata.oldTime;
					float num4 = ((num3 > 0f) ? ((tdata.time - tdata.oldTime) / num3) : 0f);
					float4 = math.lerp(dispPosArray[num], float4, num4);
					int num5 = vertexRootIndices[num2];
					if (num5 >= 0)
					{
						float3 float5 = positions[startIndex + num5];
						float num6 = math.distance(float5, @float) * 1.3f;
						float3 float6 = float4 - float5;
						float6 = MathUtility.ClampVector(float6, num6);
						float4 = float5 + float6;
					}
					float3 float7 = float4;
					dispPosArray[num] = float7;
					float3 float8 = math.lerp(positions[num2], float7, tdata.blendWeight);
					positions[num2] = float8;
				}
				else
				{
					float3 float9 = positions[num2];
					dispPosArray[num] = float9;
				}
				if (tdata.IsRunning)
				{
					oldPositionArray[num] = @float;
					oldRotationArray[num] = quaternion;
				}
				if (tdata.IsNegativeScale)
				{
					float4x4 float4x = float4x4.TRS(0, quaternion, tdata.negativeScaleDirection);
					float3 xyz = float4x.c1.xyz;
					float3 xyz2 = float4x.c2.xyz;
					quaternion = MathUtility.ToRotation(in xyz, in xyz2);
					rotations[num2] = quaternion;
				}
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0001B6E8 File Offset: 0x000198E8
		private static void SimulationClearTempBuffer(DataChunk chunk, ref TeamManager.TeamData tdata, ref NativeArray<float3> tempVectorBufferA, ref NativeArray<float3> tempVectorBufferB, ref NativeArray<int> tempCountBuffer, ref NativeArray<float> tempFloatBufferA)
		{
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				tempVectorBufferA[num] = 0;
				tempVectorBufferB[num] = 0;
				tempCountBuffer[num] = 0;
				tempFloatBufferA[num] = 0f;
				i++;
				num++;
			}
		}

		// Token: 0x0400044B RID: 1099
		public ExNativeArray<short> teamIdArray;

		// Token: 0x0400044C RID: 1100
		public ExNativeArray<float3> nextPosArray;

		// Token: 0x0400044D RID: 1101
		public ExNativeArray<float3> oldPosArray;

		// Token: 0x0400044E RID: 1102
		public ExNativeArray<quaternion> oldRotArray;

		// Token: 0x0400044F RID: 1103
		public ExNativeArray<float3> basePosArray;

		// Token: 0x04000450 RID: 1104
		public ExNativeArray<quaternion> baseRotArray;

		// Token: 0x04000451 RID: 1105
		public ExNativeArray<float3> oldPositionArray;

		// Token: 0x04000452 RID: 1106
		public ExNativeArray<quaternion> oldRotationArray;

		// Token: 0x04000453 RID: 1107
		public ExNativeArray<float3> velocityPosArray;

		// Token: 0x04000454 RID: 1108
		public ExNativeArray<float3> dispPosArray;

		// Token: 0x04000455 RID: 1109
		public ExNativeArray<float3> velocityArray;

		// Token: 0x04000456 RID: 1110
		public ExNativeArray<float3> realVelocityArray;

		// Token: 0x04000457 RID: 1111
		public ExNativeArray<float> frictionArray;

		// Token: 0x04000458 RID: 1112
		public ExNativeArray<float> staticFrictionArray;

		// Token: 0x04000459 RID: 1113
		public ExNativeArray<float3> collisionNormalArray;

		// Token: 0x0400045A RID: 1114
		public DistanceConstraint distanceConstraint;

		// Token: 0x0400045B RID: 1115
		public TriangleBendingConstraint bendingConstraint;

		// Token: 0x0400045C RID: 1116
		public TetherConstraint tetherConstraint;

		// Token: 0x0400045D RID: 1117
		public AngleConstraint angleConstraint;

		// Token: 0x0400045E RID: 1118
		public InertiaConstraint inertiaConstraint;

		// Token: 0x0400045F RID: 1119
		public ColliderCollisionConstraint colliderCollisionConstraint;

		// Token: 0x04000460 RID: 1120
		public MotionConstraint motionConstraint;

		// Token: 0x04000461 RID: 1121
		public SelfCollisionConstraint selfCollisionConstraint;

		// Token: 0x04000462 RID: 1122
		public NativeArray<float3> stepBasicPositionBuffer;

		// Token: 0x04000463 RID: 1123
		public NativeArray<quaternion> stepBasicRotationBuffer;

		// Token: 0x04000464 RID: 1124
		internal NativeArray<float3> tempVectorBufferA;

		// Token: 0x04000465 RID: 1125
		internal NativeArray<float3> tempVectorBufferB;

		// Token: 0x04000466 RID: 1126
		internal NativeArray<int> tempCountBuffer;

		// Token: 0x04000467 RID: 1127
		internal NativeArray<float> tempFloatBufferA;

		// Token: 0x04000468 RID: 1128
		internal NativeArray<quaternion> tempRotationBufferA;

		// Token: 0x04000469 RID: 1129
		internal NativeArray<quaternion> tempRotationBufferB;

		// Token: 0x0400046B RID: 1131
		internal int splitProxyMeshVertexCount = 300;

		// Token: 0x0400046C RID: 1132
		private bool isValid;

		// Token: 0x0200009B RID: 155
		[BurstCompile]
		private struct SimulationNormalJob : IJobParallelFor
		{
			// Token: 0x060002B8 RID: 696 RVA: 0x0001B760 File Offset: 0x00019960
			public unsafe void Execute(int localIndex)
			{
				int num = this.batchNormalTeamList[localIndex];
				TeamManager.TeamData* unsafePtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafePtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafePtr2 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafePtr<InertiaConstraint.CenterData>();
				TeamWindData* unsafePtr3 = (TeamWindData*)this.teamWindArray.GetUnsafePtr<TeamWindData>();
				ref TeamManager.TeamData ptr = ref unsafePtr[num];
				ref InertiaConstraint.CenterData ptr2 = ref unsafePtr2[num];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr[num];
				ref TeamWindData ptr4 = ref unsafePtr3[num];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				VirtualMeshManager.SimulationPreProxyMeshUpdate(new DataChunk(0, ptr.proxyCommonChunk.dataLength), num, ref ptr, in this.attributes, in this.localPositions, in this.localNormals, in this.localTangents, in this.boneWeights, in this.skinBoneTransformIndices, in this.skinBoneBindPoses, ref this.positions, ref this.rotations, in this.transformLocalToWorldMatrixArray);
				TeamManager.SimulationCalcCenterAndInertiaAndWind(this.simulationDeltaTime, num, ref ptr, ref ptr2, ref ptr4, ref ptr3, in this.positions, in this.rotations, in this.vertexBindPoseRotations, in this.fixedArray, in this.transformPositionArray, in this.transformRotationArray, in this.transformScaleArray, this.windZoneCount, in this.windDataArray);
				SimulationManager.SimulationPreTeamUpdate(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, in ptr3, in ptr2, in this.positions, in this.rotations, in this.vertexDepths, ref this.nextPosArray, ref this.oldPosArray, ref this.oldRotArray, ref this.basePosArray, ref this.baseRotArray, ref this.oldPositionArray, ref this.oldRotationArray, ref this.velocityPosArray, ref this.dispPosArray, ref this.velocityArray, ref this.realVelocityArray, ref this.frictionArray, ref this.staticFrictionArray, ref this.collisionNormalArray);
				if (ptr.colliderCount > 0)
				{
					ColliderManager.SimulationPreUpdate(new DataChunk(0, ptr.colliderChunk.dataLength), ref ptr, ref ptr2, ref this.colliderFlagArray, ref this.colliderCenterArray, ref this.colliderFramePositions, ref this.colliderFrameRotations, ref this.colliderFrameScales, ref this.colliderOldFramePositions, ref this.colliderOldFrameRotations, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations, ref this.colliderMainColliderIndices, ref this.transformPositionArray, ref this.transformRotationArray, ref this.transformScaleArray, ref this.transformLocalPositionArray, ref this.transformLocalRotationArray, ref this.transformLocalScaleArray);
				}
				for (int i = 0; i < ptr.updateCount; i++)
				{
					TeamManager.SimulationStepTeamUpdate(i, this.simulationDeltaTime, num, ref ptr, ref ptr3, ref ptr2, ref ptr4);
					if (ptr.colliderCount > 0)
					{
						ColliderManager.SimulationStartStep(ref ptr, ref ptr2, ref this.colliderFlagArray, ref this.colliderSizeArray, ref this.colliderFramePositions, ref this.colliderFrameRotations, ref this.colliderFrameScales, ref this.colliderOldFramePositions, ref this.colliderOldFrameRotations, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations, ref this.colliderWorkDataArray);
					}
					SimulationManager.SimulationStepUpdateParticles(new DataChunk(0, ptr.particleChunk.dataLength), this.simulationPower, this.simulationDeltaTime, num, ref ptr, ref ptr2, ref ptr3, ref ptr4, ref this.windDataArray, ref this.attributes, ref this.depthArray, ref this.positions, ref this.rotations, ref this.vertexRootIndices, ref this.nextPosArray, ref this.oldPosArray, ref this.basePosArray, ref this.baseRotArray, ref this.oldPositionArray, ref this.oldRotationArray, ref this.velocityPosArray, ref this.velocityArray, ref this.frictionArray, ref this.stepBasicPositionBuffer, ref this.stepBasicRotationBuffer);
					SimulationManager.SimulationStepUpdateBaseLinePose(new DataChunk(0, ptr.baseLineChunk.dataLength), ref ptr, ref this.attributes, ref this.vertexParentIndices, ref this.baseLineStartDataIndices, ref this.baseLineDataCounts, ref this.baseLineData, ref this.vertexLocalPositions, ref this.vertexLocalRotations, ref this.basePosArray, ref this.baseRotArray, ref this.stepBasicPositionBuffer, ref this.stepBasicRotationBuffer);
					TetherConstraint.SolverConstraint(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, ref ptr3, ref ptr2, ref this.attributes, ref this.depthArray, ref this.vertexRootIndices, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.stepBasicPositionBuffer);
					DistanceConstraint.SolverConstraint(new DataChunk(0, ptr.particleChunk.dataLength), this.simulationPower, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.basePosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.distanceIndexArray, ref this.distanceDataArray, ref this.distanceDistanceArray);
					AngleConstraint.SolverConstraint(new DataChunk(0, ptr.baseLineChunk.dataLength), in this.simulationPower, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.vertexParentIndices, ref this.baseLineStartDataIndices, ref this.baseLineDataCounts, ref this.baseLineData, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.stepBasicPositionBuffer, ref this.stepBasicRotationBuffer, ref this.tempFloatBufferA, ref this.tempVectorBufferA, ref this.tempRotationBufferA, ref this.tempRotationBufferB, ref this.tempVectorBufferB);
					SimulationManager.SimulationClearTempBuffer(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, ref this.tempVectorBufferA, ref this.tempVectorBufferB, ref this.tempCountBuffer, ref this.tempFloatBufferA);
					TriangleBendingConstraint.SolverConstraint(new DataChunk(0, ptr.bendingPairChunk.dataLength), in this.simulationPower, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.frictionArray, ref this.bendingTrianglePairArray, ref this.bendingRestAngleOrVolumeArray, ref this.bendingSignOrVolumeArray, ref this.tempVectorBufferA, ref this.tempCountBuffer);
					TriangleBendingConstraint.SumConstraint(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, ref ptr3, ref this.attributes, ref this.nextPosArray, ref this.tempVectorBufferA, ref this.tempCountBuffer);
					if (ptr3.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Point)
					{
						ColliderCollisionConstraint.SolverPointConstraint(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.frictionArray, ref this.collisionNormalArray, ref this.velocityPosArray, ref this.basePosArray, ref this.colliderFlagArray, ref this.colliderWorkDataArray);
					}
					else if (ptr3.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Edge)
					{
						ColliderCollisionConstraint.SolverEdgeConstraint(new DataChunk(0, ptr.proxyEdgeChunk.dataLength), ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.edges, ref this.nextPosArray, ref this.colliderFlagArray, ref this.colliderWorkDataArray, ref this.tempVectorBufferA, ref this.tempVectorBufferB, ref this.tempCountBuffer, ref this.tempFloatBufferA);
						ColliderCollisionConstraint.SumEdgeConstraint(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, ref ptr3, ref this.nextPosArray, ref this.frictionArray, ref this.collisionNormalArray, ref this.tempVectorBufferA, ref this.tempVectorBufferB, ref this.tempCountBuffer, ref this.tempFloatBufferA);
					}
					DistanceConstraint.SolverConstraint(new DataChunk(0, ptr.particleChunk.dataLength), this.simulationPower, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.basePosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.distanceIndexArray, ref this.distanceDataArray, ref this.distanceDistanceArray);
					MotionConstraint.SolverConstraint(new DataChunk(0, ptr.particleChunk.dataLength), ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.basePosArray, ref this.baseRotArray, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.collisionNormalArray);
					SimulationManager.SimulationStepPostTeam(new DataChunk(0, ptr.particleChunk.dataLength), this.simulationDeltaTime, num, ref ptr, ref ptr2, ref ptr3, ref this.attributes, ref this.depthArray, ref this.oldPosArray, ref this.velocityArray, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.staticFrictionArray, ref this.collisionNormalArray, ref this.realVelocityArray);
					ColliderManager.SimulationEndStep(new DataChunk(0, ptr.colliderChunk.dataLength), ref ptr, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations);
				}
				SimulationManager.SimulationCalcDisplayPosition(new DataChunk(0, ptr.particleChunk.dataLength), this.simulationDeltaTime, ref ptr, ref this.oldPosArray, ref this.realVelocityArray, ref this.oldPositionArray, ref this.oldRotationArray, ref this.dispPosArray, ref this.attributes, ref this.positions, ref this.rotations, ref this.vertexRootIndices);
				VirtualMeshManager.SimulationPostProxyMeshUpdateLine(new DataChunk(0, ptr.baseLineChunk.dataLength), ref ptr, ref ptr3, ref this.attributes, ref this.positions, ref this.rotations, ref this.vertexLocalPositions, ref this.vertexLocalRotations, ref this.vertexChildIndexArray, ref this.vertexChildDataArray, ref this.baseLineFlags, ref this.baseLineStartDataIndices, ref this.baseLineDataCounts, ref this.baseLineData);
				VirtualMeshManager.SimulationPostProxyMeshUpdateTriangle(new DataChunk(0, ptr.proxyTriangleChunk.dataLength), ref ptr, ref this.positions, ref this.triangles, ref this.triangleNormals, ref this.triangleTangents, ref this.uvs);
				VirtualMeshManager.SimulationPostProxyMeshUpdateTriangleSum(new DataChunk(0, ptr.proxyCommonChunk.dataLength), ref ptr, ref this.rotations, ref this.triangleNormals, ref this.triangleTangents, ref this.vertexToTriangles, ref this.normalAdjustmentRotations);
				VirtualMeshManager.SimulationPostProxyMeshUpdateWorldTransform(new DataChunk(0, ptr.proxyCommonChunk.dataLength), ref ptr, ref this.positions, ref this.rotations, ref this.vertexToTransformRotations, ref this.transformPositionArray, ref this.transformRotationArray);
				VirtualMeshManager.SimulationPostProxyMeshUpdateLocalTransform(ref ptr, ref this.attributes, ref this.vertexParentIndices, ref this.transformPositionArray, ref this.transformRotationArray, ref this.transformScaleArray, ref this.transformLocalPositionArray, ref this.transformLocalRotationArray);
				ColliderManager.SimulationPostUpdate(ref ptr, ref this.colliderFramePositions, ref this.colliderFrameRotations, ref this.colliderOldFramePositions, ref this.colliderOldFrameRotations);
				TeamManager.SimulationPostTeamUpdate(ref ptr, ref ptr2);
			}

			// Token: 0x0400046D RID: 1133
			[ReadOnly]
			public NativeList<int> batchNormalTeamList;

			// Token: 0x0400046E RID: 1134
			public float4 simulationPower;

			// Token: 0x0400046F RID: 1135
			public float simulationDeltaTime;

			// Token: 0x04000470 RID: 1136
			public int mappingCount;

			// Token: 0x04000471 RID: 1137
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000472 RID: 1138
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x04000473 RID: 1139
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamWindData> teamWindArray;

			// Token: 0x04000474 RID: 1140
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x04000475 RID: 1141
			public int windZoneCount;

			// Token: 0x04000476 RID: 1142
			[ReadOnly]
			public NativeArray<WindManager.WindData> windDataArray;

			// Token: 0x04000477 RID: 1143
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x04000478 RID: 1144
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x04000479 RID: 1145
			[ReadOnly]
			public NativeArray<float3> transformScaleArray;

			// Token: 0x0400047A RID: 1146
			[ReadOnly]
			public NativeArray<float4x4> transformLocalToWorldMatrixArray;

			// Token: 0x0400047B RID: 1147
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> transformLocalPositionArray;

			// Token: 0x0400047C RID: 1148
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> transformLocalRotationArray;

			// Token: 0x0400047D RID: 1149
			[ReadOnly]
			public NativeArray<float3> transformLocalScaleArray;

			// Token: 0x0400047E RID: 1150
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400047F RID: 1151
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x04000480 RID: 1152
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000481 RID: 1153
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x04000482 RID: 1154
			[ReadOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x04000483 RID: 1155
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x04000484 RID: 1156
			[ReadOnly]
			public NativeArray<int> skinBoneTransformIndices;

			// Token: 0x04000485 RID: 1157
			[ReadOnly]
			public NativeArray<float4x4> skinBoneBindPoses;

			// Token: 0x04000486 RID: 1158
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> positions;

			// Token: 0x04000487 RID: 1159
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> rotations;

			// Token: 0x04000488 RID: 1160
			[ReadOnly]
			public NativeArray<quaternion> vertexBindPoseRotations;

			// Token: 0x04000489 RID: 1161
			[ReadOnly]
			public NativeArray<float> vertexDepths;

			// Token: 0x0400048A RID: 1162
			[ReadOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x0400048B RID: 1163
			[ReadOnly]
			public NativeArray<int> vertexParentIndices;

			// Token: 0x0400048C RID: 1164
			[ReadOnly]
			public NativeArray<ushort> baseLineStartDataIndices;

			// Token: 0x0400048D RID: 1165
			[ReadOnly]
			public NativeArray<ushort> baseLineDataCounts;

			// Token: 0x0400048E RID: 1166
			[ReadOnly]
			public NativeArray<ushort> baseLineData;

			// Token: 0x0400048F RID: 1167
			[ReadOnly]
			public NativeArray<float3> vertexLocalPositions;

			// Token: 0x04000490 RID: 1168
			[ReadOnly]
			public NativeArray<quaternion> vertexLocalRotations;

			// Token: 0x04000491 RID: 1169
			[ReadOnly]
			public NativeArray<uint> vertexChildIndexArray;

			// Token: 0x04000492 RID: 1170
			[ReadOnly]
			public NativeArray<ushort> vertexChildDataArray;

			// Token: 0x04000493 RID: 1171
			[ReadOnly]
			public NativeArray<ExBitFlag8> baseLineFlags;

			// Token: 0x04000494 RID: 1172
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000495 RID: 1173
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> triangleNormals;

			// Token: 0x04000496 RID: 1174
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> triangleTangents;

			// Token: 0x04000497 RID: 1175
			[ReadOnly]
			public NativeArray<float2> uvs;

			// Token: 0x04000498 RID: 1176
			[ReadOnly]
			public NativeArray<FixedList32Bytes<uint>> vertexToTriangles;

			// Token: 0x04000499 RID: 1177
			[ReadOnly]
			public NativeArray<quaternion> normalAdjustmentRotations;

			// Token: 0x0400049A RID: 1178
			[ReadOnly]
			public NativeArray<quaternion> vertexToTransformRotations;

			// Token: 0x0400049B RID: 1179
			[ReadOnly]
			public NativeArray<int2> edges;

			// Token: 0x0400049C RID: 1180
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x0400049D RID: 1181
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPosArray;

			// Token: 0x0400049E RID: 1182
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> oldRotArray;

			// Token: 0x0400049F RID: 1183
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> basePosArray;

			// Token: 0x040004A0 RID: 1184
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> baseRotArray;

			// Token: 0x040004A1 RID: 1185
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPositionArray;

			// Token: 0x040004A2 RID: 1186
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> oldRotationArray;

			// Token: 0x040004A3 RID: 1187
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x040004A4 RID: 1188
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> dispPosArray;

			// Token: 0x040004A5 RID: 1189
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityArray;

			// Token: 0x040004A6 RID: 1190
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> realVelocityArray;

			// Token: 0x040004A7 RID: 1191
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> frictionArray;

			// Token: 0x040004A8 RID: 1192
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> staticFrictionArray;

			// Token: 0x040004A9 RID: 1193
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> collisionNormalArray;

			// Token: 0x040004AA RID: 1194
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<ExBitFlag16> colliderFlagArray;

			// Token: 0x040004AB RID: 1195
			[ReadOnly]
			public NativeArray<float3> colliderCenterArray;

			// Token: 0x040004AC RID: 1196
			[ReadOnly]
			public NativeArray<float3> colliderSizeArray;

			// Token: 0x040004AD RID: 1197
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderFramePositions;

			// Token: 0x040004AE RID: 1198
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderFrameRotations;

			// Token: 0x040004AF RID: 1199
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderFrameScales;

			// Token: 0x040004B0 RID: 1200
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldFramePositions;

			// Token: 0x040004B1 RID: 1201
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldFrameRotations;

			// Token: 0x040004B2 RID: 1202
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderNowPositions;

			// Token: 0x040004B3 RID: 1203
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderNowRotations;

			// Token: 0x040004B4 RID: 1204
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldPositions;

			// Token: 0x040004B5 RID: 1205
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldRotations;

			// Token: 0x040004B6 RID: 1206
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<ColliderManager.WorkData> colliderWorkDataArray;

			// Token: 0x040004B7 RID: 1207
			[ReadOnly]
			public NativeArray<int> colliderMainColliderIndices;

			// Token: 0x040004B8 RID: 1208
			[ReadOnly]
			public NativeArray<ushort> fixedArray;

			// Token: 0x040004B9 RID: 1209
			[ReadOnly]
			public NativeArray<uint> distanceIndexArray;

			// Token: 0x040004BA RID: 1210
			[ReadOnly]
			public NativeArray<ushort> distanceDataArray;

			// Token: 0x040004BB RID: 1211
			[ReadOnly]
			public NativeArray<float> distanceDistanceArray;

			// Token: 0x040004BC RID: 1212
			[ReadOnly]
			public NativeArray<ulong> bendingTrianglePairArray;

			// Token: 0x040004BD RID: 1213
			[ReadOnly]
			public NativeArray<float> bendingRestAngleOrVolumeArray;

			// Token: 0x040004BE RID: 1214
			[ReadOnly]
			public NativeArray<sbyte> bendingSignOrVolumeArray;

			// Token: 0x040004BF RID: 1215
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> stepBasicPositionBuffer;

			// Token: 0x040004C0 RID: 1216
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> stepBasicRotationBuffer;

			// Token: 0x040004C1 RID: 1217
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x040004C2 RID: 1218
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferB;

			// Token: 0x040004C3 RID: 1219
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> tempCountBuffer;

			// Token: 0x040004C4 RID: 1220
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> tempFloatBufferA;

			// Token: 0x040004C5 RID: 1221
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> tempRotationBufferA;

			// Token: 0x040004C6 RID: 1222
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> tempRotationBufferB;
		}

		// Token: 0x0200009C RID: 156
		[BurstCompile]
		private struct SplitPre_A_Job : IJobParallelFor
		{
			// Token: 0x060002B9 RID: 697 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.proxyCommonChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				VirtualMeshManager.SimulationPreProxyMeshUpdate(workerChunk, num3, ref ptr, in this.attributes, in this.localPositions, in this.localNormals, in this.localTangents, in this.boneWeights, in this.skinBoneTransformIndices, in this.skinBoneBindPoses, ref this.positions, ref this.rotations, in this.transformLocalToWorldMatrixArray);
			}

			// Token: 0x040004C7 RID: 1223
			public int workerCount;

			// Token: 0x040004C8 RID: 1224
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040004C9 RID: 1225
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040004CA RID: 1226
			[ReadOnly]
			public NativeArray<float4x4> transformLocalToWorldMatrixArray;

			// Token: 0x040004CB RID: 1227
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040004CC RID: 1228
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040004CD RID: 1229
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x040004CE RID: 1230
			[ReadOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x040004CF RID: 1231
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x040004D0 RID: 1232
			[ReadOnly]
			public NativeArray<int> skinBoneTransformIndices;

			// Token: 0x040004D1 RID: 1233
			[ReadOnly]
			public NativeArray<float4x4> skinBoneBindPoses;

			// Token: 0x040004D2 RID: 1234
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> positions;

			// Token: 0x040004D3 RID: 1235
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> rotations;
		}

		// Token: 0x0200009D RID: 157
		[BurstCompile]
		private struct SplitPre_B_Job : IJobParallelFor
		{
			// Token: 0x060002BA RID: 698 RVA: 0x0001C1BC File Offset: 0x0001A3BC
			public unsafe void Execute(int localIndex)
			{
				TeamManager.TeamData* unsafePtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafePtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafePtr2 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafePtr<InertiaConstraint.CenterData>();
				TeamWindData* unsafePtr3 = (TeamWindData*)this.teamWindArray.GetUnsafePtr<TeamWindData>();
				int num = this.batchSelfTeamList[localIndex];
				ref TeamManager.TeamData ptr = ref unsafePtr[num];
				ref InertiaConstraint.CenterData ptr2 = ref unsafePtr2[num];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr[num];
				ref TeamWindData ptr4 = ref unsafePtr3[num];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				TeamManager.SimulationCalcCenterAndInertiaAndWind(this.simulationDeltaTime, num, ref ptr, ref ptr2, ref ptr4, ref ptr3, in this.positions, in this.rotations, in this.vertexBindPoseRotations, in this.fixedArray, in this.transformPositionArray, in this.transformRotationArray, in this.transformScaleArray, this.windZoneCount, in this.windDataArray);
			}

			// Token: 0x040004D4 RID: 1236
			public float simulationDeltaTime;

			// Token: 0x040004D5 RID: 1237
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040004D6 RID: 1238
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040004D7 RID: 1239
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x040004D8 RID: 1240
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamWindData> teamWindArray;

			// Token: 0x040004D9 RID: 1241
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040004DA RID: 1242
			public int windZoneCount;

			// Token: 0x040004DB RID: 1243
			[ReadOnly]
			public NativeArray<WindManager.WindData> windDataArray;

			// Token: 0x040004DC RID: 1244
			[ReadOnly]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x040004DD RID: 1245
			[ReadOnly]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x040004DE RID: 1246
			[ReadOnly]
			public NativeArray<float3> transformScaleArray;

			// Token: 0x040004DF RID: 1247
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x040004E0 RID: 1248
			[ReadOnly]
			public NativeArray<quaternion> rotations;

			// Token: 0x040004E1 RID: 1249
			[ReadOnly]
			public NativeArray<quaternion> vertexBindPoseRotations;

			// Token: 0x040004E2 RID: 1250
			[ReadOnly]
			public NativeArray<ushort> fixedArray;
		}

		// Token: 0x0200009E RID: 158
		[BurstCompile]
		private struct SplitPre_C_Job : IJobParallelFor
		{
			// Token: 0x060002BB RID: 699 RVA: 0x0001C2A0 File Offset: 0x0001A4A0
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafeReadOnlyPtr3 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafeReadOnlyPtr<InertiaConstraint.CenterData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref InertiaConstraint.CenterData ptr2 = ref unsafeReadOnlyPtr3[num3];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr2[num3];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					SimulationManager.SimulationPreTeamUpdate(workerChunk, ref ptr, in ptr3, in ptr2, in this.positions, in this.rotations, in this.vertexDepths, ref this.nextPosArray, ref this.oldPosArray, ref this.oldRotArray, ref this.basePosArray, ref this.baseRotArray, ref this.oldPositionArray, ref this.oldRotationArray, ref this.velocityPosArray, ref this.dispPosArray, ref this.velocityArray, ref this.realVelocityArray, ref this.frictionArray, ref this.staticFrictionArray, ref this.collisionNormalArray);
				}
				if (ptr.colliderCount > 0)
				{
					DataChunk workerChunk2 = MathUtility.GetWorkerChunk(ptr.colliderChunk.dataLength, this.workerCount, num2);
					if (workerChunk2.IsValid)
					{
						ColliderManager.SimulationPreUpdate(workerChunk2, ref ptr, ref ptr2, ref this.colliderFlagArray, ref this.colliderCenterArray, ref this.colliderFramePositions, ref this.colliderFrameRotations, ref this.colliderFrameScales, ref this.colliderOldFramePositions, ref this.colliderOldFrameRotations, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations, ref this.colliderMainColliderIndices, ref this.transformPositionArray, ref this.transformRotationArray, ref this.transformScaleArray, ref this.transformLocalPositionArray, ref this.transformLocalRotationArray, ref this.transformLocalScaleArray);
					}
				}
			}

			// Token: 0x040004E3 RID: 1251
			public int workerCount;

			// Token: 0x040004E4 RID: 1252
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040004E5 RID: 1253
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040004E6 RID: 1254
			[ReadOnly]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x040004E7 RID: 1255
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040004E8 RID: 1256
			[ReadOnly]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x040004E9 RID: 1257
			[ReadOnly]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x040004EA RID: 1258
			[ReadOnly]
			public NativeArray<float3> transformScaleArray;

			// Token: 0x040004EB RID: 1259
			[ReadOnly]
			public NativeArray<float3> transformLocalPositionArray;

			// Token: 0x040004EC RID: 1260
			[ReadOnly]
			public NativeArray<quaternion> transformLocalRotationArray;

			// Token: 0x040004ED RID: 1261
			[ReadOnly]
			public NativeArray<float3> transformLocalScaleArray;

			// Token: 0x040004EE RID: 1262
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x040004EF RID: 1263
			[ReadOnly]
			public NativeArray<quaternion> rotations;

			// Token: 0x040004F0 RID: 1264
			[ReadOnly]
			public NativeArray<float> vertexDepths;

			// Token: 0x040004F1 RID: 1265
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040004F2 RID: 1266
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040004F3 RID: 1267
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> oldRotArray;

			// Token: 0x040004F4 RID: 1268
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> basePosArray;

			// Token: 0x040004F5 RID: 1269
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> baseRotArray;

			// Token: 0x040004F6 RID: 1270
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPositionArray;

			// Token: 0x040004F7 RID: 1271
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> oldRotationArray;

			// Token: 0x040004F8 RID: 1272
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x040004F9 RID: 1273
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> dispPosArray;

			// Token: 0x040004FA RID: 1274
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityArray;

			// Token: 0x040004FB RID: 1275
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> realVelocityArray;

			// Token: 0x040004FC RID: 1276
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> frictionArray;

			// Token: 0x040004FD RID: 1277
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> staticFrictionArray;

			// Token: 0x040004FE RID: 1278
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> collisionNormalArray;

			// Token: 0x040004FF RID: 1279
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<ExBitFlag16> colliderFlagArray;

			// Token: 0x04000500 RID: 1280
			[ReadOnly]
			public NativeArray<float3> colliderCenterArray;

			// Token: 0x04000501 RID: 1281
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderFramePositions;

			// Token: 0x04000502 RID: 1282
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderFrameRotations;

			// Token: 0x04000503 RID: 1283
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderFrameScales;

			// Token: 0x04000504 RID: 1284
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldFramePositions;

			// Token: 0x04000505 RID: 1285
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldFrameRotations;

			// Token: 0x04000506 RID: 1286
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderNowPositions;

			// Token: 0x04000507 RID: 1287
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderNowRotations;

			// Token: 0x04000508 RID: 1288
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldPositions;

			// Token: 0x04000509 RID: 1289
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldRotations;

			// Token: 0x0400050A RID: 1290
			[ReadOnly]
			public NativeArray<int> colliderMainColliderIndices;
		}

		// Token: 0x0200009F RID: 159
		[BurstCompile]
		private struct SplitStep_A_Job : IJobParallelFor
		{
			// Token: 0x060002BC RID: 700 RVA: 0x0001C470 File Offset: 0x0001A670
			public unsafe void Execute(int localIndex)
			{
				TeamManager.TeamData* unsafePtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafePtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafePtr2 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafePtr<InertiaConstraint.CenterData>();
				TeamWindData* unsafePtr3 = (TeamWindData*)this.teamWindArray.GetUnsafePtr<TeamWindData>();
				int num = this.batchSelfTeamList[localIndex];
				ref TeamManager.TeamData ptr = ref unsafePtr[num];
				ref InertiaConstraint.CenterData ptr2 = ref unsafePtr2[num];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr[num];
				ref TeamWindData ptr4 = ref unsafePtr3[num];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				TeamManager.SimulationStepTeamUpdate(this.updateIndex, this.simulationDeltaTime, num, ref ptr, ref ptr3, ref ptr2, ref ptr4);
				if (ptr.colliderCount > 0)
				{
					ColliderManager.SimulationStartStep(ref ptr, ref ptr2, ref this.colliderFlagArray, ref this.colliderSizeArray, ref this.colliderFramePositions, ref this.colliderFrameRotations, ref this.colliderFrameScales, ref this.colliderOldFramePositions, ref this.colliderOldFrameRotations, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations, ref this.colliderWorkDataArray);
				}
			}

			// Token: 0x0400050B RID: 1291
			public int updateIndex;

			// Token: 0x0400050C RID: 1292
			public float4 simulationPower;

			// Token: 0x0400050D RID: 1293
			public float simulationDeltaTime;

			// Token: 0x0400050E RID: 1294
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x0400050F RID: 1295
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000510 RID: 1296
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x04000511 RID: 1297
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamWindData> teamWindArray;

			// Token: 0x04000512 RID: 1298
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x04000513 RID: 1299
			[ReadOnly]
			public NativeArray<ExBitFlag16> colliderFlagArray;

			// Token: 0x04000514 RID: 1300
			[ReadOnly]
			public NativeArray<float3> colliderSizeArray;

			// Token: 0x04000515 RID: 1301
			[ReadOnly]
			public NativeArray<float3> colliderFramePositions;

			// Token: 0x04000516 RID: 1302
			[ReadOnly]
			public NativeArray<quaternion> colliderFrameRotations;

			// Token: 0x04000517 RID: 1303
			[ReadOnly]
			public NativeArray<float3> colliderFrameScales;

			// Token: 0x04000518 RID: 1304
			[ReadOnly]
			public NativeArray<float3> colliderOldFramePositions;

			// Token: 0x04000519 RID: 1305
			[ReadOnly]
			public NativeArray<quaternion> colliderOldFrameRotations;

			// Token: 0x0400051A RID: 1306
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderNowPositions;

			// Token: 0x0400051B RID: 1307
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderNowRotations;

			// Token: 0x0400051C RID: 1308
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldPositions;

			// Token: 0x0400051D RID: 1309
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldRotations;

			// Token: 0x0400051E RID: 1310
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<ColliderManager.WorkData> colliderWorkDataArray;
		}

		// Token: 0x020000A0 RID: 160
		[BurstCompile]
		private struct SplitStep_B_Job : IJobParallelFor
		{
			// Token: 0x060002BD RID: 701 RVA: 0x0001C590 File Offset: 0x0001A790
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafeReadOnlyPtr3 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafeReadOnlyPtr<InertiaConstraint.CenterData>();
				TeamWindData* unsafeReadOnlyPtr4 = (TeamWindData*)this.teamWindArray.GetUnsafeReadOnlyPtr<TeamWindData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref InertiaConstraint.CenterData ptr2 = ref unsafeReadOnlyPtr3[num3];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr2[num3];
				ref TeamWindData ptr4 = ref unsafeReadOnlyPtr4[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				SimulationManager.SimulationStepUpdateParticles(workerChunk, this.simulationPower, this.simulationDeltaTime, num3, ref ptr, ref ptr2, ref ptr3, ref ptr4, ref this.windDataArray, ref this.attributes, ref this.depthArray, ref this.positions, ref this.rotations, ref this.vertexRootIndices, ref this.nextPosArray, ref this.oldPosArray, ref this.basePosArray, ref this.baseRotArray, ref this.oldPositionArray, ref this.oldRotationArray, ref this.velocityPosArray, ref this.velocityArray, ref this.frictionArray, ref this.stepBasicPositionBuffer, ref this.stepBasicRotationBuffer);
			}

			// Token: 0x0400051F RID: 1311
			public int workerCount;

			// Token: 0x04000520 RID: 1312
			public int updateIndex;

			// Token: 0x04000521 RID: 1313
			public float4 simulationPower;

			// Token: 0x04000522 RID: 1314
			public float simulationDeltaTime;

			// Token: 0x04000523 RID: 1315
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000524 RID: 1316
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000525 RID: 1317
			[ReadOnly]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x04000526 RID: 1318
			[ReadOnly]
			public NativeArray<TeamWindData> teamWindArray;

			// Token: 0x04000527 RID: 1319
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x04000528 RID: 1320
			public int windZoneCount;

			// Token: 0x04000529 RID: 1321
			[ReadOnly]
			public NativeArray<WindManager.WindData> windDataArray;

			// Token: 0x0400052A RID: 1322
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400052B RID: 1323
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x0400052C RID: 1324
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x0400052D RID: 1325
			[ReadOnly]
			public NativeArray<quaternion> rotations;

			// Token: 0x0400052E RID: 1326
			[ReadOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x0400052F RID: 1327
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x04000530 RID: 1328
			[ReadOnly]
			public NativeArray<float3> oldPosArray;

			// Token: 0x04000531 RID: 1329
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> basePosArray;

			// Token: 0x04000532 RID: 1330
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> baseRotArray;

			// Token: 0x04000533 RID: 1331
			[ReadOnly]
			public NativeArray<float3> oldPositionArray;

			// Token: 0x04000534 RID: 1332
			[ReadOnly]
			public NativeArray<quaternion> oldRotationArray;

			// Token: 0x04000535 RID: 1333
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x04000536 RID: 1334
			[ReadOnly]
			public NativeArray<float3> velocityArray;

			// Token: 0x04000537 RID: 1335
			[ReadOnly]
			public NativeArray<float> frictionArray;

			// Token: 0x04000538 RID: 1336
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> stepBasicPositionBuffer;

			// Token: 0x04000539 RID: 1337
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> stepBasicRotationBuffer;
		}

		// Token: 0x020000A1 RID: 161
		[BurstCompile]
		private struct SplitStep_C_Job : IJobParallelFor
		{
			// Token: 0x060002BE RID: 702 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.baseLineChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				SimulationManager.SimulationStepUpdateBaseLinePose(workerChunk, ref ptr, ref this.attributes, ref this.vertexParentIndices, ref this.baseLineStartDataIndices, ref this.baseLineDataCounts, ref this.baseLineData, ref this.vertexLocalPositions, ref this.vertexLocalRotations, ref this.basePosArray, ref this.baseRotArray, ref this.stepBasicPositionBuffer, ref this.stepBasicRotationBuffer);
			}

			// Token: 0x0400053A RID: 1338
			public int workerCount;

			// Token: 0x0400053B RID: 1339
			public int updateIndex;

			// Token: 0x0400053C RID: 1340
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x0400053D RID: 1341
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x0400053E RID: 1342
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400053F RID: 1343
			[ReadOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x04000540 RID: 1344
			[ReadOnly]
			public NativeArray<int> vertexParentIndices;

			// Token: 0x04000541 RID: 1345
			[ReadOnly]
			public NativeArray<ushort> baseLineStartDataIndices;

			// Token: 0x04000542 RID: 1346
			[ReadOnly]
			public NativeArray<ushort> baseLineDataCounts;

			// Token: 0x04000543 RID: 1347
			[ReadOnly]
			public NativeArray<ushort> baseLineData;

			// Token: 0x04000544 RID: 1348
			[ReadOnly]
			public NativeArray<float3> vertexLocalPositions;

			// Token: 0x04000545 RID: 1349
			[ReadOnly]
			public NativeArray<quaternion> vertexLocalRotations;

			// Token: 0x04000546 RID: 1350
			[ReadOnly]
			public NativeArray<float3> basePosArray;

			// Token: 0x04000547 RID: 1351
			[ReadOnly]
			public NativeArray<quaternion> baseRotArray;

			// Token: 0x04000548 RID: 1352
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> stepBasicPositionBuffer;

			// Token: 0x04000549 RID: 1353
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> stepBasicRotationBuffer;
		}

		// Token: 0x020000A2 RID: 162
		[BurstCompile]
		private struct SplitStep_D_Job : IJobParallelFor
		{
			// Token: 0x060002BF RID: 703 RVA: 0x0001C7CC File Offset: 0x0001A9CC
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafeReadOnlyPtr3 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafeReadOnlyPtr<InertiaConstraint.CenterData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref InertiaConstraint.CenterData ptr2 = ref unsafeReadOnlyPtr3[num3];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				TetherConstraint.SolverConstraint(workerChunk, ref ptr, ref ptr3, ref ptr2, ref this.attributes, ref this.depthArray, ref this.vertexRootIndices, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.stepBasicPositionBuffer);
				DistanceConstraint.SolverConstraint(workerChunk, this.simulationPower, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.basePosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.distanceIndexArray, ref this.distanceDataArray, ref this.distanceDistanceArray);
			}

			// Token: 0x0400054A RID: 1354
			public int workerCount;

			// Token: 0x0400054B RID: 1355
			public int updateIndex;

			// Token: 0x0400054C RID: 1356
			public float4 simulationPower;

			// Token: 0x0400054D RID: 1357
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x0400054E RID: 1358
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x0400054F RID: 1359
			[ReadOnly]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x04000550 RID: 1360
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x04000551 RID: 1361
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000552 RID: 1362
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x04000553 RID: 1363
			[ReadOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x04000554 RID: 1364
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x04000555 RID: 1365
			[ReadOnly]
			public NativeArray<float3> basePosArray;

			// Token: 0x04000556 RID: 1366
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x04000557 RID: 1367
			[ReadOnly]
			public NativeArray<float> frictionArray;

			// Token: 0x04000558 RID: 1368
			[ReadOnly]
			public NativeArray<uint> distanceIndexArray;

			// Token: 0x04000559 RID: 1369
			[ReadOnly]
			public NativeArray<ushort> distanceDataArray;

			// Token: 0x0400055A RID: 1370
			[ReadOnly]
			public NativeArray<float> distanceDistanceArray;

			// Token: 0x0400055B RID: 1371
			[ReadOnly]
			public NativeArray<float3> stepBasicPositionBuffer;
		}

		// Token: 0x020000A3 RID: 163
		[BurstCompile]
		private struct SplitStep_Angle_Job : IJobParallelFor
		{
			// Token: 0x060002C0 RID: 704 RVA: 0x0001C910 File Offset: 0x0001AB10
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.baseLineChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				AngleConstraint.SolverConstraint(workerChunk, in this.simulationPower, ref ptr, ref ptr2, ref this.attributes, ref this.depthArray, ref this.vertexParentIndices, ref this.baseLineStartDataIndices, ref this.baseLineDataCounts, ref this.baseLineData, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.stepBasicPositionBuffer, ref this.stepBasicRotationBuffer, ref this.tempFloatBufferA, ref this.tempVectorBufferA, ref this.tempRotationBufferA, ref this.tempRotationBufferB, ref this.tempVectorBufferB);
			}

			// Token: 0x0400055C RID: 1372
			public int workerCount;

			// Token: 0x0400055D RID: 1373
			public int updateIndex;

			// Token: 0x0400055E RID: 1374
			public float4 simulationPower;

			// Token: 0x0400055F RID: 1375
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000560 RID: 1376
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000561 RID: 1377
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x04000562 RID: 1378
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000563 RID: 1379
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x04000564 RID: 1380
			[ReadOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x04000565 RID: 1381
			[ReadOnly]
			public NativeArray<int> vertexParentIndices;

			// Token: 0x04000566 RID: 1382
			[ReadOnly]
			public NativeArray<ushort> baseLineStartDataIndices;

			// Token: 0x04000567 RID: 1383
			[ReadOnly]
			public NativeArray<ushort> baseLineDataCounts;

			// Token: 0x04000568 RID: 1384
			[ReadOnly]
			public NativeArray<ushort> baseLineData;

			// Token: 0x04000569 RID: 1385
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x0400056A RID: 1386
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x0400056B RID: 1387
			[ReadOnly]
			public NativeArray<float> frictionArray;

			// Token: 0x0400056C RID: 1388
			[ReadOnly]
			public NativeArray<uint> distanceIndexArray;

			// Token: 0x0400056D RID: 1389
			[ReadOnly]
			public NativeArray<ushort> distanceDataArray;

			// Token: 0x0400056E RID: 1390
			[ReadOnly]
			public NativeArray<float> distanceDistanceArray;

			// Token: 0x0400056F RID: 1391
			[ReadOnly]
			public NativeArray<float3> stepBasicPositionBuffer;

			// Token: 0x04000570 RID: 1392
			[ReadOnly]
			public NativeArray<quaternion> stepBasicRotationBuffer;

			// Token: 0x04000571 RID: 1393
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x04000572 RID: 1394
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferB;

			// Token: 0x04000573 RID: 1395
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> tempFloatBufferA;

			// Token: 0x04000574 RID: 1396
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> tempRotationBufferA;

			// Token: 0x04000575 RID: 1397
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> tempRotationBufferB;
		}

		// Token: 0x020000A4 RID: 164
		[BurstCompile]
		private struct SplitStep_Triangle_Job : IJobParallelFor
		{
			// Token: 0x060002C1 RID: 705 RVA: 0x0001CA2C File Offset: 0x0001AC2C
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.bendingPairChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				TriangleBendingConstraint.SolverConstraint(workerChunk, in this.simulationPower, ref ptr, ref ptr2, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.frictionArray, ref this.bendingTrianglePairArray, ref this.bendingRestAngleOrVolumeArray, ref this.bendingSignOrVolumeArray, ref this.tempVectorBufferA, ref this.tempCountBuffer);
			}

			// Token: 0x04000576 RID: 1398
			public int workerCount;

			// Token: 0x04000577 RID: 1399
			public int updateIndex;

			// Token: 0x04000578 RID: 1400
			public float4 simulationPower;

			// Token: 0x04000579 RID: 1401
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x0400057A RID: 1402
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x0400057B RID: 1403
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x0400057C RID: 1404
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400057D RID: 1405
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x0400057E RID: 1406
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x0400057F RID: 1407
			[ReadOnly]
			public NativeArray<float> frictionArray;

			// Token: 0x04000580 RID: 1408
			[ReadOnly]
			public NativeArray<ulong> bendingTrianglePairArray;

			// Token: 0x04000581 RID: 1409
			[ReadOnly]
			public NativeArray<float> bendingRestAngleOrVolumeArray;

			// Token: 0x04000582 RID: 1410
			[ReadOnly]
			public NativeArray<sbyte> bendingSignOrVolumeArray;

			// Token: 0x04000583 RID: 1411
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x04000584 RID: 1412
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> tempCountBuffer;
		}

		// Token: 0x020000A5 RID: 165
		[BurstCompile]
		private struct SplitStep_E_Job : IJobParallelFor
		{
			// Token: 0x060002C2 RID: 706 RVA: 0x0001CB1C File Offset: 0x0001AD1C
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				TriangleBendingConstraint.SumConstraint(workerChunk, ref ptr, ref ptr2, ref this.attributes, ref this.nextPosArray, ref this.tempVectorBufferA, ref this.tempCountBuffer);
				if (ptr.UseColliderCount > 0 && ptr2.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Point)
				{
					ColliderCollisionConstraint.SolverPointConstraint(workerChunk, ref ptr, ref ptr2, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.frictionArray, ref this.collisionNormalArray, ref this.velocityPosArray, ref this.basePosArray, ref this.colliderFlagArray, ref this.colliderWorkDataArray);
				}
			}

			// Token: 0x04000585 RID: 1413
			public int workerCount;

			// Token: 0x04000586 RID: 1414
			public int updateIndex;

			// Token: 0x04000587 RID: 1415
			public float4 simulationPower;

			// Token: 0x04000588 RID: 1416
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000589 RID: 1417
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x0400058A RID: 1418
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x0400058B RID: 1419
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400058C RID: 1420
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x0400058D RID: 1421
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x0400058E RID: 1422
			[ReadOnly]
			public NativeArray<float3> basePosArray;

			// Token: 0x0400058F RID: 1423
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x04000590 RID: 1424
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> frictionArray;

			// Token: 0x04000591 RID: 1425
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> collisionNormalArray;

			// Token: 0x04000592 RID: 1426
			[ReadOnly]
			public NativeArray<ExBitFlag16> colliderFlagArray;

			// Token: 0x04000593 RID: 1427
			[ReadOnly]
			public NativeArray<ColliderManager.WorkData> colliderWorkDataArray;

			// Token: 0x04000594 RID: 1428
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x04000595 RID: 1429
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> tempCountBuffer;
		}

		// Token: 0x020000A6 RID: 166
		[BurstCompile]
		private struct SplitStep_Edge_Job : IJobParallelFor
		{
			// Token: 0x060002C3 RID: 707 RVA: 0x0001CC44 File Offset: 0x0001AE44
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				if (ptr.UseColliderCount > 0 && ptr2.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Edge)
				{
					DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.proxyEdgeChunk.dataLength, this.workerCount, num2);
					if (!workerChunk.IsValid)
					{
						return;
					}
					ColliderCollisionConstraint.SolverEdgeConstraint(workerChunk, ref ptr, ref ptr2, ref this.attributes, ref this.depthArray, ref this.edges, ref this.nextPosArray, ref this.colliderFlagArray, ref this.colliderWorkDataArray, ref this.tempVectorBufferA, ref this.tempVectorBufferB, ref this.tempCountBuffer, ref this.tempFloatBufferA);
				}
			}

			// Token: 0x04000596 RID: 1430
			public int workerCount;

			// Token: 0x04000597 RID: 1431
			public int updateIndex;

			// Token: 0x04000598 RID: 1432
			public float4 simulationPower;

			// Token: 0x04000599 RID: 1433
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x0400059A RID: 1434
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x0400059B RID: 1435
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x0400059C RID: 1436
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400059D RID: 1437
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x0400059E RID: 1438
			[ReadOnly]
			public NativeArray<int2> edges;

			// Token: 0x0400059F RID: 1439
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040005A0 RID: 1440
			[ReadOnly]
			public NativeArray<ExBitFlag16> colliderFlagArray;

			// Token: 0x040005A1 RID: 1441
			[ReadOnly]
			public NativeArray<ColliderManager.WorkData> colliderWorkDataArray;

			// Token: 0x040005A2 RID: 1442
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x040005A3 RID: 1443
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferB;

			// Token: 0x040005A4 RID: 1444
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> tempCountBuffer;

			// Token: 0x040005A5 RID: 1445
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> tempFloatBufferA;
		}

		// Token: 0x020000A7 RID: 167
		[BurstCompile]
		private struct SplitStep_F_Self_Job : IJobParallelFor
		{
			// Token: 0x060002C4 RID: 708 RVA: 0x0001CD50 File Offset: 0x0001AF50
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				if (ptr.UseColliderCount > 0 && ptr2.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Edge)
				{
					ColliderCollisionConstraint.SumEdgeConstraint(workerChunk, ref ptr, ref ptr2, ref this.nextPosArray, ref this.frictionArray, ref this.collisionNormalArray, ref this.tempVectorBufferA, ref this.tempVectorBufferB, ref this.tempCountBuffer, ref this.tempFloatBufferA);
				}
				DistanceConstraint.SolverConstraint(workerChunk, this.simulationPower, ref ptr, ref ptr2, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.basePosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.distanceIndexArray, ref this.distanceDataArray, ref this.distanceDistanceArray);
				MotionConstraint.SolverConstraint(workerChunk, ref ptr, ref ptr2, ref this.attributes, ref this.depthArray, ref this.basePosArray, ref this.baseRotArray, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.collisionNormalArray);
			}

			// Token: 0x040005A6 RID: 1446
			public int workerCount;

			// Token: 0x040005A7 RID: 1447
			public int updateIndex;

			// Token: 0x040005A8 RID: 1448
			public float4 simulationPower;

			// Token: 0x040005A9 RID: 1449
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040005AA RID: 1450
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040005AB RID: 1451
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040005AC RID: 1452
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040005AD RID: 1453
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x040005AE RID: 1454
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040005AF RID: 1455
			[ReadOnly]
			public NativeArray<float3> basePosArray;

			// Token: 0x040005B0 RID: 1456
			[ReadOnly]
			public NativeArray<quaternion> baseRotArray;

			// Token: 0x040005B1 RID: 1457
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x040005B2 RID: 1458
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> frictionArray;

			// Token: 0x040005B3 RID: 1459
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> collisionNormalArray;

			// Token: 0x040005B4 RID: 1460
			[ReadOnly]
			public NativeArray<uint> distanceIndexArray;

			// Token: 0x040005B5 RID: 1461
			[ReadOnly]
			public NativeArray<ushort> distanceDataArray;

			// Token: 0x040005B6 RID: 1462
			[ReadOnly]
			public NativeArray<float> distanceDistanceArray;

			// Token: 0x040005B7 RID: 1463
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x040005B8 RID: 1464
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferB;

			// Token: 0x040005B9 RID: 1465
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> tempCountBuffer;

			// Token: 0x040005BA RID: 1466
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> tempFloatBufferA;
		}

		// Token: 0x020000A8 RID: 168
		[BurstCompile]
		private struct SplitStep_G_Self_Job : IJobParallelFor
		{
			// Token: 0x060002C5 RID: 709 RVA: 0x0001CECC File Offset: 0x0001B0CC
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafeReadOnlyPtr3 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafeReadOnlyPtr<InertiaConstraint.CenterData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref InertiaConstraint.CenterData ptr2 = ref unsafeReadOnlyPtr3[num3];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					SimulationManager.SimulationStepPostTeam(workerChunk, this.simulationDeltaTime, num3, ref ptr, ref ptr2, ref ptr3, ref this.attributes, ref this.depthArray, ref this.oldPosArray, ref this.velocityArray, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.staticFrictionArray, ref this.collisionNormalArray, ref this.realVelocityArray);
				}
				DataChunk workerChunk2 = MathUtility.GetWorkerChunk(ptr.colliderChunk.dataLength, this.workerCount, num2);
				if (workerChunk2.IsValid)
				{
					ColliderManager.SimulationEndStep(workerChunk2, ref ptr, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations);
				}
			}

			// Token: 0x040005BB RID: 1467
			public int workerCount;

			// Token: 0x040005BC RID: 1468
			public int updateIndex;

			// Token: 0x040005BD RID: 1469
			public float simulationDeltaTime;

			// Token: 0x040005BE RID: 1470
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040005BF RID: 1471
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040005C0 RID: 1472
			[ReadOnly]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x040005C1 RID: 1473
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040005C2 RID: 1474
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040005C3 RID: 1475
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x040005C4 RID: 1476
			[ReadOnly]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040005C5 RID: 1477
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040005C6 RID: 1478
			[ReadOnly]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x040005C7 RID: 1479
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityArray;

			// Token: 0x040005C8 RID: 1480
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> realVelocityArray;

			// Token: 0x040005C9 RID: 1481
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> frictionArray;

			// Token: 0x040005CA RID: 1482
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> staticFrictionArray;

			// Token: 0x040005CB RID: 1483
			[ReadOnly]
			public NativeArray<float3> collisionNormalArray;

			// Token: 0x040005CC RID: 1484
			[ReadOnly]
			public NativeArray<float3> colliderNowPositions;

			// Token: 0x040005CD RID: 1485
			[ReadOnly]
			public NativeArray<quaternion> colliderNowRotations;

			// Token: 0x040005CE RID: 1486
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldPositions;

			// Token: 0x040005CF RID: 1487
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldRotations;
		}

		// Token: 0x020000A9 RID: 169
		[BurstCompile]
		private struct SplitStep_FG_NoSelf_Job : IJobParallelFor
		{
			// Token: 0x060002C6 RID: 710 RVA: 0x0001D028 File Offset: 0x0001B228
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				InertiaConstraint.CenterData* unsafeReadOnlyPtr3 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafeReadOnlyPtr<InertiaConstraint.CenterData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref InertiaConstraint.CenterData ptr2 = ref unsafeReadOnlyPtr3[num3];
				ref ClothParameters ptr3 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					if (ptr.UseColliderCount > 0 && ptr3.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Edge)
					{
						ColliderCollisionConstraint.SumEdgeConstraint(workerChunk, ref ptr, ref ptr3, ref this.nextPosArray, ref this.frictionArray, ref this.collisionNormalArray, ref this.tempVectorBufferA, ref this.tempVectorBufferB, ref this.tempCountBuffer, ref this.tempFloatBufferA);
					}
					DistanceConstraint.SolverConstraint(workerChunk, this.simulationPower, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.nextPosArray, ref this.basePosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.distanceIndexArray, ref this.distanceDataArray, ref this.distanceDistanceArray);
					MotionConstraint.SolverConstraint(workerChunk, ref ptr, ref ptr3, ref this.attributes, ref this.depthArray, ref this.basePosArray, ref this.baseRotArray, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.collisionNormalArray);
					SimulationManager.SimulationStepPostTeam(workerChunk, this.simulationDeltaTime, num3, ref ptr, ref ptr2, ref ptr3, ref this.attributes, ref this.depthArray, ref this.oldPosArray, ref this.velocityArray, ref this.nextPosArray, ref this.velocityPosArray, ref this.frictionArray, ref this.staticFrictionArray, ref this.collisionNormalArray, ref this.realVelocityArray);
				}
				DataChunk workerChunk2 = MathUtility.GetWorkerChunk(ptr.colliderChunk.dataLength, this.workerCount, num2);
				if (workerChunk2.IsValid)
				{
					ColliderManager.SimulationEndStep(workerChunk2, ref ptr, ref this.colliderNowPositions, ref this.colliderNowRotations, ref this.colliderOldPositions, ref this.colliderOldRotations);
				}
			}

			// Token: 0x040005D0 RID: 1488
			public int workerCount;

			// Token: 0x040005D1 RID: 1489
			public int updateIndex;

			// Token: 0x040005D2 RID: 1490
			public float4 simulationPower;

			// Token: 0x040005D3 RID: 1491
			public float simulationDeltaTime;

			// Token: 0x040005D4 RID: 1492
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040005D5 RID: 1493
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040005D6 RID: 1494
			[ReadOnly]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x040005D7 RID: 1495
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040005D8 RID: 1496
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040005D9 RID: 1497
			[ReadOnly]
			public NativeArray<float> depthArray;

			// Token: 0x040005DA RID: 1498
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040005DB RID: 1499
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040005DC RID: 1500
			[ReadOnly]
			public NativeArray<float3> basePosArray;

			// Token: 0x040005DD RID: 1501
			[ReadOnly]
			public NativeArray<quaternion> baseRotArray;

			// Token: 0x040005DE RID: 1502
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityPosArray;

			// Token: 0x040005DF RID: 1503
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> velocityArray;

			// Token: 0x040005E0 RID: 1504
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> realVelocityArray;

			// Token: 0x040005E1 RID: 1505
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> frictionArray;

			// Token: 0x040005E2 RID: 1506
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> staticFrictionArray;

			// Token: 0x040005E3 RID: 1507
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> collisionNormalArray;

			// Token: 0x040005E4 RID: 1508
			[ReadOnly]
			public NativeArray<float3> colliderNowPositions;

			// Token: 0x040005E5 RID: 1509
			[ReadOnly]
			public NativeArray<quaternion> colliderNowRotations;

			// Token: 0x040005E6 RID: 1510
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldPositions;

			// Token: 0x040005E7 RID: 1511
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldRotations;

			// Token: 0x040005E8 RID: 1512
			[ReadOnly]
			public NativeArray<uint> distanceIndexArray;

			// Token: 0x040005E9 RID: 1513
			[ReadOnly]
			public NativeArray<ushort> distanceDataArray;

			// Token: 0x040005EA RID: 1514
			[ReadOnly]
			public NativeArray<float> distanceDistanceArray;

			// Token: 0x040005EB RID: 1515
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x040005EC RID: 1516
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> tempVectorBufferB;

			// Token: 0x040005ED RID: 1517
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> tempCountBuffer;

			// Token: 0x040005EE RID: 1518
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float> tempFloatBufferA;
		}

		// Token: 0x020000AA RID: 170
		[BurstCompile]
		private struct SplitPost_DisplayPos_Job : IJobParallelFor
		{
			// Token: 0x060002C7 RID: 711 RVA: 0x0001D254 File Offset: 0x0001B454
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.particleChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					SimulationManager.SimulationCalcDisplayPosition(workerChunk, this.simulationDeltaTime, ref ptr, ref this.oldPosArray, ref this.realVelocityArray, ref this.oldPositionArray, ref this.oldRotationArray, ref this.dispPosArray, ref this.attributes, ref this.positions, ref this.rotations, ref this.vertexRootIndices);
				}
			}

			// Token: 0x040005EF RID: 1519
			public int workerCount;

			// Token: 0x040005F0 RID: 1520
			public float simulationDeltaTime;

			// Token: 0x040005F1 RID: 1521
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040005F2 RID: 1522
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040005F3 RID: 1523
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040005F4 RID: 1524
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> positions;

			// Token: 0x040005F5 RID: 1525
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> rotations;

			// Token: 0x040005F6 RID: 1526
			[ReadOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x040005F7 RID: 1527
			[ReadOnly]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040005F8 RID: 1528
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> oldPositionArray;

			// Token: 0x040005F9 RID: 1529
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> oldRotationArray;

			// Token: 0x040005FA RID: 1530
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> dispPosArray;

			// Token: 0x040005FB RID: 1531
			[ReadOnly]
			public NativeArray<float3> realVelocityArray;
		}

		// Token: 0x020000AB RID: 171
		[BurstCompile]
		private struct SplitPost_CalcProxy_Job : IJobParallelFor
		{
			// Token: 0x060002C8 RID: 712 RVA: 0x0001D318 File Offset: 0x0001B518
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.baseLineChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					VirtualMeshManager.SimulationPostProxyMeshUpdateLine(workerChunk, ref ptr, ref ptr2, ref this.attributes, ref this.positions, ref this.rotations, ref this.vertexLocalPositions, ref this.vertexLocalRotations, ref this.vertexChildIndexArray, ref this.vertexChildDataArray, ref this.baseLineFlags, ref this.baseLineStartDataIndices, ref this.baseLineDataCounts, ref this.baseLineData);
				}
			}

			// Token: 0x040005FC RID: 1532
			public int workerCount;

			// Token: 0x040005FD RID: 1533
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040005FE RID: 1534
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040005FF RID: 1535
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x04000600 RID: 1536
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000601 RID: 1537
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x04000602 RID: 1538
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> rotations;

			// Token: 0x04000603 RID: 1539
			[ReadOnly]
			public NativeArray<ushort> baseLineStartDataIndices;

			// Token: 0x04000604 RID: 1540
			[ReadOnly]
			public NativeArray<ushort> baseLineDataCounts;

			// Token: 0x04000605 RID: 1541
			[ReadOnly]
			public NativeArray<ushort> baseLineData;

			// Token: 0x04000606 RID: 1542
			[ReadOnly]
			public NativeArray<float3> vertexLocalPositions;

			// Token: 0x04000607 RID: 1543
			[ReadOnly]
			public NativeArray<quaternion> vertexLocalRotations;

			// Token: 0x04000608 RID: 1544
			[ReadOnly]
			public NativeArray<uint> vertexChildIndexArray;

			// Token: 0x04000609 RID: 1545
			[ReadOnly]
			public NativeArray<ushort> vertexChildDataArray;

			// Token: 0x0400060A RID: 1546
			[ReadOnly]
			public NativeArray<ExBitFlag8> baseLineFlags;
		}

		// Token: 0x020000AC RID: 172
		[BurstCompile]
		private struct SplitPost_CalcProxyTriangle_Job : IJobParallelFor
		{
			// Token: 0x060002C9 RID: 713 RVA: 0x0001D400 File Offset: 0x0001B600
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.proxyTriangleChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					VirtualMeshManager.SimulationPostProxyMeshUpdateTriangle(workerChunk, ref ptr, ref this.positions, ref this.triangles, ref this.triangleNormals, ref this.triangleTangents, ref this.uvs);
				}
			}

			// Token: 0x0400060B RID: 1547
			public int workerCount;

			// Token: 0x0400060C RID: 1548
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x0400060D RID: 1549
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x0400060E RID: 1550
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x0400060F RID: 1551
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000610 RID: 1552
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> triangleNormals;

			// Token: 0x04000611 RID: 1553
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> triangleTangents;

			// Token: 0x04000612 RID: 1554
			[ReadOnly]
			public NativeArray<float2> uvs;
		}

		// Token: 0x020000AD RID: 173
		[BurstCompile]
		private struct SplitPost_SumProxyTriangleAndTransform_Job : IJobParallelFor
		{
			// Token: 0x060002CA RID: 714 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(ptr.proxyCommonChunk.dataLength, this.workerCount, num2);
				if (workerChunk.IsValid)
				{
					VirtualMeshManager.SimulationPostProxyMeshUpdateTriangleSum(workerChunk, ref ptr, ref this.rotations, ref this.triangleNormals, ref this.triangleTangents, ref this.vertexToTriangles, ref this.normalAdjustmentRotations);
					VirtualMeshManager.SimulationPostProxyMeshUpdateWorldTransform(workerChunk, ref ptr, ref this.positions, ref this.rotations, ref this.vertexToTransformRotations, ref this.transformPositionArray, ref this.transformRotationArray);
				}
			}

			// Token: 0x04000613 RID: 1555
			public int workerCount;

			// Token: 0x04000614 RID: 1556
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000615 RID: 1557
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000616 RID: 1558
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x04000617 RID: 1559
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x04000618 RID: 1560
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x04000619 RID: 1561
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> rotations;

			// Token: 0x0400061A RID: 1562
			[ReadOnly]
			public NativeArray<float3> triangleNormals;

			// Token: 0x0400061B RID: 1563
			[ReadOnly]
			public NativeArray<float3> triangleTangents;

			// Token: 0x0400061C RID: 1564
			[ReadOnly]
			public NativeArray<FixedList32Bytes<uint>> vertexToTriangles;

			// Token: 0x0400061D RID: 1565
			[ReadOnly]
			public NativeArray<quaternion> normalAdjustmentRotations;

			// Token: 0x0400061E RID: 1566
			[ReadOnly]
			public NativeArray<quaternion> vertexToTransformRotations;
		}

		// Token: 0x020000AE RID: 174
		[BurstCompile]
		private struct SplitPost_TeamCollider_Job : IJobParallelFor
		{
			// Token: 0x060002CB RID: 715 RVA: 0x0001D570 File Offset: 0x0001B770
			public unsafe void Execute(int localIndex)
			{
				TeamManager.TeamData* unsafePtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafePtr<TeamManager.TeamData>();
				InertiaConstraint.CenterData* unsafePtr2 = (InertiaConstraint.CenterData*)this.centerDataArray.GetUnsafePtr<InertiaConstraint.CenterData>();
				int num = this.batchSelfTeamList[localIndex];
				ref TeamManager.TeamData ptr = ref unsafePtr[num];
				ref InertiaConstraint.CenterData ptr2 = ref unsafePtr2[num];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				VirtualMeshManager.SimulationPostProxyMeshUpdateLocalTransform(ref ptr, ref this.attributes, ref this.vertexParentIndices, ref this.transformPositionArray, ref this.transformRotationArray, ref this.transformScaleArray, ref this.transformLocalPositionArray, ref this.transformLocalRotationArray);
				ColliderManager.SimulationPostUpdate(ref ptr, ref this.colliderFramePositions, ref this.colliderFrameRotations, ref this.colliderOldFramePositions, ref this.colliderOldFrameRotations);
				TeamManager.SimulationPostTeamUpdate(ref ptr, ref ptr2);
			}

			// Token: 0x0400061F RID: 1567
			public float simulationDeltaTime;

			// Token: 0x04000620 RID: 1568
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000621 RID: 1569
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000622 RID: 1570
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x04000623 RID: 1571
			[ReadOnly]
			public NativeArray<float3> colliderFramePositions;

			// Token: 0x04000624 RID: 1572
			[ReadOnly]
			public NativeArray<quaternion> colliderFrameRotations;

			// Token: 0x04000625 RID: 1573
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> colliderOldFramePositions;

			// Token: 0x04000626 RID: 1574
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> colliderOldFrameRotations;

			// Token: 0x04000627 RID: 1575
			[ReadOnly]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x04000628 RID: 1576
			[ReadOnly]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x04000629 RID: 1577
			[ReadOnly]
			public NativeArray<float3> transformScaleArray;

			// Token: 0x0400062A RID: 1578
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float3> transformLocalPositionArray;

			// Token: 0x0400062B RID: 1579
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<quaternion> transformLocalRotationArray;

			// Token: 0x0400062C RID: 1580
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400062D RID: 1581
			[ReadOnly]
			public NativeArray<int> vertexParentIndices;
		}
	}
}
