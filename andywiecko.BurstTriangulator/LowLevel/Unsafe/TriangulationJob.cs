using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200001B RID: 27
	[BurstCompile]
	internal struct TriangulationJob<[IsUnmanaged] T, [IsUnmanaged] T2, [IsUnmanaged] TBig, [IsUnmanaged] TTransform, [IsUnmanaged] TUtils> : IJob where T : struct, ValueType, IComparable<T> where T2 : struct, ValueType where TBig : struct, ValueType, IComparable<TBig> where TTransform : struct, ValueType, ITransform<TTransform, T, T2> where TUtils : struct, ValueType, IUtils<T, T2, TBig>
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00002FDC File Offset: 0x000011DC
		public TriangulationJob(InputData<T2> input, OutputData<T2> output, Args args)
		{
			this.inputPositions = input.Positions;
			this.constraints = input.ConstraintEdges;
			this.constraintTypes = input.ConstraintEdgeTypes;
			this.holeSeeds = input.HoleSeeds;
			this.outputPositions = output.Positions;
			this.triangles = output.Triangles;
			this.halfedges = output.Halfedges;
			this.constrainedHalfedges = output.ConstrainedHalfedges;
			this.status = output.Status;
			this.args = args;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000305C File Offset: 0x0000125C
		public TriangulationJob(Triangulator<T2> @this)
		{
			this.inputPositions = @this.Input.Positions;
			this.constraints = @this.Input.ConstraintEdges;
			this.constraintTypes = @this.Input.ConstraintEdgeTypes;
			this.holeSeeds = @this.Input.HoleSeeds;
			this.outputPositions = @this.Output.Positions;
			this.triangles = @this.Output.Triangles;
			this.halfedges = @this.Output.Halfedges;
			this.constrainedHalfedges = @this.Output.ConstrainedHalfedges;
			this.status = @this.Output.Status;
			this.args = @this.Settings;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003114 File Offset: 0x00001314
		public void Execute()
		{
			default(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>).Triangulate(new InputData<T2>
			{
				Positions = this.inputPositions,
				ConstraintEdges = this.constraints,
				ConstraintEdgeTypes = this.constraintTypes,
				HoleSeeds = this.holeSeeds
			}, new OutputData<T2>
			{
				Positions = this.outputPositions,
				Triangles = this.triangles,
				Halfedges = this.halfedges,
				ConstrainedHalfedges = this.constrainedHalfedges,
				Status = this.status
			}, this.args, Allocator.Temp);
		}

		// Token: 0x0400005C RID: 92
		private NativeArray<T2> inputPositions;

		// Token: 0x0400005D RID: 93
		[NativeDisableContainerSafetyRestriction]
		private NativeArray<int> constraints;

		// Token: 0x0400005E RID: 94
		[NativeDisableContainerSafetyRestriction]
		private NativeArray<ConstraintType> constraintTypes;

		// Token: 0x0400005F RID: 95
		[NativeDisableContainerSafetyRestriction]
		private NativeArray<T2> holeSeeds;

		// Token: 0x04000060 RID: 96
		private NativeList<T2> outputPositions;

		// Token: 0x04000061 RID: 97
		private NativeList<int> triangles;

		// Token: 0x04000062 RID: 98
		private NativeList<int> halfedges;

		// Token: 0x04000063 RID: 99
		private NativeList<HalfedgeState> constrainedHalfedges;

		// Token: 0x04000064 RID: 100
		private NativeReference<Status> status;

		// Token: 0x04000065 RID: 101
		private readonly Args args;
	}
}
