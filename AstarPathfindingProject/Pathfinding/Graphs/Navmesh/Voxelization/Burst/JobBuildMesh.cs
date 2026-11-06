using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E7 RID: 487
	[BurstCompile]
	public struct JobBuildMesh : IJob
	{
		// Token: 0x06000D76 RID: 3446 RVA: 0x0004EBB8 File Offset: 0x0004CDB8
		private static bool Diagonal(int i, int j, int n, NativeArray<int> verts, NativeArray<int> indices)
		{
			return JobBuildMesh.InCone(i, j, n, verts, indices) && JobBuildMesh.Diagonalie(i, j, n, verts, indices);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0004EBD4 File Offset: 0x0004CDD4
		private static bool InCone(int i, int j, int n, NativeArray<int> verts, NativeArray<int> indices)
		{
			int num = (indices[i] & 268435455) * 3;
			int num2 = (indices[j] & 268435455) * 3;
			int num3 = (indices[JobBuildMesh.Next(i, n)] & 268435455) * 3;
			int num4 = (indices[JobBuildMesh.Prev(i, n)] & 268435455) * 3;
			if (JobBuildMesh.LeftOn(num4, num, num3, verts))
			{
				return JobBuildMesh.Left(num, num2, num4, verts) && JobBuildMesh.Left(num2, num, num3, verts);
			}
			return !JobBuildMesh.LeftOn(num, num2, num3, verts) || !JobBuildMesh.LeftOn(num2, num, num4, verts);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0004EC6C File Offset: 0x0004CE6C
		private static bool Left(int a, int b, int c, NativeArray<int> verts)
		{
			return JobBuildMesh.Area2(a, b, c, verts) < 0;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0004EC7A File Offset: 0x0004CE7A
		private static bool LeftOn(int a, int b, int c, NativeArray<int> verts)
		{
			return JobBuildMesh.Area2(a, b, c, verts) <= 0;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0004EC8B File Offset: 0x0004CE8B
		private static bool Collinear(int a, int b, int c, NativeArray<int> verts)
		{
			return JobBuildMesh.Area2(a, b, c, verts) == 0;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0004EC9C File Offset: 0x0004CE9C
		public static int Area2(int a, int b, int c, NativeArray<int> verts)
		{
			return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0004ECF8 File Offset: 0x0004CEF8
		private static bool Diagonalie(int i, int j, int n, NativeArray<int> verts, NativeArray<int> indices)
		{
			int num = (indices[i] & 268435455) * 3;
			int num2 = (indices[j] & 268435455) * 3;
			for (int k = 0; k < n; k++)
			{
				int num3 = JobBuildMesh.Next(k, n);
				if (k != i && num3 != i && k != j && num3 != j)
				{
					int num4 = (indices[k] & 268435455) * 3;
					int num5 = (indices[num3] & 268435455) * 3;
					if (!JobBuildMesh.Vequal(num, num4, verts) && !JobBuildMesh.Vequal(num2, num4, verts) && !JobBuildMesh.Vequal(num, num5, verts) && !JobBuildMesh.Vequal(num2, num5, verts) && JobBuildMesh.Intersect(num, num2, num4, num5, verts))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0004EDAC File Offset: 0x0004CFAC
		private static bool Xorb(bool x, bool y)
		{
			return !x ^ !y;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0004EDB8 File Offset: 0x0004CFB8
		private static bool IntersectProp(int a, int b, int c, int d, NativeArray<int> verts)
		{
			return !JobBuildMesh.Collinear(a, b, c, verts) && !JobBuildMesh.Collinear(a, b, d, verts) && !JobBuildMesh.Collinear(c, d, a, verts) && !JobBuildMesh.Collinear(c, d, b, verts) && JobBuildMesh.Xorb(JobBuildMesh.Left(a, b, c, verts), JobBuildMesh.Left(a, b, d, verts)) && JobBuildMesh.Xorb(JobBuildMesh.Left(c, d, a, verts), JobBuildMesh.Left(c, d, b, verts));
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0004EE30 File Offset: 0x0004D030
		private static bool Between(int a, int b, int c, NativeArray<int> verts)
		{
			if (!JobBuildMesh.Collinear(a, b, c, verts))
			{
				return false;
			}
			if (verts[a] != verts[b])
			{
				return (verts[a] <= verts[c] && verts[c] <= verts[b]) || (verts[a] >= verts[c] && verts[c] >= verts[b]);
			}
			return (verts[a + 2] <= verts[c + 2] && verts[c + 2] <= verts[b + 2]) || (verts[a + 2] >= verts[c + 2] && verts[c + 2] >= verts[b + 2]);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0004EF0C File Offset: 0x0004D10C
		private static bool Intersect(int a, int b, int c, int d, NativeArray<int> verts)
		{
			return JobBuildMesh.IntersectProp(a, b, c, d, verts) || (JobBuildMesh.Between(a, b, c, verts) || JobBuildMesh.Between(a, b, d, verts) || JobBuildMesh.Between(c, d, a, verts) || JobBuildMesh.Between(c, d, b, verts));
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0004EF5B File Offset: 0x0004D15B
		private static bool Vequal(int a, int b, NativeArray<int> verts)
		{
			return verts[a] == verts[b] && verts[a + 2] == verts[b + 2];
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0004EF87 File Offset: 0x0004D187
		private static int Prev(int i, int n)
		{
			if (i - 1 < 0)
			{
				return n - 1;
			}
			return i - 1;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0004EF96 File Offset: 0x0004D196
		private static int Next(int i, int n)
		{
			if (i + 1 >= n)
			{
				return 0;
			}
			return i + 1;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0004EFA4 File Offset: 0x0004D1A4
		private static int AddVertex(NativeList<Int3> vertices, NativeHashMap<Int3, int> vertexMap, Int3 vertex)
		{
			int num;
			if (vertexMap.TryGetValue(vertex, out num))
			{
				return num;
			}
			vertices.AddNoResize(vertex);
			vertexMap.Add(vertex, vertices.Length - 1);
			return vertices.Length - 1;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0004EFE4 File Offset: 0x0004D1E4
		public void Execute()
		{
			int num = 3;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < this.contours.Length; i++)
			{
				if (this.contours[i].nverts >= 3)
				{
					num2 += this.contours[i].nverts;
					num3 += this.contours[i].nverts - 2;
					num4 = Math.Max(num4, this.contours[i].nverts);
				}
			}
			this.mesh.verts.Clear();
			if (num2 > this.mesh.verts.Capacity)
			{
				this.mesh.verts.SetCapacity(num2);
			}
			this.mesh.tris.ResizeUninitialized(num3 * num);
			this.mesh.areas.ResizeUninitialized(num3);
			NativeList<Int3> verts = this.mesh.verts;
			NativeList<int> tris = this.mesh.tris;
			NativeList<int> areas = this.mesh.areas;
			NativeArray<int> nativeArray = new NativeArray<int>(num4, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray2 = new NativeArray<int>(num4 * 3, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<bool> nativeArray3 = new NativeArray<bool>(num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeHashMap<Int3, int> nativeHashMap = new NativeHashMap<Int3, int>(num2, Allocator.Temp);
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < this.contours.Length; j++)
			{
				VoxelContour voxelContour = this.contours[j];
				if (voxelContour.nverts >= 3)
				{
					for (int k = 0; k < voxelContour.nverts; k++)
					{
						ref NativeList<int> ptr = ref this.contourVertices;
						int num7 = voxelContour.vertexStartIndex + k * 4 + 2;
						ptr[num7] /= this.field.width;
					}
					for (int l = 0; l < voxelContour.nverts; l++)
					{
						int num8 = this.contourVertices[voxelContour.vertexStartIndex + l * 4 + 3];
						int num9 = JobBuildMesh.AddVertex(verts, nativeHashMap, new Int3(this.contourVertices[voxelContour.vertexStartIndex + l * 4], this.contourVertices[voxelContour.vertexStartIndex + l * 4 + 1], this.contourVertices[voxelContour.vertexStartIndex + l * 4 + 2]));
						nativeArray[l] = num9;
						nativeArray3[num9] = (num8 & 65536) != 0;
					}
					int num10 = JobBuildMesh.Triangulate(voxelContour.nverts, verts.AsArray().Reinterpret<int>(12), nativeArray, nativeArray2);
					if (num10 < 0)
					{
						num10 = -num10;
					}
					for (int m = 0; m < num10 * 3; m++)
					{
						tris[num5] = nativeArray2[m];
						num5++;
					}
					for (int n = 0; n < num10; n++)
					{
						areas[num6] = voxelContour.area;
						num6++;
					}
				}
			}
			this.mesh.tris.ResizeUninitialized(num5);
			this.mesh.areas.ResizeUninitialized(num6);
			this.RemoveTileBorderVertices(ref this.mesh, nativeArray3);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0004F30C File Offset: 0x0004D50C
		private void RemoveTileBorderVertices(ref VoxelMesh mesh, NativeArray<bool> verticesToRemove)
		{
			NativeArray<byte> nativeArray = new NativeArray<byte>(mesh.verts.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = mesh.verts.Length - 1; i >= 0; i--)
			{
				if (verticesToRemove[i] && this.CanRemoveVertex(ref mesh, i, nativeArray.AsUnsafeSpan<byte>()))
				{
					this.RemoveVertex(ref mesh, i);
				}
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0004F368 File Offset: 0x0004D568
		private unsafe bool CanRemoveVertex(ref VoxelMesh mesh, int vertexToRemove, UnsafeSpan<byte> vertexScratch)
		{
			int num = 0;
			for (int i = 0; i < mesh.tris.Length; i += 3)
			{
				int num2 = 0;
				for (int j = 0; j < 3; j++)
				{
					if (mesh.tris[i + j] == vertexToRemove)
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					if (num2 > 1)
					{
						throw new Exception("Degenerate triangle. This should have already been removed.");
					}
					num++;
				}
			}
			if (num <= 2)
			{
				return false;
			}
			vertexScratch.FillZeros<byte>();
			for (int k = 0; k < mesh.tris.Length; k += 3)
			{
				int l = 0;
				int num3 = 2;
				while (l < 3)
				{
					if (mesh.tris[k + l] == vertexToRemove || mesh.tris[k + num3] == vertexToRemove)
					{
						int num4 = mesh.tris[k + l];
						int num5 = mesh.tris[k + num3];
						ref byte ptr = ref vertexScratch[(num5 == vertexToRemove) ? num4 : num5];
						ptr += 1;
					}
					num3 = l++;
				}
			}
			int num6 = 0;
			int num7 = 0;
			for (int m = 0; m < vertexScratch.Length; m++)
			{
				if (*vertexScratch[m] == 1)
				{
					num6++;
				}
				else if (*vertexScratch[m] > 2)
				{
					num7++;
				}
			}
			if (num7 > 0)
			{
				Debug.LogError("Vertex has multiple shared edges. This should not happen. Navmesh must be corrupt. Trying to not make it worse.");
				return false;
			}
			return num6 <= 2;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0004F4C8 File Offset: 0x0004D6C8
		private void RemoveVertex(ref VoxelMesh mesh, int vertexToRemove)
		{
			NativeList<int> nativeList = new NativeList<int>(16, Allocator.Temp);
			int num = -1;
			int num3;
			for (int i = 0; i < mesh.tris.Length; i += 3)
			{
				int num2 = -1;
				for (int j = 0; j < 3; j++)
				{
					if (mesh.tris[i + j] == vertexToRemove)
					{
						num2 = j;
						break;
					}
				}
				if (num2 != -1)
				{
					num = mesh.areas[i / 3];
					num3 = mesh.tris[i + (num2 + 1) % 3];
					nativeList.Add(in num3);
					num3 = mesh.tris[i + (num2 + 2) % 3];
					nativeList.Add(in num3);
					mesh.tris[i] = mesh.tris[mesh.tris.Length - 3];
					mesh.tris[i + 1] = mesh.tris[mesh.tris.Length - 3 + 1];
					mesh.tris[i + 2] = mesh.tris[mesh.tris.Length - 3 + 2];
					mesh.tris.Length = mesh.tris.Length - 3;
					mesh.areas.RemoveAtSwapBack(i / 3);
					i -= 3;
				}
			}
			NativeList<int> nativeList2 = new NativeList<int>(nativeList.Length / 2 + 1, Allocator.Temp);
			num3 = nativeList[nativeList.Length - 2];
			nativeList2.Add(in num3);
			num3 = nativeList[nativeList.Length - 1];
			nativeList2.Add(in num3);
			nativeList.Length -= 2;
			while (nativeList.Length > 0)
			{
				for (int k = nativeList.Length - 2; k >= 0; k -= 2)
				{
					int num4 = nativeList[k];
					int num5 = nativeList[k + 1];
					bool flag = false;
					if (nativeList2[0] == num5)
					{
						nativeList2.InsertRange(0, 1);
						nativeList2[0] = num4;
						flag = true;
					}
					if (nativeList2[nativeList2.Length - 1] == num4)
					{
						nativeList2.AddNoResize(num5);
						flag = true;
					}
					if (flag)
					{
						nativeList[k] = nativeList[nativeList.Length - 2];
						nativeList[k + 1] = nativeList[nativeList.Length - 1];
						nativeList.Length -= 2;
					}
				}
			}
			mesh.verts.RemoveAt(vertexToRemove);
			for (int l = 0; l < mesh.tris.Length; l++)
			{
				if (mesh.tris[l] > vertexToRemove)
				{
					num3 = l;
					int num6 = mesh.tris[num3];
					mesh.tris[num3] = num6 - 1;
				}
			}
			for (int m = 0; m < nativeList2.Length; m++)
			{
				if (nativeList2[m] > vertexToRemove)
				{
					int num6 = m;
					num3 = nativeList2[num6];
					nativeList2[num6] = num3 - 1;
				}
			}
			int num7 = (nativeList2.Length - 2) * 3;
			int length = mesh.tris.Length;
			mesh.tris.Length = mesh.tris.Length + num7;
			int num8 = JobBuildMesh.Triangulate(nativeList2.Length, mesh.verts.AsArray().Reinterpret<int>(12), nativeList2.AsArray(), mesh.tris.AsArray().GetSubArray(length, num7));
			if (num8 < 0)
			{
				num8 = -num8;
			}
			mesh.tris.ResizeUninitialized(length + num8 * 3);
			mesh.areas.AddReplicate(in num, num8);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0004F884 File Offset: 0x0004DA84
		private static int Triangulate(int n, NativeArray<int> verts, NativeArray<int> indices, NativeArray<int> tris)
		{
			int num = 0;
			NativeArray<int> nativeArray = tris;
			int num2 = 0;
			for (int i = 0; i < n; i++)
			{
				int num3 = JobBuildMesh.Next(i, n);
				int num4 = JobBuildMesh.Next(num3, n);
				if (JobBuildMesh.Diagonal(i, num4, n, verts, indices))
				{
					ref NativeArray<int> ptr = ref indices;
					int num5 = num3;
					ptr[num5] |= 1073741824;
				}
			}
			while (n > 3)
			{
				int num6 = int.MaxValue;
				int num7 = -1;
				for (int j = 0; j < n; j++)
				{
					int num8 = JobBuildMesh.Next(j, n);
					if ((indices[num8] & 1073741824) != 0)
					{
						int num9 = (indices[j] & 268435455) * 3;
						int num10 = (indices[JobBuildMesh.Next(num8, n)] & 268435455) * 3;
						int num11 = verts[num10] - verts[num9];
						int num12 = verts[num10 + 2] - verts[num9 + 2];
						int num13 = num11 * num11 + num12 * num12;
						if (num13 < num6)
						{
							num6 = num13;
							num7 = j;
						}
					}
				}
				if (num7 == -1)
				{
					Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
					return -num;
				}
				int num14 = num7;
				int num15 = JobBuildMesh.Next(num14, n);
				int num16 = JobBuildMesh.Next(num15, n);
				nativeArray[num2] = indices[num14] & 268435455;
				num2++;
				nativeArray[num2] = indices[num15] & 268435455;
				num2++;
				nativeArray[num2] = indices[num16] & 268435455;
				num2++;
				num++;
				n--;
				for (int k = num15; k < n; k++)
				{
					indices[k] = indices[k + 1];
				}
				if (num15 >= n)
				{
					num15 = 0;
				}
				num14 = JobBuildMesh.Prev(num15, n);
				if (JobBuildMesh.Diagonal(JobBuildMesh.Prev(num14, n), num15, n, verts, indices))
				{
					ref NativeArray<int> ptr = ref indices;
					int num5 = num14;
					ptr[num5] |= 1073741824;
				}
				else
				{
					ref NativeArray<int> ptr = ref indices;
					int num5 = num14;
					ptr[num5] &= 268435455;
				}
				if (JobBuildMesh.Diagonal(num14, JobBuildMesh.Next(num15, n), n, verts, indices))
				{
					ref NativeArray<int> ptr = ref indices;
					int num5 = num15;
					ptr[num5] |= 1073741824;
				}
				else
				{
					ref NativeArray<int> ptr = ref indices;
					int num5 = num15;
					ptr[num5] &= 268435455;
				}
			}
			nativeArray[num2] = indices[0] & 268435455;
			num2++;
			nativeArray[num2] = indices[1] & 268435455;
			num2++;
			nativeArray[num2] = indices[2] & 268435455;
			num2++;
			return num + 1;
		}

		// Token: 0x04000920 RID: 2336
		public NativeList<int> contourVertices;

		// Token: 0x04000921 RID: 2337
		public NativeList<VoxelContour> contours;

		// Token: 0x04000922 RID: 2338
		public VoxelMesh mesh;

		// Token: 0x04000923 RID: 2339
		public CompactVoxelField field;
	}
}
