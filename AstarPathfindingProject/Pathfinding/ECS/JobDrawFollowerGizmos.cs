using System;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.PID;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x02000326 RID: 806
	public struct JobDrawFollowerGizmos : IJobChunk
	{
		// Token: 0x060013D6 RID: 5078 RVA: 0x00075990 File Offset: 0x00073B90
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			if (!this.scratchBuffer1.IsCreated)
			{
				this.scratchBuffer1 = new NativeList<float3>(32, Allocator.Temp);
			}
			if (!this.scratchBuffer2.IsCreated)
			{
				this.scratchBuffer2 = new NativeArray<int>(32, Allocator.Temp, NativeArrayOptions.ClearMemory);
			}
			LocalTransform* unsafeReadOnlyPtr = (LocalTransform*)chunk.GetNativeArray<LocalTransform>(ref this.LocalTransformTypeHandleRO).GetUnsafeReadOnlyPtr<LocalTransform>();
			AgentCylinderShape* unsafeReadOnlyPtr2 = (AgentCylinderShape*)chunk.GetNativeArray<AgentCylinderShape>(ref this.AgentCylinderShapeHandleRO).GetUnsafeReadOnlyPtr<AgentCylinderShape>();
			MovementSettings* unsafeReadOnlyPtr3 = (MovementSettings*)chunk.GetNativeArray<MovementSettings>(ref this.MovementSettingsHandleRO).GetUnsafeReadOnlyPtr<MovementSettings>();
			AgentMovementPlane* unsafeReadOnlyPtr4 = (AgentMovementPlane*)chunk.GetNativeArray<AgentMovementPlane>(ref this.AgentMovementPlaneHandleRO).GetUnsafeReadOnlyPtr<AgentMovementPlane>();
			ManagedComponentAccessor<ManagedState> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedState>(ref this.ManagedStateHandleRW, (EntityManager)this.entityManagerHandle.Target);
			MovementState* unsafeReadOnlyPtr5 = (MovementState*)chunk.GetNativeArray<MovementState>(ref this.MovementStateHandleRO).GetUnsafeReadOnlyPtr<MovementState>();
			ResolvedMovement* unsafeReadOnlyPtr6 = (ResolvedMovement*)chunk.GetNativeArray<ResolvedMovement>(ref this.ResolvedMovementHandleRO).GetUnsafeReadOnlyPtr<ResolvedMovement>();
			for (int i = 0; i < chunk.Count; i++)
			{
				this.Execute(ref unsafeReadOnlyPtr[i], ref unsafeReadOnlyPtr4[i], ref unsafeReadOnlyPtr2[i], managedComponentAccessor[i], ref unsafeReadOnlyPtr3[i], ref unsafeReadOnlyPtr5[i], ref unsafeReadOnlyPtr6[i]);
			}
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00075AD4 File Offset: 0x00073CD4
		public void Execute(ref LocalTransform transform, ref AgentMovementPlane movementPlane, ref AgentCylinderShape shape, ManagedState managedState, ref MovementSettings settings, ref MovementState movementState, ref ResolvedMovement resolvedMovement)
		{
			if ((settings.debugFlags & PIDMovement.DebugFlags.Funnel) != PIDMovement.DebugFlags.Nothing)
			{
				managedState.pathTracer.DrawFunnel(this.draw, movementPlane.value);
			}
			if ((settings.debugFlags & PIDMovement.DebugFlags.Rotation) != PIDMovement.DebugFlags.Nothing)
			{
				float num;
				float2 @float = movementPlane.value.ToPlane(transform.Position, out num);
				this.draw.PushMatrix(math.mul(new float4x4(movementPlane.value.rotation, float3.zero), float4x4.Translate(new float3(0f, num, 0f))));
				float num2 = movementPlane.value.ToPlane(transform.Rotation);
				float num3 = num2 - movementState.rotationOffset2;
				float num4 = num3 - movementState.rotationOffset;
				float targetRotation = resolvedMovement.targetRotation;
				float targetRotationHint = resolvedMovement.targetRotationHint;
				float3 float2;
				float3 float3;
				math.sincos(1.5707964f + new float3(num2, num3, num4), out float2, out float3);
				this.draw.xz.ArrowheadArc(@float, new float2(float3.x, float2.x), shape.radius * 1.1f, JobDrawFollowerGizmos.VisualRotationColor);
				this.draw.xz.ArrowheadArc(@float, new float2(float3.y, float2.y), shape.radius * 1.1f, JobDrawFollowerGizmos.UnsmoothedRotation);
				this.draw.xz.ArrowheadArc(@float, new float2(float3.z, float2.z), shape.radius * 1.1f, JobDrawFollowerGizmos.InternalRotation);
				float2 float4;
				float2 float5;
				math.sincos(1.5707964f + new float2(targetRotation, targetRotationHint), out float4, out float5);
				this.draw.xz.ArrowheadArc(@float, new float2(float5.x, float4.x), shape.radius * 1.2f, JobDrawFollowerGizmos.TargetInternalRotation);
				this.draw.xz.ArrowheadArc(@float, new float2(float5.y, float4.y), shape.radius * 1.2f, JobDrawFollowerGizmos.TargetInternalRotationHint);
				this.draw.PopMatrix();
			}
			if ((settings.debugFlags & PIDMovement.DebugFlags.Path) != PIDMovement.DebugFlags.Nothing && managedState.pathTracer.hasPath)
			{
				this.scratchBuffer1.Clear();
				managedState.pathTracer.GetNextCorners(this.scratchBuffer1, int.MaxValue, ref this.scratchBuffer2, Allocator.Temp, false);
				UnsafeSpan<float3> unsafeSpan = this.scratchBuffer1.AsUnsafeSpan<float3>();
				this.draw.PushColor(JobDrawFollowerGizmos.Path);
				DrawGizmosJobUtils.DrawPath(ref this.draw, ref unsafeSpan, ref shape);
				this.draw.PopColor();
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00075DB6 File Offset: 0x00073FB6
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E41 RID: 3649
		public CommandBuilder draw;

		// Token: 0x04000E42 RID: 3650
		public GCHandle entityManagerHandle;

		// Token: 0x04000E43 RID: 3651
		[ReadOnly]
		public ComponentTypeHandle<LocalTransform> LocalTransformTypeHandleRO;

		// Token: 0x04000E44 RID: 3652
		[ReadOnly]
		public ComponentTypeHandle<AgentCylinderShape> AgentCylinderShapeHandleRO;

		// Token: 0x04000E45 RID: 3653
		[ReadOnly]
		public ComponentTypeHandle<MovementSettings> MovementSettingsHandleRO;

		// Token: 0x04000E46 RID: 3654
		[ReadOnly]
		public ComponentTypeHandle<AgentMovementPlane> AgentMovementPlaneHandleRO;

		// Token: 0x04000E47 RID: 3655
		[NativeDisableContainerSafetyRestriction]
		public ComponentTypeHandle<ManagedState> ManagedStateHandleRW;

		// Token: 0x04000E48 RID: 3656
		[ReadOnly]
		public ComponentTypeHandle<MovementState> MovementStateHandleRO;

		// Token: 0x04000E49 RID: 3657
		[ReadOnly]
		public ComponentTypeHandle<ResolvedMovement> ResolvedMovementHandleRO;

		// Token: 0x04000E4A RID: 3658
		[NativeDisableContainerSafetyRestriction]
		public NativeList<float3> scratchBuffer1;

		// Token: 0x04000E4B RID: 3659
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> scratchBuffer2;

		// Token: 0x04000E4C RID: 3660
		public static readonly Color VisualRotationColor = Palette.Colorbrewer.Set1.Blue;

		// Token: 0x04000E4D RID: 3661
		public static readonly Color UnsmoothedRotation = Palette.Colorbrewer.Set1.Purple;

		// Token: 0x04000E4E RID: 3662
		public static readonly Color InternalRotation = Palette.Colorbrewer.Set1.Orange;

		// Token: 0x04000E4F RID: 3663
		public static readonly Color TargetInternalRotation = Palette.Colorbrewer.Set1.Yellow;

		// Token: 0x04000E50 RID: 3664
		public static readonly Color TargetInternalRotationHint = Palette.Colorbrewer.Set1.Pink;

		// Token: 0x04000E51 RID: 3665
		public static readonly Color Path = Palette.Colorbrewer.Set1.Orange;
	}
}
