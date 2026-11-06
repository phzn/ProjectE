using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x0200034B RID: 843
	[BurstCompile]
	internal static class JobRepairPathHelpers
	{
		// Token: 0x06001523 RID: 5411 RVA: 0x000795DE File Offset: 0x000777DE
		[BurstCompile]
		public static void UpdateReachedEndInfo(ref UnsafeSpan<float3> nextCorners, ref MovementState state, ref AgentMovementPlane movementPlane, ref LocalTransform transform, ref AgentCylinderShape shape, ref DestinationPoint destination, float stopDistance, ref JobRepairPathHelpers.PathTracerInfo pathTracer)
		{
			JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.Invoke(ref nextCorners, ref state, ref movementPlane, ref transform, ref shape, ref destination, stopDistance, ref pathTracer);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000795F4 File Offset: 0x000777F4
		public static void UpdateReachedOrientation(ref MovementState state, ref LocalTransform transform, ref AgentMovementPlane movementPlane, ref DestinationPoint destination)
		{
			state.reachedEndOfPathAndOrientation = state.reachedEndOfPath;
			state.reachedDestinationAndOrientation = state.reachedDestination;
			if (state.reachedEndOfPathAndOrientation || state.reachedDestinationAndOrientation)
			{
				bool flag = JobRepairPathHelpers.ReachedDesiredOrientation(ref transform, ref movementPlane, ref destination);
				state.reachedEndOfPathAndOrientation = state.reachedEndOfPathAndOrientation && flag;
				state.reachedDestinationAndOrientation = state.reachedDestinationAndOrientation && flag;
			}
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00079650 File Offset: 0x00077850
		private static bool ReachedDesiredOrientation(ref LocalTransform transform, ref AgentMovementPlane movementPlane, ref DestinationPoint destination)
		{
			float2 @float = math.normalizesafe(movementPlane.value.ToPlane(destination.facingDirection), default(float2));
			return math.all(@float == 0f) || math.dot(math.normalizesafe(movementPlane.value.ToPlane(transform.Forward()), default(float2)), @float) >= 0.9999f;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x000796C0 File Offset: 0x000778C0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void UpdateReachedEndInfo$BurstManaged(ref UnsafeSpan<float3> nextCorners, ref MovementState state, ref AgentMovementPlane movementPlane, ref LocalTransform transform, ref AgentCylinderShape shape, ref DestinationPoint destination, float stopDistance, ref JobRepairPathHelpers.PathTracerInfo pathTracer)
		{
			state.nextCorner = ((nextCorners.length > 1U) ? (*nextCorners[1]) : transform.Position);
			state.remainingDistanceToEndOfPart = PathTracer.RemainingDistanceLowerBound(in nextCorners, in pathTracer.endPointOfFirstPart, in movementPlane.value);
			float num;
			movementPlane.value.ToPlane(pathTracer.endPointOfFirstPart - transform.Position, out num);
			bool flag = num < shape.height && num > -0.5f * shape.height;
			float num2;
			movementPlane.value.ToPlane(destination.destination - transform.Position, out num2);
			bool flag2 = num2 < shape.height && num2 > -0.5f * shape.height;
			float num3 = math.length(movementPlane.value.ToPlane(destination.destination - state.endOfPath));
			state.reachedEndOfPart = !pathTracer.isStale && flag && state.remainingDistanceToEndOfPart <= shape.radius * 1.1f;
			state.reachedEndOfPath = !pathTracer.isStale && flag && pathTracer.partCount == 1 && state.remainingDistanceToEndOfPart <= stopDistance;
			state.reachedDestination = !pathTracer.isStale && flag2 && pathTracer.partCount == 1 && state.remainingDistanceToEndOfPart + num3 <= stopDistance + (state.reachedEndOfPath ? 0.01f : 0f);
			state.traversingLastPart = pathTracer.partCount == 1;
			state.hasValidEndPoint = pathTracer.hasValidEndPoint;
			JobRepairPathHelpers.UpdateReachedOrientation(ref state, ref transform, ref movementPlane, ref destination);
		}

		// Token: 0x0200034C RID: 844
		public struct PathTracerInfo
		{
			// Token: 0x04000ECA RID: 3786
			public float3 endPointOfFirstPart;

			// Token: 0x04000ECB RID: 3787
			public int partCount;

			// Token: 0x04000ECC RID: 3788
			[MarshalAs(UnmanagedType.U1)]
			public bool isStale;

			// Token: 0x04000ECD RID: 3789
			[MarshalAs(UnmanagedType.U1)]
			public bool hasValidEndPoint;
		}

		// Token: 0x0200034D RID: 845
		// (Invoke) Token: 0x06001528 RID: 5416
		internal delegate void UpdateReachedEndInfo_000013B7$PostfixBurstDelegate(ref UnsafeSpan<float3> nextCorners, ref MovementState state, ref AgentMovementPlane movementPlane, ref LocalTransform transform, ref AgentCylinderShape shape, ref DestinationPoint destination, float stopDistance, ref JobRepairPathHelpers.PathTracerInfo pathTracer);

		// Token: 0x0200034E RID: 846
		internal static class UpdateReachedEndInfo_000013B7$BurstDirectCall
		{
			// Token: 0x0600152B RID: 5419 RVA: 0x00079873 File Offset: 0x00077A73
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.Pointer == 0)
				{
					JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.DeferredCompilation, methodof(JobRepairPathHelpers.UpdateReachedEndInfo$BurstManaged(ref UnsafeSpan<float3>, ref MovementState, ref AgentMovementPlane, ref LocalTransform, ref AgentCylinderShape, ref DestinationPoint, float, ref JobRepairPathHelpers.PathTracerInfo)).MethodHandle, typeof(JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.Pointer;
			}

			// Token: 0x0600152C RID: 5420 RVA: 0x000798A0 File Offset: 0x00077AA0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600152D RID: 5421 RVA: 0x000798B8 File Offset: 0x00077AB8
			public static void Constructor()
			{
				JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(JobRepairPathHelpers.UpdateReachedEndInfo(ref UnsafeSpan<float3>, ref MovementState, ref AgentMovementPlane, ref LocalTransform, ref AgentCylinderShape, ref DestinationPoint, float, ref JobRepairPathHelpers.PathTracerInfo)).MethodHandle);
			}

			// Token: 0x0600152E RID: 5422 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x0600152F RID: 5423 RVA: 0x000798C9 File Offset: 0x00077AC9
			// Note: this type is marked as 'beforefieldinit'.
			static UpdateReachedEndInfo_000013B7$BurstDirectCall()
			{
				JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.Constructor();
			}

			// Token: 0x06001530 RID: 5424 RVA: 0x000798D0 File Offset: 0x00077AD0
			public static void Invoke(ref UnsafeSpan<float3> nextCorners, ref MovementState state, ref AgentMovementPlane movementPlane, ref LocalTransform transform, ref AgentCylinderShape shape, ref DestinationPoint destination, float stopDistance, ref JobRepairPathHelpers.PathTracerInfo pathTracer)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = JobRepairPathHelpers.UpdateReachedEndInfo_000013B7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Pathfinding.ECS.MovementState&,Pathfinding.ECS.AgentMovementPlane&,Unity.Transforms.LocalTransform&,Pathfinding.ECS.AgentCylinderShape&,Pathfinding.ECS.DestinationPoint&,System.Single,Pathfinding.ECS.JobRepairPathHelpers/PathTracerInfo&), ref nextCorners, ref state, ref movementPlane, ref transform, ref shape, ref destination, stopDistance, ref pathTracer, functionPointer);
						return;
					}
				}
				JobRepairPathHelpers.UpdateReachedEndInfo$BurstManaged(ref nextCorners, ref state, ref movementPlane, ref transform, ref shape, ref destination, stopDistance, ref pathTracer);
			}

			// Token: 0x04000ECE RID: 3790
			private static IntPtr Pointer;

			// Token: 0x04000ECF RID: 3791
			private static IntPtr DeferredCompilation;
		}
	}
}
