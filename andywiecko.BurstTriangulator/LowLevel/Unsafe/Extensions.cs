using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200001A RID: 26
	public static class Extensions
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public static void Triangulate(this UnsafeTriangulator @this, InputData<double2> input, OutputData<double2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<double, double2, double, TransformDouble, DoubleUtils>).Triangulate(input, output, args, allocator);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002DD8 File Offset: 0x00000FD8
		public static void PlantHoleSeeds(this UnsafeTriangulator @this, InputData<double2> input, OutputData<double2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<double, double2, double, TransformDouble, DoubleUtils>).PlantHoleSeeds(input, output, args, allocator);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public static void RefineMesh(this UnsafeTriangulator @this, OutputData<double2> output, Allocator allocator, double areaThreshold = 1.0, double angleThreshold = 0.0872664626, bool constrainBoundary = false)
		{
			default(UnsafeTriangulator<double, double2, double, TransformDouble, DoubleUtils>).RefineMesh(output, allocator, 2.0 * areaThreshold, angleThreshold, constrainBoundary);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002E24 File Offset: 0x00001024
		public static void Triangulate(this UnsafeTriangulator<float2> @this, InputData<float2> input, OutputData<float2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<float, float2, float, TransformFloat, FloatUtils>).Triangulate(input, output, args, allocator);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002E44 File Offset: 0x00001044
		public static void PlantHoleSeeds(this UnsafeTriangulator<float2> @this, InputData<float2> input, OutputData<float2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<float, float2, float, TransformFloat, FloatUtils>).PlantHoleSeeds(input, output, args, allocator);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002E64 File Offset: 0x00001064
		public static void RefineMesh(this UnsafeTriangulator<float2> @this, OutputData<float2> output, Allocator allocator, float areaThreshold = 1f, float angleThreshold = 0.08726646f, bool constrainBoundary = false)
		{
			default(UnsafeTriangulator<float, float2, float, TransformFloat, FloatUtils>).RefineMesh(output, allocator, 2f * areaThreshold, angleThreshold, constrainBoundary);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002E8C File Offset: 0x0000108C
		public unsafe static void Triangulate(this UnsafeTriangulator<Vector2> @this, InputData<Vector2> input, OutputData<Vector2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<float, float2, float, TransformFloat, FloatUtils>).Triangulate(*UnsafeUtility.As<InputData<Vector2>, InputData<float2>>(ref input), *UnsafeUtility.As<OutputData<Vector2>, OutputData<float2>>(ref output), args, allocator);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002EC4 File Offset: 0x000010C4
		public unsafe static void PlantHoleSeeds(this UnsafeTriangulator<Vector2> @this, InputData<Vector2> input, OutputData<Vector2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<float, float2, float, TransformFloat, FloatUtils>).PlantHoleSeeds(*UnsafeUtility.As<InputData<Vector2>, InputData<float2>>(ref input), *UnsafeUtility.As<OutputData<Vector2>, OutputData<float2>>(ref output), args, allocator);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002EFC File Offset: 0x000010FC
		public unsafe static void RefineMesh(this UnsafeTriangulator<Vector2> @this, OutputData<Vector2> output, Allocator allocator, float areaThreshold = 1f, float angleThreshold = 0.08726646f, bool constrainBoundary = false)
		{
			default(UnsafeTriangulator<float, float2, float, TransformFloat, FloatUtils>).RefineMesh(*UnsafeUtility.As<OutputData<Vector2>, OutputData<float2>>(ref output), allocator, 2f * areaThreshold, angleThreshold, constrainBoundary);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002F30 File Offset: 0x00001130
		public static void Triangulate(this UnsafeTriangulator<double2> @this, InputData<double2> input, OutputData<double2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<double, double2, double, TransformDouble, DoubleUtils>).Triangulate(input, output, args, allocator);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002F50 File Offset: 0x00001150
		public static void PlantHoleSeeds(this UnsafeTriangulator<double2> @this, InputData<double2> input, OutputData<double2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<double, double2, double, TransformDouble, DoubleUtils>).PlantHoleSeeds(input, output, args, allocator);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002F70 File Offset: 0x00001170
		public static void RefineMesh(this UnsafeTriangulator<double2> @this, OutputData<double2> output, Allocator allocator, double areaThreshold = 1.0, double angleThreshold = 0.0872664626, bool constrainBoundary = false)
		{
			default(UnsafeTriangulator<double, double2, double, TransformDouble, DoubleUtils>).RefineMesh(output, allocator, 2.0 * areaThreshold, angleThreshold, constrainBoundary);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002F9C File Offset: 0x0000119C
		public static void Triangulate(this UnsafeTriangulator<int2> @this, InputData<int2> input, OutputData<int2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<int, int2, long, TransformInt, IntUtils>).Triangulate(input, output, args, allocator);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002FBC File Offset: 0x000011BC
		public static void PlantHoleSeeds(this UnsafeTriangulator<int2> @this, InputData<int2> input, OutputData<int2> output, Args args, Allocator allocator)
		{
			default(UnsafeTriangulator<int, int2, long, TransformInt, IntUtils>).PlantHoleSeeds(input, output, args, allocator);
		}
	}
}
