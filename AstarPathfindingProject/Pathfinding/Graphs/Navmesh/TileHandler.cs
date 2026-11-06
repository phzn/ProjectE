using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using andywiecko.BurstTriangulator;
using andywiecko.BurstTriangulator.LowLevel.Unsafe;
using AOT;
using Pathfinding.Clipper2Lib;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Voxelization;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C4 RID: 452
	[BurstCompile]
	public static class TileHandler
	{
		// Token: 0x06000CD2 RID: 3282 RVA: 0x0004836C File Offset: 0x0004656C
		internal unsafe static TileHandler.CutCollection CollectCuts(GridLookup<NavmeshClipper> cuts, List<Vector2Int> tileCoordinates, float characterRadius, TileLayout tileLayout, ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags)
		{
			UnsafeList<float2> unsafeList = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> unsafeList2 = new UnsafeList<NavmeshCut.ContourBurst>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			UnsafeList<TileHandler.ContourMeta> unsafeList3 = new UnsafeList<TileHandler.ContourMeta>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			bool flag = false;
			UnsafeList<TileHandler.TileCuts> unsafeList4 = new UnsafeList<TileHandler.TileCuts>(tileCoordinates.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			Int3[] array = null;
			for (int i = 0; i < tileCoordinates.Count; i++)
			{
				Vector2Int vector2Int = tileCoordinates[i];
				Vector3 min = tileLayout.GetTileBoundsInGraphSpace(vector2Int.x, vector2Int.y, 1, 1).min;
				GraphTransform graphTransform = tileLayout.transform * Matrix4x4.Translate(min);
				Vector2Int vector2Int2 = tileCoordinates[i];
				List<NavmeshCut> list = cuts.QueryRect<NavmeshCut>(new IntRect(vector2Int2.x, vector2Int2.y, vector2Int2.x, vector2Int2.y));
				int length = unsafeList2.Length;
				flag |= list.Count > 0;
				for (int j = 0; j < list.Count; j++)
				{
					int length2 = unsafeList2.Length;
					NavmeshCut navmeshCut = list[j];
					navmeshCut.GetContourBurst(&unsafeList, &unsafeList2, graphTransform.inverseMatrix, characterRadius);
					TileHandler.ContourMeta contourMeta = new TileHandler.ContourMeta
					{
						isDual = navmeshCut.isDual,
						cutsAddedGeom = navmeshCut.cutsAddedGeom
					};
					unsafeList3.AddReplicate(in contourMeta, unsafeList2.Length - length2);
				}
				ListPool<NavmeshCut>.Release(ref list);
				List<NavmeshAdd> list2 = cuts.QueryRect<NavmeshAdd>(new IntRect(vector2Int2.x, vector2Int2.y, vector2Int2.x, vector2Int2.y));
				flag |= list2.Count > 0;
				for (int k = 0; k < list2.Count; k++)
				{
					int[] array2;
					int num;
					list2[k].GetMesh(ref array, out array2, out num, graphTransform);
					UnsafeSpan<Int3> unsafeSpan = new UnsafeSpan<Int3>(Allocator.Persistent, num);
					UnsafeSpan<int> unsafeSpan2 = new UnsafeSpan<int>(Allocator.Persistent, array2.Length);
					for (int l = 0; l < num; l++)
					{
						*unsafeSpan[l] = array[l];
					}
					for (int m = 0; m < array2.Length; m++)
					{
						*unsafeSpan2[m] = array2[m];
					}
					UnsafeSpan<int> unsafeSpan3 = new UnsafeSpan<int>(Allocator.Persistent, array2.Length / 3);
					unsafeSpan3.FillZeros<int>();
					tileVertices[i].Add(in unsafeSpan);
					tileTriangles[i].Add(in unsafeSpan2);
					tileTags[i].Add(in unsafeSpan3);
				}
				ListPool<NavmeshAdd>.Release(ref list2);
				unsafeList4.AddNoResize(new TileHandler.TileCuts
				{
					contourStartIndex = length,
					contourEndIndex = unsafeList2.Length
				});
			}
			UnsafeSpan<float2> unsafeSpan4 = unsafeList.AsUnsafeSpan<float2>();
			Vector2 tileWorldSize = tileLayout.TileWorldSize;
			UnsafeList<TileHandler.Point64Wrapper> unsafeList5;
			TileHandler.ConvertVerticesAndSnapToTileBoundaries(ref unsafeSpan4, out unsafeList5, ref tileWorldSize);
			ArrayPool<Int3>.Release(ref array, false);
			return new TileHandler.CutCollection
			{
				contourVertices = unsafeList5,
				contours = unsafeList2,
				contoursExtra = unsafeList3,
				tileCuts = unsafeList4,
				cuttingRequired = flag
			};
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00048678 File Offset: 0x00046878
		[BurstCompile]
		private static void ConvertVerticesAndSnapToTileBoundaries(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize)
		{
			TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.Invoke(ref contourVertices, out outputVertices, ref tileSize);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00048682 File Offset: 0x00046882
		[BurstCompile]
		internal static void CutTiles(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator)
		{
			TileHandler.CutTiles_00000BD7$BurstDirectCall.Invoke(ref tileVertices, ref tileTriangles, ref tileTags, ref tileSize, ref cutCollection, ref output, allocator);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00048694 File Offset: 0x00046894
		private unsafe static void ScaleUpCoordinates(UnsafeSpan<long> coords)
		{
			for (int i = 0; i < coords.Length; i++)
			{
				*coords[i] *= 16L;
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000486C4 File Offset: 0x000468C4
		private unsafe static void ScaleDownCoordinates(UnsafeSpan<int> coords)
		{
			for (int i = 0; i < coords.Length; i++)
			{
				*coords[i] /= 16;
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000486F4 File Offset: 0x000468F4
		private unsafe static void RemoveDegenerateSegments(ref UnsafeSpan<int2> polygon)
		{
			for (int i = 0; i < polygon.Length; i++)
			{
				int2 @int = *polygon[i];
				int2 int2 = *polygon[(i + 1) % polygon.Length];
				int2 int3 = *polygon[(i + 2) % polygon.Length];
				if (VectorMath.IsColinear(@int, int2, int3) && math.dot(int2 - @int, int3 - int2) < 0)
				{
					UnsafeSpan<int2>.RemoveAt(ref polygon, (i + 1) % polygon.Length);
					i--;
				}
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00048780 File Offset: 0x00046980
		private unsafe static void CollectCutsTouchingBounds(UnsafeSpan<IntBounds> cutBounds, NativeList<int> outputCutIndices, IntBounds bounds)
		{
			outputCutIndices.Clear();
			for (int i = 0; i < cutBounds.Length; i++)
			{
				if (IntBounds.Intersects(bounds, *cutBounds[i]))
				{
					outputCutIndices.AddNoResize(i);
				}
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x000487C4 File Offset: 0x000469C4
		private static IntBounds TriangleBounds(Int3 a, Int3 b, Int3 c)
		{
			int3 @int = (int3)a;
			int3 int2 = (int3)a;
			int3 int3 = math.min(math.min(@int, (int3)b), (int3)c);
			int2 = math.max(int2, (int3)b);
			int2 = math.max(int2, (int3)c);
			return new IntBounds(int3, int2 + 1);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0004881C File Offset: 0x00046A1C
		private static TileMesh.TileMeshUnsafe CompressAndRefineTile(NativeList<Int3> tileOutputVertices, NativeList<int> tileOutputTriangles, NativeList<int> tileOutputTags, Allocator allocator)
		{
			MeshUtility.JobMergeNearbyVertices jobMergeNearbyVertices = default(MeshUtility.JobMergeNearbyVertices);
			jobMergeNearbyVertices.vertices = tileOutputVertices;
			jobMergeNearbyVertices.triangles = tileOutputTriangles;
			jobMergeNearbyVertices.mergeRadiusSq = 8;
			jobMergeNearbyVertices.Execute();
			MeshUtility.JobRemoveDegenerateTriangles jobRemoveDegenerateTriangles = default(MeshUtility.JobRemoveDegenerateTriangles);
			jobRemoveDegenerateTriangles.vertices = tileOutputVertices;
			jobRemoveDegenerateTriangles.triangles = tileOutputTriangles;
			jobRemoveDegenerateTriangles.tags = tileOutputTags;
			jobRemoveDegenerateTriangles.verbose = false;
			jobRemoveDegenerateTriangles.Execute();
			int num = TileHandler.DelaunayRefinement(tileOutputVertices.AsUnsafeSpan<Int3>(), tileOutputTriangles.AsUnsafeSpan<int>(), tileOutputTags.AsUnsafeSpan<int>(), true, true);
			tileOutputTriangles.Length = num;
			tileOutputTags.Length = num / 3;
			return new TileMesh.TileMeshUnsafe
			{
				verticesInTileSpace = tileOutputVertices.AsUnsafeSpan<Int3>().Clone(allocator),
				triangles = tileOutputTriangles.AsUnsafeSpan<int>().Clone(allocator),
				tags = tileOutputTags.AsUnsafeSpan<int>().Reinterpret<uint>().Clone(allocator)
			};
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00048904 File Offset: 0x00046B04
		private static void CopyTriangulationToOutput(ref andywiecko.BurstTriangulator.LowLevel.Unsafe.OutputData<int2> triangulatorOutput, NativeList<Int3> tileOutputVertices, NativeList<int> tileOutputTriangles, NativeList<int> tileOutputTags, int tag, Int3 a, Int3 b, Int3 c)
		{
			int length = tileOutputVertices.Length;
			Polygon.BarycentricTriangleInterpolator barycentricTriangleInterpolator = new Polygon.BarycentricTriangleInterpolator(a, b, c);
			for (int i = 0; i < triangulatorOutput.Positions.Length; i++)
			{
				Int3 @int = new Int3(triangulatorOutput.Positions[i].x, 0, triangulatorOutput.Positions[i].y);
				@int.y = barycentricTriangleInterpolator.SampleY(new int2(@int.x, @int.z));
				tileOutputVertices.Add(in @int);
			}
			tileOutputTags.AddReplicate(in tag, triangulatorOutput.Triangles.Length / 3);
			for (int j = 0; j < triangulatorOutput.Triangles.Length; j += 3)
			{
				int num = triangulatorOutput.Triangles[j] + length;
				int num2 = triangulatorOutput.Triangles[j + 1] + length;
				int num3 = triangulatorOutput.Triangles[j + 2] + length;
				tileOutputTriangles.Add(in num);
				tileOutputTriangles.Add(in num2);
				tileOutputTriangles.Add(in num3);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00048A10 File Offset: 0x00046C10
		private unsafe static void SnapEdges(ref NativeArray<TileHandler.Point64Wrapper> triBuffer, ref int vertexCount, UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours, Vector2Int tileSize)
		{
			int i = 0;
			int num = vertexCount - 1;
			while (i < vertexCount)
			{
				int2 @int = new int2((int)triBuffer[num].x, (int)triBuffer[num].y);
				int2 int2 = new int2((int)triBuffer[i].x, (int)triBuffer[i].y);
				int2 int3 = int2 - @int;
				long num2 = (long)int3.x * (long)int3.x + (long)int3.y * (long)int3.y;
				long num3 = (long)math.sqrt((double)num2) * 2L;
				for (int j = 0; j < contours.Length; j++)
				{
					UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan = *contours[j];
					for (uint num4 = 0U; num4 < unsafeSpan.length; num4 += 1U)
					{
						int2 int4 = new int2((int)unsafeSpan[num4].x, (int)unsafeSpan[num4].y);
						if (math.abs(VectorMath.SignedTriangleAreaTimes2(@int, int2, int4)) <= num3)
						{
							int2 int5 = int4 - @int;
							long num5 = (long)int5.x * (long)int3.x + (long)int5.y * (long)int3.y;
							if (num5 > 0L && num5 < num2 && (@int.x != 0 || int2.x != 0 || int4.x == 0) && (@int.y != 0 || int2.y != 0 || int4.y == 0) && (@int.x != tileSize.x || int2.x != tileSize.x || int4.x == tileSize.x) && (@int.y != tileSize.y || int2.y != tileSize.y || int4.y == tileSize.y))
							{
								if (triBuffer.Length < vertexCount + 1)
								{
									NativeArray<TileHandler.Point64Wrapper> nativeArray = new NativeArray<TileHandler.Point64Wrapper>(vertexCount * 2, Allocator.Temp, NativeArrayOptions.ClearMemory);
									triBuffer.AsUnsafeSpan<TileHandler.Point64Wrapper>().CopyTo(nativeArray.AsUnsafeSpan<TileHandler.Point64Wrapper>());
									triBuffer.Dispose();
									triBuffer = nativeArray;
								}
								triBuffer.AsUnsafeSpan<TileHandler.Point64Wrapper>().Move(i, i + 1, vertexCount - i);
								triBuffer[i] = new TileHandler.Point64Wrapper((long)int4.x, (long)int4.y);
								vertexCount++;
								int2 = int4;
								int3 = int2 - @int;
								num2 = (long)int3.x * (long)int3.x + (long)int3.y * (long)int3.y;
								num3 = (long)math.sqrt((double)num2) * 2L;
							}
						}
					}
				}
				num = i;
				i++;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00048CD4 File Offset: 0x00046ED4
		private static NativeArray<IntBounds> CalculateCutBounds(ref TileHandler.CutCollection cutCollection, ref UnsafeList<TileHandler.Point64Wrapper> contourVerticesP64)
		{
			NativeArray<IntBounds> nativeArray = new NativeArray<IntBounds>(cutCollection.contours.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < cutCollection.contours.Length; i++)
			{
				NavmeshCut.ContourBurst contourBurst = cutCollection.contours[i];
				int2 @int = new int2(int.MaxValue, int.MaxValue);
				int2 int2 = new int2(int.MinValue, int.MinValue);
				for (int j = contourBurst.startIndex; j < contourBurst.endIndex; j++)
				{
					int2 int3 = new int2((int)contourVerticesP64[j].x, (int)contourVerticesP64[j].y);
					@int = math.min(@int, int3);
					int2 = math.max(int2, int3);
				}
				nativeArray[i] = new IntBounds(new int3(@int.x, (int)(contourBurst.ymin * 1000f), @int.y), new int3(int2.x + 1, (int)math.ceil(contourBurst.ymax * 1000f), int2.y + 1));
			}
			return nativeArray;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00048DE8 File Offset: 0x00046FE8
		private unsafe static void AddContours(Clipper64 clipper, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours)
		{
			for (int i = 0; i < contours.Length; i++)
			{
				clipper.AddPath((Point64*)contours[i].ptr, contours[i].Length, PathType.Clip, false);
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00048E28 File Offset: 0x00047028
		private static void CopyClipperOutput(List<List<Point64>> closedSolutions, ref UnsafeList<Vector2Int> outputVertices, ref UnsafeList<int> outputVertexCountPerPolygon)
		{
			outputVertexCountPerPolygon.Length = closedSolutions.Count;
			for (int i = 0; i < closedSolutions.Count; i++)
			{
				List<Point64> list = closedSolutions[i];
				for (int j = 0; j < list.Count; j++)
				{
					Vector2Int vector2Int = new Vector2Int((int)list[j].X, (int)list[j].Y);
					outputVertices.Add(in vector2Int);
				}
				outputVertexCountPerPolygon[i] = list.Count;
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00048EA0 File Offset: 0x000470A0
		[MonoPInvokeCallback(typeof(TileHandler.CutFunction))]
		private unsafe static bool CutPolygon(ref UnsafeSpan<TileHandler.Point64Wrapper> subject, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contoursDual, ref UnsafeList<Vector2Int> outputVertices, ref UnsafeList<int> outputVertexCountPerPolygon, int mode)
		{
			int threadIndex = JobsUtility.ThreadIndex;
			Clipper64 clipper = (TileHandlerCache.cachedClippers[threadIndex] = TileHandlerCache.cachedClippers[threadIndex] ?? new Clipper64());
			clipper.PreserveCollinear = true;
			List<List<Point64>> list = ListPool<List<Point64>>.Claim();
			List<List<Point64>> list2 = ListPool<List<Point64>>.Claim();
			if (mode != 1)
			{
				clipper.Clear();
				clipper.AddPath((Point64*)subject.ptr, subject.Length, PathType.Subject, false);
				TileHandler.AddContours(clipper, ref contours);
				TileHandler.AddContours(clipper, ref contoursDual);
			}
			else
			{
				clipper.Clear();
				clipper.AddPath((Point64*)subject.ptr, subject.Length, PathType.Subject, false);
				TileHandler.AddContours(clipper, ref contoursDual);
				if (!clipper.Execute(ClipType.Intersection, FillRule.NonZero, list, list2))
				{
					return false;
				}
				clipper.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					List<Point64> list3 = list[i];
					if (Clipper.IsPositive(list3))
					{
						clipper.AddSubject(list3);
					}
					else
					{
						clipper.AddClip(list3);
					}
				}
				TileHandler.AddContours(clipper, ref contours);
				list.Clear();
				list2.Clear();
			}
			if (!clipper.Execute(ClipType.Difference, FillRule.NonZero, list, list2))
			{
				return false;
			}
			TileHandler.CopyClipperOutput(list, ref outputVertices, ref outputVertexCountPerPolygon);
			ListPool<List<Point64>>.Release(ref list);
			ListPool<List<Point64>>.Release(ref list2);
			return true;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00048FC0 File Offset: 0x000471C0
		internal unsafe static void InitDelegates()
		{
			if (TileHandler.DelegateGCRoot == null)
			{
				TileHandler.CutFunction cutFunction = new TileHandler.CutFunction(TileHandler.CutPolygon);
				TileHandler.DelegateGCRoot = cutFunction;
				*TileHandler.CutFunctionPtr.Data = Marshal.GetFunctionPointerForDelegate<TileHandler.CutFunction>(cutFunction);
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00048FF8 File Offset: 0x000471F8
		private static int ClipAgainstRectangle(UnsafeSpan<Int3> clipIn, UnsafeSpan<Int3> clipTmp, Vector2Int size)
		{
			Int3PolygonClipper int3PolygonClipper = default(Int3PolygonClipper);
			int num = int3PolygonClipper.ClipPolygon(clipIn, 3, clipTmp, 1, 0, 0);
			if (num == 0)
			{
				return num;
			}
			num = int3PolygonClipper.ClipPolygon(clipTmp, num, clipIn, -1, size.x, 0);
			if (num == 0)
			{
				return num;
			}
			num = int3PolygonClipper.ClipPolygon(clipIn, num, clipTmp, 1, 0, 2);
			if (num == 0)
			{
				return num;
			}
			return int3PolygonClipper.ClipPolygon(clipTmp, num, clipIn, -1, size.y, 2);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00049064 File Offset: 0x00047264
		private unsafe static bool ClipAgainstHalfPlane(UnsafeSpan<TileHandler.Point64Wrapper> clipIn, NativeList<TileHandler.Point64Wrapper> clipOut, TileHandler.Point64Wrapper a, TileHandler.Point64Wrapper b)
		{
			if (clipIn.length == 0U)
			{
				return false;
			}
			bool flag = false;
			long num = TileHandler.<ClipAgainstHalfPlane>g__SignedDistanceToHalfPlane|41_0(a, b, *clipIn[clipIn.length - 1U]);
			uint num2 = 0U;
			uint num3 = clipIn.length - 1U;
			while (num2 < clipIn.length)
			{
				long num4 = TileHandler.<ClipAgainstHalfPlane>g__SignedDistanceToHalfPlane|41_0(a, b, *clipIn[num2]);
				if (num > 0L != num4 > 0L)
				{
					double num5 = (double)num / (double)(num - num4);
					long num6 = clipIn[num3].x + (long)math.round((double)(clipIn[num2].x - clipIn[num3].x) * num5);
					long num7 = clipIn[num3].y + (long)math.round((double)(clipIn[num2].y - clipIn[num3].y) * num5);
					TileHandler.Point64Wrapper point64Wrapper = new TileHandler.Point64Wrapper(num6, num7);
					clipOut.Add(in point64Wrapper);
				}
				if (num4 > 0L)
				{
					clipOut.Add(clipIn[num2]);
				}
				else
				{
					flag = true;
				}
				num = num4;
				num3 = num2;
				num2 += 1U;
			}
			return flag;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00049188 File Offset: 0x00047388
		private static void ClipAgainstHorizontalHalfPlane(ref UnsafeSpan<TileHandler.Point64Wrapper> contourVertices, NativeList<TileHandler.Point64Wrapper> scratchVertices, int h, Int3 a, Int3 b, Int3 c, bool preserveBelow)
		{
			Int3 @int = a;
			Int3 int2 = b;
			Int3 int3 = c;
			int i = 0;
			while (i < 3)
			{
				if ((int2.y < h && @int.y >= h && int3.y >= h) || (int2.y > h && @int.y <= h && int3.y <= h))
				{
					double num = (double)(h - int2.y) / (double)(@int.y - int2.y);
					double num2 = (double)(h - int2.y) / (double)(int3.y - int2.y);
					TileHandler.Point64Wrapper point64Wrapper = new TileHandler.Point64Wrapper((long)math.round(16.0 * ((double)int2.x + num * (double)(@int.x - int2.x))), (long)math.round(16.0 * ((double)int2.z + num * (double)(@int.z - int2.z))));
					TileHandler.Point64Wrapper point64Wrapper2 = new TileHandler.Point64Wrapper((long)math.round(16.0 * ((double)int2.x + num2 * (double)(int3.x - int2.x))), (long)math.round(16.0 * ((double)int2.z + num2 * (double)(int3.z - int2.z))));
					if (int2.y > h != preserveBelow)
					{
						Memory.Swap<TileHandler.Point64Wrapper>(ref point64Wrapper, ref point64Wrapper2);
					}
					int length = scratchVertices.Length;
					if (TileHandler.ClipAgainstHalfPlane(contourVertices, scratchVertices, point64Wrapper2, point64Wrapper))
					{
						contourVertices = scratchVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Slice(length);
						uint length2 = contourVertices.length;
						return;
					}
					scratchVertices.Length = length;
					return;
				}
				else
				{
					Int3 int4 = @int;
					@int = int2;
					int2 = int3;
					int3 = int4;
					i++;
				}
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00049334 File Offset: 0x00047534
		private unsafe static int DelaunayRefinement(UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, UnsafeSpan<int> tags, bool delaunay, bool colinear)
		{
			if (tris.Length % 3 != 0)
			{
				throw new ArgumentException("Triangle array length must be a multiple of 3");
			}
			if (tags.Length != tris.Length / 3)
			{
				throw new ArgumentException("There must be exactly 1 tag per 3 triangle indices");
			}
			NativeHashMap<Vector2Int, int> nativeHashMap = new NativeHashMap<Vector2Int, int>(tris.Length, Allocator.Temp);
			for (int i = 0; i < tris.Length; i += 3)
			{
				if (!VectorMath.IsClockwiseXZ(*verts[*tris[i]], *verts[*tris[i + 1]], *verts[*tris[i + 2]]))
				{
					int num = *tris[i];
					*tris[i] = *tris[i + 2];
					*tris[i + 2] = num;
				}
				TileHandler.<DelaunayRefinement>g__AddTriangleToLookup|43_0(nativeHashMap, tris, i);
			}
			int length = tris.Length;
			int j = 0;
			int num2 = 0;
			while (j < length)
			{
				int num3 = *tags[num2];
				for (int k = 0; k < 3; k++)
				{
					int num4 = *tris[j + k % 3];
					int num5 = *tris[j + (k + 1) % 3];
					int num6;
					if (nativeHashMap.TryGetValue(new Vector2Int(num5, num4), out num6))
					{
						int num7 = *tris[j + (k + 2) % 3];
						Int3 @int = *verts[num7];
						Int3 int2 = *verts[num5];
						Int3 int3 = *verts[num4];
						Int3 int4 = *verts[*tris[num6]];
						int num8 = *tags[num6 / 3];
						if (num3 == num8)
						{
							@int.y = 0;
							int2.y = 0;
							int3.y = 0;
							int4.y = 0;
							bool flag = false;
							if (!VectorMath.RightOrColinearXZ(@int, int3, int4) || VectorMath.RightXZ(@int, int2, int4))
							{
								if (!colinear)
								{
									goto IL_038F;
								}
								flag = true;
							}
							if (colinear && VectorMath.SqrDistancePointSegmentApproximate(@int, int4, int2) <= 9f && !nativeHashMap.ContainsKey(new Vector2Int(num7, num5)) && !nativeHashMap.ContainsKey(new Vector2Int(num5, *tris[num6])))
							{
								*tris[j + (k + 1) % 3] = *tris[num6];
								TileHandler.<DelaunayRefinement>g__RemoveTriangleWithVertex|43_1(num6, ref length, tris, tags, nativeHashMap);
								TileHandler.<DelaunayRefinement>g__AddTriangleToLookup|43_0(nativeHashMap, tris, j);
								k--;
							}
							else if (delaunay && !flag)
							{
								float num9 = Int3.Angle(int2 - @int, int3 - @int);
								if (Int3.Angle(int2 - int4, int3 - int4) > 6.2831855f - 2f * num9)
								{
									*tris[j + (k + 1) % 3] = *tris[num6];
									int num10 = num6 / 3 * 3;
									int num11 = num6 - num10;
									*tris[num10 + (num11 - 1 + 3) % 3] = *tris[j + (k + 2) % 3];
									TileHandler.<DelaunayRefinement>g__AddTriangleToLookup|43_0(nativeHashMap, tris, j);
									nativeHashMap[new Vector2Int(*tris[num10], *tris[num10 + 1])] = num10 + 2;
									nativeHashMap[new Vector2Int(*tris[num10 + 1], *tris[num10 + 2])] = num10;
									nativeHashMap[new Vector2Int(*tris[num10 + 2], *tris[num10])] = num10 + 1;
								}
							}
						}
					}
					IL_038F:;
				}
				j += 3;
				num2++;
			}
			return length;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000497E1 File Offset: 0x000479E1
		[CompilerGenerated]
		internal static long <ClipAgainstHalfPlane>g__SignedDistanceToHalfPlane|41_0(TileHandler.Point64Wrapper a, TileHandler.Point64Wrapper b, TileHandler.Point64Wrapper p)
		{
			return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0004981C File Offset: 0x00047A1C
		[CompilerGenerated]
		internal unsafe static void <DelaunayRefinement>g__AddTriangleToLookup|43_0(NativeHashMap<Vector2Int, int> lookup, UnsafeSpan<int> tris, int i)
		{
			lookup[new Vector2Int(*tris[i], *tris[i + 1])] = i + 2;
			lookup[new Vector2Int(*tris[i + 1], *tris[i + 2])] = i;
			lookup[new Vector2Int(*tris[i + 2], *tris[i])] = i + 1;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00049894 File Offset: 0x00047A94
		[CompilerGenerated]
		internal unsafe static void <DelaunayRefinement>g__RemoveTriangleWithVertex|43_1(int vertexIndex, ref int tCount, UnsafeSpan<int> tris, UnsafeSpan<int> tags, NativeHashMap<Vector2Int, int> lookup)
		{
			tCount -= 3;
			int num = vertexIndex / 3 * 3;
			if (num != tCount)
			{
				*tris[num] = *tris[tCount];
				*tris[num + 1] = *tris[tCount + 1];
				*tris[num + 2] = *tris[tCount + 2];
				*tags[num / 3] = *tags[tCount / 3];
				lookup[new Vector2Int(*tris[num], *tris[num + 1])] = num + 2;
				lookup[new Vector2Int(*tris[num + 1], *tris[num + 2])] = num;
				lookup[new Vector2Int(*tris[num + 2], *tris[num])] = num + 1;
				*tris[tCount] = 0;
				*tris[tCount + 1] = 0;
				*tris[tCount + 2] = 0;
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0004999C File Offset: 0x00047B9C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ConvertVerticesAndSnapToTileBoundaries$BurstManaged(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize)
		{
			outputVertices = new UnsafeList<TileHandler.Point64Wrapper>(contourVertices.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			outputVertices.Length = contourVertices.Length;
			UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan = outputVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>();
			int2 @int = new int2(Mathf.RoundToInt(tileSize.x * 1000f), Mathf.RoundToInt(tileSize.y * 1000f));
			uint num = 0U;
			while ((ulong)num < (ulong)((long)contourVertices.Length))
			{
				Hint.Assume(num < contourVertices.length);
				int2 int2 = (int2)math.round(*contourVertices[num] * 1000f);
				int2 int3 = int2 % @int;
				if (int2.x < 0)
				{
					int3.x += @int.x;
				}
				if (int2.y < 0)
				{
					int3.y += @int.y;
				}
				int2 int4 = math.select(0, -int3, int3 <= 20);
				int4 += math.select(0, @int - int3, int3 >= @int - 20);
				int2 += int4;
				Hint.Assume(num < unsafeSpan.length);
				*unsafeSpan[num] = new TileHandler.Point64Wrapper((long)int2.x, (long)int2.y);
				num += 1U;
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00049B08 File Offset: 0x00047D08
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CutTiles$BurstManaged(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator)
		{
			UnsafeList<TileHandler.Point64Wrapper> contourVertices = cutCollection.contourVertices;
			NativeArray<IntBounds> nativeArray = TileHandler.CalculateCutBounds(ref cutCollection, ref contourVertices);
			TileHandler.ScaleUpCoordinates(contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Reinterpret<long>(16));
			int length = tileVertices.Length;
			NativeList<int> nativeList = new NativeList<int>(4, Allocator.Temp);
			NativeArray<Int3> nativeArray2 = new NativeArray<Int3>(7, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<Int3> nativeArray3 = new NativeArray<Int3>(7, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<TileHandler.Point64Wrapper> nativeArray4 = new NativeArray<TileHandler.Point64Wrapper>(16, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeList<UnsafeSpan<TileHandler.Point64Wrapper>> nativeList2 = new NativeList<UnsafeSpan<TileHandler.Point64Wrapper>>(Allocator.Temp);
			NativeList<UnsafeSpan<TileHandler.Point64Wrapper>> nativeList3 = new NativeList<UnsafeSpan<TileHandler.Point64Wrapper>>(Allocator.Temp);
			NativeList<TileHandler.Point64Wrapper> nativeList4 = new NativeList<TileHandler.Point64Wrapper>(32, Allocator.Temp);
			NativeList<int2> nativeList5 = new NativeList<int2>(Allocator.Temp);
			NativeList<int> nativeList6 = new NativeList<int>(Allocator.Temp);
			NativeHashMap<int2, int> nativeHashMap = new NativeHashMap<int2, int>(64, Allocator.Temp);
			andywiecko.BurstTriangulator.LowLevel.Unsafe.OutputData<int2> outputData = new andywiecko.BurstTriangulator.LowLevel.Unsafe.OutputData<int2>
			{
				Positions = new NativeList<int2>(Allocator.Temp),
				Triangles = new NativeList<int>(Allocator.Temp),
				Status = new NativeReference<Status>(Allocator.Temp, NativeArrayOptions.ClearMemory),
				Halfedges = new NativeList<int>(Allocator.Temp),
				ConstrainedHalfedges = new NativeList<HalfedgeState>(Allocator.Temp)
			};
			NativeList<Int3> nativeList7 = new NativeList<Int3>(Allocator.Temp);
			NativeList<int> nativeList8 = new NativeList<int>(Allocator.Temp);
			NativeList<int> nativeList9 = new NativeList<int>(Allocator.Temp);
			NativeList<Vector2Int> nativeList10 = new NativeList<Vector2Int>(0, Allocator.Temp);
			NativeList<int> nativeList11 = new NativeList<int>(0, Allocator.Temp);
			for (int i = 0; i < length; i++)
			{
				UnsafeList<UnsafeSpan<Int3>> unsafeList = *tileVertices[i];
				UnsafeList<UnsafeSpan<int>> unsafeList2 = *tileTriangles[i];
				TileHandler.TileCuts tileCuts = cutCollection.tileCuts[i];
				if (tileCuts.contourStartIndex == tileCuts.contourEndIndex && unsafeList.Length == 1)
				{
					*output[i] = new TileMesh.TileMeshUnsafe
					{
						verticesInTileSpace = unsafeList[0].Clone(allocator),
						triangles = unsafeList2[0].Clone(allocator),
						tags = tileTags[i][0].Clone(allocator).Reinterpret<uint>()
					};
				}
				else
				{
					nativeList7.Clear();
					nativeList8.Clear();
					nativeList9.Clear();
					UnsafeSpan<TileHandler.ContourMeta> unsafeSpan = cutCollection.contoursExtra.AsUnsafeSpan<TileHandler.ContourMeta>().Slice(tileCuts.contourStartIndex, tileCuts.contourEndIndex - tileCuts.contourStartIndex);
					UnsafeSpan<IntBounds> unsafeSpan2 = nativeArray.AsUnsafeSpan<IntBounds>().Slice(tileCuts.contourStartIndex, tileCuts.contourEndIndex - tileCuts.contourStartIndex);
					if (unsafeSpan2.Length > nativeList.Capacity)
					{
						nativeList.SetCapacity(unsafeSpan2.Length);
					}
					for (int j = 0; j < unsafeList.Length; j++)
					{
						UnsafeSpan<Int3> unsafeSpan3 = unsafeList[j];
						UnsafeSpan<int3> unsafeSpan4 = unsafeList2[j].Reinterpret<int3>(4);
						UnsafeSpan<int> unsafeSpan5 = tileTags[i][j];
						for (int k = 0; k < unsafeSpan4.Length; k++)
						{
							int3 @int = *unsafeSpan4[k];
							Int3 int2 = *unsafeSpan3[@int.x];
							Int3 int3 = *unsafeSpan3[@int.y];
							Int3 int4 = *unsafeSpan3[@int.z];
							IntBounds intBounds = TileHandler.TriangleBounds(int2, int3, int4);
							intBounds.max.xz = intBounds.max.xz + 1;
							intBounds.min.xz = intBounds.min.xz - 1;
							TileHandler.CollectCutsTouchingBounds(unsafeSpan2, nativeList, intBounds);
							bool flag = j > 0;
							int num = *unsafeSpan5[k];
							if (nativeList.Length == 0 && !flag)
							{
								int length2 = nativeList7.Length;
								nativeList7.Capacity = math.max(nativeList7.Capacity, nativeList7.Length + 3);
								nativeList8.Capacity = math.max(nativeList8.Capacity, nativeList8.Length + 3);
								nativeList7.AddNoResize(int2);
								nativeList7.AddNoResize(int3);
								nativeList7.AddNoResize(int4);
								for (int l = 0; l < 3; l++)
								{
									nativeList8.AddNoResize(length2 + l);
								}
								nativeList9.Add(in num);
							}
							else
							{
								UnsafeSpan<NavmeshCut.ContourBurst> unsafeSpan6 = cutCollection.contours.AsUnsafeSpan<NavmeshCut.ContourBurst>().Slice(tileCuts.contourStartIndex, tileCuts.contourEndIndex - tileCuts.contourStartIndex);
								nativeList4.Clear();
								nativeList2.Clear();
								nativeList3.Clear();
								int num2;
								if (flag)
								{
									nativeArray2[0] = int2;
									nativeArray2[1] = int3;
									nativeArray2[2] = int4;
									num2 = TileHandler.ClipAgainstRectangle(nativeArray2.AsUnsafeSpan<Int3>(), nativeArray3.AsUnsafeSpan<Int3>(), tileSize);
									for (int m = 0; m < num2; m++)
									{
										nativeArray4[m] = new TileHandler.Point64Wrapper((long)(nativeArray2[m].x * 16), (long)(nativeArray2[m].z * 16));
									}
									for (int n = 0; n < nativeList.Length; n++)
									{
										int num3 = nativeList[n];
										if (unsafeSpan[num3].cutsAddedGeom)
										{
											NavmeshCut.ContourBurst contourBurst = *unsafeSpan6[num3];
											UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan7 = contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Slice(contourBurst.startIndex, contourBurst.endIndex - contourBurst.startIndex);
											if (unsafeSpan[num3].isDual)
											{
												nativeList3.Add(in unsafeSpan7);
											}
											else
											{
												nativeList2.Add(in unsafeSpan7);
											}
										}
									}
								}
								else
								{
									num2 = 3;
									nativeArray4[0] = new TileHandler.Point64Wrapper((long)(int2.x * 16), (long)(int2.z * 16));
									nativeArray4[1] = new TileHandler.Point64Wrapper((long)(int3.x * 16), (long)(int3.z * 16));
									nativeArray4[2] = new TileHandler.Point64Wrapper((long)(int4.x * 16), (long)(int4.z * 16));
									bool flag2 = intBounds.min.y == intBounds.max.y - 1;
									for (int num4 = 0; num4 < nativeList.Length; num4++)
									{
										IntBounds intBounds2 = *unsafeSpan2[nativeList[num4]];
										NavmeshCut.ContourBurst contourBurst2 = *unsafeSpan6[nativeList[num4]];
										UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan8 = contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Slice(contourBurst2.startIndex, contourBurst2.endIndex - contourBurst2.startIndex);
										if (!flag2)
										{
											int y = intBounds2.min.y;
											if (intBounds.min.y <= y && intBounds.max.y - 1 >= y)
											{
												if (y == intBounds.max.y - 1)
												{
													unsafeSpan8 = default(UnsafeSpan<TileHandler.Point64Wrapper>);
												}
												TileHandler.ClipAgainstHorizontalHalfPlane(ref unsafeSpan8, nativeList4, y, int2, int3, int4, false);
											}
											int y2 = intBounds2.max.y;
											if (intBounds.min.y <= y2 && intBounds.max.y - 1 >= y2)
											{
												if (y2 == intBounds.min.y)
												{
													unsafeSpan8 = default(UnsafeSpan<TileHandler.Point64Wrapper>);
												}
												TileHandler.ClipAgainstHorizontalHalfPlane(ref unsafeSpan8, nativeList4, y2, int2, int3, int4, true);
											}
										}
										if (unsafeSpan8.length > 0U)
										{
											(unsafeSpan[nativeList[num4]].isDual ? nativeList3 : nativeList2).Add(in unsafeSpan8);
										}
									}
								}
								TileHandler.SnapEdges(ref nativeArray4, ref num2, nativeList2.AsUnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>>(), tileSize);
								int num5 = 0;
								while (num5 < 2 && (num5 != 1 || nativeList3.Length != 0))
								{
									UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan9 = nativeArray4.AsUnsafeReadOnlySpan<TileHandler.Point64Wrapper>().Slice(0, num2);
									UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> unsafeSpan10 = nativeList2.AsUnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>>();
									UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> unsafeSpan11 = nativeList3.AsUnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>>();
									contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>();
									nativeList10.Clear();
									nativeList11.Clear();
									FunctionPointer<TileHandler.CutFunction> functionPointer = new FunctionPointer<TileHandler.CutFunction>(*TileHandler.CutFunctionPtr.Data);
									if (!functionPointer.Invoke(ref unsafeSpan9, ref unsafeSpan10, ref unsafeSpan11, UnsafeUtility.AsRef<UnsafeList<Vector2Int>>((void*)nativeList10.GetUnsafeList()), UnsafeUtility.AsRef<UnsafeList<int>>((void*)nativeList11.GetUnsafeList()), num5))
									{
										Debug.LogError("Error during cutting");
									}
									else if (nativeList11.Length != 0)
									{
										if (nativeList10.Length == 3 && nativeList11.Length == 1)
										{
											int length3 = nativeList7.Length;
											nativeList7.Capacity = math.max(nativeList7.Capacity, nativeList7.Length + 3);
											nativeList8.Capacity = math.max(nativeList8.Capacity, nativeList8.Length + 3);
											Polygon.BarycentricTriangleInterpolator barycentricTriangleInterpolator = new Polygon.BarycentricTriangleInterpolator(int2, int3, int4);
											for (int num6 = 0; num6 < 3; num6++)
											{
												Int3 int5 = new Int3(nativeList10[num6].x / 16, 0, nativeList10[num6].y / 16);
												int5.y = barycentricTriangleInterpolator.SampleY(new int2(int5.x, int5.z));
												nativeList7.Add(in int5);
											}
											int num7 = length3;
											nativeList8.Add(in num7);
											num7 = length3 + 1;
											nativeList8.Add(in num7);
											num7 = length3 + 2;
											nativeList8.Add(in num7);
											nativeList9.Add(in num);
										}
										else
										{
											nativeList5.Clear();
											nativeList6.Clear();
											nativeHashMap.Clear();
											TileHandler.ScaleDownCoordinates(nativeList10.AsUnsafeSpan<Vector2Int>().Reinterpret<int>(8));
											int num8 = 0;
											for (int num9 = 0; num9 < nativeList11.Length; num9++)
											{
												int num10 = num8;
												num8 += nativeList11[num9];
												UnsafeSpan<int2> unsafeSpan12 = nativeList10.AsUnsafeSpan<Vector2Int>().Slice(num10, num8 - num10).Reinterpret<int2>();
												TileHandler.RemoveDegenerateSegments(ref unsafeSpan12);
												int num11 = -1;
												int length4 = nativeList6.Length;
												int num12 = 0;
												while ((long)num12 < (long)((ulong)unsafeSpan12.length))
												{
													int2 int6 = *unsafeSpan12[num12];
													int length5;
													if (!nativeHashMap.TryGetValue(int6, out length5))
													{
														length5 = nativeList5.Length;
														nativeList5.Add(in int6);
														nativeHashMap.Add(int6, length5);
													}
													if (num11 != -1)
													{
														nativeList6.Add(in num11);
														nativeList6.Add(in length5);
													}
													num11 = length5;
													num12++;
												}
												nativeList6.Add(in num11);
												int num7 = nativeList6[length4];
												nativeList6.Add(in num7);
											}
											andywiecko.BurstTriangulator.LowLevel.Unsafe.InputData<int2> inputData = new andywiecko.BurstTriangulator.LowLevel.Unsafe.InputData<int2>
											{
												Positions = nativeList5.AsArray(),
												ConstraintEdges = nativeList6.AsArray()
											};
											Args args = new Args(Preprocessor.None, 1000000, true, false, false, false, false, 0f, 0f);
											default(UnsafeTriangulator<int2>).Triangulate(inputData, outputData, args, Allocator.Temp);
											if (outputData.Status.Value.IsError)
											{
												Debug.LogError("Error during triangulation");
											}
											else
											{
												TileHandler.CopyTriangulationToOutput(ref outputData, nativeList7, nativeList8, nativeList9, num, int2, int3, int4);
											}
										}
									}
									num5++;
								}
							}
						}
					}
					*output[i] = TileHandler.CompressAndRefineTile(nativeList7, nativeList8, nativeList9, allocator);
				}
			}
			nativeList7.Dispose();
			nativeList8.Dispose();
			nativeList9.Dispose();
		}

		// Token: 0x0400089C RID: 2204
		private static readonly ProfilerMarker MarkerTriangulate = new ProfilerMarker("Triangulate");

		// Token: 0x0400089D RID: 2205
		private static readonly ProfilerMarker MarkerClipping = new ProfilerMarker("Clipping");

		// Token: 0x0400089E RID: 2206
		private static readonly ProfilerMarker MarkerPrepare = new ProfilerMarker("Prepare");

		// Token: 0x0400089F RID: 2207
		private static readonly ProfilerMarker MarkerAllocate = new ProfilerMarker("Allocate");

		// Token: 0x040008A0 RID: 2208
		private static readonly ProfilerMarker MarkerCore = new ProfilerMarker("Core");

		// Token: 0x040008A1 RID: 2209
		private static readonly ProfilerMarker MarkerCompress = new ProfilerMarker("Compress");

		// Token: 0x040008A2 RID: 2210
		private static readonly ProfilerMarker MarkerRemoveDegenerateTriangles = new ProfilerMarker("Remove Degenerate Tris");

		// Token: 0x040008A3 RID: 2211
		private static readonly ProfilerMarker MarkerRefine = new ProfilerMarker("Refine");

		// Token: 0x040008A4 RID: 2212
		private static readonly ProfilerMarker MarkerEdgeSnapping = new ProfilerMarker("EdgeSnapping");

		// Token: 0x040008A5 RID: 2213
		private static readonly ProfilerMarker MarkerRemoveDegenerateLines = new ProfilerMarker("Remove Degenerate Lines");

		// Token: 0x040008A6 RID: 2214
		private static readonly ProfilerMarker MarkerClipHorizontal = new ProfilerMarker("ClipHorizontal");

		// Token: 0x040008A7 RID: 2215
		private static readonly ProfilerMarker MarkerCopyClippingResult = new ProfilerMarker("CopyClippingResult");

		// Token: 0x040008A8 RID: 2216
		private static readonly ProfilerMarker CopyTriangulationToOutputMarker = new ProfilerMarker("Copy to output");

		// Token: 0x040008A9 RID: 2217
		private static readonly SharedStatic<IntPtr> CutFunctionPtr = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -3121683876094195160L, 0L);

		// Token: 0x040008AA RID: 2218
		private static TileHandler.CutFunction DelegateGCRoot;

		// Token: 0x040008AB RID: 2219
		private const int EdgeSnappingMaxDistance = 1;

		// Token: 0x040008AC RID: 2220
		private const int Scale = 16;

		// Token: 0x040008AD RID: 2221
		public const int TileSnappingMaxDistance = 20;

		// Token: 0x020001C5 RID: 453
		// (Invoke) Token: 0x06000CED RID: 3309
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate bool CutFunction(ref UnsafeSpan<TileHandler.Point64Wrapper> subject, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contoursDual, ref UnsafeList<Vector2Int> outputVertices, ref UnsafeList<int> outputVertexCountPerPolygon, int dual);

		// Token: 0x020001C6 RID: 454
		private struct CutFunctionKey
		{
		}

		// Token: 0x020001C7 RID: 455
		internal struct TileCuts
		{
			// Token: 0x040008AE RID: 2222
			public int contourStartIndex;

			// Token: 0x040008AF RID: 2223
			public int contourEndIndex;
		}

		// Token: 0x020001C8 RID: 456
		internal struct ContourMeta
		{
			// Token: 0x040008B0 RID: 2224
			public bool isDual;

			// Token: 0x040008B1 RID: 2225
			public bool cutsAddedGeom;
		}

		// Token: 0x020001C9 RID: 457
		internal struct CutCollection : IDisposable
		{
			// Token: 0x06000CF0 RID: 3312 RVA: 0x0004A67D File Offset: 0x0004887D
			public void Dispose()
			{
				this.contourVertices.Dispose();
				this.contours.Dispose();
				this.contoursExtra.Dispose();
				this.tileCuts.Dispose();
			}

			// Token: 0x040008B2 RID: 2226
			public UnsafeList<TileHandler.Point64Wrapper> contourVertices;

			// Token: 0x040008B3 RID: 2227
			public UnsafeList<NavmeshCut.ContourBurst> contours;

			// Token: 0x040008B4 RID: 2228
			public UnsafeList<TileHandler.ContourMeta> contoursExtra;

			// Token: 0x040008B5 RID: 2229
			public UnsafeList<TileHandler.TileCuts> tileCuts;

			// Token: 0x040008B6 RID: 2230
			[MarshalAs(UnmanagedType.U1)]
			public bool cuttingRequired;
		}

		// Token: 0x020001CA RID: 458
		public struct Point64Wrapper
		{
			// Token: 0x06000CF1 RID: 3313 RVA: 0x0004A6AB File Offset: 0x000488AB
			public Point64Wrapper(long x, long y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x040008B7 RID: 2231
			public long x;

			// Token: 0x040008B8 RID: 2232
			public long y;
		}

		// Token: 0x020001CB RID: 459
		// (Invoke) Token: 0x06000CF3 RID: 3315
		internal delegate void ConvertVerticesAndSnapToTileBoundaries_00000BD6$PostfixBurstDelegate(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize);

		// Token: 0x020001CC RID: 460
		internal static class ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall
		{
			// Token: 0x06000CF6 RID: 3318 RVA: 0x0004A6BB File Offset: 0x000488BB
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.Pointer == 0)
				{
					TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.DeferredCompilation, methodof(TileHandler.ConvertVerticesAndSnapToTileBoundaries$BurstManaged(ref UnsafeSpan<float2>, ref UnsafeList<TileHandler.Point64Wrapper>, ref Vector2)).MethodHandle, typeof(TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.Pointer;
			}

			// Token: 0x06000CF7 RID: 3319 RVA: 0x0004A6E8 File Offset: 0x000488E8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000CF8 RID: 3320 RVA: 0x0004A700 File Offset: 0x00048900
			public static void Constructor()
			{
				TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TileHandler.ConvertVerticesAndSnapToTileBoundaries(ref UnsafeSpan<float2>, ref UnsafeList<TileHandler.Point64Wrapper>, ref Vector2)).MethodHandle);
			}

			// Token: 0x06000CF9 RID: 3321 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000CFA RID: 3322 RVA: 0x0004A711 File Offset: 0x00048911
			// Note: this type is marked as 'beforefieldinit'.
			static ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall()
			{
				TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.Constructor();
			}

			// Token: 0x06000CFB RID: 3323 RVA: 0x0004A718 File Offset: 0x00048918
			public static void Invoke(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000BD6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float2>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Graphs.Navmesh.TileHandler/Point64Wrapper>&,UnityEngine.Vector2&), ref contourVertices, ref outputVertices, ref tileSize, functionPointer);
						return;
					}
				}
				TileHandler.ConvertVerticesAndSnapToTileBoundaries$BurstManaged(ref contourVertices, out outputVertices, ref tileSize);
			}

			// Token: 0x040008B9 RID: 2233
			private static IntPtr Pointer;

			// Token: 0x040008BA RID: 2234
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001CD RID: 461
		// (Invoke) Token: 0x06000CFD RID: 3325
		internal delegate void CutTiles_00000BD7$PostfixBurstDelegate(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator);

		// Token: 0x020001CE RID: 462
		internal static class CutTiles_00000BD7$BurstDirectCall
		{
			// Token: 0x06000D00 RID: 3328 RVA: 0x0004A74D File Offset: 0x0004894D
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TileHandler.CutTiles_00000BD7$BurstDirectCall.Pointer == 0)
				{
					TileHandler.CutTiles_00000BD7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TileHandler.CutTiles_00000BD7$BurstDirectCall.DeferredCompilation, methodof(TileHandler.CutTiles$BurstManaged(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>>, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>>, ref Vector2Int, ref TileHandler.CutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe>, Allocator)).MethodHandle, typeof(TileHandler.CutTiles_00000BD7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TileHandler.CutTiles_00000BD7$BurstDirectCall.Pointer;
			}

			// Token: 0x06000D01 RID: 3329 RVA: 0x0004A77C File Offset: 0x0004897C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TileHandler.CutTiles_00000BD7$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000D02 RID: 3330 RVA: 0x0004A794 File Offset: 0x00048994
			public static void Constructor()
			{
				TileHandler.CutTiles_00000BD7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TileHandler.CutTiles(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>>, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>>, ref Vector2Int, ref TileHandler.CutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe>, Allocator)).MethodHandle);
			}

			// Token: 0x06000D03 RID: 3331 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000D04 RID: 3332 RVA: 0x0004A7A5 File Offset: 0x000489A5
			// Note: this type is marked as 'beforefieldinit'.
			static CutTiles_00000BD7$BurstDirectCall()
			{
				TileHandler.CutTiles_00000BD7$BurstDirectCall.Constructor();
			}

			// Token: 0x06000D05 RID: 3333 RVA: 0x0004A7AC File Offset: 0x000489AC
			public static void Invoke(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TileHandler.CutTiles_00000BD7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>>>&,Pathfinding.Collections.UnsafeSpan`1<Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<System.Int32>>>&,Pathfinding.Collections.UnsafeSpan`1<Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<System.Int32>>>&,UnityEngine.Vector2Int&,Pathfinding.Graphs.Navmesh.TileHandler/CutCollection&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.TileMesh/TileMeshUnsafe>&,Unity.Collections.Allocator), ref tileVertices, ref tileTriangles, ref tileTags, ref tileSize, ref cutCollection, ref output, allocator, functionPointer);
						return;
					}
				}
				TileHandler.CutTiles$BurstManaged(ref tileVertices, ref tileTriangles, ref tileTags, ref tileSize, ref cutCollection, ref output, allocator);
			}

			// Token: 0x040008BB RID: 2235
			private static IntPtr Pointer;

			// Token: 0x040008BC RID: 2236
			private static IntPtr DeferredCompilation;
		}
	}
}
