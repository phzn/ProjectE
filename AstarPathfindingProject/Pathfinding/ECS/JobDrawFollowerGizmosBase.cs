using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x02000327 RID: 807
	[BurstCompile]
	[CompilerGenerated]
	public struct JobDrawFollowerGizmosBase : IJobEntity, IJobEntityChunkBeginEnd, IJobChunk
	{
		// Token: 0x060013DA RID: 5082 RVA: 0x00075DC3 File Offset: 0x00073FC3
		public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.orientation = (chunk.Has<OrientationYAxisForward>() ? OrientationMode.YAxisForward : OrientationMode.ZAxisForward);
			return true;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00003788 File Offset: 0x00001988
		public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
		{
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00075DD8 File Offset: 0x00073FD8
		public void Execute(in LocalTransform transform, in AgentCylinderShape shape, in DestinationPoint destination)
		{
			JobDrawFollowerGizmosBase.DrawGizmos(ref this.draw, in transform, in shape, in destination, this.orientation == OrientationMode.YAxisForward);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00075DF1 File Offset: 0x00073FF1
		[BurstCompile]
		public static void DrawGizmos(ref CommandBuilder draw, in LocalTransform transform, in AgentCylinderShape shape, in DestinationPoint destination, bool draw2D)
		{
			JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.Invoke(ref draw, in transform, in shape, in destination, draw2D);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00075E00 File Offset: 0x00074000
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			bool flag = this.OnChunkBegin(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask);
			if (flag)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
						ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, i);
						ref DestinationPoint ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, i);
						this.Execute(in ptr, in ptr2, in ptr3);
						num++;
					}
				}
				else if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num2 = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
					{
						while (j < num2)
						{
							ref LocalTransform ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
							ref AgentCylinderShape ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, j);
							ref DestinationPoint ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, j);
							this.Execute(in ptr4, in ptr5, in ptr6);
							j++;
							num++;
						}
					}
				}
				else
				{
					ulong num3 = chunkEnabledMask.ULong0;
					int num4 = math.min(64, count);
					for (int k = 0; k < num4; k++)
					{
						if ((num3 & 1UL) != 0UL)
						{
							ref LocalTransform ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
							ref AgentCylinderShape ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, k);
							ref DestinationPoint ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, k);
							this.Execute(in ptr7, in ptr8, in ptr9);
							num++;
						}
						num3 >>= 1;
					}
					num3 = chunkEnabledMask.ULong1;
					for (int l = 64; l < count; l++)
					{
						if ((num3 & 1UL) != 0UL)
						{
							ref LocalTransform ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
							ref AgentCylinderShape ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, l);
							ref DestinationPoint ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, l);
							this.Execute(in ptr10, in ptr11, in ptr12);
							num++;
						}
						num3 >>= 1;
					}
				}
			}
			this.OnChunkEnd(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask, flag);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x00076014 File Offset: 0x00074214
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0007602B File Offset: 0x0007422B
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0007602B File Offset: 0x0007422B
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0007602B File Offset: 0x0007422B
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0007602B File Offset: 0x0007422B
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0007602B File Offset: 0x0007422B
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0007602B File Offset: 0x0007422B
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0007602B File Offset: 0x0007422B
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0007602B File Offset: 0x0007422B
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00076034 File Offset: 0x00074234
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0007602B File Offset: 0x0007422B
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0007602B File Offset: 0x0007422B
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0007602B File Offset: 0x0007422B
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0007602B File Offset: 0x0007422B
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00076057 File Offset: 0x00074257
		bool IJobEntityChunkBeginEnd.OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			return this.OnChunkBegin(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00076064 File Offset: 0x00074264
		void IJobEntityChunkBeginEnd.OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
		{
			this.OnChunkEnd(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask, chunkWasExecuted);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00076073 File Offset: 0x00074273
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00076080 File Offset: 0x00074280
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void DrawGizmos$BurstManaged(ref CommandBuilder draw, in LocalTransform transform, in AgentCylinderShape shape, in DestinationPoint destination, bool draw2D)
		{
			float radius = shape.radius;
			Color shapeGizmoColor = JobDrawFollowerGizmosBase.ShapeGizmoColor;
			LocalTransform localTransform = transform;
			draw.PushMatrix(localTransform.ToMatrix());
			if (draw2D)
			{
				draw.Circle(float3.zero, new float3(0f, 1f, 0f), radius, shapeGizmoColor);
			}
			else
			{
				draw.WireCylinder(float3.zero, new float3(0f, 1f, 0f), shape.height, radius, shapeGizmoColor);
			}
			draw.ArrowheadArc(float3.zero, new float3(0f, 0f, 1f), radius * 1.05f, shapeGizmoColor);
			draw.PopMatrix();
			if (math.all(math.isfinite(destination.destination)))
			{
				float3 facingDirection = destination.facingDirection;
				if (math.any(facingDirection != float3.zero))
				{
					draw.xz.ArrowheadArc(destination.destination, facingDirection, 0.25f, Color.blue);
				}
				draw.xz.Circle(destination.destination, 0.2f, Color.blue);
			}
		}

		// Token: 0x04000E52 RID: 3666
		public CommandBuilder draw;

		// Token: 0x04000E53 RID: 3667
		private OrientationMode orientation;

		// Token: 0x04000E54 RID: 3668
		internal static readonly Color ShapeGizmoColor = new Color(0.9411765f, 0.8352941f, 0.11764706f);

		// Token: 0x04000E55 RID: 3669
		private JobDrawFollowerGizmosBase.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x02000328 RID: 808
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x060013FB RID: 5115 RVA: 0x00076194 File Offset: 0x00074394
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAll<LocalTransform>().WithAll<AgentCylinderShape>().WithAll<DestinationPoint>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x060013FC RID: 5116 RVA: 0x000761E3 File Offset: 0x000743E3
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x060013FD RID: 5117 RVA: 0x000761FB File Offset: 0x000743FB
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobDrawFollowerGizmosBase job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobDrawFollowerGizmosBase>(query);
			}

			// Token: 0x060013FE RID: 5118 RVA: 0x00076210 File Offset: 0x00074410
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobDrawFollowerGizmosBase job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobDrawFollowerGizmosBase>(query, dependency);
			}

			// Token: 0x060013FF RID: 5119 RVA: 0x00076226 File Offset: 0x00074426
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobDrawFollowerGizmosBase job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobDrawFollowerGizmosBase>(query, dependency);
			}

			// Token: 0x06001400 RID: 5120 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobDrawFollowerGizmosBase job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x06001401 RID: 5121 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobDrawFollowerGizmosBase job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x06001402 RID: 5122 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobDrawFollowerGizmosBase job, EntityManager entityManager)
			{
			}

			// Token: 0x04000E56 RID: 3670
			public JobDrawFollowerGizmosBase.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E57 RID: 3671
			public EntityQuery DefaultQuery;

			// Token: 0x02000329 RID: 809
			public struct TypeHandle
			{
				// Token: 0x06001403 RID: 5123 RVA: 0x0007623C File Offset: 0x0007443C
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
					this.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(true);
				}

				// Token: 0x06001404 RID: 5124 RVA: 0x00076265 File Offset: 0x00074465
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E58 RID: 3672
				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				// Token: 0x04000E59 RID: 3673
				[ReadOnly]
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

				// Token: 0x04000E5A RID: 3674
				[ReadOnly]
				public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle;
			}
		}

		// Token: 0x0200032A RID: 810
		public struct InternalCompiler
		{
			// Token: 0x06001405 RID: 5125 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}

		// Token: 0x0200032B RID: 811
		// (Invoke) Token: 0x06001407 RID: 5127
		internal delegate void DrawGizmos_0000127C$PostfixBurstDelegate(ref CommandBuilder draw, in LocalTransform transform, in AgentCylinderShape shape, in DestinationPoint destination, bool draw2D);

		// Token: 0x0200032C RID: 812
		internal static class DrawGizmos_0000127C$BurstDirectCall
		{
			// Token: 0x0600140A RID: 5130 RVA: 0x0007628B File Offset: 0x0007448B
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.Pointer == 0)
				{
					JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.DeferredCompilation, methodof(JobDrawFollowerGizmosBase.DrawGizmos$BurstManaged(ref CommandBuilder, ref LocalTransform, ref AgentCylinderShape, ref DestinationPoint, bool)).MethodHandle, typeof(JobDrawFollowerGizmosBase.DrawGizmos_0000127C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.Pointer;
			}

			// Token: 0x0600140B RID: 5131 RVA: 0x000762B8 File Offset: 0x000744B8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600140C RID: 5132 RVA: 0x000762D0 File Offset: 0x000744D0
			public static void Constructor()
			{
				JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(JobDrawFollowerGizmosBase.DrawGizmos(ref CommandBuilder, ref LocalTransform, ref AgentCylinderShape, ref DestinationPoint, bool)).MethodHandle);
			}

			// Token: 0x0600140D RID: 5133 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x0600140E RID: 5134 RVA: 0x000762E1 File Offset: 0x000744E1
			// Note: this type is marked as 'beforefieldinit'.
			static DrawGizmos_0000127C$BurstDirectCall()
			{
				JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.Constructor();
			}

			// Token: 0x0600140F RID: 5135 RVA: 0x000762E8 File Offset: 0x000744E8
			public static void Invoke(ref CommandBuilder draw, in LocalTransform transform, in AgentCylinderShape shape, in DestinationPoint destination, bool draw2D)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = JobDrawFollowerGizmosBase.DrawGizmos_0000127C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Drawing.CommandBuilder&,Unity.Transforms.LocalTransform&,Pathfinding.ECS.AgentCylinderShape&,Pathfinding.ECS.DestinationPoint&,System.Boolean), ref draw, ref transform, ref shape, ref destination, draw2D, functionPointer);
						return;
					}
				}
				JobDrawFollowerGizmosBase.DrawGizmos$BurstManaged(ref draw, in transform, in shape, in destination, draw2D);
			}

			// Token: 0x04000E5B RID: 3675
			private static IntPtr Pointer;

			// Token: 0x04000E5C RID: 3676
			private static IntPtr DeferredCompilation;
		}
	}
}
