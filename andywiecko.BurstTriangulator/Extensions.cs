using System;
using System.Runtime.CompilerServices;
using andywiecko.BurstTriangulator.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000013 RID: 19
	public static class Extensions
	{
		// Token: 0x0600005B RID: 91 RVA: 0x0000281C File Offset: 0x00000A1C
		[Obsolete("Use AsNativeArray(out Handle) instead! You can learn more in the project manual.")]
		public unsafe static NativeArray<T> AsNativeArray<[IsUnmanaged] T>(this T[] array) where T : struct, ValueType
		{
			NativeArray<T> nativeArray;
			fixed (T[] array2 = array)
			{
				void* ptr;
				if (array == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = (void*)(&array2[0]);
				}
				nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(ptr, array.Length, Allocator.None);
			}
			return nativeArray;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002850 File Offset: 0x00000A50
		public static NativeArray<T> AsNativeArray<[IsUnmanaged] T>(this T[] array, out Handle handle) where T : struct, ValueType
		{
			ulong num;
			NativeArray<T> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num), array.Length, Allocator.None);
			handle = new Handle(num);
			return nativeArray;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000287A File Offset: 0x00000A7A
		public static void Run(this Triangulator<float2> @this)
		{
			new TriangulationJob<float, float2, float, TransformFloat, FloatUtils>(@this).Run<TriangulationJob<float, float2, float, TransformFloat, FloatUtils>>();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002887 File Offset: 0x00000A87
		public static JobHandle Schedule(this Triangulator<float2> @this, JobHandle dependencies = default(JobHandle))
		{
			return new TriangulationJob<float, float2, float, TransformFloat, FloatUtils>(@this).Schedule(dependencies);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002898 File Offset: 0x00000A98
		public unsafe static void Run(this Triangulator<Vector2> @this)
		{
			new TriangulationJob<float, float2, float, TransformFloat, FloatUtils>(new InputData<float2>
			{
				Positions = @this.Input.Positions.Reinterpret<float2>(),
				ConstraintEdges = @this.Input.ConstraintEdges,
				ConstraintEdgeTypes = @this.Input.ConstraintEdgeTypes,
				HoleSeeds = @this.Input.HoleSeeds.Reinterpret<float2>()
			}, new OutputData<float2>
			{
				Triangles = @this.triangles,
				Halfedges = @this.halfedges,
				Positions = *UnsafeUtility.As<NativeList<Vector2>, NativeList<float2>>(ref @this.outputPositions),
				Status = @this.status,
				ConstrainedHalfedges = @this.constrainedHalfedges
			}, @this.Settings).Run<TriangulationJob<float, float2, float, TransformFloat, FloatUtils>>();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002970 File Offset: 0x00000B70
		public unsafe static JobHandle Schedule(this Triangulator<Vector2> @this, JobHandle dependencies = default(JobHandle))
		{
			return new TriangulationJob<float, float2, float, TransformFloat, FloatUtils>(new InputData<float2>
			{
				Positions = @this.Input.Positions.Reinterpret<float2>(),
				ConstraintEdges = @this.Input.ConstraintEdges,
				ConstraintEdgeTypes = @this.Input.ConstraintEdgeTypes,
				HoleSeeds = @this.Input.HoleSeeds.Reinterpret<float2>()
			}, new OutputData<float2>
			{
				Triangles = @this.triangles,
				Halfedges = @this.halfedges,
				Positions = *UnsafeUtility.As<NativeList<Vector2>, NativeList<float2>>(ref @this.outputPositions),
				Status = @this.status,
				ConstrainedHalfedges = @this.constrainedHalfedges
			}, @this.Settings).Schedule(dependencies);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002A48 File Offset: 0x00000C48
		public static void Run(this Triangulator<double2> @this)
		{
			new TriangulationJob<double, double2, double, TransformDouble, DoubleUtils>(@this).Run<TriangulationJob<double, double2, double, TransformDouble, DoubleUtils>>();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002A55 File Offset: 0x00000C55
		public static JobHandle Schedule(this Triangulator<double2> @this, JobHandle dependencies = default(JobHandle))
		{
			return new TriangulationJob<double, double2, double, TransformDouble, DoubleUtils>(@this).Schedule(dependencies);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002A63 File Offset: 0x00000C63
		public static void Run(this Triangulator<int2> @this)
		{
			new TriangulationJob<int, int2, long, TransformInt, IntUtils>(@this).Run<TriangulationJob<int, int2, long, TransformInt, IntUtils>>();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002A70 File Offset: 0x00000C70
		public static JobHandle Schedule(this Triangulator<int2> @this, JobHandle dependencies = default(JobHandle))
		{
			return new TriangulationJob<int, int2, long, TransformInt, IntUtils>(@this).Schedule(dependencies);
		}
	}
}
