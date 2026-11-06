using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E9 RID: 489
	[BurstCompile(CompileSynchronously = true)]
	public struct JobVoxelize : IJob
	{
		// Token: 0x06000D8A RID: 3466 RVA: 0x0004FB54 File Offset: 0x0004DD54
		public unsafe void Execute()
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.graphSpaceBounds.min, Quaternion.identity, Vector3.one) * Matrix4x4.Scale(new Vector3(this.cellSize, this.cellHeight, this.cellSize));
			Matrix4x4 inverse = (this.graphTransform * matrix4x * Matrix4x4.Translate(new Vector3(0.5f, 0f, 0.5f))).inverse;
			float num = math.cos(math.atan(this.cellSize / this.cellHeight * math.tan(this.maxSlope * 0.017453292f)));
			VoxelPolygonClipper voxelPolygonClipper = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper2 = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper3 = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper4 = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper5 = default(VoxelPolygonClipper);
			int num2 = 0;
			for (int i = 0; i < this.bucket.Length; i++)
			{
				num2 = math.max(this.inputMeshes[this.bucket[i]].vertices.Length, num2);
			}
			NativeArray<float3> nativeArray = new NativeArray<float3>(num2, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			int width = this.voxelArea.width;
			int depth = this.voxelArea.depth;
			int num3 = Mathf.Min(width - 1, float.IsPositiveInfinity(this.graphSpaceLimits.x) ? int.MaxValue : Mathf.CeilToInt((this.graphSpaceLimits.x - this.graphSpaceBounds.min.x) / this.cellSize));
			int num4 = Mathf.Min(depth - 1, float.IsPositiveInfinity(this.graphSpaceLimits.y) ? int.MaxValue : Mathf.CeilToInt((this.graphSpaceLimits.y - this.graphSpaceBounds.min.z) / this.cellSize));
			for (int j = 0; j < this.bucket.Length; j++)
			{
				RasterizationMesh rasterizationMesh = this.inputMeshes[this.bucket[j]];
				bool flag = VectorMath.ReversesFaceOrientations(rasterizationMesh.matrix);
				UnsafeSpan<float3> vertices = rasterizationMesh.vertices;
				UnsafeSpan<int> triangles = rasterizationMesh.triangles;
				float4x4 float4x = inverse * rasterizationMesh.matrix;
				for (int k = 0; k < vertices.Length; k++)
				{
					nativeArray[k] = math.transform(float4x, *vertices[k]);
				}
				int area = rasterizationMesh.area;
				UnsafeSpan<int> areas = rasterizationMesh.areas;
				IntRect intRect = default(IntRect);
				int l = 0;
				int num5 = 0;
				while (l < triangles.Length)
				{
					float3 @float = nativeArray[*triangles[l]];
					float3 float2 = nativeArray[*triangles[l + 1]];
					float3 float3 = nativeArray[*triangles[l + 2]];
					if (flag)
					{
						float3 float4 = @float;
						@float = float3;
						float3 = float4;
					}
					int num6 = (int)math.min(math.min(@float.x, float2.x), float3.x);
					int num7 = (int)math.min(math.min(@float.z, float2.z), float3.z);
					int num8 = (int)math.ceil(math.max(math.max(@float.x, float2.x), float3.x));
					int num9 = (int)math.ceil(math.max(math.max(@float.z, float2.z), float3.z));
					if (num6 <= num3 && num7 <= num4 && num8 >= 0 && num9 >= 0)
					{
						num6 = math.clamp(num6, 0, num3);
						num8 = math.clamp(num8, 0, num3);
						num7 = math.clamp(num7, 0, num4);
						num9 = math.clamp(num9, num4, num4);
						if (l == 0)
						{
							intRect = new IntRect(num6, num7, num6, num7);
						}
						intRect.xmin = math.min(intRect.xmin, num6);
						intRect.xmax = math.max(intRect.xmax, num8);
						intRect.ymin = math.min(intRect.ymin, num7);
						intRect.ymax = math.max(intRect.ymax, num9);
						float num10 = math.normalizesafe(math.cross(float2 - @float, float3 - @float), default(float3)).y;
						if (rasterizationMesh.doubleSided)
						{
							num10 = math.abs(num10);
						}
						int num11 = ((areas.Length > 0) ? (1 + *areas[num5]) : (1 + area));
						if (num10 < num)
						{
							num11 = 0;
						}
						voxelPolygonClipper[0] = @float;
						voxelPolygonClipper[1] = float2;
						voxelPolygonClipper[2] = float3;
						voxelPolygonClipper.n = 3;
						for (int m = num6; m <= num8; m++)
						{
							voxelPolygonClipper.ClipPolygonAlongX(ref voxelPolygonClipper2, 1f, (float)(-(float)m) + 0.5f);
							if (voxelPolygonClipper2.n >= 3)
							{
								voxelPolygonClipper2.ClipPolygonAlongX(ref voxelPolygonClipper3, -1f, (float)m + 0.5f);
								if (voxelPolygonClipper3.n >= 3)
								{
									float num13;
									float num12 = (num13 = voxelPolygonClipper3.z.FixedElementField);
									for (int n = 1; n < voxelPolygonClipper3.n; n++)
									{
										float num14 = *((ref voxelPolygonClipper3.z.FixedElementField) + (IntPtr)n * 4);
										num13 = math.min(num13, num14);
										num12 = math.max(num12, num14);
									}
									int num15 = math.clamp((int)math.round(num13), 0, num3);
									int num16 = math.clamp((int)math.round(num12), 0, num4);
									for (int num17 = num15; num17 <= num16; num17++)
									{
										voxelPolygonClipper3.ClipPolygonAlongZWithYZ(ref voxelPolygonClipper4, 1f, (float)(-(float)num17) + 0.5f);
										if (voxelPolygonClipper4.n >= 3)
										{
											voxelPolygonClipper4.ClipPolygonAlongZWithY(ref voxelPolygonClipper5, -1f, (float)num17 + 0.5f);
											if (voxelPolygonClipper5.n >= 3)
											{
												if (rasterizationMesh.flatten)
												{
													this.voxelArea.AddFlattenedSpan(num17 * width + m, num11);
												}
												else
												{
													float num19;
													float num18 = (num19 = voxelPolygonClipper5.y.FixedElementField);
													for (int num20 = 1; num20 < voxelPolygonClipper5.n; num20++)
													{
														float num21 = *((ref voxelPolygonClipper5.y.FixedElementField) + (IntPtr)num20 * 4);
														num19 = math.min(num19, num21);
														num18 = math.max(num18, num21);
													}
													int num22 = (int)math.ceil(num18);
													int num23 = (int)num19;
													num22 = math.max(num23 + 1, num22);
													this.voxelArea.AddLinkedSpan(num17 * width + m, num23, num22, num11, this.voxelWalkableClimb, j);
												}
											}
										}
									}
								}
							}
						}
					}
					l += 3;
					num5++;
				}
				if (rasterizationMesh.solid)
				{
					for (int num24 = intRect.ymin; num24 <= intRect.ymax; num24++)
					{
						for (int num25 = intRect.xmin; num25 <= intRect.xmax; num25++)
						{
							this.voxelArea.ResolveSolid(num24 * this.voxelArea.width + num25, j, this.voxelWalkableClimb);
						}
					}
				}
			}
		}

		// Token: 0x0400092D RID: 2349
		[ReadOnly]
		public NativeArray<RasterizationMesh> inputMeshes;

		// Token: 0x0400092E RID: 2350
		[ReadOnly]
		public NativeArray<int> bucket;

		// Token: 0x0400092F RID: 2351
		public int voxelWalkableClimb;

		// Token: 0x04000930 RID: 2352
		public uint voxelWalkableHeight;

		// Token: 0x04000931 RID: 2353
		public float cellSize;

		// Token: 0x04000932 RID: 2354
		public float cellHeight;

		// Token: 0x04000933 RID: 2355
		public float maxSlope;

		// Token: 0x04000934 RID: 2356
		public Matrix4x4 graphTransform;

		// Token: 0x04000935 RID: 2357
		public Bounds graphSpaceBounds;

		// Token: 0x04000936 RID: 2358
		public Vector2 graphSpaceLimits;

		// Token: 0x04000937 RID: 2359
		public LinkedVoxelField voxelArea;
	}
}
