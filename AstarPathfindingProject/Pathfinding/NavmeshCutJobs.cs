using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x02000131 RID: 305
	[BurstCompile]
	internal static class NavmeshCutJobs
	{
		// Token: 0x06000A0D RID: 2573 RVA: 0x0003564F File Offset: 0x0003384F
		[BurstCompile(FloatPrecision.Standard, FloatMode.Fast)]
		public static void CalculateContour(ref NavmeshCutJobs.JobCalculateContour job)
		{
			NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.Invoke(ref job);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00035657 File Offset: 0x00033857
		private static float ApproximateCircleWithPolylineRadius(float radius, int resolution)
		{
			return radius / (1f - (1f - math.cos(3.1415927f / (float)resolution)) * 0.5f);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0003567C File Offset: 0x0003387C
		public unsafe static void CapsuleConvexHullXZ(float4x4 matrix, UnsafeList<float2>* points, float height, float radius, float radiusMargin, int circleResolution, out int numPoints, out float minY, out float maxY)
		{
			height = math.max(height, radius * 2f);
			int num = circleResolution / 2;
			radius = NavmeshCutJobs.ApproximateCircleWithPolylineRadius(radius, num * 2);
			float num2 = math.length(matrix.c0.xyz);
			float num3 = math.length(matrix.c2.xyz);
			radius *= math.max(num2, num3);
			float3 @float = math.normalizesafe(matrix.c1.xyz, default(float3));
			float3 float2 = math.transform(matrix, new float3(0f, -height * 0.5f, 0f)) + @float * radius;
			float3 float3 = math.transform(matrix, new float3(0f, height * 0.5f, 0f)) - @float * radius;
			float2 xz = float2.xz;
			float2 xz2 = float3.xz;
			bool flag = false;
			float2 float4;
			if (math.lengthsq(xz - xz2) < 0.005f)
			{
				float4 = new float2(1f, 0f);
				flag = true;
			}
			else
			{
				float4 = math.normalize(xz2 - xz);
			}
			float2 float5 = new float2(-float4.y, float4.x);
			radius += radiusMargin;
			float4 *= radius;
			float5 *= radius;
			minY = math.min(float2.y, float3.y) - radius;
			maxY = math.max(float2.y, float3.y) + radius;
			float num4 = 3.1415927f / (float)num;
			if (flag)
			{
				numPoints = num * 2;
				int length = points->Length;
				points->Resize(points->Length + numPoints, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < num; i++)
				{
					float num5;
					float num6;
					math.sincos((float)i * num4, out num5, out num6);
					float2 float6 = num5 * float4 + num6 * float5;
					float2 float7 = xz - float6;
					float2 float8 = xz2 + float6;
					*points->ElementAt(length + i) = float7;
					*points->ElementAt(length + i + num) = float8;
				}
				return;
			}
			numPoints = (num + 1) * 2;
			int length2 = points->Length;
			points->Resize(points->Length + numPoints, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < num + 1; j++)
			{
				float num7;
				float num8;
				math.sincos((float)j * num4, out num7, out num8);
				float2 float9 = num7 * float4 + num8 * float5;
				float2 float10 = xz - float9;
				float2 float11 = xz2 + float9;
				*points->ElementAt(length2 + j) = float10;
				*points->ElementAt(length2 + j + num + 1) = float11;
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00035938 File Offset: 0x00033B38
		public unsafe static void BoxConvexHullXZ(float4x4 matrix, UnsafeList<float2>* points, out int numPoints, out float minY, out float maxY)
		{
			minY = float.PositiveInfinity;
			maxY = float.NegativeInfinity;
			int length = points->Length;
			points->Resize(points->Length + NavmeshCutJobs.BoxCorners.Length, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < NavmeshCutJobs.BoxCorners.Length; i++)
			{
				float4 @float = math.mul(matrix, NavmeshCutJobs.BoxCorners[i]);
				minY = math.min(minY, @float.y);
				maxY = math.max(maxY, @float.y);
				*points->ElementAt(length + i) = @float.xz;
			}
			numPoints = NavmeshCutJobs.ConvexHull(points->Ptr + length, NavmeshCutJobs.BoxCorners.Length, 0.01f);
			points->Length = length + numPoints;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x000359F8 File Offset: 0x00033BF8
		public unsafe static int ConvexHull(float2* points, int nPoints, float vertexMergeDistance)
		{
			int num = 0;
			for (int i = 0; i < nPoints; i++)
			{
				if (points[i].x < points[num].x || (points[i].x == points[num].x && points[i].y < points[num].y))
				{
					num = i;
				}
			}
			NativeSortExtension.Sort<float2, NavmeshCutJobs.AngleComparator>(points, nPoints, new NavmeshCutJobs.AngleComparator
			{
				origin = points[num]
			});
			int j = 0;
			for (int k = 0; k < nPoints; k++)
			{
				float2 @float = points[k];
				while (j >= 2)
				{
					float2 float2 = points[j - 1] - @float;
					float2 float3 = points[j - 2] - @float;
					if (float2.x * float3.y - float2.y * float3.x < 0f && math.lengthsq(float2) >= vertexMergeDistance)
					{
						break;
					}
					j--;
				}
				if (j == 1 && math.lengthsq(points[j - 1] - @float) < vertexMergeDistance)
				{
					j--;
				}
				points[j] = @float;
				j++;
			}
			return j;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00035C98 File Offset: 0x00033E98
		[BurstCompile(FloatPrecision.Standard, FloatMode.Fast)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CalculateContour$BurstManaged(ref NavmeshCutJobs.JobCalculateContour job)
		{
			job.Execute();
		}

		// Token: 0x040006AA RID: 1706
		private static readonly float4[] BoxCorners = new float4[]
		{
			new float4(-0.5f, -0.5f, -0.5f, 1f),
			new float4(0.5f, -0.5f, -0.5f, 1f),
			new float4(-0.5f, 0.5f, -0.5f, 1f),
			new float4(0.5f, 0.5f, -0.5f, 1f),
			new float4(-0.5f, -0.5f, 0.5f, 1f),
			new float4(0.5f, -0.5f, 0.5f, 1f),
			new float4(-0.5f, 0.5f, 0.5f, 1f),
			new float4(0.5f, 0.5f, 0.5f, 1f)
		};

		// Token: 0x02000132 RID: 306
		public struct JobCalculateContour
		{
			// Token: 0x06000A14 RID: 2580 RVA: 0x00035CA0 File Offset: 0x00033EA0
			public unsafe void Execute()
			{
				this.circleResolution = math.max(this.circleResolution, 3);
				float4x4 float4x = math.mul(this.matrix, this.localToWorldMatrix);
				float num = math.length(float4x.c0);
				float num2 = math.length(float4x.c1);
				float num3 = math.length(float4x.c2);
				switch (this.meshType)
				{
				case NavmeshCut.MeshType.Rectangle:
				{
					this.rectangleSize = new float2(math.abs(this.rectangleSize.x), math.abs(this.rectangleSize.y)) + math.rcp(new float2(num, num3)) * this.radiusMargin * 2f;
					ref UnsafeList<float2> ptr = ref *this.outputVertices;
					float2 @float = math.transform(float4x, new float3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f).xz;
					ptr.Add(in @float);
					ref UnsafeList<float2> ptr2 = ref *this.outputVertices;
					@float = math.transform(float4x, new float3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f).xz;
					ptr2.Add(in @float);
					ref UnsafeList<float2> ptr3 = ref *this.outputVertices;
					@float = math.transform(float4x, new float3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f).xz;
					ptr3.Add(in @float);
					ref UnsafeList<float2> ptr4 = ref *this.outputVertices;
					@float = math.transform(float4x, new float3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f).xz;
					ptr4.Add(in @float);
					float y = float4x.c3.y;
					ref UnsafeList<NavmeshCut.ContourBurst> ptr5 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = y - this.height * 0.5f * num2;
					contourBurst.ymax = y + this.height * 0.5f * num2;
					contourBurst.startIndex = this.outputVertices->Length - 4;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr5.Add(in contourBurst);
					break;
				}
				case NavmeshCut.MeshType.Circle:
				{
					this.circleRadius = math.abs(this.circleRadius);
					float num4 = this.height + this.radiusMargin / num2;
					float num5 = this.circleRadius + this.radiusMargin / num;
					float num6 = this.circleRadius + this.radiusMargin / num3;
					float num7 = 6.2831855f / (float)this.circleResolution;
					for (int i = 0; i < this.circleResolution; i++)
					{
						float num8;
						float num9;
						math.sincos((float)i * num7, out num8, out num9);
						ref UnsafeList<float2> ptr6 = ref *this.outputVertices;
						float2 @float = math.transform(float4x, new float3(num9 * num5, 0f, num8 * num6)).xz;
						ptr6.Add(in @float);
					}
					float y2 = float4x.c3.y;
					ref UnsafeList<NavmeshCut.ContourBurst> ptr7 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = y2 - num4 * 0.5f * num2;
					contourBurst.ymax = y2 + num4 * 0.5f * num2;
					contourBurst.startIndex = this.outputVertices->Length - this.circleResolution;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr7.Add(in contourBurst);
					break;
				}
				case NavmeshCut.MeshType.CustomMesh:
					if (this.meshContours != null && this.meshContourVertices != null && this.meshScale > 0f)
					{
						float4x = math.mul(float4x, float4x4.Scale(new float3(this.meshScale)));
						int length = this.outputVertices->Length;
						for (int j = 0; j < this.meshContourVertices->Length; j++)
						{
							ref UnsafeList<float2> ptr8 = ref *this.outputVertices;
							float2 @float = math.transform(float4x, *this.meshContourVertices->ElementAt(j)).xz;
							ptr8.Add(in @float);
						}
						float y3 = float4x.c3.y;
						for (int k = 0; k < this.meshContours->Length; k++)
						{
							ref UnsafeList<NavmeshCut.ContourBurst> ptr9 = ref *this.outputContours;
							NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
							contourBurst.ymin = y3 - this.height * 0.5f * num2;
							contourBurst.ymax = y3 + this.height * 0.5f * num2;
							contourBurst.startIndex = length + this.meshContours->ElementAt(k).startIndex;
							contourBurst.endIndex = length + this.meshContours->ElementAt(k).endIndex;
							ptr9.Add(in contourBurst);
						}
					}
					break;
				case NavmeshCut.MeshType.Box:
				{
					float3 float2 = new float3(this.rectangleSize.x, this.height, this.rectangleSize.y) + math.rcp(new float3(num, num2, num3)) * this.radiusMargin * 2f;
					float4x = math.mul(float4x, float4x4.Scale(float2));
					int num10;
					float num11;
					float num12;
					NavmeshCutJobs.BoxConvexHullXZ(float4x, this.outputVertices, out num10, out num11, out num12);
					ref UnsafeList<NavmeshCut.ContourBurst> ptr10 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = num11;
					contourBurst.ymax = num12;
					contourBurst.startIndex = this.outputVertices->Length - num10;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr10.Add(in contourBurst);
					break;
				}
				case NavmeshCut.MeshType.Sphere:
				{
					this.circleRadius = math.abs(this.circleRadius);
					float4x = math.mul(this.matrix, float4x4.Translate(this.localToWorldMatrix.c3.xyz));
					float num13 = math.max(num, math.max(num2, num3));
					float4x = math.mul(float4x, float4x4.Scale(num13));
					float num14 = this.circleRadius + this.radiusMargin / num13;
					num14 = NavmeshCutJobs.ApproximateCircleWithPolylineRadius(num14, this.circleResolution);
					float num15 = 6.2831855f / (float)this.circleResolution;
					for (int l = 0; l < this.circleResolution; l++)
					{
						float num16;
						float num17;
						math.sincos((float)l * num15, out num16, out num17);
						ref UnsafeList<float2> ptr11 = ref *this.outputVertices;
						float2 @float = math.transform(float4x, new float3(num17 * num14, 0f, num16 * num14)).xz;
						ptr11.Add(in @float);
					}
					float y4 = float4x.c3.y;
					ref UnsafeList<NavmeshCut.ContourBurst> ptr12 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = y4 - num14 * num13;
					contourBurst.ymax = y4 + num14 * num13;
					contourBurst.startIndex = this.outputVertices->Length - this.circleResolution;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr12.Add(in contourBurst);
					break;
				}
				case NavmeshCut.MeshType.Capsule:
				{
					this.circleResolution = math.max(this.circleResolution, 6);
					float num18 = this.circleRadius;
					float num19 = this.height;
					num19 *= num2;
					float4x = math.mul(float4x, float4x4.Scale(new float3(1f, 1f / num2, 1f)));
					int num20;
					float num21;
					float num22;
					NavmeshCutJobs.CapsuleConvexHullXZ(float4x, this.outputVertices, num19, num18, this.radiusMargin, this.circleResolution, out num20, out num21, out num22);
					ref UnsafeList<NavmeshCut.ContourBurst> ptr13 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = num21;
					contourBurst.ymax = num22;
					contourBurst.startIndex = this.outputVertices->Length - num20;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr13.Add(in contourBurst);
					break;
				}
				}
				for (int m = 0; m < this.outputContours->Length; m++)
				{
					NavmeshCut.ContourBurst contourBurst2 = *this.outputContours->ElementAt(m);
					this.WindCounterClockwise(this.outputVertices, contourBurst2.startIndex, contourBurst2.endIndex);
				}
			}

			// Token: 0x06000A15 RID: 2581 RVA: 0x00036488 File Offset: 0x00034688
			private unsafe void WindCounterClockwise(UnsafeList<float2>* vertices, int startIndex, int endIndex)
			{
				int num = 0;
				float2 @float = new float2(float.PositiveInfinity, float.PositiveInfinity);
				for (int i = startIndex; i < endIndex; i++)
				{
					float2 float2 = *vertices->ElementAt(i);
					if (float2.x < @float.x || (float2.x == @float.x && float2.y < @float.y))
					{
						num = i;
						@float = float2;
					}
				}
				int num2 = endIndex - startIndex;
				float2 float3 = (*vertices)[(num - 1 - startIndex + num2) % num2 + startIndex];
				float2 float4 = @float;
				float2 float5 = (*vertices)[(num + 1 - startIndex) % num2 + startIndex];
				if ((float4.x - float3.x) * (float5.y - float3.y) - (float5.x - float3.x) * (float4.y - float3.y) > 0f)
				{
					int j = startIndex;
					int num3 = endIndex - 1;
					while (j < num3)
					{
						float2 float6 = *vertices->ElementAt(j);
						*vertices->ElementAt(j) = *vertices->ElementAt(num3);
						*vertices->ElementAt(num3) = float6;
						j++;
						num3--;
					}
				}
			}

			// Token: 0x040006AB RID: 1707
			public unsafe UnsafeList<float2>* outputVertices;

			// Token: 0x040006AC RID: 1708
			public unsafe UnsafeList<NavmeshCut.ContourBurst>* outputContours;

			// Token: 0x040006AD RID: 1709
			public unsafe UnsafeList<NavmeshCut.ContourBurst>* meshContours;

			// Token: 0x040006AE RID: 1710
			public unsafe UnsafeList<float3>* meshContourVertices;

			// Token: 0x040006AF RID: 1711
			public float4x4 matrix;

			// Token: 0x040006B0 RID: 1712
			public float4x4 localToWorldMatrix;

			// Token: 0x040006B1 RID: 1713
			public float radiusMargin;

			// Token: 0x040006B2 RID: 1714
			public int circleResolution;

			// Token: 0x040006B3 RID: 1715
			public float circleRadius;

			// Token: 0x040006B4 RID: 1716
			public float2 rectangleSize;

			// Token: 0x040006B5 RID: 1717
			public float height;

			// Token: 0x040006B6 RID: 1718
			public float meshScale;

			// Token: 0x040006B7 RID: 1719
			public NavmeshCut.MeshType meshType;
		}

		// Token: 0x02000133 RID: 307
		private struct AngleComparator : IComparer<float2>
		{
			// Token: 0x06000A16 RID: 2582 RVA: 0x000365BC File Offset: 0x000347BC
			public int Compare(float2 lhs, float2 rhs)
			{
				float2 @float = lhs - this.origin;
				float2 float2 = rhs - this.origin;
				float num = @float.x * float2.y - @float.y * float2.x;
				if (Hint.Unlikely(num == 0f))
				{
					float num2 = math.lengthsq(@float);
					float num3 = math.lengthsq(float2);
					if (num2 < num3)
					{
						return 1;
					}
					if (num2 > num3)
					{
						return -1;
					}
					if (@float.x < float2.x)
					{
						return 1;
					}
					if (@float.x > float2.x)
					{
						return -1;
					}
					if (@float.y < float2.y)
					{
						return 1;
					}
					if (@float.y > float2.y)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (num >= 0f)
					{
						return -1;
					}
					return 1;
				}
			}

			// Token: 0x040006B8 RID: 1720
			public float2 origin;
		}

		// Token: 0x02000134 RID: 308
		// (Invoke) Token: 0x06000A18 RID: 2584
		internal delegate void CalculateContour_0000099F$PostfixBurstDelegate(ref NavmeshCutJobs.JobCalculateContour job);

		// Token: 0x02000135 RID: 309
		internal static class CalculateContour_0000099F$BurstDirectCall
		{
			// Token: 0x06000A1B RID: 2587 RVA: 0x00036678 File Offset: 0x00034878
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.Pointer == 0)
				{
					NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.DeferredCompilation, methodof(NavmeshCutJobs.CalculateContour$BurstManaged(ref NavmeshCutJobs.JobCalculateContour)).MethodHandle, typeof(NavmeshCutJobs.CalculateContour_0000099F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.Pointer;
			}

			// Token: 0x06000A1C RID: 2588 RVA: 0x000366A4 File Offset: 0x000348A4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000A1D RID: 2589 RVA: 0x000366BC File Offset: 0x000348BC
			public static void Constructor()
			{
				NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(NavmeshCutJobs.CalculateContour(ref NavmeshCutJobs.JobCalculateContour)).MethodHandle);
			}

			// Token: 0x06000A1E RID: 2590 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000A1F RID: 2591 RVA: 0x000366CD File Offset: 0x000348CD
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateContour_0000099F$BurstDirectCall()
			{
				NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.Constructor();
			}

			// Token: 0x06000A20 RID: 2592 RVA: 0x000366D4 File Offset: 0x000348D4
			public static void Invoke(ref NavmeshCutJobs.JobCalculateContour job)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = NavmeshCutJobs.CalculateContour_0000099F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.NavmeshCutJobs/JobCalculateContour&), ref job, functionPointer);
						return;
					}
				}
				NavmeshCutJobs.CalculateContour$BurstManaged(ref job);
			}

			// Token: 0x040006B9 RID: 1721
			private static IntPtr Pointer;

			// Token: 0x040006BA RID: 1722
			private static IntPtr DeferredCompilation;
		}
	}
}
