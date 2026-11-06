using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200001C RID: 28
	internal readonly struct UnsafeTriangulator<[IsUnmanaged] T, [IsUnmanaged] T2, [IsUnmanaged] TBig, [IsUnmanaged] TTransform, [IsUnmanaged] TUtils> where T : struct, ValueType, IComparable<T> where T2 : struct, ValueType where TBig : struct, ValueType, IComparable<TBig> where TTransform : struct, ValueType, ITransform<TTransform, T, T2> where TUtils : struct, ValueType, IUtils<T, T2, TBig>
	{
		// Token: 0x06000080 RID: 128 RVA: 0x000031C0 File Offset: 0x000013C0
		public void Triangulate(InputData<T2> input, OutputData<T2> output, Args args, Allocator allocator)
		{
			NativeReference<Status> nativeReference = default(NativeReference<Status>);
			NativeList<T2> nativeList = default(NativeList<T2>);
			NativeList<int> nativeList2 = default(NativeList<int>);
			NativeList<HalfedgeState> nativeList3 = default(NativeList<HalfedgeState>);
			if (!output.Status.IsCreated)
			{
				nativeReference = new NativeReference<Status>(allocator, NativeArrayOptions.ClearMemory);
				output.Status = nativeReference;
			}
			if (!output.Positions.IsCreated)
			{
				nativeList = new NativeList<T2>(16384, allocator);
				output.Positions = nativeList;
			}
			if (!output.Halfedges.IsCreated)
			{
				nativeList2 = new NativeList<int>(98304, allocator);
				output.Halfedges = nativeList2;
			}
			if (!output.ConstrainedHalfedges.IsCreated)
			{
				nativeList3 = new NativeList<HalfedgeState>(98304, allocator);
				output.ConstrainedHalfedges = nativeList3;
			}
			output.Status.Value = Status.Ok;
			output.Triangles.Clear();
			output.Positions.Clear();
			output.Halfedges.Clear();
			output.ConstrainedHalfedges.Clear();
			NativeArray<T2> nativeArray;
			TTransform ttransform;
			this.PreProcessInputStep(input, output, args, out nativeArray, out ttransform, allocator);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ValidateInputStep(input, output, args).Execute();
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.DelaunayTriangulationStep(output, args).Execute(allocator);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ConstrainEdgesStep(input, output, args).Execute(allocator);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep(output, args, nativeArray).Execute(allocator, input.ConstraintEdges.IsCreated);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep(output, args, ttransform).Execute(allocator, args.RefineMesh, !input.ConstraintEdges.IsCreated || !args.RestoreBoundary);
			this.PostProcessInputStep(output, args, ttransform);
			Status value = output.Status.Value;
			if (nativeArray.IsCreated)
			{
				nativeArray.Dispose();
			}
			if (nativeReference.IsCreated)
			{
				nativeReference.Dispose();
			}
			if (nativeList.IsCreated)
			{
				nativeList.Dispose();
			}
			if (nativeList2.IsCreated)
			{
				nativeList2.Dispose();
			}
			if (nativeList3.IsCreated)
			{
				nativeList3.Dispose();
			}
			if (args.Verbose && value.IsError)
			{
				Debug.LogError(value.ToFixedString());
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000033F4 File Offset: 0x000015F4
		public void PlantHoleSeeds(InputData<T2> input, OutputData<T2> output, Args args, Allocator allocator)
		{
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep(input, output, args).Execute(allocator, true);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003414 File Offset: 0x00001614
		public void RefineMesh(OutputData<T2> output, Allocator allocator, T area2Threshold, T angleThreshold, bool constrainBoundary = false)
		{
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep(output, area2Threshold, angleThreshold).Execute(allocator, true, constrainBoundary);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003438 File Offset: 0x00001638
		private void PreProcessInputStep(InputData<T2> input, OutputData<T2> output, Args args, out NativeArray<T2> localHoles, out TTransform lt, Allocator allocator)
		{
			using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.PreProcessInputStep.Auto())
			{
				NativeList<T2> positions = output.Positions;
				positions.ResizeUninitialized(input.Positions.Length);
				if (args.Preprocessor == Preprocessor.PCA || args.Preprocessor == Preprocessor.COM)
				{
					TTransform ttransform2;
					if (args.Preprocessor != Preprocessor.PCA)
					{
						TTransform ttransform = default(TTransform);
						ttransform2 = ttransform.CalculateLocalTransformation(input.Positions);
					}
					else
					{
						TTransform ttransform = default(TTransform);
						ttransform2 = ttransform.CalculatePCATransformation(input.Positions);
					}
					lt = ttransform2;
					for (int i = 0; i < input.Positions.Length; i++)
					{
						positions[i] = lt.Transform(input.Positions[i]);
					}
					localHoles = (input.HoleSeeds.IsCreated ? new NativeArray<T2>(input.HoleSeeds.Length, allocator, NativeArrayOptions.ClearMemory) : default(NativeArray<T2>));
					for (int j = 0; j < input.HoleSeeds.Length; j++)
					{
						localHoles[j] = lt.Transform(input.HoleSeeds[j]);
					}
				}
				else
				{
					if (args.Preprocessor != Preprocessor.None)
					{
						throw new ArgumentException();
					}
					positions.CopyFrom(in input.Positions);
					localHoles = (input.HoleSeeds.IsCreated ? new NativeArray<T2>(input.HoleSeeds, allocator) : default(NativeArray<T2>));
					TTransform ttransform = default(TTransform);
					lt = ttransform.Identity;
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003610 File Offset: 0x00001810
		private void PostProcessInputStep(OutputData<T2> output, Args args, TTransform lt)
		{
			if (args.Preprocessor == Preprocessor.None)
			{
				return;
			}
			using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.PostProcessInputStep.Auto())
			{
				TTransform ttransform = lt.Inverse();
				for (int i = 0; i < output.Positions.Length; i++)
				{
					output.Positions[i] = ttransform.Transform(output.Positions[i]);
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000036A0 File Offset: 0x000018A0
		internal static bool AngleIsTooSmall(T2 pA, T2 pB, T2 pC, T minimumAngle)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t = tutils.cos(minimumAngle);
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t2 = tutils.normalizesafe(tutils2.diff(pB, pA));
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t3 = tutils.normalizesafe(tutils2.diff(pC, pB));
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t4 = tutils.normalizesafe(tutils2.diff(pA, pC));
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t5 = t2;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t6 = tutils2.dot(t5, tutils3.neg(t4));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t7 = t3;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t8 = tutils2.dot(t7, tutils3.neg(t2));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t9 = t4;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.anygreaterthan(t6, t8, tutils2.dot(t9, tutils3.neg(t3)), t);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000037D8 File Offset: 0x000019D8
		internal static T Area2(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t = tutils2.diff(b, a);
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Cross(t, tutils2.diff(c, a)));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003828 File Offset: 0x00001A28
		private static T Cross(T2 a, T2 b)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t = tutils4.X(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig tbig = tutils3.mul(t, tutils4.Y(b));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t2 = tutils4.Y(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.Cast(tutils2.diff(tbig, tutils3.mul(t2, tutils4.X(b))));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000038D4 File Offset: 0x00001AD4
		private static TBig CircumRadiusSq(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.distancesq(tutils2.CircumCenter(a, b, c), a);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000390C File Offset: 0x00001B0C
		private static ValueTuple<T2, T> CalculateCircumCircle(int i, int j, int k, NativeArray<T2> positions)
		{
			T2 t = positions[i];
			T2 t2 = positions[j];
			T2 t3 = positions[k];
			T2 t4 = t;
			T2 t5 = t2;
			T2 t6 = t3;
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t7 = tutils.CircumCenter(t4, t5, t6);
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return new ValueTuple<T2, T>(t7, tutils.Cast(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CircumRadiusSq(t4, t5, t6)));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003974 File Offset: 0x00001B74
		private static bool ccw(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t = tutils4.Y(c);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t2 = tutils3.diff(t, tutils4.Y(a));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t3 = tutils4.X(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig tbig = tutils2.mul(t2, tutils3.diff(t3, tutils4.X(a)));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t4 = tutils4.Y(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t5 = tutils3.diff(t4, tutils4.Y(a));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t6 = tutils4.X(c);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.greater(tbig, tutils2.mul(t5, tutils3.diff(t6, tutils4.X(a))));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003AA6 File Offset: 0x00001CA6
		internal static bool EdgeEdgeIntersection(T2 a0, T2 a1, T2 b0, T2 b1)
		{
			return UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(a0, a1, b0) != UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(a0, a1, b1) && UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(b0, b1, a0) != UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(b0, b1, a1);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003AD1 File Offset: 0x00001CD1
		private static int NextHalfedge(int he)
		{
			if (he % 3 != 2)
			{
				return he + 1;
			}
			return he - 2;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003AE0 File Offset: 0x00001CE0
		internal static bool IsConvexQuadrilateral(T2 a, T2 b, T2 c, T2 d)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig tbig = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(a, c, b));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			if (tutils.greater(tbig, tutils2.EPSILON()))
			{
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TBig tbig2 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(a, c, d));
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				if (tutils.greater(tbig2, tutils2.EPSILON()))
				{
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TBig tbig3 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(b, d, a));
					tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (tutils.greater(tbig3, tutils2.EPSILON()))
					{
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						TBig tbig4 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(b, d, c));
						tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (tutils.greater(tbig4, tutils2.EPSILON()))
						{
							return UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.EdgeEdgeIntersection(a, c, b, d);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003C0C File Offset: 0x00001E0C
		private static TBig Orient2dFast(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t = tutils4.Y(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t2 = tutils3.diff(t, tutils4.Y(c));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t3 = tutils4.X(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig tbig = tutils2.mul(t2, tutils3.diff(t3, tutils4.X(c)));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t4 = tutils4.X(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t5 = tutils3.diff(t4, tutils4.X(c));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T t6 = tutils4.Y(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.diff(tbig, tutils2.mul(t5, tutils3.diff(t6, tutils4.Y(c))));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003D40 File Offset: 0x00001F40
		internal static bool PointLineSegmentIntersection(T2 a, T2 b0, T2 b1)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig tbig = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(a, b0, b1));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			if (tutils.le(tbig, tutils2.EPSILON()))
			{
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				bool2 @bool = tutils.ge(a, tutils2.min(b0, b1));
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				return math.all(@bool & tutils.le(a, tutils2.max(b0, b1)));
			}
			return false;
		}

		// Token: 0x04000066 RID: 102
		private static readonly TUtils utils;

		// Token: 0x0200001D RID: 29
		private readonly struct Markers
		{
			// Token: 0x04000067 RID: 103
			public static readonly ProfilerMarker PreProcessInputStep = new ProfilerMarker("PreProcessInputStep");

			// Token: 0x04000068 RID: 104
			public static readonly ProfilerMarker PostProcessInputStep = new ProfilerMarker("PostProcessInputStep");

			// Token: 0x04000069 RID: 105
			public static readonly ProfilerMarker ValidateInputStep = new ProfilerMarker("ValidateInputStep");

			// Token: 0x0400006A RID: 106
			public static readonly ProfilerMarker DelaunayTriangulationStep = new ProfilerMarker("DelaunayTriangulationStep");

			// Token: 0x0400006B RID: 107
			public static readonly ProfilerMarker ConstrainEdgesStep = new ProfilerMarker("ConstrainEdgesStep");

			// Token: 0x0400006C RID: 108
			public static readonly ProfilerMarker PlantingSeedStep = new ProfilerMarker("PlantingSeedStep");

			// Token: 0x0400006D RID: 109
			public static readonly ProfilerMarker RefineMeshStep = new ProfilerMarker("RefineMeshStep");
		}

		// Token: 0x0200001E RID: 30
		private struct ValidateInputStep
		{
			// Token: 0x06000092 RID: 146 RVA: 0x00003E68 File Offset: 0x00002068
			public ValidateInputStep(InputData<T2> input, OutputData<T2> output, Args args)
			{
				this.positions = output.Positions.AsArray().AsReadOnly();
				this.status = output.Status;
				this.args = args;
				this.constraints = input.ConstraintEdges.AsReadOnly();
				this.constraintTypes = input.ConstraintEdgeTypes.AsReadOnly();
				this.holes = input.HoleSeeds.AsReadOnly();
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00003ED8 File Offset: 0x000020D8
			public void Execute()
			{
				if (!this.args.ValidateInput)
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.ValidateInputStep.Auto())
				{
					this.ValidateArgs();
					this.ValidatePositions();
					this.ValidateConstraints();
					this.ValidateHoles();
				}
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00003F3C File Offset: 0x0000213C
			private void ValidateArgs()
			{
				if (this.args.AutoHolesAndBoundary && !this.constraints.IsCreated)
				{
					this.status.Value = Status.ConstraintEdgesMissingForAutoHolesAndBoundary;
				}
				if (this.args.RestoreBoundary && !this.constraints.IsCreated)
				{
					this.status.Value = Status.ConstraintEdgesMissingForRestoreBoundary;
				}
				if (this.args.RefineMesh)
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (!utils.SupportsRefinement())
					{
						this.status.Value = Status.RefinementNotSupportedForCoordinateType;
					}
				}
				if (this.constraints.IsCreated && this.args.SloanMaxIters < 1)
				{
					this.status.Value = Status.SloanMaxItersMustBePositive(this.args.SloanMaxIters);
				}
				if (this.args.RefineMesh && this.args.RefinementThresholdArea < 0f)
				{
					this.status.Value = Status.RefinementThresholdAreaMustBePositive;
				}
				if ((this.args.RefineMesh && this.args.RefinementThresholdAngle < 0f) || this.args.RefinementThresholdAngle > 0.7853982f)
				{
					this.status.Value = Status.RefinementThresholdAngleOutOfRange;
				}
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00004078 File Offset: 0x00002278
			private void ValidatePositions()
			{
				if (this.positions.Length < 3)
				{
					this.status.Value = Status.PositionsLengthLessThan3(this.positions.Length);
					return;
				}
				for (int i = 0; i < this.positions.Length; i++)
				{
					TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (math.any(!tutils.isfinite(this.positions[i])))
					{
						this.status.Value = Status.PositionsMustBeFinite(i);
						return;
					}
					T2 t = this.positions[i];
					for (int j = i + 1; j < this.positions.Length; j++)
					{
						T2 t2 = this.positions[j];
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (math.all(tutils.eq(t, t2)))
						{
							this.status.Value = Status.DuplicatePosition(i);
							return;
						}
					}
				}
			}

			// Token: 0x06000096 RID: 150 RVA: 0x0000416C File Offset: 0x0000236C
			private void ValidateConstraints()
			{
				if (!this.constraints.IsCreated)
				{
					return;
				}
				if (this.constraints.Length % 2 != 0)
				{
					this.status.Value = Status.ConstraintsLengthNotDivisibleBy2(this.constraints.Length);
					return;
				}
				if (this.constraintTypes.IsCreated && this.constraintTypes.Length * 2 != this.constraints.Length)
				{
					this.status.Value = Status.ConstraintArrayLengthMismatch(this.constraints.Length, this.constraintTypes.Length);
					return;
				}
				for (int i = 0; i < this.constraints.Length / 2; i++)
				{
					int num = this.constraints[2 * i];
					int num2 = this.constraints[2 * i + 1];
					int num3 = num;
					int num4 = num2;
					int length = this.positions.Length;
					if (num3 >= length || num3 < 0 || num4 >= length || num4 < 0)
					{
						this.status.Value = Status.ConstraintOutOfBounds(i, new int2(num3, num4), length);
						return;
					}
					if (num3 == num4)
					{
						this.status.Value = Status.ConstraintSelfLoop(i, new int2(num3, num4));
						return;
					}
				}
				for (int j = 0; j < this.constraints.Length / 2; j++)
				{
					int num = this.constraints[2 * j];
					int num5 = this.constraints[2 * j + 1];
					int num6 = num;
					int num7 = num5;
					T2 t = this.positions[num6];
					T2 t2 = this.positions[num7];
					T2 t3 = t;
					T2 t4 = t2;
					for (int k = j + 1; k < this.constraints.Length / 2; k++)
					{
						num = this.constraints[2 * k];
						int num8 = this.constraints[2 * k + 1];
						int num9 = num;
						int num10 = num8;
						if ((num6 == num9 && num7 == num10) || (num6 == num10 && num7 == num9))
						{
							this.status.Value = Status.DuplicateConstraint(j, k);
							return;
						}
						if (num6 != num9 && num6 != num10 && num7 != num9 && num7 != num10)
						{
							t = this.positions[num9];
							T2 t5 = this.positions[num10];
							T2 t6 = t;
							T2 t7 = t5;
							if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.EdgeEdgeIntersection(t3, t4, t6, t7) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t3, t6, t7) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t4, t6, t7) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t6, t3, t4) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t7, t3, t4))
							{
								this.status.Value = Status.ConstraintIntersection(j, k);
								return;
							}
						}
					}
				}
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00004414 File Offset: 0x00002614
			private void ValidateHoles()
			{
				if (!this.holes.IsCreated)
				{
					return;
				}
				if (!this.constraints.IsCreated)
				{
					this.status.Value = Status.RedudantHolesArray;
				}
				for (int i = 0; i < this.holes.Length; i++)
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (math.any(!utils.isfinite(this.holes[i])))
					{
						this.status.Value = Status.HoleMustBeFinite(i);
						return;
					}
				}
			}

			// Token: 0x0400006E RID: 110
			private NativeArray<T2>.ReadOnly positions;

			// Token: 0x0400006F RID: 111
			private NativeReference<Status> status;

			// Token: 0x04000070 RID: 112
			private readonly Args args;

			// Token: 0x04000071 RID: 113
			private NativeArray<int>.ReadOnly constraints;

			// Token: 0x04000072 RID: 114
			private NativeArray<ConstraintType>.ReadOnly constraintTypes;

			// Token: 0x04000073 RID: 115
			private NativeArray<T2>.ReadOnly holes;
		}

		// Token: 0x0200001F RID: 31
		private struct DelaunayTriangulationStep
		{
			// Token: 0x06000098 RID: 152 RVA: 0x000044A0 File Offset: 0x000026A0
			public DelaunayTriangulationStep(OutputData<T2> output, Args args)
			{
				this.status = output.Status;
				this.positions = output.Positions.AsArray().AsReadOnly();
				this.triangles = output.Triangles;
				this.halfedges = output.Halfedges;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.hullStart = int.MaxValue;
				this.verbose = args.Verbose;
				this.hashSize = (int)math.ceil(math.sqrt((float)this.positions.Length));
				this.trianglesLen = 0;
				this.hullNext = default(NativeArray<int>);
				this.hullPrev = default(NativeArray<int>);
				this.hullTri = default(NativeArray<int>);
				this.hullHash = default(NativeArray<int>);
				this.EDGE_STACK = default(NativeArray<int>);
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00004570 File Offset: 0x00002770
			public void Execute(Allocator allocator)
			{
				if (this.status.Value.IsError)
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.DelaunayTriangulationStep.Auto())
				{
					int length = this.positions.Length;
					int num = math.max(2 * length - 5, 0);
					this.triangles.Length = 3 * num;
					this.halfedges.Length = 3 * num;
					NativeArray<int> nativeArray = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory);
					TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T2 t = tutils.MaxValue2();
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T2 t2 = tutils.MinValue2();
					for (int i = 0; i < this.positions.Length; i++)
					{
						T2 t3 = this.positions[i];
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t = tutils.min(t, t3);
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t2 = tutils.max(t2, t3);
						nativeArray[i] = i;
					}
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T2 t4 = tutils.avg(t, t2);
					int num2 = int.MaxValue;
					int num3 = int.MaxValue;
					int num4 = int.MaxValue;
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TBig tbig = tutils.MaxValue();
					for (int j = 0; j < this.positions.Length; j++)
					{
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						TBig tbig2 = tutils.distancesq(t4, this.positions[j]);
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (tutils.less(tbig2, tbig))
						{
							num2 = j;
							tbig = tbig2;
						}
					}
					T2 t5 = this.positions[num2];
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					tbig = tutils.MaxValue();
					for (int k = 0; k < this.positions.Length; k++)
					{
						if (k != num2)
						{
							tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							TBig tbig3 = tutils.distancesq(t5, this.positions[k]);
							tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							if (tutils.less(tbig3, tbig))
							{
								num3 = k;
								tbig = tbig3;
							}
						}
					}
					T2 t6 = this.positions[num3];
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TBig tbig4 = tutils.MaxValue();
					for (int l = 0; l < this.positions.Length; l++)
					{
						if (l != num2 && l != num3)
						{
							T2 t7 = this.positions[l];
							TBig tbig5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CircumRadiusSq(t5, t6, t7);
							tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							if (tutils.less(tbig5, tbig4))
							{
								num4 = l;
								tbig4 = tbig5;
							}
						}
					}
					if (num4 != 2147483647)
					{
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						T2 t8 = tutils2.CircumCenter(t5, t6, this.positions[num4]);
						tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (!math.any(tutils.eq(t8, tutils2.MaxValue2())))
						{
							using (this.hullPrev = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory))
							{
								using (this.hullNext = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory))
								{
									using (this.hullTri = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory))
									{
										using (this.hullHash = new NativeArray<int>(this.hashSize, allocator, NativeArrayOptions.ClearMemory))
										{
											using (this.EDGE_STACK = new NativeArray<int>(math.min(3 * num, 512), allocator, NativeArrayOptions.ClearMemory))
											{
												NativeArray<TBig> nativeArray7 = new NativeArray<TBig>(length, allocator, NativeArrayOptions.ClearMemory);
												T2 t9 = this.positions[num4];
												tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												TBig tbig6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t5, t6, t9);
												tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												if (tutils.less(tbig6, tutils2.ZeroTBig()))
												{
													int num5 = num4;
													int num6 = num3;
													num3 = num5;
													num4 = num6;
													T2 t10 = t9;
													T2 t11 = t6;
													t6 = t10;
													t9 = t11;
												}
												tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												T2 t12 = tutils.CircumCenter(t5, t6, t9);
												for (int m = 0; m < this.positions.Length; m++)
												{
													int num7 = m;
													tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
													nativeArray7[num7] = tutils.distancesq(t12, this.positions[m]);
												}
												nativeArray.Sort(new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.DelaunayTriangulationStep.DistComparer(nativeArray7));
												this.hullStart = num2;
												this.hullNext[num2] = (this.hullPrev[num4] = num3);
												this.hullNext[num3] = (this.hullPrev[num2] = num4);
												this.hullNext[num4] = (this.hullPrev[num3] = num2);
												this.hullTri[num2] = 0;
												this.hullTri[num3] = 1;
												this.hullTri[num4] = 2;
												tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												this.hullHash[tutils.hashkey(t5, t12, this.hashSize)] = num2;
												tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												this.hullHash[tutils.hashkey(t6, t12, this.hashSize)] = num3;
												tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												this.hullHash[tutils.hashkey(t9, t12, this.hashSize)] = num4;
												this.AddTriangle(num2, num3, num4, -1, -1, -1);
												for (int n = 0; n < nativeArray.Length; n++)
												{
													int num8 = nativeArray[n];
													if (num8 != num2 && num8 != num3 && num8 != num4)
													{
														T2 t13 = this.positions[num8];
														int num9 = 0;
														tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
														int num10 = tutils.hashkey(t13, t12, this.hashSize);
														for (int num11 = 0; num11 < this.hashSize; num11++)
														{
															num9 = this.hullHash[(num10 + num11) % this.hashSize];
															if (num9 != -1 && num9 != this.hullNext[num9])
															{
																break;
															}
														}
														num9 = this.hullPrev[num9];
														int num12 = num9;
														int num13 = this.hullNext[num12];
														for (;;)
														{
															tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															TBig tbig7 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t13, this.positions[num12], this.positions[num13]);
															tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															if (tutils.less(tbig7, tutils2.ZeroTBig()))
															{
																break;
															}
															num12 = num13;
															if (num12 == num9)
															{
																goto Block_31;
															}
															num13 = this.hullNext[num12];
														}
														IL_06D5:
														if (num12 != 2147483647)
														{
															int num14 = this.AddTriangle(num12, num8, this.hullNext[num12], -1, -1, this.hullTri[num12]);
															this.hullTri[num8] = this.Legalize(num14 + 2);
															this.hullTri[num12] = num14;
															int num15 = this.hullNext[num12];
															num13 = this.hullNext[num15];
															for (;;)
															{
																tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																TBig tbig8 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t13, this.positions[num15], this.positions[num13]);
																tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																if (!tutils.less(tbig8, tutils2.ZeroTBig()))
																{
																	break;
																}
																num14 = this.AddTriangle(num15, num8, num13, this.hullTri[num8], -1, this.hullTri[num15]);
																this.hullTri[num8] = this.Legalize(num14 + 2);
																this.hullNext[num15] = num15;
																num15 = num13;
																num13 = this.hullNext[num15];
															}
															if (num12 == num9)
															{
																num13 = this.hullPrev[num12];
																for (;;)
																{
																	tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																	TBig tbig9 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t13, this.positions[num13], this.positions[num12]);
																	tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																	if (!tutils.less(tbig9, tutils2.ZeroTBig()))
																	{
																		break;
																	}
																	num14 = this.AddTriangle(num13, num8, num12, -1, this.hullTri[num12], this.hullTri[num13]);
																	this.Legalize(num14 + 2);
																	this.hullTri[num13] = num14;
																	this.hullNext[num12] = num12;
																	num12 = num13;
																	num13 = this.hullPrev[num12];
																}
															}
															this.hullStart = (this.hullPrev[num8] = num12);
															this.hullNext[num12] = (this.hullPrev[num15] = num8);
															this.hullNext[num8] = num15;
															tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															this.hullHash[tutils.hashkey(t13, t12, this.hashSize)] = num8;
															tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															this.hullHash[tutils.hashkey(this.positions[num12], t12, this.hashSize)] = num12;
															goto IL_0977;
														}
														goto IL_0977;
														Block_31:
														num12 = int.MaxValue;
														goto IL_06D5;
													}
													IL_0977:;
												}
												this.triangles.Length = this.trianglesLen;
												this.halfedges.Length = this.trianglesLen;
												this.constrainedHalfedges.Length = this.trianglesLen;
												nativeArray.Dispose();
												nativeArray7.Dispose();
												return;
											}
										}
									}
								}
							}
						}
					}
					this.status.Value = Status.DegenerateInput;
					nativeArray.Dispose();
				}
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00005034 File Offset: 0x00003234
			private int Legalize(int a)
			{
				int num = 0;
				int num4;
				for (;;)
				{
					int num2 = this.halfedges[a];
					int num3 = a - a % 3;
					num4 = num3 + (a + 2) % 3;
					if (num2 == -1)
					{
						if (num == 0)
						{
							break;
						}
						a = this.EDGE_STACK[--num];
					}
					else
					{
						int num5 = num2 - num2 % 3;
						int num6 = num3 + (a + 1) % 3;
						int num7 = num5 + (num2 + 2) % 3;
						int num8 = this.triangles[num4];
						int num9 = this.triangles[a];
						int num10 = this.triangles[num6];
						int num11 = this.triangles[num7];
						TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (utils.InCircle(this.positions[num8], this.positions[num9], this.positions[num10], this.positions[num11]))
						{
							this.triangles[a] = num11;
							this.triangles[num2] = num8;
							int num12 = this.halfedges[num7];
							if (num12 == -1)
							{
								int num13 = this.hullStart;
								while (this.hullTri[num13] != num7)
								{
									num13 = this.hullPrev[num13];
									if (num13 == this.hullStart)
									{
										goto IL_014E;
									}
								}
								this.hullTri[num13] = a;
							}
							IL_014E:
							this.Link(a, num12);
							this.Link(num2, this.halfedges[num4]);
							this.Link(num4, num7);
							int num14 = num5 + (num2 + 1) % 3;
							if (num < this.EDGE_STACK.Length)
							{
								this.EDGE_STACK[num++] = num14;
							}
						}
						else
						{
							if (num == 0)
							{
								break;
							}
							a = this.EDGE_STACK[--num];
						}
					}
				}
				return num4;
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00005204 File Offset: 0x00003404
			private int AddTriangle(int i0, int i1, int i2, int a, int b, int c)
			{
				int num = this.trianglesLen;
				this.triangles[num] = i0;
				this.triangles[num + 1] = i1;
				this.triangles[num + 2] = i2;
				this.Link(num, a);
				this.Link(num + 1, b);
				this.Link(num + 2, c);
				this.trianglesLen += 3;
				return num;
			}

			// Token: 0x0600009C RID: 156 RVA: 0x00005271 File Offset: 0x00003471
			private void Link(int a, int b)
			{
				this.halfedges[a] = b;
				if (b != -1)
				{
					this.halfedges[b] = a;
				}
			}

			// Token: 0x04000074 RID: 116
			private NativeReference<Status> status;

			// Token: 0x04000075 RID: 117
			private NativeArray<T2>.ReadOnly positions;

			// Token: 0x04000076 RID: 118
			private NativeList<int> triangles;

			// Token: 0x04000077 RID: 119
			private NativeList<int> halfedges;

			// Token: 0x04000078 RID: 120
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x04000079 RID: 121
			private NativeArray<int> hullNext;

			// Token: 0x0400007A RID: 122
			private NativeArray<int> hullPrev;

			// Token: 0x0400007B RID: 123
			private NativeArray<int> hullTri;

			// Token: 0x0400007C RID: 124
			private NativeArray<int> hullHash;

			// Token: 0x0400007D RID: 125
			private NativeArray<int> EDGE_STACK;

			// Token: 0x0400007E RID: 126
			private readonly int hashSize;

			// Token: 0x0400007F RID: 127
			private readonly bool verbose;

			// Token: 0x04000080 RID: 128
			private int hullStart;

			// Token: 0x04000081 RID: 129
			private int trianglesLen;

			// Token: 0x02000020 RID: 32
			private struct DistComparer : IComparer<int>
			{
				// Token: 0x0600009D RID: 157 RVA: 0x00005291 File Offset: 0x00003491
				public DistComparer(NativeArray<TBig> dist)
				{
					this.dist = dist;
				}

				// Token: 0x0600009E RID: 158 RVA: 0x0000529C File Offset: 0x0000349C
				public int Compare(int x, int y)
				{
					TBig tbig = this.dist[x];
					return tbig.CompareTo(this.dist[y]);
				}

				// Token: 0x04000082 RID: 130
				private NativeArray<TBig> dist;
			}
		}

		// Token: 0x02000021 RID: 33
		private struct ConstrainEdgesStep
		{
			// Token: 0x0600009F RID: 159 RVA: 0x000052D0 File Offset: 0x000034D0
			public ConstrainEdgesStep(InputData<T2> input, OutputData<T2> output, Args args)
			{
				this.status = output.Status;
				this.positions = output.Positions.AsArray().AsReadOnly();
				this.triangles = output.Triangles.AsArray();
				this.inputConstraintEdges = input.ConstraintEdges.AsReadOnly();
				this.inputConstraintEdgeTypes = input.ConstraintEdgeTypes.AsReadOnly();
				this.halfedges = output.Halfedges;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.args = args;
				this.intersections = default(NativeList<int>);
				this.unresolvedIntersections = default(NativeList<int>);
				this.pointToHalfedge = default(NativeArray<int>);
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x0000537C File Offset: 0x0000357C
			public void Execute(Allocator allocator)
			{
				if (!this.inputConstraintEdges.IsCreated || this.status.Value.IsError)
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.ConstrainEdgesStep.Auto())
				{
					using (this.intersections = new NativeList<int>(allocator))
					{
						using (this.unresolvedIntersections = new NativeList<int>(allocator))
						{
							using (this.pointToHalfedge = new NativeArray<int>(this.positions.Length, allocator, NativeArrayOptions.ClearMemory))
							{
								for (int i = 0; i < this.triangles.Length; i++)
								{
									this.pointToHalfedge[this.triangles[i]] = i;
								}
								for (int j = 0; j < this.inputConstraintEdges.Length / 2; j++)
								{
									int2 @int = math.int2(this.inputConstraintEdges[2 * j], this.inputConstraintEdges[2 * j + 1]);
									@int = ((@int.x < @int.y) ? @int.xy : @int.yx);
									ConstraintType constraintType = (this.inputConstraintEdgeTypes.IsCreated ? this.inputConstraintEdgeTypes[j] : ConstraintType.ConstrainedAndHoleBoundary);
									this.TryApplyConstraint(@int, (constraintType == ConstraintType.Constrained) ? HalfedgeState.Constrained : HalfedgeState.ConstrainedAndHoleBoundary);
								}
							}
						}
					}
				}
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x00005574 File Offset: 0x00003774
			private void TryResolveIntersections(int2 c, HalfedgeState constrainValue, ref int iter)
			{
				for (int i = 0; i < this.intersections.Length; i++)
				{
					int num = iter;
					iter = num + 1;
					if (this.IsMaxItersExceeded(num, this.args.SloanMaxIters))
					{
						return;
					}
					int num2 = this.intersections[i];
					int num3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num2);
					int num4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num3);
					int num5 = this.halfedges[num2];
					int num6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
					int num7 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
					int num8 = this.triangles[num2];
					int num9 = this.triangles[num3];
					int num10 = this.triangles[num4];
					int num11 = this.triangles[num7];
					T2 t = this.positions[num8];
					T2 t2 = this.positions[num11];
					T2 t3 = this.positions[num9];
					T2 t4 = this.positions[num10];
					T2 t5 = t;
					T2 t6 = t2;
					T2 t7 = t3;
					T2 t8 = t4;
					if (!UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.IsConvexQuadrilateral(t5, t6, t7, t8))
					{
						this.unresolvedIntersections.Add(in num2);
					}
					else
					{
						this.triangles[num2] = num11;
						this.triangles[num5] = num10;
						this.pointToHalfedge[num11] = num2;
						this.pointToHalfedge[num10] = num5;
						this.pointToHalfedge[num8] = num6;
						this.pointToHalfedge[num9] = num3;
						this.ReplaceHalfedge(num7, num2);
						this.ReplaceHalfedge(num4, num5);
						this.halfedges[num4] = num7;
						this.halfedges[num7] = num4;
						this.constrainedHalfedges[num4] = HalfedgeState.Unconstrained;
						this.constrainedHalfedges[num7] = HalfedgeState.Unconstrained;
						for (int j = i + 1; j < this.intersections.Length; j++)
						{
							int num12 = this.intersections[j];
							this.intersections[j] = ((num12 == num4) ? num5 : ((num12 == num7) ? num2 : num12));
						}
						for (int k = 0; k < this.unresolvedIntersections.Length; k++)
						{
							int num13 = this.unresolvedIntersections[k];
							this.unresolvedIntersections[k] = ((num13 == num4) ? num5 : ((num13 == num7) ? num2 : num13));
						}
						int2 @int = math.int2(num10, num11);
						if (math.all(c.xy == @int.xy) || math.all(c.xy == @int.yx))
						{
							this.constrainedHalfedges[num4] = constrainValue;
							this.constrainedHalfedges[num7] = constrainValue;
						}
						if (this.EdgeEdgeIntersection(c, @int))
						{
							this.unresolvedIntersections.Add(in num4);
						}
					}
				}
				this.intersections.Clear();
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x00005850 File Offset: 0x00003A50
			private void ReplaceHalfedge(int h0, int h1)
			{
				int num = this.halfedges[h0];
				this.halfedges[h1] = num;
				this.constrainedHalfedges[h1] = this.constrainedHalfedges[h0];
				if (num != -1)
				{
					this.halfedges[num] = h1;
					this.constrainedHalfedges[num] = this.constrainedHalfedges[h0];
				}
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x000058B8 File Offset: 0x00003AB8
			private bool EdgeEdgeIntersection(int2 e1, int2 e2)
			{
				T2 t = this.positions[e1.x];
				T2 t2 = this.positions[e1.y];
				T2 t3 = t;
				T2 t4 = t2;
				t = this.positions[e2.x];
				T2 t5 = this.positions[e2.y];
				T2 t6 = t;
				T2 t7 = t5;
				return !math.any((e1.xy == e2.xy) | (e1.xy == e2.yx)) && UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.EdgeEdgeIntersection(t3, t4, t6, t7);
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00005954 File Offset: 0x00003B54
			private void MarkHalfedgeConstrained(int halfedge, HalfedgeState constrainValue)
			{
				constrainValue = (HalfedgeState)math.max((int)this.constrainedHalfedges[halfedge], (int)constrainValue);
				this.constrainedHalfedges[halfedge] = constrainValue;
				int num = this.halfedges[halfedge];
				if (num != -1)
				{
					this.constrainedHalfedges[num] = constrainValue;
				}
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x000059A4 File Offset: 0x00003BA4
			private void TryApplyConstraint(int2 edge, HalfedgeState constrainValue)
			{
				this.intersections.Clear();
				this.unresolvedIntersections.Clear();
				int num = -1;
				int x = edge.x;
				int y = edge.y;
				int num2 = x;
				int num3 = y;
				int num4 = this.pointToHalfedge[num2];
				int num5 = num4;
				int num6;
				int num7;
				for (;;)
				{
					num6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
					if (this.triangles[num6] == num3)
					{
						break;
					}
					num7 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
					if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num6]], this.positions[num2], this.positions[num3]))
					{
						goto Block_2;
					}
					if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num7]], this.positions[num2], this.positions[num3]) && this.triangles[num7] != num3)
					{
						goto Block_4;
					}
					if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num6], this.triangles[num7])))
					{
						goto Block_5;
					}
					num5 = this.halfedges[num7];
					if (num5 == -1)
					{
						goto Block_6;
					}
					if (num5 == num4)
					{
						goto IL_01A2;
					}
				}
				this.MarkHalfedgeConstrained(num5, constrainValue);
				goto IL_01A2;
				Block_2:
				this.MarkHalfedgeConstrained(num5, constrainValue);
				num3 = this.triangles[num6];
				goto IL_01A2;
				Block_4:
				this.MarkHalfedgeConstrained(num7, constrainValue);
				num3 = this.triangles[num7];
				goto IL_01A2;
				Block_5:
				this.unresolvedIntersections.Add(in num6);
				num = this.halfedges[num6];
				goto IL_01A2;
				Block_6:
				if (this.triangles[num7] == num3)
				{
					this.MarkHalfedgeConstrained(num7, constrainValue);
				}
				IL_01A2:
				num5 = this.halfedges[num4];
				if (num == -1 && num5 != -1)
				{
					num5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
					int num8;
					int num9;
					for (;;)
					{
						num8 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
						if (this.triangles[num8] == num3)
						{
							break;
						}
						num9 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num8);
						if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num8]], this.positions[num2], this.positions[num3]))
						{
							goto Block_11;
						}
						if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num9]], this.positions[num2], this.positions[num3]) && this.triangles[num9] != num3)
						{
							goto Block_13;
						}
						if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num8], this.triangles[num9])))
						{
							goto Block_14;
						}
						num5 = this.halfedges[num5];
						if (num5 == -1)
						{
							goto IL_0431;
						}
						num5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
						if (num5 == num4)
						{
							goto Block_16;
						}
					}
					this.MarkHalfedgeConstrained(num5, constrainValue);
					goto IL_0431;
					Block_11:
					this.MarkHalfedgeConstrained(num5, constrainValue);
					num3 = this.triangles[num8];
					goto IL_0431;
					Block_13:
					this.MarkHalfedgeConstrained(num9, constrainValue);
					num3 = this.triangles[num9];
					goto IL_0431;
					Block_14:
					this.unresolvedIntersections.Add(in num8);
					num = this.halfedges[num8];
					Block_16:;
				}
				IL_0431:
				while (num != -1)
				{
					int num10 = num;
					num = -1;
					int num11 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num10);
					int num12 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num11);
					if (this.triangles[num12] == num3)
					{
						break;
					}
					if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num12]], this.positions[num2], this.positions[num3]))
					{
						num3 = this.triangles[num12];
						break;
					}
					if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num11], this.triangles[num12])))
					{
						this.unresolvedIntersections.Add(in num11);
						num = this.halfedges[num11];
					}
					else if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num12], this.triangles[num10])))
					{
						this.unresolvedIntersections.Add(in num12);
						num = this.halfedges[num12];
					}
				}
				int num13 = 0;
				while (!this.status.Value.IsError)
				{
					NativeList<int> nativeList = this.unresolvedIntersections;
					NativeList<int> nativeList2 = this.intersections;
					this.intersections = nativeList;
					this.unresolvedIntersections = nativeList2;
					this.TryResolveIntersections(new int2(num2, num3), constrainValue, ref num13);
					if (this.unresolvedIntersections.IsEmpty)
					{
						if (edge.y != num3)
						{
							this.TryApplyConstraint(new int2(num3, edge.y), constrainValue);
						}
						return;
					}
				}
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00005E5C File Offset: 0x0000405C
			private bool IsMaxItersExceeded(int iter, int maxIters)
			{
				if (iter >= maxIters)
				{
					this.status.Value = Status.SloanMaxItersExceeded;
					return true;
				}
				return false;
			}

			// Token: 0x04000083 RID: 131
			private NativeReference<Status> status;

			// Token: 0x04000084 RID: 132
			private NativeArray<T2>.ReadOnly positions;

			// Token: 0x04000085 RID: 133
			private NativeArray<int> triangles;

			// Token: 0x04000086 RID: 134
			private NativeArray<int>.ReadOnly inputConstraintEdges;

			// Token: 0x04000087 RID: 135
			private NativeArray<ConstraintType>.ReadOnly inputConstraintEdgeTypes;

			// Token: 0x04000088 RID: 136
			private NativeList<int> halfedges;

			// Token: 0x04000089 RID: 137
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x0400008A RID: 138
			private readonly Args args;

			// Token: 0x0400008B RID: 139
			private NativeList<int> intersections;

			// Token: 0x0400008C RID: 140
			private NativeList<int> unresolvedIntersections;

			// Token: 0x0400008D RID: 141
			private NativeArray<int> pointToHalfedge;
		}

		// Token: 0x02000022 RID: 34
		private struct PlantingSeedStep
		{
			// Token: 0x060000A7 RID: 167 RVA: 0x00005E75 File Offset: 0x00004075
			public PlantingSeedStep(InputData<T2> input, OutputData<T2> output, Args args)
			{
				this = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep(output, args, input.HoleSeeds);
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00005E88 File Offset: 0x00004088
			public PlantingSeedStep(OutputData<T2> output, Args args, NativeArray<T2> localHoles)
			{
				this.status = output.Status;
				this.triangles = output.Triangles;
				this.positions = output.Positions;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.halfedges = output.Halfedges;
				this.holes = localHoles;
				this.args = args;
				this.shouldRemoveTriangle = default(NativeArray<bool>);
				this.trianglesQueue = default(NativeQueue<int>);
				this.anyRemovedTriangles = false;
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00005F00 File Offset: 0x00004100
			public void Execute(Allocator allocator, bool constraintsIsCreated)
			{
				if (!constraintsIsCreated || (this.status.IsCreated && this.status.Value.IsError))
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.PlantingSeedStep.Auto())
				{
					using (this.shouldRemoveTriangle = new NativeArray<bool>(this.triangles.Length / 3, allocator, NativeArrayOptions.ClearMemory))
					{
						if (this.args.AutoHolesAndBoundary)
						{
							this.PlantAuto(allocator);
						}
						if (this.holes.IsCreated || this.args.RestoreBoundary)
						{
							this.trianglesQueue = new NativeQueue<int>(allocator);
							if (this.holes.IsCreated)
							{
								this.PlantHoleSeeds(this.holes);
							}
							if (this.args.RestoreBoundary)
							{
								this.PlantBoundarySeeds();
							}
							this.trianglesQueue.Dispose();
						}
						this.RemoveVisitedTriangles(allocator);
					}
				}
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00006018 File Offset: 0x00004218
			private void PlantBoundarySeeds()
			{
				for (int i = 0; i < this.halfedges.Length; i++)
				{
					if (this.halfedges[i] == -1 && !this.shouldRemoveTriangle[i / 3] && this.constrainedHalfedges[i] != HalfedgeState.ConstrainedAndHoleBoundary)
					{
						this.PlantSeed(i / 3);
					}
				}
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00006074 File Offset: 0x00004274
			private void PlantHoleSeeds(NativeArray<T2> holeSeeds)
			{
				foreach (T2 t in holeSeeds)
				{
					int num = this.FindTriangle(t);
					if (num != -1)
					{
						this.PlantSeed(num);
					}
				}
			}

			// Token: 0x060000AC RID: 172 RVA: 0x000060D0 File Offset: 0x000042D0
			private void RemoveVisitedTriangles(Allocator allocator)
			{
				if (!this.anyRemovedTriangles)
				{
					return;
				}
				UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<>c__DisplayClass15_0 CS$<>8__locals1;
				CS$<>8__locals1.indexRemap = new NativeArray<int>(this.triangles.Length / 3, allocator, NativeArrayOptions.ClearMemory);
				int num = 0;
				for (int i = 0; i < this.shouldRemoveTriangle.Length; i++)
				{
					int num2 = i;
					int num3;
					if (!this.shouldRemoveTriangle[i])
					{
						num = (num3 = num) + 1;
					}
					else
					{
						num3 = -1;
					}
					CS$<>8__locals1.indexRemap[num2] = num3;
				}
				NativeArray<bool3> nativeArray = this.constrainedHalfedges.AsArray().Reinterpret<bool3>(1);
				NativeArray<int3> nativeArray2 = this.triangles.AsArray().Reinterpret<int3>(4);
				for (int j = 0; j < CS$<>8__locals1.indexRemap.Length; j++)
				{
					int num4 = CS$<>8__locals1.indexRemap[j];
					if (num4 != -1)
					{
						nativeArray2[num4] = nativeArray2[j];
						nativeArray[num4] = nativeArray[j];
						this.halfedges[3 * num4] = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<RemoveVisitedTriangles>g__RemapHalfedge|15_0(this.halfedges[3 * j], ref CS$<>8__locals1);
						this.halfedges[3 * num4 + 1] = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<RemoveVisitedTriangles>g__RemapHalfedge|15_0(this.halfedges[3 * j + 1], ref CS$<>8__locals1);
						this.halfedges[3 * num4 + 2] = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<RemoveVisitedTriangles>g__RemapHalfedge|15_0(this.halfedges[3 * j + 2], ref CS$<>8__locals1);
					}
				}
				this.triangles.Length = 3 * num;
				this.constrainedHalfedges.Length = 3 * num;
				this.halfedges.Length = 3 * num;
				CS$<>8__locals1.indexRemap.Dispose();
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00006278 File Offset: 0x00004478
			private void PlantSeed(int tId)
			{
				NativeArray<bool> nativeArray = this.shouldRemoveTriangle;
				NativeQueue<int> nativeQueue = this.trianglesQueue;
				if (nativeArray[tId])
				{
					return;
				}
				nativeArray[tId] = true;
				nativeQueue.Enqueue(tId);
				this.anyRemovedTriangles = true;
				while (nativeQueue.TryDequeue(out tId))
				{
					for (int i = 0; i < 3; i++)
					{
						int num = 3 * tId + i;
						int num2 = this.halfedges[num];
						if (this.constrainedHalfedges[num] != HalfedgeState.ConstrainedAndHoleBoundary && num2 != -1)
						{
							int num3 = num2 / 3;
							if (!nativeArray[num3])
							{
								nativeArray[num3] = true;
								nativeQueue.Enqueue(num3);
							}
						}
					}
				}
			}

			// Token: 0x060000AE RID: 174 RVA: 0x0000631C File Offset: 0x0000451C
			private int FindTriangle(T2 p)
			{
				for (int i = 0; i < this.triangles.Length / 3; i++)
				{
					int num = this.triangles[3 * i];
					int num2 = this.triangles[3 * i + 1];
					int num3 = this.triangles[3 * i + 2];
					int num4 = num;
					int num5 = num2;
					int num6 = num3;
					T2 t = this.positions[num4];
					T2 t2 = this.positions[num5];
					T2 t3 = this.positions[num6];
					T2 t4 = t;
					T2 t5 = t2;
					T2 t6 = t3;
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (utils.PointInsideTriangle(p, t4, t5, t6))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060000AF RID: 175 RVA: 0x000063D4 File Offset: 0x000045D4
			private void PlantAuto(Allocator allocator)
			{
				int num = this.triangles.Length / 3;
				NativeQueue<int> nativeQueue = new NativeQueue<int>(allocator);
				NativeQueue<int> nativeQueue2 = new NativeQueue<int>(allocator);
				NativeArray<bool> nativeArray = new NativeArray<bool>(num, allocator, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						if (this.halfedges[3 * i + j] == -1)
						{
							((this.constrainedHalfedges[3 * i + j] == HalfedgeState.ConstrainedAndHoleBoundary) ? nativeQueue2 : nativeQueue).Enqueue(i);
							nativeArray[i] = true;
							break;
						}
					}
				}
				bool flag = false;
				bool flag2 = true;
				while (!nativeQueue.IsEmpty() || !nativeQueue2.IsEmpty())
				{
					int num2;
					while (nativeQueue.TryDequeue(out num2))
					{
						if (flag2)
						{
							this.shouldRemoveTriangle[num2] = true;
							flag = true;
						}
						for (int k = 0; k < 3; k++)
						{
							int num3 = 3 * num2 + k;
							int num4 = this.halfedges[num3];
							int num5 = num4 / 3;
							if (num4 != -1 && !nativeArray[num5])
							{
								((this.constrainedHalfedges[num3] == HalfedgeState.ConstrainedAndHoleBoundary) ? nativeQueue2 : nativeQueue).Enqueue(num5);
								nativeArray[num5] = true;
							}
						}
					}
					NativeQueue<int> nativeQueue3 = nativeQueue2;
					NativeQueue<int> nativeQueue4 = nativeQueue;
					nativeQueue = nativeQueue3;
					nativeQueue2 = nativeQueue4;
					flag2 = !flag2;
				}
				this.anyRemovedTriangles = flag;
				nativeQueue.Dispose();
				nativeQueue2.Dispose();
				nativeArray.Dispose();
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00006550 File Offset: 0x00004750
			[CompilerGenerated]
			internal static int <RemoveVisitedTriangles>g__RemapHalfedge|15_0(int he, ref UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<>c__DisplayClass15_0 A_1)
			{
				if (he == -1)
				{
					return -1;
				}
				int num = A_1.indexRemap[he / 3];
				if (num != -1)
				{
					return 3 * num + he % 3;
				}
				return -1;
			}

			// Token: 0x0400008E RID: 142
			private NativeReference<Status> status;

			// Token: 0x0400008F RID: 143
			private NativeList<int> triangles;

			// Token: 0x04000090 RID: 144
			[ReadOnly]
			private NativeList<T2> positions;

			// Token: 0x04000091 RID: 145
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x04000092 RID: 146
			private NativeList<int> halfedges;

			// Token: 0x04000093 RID: 147
			private NativeArray<bool> shouldRemoveTriangle;

			// Token: 0x04000094 RID: 148
			private NativeQueue<int> trianglesQueue;

			// Token: 0x04000095 RID: 149
			private NativeArray<T2> holes;

			// Token: 0x04000096 RID: 150
			private bool anyRemovedTriangles;

			// Token: 0x04000097 RID: 151
			private readonly Args args;
		}

		// Token: 0x02000024 RID: 36
		private struct RefineMeshStep
		{
			// Token: 0x060000B1 RID: 177 RVA: 0x00006580 File Offset: 0x00004780
			public RefineMeshStep(OutputData<T2> output, Args args, TTransform lt)
			{
				TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T t = tutils2.Const(2f);
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T t2 = tutils.Cast(utils.mul(utils2.Cast(utils3.mul(t, tutils2.Const(args.RefinementThresholdArea))), lt.AreaScalingFactor));
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				this = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep(output, t2, tutils.Const(args.RefinementThresholdAngle));
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x0000663C File Offset: 0x0000483C
			public RefineMeshStep(OutputData<T2> output, T area2Threshold, T angleThreshold)
			{
				this.status = output.Status;
				this.initialPointsCount = output.Positions.Length;
				this.maximumArea2 = area2Threshold;
				this.angleThreshold = angleThreshold;
				this.triangles = output.Triangles;
				this.outputPositions = output.Positions;
				this.halfedges = output.Halfedges;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.circles = default(NativeList<UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle>);
				this.trianglesQueue = default(NativeQueue<int>);
				this.badTriangles = default(NativeList<int>);
				this.pathPoints = default(NativeList<int>);
				this.pathHalfedges = default(NativeList<int>);
				this.visitedTriangles = default(NativeList<bool>);
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x000066F0 File Offset: 0x000048F0
			public void Execute(Allocator allocator, bool refineMesh, bool constrainBoundary)
			{
				if (!refineMesh || (this.status.IsCreated && this.status.Value.IsError))
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.RefineMeshStep.Auto())
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (!utils.SupportsRefinement())
					{
						this.status.Value = Status.IntegersDoNotSupportMeshRefinement;
					}
					else
					{
						if (constrainBoundary)
						{
							for (int i = 0; i < this.constrainedHalfedges.Length; i++)
							{
								this.constrainedHalfedges[i] = ((this.halfedges[i] == -1) ? HalfedgeState.ConstrainedAndHoleBoundary : HalfedgeState.Unconstrained);
							}
						}
						using (this.circles = new NativeList<UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle>(allocator)
						{
							Length = this.triangles.Length / 3
						})
						{
							using (this.trianglesQueue = new NativeQueue<int>(allocator))
							{
								using (this.badTriangles = new NativeList<int>(this.triangles.Length / 3, allocator))
								{
									using (this.pathPoints = new NativeList<int>(allocator))
									{
										using (this.pathHalfedges = new NativeList<int>(allocator))
										{
											using (this.visitedTriangles = new NativeList<bool>(this.triangles.Length / 3, allocator))
											{
												using (NativeList<int> nativeList6 = new NativeList<int>(this.triangles.Length, allocator))
												{
													using (NativeList<int> nativeList7 = new NativeList<int>(this.triangles.Length, allocator))
													{
														for (int j = 0; j < this.triangles.Length / 3; j++)
														{
															int num = this.triangles[3 * j];
															int num2 = this.triangles[3 * j + 1];
															int num3 = this.triangles[3 * j + 2];
															int num4 = num;
															int num5 = num2;
															int num6 = num3;
															this.circles[j] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(num4, num5, num6, this.outputPositions.AsArray()));
														}
														for (int k = 0; k < this.constrainedHalfedges.Length; k++)
														{
															if (this.constrainedHalfedges[k] >= HalfedgeState.Constrained && this.IsEncroached(k))
															{
																nativeList6.Add(in k);
															}
														}
														this.SplitEncroachedEdges(nativeList6, default(NativeList<int>));
														for (int l = 0; l < this.triangles.Length / 3; l++)
														{
															if (this.IsBadTriangle(l))
															{
																nativeList7.Add(in l);
															}
														}
														for (int m = 0; m < nativeList7.Length; m++)
														{
															int num7 = nativeList7[m];
															if (num7 != -1)
															{
																this.SplitTriangle(num7, nativeList6, nativeList7, allocator);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00006B1C File Offset: 0x00004D1C
			private void SplitEncroachedEdges(NativeList<int> heQueue, NativeList<int> tQueue)
			{
				for (int i = 0; i < heQueue.Length; i++)
				{
					int num = heQueue[i];
					if (num != -1)
					{
						this.SplitEdge(num, heQueue, tQueue);
					}
				}
				heQueue.Clear();
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x00006B58 File Offset: 0x00004D58
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool IsEncroached(int he0)
			{
				int num = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(he0);
				int num2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num);
				T2 t = this.outputPositions[this.triangles[he0]];
				T2 t2 = this.outputPositions[this.triangles[num]];
				T2 t3 = this.outputPositions[this.triangles[num2]];
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T2 t4 = tutils2.diff(t, t3);
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T t5 = tutils.dot(t4, tutils2.diff(t2, t3));
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				return utils.le(t5, tutils.Zero());
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00006C28 File Offset: 0x00004E28
			private void SplitEdge(int he, NativeList<int> heQueue, NativeList<int> tQueue)
			{
				int num = this.triangles[he];
				int num2 = this.triangles[UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(he)];
				int num3 = num;
				int num4 = num2;
				T2 t = this.outputPositions[num3];
				T2 t2 = this.outputPositions[num4];
				T2 t3 = t;
				T2 t4 = t2;
				T2 t5;
				if ((num3 < this.initialPointsCount && num4 < this.initialPointsCount) || (num3 >= this.initialPointsCount && num4 >= this.initialPointsCount))
				{
					TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					t5 = tutils.avg(t3, t4);
				}
				else
				{
					TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T t6 = tutils2.Const(0.001f);
					tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T t7 = tutils.alpha(t6, tutils2.Cast(utils.distancesq(t3, t4)));
					T2 t8;
					if (num3 >= this.initialPointsCount)
					{
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t8 = tutils.lerp(t4, t3, t7);
					}
					else
					{
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t8 = tutils.lerp(t3, t4, t7);
					}
					t5 = t8;
				}
				this.constrainedHalfedges[he] = HalfedgeState.Unconstrained;
				int num5 = this.halfedges[he];
				if (num5 != -1)
				{
					this.constrainedHalfedges[num5] = HalfedgeState.Unconstrained;
				}
				if (this.halfedges[he] != -1)
				{
					this.UnsafeInsertPointBulk(t5, he / 3, heQueue, tQueue);
					int num6 = this.triangles.Length - 3;
					int num7 = -1;
					int num8 = -1;
					while (num7 == -1 || num8 == -1)
					{
						int num9 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
						if (this.triangles[num9] == num3)
						{
							num7 = num6;
						}
						if (this.triangles[num9] == num4)
						{
							num8 = num6;
						}
						int num10 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num9);
						num6 = this.halfedges[num10];
					}
					if (this.IsEncroached(num7))
					{
						heQueue.Add(in num7);
					}
					int num11 = this.halfedges[num7];
					if (this.IsEncroached(num11))
					{
						heQueue.Add(in num11);
					}
					if (this.IsEncroached(num8))
					{
						heQueue.Add(in num8);
					}
					int num12 = this.halfedges[num8];
					if (this.IsEncroached(num12))
					{
						heQueue.Add(in num12);
					}
					this.constrainedHalfedges[num7] = HalfedgeState.Constrained;
					this.constrainedHalfedges[num11] = HalfedgeState.Constrained;
					this.constrainedHalfedges[num8] = HalfedgeState.Constrained;
					this.constrainedHalfedges[num12] = HalfedgeState.Constrained;
					return;
				}
				this.UnsafeInsertPointBoundary(t5, he, heQueue, tQueue);
				int num13 = 3 * (this.pathPoints.Length - 1);
				int num14 = this.halfedges.Length - 1;
				int num15 = this.halfedges.Length - num13;
				if (this.IsEncroached(num14))
				{
					heQueue.Add(in num14);
				}
				if (this.IsEncroached(num15))
				{
					heQueue.Add(in num15);
				}
				this.constrainedHalfedges[num14] = HalfedgeState.Constrained;
				this.constrainedHalfedges[num15] = HalfedgeState.Constrained;
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x00006F24 File Offset: 0x00005124
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool IsBadTriangle(int tId)
			{
				int num = this.triangles[3 * tId];
				int num2 = this.triangles[3 * tId + 1];
				int num3 = this.triangles[3 * tId + 2];
				int num4 = num;
				int num5 = num2;
				int num6 = num3;
				T2 t = this.outputPositions[num4];
				T2 t2 = this.outputPositions[num5];
				T2 t3 = this.outputPositions[num6];
				T2 t4 = t;
				T2 t5 = t2;
				T2 t6 = t3;
				T t7 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Area2(t4, t5, t6);
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				return utils.greater(t7, this.maximumArea2) || this.AngleIsTooSmall(tId, this.angleThreshold);
			}

			// Token: 0x060000B8 RID: 184 RVA: 0x00006FD4 File Offset: 0x000051D4
			private void SplitTriangle(int tId, NativeList<int> heQueue, NativeList<int> tQueue, Allocator allocator)
			{
				UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle circle = this.circles[tId];
				NativeList<int> nativeList = new NativeList<int>(allocator);
				for (int i = 0; i < this.constrainedHalfedges.Length; i++)
				{
					if (this.constrainedHalfedges[i] != HalfedgeState.Unconstrained)
					{
						int num = this.triangles[i];
						int num2 = this.triangles[UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(i)];
						int num3 = num;
						int num4 = num2;
						if (this.halfedges[i] == -1 || num3 < num4)
						{
							T2 t = this.outputPositions[num3];
							T2 t2 = this.outputPositions[num4];
							T2 t3 = t;
							T2 t4 = t2;
							TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							T2 t5 = tutils3.diff(t3, circle.Center);
							tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							T t6 = tutils2.dot(t5, tutils3.diff(t4, circle.Center));
							tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							if (tutils.le(t6, tutils2.Zero()))
							{
								nativeList.Add(in i);
							}
						}
					}
				}
				if (nativeList.IsEmpty)
				{
					this.UnsafeInsertPointBulk(circle.Center, tId, heQueue, tQueue);
				}
				else
				{
					int num = this.triangles[3 * tId];
					int num5 = this.triangles[3 * tId + 1];
					int num6 = this.triangles[3 * tId + 2];
					int num7 = num;
					int num8 = num5;
					int num9 = num6;
					T2 t = this.outputPositions[num7];
					T2 t7 = this.outputPositions[num8];
					T2 t8 = this.outputPositions[num9];
					T2 t9 = t;
					T2 t10 = t7;
					T2 t11 = t8;
					T t12 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Area2(t9, t10, t11);
					TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (tutils.greater(t12, this.maximumArea2))
					{
						foreach (int num10 in nativeList.AsArray().AsReadOnly())
						{
							heQueue.Add(in num10);
						}
					}
					if (!heQueue.IsEmpty)
					{
						tQueue.Add(in tId);
						this.SplitEncroachedEdges(heQueue, tQueue);
					}
				}
				nativeList.Dispose();
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00007238 File Offset: 0x00005438
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool AngleIsTooSmall(int tId, T minimumAngle)
			{
				int num = this.triangles[3 * tId];
				int num2 = this.triangles[3 * tId + 1];
				int num3 = this.triangles[3 * tId + 2];
				int num4 = num;
				int num5 = num2;
				int num6 = num3;
				T2 t = this.outputPositions[num4];
				T2 t2 = this.outputPositions[num5];
				T2 t3 = this.outputPositions[num6];
				T2 t4 = t;
				T2 t5 = t2;
				T2 t6 = t3;
				return UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.AngleIsTooSmall(t4, t5, t6, minimumAngle);
			}

			// Token: 0x060000BA RID: 186 RVA: 0x000072BC File Offset: 0x000054BC
			private int UnsafeInsertPointCommon(T2 p, int initTriangle)
			{
				int length = this.outputPositions.Length;
				this.outputPositions.Add(in p);
				this.badTriangles.Clear();
				this.trianglesQueue.Clear();
				this.pathPoints.Clear();
				this.pathHalfedges.Clear();
				this.visitedTriangles.Clear();
				this.visitedTriangles.Length = this.triangles.Length / 3;
				this.trianglesQueue.Enqueue(initTriangle);
				this.badTriangles.Add(in initTriangle);
				this.visitedTriangles[initTriangle] = true;
				this.RecalculateBadTriangles(p);
				return length;
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00007360 File Offset: 0x00005560
			private void UnsafeInsertPointBulk(T2 p, int initTriangle, NativeList<int> heQueue = default(NativeList<int>), NativeList<int> tQueue = default(NativeList<int>))
			{
				int num = this.UnsafeInsertPointCommon(p, initTriangle);
				this.BuildStarPolygon();
				this.ProcessBadTriangles(heQueue, tQueue);
				this.BuildNewTrianglesForStar(num, heQueue, tQueue);
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00007390 File Offset: 0x00005590
			private void UnsafeInsertPointBoundary(T2 p, int initHe, NativeList<int> heQueue = default(NativeList<int>), NativeList<int> tQueue = default(NativeList<int>))
			{
				int num = this.UnsafeInsertPointCommon(p, initHe / 3);
				this.BuildAmphitheaterPolygon(initHe);
				this.ProcessBadTriangles(heQueue, tQueue);
				this.BuildNewTrianglesForAmphitheater(num, heQueue, tQueue);
			}

			// Token: 0x060000BD RID: 189 RVA: 0x000073C4 File Offset: 0x000055C4
			private void RecalculateBadTriangles(T2 p)
			{
				int num;
				while (this.trianglesQueue.TryDequeue(out num))
				{
					this.VisitEdge(p, 3 * num);
					this.VisitEdge(p, 3 * num + 1);
					this.VisitEdge(p, 3 * num + 2);
				}
			}

			// Token: 0x060000BE RID: 190 RVA: 0x00007404 File Offset: 0x00005604
			private void VisitEdge(T2 p, int t0)
			{
				int num = this.halfedges[t0];
				if (num == -1 || this.constrainedHalfedges[num] >= HalfedgeState.Constrained)
				{
					return;
				}
				int num2 = num / 3;
				if (this.visitedTriangles[num2])
				{
					return;
				}
				UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle circle = this.circles[num2];
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				if (utils.le(utils2.Cast(utils3.distancesq(circle.Center, p)), circle.RadiusSq))
				{
					this.badTriangles.Add(in num2);
					this.trianglesQueue.Enqueue(num2);
					this.visitedTriangles[num2] = true;
				}
			}

			// Token: 0x060000BF RID: 191 RVA: 0x000074C4 File Offset: 0x000056C4
			private void BuildAmphitheaterPolygon(int initHe)
			{
				int num = initHe;
				int num2 = this.triangles[num];
				int num4;
				for (;;)
				{
					num = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num);
					if (this.triangles[num] == num2)
					{
						break;
					}
					int num3 = this.halfedges[num];
					if (num3 == -1 || !this.badTriangles.Contains(num3 / 3))
					{
						num4 = this.triangles[num];
						this.pathPoints.Add(in num4);
						this.pathHalfedges.Add(in num3);
					}
					else
					{
						num = num3;
					}
				}
				num4 = this.triangles[initHe];
				this.pathPoints.Add(in num4);
				num4 = -1;
				this.pathHalfedges.Add(in num4);
			}

			// Token: 0x060000C0 RID: 192 RVA: 0x00007570 File Offset: 0x00005770
			private void BuildStarPolygon()
			{
				int num = -1;
				for (int i = 0; i < this.badTriangles.Length; i++)
				{
					int num2 = this.badTriangles[i];
					for (int j = 0; j < 3; j++)
					{
						int num3 = 3 * num2 + j;
						int num4 = this.halfedges[num3];
						if (num4 == -1 || !this.badTriangles.Contains(num4 / 3))
						{
							int num5 = this.triangles[num3];
							this.pathPoints.Add(in num5);
							this.pathHalfedges.Add(in num4);
							num = num3;
							break;
						}
					}
					if (num != -1)
					{
						break;
					}
				}
				int num6 = num;
				int num7 = this.pathPoints[0];
				for (;;)
				{
					num6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
					if (this.triangles[num6] == num7)
					{
						break;
					}
					int num8 = this.halfedges[num6];
					if (num8 == -1 || !this.badTriangles.Contains(num8 / 3))
					{
						int num5 = this.triangles[num6];
						this.pathPoints.Add(in num5);
						this.pathHalfedges.Add(in num8);
					}
					else
					{
						num6 = num8;
					}
				}
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x00007694 File Offset: 0x00005894
			private void ProcessBadTriangles(NativeList<int> heQueue, NativeList<int> tQueue)
			{
				this.badTriangles.Sort<int>();
				for (int i = this.badTriangles.Length - 1; i >= 0; i--)
				{
					int num = this.badTriangles[i];
					this.triangles.RemoveAt(3 * num + 2);
					this.triangles.RemoveAt(3 * num + 1);
					this.triangles.RemoveAt(3 * num);
					this.circles.RemoveAt(num);
					this.RemoveHalfedge(3 * num + 2, 0);
					this.RemoveHalfedge(3 * num + 1, 1);
					this.RemoveHalfedge(3 * num, 2);
					this.constrainedHalfedges.RemoveAt(3 * num + 2);
					this.constrainedHalfedges.RemoveAt(3 * num + 1);
					this.constrainedHalfedges.RemoveAt(3 * num);
					for (int j = 3 * num; j < this.halfedges.Length; j++)
					{
						int num2 = this.halfedges[j];
						if (num2 != -1)
						{
							ref NativeList<int> ptr = ref this.halfedges;
							int num3 = ((num2 < 3 * num) ? num2 : j);
							ptr[num3] -= 3;
						}
					}
					for (int k = 0; k < this.pathHalfedges.Length; k++)
					{
						if (this.pathHalfedges[k] > 3 * num + 2)
						{
							ref NativeList<int> ptr = ref this.pathHalfedges;
							int num3 = k;
							ptr[num3] -= 3;
						}
					}
					if (heQueue.IsCreated)
					{
						for (int l = 0; l < heQueue.Length; l++)
						{
							int num4 = heQueue[l];
							if (num4 == 3 * num || num4 == 3 * num + 1 || num4 == 3 * num + 2)
							{
								heQueue[l] = -1;
							}
							else if (num4 > 3 * num + 2)
							{
								ref NativeList<int> ptr = ref heQueue;
								int num3 = l;
								ptr[num3] -= 3;
							}
						}
					}
					if (tQueue.IsCreated)
					{
						for (int m = 0; m < tQueue.Length; m++)
						{
							int num5 = tQueue[m];
							if (num5 == num)
							{
								tQueue[m] = -1;
							}
							else if (num5 > num)
							{
								int num3 = m;
								int num6 = tQueue[num3];
								tQueue[num3] = num6 - 1;
							}
						}
					}
				}
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x000078D0 File Offset: 0x00005AD0
			private void RemoveHalfedge(int he, int offset)
			{
				int num = this.halfedges[he];
				int num2 = ((num > he) ? (num - offset) : num);
				if (num2 > -1)
				{
					this.halfedges[num2] = -1;
				}
				this.halfedges.RemoveAt(he);
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x00007914 File Offset: 0x00005B14
			private void BuildNewTrianglesForStar(int pId, NativeList<int> heQueue, NativeList<int> tQueue)
			{
				int length = this.triangles.Length;
				this.triangles.Length = this.triangles.Length + 3 * this.pathPoints.Length;
				this.circles.Length = this.circles.Length + this.pathPoints.Length;
				for (int i = 0; i < this.pathPoints.Length - 1; i++)
				{
					this.triangles[length + 3 * i] = pId;
					this.triangles[length + 3 * i + 1] = this.pathPoints[i];
					this.triangles[length + 3 * i + 2] = this.pathPoints[i + 1];
					this.circles[length / 3 + i] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(pId, this.pathPoints[i], this.pathPoints[i + 1], this.outputPositions.AsArray()));
				}
				this.triangles[this.triangles.Length - 3] = pId;
				this.triangles[this.triangles.Length - 2] = this.pathPoints[this.pathPoints.Length - 1];
				this.triangles[this.triangles.Length - 1] = this.pathPoints[0];
				this.circles[this.circles.Length - 1] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(pId, this.pathPoints[this.pathPoints.Length - 1], this.pathPoints[0], this.outputPositions.AsArray()));
				int length2 = this.halfedges.Length;
				this.halfedges.Length = this.halfedges.Length + 3 * this.pathPoints.Length;
				this.constrainedHalfedges.Length = this.constrainedHalfedges.Length + 3 * this.pathPoints.Length;
				for (int j = 0; j < this.pathPoints.Length - 1; j++)
				{
					int num = this.pathHalfedges[j];
					this.halfedges[3 * j + 1 + length2] = num;
					if (num != -1)
					{
						this.halfedges[num] = 3 * j + 1 + length2;
						this.constrainedHalfedges[3 * j + 1 + length2] = this.constrainedHalfedges[num];
					}
					else
					{
						this.constrainedHalfedges[3 * j + 1 + length2] = HalfedgeState.Constrained;
					}
					this.halfedges[3 * j + 2 + length2] = 3 * j + 3 + length2;
					this.halfedges[3 * j + 3 + length2] = 3 * j + 2 + length2;
				}
				int num2 = this.pathHalfedges[this.pathHalfedges.Length - 1];
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 1) + 1] = num2;
				if (num2 != -1)
				{
					this.halfedges[num2] = length2 + 3 * (this.pathPoints.Length - 1) + 1;
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 1) + 1] = this.constrainedHalfedges[num2];
				}
				else
				{
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 1) + 1] = HalfedgeState.Constrained;
				}
				this.halfedges[length2] = length2 + 3 * (this.pathPoints.Length - 1) + 2;
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 1) + 2] = length2;
				if (heQueue.IsCreated)
				{
					for (int k = 0; k < this.pathPoints.Length - 1; k++)
					{
						int num3 = length2 + 3 * k + 1;
						if (this.constrainedHalfedges[num3] >= HalfedgeState.Constrained && this.IsEncroached(num3))
						{
							heQueue.Add(in num3);
						}
						else if (tQueue.IsCreated && this.IsBadTriangle(num3 / 3))
						{
							int num4 = num3 / 3;
							tQueue.Add(in num4);
						}
					}
				}
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00007D24 File Offset: 0x00005F24
			private void BuildNewTrianglesForAmphitheater(int pId, NativeList<int> heQueue, NativeList<int> tQueue)
			{
				int length = this.triangles.Length;
				this.triangles.Length = this.triangles.Length + 3 * (this.pathPoints.Length - 1);
				this.circles.Length = this.circles.Length + (this.pathPoints.Length - 1);
				for (int i = 0; i < this.pathPoints.Length - 1; i++)
				{
					this.triangles[length + 3 * i] = pId;
					this.triangles[length + 3 * i + 1] = this.pathPoints[i];
					this.triangles[length + 3 * i + 2] = this.pathPoints[i + 1];
					this.circles[length / 3 + i] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(pId, this.pathPoints[i], this.pathPoints[i + 1], this.outputPositions.AsArray()));
				}
				int length2 = this.halfedges.Length;
				this.halfedges.Length = this.halfedges.Length + 3 * (this.pathPoints.Length - 1);
				this.constrainedHalfedges.Length = this.constrainedHalfedges.Length + 3 * (this.pathPoints.Length - 1);
				for (int j = 0; j < this.pathPoints.Length - 2; j++)
				{
					int num = this.pathHalfedges[j];
					this.halfedges[3 * j + 1 + length2] = num;
					if (num != -1)
					{
						this.halfedges[num] = 3 * j + 1 + length2;
						this.constrainedHalfedges[3 * j + 1 + length2] = this.constrainedHalfedges[num];
					}
					else
					{
						this.constrainedHalfedges[3 * j + 1 + length2] = HalfedgeState.Constrained;
					}
					this.halfedges[3 * j + 2 + length2] = 3 * j + 3 + length2;
					this.halfedges[3 * j + 3 + length2] = 3 * j + 2 + length2;
				}
				int num2 = this.pathHalfedges[this.pathHalfedges.Length - 2];
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 2) + 1] = num2;
				if (num2 != -1)
				{
					this.halfedges[num2] = length2 + 3 * (this.pathPoints.Length - 2) + 1;
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 2) + 1] = this.constrainedHalfedges[num2];
				}
				else
				{
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 2) + 1] = HalfedgeState.Constrained;
				}
				this.halfedges[length2] = -1;
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 2) + 2] = -1;
				if (heQueue.IsCreated)
				{
					for (int k = 0; k < this.pathPoints.Length - 1; k++)
					{
						int num3 = length2 + 3 * k + 1;
						if (this.constrainedHalfedges[num3] >= HalfedgeState.Constrained && this.IsEncroached(num3))
						{
							heQueue.Add(in num3);
						}
						else if (tQueue.IsCreated && this.IsBadTriangle(num3 / 3))
						{
							int num4 = num3 / 3;
							tQueue.Add(in num4);
						}
					}
				}
			}

			// Token: 0x04000099 RID: 153
			private NativeReference<Status> status;

			// Token: 0x0400009A RID: 154
			private NativeList<int> triangles;

			// Token: 0x0400009B RID: 155
			private NativeList<T2> outputPositions;

			// Token: 0x0400009C RID: 156
			private NativeList<int> halfedges;

			// Token: 0x0400009D RID: 157
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x0400009E RID: 158
			private NativeList<UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle> circles;

			// Token: 0x0400009F RID: 159
			private NativeQueue<int> trianglesQueue;

			// Token: 0x040000A0 RID: 160
			private NativeList<int> badTriangles;

			// Token: 0x040000A1 RID: 161
			private NativeList<int> pathPoints;

			// Token: 0x040000A2 RID: 162
			private NativeList<int> pathHalfedges;

			// Token: 0x040000A3 RID: 163
			private NativeList<bool> visitedTriangles;

			// Token: 0x040000A4 RID: 164
			private readonly T maximumArea2;

			// Token: 0x040000A5 RID: 165
			private readonly T angleThreshold;

			// Token: 0x040000A6 RID: 166
			private readonly int initialPointsCount;

			// Token: 0x040000A7 RID: 167
			private const float ConcentricShellReferenceRadius = 0.001f;

			// Token: 0x02000025 RID: 37
			private readonly struct Circle
			{
				// Token: 0x060000C5 RID: 197 RVA: 0x0000808C File Offset: 0x0000628C
				public Circle([TupleElementNames(new string[] { "center", "radiusSq" })] ValueTuple<T2, T> circle)
				{
					T2 item = circle.Item1;
					T item2 = circle.Item2;
					this.Center = item;
					this.RadiusSq = item2;
				}

				// Token: 0x040000A8 RID: 168
				public readonly T2 Center;

				// Token: 0x040000A9 RID: 169
				public readonly T RadiusSq;
			}
		}
	}
}
